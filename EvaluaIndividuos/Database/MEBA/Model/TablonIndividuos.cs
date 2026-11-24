namespace EvaluaIndividuos.Database.MEBA.Model
{
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TABLON_INDIVIDUOS", Schema = "dbo")]
    public class TablonIndividuos
    {
        [Column("CUIL")]
        public string? Cuil { get; set; }

        [Column("CODIGO_CLIENTE")]
        public string? CodigoCliente { get; set; }

        [Column("SEGMENTO")]
        public string? Segmento { get; set; }

        [Column("CALIFICACION_CONV_PRINCIPAL")]
        public string? CalificacionConvPrincipal { get; set; }

        [Column("NRO_CUENTA_PRINCIPAL")]
        public string? NroCuentaPrincipal { get; set; }

        [Column("COD_CATEGORIA_CUENTA")]
        public string? CodCategoriaCuenta { get; set; }

        [Column("MARCA_OBS_CLIENTE_NO_PRECALIFICAR")]
        public int? MarcaObsClienteNoPrecalificar { get; set; }

        [Column("MOTIVO_SITUACION_BM")]
        public int? MotivoSituacionBm { get; set; }

        [Column("MOTIVO_SIT_CART_CEDIDA")]
        public int? MotivoSitCartCedida { get; set; }

        [Column("MOTIVO_MORA_HISTORICA")]
        public int? MotivoMoraHistorica { get; set; }

        [Column("MOTIVO_BANCA_CLIENTE")]
        public int? MotivoBancaCliente { get; set; }

        [Column("MOTIVO_REFINANCIACION")]
        public int? MotivoRefinanciacion { get; set; }

        [Column("MOTIVO_REESTRUCTURACION")]
        public int? MotivoReestructuracion { get; set; }

        [Column("MOTIVO_DIFERIMIENTO")]
        public int? MotivoDiferimiento { get; set; }

        [Column("CATEGORIA_MONOTRIBUTO")]
        public string? CategoriaMonotributo { get; set; }

        [Column("ACTIVIDAD_MONOTRIBUTO")]
        public string? ActividadMonotributo { get; set; }

        [Column("MONTO_INVERSION", TypeName = "decimal(19, 2)")]
        public decimal? MontoInversion { get; set; }

        [Column("NO_ACREDITO_ULT_MES")]
        public int? NoAcreditoUltMes { get; set; }

        [Column("NO_ACREDITO_ULT_2M")]
        public int? NoAcreditoUlt2m { get; set; }

        [Column("INGRESO_CALCULADO_FINAL", TypeName = "decimal(19, 2)")]
        public decimal? IngresoCalculadoFinal { get; set; }

        [Column("TIPO_INGRESO")]
        public string? TipoIngreso { get; set; }

        [Column("CONVENIO_LIQUI_PRESTAMO")]
        public string? ConvenioLiquiPrestamo { get; set; }

        [Column("ENTE_CONVENIO")]
        public string? EnteConvenio { get; set; }

        [Column("MERCADO")]
        public string? Mercado { get; set; }

        [Column("SUBMERCADO")]
        public string? Submercado { get; set; }

        [Column("SIN_MERC_SUBM")]
        public int? SinMercSubm { get; set; }

        [Column("COD_SEGMENTO")]
        public string? CodSegmento { get; set; }

        [Column("COD_SUBSEGMENTO")]
        public string? CodSubsegmento { get; set; }

        [Column("SEXO")]
        public string? Sexo { get; set; }

        [Column("MOTIVO_CHEQ_RECH")]
        public int? MotivoCheqRech { get; set; }
    }
}