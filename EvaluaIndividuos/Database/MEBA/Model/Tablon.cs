using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Table("fct_ind_diarios_politicas_riesgos", Schema = "dbo")]
    public class Tablon
    {
        [Column("nro_doc_tributario")]
        public long NumeroTributario { get; set; } = 0;

        [Column("POL_FLAG_ACUERDO_CC", TypeName = "int")]
        public int? POL_Flag_Acuerdo_CC { get; set; }

        [Column("POL_FLAG_PRODUCTO_CREDITICIO_ACTIVO", TypeName = "int")]
        public int? FlagProductoCrediticioActivo { get; set; }

        [Column("POL_ANTIGUEDAD_TC", TypeName = "decimal(19, 2)")]
        public decimal? AntiguedadTc { get; set; }

        [Column("POL_ANTIGUEDAD_ACUERDO_CC_MAX_6", TypeName = "decimal(19, 2)")]
        public decimal? AntiguedadCc { get; set; }

        [Column("POL_CANTIDAD_OCURRENCIA_6", TypeName = "int")]
        public int? CantidadOcurrencia6 { get; set; }
    }
}
