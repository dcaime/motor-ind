using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Serilog;

namespace EvaluaIndividuos.Model
{
    public interface IEvaluador
    {
        Task<ResponseEvaluaIndividuosArray> EvaluarIndividuosAsync(RequestEvaluaIndividuosArray requestArray, Registro registro);
        Task<string> EvaluaRentaPresuntaAsync(RequestServicioLegacy request, Registro registro);
        Task<string> EvaluaMercadoAbiertoAsync(RequestServicioLegacy request, Registro registro);
        Task<string> EvaluaPlanSueldoPrivadoAsync(RequestServicioLegacy request, Registro registro);
        Task<string> EvaluaPlanSueldoPublicoAsync(RequestServicioLegacy request, Registro registro);
    }

    public class Evaluador : IEvaluador
    {
        private readonly IMEBAService _meba;
        private readonly IMEBIService _mebi;
        private readonly Parametros _parametros;

        public Evaluador(IMEBAService meba, IMEBIService mebi, Parametros parametros)
        {
            _meba = meba;
            _mebi = mebi;
            _parametros = parametros;
        }

        public async Task<ResponseEvaluaIndividuosArray> EvaluarIndividuosAsync(RequestEvaluaIndividuosArray requestArray, Registro registro)
        {
            ResponseEvaluaIndividuos respuesta;
            Validar(requestArray);
            var integrantes = await _meba.GetIntegrantesPorCuilAsync(requestArray.element);
            var politica = new PoliticaEvaluaIndividuos();
            var tareasRequest = new List<Task<IOrigen>>();
            tareasRequest.Add(_meba.GetClientesDepuradosAsync(integrantes));
            tareasRequest.Add(_meba.GetClientesAsync(integrantes));
            tareasRequest.Add(_meba.GetValidacionActividadAsync(integrantes));
            tareasRequest.Add(_meba.GetLkpIrregularidadesBcraAsync(integrantes));
            tareasRequest.Add(_meba.GetEndeudamientoAtrasoAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonIndividuosAsync(integrantes));
            tareasRequest.Add(_meba.GetLimiteTransitorioAsync(integrantes));
            tareasRequest.Add(_meba.GetCpAnalisisAsync(integrantes));
            tareasRequest.Add(_meba.GetCpAuxiliarAsync(integrantes));
            tareasRequest.Add(_meba.GetLkpConveniosAsync(requestArray.element.Titular?.NumeroConvenio ?? -1));
            if (integrantes.PorIndice(0)?.Cobis is not null) tareasRequest.Add(PagosPrestamosCanales.GetEndeudamientoOrigenAsync(_parametros, registro, integrantes.PorIndice(0).Cobis.ToString(), 0));
            if (integrantes.PorIndice(1)?.Cobis is not null) tareasRequest.Add(PagosPrestamosCanales.GetEndeudamientoOrigenAsync(_parametros, registro, integrantes.PorIndice(1).Cobis.ToString(), 1));
            if (integrantes.PorIndice(2)?.Cobis is not null) tareasRequest.Add(PagosPrestamosCanales.GetEndeudamientoOrigenAsync(_parametros, registro, integrantes.PorIndice(2).Cobis.ToString(), 2));
            if (integrantes.PorIndice(3)?.Cobis is not null) tareasRequest.Add(PagosPrestamosCanales.GetEndeudamientoOrigenAsync(_parametros, registro, integrantes.PorIndice(3).Cobis.ToString(), 3));
            tareasRequest.Add(_meba.GetCalificacionConstanteAsync(integrantes, requestArray.element.OrigenId?.ToString() ?? "", requestArray.element?.ComercioId?.ToString() ?? ""));
            tareasRequest.Add(_mebi.GetCampañasExceptuadasAsync(requestArray.element?.OrigenId?.ToString() ?? "", requestArray.element?.ComercioSucursal?.ToString() ?? ""));
            tareasRequest.Add(_mebi.GetPrecacomAsync(integrantes, requestArray.element?.OrigenId ?? -1, requestArray.element?.ComercioId ?? -1));
            tareasRequest.Add(_mebi.GetListaNegraAsync(integrantes, requestArray.element?.SolicitaTarjeta));
            if(int.TryParse(requestArray.element.Titular?.Submercado ?? "-1", out var numeroSubmercado)) tareasRequest.Add(_mebi.GetConveniosCodDescuentoAsync(numeroSubmercado));
            var fuentesInformacion = await FuentesDeInformacion.CreateAsync(tareasRequest, _parametros, registro);
            fuentesInformacion.RequestEvaluaIndividuos = requestArray.element;
            politica.MapeaEntradaPolitica(fuentesInformacion, integrantes);
            Log.Information("Fin Enriquecimiento");

            //Parche ConsultaPreca a falta de API Masivas
            if (politica.Entrada.Origen_ID is 13 && politica.Entrada.Circuito is "ConsultaPreca") return DevuelvePreca(politica, registro, fuentesInformacion, integrantes);

            //Evalua Precalificados - QUITAR
            if (await politica.EvaluaPrecalificado(_parametros, registro))
            {
                Log.Information("Mapea respuesta Precalificada");
                respuesta = politica.MapeaSalida(politica.Salida);
                respuesta.IdInterno = registro.Id;
                _mebi.InsertDictamenes(politica);
                Log.Information("Envía respuesta");
                return new ResponseEvaluaIndividuosArray(respuesta);
            }

            Log.Information("Llama Modeler");
            EvaluacionCrediticia evaluacion = await EvaluacionCrediticia.EvaluarFullAsync(politica, registro, _parametros);

            Log.Information("Mapea respuesta");
            politica.Decodifica(evaluacion.ArraySalida);
            respuesta = politica.MapeaSalida(politica.Salida);
            respuesta.IdInterno = registro.Id;
            _mebi.InsertDictamenes(politica);

            Log.Information("Envía respuesta");
            return new ResponseEvaluaIndividuosArray(respuesta);
        }

