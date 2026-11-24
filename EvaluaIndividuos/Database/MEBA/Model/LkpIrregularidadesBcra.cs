using Azure;
using Azure.Core;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Endpoints;
using EvaluaIndividuos.Model;
using EvaluaIndividuos.Database.MEBA;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using System.Threading;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Table("lkp_irregularidades_bcra")]
    public class LkpIrregularidadesBcra
    {
        [Column("FECHA_KEY")]
        public int? BcraDeuFechaKey { get; set; }

        [Column("CLIENTE_KEY")]
        public int? BcraDeuClienteKey { get; set; }

        [Column("ENTIDAD_1_KEY")]
        public int? BcraDeuEntidadKey1 { get; set; }

        [Column("RECATEGORIZACION_OBLIGATORIA_ENTIDAD_1")]
        public int? BcraDeuRecObligEntidad1 { get; set; }

        [Column("ENTIDAD_2_KEY")]
        public int? BcraDeuEntidadKey2 { get; set; }

        [Column("RECATEGORIZACION_OBLIGATORIA_ENTIDAD_2")]
        public int? BcraDeuRecObligEntidad2 { get; set; }

        [Column("ENTIDAD_3_KEY")]
        public int? BcraDeuEntidadKey3 { get; set; }

        [Column("RECATEGORIZACION_OBLIGATORIA_ENTIDAD_3")]
        public int? BcraDeuRecObligEntidad3 { get; set; }

        [Column("ENTIDAD_4_KEY")]
        public int? BcraDeuEntidadKey4 { get; set; }

        [Column("RECATEGORIZACION_OBLIGATORIA_ENTIDAD_4")]
        public int? BcraDeuRecObligEntidad4 { get; set; }

        [Column("ENTIDAD_5_KEY")]
        public int? BcraDeuEntidadKey5 { get; set; }

        [Column("RECATEGORIZACION_OBLIGATORIA_ENTIDAD_5")]
        public int? BcraDeuRecObligEntidad5 { get; set; }

        [Column("CANT_SITUACION_2_ULT_PERIODO")]
        public int? BcraDeuCantDeSit2UltPeriodo { get; set; }

        [Column("CANT_SITUACION_2_ULT_12M")]
        public int? BcraDeuCantDeSit212m { get; set; }

        [Column("CANT_SITUACION_2_ULT_24M")]
        public int? BcraDeuCantDeSit2Ult24m { get; set; }

        [Column("CANT_SITUACION_IRREGULAR_ULT_24M")]
        public int? BcraDeuCantDeSitIrregUlt24m { get; set; }

        [Column("CANT_SITUACION_6_ULT_PERIODO")]
        public int? BcraDeuCantDeSit6UltPeriodo { get; set; }

        [Column("CANT_SITUACION_6_ULT_12M")]
        public int? BcraDeuCantDeSit6Ult12m { get; set; }

        [Column("CANT_SITUACION_6_ULT_24M")]
        public int? BcraDeuCantDeSit6Ult24m { get; set; }

        [Column("PEOR_SITUACION_VIGENTE")]
        public int? BcraDeuPeorSitVigente { get; set; }

        [Column("PEOR_SITUACION_ULT_6M")]
        public int? BcraDeuPeorSitUlt6m { get; set; }

        [Column("PEOR_SITUACION_ULT_12M")]
        public int? BcraDeuPeorSitUlt12m { get; set; }

        [Column("PEOR_SITUACION_ULT_24M")]
        public int? BcraDeuPeorSitUlt24m { get; set; }

        [Column("CANT_SITUACION_IRREGULAR_ULT_12M")]
        public int? BcraDeuCantDeSitIrreg12m { get; set; }

        [Column("CANT_SITUACION_IRREGULAR_ULT_PERIODO")]
        public int? BcraDeuCantDeSitIrregUltPeriodo { get; set; }

        [Column("PEOR_SITUACION_ULT_3M")]
        public int? BcraDeuPeorSitUlt3m { get; set; }

        [Column("CANT_SITUACION_2_ULT_3M")]
        public int? BcraDeuCantDeSit2Ult3m { get; set; }

        [Column("CANT_SITUACION_2_ULT_6M")]
        public int? BcraDeuCantDeSit2Ult6m { get; set; }

        [Column("CANT_SITUACION_IRREGULAR_ULT_3M")]
        public int? BcraDeuCantDeSitIrregUlt3m { get; set; }

        [Column("CANT_SITUACION_IRREGULAR_ULT_6M")]
        public int? BcraDeuCantDeSitIrregUlt6m { get; set; }

        [Column("CANT_SITUACION_6_ULT_3M")]
        public int? BcraDeuCantDeSit6Ult3m { get; set; }

        [Column("CANT_SITUACION_6_ULT_6M")]
        public int? BcraDeuCantDeSit6Ult6m { get; set; }

        [Column("NRO_DOC_TRIBUTARIO", TypeName = "bigint")]
        public long BcraDeuDocTributario { get; set; }

        [Column("MONTO_ENTIDAD_1", TypeName = "decimal(20, 4)")]
        public decimal? BcraDeuMontoEntidad1 { get; set; }

        [Column("MONTO_ENTIDAD_2", TypeName = "decimal(20, 4)")]
        public decimal? BcraDeuMontoEntidad2 { get; set; }

        [Column("MONTO_ENTIDAD_3", TypeName = "decimal(20, 4)")]
        public decimal? BcraDeuMontoEntidad3 { get; set; }

        [Column("MONTO_ENTIDAD_4", TypeName = "decimal(20, 4)")]
        public decimal? BcraDeuMontoEntidad4 { get; set; }

        [Column("MONTO_ENTIDAD_5", TypeName = "decimal(20, 4)")]
        public decimal? BcraDeuMontoEntidad5 { get; set; }

        [Column("PERIODO_VIGENCIA_IRREGULAR")]
        [MaxLength(7)]
        public string BcraDeuFecInf { get; set; } = string.Empty;

        [Column("SITUACION_ENTIDAD_1")]
        [MaxLength(5)]
        public string? BcraDeuSituacionEntidad1 { get; set; }

        [Column("SITUACION_ENTIDAD_2")]
        [MaxLength(5)]
        public string? BcraDeuSituacionEntidad2 { get; set; }

        [Column("SITUACION_ENTIDAD_3")]
        [MaxLength(5)]
        public string? BcraDeuSituacionEntidad3 { get; set; }

        [Column("SITUACION_ENTIDAD_4")]
        [MaxLength(5)]
        public string? BcraDeuSituacionEntidad4 { get; set; }

        [Column("SITUACION_ENTIDAD_5")]
        [MaxLength(5)]
        public string? BcraDeuSituacionEntidad5 { get; set; }

        [Column("ORIGEN")]
        [MaxLength(20)]
        public string? BcraDeuOrigen { get; set; }
    }
}
