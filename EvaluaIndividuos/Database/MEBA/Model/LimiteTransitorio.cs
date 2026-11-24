namespace EvaluaIndividuos.Database.MEBA.Model
{
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("limite_transitorio", Schema = "dbo")]
    public class LimiteTransitorio
    {
        [Column("numero_tributario")]
        public string NumeroTributario { get; set; } = string.Empty;

        [Column("limite_transitorio_total", TypeName = "decimal(19, 2)")]
        public decimal? LimiteTransitorioTotal { get; set; }

        [Column("limite_transitorio_diferencial", TypeName = "decimal(19, 2)")]
        public decimal? LimiteTransitorioDiferencial { get; set; }

        [Column("limite_fecha_vencimiento")]
        public DateTime? LimiteFechaVencimiento { get; set; }
    }
}