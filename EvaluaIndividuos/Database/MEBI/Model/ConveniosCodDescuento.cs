using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Keyless]
    [Table("CONVENIOS_COD_DESCUENTO")]
    public class ConveniosCodDescuento
    {
        [Column("Numero_submercado_CCD")]
        public int? NumeroSubmercado { get; set; }

        [Column("Nombre_submercado_CCD")]
        public string? NombreSubmercado { get; set; }

        [Column("Estado_convenio_CCD")]
        public string? Estado { get; set; }

        [Column("Fecha_ult_actualizacion_CCD")]
        public DateTime? FechaUltActualizacion { get; set; }

        [Column("Calificacion_permanente_CCD")]
        public string? CalificacionPermanente { get; set; }

        [Column("Calificacion_otra_planta_CCD")]
        public string? CalificacionOtraPlanta { get; set; }

        [Column("Afectacion_pp_permanente_CCD")]
        public double? AfectacionPpPermanente { get; set; }

        [Column("Afectacion_pp_otra_planta_CCD")]
        public double? AfectacionPpOtraPlanta { get; set; }

        [Column("Afectacion_ph_permanente_CCD")]
        public double? AfectacionPhPermanente { get; set; }

        [Column("Afectacion_ph_otra_planta_CCD")]
        public double? AfectacionPhOtraPlanta { get; set; }

        [Column("Monto_maximo_permanente_CCD")]
        public double? MontoMaximoPermanente { get; set; }

        [Column("Monto_maximo_otra_planta_CCD")]
        public double? MontoMaximoOtraPlanta { get; set; }

        [Column("Cantsueldos_maximo_permanente_CCD")]
        public double? CantsueldosMaximoPermanente { get; set; }

        [Column("Cantsueldos_maximo_otra_planta_CCD")]
        public double? CantsueldosMaximoOtraPlanta { get; set; }

        [Column("Plazo_permanente_CCD")]
        public int? PlazoPermanente { get; set; }

        [Column("Plazo_otra_planta_CCD")]
        public int? PlazoOtraPlanta { get; set; }

        [Column("Max_endeu_global_permanente_CCD")]
        public double? MaxEndeuGlobalPermanente { get; set; }

        [Column("Max_endeu_global_otra_planta_CCD")]
        public double? MaxEndeuGlobalOtraPlanta { get; set; }

        [Column("Opcional1_permanente_CCD")]
        public double? Opcional1Permanente { get; set; }

        [Column("Opcional1_otra_planta_CCD")]
        public double? Opcional1OtraPlanta { get; set; }

        [Column("Opcional2_permanente_CCD")]
        public double? Opcional2Permanente { get; set; }

        [Column("Opcional2_otra_planta_CCD")]
        public double? Opcional2OtraPlanta { get; set; }
    }
}