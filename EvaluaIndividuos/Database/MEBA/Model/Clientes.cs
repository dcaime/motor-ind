namespace EvaluaIndividuos.Database.MEBA.Model
{
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CLIENTES")]
    public class Clientes
    {
        [Column("cliente_key")]
        [MaxLength(10)]
        public string? ClienteKey { get; set; }

        [Column("nombre")]
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Column("apellido")]
        [MaxLength(100)]
        public string? Apellido { get; set; }

        [Column("codigo_cliente")]
        [MaxLength(10)]
        public string? CodigoCliente { get; set; }

        [Column("tipo_documento")]
        [MaxLength(12)]
        public string? TipoDocumento { get; set; }

        [Column("nro_documento")]
        [MaxLength(11)]
        public string? NroDocumento { get; set; }

        [Column("tipo_doc_tributario")]
        [MaxLength(12)]
        public string? TipoDocTributario { get; set; }

        [Column(name:"nro_doc_tributario", TypeName = "nvarchar(11)")]
        [MaxLength(11)]
        public string? NroDocTributario { get; set; }

        [Column("sexo")]
        [MaxLength(1)]
        public string? Sexo { get; set; }

        [Column("Categoria_selecta")]
        [MaxLength(30)]
        public string? CategoriaSelecta { get; set; }

        [Column("categoria_source")]
        [MaxLength(8)]
        public string? CategoriaSource { get; set; }

        [Column("categoria")]
        [MaxLength(30)]
        public string? Categoria { get; set; }

        [Column("clase")]
        [MaxLength(20)]
        public string? Clase { get; set; }

        [Column("restringido")]
        [MaxLength(4)]
        public string? Restringido { get; set; }

        [Column("marca_pep")]
        [MaxLength(4)]
        public string? MarcaPep { get; set; }

        [Column("fallecido")]
        [MaxLength(4)]
        public string? Fallecido { get; set; }

        [Column("Codigo_Canal_Source")]
        [MaxLength(8)]
        public string? CodigoCanalSource { get; set; }

        [Column("Descripcion_Canal")]
        [MaxLength(30)]
        public string? DescripcionCanal { get; set; }

        [Column("banca_source")]
        [MaxLength(8)]
        public string? BancaSource { get; set; }

        [Column("banca")]
        [MaxLength(15)]
        public string? Banca { get; set; }

        [Column("segmento_source")]
        [MaxLength(8)]
        public string? SegmentoSource { get; set; }

        [Column("segmento")]
        [MaxLength(30)]
        public string? Segmento { get; set; }

        [Column("subsegmento_source")]
        [MaxLength(8)]
        public string? SubsegmentoSource { get; set; }

        [Column("subsegmento")]
        [MaxLength(30)]
        public string? Subsegmento { get; set; }

        [Column("region")]
        [MaxLength(60)]
        public string? Region { get; set; }

        [Column("division")]
        [MaxLength(40)]
        public string? Division { get; set; }

        [Column("tipo_persona")]
        [MaxLength(2)]
        public string? TipoPersona { get; set; }

        [Column("tipos_persona")]
        [MaxLength(10)]
        public string? TiposPersona { get; set; }

        [Column("Version")]
        [MaxLength(15)]
        public string? Version { get; set; }

        [Column("Opcional")]
        [MaxLength(20)]
        public string? Opcional { get; set; }

        [Column("ID_EJECUCION")]
        [MaxLength(17)]
        public string? IdEjecucion { get; set; }

        [Column("eventual")]
        public int? Eventual { get; set; }

        [Column("canal_captacion_key")]
        public int? CanalCaptacionKey { get; set; }

        [Column("sucursal_source")]
        public int? SucursalSource { get; set; }

        [Column("fecha_nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("fecha_alta")]
        public DateTime? FechaAlta { get; set; }

        [Column("fecha_depuracion_cobis")]
        public DateTime? FechaDepuracionCobis { get; set; }

        [Column("fecha_carga")]
        public DateTime? FechaCarga { get; set; }
    }
}
