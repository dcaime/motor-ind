using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Keyless]
    [Table("cp_analisis")]
    public class CpAnalisis
    {
        [Column("cuil")]
        public long? Cuil { get; set; }

        [Column("codigo_cliente")]
        public int? CodigoCliente { get; set; }

        [Column("convenio")]
        public int? Convenio { get; set; }

        [Column("nombre_convenio")]
        public string? NombreConvenio { get; set; }

        [Column("sueldo_5")]
        public decimal? Sueldo5 { get; set; }

        [Column("sueldo_4")]
        public decimal? Sueldo4 { get; set; }

        [Column("sueldo_3")]
        public decimal? Sueldo3 { get; set; }

        [Column("sueldo_2")]
        public decimal? Sueldo2 { get; set; }

        [Column("sueldo_1")]
        public decimal? Sueldo1 { get; set; }

        [Column("sueldo_0")]
        public decimal? Sueldo0 { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("score_veraz")]
        public int? ScoreVeraz { get; set; }

        [Column("limite_maximo_sist_finan")]
        public decimal? LimiteMaximoSistFinan { get; set; }

        [Column("poblacion")]
        public string? Poblacion { get; set; }

        [Column("ip")]
        public string? Ip { get; set; }

        [Column("marca_onb_cp")]
        public bool? MarcaOnbCp { get; set; }

        [Column("fecha_alta_cuenta")]
        public DateTime? FechaAltaCuenta { get; set; }

        [Column("cuenta_ahorro")]
        public long? CuentaAhorro { get; set; }

        [Column("fecha_nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("alta_cp")]
        public bool? AltaCp { get; set; }

        [Column("marca_tramite")]
        public bool? MarcaTramite { get; set; }

        [Column("cliente_key")]
        public int? ClienteKey { get; set; }

        [Column("nombre_ente")]
        public string? NombreEnte { get; set; }

        [Column("apellido_ente")]
        public string? ApellidoEnte { get; set; }

        [Column("categoria_riesgo_behavior")]
        public string? CategoriaRiesgoBehavior { get; set; }

        [Column("control_bcra_ult")]
        public bool? ControlBcraUlt { get; set; }

        [Column("control_bcra_ult_3")]
        public bool? ControlBcraUlt3 { get; set; }

        [Column("marca_pd")]
        public bool? MarcaPd { get; set; }

        [Column("categoria_riesgo_interno")]
        public string? CategoriaRiesgoInterno { get; set; }

        [Column("motivo_categoria_riesgo_interno")]
        public bool? MotivoCategoriaRiesgoInterno { get; set; }

        [Column("motivo_refi_reestru")]
        public bool? MotivoRefiReestru { get; set; }

        [Column("sucursal")]
        public int? Sucursal { get; set; }

        [Column("edad")]
        public decimal? Edad { get; set; }

        [Column("calificacion")]
        public string? Calificacion { get; set; }

        [Column("nombre_campaña")]
        public string? NombreCampaña { get; set; }

        [Column("id_campaña")]
        public string? IdCampaña { get; set; }

        [Column("segmento")]
        public string? Segmento { get; set; }

        [Column("nro_cuenta")]
        public long? NroCuenta { get; set; }

        [Column("segmento_cobis")]
        public string? SegmentoCobis { get; set; }

        [Column("nro_beneficio")]
        public string? NroBeneficio { get; set; }

        [Column("pago_minimo")]
        public decimal? PagoMinimo { get; set; }

        [Column("limite_compra")]
        public decimal? LimiteCompra { get; set; }

        [Column("fecha_cabalope")]
        public DateTime? FechaCabalope { get; set; }

        [Column("motivo_deudores_bcra")]
        public bool? MotivoDeudoresBcra { get; set; }

        [Column("motivo_cheq_rech")]
        public bool? MotivoCheqRech { get; set; }

        [Column("motivo_behavior")]
        public bool? MotivoBehavior { get; set; }

        [Column("comentario")]
        public string? Comentario { get; set; }

        [Column("sub_segmento_cobis")]
        public string? SubSegmentoCobis { get; set; }

        [Column("jovenes_profesionales")]
        public bool? JovenesProfesionales { get; set; }

        [Column("id_prefiltro")]
        public string? IdPrefiltro { get; set; }

        [Column("jovenes_profesionales_analisis")]
        public bool? JovenesProfesionalesAnalisis { get; set; }

        [Column("meses_con_acreditacion_gest_6m")]
        public int? MesesConAcreditacionGest6m { get; set; }

        [Column("meses_con_acreditacion_gest_3m")]
        public int? MesesConAcreditacionGest3m { get; set; }

        [Column("acredita_ult_mes_gest")]
        public bool? AcreditaUltMesGest { get; set; }

        [Column("acredita_ant_mes_gest")]
        public bool? AcreditaAntMesGest { get; set; }

        [Column("acreditacion_corregida_promedio_neto_gest")]
        public decimal? AcreditacionCorregidaPromedioNetoGest { get; set; }

        [Column("sueldo_ip")]
        public int? SueldoIp { get; set; }

        [Column("ingreso_tomado")]
        public decimal? IngresoTomado { get; set; }

        [Column("tipo_ingreso")]
        public string? TipoIngreso { get; set; }

        [Column("division")]
        public string? Division { get; set; }

        [Column("cuota_total_sistfinan")]
        public decimal? CuotaTotalSistfinan { get; set; }

        [Column("cuota_total")]
        public decimal? CuotaTotal { get; set; }

        [Column("categoria_riesgo")]
        public string? CategoriaRiesgo { get; set; }

        [Column("max_afectacion")]
        public decimal? MaxAfectacion { get; set; }

        [Column("afectacion_disponible")]
        public decimal? AfectacionDisponible { get; set; }

        [Column("tiempo_de_permanencia")]
        public int? TiempoDePermanencia { get; set; }

        [Column("marca_potencial_selecta")]
        public bool? MarcaPotencialSelecta { get; set; }

        [Column("porc_pp")]
        public decimal? PorcPp { get; set; }

        [Column("porc_tc")]
        public decimal? PorcTc { get; set; }

        [Column("afectacion_combo_pp")]
        public decimal? AfectacionComboPp { get; set; }

        [Column("afectacion_combo_tc")]
        public decimal? AfectacionComboTc { get; set; }

        [Column("afectacion_disponible_tarjetas")]
        public long? AfectacionDisponibleTarjetas { get; set; }

        [Column("limite_x_afectacion")]
        public decimal? LimiteXAfectacion { get; set; }

        [Column("max_x_sueldo")]
        public decimal? MaxXSueldo { get; set; }

        [Column("marca_copia_limite")]
        public bool? MarcaCopiaLimite { get; set; }

        [Column("copia_limite_ssff")]
        public string? CopiaLimiteSsff { get; set; }

        [Column("maximo_tc_ssff")]
        public decimal? MaximoTcSsff { get; set; }

        [Column("limite_a_otorgar_tc")]
        public long? LimiteAOtorgarTc { get; set; }

        [Column("afectacion_maxima_adelanto")]
        public decimal? AfectacionMaximaAdelanto { get; set; }

        [Column("adelanto_sueldo_final")]
        public long? AdelantoSueldoFinal { get; set; }

        [Column("tna_adelanto")]
        public int? TnaAdelanto { get; set; }

        [Column("plazo_adelanto")]
        public int? PlazoAdelanto { get; set; }

        [Column("motivo_edad")]
        public bool? MotivoEdad { get; set; }

        [Column("motivo_ingreso_minimo")]
        public bool? MotivoIngresoMinimo { get; set; }

        [Column("motivo_afectación_disponible")]
        public bool? MotivoAfectaciónDisponible { get; set; }

        [Column("motivo_validacion_veraz")]
        public bool? MotivoValidacionVeraz { get; set; }

        [Column("motivo_nivel_riesgos")]
        public bool? MotivoNivelRiesgos { get; set; }

        [Column("motivo_situacion_bureau")]
        public bool? MotivoSituacionBureau { get; set; }

        [Column("motivo_propuesta_mínima_tc")]
        public bool? MotivoPropuestaMínimaTc { get; set; }

        [Column("motivo_propuesta_mínima_adelanto")]
        public bool? MotivoPropuestaMínimaAdelanto { get; set; }

        [Column("dictamen")]
        public string? Dictamen { get; set; }

        [Column("motivo")]
        public string? Motivo { get; set; }

        [Column("explicacion")]
        public string? Explicacion { get; set; }

        [Column("comentario_p1")]
        public string? ComentarioP1 { get; set; }

        [Column("comentario_p2")]
        public string? ComentarioP2 { get; set; }

        [Column("tipo_edad_seguro")]
        public string? TipoEdadSeguro { get; set; }

        [Column("endeudamiento_prestamos")]
        public decimal? EndeudamientoPrestamos { get; set; }

        [Column("endeudamiento_tarjeta")]
        public decimal? EndeudamientoTarjeta { get; set; }

        [Column("endeudamiento_acuerdo")]
        public decimal? EndeudamientoAcuerdo { get; set; }

        [Column("fecha_calificacion")]
        public DateTime? FechaCalificacion { get; set; }

        [Column("fecha_inicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("fecha_ejecucion")]
        public DateTime? FechaEjecucion { get; set; }

        [Column("vigencia_adelanto")]
        public DateTime? VigenciaAdelanto { get; set; }

        [Column("vigencia_tc")]
        public DateTime? VigenciaTc { get; set; }

        [Column("cuit_convenio")]
        public long? CuitConvenio { get; set; }

        [Column("tipo_convenio2")]
        public string? TipoConvenio2 { get; set; }

        [Column("tipo_convenio")]
        public string? TipoConvenio { get; set; }

        [Column("grupo_convenio")]
        public string? GrupoConvenio { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("desc_calificacion")]
        public string? DescCalificacion { get; set; }

        [Column("id_ejecucion")]
        public string? IdEjecucion { get; set; }

        [Column("id_prefiltrado")]
        public string? IdPrefiltrado { get; set; }

        [Column("fecha_hora")]
        public DateTime? FechaHora { get; set; }
    }
}