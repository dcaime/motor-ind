using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Keyless]
    [Table("CALIFICACION_CONVENIOS")]
    public class CalificacionConvenios
    {
        [Column("Convenio")]
        public string? Convenio { get; set; }

        [Column("Numero_Convenio")]
        public string? NumeroConvenio { get; set; }

        [Column("Calificacion")]
        public string? Calificacion { get; set; }
    }
}
