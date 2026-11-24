using System.ComponentModel.DataAnnotations;
using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("LISTA_NEGRA")]
    public class ListaNegra
    {
        [Column("LN_NRO_DOC_TRIBUTARIO", TypeName = "decimal(15, 0)")]
        public decimal NroDocTributario { get; set; }

        [Column("LN_MOTIVO_OBSERVACION_KEY")]
        public int MotivoObservacionKey { get; set; }

        [Column("LN_MOTIVO_OBSERVACION")]
        public string MotivoObservacion { get; set; }
    }
}
