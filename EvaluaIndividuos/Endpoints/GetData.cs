using Azure;
using Azure.Core;
using Carter;
using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EvaluaIndividuos.Endpoints
{
    public class GetData : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/GetData", async (GetDataRequest request, [FromServices] Registro registro, HttpContext httpContext, [FromServices] Parametros parametros, [FromServices] IMEBIService mebi, [FromServices] MEBIContext dbContext, [FromServices] WebhookCoordinator coordinator) =>
            {
                GetDataResponse response;

                switch (request.Servicio)
                {
                    case "SIISA":
                        registro.Encadena(request.Id, "SIISA", request.Datos);
                        api_fp_credit_assessment_information_Client clientSiisa = httpContext.RequestServices.GetRequiredService<api_fp_credit_assessment_information_Client>();
                        response = await GetSIISA(request, clientSiisa, dbContext, mebi, parametros);
                        registro.Finaliza(response);
                        break;
                    case "Veraz":
                        registro.Encadena(request.Id, "Veraz", request.Datos);
                        api_bureaus_v1_0_6_Client clientVeraz = httpContext.RequestServices.GetRequiredService<api_bureaus_v1_0_6_Client>();
                        response = await GetVeraz(request, clientVeraz, parametros, mebi, coordinator);
                        registro.Finaliza(response);
                        break;
                    default:
                        response = new();
                        response.Codigo = "ERROR";
                        response.Mensaje = "Servicio desconocido";
                        break;
                } 
                return response;
            //}).ExcludeFromDescription();
            });
        }

        private async Task<GetDataResponse> GetVeraz(GetDataRequest request, api_bureaus_v1_0_6_Client client, Parametros parametros, IMEBIService mebi, WebhookCoordinator coordinator)
        {
            var dataResponse = new GetDataResponse();

            try
            {
                long clave = -1;
                var clavesValidas = new List<long>();
                var datos = request.Datos.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var cuil = (datos.Length > 0) ? datos[0] : string.Empty;
                var dni = (datos.Length > 1) ? datos[1] : string.Empty;
                var sexo = (datos.Length > 2) ? datos[2] : string.Empty;
                var nombre = (datos.Length > 3) ? datos[3] : string.Empty;
                var ignoraCache = (datos.Length > 5) ? datos[5] : "0";
                var cortesScore = ((datos.Length > 4) ? datos[4] : "999-999").Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                int corteThin = 999;
                int.TryParse(((cortesScore.Length > 0) ? cortesScore[0] : "999"), out corteThin);
                int corteThick = 999;
                int.TryParse(((cortesScore.Length > 1) ? cortesScore[1] : "999"), out corteThick);
                if (!(long.TryParse(cuil, out clave))) long.TryParse(dni, out clave);
                var caches = await mebi.GetCacheMapiAsync([clave], ["veraz", "verazX6", "verazX6c", "verazX6l"]);
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower
                };
                var vigencia_cache = parametros.GetInt("veraz_vigencia_cache", 30);
                string estadoTransacCorto = "7";
                string estadoTransacLargo = "7";
                //Hasta acá se preparan variables

                //Obtiene informe corto
                WebhooksVerazPostRequest informeCorto = null;
                
                //1ro se fija si está en caché
                if (caches.TryGetValue(clave.ToString() + "verazX6C", out var cacheCorto) && ignoraCache is not "1") if(cacheCorto.FechaHora.AddDays(vigencia_cache) > DateTime.Today) informeCorto = System.Text.Json.JsonSerializer.Deserialize<WebhooksVerazPostRequest>(cacheCorto.Cache, options);

                //si no recuperó el caché corto, llama al servicio para el informe corto
                if (informeCorto is null)
                {
                    informeCorto = await GetInformeVeraz(clave, nombre, sexo, ignoraCache, "X6C", request.Id, client, parametros, coordinator);
                    estadoTransacCorto = "0";
                }
                else
                {
                    estadoTransacCorto = "6";
                }
                //Fin informe corto

                WebhooksVerazPostRequest informeLargo = null;
                //Verifica si busca informe largo
                if (((informeCorto.Result.X6c.Evaluation.Customer.ScorePopulation is "OK")
                    && informeCorto.Result.X6c.Evaluation.Customer.RiskScore > corteThick)
                    || (informeCorto.Result.X6c.Evaluation.Customer.RiskScore > corteThin))
                {
                    //Busca informe largo    
                    //1ro se fija si está en caché
                    if (caches.TryGetValue(clave.ToString() + "verazX6L", out var cacheLargo) && ignoraCache is not "1") if (cacheLargo.FechaHora.AddDays(vigencia_cache) > DateTime.Today) informeCorto = System.Text.Json.JsonSerializer.Deserialize<WebhooksVerazPostRequest>(cacheLargo.Cache, options);
                    //si no recuperó el caché largo, llama al servicio para el informe largo
                    if (informeLargo is null) {
                        informeLargo = await GetInformeVeraz(clave, nombre, sexo, ignoraCache, "X6L", request.Id, client, parametros, coordinator);
                        estadoTransacLargo = "0";
                    }
                    else
                    {
                        estadoTransacLargo = "6";
                    }
                        
                }

                //Haya recuperado o no el largo, se mapean
                if (informeLargo is null) informeLargo = new(); //Se inicializa en caso que no se haya recuperado
                dataResponse.Mensaje = MapeaVeraz(informeCorto, informeLargo, estadoTransacCorto, estadoTransacLargo);
                dataResponse.Codigo = "OK";
                return dataResponse;
            }
            catch (Exception)
            {
                Log.Information("Error con Veraz");
                dataResponse.Mensaje = "No se obtuvo informe";
                dataResponse.Codigo = "KO";
                return dataResponse;
            }
        }

        private async Task<WebhooksVerazPostRequest> GetInformeVeraz(long clave, string nombre, string sexo, string ignoraCache, string nombreInforme, Guid id, api_bureaus_v1_0_6_Client client, Parametros parametros, WebhookCoordinator coordinator)
        {
            var timeout = parametros.GetInt("veraz_timeout", 10);
            var codigoDeFacturacion = parametros.GetString("veraz_codigo_de_facturacion", "001");
            var centroDeCostos = parametros.GetString("veraz_centro_de_costos", "002");
            var vigencia_cache = parametros.GetInt("veraz_vigencia_cache", 30);

            var callbackUrl = "https://" + Environment.MachineName + ".macro.com.ar:50008/EvaluaIndividuos/Webhooks/Veraz";
            var apikey = CryptoService.Decrypt(parametros.GetString("apigee_client_id"));
            if (Herramientas.EsAmbienteBajo())
            {
                apikey = CryptoService.Decrypt("cvGdPayBloBSuOdKK+Xjc9ytAFgKZSmXYw3i1fQ0FKxD8VI2X0A1g7O/aprGfEGZppbjxhrO/950LRbGk08k7Q==");
                client.BaseUrl = "https://api-test.gcp.macro.com.ar" + client.BaseUrl;
            }
            else client.BaseUrl = parametros.GetString("apigee_base_url") + client.BaseUrl;
            Log.Information("url veraz: " + client.BaseUrl);

            var docType = (clave.ToString().Length is 11) ? "CUIL" : "DNI";

            Guid requestId = Guid.NewGuid();
            Log.Information("Request id: " + requestId.ToString());
            PostReportRequestRequest r = new();
            PostReportRequestRequestBureaus b = new();
            PostReportRequestRequestServices s = new();
            r.DocumentNumber = clave;
            r.DocumentType = docType;
            r.FullName = nombre;
            r.Gender = sexo;
            b.Name = PostReportRequestRequestBureausName.Veraz;
            b.Services.Add(s);
            s.Name = nombreInforme;
            s.CachePolicy = new();
            s.CachePolicy.ValidityDays = vigencia_cache;
            s.CachePolicy.IsIgnored = ignoraCache is "1";
            s.Billing = new();
            s.Billing.Code = codigoDeFacturacion;
            s.Billing.CostCenterCode = centroDeCostos;
            s.Billing.IdTransaction = id.ToString();
            r.Bureaus.Add(b);
            var prrr = await client.PostReportRequestAsync(requestId, 1, callbackUrl, "30000000007", r);

            //Si explota por timeout, la excepcion se captura mas arriba
            return await coordinator.WaitForResponseAsync(requestId.ToString(), TimeSpan.FromSeconds(timeout));
        }

        private string MapeaVeraz(WebhooksVerazPostRequest informeCorto, WebhooksVerazPostRequest informeLargo, string estadoTransacCorto, string estadoTransacLargo)
        {
            var resultCorto = informeCorto.Result;
            var resultLargo = informeLargo.Result;
            var sb = new StringBuilder();
            sb.Append(resultCorto.X6c.Evaluation.Customer.FullName).Append("|")  //nombre
                .Append(resultCorto.X6c.Evaluation.Customer.RiskScore?.ToString() ?? "0").Append("|")  //score
                .Append(resultCorto.X6c.Evaluation.Customer.ScorePopulation).Append("|") //poblacion
                .Append(resultCorto.X6c.Evaluation.Customer.IncomePredictorScore).Append("|") //ip
                .Append(resultLargo.X6l.Evaluation.CategoryCode).Append("|") //categoria
                .Append(resultLargo.X6l.Evaluation.Customer.TaxpayerNumber).Append("|") //cuil
                .Append(resultLargo.X6l.Evaluation.Customer.Addresses.FirstOrDefault()).Append("|") //domicilio
                .Append(resultLargo.X6l.Evaluation.Customer.Type).Append("|") //tipo_de_cliente
                .Append(resultLargo.X6l.Evaluation.Customer.DocumentVersion).Append("|") //version_dni
                .Append(resultLargo.X6l.Evaluation.CreditCards.OldestMonthsCount?.ToString() ?? "0").Append("|") //antiguedad_tc
                .Append((resultLargo.X6l.Evaluation.CreditCards.HasNormalStatusLastTwelveMonths) ? "Si" : "No").Append("|") //peor_sit_tc_12
                .Append((resultLargo.X6l.Evaluation.CreditCards.HasFinancedBalanceLimitRatio) ? "Si" : "No").Append("|") //rel_sldofin_lim
                .Append((resultLargo.X6l.Evaluation.CreditCards.HasPeerbankActive) ? "Si" : "No").Append("|") //peerbank
                .Append(resultLargo.X6l.Evaluation.CreditCards.ExigibleAmount).Append("|") //uso_tc_exigible
                .Append(resultLargo.X6l.Evaluation.CreditCards.CompetitorMaximumLimitAmount).Append("|") //lim_max_tc_compet
                .Append(resultLargo.X6l.Evaluation.CreditCards.CompetitorMinimumPaymentAmount).Append("|") //pm_max_tc_compet
                .Append(resultLargo.X6l.Evaluation.CurrentAccounts.OldestMonthsCount?.ToString() ?? "0").Append("|") //antiguedad_cc
                .Append((resultLargo.X6l.Evaluation.CurrentAccounts.HasWorstStatusTwelveMonths) ? "Si" : "No").Append("|") //peor_sit_cc_12
                .Append((resultLargo.X6l.Evaluation.CurrentAccounts.IsAverageUsageUnderLimit) ? "Si" : "No").Append("|") //rel_usoprom_lim
                .Append(resultLargo.X6l.Evaluation.CurrentAccounts.AverageBalanceAmount).Append("|") //sldo_prom_cc
                .Append(resultLargo.X6l.Evaluation.Loans.RefinancedCount?.ToString() ?? "0").Append("|") //p_ref
                .Append(resultLargo.X6l.Evaluation.Loans.Unsecured.ExigibleAmount).Append("|") //uso_sg_exigible
                .Append(resultLargo.X6l.Evaluation.Loans.Pledge.ExigibleAmount).Append("|") //uso_gp_exigible
                .Append(resultLargo.X6l.Evaluation.Loans.Mortgage.ExigibleAmount).Append("|") //uso_gh_exigible
                .Append(resultLargo.X6l.Evaluation.Bureau.WorstStatusCode).Append("|") //bureau_mx_actual
                .Append(resultLargo.X6l.Evaluation.Bcra.WorstSituationTwelveMonthsCode).Append("|") //situacion_bcra_12
                .Append(resultLargo.X6l.Evaluation.Bcra.WorstSituationThreeMonthsCode).Append("|") //situacion_bcra_3
                .Append((resultLargo.X6l.Evaluation.LegalSituation.HasDelinquencyObservations) ? "Si" : "No").Append("|") //obs_de_morosidad
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Lawsuits.ActiveExecutiveCount?.ToString() ?? "0").Append("|") //juicio_ejec_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Lawsuits.ExecutiveFiveYearsCount?.ToString() ?? "0").Append("|") //juicios_ejec_5
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Lawsuits.OrdinaryCount?.ToString() ?? "0").Append("|") //j_ord_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Bankruptcy.RequestsLastYearCount?.ToString() ?? "0").Append("|") //pedido_quiebra_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Bankruptcy.RequestsFiveYearsCount?.ToString() ?? "0").Append("|") //pedido_quiebra_5
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Bankruptcy.PreventiveCount?.ToString() ?? "0").Append("|") //conc_prev_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Bankruptcy.DecreedCount?.ToString() ?? "0").Append("|") //quieb_decret_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Inhibitions.AssetCount?.ToString() ?? "0").Append("|") //inh_bienes_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Inhibitions.OtherCount?.ToString() ?? "0").Append("|") //inh_otras_q
                .Append(resultLargo.X6l.Evaluation.LegalSituation.Inhibitions.JudicialCount?.ToString() ?? "0").Append("|") //inh_jud_q
                .Append(resultLargo.X6l.Evaluation.Cheqs.UnpaidBouncedTwentyFourMonthsAmount).Append("|") //monto_cheq_rech_sf_no_pag_24m
                .Append(resultLargo.X6l.Evaluation.Cheqs.PaidBouncedTwentyFourMonthsAmount).Append("|") //monto_cheq_rech_sf_pag_24m
                .Append(resultLargo.X6l.Evaluation.Cheqs.PaidBouncedTwentyFourMonthsCount?.ToString() ?? "0").Append("|") //cant_cheq_rech_sf_pag_24m
                .Append(estadoTransacCorto).Append("|") //estado_transact
                .Append(estadoTransacLargo).Append("|") //estado_transact
                .Append("Respuesta OK").Append("|") //mensaje
                ;
            return sb.ToString();
        }

        private async Task<GetDataResponse> GetSIISA(GetDataRequest request, api_fp_credit_assessment_information_Client client, [FromServices] MEBIContext dbContext, [FromServices] IMEBIService mebi, [FromServices] Parametros parametros)
        {
            string? aux;
            var dataResponse = new GetDataResponse();
            if(parametros.GetString("siisa_activo", "SI") is "NO"){
                dataResponse.Codigo = "INACTIVO";
                dataResponse.Mensaje = "";
                return dataResponse;
            }

            var datosRequest = request.Datos.Split("|");
            var taxpayer_number = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(0))) ? null : datosRequest.ElementAtOrDefault(0);
            var document_number = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(1))) ? null : datosRequest.ElementAtOrDefault(1);
            var full_name = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(2))) ? null : datosRequest.ElementAtOrDefault(2);
            var apikey = CryptoService.Decrypt(parametros.GetString("apigee_client_id"));
            var vigencia_cache = parametros.GetInt("siisa_vigencia_cache", 30);

            long clave = -1;
            long.TryParse(taxpayer_number, out clave);
            if(clave < 1) long.TryParse(document_number, out clave);
            var clavesValidas = new List<long>();
            var datos = request.Datos.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (datos.Length > 1) long.TryParse(datos[0], out clave);
            var caches = await mebi.GetCacheMapiAsync([clave], ["siisa"]);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower
            };

            if (caches.TryGetValue(clave.ToString() + "siisa", out var cache))
            {
                if (cache.FechaHora.AddDays(vigencia_cache) > DateTime.Today)
                {
                    GetRetrieveSiisaResponse respuestaCache = System.Text.Json.JsonSerializer.Deserialize<GetRetrieveSiisaResponse>(cache.Cache, options);
                    dbContext.DictamenesSiisa.Add(MapeaDictamenSIISA(respuestaCache, 6, request.Id.ToString()));
                    await dbContext.SaveChangesAsync();
                    dataResponse.Mensaje = MapeaSIISA(respuestaCache);
                    dataResponse.Codigo = "OK";
                    return dataResponse;
                }
            }

            if (Herramientas.EsAmbienteBajo())
            {
                Log.Information("Es ambiente bajo, se ajustan credenciales");
                apikey = CryptoService.Decrypt("cvGdPayBloBSuOdKK+Xjc9ytAFgKZSmXYw3i1fQ0FKxD8VI2X0A1g7O/aprGfEGZppbjxhrO/950LRbGk08k7Q==");
                client.BaseUrl = "https://api-test.gcp.macro.com.ar" + client.BaseUrl;
            }
            else
            {
                client.BaseUrl = parametros.GetString("apigee_base_url") + client.BaseUrl;
            }

            Gender? gender;
            aux = datosRequest.ElementAtOrDefault(3);
            if (string.IsNullOrEmpty(aux)) gender = null;
            else if (aux is "0" or "M" ) gender = Gender.M;
            else if (aux is "1" or "F") gender = Gender.F;
            else throw new MAPIException("SIISA", 120, "Género inválido", 409);

            var payroll_income_amount = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(4))) ? null : datosRequest.ElementAtOrDefault(4);
            var employer_taxpayer_number = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(5))) ? null : datosRequest.ElementAtOrDefault(5);

            DateTimeOffset operation_date_time;
            bool operation_date_timeFlag;
            operation_date_timeFlag = DateTimeOffset.TryParse(datosRequest.ElementAtOrDefault(6), out operation_date_time);
            
            int operation_installment_count;
            bool operation_installment_countFlag;
            operation_installment_countFlag = int.TryParse(datosRequest.ElementAtOrDefault(8), out operation_installment_count);
            var operation_installment_amount = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(9))) ? null : datosRequest.ElementAtOrDefault(9);
            var operation_total_amount = (string.IsNullOrEmpty(datosRequest.ElementAtOrDefault(10))) ? null : datosRequest.ElementAtOrDefault(10);

            try
            {
                Log.Information("Llama SIISA");
                var siisa = await client.GetRetrieveSiisaAsync(
                    x_request_id: request.Id,
                    apikey: apikey,
                    taxpayer_number: taxpayer_number,
                    document_number: document_number,
                    full_name: full_name,
                    gender: gender
                );
                Log.Information("Fin invocacion SIISA");

                var cacheEntry = new CacheMapi
                {
                    FechaHora = DateTime.Now,
                    Servicio = "siisa",
                    Clave = clave,
                    Cache = System.Text.Json.JsonSerializer.Serialize(siisa, options)
                };

                var dictamenesSiisa = MapeaDictamenSIISA(siisa, 0, request.Id.ToString());
                dbContext.DictamenesSiisa.Add(dictamenesSiisa);
                dbContext.CacheMapi.Add(cacheEntry);
                await dbContext.SaveChangesAsync();

                dataResponse.Codigo = "OK";
                dataResponse.Mensaje = MapeaSIISA(siisa);
                return dataResponse;
            }
            catch (Exception ex) {
                if (ex is ApiException<Error> typedApiEx) Log.Error(ex, "SIISA explotó. Código: {codigo}. Mensaje: {ErrorMessage}", typedApiEx.Result?.Code, typedApiEx.Result?.Message);
                if (ex is ApiException apiEx) Log.Error(ex, "SIISA explotó, status {codigoHttp}. Body: {response}", apiEx.StatusCode, apiEx.Response);
                else Log.Error(ex, "Error al invocar SIISA: {mensaje}", ex.Message);

                dataResponse.Codigo = "ERROR";
                dataResponse.Mensaje = ex.Message;
                return dataResponse;
            }
        }

        private DictamenesSiisa MapeaDictamenSIISA(GetRetrieveSiisaResponse siisa, int estadoTransac, string idInterno)
        {
            var dictamenesSiisa = new DictamenesSiisa
            {
                FechaHora = DateTime.Now,
                Cuil = siisa.Persons?.ElementAtOrDefault(0)?.TaxpayerNumber,
                Documento = siisa.Persons?.ElementAtOrDefault(0)?.DocumentNumber,
                Sexo = siisa.Persons?.ElementAtOrDefault(0)?.Gender?.ToString(),
                Nacimiento = DateOnly.FromDateTime((siisa.Persons?.ElementAtOrDefault(0)?.BirthDate ?? new DateTimeOffset(new DateTime(1900, 1, 1))).DateTime),
                Apellido = siisa.Persons?.ElementAtOrDefault(0)?.LastName,
                Nombre = siisa.Persons?.ElementAtOrDefault(0)?.FirstName,
                Fallecido = (siisa.Persons?.ElementAtOrDefault(0)?.IsDeceased ?? false) ? "Si" : "No",
                Score = siisa.Scorings?.ElementAtOrDefault(0)?.Type,
                Modelo = siisa.Scorings?.ElementAtOrDefault(0)?.ModelCode,
                IdCategoria = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.CategoryId,
                NombreCategoria = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.CategoryName,
                IdModelo = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.ModelId,
                NombreModelo = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.ModelName,
                MaxMontoCategoria = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.MaxAmount,
                PromMontoCategoria = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.AverageAmount,
                MinMontoCategoria = siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.MinAmount,
                Cuit = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.TaxpayerNumber,
                RazonSocial = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.LegalName,
                Antiguedad = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.SeniorityDescription,
                Sector = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.SectorName,
                Actividad = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.ActivityName,
                ActividadCompleta = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.FullActivityDescription,
                ActividadCompletaCodigo = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.FullActivityCode,
                FechaIngreso = DateOnly.FromDateTime((siisa.EmploymentRecords?.ElementAtOrDefault(0)?.StartDate ?? new DateTimeOffset(new DateTime(1900, 1, 1))).DateTime),
                EsRelDep = (siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsEmployee ?? false) ? "Si" : "No",
                EsRelDep6m = (siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.HasSixMonthsHistory ?? false) ? "Si" : "No",
                EsRelDep12m = (siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.HasTwelveMonthsHistory ?? false) ? "Si" : "No",
                EsJubilado = (siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsRetired ?? false) ? "Si" : "No",
                EsMonotributo = (siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsMonotaxContributor ?? false) ? "Si" : "No",
                EsAutonomo = (siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsSelfEmployed ?? false) ? "Si" : "No",
                CategoriaMonotributo = siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.MonotaxCategory,
                FechaInscripcion = DateOnly.FromDateTime((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.RegistrationDate ?? new DateTimeOffset(new DateTime(1900, 1, 1))).DateTime),
                Autonomo = (siisa.TaxStatuses?.ElementAtOrDefault(0)?.IsSelfEmployed ?? false) ? "Si" : "No",
                ImpuestoGanancias = siisa.TaxStatuses?.ElementAtOrDefault(0)?.IncomeTaxPayerCode,
                ImpuestoIva = siisa.TaxStatuses?.ElementAtOrDefault(0)?.IvaTax,
                CodigoCategoriaMonotributo = siisa.TaxStatuses?.ElementAtOrDefault(0)?.MonotaxCategoryCode,
                EsIntegranteSociedad = (siisa.TaxStatuses?.ElementAtOrDefault(0)?.IsSocietyMember ?? false) ? "Si" : "No",
                EsEmpleador = (siisa.TaxStatuses?.ElementAtOrDefault(0)?.IsEmployer ?? false) ? "Si" : "No",
                FueEmpleador = (siisa.EmploymentRecords?.ElementAtOrDefault(0)?.IsPreviousEmployer ?? false) ? "Si" : "No",
                Pc = siisa.EmploymentRecords?.ElementAtOrDefault(0)?.RiskScoreValue,
                ConcursoYQuiebraProceso = siisa.InsolvencyProceedings?.ElementAtOrDefault(0)?.StatusDescription,
                JuiciosArba = siisa.ArbaLawsuits?.ElementAtOrDefault(0)?.TaxpayerType?.ToString() ?? string.Empty
                    + ";" + siisa.ArbaLawsuits?.ElementAtOrDefault(0)?.StartDate?.ToString() ?? string.Empty
                    + ";" + siisa.ArbaLawsuits?.ElementAtOrDefault(0)?.Amount ?? string.Empty,
                FraudeOperacion = siisa.Frauds?.ElementAtOrDefault(0)?.OperationType,
                EstadoTransac = estadoTransac,
                IdInterno = idInterno,
            };
            return dictamenesSiisa;
        }

        private string MapeaSIISA(GetRetrieveSiisaResponse siisa)
        {
            string arbaLawsuits = siisa.ArbaLawsuits?.ElementAtOrDefault(0)?.TaxpayerType?.ToString() ?? string.Empty
                + ";" + siisa.ArbaLawsuits?.ElementAtOrDefault(0)?.StartDate?.ToString() ?? string.Empty
                + ";" + siisa.ArbaLawsuits?.ElementAtOrDefault(0)?.Amount ?? string.Empty;

            var sb = new StringBuilder();
            sb.Append(siisa.Persons?.ElementAtOrDefault(0)?.TaxpayerNumber.ToString() ?? "").Append("|")  //Cuil
              .Append(siisa.Persons?.ElementAtOrDefault(0)?.DocumentNumber.ToString() ?? "").Append("|")  //NroDoc
              .Append(siisa.Persons?.ElementAtOrDefault(0)?.Gender?.ToString()).Append("|") //Sexo
              .Append(siisa.Persons?.ElementAtOrDefault(0)?.BirthDate?.ToString("yyyyMMdd" ?? "19000101")).Append("|")   //Fnac
              .Append(siisa.Persons?.ElementAtOrDefault(0)?.LastName ?? "").Append("|")  //Apellido
              .Append(siisa.Persons?.ElementAtOrDefault(0)?.FirstName ?? "").Append("|") //Nombre
              .Append((siisa.Persons?.ElementAtOrDefault(0)?.IsDeceased ?? false) ? "Si" : "No").Append("|") //Fallecido
              .Append(siisa.Scorings?.ElementAtOrDefault(0)?.Type?.ToString() ?? "0").Append("|")    //score
              .Append(siisa.Scorings?.ElementAtOrDefault(0)?.ModelCode ?? "").Append("|") //modelo
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.CategoryId?.ToString() ?? "0").Append("|") //idCat
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.CategoryName ?? "").Append("|") //nombreCat
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.ModelId?.ToString() ?? "0").Append("|")  //idModelo
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.ModelName ?? "").Append("|")  //nombreModelo
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.MaxAmount ?? "").Append("|")  //maxMontoCat
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.AverageAmount ?? "").Append("|")  //promMontoCat
              .Append(siisa.SocioEconomicModels?.ElementAtOrDefault(0)?.Level?.MinAmount ?? "").Append("|")  //minMontoCat
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.TaxpayerNumber ?? "").Append("|")  //cuit
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.LegalName ?? "").Append("|")  //razonsocial
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.SeniorityDescription ?? "").Append("|")  //antiguedad
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.SectorName ?? "").Append("|")  //sector
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.ActivityName ?? "").Append("|")  //actividad
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.FullActivityDescription ?? "").Append("|")  //actividad_completa
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.FullActivityCode ?? "").Append("|")  //actividad_completa_codigo
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.StartDate?.ToString("yyyyMMdd") ?? "19000101").Append("|")  //FechaIngreso
              .Append((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsEmployee ?? false) ? "Si" : "No").Append("|")  //esRelDep
              .Append((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.HasSixMonthsHistory ?? false) ? "Si" : "No").Append("|")  //esRelDep6m
              .Append((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.HasTwelveMonthsHistory ?? false) ? "Si" : "No").Append("|")  //esRelDep12m
              .Append((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsRetired ?? false) ? "Si" : "No").Append("|")  //esJubilado
              .Append((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsMonotaxContributor ?? false) ? "Si" : "No").Append("|")  //esMonotributo
              .Append((siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.IsSelfEmployed ?? false) ? "Si" : "No").Append("|")  //esAutonomo
              .Append(siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.MonotaxCategory ?? "").Append("|")  //monotributoCat
              .Append(siisa.SummarizedVariables?.ElementAtOrDefault(0)?.EmploymentStatus?.RegistrationDate?.ToString("yyyyMMdd") ?? "19000101").Append("|")  //fechaInscripcion
              .Append((siisa.TaxStatuses?.ElementAtOrDefault(0)?.IsSelfEmployed ?? false) ? "Si" : "No").Append("|")  //autonomo
              .Append(siisa.TaxStatuses?.ElementAtOrDefault(0)?.IncomeTaxPayerCode ?? "").Append("|")  //impuesto_ganancias
              .Append(siisa.TaxStatuses?.ElementAtOrDefault(0)?.IvaTax ?? "").Append("|")  //impuesto_iva
              .Append(siisa.TaxStatuses?.ElementAtOrDefault(0)?.MonotaxCategoryCode ?? "").Append("|")  //categoria_monotributo
              .Append((siisa.TaxStatuses?.ElementAtOrDefault(0)?.IsSocietyMember ?? false) ? "Si" : "No").Append("|")  //integrante_sociedad
              .Append((siisa.TaxStatuses?.ElementAtOrDefault(0)?.IsEmployer ?? false) ? "Si" : "No").Append("|")  //empleador
              .Append((siisa.EmploymentRecords?.ElementAtOrDefault(0)?.IsPreviousEmployer ?? false) ? "Si" : "No").Append("|")  //anterior
              .Append(siisa.EmploymentRecords?.ElementAtOrDefault(0)?.RiskScoreValue ?? "").Append("|")  //pc
              .Append(siisa.InsolvencyProceedings?.ElementAtOrDefault(0)?.StatusDescription ?? "").Append("|")  //ConcursoyQuiebra - Proceso
              .Append(arbaLawsuits).Append("|")  //JuiciosArba
              .Append(siisa.Frauds?.ElementAtOrDefault(0)?.OperationType ?? "").Append("|")  //Fraude - Operacion
              ;
            return sb.ToString();
        }
    }

    public class GetDataRequest
    {
        public string Servicio { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Datos { get; set; } = string.Empty;
    }

    public class GetDataResponse
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
