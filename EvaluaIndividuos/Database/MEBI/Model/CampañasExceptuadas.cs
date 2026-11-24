using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Endpoints;
using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("BD_CAMP_EXCEPTUADAS")]
    public class CampañasExceptuadas
    {
        [Column("CAMPEXCEP_Codigo_campana", TypeName = "varchar(48)")]
        public string? CodigoCampana { get; set; }

        [Column("CAMPEXCEP_FECHA_FIN_PREVISTO", TypeName = "date")]
        public DateTime? FechaFinPrevisto { get; set; }

        [Column("CAMPEXCEP_ESTADO", TypeName = "varchar(28)")]
        public string? Estado { get; set; }

        [Column("CAMPEXCEP_BCRA_EXCEPTUADO", TypeName = "varchar(8)")]
        public string? BcraExceptuado { get; set; }

        [Column("CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO", TypeName = "varchar(8)")]
        public string? ActividadExceptuado { get; set; }

        [Column("CAMPEXCEP_SUCURSAL_EXCLUSIVA", TypeName = "varchar(20)")]
        public string? SucursalExclusiva { get; set; }
    }
}
