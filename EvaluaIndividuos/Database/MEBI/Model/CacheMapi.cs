using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [PrimaryKey(nameof(FechaHora), nameof(Servicio), nameof(Clave))]
    [Table("cache")]
    public class CacheMapi
    {
        [Column("fecha_hora")]
        public DateTime FechaHora { get; set; }

        [Column("servicio")]
        public string Servicio { get; set; }

        [Column("clave")]
        public long Clave { get; set; }

        [Column("cache")]
        public string? Cache { get; set; }
    }
}
