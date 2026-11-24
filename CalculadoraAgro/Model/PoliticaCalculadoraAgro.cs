using System.Data;

namespace CalculadoraAgro.Model
{
    public class PoliticaCalculadoraAgro : IEvaluable
    {
        public PoliticaCalculadoraAgroEntrada Entrada { get; set; }

        private PoliticaCalculadoraAgroSalida salida;

        public PoliticaCalculadoraAgroSalida Salida
        {
            get { return salida; }
        }

        private Politica _Politica;

        public PoliticaCalculadoraAgro()
        {
            //_Politica = Politica.CalculadoraAgroV2;
            _Politica = Politica.CalculadoraAgro;
        }

        public Politica GetPolitica()
        {
            return _Politica;
        }

        public string[] codificaEntrada()
        {
            //var diccionarioEntrada = new Dictionary<int, string>();
            var diccionarioEntrada = new string[_Politica.CamposEntrada];
            diccionarioEntrada[0] = Herramientas.ConvertirATexto(Entrada.fecha_solicitud, "", "yyyyMMdd HHmmss");
            diccionarioEntrada[1] = Herramientas.ConvertirATexto(Entrada.ap_origen);
            diccionarioEntrada[2] = Herramientas.ConvertirATexto(Entrada.nro_calificacion);
            diccionarioEntrada[3] = Herramientas.ConvertirATexto(Entrada.nro_eval);
            diccionarioEntrada[4] = Herramientas.ConvertirATexto(Entrada.metodo_de_calificacion);
            diccionarioEntrada[5] = Herramientas.ConvertirATexto(Entrada.cuit);
            diccionarioEntrada[6] = Herramientas.ConvertirATexto(Entrada.Segmento);
            diccionarioEntrada[7] = Herramientas.ConvertirATexto(Entrada.Subsegmento);
            diccionarioEntrada[8] = Herramientas.ConvertirATexto(Entrada.Sucursal);
            diccionarioEntrada[9] = Herramientas.ConvertirATexto(Entrada.fecha_hacienda_cereales_sementeras, "", "yyyyMMdd");
            diccionarioEntrada[10] = Herramientas.ConvertirATexto(Entrada.modalidad_carga_hacienda);
            diccionarioEntrada[11] = Herramientas.ConvertirATexto(Entrada.posee_hacienda);
            diccionarioEntrada[12] = Herramientas.ConvertirATexto(Entrada.vacas_tambo_cabezas);
            diccionarioEntrada[13] = Herramientas.ConvertirATexto(Entrada.vaquillonas_tambo_cabezas);
            diccionarioEntrada[14] = Herramientas.ConvertirATexto(Entrada.toros_tambo_cabezas);
            diccionarioEntrada[15] = Herramientas.ConvertirATexto(Entrada.vacas_tambo_pesos);
            diccionarioEntrada[16] = Herramientas.ConvertirATexto(Entrada.vaquillonas_tambo_pesos);
            diccionarioEntrada[17] = Herramientas.ConvertirATexto(Entrada.toros_tambo_pesos);
            diccionarioEntrada[18] = Herramientas.ConvertirATexto(Entrada.vacas_cria_inv_cabezas);
            diccionarioEntrada[19] = Herramientas.ConvertirATexto(Entrada.toros_cria_inv_cabezas);
            diccionarioEntrada[20] = Herramientas.ConvertirATexto(Entrada.otros_cria_inv_cabezas);
            diccionarioEntrada[21] = Herramientas.ConvertirATexto(Entrada.total_bs_uso_tci_cabezas_por_totales);
            diccionarioEntrada[22] = Herramientas.ConvertirATexto(Entrada.total_bs_uso_tci_pesos_por_totales);
            diccionarioEntrada[23] = Herramientas.ConvertirATexto(Entrada.vacas_cria_inv_pesos);
            diccionarioEntrada[24] = Herramientas.ConvertirATexto(Entrada.toros_cria_inv_pesos);
            diccionarioEntrada[25] = Herramientas.ConvertirATexto(Entrada.otros_cria_inv_pesos);
            diccionarioEntrada[26] = Herramientas.ConvertirATexto(Entrada.vacas_tci_venta_cabezas);
            diccionarioEntrada[27] = Herramientas.ConvertirATexto(Entrada.vaquillonas_tci_venta_cabezas);
            diccionarioEntrada[28] = Herramientas.ConvertirATexto(Entrada.terneros_tci_venta_cabezas);
            diccionarioEntrada[29] = Herramientas.ConvertirATexto(Entrada.novillos_tci_venta_cabezas);
            diccionarioEntrada[30] = Herramientas.ConvertirATexto(Entrada.novillitos_tci_venta_cabezas);
            diccionarioEntrada[31] = Herramientas.ConvertirATexto(Entrada.otros_tci_venta_cabezas);
            diccionarioEntrada[32] = Herramientas.ConvertirATexto(Entrada.total_bs_cambio_tci_cabezas_por_totales);
            diccionarioEntrada[33] = Herramientas.ConvertirATexto(Entrada.vacas_feedlot_venta_cabezas);
            diccionarioEntrada[34] = Herramientas.ConvertirATexto(Entrada.vaquillonas_feedlot_venta_cabezas);
            diccionarioEntrada[35] = Herramientas.ConvertirATexto(Entrada.terneros_feedlot_venta_cabezas);
            diccionarioEntrada[36] = Herramientas.ConvertirATexto(Entrada.novillos_feedlot_venta_cabezas);
            diccionarioEntrada[37] = Herramientas.ConvertirATexto(Entrada.novillitos_feedlot_venta_cabezas);
            diccionarioEntrada[38] = Herramientas.ConvertirATexto(Entrada.total_bs_cambio_feedlot_cabezas_por_totales);
            diccionarioEntrada[39] = Herramientas.ConvertirATexto(Entrada.vacas_venta_pesos);
            diccionarioEntrada[40] = Herramientas.ConvertirATexto(Entrada.vaquilonas_venta_pesos);
            diccionarioEntrada[41] = Herramientas.ConvertirATexto(Entrada.terneros_venta_pesos);
            diccionarioEntrada[42] = Herramientas.ConvertirATexto(Entrada.novillos_venta_pesos);
            diccionarioEntrada[43] = Herramientas.ConvertirATexto(Entrada.novillitos_venta_pesos);
            diccionarioEntrada[44] = Herramientas.ConvertirATexto(Entrada.otros_venta_pesos);
            diccionarioEntrada[45] = Herramientas.ConvertirATexto(Entrada.total_bs_cambio_tci_feedlot_pesos_por_totales);
            diccionarioEntrada[46] = Herramientas.ConvertirATexto(Entrada.cant_cabezas_acta_vacunacion);
            diccionarioEntrada[47] = Herramientas.ConvertirATexto(Entrada.posee_cereales_sementeras);
            diccionarioEntrada[48] = Herramientas.ConvertirATexto(Entrada.modalidad_carga_cereales_y_sementeras);
            diccionarioEntrada[49] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_1);
            diccionarioEntrada[50] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_2);
            diccionarioEntrada[51] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_3);
            diccionarioEntrada[52] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_4);
            diccionarioEntrada[53] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_5);
            diccionarioEntrada[54] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_6);
            diccionarioEntrada[55] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_7);
            diccionarioEntrada[56] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_8);
            diccionarioEntrada[57] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_9);
            diccionarioEntrada[58] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_10);
            diccionarioEntrada[59] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_11);
            diccionarioEntrada[60] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_12);
            diccionarioEntrada[61] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_13);
            diccionarioEntrada[62] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_14);
            diccionarioEntrada[63] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_15);
            diccionarioEntrada[64] = Herramientas.ConvertirATexto(Entrada.cultivo_cereal_sementera_16);
            diccionarioEntrada[65] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_1);
            diccionarioEntrada[66] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_2);
            diccionarioEntrada[67] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_3);
            diccionarioEntrada[68] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_4);
            diccionarioEntrada[69] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_5);
            diccionarioEntrada[70] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_6);
            diccionarioEntrada[71] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_7);
            diccionarioEntrada[72] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_8);
            diccionarioEntrada[73] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_9);
            diccionarioEntrada[74] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_10);
            diccionarioEntrada[75] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_11);
            diccionarioEntrada[76] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_12);
            diccionarioEntrada[77] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_13);
            diccionarioEntrada[78] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_14);
            diccionarioEntrada[79] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_15);
            diccionarioEntrada[80] = Herramientas.ConvertirATexto(Entrada.cant_cereal_sementera_16);
            diccionarioEntrada[81] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_1);
            diccionarioEntrada[82] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_2);
            diccionarioEntrada[83] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_3);
            diccionarioEntrada[84] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_4);
            diccionarioEntrada[85] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_5);
            diccionarioEntrada[86] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_6);
            diccionarioEntrada[87] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_7);
            diccionarioEntrada[88] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_8);
            diccionarioEntrada[89] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_9);
            diccionarioEntrada[90] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_10);
            diccionarioEntrada[91] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_11);
            diccionarioEntrada[92] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_12);
            diccionarioEntrada[93] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_13);
            diccionarioEntrada[94] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_14);
            diccionarioEntrada[95] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_15);
            diccionarioEntrada[96] = Herramientas.ConvertirATexto(Entrada.medida_cereal_sementera_16);
            diccionarioEntrada[97] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_1);
            diccionarioEntrada[98] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_2);
            diccionarioEntrada[99] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_3);
            diccionarioEntrada[100] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_4);
            diccionarioEntrada[101] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_5);
            diccionarioEntrada[102] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_6);
            diccionarioEntrada[103] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_7);
            diccionarioEntrada[104] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_8);
            diccionarioEntrada[105] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_9);
            diccionarioEntrada[106] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_10);
            diccionarioEntrada[107] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_11);
            diccionarioEntrada[108] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_12);
            diccionarioEntrada[109] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_13);
            diccionarioEntrada[110] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_14);
            diccionarioEntrada[111] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_15);
            diccionarioEntrada[112] = Herramientas.ConvertirATexto(Entrada.monto_cereal_sementera_16);
            diccionarioEntrada[113] = Herramientas.ConvertirATexto(Entrada.total_cereales_pesos_por_totales);
            diccionarioEntrada[114] = Herramientas.ConvertirATexto(Entrada.total_sementeras_pesos_por_totales);
            diccionarioEntrada[115] = Herramientas.ConvertirATexto(Entrada.fecha_estructura_productiva, "", "yyyyMMdd");
            diccionarioEntrada[116] = Herramientas.ConvertirATexto(Entrada.posee_agricultura);
            diccionarioEntrada[117] = Herramientas.ConvertirATexto(Entrada.ag_provincia_1);
            diccionarioEntrada[118] = Herramientas.ConvertirATexto(Entrada.ag_provincia_2);
            diccionarioEntrada[119] = Herramientas.ConvertirATexto(Entrada.ag_provincia_3);
            diccionarioEntrada[120] = Herramientas.ConvertirATexto(Entrada.ag_provincia_4);
            diccionarioEntrada[121] = Herramientas.ConvertirATexto(Entrada.ag_provincia_5);
            diccionarioEntrada[122] = Herramientas.ConvertirATexto(Entrada.ag_provincia_6);
            diccionarioEntrada[123] = Herramientas.ConvertirATexto(Entrada.ag_provincia_7);
            diccionarioEntrada[124] = Herramientas.ConvertirATexto(Entrada.ag_provincia_8);
            diccionarioEntrada[125] = Herramientas.ConvertirATexto(Entrada.ag_provincia_9);
            diccionarioEntrada[126] = Herramientas.ConvertirATexto(Entrada.ag_provincia_10);
            diccionarioEntrada[127] = Herramientas.ConvertirATexto(Entrada.ag_provincia_11);
            diccionarioEntrada[128] = Herramientas.ConvertirATexto(Entrada.ag_provincia_12);
            diccionarioEntrada[129] = Herramientas.ConvertirATexto(Entrada.ag_provincia_13);
            diccionarioEntrada[130] = Herramientas.ConvertirATexto(Entrada.ag_provincia_14);
            diccionarioEntrada[131] = Herramientas.ConvertirATexto(Entrada.ag_provincia_15);
            diccionarioEntrada[132] = Herramientas.ConvertirATexto(Entrada.ag_zona_1);
            diccionarioEntrada[133] = Herramientas.ConvertirATexto(Entrada.ag_zona_2);
            diccionarioEntrada[134] = Herramientas.ConvertirATexto(Entrada.ag_zona_3);
            diccionarioEntrada[135] = Herramientas.ConvertirATexto(Entrada.ag_zona_4);
            diccionarioEntrada[136] = Herramientas.ConvertirATexto(Entrada.ag_zona_5);
            diccionarioEntrada[137] = Herramientas.ConvertirATexto(Entrada.ag_zona_6);
            diccionarioEntrada[138] = Herramientas.ConvertirATexto(Entrada.ag_zona_7);
            diccionarioEntrada[139] = Herramientas.ConvertirATexto(Entrada.ag_zona_8);
            diccionarioEntrada[140] = Herramientas.ConvertirATexto(Entrada.ag_zona_9);
            diccionarioEntrada[141] = Herramientas.ConvertirATexto(Entrada.ag_zona_10);
            diccionarioEntrada[142] = Herramientas.ConvertirATexto(Entrada.ag_zona_11);
            diccionarioEntrada[143] = Herramientas.ConvertirATexto(Entrada.ag_zona_12);
            diccionarioEntrada[144] = Herramientas.ConvertirATexto(Entrada.ag_zona_13);
            diccionarioEntrada[145] = Herramientas.ConvertirATexto(Entrada.ag_zona_14);
            diccionarioEntrada[146] = Herramientas.ConvertirATexto(Entrada.ag_zona_15);
            diccionarioEntrada[147] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_1);
            diccionarioEntrada[148] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_2);
            diccionarioEntrada[149] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_3);
            diccionarioEntrada[150] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_4);
            diccionarioEntrada[151] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_5);
            diccionarioEntrada[152] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_6);
            diccionarioEntrada[153] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_7);
            diccionarioEntrada[154] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_8);
            diccionarioEntrada[155] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_9);
            diccionarioEntrada[156] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_10);
            diccionarioEntrada[157] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_11);
            diccionarioEntrada[158] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_12);
            diccionarioEntrada[159] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_13);
            diccionarioEntrada[160] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_14);
            diccionarioEntrada[161] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_1ro_15);
            diccionarioEntrada[162] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_1);
            diccionarioEntrada[163] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_2);
            diccionarioEntrada[164] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_3);
            diccionarioEntrada[165] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_4);
            diccionarioEntrada[166] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_5);
            diccionarioEntrada[167] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_6);
            diccionarioEntrada[168] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_7);
            diccionarioEntrada[169] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_8);
            diccionarioEntrada[170] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_9);
            diccionarioEntrada[171] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_10);
            diccionarioEntrada[172] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_11);
            diccionarioEntrada[173] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_12);
            diccionarioEntrada[174] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_13);
            diccionarioEntrada[175] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_14);
            diccionarioEntrada[176] = Herramientas.ConvertirATexto(Entrada.ag_cultivo_2do_15);
            diccionarioEntrada[177] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_1);
            diccionarioEntrada[178] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_2);
            diccionarioEntrada[179] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_3);
            diccionarioEntrada[180] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_4);
            diccionarioEntrada[181] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_5);
            diccionarioEntrada[182] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_6);
            diccionarioEntrada[183] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_7);
            diccionarioEntrada[184] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_8);
            diccionarioEntrada[185] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_9);
            diccionarioEntrada[186] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_10);
            diccionarioEntrada[187] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_11);
            diccionarioEntrada[188] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_12);
            diccionarioEntrada[189] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_13);
            diccionarioEntrada[190] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_14);
            diccionarioEntrada[191] = Herramientas.ConvertirATexto(Entrada.ag_has_propias_15);
            diccionarioEntrada[192] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_1);
            diccionarioEntrada[193] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_2);
            diccionarioEntrada[194] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_3);
            diccionarioEntrada[195] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_4);
            diccionarioEntrada[196] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_5);
            diccionarioEntrada[197] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_6);
            diccionarioEntrada[198] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_7);
            diccionarioEntrada[199] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_8);
            diccionarioEntrada[200] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_9);
            diccionarioEntrada[201] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_10);
            diccionarioEntrada[202] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_11);
            diccionarioEntrada[203] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_12);
            diccionarioEntrada[204] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_13);
            diccionarioEntrada[205] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_14);
            diccionarioEntrada[206] = Herramientas.ConvertirATexto(Entrada.ag_has_avalistas_15);
            diccionarioEntrada[207] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_1);
            diccionarioEntrada[208] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_2);
            diccionarioEntrada[209] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_3);
            diccionarioEntrada[210] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_4);
            diccionarioEntrada[211] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_5);
            diccionarioEntrada[212] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_6);
            diccionarioEntrada[213] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_7);
            diccionarioEntrada[214] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_8);
            diccionarioEntrada[215] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_9);
            diccionarioEntrada[216] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_10);
            diccionarioEntrada[217] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_11);
            diccionarioEntrada[218] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_12);
            diccionarioEntrada[219] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_13);
            diccionarioEntrada[220] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_14);
            diccionarioEntrada[221] = Herramientas.ConvertirATexto(Entrada.ag_has_terceros_15);
            diccionarioEntrada[222] = Herramientas.ConvertirATexto(Entrada.ag_has_con_contrato_avalistas);
            diccionarioEntrada[223] = Herramientas.ConvertirATexto(Entrada.ag_has_con_contrato_terceros);
            diccionarioEntrada[224] = Herramientas.ConvertirATexto(Entrada.posee_tambo);
            diccionarioEntrada[225] = Herramientas.ConvertirATexto(Entrada.ta_vacas_en_ordenie);
            diccionarioEntrada[226] = Herramientas.ConvertirATexto(Entrada.ta_usina_lactea_pyme);
            diccionarioEntrada[227] = Herramientas.ConvertirATexto(Entrada.ta_has_avalistas);
            diccionarioEntrada[228] = Herramientas.ConvertirATexto(Entrada.ta_has_terceros);
            diccionarioEntrada[229] = Herramientas.ConvertirATexto(Entrada.ta_has_avalistas_con_contrato);
            diccionarioEntrada[230] = Herramientas.ConvertirATexto(Entrada.ta_has_terceros_con_contrato);
            diccionarioEntrada[231] = Herramientas.ConvertirATexto(Entrada.ta_superficie_propia);
            diccionarioEntrada[232] = Herramientas.ConvertirATexto(Entrada.ta_fecha_liq_1, "", "yyyyMMdd");
            diccionarioEntrada[233] = Herramientas.ConvertirATexto(Entrada.ta_fecha_liq_2, "", "yyyyMMdd");
            diccionarioEntrada[234] = Herramientas.ConvertirATexto(Entrada.ta_fecha_liq_3, "", "yyyyMMdd");
            diccionarioEntrada[235] = Herramientas.ConvertirATexto(Entrada.ta_litros_mensuales_1);
            diccionarioEntrada[236] = Herramientas.ConvertirATexto(Entrada.ta_litros_mensuales_2);
            diccionarioEntrada[237] = Herramientas.ConvertirATexto(Entrada.ta_litros_mensuales_3);
            diccionarioEntrada[238] = Herramientas.ConvertirATexto(Entrada.posee_ganaderia);
            diccionarioEntrada[239] = Herramientas.ConvertirATexto(Entrada.ga_cria_completo);
            diccionarioEntrada[240] = Herramientas.ConvertirATexto(Entrada.ga_invernada);
            diccionarioEntrada[241] = Herramientas.ConvertirATexto(Entrada.ga_feed_lot);
            diccionarioEntrada[242] = Herramientas.ConvertirATexto(Entrada.ga_cap_feed_lot);
            diccionarioEntrada[243] = Herramientas.ConvertirATexto(Entrada.ga_has_avalistas);
            diccionarioEntrada[244] = Herramientas.ConvertirATexto(Entrada.ga_has_terceros);
            diccionarioEntrada[245] = Herramientas.ConvertirATexto(Entrada.ga_has_avalistas_con_contrato);
            diccionarioEntrada[246] = Herramientas.ConvertirATexto(Entrada.ga_has_terceros_con_contrato);
            diccionarioEntrada[247] = Herramientas.ConvertirATexto(Entrada.ga_superficie_propia);
            diccionarioEntrada[248] = Herramientas.ConvertirATexto(Entrada.posee_activ_contratista);
            diccionarioEntrada[249] = Herramientas.ConvertirATexto(Entrada.co_servicio_1);
            diccionarioEntrada[250] = Herramientas.ConvertirATexto(Entrada.co_servicio_2);
            diccionarioEntrada[251] = Herramientas.ConvertirATexto(Entrada.co_servicio_3);
            diccionarioEntrada[252] = Herramientas.ConvertirATexto(Entrada.co_servicio_4);
            diccionarioEntrada[253] = Herramientas.ConvertirATexto(Entrada.co_servicio_5);
            diccionarioEntrada[254] = Herramientas.ConvertirATexto(Entrada.co_servicio_6);
            diccionarioEntrada[255] = Herramientas.ConvertirATexto(Entrada.co_servicio_7);
            diccionarioEntrada[256] = Herramientas.ConvertirATexto(Entrada.co_servicio_8);
            diccionarioEntrada[257] = Herramientas.ConvertirATexto(Entrada.co_servicio_9);
            diccionarioEntrada[258] = Herramientas.ConvertirATexto(Entrada.co_servicio_10);
            diccionarioEntrada[259] = Herramientas.ConvertirATexto(Entrada.co_servicio_11);
            diccionarioEntrada[260] = Herramientas.ConvertirATexto(Entrada.co_cant_1);
            diccionarioEntrada[261] = Herramientas.ConvertirATexto(Entrada.co_cant_2);
            diccionarioEntrada[262] = Herramientas.ConvertirATexto(Entrada.co_cant_3);
            diccionarioEntrada[263] = Herramientas.ConvertirATexto(Entrada.co_cant_4);
            diccionarioEntrada[264] = Herramientas.ConvertirATexto(Entrada.co_cant_5);
            diccionarioEntrada[265] = Herramientas.ConvertirATexto(Entrada.co_cant_6);
            diccionarioEntrada[266] = Herramientas.ConvertirATexto(Entrada.co_cant_7);
            diccionarioEntrada[267] = Herramientas.ConvertirATexto(Entrada.co_cant_8);
            diccionarioEntrada[268] = Herramientas.ConvertirATexto(Entrada.co_cant_9);
            diccionarioEntrada[269] = Herramientas.ConvertirATexto(Entrada.co_cant_10);
            diccionarioEntrada[270] = Herramientas.ConvertirATexto(Entrada.co_cant_11);
            diccionarioEntrada[271] = Herramientas.ConvertirATexto(Entrada.parametros_manuales);
            diccionarioEntrada[272] = Herramientas.ConvertirATexto(Entrada.extra);
            return diccionarioEntrada;
        }

        public void decodificaSalida(string[] ArraySalida)
        {
            salida = new PoliticaCalculadoraAgroSalida();
            salida.ap_origen = (string)Herramientas.ConvertirDeTexto(ArraySalida[0], DbType.String, "");
            salida.nro_calificacion = (int)Herramientas.ConvertirDeTexto(ArraySalida[1], DbType.Int32, 0);
            salida.nro_eval = (int)Herramientas.ConvertirDeTexto(ArraySalida[2], DbType.Int32, 0);
            salida.Status = (string)Herramientas.ConvertirDeTexto(ArraySalida[3], DbType.String, "");
            salida.Mensaje = (string)Herramientas.ConvertirDeTexto(ArraySalida[4], DbType.String, "");
            salida.fecha_estructura_productiva = (string)Herramientas.ConvertirDeTexto(ArraySalida[5], DbType.String, "");
            salida.p_calc_fecha_actualizacion = (string)Herramientas.ConvertirDeTexto(ArraySalida[6], DbType.String, "");
            salida.tipo_de_cambio = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[7], DbType.Decimal, decimal.Zero);
            salida.total_bovinos_pesos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[8], DbType.Decimal, decimal.Zero);
            salida.total_cereales_pesos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[9], DbType.Decimal, decimal.Zero);
            salida.total_sementeras_pesos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[10], DbType.Decimal, decimal.Zero);
            salida.ag_total_has_propias = (int)Herramientas.ConvertirDeTexto(ArraySalida[11], DbType.Int32, 0);
            salida.ag_total_has_avalistas = (int)Herramientas.ConvertirDeTexto(ArraySalida[12], DbType.Int32, 0);
            salida.ag_total_has_terceros = (int)Herramientas.ConvertirDeTexto(ArraySalida[13], DbType.Int32, 0);
            salida.ag_total_has_efectivas = (int)Herramientas.ConvertirDeTexto(ArraySalida[14], DbType.Int32, 0);
            salida.ga_superficie_propia = (int)Herramientas.ConvertirDeTexto(ArraySalida[15], DbType.Int32, 0);
            salida.ga_has_avalistas = (int)Herramientas.ConvertirDeTexto(ArraySalida[16], DbType.Int32, 0);
            salida.ga_has_terceros = (int)Herramientas.ConvertirDeTexto(ArraySalida[17], DbType.Int32, 0);
            salida.ga_sup_total = (int)Herramientas.ConvertirDeTexto(ArraySalida[18], DbType.Int32, 0);
            salida.ta_superficie_propia = (int)Herramientas.ConvertirDeTexto(ArraySalida[19], DbType.Int32, 0);
            salida.ta_has_avalistas = (int)Herramientas.ConvertirDeTexto(ArraySalida[20], DbType.Int32, 0);
            salida.ta_has_terceros = (int)Herramientas.ConvertirDeTexto(ArraySalida[21], DbType.Int32, 0);
            salida.ta_sup_total = (int)Herramientas.ConvertirDeTexto(ArraySalida[22], DbType.Int32, 0);
            salida.has_totales_declaradas_cliente = (int)Herramientas.ConvertirDeTexto(ArraySalida[23], DbType.Int32, 0);
            salida.has_totales_declaradas_avalistas = (int)Herramientas.ConvertirDeTexto(ArraySalida[24], DbType.Int32, 0);
            salida.has_totales_declaradas_terceros = (int)Herramientas.ConvertirDeTexto(ArraySalida[25], DbType.Int32, 0);
            salida.has_totales_declaradas = (int)Herramientas.ConvertirDeTexto(ArraySalida[26], DbType.Int32, 0);
            salida.has_totales_declaradas_avalistas_vinculados_terceros = (int)Herramientas.ConvertirDeTexto(ArraySalida[27], DbType.Int32, 0);
            salida.has_totales_con_contrato_avalistas = (int)Herramientas.ConvertirDeTexto(ArraySalida[28], DbType.Int32, 0);
            salida.has_totales_con_contrato_terceros = (int)Herramientas.ConvertirDeTexto(ArraySalida[29], DbType.Int32, 0);
            salida.has_totales_con_contrato = (int)Herramientas.ConvertirDeTexto(ArraySalida[30], DbType.Int32, 0);
            salida.has_totales_soja = (int)Herramientas.ConvertirDeTexto(ArraySalida[31], DbType.Int32, 0);
            salida.has_totales_soja_2da = (int)Herramientas.ConvertirDeTexto(ArraySalida[32], DbType.Int32, 0);
            salida.has_totales_trigo = (int)Herramientas.ConvertirDeTexto(ArraySalida[33], DbType.Int32, 0);
            salida.has_totales_maiz = (int)Herramientas.ConvertirDeTexto(ArraySalida[34], DbType.Int32, 0);
            salida.has_totales_girasol = (int)Herramientas.ConvertirDeTexto(ArraySalida[35], DbType.Int32, 0);
            salida.has_totales_cebada = (int)Herramientas.ConvertirDeTexto(ArraySalida[36], DbType.Int32, 0);
            salida.has_totales_sorgo = (int)Herramientas.ConvertirDeTexto(ArraySalida[37], DbType.Int32, 0);
            salida.ag_total_has_propias_nucleo = (int)Herramientas.ConvertirDeTexto(ArraySalida[38], DbType.Int32, 0);
            salida.ag_total_has_avalistas_nucleo = (int)Herramientas.ConvertirDeTexto(ArraySalida[39], DbType.Int32, 0);
            salida.ag_total_has_terceros_nucleo = (int)Herramientas.ConvertirDeTexto(ArraySalida[40], DbType.Int32, 0);
            salida.ag_total_has_nucleo = (int)Herramientas.ConvertirDeTexto(ArraySalida[41], DbType.Int32, 0);
            salida.ag_total_has_propias_perimetral_1 = (int)Herramientas.ConvertirDeTexto(ArraySalida[42], DbType.Int32, 0);
            salida.ag_total_has_avalistas_perimetral_1 = (int)Herramientas.ConvertirDeTexto(ArraySalida[43], DbType.Int32, 0);
            salida.ag_total_has_terceros_perimetral_1 = (int)Herramientas.ConvertirDeTexto(ArraySalida[44], DbType.Int32, 0);
            salida.ag_total_has_perimetral_1 = (int)Herramientas.ConvertirDeTexto(ArraySalida[45], DbType.Int32, 0);
            salida.ag_total_has_propias_perimetral_2 = (int)Herramientas.ConvertirDeTexto(ArraySalida[46], DbType.Int32, 0);
            salida.ag_total_has_avalistas_perimetral_2 = (int)Herramientas.ConvertirDeTexto(ArraySalida[47], DbType.Int32, 0);
            salida.ag_total_has_terceros_perimetral_2 = (int)Herramientas.ConvertirDeTexto(ArraySalida[48], DbType.Int32, 0);
            salida.ag_total_has_perimetral_2 = (int)Herramientas.ConvertirDeTexto(ArraySalida[49], DbType.Int32, 0);
            salida.ag_total_has_propias_se_triguera = (int)Herramientas.ConvertirDeTexto(ArraySalida[50], DbType.Int32, 0);
            salida.ag_total_has_avalistas_se_triguera = (int)Herramientas.ConvertirDeTexto(ArraySalida[51], DbType.Int32, 0);
            salida.ag_total_has_terceros_se_triguera = (int)Herramientas.ConvertirDeTexto(ArraySalida[52], DbType.Int32, 0);
            salida.ag_total_has_se_triguera = (int)Herramientas.ConvertirDeTexto(ArraySalida[53], DbType.Int32, 0);
            salida.ag_total_has_propias_marginal = (int)Herramientas.ConvertirDeTexto(ArraySalida[54], DbType.Int32, 0);
            salida.ag_total_has_avalistas_marginal = (int)Herramientas.ConvertirDeTexto(ArraySalida[55], DbType.Int32, 0);
            salida.ag_total_has_terceros_marginal = (int)Herramientas.ConvertirDeTexto(ArraySalida[56], DbType.Int32, 0);
            salida.ag_total_has_marginal = (int)Herramientas.ConvertirDeTexto(ArraySalida[57], DbType.Int32, 0);
            salida.co_cosecha = (int)Herramientas.ConvertirDeTexto(ArraySalida[58], DbType.Int32, 0);
            salida.co_siembra = (int)Herramientas.ConvertirDeTexto(ArraySalida[59], DbType.Int32, 0);
            salida.co_picado_fino = (int)Herramientas.ConvertirDeTexto(ArraySalida[60], DbType.Int32, 0);
            salida.co_picado_grueso = (int)Herramientas.ConvertirDeTexto(ArraySalida[61], DbType.Int32, 0);
            salida.co_fumig_aerea = (int)Herramientas.ConvertirDeTexto(ArraySalida[62], DbType.Int32, 0);
            salida.co_fumig_terrestre = (int)Herramientas.ConvertirDeTexto(ArraySalida[63], DbType.Int32, 0);
            salida.co_laboreo = (int)Herramientas.ConvertirDeTexto(ArraySalida[64], DbType.Int32, 0);
            salida.co_embolsado = (int)Herramientas.ConvertirDeTexto(ArraySalida[65], DbType.Int32, 0);
            salida.co_rotoenfardado = (int)Herramientas.ConvertirDeTexto(ArraySalida[66], DbType.Int32, 0);
            salida.ta_total_vacas = (int)Herramientas.ConvertirDeTexto(ArraySalida[67], DbType.Int32, 0);
            salida.ta_vacas_en_ordenie = (int)Herramientas.ConvertirDeTexto(ArraySalida[68], DbType.Int32, 0);
            salida.ta_porc_vacas_en_ordenie = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[69], DbType.Decimal, decimal.Zero);
            salida.ta_litros_estimados_mensuales = (int)Herramientas.ConvertirDeTexto(ArraySalida[70], DbType.Int32, 0);
            salida.ta_litros_reales_diarios = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[71], DbType.Decimal, decimal.Zero);
            salida.ta_litros_prom_mensual_ult_3_liq = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[72], DbType.Decimal, decimal.Zero);
            salida.ta_usina_lactea_pyme = (string)Herramientas.ConvertirDeTexto(ArraySalida[73], DbType.String, "");
            salida.ga_cria_completo = (string)Herramientas.ConvertirDeTexto(ArraySalida[74], DbType.String, "");
            salida.ga_invernada = (string)Herramientas.ConvertirDeTexto(ArraySalida[75], DbType.String, "");
            salida.ga_feed_lot = (string)Herramientas.ConvertirDeTexto(ArraySalida[76], DbType.String, "");
            salida.ga_total_feedlot = (int)Herramientas.ConvertirDeTexto(ArraySalida[77], DbType.Int32, 0);
            salida.vacas_cria_inv_cabezas = (int)Herramientas.ConvertirDeTexto(ArraySalida[78], DbType.Int32, 0);
            salida.ga_stock_resto_cria_inv = (int)Herramientas.ConvertirDeTexto(ArraySalida[79], DbType.Int32, 0);
            salida.ga_cap_feed_lot = (int)Herramientas.ConvertirDeTexto(ArraySalida[80], DbType.Int32, 0);
            salida.ga_cab_has = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[81], DbType.Decimal, decimal.Zero);
            salida.ga_porc_ocupacion = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[82], DbType.Decimal, decimal.Zero);
            salida.total_bovinos_cabezas = (int)Herramientas.ConvertirDeTexto(ArraySalida[83], DbType.Int32, 0);
            salida.cant_cabezas_acta_vacunacion = (int)Herramientas.ConvertirDeTexto(ArraySalida[84], DbType.Int32, 0);
            salida.ag_ingresos_brutos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[85], DbType.Decimal, decimal.Zero);
            salida.ag_gs_com_mas_gs_cosecha = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[86], DbType.Decimal, decimal.Zero);
            salida.ag_ingresos_netos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[87], DbType.Decimal, decimal.Zero);
            salida.ag_gs_directos_mas_gs_arrend = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[88], DbType.Decimal, decimal.Zero);
            salida.ag_rdo_operativo_potencial = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[89], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_partic_agricultura = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[90], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_promedio_agricultura = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[91], DbType.Decimal, decimal.Zero);
            salida.co_ingresos_brutos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[92], DbType.Decimal, decimal.Zero);
            salida.co_costo = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[93], DbType.Decimal, decimal.Zero);
            salida.co_ebitda = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[94], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_partic_contratista = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[95], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_promedio_contratista = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[96], DbType.Decimal, decimal.Zero);
            salida.ta_ingresos_brutos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[97], DbType.Decimal, decimal.Zero);
            salida.ta_costo = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[98], DbType.Decimal, decimal.Zero);
            salida.ta_ebitda = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[99], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_partic_tambo = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[100], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_promedio_tambo = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[101], DbType.Decimal, decimal.Zero);
            salida.ga_ingresos_brutos = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[102], DbType.Decimal, decimal.Zero);
            salida.ga_costo = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[103], DbType.Decimal, decimal.Zero);
            salida.ga_ebitda = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[104], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_partic_ganaderia = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[105], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_promedio_ganaderia = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[106], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_total = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[107], DbType.Decimal, decimal.Zero);
            salida.gs_com_mas_gs_cosecha_total = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[108], DbType.Decimal, decimal.Zero);
            salida.ingresos_netos_total = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[109], DbType.Decimal, decimal.Zero);
            salida.gs_directos_mas_gs_arrend = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[110], DbType.Decimal, decimal.Zero);
            salida.ebitda_total = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[111], DbType.Decimal, decimal.Zero);
            salida.ingresos_brutos_promedio = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[112], DbType.Decimal, decimal.Zero);
            salida.ag_ingresos_brutos_usd = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[113], DbType.Decimal, decimal.Zero);
            salida.ag_gs_com_mas_gs_cosecha_usd = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[114], DbType.Decimal, decimal.Zero);
            salida.ag_ingresos_netos_usd = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[115], DbType.Decimal, decimal.Zero);
            salida.ag_gs_directos_mas_gs_arrend_usd = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[116], DbType.Decimal, decimal.Zero);
            salida.ag_rdo_operativo_potencial_usd = (decimal)Herramientas.ConvertirDeTexto(ArraySalida[117], DbType.Decimal, decimal.Zero);
            salida.extra = (string)Herramientas.ConvertirDeTexto(ArraySalida[118], DbType.String, "");
            salida.version_politica = (string)Herramientas.ConvertirDeTexto(ArraySalida[119], DbType.String, "");
            if (ArraySalida.Length > 120)
            {
                salida.cultivos = (string)Herramientas.ConvertirDeTexto(ArraySalida[120], DbType.String, "");
                salida.has_totales_cultivo = (string)Herramientas.ConvertirDeTexto(ArraySalida[121], DbType.String, "");
                salida.ag_zonas_desagregado = (string)Herramientas.ConvertirDeTexto(ArraySalida[122], DbType.String, "");
                salida.ag_total_has_propias_desagregado = (string)Herramientas.ConvertirDeTexto(ArraySalida[123], DbType.String, "");
                salida.ag_total_has_avalistas_desagregado = (string)Herramientas.ConvertirDeTexto(ArraySalida[124], DbType.String, "");
                salida.ag_total_has_terceros_desagregado = (string)Herramientas.ConvertirDeTexto(ArraySalida[125], DbType.String, "");
                salida.ag_total_has_desagregado = (string)Herramientas.ConvertirDeTexto(ArraySalida[126], DbType.String, "");
                salida.co_servicios = (string)Herramientas.ConvertirDeTexto(ArraySalida[127], DbType.String, "");
                salida.co_servicios_hectareas = (string)Herramientas.ConvertirDeTexto(ArraySalida[128], DbType.String, "");
            }
        }
    }

    public class PoliticaCalculadoraAgroEntrada
    {
        public DateTime? fecha_solicitud { get; set; }
        public int? ap_origen { get; set; }
        public int? nro_calificacion { get; set; }
        public int? nro_eval { get; set; }
        public string metodo_de_calificacion { get; set; }
        public long? cuit { get; set; }
        public string? Segmento { get; set; }
        public string? Subsegmento { get; set; }
        public string? Sucursal { get; set; }
        public DateTime? fecha_hacienda_cereales_sementeras { get; set; }
        public string modalidad_carga_hacienda { get; set; }
        public string posee_hacienda { get; set; }
        public int? vacas_tambo_cabezas { get; set; }
        public int? vaquillonas_tambo_cabezas { get; set; }
        public int? toros_tambo_cabezas { get; set; }
        public decimal? vacas_tambo_pesos { get; set; }
        public decimal? vaquillonas_tambo_pesos { get; set; }
        public decimal? toros_tambo_pesos { get; set; }
        public int? vacas_cria_inv_cabezas { get; set; }
        public int? toros_cria_inv_cabezas { get; set; }
        public int? otros_cria_inv_cabezas { get; set; }
        public int? total_bs_uso_tci_cabezas_por_totales { get; set; }
        public decimal? total_bs_uso_tci_pesos_por_totales { get; set; }
        public decimal? vacas_cria_inv_pesos { get; set; }
        public decimal? toros_cria_inv_pesos { get; set; }
        public decimal? otros_cria_inv_pesos { get; set; }
        public int? vacas_tci_venta_cabezas { get; set; }
        public int? vaquillonas_tci_venta_cabezas { get; set; }
        public int? terneros_tci_venta_cabezas { get; set; }
        public int? novillos_tci_venta_cabezas { get; set; }
        public int? novillitos_tci_venta_cabezas { get; set; }
        public int? otros_tci_venta_cabezas { get; set; }
        public int? total_bs_cambio_tci_cabezas_por_totales { get; set; }
        public int? vacas_feedlot_venta_cabezas { get; set; }
        public int? vaquillonas_feedlot_venta_cabezas { get; set; }
        public int? terneros_feedlot_venta_cabezas { get; set; }
        public int? novillos_feedlot_venta_cabezas { get; set; }
        public int? novillitos_feedlot_venta_cabezas { get; set; }
        public int? total_bs_cambio_feedlot_cabezas_por_totales { get; set; }
        public decimal? vacas_venta_pesos { get; set; }
        public decimal? vaquilonas_venta_pesos { get; set; }
        public decimal? terneros_venta_pesos { get; set; }
        public decimal? novillos_venta_pesos { get; set; }
        public decimal? novillitos_venta_pesos { get; set; }
        public decimal? otros_venta_pesos { get; set; }
        public decimal? total_bs_cambio_tci_feedlot_pesos_por_totales { get; set; }
        public int? cant_cabezas_acta_vacunacion { get; set; }
        public string posee_cereales_sementeras { get; set; }
        public string modalidad_carga_cereales_y_sementeras { get; set; }
        public string cultivo_cereal_sementera_1 { get; set; }
        public string cultivo_cereal_sementera_2 { get; set; }
        public string cultivo_cereal_sementera_3 { get; set; }
        public string cultivo_cereal_sementera_4 { get; set; }
        public string cultivo_cereal_sementera_5 { get; set; }
        public string cultivo_cereal_sementera_6 { get; set; }
        public string cultivo_cereal_sementera_7 { get; set; }
        public string cultivo_cereal_sementera_8 { get; set; }
        public string cultivo_cereal_sementera_9 { get; set; }
        public string cultivo_cereal_sementera_10 { get; set; }
        public string cultivo_cereal_sementera_11 { get; set; }
        public string cultivo_cereal_sementera_12 { get; set; }
        public string cultivo_cereal_sementera_13 { get; set; }
        public string cultivo_cereal_sementera_14 { get; set; }
        public string cultivo_cereal_sementera_15 { get; set; }
        public string cultivo_cereal_sementera_16 { get; set; }
        public int? cant_cereal_sementera_1 { get; set; }
        public int? cant_cereal_sementera_2 { get; set; }
        public int? cant_cereal_sementera_3 { get; set; }
        public int? cant_cereal_sementera_4 { get; set; }
        public int? cant_cereal_sementera_5 { get; set; }
        public int? cant_cereal_sementera_6 { get; set; }
        public int? cant_cereal_sementera_7 { get; set; }
        public int? cant_cereal_sementera_8 { get; set; }
        public int? cant_cereal_sementera_9 { get; set; }
        public int? cant_cereal_sementera_10 { get; set; }
        public int? cant_cereal_sementera_11 { get; set; }
        public int? cant_cereal_sementera_12 { get; set; }
        public int? cant_cereal_sementera_13 { get; set; }
        public int? cant_cereal_sementera_14 { get; set; }
        public int? cant_cereal_sementera_15 { get; set; }
        public int? cant_cereal_sementera_16 { get; set; }
        public string medida_cereal_sementera_1 { get; set; }
        public string medida_cereal_sementera_2 { get; set; }
        public string medida_cereal_sementera_3 { get; set; }
        public string medida_cereal_sementera_4 { get; set; }
        public string medida_cereal_sementera_5 { get; set; }
        public string medida_cereal_sementera_6 { get; set; }
        public string medida_cereal_sementera_7 { get; set; }
        public string medida_cereal_sementera_8 { get; set; }
        public string medida_cereal_sementera_9 { get; set; }
        public string medida_cereal_sementera_10 { get; set; }
        public string medida_cereal_sementera_11 { get; set; }
        public string medida_cereal_sementera_12 { get; set; }
        public string medida_cereal_sementera_13 { get; set; }
        public string medida_cereal_sementera_14 { get; set; }
        public string medida_cereal_sementera_15 { get; set; }
        public string medida_cereal_sementera_16 { get; set; }
        public decimal? monto_cereal_sementera_1 { get; set; }
        public decimal? monto_cereal_sementera_2 { get; set; }
        public decimal? monto_cereal_sementera_3 { get; set; }
        public decimal? monto_cereal_sementera_4 { get; set; }
        public decimal? monto_cereal_sementera_5 { get; set; }
        public decimal? monto_cereal_sementera_6 { get; set; }
        public decimal? monto_cereal_sementera_7 { get; set; }
        public decimal? monto_cereal_sementera_8 { get; set; }
        public decimal? monto_cereal_sementera_9 { get; set; }
        public decimal? monto_cereal_sementera_10 { get; set; }
        public decimal? monto_cereal_sementera_11 { get; set; }
        public decimal? monto_cereal_sementera_12 { get; set; }
        public decimal? monto_cereal_sementera_13 { get; set; }
        public decimal? monto_cereal_sementera_14 { get; set; }
        public decimal? monto_cereal_sementera_15 { get; set; }
        public decimal? monto_cereal_sementera_16 { get; set; }
        public decimal? total_cereales_pesos_por_totales { get; set; }
        public decimal? total_sementeras_pesos_por_totales { get; set; }
        public DateTime? fecha_estructura_productiva { get; set; }
        public string posee_agricultura { get; set; }
        public string ag_provincia_1 { get; set; }
        public string ag_provincia_2 { get; set; }
        public string ag_provincia_3 { get; set; }
        public string ag_provincia_4 { get; set; }
        public string ag_provincia_5 { get; set; }
        public string ag_provincia_6 { get; set; }
        public string ag_provincia_7 { get; set; }
        public string ag_provincia_8 { get; set; }
        public string ag_provincia_9 { get; set; }
        public string ag_provincia_10 { get; set; }
        public string ag_provincia_11 { get; set; }
        public string ag_provincia_12 { get; set; }
        public string ag_provincia_13 { get; set; }
        public string ag_provincia_14 { get; set; }
        public string ag_provincia_15 { get; set; }
        public string ag_zona_1 { get; set; }
        public string ag_zona_2 { get; set; }
        public string ag_zona_3 { get; set; }
        public string ag_zona_4 { get; set; }
        public string ag_zona_5 { get; set; }
        public string ag_zona_6 { get; set; }
        public string ag_zona_7 { get; set; }
        public string ag_zona_8 { get; set; }
        public string ag_zona_9 { get; set; }
        public string ag_zona_10 { get; set; }
        public string ag_zona_11 { get; set; }
        public string ag_zona_12 { get; set; }
        public string ag_zona_13 { get; set; }
        public string ag_zona_14 { get; set; }
        public string ag_zona_15 { get; set; }
        public string ag_cultivo_1ro_1 { get; set; }
        public string ag_cultivo_1ro_2 { get; set; }
        public string ag_cultivo_1ro_3 { get; set; }
        public string ag_cultivo_1ro_4 { get; set; }
        public string ag_cultivo_1ro_5 { get; set; }
        public string ag_cultivo_1ro_6 { get; set; }
        public string ag_cultivo_1ro_7 { get; set; }
        public string ag_cultivo_1ro_8 { get; set; }
        public string ag_cultivo_1ro_9 { get; set; }
        public string ag_cultivo_1ro_10 { get; set; }
        public string ag_cultivo_1ro_11 { get; set; }
        public string ag_cultivo_1ro_12 { get; set; }
        public string ag_cultivo_1ro_13 { get; set; }
        public string ag_cultivo_1ro_14 { get; set; }
        public string ag_cultivo_1ro_15 { get; set; }
        public string ag_cultivo_2do_1 { get; set; }
        public string ag_cultivo_2do_2 { get; set; }
        public string ag_cultivo_2do_3 { get; set; }
        public string ag_cultivo_2do_4 { get; set; }
        public string ag_cultivo_2do_5 { get; set; }
        public string ag_cultivo_2do_6 { get; set; }
        public string ag_cultivo_2do_7 { get; set; }
        public string ag_cultivo_2do_8 { get; set; }
        public string ag_cultivo_2do_9 { get; set; }
        public string ag_cultivo_2do_10 { get; set; }
        public string ag_cultivo_2do_11 { get; set; }
        public string ag_cultivo_2do_12 { get; set; }
        public string ag_cultivo_2do_13 { get; set; }
        public string ag_cultivo_2do_14 { get; set; }
        public string ag_cultivo_2do_15 { get; set; }
        public int? ag_has_propias_1 { get; set; }
        public int? ag_has_propias_2 { get; set; }
        public int? ag_has_propias_3 { get; set; }
        public int? ag_has_propias_4 { get; set; }
        public int? ag_has_propias_5 { get; set; }
        public int? ag_has_propias_6 { get; set; }
        public int? ag_has_propias_7 { get; set; }
        public int? ag_has_propias_8 { get; set; }
        public int? ag_has_propias_9 { get; set; }
        public int? ag_has_propias_10 { get; set; }
        public int? ag_has_propias_11 { get; set; }
        public int? ag_has_propias_12 { get; set; }
        public int? ag_has_propias_13 { get; set; }
        public int? ag_has_propias_14 { get; set; }
        public int? ag_has_propias_15 { get; set; }
        public int? ag_has_avalistas_1 { get; set; }
        public int? ag_has_avalistas_2 { get; set; }
        public int? ag_has_avalistas_3 { get; set; }
        public int? ag_has_avalistas_4 { get; set; }
        public int? ag_has_avalistas_5 { get; set; }
        public int? ag_has_avalistas_6 { get; set; }
        public int? ag_has_avalistas_7 { get; set; }
        public int? ag_has_avalistas_8 { get; set; }
        public int? ag_has_avalistas_9 { get; set; }
        public int? ag_has_avalistas_10 { get; set; }
        public int? ag_has_avalistas_11 { get; set; }
        public int? ag_has_avalistas_12 { get; set; }
        public int? ag_has_avalistas_13 { get; set; }
        public int? ag_has_avalistas_14 { get; set; }
        public int? ag_has_avalistas_15 { get; set; }
        public int? ag_has_terceros_1 { get; set; }
        public int? ag_has_terceros_2 { get; set; }
        public int? ag_has_terceros_3 { get; set; }
        public int? ag_has_terceros_4 { get; set; }
        public int? ag_has_terceros_5 { get; set; }
        public int? ag_has_terceros_6 { get; set; }
        public int? ag_has_terceros_7 { get; set; }
        public int? ag_has_terceros_8 { get; set; }
        public int? ag_has_terceros_9 { get; set; }
        public int? ag_has_terceros_10 { get; set; }
        public int? ag_has_terceros_11 { get; set; }
        public int? ag_has_terceros_12 { get; set; }
        public int? ag_has_terceros_13 { get; set; }
        public int? ag_has_terceros_14 { get; set; }
        public int? ag_has_terceros_15 { get; set; }
        public int? ag_has_con_contrato_avalistas { get; set; }
        public int? ag_has_con_contrato_terceros { get; set; }
        public string posee_tambo { get; set; }
        public int? ta_vacas_en_ordenie { get; set; }
        public string ta_usina_lactea_pyme { get; set; }
        public int? ta_has_avalistas { get; set; }
        public int? ta_has_terceros { get; set; }
        public int? ta_has_avalistas_con_contrato { get; set; }
        public int? ta_has_terceros_con_contrato { get; set; }
        public int? ta_superficie_propia { get; set; }
        public DateTime? ta_fecha_liq_1 { get; set; }
        public DateTime? ta_fecha_liq_2 { get; set; }
        public DateTime? ta_fecha_liq_3 { get; set; }
        public int? ta_litros_mensuales_1 { get; set; }
        public int? ta_litros_mensuales_2 { get; set; }
        public int? ta_litros_mensuales_3 { get; set; }
        public string posee_ganaderia { get; set; }
        public string ga_cria_completo { get; set; }
        public string ga_invernada { get; set; }
        public string ga_feed_lot { get; set; }
        public int? ga_cap_feed_lot { get; set; }
        public int? ga_has_avalistas { get; set; }
        public int? ga_has_terceros { get; set; }
        public int? ga_has_avalistas_con_contrato { get; set; }
        public int? ga_has_terceros_con_contrato { get; set; }
        public int? ga_superficie_propia { get; set; }
        public string posee_activ_contratista { get; set; }
        public string co_servicio_1 { get; set; }
        public string co_servicio_2 { get; set; }
        public string co_servicio_3 { get; set; }
        public string co_servicio_4 { get; set; }
        public string co_servicio_5 { get; set; }
        public string co_servicio_6 { get; set; }
        public string co_servicio_7 { get; set; }
        public string co_servicio_8 { get; set; }
        public string co_servicio_9 { get; set; }
        public string co_servicio_10 { get; set; }
        public string co_servicio_11 { get; set; }
        public int? co_cant_1 { get; set; }
        public int? co_cant_2 { get; set; }
        public int? co_cant_3 { get; set; }
        public int? co_cant_4 { get; set; }
        public int? co_cant_5 { get; set; }
        public int? co_cant_6 { get; set; }
        public int? co_cant_7 { get; set; }
        public int? co_cant_8 { get; set; }
        public int? co_cant_9 { get; set; }
        public int? co_cant_10 { get; set; }
        public int? co_cant_11 { get; set; }
        public string parametros_manuales { get; set; }
        public string extra { get; set; }
    }

    public class PoliticaCalculadoraAgroSalida
    {
        public string? ap_origen { get; set; }
        public int? nro_calificacion { get; set; }
        public int? nro_eval { get; set; }
        public string? Status { get; set; }
        public string? Mensaje { get; set; }
        public string? fecha_estructura_productiva { get; set; }
        public string? p_calc_fecha_actualizacion { get; set; }
        public decimal? tipo_de_cambio { get; set; }
        public decimal? total_bovinos_pesos { get; set; }
        public decimal? total_cereales_pesos { get; set; }
        public decimal? total_sementeras_pesos { get; set; }
        public int? ag_total_has_propias { get; set; }
        public int? ag_total_has_avalistas { get; set; }
        public int? ag_total_has_terceros { get; set; }
        public int? ag_total_has_efectivas { get; set; }
        public int? ga_superficie_propia { get; set; }
        public int? ga_has_avalistas { get; set; }
        public int? ga_has_terceros { get; set; }
        public int? ga_sup_total { get; set; }
        public int? ta_superficie_propia { get; set; }
        public int? ta_has_avalistas { get; set; }
        public int? ta_has_terceros { get; set; }
        public int? ta_sup_total { get; set; }
        public int? has_totales_declaradas_cliente { get; set; }
        public int? has_totales_declaradas_avalistas { get; set; }
        public int? has_totales_declaradas_terceros { get; set; }
        public int? has_totales_declaradas { get; set; }
        public int? has_totales_declaradas_avalistas_vinculados_terceros { get; set; }
        public int? has_totales_con_contrato_avalistas { get; set; }
        public int? has_totales_con_contrato_terceros { get; set; }
        public int? has_totales_con_contrato { get; set; }
        public int? has_totales_soja { get; set; }
        public int? has_totales_soja_2da { get; set; }
        public int? has_totales_trigo { get; set; }
        public int? has_totales_maiz { get; set; }
        public int? has_totales_girasol { get; set; }
        public int? has_totales_cebada { get; set; }
        public int? has_totales_sorgo { get; set; }
        public int? ag_total_has_propias_nucleo { get; set; }
        public int? ag_total_has_avalistas_nucleo { get; set; }
        public int? ag_total_has_terceros_nucleo { get; set; }
        public int? ag_total_has_nucleo { get; set; }
        public int? ag_total_has_propias_perimetral_1 { get; set; }
        public int? ag_total_has_avalistas_perimetral_1 { get; set; }
        public int? ag_total_has_terceros_perimetral_1 { get; set; }
        public int? ag_total_has_perimetral_1 { get; set; }
        public int? ag_total_has_propias_perimetral_2 { get; set; }
        public int? ag_total_has_avalistas_perimetral_2 { get; set; }
        public int? ag_total_has_terceros_perimetral_2 { get; set; }
        public int? ag_total_has_perimetral_2 { get; set; }
        public int? ag_total_has_propias_se_triguera { get; set; }
        public int? ag_total_has_avalistas_se_triguera { get; set; }
        public int? ag_total_has_terceros_se_triguera { get; set; }
        public int? ag_total_has_se_triguera { get; set; }
        public int? ag_total_has_propias_marginal { get; set; }
        public int? ag_total_has_avalistas_marginal { get; set; }
        public int? ag_total_has_terceros_marginal { get; set; }
        public int? ag_total_has_marginal { get; set; }
        public int? co_cosecha { get; set; }
        public int? co_siembra { get; set; }
        public int? co_picado_fino { get; set; }
        public int? co_picado_grueso { get; set; }
        public int? co_fumig_aerea { get; set; }
        public int? co_fumig_terrestre { get; set; }
        public int? co_laboreo { get; set; }
        public int? co_embolsado { get; set; }
        public int? co_rotoenfardado { get; set; }
        public int? ta_total_vacas { get; set; }
        public int? ta_vacas_en_ordenie { get; set; }
        public decimal? ta_porc_vacas_en_ordenie { get; set; }
        public int? ta_litros_estimados_mensuales { get; set; }
        public decimal? ta_litros_reales_diarios { get; set; }
        public decimal? ta_litros_prom_mensual_ult_3_liq { get; set; }
        public string? ta_usina_lactea_pyme { get; set; }
        public string? ga_cria_completo { get; set; }
        public string? ga_invernada { get; set; }
        public string? ga_feed_lot { get; set; }
        public int? ga_total_feedlot { get; set; }
        public int? vacas_cria_inv_cabezas { get; set; }
        public int? ga_stock_resto_cria_inv { get; set; }
        public int? ga_cap_feed_lot { get; set; }
        public decimal? ga_cab_has { get; set; }
        public decimal? ga_porc_ocupacion { get; set; }
        public int? total_bovinos_cabezas { get; set; }
        public int? cant_cabezas_acta_vacunacion { get; set; }
        public decimal? ag_ingresos_brutos { get; set; }
        public decimal? ag_gs_com_mas_gs_cosecha { get; set; }
        public decimal? ag_ingresos_netos { get; set; }
        public decimal? ag_gs_directos_mas_gs_arrend { get; set; }
        public decimal? ag_rdo_operativo_potencial { get; set; }
        public decimal? ingresos_brutos_partic_agricultura { get; set; }
        public decimal? ingresos_brutos_promedio_agricultura { get; set; }
        public decimal? co_ingresos_brutos { get; set; }
        public decimal? co_costo { get; set; }
        public decimal? co_ebitda { get; set; }
        public decimal? ingresos_brutos_partic_contratista { get; set; }
        public decimal? ingresos_brutos_promedio_contratista { get; set; }
        public decimal? ta_ingresos_brutos { get; set; }
        public decimal? ta_costo { get; set; }
        public decimal? ta_ebitda { get; set; }
        public decimal? ingresos_brutos_partic_tambo { get; set; }
        public decimal? ingresos_brutos_promedio_tambo { get; set; }
        public decimal? ga_ingresos_brutos { get; set; }
        public decimal? ga_costo { get; set; }
        public decimal? ga_ebitda { get; set; }
        public decimal? ingresos_brutos_partic_ganaderia { get; set; }
        public decimal? ingresos_brutos_promedio_ganaderia { get; set; }
        public decimal? ingresos_brutos_total { get; set; }
        public decimal? gs_com_mas_gs_cosecha_total { get; set; }
        public decimal? ingresos_netos_total { get; set; }
        public decimal? gs_directos_mas_gs_arrend { get; set; }
        public decimal? ebitda_total { get; set; }
        public decimal? ingresos_brutos_promedio { get; set; }
        public decimal? ag_ingresos_brutos_usd { get; set; }
        public decimal? ag_gs_com_mas_gs_cosecha_usd { get; set; }
        public decimal? ag_ingresos_netos_usd { get; set; }
        public decimal? ag_gs_directos_mas_gs_arrend_usd { get; set; }
        public decimal? ag_rdo_operativo_potencial_usd { get; set; }
        public string extra { get; set; }
        public string version_politica { get; set; }
        public string cultivos { get; set; }
        public string has_totales_cultivo { get; set; }
        public string ag_zonas_desagregado { get; set; }
        public string ag_total_has_propias_desagregado { get; set; }
        public string ag_total_has_avalistas_desagregado { get; set; }
        public string ag_total_has_terceros_desagregado { get; set; }
        public string ag_total_has_desagregado { get; set; }
        public string co_servicios { get; set; }
        public string co_servicios_hectareas { get; set; }
    }
}