        private ResponseEvaluaIndividuosArray DevuelvePreca(PoliticaEvaluaIndividuos politica, Registro registro, FuentesDeInformacion fuentesInformacion, Integrantes integrantes)
        {
            ResponseEvaluaIndividuos respuesta = new();
            respuesta.IdInterno = registro.Id;
            respuesta.OrigenId = politica.Entrada.Origen_ID ?? 0;
            respuesta.NumeroOportunidad = politica.Entrada.Numero_Oportunidad ?? 0;
            respuesta.NumeroEvaluacion = politica.Entrada.Numero_Evaluacion ?? 0;
            respuesta.Rama = "CONSULTA PRECA";
            respuesta.Cobis = integrantes.Titular?.Cobis ?? 0;
            if (respuesta.Cobis > 0) respuesta.EsCliente = "SI";
            respuesta.NumeroCUIL = integrantes.Titular?.NumeroTributario ?? 0;
            if (politica.Entrada.Solicita_Tarjeta is "SI" && fuentesInformacion.CpAnalisis.TryGetValue(integrantes.Titular?.NumeroTributario ?? 0, out var cpAnalisis))
            {
                if (cpAnalisis.VigenciaTc >= DateTime.Today)
                {
                    if (fuentesInformacion.CpAuxiliar.TryGetValue(integrantes.Titular?.NumeroTributario ?? 0, out var cpAuxiliar)) respuesta.NivelDeRiesgo = cpAuxiliar.NivelRiesgo;
                    respuesta.IngresoComputableNeto = cpAnalisis.IngresoTomado ?? decimal.Zero;
                    respuesta.Dictamen = "Precalificado";
                    respuesta.Explicacion = cpAnalisis.Dictamen;
                    respuesta.TarjetaCreditoLimite = (cpAnalisis.LimiteAOtorgarTc ?? 0);
                    return new ResponseEvaluaIndividuosArray(respuesta);
                }
            }
            respuesta.Dictamen = "No precalificado";
            respuesta.TarjetaCreditoLimite = decimal.Zero;
            return new ResponseEvaluaIndividuosArray(respuesta);
        }

