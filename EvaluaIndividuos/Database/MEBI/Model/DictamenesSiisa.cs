using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("DICTAMENES_SIISA")]
    public class DictamenesSiisa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long id { get; set; }
        [Column("fecha_hora")]
        public DateTime FechaHora { get; set; }
        [Column("cuil")]
        public string? Cuil { get; set; }
        [Column("documento")]
        public string? Documento { get; set; }
        [Column("sexo")]
        public string? Sexo { get; set; }
        [Column("nacimiento")]
        public DateOnly? Nacimiento { get; set; }
        [Column("apellido")]
        public string? Apellido { get; set; }
        [Column("nombre")]
        public string? Nombre { get; set; }
        [Column("fallecido")] 
        public string? Fallecido { get; set; }
        [Column("score")]
        public int? Score { get; set; }
        [Column("modelo")]
        public string? Modelo { get; set; }
        [Column("id_categoria")]
        public int? IdCategoria { get; set; }
        [Column("nombre_categoria")]
        public string? NombreCategoria { get; set; }
        [Column("id_modelo")]
        public int? IdModelo { get; set; }
        [Column("nombre_modelo")]
        public string? NombreModelo { get; set; }
        [Column("max_monto_categoria")]
        public string? MaxMontoCategoria { get; set; }
        [Column("prom_monto_categoria")]
        public string? PromMontoCategoria { get; set; }
        [Column("min_monto_categoria")] 
        public string? MinMontoCategoria { get; set; }
        [Column("cuit")] 
        public string? Cuit { get; set; }
        [Column("razon_social")] 
        public string? RazonSocial { get; set; }
        [Column("antiguedad")]
        public string? Antiguedad { get; set; }
        [Column("sector")] 
        public string? Sector { get; set; }
        [Column("actividad")] 
        public string? Actividad { get; set; }
        [Column("actividad_completa")] 
        public string? ActividadCompleta { get; set; }
        [Column("actividad_completa_codigo")] 
        public string? ActividadCompletaCodigo { get; set; }
        [Column("fecha_ingreso")]
        public DateOnly? FechaIngreso { get; set; }
        [Column("es_rel_dep")]
        public string? EsRelDep { get; set; }
        [Column("es_rel_dep_6m")] 
        public string? EsRelDep6m { get; set; }
        [Column("es_rel_dep_12m")] 
        public string? EsRelDep12m { get; set; }
        [Column("es_jubilado")] 
        public string? EsJubilado { get; set; }
        [Column("es_monotributo")] 
        public string? EsMonotributo { get; set; }
        [Column("es_autonomo")] 
        public string? EsAutonomo { get; set; }
        [Column("categoria_monotributo")] 
        public string? CategoriaMonotributo { get; set; }
        [Column("fecha_inscripcion")] 
        public DateOnly? FechaInscripcion { get; set; }
        [Column("autonomo")]
        public string? Autonomo { get; set; }
        [Column("impuesto_ganancias")] 
        public string? ImpuestoGanancias { get; set; }
        [Column("impuesto_iva")] 
        public string? ImpuestoIva { get; set; }
        [Column("codigo_categoria_monotributo")]
        public string? CodigoCategoriaMonotributo { get; set; }
        [Column("es_integrante_sociedad")]
        public string? EsIntegranteSociedad { get; set; }
        [Column("es_empleador")] 
        public string? EsEmpleador { get; set; }
        [Column("fue_empleador")] 
        public string? FueEmpleador { get; set; }
        [Column("pc")] 
        public string? Pc { get; set; }
        [Column("concurso_quiebra_proceso")] 
        public string? ConcursoYQuiebraProceso { get; set; }
        [Column("juicios_arba")]
        public string? JuiciosArba { get; set; }
        [Column("fraude_operacion")] 
        public string? FraudeOperacion { get; set; }
        [Column("estado_transac")]
        public int? EstadoTransac { get; set; }
        [Column("id_interno")]
        public string? IdInterno { get; set; }
    }
}
