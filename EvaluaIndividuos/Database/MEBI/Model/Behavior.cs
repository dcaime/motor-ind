using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Keyless]
    [Table("BEHAVIOUR")]
    public class Behavior
    {
        [Column("Cliente_Cobis")]
        public int? ClienteCobis { get; set; }

        [Column("Riesgo_Score")]
        public int? RiesgoScore { get; set; }

        [Column("Riesgo_6")]
        public int? Riesgo6 { get; set; }

        [Column("Uso_Tc_Limite")]
        public decimal? UsoTcLimite { get; set; }

        [Column("Uso_Tc_Exigible")]
        public decimal? UsoTcExigible { get; set; }

        [Column("Uso_Tc_Limite_Max")]
        public decimal? UsoTcLimiteMax { get; set; }

        [Column("Uso_Cc_Limite")]
        public decimal? UsoCcLimite { get; set; }

        [Column("Uso_Cc_Promedio3")]
        public decimal? UsoCcPromedio3 { get; set; }

        [Column("Uso_Sg_Exigible")]
        public decimal? UsoSgExigible { get; set; }

        [Column("Uso_Gp_Exigible")]
        public decimal? UsoGpExigible { get; set; }

        [Column("Uso_Gh_Exigible")]
        public decimal? UsoGhExigible { get; set; }

        [Column("Antiguedad_Tc")]
        public int? AntiguedadTc { get; set; }

        [Column("Antiguedad_Cc")]
        public int? AntiguedadCc { get; set; }

        [Column("Cob_Status")]
        public int? CobStatus { get; set; }

        [Column("Hit")]
        public int? Hit { get; set; }

        [Column("V_Uso_Tc_Saldo_Fin")]
        public decimal? VUsoTcSaldoFin { get; set; }

        [Column("IP2")]
        public string? Ip2 { get; set; }

        [Column("Consultar_Veraz")]
        public string? ConsultarVeraz { get; set; }
    }
}
