namespace EvaluaIndividuos.Database.MEBA.Model
{
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ENDEUDAMIENTO_ATRASO", Schema = "dbo")]
    public class EndeudamientoAtraso
    {
        [Column("CODIGO_CLIENTE")]
        public long? CodigoCliente { get; set; }

        [Column("FECHA_CARGA")]
        public DateOnly? FechaCarga { get; set; }

        [Column("FECHA_CABALOPE")]
        public DateOnly? FechaCabalope { get; set; }

        [Column("FECHA_CONSULTA_REESTRUCTURACION")]
        public DateOnly? FechaConsultaReestructuracion { get; set; }

        [Column("FECHA_CONSULTA_REFI_DEU")]
        public DateOnly? FechaConsultaRefiDeu { get; set; }

        [Column("SALDO_CONTABLE_PP", TypeName = "decimal(18, 2)")]
        public decimal? SaldoContablePp { get; set; }

        [Column("MONTO_PROXIMA_CUOTA_PP", TypeName = "decimal(18, 2)")]
        public decimal? MontoProximaCuotaPp { get; set; }

        [Column("SALDO_CONTABLE_PREST_PRENDARIO", TypeName = "decimal(18, 2)")]
        public decimal? SaldoContablePrestPrendario { get; set; }

        [Column("MONTO_PROXIMA_CUOTA_PREST_PRENDARIO", TypeName = "decimal(18, 2)")]
        public decimal? MontoProximaCuotaPrestPrendario { get; set; }

        [Column("SALDO_CONTABLE_PREST_HIPOTECARIO", TypeName = "decimal(18, 2)")]
        public decimal? SaldoContablePrestHipotecario { get; set; }

        [Column("MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO", TypeName = "decimal(18, 2)")]
        public decimal? MontoProximaCuotaPrestHipotecario { get; set; }

        [Column("SALDO_CONTABLE_OTROS_PRESTAMOS", TypeName = "decimal(18, 2)")]
        public decimal? SaldoContableOtrosPrestamos { get; set; }

        [Column("MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS", TypeName = "decimal(18, 2)")]
        public decimal? MontoProximaCuotaOtrosPrestamos { get; set; }

        [Column("MAX_DIAS_ATRASO_PRESTAMOS")]
        public int? MaxDiasAtrasoPrestamos { get; set; }

        [Column("MOTIVO_REESTRUCTURACION")]
        public string? MotivoReestructuracion { get; set; }

        [Column("MOTIVO_REFINANCIACION")]
        public string? MotivoRefinanciacion { get; set; }

        [Column("MOTIVO_DEU")]
        public string? MotivoDeu { get; set; }

        [Column("DIAS_ATRASO_TC")]
        public int? DiasAtrasoTc { get; set; }

        [Column("MAX_DIAS_DESC_SIN_ACUERDO")]
        public int? MaxDiasDescSinAcuerdo { get; set; }

        [Column("LIMITE_ACUERDO", TypeName = "decimal(18, 2)")]
        public decimal? LimiteAcuerdo { get; set; }

        [Column("UTILIZACION_ACUERDO", TypeName = "decimal(18, 2)")]
        public decimal? UtilizacionAcuerdo { get; set; }

        [Column("MAX_DIAS_CLEAN_UP")]
        public int? MaxDiasCleanUp { get; set; }

        [Column("MAX_DIAS_EXCESO_ACUERDO")]
        public int? MaxDiasExcesoAcuerdo { get; set; }

        [Column("LIMITE_COMPRA_TC", TypeName = "decimal(18, 2)")]
        public decimal? LimiteCompraTc { get; set; }

        [Column("PAGO_MINIMO_PROM_ULT_2_MESES", TypeName = "decimal(18, 2)")]
        public decimal? PagoMinimoPromUlt2Meses { get; set; }
    }
}