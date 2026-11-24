using EvaluaIndividuos.Database;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI.Model;
using System.Data;
using System.Text;

namespace EvaluaIndividuos.Model
{
    public class PoliticaPlanSueldoPublico : IEvaluable, IDictaminable
    {
        public PoliticaPlanSueldoPublicoEntrada Entrada { get; set; } = new();
        private PoliticaPlanSueldoPublicoSalida _Salida = new();
        private Politica _Politica;

        public PoliticaPlanSueldoPublico()
        {
            _Politica = Politica.PlanSueldoPublico;
        }

        public PoliticaPlanSueldoPublico(FuentesDeInformacion fuentes) : this()
        {
            MapeaEntrada(fuentes);
        }

        public PoliticaPlanSueldoPublicoSalida Salida
        {
            get { return _Salida; }
        }

        public Politica GetPolitica()
        {
            return _Politica;
        }

        public string[] Codifica()
        {
            var diccionarioEntrada = new string[_Politica.CamposEntrada + 1];
            diccionarioEntrada[diccionarioEntrada.Length - 1] = "FIN";
            diccionarioEntrada[0] = Herramientas.ConvertirATexto(Entrada.id_oportunidad);
            diccionarioEntrada[1] = Herramientas.ConvertirATexto(Entrada.nombre_oportunidad);
            diccionarioEntrada[2] = Herramientas.ConvertirATexto(Entrada.id_evaluacion);
            diccionarioEntrada[3] = Herramientas.ConvertirATexto(Entrada.ap_origen);
            diccionarioEntrada[4] = Herramientas.ConvertirATexto(Entrada.nro_llamado);
            diccionarioEntrada[5] = Herramientas.ConvertirATexto(Entrada.id_cobis);
            diccionarioEntrada[6] = Herramientas.ConvertirATexto(Entrada.documento);
            diccionarioEntrada[7] = Herramientas.ConvertirATexto(Entrada.nombre);
            diccionarioEntrada[8] = Herramientas.ConvertirATexto(Entrada.apellido);
            diccionarioEntrada[9] = Herramientas.ConvertirATexto(Entrada.sexo);
            diccionarioEntrada[10] = Herramientas.ConvertirATexto(Entrada.version_doc);
            diccionarioEntrada[11] = Herramientas.ConvertirATexto(Entrada.tipo_doc);
            diccionarioEntrada[12] = Herramientas.ConvertirATexto(Entrada.fecha_nac);
            diccionarioEntrada[13] = Herramientas.ConvertirATexto(Entrada.nro_convenio);
            diccionarioEntrada[14] = Herramientas.ConvertirATexto(Entrada.nombre_convenio);
            diccionarioEntrada[15] = Herramientas.ConvertirATexto(Entrada.calificacion);
            diccionarioEntrada[16] = Herramientas.ConvertirATexto(Entrada.submercado);
            diccionarioEntrada[17] = Herramientas.ConvertirATexto(Entrada.prov);
            diccionarioEntrada[18] = Herramientas.ConvertirATexto(Entrada.zona);
            diccionarioEntrada[19] = Herramientas.ConvertirATexto(Entrada.cod_sucursal);
            diccionarioEntrada[20] = Herramientas.ConvertirATexto(Entrada.est_civil);
            diccionarioEntrada[21] = Herramientas.ConvertirATexto(Entrada.flia);
            diccionarioEntrada[22] = Herramientas.ConvertirATexto(Entrada.vivienda);
            diccionarioEntrada[23] = Herramientas.ConvertirATexto(Entrada.contribuyente);
            diccionarioEntrada[24] = Herramientas.ConvertirATexto(Entrada.laboral_ppal);
            diccionarioEntrada[25] = Herramientas.ConvertirATexto(Entrada.fech_laboral);
            diccionarioEntrada[26] = Herramientas.ConvertirATexto(Entrada.neto_fijo_ppal);
            diccionarioEntrada[27] = Herramientas.ConvertirATexto(Entrada.neto_var_ppal);
            diccionarioEntrada[28] = Herramientas.ConvertirATexto(Entrada.act_sec);
            diccionarioEntrada[29] = Herramientas.ConvertirATexto(Entrada.laboral_sec);
            diccionarioEntrada[30] = Herramientas.ConvertirATexto(Entrada.monotributo_sec);
            diccionarioEntrada[31] = Herramientas.ConvertirATexto(Entrada.categoria_selecta);
            diccionarioEntrada[32] = Herramientas.ConvertirATexto(Entrada.actividad_monotributo_sec);
            diccionarioEntrada[33] = Herramientas.ConvertirATexto(Entrada.fech_laboral_sec);
            diccionarioEntrada[34] = Herramientas.ConvertirATexto(Entrada.neto_fijo_sec);
            diccionarioEntrada[35] = Herramientas.ConvertirATexto(Entrada.neto_var_sec);
            diccionarioEntrada[36] = Herramientas.ConvertirATexto(Entrada.pp_cuota_macro);
            diccionarioEntrada[37] = Herramientas.ConvertirATexto(Entrada.pp_monto_macro);
            diccionarioEntrada[38] = Herramientas.ConvertirATexto(Entrada.pr_cuota_macro);
            diccionarioEntrada[39] = Herramientas.ConvertirATexto(Entrada.pr_monto_macro);
            diccionarioEntrada[40] = Herramientas.ConvertirATexto(Entrada.ph_cuota_macro);
            diccionarioEntrada[41] = Herramientas.ConvertirATexto(Entrada.ph_monto_macro);
            diccionarioEntrada[42] = Herramientas.ConvertirATexto(Entrada.op_cuota_macro);
            diccionarioEntrada[43] = Herramientas.ConvertirATexto(Entrada.op_monto_macro);
            diccionarioEntrada[44] = Herramientas.ConvertirATexto(Entrada.lim_tarj_macro);
            diccionarioEntrada[45] = Herramientas.ConvertirATexto(Entrada.pago_min_tarj_macro);
            diccionarioEntrada[46] = Herramientas.ConvertirATexto(Entrada.acue_cta_macro);
            diccionarioEntrada[47] = Herramientas.ConvertirATexto(Entrada.uti_cta_cte_macro);
            diccionarioEntrada[48] = Herramientas.ConvertirATexto(Entrada.dias_atraso);
            diccionarioEntrada[49] = Herramientas.ConvertirATexto(Entrada.sum_cuotas_p_sg_no_bureau);
            diccionarioEntrada[50] = Herramientas.ConvertirATexto(Entrada.sum_cuotas_p_cg_no_bureau);
            diccionarioEntrada[51] = Herramientas.ConvertirATexto(Entrada.sum_pagos_min_tarj_no_bureau);
            diccionarioEntrada[52] = Herramientas.ConvertirATexto(Entrada.sum_util_cta_cte_no_bureau);
            diccionarioEntrada[53] = Herramientas.ConvertirATexto(Entrada.prestamo);
            diccionarioEntrada[54] = Herramientas.ConvertirATexto(Entrada.tarjeta);
            diccionarioEntrada[55] = Herramientas.ConvertirATexto(Entrada.cta_cte);
            diccionarioEntrada[56] = Herramientas.ConvertirATexto(Entrada.tipo_prestamo);
            diccionarioEntrada[57] = Herramientas.ConvertirATexto(Entrada.plazo);
            diccionarioEntrada[58] = Herramientas.ConvertirATexto(Entrada.destino_personal);
            diccionarioEntrada[59] = Herramientas.ConvertirATexto(Entrada.monto);
            diccionarioEntrada[60] = Herramientas.ConvertirATexto(Entrada.tipo_tasa);
            diccionarioEntrada[61] = Herramientas.ConvertirATexto(Entrada.tna);
            diccionarioEntrada[62] = Herramientas.ConvertirATexto(Entrada.modelo_auto);
            diccionarioEntrada[63] = Herramientas.ConvertirATexto(Entrada.auto);
            diccionarioEntrada[64] = Herramientas.ConvertirATexto(Entrada.seg_auto);
            diccionarioEntrada[65] = Herramientas.ConvertirATexto(Entrada.tasacion);
            diccionarioEntrada[66] = Herramientas.ConvertirATexto(Entrada.val_compra_mejora_proyecto);
            diccionarioEntrada[67] = Herramientas.ConvertirATexto(Entrada.val_terreno);
            diccionarioEntrada[68] = Herramientas.ConvertirATexto(Entrada.destino);
            diccionarioEntrada[69] = Herramientas.ConvertirATexto(Entrada.lim_tarj);
            diccionarioEntrada[70] = Herramientas.ConvertirATexto(Entrada.acuerdo_cta_cte);
            diccionarioEntrada[71] = Herramientas.ConvertirATexto(Entrada.cotitu);
            diccionarioEntrada[72] = Herramientas.ConvertirATexto(Entrada.id_cobis_cotitu1);
            diccionarioEntrada[73] = Herramientas.ConvertirATexto(Entrada.documento_cotitu1);
            diccionarioEntrada[74] = Herramientas.ConvertirATexto(Entrada.nombre_cotitu1);
            diccionarioEntrada[75] = Herramientas.ConvertirATexto(Entrada.apellido_cotitu1);
            diccionarioEntrada[76] = Herramientas.ConvertirATexto(Entrada.sexo_cotitu1);
            diccionarioEntrada[77] = Herramientas.ConvertirATexto(Entrada.version_doc_cotitu1);
            diccionarioEntrada[78] = Herramientas.ConvertirATexto(Entrada.tipo_doc_cotitu1);
            diccionarioEntrada[79] = Herramientas.ConvertirATexto(Entrada.fech_laboral_cotitu1);
            diccionarioEntrada[80] = Herramientas.ConvertirATexto(Entrada.neto_fijo_cotitu1);
            diccionarioEntrada[81] = Herramientas.ConvertirATexto(Entrada.neto_var_cotitu1);
            diccionarioEntrada[82] = Herramientas.ConvertirATexto(Entrada.dias_atraso_cotitu1);
            diccionarioEntrada[83] = Herramientas.ConvertirATexto(Entrada.id_cobis_cotitu2);
            diccionarioEntrada[84] = Herramientas.ConvertirATexto(Entrada.documento_cotitu2);
            diccionarioEntrada[85] = Herramientas.ConvertirATexto(Entrada.nombre_cotitu2);
            diccionarioEntrada[86] = Herramientas.ConvertirATexto(Entrada.apellido_cotitu2);
            diccionarioEntrada[87] = Herramientas.ConvertirATexto(Entrada.sexo_cotitu2);
            diccionarioEntrada[88] = Herramientas.ConvertirATexto(Entrada.version_doc_cotitu2);
            diccionarioEntrada[89] = Herramientas.ConvertirATexto(Entrada.tipo_doc_cotitu2);
            diccionarioEntrada[90] = Herramientas.ConvertirATexto(Entrada.fech_laboral_cotitu2);
            diccionarioEntrada[91] = Herramientas.ConvertirATexto(Entrada.neto_fijo_cotitu2);
            diccionarioEntrada[92] = Herramientas.ConvertirATexto(Entrada.neto_var_cotitu2);
            diccionarioEntrada[93] = Herramientas.ConvertirATexto(Entrada.dias_atraso_cotitu2);
            diccionarioEntrada[94] = Herramientas.ConvertirATexto(Entrada.id_cobis_cotitu3);
            diccionarioEntrada[95] = Herramientas.ConvertirATexto(Entrada.documento_cotitu3);
            diccionarioEntrada[96] = Herramientas.ConvertirATexto(Entrada.nombre_cotitu3);
            diccionarioEntrada[97] = Herramientas.ConvertirATexto(Entrada.apellido_cotitu3);
            diccionarioEntrada[98] = Herramientas.ConvertirATexto(Entrada.sexo_cotitu3);
            diccionarioEntrada[99] = Herramientas.ConvertirATexto(Entrada.version_doc_cotitu3);
            diccionarioEntrada[100] = Herramientas.ConvertirATexto(Entrada.tipo_doc_cotitu3);
            diccionarioEntrada[101] = Herramientas.ConvertirATexto(Entrada.fech_laboral_cotitu3);
            diccionarioEntrada[102] = Herramientas.ConvertirATexto(Entrada.neto_fijo_cotitu3);
            diccionarioEntrada[103] = Herramientas.ConvertirATexto(Entrada.neto_var_cotitu3);
            diccionarioEntrada[104] = Herramientas.ConvertirATexto(Entrada.dias_atraso_cotitu3);
            diccionarioEntrada[105] = Herramientas.ConvertirATexto(Entrada.neto_fijo_cotitu123);
            diccionarioEntrada[106] = Herramientas.ConvertirATexto(Entrada.neto_var_cotitu123);
            diccionarioEntrada[107] = Herramientas.ConvertirATexto(Entrada.canal);
            diccionarioEntrada[108] = Herramientas.ConvertirATexto(Entrada.equipo_ventas);
            diccionarioEntrada[109] = Herramientas.ConvertirATexto(Entrada.tipo_fuente);
            diccionarioEntrada[110] = Herramientas.ConvertirATexto(Entrada.fuente);
            diccionarioEntrada[111] = Herramientas.ConvertirATexto(Entrada.segmento);
            diccionarioEntrada[112] = Herramientas.ConvertirATexto(Entrada.subsegmento);
            diccionarioEntrada[113] = Herramientas.ConvertirATexto(Entrada.nombre_plantilla);
            diccionarioEntrada[114] = Herramientas.ConvertirATexto(Entrada.estado);
            diccionarioEntrada[115] = Herramientas.ConvertirATexto(Entrada.atributo1);
            diccionarioEntrada[116] = Herramientas.ConvertirATexto(Entrada.valor1);
            diccionarioEntrada[117] = Herramientas.ConvertirATexto(Entrada.atributo2);
            diccionarioEntrada[118] = Herramientas.ConvertirATexto(Entrada.valor2);
            diccionarioEntrada[119] = Herramientas.ConvertirATexto(Entrada.atributo3);
            diccionarioEntrada[120] = Herramientas.ConvertirATexto(Entrada.valor3);
            diccionarioEntrada[121] = Herramientas.ConvertirATexto(Entrada.atributo4);
            diccionarioEntrada[122] = Herramientas.ConvertirATexto(Entrada.valor4);
            diccionarioEntrada[123] = Herramientas.ConvertirATexto(Entrada.atributo5);
            diccionarioEntrada[124] = Herramientas.ConvertirATexto(Entrada.valor5);
            diccionarioEntrada[125] = Herramientas.ConvertirATexto(Entrada.atributo6);
            diccionarioEntrada[126] = Herramientas.ConvertirATexto(Entrada.valor6);
            diccionarioEntrada[127] = Herramientas.ConvertirATexto(Entrada.atributo7);
            diccionarioEntrada[128] = Herramientas.ConvertirATexto(Entrada.valor7);
            diccionarioEntrada[129] = Herramientas.ConvertirATexto(Entrada.atributo8);
            diccionarioEntrada[130] = Herramientas.ConvertirATexto(Entrada.valor8);
            diccionarioEntrada[131] = Herramientas.ConvertirATexto(Entrada.atributo9);
            diccionarioEntrada[132] = Herramientas.ConvertirATexto(Entrada.valor9);
            diccionarioEntrada[133] = Herramientas.ConvertirATexto(Entrada.atributo10);
            diccionarioEntrada[134] = Herramientas.ConvertirATexto(Entrada.valor10);
            diccionarioEntrada[135] = Herramientas.ConvertirATexto(Entrada.usuario);
            diccionarioEntrada[136] = Herramientas.ConvertirATexto(Entrada.cliente_inversor);
            diccionarioEntrada[137] = Herramientas.ConvertirATexto(Entrada.monto_inversion);
            diccionarioEntrada[138] = Herramientas.ConvertirATexto(Entrada.monto_subsidio);
            diccionarioEntrada[139] = Herramientas.ConvertirATexto(Entrada.opcional_1);
            diccionarioEntrada[140] = Herramientas.ConvertirATexto(Entrada.opcional_2);
            diccionarioEntrada[141] = Herramientas.ConvertirATexto(Entrada.opcional_3);
            diccionarioEntrada[142] = Herramientas.ConvertirATexto(Entrada.opcional_4);
            diccionarioEntrada[143] = Herramientas.ConvertirATexto(Entrada.opcional_5);
            diccionarioEntrada[144] = Herramientas.ConvertirATexto(Entrada.opcional_6);
            diccionarioEntrada[145] = Herramientas.ConvertirATexto(Entrada.opcional_7);
            diccionarioEntrada[146] = Herramientas.ConvertirATexto(Entrada.opcional_8);
            diccionarioEntrada[147] = Herramientas.ConvertirATexto(Entrada.opcional_9);
            diccionarioEntrada[148] = Herramientas.ConvertirATexto(Entrada.opcional_10);
            diccionarioEntrada[149] = Herramientas.ConvertirATexto(Entrada.opcional_11);
            diccionarioEntrada[150] = Herramientas.ConvertirATexto(Entrada.opcional_12);
            diccionarioEntrada[151] = Herramientas.ConvertirATexto(Entrada.opcional_13);
            diccionarioEntrada[152] = Herramientas.ConvertirATexto(Entrada.opcional_14);
            diccionarioEntrada[153] = Herramientas.ConvertirATexto(Entrada.opcional_15);
            diccionarioEntrada[154] = Herramientas.ConvertirATexto(Entrada.EA_TIT_codigo_cliente);
            diccionarioEntrada[155] = Herramientas.ConvertirATexto(Entrada.EA_TIT_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[156] = Herramientas.ConvertirATexto(Entrada.EA_TIT_fecha_cabalope, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[157] = Herramientas.ConvertirATexto(Entrada.EA_TIT_fecha_consulta_reestructuracion, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[158] = Herramientas.ConvertirATexto(Entrada.EA_TIT_motivo_reestructuracion);
            diccionarioEntrada[159] = Herramientas.ConvertirATexto(Entrada.EA_TIT_fecha_consulta_refi_deu, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[160] = Herramientas.ConvertirATexto(Entrada.EA_TIT_motivo_refinanciacion);
            diccionarioEntrada[161] = Herramientas.ConvertirATexto(Entrada.EA_TIT_motivo_deu);
            diccionarioEntrada[162] = Herramientas.ConvertirATexto(Entrada.CND_TIT_cobis);
            diccionarioEntrada[163] = Herramientas.ConvertirATexto(Entrada.CND_TIT_nro_tributario);
            diccionarioEntrada[164] = Herramientas.ConvertirATexto(Entrada.CND_TIT_codigo_campana);
            diccionarioEntrada[165] = Herramientas.ConvertirATexto(Entrada.CND_TIT_vencimiento, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[166] = Herramientas.ConvertirATexto(Entrada.CND_TIT_motivo_rechazo);
            diccionarioEntrada[167] = Herramientas.ConvertirATexto(Entrada.BH_TIT_id_cobis);
            diccionarioEntrada[168] = Herramientas.ConvertirATexto(Entrada.BH_TIT_riesgo_score);
            diccionarioEntrada[169] = Herramientas.ConvertirATexto(Entrada.BH_TIT_riesgo_6);
            diccionarioEntrada[170] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_tc_limite);
            diccionarioEntrada[171] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_tc_exigible);
            diccionarioEntrada[172] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_tc_limite_max);
            diccionarioEntrada[173] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_cc_limite);
            diccionarioEntrada[174] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_cc_promedio3);
            diccionarioEntrada[175] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_sg_exigible);
            diccionarioEntrada[176] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_gp_exigible);
            diccionarioEntrada[177] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_gh_exigible);
            diccionarioEntrada[178] = Herramientas.ConvertirATexto(Entrada.BH_TIT_antiguedad_tc);
            diccionarioEntrada[179] = Herramientas.ConvertirATexto(Entrada.BH_TIT_antiguedad_cc);
            diccionarioEntrada[180] = Herramientas.ConvertirATexto(Entrada.BH_TIT_cob_status);
            diccionarioEntrada[181] = Herramientas.ConvertirATexto(Entrada.BH_TIT_hit);
            diccionarioEntrada[182] = Herramientas.ConvertirATexto(Entrada.BH_TIT_uso_tc_saldo_fin);
            diccionarioEntrada[183] = Herramientas.ConvertirATexto(Entrada.BH_TIT_ip2);
            diccionarioEntrada[184] = Herramientas.ConvertirATexto(Entrada.BH_TIT_consultar_veraz);
            diccionarioEntrada[185] = Herramientas.ConvertirATexto(Entrada.CHEQ_TIT_rech_lev_sf_u24m_cant);
            diccionarioEntrada[186] = Herramientas.ConvertirATexto(Entrada.CHEQ_TIT_rech_nolev_sf_u24m_monto);
            diccionarioEntrada[187] = Herramientas.ConvertirATexto(Entrada.CHEQ_TIT_rech_lev_sf_u24m_monto);
            diccionarioEntrada[188] = Herramientas.ConvertirATexto(Entrada.SC_TIT_exclusion);
            diccionarioEntrada[189] = Herramientas.ConvertirATexto(Entrada.SC_TIT_score_comp);
            diccionarioEntrada[190] = Herramientas.ConvertirATexto(Entrada.SC_TIT_categoria);
            diccionarioEntrada[191] = Herramientas.ConvertirATexto(Entrada.POL_TIT_flag_acuerdo_cc);
            diccionarioEntrada[192] = Herramientas.ConvertirATexto(Entrada.POL_TIT_flag_producto_crediticio_activo);
            diccionarioEntrada[193] = Herramientas.ConvertirATexto(Entrada.POL_TIT_antiguedad_tc_macro);
            diccionarioEntrada[194] = Herramientas.ConvertirATexto(Entrada.POL_TIT_antiguedad_cc_macro);
            diccionarioEntrada[195] = Herramientas.ConvertirATexto(Entrada.POL_TIT_cantidad_ocurrencia_6);
            diccionarioEntrada[196] = Herramientas.ConvertirATexto(Entrada.EX_TIT_fin_excepcion_vz, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[197] = Herramientas.ConvertirATexto(Entrada.VA_TIT_cuit);
            diccionarioEntrada[198] = Herramientas.ConvertirATexto(Entrada.VA_TIT_nombre_pe);
            diccionarioEntrada[199] = Herramientas.ConvertirATexto(Entrada.VA_TIT_apellido_pe);
            diccionarioEntrada[200] = Herramientas.ConvertirATexto(Entrada.VA_TIT_sexo_pe);
            diccionarioEntrada[201] = Herramientas.ConvertirATexto(Entrada.VA_TIT_fecha_nacimiento_pe, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[202] = Herramientas.ConvertirATexto(Entrada.VA_TIT_provincia_pe);
            diccionarioEntrada[203] = Herramientas.ConvertirATexto(Entrada.VA_TIT_codigo_postal_pe);
            diccionarioEntrada[204] = Herramientas.ConvertirATexto(Entrada.VA_TIT_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[205] = Herramientas.ConvertirATexto(Entrada.VA_TIT_periodo_score);
            diccionarioEntrada[206] = Herramientas.ConvertirATexto(Entrada.VA_TIT_empleador);
            diccionarioEntrada[207] = Herramientas.ConvertirATexto(Entrada.VA_TIT_flag_crediticio);
            diccionarioEntrada[208] = Herramientas.ConvertirATexto(Entrada.VA_TIT_sum_score);
            diccionarioEntrada[209] = Herramientas.ConvertirATexto(Entrada.VA_TIT_razon_social_afip);
            diccionarioEntrada[210] = Herramientas.ConvertirATexto(Entrada.VA_TIT_impuesto_ganancias);
            diccionarioEntrada[211] = Herramientas.ConvertirATexto(Entrada.VA_TIT_impuesto_iva);
            diccionarioEntrada[212] = Herramientas.ConvertirATexto(Entrada.VA_TIT_integrante_sociedad);
            diccionarioEntrada[213] = Herramientas.ConvertirATexto(Entrada.VA_TIT_es_empleador_afip);
            diccionarioEntrada[214] = Herramientas.ConvertirATexto(Entrada.VA_TIT_cat_monotributo);
            diccionarioEntrada[215] = Herramientas.ConvertirATexto(Entrada.VA_TIT_actividad_monotributo);
            diccionarioEntrada[216] = Herramientas.ConvertirATexto(Entrada.VA_TIT_actividad_monotributo_descripcion);
            diccionarioEntrada[217] = Herramientas.ConvertirATexto(Entrada.VA_TIT_fact_monotributo);
            diccionarioEntrada[218] = Herramientas.ConvertirATexto(Entrada.VA_TIT_ingreso_servicios);
            diccionarioEntrada[219] = Herramientas.ConvertirATexto(Entrada.VA_TIT_ingreso_ventas);
            diccionarioEntrada[220] = Herramientas.ConvertirATexto(Entrada.VA_TIT_ingreso_monotributo);
            diccionarioEntrada[221] = Herramientas.ConvertirATexto(Entrada.VA_TIT_es_empleado);
            diccionarioEntrada[222] = Herramientas.ConvertirATexto(Entrada.VA_TIT_es_autonomo);
            diccionarioEntrada[223] = Herramientas.ConvertirATexto(Entrada.VA_TIT_es_monotributista);
            diccionarioEntrada[224] = Herramientas.ConvertirATexto(Entrada.VA_TIT_es_jubilado);
            diccionarioEntrada[225] = Herramientas.ConvertirATexto(Entrada.VA_TIT_ingreso_transaccionalidad);
            diccionarioEntrada[226] = Herramientas.ConvertirATexto(Entrada.VA_TIT_opcional_1);
            diccionarioEntrada[227] = Herramientas.ConvertirATexto(Entrada.VA_TIT_1);
            diccionarioEntrada[228] = Herramientas.ConvertirATexto(Entrada.VA_TIT_2);
            diccionarioEntrada[229] = Herramientas.ConvertirATexto(Entrada.VA_TIT_3);
            diccionarioEntrada[230] = Herramientas.ConvertirATexto(Entrada.VA_TIT_4);
            diccionarioEntrada[231] = Herramientas.ConvertirATexto(Entrada.VA_TIT_5);
            diccionarioEntrada[232] = Herramientas.ConvertirATexto(Entrada.VA_TIT_6);
            diccionarioEntrada[233] = Herramientas.ConvertirATexto(Entrada.VA_TIT_7);
            diccionarioEntrada[234] = Herramientas.ConvertirATexto(Entrada.VA_TIT_8);
            diccionarioEntrada[235] = Herramientas.ConvertirATexto(Entrada.VA_TIT_9);
            diccionarioEntrada[236] = Herramientas.ConvertirATexto(Entrada.VA_TIT_10);
            diccionarioEntrada[237] = Herramientas.ConvertirATexto(Entrada.VA_TIT_11);
            diccionarioEntrada[238] = Herramientas.ConvertirATexto(Entrada.VA_TIT_12);
            diccionarioEntrada[239] = Herramientas.ConvertirATexto(Entrada.VA_TIT_13);
            diccionarioEntrada[240] = Herramientas.ConvertirATexto(Entrada.VA_TIT_14);
            diccionarioEntrada[241] = Herramientas.ConvertirATexto(Entrada.VA_TIT_15);
            diccionarioEntrada[242] = Herramientas.ConvertirATexto(Entrada.VA_TIT_16);
            diccionarioEntrada[243] = Herramientas.ConvertirATexto(Entrada.VA_TIT_17);
            diccionarioEntrada[244] = Herramientas.ConvertirATexto(Entrada.VA_TIT_18);
            diccionarioEntrada[245] = Herramientas.ConvertirATexto(Entrada.VA_TIT_19);
            diccionarioEntrada[246] = Herramientas.ConvertirATexto(Entrada.VA_TIT_20);
            diccionarioEntrada[247] = Herramientas.ConvertirATexto(Entrada.VA_TIT_21);
            diccionarioEntrada[248] = Herramientas.ConvertirATexto(Entrada.VA_TIT_22);
            diccionarioEntrada[249] = Herramientas.ConvertirATexto(Entrada.VA_TIT_23);
            diccionarioEntrada[250] = Herramientas.ConvertirATexto(Entrada.VA_TIT_24);
            diccionarioEntrada[251] = Herramientas.ConvertirATexto(Entrada.VA_TIT_25);
            diccionarioEntrada[252] = Herramientas.ConvertirATexto(Entrada.VA_TIT_26);
            diccionarioEntrada[253] = Herramientas.ConvertirATexto(Entrada.VA_TIT_27);
            diccionarioEntrada[254] = Herramientas.ConvertirATexto(Entrada.VA_TIT_28);
            diccionarioEntrada[255] = Herramientas.ConvertirATexto(Entrada.VA_TIT_29);
            diccionarioEntrada[256] = Herramientas.ConvertirATexto(Entrada.VA_TIT_30);
            diccionarioEntrada[257] = Herramientas.ConvertirATexto(Entrada.VA_TIT_31);
            diccionarioEntrada[258] = Herramientas.ConvertirATexto(Entrada.VA_TIT_32);
            diccionarioEntrada[259] = Herramientas.ConvertirATexto(Entrada.VA_TIT_33);
            diccionarioEntrada[260] = Herramientas.ConvertirATexto(Entrada.VA_TIT_34);
            diccionarioEntrada[261] = Herramientas.ConvertirATexto(Entrada.VA_TIT_35);
            diccionarioEntrada[262] = Herramientas.ConvertirATexto(Entrada.VA_TIT_36);
            diccionarioEntrada[263] = Herramientas.ConvertirATexto(Entrada.VA_TIT_37);
            diccionarioEntrada[264] = Herramientas.ConvertirATexto(Entrada.VA_TIT_38);
            diccionarioEntrada[265] = Herramientas.ConvertirATexto(Entrada.VA_TIT_39);
            diccionarioEntrada[266] = Herramientas.ConvertirATexto(Entrada.VA_TIT_40);
            diccionarioEntrada[267] = Herramientas.ConvertirATexto(Entrada.VA_TIT_41);
            diccionarioEntrada[268] = Herramientas.ConvertirATexto(Entrada.VA_TIT_42);
            diccionarioEntrada[269] = Herramientas.ConvertirATexto(Entrada.VA_TIT_43);
            diccionarioEntrada[270] = Herramientas.ConvertirATexto(Entrada.VA_TIT_44);
            diccionarioEntrada[271] = Herramientas.ConvertirATexto(Entrada.VA_TIT_45);
            diccionarioEntrada[272] = Herramientas.ConvertirATexto(Entrada.VA_TIT_46);
            diccionarioEntrada[273] = Herramientas.ConvertirATexto(Entrada.VA_TIT_47);
            diccionarioEntrada[274] = Herramientas.ConvertirATexto(Entrada.VA_TIT_48);
            diccionarioEntrada[275] = Herramientas.ConvertirATexto(Entrada.VA_TIT_49);
            diccionarioEntrada[276] = Herramientas.ConvertirATexto(Entrada.VA_TIT_50);
            diccionarioEntrada[277] = Herramientas.ConvertirATexto(Entrada.TIND_TIT_ingreso_calculado_final);
            diccionarioEntrada[278] = Herramientas.ConvertirATexto(Entrada.TIND_TIT_tipo_ingreso);
            diccionarioEntrada[279] = Herramientas.ConvertirATexto(Entrada.EA_COT1_codigo_cliente);
            diccionarioEntrada[280] = Herramientas.ConvertirATexto(Entrada.EA_COT1_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[281] = Herramientas.ConvertirATexto(Entrada.EA_COT1_fecha_cabalope, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[282] = Herramientas.ConvertirATexto(Entrada.EA_COT1_fecha_consulta_reestructuracion, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[283] = Herramientas.ConvertirATexto(Entrada.EA_COT1_motivo_reestructuracion);
            diccionarioEntrada[284] = Herramientas.ConvertirATexto(Entrada.EA_COT1_fecha_consulta_refi_deu, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[285] = Herramientas.ConvertirATexto(Entrada.EA_COT1_motivo_refinanciacion);
            diccionarioEntrada[286] = Herramientas.ConvertirATexto(Entrada.EA_COT1_motivo_deu);
            diccionarioEntrada[287] = Herramientas.ConvertirATexto(Entrada.CND_COT1_cobis);
            diccionarioEntrada[288] = Herramientas.ConvertirATexto(Entrada.CND_COT1_nro_tributario);
            diccionarioEntrada[289] = Herramientas.ConvertirATexto(Entrada.CND_COT1_codigo_campana);
            diccionarioEntrada[290] = Herramientas.ConvertirATexto(Entrada.CND_COT1_vencimiento, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[291] = Herramientas.ConvertirATexto(Entrada.CND_COT1_motivo_rechazo);
            diccionarioEntrada[292] = Herramientas.ConvertirATexto(Entrada.BH_COT1_id_cobis);
            diccionarioEntrada[293] = Herramientas.ConvertirATexto(Entrada.BH_COT1_riesgo_score);
            diccionarioEntrada[294] = Herramientas.ConvertirATexto(Entrada.BH_COT1_riesgo_6);
            diccionarioEntrada[295] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_tc_limite);
            diccionarioEntrada[296] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_tc_exigible);
            diccionarioEntrada[297] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_tc_limite_max);
            diccionarioEntrada[298] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_cc_limite);
            diccionarioEntrada[299] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_cc_promedio3);
            diccionarioEntrada[300] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_sg_exigible);
            diccionarioEntrada[301] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_gp_exigible);
            diccionarioEntrada[302] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_gh_exigible);
            diccionarioEntrada[303] = Herramientas.ConvertirATexto(Entrada.BH_COT1_antiguedad_tc);
            diccionarioEntrada[304] = Herramientas.ConvertirATexto(Entrada.BH_COT1_antiguedad_cc);
            diccionarioEntrada[305] = Herramientas.ConvertirATexto(Entrada.BH_COT1_cob_status);
            diccionarioEntrada[306] = Herramientas.ConvertirATexto(Entrada.BH_COT1_hit);
            diccionarioEntrada[307] = Herramientas.ConvertirATexto(Entrada.BH_COT1_uso_tc_saldo_fin);
            diccionarioEntrada[308] = Herramientas.ConvertirATexto(Entrada.BH_COT1_ip2);
            diccionarioEntrada[309] = Herramientas.ConvertirATexto(Entrada.BH_COT1_consultar_veraz);
            diccionarioEntrada[310] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT1_rech_lev_sf_u24m_cant);
            diccionarioEntrada[311] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT1_rech_nolev_sf_u24m_monto);
            diccionarioEntrada[312] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT1_rech_lev_sf_u24m_monto);
            diccionarioEntrada[313] = Herramientas.ConvertirATexto(Entrada.SC_COT1_exclusion);
            diccionarioEntrada[314] = Herramientas.ConvertirATexto(Entrada.SC_COT1_score_comp);
            diccionarioEntrada[315] = Herramientas.ConvertirATexto(Entrada.SC_COT1_categoria);
            diccionarioEntrada[316] = Herramientas.ConvertirATexto(Entrada.POL_COT1_flag_acuerdo_cc);
            diccionarioEntrada[317] = Herramientas.ConvertirATexto(Entrada.POL_COT1_flag_producto_crediticio_activo);
            diccionarioEntrada[318] = Herramientas.ConvertirATexto(Entrada.POL_COT1_antiguedad_tc_macro);
            diccionarioEntrada[319] = Herramientas.ConvertirATexto(Entrada.POL_COT1_antiguedad_cc_macro);
            diccionarioEntrada[320] = Herramientas.ConvertirATexto(Entrada.POL_COT1_cantidad_ocurrencia_6);
            diccionarioEntrada[321] = Herramientas.ConvertirATexto(Entrada.EX_COT1_fin_excepcion_vz, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[322] = Herramientas.ConvertirATexto(Entrada.VA_COT1_cuit);
            diccionarioEntrada[323] = Herramientas.ConvertirATexto(Entrada.VA_COT1_nombre_pe);
            diccionarioEntrada[324] = Herramientas.ConvertirATexto(Entrada.VA_COT1_apellido_pe);
            diccionarioEntrada[325] = Herramientas.ConvertirATexto(Entrada.VA_COT1_sexo_pe);
            diccionarioEntrada[326] = Herramientas.ConvertirATexto(Entrada.VA_COT1_fecha_nacimiento_pe, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[327] = Herramientas.ConvertirATexto(Entrada.VA_COT1_provincia_pe);
            diccionarioEntrada[328] = Herramientas.ConvertirATexto(Entrada.VA_COT1_codigo_postal_pe);
            diccionarioEntrada[329] = Herramientas.ConvertirATexto(Entrada.VA_COT1_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[330] = Herramientas.ConvertirATexto(Entrada.VA_COT1_periodo_score);
            diccionarioEntrada[331] = Herramientas.ConvertirATexto(Entrada.VA_COT1_empleador);
            diccionarioEntrada[332] = Herramientas.ConvertirATexto(Entrada.VA_COT1_flag_crediticio);
            diccionarioEntrada[333] = Herramientas.ConvertirATexto(Entrada.VA_COT1_sum_score);
            diccionarioEntrada[334] = Herramientas.ConvertirATexto(Entrada.VA_COT1_razon_social_afip);
            diccionarioEntrada[335] = Herramientas.ConvertirATexto(Entrada.VA_COT1_impuesto_ganancias);
            diccionarioEntrada[336] = Herramientas.ConvertirATexto(Entrada.VA_COT1_impuesto_iva);
            diccionarioEntrada[337] = Herramientas.ConvertirATexto(Entrada.VA_COT1_integrante_sociedad);
            diccionarioEntrada[338] = Herramientas.ConvertirATexto(Entrada.VA_COT1_es_empleador_afip);
            diccionarioEntrada[339] = Herramientas.ConvertirATexto(Entrada.VA_COT1_cat_monotributo);
            diccionarioEntrada[340] = Herramientas.ConvertirATexto(Entrada.VA_COT1_actividad_monotributo);
            diccionarioEntrada[341] = Herramientas.ConvertirATexto(Entrada.VA_COT1_actividad_monotributo_descripcion);
            diccionarioEntrada[342] = Herramientas.ConvertirATexto(Entrada.VA_COT1_fact_monotributo);
            diccionarioEntrada[343] = Herramientas.ConvertirATexto(Entrada.VA_COT1_ingreso_servicios);
            diccionarioEntrada[344] = Herramientas.ConvertirATexto(Entrada.VA_COT1_ingreso_ventas);
            diccionarioEntrada[345] = Herramientas.ConvertirATexto(Entrada.VA_COT1_ingreso_monotributo);
            diccionarioEntrada[346] = Herramientas.ConvertirATexto(Entrada.VA_COT1_es_empleado);
            diccionarioEntrada[347] = Herramientas.ConvertirATexto(Entrada.VA_COT1_es_autonomo);
            diccionarioEntrada[348] = Herramientas.ConvertirATexto(Entrada.VA_COT1_es_monotributista);
            diccionarioEntrada[349] = Herramientas.ConvertirATexto(Entrada.VA_COT1_es_jubilado);
            diccionarioEntrada[350] = Herramientas.ConvertirATexto(Entrada.VA_COT1_ingreso_transaccionalidad);
            diccionarioEntrada[351] = Herramientas.ConvertirATexto(Entrada.VA_COT1_opcional_1);
            diccionarioEntrada[352] = Herramientas.ConvertirATexto(Entrada.VA_COT1_1);
            diccionarioEntrada[353] = Herramientas.ConvertirATexto(Entrada.VA_COT1_2);
            diccionarioEntrada[354] = Herramientas.ConvertirATexto(Entrada.VA_COT1_3);
            diccionarioEntrada[355] = Herramientas.ConvertirATexto(Entrada.VA_COT1_4);
            diccionarioEntrada[356] = Herramientas.ConvertirATexto(Entrada.VA_COT1_5);
            diccionarioEntrada[357] = Herramientas.ConvertirATexto(Entrada.VA_COT1_6);
            diccionarioEntrada[358] = Herramientas.ConvertirATexto(Entrada.VA_COT1_7);
            diccionarioEntrada[359] = Herramientas.ConvertirATexto(Entrada.VA_COT1_8);
            diccionarioEntrada[360] = Herramientas.ConvertirATexto(Entrada.VA_COT1_9);
            diccionarioEntrada[361] = Herramientas.ConvertirATexto(Entrada.VA_COT1_10);
            diccionarioEntrada[362] = Herramientas.ConvertirATexto(Entrada.VA_COT1_11);
            diccionarioEntrada[363] = Herramientas.ConvertirATexto(Entrada.VA_COT1_12);
            diccionarioEntrada[364] = Herramientas.ConvertirATexto(Entrada.VA_COT1_13);
            diccionarioEntrada[365] = Herramientas.ConvertirATexto(Entrada.VA_COT1_14);
            diccionarioEntrada[366] = Herramientas.ConvertirATexto(Entrada.VA_COT1_15);
            diccionarioEntrada[367] = Herramientas.ConvertirATexto(Entrada.VA_COT1_16);
            diccionarioEntrada[368] = Herramientas.ConvertirATexto(Entrada.VA_COT1_17);
            diccionarioEntrada[369] = Herramientas.ConvertirATexto(Entrada.VA_COT1_18);
            diccionarioEntrada[370] = Herramientas.ConvertirATexto(Entrada.VA_COT1_19);
            diccionarioEntrada[371] = Herramientas.ConvertirATexto(Entrada.VA_COT1_20);
            diccionarioEntrada[372] = Herramientas.ConvertirATexto(Entrada.VA_COT1_21);
            diccionarioEntrada[373] = Herramientas.ConvertirATexto(Entrada.VA_COT1_22);
            diccionarioEntrada[374] = Herramientas.ConvertirATexto(Entrada.VA_COT1_23);
            diccionarioEntrada[375] = Herramientas.ConvertirATexto(Entrada.VA_COT1_24);
            diccionarioEntrada[376] = Herramientas.ConvertirATexto(Entrada.VA_COT1_25);
            diccionarioEntrada[377] = Herramientas.ConvertirATexto(Entrada.VA_COT1_26);
            diccionarioEntrada[378] = Herramientas.ConvertirATexto(Entrada.VA_COT1_27);
            diccionarioEntrada[379] = Herramientas.ConvertirATexto(Entrada.VA_COT1_28);
            diccionarioEntrada[380] = Herramientas.ConvertirATexto(Entrada.VA_COT1_29);
            diccionarioEntrada[381] = Herramientas.ConvertirATexto(Entrada.VA_COT1_30);
            diccionarioEntrada[382] = Herramientas.ConvertirATexto(Entrada.VA_COT1_31);
            diccionarioEntrada[383] = Herramientas.ConvertirATexto(Entrada.VA_COT1_32);
            diccionarioEntrada[384] = Herramientas.ConvertirATexto(Entrada.VA_COT1_33);
            diccionarioEntrada[385] = Herramientas.ConvertirATexto(Entrada.VA_COT1_34);
            diccionarioEntrada[386] = Herramientas.ConvertirATexto(Entrada.VA_COT1_35);
            diccionarioEntrada[387] = Herramientas.ConvertirATexto(Entrada.VA_COT1_36);
            diccionarioEntrada[388] = Herramientas.ConvertirATexto(Entrada.VA_COT1_37);
            diccionarioEntrada[389] = Herramientas.ConvertirATexto(Entrada.VA_COT1_38);
            diccionarioEntrada[390] = Herramientas.ConvertirATexto(Entrada.VA_COT1_39);
            diccionarioEntrada[391] = Herramientas.ConvertirATexto(Entrada.VA_COT1_40);
            diccionarioEntrada[392] = Herramientas.ConvertirATexto(Entrada.VA_COT1_41);
            diccionarioEntrada[393] = Herramientas.ConvertirATexto(Entrada.VA_COT1_42);
            diccionarioEntrada[394] = Herramientas.ConvertirATexto(Entrada.VA_COT1_43);
            diccionarioEntrada[395] = Herramientas.ConvertirATexto(Entrada.VA_COT1_44);
            diccionarioEntrada[396] = Herramientas.ConvertirATexto(Entrada.VA_COT1_45);
            diccionarioEntrada[397] = Herramientas.ConvertirATexto(Entrada.VA_COT1_46);
            diccionarioEntrada[398] = Herramientas.ConvertirATexto(Entrada.VA_COT1_47);
            diccionarioEntrada[399] = Herramientas.ConvertirATexto(Entrada.VA_COT1_48);
            diccionarioEntrada[400] = Herramientas.ConvertirATexto(Entrada.VA_COT1_49);
            diccionarioEntrada[401] = Herramientas.ConvertirATexto(Entrada.VA_COT1_50);
            diccionarioEntrada[402] = Herramientas.ConvertirATexto(Entrada.TIND_COT1_ingreso_calculado_final);
            diccionarioEntrada[403] = Herramientas.ConvertirATexto(Entrada.TIND_COT1_tipo_ingreso);
            diccionarioEntrada[404] = Herramientas.ConvertirATexto(Entrada.EA_COT2_codigo_cliente);
            diccionarioEntrada[405] = Herramientas.ConvertirATexto(Entrada.EA_COT2_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[406] = Herramientas.ConvertirATexto(Entrada.EA_COT2_fecha_cabalope, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[407] = Herramientas.ConvertirATexto(Entrada.EA_COT2_fecha_consulta_reestructuracion, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[408] = Herramientas.ConvertirATexto(Entrada.EA_COT2_motivo_reestructuracion);
            diccionarioEntrada[409] = Herramientas.ConvertirATexto(Entrada.EA_COT2_fecha_consulta_refi_deu, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[410] = Herramientas.ConvertirATexto(Entrada.EA_COT2_motivo_refinanciacion);
            diccionarioEntrada[411] = Herramientas.ConvertirATexto(Entrada.EA_COT2_motivo_deu);
            diccionarioEntrada[412] = Herramientas.ConvertirATexto(Entrada.CND_COT2_cobis);
            diccionarioEntrada[413] = Herramientas.ConvertirATexto(Entrada.CND_COT2_nro_tributario);
            diccionarioEntrada[414] = Herramientas.ConvertirATexto(Entrada.CND_COT2_codigo_campana);
            diccionarioEntrada[415] = Herramientas.ConvertirATexto(Entrada.CND_COT2_vencimiento, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[416] = Herramientas.ConvertirATexto(Entrada.CND_COT2_motivo_rechazo);
            diccionarioEntrada[417] = Herramientas.ConvertirATexto(Entrada.BH_COT2_id_cobis);
            diccionarioEntrada[418] = Herramientas.ConvertirATexto(Entrada.BH_COT2_riesgo_score);
            diccionarioEntrada[419] = Herramientas.ConvertirATexto(Entrada.BH_COT2_riesgo_6);
            diccionarioEntrada[420] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_tc_limite);
            diccionarioEntrada[421] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_tc_exigible);
            diccionarioEntrada[422] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_tc_limite_max);
            diccionarioEntrada[423] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_cc_limite);
            diccionarioEntrada[424] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_cc_promedio3);
            diccionarioEntrada[425] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_sg_exigible);
            diccionarioEntrada[426] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_gp_exigible);
            diccionarioEntrada[427] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_gh_exigible);
            diccionarioEntrada[428] = Herramientas.ConvertirATexto(Entrada.BH_COT2_antiguedad_tc);
            diccionarioEntrada[429] = Herramientas.ConvertirATexto(Entrada.BH_COT2_antiguedad_cc);
            diccionarioEntrada[430] = Herramientas.ConvertirATexto(Entrada.BH_COT2_cob_status);
            diccionarioEntrada[431] = Herramientas.ConvertirATexto(Entrada.BH_COT2_hit);
            diccionarioEntrada[432] = Herramientas.ConvertirATexto(Entrada.BH_COT2_uso_tc_saldo_fin);
            diccionarioEntrada[433] = Herramientas.ConvertirATexto(Entrada.BH_COT2_ip2);
            diccionarioEntrada[434] = Herramientas.ConvertirATexto(Entrada.BH_COT2_consultar_veraz);
            diccionarioEntrada[435] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT2_rech_lev_sf_u24m_cant);
            diccionarioEntrada[436] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT2_rech_nolev_sf_u24m_monto);
            diccionarioEntrada[437] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT2_rech_lev_sf_u24m_monto);
            diccionarioEntrada[438] = Herramientas.ConvertirATexto(Entrada.SC_COT2_exclusion);
            diccionarioEntrada[439] = Herramientas.ConvertirATexto(Entrada.SC_COT2_score_comp);
            diccionarioEntrada[440] = Herramientas.ConvertirATexto(Entrada.SC_COT2_categoria);
            diccionarioEntrada[441] = Herramientas.ConvertirATexto(Entrada.POL_COT2_flag_acuerdo_cc);
            diccionarioEntrada[442] = Herramientas.ConvertirATexto(Entrada.POL_COT2_flag_producto_crediticio_activo);
            diccionarioEntrada[443] = Herramientas.ConvertirATexto(Entrada.POL_COT2_antiguedad_tc_macro);
            diccionarioEntrada[444] = Herramientas.ConvertirATexto(Entrada.POL_COT2_antiguedad_cc_macro);
            diccionarioEntrada[445] = Herramientas.ConvertirATexto(Entrada.POL_COT2_cantidad_ocurrencia_6);
            diccionarioEntrada[446] = Herramientas.ConvertirATexto(Entrada.EX_COT2_fin_excepcion_vz, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[447] = Herramientas.ConvertirATexto(Entrada.VA_COT2_cuit);
            diccionarioEntrada[448] = Herramientas.ConvertirATexto(Entrada.VA_COT2_nombre_pe);
            diccionarioEntrada[449] = Herramientas.ConvertirATexto(Entrada.VA_COT2_apellido_pe);
            diccionarioEntrada[450] = Herramientas.ConvertirATexto(Entrada.VA_COT2_sexo_pe);
            diccionarioEntrada[451] = Herramientas.ConvertirATexto(Entrada.VA_COT2_fecha_nacimiento_pe, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[452] = Herramientas.ConvertirATexto(Entrada.VA_COT2_provincia_pe);
            diccionarioEntrada[453] = Herramientas.ConvertirATexto(Entrada.VA_COT2_codigo_postal_pe);
            diccionarioEntrada[454] = Herramientas.ConvertirATexto(Entrada.VA_COT2_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[455] = Herramientas.ConvertirATexto(Entrada.VA_COT2_periodo_score);
            diccionarioEntrada[456] = Herramientas.ConvertirATexto(Entrada.VA_COT2_empleador);
            diccionarioEntrada[457] = Herramientas.ConvertirATexto(Entrada.VA_COT2_flag_crediticio);
            diccionarioEntrada[458] = Herramientas.ConvertirATexto(Entrada.VA_COT2_sum_score);
            diccionarioEntrada[459] = Herramientas.ConvertirATexto(Entrada.VA_COT2_razon_social_afip);
            diccionarioEntrada[460] = Herramientas.ConvertirATexto(Entrada.VA_COT2_impuesto_ganancias);
            diccionarioEntrada[461] = Herramientas.ConvertirATexto(Entrada.VA_COT2_impuesto_iva);
            diccionarioEntrada[462] = Herramientas.ConvertirATexto(Entrada.VA_COT2_integrante_sociedad);
            diccionarioEntrada[463] = Herramientas.ConvertirATexto(Entrada.VA_COT2_es_empleador_afip);
            diccionarioEntrada[464] = Herramientas.ConvertirATexto(Entrada.VA_COT2_cat_monotributo);
            diccionarioEntrada[465] = Herramientas.ConvertirATexto(Entrada.VA_COT2_actividad_monotributo);
            diccionarioEntrada[466] = Herramientas.ConvertirATexto(Entrada.VA_COT2_actividad_monotributo_descripcion);
            diccionarioEntrada[467] = Herramientas.ConvertirATexto(Entrada.VA_COT2_fact_monotributo);
            diccionarioEntrada[468] = Herramientas.ConvertirATexto(Entrada.VA_COT2_ingreso_servicios);
            diccionarioEntrada[469] = Herramientas.ConvertirATexto(Entrada.VA_COT2_ingreso_ventas);
            diccionarioEntrada[470] = Herramientas.ConvertirATexto(Entrada.VA_COT2_ingreso_monotributo);
            diccionarioEntrada[471] = Herramientas.ConvertirATexto(Entrada.VA_COT2_es_empleado);
            diccionarioEntrada[472] = Herramientas.ConvertirATexto(Entrada.VA_COT2_es_autonomo);
            diccionarioEntrada[473] = Herramientas.ConvertirATexto(Entrada.VA_COT2_es_monotributista);
            diccionarioEntrada[474] = Herramientas.ConvertirATexto(Entrada.VA_COT2_es_jubilado);
            diccionarioEntrada[475] = Herramientas.ConvertirATexto(Entrada.VA_COT2_ingreso_transaccionalidad);
            diccionarioEntrada[476] = Herramientas.ConvertirATexto(Entrada.VA_COT2_opcional_1);
            diccionarioEntrada[477] = Herramientas.ConvertirATexto(Entrada.VA_COT2_1);
            diccionarioEntrada[478] = Herramientas.ConvertirATexto(Entrada.VA_COT2_2);
            diccionarioEntrada[479] = Herramientas.ConvertirATexto(Entrada.VA_COT2_3);
            diccionarioEntrada[480] = Herramientas.ConvertirATexto(Entrada.VA_COT2_4);
            diccionarioEntrada[481] = Herramientas.ConvertirATexto(Entrada.VA_COT2_5);
            diccionarioEntrada[482] = Herramientas.ConvertirATexto(Entrada.VA_COT2_6);
            diccionarioEntrada[483] = Herramientas.ConvertirATexto(Entrada.VA_COT2_7);
            diccionarioEntrada[484] = Herramientas.ConvertirATexto(Entrada.VA_COT2_8);
            diccionarioEntrada[485] = Herramientas.ConvertirATexto(Entrada.VA_COT2_9);
            diccionarioEntrada[486] = Herramientas.ConvertirATexto(Entrada.VA_COT2_10);
            diccionarioEntrada[487] = Herramientas.ConvertirATexto(Entrada.VA_COT2_11);
            diccionarioEntrada[488] = Herramientas.ConvertirATexto(Entrada.VA_COT2_12);
            diccionarioEntrada[489] = Herramientas.ConvertirATexto(Entrada.VA_COT2_13);
            diccionarioEntrada[490] = Herramientas.ConvertirATexto(Entrada.VA_COT2_14);
            diccionarioEntrada[491] = Herramientas.ConvertirATexto(Entrada.VA_COT2_15);
            diccionarioEntrada[492] = Herramientas.ConvertirATexto(Entrada.VA_COT2_16);
            diccionarioEntrada[493] = Herramientas.ConvertirATexto(Entrada.VA_COT2_17);
            diccionarioEntrada[494] = Herramientas.ConvertirATexto(Entrada.VA_COT2_18);
            diccionarioEntrada[495] = Herramientas.ConvertirATexto(Entrada.VA_COT2_19);
            diccionarioEntrada[496] = Herramientas.ConvertirATexto(Entrada.VA_COT2_20);
            diccionarioEntrada[497] = Herramientas.ConvertirATexto(Entrada.VA_COT2_21);
            diccionarioEntrada[498] = Herramientas.ConvertirATexto(Entrada.VA_COT2_22);
            diccionarioEntrada[499] = Herramientas.ConvertirATexto(Entrada.VA_COT2_23);
            diccionarioEntrada[500] = Herramientas.ConvertirATexto(Entrada.VA_COT2_24);
            diccionarioEntrada[501] = Herramientas.ConvertirATexto(Entrada.VA_COT2_25);
            diccionarioEntrada[502] = Herramientas.ConvertirATexto(Entrada.VA_COT2_26);
            diccionarioEntrada[503] = Herramientas.ConvertirATexto(Entrada.VA_COT2_27);
            diccionarioEntrada[504] = Herramientas.ConvertirATexto(Entrada.VA_COT2_28);
            diccionarioEntrada[505] = Herramientas.ConvertirATexto(Entrada.VA_COT2_29);
            diccionarioEntrada[506] = Herramientas.ConvertirATexto(Entrada.VA_COT2_30);
            diccionarioEntrada[507] = Herramientas.ConvertirATexto(Entrada.VA_COT2_31);
            diccionarioEntrada[508] = Herramientas.ConvertirATexto(Entrada.VA_COT2_32);
            diccionarioEntrada[509] = Herramientas.ConvertirATexto(Entrada.VA_COT2_33);
            diccionarioEntrada[510] = Herramientas.ConvertirATexto(Entrada.VA_COT2_34);
            diccionarioEntrada[511] = Herramientas.ConvertirATexto(Entrada.VA_COT2_35);
            diccionarioEntrada[512] = Herramientas.ConvertirATexto(Entrada.VA_COT2_36);
            diccionarioEntrada[513] = Herramientas.ConvertirATexto(Entrada.VA_COT2_37);
            diccionarioEntrada[514] = Herramientas.ConvertirATexto(Entrada.VA_COT2_38);
            diccionarioEntrada[515] = Herramientas.ConvertirATexto(Entrada.VA_COT2_39);
            diccionarioEntrada[516] = Herramientas.ConvertirATexto(Entrada.VA_COT2_40);
            diccionarioEntrada[517] = Herramientas.ConvertirATexto(Entrada.VA_COT2_41);
            diccionarioEntrada[518] = Herramientas.ConvertirATexto(Entrada.VA_COT2_42);
            diccionarioEntrada[519] = Herramientas.ConvertirATexto(Entrada.VA_COT2_43);
            diccionarioEntrada[520] = Herramientas.ConvertirATexto(Entrada.VA_COT2_44);
            diccionarioEntrada[521] = Herramientas.ConvertirATexto(Entrada.VA_COT2_45);
            diccionarioEntrada[522] = Herramientas.ConvertirATexto(Entrada.VA_COT2_46);
            diccionarioEntrada[523] = Herramientas.ConvertirATexto(Entrada.VA_COT2_47);
            diccionarioEntrada[524] = Herramientas.ConvertirATexto(Entrada.VA_COT2_48);
            diccionarioEntrada[525] = Herramientas.ConvertirATexto(Entrada.VA_COT2_49);
            diccionarioEntrada[526] = Herramientas.ConvertirATexto(Entrada.VA_COT2_50);
            diccionarioEntrada[527] = Herramientas.ConvertirATexto(Entrada.TIND_COT2_ingreso_calculado_final);
            diccionarioEntrada[528] = Herramientas.ConvertirATexto(Entrada.TIND_COT2_tipo_ingreso);
            diccionarioEntrada[529] = Herramientas.ConvertirATexto(Entrada.EA_COT3_codigo_cliente);
            diccionarioEntrada[530] = Herramientas.ConvertirATexto(Entrada.EA_COT3_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[531] = Herramientas.ConvertirATexto(Entrada.EA_COT3_fecha_cabalope, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[532] = Herramientas.ConvertirATexto(Entrada.EA_COT3_fecha_consulta_reestructuracion, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[533] = Herramientas.ConvertirATexto(Entrada.EA_COT3_motivo_reestructuracion);
            diccionarioEntrada[534] = Herramientas.ConvertirATexto(Entrada.EA_COT3_fecha_consulta_refi_deu, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[535] = Herramientas.ConvertirATexto(Entrada.EA_COT3_motivo_refinanciacion);
            diccionarioEntrada[536] = Herramientas.ConvertirATexto(Entrada.EA_COT3_motivo_deu);
            diccionarioEntrada[537] = Herramientas.ConvertirATexto(Entrada.CND_COT3_cobis);
            diccionarioEntrada[538] = Herramientas.ConvertirATexto(Entrada.CND_COT3_nro_tributario);
            diccionarioEntrada[539] = Herramientas.ConvertirATexto(Entrada.CND_COT3_codigo_campana);
            diccionarioEntrada[540] = Herramientas.ConvertirATexto(Entrada.CND_COT3_vencimiento, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[541] = Herramientas.ConvertirATexto(Entrada.CND_COT3_motivo_rechazo);
            diccionarioEntrada[542] = Herramientas.ConvertirATexto(Entrada.BH_COT3_id_cobis);
            diccionarioEntrada[543] = Herramientas.ConvertirATexto(Entrada.BH_COT3_riesgo_score);
            diccionarioEntrada[544] = Herramientas.ConvertirATexto(Entrada.BH_COT3_riesgo_6);
            diccionarioEntrada[545] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_tc_limite);
            diccionarioEntrada[546] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_tc_exigible);
            diccionarioEntrada[547] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_tc_limite_max);
            diccionarioEntrada[548] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_cc_limite);
            diccionarioEntrada[549] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_cc_promedio3);
            diccionarioEntrada[550] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_sg_exigible);
            diccionarioEntrada[551] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_gp_exigible);
            diccionarioEntrada[552] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_gh_exigible);
            diccionarioEntrada[553] = Herramientas.ConvertirATexto(Entrada.BH_COT3_antiguedad_tc);
            diccionarioEntrada[554] = Herramientas.ConvertirATexto(Entrada.BH_COT3_antiguedad_cc);
            diccionarioEntrada[555] = Herramientas.ConvertirATexto(Entrada.BH_COT3_cob_status);
            diccionarioEntrada[556] = Herramientas.ConvertirATexto(Entrada.BH_COT3_hit);
            diccionarioEntrada[557] = Herramientas.ConvertirATexto(Entrada.BH_COT3_uso_tc_saldo_fin);
            diccionarioEntrada[558] = Herramientas.ConvertirATexto(Entrada.BH_COT3_ip2);
            diccionarioEntrada[559] = Herramientas.ConvertirATexto(Entrada.BH_COT3_consultar_veraz);
            diccionarioEntrada[560] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT3_rech_lev_sf_u24m_cant);
            diccionarioEntrada[561] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT3_rech_nolev_sf_u24m_monto);
            diccionarioEntrada[562] = Herramientas.ConvertirATexto(Entrada.CHEQ_COT3_rech_lev_sf_u24m_monto);
            diccionarioEntrada[563] = Herramientas.ConvertirATexto(Entrada.SC_COT3_exclusion);
            diccionarioEntrada[564] = Herramientas.ConvertirATexto(Entrada.SC_COT3_score_comp);
            diccionarioEntrada[565] = Herramientas.ConvertirATexto(Entrada.SC_COT3_categoria);
            diccionarioEntrada[566] = Herramientas.ConvertirATexto(Entrada.POL_COT3_flag_acuerdo_cc);
            diccionarioEntrada[567] = Herramientas.ConvertirATexto(Entrada.POL_COT3_flag_producto_crediticio_activo);
            diccionarioEntrada[568] = Herramientas.ConvertirATexto(Entrada.POL_COT3_antiguedad_tc_macro);
            diccionarioEntrada[569] = Herramientas.ConvertirATexto(Entrada.POL_COT3_antiguedad_cc_macro);
            diccionarioEntrada[570] = Herramientas.ConvertirATexto(Entrada.POL_COT3_cantidad_ocurrencia_6);
            diccionarioEntrada[571] = Herramientas.ConvertirATexto(Entrada.EX_COT3_fin_excepcion_vz, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[572] = Herramientas.ConvertirATexto(Entrada.VA_COT3_cuit);
            diccionarioEntrada[573] = Herramientas.ConvertirATexto(Entrada.VA_COT3_nombre_pe);
            diccionarioEntrada[574] = Herramientas.ConvertirATexto(Entrada.VA_COT3_apellido_pe);
            diccionarioEntrada[575] = Herramientas.ConvertirATexto(Entrada.VA_COT3_sexo_pe);
            diccionarioEntrada[576] = Herramientas.ConvertirATexto(Entrada.VA_COT3_fecha_nacimiento_pe, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[577] = Herramientas.ConvertirATexto(Entrada.VA_COT3_provincia_pe);
            diccionarioEntrada[578] = Herramientas.ConvertirATexto(Entrada.VA_COT3_codigo_postal_pe);
            diccionarioEntrada[579] = Herramientas.ConvertirATexto(Entrada.VA_COT3_fecha_carga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[580] = Herramientas.ConvertirATexto(Entrada.VA_COT3_periodo_score);
            diccionarioEntrada[581] = Herramientas.ConvertirATexto(Entrada.VA_COT3_empleador);
            diccionarioEntrada[582] = Herramientas.ConvertirATexto(Entrada.VA_COT3_flag_crediticio);
            diccionarioEntrada[583] = Herramientas.ConvertirATexto(Entrada.VA_COT3_sum_score);
            diccionarioEntrada[584] = Herramientas.ConvertirATexto(Entrada.VA_COT3_razon_social_afip);
            diccionarioEntrada[585] = Herramientas.ConvertirATexto(Entrada.VA_COT3_impuesto_ganancias);
            diccionarioEntrada[586] = Herramientas.ConvertirATexto(Entrada.VA_COT3_impuesto_iva);
            diccionarioEntrada[587] = Herramientas.ConvertirATexto(Entrada.VA_COT3_integrante_sociedad);
            diccionarioEntrada[588] = Herramientas.ConvertirATexto(Entrada.VA_COT3_es_empleador_afip);
            diccionarioEntrada[589] = Herramientas.ConvertirATexto(Entrada.VA_COT3_cat_monotributo);
            diccionarioEntrada[590] = Herramientas.ConvertirATexto(Entrada.VA_COT3_actividad_monotributo);
            diccionarioEntrada[591] = Herramientas.ConvertirATexto(Entrada.VA_COT3_actividad_monotributo_descripcion);
            diccionarioEntrada[592] = Herramientas.ConvertirATexto(Entrada.VA_COT3_fact_monotributo);
            diccionarioEntrada[593] = Herramientas.ConvertirATexto(Entrada.VA_COT3_ingreso_servicios);
            diccionarioEntrada[594] = Herramientas.ConvertirATexto(Entrada.VA_COT3_ingreso_ventas);
            diccionarioEntrada[595] = Herramientas.ConvertirATexto(Entrada.VA_COT3_ingreso_monotributo);
            diccionarioEntrada[596] = Herramientas.ConvertirATexto(Entrada.VA_COT3_es_empleado);
            diccionarioEntrada[597] = Herramientas.ConvertirATexto(Entrada.VA_COT3_es_autonomo);
            diccionarioEntrada[598] = Herramientas.ConvertirATexto(Entrada.VA_COT3_es_monotributista);
            diccionarioEntrada[599] = Herramientas.ConvertirATexto(Entrada.VA_COT3_es_jubilado);
            diccionarioEntrada[600] = Herramientas.ConvertirATexto(Entrada.VA_COT3_ingreso_transaccionalidad);
            diccionarioEntrada[601] = Herramientas.ConvertirATexto(Entrada.VA_COT3_opcional_1);
            diccionarioEntrada[602] = Herramientas.ConvertirATexto(Entrada.VA_COT3_1);
            diccionarioEntrada[603] = Herramientas.ConvertirATexto(Entrada.VA_COT3_2);
            diccionarioEntrada[604] = Herramientas.ConvertirATexto(Entrada.VA_COT3_3);
            diccionarioEntrada[605] = Herramientas.ConvertirATexto(Entrada.VA_COT3_4);
            diccionarioEntrada[606] = Herramientas.ConvertirATexto(Entrada.VA_COT3_5);
            diccionarioEntrada[607] = Herramientas.ConvertirATexto(Entrada.VA_COT3_6);
            diccionarioEntrada[608] = Herramientas.ConvertirATexto(Entrada.VA_COT3_7);
            diccionarioEntrada[609] = Herramientas.ConvertirATexto(Entrada.VA_COT3_8);
            diccionarioEntrada[610] = Herramientas.ConvertirATexto(Entrada.VA_COT3_9);
            diccionarioEntrada[611] = Herramientas.ConvertirATexto(Entrada.VA_COT3_10);
            diccionarioEntrada[612] = Herramientas.ConvertirATexto(Entrada.VA_COT3_11);
            diccionarioEntrada[613] = Herramientas.ConvertirATexto(Entrada.VA_COT3_12);
            diccionarioEntrada[614] = Herramientas.ConvertirATexto(Entrada.VA_COT3_13);
            diccionarioEntrada[615] = Herramientas.ConvertirATexto(Entrada.VA_COT3_14);
            diccionarioEntrada[616] = Herramientas.ConvertirATexto(Entrada.VA_COT3_15);
            diccionarioEntrada[617] = Herramientas.ConvertirATexto(Entrada.VA_COT3_16);
            diccionarioEntrada[618] = Herramientas.ConvertirATexto(Entrada.VA_COT3_17);
            diccionarioEntrada[619] = Herramientas.ConvertirATexto(Entrada.VA_COT3_18);
            diccionarioEntrada[620] = Herramientas.ConvertirATexto(Entrada.VA_COT3_19);
            diccionarioEntrada[621] = Herramientas.ConvertirATexto(Entrada.VA_COT3_20);
            diccionarioEntrada[622] = Herramientas.ConvertirATexto(Entrada.VA_COT3_21);
            diccionarioEntrada[623] = Herramientas.ConvertirATexto(Entrada.VA_COT3_22);
            diccionarioEntrada[624] = Herramientas.ConvertirATexto(Entrada.VA_COT3_23);
            diccionarioEntrada[625] = Herramientas.ConvertirATexto(Entrada.VA_COT3_24);
            diccionarioEntrada[626] = Herramientas.ConvertirATexto(Entrada.VA_COT3_25);
            diccionarioEntrada[627] = Herramientas.ConvertirATexto(Entrada.VA_COT3_26);
            diccionarioEntrada[628] = Herramientas.ConvertirATexto(Entrada.VA_COT3_27);
            diccionarioEntrada[629] = Herramientas.ConvertirATexto(Entrada.VA_COT3_28);
            diccionarioEntrada[630] = Herramientas.ConvertirATexto(Entrada.VA_COT3_29);
            diccionarioEntrada[631] = Herramientas.ConvertirATexto(Entrada.VA_COT3_30);
            diccionarioEntrada[632] = Herramientas.ConvertirATexto(Entrada.VA_COT3_31);
            diccionarioEntrada[633] = Herramientas.ConvertirATexto(Entrada.VA_COT3_32);
            diccionarioEntrada[634] = Herramientas.ConvertirATexto(Entrada.VA_COT3_33);
            diccionarioEntrada[635] = Herramientas.ConvertirATexto(Entrada.VA_COT3_34);
            diccionarioEntrada[636] = Herramientas.ConvertirATexto(Entrada.VA_COT3_35);
            diccionarioEntrada[637] = Herramientas.ConvertirATexto(Entrada.VA_COT3_36);
            diccionarioEntrada[638] = Herramientas.ConvertirATexto(Entrada.VA_COT3_37);
            diccionarioEntrada[639] = Herramientas.ConvertirATexto(Entrada.VA_COT3_38);
            diccionarioEntrada[640] = Herramientas.ConvertirATexto(Entrada.VA_COT3_39);
            diccionarioEntrada[641] = Herramientas.ConvertirATexto(Entrada.VA_COT3_40);
            diccionarioEntrada[642] = Herramientas.ConvertirATexto(Entrada.VA_COT3_41);
            diccionarioEntrada[643] = Herramientas.ConvertirATexto(Entrada.VA_COT3_42);
            diccionarioEntrada[644] = Herramientas.ConvertirATexto(Entrada.VA_COT3_43);
            diccionarioEntrada[645] = Herramientas.ConvertirATexto(Entrada.VA_COT3_44);
            diccionarioEntrada[646] = Herramientas.ConvertirATexto(Entrada.VA_COT3_45);
            diccionarioEntrada[647] = Herramientas.ConvertirATexto(Entrada.VA_COT3_46);
            diccionarioEntrada[648] = Herramientas.ConvertirATexto(Entrada.VA_COT3_47);
            diccionarioEntrada[649] = Herramientas.ConvertirATexto(Entrada.VA_COT3_48);
            diccionarioEntrada[650] = Herramientas.ConvertirATexto(Entrada.VA_COT3_49);
            diccionarioEntrada[651] = Herramientas.ConvertirATexto(Entrada.VA_COT3_50);
            diccionarioEntrada[652] = Herramientas.ConvertirATexto(Entrada.TIND_COT3_ingreso_calculado_final);
            diccionarioEntrada[653] = Herramientas.ConvertirATexto(Entrada.TIND_COT3_tipo_ingreso);
            diccionarioEntrada[654] = Herramientas.ConvertirATexto(Entrada.DM_nombre_convenio);
            diccionarioEntrada[655] = Herramientas.ConvertirATexto(Entrada.DM_nro_convenio);
            diccionarioEntrada[656] = Herramientas.ConvertirATexto(Entrada.DM_calificacion);
            diccionarioEntrada[657] = Herramientas.ConvertirATexto(Entrada.CCD_numero_submercado);
            diccionarioEntrada[658] = Herramientas.ConvertirATexto(Entrada.CCD_nombre_submercado);
            diccionarioEntrada[659] = Herramientas.ConvertirATexto(Entrada.CCD_estado_convenio);
            diccionarioEntrada[660] = Herramientas.ConvertirATexto(Entrada.CCD_fecha_ult_actualizacion, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[661] = Herramientas.ConvertirATexto(Entrada.CCD_calificacion_permanente);
            diccionarioEntrada[662] = Herramientas.ConvertirATexto(Entrada.CCD_calificacion_otra_planta);
            diccionarioEntrada[663] = Herramientas.ConvertirATexto(Entrada.CCD_afectacion_pp_permanente);
            diccionarioEntrada[664] = Herramientas.ConvertirATexto(Entrada.CCD_afectacion_pp_otra_planta);
            diccionarioEntrada[665] = Herramientas.ConvertirATexto(Entrada.CCD_afectacion_ph_permanente);
            diccionarioEntrada[666] = Herramientas.ConvertirATexto(Entrada.CCD_afectacion_ph_otra_planta);
            diccionarioEntrada[667] = Herramientas.ConvertirATexto(Entrada.CCD_monto_maximo_permanente);
            diccionarioEntrada[668] = Herramientas.ConvertirATexto(Entrada.CCD_monto_maximo_otra_planta);
            diccionarioEntrada[669] = Herramientas.ConvertirATexto(Entrada.CCD_cant_sueldos_maximo_permanente);
            diccionarioEntrada[670] = Herramientas.ConvertirATexto(Entrada.CCD_cant_sueldos_maximo_otra_planta);
            diccionarioEntrada[671] = Herramientas.ConvertirATexto(Entrada.CCD_plazo_permanente);
            diccionarioEntrada[672] = Herramientas.ConvertirATexto(Entrada.CCD_plazo_otra_planta);
            diccionarioEntrada[673] = Herramientas.ConvertirATexto(Entrada.CCD_max_endeu_global_permanente);
            diccionarioEntrada[674] = Herramientas.ConvertirATexto(Entrada.CCD_max_endeu_global_otra_planta);
            diccionarioEntrada[675] = Herramientas.ConvertirATexto(Entrada.CCD_opcional1_permanente);
            diccionarioEntrada[676] = Herramientas.ConvertirATexto(Entrada.CCD_opcional1_otra_planta);
            diccionarioEntrada[677] = Herramientas.ConvertirATexto(Entrada.CCD_opcional2_permanente);
            diccionarioEntrada[678] = Herramientas.ConvertirATexto(Entrada.CCD_opcional2_otra_planta);
            return diccionarioEntrada;
        }

        public string GeneraTramaSalida()
        {
            var sb = new StringBuilder();
            sb.Append(_Salida.fecha.ToString("yyyy-MM-dd") ?? string.Empty).Append(";")
                .Append(_Salida.id_oportunidad.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.id_evaluacion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.ap_origen.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nro_llamado.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.documento.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.edad.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_domicilio.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.ddjj_de_salud.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.antig_lab_ppal.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nombre_convenio.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nombre_submercado.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_computable_neto)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_ppal)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_sec)).Append(";")
                .Append(_Salida.sobre_que_ingreso_tope.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_total)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.pm_tarj_bureau)).Append(";")
                .Append(_Salida.sumo_pm_tj_bureau.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.sumo_sum_pagos_min_tarj_no_bureau.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.sprom_ac_bureau)).Append(";")
                .Append(_Salida.sumo_sprom_ac_bureau.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.sumo_sum_util_cta_cte_no_bureau.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.pago_min)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_total_final)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_total_actual_final)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_solicitado)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_titu_macro)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_bureau)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_total_bureau)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.endeudamiento_global_productos_con_garantia)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.endeudamiento_global_productos_sin_garantia)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_endeudamiento_bureau)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_endeudamiento_macro)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.max_afect_global_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.max_afect_global_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_ingreso)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_global_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.max_afect_pmo_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.max_afect_pmo_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_pmo_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_pmo_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.max_afect_pmo_tarj_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.max_afect_pmo_tarj_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_pmo_tarj_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_pmo_tarj_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_acuerdo_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_acuerdo_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_max_acuerdo)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_solicitado)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.tope_endeudamiento_acuerdo_interno)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_afect_actsec_cotitu)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_afect_macro)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.global_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.porc_afect_sf)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afect_adic_disp_global_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.cuota_total)).Append(";")
                .Append(_Salida.resolucion_titu.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_ip.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ltv)).Append(";")
                .Append(_Salida.VZ_poblacion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_score.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.calificacion_original.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.explicacion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.categoria.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.score_app.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.monto_recalculado_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.lim_tarj_recomendado_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.limite_acuerdo_recomendado_pesos)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_cotitu)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_bureau_cotitu)).Append(";")
                .Append(_Salida.documento_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nombre_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.apellido_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.tipo_doc_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_cuil_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_estimado_cotitu1)).Append(";")
                .Append(_Salida.resolucion_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_ip_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_score_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_poblacion_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.explicacion_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.categoria_cotitu1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.documento_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nombre_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.apellido_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.tipo_doc_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_cuil_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_estimado_cotitu2)).Append(";")
                .Append(_Salida.resolucion_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_ip_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_score_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_poblacion_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.explicacion_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.categoria_cotitu2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.documento_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nombre_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.apellido_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.tipo_doc_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_cuil_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_estimado_cotitu3)).Append(";")
                .Append(_Salida.resolucion_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_ip_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_score_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_poblacion_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.explicacion_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.categoria_cotitu3.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ph_porc)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ahorro)).Append(";")
                .Append(_Salida.opcional_1.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_2.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_4.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_5.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_6.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_7.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_8.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_9.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_10.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_11.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_12.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_13.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_14.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_15.ToString() ?? string.Empty);
            return sb.ToString();
        }


        public void Decodifica(string[] arraySalida)
        {
            _Salida.fecha = (DateOnly)Herramientas.ConvertirDeTexto(arraySalida[0], DbType.Date, new DateOnly(1900,1,1), "dd/MM/yyyy");
            _Salida.id_oportunidad = (string)Herramientas.ConvertirDeTexto(arraySalida[1], DbType.String, string.Empty);
            _Salida.id_evaluacion = (string)Herramientas.ConvertirDeTexto(arraySalida[2], DbType.String, string.Empty);
            _Salida.ap_origen = (string)Herramientas.ConvertirDeTexto(arraySalida[3], DbType.String, string.Empty);
            _Salida.nro_llamado = (string)Herramientas.ConvertirDeTexto(arraySalida[4], DbType.String, string.Empty);
            _Salida.documento = (string)Herramientas.ConvertirDeTexto(arraySalida[5], DbType.String, string.Empty);
            _Salida.edad = (string)Herramientas.ConvertirDeTexto(arraySalida[6], DbType.String, string.Empty);
            _Salida.VZ_domicilio = (string)Herramientas.ConvertirDeTexto(arraySalida[7], DbType.String, string.Empty);
            _Salida.ddjj_de_salud = (string)Herramientas.ConvertirDeTexto(arraySalida[8], DbType.String, string.Empty);
            _Salida.antig_lab_ppal = (string)Herramientas.ConvertirDeTexto(arraySalida[9], DbType.String, string.Empty);
            _Salida.nombre_convenio = (string)Herramientas.ConvertirDeTexto(arraySalida[10], DbType.String, string.Empty);
            _Salida.nombre_submercado = (string)Herramientas.ConvertirDeTexto(arraySalida[11], DbType.String, string.Empty);
            _Salida.ingreso_computable_neto = (decimal)Herramientas.ConvertirDeTexto(arraySalida[12], DbType.Decimal, decimal.Zero);
            _Salida.ingreso_ppal = (decimal)Herramientas.ConvertirDeTexto(arraySalida[13], DbType.Decimal, decimal.Zero);
            _Salida.ingreso_sec = (decimal)Herramientas.ConvertirDeTexto(arraySalida[14], DbType.Decimal, decimal.Zero);
            _Salida.sobre_que_ingreso_tope = (string)Herramientas.ConvertirDeTexto(arraySalida[15], DbType.String, string.Empty);
            _Salida.ingreso_total = (decimal)Herramientas.ConvertirDeTexto(arraySalida[16], DbType.Decimal, decimal.Zero);
            _Salida.pm_tarj_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[17], DbType.Decimal, decimal.Zero);
            _Salida.sumo_pm_tj_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[18], DbType.String, string.Empty);
            _Salida.sumo_sum_pagos_min_tarj_no_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[19], DbType.String, string.Empty);
            _Salida.sprom_ac_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[20], DbType.Decimal, decimal.Zero);
            _Salida.sumo_sprom_ac_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[21], DbType.String, string.Empty);
            _Salida.sumo_sum_util_cta_cte_no_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[22], DbType.String, string.Empty);
            _Salida.pago_min = (decimal)Herramientas.ConvertirDeTexto(arraySalida[23], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_total_final = (decimal)Herramientas.ConvertirDeTexto(arraySalida[24], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_total_actual_final = (decimal)Herramientas.ConvertirDeTexto(arraySalida[25], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_solicitado = (decimal)Herramientas.ConvertirDeTexto(arraySalida[26], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_titu_macro = (decimal)Herramientas.ConvertirDeTexto(arraySalida[27], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[28], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_total_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[29], DbType.Decimal, decimal.Zero);
            _Salida.endeudamiento_global_productos_con_garantia = (decimal)Herramientas.ConvertirDeTexto(arraySalida[30], DbType.Decimal, decimal.Zero);
            _Salida.endeudamiento_global_productos_sin_garantia = (decimal)Herramientas.ConvertirDeTexto(arraySalida[31], DbType.Decimal, decimal.Zero);
            _Salida.porc_endeudamiento_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[32], DbType.Decimal, decimal.Zero);
            _Salida.porc_endeudamiento_macro = (decimal)Herramientas.ConvertirDeTexto(arraySalida[33], DbType.Decimal, decimal.Zero);
            _Salida.max_afect_global_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[34], DbType.Decimal, decimal.Zero);
            _Salida.max_afect_global_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[35], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_ingreso = (decimal)Herramientas.ConvertirDeTexto(arraySalida[36], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_global_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[37], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[38], DbType.Decimal, decimal.Zero);
            _Salida.max_afect_pmo_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[39], DbType.Decimal, decimal.Zero);
            _Salida.max_afect_pmo_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[40], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_pmo_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[41], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_pmo_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[42], DbType.Decimal, decimal.Zero);
            _Salida.max_afect_pmo_tarj_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[43], DbType.Decimal, decimal.Zero);
            _Salida.max_afect_pmo_tarj_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[44], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_pmo_tarj_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[45], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_pmo_tarj_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[46], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_acuerdo_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[47], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_acuerdo_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[48], DbType.Decimal, decimal.Zero);
            _Salida.porc_max_acuerdo = (decimal)Herramientas.ConvertirDeTexto(arraySalida[49], DbType.Decimal, decimal.Zero);
            _Salida.porc_solicitado = (decimal)Herramientas.ConvertirDeTexto(arraySalida[50], DbType.Decimal, decimal.Zero);
            _Salida.tope_endeudamiento_acuerdo_interno = (decimal)Herramientas.ConvertirDeTexto(arraySalida[51], DbType.Decimal, decimal.Zero);
            _Salida.porc_afect_actsec_cotitu = (decimal)Herramientas.ConvertirDeTexto(arraySalida[52], DbType.Decimal, decimal.Zero);
            _Salida.porc_afect_macro = (decimal)Herramientas.ConvertirDeTexto(arraySalida[53], DbType.Decimal, decimal.Zero);
            _Salida.global_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[54], DbType.Decimal, decimal.Zero);
            _Salida.porc_afect_sf = (decimal)Herramientas.ConvertirDeTexto(arraySalida[55], DbType.Decimal, decimal.Zero);
            _Salida.afect_adic_disp_global_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[56], DbType.Decimal, decimal.Zero);
            _Salida.cuota_total = (decimal)Herramientas.ConvertirDeTexto(arraySalida[57], DbType.Decimal, decimal.Zero);
            _Salida.resolucion_titu = (string)Herramientas.ConvertirDeTexto(arraySalida[58], DbType.String, string.Empty);
            _Salida.VZ_ip = (string)Herramientas.ConvertirDeTexto(arraySalida[59], DbType.String, string.Empty);
            _Salida.ltv = (decimal)Herramientas.ConvertirDeTexto(arraySalida[60], DbType.Decimal, decimal.Zero);
            _Salida.VZ_poblacion = (string)Herramientas.ConvertirDeTexto(arraySalida[61], DbType.String, string.Empty);
            _Salida.VZ_score = (string)Herramientas.ConvertirDeTexto(arraySalida[62], DbType.String, string.Empty);
            _Salida.calificacion_original = (string)Herramientas.ConvertirDeTexto(arraySalida[63], DbType.String, string.Empty);
            _Salida.explicacion = (string)Herramientas.ConvertirDeTexto(arraySalida[64], DbType.String, string.Empty);
            _Salida.categoria = (string)Herramientas.ConvertirDeTexto(arraySalida[65], DbType.String, string.Empty);
            _Salida.score_app = (string)Herramientas.ConvertirDeTexto(arraySalida[66], DbType.String, string.Empty);
            _Salida.monto_recalculado_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[67], DbType.Decimal, decimal.Zero);
            _Salida.lim_tarj_recomendado_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[68], DbType.Decimal, decimal.Zero);
            _Salida.limite_acuerdo_recomendado_pesos = (decimal)Herramientas.ConvertirDeTexto(arraySalida[69], DbType.Decimal, decimal.Zero);
            _Salida.ingreso_cotitu = (decimal)Herramientas.ConvertirDeTexto(arraySalida[70], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_bureau_cotitu = (decimal)Herramientas.ConvertirDeTexto(arraySalida[71], DbType.Decimal, decimal.Zero);
            _Salida.documento_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[72], DbType.String, string.Empty);
            _Salida.nombre_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[73], DbType.String, string.Empty);
            _Salida.apellido_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[74], DbType.String, string.Empty);
            _Salida.tipo_doc_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[75], DbType.String, string.Empty);
            _Salida.VZ_cuil_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[76], DbType.String, string.Empty);
            _Salida.ingreso_estimado_cotitu1 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[77], DbType.Decimal, decimal.Zero);
            _Salida.resolucion_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[78], DbType.String, string.Empty);
            _Salida.VZ_ip_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[79], DbType.String, string.Empty);
            _Salida.VZ_score_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[80], DbType.String, string.Empty);
            _Salida.VZ_poblacion_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[81], DbType.String, string.Empty);
            _Salida.explicacion_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[82], DbType.String, string.Empty);
            _Salida.categoria_cotitu1 = (string)Herramientas.ConvertirDeTexto(arraySalida[83], DbType.String, string.Empty);
            _Salida.documento_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[84], DbType.String, string.Empty);
            _Salida.nombre_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[85], DbType.String, string.Empty);
            _Salida.apellido_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[86], DbType.String, string.Empty);
            _Salida.tipo_doc_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[87], DbType.String, string.Empty);
            _Salida.VZ_cuil_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[88], DbType.String, string.Empty);
            _Salida.ingreso_estimado_cotitu2 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[89], DbType.Decimal, decimal.Zero);
            _Salida.resolucion_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[90], DbType.String, string.Empty);
            _Salida.VZ_ip_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[91], DbType.String, string.Empty);
            _Salida.VZ_score_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[92], DbType.String, string.Empty);
            _Salida.VZ_poblacion_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[93], DbType.String, string.Empty);
            _Salida.explicacion_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[94], DbType.String, string.Empty);
            _Salida.categoria_cotitu2 = (string)Herramientas.ConvertirDeTexto(arraySalida[95], DbType.String, string.Empty);
            _Salida.documento_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[96], DbType.String, string.Empty);
            _Salida.nombre_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[97], DbType.String, string.Empty);
            _Salida.apellido_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[98], DbType.String, string.Empty);
            _Salida.tipo_doc_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[99], DbType.String, string.Empty);
            _Salida.VZ_cuil_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[100], DbType.String, string.Empty);
            _Salida.ingreso_estimado_cotitu3 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[101], DbType.Decimal, decimal.Zero);
            _Salida.resolucion_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[102], DbType.String, string.Empty);
            _Salida.VZ_ip_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[103], DbType.String, string.Empty);
            _Salida.VZ_score_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[104], DbType.String, string.Empty);
            _Salida.VZ_poblacion_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[105], DbType.String, string.Empty);
            _Salida.explicacion_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[106], DbType.String, string.Empty);
            _Salida.categoria_cotitu3 = (string)Herramientas.ConvertirDeTexto(arraySalida[107], DbType.String, string.Empty);
            _Salida.ph_porc = (decimal)Herramientas.ConvertirDeTexto(arraySalida[108], DbType.Decimal, decimal.Zero);
            _Salida.ahorro = (decimal)Herramientas.ConvertirDeTexto(arraySalida[109], DbType.Decimal, decimal.Zero);
            _Salida.opcional_1 = (string)Herramientas.ConvertirDeTexto(arraySalida[110], DbType.String, string.Empty);
            _Salida.opcional_2 = (string)Herramientas.ConvertirDeTexto(arraySalida[111], DbType.String, string.Empty);
            _Salida.opcional_3 = (string)Herramientas.ConvertirDeTexto(arraySalida[112], DbType.String, string.Empty);
            _Salida.opcional_4 = (string)Herramientas.ConvertirDeTexto(arraySalida[113], DbType.String, string.Empty);
            _Salida.opcional_5 = (string)Herramientas.ConvertirDeTexto(arraySalida[114], DbType.String, string.Empty);
            _Salida.opcional_6 = (string)Herramientas.ConvertirDeTexto(arraySalida[115], DbType.String, string.Empty);
            _Salida.opcional_7 = (string)Herramientas.ConvertirDeTexto(arraySalida[116], DbType.String, string.Empty);
            _Salida.opcional_8 = (string)Herramientas.ConvertirDeTexto(arraySalida[117], DbType.String, string.Empty);
            _Salida.opcional_9 = (string)Herramientas.ConvertirDeTexto(arraySalida[118], DbType.String, string.Empty);
            _Salida.opcional_10 = (string)Herramientas.ConvertirDeTexto(arraySalida[119], DbType.String, string.Empty);
            _Salida.opcional_11 = (string)Herramientas.ConvertirDeTexto(arraySalida[120], DbType.String, string.Empty);
            _Salida.opcional_12 = (string)Herramientas.ConvertirDeTexto(arraySalida[121], DbType.String, string.Empty);
            _Salida.opcional_13 = (string)Herramientas.ConvertirDeTexto(arraySalida[122], DbType.String, string.Empty);
            _Salida.opcional_14 = (string)Herramientas.ConvertirDeTexto(arraySalida[123], DbType.String, string.Empty);
            _Salida.opcional_15 = (string)Herramientas.ConvertirDeTexto(arraySalida[124], DbType.String, string.Empty);
        }
        public void InsertaDictamenes(IDictaminador dictamenes)
        {
            throw new NotImplementedException();
        }

        void IEvaluable.GarantizaValores()
        {
            throw new NotImplementedException();
        }
        private void MapeaEntrada(FuentesDeInformacion fuentes)
        {
            var request = fuentes.RequestLegacy.entrada.Split(";");
            var integrantes = fuentes.Integrantes;
            Entrada = new();
            Entrada.id_oportunidad = request[0];
            Entrada.nombre_oportunidad = request[1];
            Entrada.id_evaluacion = request[2];
            Entrada.ap_origen = request[3];
            Entrada.nro_llamado = request[4];
            Entrada.id_cobis = request[5];
            Entrada.documento = request[6];
            Entrada.nombre = request[7];
            Entrada.apellido = request[8];
            Entrada.sexo = request[9];
            Entrada.version_doc = request[10];
            Entrada.tipo_doc = request[11];
            Entrada.fecha_nac = request[12];
            Entrada.nro_convenio = request[13];
            Entrada.nombre_convenio = request[14];
            Entrada.calificacion = request[15];
            Entrada.submercado = request[16];
            Entrada.prov = request[17];
            Entrada.zona = request[18];
            Entrada.cod_sucursal = request[19];
            Entrada.est_civil = request[20];
            Entrada.flia = request[21];
            Entrada.vivienda = request[22];
            Entrada.contribuyente = request[23];
            Entrada.laboral_ppal = request[24];
            Entrada.fech_laboral = request[25];
            Entrada.neto_fijo_ppal = request[26];
            Entrada.neto_var_ppal = request[27];
            Entrada.act_sec = request[28];
            Entrada.laboral_sec = request[29];
            Entrada.monotributo_sec = request[30];
            Entrada.categoria_selecta = request[31];
            Entrada.actividad_monotributo_sec = request[32];
            Entrada.fech_laboral_sec = request[33];
            Entrada.neto_fijo_sec = request[34];
            Entrada.neto_var_sec = request[35];
            Entrada.pp_cuota_macro = request[36];
            Entrada.pp_monto_macro = request[37];
            Entrada.pr_cuota_macro = request[38];
            Entrada.pr_monto_macro = request[39];
            Entrada.ph_cuota_macro = request[40];
            Entrada.ph_monto_macro = request[41];
            Entrada.op_cuota_macro = request[42];
            Entrada.op_monto_macro = request[43];
            Entrada.lim_tarj_macro = request[44];
            Entrada.pago_min_tarj_macro = request[45];
            Entrada.acue_cta_macro = request[46];
            Entrada.uti_cta_cte_macro = request[47];
            Entrada.dias_atraso = request[48];
            Entrada.sum_cuotas_p_sg_no_bureau = request[49];
            Entrada.sum_cuotas_p_cg_no_bureau = request[50];
            Entrada.sum_pagos_min_tarj_no_bureau = request[51];
            Entrada.sum_util_cta_cte_no_bureau = request[52];
            Entrada.prestamo = request[53];
            Entrada.tarjeta = request[54];
            Entrada.cta_cte = request[55];
            Entrada.tipo_prestamo = request[56];
            Entrada.plazo = request[57];
            Entrada.destino_personal = request[58];
            Entrada.monto = request[59];
            Entrada.tipo_tasa = request[60];
            Entrada.tna = request[61];
            Entrada.modelo_auto = request[62];
            Entrada.auto = request[63];
            Entrada.seg_auto = request[64];
            Entrada.tasacion = request[65];
            Entrada.val_compra_mejora_proyecto = request[66];
            Entrada.val_terreno = request[67];
            Entrada.destino = request[68];
            Entrada.lim_tarj = request[69];
            Entrada.acuerdo_cta_cte = request[70];
            Entrada.cotitu = request[71];
            Entrada.id_cobis_cotitu1 = request[72];
            Entrada.documento_cotitu1 = request[73];
            Entrada.nombre_cotitu1 = request[74];
            Entrada.apellido_cotitu1 = request[75];
            Entrada.sexo_cotitu1 = request[76];
            Entrada.version_doc_cotitu1 = request[77];
            Entrada.tipo_doc_cotitu1 = request[78];
            Entrada.fech_laboral_cotitu1 = request[79];
            Entrada.neto_fijo_cotitu1 = request[80];
            Entrada.neto_var_cotitu1 = request[81];
            Entrada.dias_atraso_cotitu1 = request[82];
            Entrada.id_cobis_cotitu2 = request[83];
            Entrada.documento_cotitu2 = request[84];
            Entrada.nombre_cotitu2 = request[85];
            Entrada.apellido_cotitu2 = request[86];
            Entrada.sexo_cotitu2 = request[87];
            Entrada.version_doc_cotitu2 = request[88];
            Entrada.tipo_doc_cotitu2 = request[89];
            Entrada.fech_laboral_cotitu2 = request[90];
            Entrada.neto_fijo_cotitu2 = request[91];
            Entrada.neto_var_cotitu2 = request[92];
            Entrada.dias_atraso_cotitu2 = request[93];
            Entrada.id_cobis_cotitu3 = request[94];
            Entrada.documento_cotitu3 = request[95];
            Entrada.nombre_cotitu3 = request[96];
            Entrada.apellido_cotitu3 = request[97];
            Entrada.sexo_cotitu3 = request[98];
            Entrada.version_doc_cotitu3 = request[99];
            Entrada.tipo_doc_cotitu3 = request[100];
            Entrada.fech_laboral_cotitu3 = request[101];
            Entrada.neto_fijo_cotitu3 = request[102];
            Entrada.neto_var_cotitu3 = request[103];
            Entrada.dias_atraso_cotitu3 = request[104];
            Entrada.neto_fijo_cotitu123 = request[105];
            Entrada.neto_var_cotitu123 = request[106];
            Entrada.canal = request[107];
            Entrada.equipo_ventas = request[108];
            Entrada.tipo_fuente = request[109];
            Entrada.fuente = request[110];
            Entrada.segmento = request[111];
            Entrada.subsegmento = request[112];
            Entrada.nombre_plantilla = request[113];
            Entrada.estado = request[114];
            Entrada.atributo1 = request[115];
            Entrada.valor1 = request[116];
            Entrada.atributo2 = request[117];
            Entrada.valor2 = request[118];
            Entrada.atributo3 = request[119];
            Entrada.valor3 = request[120];
            Entrada.atributo4 = request[121];
            Entrada.valor4 = request[122];
            Entrada.atributo5 = request[123];
            Entrada.valor5 = request[124];
            Entrada.atributo6 = request[125];
            Entrada.valor6 = request[126];
            Entrada.atributo7 = request[127];
            Entrada.valor7 = request[128];
            Entrada.atributo8 = request[129];
            Entrada.valor8 = request[130];
            Entrada.atributo9 = request[131];
            Entrada.valor9 = request[132];
            Entrada.atributo10 = request[133];
            Entrada.valor10 = request[134];
            Entrada.usuario = request[135];
            Entrada.cliente_inversor = request[136];
            Entrada.monto_inversion = request[137];
            Entrada.monto_subsidio = request[138];
            if (request.Count() > 139) Entrada.opcional_1 = string.IsNullOrEmpty(request[139]) ? fuentes.Registro.Id.ToString() : request[139];
            else Entrada.opcional_1 = fuentes.Registro.Id.ToString();
            Entrada.opcional_2 = (request.Count() > 140) ? request[140] : string.Empty;
            Entrada.opcional_3 = (request.Count() > 141) ? request[141] : string.Empty;
            Entrada.opcional_4 = (request.Count() > 142) ? request[142] : string.Empty;
            Entrada.opcional_5 = (request.Count() > 143) ? request[143] : string.Empty;
            Entrada.opcional_6 = (request.Count() > 144) ? request[144] : string.Empty;
            Entrada.opcional_7 = (request.Count() > 145) ? request[145] : string.Empty;
            Entrada.opcional_8 = (request.Count() > 146) ? request[146] : string.Empty;
            Entrada.opcional_9 = (request.Count() > 147) ? request[147] : string.Empty;
            Entrada.opcional_10 = (request.Count() > 148) ? request[148] : string.Empty;
            Entrada.opcional_11 = (request.Count() > 149) ? request[149] : string.Empty;
            Entrada.opcional_12 = (request.Count() > 150) ? request[150] : string.Empty;
            Entrada.opcional_13 = (request.Count() > 151) ? request[151] : string.Empty;
            Entrada.opcional_14 = (request.Count() > 152) ? request[152] : string.Empty;
            Entrada.opcional_15 = (request.Count() > 153) ? request[153] : string.Empty;

            EndeudamientoAtraso endeudamientoAtraso;
            if (fuentes.EndeudamientoAtraso.TryGetValue(integrantes.Titular?.Cobis ?? -1, out endeudamientoAtraso))
            {
                Entrada.EA_TIT_codigo_cliente = endeudamientoAtraso.CodigoCliente ?? 0;
                Entrada.EA_TIT_fecha_carga = endeudamientoAtraso.FechaCarga ?? new DateOnly(1900, 1, 1);
                Entrada.EA_TIT_fecha_cabalope = endeudamientoAtraso.FechaCabalope ?? new DateOnly(1900, 1, 1);
                Entrada.EA_TIT_fecha_consulta_reestructuracion = endeudamientoAtraso.FechaConsultaReestructuracion ?? new DateOnly(1900, 1, 1);
                Entrada.EA_TIT_motivo_reestructuracion = endeudamientoAtraso.MotivoReestructuracion ?? string.Empty;
                Entrada.EA_TIT_fecha_consulta_refi_deu = endeudamientoAtraso.FechaConsultaRefiDeu ?? new DateOnly(1900, 1, 1);
                Entrada.EA_TIT_motivo_refinanciacion = endeudamientoAtraso.MotivoRefinanciacion ?? string.Empty;
                Entrada.EA_TIT_motivo_deu = endeudamientoAtraso.MotivoDeu ?? string.Empty;
            }

            ClientesDepurados clientesDepurados;
            if (fuentes.ClientesDepurados.TryGetValue(integrantes.Titular?.NumeroTributario.ToString() ?? "", out clientesDepurados))
            {
                Entrada.CND_TIT_cobis = (int)Herramientas.ConvertirDeTexto(clientesDepurados.CodigoCliente, DbType.Int32, 0);
                Entrada.CND_TIT_nro_tributario = clientesDepurados.Cuil ?? string.Empty;
                Entrada.CND_TIT_codigo_campana = clientesDepurados.Depuracion ?? string.Empty;
                Entrada.CND_TIT_vencimiento = DateOnly.FromDateTime(clientesDepurados.DepuracionHasta ?? new DateTime(1900, 1, 1));
                Entrada.CND_TIT_motivo_rechazo = clientesDepurados.DepuracionMotivo ?? string.Empty;
            }

            Behavior behavior;
            if (fuentes.Behavior.TryGetValue(integrantes.Titular?.Cobis ?? -1, out behavior))
            {
                Entrada.BH_TIT_id_cobis = behavior.ClienteCobis ?? 0;
                Entrada.BH_TIT_riesgo_score = behavior.RiesgoScore ?? 0;
                Entrada.BH_TIT_riesgo_6 = behavior.Riesgo6 ?? 0;
                Entrada.BH_TIT_uso_tc_limite = behavior.UsoTcLimite ?? decimal.Zero;
                Entrada.BH_TIT_uso_tc_exigible = behavior.UsoTcExigible ?? decimal.Zero;
                Entrada.BH_TIT_uso_tc_limite_max = behavior.UsoTcLimiteMax ?? decimal.Zero;
                Entrada.BH_TIT_uso_cc_limite = behavior.UsoCcLimite ?? decimal.Zero;
                Entrada.BH_TIT_uso_cc_promedio3 = behavior.UsoCcPromedio3 ?? decimal.Zero;
                Entrada.BH_TIT_uso_sg_exigible = behavior.UsoSgExigible ?? decimal.Zero;
                Entrada.BH_TIT_uso_gp_exigible = behavior.UsoGpExigible ?? decimal.Zero;
                Entrada.BH_TIT_uso_gh_exigible = behavior.UsoGhExigible ?? decimal.Zero;
                Entrada.BH_TIT_antiguedad_tc = behavior.AntiguedadTc ?? 0;
                Entrada.BH_TIT_antiguedad_cc = behavior.AntiguedadCc ?? 0;
                Entrada.BH_TIT_cob_status = behavior.CobStatus ?? 0;
                Entrada.BH_TIT_hit = behavior.Hit ?? -1;
                Entrada.BH_TIT_uso_tc_saldo_fin = behavior.VUsoTcSaldoFin ?? decimal.Zero;
                Entrada.BH_TIT_ip2 = behavior.Ip2 ?? string.Empty;
                Entrada.BH_TIT_consultar_veraz = behavior.ConsultarVeraz ?? string.Empty;
            }

            ChequesRechazados chequesRechazados;
            if (fuentes.ChequesRechazados.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out chequesRechazados))
            {
                Entrada.CHEQ_TIT_rech_lev_sf_u24m_cant = chequesRechazados.RechLevSFu24mCant ?? 0;
                Entrada.CHEQ_TIT_rech_nolev_sf_u24m_monto = chequesRechazados.RechNoLevSFu24mMonto ?? decimal.Zero;
                Entrada.CHEQ_TIT_rech_lev_sf_u24m_monto = chequesRechazados.RechLevSFu24mMonto ?? decimal.Zero;
            }

            ScoreComportamiento scoreComportamiento;
            if (fuentes.ScoreComportamiento.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out scoreComportamiento))
            {
                Entrada.SC_TIT_exclusion = scoreComportamiento.exclusion ?? string.Empty;
                Entrada.SC_TIT_score_comp = (int)(scoreComportamiento.score ?? decimal.Zero);
                Entrada.SC_TIT_categoria = scoreComportamiento.categoria ?? string.Empty;
            }

            Tablon tablon;
            if (fuentes.Tablon.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out tablon))
            {
                Entrada.POL_TIT_flag_acuerdo_cc = tablon.POL_Flag_Acuerdo_CC ?? 0;
                Entrada.POL_TIT_flag_producto_crediticio_activo = tablon.FlagProductoCrediticioActivo ?? 0;
                Entrada.POL_TIT_antiguedad_tc_macro = tablon.AntiguedadTc ?? decimal.Zero;
                Entrada.POL_TIT_antiguedad_cc_macro = tablon.AntiguedadCc ?? decimal.Zero;
                Entrada.POL_TIT_cantidad_ocurrencia_6 = tablon.CantidadOcurrencia6 ?? 0;
            }

            Excepciones excepciones;
            if (fuentes.Excepciones.TryGetValue(integrantes.Titular?.NumeroTributario.ToString() ?? "", out excepciones))
            {
                Entrada.EX_TIT_fin_excepcion_vz = DateOnly.FromDateTime(excepciones.FechaVencimiento ?? new DateTime(1900, 1, 1));
            }

            ValidacionActividad validacionActividad;
            if (fuentes.ValidacionActividad.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out validacionActividad))
            {
                Entrada.VA_TIT_cuit = validacionActividad.Cuit ?? 0;
                Entrada.VA_TIT_nombre_pe = validacionActividad.NombrePe ?? string.Empty;
                Entrada.VA_TIT_apellido_pe = validacionActividad.ApellidoPe ?? string.Empty;
                Entrada.VA_TIT_sexo_pe = validacionActividad.SexoPe ?? string.Empty;
                Entrada.VA_TIT_fecha_nacimiento_pe = DateOnly.FromDateTime(validacionActividad.FechaNacimientoPe ?? new DateTime(1900, 1, 1));
                Entrada.VA_TIT_provincia_pe = validacionActividad.ProvinciaPe ?? string.Empty;
                Entrada.VA_TIT_codigo_postal_pe = validacionActividad.CodigoPostalPe ?? string.Empty;
                Entrada.VA_TIT_fecha_carga = DateOnly.FromDateTime(validacionActividad.FechaCarga ?? new DateTime(1900, 1, 1));
                Entrada.VA_TIT_periodo_score = validacionActividad.PeriodoScore ?? 0;
                Entrada.VA_TIT_empleador = validacionActividad.Empleador ?? 0;
                Entrada.VA_TIT_flag_crediticio = validacionActividad.FlagCrediticio ?? 0;
                Entrada.VA_TIT_sum_score = validacionActividad.SumScore ?? decimal.Zero;
                Entrada.VA_TIT_razon_social_afip = validacionActividad.RazonSocialAfip ?? string.Empty;
                Entrada.VA_TIT_impuesto_ganancias = validacionActividad.ImpuestoGanancias ?? string.Empty;
                Entrada.VA_TIT_impuesto_iva = validacionActividad.ImpuestoIva ?? string.Empty;
                Entrada.VA_TIT_integrante_sociedad = validacionActividad.IntegranteSociedad ?? string.Empty;
                Entrada.VA_TIT_es_empleador_afip = validacionActividad.EsEmpleadorAfip ?? string.Empty;
                Entrada.VA_TIT_cat_monotributo = validacionActividad.CatMonotributo ?? string.Empty;
                Entrada.VA_TIT_actividad_monotributo = validacionActividad.ActividadMonotributo ?? string.Empty;
                Entrada.VA_TIT_actividad_monotributo_descripcion = validacionActividad.ActividadMonotributoDescripcion ?? string.Empty;
                Entrada.VA_TIT_fact_monotributo = validacionActividad.FactMonotributo ?? decimal.Zero;
                Entrada.VA_TIT_ingreso_servicios = validacionActividad.IngresoServicios ?? decimal.Zero;
                Entrada.VA_TIT_ingreso_ventas = validacionActividad.IngresoVentas ?? decimal.Zero;
                Entrada.VA_TIT_ingreso_monotributo = validacionActividad.IngresoMonotributo ?? decimal.Zero;
                Entrada.VA_TIT_es_empleado = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_TIT_es_autonomo = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_TIT_es_monotributista = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_TIT_es_jubilado = validacionActividad.EsJubilado ?? string.Empty;
                Entrada.VA_TIT_ingreso_transaccionalidad = validacionActividad.IngresoTransaccionalidad ?? decimal.Zero;
                Entrada.VA_TIT_opcional_1 = validacionActividad.Opcional1 ?? string.Empty;
                Entrada.VA_TIT_1 = validacionActividad.Va1 ?? string.Empty;
                Entrada.VA_TIT_2 = validacionActividad.Va2 ?? string.Empty;
                Entrada.VA_TIT_3 = validacionActividad.Va3 ?? string.Empty;
                Entrada.VA_TIT_4 = validacionActividad.Va4 ?? string.Empty;
                Entrada.VA_TIT_5 = validacionActividad.Va5 ?? string.Empty;
                Entrada.VA_TIT_6 = validacionActividad.Va6 ?? string.Empty;
                Entrada.VA_TIT_7 = validacionActividad.Va7 ?? string.Empty;
                Entrada.VA_TIT_8 = validacionActividad.Va8 ?? string.Empty;
                Entrada.VA_TIT_9 = validacionActividad.Va9 ?? string.Empty;
                Entrada.VA_TIT_10 = validacionActividad.Va10 ?? string.Empty;
                Entrada.VA_TIT_11 = validacionActividad.Va11 ?? string.Empty;
                Entrada.VA_TIT_12 = validacionActividad.Va12 ?? string.Empty;
                Entrada.VA_TIT_13 = validacionActividad.Va13 ?? string.Empty;
                Entrada.VA_TIT_14 = validacionActividad.Va14 ?? string.Empty;
                Entrada.VA_TIT_15 = validacionActividad.Va15 ?? string.Empty;
                Entrada.VA_TIT_16 = validacionActividad.Va16 ?? string.Empty;
                Entrada.VA_TIT_17 = validacionActividad.Va17 ?? string.Empty;
                Entrada.VA_TIT_18 = validacionActividad.Va18 ?? string.Empty;
                Entrada.VA_TIT_19 = validacionActividad.Va19 ?? string.Empty;
                Entrada.VA_TIT_20 = validacionActividad.Va20 ?? string.Empty;
                Entrada.VA_TIT_21 = validacionActividad.Va21 ?? string.Empty;
                Entrada.VA_TIT_22 = validacionActividad.Va22 ?? string.Empty;
                Entrada.VA_TIT_23 = validacionActividad.Va23 ?? string.Empty;
                Entrada.VA_TIT_24 = validacionActividad.Va24 ?? string.Empty;
                Entrada.VA_TIT_25 = validacionActividad.Va25 ?? string.Empty;
                Entrada.VA_TIT_26 = validacionActividad.Va26 ?? string.Empty;
                Entrada.VA_TIT_27 = validacionActividad.Va27 ?? string.Empty;
                Entrada.VA_TIT_28 = validacionActividad.Va28 ?? string.Empty;
                Entrada.VA_TIT_29 = validacionActividad.Va29 ?? string.Empty;
                Entrada.VA_TIT_30 = validacionActividad.Va30 ?? string.Empty;
                Entrada.VA_TIT_31 = validacionActividad.Va31 ?? string.Empty;
                Entrada.VA_TIT_32 = validacionActividad.Va32 ?? string.Empty;
                Entrada.VA_TIT_33 = validacionActividad.Va33 ?? string.Empty;
                Entrada.VA_TIT_34 = validacionActividad.Va34 ?? string.Empty;
                Entrada.VA_TIT_35 = validacionActividad.Va35 ?? string.Empty;
                Entrada.VA_TIT_36 = validacionActividad.Va36 ?? string.Empty;
                Entrada.VA_TIT_37 = validacionActividad.Va37 ?? string.Empty;
                Entrada.VA_TIT_38 = validacionActividad.Va38 ?? string.Empty;
                Entrada.VA_TIT_39 = validacionActividad.Va39 ?? string.Empty;
                Entrada.VA_TIT_40 = validacionActividad.Va40 ?? string.Empty;
                Entrada.VA_TIT_41 = validacionActividad.Va41 ?? string.Empty;
                Entrada.VA_TIT_42 = validacionActividad.Va42 ?? string.Empty;
                Entrada.VA_TIT_43 = validacionActividad.Va43 ?? string.Empty;
                Entrada.VA_TIT_44 = validacionActividad.Va44 ?? string.Empty;
                Entrada.VA_TIT_45 = validacionActividad.Va45 ?? string.Empty;
                Entrada.VA_TIT_46 = validacionActividad.Va46 ?? string.Empty;
                Entrada.VA_TIT_47 = validacionActividad.Va47 ?? string.Empty;
                Entrada.VA_TIT_48 = validacionActividad.Va48 ?? string.Empty;
                Entrada.VA_TIT_49 = validacionActividad.Va49 ?? string.Empty;
                Entrada.VA_TIT_50 = validacionActividad.Va50 ?? string.Empty;
            }

            TablonIndividuos tablonIndividuos;
            if (fuentes.TablonIndividuos.TryGetValue(integrantes.Titular?.NumeroTributario.ToString() ?? "", out tablonIndividuos))
            {
                Entrada.TIND_TIT_ingreso_calculado_final = tablonIndividuos.IngresoCalculadoFinal ?? decimal.Zero;
                Entrada.TIND_TIT_tipo_ingreso = tablonIndividuos.TipoIngreso ?? string.Empty;
            }

            //Cotitu 1
            if (fuentes.EndeudamientoAtraso.TryGetValue(integrantes.PorIndice(1)?.Cobis ?? -1, out endeudamientoAtraso))
            {
                Entrada.EA_COT1_codigo_cliente = endeudamientoAtraso.CodigoCliente ?? 0;
                Entrada.EA_COT1_fecha_carga = endeudamientoAtraso.FechaCarga ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT1_fecha_cabalope = endeudamientoAtraso.FechaCabalope ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT1_fecha_consulta_reestructuracion = endeudamientoAtraso.FechaConsultaReestructuracion ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT1_motivo_reestructuracion = endeudamientoAtraso.MotivoReestructuracion ?? string.Empty;
                Entrada.EA_COT1_fecha_consulta_refi_deu = endeudamientoAtraso.FechaConsultaRefiDeu ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT1_motivo_refinanciacion = endeudamientoAtraso.MotivoRefinanciacion ?? string.Empty;
                Entrada.EA_COT1_motivo_deu = endeudamientoAtraso.MotivoDeu ?? string.Empty;
            }

            if (fuentes.ClientesDepurados.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario.ToString() ?? "", out clientesDepurados))
            {
                Entrada.CND_COT1_cobis = (int)Herramientas.ConvertirDeTexto(clientesDepurados.CodigoCliente, DbType.Int32, 0);
                Entrada.CND_COT1_nro_tributario = clientesDepurados.Cuil ?? string.Empty;
                Entrada.CND_COT1_codigo_campana = clientesDepurados.Depuracion ?? string.Empty;
                Entrada.CND_COT1_vencimiento = DateOnly.FromDateTime(clientesDepurados.DepuracionHasta ?? new DateTime(1900, 1, 1));
                Entrada.CND_COT1_motivo_rechazo = clientesDepurados.DepuracionMotivo ?? string.Empty;
            }

            if (fuentes.Behavior.TryGetValue(integrantes.PorIndice(1)?.Cobis ?? -1, out behavior))
            {
                Entrada.BH_COT1_id_cobis = behavior.ClienteCobis ?? 0;
                Entrada.BH_COT1_riesgo_score = behavior.RiesgoScore ?? 0;
                Entrada.BH_COT1_riesgo_6 = behavior.Riesgo6 ?? 0;
                Entrada.BH_COT1_uso_tc_limite = behavior.UsoTcLimite ?? decimal.Zero;
                Entrada.BH_COT1_uso_tc_exigible = behavior.UsoTcExigible ?? decimal.Zero;
                Entrada.BH_COT1_uso_tc_limite_max = behavior.UsoTcLimiteMax ?? decimal.Zero;
                Entrada.BH_COT1_uso_cc_limite = behavior.UsoCcLimite ?? decimal.Zero;
                Entrada.BH_COT1_uso_cc_promedio3 = behavior.UsoCcPromedio3 ?? decimal.Zero;
                Entrada.BH_COT1_uso_sg_exigible = behavior.UsoSgExigible ?? decimal.Zero;
                Entrada.BH_COT1_uso_gp_exigible = behavior.UsoGpExigible ?? decimal.Zero;
                Entrada.BH_COT1_uso_gh_exigible = behavior.UsoGhExigible ?? decimal.Zero;
                Entrada.BH_COT1_antiguedad_tc = behavior.AntiguedadTc ?? 0;
                Entrada.BH_COT1_antiguedad_cc = behavior.AntiguedadCc ?? 0;
                Entrada.BH_COT1_cob_status = behavior.CobStatus ?? 0;
                Entrada.BH_COT1_hit = behavior.Hit ?? -1;
                Entrada.BH_COT1_uso_tc_saldo_fin = behavior.VUsoTcSaldoFin ?? decimal.Zero;
                Entrada.BH_COT1_ip2 = behavior.Ip2 ?? string.Empty;
                Entrada.BH_COT1_consultar_veraz = behavior.ConsultarVeraz ?? string.Empty;
            }

            if (fuentes.ChequesRechazados.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario ?? -1, out chequesRechazados))
            {
                Entrada.CHEQ_COT1_rech_lev_sf_u24m_cant = chequesRechazados.RechLevSFu24mCant ?? 0;
                Entrada.CHEQ_COT1_rech_nolev_sf_u24m_monto = chequesRechazados.RechNoLevSFu24mMonto ?? decimal.Zero;
                Entrada.CHEQ_COT1_rech_lev_sf_u24m_monto = chequesRechazados.RechLevSFu24mMonto ?? decimal.Zero;
            }

            if (fuentes.ScoreComportamiento.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario ?? -1, out scoreComportamiento))
            {
                Entrada.SC_COT1_exclusion = scoreComportamiento.exclusion ?? string.Empty;
                Entrada.SC_COT1_score_comp = (int)(scoreComportamiento.score ?? decimal.Zero);
                Entrada.SC_COT1_categoria = scoreComportamiento.categoria ?? string.Empty;
            }

            if (fuentes.Tablon.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario ?? -1, out tablon))
            {
                Entrada.POL_COT1_flag_acuerdo_cc = tablon.POL_Flag_Acuerdo_CC ?? 0;
                Entrada.POL_COT1_flag_producto_crediticio_activo = tablon.FlagProductoCrediticioActivo ?? 0;
                Entrada.POL_COT1_antiguedad_tc_macro = tablon.AntiguedadTc ?? decimal.Zero;
                Entrada.POL_COT1_antiguedad_cc_macro = tablon.AntiguedadCc ?? decimal.Zero;
                Entrada.POL_COT1_cantidad_ocurrencia_6 = tablon.CantidadOcurrencia6 ?? 0;
            }

            if (fuentes.Excepciones.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario.ToString() ?? "", out excepciones))
            {
                Entrada.EX_COT1_fin_excepcion_vz = DateOnly.FromDateTime(excepciones.FechaVencimiento ?? new DateTime(1900, 1, 1));
            }

            if (fuentes.ValidacionActividad.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario ?? -1, out validacionActividad))
            {
                Entrada.VA_COT1_cuit = validacionActividad.Cuit ?? 0;
                Entrada.VA_COT1_nombre_pe = validacionActividad.NombrePe ?? string.Empty;
                Entrada.VA_COT1_apellido_pe = validacionActividad.ApellidoPe ?? string.Empty;
                Entrada.VA_COT1_sexo_pe = validacionActividad.SexoPe ?? string.Empty;
                Entrada.VA_COT1_fecha_nacimiento_pe = DateOnly.FromDateTime(validacionActividad.FechaNacimientoPe ?? new DateTime(1900, 1, 1));
                Entrada.VA_COT1_provincia_pe = validacionActividad.ProvinciaPe ?? string.Empty;
                Entrada.VA_COT1_codigo_postal_pe = validacionActividad.CodigoPostalPe ?? string.Empty;
                Entrada.VA_COT1_fecha_carga = DateOnly.FromDateTime(validacionActividad.FechaCarga ?? new DateTime(1900, 1, 1));
                Entrada.VA_COT1_periodo_score = validacionActividad.PeriodoScore ?? 0;
                Entrada.VA_COT1_empleador = validacionActividad.Empleador ?? 0;
                Entrada.VA_COT1_flag_crediticio = validacionActividad.FlagCrediticio ?? 0;
                Entrada.VA_COT1_sum_score = validacionActividad.SumScore ?? decimal.Zero;
                Entrada.VA_COT1_razon_social_afip = validacionActividad.RazonSocialAfip ?? string.Empty;
                Entrada.VA_COT1_impuesto_ganancias = validacionActividad.ImpuestoGanancias ?? string.Empty;
                Entrada.VA_COT1_impuesto_iva = validacionActividad.ImpuestoIva ?? string.Empty;
                Entrada.VA_COT1_integrante_sociedad = validacionActividad.IntegranteSociedad ?? string.Empty;
                Entrada.VA_COT1_es_empleador_afip = validacionActividad.EsEmpleadorAfip ?? string.Empty;
                Entrada.VA_COT1_cat_monotributo = validacionActividad.CatMonotributo ?? string.Empty;
                Entrada.VA_COT1_actividad_monotributo = validacionActividad.ActividadMonotributo ?? string.Empty;
                Entrada.VA_COT1_actividad_monotributo_descripcion = validacionActividad.ActividadMonotributoDescripcion ?? string.Empty;
                Entrada.VA_COT1_fact_monotributo = validacionActividad.FactMonotributo ?? decimal.Zero;
                Entrada.VA_COT1_ingreso_servicios = validacionActividad.IngresoServicios ?? decimal.Zero;
                Entrada.VA_COT1_ingreso_ventas = validacionActividad.IngresoVentas ?? decimal.Zero;
                Entrada.VA_COT1_ingreso_monotributo = validacionActividad.IngresoMonotributo ?? decimal.Zero;
                Entrada.VA_COT1_es_empleado = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_COT1_es_autonomo = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_COT1_es_monotributista = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_COT1_es_jubilado = validacionActividad.EsJubilado ?? string.Empty;
                Entrada.VA_COT1_ingreso_transaccionalidad = validacionActividad.IngresoTransaccionalidad ?? decimal.Zero;
                Entrada.VA_COT1_opcional_1 = validacionActividad.Opcional1 ?? string.Empty;
                Entrada.VA_COT1_1 = validacionActividad.Va1 ?? string.Empty;
                Entrada.VA_COT1_2 = validacionActividad.Va2 ?? string.Empty;
                Entrada.VA_COT1_3 = validacionActividad.Va3 ?? string.Empty;
                Entrada.VA_COT1_4 = validacionActividad.Va4 ?? string.Empty;
                Entrada.VA_COT1_5 = validacionActividad.Va5 ?? string.Empty;
                Entrada.VA_COT1_6 = validacionActividad.Va6 ?? string.Empty;
                Entrada.VA_COT1_7 = validacionActividad.Va7 ?? string.Empty;
                Entrada.VA_COT1_8 = validacionActividad.Va8 ?? string.Empty;
                Entrada.VA_COT1_9 = validacionActividad.Va9 ?? string.Empty;
                Entrada.VA_COT1_10 = validacionActividad.Va10 ?? string.Empty;
                Entrada.VA_COT1_11 = validacionActividad.Va11 ?? string.Empty;
                Entrada.VA_COT1_12 = validacionActividad.Va12 ?? string.Empty;
                Entrada.VA_COT1_13 = validacionActividad.Va13 ?? string.Empty;
                Entrada.VA_COT1_14 = validacionActividad.Va14 ?? string.Empty;
                Entrada.VA_COT1_15 = validacionActividad.Va15 ?? string.Empty;
                Entrada.VA_COT1_16 = validacionActividad.Va16 ?? string.Empty;
                Entrada.VA_COT1_17 = validacionActividad.Va17 ?? string.Empty;
                Entrada.VA_COT1_18 = validacionActividad.Va18 ?? string.Empty;
                Entrada.VA_COT1_19 = validacionActividad.Va19 ?? string.Empty;
                Entrada.VA_COT1_20 = validacionActividad.Va20 ?? string.Empty;
                Entrada.VA_COT1_21 = validacionActividad.Va21 ?? string.Empty;
                Entrada.VA_COT1_22 = validacionActividad.Va22 ?? string.Empty;
                Entrada.VA_COT1_23 = validacionActividad.Va23 ?? string.Empty;
                Entrada.VA_COT1_24 = validacionActividad.Va24 ?? string.Empty;
                Entrada.VA_COT1_25 = validacionActividad.Va25 ?? string.Empty;
                Entrada.VA_COT1_26 = validacionActividad.Va26 ?? string.Empty;
                Entrada.VA_COT1_27 = validacionActividad.Va27 ?? string.Empty;
                Entrada.VA_COT1_28 = validacionActividad.Va28 ?? string.Empty;
                Entrada.VA_COT1_29 = validacionActividad.Va29 ?? string.Empty;
                Entrada.VA_COT1_30 = validacionActividad.Va30 ?? string.Empty;
                Entrada.VA_COT1_31 = validacionActividad.Va31 ?? string.Empty;
                Entrada.VA_COT1_32 = validacionActividad.Va32 ?? string.Empty;
                Entrada.VA_COT1_33 = validacionActividad.Va33 ?? string.Empty;
                Entrada.VA_COT1_34 = validacionActividad.Va34 ?? string.Empty;
                Entrada.VA_COT1_35 = validacionActividad.Va35 ?? string.Empty;
                Entrada.VA_COT1_36 = validacionActividad.Va36 ?? string.Empty;
                Entrada.VA_COT1_37 = validacionActividad.Va37 ?? string.Empty;
                Entrada.VA_COT1_38 = validacionActividad.Va38 ?? string.Empty;
                Entrada.VA_COT1_39 = validacionActividad.Va39 ?? string.Empty;
                Entrada.VA_COT1_40 = validacionActividad.Va40 ?? string.Empty;
                Entrada.VA_COT1_41 = validacionActividad.Va41 ?? string.Empty;
                Entrada.VA_COT1_42 = validacionActividad.Va42 ?? string.Empty;
                Entrada.VA_COT1_43 = validacionActividad.Va43 ?? string.Empty;
                Entrada.VA_COT1_44 = validacionActividad.Va44 ?? string.Empty;
                Entrada.VA_COT1_45 = validacionActividad.Va45 ?? string.Empty;
                Entrada.VA_COT1_46 = validacionActividad.Va46 ?? string.Empty;
                Entrada.VA_COT1_47 = validacionActividad.Va47 ?? string.Empty;
                Entrada.VA_COT1_48 = validacionActividad.Va48 ?? string.Empty;
                Entrada.VA_COT1_49 = validacionActividad.Va49 ?? string.Empty;
                Entrada.VA_COT1_50 = validacionActividad.Va50 ?? string.Empty;
            }

            if (fuentes.TablonIndividuos.TryGetValue(integrantes.PorIndice(1)?.NumeroTributario.ToString() ?? "", out tablonIndividuos))
            {
                Entrada.TIND_COT1_ingreso_calculado_final = tablonIndividuos.IngresoCalculadoFinal ?? decimal.Zero;
                Entrada.TIND_COT1_tipo_ingreso = tablonIndividuos.TipoIngreso ?? string.Empty;
            }

            // Cotitu 2
            if (fuentes.EndeudamientoAtraso.TryGetValue(integrantes.PorIndice(2)?.Cobis ?? -1, out endeudamientoAtraso))
            {
                Entrada.EA_COT2_codigo_cliente = endeudamientoAtraso.CodigoCliente ?? 0;
                Entrada.EA_COT2_fecha_carga = endeudamientoAtraso.FechaCarga ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT2_fecha_cabalope = endeudamientoAtraso.FechaCabalope ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT2_fecha_consulta_reestructuracion = endeudamientoAtraso.FechaConsultaReestructuracion ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT2_motivo_reestructuracion = endeudamientoAtraso.MotivoReestructuracion ?? string.Empty;
                Entrada.EA_COT2_fecha_consulta_refi_deu = endeudamientoAtraso.FechaConsultaRefiDeu ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT2_motivo_refinanciacion = endeudamientoAtraso.MotivoRefinanciacion ?? string.Empty;
                Entrada.EA_COT2_motivo_deu = endeudamientoAtraso.MotivoDeu ?? string.Empty;
            }

            if (fuentes.ClientesDepurados.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario.ToString() ?? "", out clientesDepurados))
            {
                Entrada.CND_COT2_cobis = (int)Herramientas.ConvertirDeTexto(clientesDepurados.CodigoCliente, DbType.Int32, 0);
                Entrada.CND_COT2_nro_tributario = clientesDepurados.Cuil ?? string.Empty;
                Entrada.CND_COT2_codigo_campana = clientesDepurados.Depuracion ?? string.Empty;
                Entrada.CND_COT2_vencimiento = DateOnly.FromDateTime(clientesDepurados.DepuracionHasta ?? new DateTime(1900, 1, 1));
                Entrada.CND_COT2_motivo_rechazo = clientesDepurados.DepuracionMotivo ?? string.Empty;
            }

            if (fuentes.Behavior.TryGetValue(integrantes.PorIndice(2)?.Cobis ?? -1, out behavior))
            {
                Entrada.BH_COT2_id_cobis = behavior.ClienteCobis ?? 0;
                Entrada.BH_COT2_riesgo_score = behavior.RiesgoScore ?? 0;
                Entrada.BH_COT2_riesgo_6 = behavior.Riesgo6 ?? 0;
                Entrada.BH_COT2_uso_tc_limite = behavior.UsoTcLimite ?? decimal.Zero;
                Entrada.BH_COT2_uso_tc_exigible = behavior.UsoTcExigible ?? decimal.Zero;
                Entrada.BH_COT2_uso_tc_limite_max = behavior.UsoTcLimiteMax ?? decimal.Zero;
                Entrada.BH_COT2_uso_cc_limite = behavior.UsoCcLimite ?? decimal.Zero;
                Entrada.BH_COT2_uso_cc_promedio3 = behavior.UsoCcPromedio3 ?? decimal.Zero;
                Entrada.BH_COT2_uso_sg_exigible = behavior.UsoSgExigible ?? decimal.Zero;
                Entrada.BH_COT2_uso_gp_exigible = behavior.UsoGpExigible ?? decimal.Zero;
                Entrada.BH_COT2_uso_gh_exigible = behavior.UsoGhExigible ?? decimal.Zero;
                Entrada.BH_COT2_antiguedad_tc = behavior.AntiguedadTc ?? 0;
                Entrada.BH_COT2_antiguedad_cc = behavior.AntiguedadCc ?? 0;
                Entrada.BH_COT2_cob_status = behavior.CobStatus ?? 0;
                Entrada.BH_COT2_hit = behavior.Hit ?? -1;
                Entrada.BH_COT2_uso_tc_saldo_fin = behavior.VUsoTcSaldoFin ?? decimal.Zero;
                Entrada.BH_COT2_ip2 = behavior.Ip2 ?? string.Empty;
                Entrada.BH_COT2_consultar_veraz = behavior.ConsultarVeraz ?? string.Empty;
            }

            if (fuentes.ChequesRechazados.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario ?? -1, out chequesRechazados))
            {
                Entrada.CHEQ_COT2_rech_lev_sf_u24m_cant = chequesRechazados.RechLevSFu24mCant ?? 0;
                Entrada.CHEQ_COT2_rech_nolev_sf_u24m_monto = chequesRechazados.RechNoLevSFu24mMonto ?? decimal.Zero;
                Entrada.CHEQ_COT2_rech_lev_sf_u24m_monto = chequesRechazados.RechLevSFu24mMonto ?? decimal.Zero;
            }

            if (fuentes.ScoreComportamiento.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario ?? -1, out scoreComportamiento))
            {
                Entrada.SC_COT2_exclusion = scoreComportamiento.exclusion ?? string.Empty;
                Entrada.SC_COT2_score_comp = (int)(scoreComportamiento.score ?? decimal.Zero);
                Entrada.SC_COT2_categoria = scoreComportamiento.categoria ?? string.Empty;
            }

            if (fuentes.Tablon.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario ?? -1, out tablon))
            {
                Entrada.POL_COT2_flag_acuerdo_cc = tablon.POL_Flag_Acuerdo_CC ?? 0;
                Entrada.POL_COT2_flag_producto_crediticio_activo = tablon.FlagProductoCrediticioActivo ?? 0;
                Entrada.POL_COT2_antiguedad_tc_macro = tablon.AntiguedadTc ?? decimal.Zero;
                Entrada.POL_COT2_antiguedad_cc_macro = tablon.AntiguedadCc ?? decimal.Zero;
                Entrada.POL_COT2_cantidad_ocurrencia_6 = tablon.CantidadOcurrencia6 ?? 0;
            }

            if (fuentes.Excepciones.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario.ToString() ?? "", out excepciones))
            {
                Entrada.EX_COT2_fin_excepcion_vz = DateOnly.FromDateTime(excepciones.FechaVencimiento ?? new DateTime(1900, 1, 1));
            }

            if (fuentes.ValidacionActividad.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario ?? -1, out validacionActividad))
            {
                Entrada.VA_COT2_cuit = validacionActividad.Cuit ?? 0;
                Entrada.VA_COT2_nombre_pe = validacionActividad.NombrePe ?? string.Empty;
                Entrada.VA_COT2_apellido_pe = validacionActividad.ApellidoPe ?? string.Empty;
                Entrada.VA_COT2_sexo_pe = validacionActividad.SexoPe ?? string.Empty;
                Entrada.VA_COT2_fecha_nacimiento_pe = DateOnly.FromDateTime(validacionActividad.FechaNacimientoPe ?? new DateTime(1900, 1, 1));
                Entrada.VA_COT2_provincia_pe = validacionActividad.ProvinciaPe ?? string.Empty;
                Entrada.VA_COT2_codigo_postal_pe = validacionActividad.CodigoPostalPe ?? string.Empty;
                Entrada.VA_COT2_fecha_carga = DateOnly.FromDateTime(validacionActividad.FechaCarga ?? new DateTime(1900, 1, 1));
                Entrada.VA_COT2_periodo_score = validacionActividad.PeriodoScore ?? 0;
                Entrada.VA_COT2_empleador = validacionActividad.Empleador ?? 0;
                Entrada.VA_COT2_flag_crediticio = validacionActividad.FlagCrediticio ?? 0;
                Entrada.VA_COT2_sum_score = validacionActividad.SumScore ?? decimal.Zero;
                Entrada.VA_COT2_razon_social_afip = validacionActividad.RazonSocialAfip ?? string.Empty;
                Entrada.VA_COT2_impuesto_ganancias = validacionActividad.ImpuestoGanancias ?? string.Empty;
                Entrada.VA_COT2_impuesto_iva = validacionActividad.ImpuestoIva ?? string.Empty;
                Entrada.VA_COT2_integrante_sociedad = validacionActividad.IntegranteSociedad ?? string.Empty;
                Entrada.VA_COT2_es_empleador_afip = validacionActividad.EsEmpleadorAfip ?? string.Empty;
                Entrada.VA_COT2_cat_monotributo = validacionActividad.CatMonotributo ?? string.Empty;
                Entrada.VA_COT2_actividad_monotributo = validacionActividad.ActividadMonotributo ?? string.Empty;
                Entrada.VA_COT2_actividad_monotributo_descripcion = validacionActividad.ActividadMonotributoDescripcion ?? string.Empty;
                Entrada.VA_COT2_fact_monotributo = validacionActividad.FactMonotributo ?? decimal.Zero;
                Entrada.VA_COT2_ingreso_servicios = validacionActividad.IngresoServicios ?? decimal.Zero;
                Entrada.VA_COT2_ingreso_ventas = validacionActividad.IngresoVentas ?? decimal.Zero;
                Entrada.VA_COT2_ingreso_monotributo = validacionActividad.IngresoMonotributo ?? decimal.Zero;
                Entrada.VA_COT2_es_empleado = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_COT2_es_autonomo = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_COT2_es_monotributista = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_COT2_es_jubilado = validacionActividad.EsJubilado ?? string.Empty;
                Entrada.VA_COT2_ingreso_transaccionalidad = validacionActividad.IngresoTransaccionalidad ?? decimal.Zero;
                Entrada.VA_COT2_opcional_1 = validacionActividad.Opcional1 ?? string.Empty;
                Entrada.VA_COT2_1 = validacionActividad.Va1 ?? string.Empty;
                Entrada.VA_COT2_2 = validacionActividad.Va2 ?? string.Empty;
                Entrada.VA_COT2_3 = validacionActividad.Va3 ?? string.Empty;
                Entrada.VA_COT2_4 = validacionActividad.Va4 ?? string.Empty;
                Entrada.VA_COT2_5 = validacionActividad.Va5 ?? string.Empty;
                Entrada.VA_COT2_6 = validacionActividad.Va6 ?? string.Empty;
                Entrada.VA_COT2_7 = validacionActividad.Va7 ?? string.Empty;
                Entrada.VA_COT2_8 = validacionActividad.Va8 ?? string.Empty;
                Entrada.VA_COT2_9 = validacionActividad.Va9 ?? string.Empty;
                Entrada.VA_COT2_10 = validacionActividad.Va10 ?? string.Empty;
                Entrada.VA_COT2_11 = validacionActividad.Va11 ?? string.Empty;
                Entrada.VA_COT2_12 = validacionActividad.Va12 ?? string.Empty;
                Entrada.VA_COT2_13 = validacionActividad.Va13 ?? string.Empty;
                Entrada.VA_COT2_14 = validacionActividad.Va14 ?? string.Empty;
                Entrada.VA_COT2_15 = validacionActividad.Va15 ?? string.Empty;
                Entrada.VA_COT2_16 = validacionActividad.Va16 ?? string.Empty;
                Entrada.VA_COT2_17 = validacionActividad.Va17 ?? string.Empty;
                Entrada.VA_COT2_18 = validacionActividad.Va18 ?? string.Empty;
                Entrada.VA_COT2_19 = validacionActividad.Va19 ?? string.Empty;
                Entrada.VA_COT2_20 = validacionActividad.Va20 ?? string.Empty;
                Entrada.VA_COT2_21 = validacionActividad.Va21 ?? string.Empty;
                Entrada.VA_COT2_22 = validacionActividad.Va22 ?? string.Empty;
                Entrada.VA_COT2_23 = validacionActividad.Va23 ?? string.Empty;
                Entrada.VA_COT2_24 = validacionActividad.Va24 ?? string.Empty;
                Entrada.VA_COT2_25 = validacionActividad.Va25 ?? string.Empty;
                Entrada.VA_COT2_26 = validacionActividad.Va26 ?? string.Empty;
                Entrada.VA_COT2_27 = validacionActividad.Va27 ?? string.Empty;
                Entrada.VA_COT2_28 = validacionActividad.Va28 ?? string.Empty;
                Entrada.VA_COT2_29 = validacionActividad.Va29 ?? string.Empty;
                Entrada.VA_COT2_30 = validacionActividad.Va30 ?? string.Empty;
                Entrada.VA_COT2_31 = validacionActividad.Va31 ?? string.Empty;
                Entrada.VA_COT2_32 = validacionActividad.Va32 ?? string.Empty;
                Entrada.VA_COT2_33 = validacionActividad.Va33 ?? string.Empty;
                Entrada.VA_COT2_34 = validacionActividad.Va34 ?? string.Empty;
                Entrada.VA_COT2_35 = validacionActividad.Va35 ?? string.Empty;
                Entrada.VA_COT2_36 = validacionActividad.Va36 ?? string.Empty;
                Entrada.VA_COT2_37 = validacionActividad.Va37 ?? string.Empty;
                Entrada.VA_COT2_38 = validacionActividad.Va38 ?? string.Empty;
                Entrada.VA_COT2_39 = validacionActividad.Va39 ?? string.Empty;
                Entrada.VA_COT2_40 = validacionActividad.Va40 ?? string.Empty;
                Entrada.VA_COT2_41 = validacionActividad.Va41 ?? string.Empty;
                Entrada.VA_COT2_42 = validacionActividad.Va42 ?? string.Empty;
                Entrada.VA_COT2_43 = validacionActividad.Va43 ?? string.Empty;
                Entrada.VA_COT2_44 = validacionActividad.Va44 ?? string.Empty;
                Entrada.VA_COT2_45 = validacionActividad.Va45 ?? string.Empty;
                Entrada.VA_COT2_46 = validacionActividad.Va46 ?? string.Empty;
                Entrada.VA_COT2_47 = validacionActividad.Va47 ?? string.Empty;
                Entrada.VA_COT2_48 = validacionActividad.Va48 ?? string.Empty;
                Entrada.VA_COT2_49 = validacionActividad.Va49 ?? string.Empty;
                Entrada.VA_COT2_50 = validacionActividad.Va50 ?? string.Empty;
            }

            if (fuentes.TablonIndividuos.TryGetValue(integrantes.PorIndice(2)?.NumeroTributario.ToString() ?? "", out tablonIndividuos))
            {
                Entrada.TIND_COT2_ingreso_calculado_final = tablonIndividuos.IngresoCalculadoFinal ?? decimal.Zero;
                Entrada.TIND_COT2_tipo_ingreso = tablonIndividuos.TipoIngreso ?? string.Empty;
            }

            //Cotitu 3
            if (fuentes.EndeudamientoAtraso.TryGetValue(integrantes.PorIndice(3)?.Cobis ?? -1, out endeudamientoAtraso))
            {
                Entrada.EA_COT3_codigo_cliente = endeudamientoAtraso.CodigoCliente ?? 0;
                Entrada.EA_COT3_fecha_carga = endeudamientoAtraso.FechaCarga ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT3_fecha_cabalope = endeudamientoAtraso.FechaCabalope ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT3_fecha_consulta_reestructuracion = endeudamientoAtraso.FechaConsultaReestructuracion ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT3_motivo_reestructuracion = endeudamientoAtraso.MotivoReestructuracion ?? string.Empty;
                Entrada.EA_COT3_fecha_consulta_refi_deu = endeudamientoAtraso.FechaConsultaRefiDeu ?? new DateOnly(1900, 1, 1);
                Entrada.EA_COT3_motivo_refinanciacion = endeudamientoAtraso.MotivoRefinanciacion ?? string.Empty;
                Entrada.EA_COT3_motivo_deu = endeudamientoAtraso.MotivoDeu ?? string.Empty;
            }

            if (fuentes.ClientesDepurados.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario.ToString() ?? "", out clientesDepurados))
            {
                Entrada.CND_COT3_cobis = (int)Herramientas.ConvertirDeTexto(clientesDepurados.CodigoCliente, DbType.Int32, 0);
                Entrada.CND_COT3_nro_tributario = clientesDepurados.Cuil ?? string.Empty;
                Entrada.CND_COT3_codigo_campana = clientesDepurados.Depuracion ?? string.Empty;
                Entrada.CND_COT3_vencimiento = DateOnly.FromDateTime(clientesDepurados.DepuracionHasta ?? new DateTime(1900, 1, 1));
                Entrada.CND_COT3_motivo_rechazo = clientesDepurados.DepuracionMotivo ?? string.Empty;
            }

            if (fuentes.Behavior.TryGetValue(integrantes.PorIndice(3)?.Cobis ?? -1, out behavior))
            {
                Entrada.BH_COT3_id_cobis = behavior.ClienteCobis ?? 0;
                Entrada.BH_COT3_riesgo_score = behavior.RiesgoScore ?? 0;
                Entrada.BH_COT3_riesgo_6 = behavior.Riesgo6 ?? 0;
                Entrada.BH_COT3_uso_tc_limite = behavior.UsoTcLimite ?? decimal.Zero;
                Entrada.BH_COT3_uso_tc_exigible = behavior.UsoTcExigible ?? decimal.Zero;
                Entrada.BH_COT3_uso_tc_limite_max = behavior.UsoTcLimiteMax ?? decimal.Zero;
                Entrada.BH_COT3_uso_cc_limite = behavior.UsoCcLimite ?? decimal.Zero;
                Entrada.BH_COT3_uso_cc_promedio3 = behavior.UsoCcPromedio3 ?? decimal.Zero;
                Entrada.BH_COT3_uso_sg_exigible = behavior.UsoSgExigible ?? decimal.Zero;
                Entrada.BH_COT3_uso_gp_exigible = behavior.UsoGpExigible ?? decimal.Zero;
                Entrada.BH_COT3_uso_gh_exigible = behavior.UsoGhExigible ?? decimal.Zero;
                Entrada.BH_COT3_antiguedad_tc = behavior.AntiguedadTc ?? 0;
                Entrada.BH_COT3_antiguedad_cc = behavior.AntiguedadCc ?? 0;
                Entrada.BH_COT3_cob_status = behavior.CobStatus ?? 0;
                Entrada.BH_COT3_hit = behavior.Hit ?? -1;
                Entrada.BH_COT3_uso_tc_saldo_fin = behavior.VUsoTcSaldoFin ?? decimal.Zero;
                Entrada.BH_COT3_ip2 = behavior.Ip2 ?? string.Empty;
                Entrada.BH_COT3_consultar_veraz = behavior.ConsultarVeraz ?? string.Empty;
            }

            if (fuentes.ChequesRechazados.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario ?? -1, out chequesRechazados))
            {
                Entrada.CHEQ_COT3_rech_lev_sf_u24m_cant = chequesRechazados.RechLevSFu24mCant ?? 0;
                Entrada.CHEQ_COT3_rech_nolev_sf_u24m_monto = chequesRechazados.RechNoLevSFu24mMonto ?? decimal.Zero;
                Entrada.CHEQ_COT3_rech_lev_sf_u24m_monto = chequesRechazados.RechLevSFu24mMonto ?? decimal.Zero;
            }

            if (fuentes.ScoreComportamiento.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario ?? -1, out scoreComportamiento))
            {
                Entrada.SC_COT3_exclusion = scoreComportamiento.exclusion ?? string.Empty;
                Entrada.SC_COT3_score_comp = (int)(scoreComportamiento.score ?? decimal.Zero);
                Entrada.SC_COT3_categoria = scoreComportamiento.categoria ?? string.Empty;
            }

            if (fuentes.Tablon.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario ?? -1, out tablon))
            {
                Entrada.POL_COT3_flag_acuerdo_cc = tablon.POL_Flag_Acuerdo_CC ?? 0;
                Entrada.POL_COT3_flag_producto_crediticio_activo = tablon.FlagProductoCrediticioActivo ?? 0;
                Entrada.POL_COT3_antiguedad_tc_macro = tablon.AntiguedadTc ?? decimal.Zero;
                Entrada.POL_COT3_antiguedad_cc_macro = tablon.AntiguedadCc ?? decimal.Zero;
                Entrada.POL_COT3_cantidad_ocurrencia_6 = tablon.CantidadOcurrencia6 ?? 0;
            }

            if (fuentes.Excepciones.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario.ToString() ?? "", out excepciones))
            {
                Entrada.EX_COT3_fin_excepcion_vz = DateOnly.FromDateTime(excepciones.FechaVencimiento ?? new DateTime(1900, 1, 1));
            }

            if (fuentes.ValidacionActividad.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario ?? -1, out validacionActividad))
            {
                Entrada.VA_COT3_cuit = validacionActividad.Cuit ?? 0;
                Entrada.VA_COT3_nombre_pe = validacionActividad.NombrePe ?? string.Empty;
                Entrada.VA_COT3_apellido_pe = validacionActividad.ApellidoPe ?? string.Empty;
                Entrada.VA_COT3_sexo_pe = validacionActividad.SexoPe ?? string.Empty;
                Entrada.VA_COT3_fecha_nacimiento_pe = DateOnly.FromDateTime(validacionActividad.FechaNacimientoPe ?? new DateTime(1900, 1, 1));
                Entrada.VA_COT3_provincia_pe = validacionActividad.ProvinciaPe ?? string.Empty;
                Entrada.VA_COT3_codigo_postal_pe = validacionActividad.CodigoPostalPe ?? string.Empty;
                Entrada.VA_COT3_fecha_carga = DateOnly.FromDateTime(validacionActividad.FechaCarga ?? new DateTime(1900, 1, 1));
                Entrada.VA_COT3_periodo_score = validacionActividad.PeriodoScore ?? 0;
                Entrada.VA_COT3_empleador = validacionActividad.Empleador ?? 0;
                Entrada.VA_COT3_flag_crediticio = validacionActividad.FlagCrediticio ?? 0;
                Entrada.VA_COT3_sum_score = validacionActividad.SumScore ?? decimal.Zero;
                Entrada.VA_COT3_razon_social_afip = validacionActividad.RazonSocialAfip ?? string.Empty;
                Entrada.VA_COT3_impuesto_ganancias = validacionActividad.ImpuestoGanancias ?? string.Empty;
                Entrada.VA_COT3_impuesto_iva = validacionActividad.ImpuestoIva ?? string.Empty;
                Entrada.VA_COT3_integrante_sociedad = validacionActividad.IntegranteSociedad ?? string.Empty;
                Entrada.VA_COT3_es_empleador_afip = validacionActividad.EsEmpleadorAfip ?? string.Empty;
                Entrada.VA_COT3_cat_monotributo = validacionActividad.CatMonotributo ?? string.Empty;
                Entrada.VA_COT3_actividad_monotributo = validacionActividad.ActividadMonotributo ?? string.Empty;
                Entrada.VA_COT3_actividad_monotributo_descripcion = validacionActividad.ActividadMonotributoDescripcion ?? string.Empty;
                Entrada.VA_COT3_fact_monotributo = validacionActividad.FactMonotributo ?? decimal.Zero;
                Entrada.VA_COT3_ingreso_servicios = validacionActividad.IngresoServicios ?? decimal.Zero;
                Entrada.VA_COT3_ingreso_ventas = validacionActividad.IngresoVentas ?? decimal.Zero;
                Entrada.VA_COT3_ingreso_monotributo = validacionActividad.IngresoMonotributo ?? decimal.Zero;
                Entrada.VA_COT3_es_empleado = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_COT3_es_autonomo = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_COT3_es_monotributista = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_COT3_es_jubilado = validacionActividad.EsJubilado ?? string.Empty;
                Entrada.VA_COT3_ingreso_transaccionalidad = validacionActividad.IngresoTransaccionalidad ?? decimal.Zero;
                Entrada.VA_COT3_opcional_1 = validacionActividad.Opcional1 ?? string.Empty;
                Entrada.VA_COT3_1 = validacionActividad.Va1 ?? string.Empty;
                Entrada.VA_COT3_2 = validacionActividad.Va2 ?? string.Empty;
                Entrada.VA_COT3_3 = validacionActividad.Va3 ?? string.Empty;
                Entrada.VA_COT3_4 = validacionActividad.Va4 ?? string.Empty;
                Entrada.VA_COT3_5 = validacionActividad.Va5 ?? string.Empty;
                Entrada.VA_COT3_6 = validacionActividad.Va6 ?? string.Empty;
                Entrada.VA_COT3_7 = validacionActividad.Va7 ?? string.Empty;
                Entrada.VA_COT3_8 = validacionActividad.Va8 ?? string.Empty;
                Entrada.VA_COT3_9 = validacionActividad.Va9 ?? string.Empty;
                Entrada.VA_COT3_10 = validacionActividad.Va10 ?? string.Empty;
                Entrada.VA_COT3_11 = validacionActividad.Va11 ?? string.Empty;
                Entrada.VA_COT3_12 = validacionActividad.Va12 ?? string.Empty;
                Entrada.VA_COT3_13 = validacionActividad.Va13 ?? string.Empty;
                Entrada.VA_COT3_14 = validacionActividad.Va14 ?? string.Empty;
                Entrada.VA_COT3_15 = validacionActividad.Va15 ?? string.Empty;
                Entrada.VA_COT3_16 = validacionActividad.Va16 ?? string.Empty;
                Entrada.VA_COT3_17 = validacionActividad.Va17 ?? string.Empty;
                Entrada.VA_COT3_18 = validacionActividad.Va18 ?? string.Empty;
                Entrada.VA_COT3_19 = validacionActividad.Va19 ?? string.Empty;
                Entrada.VA_COT3_20 = validacionActividad.Va20 ?? string.Empty;
                Entrada.VA_COT3_21 = validacionActividad.Va21 ?? string.Empty;
                Entrada.VA_COT3_22 = validacionActividad.Va22 ?? string.Empty;
                Entrada.VA_COT3_23 = validacionActividad.Va23 ?? string.Empty;
                Entrada.VA_COT3_24 = validacionActividad.Va24 ?? string.Empty;
                Entrada.VA_COT3_25 = validacionActividad.Va25 ?? string.Empty;
                Entrada.VA_COT3_26 = validacionActividad.Va26 ?? string.Empty;
                Entrada.VA_COT3_27 = validacionActividad.Va27 ?? string.Empty;
                Entrada.VA_COT3_28 = validacionActividad.Va28 ?? string.Empty;
                Entrada.VA_COT3_29 = validacionActividad.Va29 ?? string.Empty;
                Entrada.VA_COT3_30 = validacionActividad.Va30 ?? string.Empty;
                Entrada.VA_COT3_31 = validacionActividad.Va31 ?? string.Empty;
                Entrada.VA_COT3_32 = validacionActividad.Va32 ?? string.Empty;
                Entrada.VA_COT3_33 = validacionActividad.Va33 ?? string.Empty;
                Entrada.VA_COT3_34 = validacionActividad.Va34 ?? string.Empty;
                Entrada.VA_COT3_35 = validacionActividad.Va35 ?? string.Empty;
                Entrada.VA_COT3_36 = validacionActividad.Va36 ?? string.Empty;
                Entrada.VA_COT3_37 = validacionActividad.Va37 ?? string.Empty;
                Entrada.VA_COT3_38 = validacionActividad.Va38 ?? string.Empty;
                Entrada.VA_COT3_39 = validacionActividad.Va39 ?? string.Empty;
                Entrada.VA_COT3_40 = validacionActividad.Va40 ?? string.Empty;
                Entrada.VA_COT3_41 = validacionActividad.Va41 ?? string.Empty;
                Entrada.VA_COT3_42 = validacionActividad.Va42 ?? string.Empty;
                Entrada.VA_COT3_43 = validacionActividad.Va43 ?? string.Empty;
                Entrada.VA_COT3_44 = validacionActividad.Va44 ?? string.Empty;
                Entrada.VA_COT3_45 = validacionActividad.Va45 ?? string.Empty;
                Entrada.VA_COT3_46 = validacionActividad.Va46 ?? string.Empty;
                Entrada.VA_COT3_47 = validacionActividad.Va47 ?? string.Empty;
                Entrada.VA_COT3_48 = validacionActividad.Va48 ?? string.Empty;
                Entrada.VA_COT3_49 = validacionActividad.Va49 ?? string.Empty;
                Entrada.VA_COT3_50 = validacionActividad.Va50 ?? string.Empty;
            }

            if (fuentes.TablonIndividuos.TryGetValue(integrantes.PorIndice(3)?.NumeroTributario.ToString() ?? "", out tablonIndividuos))
            {
                Entrada.TIND_COT3_ingreso_calculado_final = tablonIndividuos.IngresoCalculadoFinal ?? decimal.Zero;
                Entrada.TIND_COT3_tipo_ingreso = tablonIndividuos.TipoIngreso ?? string.Empty;
            }

            if (fuentes.CalificacionConvenios.Count > 0)
            {
                var calificacionConvenio = fuentes.CalificacionConvenios.FirstOrDefault().Value;
                Entrada.DM_calificacion = calificacionConvenio.Calificacion ?? string.Empty;
                Entrada.DM_nombre_convenio = calificacionConvenio.Convenio ?? string.Empty;
                Entrada.DM_nro_convenio = calificacionConvenio.NumeroConvenio ?? string.Empty;
            }

            if (fuentes.ConveniosCodDescuento.Count > 0)
            {
                var conveniosCodDescuento = fuentes.ConveniosCodDescuento.FirstOrDefault().Value;
                Entrada.CCD_estado_convenio = conveniosCodDescuento.Estado ?? string.Empty;
                Entrada.CCD_fecha_ult_actualizacion = DateOnly.FromDateTime(conveniosCodDescuento.FechaUltActualizacion ?? new DateTime(1900, 1, 1));
                Entrada.CCD_calificacion_permanente = conveniosCodDescuento.CalificacionPermanente ?? string.Empty;
                Entrada.CCD_calificacion_otra_planta = conveniosCodDescuento.CalificacionOtraPlanta ?? string.Empty;
                Entrada.CCD_afectacion_pp_permanente = (decimal)(conveniosCodDescuento.AfectacionPpPermanente ?? 0d);
                Entrada.CCD_afectacion_pp_otra_planta = (decimal)(conveniosCodDescuento.AfectacionPpOtraPlanta ?? 0d);
                Entrada.CCD_afectacion_ph_permanente = (decimal)(conveniosCodDescuento.AfectacionPhPermanente ?? 0d);
                Entrada.CCD_afectacion_ph_otra_planta = (decimal)(conveniosCodDescuento.AfectacionPhOtraPlanta ?? 0d);
                Entrada.CCD_monto_maximo_permanente = (decimal)(conveniosCodDescuento.MontoMaximoPermanente ?? 0d);
                Entrada.CCD_monto_maximo_otra_planta = (decimal)(conveniosCodDescuento.MontoMaximoOtraPlanta ?? 0d);
                Entrada.CCD_cant_sueldos_maximo_permanente = (decimal)(conveniosCodDescuento.CantsueldosMaximoPermanente ?? 0d);
                Entrada.CCD_cant_sueldos_maximo_otra_planta = (decimal)(conveniosCodDescuento.CantsueldosMaximoOtraPlanta ?? 0d);
                Entrada.CCD_plazo_permanente = conveniosCodDescuento.PlazoPermanente ?? 0;
                Entrada.CCD_plazo_otra_planta = conveniosCodDescuento.PlazoOtraPlanta ?? 0;
                Entrada.CCD_max_endeu_global_permanente = (decimal)(conveniosCodDescuento.MaxEndeuGlobalPermanente ?? 0d);
                Entrada.CCD_max_endeu_global_otra_planta = (decimal)(conveniosCodDescuento.MaxEndeuGlobalOtraPlanta ?? 0d);
                Entrada.CCD_opcional1_permanente = (decimal)(conveniosCodDescuento.Opcional1Permanente ?? 0d);
                Entrada.CCD_opcional1_otra_planta = (decimal)(conveniosCodDescuento.Opcional1OtraPlanta ?? 0d);
                Entrada.CCD_opcional2_permanente = (decimal)(conveniosCodDescuento.Opcional2Permanente ?? 0d);
                Entrada.CCD_opcional2_otra_planta = (decimal)(conveniosCodDescuento.Opcional2OtraPlanta ?? 0d);
            }
        }
    }

    public class PoliticaPlanSueldoPublicoEntrada
    {
        public string id_oportunidad { get; set; } = string.Empty;
        public string nombre_oportunidad { get; set; } = string.Empty;
        public string id_evaluacion { get; set; } = string.Empty;
        public string ap_origen { get; set; } = string.Empty;
        public string nro_llamado { get; set; } = string.Empty;
        public string id_cobis { get; set; } = string.Empty;
        public string documento { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string sexo { get; set; } = string.Empty;
        public string version_doc { get; set; } = string.Empty;
        public string tipo_doc { get; set; } = string.Empty;
        public string fecha_nac { get; set; } = string.Empty;
        public string nro_convenio { get; set; } = string.Empty;
        public string nombre_convenio { get; set; } = string.Empty;
        public string calificacion { get; set; } = string.Empty;
        public string submercado { get; set; } = string.Empty;
        public string prov { get; set; } = string.Empty;
        public string zona { get; set; } = string.Empty;
        public string cod_sucursal { get; set; } = string.Empty;
        public string est_civil { get; set; } = string.Empty;
        public string flia { get; set; } = string.Empty;
        public string vivienda { get; set; } = string.Empty;
        public string contribuyente { get; set; } = string.Empty;
        public string laboral_ppal { get; set; } = string.Empty;
        public string fech_laboral { get; set; } = string.Empty;
        public string neto_fijo_ppal { get; set; } = string.Empty;
        public string neto_var_ppal { get; set; } = string.Empty;
        public string act_sec { get; set; } = string.Empty;
        public string laboral_sec { get; set; } = string.Empty;
        public string monotributo_sec { get; set; } = string.Empty;
        public string categoria_selecta { get; set; } = string.Empty;
        public string actividad_monotributo_sec { get; set; } = string.Empty;
        public string fech_laboral_sec { get; set; } = string.Empty;
        public string neto_fijo_sec { get; set; } = string.Empty;
        public string neto_var_sec { get; set; } = string.Empty;
        public string pp_cuota_macro { get; set; } = string.Empty;
        public string pp_monto_macro { get; set; } = string.Empty;
        public string pr_cuota_macro { get; set; } = string.Empty;
        public string pr_monto_macro { get; set; } = string.Empty;
        public string ph_cuota_macro { get; set; } = string.Empty;
        public string ph_monto_macro { get; set; } = string.Empty;
        public string op_cuota_macro { get; set; } = string.Empty;
        public string op_monto_macro { get; set; } = string.Empty;
        public string lim_tarj_macro { get; set; } = string.Empty;
        public string pago_min_tarj_macro { get; set; } = string.Empty;
        public string acue_cta_macro { get; set; } = string.Empty;
        public string uti_cta_cte_macro { get; set; } = string.Empty;
        public string dias_atraso { get; set; } = string.Empty;
        public string sum_cuotas_p_sg_no_bureau { get; set; } = string.Empty;
        public string sum_cuotas_p_cg_no_bureau { get; set; } = string.Empty;
        public string sum_pagos_min_tarj_no_bureau { get; set; } = string.Empty;
        public string sum_util_cta_cte_no_bureau { get; set; } = string.Empty;
        public string prestamo { get; set; } = string.Empty;
        public string tarjeta { get; set; } = string.Empty;
        public string cta_cte { get; set; } = string.Empty;
        public string tipo_prestamo { get; set; } = string.Empty;
        public string plazo { get; set; } = string.Empty;
        public string destino_personal { get; set; } = string.Empty;
        public string monto { get; set; } = string.Empty;
        public string tipo_tasa { get; set; } = string.Empty;
        public string tna { get; set; } = string.Empty;
        public string modelo_auto { get; set; } = string.Empty;
        public string auto { get; set; } = string.Empty;
        public string seg_auto { get; set; } = string.Empty;
        public string tasacion { get; set; } = string.Empty;
        public string val_compra_mejora_proyecto { get; set; } = string.Empty;
        public string val_terreno { get; set; } = string.Empty;
        public string destino { get; set; } = string.Empty;
        public string lim_tarj { get; set; } = string.Empty;
        public string acuerdo_cta_cte { get; set; } = string.Empty;
        public string cotitu { get; set; } = string.Empty;
        public string id_cobis_cotitu1 { get; set; } = string.Empty;
        public string documento_cotitu1 { get; set; } = string.Empty;
        public string nombre_cotitu1 { get; set; } = string.Empty;
        public string apellido_cotitu1 { get; set; } = string.Empty;
        public string sexo_cotitu1 { get; set; } = string.Empty;
        public string version_doc_cotitu1 { get; set; } = string.Empty;
        public string tipo_doc_cotitu1 { get; set; } = string.Empty;
        public string fech_laboral_cotitu1 { get; set; } = string.Empty;
        public string neto_fijo_cotitu1 { get; set; } = string.Empty;
        public string neto_var_cotitu1 { get; set; } = string.Empty;
        public string dias_atraso_cotitu1 { get; set; } = string.Empty;
        public string id_cobis_cotitu2 { get; set; } = string.Empty;
        public string documento_cotitu2 { get; set; } = string.Empty;
        public string nombre_cotitu2 { get; set; } = string.Empty;
        public string apellido_cotitu2 { get; set; } = string.Empty;
        public string sexo_cotitu2 { get; set; } = string.Empty;
        public string version_doc_cotitu2 { get; set; } = string.Empty;
        public string tipo_doc_cotitu2 { get; set; } = string.Empty;
        public string fech_laboral_cotitu2 { get; set; } = string.Empty;
        public string neto_fijo_cotitu2 { get; set; } = string.Empty;
        public string neto_var_cotitu2 { get; set; } = string.Empty;
        public string dias_atraso_cotitu2 { get; set; } = string.Empty;
        public string id_cobis_cotitu3 { get; set; } = string.Empty;
        public string documento_cotitu3 { get; set; } = string.Empty;
        public string nombre_cotitu3 { get; set; } = string.Empty;
        public string apellido_cotitu3 { get; set; } = string.Empty;
        public string sexo_cotitu3 { get; set; } = string.Empty;
        public string version_doc_cotitu3 { get; set; } = string.Empty;
        public string tipo_doc_cotitu3 { get; set; } = string.Empty;
        public string fech_laboral_cotitu3 { get; set; } = string.Empty;
        public string neto_fijo_cotitu3 { get; set; } = string.Empty;
        public string neto_var_cotitu3 { get; set; } = string.Empty;
        public string dias_atraso_cotitu3 { get; set; } = string.Empty;
        public string neto_fijo_cotitu123 { get; set; } = string.Empty;
        public string neto_var_cotitu123 { get; set; } = string.Empty;
        public string canal { get; set; } = string.Empty;
        public string equipo_ventas { get; set; } = string.Empty;
        public string tipo_fuente { get; set; } = string.Empty;
        public string fuente { get; set; } = string.Empty;
        public string segmento { get; set; } = string.Empty;
        public string subsegmento { get; set; } = string.Empty;
        public string nombre_plantilla { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string atributo1 { get; set; } = string.Empty;
        public string valor1 { get; set; } = string.Empty;
        public string atributo2 { get; set; } = string.Empty;
        public string valor2 { get; set; } = string.Empty;
        public string atributo3 { get; set; } = string.Empty;
        public string valor3 { get; set; } = string.Empty;
        public string atributo4 { get; set; } = string.Empty;
        public string valor4 { get; set; } = string.Empty;
        public string atributo5 { get; set; } = string.Empty;
        public string valor5 { get; set; } = string.Empty;
        public string atributo6 { get; set; } = string.Empty;
        public string valor6 { get; set; } = string.Empty;
        public string atributo7 { get; set; } = string.Empty;
        public string valor7 { get; set; } = string.Empty;
        public string atributo8 { get; set; } = string.Empty;
        public string valor8 { get; set; } = string.Empty;
        public string atributo9 { get; set; } = string.Empty;
        public string valor9 { get; set; } = string.Empty;
        public string atributo10 { get; set; } = string.Empty;
        public string valor10 { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public string cliente_inversor { get; set; } = string.Empty;
        public string monto_inversion { get; set; } = string.Empty;
        public string monto_subsidio { get; set; } = string.Empty;
        public string opcional_1 { get; set; } = string.Empty;
        public string opcional_2 { get; set; } = string.Empty;
        public string opcional_3 { get; set; } = string.Empty;
        public string opcional_4 { get; set; } = string.Empty;
        public string opcional_5 { get; set; } = string.Empty;
        public string opcional_6 { get; set; } = string.Empty;
        public string opcional_7 { get; set; } = string.Empty;
        public string opcional_8 { get; set; } = string.Empty;
        public string opcional_9 { get; set; } = string.Empty;
        public string opcional_10 { get; set; } = string.Empty;
        public string opcional_11 { get; set; } = string.Empty;
        public string opcional_12 { get; set; } = string.Empty;
        public string opcional_13 { get; set; } = string.Empty;
        public string opcional_14 { get; set; } = string.Empty;
        public string opcional_15 { get; set; } = string.Empty;
        public long EA_TIT_codigo_cliente { get; set; } = 0;
        public DateOnly EA_TIT_fecha_carga { get; set; } = new(1900, 1, 1);
        public DateOnly EA_TIT_fecha_cabalope { get; set; } = new(1900, 1, 1);
        public DateOnly EA_TIT_fecha_consulta_reestructuracion { get; set; } = new(1900, 1, 1);
        public string EA_TIT_motivo_reestructuracion { get; set; } = string.Empty;
        public DateOnly EA_TIT_fecha_consulta_refi_deu { get; set; } = new(1900, 1, 1);
        public string EA_TIT_motivo_refinanciacion { get; set; } = string.Empty;
        public string EA_TIT_motivo_deu { get; set; } = string.Empty;
        public int CND_TIT_cobis { get; set; } = 0;
        public string CND_TIT_nro_tributario { get; set; } = string.Empty;
        public string CND_TIT_codigo_campana { get; set; } = string.Empty;
        public DateOnly CND_TIT_vencimiento { get; set; } = new(1900, 1, 1);
        public string CND_TIT_motivo_rechazo { get; set; } = string.Empty;
        public int BH_TIT_id_cobis { get; set; } = 0;
        public int BH_TIT_riesgo_score { get; set; } = 0;
        public int BH_TIT_riesgo_6 { get; set; } = 0;
        public decimal BH_TIT_uso_tc_limite { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_tc_exigible { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_tc_limite_max { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_cc_limite { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_cc_promedio3 { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_sg_exigible { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_gp_exigible { get; set; } = decimal.Zero;
        public decimal BH_TIT_uso_gh_exigible { get; set; } = decimal.Zero;
        public int BH_TIT_antiguedad_tc { get; set; } = 0;
        public int BH_TIT_antiguedad_cc { get; set; } = 0;
        public int BH_TIT_cob_status { get; set; } = 0;
        public int BH_TIT_hit { get; set; } = 0;
        public decimal BH_TIT_uso_tc_saldo_fin { get; set; } = decimal.Zero;
        public string BH_TIT_ip2 { get; set; } = string.Empty;
        public string BH_TIT_consultar_veraz { get; set; } = string.Empty;
        public decimal CHEQ_TIT_rech_lev_sf_u24m_cant { get; set; } = decimal.Zero;
        public decimal CHEQ_TIT_rech_nolev_sf_u24m_monto { get; set; } = decimal.Zero;
        public decimal CHEQ_TIT_rech_lev_sf_u24m_monto { get; set; } = decimal.Zero;
        public string SC_TIT_exclusion { get; set; } = string.Empty;
        public int SC_TIT_score_comp { get; set; } = 0;
        public string SC_TIT_categoria { get; set; } = string.Empty;
        public int POL_TIT_flag_acuerdo_cc { get; set; } = 0;
        public int POL_TIT_flag_producto_crediticio_activo { get; set; } = 0;
        public decimal POL_TIT_antiguedad_tc_macro { get; set; } = decimal.Zero;
        public decimal POL_TIT_antiguedad_cc_macro { get; set; } = decimal.Zero;
        public int POL_TIT_cantidad_ocurrencia_6 { get; set; } = 0;
        public DateOnly EX_TIT_fin_excepcion_vz { get; set; } = new(1900, 1, 1);
        public long VA_TIT_cuit { get; set; } = 0;
        public string VA_TIT_nombre_pe { get; set; } = string.Empty;
        public string VA_TIT_apellido_pe { get; set; } = string.Empty;
        public string VA_TIT_sexo_pe { get; set; } = string.Empty;
        public DateOnly VA_TIT_fecha_nacimiento_pe { get; set; } = new(1900, 1, 1);
        public string VA_TIT_provincia_pe { get; set; } = string.Empty;
        public string VA_TIT_codigo_postal_pe { get; set; } = string.Empty;
        public DateOnly VA_TIT_fecha_carga { get; set; } = new(1900, 1, 1);
        public int VA_TIT_periodo_score { get; set; } = 0;
        public long VA_TIT_empleador { get; set; } = 0;
        public int VA_TIT_flag_crediticio { get; set; } = 0;
        public decimal VA_TIT_sum_score { get; set; } = decimal.Zero;
        public string VA_TIT_razon_social_afip { get; set; } = string.Empty;
        public string VA_TIT_impuesto_ganancias { get; set; } = string.Empty;
        public string VA_TIT_impuesto_iva { get; set; } = string.Empty;
        public string VA_TIT_integrante_sociedad { get; set; } = string.Empty;
        public string VA_TIT_es_empleador_afip { get; set; } = string.Empty;
        public string VA_TIT_cat_monotributo { get; set; } = string.Empty;
        public string VA_TIT_actividad_monotributo { get; set; } = string.Empty;
        public string VA_TIT_actividad_monotributo_descripcion { get; set; } = string.Empty;
        public decimal VA_TIT_fact_monotributo { get; set; } = decimal.Zero;
        public decimal VA_TIT_ingreso_servicios { get; set; } = decimal.Zero;
        public decimal VA_TIT_ingreso_ventas { get; set; } = decimal.Zero;
        public decimal VA_TIT_ingreso_monotributo { get; set; } = decimal.Zero;
        public string VA_TIT_es_empleado { get; set; } = string.Empty;
        public string VA_TIT_es_autonomo { get; set; } = string.Empty;
        public string VA_TIT_es_monotributista { get; set; } = string.Empty;
        public string VA_TIT_es_jubilado { get; set; } = string.Empty;
        public decimal VA_TIT_ingreso_transaccionalidad { get; set; } = decimal.Zero;
        public string VA_TIT_opcional_1 { get; set; } = string.Empty;
        public string VA_TIT_1 { get; set; } = string.Empty;
        public string VA_TIT_2 { get; set; } = string.Empty;
        public string VA_TIT_3 { get; set; } = string.Empty;
        public string VA_TIT_4 { get; set; } = string.Empty;
        public string VA_TIT_5 { get; set; } = string.Empty;
        public string VA_TIT_6 { get; set; } = string.Empty;
        public string VA_TIT_7 { get; set; } = string.Empty;
        public string VA_TIT_8 { get; set; } = string.Empty;
        public string VA_TIT_9 { get; set; } = string.Empty;
        public string VA_TIT_10 { get; set; } = string.Empty;
        public string VA_TIT_11 { get; set; } = string.Empty;
        public string VA_TIT_12 { get; set; } = string.Empty;
        public string VA_TIT_13 { get; set; } = string.Empty;
        public string VA_TIT_14 { get; set; } = string.Empty;
        public string VA_TIT_15 { get; set; } = string.Empty;
        public string VA_TIT_16 { get; set; } = string.Empty;
        public string VA_TIT_17 { get; set; } = string.Empty;
        public string VA_TIT_18 { get; set; } = string.Empty;
        public string VA_TIT_19 { get; set; } = string.Empty;
        public string VA_TIT_20 { get; set; } = string.Empty;
        public string VA_TIT_21 { get; set; } = string.Empty;
        public string VA_TIT_22 { get; set; } = string.Empty;
        public string VA_TIT_23 { get; set; } = string.Empty;
        public string VA_TIT_24 { get; set; } = string.Empty;
        public string VA_TIT_25 { get; set; } = string.Empty;
        public string VA_TIT_26 { get; set; } = string.Empty;
        public string VA_TIT_27 { get; set; } = string.Empty;
        public string VA_TIT_28 { get; set; } = string.Empty;
        public string VA_TIT_29 { get; set; } = string.Empty;
        public string VA_TIT_30 { get; set; } = string.Empty;
        public string VA_TIT_31 { get; set; } = string.Empty;
        public string VA_TIT_32 { get; set; } = string.Empty;
        public string VA_TIT_33 { get; set; } = string.Empty;
        public string VA_TIT_34 { get; set; } = string.Empty;
        public string VA_TIT_35 { get; set; } = string.Empty;
        public string VA_TIT_36 { get; set; } = string.Empty;
        public string VA_TIT_37 { get; set; } = string.Empty;
        public string VA_TIT_38 { get; set; } = string.Empty;
        public string VA_TIT_39 { get; set; } = string.Empty;
        public string VA_TIT_40 { get; set; } = string.Empty;
        public string VA_TIT_41 { get; set; } = string.Empty;
        public string VA_TIT_42 { get; set; } = string.Empty;
        public string VA_TIT_43 { get; set; } = string.Empty;
        public string VA_TIT_44 { get; set; } = string.Empty;
        public string VA_TIT_45 { get; set; } = string.Empty;
        public string VA_TIT_46 { get; set; } = string.Empty;
        public string VA_TIT_47 { get; set; } = string.Empty;
        public string VA_TIT_48 { get; set; } = string.Empty;
        public string VA_TIT_49 { get; set; } = string.Empty;
        public string VA_TIT_50 { get; set; } = string.Empty;
        public decimal TIND_TIT_ingreso_calculado_final { get; set; } = decimal.Zero;
        public string TIND_TIT_tipo_ingreso { get; set; } = string.Empty;
        public long EA_COT1_codigo_cliente { get; set; } = 0;
        public DateOnly EA_COT1_fecha_carga { get; set; } = new(1900, 1, 1);
        public DateOnly EA_COT1_fecha_cabalope { get; set; } = new(1900, 1, 1);
        public DateOnly EA_COT1_fecha_consulta_reestructuracion { get; set; } = new(1900, 1, 1);
        public string EA_COT1_motivo_reestructuracion { get; set; } = string.Empty;
        public DateOnly EA_COT1_fecha_consulta_refi_deu { get; set; } = new(1900, 1, 1);
        public string EA_COT1_motivo_refinanciacion { get; set; } = string.Empty;
        public string EA_COT1_motivo_deu { get; set; } = string.Empty;
        public int CND_COT1_cobis { get; set; } = 0;
        public string CND_COT1_nro_tributario { get; set; } = string.Empty;
        public string CND_COT1_codigo_campana { get; set; } = string.Empty;
        public DateOnly CND_COT1_vencimiento { get; set; } = new(1900, 1, 1);
        public string CND_COT1_motivo_rechazo { get; set; } = string.Empty;
        public int BH_COT1_id_cobis { get; set; } = 0;
        public int BH_COT1_riesgo_score { get; set; } = 0;
        public int BH_COT1_riesgo_6 { get; set; } = 0;
        public decimal BH_COT1_uso_tc_limite { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_tc_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_tc_limite_max { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_cc_limite { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_cc_promedio3 { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_sg_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_gp_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT1_uso_gh_exigible { get; set; } = decimal.Zero;
        public int BH_COT1_antiguedad_tc { get; set; } = 0;
        public int BH_COT1_antiguedad_cc { get; set; } = 0;
        public int BH_COT1_cob_status { get; set; } = 0;
        public int BH_COT1_hit { get; set; } = 0;
        public decimal BH_COT1_uso_tc_saldo_fin { get; set; } = decimal.Zero;
        public string BH_COT1_ip2 { get; set; } = string.Empty;
        public string BH_COT1_consultar_veraz { get; set; } = string.Empty;
        public decimal CHEQ_COT1_rech_lev_sf_u24m_cant { get; set; } = decimal.Zero;
        public decimal CHEQ_COT1_rech_nolev_sf_u24m_monto { get; set; } = decimal.Zero;
        public decimal CHEQ_COT1_rech_lev_sf_u24m_monto { get; set; } = decimal.Zero;
        public string SC_COT1_exclusion { get; set; } = string.Empty;
        public int SC_COT1_score_comp { get; set; } = 0;
        public string SC_COT1_categoria { get; set; } = string.Empty;
        public int POL_COT1_flag_acuerdo_cc { get; set; } = 0;
        public int POL_COT1_flag_producto_crediticio_activo { get; set; } = 0;
        public decimal POL_COT1_antiguedad_tc_macro { get; set; } = decimal.Zero;
        public decimal POL_COT1_antiguedad_cc_macro { get; set; } = decimal.Zero;
        public int POL_COT1_cantidad_ocurrencia_6 { get; set; } = 0;
        public DateOnly EX_COT1_fin_excepcion_vz { get; set; } = new(1900, 1, 1);
        public long VA_COT1_cuit { get; set; } = 0;
        public string VA_COT1_nombre_pe { get; set; } = string.Empty;
        public string VA_COT1_apellido_pe { get; set; } = string.Empty;
        public string VA_COT1_sexo_pe { get; set; } = string.Empty;
        public DateOnly VA_COT1_fecha_nacimiento_pe { get; set; } = new(1900, 1, 1);
        public string VA_COT1_provincia_pe { get; set; } = string.Empty;
        public string VA_COT1_codigo_postal_pe { get; set; } = string.Empty;
        public DateOnly VA_COT1_fecha_carga { get; set; } = new(1900, 1, 1);
        public int VA_COT1_periodo_score { get; set; } = 0;
        public long VA_COT1_empleador { get; set; } = 0;
        public int VA_COT1_flag_crediticio { get; set; } = 0;
        public decimal VA_COT1_sum_score { get; set; } = decimal.Zero;
        public string VA_COT1_razon_social_afip { get; set; } = string.Empty;
        public string VA_COT1_impuesto_ganancias { get; set; } = string.Empty;
        public string VA_COT1_impuesto_iva { get; set; } = string.Empty;
        public string VA_COT1_integrante_sociedad { get; set; } = string.Empty;
        public string VA_COT1_es_empleador_afip { get; set; } = string.Empty;
        public string VA_COT1_cat_monotributo { get; set; } = string.Empty;
        public string VA_COT1_actividad_monotributo { get; set; } = string.Empty;
        public string VA_COT1_actividad_monotributo_descripcion { get; set; } = string.Empty;
        public decimal VA_COT1_fact_monotributo { get; set; } = decimal.Zero;
        public decimal VA_COT1_ingreso_servicios { get; set; } = decimal.Zero;
        public decimal VA_COT1_ingreso_ventas { get; set; } = decimal.Zero;
        public decimal VA_COT1_ingreso_monotributo { get; set; } = decimal.Zero;
        public string VA_COT1_es_empleado { get; set; } = string.Empty;
        public string VA_COT1_es_autonomo { get; set; } = string.Empty;
        public string VA_COT1_es_monotributista { get; set; } = string.Empty;
        public string VA_COT1_es_jubilado { get; set; } = string.Empty;
        public decimal VA_COT1_ingreso_transaccionalidad { get; set; } = decimal.Zero;
        public string VA_COT1_opcional_1 { get; set; } = string.Empty;
        public string VA_COT1_1 { get; set; } = string.Empty;
        public string VA_COT1_2 { get; set; } = string.Empty;
        public string VA_COT1_3 { get; set; } = string.Empty;
        public string VA_COT1_4 { get; set; } = string.Empty;
        public string VA_COT1_5 { get; set; } = string.Empty;
        public string VA_COT1_6 { get; set; } = string.Empty;
        public string VA_COT1_7 { get; set; } = string.Empty;
        public string VA_COT1_8 { get; set; } = string.Empty;
        public string VA_COT1_9 { get; set; } = string.Empty;
        public string VA_COT1_10 { get; set; } = string.Empty;
        public string VA_COT1_11 { get; set; } = string.Empty;
        public string VA_COT1_12 { get; set; } = string.Empty;
        public string VA_COT1_13 { get; set; } = string.Empty;
        public string VA_COT1_14 { get; set; } = string.Empty;
        public string VA_COT1_15 { get; set; } = string.Empty;
        public string VA_COT1_16 { get; set; } = string.Empty;
        public string VA_COT1_17 { get; set; } = string.Empty;
        public string VA_COT1_18 { get; set; } = string.Empty;
        public string VA_COT1_19 { get; set; } = string.Empty;
        public string VA_COT1_20 { get; set; } = string.Empty;
        public string VA_COT1_21 { get; set; } = string.Empty;
        public string VA_COT1_22 { get; set; } = string.Empty;
        public string VA_COT1_23 { get; set; } = string.Empty;
        public string VA_COT1_24 { get; set; } = string.Empty;
        public string VA_COT1_25 { get; set; } = string.Empty;
        public string VA_COT1_26 { get; set; } = string.Empty;
        public string VA_COT1_27 { get; set; } = string.Empty;
        public string VA_COT1_28 { get; set; } = string.Empty;
        public string VA_COT1_29 { get; set; } = string.Empty;
        public string VA_COT1_30 { get; set; } = string.Empty;
        public string VA_COT1_31 { get; set; } = string.Empty;
        public string VA_COT1_32 { get; set; } = string.Empty;
        public string VA_COT1_33 { get; set; } = string.Empty;
        public string VA_COT1_34 { get; set; } = string.Empty;
        public string VA_COT1_35 { get; set; } = string.Empty;
        public string VA_COT1_36 { get; set; } = string.Empty;
        public string VA_COT1_37 { get; set; } = string.Empty;
        public string VA_COT1_38 { get; set; } = string.Empty;
        public string VA_COT1_39 { get; set; } = string.Empty;
        public string VA_COT1_40 { get; set; } = string.Empty;
        public string VA_COT1_41 { get; set; } = string.Empty;
        public string VA_COT1_42 { get; set; } = string.Empty;
        public string VA_COT1_43 { get; set; } = string.Empty;
        public string VA_COT1_44 { get; set; } = string.Empty;
        public string VA_COT1_45 { get; set; } = string.Empty;
        public string VA_COT1_46 { get; set; } = string.Empty;
        public string VA_COT1_47 { get; set; } = string.Empty;
        public string VA_COT1_48 { get; set; } = string.Empty;
        public string VA_COT1_49 { get; set; } = string.Empty;
        public string VA_COT1_50 { get; set; } = string.Empty;
        public decimal TIND_COT1_ingreso_calculado_final { get; set; } = decimal.Zero;
        public string TIND_COT1_tipo_ingreso { get; set; } = string.Empty;
        public long EA_COT2_codigo_cliente { get; set; } = 0;
        public DateOnly EA_COT2_fecha_carga { get; set; } = new(1900, 1, 1);
        public DateOnly EA_COT2_fecha_cabalope { get; set; } = new(1900, 1, 1);
        public DateOnly EA_COT2_fecha_consulta_reestructuracion { get; set; } = new(1900, 1, 1);
        public string EA_COT2_motivo_reestructuracion { get; set; } = string.Empty;
        public DateOnly EA_COT2_fecha_consulta_refi_deu { get; set; } = new(1900, 1, 1);
        public string EA_COT2_motivo_refinanciacion { get; set; } = string.Empty;
        public string EA_COT2_motivo_deu { get; set; } = string.Empty;
        public int CND_COT2_cobis { get; set; } = 0;
        public string CND_COT2_nro_tributario { get; set; } = string.Empty;
        public string CND_COT2_codigo_campana { get; set; } = string.Empty;
        public DateOnly CND_COT2_vencimiento { get; set; } = new(1900, 1, 1);
        public string CND_COT2_motivo_rechazo { get; set; } = string.Empty;
        public int BH_COT2_id_cobis { get; set; } = 0;
        public int BH_COT2_riesgo_score { get; set; } = 0;
        public int BH_COT2_riesgo_6 { get; set; } = 0;
        public decimal BH_COT2_uso_tc_limite { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_tc_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_tc_limite_max { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_cc_limite { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_cc_promedio3 { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_sg_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_gp_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT2_uso_gh_exigible { get; set; } = decimal.Zero;
        public int BH_COT2_antiguedad_tc { get; set; } = 0;
        public int BH_COT2_antiguedad_cc { get; set; } = 0;
        public int BH_COT2_cob_status { get; set; } = 0;
        public int BH_COT2_hit { get; set; } = 0;
        public decimal BH_COT2_uso_tc_saldo_fin { get; set; } = decimal.Zero;
        public string BH_COT2_ip2 { get; set; } = string.Empty;
        public string BH_COT2_consultar_veraz { get; set; } = string.Empty;
        public decimal CHEQ_COT2_rech_lev_sf_u24m_cant { get; set; } = decimal.Zero;
        public decimal CHEQ_COT2_rech_nolev_sf_u24m_monto { get; set; } = decimal.Zero;
        public decimal CHEQ_COT2_rech_lev_sf_u24m_monto { get; set; } = decimal.Zero;
        public string SC_COT2_exclusion { get; set; } = string.Empty;
        public int SC_COT2_score_comp { get; set; } = 0;
        public string SC_COT2_categoria { get; set; } = string.Empty;
        public int POL_COT2_flag_acuerdo_cc { get; set; } = 0;
        public int POL_COT2_flag_producto_crediticio_activo { get; set; } = 0;
        public decimal POL_COT2_antiguedad_tc_macro { get; set; } = decimal.Zero;
        public decimal POL_COT2_antiguedad_cc_macro { get; set; } = decimal.Zero;
        public int POL_COT2_cantidad_ocurrencia_6 { get; set; } = 0;
        public DateOnly EX_COT2_fin_excepcion_vz { get; set; } = new(1900, 1, 1);
        public long VA_COT2_cuit { get; set; } = 0;
        public string VA_COT2_nombre_pe { get; set; } = string.Empty;
        public string VA_COT2_apellido_pe { get; set; } = string.Empty;
        public string VA_COT2_sexo_pe { get; set; } = string.Empty;
        public DateOnly VA_COT2_fecha_nacimiento_pe { get; set; } = new(1900, 1, 1);
        public string VA_COT2_provincia_pe { get; set; } = string.Empty;
        public string VA_COT2_codigo_postal_pe { get; set; } = string.Empty;
        public DateOnly VA_COT2_fecha_carga { get; set; } = new(1900, 1, 1);
        public int VA_COT2_periodo_score { get; set; } = 0;
        public long VA_COT2_empleador { get; set; } = 0;
        public int VA_COT2_flag_crediticio { get; set; } = 0;
        public decimal VA_COT2_sum_score { get; set; } = decimal.Zero;
        public string VA_COT2_razon_social_afip { get; set; } = string.Empty;
        public string VA_COT2_impuesto_ganancias { get; set; } = string.Empty;
        public string VA_COT2_impuesto_iva { get; set; } = string.Empty;
        public string VA_COT2_integrante_sociedad { get; set; } = string.Empty;
        public string VA_COT2_es_empleador_afip { get; set; } = string.Empty;
        public string VA_COT2_cat_monotributo { get; set; } = string.Empty;
        public string VA_COT2_actividad_monotributo { get; set; } = string.Empty;
        public string VA_COT2_actividad_monotributo_descripcion { get; set; } = string.Empty;
        public decimal VA_COT2_fact_monotributo { get; set; } = decimal.Zero;
        public decimal VA_COT2_ingreso_servicios { get; set; } = decimal.Zero;
        public decimal VA_COT2_ingreso_ventas { get; set; } = decimal.Zero;
        public decimal VA_COT2_ingreso_monotributo { get; set; } = decimal.Zero;
        public string VA_COT2_es_empleado { get; set; } = string.Empty;
        public string VA_COT2_es_autonomo { get; set; } = string.Empty;
        public string VA_COT2_es_monotributista { get; set; } = string.Empty;
        public string VA_COT2_es_jubilado { get; set; } = string.Empty;
        public decimal VA_COT2_ingreso_transaccionalidad { get; set; } = decimal.Zero;
        public string VA_COT2_opcional_1 { get; set; } = string.Empty;
        public string VA_COT2_1 { get; set; } = string.Empty;
        public string VA_COT2_2 { get; set; } = string.Empty;
        public string VA_COT2_3 { get; set; } = string.Empty;
        public string VA_COT2_4 { get; set; } = string.Empty;
        public string VA_COT2_5 { get; set; } = string.Empty;
        public string VA_COT2_6 { get; set; } = string.Empty;
        public string VA_COT2_7 { get; set; } = string.Empty;
        public string VA_COT2_8 { get; set; } = string.Empty;
        public string VA_COT2_9 { get; set; } = string.Empty;
        public string VA_COT2_10 { get; set; } = string.Empty;
        public string VA_COT2_11 { get; set; } = string.Empty;
        public string VA_COT2_12 { get; set; } = string.Empty;
        public string VA_COT2_13 { get; set; } = string.Empty;
        public string VA_COT2_14 { get; set; } = string.Empty;
        public string VA_COT2_15 { get; set; } = string.Empty;
        public string VA_COT2_16 { get; set; } = string.Empty;
        public string VA_COT2_17 { get; set; } = string.Empty;
        public string VA_COT2_18 { get; set; } = string.Empty;
        public string VA_COT2_19 { get; set; } = string.Empty;
        public string VA_COT2_20 { get; set; } = string.Empty;
        public string VA_COT2_21 { get; set; } = string.Empty;
        public string VA_COT2_22 { get; set; } = string.Empty;
        public string VA_COT2_23 { get; set; } = string.Empty;
        public string VA_COT2_24 { get; set; } = string.Empty;
        public string VA_COT2_25 { get; set; } = string.Empty;
        public string VA_COT2_26 { get; set; } = string.Empty;
        public string VA_COT2_27 { get; set; } = string.Empty;
        public string VA_COT2_28 { get; set; } = string.Empty;
        public string VA_COT2_29 { get; set; } = string.Empty;
        public string VA_COT2_30 { get; set; } = string.Empty;
        public string VA_COT2_31 { get; set; } = string.Empty;
        public string VA_COT2_32 { get; set; } = string.Empty;
        public string VA_COT2_33 { get; set; } = string.Empty;
        public string VA_COT2_34 { get; set; } = string.Empty;
        public string VA_COT2_35 { get; set; } = string.Empty;
        public string VA_COT2_36 { get; set; } = string.Empty;
        public string VA_COT2_37 { get; set; } = string.Empty;
        public string VA_COT2_38 { get; set; } = string.Empty;
        public string VA_COT2_39 { get; set; } = string.Empty;
        public string VA_COT2_40 { get; set; } = string.Empty;
        public string VA_COT2_41 { get; set; } = string.Empty;
        public string VA_COT2_42 { get; set; } = string.Empty;
        public string VA_COT2_43 { get; set; } = string.Empty;
        public string VA_COT2_44 { get; set; } = string.Empty;
        public string VA_COT2_45 { get; set; } = string.Empty;
        public string VA_COT2_46 { get; set; } = string.Empty;
        public string VA_COT2_47 { get; set; } = string.Empty;
        public string VA_COT2_48 { get; set; } = string.Empty;
        public string VA_COT2_49 { get; set; } = string.Empty;
        public string VA_COT2_50 { get; set; } = string.Empty;
        public decimal TIND_COT2_ingreso_calculado_final { get; set; } = decimal.Zero;
        public string TIND_COT2_tipo_ingreso { get; set; } = string.Empty;
        public long EA_COT3_codigo_cliente { get; set; } = 0;
        public DateOnly EA_COT3_fecha_carga { get; set; } = new(1900, 1, 1);
        public DateOnly EA_COT3_fecha_cabalope { get; set; } = new(1900, 1, 1);
        public DateOnly EA_COT3_fecha_consulta_reestructuracion { get; set; } = new(1900, 1, 1);
        public string EA_COT3_motivo_reestructuracion { get; set; } = string.Empty;
        public DateOnly EA_COT3_fecha_consulta_refi_deu { get; set; } = new(1900, 1, 1);
        public string EA_COT3_motivo_refinanciacion { get; set; } = string.Empty;
        public string EA_COT3_motivo_deu { get; set; } = string.Empty;
        public int CND_COT3_cobis { get; set; } = 0;
        public string CND_COT3_nro_tributario { get; set; } = string.Empty;
        public string CND_COT3_codigo_campana { get; set; } = string.Empty;
        public DateOnly CND_COT3_vencimiento { get; set; } = new(1900, 1, 1);
        public string CND_COT3_motivo_rechazo { get; set; } = string.Empty;
        public int BH_COT3_id_cobis { get; set; } = 0;
        public int BH_COT3_riesgo_score { get; set; } = 0;
        public int BH_COT3_riesgo_6 { get; set; } = 0;
        public decimal BH_COT3_uso_tc_limite { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_tc_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_tc_limite_max { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_cc_limite { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_cc_promedio3 { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_sg_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_gp_exigible { get; set; } = decimal.Zero;
        public decimal BH_COT3_uso_gh_exigible { get; set; } = decimal.Zero;
        public int BH_COT3_antiguedad_tc { get; set; } = 0;
        public int BH_COT3_antiguedad_cc { get; set; } = 0;
        public int BH_COT3_cob_status { get; set; } = 0;
        public int BH_COT3_hit { get; set; } = 0;
        public decimal BH_COT3_uso_tc_saldo_fin { get; set; } = decimal.Zero;
        public string BH_COT3_ip2 { get; set; } = string.Empty;
        public string BH_COT3_consultar_veraz { get; set; } = string.Empty;
        public decimal CHEQ_COT3_rech_lev_sf_u24m_cant { get; set; } = decimal.Zero;
        public decimal CHEQ_COT3_rech_nolev_sf_u24m_monto { get; set; } = decimal.Zero;
        public decimal CHEQ_COT3_rech_lev_sf_u24m_monto { get; set; } = decimal.Zero;
        public string SC_COT3_exclusion { get; set; } = string.Empty;
        public int SC_COT3_score_comp { get; set; } = 0;
        public string SC_COT3_categoria { get; set; } = string.Empty;
        public int POL_COT3_flag_acuerdo_cc { get; set; } = 0;
        public int POL_COT3_flag_producto_crediticio_activo { get; set; } = 0;
        public decimal POL_COT3_antiguedad_tc_macro { get; set; } = decimal.Zero;
        public decimal POL_COT3_antiguedad_cc_macro { get; set; } = decimal.Zero;
        public int POL_COT3_cantidad_ocurrencia_6 { get; set; } = 0;
        public DateOnly EX_COT3_fin_excepcion_vz { get; set; } = new(1900, 1, 1);
        public long VA_COT3_cuit { get; set; } = 0;
        public string VA_COT3_nombre_pe { get; set; } = string.Empty;
        public string VA_COT3_apellido_pe { get; set; } = string.Empty;
        public string VA_COT3_sexo_pe { get; set; } = string.Empty;
        public DateOnly VA_COT3_fecha_nacimiento_pe { get; set; } = new(1900, 1, 1);
        public string VA_COT3_provincia_pe { get; set; } = string.Empty;
        public string VA_COT3_codigo_postal_pe { get; set; } = string.Empty;
        public DateOnly VA_COT3_fecha_carga { get; set; } = new(1900, 1, 1);
        public int VA_COT3_periodo_score { get; set; } = 0;
        public long VA_COT3_empleador { get; set; } = 0;
        public int VA_COT3_flag_crediticio { get; set; } = 0;
        public decimal VA_COT3_sum_score { get; set; } = decimal.Zero;
        public string VA_COT3_razon_social_afip { get; set; } = string.Empty;
        public string VA_COT3_impuesto_ganancias { get; set; } = string.Empty;
        public string VA_COT3_impuesto_iva { get; set; } = string.Empty;
        public string VA_COT3_integrante_sociedad { get; set; } = string.Empty;
        public string VA_COT3_es_empleador_afip { get; set; } = string.Empty;
        public string VA_COT3_cat_monotributo { get; set; } = string.Empty;
        public string VA_COT3_actividad_monotributo { get; set; } = string.Empty;
        public string VA_COT3_actividad_monotributo_descripcion { get; set; } = string.Empty;
        public decimal VA_COT3_fact_monotributo { get; set; } = decimal.Zero;
        public decimal VA_COT3_ingreso_servicios { get; set; } = decimal.Zero;
        public decimal VA_COT3_ingreso_ventas { get; set; } = decimal.Zero;
        public decimal VA_COT3_ingreso_monotributo { get; set; } = decimal.Zero;
        public string VA_COT3_es_empleado { get; set; } = string.Empty;
        public string VA_COT3_es_autonomo { get; set; } = string.Empty;
        public string VA_COT3_es_monotributista { get; set; } = string.Empty;
        public string VA_COT3_es_jubilado { get; set; } = string.Empty;
        public decimal VA_COT3_ingreso_transaccionalidad { get; set; } = decimal.Zero;
        public string VA_COT3_opcional_1 { get; set; } = string.Empty;
        public string VA_COT3_1 { get; set; } = string.Empty;
        public string VA_COT3_2 { get; set; } = string.Empty;
        public string VA_COT3_3 { get; set; } = string.Empty;
        public string VA_COT3_4 { get; set; } = string.Empty;
        public string VA_COT3_5 { get; set; } = string.Empty;
        public string VA_COT3_6 { get; set; } = string.Empty;
        public string VA_COT3_7 { get; set; } = string.Empty;
        public string VA_COT3_8 { get; set; } = string.Empty;
        public string VA_COT3_9 { get; set; } = string.Empty;
        public string VA_COT3_10 { get; set; } = string.Empty;
        public string VA_COT3_11 { get; set; } = string.Empty;
        public string VA_COT3_12 { get; set; } = string.Empty;
        public string VA_COT3_13 { get; set; } = string.Empty;
        public string VA_COT3_14 { get; set; } = string.Empty;
        public string VA_COT3_15 { get; set; } = string.Empty;
        public string VA_COT3_16 { get; set; } = string.Empty;
        public string VA_COT3_17 { get; set; } = string.Empty;
        public string VA_COT3_18 { get; set; } = string.Empty;
        public string VA_COT3_19 { get; set; } = string.Empty;
        public string VA_COT3_20 { get; set; } = string.Empty;
        public string VA_COT3_21 { get; set; } = string.Empty;
        public string VA_COT3_22 { get; set; } = string.Empty;
        public string VA_COT3_23 { get; set; } = string.Empty;
        public string VA_COT3_24 { get; set; } = string.Empty;
        public string VA_COT3_25 { get; set; } = string.Empty;
        public string VA_COT3_26 { get; set; } = string.Empty;
        public string VA_COT3_27 { get; set; } = string.Empty;
        public string VA_COT3_28 { get; set; } = string.Empty;
        public string VA_COT3_29 { get; set; } = string.Empty;
        public string VA_COT3_30 { get; set; } = string.Empty;
        public string VA_COT3_31 { get; set; } = string.Empty;
        public string VA_COT3_32 { get; set; } = string.Empty;
        public string VA_COT3_33 { get; set; } = string.Empty;
        public string VA_COT3_34 { get; set; } = string.Empty;
        public string VA_COT3_35 { get; set; } = string.Empty;
        public string VA_COT3_36 { get; set; } = string.Empty;
        public string VA_COT3_37 { get; set; } = string.Empty;
        public string VA_COT3_38 { get; set; } = string.Empty;
        public string VA_COT3_39 { get; set; } = string.Empty;
        public string VA_COT3_40 { get; set; } = string.Empty;
        public string VA_COT3_41 { get; set; } = string.Empty;
        public string VA_COT3_42 { get; set; } = string.Empty;
        public string VA_COT3_43 { get; set; } = string.Empty;
        public string VA_COT3_44 { get; set; } = string.Empty;
        public string VA_COT3_45 { get; set; } = string.Empty;
        public string VA_COT3_46 { get; set; } = string.Empty;
        public string VA_COT3_47 { get; set; } = string.Empty;
        public string VA_COT3_48 { get; set; } = string.Empty;
        public string VA_COT3_49 { get; set; } = string.Empty;
        public string VA_COT3_50 { get; set; } = string.Empty;
        public decimal TIND_COT3_ingreso_calculado_final { get; set; } = decimal.Zero;
        public string TIND_COT3_tipo_ingreso { get; set; } = string.Empty;
        public string DM_nombre_convenio { get; set; } = string.Empty;
        public string DM_nro_convenio { get; set; } = string.Empty;
        public string DM_calificacion { get; set; } = string.Empty;
        public int CCD_numero_submercado { get; set; } = 0;
        public string CCD_nombre_submercado { get; set; } = string.Empty;
        public string CCD_estado_convenio { get; set; } = string.Empty;
        public DateOnly CCD_fecha_ult_actualizacion { get; set; } = new(1900, 1, 1);
        public string CCD_calificacion_permanente { get; set; } = string.Empty;
        public string CCD_calificacion_otra_planta { get; set; } = string.Empty;
        public decimal CCD_afectacion_pp_permanente { get; set; } = decimal.Zero;
        public decimal CCD_afectacion_pp_otra_planta { get; set; } = decimal.Zero;
        public decimal CCD_afectacion_ph_permanente { get; set; } = decimal.Zero;
        public decimal CCD_afectacion_ph_otra_planta { get; set; } = decimal.Zero;
        public decimal CCD_monto_maximo_permanente { get; set; } = decimal.Zero;
        public decimal CCD_monto_maximo_otra_planta { get; set; } = decimal.Zero;
        public decimal CCD_cant_sueldos_maximo_permanente { get; set; } = decimal.Zero;
        public decimal CCD_cant_sueldos_maximo_otra_planta { get; set; } = decimal.Zero;
        public int CCD_plazo_permanente { get; set; } = 0;
        public int CCD_plazo_otra_planta { get; set; } = 0;
        public decimal CCD_max_endeu_global_permanente { get; set; } = decimal.Zero;
        public decimal CCD_max_endeu_global_otra_planta { get; set; } = decimal.Zero;
        public decimal CCD_opcional1_permanente { get; set; } = decimal.Zero;
        public decimal CCD_opcional1_otra_planta { get; set; } = decimal.Zero;
        public decimal CCD_opcional2_permanente { get; set; } = decimal.Zero;
        public decimal CCD_opcional2_otra_planta { get; set; } = decimal.Zero;
    }
    public class PoliticaPlanSueldoPublicoSalida
    {
        public DateOnly fecha { get; set; } = new(1900,1,1);
        public string id_oportunidad { get; set; } = string.Empty;
        public string id_evaluacion { get; set; } = string.Empty;
        public string ap_origen { get; set; } = string.Empty;
        public string nro_llamado { get; set; } = string.Empty;
        public string documento { get; set; } = string.Empty;
        public string edad { get; set; } = string.Empty;
        public string VZ_domicilio { get; set; } = string.Empty;
        public string ddjj_de_salud { get; set; } = string.Empty;
        public string antig_lab_ppal { get; set; } = string.Empty;
        public string nombre_convenio { get; set; } = string.Empty;
        public string nombre_submercado { get; set; } = string.Empty;
        public decimal ingreso_computable_neto { get; set; } = decimal.Zero;
        public decimal ingreso_ppal { get; set; } = decimal.Zero;
        public decimal ingreso_sec { get; set; } = decimal.Zero;
        public string sobre_que_ingreso_tope { get; set; } = string.Empty;
        public decimal ingreso_total { get; set; } = decimal.Zero;
        public decimal pm_tarj_bureau { get; set; } = decimal.Zero;
        public string sumo_pm_tj_bureau { get; set; } = string.Empty;
        public string sumo_sum_pagos_min_tarj_no_bureau { get; set; } = string.Empty;
        public decimal sprom_ac_bureau { get; set; } = decimal.Zero;
        public string sumo_sprom_ac_bureau { get; set; } = string.Empty;
        public string sumo_sum_util_cta_cte_no_bureau { get; set; } = string.Empty;
        public decimal pago_min { get; set; } = decimal.Zero;
        public decimal compromiso_total_final { get; set; } = decimal.Zero;
        public decimal compromiso_total_actual_final { get; set; } = decimal.Zero;
        public decimal compromiso_solicitado { get; set; } = decimal.Zero;
        public decimal compromiso_titu_macro { get; set; } = decimal.Zero;
        public decimal compromiso_bureau { get; set; } = decimal.Zero;
        public decimal compromiso_total_bureau { get; set; } = decimal.Zero;
        public decimal endeudamiento_global_productos_con_garantia { get; set; } = decimal.Zero;
        public decimal endeudamiento_global_productos_sin_garantia { get; set; } = decimal.Zero;
        public decimal porc_endeudamiento_bureau { get; set; } = decimal.Zero;
        public decimal porc_endeudamiento_macro { get; set; } = decimal.Zero;
        public decimal max_afect_global_pesos { get; set; } = decimal.Zero;
        public decimal max_afect_global_porc { get; set; } = decimal.Zero;
        public decimal compromiso_ingreso { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_global_porc { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_porc { get; set; } = decimal.Zero;
        public decimal max_afect_pmo_pesos { get; set; } = decimal.Zero;
        public decimal max_afect_pmo_porc { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_pmo_pesos { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_pmo_porc { get; set; } = decimal.Zero;
        public decimal max_afect_pmo_tarj_pesos { get; set; } = decimal.Zero;
        public decimal max_afect_pmo_tarj_porc { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_pmo_tarj_pesos { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_pmo_tarj_porc { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_acuerdo_pesos { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_acuerdo_porc { get; set; } = decimal.Zero;
        public decimal porc_max_acuerdo { get; set; } = decimal.Zero;
        public decimal porc_solicitado { get; set; } = decimal.Zero;
        public decimal tope_endeudamiento_acuerdo_interno { get; set; } = decimal.Zero;
        public decimal porc_afect_actsec_cotitu { get; set; } = decimal.Zero;
        public decimal porc_afect_macro { get; set; } = decimal.Zero;
        public decimal global_porc { get; set; } = decimal.Zero;
        public decimal porc_afect_sf { get; set; } = decimal.Zero;
        public decimal afect_adic_disp_global_pesos { get; set; } = decimal.Zero;
        public decimal cuota_total { get; set; } = decimal.Zero;
        public string resolucion_titu { get; set; } = string.Empty;
        public string VZ_ip { get; set; } = string.Empty;
        public decimal ltv { get; set; } = decimal.Zero;
        public string VZ_poblacion { get; set; } = string.Empty;
        public string VZ_score { get; set; } = string.Empty;
        public string calificacion_original { get; set; } = string.Empty;
        public string explicacion { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public string score_app { get; set; } = string.Empty;
        public decimal monto_recalculado_pesos { get; set; } = decimal.Zero;
        public decimal lim_tarj_recomendado_pesos { get; set; } = decimal.Zero;
        public decimal limite_acuerdo_recomendado_pesos { get; set; } = decimal.Zero;
        public decimal ingreso_cotitu { get; set; } = decimal.Zero;
        public decimal compromiso_bureau_cotitu { get; set; } = decimal.Zero;
        public string documento_cotitu1 { get; set; } = string.Empty;
        public string nombre_cotitu1 { get; set; } = string.Empty;
        public string apellido_cotitu1 { get; set; } = string.Empty;
        public string tipo_doc_cotitu1 { get; set; } = string.Empty;
        public string VZ_cuil_cotitu1 { get; set; } = string.Empty;
        public decimal ingreso_estimado_cotitu1 { get; set; } = decimal.Zero;
        public string resolucion_cotitu1 { get; set; } = string.Empty;
        public string VZ_ip_cotitu1 { get; set; } = string.Empty;
        public string VZ_score_cotitu1 { get; set; } = string.Empty;
        public string VZ_poblacion_cotitu1 { get; set; } = string.Empty;
        public string explicacion_cotitu1 { get; set; } = string.Empty;
        public string categoria_cotitu1 { get; set; } = string.Empty;
        public string documento_cotitu2 { get; set; } = string.Empty;
        public string nombre_cotitu2 { get; set; } = string.Empty;
        public string apellido_cotitu2 { get; set; } = string.Empty;
        public string tipo_doc_cotitu2 { get; set; } = string.Empty;
        public string VZ_cuil_cotitu2 { get; set; } = string.Empty;
        public decimal ingreso_estimado_cotitu2 { get; set; } = decimal.Zero;
        public string resolucion_cotitu2 { get; set; } = string.Empty;
        public string VZ_ip_cotitu2 { get; set; } = string.Empty;
        public string VZ_score_cotitu2 { get; set; } = string.Empty;
        public string VZ_poblacion_cotitu2 { get; set; } = string.Empty;
        public string explicacion_cotitu2 { get; set; } = string.Empty;
        public string categoria_cotitu2 { get; set; } = string.Empty;
        public string documento_cotitu3 { get; set; } = string.Empty;
        public string nombre_cotitu3 { get; set; } = string.Empty;
        public string apellido_cotitu3 { get; set; } = string.Empty;
        public string tipo_doc_cotitu3 { get; set; } = string.Empty;
        public string VZ_cuil_cotitu3 { get; set; } = string.Empty;
        public decimal ingreso_estimado_cotitu3 { get; set; } = decimal.Zero;
        public string resolucion_cotitu3 { get; set; } = string.Empty;
        public string VZ_ip_cotitu3 { get; set; } = string.Empty;
        public string VZ_score_cotitu3 { get; set; } = string.Empty;
        public string VZ_poblacion_cotitu3 { get; set; } = string.Empty;
        public string explicacion_cotitu3 { get; set; } = string.Empty;
        public string categoria_cotitu3 { get; set; } = string.Empty;
        public decimal ph_porc { get; set; } = decimal.Zero;
        public decimal ahorro { get; set; } = decimal.Zero;
        public string opcional_1 { get; set; } = string.Empty;
        public string opcional_2 { get; set; } = string.Empty;
        public string opcional_3 { get; set; } = string.Empty;
        public string opcional_4 { get; set; } = string.Empty;
        public string opcional_5 { get; set; } = string.Empty;
        public string opcional_6 { get; set; } = string.Empty;
        public string opcional_7 { get; set; } = string.Empty;
        public string opcional_8 { get; set; } = string.Empty;
        public string opcional_9 { get; set; } = string.Empty;
        public string opcional_10 { get; set; } = string.Empty;
        public string opcional_11 { get; set; } = string.Empty;
        public string opcional_12 { get; set; } = string.Empty;
        public string opcional_13 { get; set; } = string.Empty;
        public string opcional_14 { get; set; } = string.Empty;
        public string opcional_15 { get; set; } = string.Empty;
    }
}
