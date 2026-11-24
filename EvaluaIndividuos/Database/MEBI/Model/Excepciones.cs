using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("EXCEPCIONES", Schema = "dbo")]
    public class Excepciones
    {
        [Column("DOC")]
        public string Documento { get; set; } = "";

        [Column("FECHA_VENCIMIENTO")]
        public DateTime? FechaVencimiento { get; set; }
    }
}
