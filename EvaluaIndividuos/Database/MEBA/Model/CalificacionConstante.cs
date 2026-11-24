namespace EvaluaIndividuos.Database.MEBA.Model
{
    using EvaluaIndividuos.Endpoints;
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    [Keyless]
    public class CalificacionConstante
    {
        [Column("CUIL")]
        public string? Cuil { get; set; }

        [Column("CC_ID_CAMPANA")]
        public string? CcIdCampana { get; set; }

        [Column("CC_CODIGO_CAMPANA")]
        public string? CcCodigoCampana { get; set; }

        [Column("CC_SEGMENTO_CRM")]
        public string? CcSegmentoCrm { get; set; }

        [Column("CC_SUBSEGMENTO_CRM")]
        public string? CcSubsegmentoCrm { get; set; }

        [Column("CC_SUCURSAL_MIEMBRO")]
        public int? CcSucursalMiembro { get; set; }

        [Column("CC_CANAL")]
        public string? CcCanal { get; set; }

        [Column("CC_TIPO_PRECALIFICACION")]
        public string? CcTipoPrecalificacion { get; set; }

        [Column("CC_FECHA_CALIFICACION")]
        public DateTime? CcFechaCalificacion { get; set; }

        [Column("CC_FECHA_VENCIMIENTO")]
        public DateTime? CcFechaVencimiento { get; set; }

        [Column("CC_CONVENIO")]
        public string? CcConvenio { get; set; }

        [Column("CC_PRODUCTO_PRESTAMO")]
        public string? CcProductoPrestamo { get; set; }

        [Column("CC_PRODUCTO_TARJETA")]
        public string? CcProductoTarjeta { get; set; }

        [Column("CC_PRODUCTO_ACUERDO")]
        public string? CcProductoAcuerdo { get; set; }

        [Column("CC_PRESTAMO_TASA")]
        public decimal? CcPrestamoTasa { get; set; }

        [Column("CC_PRESTAMO_PLAZO")]
        public int? CcPrestamoPlazo { get; set; }

        [Column("CC_PRESTAMO_MONTO")]
        public long? CcPrestamoMonto { get; set; }

        [Column("CC_PRESTAMO_AFECTACION")]
        public long? CcPrestamoAfectacion { get; set; }

        [Column("CC_PRESTAMO_CUOTA_PROMEDIO")]
        public long? CcPrestamoCuotaPromedio { get; set; }

        [Column("CC_PRESTAMO_CUOTA_INICIAL")]
        public long? CcPrestamoCuotaInicial { get; set; }

        [Column("CC_TARJETA_MONTO")]
        public long? CcTarjetaMonto { get; set; }

        [Column("CC_ACUERDO_MONTO")]
        public long? CcAcuerdoMonto { get; set; }

        [Column("CC_ID_OFERTAS")]
        public string? CcIdOfertas { get; set; }
    }
}