        private void Validar(RequestEvaluaIndividuosArray requestArray)
        {
            string mensaje = "";
            RequestEvaluaIndividuos request = requestArray.element;
            if (request.Titular?.NumeroCUIL is null)
            {
                mensaje += "El número tributario del titular es obligatorio. ";
            }
            else
            {
                var cuilTexto = request.Titular.NumeroCUIL.ToString();
                if (cuilTexto.Length != 11) mensaje += "El número tributario del titular es inválido. ";
            }

            var cotituVacio = new RequestEvaluaIndividuosCotitular();
            cotituVacio.Actividad1 = new RequestEvaluaIndividuosActividad();
            cotituVacio.Actividad2 = new RequestEvaluaIndividuosActividad();
            if (request.Cotitular1 is not null && !cotituVacio.Equals(request.Cotitular1))
            {
                if (request.Cotitular1.NumeroCUIL is null)
                {
                    mensaje += "El número tributario del primer cotitular es obligatorio. ";
                }
                else
                {
                    var cuilTexto = request.Cotitular1.NumeroCUIL.ToString();
                    if (cuilTexto.Length != 11) mensaje += "El número tributario del primer cotitular es inválido. ";
                }
            }

            if (request.Cotitular2 is not null && !cotituVacio.Equals(request.Cotitular2))
            {
                if (request.Cotitular2.NumeroCUIL is null)
                {
                    mensaje += "El número tributario del segundo cotitular es obligatorio. ";
                }
                else
                {
                    var cuilTexto = request.Cotitular2.NumeroCUIL.ToString();
                    if (cuilTexto.Length != 11) mensaje += "El número tributario del segundo cotitular es inválido. ";
                }
            }

            if (request.Cotitular3 is not null && !cotituVacio.Equals(request.Cotitular3))
            {
                if (request.Cotitular3.NumeroCUIL is null)
                {
                    mensaje += "El número tributario del tercer cotitular es obligatorio. ";
                }
                else
                {
                    var cuilTexto = request.Cotitular3.NumeroCUIL.ToString();
                    if (cuilTexto.Length != 11) mensaje += "El número tributario del tercer cotitular es inválido. ";
                }
            }

            var cuil_Titular = request.Titular?.NumeroCUIL ?? 1;
            var cuil_Cotitu1 = request.Cotitular1?.NumeroCUIL ?? 2;
            var cuil_Cotitu2 = request.Cotitular2?.NumeroCUIL ?? 3;
            var cuil_Cotitu3 = request.Cotitular3?.NumeroCUIL ?? 4;

            if (cuil_Titular == cuil_Cotitu1
                || cuil_Titular == cuil_Cotitu2
                || cuil_Titular == cuil_Cotitu3
                || cuil_Cotitu1 == cuil_Cotitu2
                || cuil_Cotitu1 == cuil_Cotitu3
                || cuil_Cotitu2 == cuil_Cotitu3
                ) mensaje += "El número tributario de los integrantes no puede ser el mismo. ";

            if ((cuil_Cotitu3 != 4 && (cuil_Cotitu2 == 3 || cuil_Cotitu1 == 2))
                || (cuil_Cotitu2 != 3 && cuil_Cotitu1 == 2)
                ) mensaje += "Los cotitulares deben informarse en orden, arrancando por el primero. ";

            if (mensaje != "") throw new MAPIException("Validacion", 6, mensaje.TrimEnd(), 400);
        }

