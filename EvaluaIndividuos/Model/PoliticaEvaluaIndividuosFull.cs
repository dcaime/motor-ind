using EvaluaIndividuos.Endpoints;
using System.Data;

namespace EvaluaIndividuos.Model
{
    public class PoliticaEvaluaIndividuosFull : IEvaluable
    {
        public PoliticaEvaluaIndividuosFullEntrada Entrada { get; set; }

        private PoliticaEvaluaIndividuosSalida _Salida;

        private Politica _Politica;

        public PoliticaEvaluaIndividuosFull()
        {
            _Politica = Politica.EvaluaIndividuosFull;
        }

        public PoliticaEvaluaIndividuosSalida Salida
        {
            get { return _Salida; }
        }

        public Politica GetPolitica()
        {
            return _Politica;
        }

        public string[] codificaEntrada()
        {
            //var diccionarioEntrada = new Dictionary<int, string>();
            var diccionarioEntrada = new string[_Politica.CamposEntrada];
            diccionarioEntrada[0] = Herramientas.ConvertirATexto(Entrada.Origen_ID);
            diccionarioEntrada[1] = Herramientas.ConvertirATexto(Entrada.Numero_Oportunidad);
            diccionarioEntrada[2] = Herramientas.ConvertirATexto(Entrada.Numero_Evaluacion);
            diccionarioEntrada[3] = Herramientas.ConvertirATexto(Entrada.Comercio_ID);
            diccionarioEntrada[4] = Herramientas.ConvertirATexto(Entrada.Comercio_Sucursal);
            diccionarioEntrada[5] = Herramientas.ConvertirATexto(Entrada.Comercio_Usuario);
            diccionarioEntrada[6] = Herramientas.ConvertirATexto(Entrada.Provincia);
            diccionarioEntrada[7] = Herramientas.ConvertirATexto(Entrada.Sucursal_Macro_Asociada);
            diccionarioEntrada[8] = Herramientas.ConvertirATexto(Entrada.Canal);
            diccionarioEntrada[9] = Herramientas.ConvertirATexto(Entrada.Productos_Precancelados);
            diccionarioEntrada[10] = Herramientas.ConvertirATexto(Entrada.Preca_Utiliza);
            diccionarioEntrada[11] = Herramientas.ConvertirATexto(Entrada.Preca_Codigo);
            diccionarioEntrada[12] = Herramientas.ConvertirATexto(Entrada.Preca_Oferta);
            diccionarioEntrada[13] = Herramientas.ConvertirATexto(Entrada.Circuito);
            diccionarioEntrada[14] = Herramientas.ConvertirATexto(Entrada.TIT_Cobis);
            diccionarioEntrada[15] = Herramientas.ConvertirATexto(Entrada.TIT_Cuil);
            diccionarioEntrada[16] = Herramientas.ConvertirATexto(Entrada.TIT_Numero_Documento);
            diccionarioEntrada[17] = Herramientas.ConvertirATexto(Entrada.TIT_Tipo_Documento);
            diccionarioEntrada[18] = Herramientas.ConvertirATexto(Entrada.TIT_Version_Documento);
            diccionarioEntrada[19] = Herramientas.ConvertirATexto(Entrada.TIT_Sexo);
            diccionarioEntrada[20] = Herramientas.ConvertirATexto(Entrada.TIT_Apellido);
            diccionarioEntrada[21] = Herramientas.ConvertirATexto(Entrada.TIT_Nombre);
            diccionarioEntrada[22] = Herramientas.ConvertirATexto(Entrada.TIT_Nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[23] = Herramientas.ConvertirATexto(Entrada.TIT_Estado_Civil);
            diccionarioEntrada[24] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_1);
            diccionarioEntrada[25] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_1_Fecha_Inicio, "", "yyyyMMdd");
            diccionarioEntrada[26] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_1_Neto_Fijo);
            diccionarioEntrada[27] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_1_Neto_Variable);
            diccionarioEntrada[28] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_1_Categoria_Monotributo);
            diccionarioEntrada[29] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_1_Actividad_Monotributo);
            diccionarioEntrada[30] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_2);
            diccionarioEntrada[31] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_2_Fecha_Inicio, "", "yyyyMMdd");
            diccionarioEntrada[32] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_2_Neto_Fijo);
            diccionarioEntrada[33] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_2_Neto_Variable);
            diccionarioEntrada[34] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_2_Categoria_Monotributo);
            diccionarioEntrada[35] = Herramientas.ConvertirATexto(Entrada.TIT_Actividad_2_Actividad_Monotributo);
            diccionarioEntrada[36] = Herramientas.ConvertirATexto(Entrada.TIT_Vivienda);
            diccionarioEntrada[37] = Herramientas.ConvertirATexto(Entrada.TIT_Alquiler_Monto);
            diccionarioEntrada[38] = Herramientas.ConvertirATexto(Entrada.TIT_Segmento);
            diccionarioEntrada[39] = Herramientas.ConvertirATexto(Entrada.TIT_Subsegmento);
            diccionarioEntrada[40] = Herramientas.ConvertirATexto(Entrada.TIT_Categoria_Cliente);
            diccionarioEntrada[41] = Herramientas.ConvertirATexto(Entrada.TIT_Numero_Convenio);
            diccionarioEntrada[42] = Herramientas.ConvertirATexto(Entrada.TIT_Nombre_Convenio);
            diccionarioEntrada[43] = Herramientas.ConvertirATexto(Entrada.TIT_Calificacion);
            diccionarioEntrada[44] = Herramientas.ConvertirATexto(Entrada.TIT_Submercado);
            diccionarioEntrada[45] = Herramientas.ConvertirATexto(Entrada.TIT_Monto_Inversor);
            diccionarioEntrada[46] = Herramientas.ConvertirATexto(Entrada.TIT_Tipo_Contribuyente);
            diccionarioEntrada[47] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_PP);
            diccionarioEntrada[48] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_PP);
            diccionarioEntrada[49] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_PR);
            diccionarioEntrada[50] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_PR);
            diccionarioEntrada[51] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_PH);
            diccionarioEntrada[52] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_PH);
            diccionarioEntrada[53] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_OP);
            diccionarioEntrada[54] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_OP);
            diccionarioEntrada[55] = Herramientas.ConvertirATexto(Entrada.SUM_limites_TC_Macro);
            diccionarioEntrada[56] = Herramientas.ConvertirATexto(Entrada.SUM_pago_min_prom_ult2_TC_Macro);
            diccionarioEntrada[57] = Herramientas.ConvertirATexto(Entrada.SUM_acuerdos_cc_Macro);
            diccionarioEntrada[58] = Herramientas.ConvertirATexto(Entrada.SUM_uso_prom_ult2_acuerdos_cc_Macro);
            diccionarioEntrada[59] = Herramientas.ConvertirATexto(Entrada.Cuota_prest_sin_gtia_no_bureau);
            diccionarioEntrada[60] = Herramientas.ConvertirATexto(Entrada.Cuota_prest_con_gtia_no_bureau);
            diccionarioEntrada[61] = Herramientas.ConvertirATexto(Entrada.Pago_min_tarj_no_bureau);
            diccionarioEntrada[62] = Herramientas.ConvertirATexto(Entrada.Util_cta_cte_no_bureau);
            diccionarioEntrada[63] = Herramientas.ConvertirATexto(Entrada.Solicita_Tarjeta);
            diccionarioEntrada[64] = Herramientas.ConvertirATexto(Entrada.Tarjeta_Limite);
            diccionarioEntrada[65] = Herramientas.ConvertirATexto(Entrada.Solicita_Prestamo);
            diccionarioEntrada[66] = Herramientas.ConvertirATexto(Entrada.Prestamo_Monto);
            diccionarioEntrada[67] = Herramientas.ConvertirATexto(Entrada.Prestamo_Destino);
            diccionarioEntrada[68] = Herramientas.ConvertirATexto(Entrada.Prestamo_Plazo);
            diccionarioEntrada[69] = Herramientas.ConvertirATexto(Entrada.Prestamo_Tipo_Tasa);
            diccionarioEntrada[70] = Herramientas.ConvertirATexto(Entrada.Prestamo_TNA);
            diccionarioEntrada[71] = Herramientas.ConvertirATexto(Entrada.Con_Garantia_Valor_Bien);
            diccionarioEntrada[72] = Herramientas.ConvertirATexto(Entrada.Con_Garantia_Subsidio);
            diccionarioEntrada[73] = Herramientas.ConvertirATexto(Entrada.Con_Garantia_Tasacion);
            diccionarioEntrada[74] = Herramientas.ConvertirATexto(Entrada.Hipotecario_Valor_Terreno);
            diccionarioEntrada[75] = Herramientas.ConvertirATexto(Entrada.Modelo_Automovil);
            diccionarioEntrada[76] = Herramientas.ConvertirATexto(Entrada.Solicita_Acuerdo);
            diccionarioEntrada[77] = Herramientas.ConvertirATexto(Entrada.Acuerdo_Monto);
            diccionarioEntrada[78] = Herramientas.ConvertirATexto(Entrada.COT_1_Cobis);
            diccionarioEntrada[79] = Herramientas.ConvertirATexto(Entrada.COT_1_Cuil);
            diccionarioEntrada[80] = Herramientas.ConvertirATexto(Entrada.COT_1_Numero_Documento);
            diccionarioEntrada[81] = Herramientas.ConvertirATexto(Entrada.COT_1_Tipo_Documento);
            diccionarioEntrada[82] = Herramientas.ConvertirATexto(Entrada.COT_1_Version_Documento);
            diccionarioEntrada[83] = Herramientas.ConvertirATexto(Entrada.COT_1_Sexo);
            diccionarioEntrada[84] = Herramientas.ConvertirATexto(Entrada.COT_1_Apellido);
            diccionarioEntrada[85] = Herramientas.ConvertirATexto(Entrada.COT_1_Nombre);
            diccionarioEntrada[86] = Herramientas.ConvertirATexto(Entrada.COT_1_Nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[87] = Herramientas.ConvertirATexto(Entrada.COT_1_Estado_Civil);
            diccionarioEntrada[88] = Herramientas.ConvertirATexto(Entrada.COT_1_Segmento);
            diccionarioEntrada[89] = Herramientas.ConvertirATexto(Entrada.COT_1_Subsegmento);
            diccionarioEntrada[90] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_1);
            diccionarioEntrada[91] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_1_Fecha, "", "yyyyMMdd");
            diccionarioEntrada[92] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_1_Neto_Fijo);
            diccionarioEntrada[93] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_1_Neto_Variable);
            diccionarioEntrada[94] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_1_Categoria_Monotributo);
            diccionarioEntrada[95] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_1_Actividad_Monotributo);
            diccionarioEntrada[96] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_2);
            diccionarioEntrada[97] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_2_Fecha, "", "yyyyMMdd");
            diccionarioEntrada[98] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_2_Neto_Fijo);
            diccionarioEntrada[99] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_2_Neto_Variable);
            diccionarioEntrada[100] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_2_Categoria_Monotributo);
            diccionarioEntrada[101] = Herramientas.ConvertirATexto(Entrada.COT_1_Actividad_2_Actividad_Monotributo);
            diccionarioEntrada[102] = Herramientas.ConvertirATexto(Entrada.COT_2_Cobis);
            diccionarioEntrada[103] = Herramientas.ConvertirATexto(Entrada.COT_2_Cuil);
            diccionarioEntrada[104] = Herramientas.ConvertirATexto(Entrada.COT_2_Numero_Documento);
            diccionarioEntrada[105] = Herramientas.ConvertirATexto(Entrada.COT_2_Tipo_Documento);
            diccionarioEntrada[106] = Herramientas.ConvertirATexto(Entrada.COT_2_Version_Documento);
            diccionarioEntrada[107] = Herramientas.ConvertirATexto(Entrada.COT_2_Sexo);
            diccionarioEntrada[108] = Herramientas.ConvertirATexto(Entrada.COT_2_Apellido);
            diccionarioEntrada[109] = Herramientas.ConvertirATexto(Entrada.COT_2_Nombre);
            diccionarioEntrada[110] = Herramientas.ConvertirATexto(Entrada.COT_2_Nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[111] = Herramientas.ConvertirATexto(Entrada.COT_2_Estado_Civil);
            diccionarioEntrada[112] = Herramientas.ConvertirATexto(Entrada.COT_2_Segmento);
            diccionarioEntrada[113] = Herramientas.ConvertirATexto(Entrada.COT_2_Subsegmento);
            diccionarioEntrada[114] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_1);
            diccionarioEntrada[115] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_1_Fecha, "", "yyyyMMdd");
            diccionarioEntrada[116] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_1_Neto_Fijo);
            diccionarioEntrada[117] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_1_Neto_Variable);
            diccionarioEntrada[118] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_1_Categoria_Monotributo);
            diccionarioEntrada[119] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_1_Actividad_Monotributo);
            diccionarioEntrada[120] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_2);
            diccionarioEntrada[121] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_2_Fecha, "", "yyyyMMdd");
            diccionarioEntrada[122] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_2_Neto_Fijo);
            diccionarioEntrada[123] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_2_Neto_Variable);
            diccionarioEntrada[124] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_2_Categoria_Monotributo);
            diccionarioEntrada[125] = Herramientas.ConvertirATexto(Entrada.COT_2_Actividad_2_Actividad_Monotributo);
            diccionarioEntrada[126] = Herramientas.ConvertirATexto(Entrada.COT_3_Cobis);
            diccionarioEntrada[127] = Herramientas.ConvertirATexto(Entrada.COT_3_Cuil);
            diccionarioEntrada[128] = Herramientas.ConvertirATexto(Entrada.COT_3_Numero_Documento);
            diccionarioEntrada[129] = Herramientas.ConvertirATexto(Entrada.COT_3_Tipo_Documento);
            diccionarioEntrada[130] = Herramientas.ConvertirATexto(Entrada.COT_3_Version_Documento);
            diccionarioEntrada[131] = Herramientas.ConvertirATexto(Entrada.COT_3_Sexo);
            diccionarioEntrada[132] = Herramientas.ConvertirATexto(Entrada.COT_3_Apellido);
            diccionarioEntrada[133] = Herramientas.ConvertirATexto(Entrada.COT_3_Nombre);
            diccionarioEntrada[134] = Herramientas.ConvertirATexto(Entrada.COT_3_Nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[135] = Herramientas.ConvertirATexto(Entrada.COT_3_Estado_Civil);
            diccionarioEntrada[136] = Herramientas.ConvertirATexto(Entrada.COT_3_Segmento);
            diccionarioEntrada[137] = Herramientas.ConvertirATexto(Entrada.COT_3_Subsegmento);
            diccionarioEntrada[138] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_1);
            diccionarioEntrada[139] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_1_Fecha, "", "yyyyMMdd");
            diccionarioEntrada[140] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_1_Neto_Fijo);
            diccionarioEntrada[141] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_1_Neto_Variable);
            diccionarioEntrada[142] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_1_Categoria_Monotributo);
            diccionarioEntrada[143] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_1_Actividad_Monotributo);
            diccionarioEntrada[144] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_2);
            diccionarioEntrada[145] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_2_Fecha, "", "yyyyMMdd");
            diccionarioEntrada[146] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_2_Neto_Fijo);
            diccionarioEntrada[147] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_2_Neto_Variable);
            diccionarioEntrada[148] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_2_Categoria_Monotributo);
            diccionarioEntrada[149] = Herramientas.ConvertirATexto(Entrada.COT_3_Actividad_2_Actividad_Monotributo);
            diccionarioEntrada[150] = Herramientas.ConvertirATexto(Entrada.Extra);
            diccionarioEntrada[151] = Herramientas.ConvertirATexto(Entrada.Seguro_Automovil);
            diccionarioEntrada[152] = Herramientas.ConvertirATexto(Entrada.ID_Transaccion);
            diccionarioEntrada[153] = Herramientas.ConvertirATexto(Entrada.CND_Codigo_Campana);
            diccionarioEntrada[154] = Herramientas.ConvertirATexto(Entrada.CLI_nombre);
            diccionarioEntrada[155] = Herramientas.ConvertirATexto(Entrada.CLI_apellido);
            diccionarioEntrada[156] = Herramientas.ConvertirATexto(Entrada.CLI_codigo_cliente);
            diccionarioEntrada[157] = Herramientas.ConvertirATexto(Entrada.CLI_nro_doc_tributario);
            diccionarioEntrada[158] = Herramientas.ConvertirATexto(Entrada.CLI_fecha_nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[159] = Herramientas.ConvertirATexto(Entrada.CLI_categoria_source);
            diccionarioEntrada[160] = Herramientas.ConvertirATexto(Entrada.CLI_clase);
            diccionarioEntrada[161] = Herramientas.ConvertirATexto(Entrada.CLI_restringido);
            diccionarioEntrada[162] = Herramientas.ConvertirATexto(Entrada.CLI_marca_pep);
            diccionarioEntrada[163] = Herramientas.ConvertirATexto(Entrada.CLI_fallecido);
            diccionarioEntrada[164] = Herramientas.ConvertirATexto(Entrada.CLI_banca);
            diccionarioEntrada[165] = Herramientas.ConvertirATexto(Entrada.CLI_segmento_source);
            diccionarioEntrada[166] = Herramientas.ConvertirATexto(Entrada.CLI_subsegmento_source);
            diccionarioEntrada[167] = Herramientas.ConvertirATexto(Entrada.CC_ID_CAMPANA);
            diccionarioEntrada[168] = Herramientas.ConvertirATexto(Entrada.CC_CODIGO_CAMPANA);
            diccionarioEntrada[169] = Herramientas.ConvertirATexto(Entrada.CC_SEGMENTO_CRM);
            diccionarioEntrada[170] = Herramientas.ConvertirATexto(Entrada.CC_SUBSEGMENTO_CRM);
            diccionarioEntrada[171] = Herramientas.ConvertirATexto(Entrada.CC_SUCURSAL_MIEMBRO);
            diccionarioEntrada[172] = Herramientas.ConvertirATexto(Entrada.CC_CANAL);
            diccionarioEntrada[173] = Herramientas.ConvertirATexto(Entrada.CC_TIPO_PRECALIFICACION);
            diccionarioEntrada[174] = Herramientas.ConvertirATexto(Entrada.CC_FECHA_CALIFICACION, "", "yyyyMMdd");
            diccionarioEntrada[175] = Herramientas.ConvertirATexto(Entrada.CC_FECHA_VENCIMIENTO, "", "yyyyMMdd");
            diccionarioEntrada[176] = Herramientas.ConvertirATexto(Entrada.CC_CONVENIO);
            diccionarioEntrada[177] = Herramientas.ConvertirATexto(Entrada.CC_PRODUCTO_PRESTAMO);
            diccionarioEntrada[178] = Herramientas.ConvertirATexto(Entrada.CC_PRODUCTO_TARJETA);
            diccionarioEntrada[179] = Herramientas.ConvertirATexto(Entrada.CC_PRODUCTO_ACUERDO);
            diccionarioEntrada[180] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_TASA);
            diccionarioEntrada[181] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_PLAZO);
            diccionarioEntrada[182] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_MONTO);
            diccionarioEntrada[183] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_AFECTACION);
            diccionarioEntrada[184] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_CUOTA_PROMEDIO);
            diccionarioEntrada[185] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_CUOTA_INICIAL);
            diccionarioEntrada[186] = Herramientas.ConvertirATexto(Entrada.CC_TARJETA_MONTO);
            diccionarioEntrada[187] = Herramientas.ConvertirATexto(Entrada.CC_ACUERDO_MONTO);
            diccionarioEntrada[188] = Herramientas.ConvertirATexto(Entrada.CC_ID_OFERTAS);
            diccionarioEntrada[189] = Herramientas.ConvertirATexto(Entrada.PRECACOM_CODIGO);
            diccionarioEntrada[190] = Herramientas.ConvertirATexto(Entrada.PRECACOM_CUIL);
            diccionarioEntrada[191] = Herramientas.ConvertirATexto(Entrada.PRECACOM_ORIGEN_ID);
            diccionarioEntrada[192] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COMERCIO_ID);
            diccionarioEntrada[193] = Herramientas.ConvertirATexto(Entrada.PRECACOM_ID_ALTERNATIVO);
            diccionarioEntrada[194] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO);
            diccionarioEntrada[195] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_PLAZO_MAXIMO);
            diccionarioEntrada[196] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_TNA);
            diccionarioEntrada[197] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_TIPO_TASA);
            diccionarioEntrada[198] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_CUOTA_MAXIMO);
            diccionarioEntrada[199] = Herramientas.ConvertirATexto(Entrada.PRECACOM_TARJETA_CREDITO_LIMITE);
            diccionarioEntrada[200] = Herramientas.ConvertirATexto(Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE);
            diccionarioEntrada[201] = Herramientas.ConvertirATexto(Entrada.PRECACOM_VIGENTE_DESDE, "", "yyyyMMdd");
            diccionarioEntrada[202] = Herramientas.ConvertirATexto(Entrada.PRECACOM_VIGENTE_HASTA, "", "yyyyMMdd");
            diccionarioEntrada[203] = Herramientas.ConvertirATexto(Entrada.PRECACOM_VIGENCIA);
            diccionarioEntrada[204] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COM1);
            diccionarioEntrada[205] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COM2);
            diccionarioEntrada[206] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COM3);
            diccionarioEntrada[207] = Herramientas.ConvertirATexto(Entrada.TIND_CODIGO_CLIENTE);
            diccionarioEntrada[208] = Herramientas.ConvertirATexto(Entrada.TIND_SEGMENTO);
            diccionarioEntrada[209] = Herramientas.ConvertirATexto(Entrada.TIND_CALIFICACION_CONV_PRINCIPAL);
            diccionarioEntrada[210] = Herramientas.ConvertirATexto(Entrada.TIND_NRO_CUENTA_PRINCIPAL);
            diccionarioEntrada[211] = Herramientas.ConvertirATexto(Entrada.TIND_COD_CATEGORIA_CUENTA);
            diccionarioEntrada[212] = Herramientas.ConvertirATexto(Entrada.TIND_MARCA_NO_PRECALIFICAR);
            diccionarioEntrada[213] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_SITUACION_BM);
            diccionarioEntrada[214] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_SIT_CART_CEDIDA);
            diccionarioEntrada[215] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_MORA_HISTORICA);
            diccionarioEntrada[216] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_BANCA_CLIENTE);
            diccionarioEntrada[217] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_REFINANCIACION);
            diccionarioEntrada[218] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[219] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_DIFERIMIENTO);
            diccionarioEntrada[220] = Herramientas.ConvertirATexto(Entrada.TIND_CATEGORIA_MONOTRIBUTO);
            diccionarioEntrada[221] = Herramientas.ConvertirATexto(Entrada.TIND_ACTIVIDAD_MONOTRIBUTO);
            diccionarioEntrada[222] = Herramientas.ConvertirATexto(Entrada.TIND_MONTO_INVERSION);
            diccionarioEntrada[223] = Herramientas.ConvertirATexto(Entrada.TIND_NO_ACREDITO_ULT_MES);
            diccionarioEntrada[224] = Herramientas.ConvertirATexto(Entrada.TIND_NO_ACREDITO_ULT_2M);
            diccionarioEntrada[225] = Herramientas.ConvertirATexto(Entrada.TIND_INGRESO_CALCULADO_FINAL);
            diccionarioEntrada[226] = Herramientas.ConvertirATexto(Entrada.TIND_TIPO_INGRESO);
            diccionarioEntrada[227] = Herramientas.ConvertirATexto(Entrada.TIND_CONVENIO_LIQUI_PRESTAMO);
            diccionarioEntrada[228] = Herramientas.ConvertirATexto(Entrada.TIND_ENTE_CONVENIO);
            diccionarioEntrada[229] = Herramientas.ConvertirATexto(Entrada.TIND_MERCADO);
            diccionarioEntrada[230] = Herramientas.ConvertirATexto(Entrada.TIND_SUBMERCADO);
            diccionarioEntrada[231] = Herramientas.ConvertirATexto(Entrada.TIND_SIN_MERC_SUBM);
            diccionarioEntrada[232] = Herramientas.ConvertirATexto(Entrada.TIND_COD_SEGMENTO_CRM);
            diccionarioEntrada[233] = Herramientas.ConvertirATexto(Entrada.TIND_COD_SUBSEGMENTO_CRM);
            diccionarioEntrada[234] = Herramientas.ConvertirATexto(Entrada.TIND_SEXO);
            diccionarioEntrada[235] = Herramientas.ConvertirATexto(Entrada.EA_CODIGO_CLIENTE);
            diccionarioEntrada[236] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_PP);
            diccionarioEntrada[237] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_PP);
            diccionarioEntrada[238] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO);
            diccionarioEntrada[239] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO);
            diccionarioEntrada[240] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO);
            diccionarioEntrada[241] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO);
            diccionarioEntrada[242] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS);
            diccionarioEntrada[243] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS);
            diccionarioEntrada[244] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_ATRASO_PRESTAMOS);
            diccionarioEntrada[245] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[246] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_REFINANCIACION);
            diccionarioEntrada[247] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_DEU);
            diccionarioEntrada[248] = Herramientas.ConvertirATexto(Entrada.EA_DIAS_ATRASO_TC);
            diccionarioEntrada[249] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_DESC_SIN_ACUERDO);
            diccionarioEntrada[250] = Herramientas.ConvertirATexto(Entrada.EA_LIMITE_ACUERDO);
            diccionarioEntrada[251] = Herramientas.ConvertirATexto(Entrada.EA_UTILIZACION_ACUERDO);
            diccionarioEntrada[252] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_CLEAN_UP);
            diccionarioEntrada[253] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_EXCESO_ACUERDO);
            diccionarioEntrada[254] = Herramientas.ConvertirATexto(Entrada.EA_LIMITE_COMPRA_TC);
            diccionarioEntrada[255] = Herramientas.ConvertirATexto(Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES);
            diccionarioEntrada[256] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad1);
            diccionarioEntrada[257] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad1);
            diccionarioEntrada[258] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad2);
            diccionarioEntrada[259] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad2);
            diccionarioEntrada[260] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad3);
            diccionarioEntrada[261] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad3);
            diccionarioEntrada[262] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad4);
            diccionarioEntrada[263] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad4);
            diccionarioEntrada[264] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad5);
            diccionarioEntrada[265] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad5);
            diccionarioEntrada[266] = Herramientas.ConvertirATexto(Entrada.BCRADEU_origen);
            diccionarioEntrada[267] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_ireg_ult_24m);
            diccionarioEntrada[268] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Peor_sit_vigente);
            diccionarioEntrada[269] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Peor_sit_ult_6m);
            diccionarioEntrada[270] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_irreg_ult_periodo);
            diccionarioEntrada[271] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Peor_sit_ult_3m);
            diccionarioEntrada[272] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_2_ult_3m);
            diccionarioEntrada[273] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m);
            diccionarioEntrada[274] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_Codigo_campana);
            diccionarioEntrada[275] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_FECHA_FIN_PREVISTO, "", "yyyyMMdd");
            diccionarioEntrada[276] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_ESTADO);
            diccionarioEntrada[277] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_BCRA_EXCEPTUADO);
            diccionarioEntrada[278] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO);
            diccionarioEntrada[279] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_SUCURSAL_EXCLUSIVA);
            diccionarioEntrada[280] = Herramientas.ConvertirATexto(Entrada.LN_Cant);
            diccionarioEntrada[281] = Herramientas.ConvertirATexto(Entrada.LN_Motivo);
            diccionarioEntrada[282] = Herramientas.ConvertirATexto(Entrada.VA_ES_AUTONOMO);
            diccionarioEntrada[283] = Herramientas.ConvertirATexto(Entrada.VA_ES_EMPLEADO);
            diccionarioEntrada[284] = Herramientas.ConvertirATexto(Entrada.VA_ES_MONOTRIBUTISTA);
            diccionarioEntrada[285] = Herramientas.ConvertirATexto(Entrada.dias_vigencia);
            diccionarioEntrada[286] = Herramientas.ConvertirATexto(Entrada.PM_Ajuste_monto_Camuzzi);
            diccionarioEntrada[287] = Herramientas.ConvertirATexto(Entrada.PM_Ajuste_monto);
            diccionarioEntrada[288] = Herramientas.ConvertirATexto(Entrada.PM_Ajuste_cuota_maxima);
            diccionarioEntrada[289] = Herramientas.ConvertirATexto(Entrada.PM_PrecaAConsultar);
            return diccionarioEntrada;
        }

        public void decodificaSalida(string[] arraySalida)
        {
            _Salida = new PoliticaEvaluaIndividuosSalida();
            _Salida.Origen_ID = (int)Herramientas.ConvertirDeTexto(arraySalida[0], DbType.Int32, 0);
            _Salida.Numero_Oportunidad = (long)Herramientas.ConvertirDeTexto(arraySalida[1], DbType.Int64, 0);
            _Salida.Numero_Evaluacion = (int)Herramientas.ConvertirDeTexto(arraySalida[2], DbType.Int32, 0);
            _Salida.Es_Cliente = (string)Herramientas.ConvertirDeTexto(arraySalida[5], DbType.String, "");
            _Salida.Cobis = (int)Herramientas.ConvertirDeTexto(arraySalida[6], DbType.Int32, 0);
            _Salida.Cuil = (long)Herramientas.ConvertirDeTexto(arraySalida[7], DbType.Int64, 0);
            _Salida.Rama = (string)Herramientas.ConvertirDeTexto(arraySalida[8], DbType.String, "");
            _Salida.Dictamen = (string)Herramientas.ConvertirDeTexto(arraySalida[9], DbType.String, "");
            _Salida.Codigos_Rechazo = (string)Herramientas.ConvertirDeTexto(arraySalida[10], DbType.String, "");
            _Salida.Explicacion = (string)Herramientas.ConvertirDeTexto(arraySalida[11], DbType.String, "");
            _Salida.Prestamo_Monto_Maximo = (decimal)Herramientas.ConvertirDeTexto(arraySalida[12], DbType.Decimal, decimal.Zero);
            _Salida.Prestamo_Cuota_Maxima = (decimal)Herramientas.ConvertirDeTexto(arraySalida[13], DbType.Decimal, decimal.Zero);
            _Salida.Prestamo_Plazo_Maximo = Convert.ToInt16((decimal)Herramientas.ConvertirDeTexto(arraySalida[14], DbType.Decimal, decimal.Zero));
            _Salida.Prestamo_Tipo_Tasa = (string)Herramientas.ConvertirDeTexto(arraySalida[15], DbType.String, "");
            _Salida.Prestamo_Tna = (decimal)Herramientas.ConvertirDeTexto(arraySalida[16], DbType.Decimal, decimal.Zero);
            _Salida.Tarjeta_Credito_Limite = (decimal)Herramientas.ConvertirDeTexto(arraySalida[17], DbType.Decimal, decimal.Zero);
            _Salida.Cuenta_Corriente_Limite = (decimal)Herramientas.ConvertirDeTexto(arraySalida[18], DbType.Decimal, decimal.Zero);
            _Salida.Reporte_Politica = (string)Herramientas.ConvertirDeTexto(arraySalida[19], DbType.String, "");
            _Salida.Output_Adicional = (string)Herramientas.ConvertirDeTexto(arraySalida[20], DbType.String, "");
            _Salida.Nivel_de_Riesgo = (string)Herramientas.ConvertirDeTexto(arraySalida[21], DbType.String, "");
            _Salida.Ingreso_Computable = (decimal)Herramientas.ConvertirDeTexto(arraySalida[22], DbType.Decimal, decimal.Zero);
        }
    }

    public class PoliticaEvaluaIndividuosFullEntrada
    {
        public int? Origen_ID { get; set; }
        public long? Numero_Oportunidad { get; set; }
        public int? Numero_Evaluacion { get; set; }
        public int? Comercio_ID { get; set; }
        public string? Comercio_Sucursal { get; set; }
        public string Comercio_Usuario { get; set; }
        public string Provincia { get; set; }
        public int? Sucursal_Macro_Asociada { get; set; }
        public string Canal { get; set; }
        public string Productos_Precancelados { get; set; }
        public string Preca_Utiliza { get; set; }
        public string Preca_Codigo { get; set; }
        public string Preca_Oferta { get; set; }
        public string Circuito { get; set; }
        public int? TIT_Cobis { get; set; }
        public long? TIT_Cuil { get; set; }
        public int? TIT_Numero_Documento { get; set; }
        public string TIT_Tipo_Documento { get; set; }
        public string TIT_Version_Documento { get; set; }
        public string TIT_Sexo { get; set; }
        public string TIT_Apellido { get; set; }
        public string TIT_Nombre { get; set; }
        public DateTime? TIT_Nacimiento { get; set; }
        public string TIT_Estado_Civil { get; set; }
        public string TIT_Actividad_1 { get; set; }
        public DateTime? TIT_Actividad_1_Fecha_Inicio { get; set; }
        public decimal? TIT_Actividad_1_Neto_Fijo { get; set; }
        public decimal? TIT_Actividad_1_Neto_Variable { get; set; }
        public string TIT_Actividad_1_Categoria_Monotributo { get; set; }
        public string TIT_Actividad_1_Actividad_Monotributo { get; set; }
        public string TIT_Actividad_2 { get; set; }
        public DateTime? TIT_Actividad_2_Fecha_Inicio { get; set; }
        public decimal? TIT_Actividad_2_Neto_Fijo { get; set; }
        public decimal? TIT_Actividad_2_Neto_Variable { get; set; }
        public string TIT_Actividad_2_Categoria_Monotributo { get; set; }
        public string TIT_Actividad_2_Actividad_Monotributo { get; set; }
        public string TIT_Vivienda { get; set; }
        public decimal? TIT_Alquiler_Monto { get; set; }
        public string TIT_Segmento { get; set; }
        public string TIT_Subsegmento { get; set; }
        public string TIT_Categoria_Cliente { get; set; }
        public int? TIT_Numero_Convenio { get; set; }
        public string TIT_Nombre_Convenio { get; set; }
        public string TIT_Calificacion { get; set; }
        public string TIT_Submercado { get; set; }
        public decimal? TIT_Monto_Inversor { get; set; }
        public string TIT_Tipo_Contribuyente { get; set; }
        public decimal? SUM_cuota_actual_PP { get; set; }
        public decimal? SUM_monto_deuda_actual_PP { get; set; }
        public decimal? SUM_cuota_actual_PR { get; set; }
        public decimal? SUM_monto_deuda_actual_PR { get; set; }
        public decimal? SUM_cuota_actual_PH { get; set; }
        public decimal? SUM_monto_deuda_actual_PH { get; set; }
        public decimal? SUM_cuota_actual_OP { get; set; }
        public decimal? SUM_monto_deuda_actual_OP { get; set; }
        public decimal? SUM_limites_TC_Macro { get; set; }
        public decimal? SUM_pago_min_prom_ult2_TC_Macro { get; set; }
        public decimal? SUM_acuerdos_cc_Macro { get; set; }
        public decimal? SUM_uso_prom_ult2_acuerdos_cc_Macro { get; set; }
        public decimal? Cuota_prest_sin_gtia_no_bureau { get; set; }
        public decimal? Cuota_prest_con_gtia_no_bureau { get; set; }
        public decimal? Pago_min_tarj_no_bureau { get; set; }
        public decimal? Util_cta_cte_no_bureau { get; set; }
        public string Solicita_Tarjeta { get; set; }
        public decimal? Tarjeta_Limite { get; set; }
        public string Solicita_Prestamo { get; set; }
        public decimal? Prestamo_Monto { get; set; }
        public string Prestamo_Destino { get; set; }
        public int? Prestamo_Plazo { get; set; }
        public string Prestamo_Tipo_Tasa { get; set; }
        public decimal? Prestamo_TNA { get; set; }
        public decimal? Con_Garantia_Valor_Bien { get; set; }
        public decimal? Con_Garantia_Subsidio { get; set; }
        public decimal? Con_Garantia_Tasacion { get; set; }
        public decimal? Hipotecario_Valor_Terreno { get; set; }
        public int? Modelo_Automovil { get; set; }
        public string Solicita_Acuerdo { get; set; }
        public decimal? Acuerdo_Monto { get; set; }
        public int? COT_1_Cobis { get; set; }
        public long? COT_1_Cuil { get; set; }
        public int? COT_1_Numero_Documento { get; set; }
        public string COT_1_Tipo_Documento { get; set; }
        public string COT_1_Version_Documento { get; set; }
        public string COT_1_Sexo { get; set; }
        public string COT_1_Apellido { get; set; }
        public string COT_1_Nombre { get; set; }
        public DateTime? COT_1_Nacimiento { get; set; }
        public string COT_1_Estado_Civil { get; set; }
        public string COT_1_Segmento { get; set; }
        public string COT_1_Subsegmento { get; set; }
        public string COT_1_Actividad_1 { get; set; }
        public DateTime? COT_1_Actividad_1_Fecha { get; set; }
        public decimal? COT_1_Actividad_1_Neto_Fijo { get; set; }
        public decimal? COT_1_Actividad_1_Neto_Variable { get; set; }
        public string COT_1_Actividad_1_Categoria_Monotributo { get; set; }
        public string COT_1_Actividad_1_Actividad_Monotributo { get; set; }
        public string COT_1_Actividad_2 { get; set; }
        public DateTime? COT_1_Actividad_2_Fecha { get; set; }
        public decimal? COT_1_Actividad_2_Neto_Fijo { get; set; }
        public decimal? COT_1_Actividad_2_Neto_Variable { get; set; }
        public string COT_1_Actividad_2_Categoria_Monotributo { get; set; }
        public string COT_1_Actividad_2_Actividad_Monotributo { get; set; }
        public int? COT_2_Cobis { get; set; }
        public long? COT_2_Cuil { get; set; }
        public int? COT_2_Numero_Documento { get; set; }
        public string COT_2_Tipo_Documento { get; set; }
        public string COT_2_Version_Documento { get; set; }
        public string COT_2_Sexo { get; set; }
        public string COT_2_Apellido { get; set; }
        public string COT_2_Nombre { get; set; }
        public DateTime? COT_2_Nacimiento { get; set; }
        public string COT_2_Estado_Civil { get; set; }
        public string COT_2_Segmento { get; set; }
        public string COT_2_Subsegmento { get; set; }
        public string COT_2_Actividad_1 { get; set; }
        public DateTime? COT_2_Actividad_1_Fecha { get; set; }
        public decimal? COT_2_Actividad_1_Neto_Fijo { get; set; }
        public decimal? COT_2_Actividad_1_Neto_Variable { get; set; }
        public string COT_2_Actividad_1_Categoria_Monotributo { get; set; }
        public string COT_2_Actividad_1_Actividad_Monotributo { get; set; }
        public string COT_2_Actividad_2 { get; set; }
        public DateTime? COT_2_Actividad_2_Fecha { get; set; }
        public decimal? COT_2_Actividad_2_Neto_Fijo { get; set; }
        public decimal? COT_2_Actividad_2_Neto_Variable { get; set; }
        public string COT_2_Actividad_2_Categoria_Monotributo { get; set; }
        public string COT_2_Actividad_2_Actividad_Monotributo { get; set; }
        public int? COT_3_Cobis { get; set; }
        public long? COT_3_Cuil { get; set; }
        public int? COT_3_Numero_Documento { get; set; }
        public string COT_3_Tipo_Documento { get; set; }
        public string COT_3_Version_Documento { get; set; }
        public string COT_3_Sexo { get; set; }
        public string COT_3_Apellido { get; set; }
        public string COT_3_Nombre { get; set; }
        public DateTime? COT_3_Nacimiento { get; set; }
        public string COT_3_Estado_Civil { get; set; }
        public string COT_3_Segmento { get; set; }
        public string COT_3_Subsegmento { get; set; }
        public string COT_3_Actividad_1 { get; set; }
        public DateTime? COT_3_Actividad_1_Fecha { get; set; }
        public decimal? COT_3_Actividad_1_Neto_Fijo { get; set; }
        public decimal? COT_3_Actividad_1_Neto_Variable { get; set; }
        public string COT_3_Actividad_1_Categoria_Monotributo { get; set; }
        public string COT_3_Actividad_1_Actividad_Monotributo { get; set; }
        public string COT_3_Actividad_2 { get; set; }
        public DateTime? COT_3_Actividad_2_Fecha { get; set; }
        public decimal? COT_3_Actividad_2_Neto_Fijo { get; set; }
        public decimal? COT_3_Actividad_2_Neto_Variable { get; set; }
        public string COT_3_Actividad_2_Categoria_Monotributo { get; set; }
        public string COT_3_Actividad_2_Actividad_Monotributo { get; set; }
        public string Extra { get; set; }
        public decimal? Seguro_Automovil { get; set; }
        public string ID_Transaccion { get; set; }
        public int? NUC_Numero { get; set; }
        public string NUC_Tipo { get; set; }
        public string NUC_Medio { get; set; }
        public string NUC_Adicional { get; set; }
        public string CND_Codigo_Campana { get; set; }
        public string CLI_nombre { get; set; }
        public string CLI_apellido { get; set; }
        public string CLI_codigo_cliente { get; set; }
        public string CLI_nro_doc_tributario { get; set; }
        public DateTime? CLI_fecha_nacimiento { get; set; }
        public string CLI_categoria_source { get; set; }
        public string CLI_clase { get; set; }
        public string CLI_restringido { get; set; }
        public string CLI_marca_pep { get; set; }
        public string CLI_fallecido { get; set; }
        public string CLI_banca { get; set; }
        public string CLI_segmento_source { get; set; }
        public string CLI_subsegmento_source { get; set; }
        public string CC_ID_CAMPANA { get; set; }
        public string CC_CODIGO_CAMPANA { get; set; }
        public string CC_SEGMENTO_CRM { get; set; }
        public string CC_SUBSEGMENTO_CRM { get; set; }
        public int? CC_SUCURSAL_MIEMBRO { get; set; }
        public string CC_CANAL { get; set; }
        public string CC_TIPO_PRECALIFICACION { get; set; }
        public DateTime? CC_FECHA_CALIFICACION { get; set; }
        public DateTime? CC_FECHA_VENCIMIENTO { get; set; }
        public string CC_CONVENIO { get; set; }
        public string CC_PRODUCTO_PRESTAMO { get; set; }
        public string CC_PRODUCTO_TARJETA { get; set; }
        public string CC_PRODUCTO_ACUERDO { get; set; }
        public Decimal? CC_PRESTAMO_TASA { get; set; }
        public int? CC_PRESTAMO_PLAZO { get; set; }
        public string CC_PRESTAMO_MONTO { get; set; }
        public Int64? CC_PRESTAMO_AFECTACION { get; set; }
        public Int64? CC_PRESTAMO_CUOTA_PROMEDIO { get; set; }
        public Int64? CC_PRESTAMO_CUOTA_INICIAL { get; set; }
        public Int64? CC_TARJETA_MONTO { get; set; }
        public Int64? CC_ACUERDO_MONTO { get; set; }
        public string CC_ID_OFERTAS { get; set; }
        public string PRECACOM_CODIGO { get; set; }
        public Int64? PRECACOM_CUIL { get; set; }
        public Int64? PRECACOM_ORIGEN_ID { get; set; }
        public Int64? PRECACOM_COMERCIO_ID { get; set; }
        public Int64? PRECACOM_ID_ALTERNATIVO { get; set; }
        public Int64? PRECACOM_PRESTAMO_MONTO_MAXIMO { get; set; }
        public Int64? PRECACOM_PRESTAMO_PLAZO_MAXIMO { get; set; }
        public Decimal? PRECACOM_PRESTAMO_TNA { get; set; }
        public string PRECACOM_PRESTAMO_TIPO_TASA { get; set; }
        public Int64? PRECACOM_PRESTAMO_CUOTA_MAXIMO { get; set; }
        public Int64? PRECACOM_TARJETA_CREDITO_LIMITE { get; set; }
        public Int64? PRECACOM_CUENTA_CORRIENTE_LIMITE { get; set; }
        public DateTime? PRECACOM_VIGENTE_DESDE { get; set; }
        public DateTime? PRECACOM_VIGENTE_HASTA { get; set; }
        public int? PRECACOM_VIGENCIA { get; set; }
        public string PRECACOM_COM1 { get; set; }
        public string PRECACOM_COM2 { get; set; }
        public string PRECACOM_COM3 { get; set; }
        public string TIND_CODIGO_CLIENTE { get; set; }
        public string TIND_SEGMENTO { get; set; }
        public string TIND_CALIFICACION_CONV_PRINCIPAL { get; set; }
        public string TIND_NRO_CUENTA_PRINCIPAL { get; set; }
        public string TIND_COD_CATEGORIA_CUENTA { get; set; }
        public int? TIND_MARCA_NO_PRECALIFICAR { get; set; }
        public int? TIND_MOTIVO_SITUACION_BM { get; set; }
        public int? TIND_MOTIVO_SIT_CART_CEDIDA { get; set; }
        public int? TIND_MOTIVO_MORA_HISTORICA { get; set; }
        public int? TIND_MOTIVO_BANCA_CLIENTE { get; set; }
        public int? TIND_MOTIVO_REFINANCIACION { get; set; }
        public int? TIND_MOTIVO_REESTRUCTURACION { get; set; }
        public int? TIND_MOTIVO_DIFERIMIENTO { get; set; }
        public string TIND_CATEGORIA_MONOTRIBUTO { get; set; }
        public string TIND_ACTIVIDAD_MONOTRIBUTO { get; set; }
        public Decimal? TIND_MONTO_INVERSION { get; set; }
        public int? TIND_NO_ACREDITO_ULT_MES { get; set; }
        public int? TIND_NO_ACREDITO_ULT_2M { get; set; }
        public Decimal? TIND_INGRESO_CALCULADO_FINAL { get; set; }
        public string TIND_TIPO_INGRESO { get; set; }
        public string TIND_CONVENIO_LIQUI_PRESTAMO { get; set; }
        public string TIND_ENTE_CONVENIO { get; set; }
        public string TIND_MERCADO { get; set; }
        public string TIND_SUBMERCADO { get; set; }
        public int? TIND_SIN_MERC_SUBM { get; set; }
        public string TIND_COD_SEGMENTO_CRM { get; set; }
        public string TIND_COD_SUBSEGMENTO_CRM { get; set; }
        public string TIND_SEXO { get; set; }
        public Int64? EA_CODIGO_CLIENTE { get; set; }
        public Decimal? EA_SALDO_CONTABLE_PP { get; set; }
        public Decimal? EA_MONTO_PROXIMA_CUOTA_PP { get; set; }
        public Decimal? EA_SALDO_CONTABLE_PREST_PRENDARIO { get; set; }
        public Decimal? EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO { get; set; }
        public Decimal? EA_SALDO_CONTABLE_PREST_HIPOTECARIO { get; set; }
        public Decimal? EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO { get; set; }
        public Decimal? EA_SALDO_CONTABLE_OTROS_PRESTAMOS { get; set; }
        public Decimal? EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS { get; set; }
        public int? EA_MAX_DIAS_ATRASO_PRESTAMOS { get; set; }
        public string EA_MOTIVO_REESTRUCTURACION { get; set; }
        public string EA_MOTIVO_REFINANCIACION { get; set; }
        public string EA_MOTIVO_DEU { get; set; }
        public int? EA_DIAS_ATRASO_TC { get; set; }
        public int? EA_MAX_DIAS_DESC_SIN_ACUERDO { get; set; }
        public Decimal? EA_LIMITE_ACUERDO { get; set; }
        public Decimal? EA_UTILIZACION_ACUERDO { get; set; }
        public int? EA_MAX_DIAS_CLEAN_UP { get; set; }
        public int? EA_MAX_DIAS_EXCESO_ACUERDO { get; set; }
        public Decimal? EA_LIMITE_COMPRA_TC { get; set; }
        public Decimal? EA_PAGO_MINIMO_PROM_ULT_2_MESES { get; set; }
        public string BCRADEU_situacion_entidad1 { get; set; }
        public Decimal? BCRADEU_monto_entidad1 { get; set; }
        public string BCRADEU_situacion_entidad2 { get; set; }
        public Decimal? BCRADEU_monto_entidad2 { get; set; }
        public string BCRADEU_situacion_entidad3 { get; set; }
        public Decimal? BCRADEU_monto_entidad3 { get; set; }
        public string BCRADEU_situacion_entidad4 { get; set; }
        public Decimal? BCRADEU_monto_entidad4 { get; set; }
        public string BCRADEU_situacion_entidad5 { get; set; }
        public Decimal? BCRADEU_monto_entidad5 { get; set; }
        public string BCRADEU_origen { get; set; }
        public int? BCRADEU_Cant_de_sit_ireg_ult_24m { get; set; }
        public int? BCRADEU_Peor_sit_vigente { get; set; }
        public int? BCRADEU_Peor_sit_ult_6m { get; set; }
        public int? BCRADEU_Cant_de_sit_irreg_ult_periodo { get; set; }
        public int? BCRADEU_Peor_sit_ult_3m { get; set; }
        public int? BCRADEU_Cant_de_sit_2_ult_3m { get; set; }
        public int? BCRADEU_Cant_de_sit_irreg_ult_3m { get; set; }
        public string CAMPEXCEP_Codigo_campana { get; set; }
        public DateTime? CAMPEXCEP_FECHA_FIN_PREVISTO { get; set; }
        public string CAMPEXCEP_ESTADO { get; set; }
        public string CAMPEXCEP_BCRA_EXCEPTUADO { get; set; }
        public string CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO { get; set; }
        public string CAMPEXCEP_SUCURSAL_EXCLUSIVA { get; set; }
        public int? LN_Cant { get; set; }
        public string LN_Motivo { get; set; }
        public string VA_ES_AUTONOMO { get; set; }
        public string VA_ES_EMPLEADO { get; set; }
        public string VA_ES_MONOTRIBUTISTA { get; set; }
        public int? dias_vigencia { get; set; }
        public Decimal? PM_Ajuste_monto_Camuzzi { get; set; }
        public Decimal? PM_Ajuste_monto { get; set; }
        public Decimal? PM_Ajuste_cuota_maxima { get; set; }
        public string PM_PrecaAConsultar { get; set; }
    }

    public class PoliticaEvaluaIndividuosFullSalida
    {
        public int Origen_ID { get; set; }
        public long Numero_Oportunidad { get; set; }
        public int Numero_Evaluacion { get; set; }
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public string Es_Cliente { get; set; }
        public int Cobis { get; set; }
        public long Cuil { get; set; }
        public string Rama { get; set; }
        public string Dictamen { get; set; }
        public string Codigos_Rechazo { get; set; }
        public string Explicacion { get; set; }
        public decimal Prestamo_Monto_Maximo { get; set; }
        public decimal Prestamo_Cuota_Maxima { get; set; }
        public int Prestamo_Plazo_Maximo { get; set; }
        public string Prestamo_Tipo_Tasa { get; set; }
        public decimal Prestamo_Tna { get; set; }
        public decimal Tarjeta_Credito_Limite { get; set; }
        public decimal Cuenta_Corriente_Limite { get; set; }
        public string Reporte_Politica { get; set; }
        public string Output_Adicional { get; set; }
        public string Nivel_de_Riesgo { get; set; }
        public decimal Ingreso_Computable { get; set; }
    }

    public class HerramientasFull
    {
        public static PoliticaEvaluaIndividuosFullEntrada MapeaEntrada(RequestEvaluaIndividuosArray requestArray)
        {
            var request = requestArray.element;
            var entrada = new PoliticaEvaluaIndividuosFullEntrada();
            entrada.Origen_ID = request.OrigenId;
            entrada.Numero_Oportunidad = request.NumeroOportunidad;
            entrada.Numero_Evaluacion = request.NumeroEvaluacion;
            entrada.Comercio_ID = request.ComercioId;
            entrada.Comercio_Sucursal = request.ComercioSucursal;
            entrada.Comercio_Usuario = request.ComercioUsuario;
            entrada.Provincia = request.Provincia;
            entrada.Sucursal_Macro_Asociada = request.SucursalMacroAsociada;
            entrada.Canal = request.Canal;
            if (request.ProductosPrecancelados is not null)
            {
                entrada.Productos_Precancelados = string.Join("|", request.ProductosPrecancelados);
            }
            entrada.Preca_Utiliza = request.PrecalificacionUtiliza;
            entrada.Preca_Codigo = request.PrecalificacionCodigo;
            entrada.Preca_Oferta = request.PrecalificacionOferta;
            entrada.Circuito = request.Circuito;
            if (request.Titular is not null)
            {
                entrada.TIT_Cobis = request.Titular.Cobis;
                entrada.TIT_Cuil = request.Titular.NumeroCUIL;
                entrada.TIT_Numero_Documento = request.Titular.NumeroDocumento;
                entrada.TIT_Tipo_Documento = request.Titular.TipoDocumento;
                entrada.TIT_Version_Documento = request.Titular.VersionDocumento;
                entrada.TIT_Sexo = request.Titular.Sexo;
                entrada.TIT_Apellido = request.Titular.Apellido;
                entrada.TIT_Nombre = request.Titular.Nombre;
                entrada.TIT_Nacimiento = request.Titular.FechaNacimiento;
                entrada.TIT_Estado_Civil = request.Titular.EstadoCivil;
                if (request.Titular.Actividad1 is not null)
                {
                    entrada.TIT_Actividad_1 = request.Titular.Actividad1.Actividad;
                    entrada.TIT_Actividad_1_Fecha_Inicio = request.Titular.Actividad1.FechaInicio;
                    entrada.TIT_Actividad_1_Neto_Fijo = request.Titular.Actividad1.NetoFijo;
                    entrada.TIT_Actividad_1_Neto_Variable = request.Titular.Actividad1.NetoVariable;
                    entrada.TIT_Actividad_1_Categoria_Monotributo = request.Titular.Actividad1.CategoriaMonotributo;
                    entrada.TIT_Actividad_1_Actividad_Monotributo = request.Titular.Actividad1.ActividadMonotributo;
                }
                if (request.Titular.Actividad2 is not null)
                {
                    entrada.TIT_Actividad_2 = request.Titular.Actividad2.Actividad;
                    entrada.TIT_Actividad_2_Fecha_Inicio = request.Titular.Actividad2.FechaInicio;
                    entrada.TIT_Actividad_2_Neto_Fijo = request.Titular.Actividad2.NetoFijo;
                    entrada.TIT_Actividad_2_Neto_Variable = request.Titular.Actividad2.NetoVariable;
                    entrada.TIT_Actividad_2_Categoria_Monotributo = request.Titular.Actividad2.CategoriaMonotributo;
                    entrada.TIT_Actividad_2_Actividad_Monotributo = request.Titular.Actividad2.ActividadMonotributo;
                }
                entrada.TIT_Vivienda = request.Titular.Vivienda;
                entrada.TIT_Alquiler_Monto = request.Titular.AlquilerMonto;
                entrada.TIT_Segmento = request.Titular.Segmento;
                entrada.TIT_Subsegmento = request.Titular.Subsegmento;
                entrada.TIT_Categoria_Cliente = request.Titular.CategoriaCliente;
                entrada.TIT_Numero_Convenio = request.Titular.NumeroConvenio;
                entrada.TIT_Nombre_Convenio = request.Titular.NombreConvenio;
                entrada.TIT_Calificacion = request.Titular.Calificacion;
                entrada.TIT_Submercado = request.Titular.Submercado;
                entrada.TIT_Monto_Inversor = request.Titular.MontoInversor;
                entrada.TIT_Tipo_Contribuyente = request.Titular.TipoContribuyente;
            }

            if (request.Sumatoria is not null)
            {
                entrada.SUM_cuota_actual_PP = request.Sumatoria.CuotaActualPrestamoPersonal;
                entrada.SUM_monto_deuda_actual_PP = request.Sumatoria.MontoDeudaActualPrestamoPersonal;
                entrada.SUM_cuota_actual_PR = request.Sumatoria.CuotaActualPrestamoPrendario;
                entrada.SUM_monto_deuda_actual_PR = request.Sumatoria.MontoDeudaActualPrestamoPrendario;
                entrada.SUM_cuota_actual_PH = request.Sumatoria.CuotaActualPrestamoHipotecario;
                entrada.SUM_monto_deuda_actual_PH = request.Sumatoria.MontoDeudaActualPrestamoHipotecario;
                entrada.SUM_cuota_actual_OP = request.Sumatoria.CuotaActualOtrosPrestamos;
                entrada.SUM_monto_deuda_actual_OP = request.Sumatoria.MontoDeudaActualOtrosPrestamos;
                entrada.SUM_limites_TC_Macro = request.Sumatoria.LimitesTarjetaCreditoMacro;
                entrada.SUM_pago_min_prom_ult2_TC_Macro = request.Sumatoria.PagoMinPromUltimos2MesesTCMacro;
                entrada.SUM_acuerdos_cc_Macro = request.Sumatoria.AcuerdosCuentaCorrienteMacro;
                entrada.SUM_uso_prom_ult2_acuerdos_cc_Macro = request.Sumatoria.UsoPromUltimos2MesesAcuerdosCCMacro;
            }

            entrada.Cuota_prest_sin_gtia_no_bureau = request.CuotaPrestamoSinGarantiaNoBureau;
            entrada.Cuota_prest_con_gtia_no_bureau = request.CuotaPrestamoConGarantiaNoBureau;
            entrada.Pago_min_tarj_no_bureau = request.PagoMinimoTarjetaNoBureau;
            entrada.Util_cta_cte_no_bureau = request.UtilidadCuentaCorrienteNoBureau;
            entrada.Solicita_Tarjeta = request.SolicitaTarjeta;
            entrada.Tarjeta_Limite = request.TarjetaLimite;
            entrada.Solicita_Prestamo = request.SolicitaPrestamo;
            entrada.Prestamo_Monto = request.PrestamoMonto;
            entrada.Prestamo_Destino = request.PrestamoDestino;
            entrada.Prestamo_Plazo = request.PrestamoPlazo;
            entrada.Prestamo_Tipo_Tasa = request.PrestamoTipoTasa;
            entrada.Prestamo_TNA = request.PrestamoTNA;
            entrada.Con_Garantia_Valor_Bien = request.ConGarantiaValorBien;
            entrada.Con_Garantia_Subsidio = request.ConGarantiaSubsidio;
            entrada.Con_Garantia_Tasacion = request.ConGarantiaTasacion;
            entrada.Hipotecario_Valor_Terreno = request.HipotecarioValorTerreno;
            entrada.Modelo_Automovil = request.ModeloAutomovil;
            entrada.Solicita_Acuerdo = request.SolicitaAcuerdo;
            entrada.Acuerdo_Monto = request.AcuerdoMonto;
            if (request.Cotitular1 is not null)
            {
                entrada.COT_1_Cobis = request.Cotitular1.Cobis;
                entrada.COT_1_Cuil = request.Cotitular1.NumeroCUIL;
                entrada.COT_1_Numero_Documento = request.Cotitular1.NumeroDocumento;
                entrada.COT_1_Tipo_Documento = request.Cotitular1.TipoDocumento;
                entrada.COT_1_Version_Documento = request.Cotitular1.VersionDocumento;
                entrada.COT_1_Sexo = request.Cotitular1.Sexo;
                entrada.COT_1_Apellido = request.Cotitular1.Apellido;
                entrada.COT_1_Nombre = request.Cotitular1.Nombre;
                entrada.COT_1_Nacimiento = request.Cotitular1.FechaNacimiento;
                entrada.COT_1_Estado_Civil = request.Cotitular1.EstadoCivil;
                entrada.COT_1_Segmento = request.Cotitular1.Segmento;
                entrada.COT_1_Subsegmento = request.Cotitular1.Subsegmento;
                if (request.Cotitular1.Actividad1 is not null)
                {
                    entrada.COT_1_Actividad_1 = request.Cotitular1.Actividad1.Actividad;
                    entrada.COT_1_Actividad_1_Fecha = request.Cotitular1.Actividad1.FechaInicio;
                    entrada.COT_1_Actividad_1_Neto_Fijo = request.Cotitular1.Actividad1.NetoFijo;
                    entrada.COT_1_Actividad_1_Neto_Variable = request.Cotitular1.Actividad1.NetoVariable;
                    entrada.COT_1_Actividad_1_Categoria_Monotributo = request.Cotitular1.Actividad1.CategoriaMonotributo;
                    entrada.COT_1_Actividad_1_Actividad_Monotributo = request.Cotitular1.Actividad1.ActividadMonotributo;
                }
                if (request.Cotitular1.Actividad2 is not null)
                {

                    entrada.COT_1_Actividad_2 = request.Cotitular1.Actividad2.Actividad;
                    entrada.COT_1_Actividad_2_Fecha = request.Cotitular1.Actividad2.FechaInicio;
                    entrada.COT_1_Actividad_2_Neto_Fijo = request.Cotitular1.Actividad2.NetoFijo;
                    entrada.COT_1_Actividad_2_Neto_Variable = request.Cotitular1.Actividad2.NetoVariable;
                    entrada.COT_1_Actividad_2_Categoria_Monotributo = request.Cotitular1.Actividad2.CategoriaMonotributo;
                    entrada.COT_1_Actividad_2_Actividad_Monotributo = request.Cotitular1.Actividad2.ActividadMonotributo;
                }
            }

            if (request.Cotitular2 is not null)
            {
                entrada.COT_2_Cobis = request.Cotitular2.Cobis;
                entrada.COT_2_Cuil = request.Cotitular2.NumeroCUIL;
                entrada.COT_2_Numero_Documento = request.Cotitular2.NumeroDocumento;
                entrada.COT_2_Tipo_Documento = request.Cotitular2.TipoDocumento;
                entrada.COT_2_Version_Documento = request.Cotitular2.VersionDocumento;
                entrada.COT_2_Sexo = request.Cotitular2.Sexo;
                entrada.COT_2_Apellido = request.Cotitular2.Apellido;
                entrada.COT_2_Nombre = request.Cotitular2.Nombre;
                entrada.COT_2_Nacimiento = request.Cotitular2.FechaNacimiento;
                entrada.COT_2_Estado_Civil = request.Cotitular2.EstadoCivil;
                entrada.COT_2_Segmento = request.Cotitular2.Segmento;
                entrada.COT_2_Subsegmento = request.Cotitular2.Subsegmento;
                if (request.Cotitular2.Actividad1 is not null)
                {
                    entrada.COT_2_Actividad_1 = request.Cotitular2.Actividad1.Actividad;
                    entrada.COT_2_Actividad_1_Fecha = request.Cotitular2.Actividad1.FechaInicio;
                    entrada.COT_2_Actividad_1_Neto_Fijo = request.Cotitular2.Actividad1.NetoFijo;
                    entrada.COT_2_Actividad_1_Neto_Variable = request.Cotitular2.Actividad1.NetoVariable;
                    entrada.COT_2_Actividad_1_Categoria_Monotributo = request.Cotitular2.Actividad1.CategoriaMonotributo;
                    entrada.COT_2_Actividad_1_Actividad_Monotributo = request.Cotitular2.Actividad1.ActividadMonotributo;
                }
                if (request.Cotitular2.Actividad2 is not null)
                {
                    entrada.COT_2_Actividad_2 = request.Cotitular2.Actividad2.Actividad;
                    entrada.COT_2_Actividad_2_Fecha = request.Cotitular2.Actividad2.FechaInicio;
                    entrada.COT_2_Actividad_2_Neto_Fijo = request.Cotitular2.Actividad2.NetoFijo;
                    entrada.COT_2_Actividad_2_Neto_Variable = request.Cotitular2.Actividad2.NetoVariable;
                    entrada.COT_2_Actividad_2_Categoria_Monotributo = request.Cotitular2.Actividad2.CategoriaMonotributo;
                    entrada.COT_2_Actividad_2_Actividad_Monotributo = request.Cotitular2.Actividad2.ActividadMonotributo;
                }
            }

            if (request.Cotitular3 is not null)
            {
                entrada.COT_3_Cobis = request.Cotitular3.Cobis;
                entrada.COT_3_Cuil = request.Cotitular3.NumeroCUIL;
                entrada.COT_3_Numero_Documento = request.Cotitular3.NumeroDocumento;
                entrada.COT_3_Tipo_Documento = request.Cotitular3.TipoDocumento;
                entrada.COT_3_Version_Documento = request.Cotitular3.VersionDocumento;
                entrada.COT_3_Sexo = request.Cotitular3.Sexo;
                entrada.COT_3_Apellido = request.Cotitular3.Apellido;
                entrada.COT_3_Nombre = request.Cotitular3.Nombre;
                entrada.COT_3_Nacimiento = request.Cotitular3.FechaNacimiento;
                entrada.COT_3_Estado_Civil = request.Cotitular3.EstadoCivil;
                entrada.COT_3_Segmento = request.Cotitular3.Segmento;
                entrada.COT_3_Subsegmento = request.Cotitular3.Subsegmento;
                if (request.Cotitular3.Actividad1 is not null)
                {
                    entrada.COT_3_Actividad_1 = request.Cotitular3.Actividad1.Actividad;
                    entrada.COT_3_Actividad_1_Fecha = request.Cotitular3.Actividad1.FechaInicio;
                    entrada.COT_3_Actividad_1_Neto_Fijo = request.Cotitular3.Actividad1.NetoFijo;
                    entrada.COT_3_Actividad_1_Neto_Variable = request.Cotitular3.Actividad1.NetoVariable;
                    entrada.COT_3_Actividad_1_Categoria_Monotributo = request.Cotitular3.Actividad1.CategoriaMonotributo;
                    entrada.COT_3_Actividad_1_Actividad_Monotributo = request.Cotitular3.Actividad1.ActividadMonotributo;
                }
                if (request.Cotitular3.Actividad2 is not null)
                {
                    entrada.COT_3_Actividad_2 = request.Cotitular3.Actividad2.Actividad;
                    entrada.COT_3_Actividad_2_Fecha = request.Cotitular3.Actividad2.FechaInicio;
                    entrada.COT_3_Actividad_2_Neto_Fijo = request.Cotitular3.Actividad2.NetoFijo;
                    entrada.COT_3_Actividad_2_Neto_Variable = request.Cotitular3.Actividad2.NetoVariable;
                    entrada.COT_3_Actividad_2_Categoria_Monotributo = request.Cotitular3.Actividad2.CategoriaMonotributo;
                    entrada.COT_3_Actividad_2_Actividad_Monotributo = request.Cotitular3.Actividad2.ActividadMonotributo;
                }
            }

            entrada.Extra = request.Extra;
            entrada.Seguro_Automovil = request.SeguroAutomovil;
            return entrada;
        }
    }
}