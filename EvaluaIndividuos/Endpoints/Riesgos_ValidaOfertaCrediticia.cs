using Azure.Core;
using Carter;
using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Serilog;
using static EvaluaIndividuos.Model.PagosPrestamosCanales;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EvaluaIndividuos.Endpoints
{
    public class Riesgos_ValidaOfertaCrediticia : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/Riesgos_ValidaOfertaCrediticia", async (RequestEvaluaIndividuosArray requestArray, [FromServices] Registro registro, [FromServices] IEvaluador evaluador, HttpContext httpContext) =>
            {
                if (Mock.AplicaMockRiesgos_ValidaOfertaCrediticia(requestArray)) return Mock.GetMockRiesgos_ValidaOfertaCrediticia(requestArray);
                registro.Inicia(httpContext, requestArray, requestArray.element.Titular?.NumeroCUIL?.ToString() ?? "", requestArray.element.OrigenId, requestArray.element.IdCanal ?? "");
                var respuesta = await evaluador.EvaluarIndividuosAsync(requestArray, registro);
                registro.Finaliza(respuesta);
                return respuesta;
            });
        }
    }

    public record RequestEvaluaIndividuosArray
    {
        public RequestEvaluaIndividuos element { get; set; }
    }

    public record RequestEvaluaIndividuos
    {
        public int? OrigenId { get; set; }
        public long? NumeroOportunidad { get; set; }
        public int? NumeroEvaluacion { get; set; }
        public int? ComercioId { get; set; }
        public string? ComercioSucursal { get; set; }
        public string? ComercioUsuario { get; set; }
        public string? Provincia { get; set; }
        public int? SucursalMacroAsociada { get; set; }
        public string? Canal { get; set; }
        public string? IdCanal { get; set; }
        public RequestEvaluaIndividuosNucleador? Nucleador { get; set; }
        public string[]? ProductosPrecancelados { get; set; }
        public string? PrecalificacionUtiliza { get; set; }
        public string? PrecalificacionCodigo { get; set; }
        public string? PrecalificacionOferta { get; set; }
        public string? Circuito { get; set; }
        public RequestEvaluaIndividuosTitular? Titular { get; set; }
        public RequestEvaluaIndividuosSumatoria? Sumatoria { get; set; }
        public decimal? CuotaPrestamoSinGarantiaNoBureau { get; set; }
        public decimal? CuotaPrestamoConGarantiaNoBureau { get; set; }
        public decimal? PagoMinimoTarjetaNoBureau { get; set; }
        public decimal? UtilidadCuentaCorrienteNoBureau { get; set; }
        public string? SolicitaTarjeta { get; set; }
        public decimal? TarjetaLimite { get; set; }
        public string? SolicitaPrestamo { get; set; }
        public decimal? PrestamoMonto { get; set; }
        public string? PrestamoDestino { get; set; }
        public int? PrestamoPlazo { get; set; }
        public string? PrestamoTipoTasa { get; set; }
        public decimal? PrestamoTNA { get; set; }
        public decimal? ConGarantiaValorBien { get; set; }
        public decimal? ConGarantiaSubsidio { get; set; }
        public decimal? ConGarantiaTasacion { get; set; }
        public decimal? HipotecarioValorTerreno { get; set; }
        public int? ModeloAutomovil { get; set; }
        public decimal? SeguroAutomovil { get; set; }
        public string? SolicitaAcuerdo { get; set; }
        public decimal? AcuerdoMonto { get; set; }
        public RequestEvaluaIndividuosCotitular? Cotitular1 { get; set; }
        public RequestEvaluaIndividuosCotitular? Cotitular2 { get; set; }
        public RequestEvaluaIndividuosCotitular? Cotitular3 { get; set; }
        public string Extra { get; set; }
    }

    public record RequestEvaluaIndividuosNucleador
    {
        public string? Numero { get; set; }
        public string? Tipo { get; set; }
        public string? Medio { get; set; }
        public string? Adicional { get; set; }
    }

    public record RequestEvaluaIndividuosTitular
    {
        public int? Cobis { get; set; }
        public long? NumeroCUIL { get; set; }
        public int? NumeroDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public string? VersionDocumento { get; set; }
        public string? Sexo { get; set; }
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? EstadoCivil { get; set; }
        public RequestEvaluaIndividuosActividad? Actividad1 { get; set; }
        public RequestEvaluaIndividuosActividad? Actividad2 { get; set; }
        public string? Vivienda { get; set; }
        public decimal? AlquilerMonto { get; set; }
        public string? Segmento { get; set; }
        public string? Subsegmento { get; set; }
        public string? CategoriaCliente { get; set; }
        public int? NumeroConvenio { get; set; }
        public string? NombreConvenio { get; set; }
        public string? Calificacion { get; set; }
        public string? Submercado { get; set; }
        public decimal? MontoInversor { get; set; }
        public string? TipoContribuyente { get; set; }
    }

    public record RequestEvaluaIndividuosSumatoria
    {
        public decimal? CuotaActualPrestamoPersonal { get; set; }
        public decimal? MontoDeudaActualPrestamoPersonal { get; set; }
        public decimal? CuotaActualPrestamoPrendario { get; set; }
        public decimal? MontoDeudaActualPrestamoPrendario { get; set; }
        public decimal? CuotaActualPrestamoHipotecario { get; set; }
        public decimal? MontoDeudaActualPrestamoHipotecario { get; set; }
        public decimal? CuotaActualOtrosPrestamos { get; set; }
        public decimal? MontoDeudaActualOtrosPrestamos { get; set; }
        public decimal? LimitesTarjetaCreditoMacro { get; set; }
        public decimal? PagoMinPromUltimos2MesesTCMacro { get; set; }
        public decimal? AcuerdosCuentaCorrienteMacro { get; set; }
        public decimal? UsoPromUltimos2MesesAcuerdosCCMacro { get; set; }
    }

    public record RequestEvaluaIndividuosCotitular
    {
        public int? Cobis { get; set; }
        public long? NumeroCUIL { get; set; }
        public int? NumeroDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public string? VersionDocumento { get; set; }
        public string? Sexo { get; set; }
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Segmento { get; set; }
        public string? Subsegmento { get; set; }
        public RequestEvaluaIndividuosActividad? Actividad1 { get; set; }
        public RequestEvaluaIndividuosActividad? Actividad2 { get; set; }
    }

    public record RequestEvaluaIndividuosActividad
    {
        public string? Actividad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public decimal? NetoFijo { get; set; }
        public decimal? NetoVariable { get; set; }
        public string? CategoriaMonotributo { get; set; }
        public string? ActividadMonotributo { get; set; }
    }

    public class ResponseEvaluaIndividuosArray
    {
        public List<ResponseEvaluaIndividuos> elements { get; set; } = new List<ResponseEvaluaIndividuos>();

        public ResponseEvaluaIndividuosArray() {}

        public ResponseEvaluaIndividuosArray(ResponseEvaluaIndividuos element)
        {
            elements.Add(element);
        }
    }

    public class ResponseEvaluaIndividuos
    {
        public Guid IdInterno { get; set; }
        public int OrigenId { get; set; }
        public long NumeroOportunidad { get; set; }
        public int NumeroEvaluacion { get; set; }
        public string EsCliente { get; set; }
        public int Cobis { get; set; }
        public long NumeroCUIL { get; set; }
        public string Rama { get; set; }
        public string Dictamen { get; set; }
        public string Codigos_Rechazos { get; set; }
        public string Explicacion { get; set; }
        public string NivelDeRiesgo { get; set; }
        public decimal IngresoComputableNeto { get; set; }
        public decimal PrestamoMontoMaximo { get; set; }
        public decimal PrestamoCuotaMaxima { get; set; }
        public int PrestamoPlazoMaximo { get; set; }
        public string PrestamoTipoTasa { get; set; }
        public decimal PrestamoTNA { get; set; }
        public decimal TarjetaCreditoLimite { get; set; }
        public decimal CuentaCorrienteLimite { get; set; }
        public string ReportePolitica { get; set; }
        public string OutputAdicional { get; set; }
    }
}
