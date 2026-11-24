using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Table("cheques_rechazados_agrupados", Schema = "dbo")]
    public class ChequesRechazados
    {
        [Column("cuit_cuit_cdi", TypeName = "bigint")]
        public long numeroTributario { get; set; } = 0;

        [Column("Rech_Lev_SF_u24m_Cant", TypeName = "bigint")]
        public long? RechLevSFu24mCant { get; set; }

        [Column("Rech_NoLev_SF_u24m_Monto", TypeName = "decimal(19, 2)")]
        public decimal? RechNoLevSFu24mMonto { get; set; }

        [Column("Rech_Lev_SF_u24m_Monto", TypeName = "decimal(19, 2)")]
        public decimal? RechLevSFu24mMonto { get; set; }
    }
}