        public async Task<string> EvaluaRentaPresuntaAsync(RequestServicioLegacy request, Registro registro)
        {
            Guid guidAux;
            string[] campos = request.entrada.Split(";");
            string numeroTributario = (campos.Length > 6) ? campos[6] : "";
            string canal = (campos.Length > 3) ? campos[3] : "";
            string referenciaCanal = (campos.Length > 0) ? campos[0] : "";
            if (canal is "CRM") canal = "1";
            int.TryParse(canal, out var canal_int);
            registro.Inicia("EjecutaPolitica", request, numeroTributario, canal_int, referenciaCanal);
            if (Guid.TryParse((campos.Length > 80) ? campos[80] : "", out guidAux)) registro.Encadena(guidAux);
            if (_parametros.GetInt("Switch_RP", 100) < Random.Shared.Next(1, 100))
            {
                string respuesta = string.Empty;
                registro.RegistraInicio("RP", request.entrada);
                respuesta = await EvaluacionCrediticia.InvocarPoliticaAsync(request.politica, request.entrada);
                registro.RegistraFin("RP", respuesta);
                registro.Finaliza(respuesta);
                return respuesta;
            }
            int.TryParse(((campos.Length > 5) ? campos[5] : "-1"), out var cobisTIT);
            Integrantes integrantes = await _meba.GetIntegrantesPorCobisAsync([cobisTIT]);
            var tareasRequest = new List<Task<IOrigen>>();
            tareasRequest.Add(_meba.GetEndeudamientoAtrasoAsync(integrantes));
            tareasRequest.Add(_meba.GetClientesDepuradosAsync(integrantes));
            tareasRequest.Add(_mebi.GetBehaviorAsync(integrantes));
            tareasRequest.Add(_meba.GetChequesRechazadosAsync(integrantes));
            tareasRequest.Add(_meba.GetScoreComportamientoAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonAsync(integrantes));
            tareasRequest.Add(_mebi.GetExcepcionesAsync(integrantes));
            tareasRequest.Add(_meba.GetValidacionActividadAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonIndividuosAsync(integrantes));
            var fuentesInformacion = await FuentesDeInformacion.CreateAsync(tareasRequest, _parametros, registro);
            fuentesInformacion.RequestLegacy = request;
            fuentesInformacion.Integrantes = integrantes;

            var politica = new PoliticaRentaPresunta(fuentesInformacion);
            EvaluacionCrediticia evaluacion = await EvaluacionCrediticia.EvaluarFullAsync(politica, registro, _parametros);
            politica.Decodifica(evaluacion.ArraySalida);
            string tramaSalida = politica.GeneraTramaSalida();
            registro.Finaliza(tramaSalida);
            return tramaSalida;
        }
        public async Task<string> EvaluaMercadoAbiertoAsync(RequestServicioLegacy request, Registro registro)
        {
            Guid guidAux;
            string[] campos = request.entrada.Split(";");
            var numeroTributario = (campos.Length > 6) ? campos[6] : "";
            var canal = (campos.Length > 3) ? campos[3] : "";
            var referenciaCanal = (campos.Length > 0) ? campos[0] : "";
            if (Guid.TryParse((campos.Length > 134) ? campos[134] : "", out guidAux)) registro.Encadena(guidAux);
            if (canal is "CRM") canal = "1";
            int.TryParse(canal, out var canal_int);
            registro.Inicia("EjecutaPolitica", request, numeroTributario, canal_int, referenciaCanal);
            if (_parametros.GetInt("Switch_MA", 100) < Random.Shared.Next(1, 100))
            {
                string respuesta = string.Empty;
                registro.RegistraInicio("MA", request.entrada);
                respuesta = await EvaluacionCrediticia.InvocarPoliticaAsync(request.politica, request.entrada);
                registro.RegistraFin("MA", respuesta);
                registro.Finaliza(respuesta);
                return respuesta;
            }

            int.TryParse(((campos.Length > 5) ? campos[5] : "-1"), out var cobisTIT);
            int.TryParse(((campos.Length > 68) ? campos[68] : "-1"), out var cobisCOT1);
            int.TryParse(((campos.Length > 78) ? campos[78] : "-1"), out var cobisCOT2);
            int.TryParse(((campos.Length > 88) ? campos[88] : "-1"), out var cobisCOT3);
            Integrantes integrantes = await _meba.GetIntegrantesPorCobisAsync([cobisTIT, cobisCOT1, cobisCOT2, cobisCOT3]);
            var tareasRequest = new List<Task<IOrigen>>();
            tareasRequest.Add(_meba.GetEndeudamientoAtrasoAsync(integrantes));
            tareasRequest.Add(_meba.GetClientesDepuradosAsync(integrantes));
            tareasRequest.Add(_mebi.GetBehaviorAsync(integrantes));
            tareasRequest.Add(_meba.GetChequesRechazadosAsync(integrantes));
            tareasRequest.Add(_meba.GetScoreComportamientoAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonAsync(integrantes));
            tareasRequest.Add(_mebi.GetExcepcionesAsync(integrantes));
            tareasRequest.Add(_meba.GetValidacionActividadAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonIndividuosAsync(integrantes));
            var fuentesInformacion = await FuentesDeInformacion.CreateAsync(tareasRequest, _parametros, registro);
            fuentesInformacion.RequestLegacy = request;
            fuentesInformacion.Integrantes = integrantes;

            var politica = new PoliticaMercadoAbierto(fuentesInformacion);
            EvaluacionCrediticia evaluacion = await EvaluacionCrediticia.EvaluarFullAsync(politica, registro, _parametros);
            politica.Decodifica(evaluacion.ArraySalida);
            string tramaSalida = politica.GeneraTramaSalida();
            registro.Finaliza(tramaSalida);
            return tramaSalida;
        }

