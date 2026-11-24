namespace EvaluaIndividuos.Database.MEBI.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Estructura de la tabla DICTAMENES_INDIVIDUOS_XPER de MEBI
    /// </summary>
    [Table("DICTAMENES_INDIVIDUOS_XPER")]
    public class DictamenesIndividuosXper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long? ID_Cabecera { get; set; }
        public int? Origen_ID { get; set; }
        public long? Numero_Oportunidad { get; set; }
        public int? Numero_Evaluacion { get; set; }
        public DateTime? Fecha_Hora { get; set; }
        public int? Integrante { get; set; }
        public int? Cobis { get; set; }
        public long? Cuil { get; set; }
        public int? Numero_Documento { get; set; }
        [MaxLength(20)]
        public string? Tipo_Documento { get; set; }
        [MaxLength(30)]
        public string? Version_Documento { get; set; }
        [MaxLength(1)]
        public string? Sexo { get; set; }
        [MaxLength(100)]
        public string? Apellido { get; set; }
        [MaxLength(100)]
        public string? Nombre { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Nacimiento { get; set; }
        [MaxLength(30)]
        public string? Estado_Civil { get; set; }
        [MaxLength(30)]
        public string? Segmento { get; set; }
        [MaxLength(30)]
        public string? Subsegmento { get; set; }
        [MaxLength(50)]
        public string? Actividad_1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Actividad_1_Fecha_Inicio { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Actividad_1_Neto_Fijo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Actividad_1_Neto_Variable { get; set; }
        [MaxLength(20)]
        public string? Actividad_1_Categoria_Monotributo { get; set; }
        [MaxLength(100)]
        public string? Actividad_1_Actividad_Monotributo { get; set; }
        [MaxLength(50)]
        public string? Actividad_2 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Actividad_2_Fecha_Inicio { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Actividad_2_Neto_Fijo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Actividad_2_Neto_Variable { get; set; }
        [MaxLength(20)]
        public string? Actividad_2_Categoria_Monotributo { get; set; }
        [MaxLength(100)]
        public string? Actividad_2_Actividad_Monotributo { get; set; }
        [MaxLength(36)]
        public string? ID_Interno { get; set; }
    }
}
