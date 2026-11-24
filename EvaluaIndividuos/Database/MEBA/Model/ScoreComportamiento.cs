using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Table("fct_sc_bh_individuos_current", Schema = "dbo")]
    public class ScoreComportamiento
    {
        [Column("nro_doc_tributario")]
        public long nro_doc_tributario { get; set; } = 0;

        [Column("EXCLUSION", TypeName = "varchar(200)")]
        public string? exclusion { get; set; }

        [Column("SCORE", TypeName = "float")]
        public decimal? score { get; set; }

        [Column("CATEGORIA", TypeName = "varchar(200)")]
        public string? categoria { get; set; }
    }
}