        public async Task<string> EvaluaPlanSueldoPrivadoAsync(RequestServicioLegacy request, Registro registro)
        {
            Guid guidAux;
            string[] campos = request.entrada.Split(";");
            var numeroTributario = (campos.Length > 6) ? campos[6] : "";
            var canal = (campos.Length > 3) ? campos[3] : "";
            var referenciaCanal = (campos.Length > 0) ? campos[0] : "";
            if (Guid.TryParse((campos.Length > 134) ? campos[134] : "", out guidAux)) registro.Encadena(guidAux);
            if (canal is "CRM") canal = "1";
            int.TryParse(canal, out var canal_int);
            registro.Inicia("EjecutaPolitica", request, numeroTributario, canal_int, referenciaCanal);
            if (_parametros.GetInt("Switch_PSP", 100) < Random.Shared.Next(1, 100))
            {
                string respuesta = string.Empty;
                registro.RegistraInicio("PSP", request.entrada);
                respuesta = await EvaluacionCrediticia.InvocarPoliticaAsync(request.politica, request.entrada);
                registro.RegistraFin("PSP", respuesta);
                registro.Finaliza(respuesta);
                return respuesta;
            }

            int.TryParse(((campos.Length > 5) ? campos[5] : "-1"), out var cobisTIT);
            int.TryParse(((campos.Length > 68) ? campos[68] : "-1"), out var cobisCOT1);
            int.TryParse(((campos.Length > 78) ? campos[78] : "-1"), out var cobisCOT2);
            int.TryParse(((campos.Length > 88) ? campos[88] : "-1"), out var cobisCOT3);
            var numeroConvenio = ((campos.Length > 88) ? campos[13] : "-1");
            Integrantes integrantes = await _meba.GetIntegrantesPorCobisAsync([cobisTIT, cobisCOT1, cobisCOT2, cobisCOT3]);
            var tareasRequest = new List<Task<IOrigen>>();
            tareasRequest.Add(_meba.GetEndeudamientoAtrasoAsync(integrantes));
            tareasRequest.Add(_meba.GetClientesDepuradosAsync(integrantes));
            tareasRequest.Add(_mebi.GetBehaviorAsync(integrantes));
            tareasRequest.Add(_meba.GetChequesRechazadosAsync(integrantes));
            tareasRequest.Add(_meba.GetScoreComportamientoAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonAsync(integrantes));
            tareasRequest.Add(_mebi.GetExcepcionesAsync(integrantes));
            tareasRequest.Add(_meba.GetValidacionActividadAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonIndividuosAsync(integrantes));
            tareasRequest.Add(_mebi.GetCalificacionConveniosAsync(numeroConvenio));
            var fuentesInformacion = await FuentesDeInformacion.CreateAsync(tareasRequest, _parametros, registro);
            fuentesInformacion.RequestLegacy = request;
            fuentesInformacion.Integrantes = integrantes;

            var politica = new PoliticaPlanSueldoPrivado(fuentesInformacion);
            EvaluacionCrediticia evaluacion = await EvaluacionCrediticia.EvaluarFullAsync(politica, registro, _parametros);
            politica.Decodifica(evaluacion.ArraySalida);
            string tramaSalida = politica.GeneraTramaSalida();
            registro.Finaliza(tramaSalida);
            return tramaSalida;
        }

