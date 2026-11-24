using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Keyless]
    [Table("cp_auxiliar")]
    public class CpAuxiliar
    {
        [Column("cuil")]
        public long? Cuil { get; set; }

        [Column("codigo_cliente")]
        public long? CodigoCliente { get; set; }

        [Column("convenio")]
        public long? Convenio { get; set; }

        [Column("sucursal_riesgo")]
        public long? SucursalRiesgo { get; set; }

        [Column("segmento")]
        public string? Segmento { get; set; }

        [Column("segmento_sigma")]
        public string? SegmentoSigma { get; set; }

        [Column("subsegmento_sigma")]
        public string? SubsegmentoSigma { get; set; }

        [Column("tipo_ingreso")]
        public string? TipoIngreso { get; set; }

        [Column("limite_a_otorgar_tc")]
        public long? LimiteAOtorgarTc { get; set; }

        [Column("afectacion_maxima_adelanto")]
        public double? AfectacionMaximaAdelanto { get; set; }

        [Column("adelanto_sueldo_final")]
        public long? AdelantoSueldoFinal { get; set; }

        [Column("tna_adelanto")]
        public int? TnaAdelanto { get; set; }

        [Column("plazo_adelanto")]
        public int? PlazoAdelanto { get; set; }

        [Column("fecha_inicio")]
        public string? FechaInicio { get; set; }

        [Column("fecha_vencimiento")]
        public string? FechaVencimiento { get; set; }

        [Column("id_ejecucion")]
        public string? IdEjecucion { get; set; }

        [Column("segmento_cod")]
        public string? SegmentoCod { get; set; }

        [Column("subsegmento_cod")]
        public string? SubsegmentoCod { get; set; }

        [Column("rama_politica")]
        public string? RamaPolitica { get; set; }

        [Column("nombre_oferta")]
        public string? NombreOferta { get; set; }

        [Column("mensaje_oferta")]
        public string? MensajeOferta { get; set; }

        [Column("fecha_hora")]
        public DateTime? FechaHora { get; set; }

        [Column("nivel_riesgo")]
        public string? NivelRiesgo { get; set; }
    }
}