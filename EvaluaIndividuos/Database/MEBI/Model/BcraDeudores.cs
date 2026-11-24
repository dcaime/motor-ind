using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("BD_BCRA_DEUDORES")]
    public class BcraDeudores
    {
        [Column("BCRADEU_fecha_key")]
        public int? BcraDeuFechaKey { get; set; }

        [Column("BCRADEU_cliente_key")]
        public int? BcraDeuClienteKey { get; set; }

        [Column("BCRADEU_entidad_key1")]
        public int? BcraDeuEntidadKey1 { get; set; }

        [Column("BCRADEU_rec_oblig_entidad1")]
        public int? BcraDeuRecObligEntidad1 { get; set; }

        [Column("BCRADEU_entidad_key2")]
        public int? BcraDeuEntidadKey2 { get; set; }

        [Column("BCRADEU_rec_oblig_entidad2")]
        public int? BcraDeuRecObligEntidad2 { get; set; }

        [Column("BCRADEU_entidad_key3")]
        public int? BcraDeuEntidadKey3 { get; set; }

        [Column("BCRADEU_rec_oblig_entidad3")]
        public int? BcraDeuRecObligEntidad3 { get; set; }

        [Column("BCRADEU_entidad_key4")]
        public int? BcraDeuEntidadKey4 { get; set; }

        [Column("BCRADEU_rec_oblig_entidad4")]
        public int? BcraDeuRecObligEntidad4 { get; set; }

        [Column("BCRADEU_entidad_key5")]
        public int? BcraDeuEntidadKey5 { get; set; }

        [Column("BCRADEU_rec_oblig_entidad5")]
        public int? BcraDeuRecObligEntidad5 { get; set; }

        [Column("BCRADEU_cant_de_sit_2_ult_periodo")]
        public int? BcraDeuCantDeSit2UltPeriodo { get; set; }

        [Column("BCRADEU_cant_de_sit_2_12m")]
        public int? BcraDeuCantDeSit212m { get; set; }

        [Column("BCRADEU_cant_de_sit_2_ult_24m")]
        public int? BcraDeuCantDeSit2Ult24m { get; set; }

        [Column("BCRADEU_cant_de_sit_ireg_ult_24m")]
        public int? BcraDeuCantDeSitIrregUlt24m { get; set; }

        [Column("BCRADEU_cant_de_sit_6_ult_periodo")]
        public int? BcraDeuCantDeSit6UltPeriodo { get; set; }

        [Column("BCRADEU_cant_de_sit_6_ult_12m")]
        public int? BcraDeuCantDeSit6Ult12m { get; set; }

        [Column("BCRADEU_cant_de_sit_6_ult_24m")]
        public int? BcraDeuCantDeSit6Ult24m { get; set; }

        [Column("BCRADEU_peor_sit_vigente")]
        public int? BcraDeuPeorSitVigente { get; set; }

        [Column("BCRADEU_peor_sit_ult_6m")]
        public int? BcraDeuPeorSitUlt6m { get; set; }

        [Column("BCRADEU_peor_sit_ult_12m")]
        public int? BcraDeuPeorSitUlt12m { get; set; }

        [Column("BCRADEU_peor_sit_ult_24m")]
        public int? BcraDeuPeorSitUlt24m { get; set; }

        [Column("BCRADEU_cant_de_sit_irreg_12m")]
        public int? BcraDeuCantDeSitIrreg12m { get; set; }

        [Column("BCRADEU_cant_de_sit_irreg_ult_periodo")]
        public int? BcraDeuCantDeSitIrregUltPeriodo { get; set; }

        [Column("BCRADEU_Peor_sit_ult_3m")]
        public int? BcraDeuPeorSitUlt3m { get; set; }

        [Column("BCRADEU_Cant_de_sit_2_ult_3m")]
        public int? BcraDeuCantDeSit2Ult3m { get; set; }

        [Column("BCRADEU_Cant_de_sit_2_ult_6m")]
        public int? BcraDeuCantDeSit2Ult6m { get; set; }

        [Column("BCRADEU_Cant_de_sit_irreg_ult_3m")]
        public int? BcraDeuCantDeSitIrregUlt3m { get; set; }

        [Column("BCRADEU_Cant_de_sit_irreg_ult_6m")]
        public int? BcraDeuCantDeSitIrregUlt6m { get; set; }

        [Column("BCRADEU_Cant_de_sit_6_ult_3m")]
        public int? BcraDeuCantDeSit6Ult3m { get; set; }

        [Column("BCRADEU_Cant_de_sit_6_ult_6m")]
        public int? BcraDeuCantDeSit6Ult6m { get; set; }

        [Column("BCRADEU_doc_tributario", TypeName = "decimal(15, 0)")]
        public decimal BcraDeuDocTributario { get; set; }

        [Column("BCRADEU_monto_entidad1", TypeName = "decimal(16, 4)")]
        public decimal? BcraDeuMontoEntidad1 { get; set; }

        [Column("BCRADEU_monto_entidad2", TypeName = "decimal(16, 4)")]
        public decimal? BcraDeuMontoEntidad2 { get; set; }

        [Column("BCRADEU_monto_entidad3", TypeName = "decimal(16, 4)")]
        public decimal? BcraDeuMontoEntidad3 { get; set; }

        [Column("BCRADEU_monto_entidad4", TypeName = "decimal(16, 4)")]
        public decimal? BcraDeuMontoEntidad4 { get; set; }

        [Column("BCRADEU_monto_entidad5", TypeName = "decimal(16, 4)")]
        public decimal? BcraDeuMontoEntidad5 { get; set; }

        [Column("BCRADEU_fec_inf")]
        [MaxLength(10)]
        public string BcraDeuFecInf { get; set; } = string.Empty;

        [Column("BCRADEU_nro_entidad1")]
        [MaxLength(10)]
        public string? BcraDeuNroEntidad1 { get; set; }

        [Column("BCRADEU_entidad1")]
        [MaxLength(150)]
        public string? BcraDeuEntidad1 { get; set; }

        [Column("BCRADEU_situacion_entidad1")]
        [MaxLength(10)]
        public string? BcraDeuSituacionEntidad1 { get; set; }

        [Column("BCRADEU_nro_entidad2")]
        [MaxLength(10)]
        public string? BcraDeuNroEntidad2 { get; set; }

        [Column("BCRADEU_entidad2")]
        [MaxLength(150)]
        public string? BcraDeuEntidad2 { get; set; }

        [Column("BCRADEU_situacion_entidad2")]
        [MaxLength(10)]
        public string? BcraDeuSituacionEntidad2 { get; set; }

        [Column("BCRADEU_nro_entidad3")]
        [MaxLength(10)]
        public string? BcraDeuNroEntidad3 { get; set; }

        [Column("BCRADEU_entidad3")]
        [MaxLength(150)]
        public string? BcraDeuEntidad3 { get; set; }

        [Column("BCRADEU_situacion_entidad3")]
        [MaxLength(10)]
        public string? BcraDeuSituacionEntidad3 { get; set; }

        [Column("BCRADEU_nro_entidad4")]
        [MaxLength(10)]
        public string? BcraDeuNroEntidad4 { get; set; }

        [Column("BCRADEU_entidad4")]
        [MaxLength(150)]
        public string? BcraDeuEntidad4 { get; set; }

        [Column("BCRADEU_situacion_entidad4")]
        [MaxLength(10)]
        public string? BcraDeuSituacionEntidad4 { get; set; }

        [Column("BCRADEU_nro_entidad5")]
        [MaxLength(10)]
        public string? BcraDeuNroEntidad5 { get; set; }

        [Column("BCRADEU_entidad5")]
        [MaxLength(150)]
        public string? BcraDeuEntidad5 { get; set; }

        [Column("BCRADEU_situacion_entidad5")]
        [MaxLength(10)]
        public string? BcraDeuSituacionEntidad5 { get; set; }

        [Column("BCRADEU_origen")]
        [MaxLength(20)]
        public string? BcraDeuOrigen { get; set; }
    }
}
