using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [PrimaryKey(nameof(IdOportunidad), nameof(NroLlamado), nameof(ApOrigen), nameof(IdEvaluacion), nameof(Now2))]
    [Table("DICTAMENES_RP_XPER")]
    public class DictamenesRentaPresuntaXper
    {
        [Column("id_oportunidad")]
        public long IdOportunidad { get; set; }

        [Column("nro_llamado")]
        public long NroLlamado { get; set; }

        [Column("ap_origen")]
        public string ApOrigen { get; set; } = string.Empty;

        [Column("id_evaluacion")]
        public long IdEvaluacion { get; set; }

        [Column("NOW2")]
        public DateTime Now2 { get; set; }

        [Column("ID")]
        public byte Id { get; set; }

        [Column("id_cobis")]
        public long? IdCobis { get; set; }

        [Column("documento")]
        public long? Documento { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("apellido")]
        public string? Apellido { get; set; }

        [Column("sexo")]
        public string? Sexo { get; set; }

        [Column("version_doc")]
        public string? VersionDoc { get; set; }

        [Column("tipo_doc")]
        public string? TipoDoc { get; set; }

        [Column("neto_declarado")]
        public double? NetoDeclarado { get; set; }

        [Column("dias_atraso")]
        public int? DiasAtraso { get; set; }

        [Column("Cliente_Macro")]
        public string? ClienteMacro { get; set; }

        [Column("Rech_NoLev_SF_u24m_Monto")]
        public double? RechNoLevSfU24mMonto { get; set; }

        [Column("Rech_Lev_SF_u24m_Cant")]
        public int? RechLevSfU24mCant { get; set; }

        [Column("Rech_Lev_SF_u24m_Monto")]
        public double? RechLevSfU24mMonto { get; set; }

        [Column("score_comportamiento_interno")]
        public int? ScoreComportamientoInterno { get; set; }

        [Column("ingreso_estimado")]
        public double? IngresoEstimado { get; set; }

        [Column("sumo_sum_pagos_min_tarj_no_bureau")]
        public string? SumoSumPagosMinTarjNoBureau { get; set; }

        [Column("sumo_sum_util_cta_cte_no_bureau")]
        public string? SumoSumUtilCtaCteNoBureau { get; set; }

        [Column("sumo_pm_tj_bureau")]
        public string? SumoPmTjBureau { get; set; }

        [Column("sumo_sprom_ac_bureau")]
        public string? SumoSpromAcBureau { get; set; }

        [Column("compromiso_bureau")]
        public double? CompromisoBureau { get; set; }

        [Column("explica_Informe")]
        public string? ExplicaInforme { get; set; }

        [Column("control_Datos")]
        public string? ControlDatos { get; set; }

        [Column("nivel_Riesgo")]
        public string? NivelRiesgo { get; set; }

        [Column("resolucion")]
        public string? Resolucion { get; set; }

        [Column("categoria")]
        public string? Categoria { get; set; }

        [Column("exclusion")]
        public string? Exclusion { get; set; }

        [Column("ID_Interno")]
        public string? IdInterno { get; set; }
    }
}