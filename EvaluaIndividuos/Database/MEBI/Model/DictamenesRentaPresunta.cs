using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluaIndividuos.Database.MEBI.Model
{
    [Table("DICTAMENES_RP")]
    [PrimaryKey(nameof(IdOportunidad), nameof(NroLlamado), nameof(ApOrigen), nameof(IdEvaluacion), nameof(Now2))]
    public class DictamenesRentaPresunta
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

        [Column("nombre_oportunidad")]
        public string? NombreOportunidad { get; set; }

        [Column("fecha_nac")]
        public DateTime? FechaNac { get; set; }

        [Column("prov")]
        public byte? Prov { get; set; }

        [Column("zona")]
        public byte? Zona { get; set; }

        [Column("cod_sucursal")]
        public short? CodSucursal { get; set; }

        [Column("est_civil")]
        public byte? EstCivil { get; set; }

        [Column("vivienda")]
        public byte? Vivienda { get; set; }

        [Column("alquiler")]
        public double? Alquiler { get; set; }

        [Column("contribuyente")]
        public byte? Contribuyente { get; set; }

        [Column("laboral_ppal")]
        public byte? LaboralPpal { get; set; }

        [Column("fech_laboral")]
        public DateTime? FechLaboral { get; set; }

        [Column("pp_cuota_macro")]
        public double? PpCuotaMacro { get; set; }

        [Column("pp_monto_macro")]
        public double? PpMontoMacro { get; set; }

        [Column("pr_cuota_macro")]
        public double? PrCuotaMacro { get; set; }

        [Column("pr_monto_macro")]
        public double? PrMontoMacro { get; set; }

        [Column("ph_cuota_macro")]
        public double? PhCuotaMacro { get; set; }

        [Column("ph_monto_macro")]
        public double? PhMontoMacro { get; set; }

        [Column("op_cuota_macro")]
        public double? OpCuotaMacro { get; set; }

        [Column("op_monto_macro")]
        public double? OpMontoMacro { get; set; }

        [Column("lim_tarj_macro")]
        public double? LimTarjMacro { get; set; }

        [Column("pago_min_tarj_macro")]
        public double? PagoMinTarjMacro { get; set; }

        [Column("acue_cta_macro")]
        public double? AcueCtaMacro { get; set; }

        [Column("uti_cta_cte_macro")]
        public double? UtiCtaCteMacro { get; set; }

        [Column("sum_cuotas_p_sg_no_bureau")]
        public double? SumCuotasPSgNoBureau { get; set; }

        [Column("sum_cuotas_p_cg_no_bureau")]
        public double? SumCuotasPCgNoBureau { get; set; }

        [Column("sum_pagos_min_tarj_no_bureau")]
        public double? SumPagosMinTarjNoBureau { get; set; }

        [Column("sum_util_cta_cte_no_bureau")]
        public double? SumUtilCtaCteNoBureau { get; set; }

        [Column("prestamo")]
        public string? Prestamo { get; set; }

        [Column("tarjeta")]
        public string? Tarjeta { get; set; }

        [Column("cta_cte")]
        public string? CtaCte { get; set; }

        [Column("tipo_prestamo")]
        public byte? TipoPrestamo { get; set; }

        [Column("plazo")]
        public short? Plazo { get; set; }

        [Column("destino_personal")]
        public byte? DestinoPersonal { get; set; }

        [Column("tipo_tasa")]
        public byte? TipoTasa { get; set; }

        [Column("tna")]
        public double? Tna { get; set; }

        [Column("canal")]
        public string? Canal { get; set; }

        [Column("equipo_ventas")]
        public string? EquipoVentas { get; set; }

        [Column("tipo_fuente")]
        public string? TipoFuente { get; set; }

        [Column("fuente")]
        public string? Fuente { get; set; }

        [Column("segmento")]
        public string? Segmento { get; set; }

        [Column("subsegmento")]
        public string? Subsegmento { get; set; }

        [Column("nombre_plantilla")]
        public string? NombrePlantilla { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("atributo1")]
        public string? Atributo1 { get; set; }

        [Column("valor1")]
        public string? Valor1 { get; set; }

        [Column("atributo2")]
        public string? Atributo2 { get; set; }

        [Column("valor2")]
        public string? Valor2 { get; set; }

        [Column("atributo3")]
        public string? Atributo3 { get; set; }

        [Column("valor3")]
        public string? Valor3 { get; set; }

        [Column("atributo4")]
        public string? Atributo4 { get; set; }

        [Column("valor4")]
        public string? Valor4 { get; set; }

        [Column("atributo5")]
        public string? Atributo5 { get; set; }

        [Column("valor5")]
        public string? Valor5 { get; set; }

        [Column("atributo6")]
        public string? Atributo6 { get; set; }

        [Column("valor6")]
        public string? Valor6 { get; set; }

        [Column("usuario")]
        public string? Usuario { get; set; }

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("explica_Validacion")]
        public string? ExplicaValidacion { get; set; }

        [Column("edad")]
        public double? Edad { get; set; }

        [Column("antig_lab_ppal")]
        public double? AntigLabPpal { get; set; }

        [Column("tem")]
        public double? Tem { get; set; }

        [Column("alicuota_iva")]
        public double? AlicuotaIva { get; set; }

        [Column("alicuota_seguro_vida")]
        public double? AlicuotaSeguroVida { get; set; }

        [Column("pmo_actual_pesos")]
        public double? PmoActualPesos { get; set; }

        [Column("relacion_pagomin_limite_tarj_macro")]
        public double? RelacionPagominLimiteTarjMacro { get; set; }

        [Column("lim_inferior_tarj_macro")]
        public double? LimInferiorTarjMacro { get; set; }

        [Column("pm_fijo_tarj_macro")]
        public double? PmFijoTarjMacro { get; set; }

        [Column("endeudamiento_pp_interno")]
        public double? EndeudamientoPpInterno { get; set; }

        [Column("deuda_macro_sin_garantia")]
        public double? DeudaMacroSinGarantia { get; set; }

        [Column("deuda_macro_con_garantia")]
        public double? DeudaMacroConGarantia { get; set; }

        [Column("total_deuda_macro")]
        public double? TotalDeudaMacro { get; set; }

        [Column("explica_Requisitos")]
        public string? ExplicaRequisitos { get; set; }

        [Column("antiguedad_tc_macro")]
        public double? AntiguedadTcMacro { get; set; }

        [Column("antiguedad_cc_macro")]
        public double? AntiguedadCcMacro { get; set; }

        [Column("compromiso_titu_macro")]
        public double? CompromisoTituMacro { get; set; }

        [Column("score_applicant")]
        public short? ScoreApplicant { get; set; }

        [Column("ingreso_min")]
        public double? IngresoMin { get; set; }

        [Column("pm_tarj_bureau")]
        public double? PmTarjBureau { get; set; }

        [Column("sprom_ac_bureau")]
        public double? SpromAcBureau { get; set; }

        [Column("ingreso_computable")]
        public double? IngresoComputable { get; set; }

        [Column("cuota_hipo")]
        public double? CuotaHipo { get; set; }

        [Column("alquiler_final")]
        public double? AlquilerFinal { get; set; }

        [Column("ingreso_computable_neto")]
        public double? IngresoComputableNeto { get; set; }

        [Column("compromiso_total_final")]
        public double? CompromisoTotalFinal { get; set; }

        [Column("afectacion_ingreso_computable_neto")]
        public double? AfectacionIngresoComputableNeto { get; set; }

        [Column("compromiso_ingreso")]
        public double? CompromisoIngreso { get; set; }

        [Column("afectacion_adicional_disponible")]
        public double? AfectacionAdicionalDisponible { get; set; }

        [Column("tope_endeudamiento_global_interno")]
        public double? TopeEndeudamientoGlobalInterno { get; set; }

        [Column("tope_maximo_modelo")]
        public double? TopeMaximoModelo { get; set; }

        [Column("tope_endeudamiento_pp_interno")]
        public double? TopeEndeudamientoPpInterno { get; set; }

        [Column("tope_endeudamiento_limite_interno")]
        public double? TopeEndeudamientoLimiteInterno { get; set; }

        [Column("tope_endeudamiento_acuerdo_interno")]
        public double? TopeEndeudamientoAcuerdoInterno { get; set; }

        [Column("porc_afectacion_prestamo")]
        public double? PorcAfectacionPrestamo { get; set; }

        [Column("importe_disp_prestamo_recalculo")]
        public double? ImporteDispPrestamoRecalculo { get; set; }

        [Column("monto_prestamo_recalculo")]
        public double? MontoPrestamoRecalculo { get; set; }

        [Column("seguro_vida_recalculo")]
        public double? SeguroVidaRecalculo { get; set; }

        [Column("interes_primera_cuota_recalculo")]
        public double? InteresPrimeraCuotaRecalculo { get; set; }

        [Column("cuota_recalculada")]
        public double? CuotaRecalculada { get; set; }

        [Column("monto_recalculado_pesos")]
        public double? MontoRecalculadoPesos { get; set; }

        [Column("cuota_pura_final")]
        public double? CuotaPuraFinal { get; set; }

        [Column("seguro_vida_final")]
        public double? SeguroVidaFinal { get; set; }

        [Column("interes_primera_cuota_final")]
        public double? InteresPrimeraCuotaFinal { get; set; }

        [Column("cuota_total")]
        public double? CuotaTotal { get; set; }

        [Column("monto_prestamo")]
        public double? MontoPrestamo { get; set; }

        [Column("porc_afectacion_tarjeta")]
        public double? PorcAfectacionTarjeta { get; set; }

        [Column("tarj_activa_peerbank")]
        public string? TarjActivaPeerbank { get; set; }

        [Column("tarjeta_competitiva")]
        public string? TarjetaCompetitiva { get; set; }

        [Column("limite_externo")]
        public double? LimiteExterno { get; set; }

        [Column("pago_minimo_tarj_sugerido_pesos")]
        public double? PagoMinimoTarjSugeridoPesos { get; set; }

        [Column("pm_fijo_recom")]
        public double? PmFijoRecom { get; set; }

        [Column("relacion_pagomin_limite_recom")]
        public double? RelacionPagominLimiteRecom { get; set; }

        [Column("lim_inferior_recom")]
        public double? LimInferiorRecom { get; set; }

        [Column("compromiso_total_actual_final")]
        public double? CompromisoTotalActualFinal { get; set; }

        [Column("pm_behavior")]
        public double? PmBehavior { get; set; }

        [Column("copia_lim_externo")]
        public string? CopiaLimExterno { get; set; }

        [Column("recomendado_inicial")]
        public double? RecomendadoInicial { get; set; }

        [Column("ltrp_cantidad")]
        public double? LtrpCantidad { get; set; }

        [Column("lim_tarj_recomendado_pesos_copia")]
        public double? LimTarjRecomendadoPesosCopia { get; set; }

        [Column("lim_tarj_recomendado_pesos_propio")]
        public double? LimTarjRecomendadoPesosPropio { get; set; }

        [Column("lim_tarj_recomendado_final")]
        public double? LimTarjRecomendadoFinal { get; set; }

        [Column("limite_tarjeta")]
        public double? LimiteTarjeta { get; set; }

        [Column("porc_afectacion_acuerdo")]
        public double? PorcAfectacionAcuerdo { get; set; }

        [Column("pago_minimo_acuerdo_sugerido")]
        public double? PagoMinimoAcuerdoSugerido { get; set; }

        [Column("limite_acuerdo_recomendado_pesos")]
        public double? LimiteAcuerdoRecomendadoPesos { get; set; }

        [Column("monto_acuerdo_cta_cte")]
        public double? MontoAcuerdoCtaCte { get; set; }

        [Column("ddjj_de_salud")]
        public string? DdjjDeSalud { get; set; }

        [Column("explica_Disponible")]
        public string? ExplicaDisponible { get; set; }

        [Column("aclaracion_Disponible")]
        public string? AclaracionDisponible { get; set; }

        [Column("Flag_copia_limite")]
        public byte? FlagCopiaLimite { get; set; }

        [Column("motivo")]
        public string? Motivo { get; set; }

        [Column("ingreso_total_mediano")]
        public double? IngresoTotalMediano { get; set; }

        [Column("codigo_rechazo")]
        public string? CodigoRechazo { get; set; }

        [Column("Mora_Suc_6M_90")]
        public double? MoraSuc6M90 { get; set; }

        [Column("Mora_Suc_12M_90")]
        public double? MoraSuc12M90 { get; set; }

        [Column("resolucion")]
        public string? Resolucion { get; set; }

        [Column("POL_Flag_Acuerdo_CC")]
        public byte? PolFlagAcuerdoCc { get; set; }

        [Column("POL_Cantidad_Ocurrencia_6")]
        public byte? PolCantidadOcurrencia6 { get; set; }

        [Column("atributo7")]
        public string? Atributo7 { get; set; }

        [Column("valor7")]
        public string? Valor7 { get; set; }

        [Column("atributo8")]
        public string? Atributo8 { get; set; }

        [Column("valor8")]
        public string? Valor8 { get; set; }

        [Column("atributo9")]
        public string? Atributo9 { get; set; }

        [Column("valor9")]
        public string? Valor9 { get; set; }

        [Column("atributo10")]
        public string? Atributo10 { get; set; }

        [Column("valor10")]
        public string? Valor10 { get; set; }

        [Column("version")]
        public string? Version { get; set; }

        [Column("Nivel_de_Riesgo")]
        public string? NivelDeRiesgo { get; set; }

        [Column("ID_Interno")]
        public string? IdInterno { get; set; }

        [Column("porc_afect_pp")]
        public decimal? PorcAfectPp { get; set; }

        [Column("porc_afect_macro")]
        public decimal? PorcAfectMacro { get; set; }

        [Column("pmo_porc")]
        public decimal? PmoPorc { get; set; }

        [Column("macro_porc")]
        public decimal? MacroPorc { get; set; }

        [Column("afect_adic_disp_pmo_porc")]
        public decimal? AfectAdicDispPmoPorc { get; set; }

        [Column("afect_adic_disp_pmo_pesos")]
        public decimal? AfectAdicDispPmoPesos { get; set; }

        [Column("afect_adic_disp_macro_porc")]
        public decimal? AfectAdicDispMacroPorc { get; set; }

        [Column("afect_adic_disp_macro_pesos")]
        public decimal? AfectAdicDispMacroPesos { get; set; }

        [Column("monto_afect_adic_disp")]
        public decimal? MontoAfectAdicDisp { get; set; }

        [Column("afect_adic_disp_porc")]
        public decimal? AfectAdicDispPorc { get; set; }

        [Column("cuota_pmo_recalculo_pesos")]
        public decimal? CuotaPmoRecalculoPesos { get; set; }
    }
}
