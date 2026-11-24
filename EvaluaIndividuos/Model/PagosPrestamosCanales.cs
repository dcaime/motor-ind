//using Activas_ConsultaPagosPrestamosCanales;
using EvaluaIndividuos.Database.MEBA;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Serilog;
using System.Data;

namespace EvaluaIndividuos.Model
{
    public static class PagosPrestamosCanales
    {
        public static async Task<ConsultaPagosPrestamosCanalesResponseESB> GetIntradiarias(String entrada, String cobis)
        {
            //Integracion con servicio            
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var client = new HttpClient(clientHandler);

                //Armado manual del request
                client.DefaultRequestHeaders.Add("X-Authorization-impersonationLevel", "service");
                client.DefaultRequestHeaders.Add("X-Authorization-token", Herramientas.GetTokenESB());
                client.DefaultRequestHeaders.Add("X-Application-identity", Herramientas.GetApplicationId());
                var BaseURL = ESB.GetBaseURL();
                var request = new HttpRequestMessage(HttpMethod.Get, BaseURL + "/services/Activas_ConsultaPagosPrestamosCanales/V1/R0" + entrada);
                request.Headers.Add("Accept", "application/json");
                var response = client.Send(request);

                //Maneja response Como JSON
                ConsultaPagosPrestamosCanalesResponseESB IntradiariasESB;
                if (response.IsSuccessStatusCode)
                {
                    IntradiariasESB = await response.Content.ReadFromJsonAsync<ConsultaPagosPrestamosCanalesResponseESB>();
                }
                else
                {
                    var errorESB = await response.Content.ReadFromJsonAsync<FaultResponse>();
                    var msjError = ESB.GetErrorMessage(errorESB);
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound && msjError == "No data is available from result.")
                    {
                        IntradiariasESB = new ConsultaPagosPrestamosCanalesResponseESB();
                    }
                    else
                    {
                        throw new MAPIException("ConsultaPagosPrestamosCanales", 111, msjError, 502);
                    }
                }

                //BD y Retorno
                return IntradiariasESB;
            }
            //catch (Exception ex)
            catch (Exception exception) when (exception is not MAPIException)

