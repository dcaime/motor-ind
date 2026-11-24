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
    [Table("MORA_SUCURSAL")]
    public class MoraSucursal
    {
        [Column("Sucursal_Source")]
        public long SucursalSource { get; set; }

        [Column("Mora_Suc_6M_90")]
        public decimal MoraSuc6M90 { get; set; }

        [Column("Mora_Suc_12M_90")]
        public decimal MoraSuc12M90 { get; set; }
    }
}