        public async Task<string> EvaluaPlanSueldoPublicoAsync(RequestServicioLegacy request, Registro registro)
        {
            Guid guidAux;
            string[] campos = request.entrada.Split(";");
            var numeroTributario = (campos.Length > 6) ? campos[6] : "";
            var canal = (campos.Length > 3) ? campos[3] : "";
            var referenciaCanal = (campos.Length > 0) ? campos[0] : "";
            if (Guid.TryParse((campos.Length > 139) ? campos[139] : "", out guidAux)) registro.Encadena(guidAux);
            if (canal is "CRM") canal = "1";
            int.TryParse(canal, out var canal_int);
            registro.Inicia("EjecutaPolitica", request, numeroTributario, canal_int, referenciaCanal);
            if (_parametros.GetInt("Switch_PSPJ", 100) < Random.Shared.Next(1, 100))
            {
                string respuesta = string.Empty;
                registro.RegistraInicio("PSPJ", request.entrada);
                respuesta = await EvaluacionCrediticia.InvocarPoliticaAsync(request.politica, request.entrada);
                registro.RegistraFin("PSPJ", respuesta);
                registro.Finaliza(respuesta);
                return respuesta;
            }

            int.TryParse(((campos.Length > 5) ? campos[5] : "-1"), out var cobisTIT);
            int.TryParse(((campos.Length > 72) ? campos[72] : "-1"), out var cobisCOT1);
            int.TryParse(((campos.Length > 83) ? campos[83] : "-1"), out var cobisCOT2);
            int.TryParse(((campos.Length > 94) ? campos[94] : "-1"), out var cobisCOT3);
            Integrantes integrantes = await _meba.GetIntegrantesPorCobisAsync([cobisTIT, cobisCOT1, cobisCOT2, cobisCOT3]);
            var tareasRequest = new List<Task<IOrigen>>();
            tareasRequest.Add(_meba.GetEndeudamientoAtrasoAsync(integrantes));
            tareasRequest.Add(_meba.GetClientesDepuradosAsync(integrantes));
            tareasRequest.Add(_mebi.GetBehaviorAsync(integrantes));
            tareasRequest.Add(_meba.GetChequesRechazadosAsync(integrantes));
            tareasRequest.Add(_meba.GetScoreComportamientoAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonAsync(integrantes));
            tareasRequest.Add(_mebi.GetExcepcionesAsync(integrantes));
            tareasRequest.Add(_meba.GetValidacionActividadAsync(integrantes));
            tareasRequest.Add(_meba.GetTablonIndividuosAsync(integrantes));
            var fuentesInformacion = await FuentesDeInformacion.CreateAsync(tareasRequest, _parametros, registro);
            fuentesInformacion.RequestLegacy = request;
            fuentesInformacion.Integrantes = integrantes;

            var politica = new PoliticaPlanSueldoPublico(fuentesInformacion);
            EvaluacionCrediticia evaluacion = await EvaluacionCrediticia.EvaluarFullAsync(politica, registro, _parametros);
            politica.Decodifica(evaluacion.ArraySalida);
            string tramaSalida = politica.GeneraTramaSalida();
            registro.Finaliza(tramaSalida);
            return tramaSalida;
        }
    }
}
