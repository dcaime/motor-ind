namespace EvaluaIndividuos.Database.MEBA.Model
{
    using EvaluaIndividuos.Database.MEBI.Model;
    using EvaluaIndividuos.Model;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CLIENTES_DEPURADOS")]
    public class ClientesDepurados
    {
        [Column("CODIGO_CLIENTE")]
        [MaxLength(10)]
        public string? CodigoCliente { get; set; }

        [Column("CUIL")]
        [MaxLength(11)]
        public string? Cuil { get; set; }

        [Column("DEPURACION")]
        [MaxLength(30)]
        public string? Depuracion { get; set; }

        [Column("DEPURACION_HASTA")] 
        public DateTime? DepuracionHasta { get; set; }

        [Column("DEPURACION_MOTIVO")]
        [MaxLength(50)]
        public string? DepuracionMotivo { get; set; }
    }
}
