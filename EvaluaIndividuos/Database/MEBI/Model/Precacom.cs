namespace EvaluaIndividuos.Database.MEBI.Model
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
    public class Precacom
    {
        [Column("PRECACOM_CODIGO")]
        public string? PrecacomCodigo { get; set; }

        [Column("PRECACOM_CUIL")]
        public long? PrecacomCuil { get; set; }

        [Column("PRECACOM_ORIGEN_ID")]
        public long? PrecacomOrigenId { get; set; }

        [Column("PRECACOM_COMERCIO_ID")]
        public long? PrecacomComercioId { get; set; }

        [Column("PRECACOM_ID_ALTERNATIVO")]
        public long? PrecacomIdAlternativo { get; set; }

        [Column("PRECACOM_PRESTAMO_MONTO_MAXIMO")]
        public long? PrecacomPrestamoMontoMaximo { get; set; }

        [Column("PRECACOM_PRESTAMO_PLAZO_MAXIMO")]
        public long? PrecacomPrestamoPlazoMaximo { get; set; }

        [Column("PRECACOM_PRESTAMO_TNA")]
        public decimal? PrecacomPrestamoTna { get; set; }

        [Column("PRECACOM_PRESTAMO_TIPO_TASA")]
        public string? PrecacomPrestamoTipoTasa { get; set; }

        [Column("PRECACOM_PRESTAMO_CUOTA_MAXIMO")]
        public long? PrecacomPrestamoCuotaMaximo { get; set; }

        [Column("PRECACOM_TARJETA_CREDITO_LIMITE")]
        public long? PrecacomTarjetaCreditoLimite { get; set; }

        [Column("PRECACOM_TARJETA_CUENTA_CORRIENTE_LIMITE")]
        public long? PrecacomTarjetaCuentaCorrienteLimite { get; set; }

        [Column("PRECACOM_VIGENTE_DESDE")]
        public DateTime? PrecacomVigenteDesde { get; set; }

        [Column("PRECACOM_VIGENTE_HASTA")]
        public DateTime? PrecacomVigenteHasta { get; set; }

        [Column("PRECACOM_VIGENCIA")]
        public int? PrecacomVigencia { get; set; }

        [Column("PRECACOM_COM1")]
        public string? PrecacomCom1 { get; set; }

        [Column("PRECACOM_COM2")]
        public string? PrecacomCom2 { get; set; }

        [Column("PRECACOM_COM3")]
        public string? PrecacomCom3 { get; set; }

        // --- NEW COLUMNS FROM BD_CAMP_EXCEPTUADAS ---
        [Column("CAMPEXCEP_Codigo_campana")]
        public string? CampexcepCodigoCampana { get; set; }

        [Column("CAMPEXCEP_FECHA_FIN_PREVISTO")]
        public DateTime? CampexcepFechaFinPrevisto { get; set; }

        [Column("CAMPEXCEP_ESTADO")]
        public string? CampexcepEstado { get; set; }

        [Column("CAMPEXCEP_BCRA_EXCEPTUADO")]
        public string? CampexcepBcraExceptuado { get; set; }

        [Column("CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO")]
        public string? CampexcepCtrlActividadExceptuado { get; set; }

        [Column("CAMPEXCEP_SUCURSAL_EXCLUSIVA")]
        public string? CampexcepSucursalExclusiva { get; set; }
    }
}