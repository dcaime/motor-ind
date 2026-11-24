namespace EvaluaIndividuos.Database.MEBA.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("lkp_clientes_completa")]
    public class LkpClientesCompleta
    {
        [Column("Codigo_Cliente", TypeName = "int")]
        public int? CodigoCliente { get; set; }

        [Column(name: "Nro_Doc_Tributario", TypeName = "bigint")]
        public long? NumeroTributario { get; set; }
    }
}