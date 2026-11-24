using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Table("VALIDACION_ACTIVIDAD")]
    public class ValidacionActividad
    {
        [Column("CUIT")]
        public long? Cuit { get; set; }

        [Column("NOMBRE_PE")]
        [MaxLength(100)]
        public string? NombrePe { get; set; }

        [Column("APELLIDO_PE")]
        [MaxLength(100)]
        public string? ApellidoPe { get; set; }

        [Column("SEXO_PE")]
        [MaxLength(2)]
        public string? SexoPe { get; set; }

        [Column("FECHA_NACIMIENTO_PE")]
        public DateTime? FechaNacimientoPe { get; set; }

        [Column("PROVINCIA_PE")]
        [MaxLength(20)]
        public string? ProvinciaPe { get; set; }

        [Column("CODIGO_POSTAL_PE")]
        [MaxLength(10)]
        public string? CodigoPostalPe { get; set; }

        [Column("FECHA_CARGA")]
        public DateTime? FechaCarga { get; set; }

        [Column("PERIODO_SCORE")]
        public int? PeriodoScore { get; set; }

        [Column("EMPLEADOR")]
        public long? Empleador { get; set; }

        [Column("FLAG_CREDITICIO")]
        public int? FlagCrediticio { get; set; }

        [Column("SUM_SCORE", TypeName = "decimal(18, 2)")]
        public decimal? SumScore { get; set; }

        [Column("RAZON_SOCIAL_AFIP")]
        [MaxLength(200)]
        public string? RazonSocialAfip { get; set; }

        [Column("IMPUESTO_GANANCIAS")]
        [MaxLength(10)]
        public string? ImpuestoGanancias { get; set; }

        [Column("IMPUESTO_IVA")]
        [MaxLength(10)]
        public string? ImpuestoIva { get; set; }

        [Column("INTEGRANTE_SOCIEDAD")]
        [MaxLength(10)]
        public string? IntegranteSociedad { get; set; }

        [Column("ES_EMPLEADOR_AFIP")]
        [MaxLength(10)]
        public string? EsEmpleadorAfip { get; set; }

        [Column("CAT_MONOTRIBUTO")]
        [MaxLength(10)]
        public string? CatMonotributo { get; set; }

        [Column("ACTIVIDAD_MONOTRIBUTO")]
        [MaxLength(10)]
        public string? ActividadMonotributo { get; set; }

        [Column("ACTIVIDAD_MONOTRIBUTO_DESCRIPCION")]
        [MaxLength(50)]
        public string? ActividadMonotributoDescripcion { get; set; }

        [Column("FACT_MONOTRIBUTO", TypeName = "decimal(18, 2)")]
        public decimal? FactMonotributo { get; set; }

        [Column("INGRESO_SERVICIOS", TypeName = "decimal(18, 2)")]
        public decimal? IngresoServicios { get; set; }

        [Column("INGRESO_VENTAS", TypeName = "decimal(18, 2)")]
        public decimal? IngresoVentas { get; set; }

        [Column("INGRESO_MONOTRIBUTO", TypeName = "decimal(18, 2)")]
        public decimal? IngresoMonotributo { get; set; }

        [Column("ES_EMPLEADO")]
        [MaxLength(10)]
        public string? EsEmpleado { get; set; }

        [Column("ES_AUTONOMO")]
        [MaxLength(10)]
        public string? EsAutonomo { get; set; }

        [Column("ES_MONOTRIBUTISTA")]
        [MaxLength(10)]
        public string? EsMonotributista { get; set; }

        [Column("ES_JUBILADO")]
        [MaxLength(10)]
        public string? EsJubilado { get; set; }

        [Column("INGRESO_TRANSACCIONALIDAD", TypeName = "decimal(18, 2)")]
        public decimal? IngresoTransaccionalidad { get; set; }

        [Column("OPCIONAL1")]
        [MaxLength(200)]
        public string? Opcional1 { get; set; }

        [Column("VA1")]
        public string? Va1 { get; set; }

        [Column("VA2")]
        public string? Va2 { get; set; }

        [Column("VA3")]
        public string? Va3 { get; set; }

        [Column("VA4")]
        public string? Va4 { get; set; }

        [Column("VA5")]
        public string? Va5 { get; set; }

        [Column("VA6")]
        public string? Va6 { get; set; }

        [Column("VA7")]
        public string? Va7 { get; set; }

        [Column("VA8")]
        public string? Va8 { get; set; }

        [Column("VA9")]
        public string? Va9 { get; set; }

        [Column("VA10")]
        public string? Va10 { get; set; }

        [Column("VA11")]
        public string? Va11 { get; set; }

        [Column("VA12")]
        public string? Va12 { get; set; }

        [Column("VA13")]
        public string? Va13 { get; set; }

        [Column("VA14")]
        public string? Va14 { get; set; }

        [Column("VA15")]
        public string? Va15 { get; set; }

        [Column("VA16")]
        public string? Va16 { get; set; }

        [Column("VA17")]
        public string? Va17 { get; set; }

        [Column("VA18")]
        public string? Va18 { get; set; }

        [Column("VA19")]
        public string? Va19 { get; set; }

        [Column("VA20")]
        public string? Va20 { get; set; }

        [Column("VA21")]
        public string? Va21 { get; set; }

        [Column("VA22")]
        public string? Va22 { get; set; }

        [Column("VA23")]
        public string? Va23 { get; set; }

        [Column("VA24")]
        public string? Va24 { get; set; }

        [Column("VA25")]
        public string? Va25 { get; set; }

        [Column("VA26")]
        public string? Va26 { get; set; }

        [Column("VA27")]
        public string? Va27 { get; set; }

        [Column("VA28")]
        public string? Va28 { get; set; }

        [Column("VA29")]
        public string? Va29 { get; set; }

        [Column("VA30")]
        public string? Va30 { get; set; }

        [Column("VA31")]
        public string? Va31 { get; set; }

        [Column("VA32")]
        public string? Va32 { get; set; }

        [Column("VA33")]
        public string? Va33 { get; set; }

        [Column("VA34")]
        public string? Va34 { get; set; }

        [Column("VA35")]
        public string? Va35 { get; set; }

        [Column("VA36")]
        public string? Va36 { get; set; }

        [Column("VA37")]
        public string? Va37 { get; set; }

        [Column("VA38")]
        public string? Va38 { get; set; }

        [Column("VA39")]
        public string? Va39 { get; set; }

        [Column("VA40")]
        public string? Va40 { get; set; }

        [Column("VA41")]
        public string? Va41 { get; set; }

        [Column("VA42")]
        public string? Va42 { get; set; }

        [Column("VA43")]
        public string? Va43 { get; set; }

        [Column("VA44")]
        public string? Va44 { get; set; }

        [Column("VA45")]
        public string? Va45 { get; set; }

        [Column("VA46")]
        public string? Va46 { get; set; }

        [Column("VA47")]
        public string? Va47 { get; set; }

        [Column("VA48")]
        public string? Va48 { get; set; }

        [Column("VA49")]
        public string? Va49 { get; set; }

        [Column("VA50")]
        public string? Va50 { get; set; }
    }
}