            {
                throw new MAPIException(exception, "ConsultaPagosPrestamosCanales", 110, 502);
            }
        }

        public class IntradiariasSumarizadas
        {
            public decimal SUM_cuota_actual_PP = 0;
            public decimal SUM_monto_deuda_actual_PP = 0;
            public decimal SUM_cuota_actual_OP = 0;
            public decimal SUM_monto_deuda_actual_OP = 0;
            public decimal SUM_cuota_actual_PH = 0;
            public decimal SUM_monto_deuda_actual_PH = 0;
            public decimal SUM_cuota_actual_PR = 0;
            public decimal SUM_monto_deuda_actual_PR = 0;

            public IntradiariasSumarizadas() { }
        }

        public struct LKPProducto
        {
            public string Producto_Source;
            public string Familia_Productos;
        }

        public static async Task<IntradiariasSumarizadas> GetEndeudamientoAsync(Parametros parametros, Registro registro, string cobis, int integrante)
        {
            //Inicializaciones
            var camposEndeudamiento = new IntradiariasSumarizadas();

            if (cobis == "0" || cobis == "") return camposEndeudamiento;

            if (parametros.GetString("Switch_Intradiarias", "ACTIVO") == "ACTIVO")
            {
                Log.Information("Funcionalidad Intradiarias ACTIVO");
                Log.Information("Busca Intradiarias");
                var fechaCabalopeString = MEBAContext.GetFechaCabalope();
                ConsultaPagosPrestamosCanalesResponseESB intradiarias = new ConsultaPagosPrestamosCanalesResponseESB();
                ConsultaPagosPrestamosCanalesResponseESB intradiariasAux;
                int contador = 0;
                var entradaIntradiarias = "?IdCliente=" + cobis + "&Fecha=" + fechaCabalopeString;

                //Resuelve con Caché en ambientes bajos
                //if (Herramientas.EsAmbienteBajo() && integrante == 0)
                //{
                //    registro.RegistraInicio("Intradiarias_cache", registro.NumeroTributario);
                //    string cache = Herramientas.GetCache("Intradiarias", registro.NumeroTributario);
                //    if (cache != "") intradiarias = JsonConvert.DeserializeObject<ConsultaPagosPrestamosCanalesResponseESB>(cache);
                //    registro.RegistraFin("Intradiarias_cache", intradiarias);
                //}

                if (intradiarias.Elements.Count() == 0)
                {
                    do
                    {
                        contador++;
                        registro.RegistraInicio("Intradiarias_" + integrante + "_" + contador, entradaIntradiarias);
                        intradiariasAux = new ConsultaPagosPrestamosCanalesResponseESB();
                        intradiariasAux = await PagosPrestamosCanales.GetIntradiarias(entradaIntradiarias, cobis);
                        foreach (ConsultaPagosPrestamosCanalesResponseType e in intradiariasAux.Elements) { intradiarias.Elements.Add(e); }
                        if (intradiariasAux.Elements.Count == 20)
                        {
                            entradaIntradiarias = "?IdCliente=" + cobis + "&Fecha=" + intradiariasAux.Elements.ElementAt(intradiariasAux.Elements.Count - 1).FechaNovedad;
                        }
                        registro.RegistraFin("Intradiarias_" + integrante + "_" + contador, intradiarias);

                    } while (intradiariasAux.Elements.Count == 20);
                }

                if (intradiarias.Elements.Count > 0)
                {
                    //Recupera familia de productos
                    Dictionary<String, LKPProducto> listaFamiliaProducto = getLKPProductos(intradiarias);

                    foreach (ConsultaPagosPrestamosCanalesResponseType element in intradiarias.Elements)
                    {
                        LKPProducto familiaProducto = listaFamiliaProducto[element.TipoPrestamo];
                        switch (familiaProducto.Familia_Productos)
                        {
                            case "Personales" or "Otros Adelantos":
                                camposEndeudamiento.SUM_cuota_actual_PP += AjustarSignoIntradiaria((decimal)element.MontoCuota, element.TipoNovedad, element.Estado, true);
                                camposEndeudamiento.SUM_monto_deuda_actual_PP += AjustarSignoIntradiaria((decimal)element.MontoCapitalSolicitado, element.TipoNovedad, element.Estado, false);
                                break;
                            case "Hipotecarios":
                                camposEndeudamiento.SUM_cuota_actual_PH += AjustarSignoIntradiaria((decimal)element.MontoCuota, element.TipoNovedad, element.Estado, true);
                                camposEndeudamiento.SUM_monto_deuda_actual_PH += AjustarSignoIntradiaria((decimal)element.MontoCapitalSolicitado, element.TipoNovedad, element.Estado, false);
                                break;
                            case "Prendarios":
                                camposEndeudamiento.SUM_cuota_actual_PR += AjustarSignoIntradiaria((decimal)element.MontoCuota, element.TipoNovedad, element.Estado, true);
                                camposEndeudamiento.SUM_monto_deuda_actual_PR += AjustarSignoIntradiaria((decimal)element.MontoCapitalSolicitado, element.TipoNovedad, element.Estado, false);
                                break;
                            default:
                                camposEndeudamiento.SUM_cuota_actual_OP += AjustarSignoIntradiaria((decimal)element.MontoCuota, element.TipoNovedad, element.Estado, true);
                                camposEndeudamiento.SUM_monto_deuda_actual_OP += AjustarSignoIntradiaria((decimal)element.MontoCapitalSolicitado, element.TipoNovedad, element.Estado, false);
                                break;
                        }
                    }
                }
            }
            else
            {
                Log.Information("Funcionalidad Intradiarias INACTIVA");
            }
            return camposEndeudamiento;
        }

        private static decimal AjustarSignoIntradiaria(decimal monto, string tipoNovedad, string estado, bool esCuota)
        {
            var _tipoNovedad = tipoNovedad.Trim();
            var _estado = estado.Trim();

            if (_tipoNovedad == "LIQ" && _estado == "ING")
            {
                return monto;
            }
            else if (_tipoNovedad == "LIQ" && _estado == "RV")
            {
                //return monto * -1; //V1.3 - Solo LIQ ING
                return 0;
            }
            else if ((_tipoNovedad == "CAN" || _tipoNovedad == "PRE") && _estado == "ING")
            {
                if (esCuota)
                {
                    return 0;
                }
                else
                {
                    //return monto * -1; //V1.3 - Solo LIQ ING
                    return 0;
                }
            }
            else if ((_tipoNovedad == "CAN" || _tipoNovedad == "PRE") && _estado == "RV")
            {
                if (esCuota)
                {
                    return 0;
                }
                else
                {
                    //return monto; //V1.3 - Solo LIQ ING
                    return 0;
                }
            }
            throw new MAPIException("ConsultaPagosPrestamosCanales", 112, "Tipo o Estado de novedad invalidos", 409);
        }

        private static Dictionary<String, LKPProducto> getLKPProductos(ConsultaPagosPrestamosCanalesResponseESB intradiarias)
        {
            string query = @"SELECT Familia_Productos, Producto_Source FROM MEBADB.dbo.LKP_PRODUCTOS WHERE Producto_Source IN ({0})";
            string connStr = MEBAContext.GetConnectionString();
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            var listaFamiliaProducto = new Dictionary<String, LKPProducto>();
            var idParameterList = new List<string>();
            var index = 0;
            foreach (ConsultaPagosPrestamosCanalesResponseType element in intradiarias.Elements)
            {
                if (!listaFamiliaProducto.ContainsKey(element.TipoPrestamo))
                {
                    var paramName = "@idParam" + index;
                    comm.Parameters.AddWithValue(paramName, element.TipoPrestamo);
                    idParameterList.Add(paramName);
                    index++;
                    listaFamiliaProducto.Add(element.TipoPrestamo, new LKPProducto
                    {
                        Familia_Productos = "Otros",
                        Producto_Source = element.TipoPrestamo
                    });
                }
            }
            comm.CommandText = String.Format(query, string.Join(",", idParameterList));
            comm.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string producto = (string)reader["Producto_Source"];
                        string familia = (string)reader["Familia_Productos"];
                        listaFamiliaProducto[producto] = new LKPProducto
                        {
                            Familia_Productos = familia,
                            Producto_Source = producto
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                conn.Close();
                throw new MAPIException(exception, "Error SQL MEBA", 4, 500);
            }
            return listaFamiliaProducto;
        }

        public static async Task<IOrigen> GetEndeudamientoOrigenAsync(Parametros parametros, Registro registro, string cobis, int index)
        {
            var resultado = await PagosPrestamosCanales.GetEndeudamientoAsync(parametros, registro, cobis, index);

            var dict = new Dictionary<int, IntradiariasSumarizadas>();
            if (resultado != null)
            {
                dict.Add(index, resultado);
            }

            return new Origen<int, IntradiariasSumarizadas>(Origenes.Intradiarias, dict);
        }
    }

    public partial class ConsultaPagosPrestamosCanalesResponseESB
    {
        [Newtonsoft.Json.JsonProperty("elements", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<ConsultaPagosPrestamosCanalesResponseType> Elements { get; set; } = new List<ConsultaPagosPrestamosCanalesResponseType>();
    }
}
