using Azure;
using Azure.Core;
using EvaluaIndividuos.Database;
using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Endpoints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Win32;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EvaluaIndividuos.Model
{
    public class PoliticaRentaPresunta : IEvaluable, IDictaminable
    {
        public PoliticaRentaPresuntaEntrada Entrada { get; set; } = new();
        private PoliticaRentaPresuntaSalida _Salida = new();
        private Politica _Politica;

        public PoliticaRentaPresunta()
        {
            _Politica = Politica.RentaPresunta;
        }

        public PoliticaRentaPresunta(FuentesDeInformacion fuentes) : this()
        {
            MapeaEntrada(fuentes);
        }

        public PoliticaRentaPresuntaSalida Salida
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
            diccionarioEntrada[12] = Herramientas.ConvertirATexto(Entrada.fecha_nac, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[13] = Herramientas.ConvertirATexto(Entrada.prov);
            diccionarioEntrada[14] = Herramientas.ConvertirATexto(Entrada.zona);
            diccionarioEntrada[15] = Herramientas.ConvertirATexto(Entrada.cod_sucursal);
            diccionarioEntrada[16] = Herramientas.ConvertirATexto(Entrada.est_civil);
            diccionarioEntrada[17] = Herramientas.ConvertirATexto(Entrada.vivienda);
            diccionarioEntrada[18] = Herramientas.ConvertirATexto(Entrada.alquiler);
            diccionarioEntrada[19] = Herramientas.ConvertirATexto(Entrada.contribuyente);
            diccionarioEntrada[20] = Herramientas.ConvertirATexto(Entrada.laboral_ppal);
            diccionarioEntrada[21] = Herramientas.ConvertirATexto(Entrada.fech_laboral, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[22] = Herramientas.ConvertirATexto(Entrada.neto_declarado);
            diccionarioEntrada[23] = Herramientas.ConvertirATexto(Entrada.pp_cuota_macro);
            diccionarioEntrada[24] = Herramientas.ConvertirATexto(Entrada.pp_monto_macro);
            diccionarioEntrada[25] = Herramientas.ConvertirATexto(Entrada.pr_cuota_macro);
            diccionarioEntrada[26] = Herramientas.ConvertirATexto(Entrada.pr_monto_macro);
            diccionarioEntrada[27] = Herramientas.ConvertirATexto(Entrada.ph_cuota_macro);
            diccionarioEntrada[28] = Herramientas.ConvertirATexto(Entrada.ph_monto_macro);
            diccionarioEntrada[29] = Herramientas.ConvertirATexto(Entrada.op_cuota_macro);
            diccionarioEntrada[30] = Herramientas.ConvertirATexto(Entrada.op_monto_macro);
            diccionarioEntrada[31] = Herramientas.ConvertirATexto(Entrada.lim_tarj_macro);
            diccionarioEntrada[32] = Herramientas.ConvertirATexto(Entrada.pago_min_tarj_macro);
            diccionarioEntrada[33] = Herramientas.ConvertirATexto(Entrada.acue_cta_macro);
            diccionarioEntrada[34] = Herramientas.ConvertirATexto(Entrada.uti_cta_cte_macro);
            diccionarioEntrada[35] = Herramientas.ConvertirATexto(Entrada.dias_atraso);
            diccionarioEntrada[36] = Herramientas.ConvertirATexto(Entrada.sum_cuotas_p_sg_no_bureau);
            diccionarioEntrada[37] = Herramientas.ConvertirATexto(Entrada.sum_cuotas_p_cg_no_bureau);
            diccionarioEntrada[38] = Herramientas.ConvertirATexto(Entrada.sum_pagos_min_tarj_no_bureau);
            diccionarioEntrada[39] = Herramientas.ConvertirATexto(Entrada.sum_util_cta_cte_no_bureau);
            diccionarioEntrada[40] = Herramientas.ConvertirATexto(Entrada.prestamo);
            diccionarioEntrada[41] = Herramientas.ConvertirATexto(Entrada.tarjeta);
            diccionarioEntrada[42] = Herramientas.ConvertirATexto(Entrada.cta_cte);
            diccionarioEntrada[43] = Herramientas.ConvertirATexto(Entrada.tipo_prestamo);
            diccionarioEntrada[44] = Herramientas.ConvertirATexto(Entrada.plazo);
            diccionarioEntrada[45] = Herramientas.ConvertirATexto(Entrada.destino_personal);
            diccionarioEntrada[46] = Herramientas.ConvertirATexto(Entrada.tipo_tasa);
            diccionarioEntrada[47] = Herramientas.ConvertirATexto(Entrada.tna);
            diccionarioEntrada[48] = Herramientas.ConvertirATexto(Entrada.canal);
            diccionarioEntrada[49] = Herramientas.ConvertirATexto(Entrada.equipo_ventas);
            diccionarioEntrada[50] = Herramientas.ConvertirATexto(Entrada.tipo_fuente);
            diccionarioEntrada[51] = Herramientas.ConvertirATexto(Entrada.fuente);
            diccionarioEntrada[52] = Herramientas.ConvertirATexto(Entrada.segmento);
            diccionarioEntrada[53] = Herramientas.ConvertirATexto(Entrada.subsegmento);
            diccionarioEntrada[54] = Herramientas.ConvertirATexto(Entrada.nombre_plantilla);
            diccionarioEntrada[55] = Herramientas.ConvertirATexto(Entrada.estado);
            diccionarioEntrada[56] = Herramientas.ConvertirATexto(Entrada.atributo1);
            diccionarioEntrada[57] = Herramientas.ConvertirATexto(Entrada.valor1);
            diccionarioEntrada[58] = Herramientas.ConvertirATexto(Entrada.atributo2);
            diccionarioEntrada[59] = Herramientas.ConvertirATexto(Entrada.valor2);
            diccionarioEntrada[60] = Herramientas.ConvertirATexto(Entrada.atributo3);
            diccionarioEntrada[61] = Herramientas.ConvertirATexto(Entrada.valor3);
            diccionarioEntrada[62] = Herramientas.ConvertirATexto(Entrada.atributo4);
            diccionarioEntrada[63] = Herramientas.ConvertirATexto(Entrada.valor4);
            diccionarioEntrada[64] = Herramientas.ConvertirATexto(Entrada.atributo5);
            diccionarioEntrada[65] = Herramientas.ConvertirATexto(Entrada.valor5);
            diccionarioEntrada[66] = Herramientas.ConvertirATexto(Entrada.atributo6);
            diccionarioEntrada[67] = Herramientas.ConvertirATexto(Entrada.valor6);
            diccionarioEntrada[68] = Herramientas.ConvertirATexto(Entrada.atributo7);
            diccionarioEntrada[69] = Herramientas.ConvertirATexto(Entrada.valor7);
            diccionarioEntrada[70] = Herramientas.ConvertirATexto(Entrada.atributo8);
            diccionarioEntrada[71] = Herramientas.ConvertirATexto(Entrada.valor8);
            diccionarioEntrada[72] = Herramientas.ConvertirATexto(Entrada.atributo9);
            diccionarioEntrada[73] = Herramientas.ConvertirATexto(Entrada.valor9);
            diccionarioEntrada[74] = Herramientas.ConvertirATexto(Entrada.atributo10);
            diccionarioEntrada[75] = Herramientas.ConvertirATexto(Entrada.valor10);
            diccionarioEntrada[76] = Herramientas.ConvertirATexto(Entrada.usuario);
            diccionarioEntrada[77] = Herramientas.ConvertirATexto(Entrada.PRECACOM_MONTO);
            diccionarioEntrada[78] = Herramientas.ConvertirATexto(Entrada.field79);
            diccionarioEntrada[79] = Herramientas.ConvertirATexto(Entrada.field80);
            diccionarioEntrada[80] = Herramientas.ConvertirATexto(Entrada.ID_Interno);
            diccionarioEntrada[81] = Herramientas.ConvertirATexto(Entrada.field82);
            diccionarioEntrada[82] = Herramientas.ConvertirATexto(Entrada.field83);
            diccionarioEntrada[83] = Herramientas.ConvertirATexto(Entrada.field84);
            diccionarioEntrada[84] = Herramientas.ConvertirATexto(Entrada.field85);
            diccionarioEntrada[85] = Herramientas.ConvertirATexto(Entrada.field86);
            diccionarioEntrada[86] = Herramientas.ConvertirATexto(Entrada.field87);
            diccionarioEntrada[87] = Herramientas.ConvertirATexto(Entrada.Mora_Suc_6M_90);
            diccionarioEntrada[88] = Herramientas.ConvertirATexto(Entrada.Mora_Suc_12M_90);
            diccionarioEntrada[89] = Herramientas.ConvertirATexto(Entrada.EA_CODIGO_CLIENTE);
            diccionarioEntrada[90] = Herramientas.ConvertirATexto(Entrada.EA_FECHA_CARGA, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[91] = Herramientas.ConvertirATexto(Entrada.EA_FECHA_CABALOPE);
            diccionarioEntrada[92] = Herramientas.ConvertirATexto(Entrada.EA_FECHA_CONSULTA_REESTRUCTURACION);
            diccionarioEntrada[93] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[94] = Herramientas.ConvertirATexto(Entrada.EA_FECHA_CONSULTA_REFI_DEU);
            diccionarioEntrada[95] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_REFINANCIACION);
            diccionarioEntrada[96] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_DEU);
            diccionarioEntrada[97] = Herramientas.ConvertirATexto(Entrada.CND_Cobis);
            diccionarioEntrada[98] = Herramientas.ConvertirATexto(Entrada.CND_Nro_Tributario);
            diccionarioEntrada[99] = Herramientas.ConvertirATexto(Entrada.CND_Codigo_Campana);
            diccionarioEntrada[100] = Herramientas.ConvertirATexto(Entrada.CND_Vencimiento, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[101] = Herramientas.ConvertirATexto(Entrada.CND_Motivo_Rechazo);
            diccionarioEntrada[102] = Herramientas.ConvertirATexto(Entrada.BH_id_cobis);
            diccionarioEntrada[103] = Herramientas.ConvertirATexto(Entrada.BH_Riesgo_Score);
            diccionarioEntrada[104] = Herramientas.ConvertirATexto(Entrada.BH_Riesgo_6);
            diccionarioEntrada[105] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Tc_Limite);
            diccionarioEntrada[106] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Tc_Exigible);
            diccionarioEntrada[107] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Tc_Limite_Max);
            diccionarioEntrada[108] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Cc_Limite);
            diccionarioEntrada[109] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Cc_Promedio3);
            diccionarioEntrada[110] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Sg_Exigible);
            diccionarioEntrada[111] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Gp_Exigible);
            diccionarioEntrada[112] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Gh_Exigible);
            diccionarioEntrada[113] = Herramientas.ConvertirATexto(Entrada.BH_Antiguedad_Tc);
            diccionarioEntrada[114] = Herramientas.ConvertirATexto(Entrada.BH_Antiguedad_Cc);
            diccionarioEntrada[115] = Herramientas.ConvertirATexto(Entrada.BH_Cob_Status);
            diccionarioEntrada[116] = Herramientas.ConvertirATexto(Entrada.BH_Hit);
            diccionarioEntrada[117] = Herramientas.ConvertirATexto(Entrada.BH_Uso_Tc_Saldo_Fin);
            diccionarioEntrada[118] = Herramientas.ConvertirATexto(Entrada.BH_IP2);
            diccionarioEntrada[119] = Herramientas.ConvertirATexto(Entrada.Consultar_Veraz);
            diccionarioEntrada[120] = Herramientas.ConvertirATexto(Entrada.Rech_Lev_SF_u24m_Cant);
            diccionarioEntrada[121] = Herramientas.ConvertirATexto(Entrada.Rech_NoLev_SF_u24m_Monto);
            diccionarioEntrada[122] = Herramientas.ConvertirATexto(Entrada.Rech_Lev_SF_u24m_Monto);
            diccionarioEntrada[123] = Herramientas.ConvertirATexto(Entrada.exclusion);
            diccionarioEntrada[124] = Herramientas.ConvertirATexto(Entrada.Score_Comp);
            diccionarioEntrada[125] = Herramientas.ConvertirATexto(Entrada.categoria);
            diccionarioEntrada[126] = Herramientas.ConvertirATexto(Entrada.POL_Flag_Acuerdo_CC);
            diccionarioEntrada[127] = Herramientas.ConvertirATexto(Entrada.POL_Flag_Producto_Crediticio_Activo);
            diccionarioEntrada[128] = Herramientas.ConvertirATexto(Entrada.antiguedad_tc_macro);
            diccionarioEntrada[129] = Herramientas.ConvertirATexto(Entrada.antiguedad_cc_macro);
            diccionarioEntrada[130] = Herramientas.ConvertirATexto(Entrada.POL_Cantidad_Ocurrencia_6);
            diccionarioEntrada[131] = Herramientas.ConvertirATexto(Entrada.FECHA_VENCIMIENTO, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[132] = Herramientas.ConvertirATexto(Entrada.Cuit);
            diccionarioEntrada[133] = Herramientas.ConvertirATexto(Entrada.NombrePe);
            diccionarioEntrada[134] = Herramientas.ConvertirATexto(Entrada.ApellidoPe);
            diccionarioEntrada[135] = Herramientas.ConvertirATexto(Entrada.SexoPe);
            diccionarioEntrada[136] = Herramientas.ConvertirATexto(Entrada.FechaNacimientoPe, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[137] = Herramientas.ConvertirATexto(Entrada.ProvinciaPe);
            diccionarioEntrada[138] = Herramientas.ConvertirATexto(Entrada.CodigoPostalPe);
            diccionarioEntrada[139] = Herramientas.ConvertirATexto(Entrada.FechaCarga, "01/01/1900", "dd/MM/yyyy");
            diccionarioEntrada[140] = Herramientas.ConvertirATexto(Entrada.PeriodoScore);
            diccionarioEntrada[141] = Herramientas.ConvertirATexto(Entrada.Empleador);
            diccionarioEntrada[142] = Herramientas.ConvertirATexto(Entrada.FlagCrediticio);
            diccionarioEntrada[143] = Herramientas.ConvertirATexto(Entrada.SumScore);
            diccionarioEntrada[144] = Herramientas.ConvertirATexto(Entrada.RazonSocialAfip);
            diccionarioEntrada[145] = Herramientas.ConvertirATexto(Entrada.ImpuestoGanancias);
            diccionarioEntrada[146] = Herramientas.ConvertirATexto(Entrada.ImpuestoIva);
            diccionarioEntrada[147] = Herramientas.ConvertirATexto(Entrada.IntegranteSociedad);
            diccionarioEntrada[148] = Herramientas.ConvertirATexto(Entrada.EsEmpleadorAfip);
            diccionarioEntrada[149] = Herramientas.ConvertirATexto(Entrada.CatMonotributo);
            diccionarioEntrada[150] = Herramientas.ConvertirATexto(Entrada.ActividadMonotributo);
            diccionarioEntrada[151] = Herramientas.ConvertirATexto(Entrada.ActividadMonotributoDescripcion);
            diccionarioEntrada[152] = Herramientas.ConvertirATexto(Entrada.FactMonotributo);
            diccionarioEntrada[153] = Herramientas.ConvertirATexto(Entrada.IngresoServicios);
            diccionarioEntrada[154] = Herramientas.ConvertirATexto(Entrada.IngresoVentas);
            diccionarioEntrada[155] = Herramientas.ConvertirATexto(Entrada.IngresoMonotributo);
            diccionarioEntrada[156] = Herramientas.ConvertirATexto(Entrada.EsEmpleado);
            diccionarioEntrada[157] = Herramientas.ConvertirATexto(Entrada.EsAutonomo);
            diccionarioEntrada[158] = Herramientas.ConvertirATexto(Entrada.EsMonotributista);
            diccionarioEntrada[159] = Herramientas.ConvertirATexto(Entrada.EsJubilado);
            diccionarioEntrada[160] = Herramientas.ConvertirATexto(Entrada.IngresoTransaccionalidad);
            diccionarioEntrada[161] = Herramientas.ConvertirATexto(Entrada.Opcional1);
            diccionarioEntrada[162] = Herramientas.ConvertirATexto(Entrada.Va1);
            diccionarioEntrada[163] = Herramientas.ConvertirATexto(Entrada.Va2);
            diccionarioEntrada[164] = Herramientas.ConvertirATexto(Entrada.Va3);
            diccionarioEntrada[165] = Herramientas.ConvertirATexto(Entrada.Va4);
            diccionarioEntrada[166] = Herramientas.ConvertirATexto(Entrada.Va5);
            diccionarioEntrada[167] = Herramientas.ConvertirATexto(Entrada.Va6);
            diccionarioEntrada[168] = Herramientas.ConvertirATexto(Entrada.Va7);
            diccionarioEntrada[169] = Herramientas.ConvertirATexto(Entrada.Va8);
            diccionarioEntrada[170] = Herramientas.ConvertirATexto(Entrada.Va9);
            diccionarioEntrada[171] = Herramientas.ConvertirATexto(Entrada.Va10);
            diccionarioEntrada[172] = Herramientas.ConvertirATexto(Entrada.Va11);
            diccionarioEntrada[173] = Herramientas.ConvertirATexto(Entrada.Va12);
            diccionarioEntrada[174] = Herramientas.ConvertirATexto(Entrada.Va13);
            diccionarioEntrada[175] = Herramientas.ConvertirATexto(Entrada.Va14);
            diccionarioEntrada[176] = Herramientas.ConvertirATexto(Entrada.Va15);
            diccionarioEntrada[177] = Herramientas.ConvertirATexto(Entrada.Va16);
            diccionarioEntrada[178] = Herramientas.ConvertirATexto(Entrada.Va17);
            diccionarioEntrada[179] = Herramientas.ConvertirATexto(Entrada.Va18);
            diccionarioEntrada[180] = Herramientas.ConvertirATexto(Entrada.Va19);
            diccionarioEntrada[181] = Herramientas.ConvertirATexto(Entrada.Va20);
            diccionarioEntrada[182] = Herramientas.ConvertirATexto(Entrada.Va21);
            diccionarioEntrada[183] = Herramientas.ConvertirATexto(Entrada.Va22);
            diccionarioEntrada[184] = Herramientas.ConvertirATexto(Entrada.Va23);
            diccionarioEntrada[185] = Herramientas.ConvertirATexto(Entrada.Va24);
            diccionarioEntrada[186] = Herramientas.ConvertirATexto(Entrada.Va25);
            diccionarioEntrada[187] = Herramientas.ConvertirATexto(Entrada.Va26);
            diccionarioEntrada[188] = Herramientas.ConvertirATexto(Entrada.Va27);
            diccionarioEntrada[189] = Herramientas.ConvertirATexto(Entrada.Va28);
            diccionarioEntrada[190] = Herramientas.ConvertirATexto(Entrada.Va29);
            diccionarioEntrada[191] = Herramientas.ConvertirATexto(Entrada.Va30);
            diccionarioEntrada[192] = Herramientas.ConvertirATexto(Entrada.Va31);
            diccionarioEntrada[193] = Herramientas.ConvertirATexto(Entrada.Va32);
            diccionarioEntrada[194] = Herramientas.ConvertirATexto(Entrada.Va33);
            diccionarioEntrada[195] = Herramientas.ConvertirATexto(Entrada.Va34);
            diccionarioEntrada[196] = Herramientas.ConvertirATexto(Entrada.Va35);
            diccionarioEntrada[197] = Herramientas.ConvertirATexto(Entrada.Va36);
            diccionarioEntrada[198] = Herramientas.ConvertirATexto(Entrada.Va37);
            diccionarioEntrada[199] = Herramientas.ConvertirATexto(Entrada.Va38);
            diccionarioEntrada[200] = Herramientas.ConvertirATexto(Entrada.Va39);
            diccionarioEntrada[201] = Herramientas.ConvertirATexto(Entrada.Va40);
            diccionarioEntrada[202] = Herramientas.ConvertirATexto(Entrada.Va41);
            diccionarioEntrada[203] = Herramientas.ConvertirATexto(Entrada.Va42);
            diccionarioEntrada[204] = Herramientas.ConvertirATexto(Entrada.Va43);
            diccionarioEntrada[205] = Herramientas.ConvertirATexto(Entrada.Va44);
            diccionarioEntrada[206] = Herramientas.ConvertirATexto(Entrada.Va45);
            diccionarioEntrada[207] = Herramientas.ConvertirATexto(Entrada.Va46);
            diccionarioEntrada[208] = Herramientas.ConvertirATexto(Entrada.Va47);
            diccionarioEntrada[209] = Herramientas.ConvertirATexto(Entrada.Va48);
            diccionarioEntrada[210] = Herramientas.ConvertirATexto(Entrada.Va49);
            diccionarioEntrada[211] = Herramientas.ConvertirATexto(Entrada.Va50);
            diccionarioEntrada[212] = Herramientas.ConvertirATexto(Entrada.TindIngresoCalculadoFinal);
            diccionarioEntrada[213] = Herramientas.ConvertirATexto(Entrada.TindTipoIngreso);
            return diccionarioEntrada;
        }

        public string GeneraTramaSalida()
        {
            var sb = new StringBuilder();
            sb.Append(_Salida.fecha.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.id_oportunidad.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.id_evaluacion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.ap_origen.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.nro_llamado.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.documento.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.edad.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_domicilio.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.ddjj_de_salud.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.ingreso_computable_neto)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afectacion_ingreso_computable_neto)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_ingreso)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.afectacion_adicional_disponible)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.pm_tarj_bureau)).Append(";")
                .Append(_Salida.sumo_pm_tj_bureau.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.sumo_sum_pagos_min_tarj_no_bureau.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.sprom_ac_bureau)).Append(";")
                .Append(_Salida.sumo_sprom_ac_bureau.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.sumo_sum_util_cta_cte_no_bureau.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.tarj_activa_peerbank.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_total_final)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.compromiso_total_actual_final)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.deuda_macro_con_garantia)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.deuda_macro_sin_garantia)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.total_deuda_macro)).Append(";")
                .Append(_Salida.resolucion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_ip.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_score.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_poblacion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.explicacion.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.categoria.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.score_applicant_categoria.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.VZ_Cliente_Macro.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.monto_prestamo)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.cuota_total)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.limite_tarjeta)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.monto_acuerdo_cta_cte)).Append(";")
                .Append(_Salida.copia_lim_externo.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.plazo.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.Nivel_de_Riesgo.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_3.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_4.ToString() ?? string.Empty).Append(";")
                .Append(_Salida.opcional_5.ToString() ?? string.Empty).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.opcional_6)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.opcional_7)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.opcional_8)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.opcional_9)).Append(";")
                .Append(Herramientas.NumeroATexto(_Salida.opcional_10))
              ;
           return sb.ToString();
        }

        public void Decodifica(string[] arraySalida)
        {
            _Salida.fecha = (string)Herramientas.ConvertirDeTexto(arraySalida[0], DbType.String, string.Empty);
            _Salida.id_oportunidad = (string)Herramientas.ConvertirDeTexto(arraySalida[1], DbType.String, string.Empty);
            _Salida.id_evaluacion = (string)Herramientas.ConvertirDeTexto(arraySalida[2], DbType.String, string.Empty);
            _Salida.ap_origen = (string)Herramientas.ConvertirDeTexto(arraySalida[3], DbType.String, string.Empty);
            _Salida.nro_llamado = (string)Herramientas.ConvertirDeTexto(arraySalida[4], DbType.String, string.Empty);
            _Salida.documento = (string)Herramientas.ConvertirDeTexto(arraySalida[5], DbType.String, string.Empty);
            _Salida.edad = (string)Herramientas.ConvertirDeTexto(arraySalida[6], DbType.String, string.Empty);
            _Salida.VZ_domicilio = (string)Herramientas.ConvertirDeTexto(arraySalida[7], DbType.String, string.Empty);
            _Salida.ddjj_de_salud = (string)Herramientas.ConvertirDeTexto(arraySalida[8], DbType.String, string.Empty);
            _Salida.ingreso_computable_neto = (decimal)Herramientas.ConvertirDeTexto(arraySalida[9], DbType.Decimal, decimal.Zero);
            _Salida.afectacion_ingreso_computable_neto = (decimal)Herramientas.ConvertirDeTexto(arraySalida[10], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_ingreso = (decimal)Herramientas.ConvertirDeTexto(arraySalida[11], DbType.Decimal, decimal.Zero);
            _Salida.afectacion_adicional_disponible = (decimal)Herramientas.ConvertirDeTexto(arraySalida[12], DbType.Decimal, decimal.Zero);
            _Salida.pm_tarj_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[13], DbType.Decimal, decimal.Zero);
            _Salida.sumo_pm_tj_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[14], DbType.String, string.Empty);
            _Salida.sumo_sum_pagos_min_tarj_no_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[15], DbType.String, string.Empty);
            _Salida.sprom_ac_bureau = (decimal)Herramientas.ConvertirDeTexto(arraySalida[16], DbType.Decimal, decimal.Zero);
            _Salida.sumo_sprom_ac_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[17], DbType.String, string.Empty);
            _Salida.sumo_sum_util_cta_cte_no_bureau = (string)Herramientas.ConvertirDeTexto(arraySalida[18], DbType.String, string.Empty);
            _Salida.tarj_activa_peerbank = (string)Herramientas.ConvertirDeTexto(arraySalida[19], DbType.String, string.Empty);
            _Salida.compromiso_total_final = (decimal)Herramientas.ConvertirDeTexto(arraySalida[20], DbType.Decimal, decimal.Zero);
            _Salida.compromiso_total_actual_final = (decimal)Herramientas.ConvertirDeTexto(arraySalida[21], DbType.Decimal, decimal.Zero);
            _Salida.deuda_macro_con_garantia = (decimal)Herramientas.ConvertirDeTexto(arraySalida[22], DbType.Decimal, decimal.Zero);
            _Salida.deuda_macro_sin_garantia = (decimal)Herramientas.ConvertirDeTexto(arraySalida[23], DbType.Decimal, decimal.Zero);
            _Salida.total_deuda_macro = (decimal)Herramientas.ConvertirDeTexto(arraySalida[24], DbType.Decimal, decimal.Zero);
            _Salida.resolucion = (string)Herramientas.ConvertirDeTexto(arraySalida[25], DbType.String, string.Empty);
            _Salida.VZ_ip = (string)Herramientas.ConvertirDeTexto(arraySalida[26], DbType.String, string.Empty);
            _Salida.VZ_score = (string)Herramientas.ConvertirDeTexto(arraySalida[27], DbType.String, string.Empty);
            _Salida.VZ_poblacion = (string)Herramientas.ConvertirDeTexto(arraySalida[28], DbType.String, string.Empty);
            _Salida.explicacion = (string)Herramientas.ConvertirDeTexto(arraySalida[29], DbType.String, string.Empty);
            _Salida.categoria = (string)Herramientas.ConvertirDeTexto(arraySalida[30], DbType.String, string.Empty);
            _Salida.score_applicant_categoria = (string)Herramientas.ConvertirDeTexto(arraySalida[31], DbType.String, string.Empty);
            _Salida.VZ_Cliente_Macro = (string)Herramientas.ConvertirDeTexto(arraySalida[32], DbType.String, string.Empty);
            _Salida.monto_prestamo = (decimal)Herramientas.ConvertirDeTexto(arraySalida[33], DbType.Decimal, decimal.Zero);
            _Salida.cuota_total = (decimal)Herramientas.ConvertirDeTexto(arraySalida[34], DbType.Decimal, decimal.Zero);
            _Salida.limite_tarjeta = (decimal)Herramientas.ConvertirDeTexto(arraySalida[35], DbType.Decimal, decimal.Zero);
            _Salida.monto_acuerdo_cta_cte = (decimal)Herramientas.ConvertirDeTexto(arraySalida[36], DbType.Decimal, decimal.Zero);
            _Salida.copia_lim_externo = (string)Herramientas.ConvertirDeTexto(arraySalida[37], DbType.String, string.Empty);
            _Salida.plazo = (string)Herramientas.ConvertirDeTexto(arraySalida[38], DbType.String, string.Empty);
            _Salida.Nivel_de_Riesgo = (string)Herramientas.ConvertirDeTexto(arraySalida[39], DbType.String, string.Empty);
            _Salida.opcional_3 = (string)Herramientas.ConvertirDeTexto(arraySalida[40], DbType.String, string.Empty);
            _Salida.opcional_4 = (string)Herramientas.ConvertirDeTexto(arraySalida[41], DbType.String, string.Empty);
            _Salida.opcional_5 = (string)Herramientas.ConvertirDeTexto(arraySalida[42], DbType.String, string.Empty);
            _Salida.opcional_6 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[43], DbType.Decimal, decimal.Zero);
            _Salida.opcional_7 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[44], DbType.Decimal, decimal.Zero);
            _Salida.opcional_8 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[45], DbType.Decimal, decimal.Zero);
            _Salida.opcional_9 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[46], DbType.Decimal, decimal.Zero);
            _Salida.opcional_10 = (decimal)Herramientas.ConvertirDeTexto(arraySalida[47], DbType.Decimal, decimal.Zero);
        }

        public void InsertaDictamenes(IDictaminador dictamenes)
        {
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
            Entrada.prov = request[13];
            Entrada.zona = request[14];
            Entrada.cod_sucursal = request[15];
            Entrada.est_civil = request[16];
            Entrada.vivienda = request[17];
            Entrada.alquiler = request[18];
            Entrada.contribuyente = request[19];
            Entrada.laboral_ppal = request[20];
            Entrada.fech_laboral = request[21];
            Entrada.neto_declarado = request[22];
            Entrada.pp_cuota_macro = request[23];
            Entrada.pp_monto_macro = request[24];
            Entrada.pr_cuota_macro = request[25];
            Entrada.pr_monto_macro = request[26];
            Entrada.ph_cuota_macro = request[27];
            Entrada.ph_monto_macro = request[28];
            Entrada.op_cuota_macro = request[29];
            Entrada.op_monto_macro = request[30];
            Entrada.lim_tarj_macro = request[31];
            Entrada.pago_min_tarj_macro = request[32];
            Entrada.acue_cta_macro = request[33];
            Entrada.uti_cta_cte_macro = request[34];
            Entrada.dias_atraso = request[35];
            Entrada.sum_cuotas_p_sg_no_bureau = request[36];
            Entrada.sum_cuotas_p_cg_no_bureau = request[37];
            Entrada.sum_pagos_min_tarj_no_bureau = request[38];
            Entrada.sum_util_cta_cte_no_bureau = request[39];
            Entrada.prestamo = request[40];
            Entrada.tarjeta = request[41];
            Entrada.cta_cte = request[42];
            Entrada.tipo_prestamo = request[43];
            Entrada.plazo = request[44];
            Entrada.destino_personal = request[45];
            Entrada.tipo_tasa = request[46];
            Entrada.tna = request[47];
            Entrada.canal = request[48];
            Entrada.equipo_ventas = request[49];
            Entrada.tipo_fuente = request[50];
            Entrada.fuente = request[51];
            Entrada.segmento = request[52];
            Entrada.subsegmento = request[53];
            Entrada.nombre_plantilla = request[54];
            Entrada.estado = request[55];
            Entrada.atributo1 = request[56];
            Entrada.valor1 = request[57];
            Entrada.atributo2 = request[58];
            Entrada.valor2 = request[59];
            Entrada.atributo3 = request[60];
            Entrada.valor3 = request[61];
            Entrada.atributo4 = request[62];
            Entrada.valor4 = request[63];
            Entrada.atributo5 = request[64];
            Entrada.valor5 = request[65];
            Entrada.atributo6 = request[66];
            Entrada.valor6 = request[67];
            Entrada.atributo7 = request[68];
            Entrada.valor7 = request[69];
            Entrada.atributo8 = request[70];
            Entrada.valor8 = request[71];
            Entrada.atributo9 = request[72];
            Entrada.valor9 = request[73];
            Entrada.atributo10 = request[74];
            Entrada.valor10 = request[75];
            Entrada.usuario = request[76];
            Entrada.PRECACOM_MONTO = request[77];
            Entrada.field79 = (request.Count() > 78) ? request[78] : string.Empty;
            Entrada.field80 = (request.Count() > 79) ? request[79] : string.Empty;
            if(request.Count() > 80) Entrada.ID_Interno = string.IsNullOrEmpty(request[80]) ? fuentes.Registro.Id.ToString() : request[80];
            else Entrada.ID_Interno = fuentes.Registro.Id.ToString();
            Entrada.field82 = (request.Count() > 81) ? request[81] : string.Empty;
            Entrada.field83 = (request.Count() > 82) ? request[82] : string.Empty;
            Entrada.field84 = (request.Count() > 83) ? request[83] : string.Empty;
            Entrada.field85 = (request.Count() > 84) ? request[84] : string.Empty;
            Entrada.field86 = (request.Count() > 85) ? request[85] : string.Empty;
            Entrada.field87 = (request.Count() > 86) ? request[86] : string.Empty;
            if (fuentes.MoraSucursal.Count > 0)
            {
                var moraSucursal = fuentes.MoraSucursal.ElementAt(0).Value;
                Entrada.Mora_Suc_6M_90 = moraSucursal.MoraSuc6M90;
                Entrada.Mora_Suc_12M_90 = moraSucursal.MoraSuc12M90;
            }
            if(fuentes.EndeudamientoAtraso.TryGetValue(integrantes.Titular?.Cobis ?? -1, out var endeudamientoAtraso)){
                Entrada.EA_CODIGO_CLIENTE = endeudamientoAtraso.CodigoCliente?.ToString() ?? string.Empty;
                Entrada.EA_FECHA_CARGA = endeudamientoAtraso.FechaCarga ?? new DateOnly(1900,1,1);
                Entrada.EA_FECHA_CABALOPE = endeudamientoAtraso.FechaCabalope ?? new DateOnly(1900, 1, 1);
                Entrada.EA_FECHA_CONSULTA_REESTRUCTURACION = endeudamientoAtraso.FechaConsultaReestructuracion ?? new DateOnly(1900, 1, 1);
                Entrada.EA_MOTIVO_REESTRUCTURACION = endeudamientoAtraso.MotivoReestructuracion ?? string.Empty;
                Entrada.EA_FECHA_CONSULTA_REFI_DEU = endeudamientoAtraso.FechaConsultaRefiDeu ?? new DateOnly(1900, 1, 1);
                Entrada.EA_MOTIVO_REFINANCIACION = endeudamientoAtraso.MotivoRefinanciacion ?? string.Empty;
                Entrada.EA_MOTIVO_DEU = endeudamientoAtraso.MotivoDeu ?? string.Empty;
            }

            if (fuentes.ClientesDepurados.TryGetValue(integrantes.Titular?.NumeroTributario.ToString() ?? "", out var clientesDepurados)){
                Entrada.CND_Cobis = (int)Herramientas.ConvertirDeTexto(clientesDepurados.CodigoCliente, DbType.Int32, 0);
                Entrada.CND_Nro_Tributario = clientesDepurados.Cuil ?? string.Empty;
                Entrada.CND_Codigo_Campana = clientesDepurados.Depuracion ?? string.Empty;
                Entrada.CND_Vencimiento = DateOnly.FromDateTime(clientesDepurados.DepuracionHasta ?? new DateTime(1900, 1, 1));
                Entrada.CND_Motivo_Rechazo = clientesDepurados.DepuracionMotivo ?? string.Empty;
            }

            if (fuentes.Behavior.TryGetValue(integrantes.Titular?.Cobis ?? -1, out var behavior)){
                Entrada.BH_id_cobis = behavior.ClienteCobis ?? 0;
                Entrada.BH_Riesgo_Score = behavior.RiesgoScore ?? 0;
                Entrada.BH_Riesgo_6 = behavior.Riesgo6 ?? 0;
                Entrada.BH_Uso_Tc_Limite = behavior.UsoTcLimite ?? decimal.Zero;
                Entrada.BH_Uso_Tc_Exigible = behavior.UsoTcExigible ?? decimal.Zero;
                Entrada.BH_Uso_Tc_Limite_Max = behavior.UsoTcLimiteMax ?? decimal.Zero;
                Entrada.BH_Uso_Cc_Limite = behavior.UsoCcLimite ?? decimal.Zero;
                Entrada.BH_Uso_Cc_Promedio3 = behavior.UsoCcPromedio3 ?? decimal.Zero;
                Entrada.BH_Uso_Sg_Exigible = behavior.UsoSgExigible ?? decimal.Zero;
                Entrada.BH_Uso_Gp_Exigible = behavior.UsoGpExigible ?? decimal.Zero;
                Entrada.BH_Uso_Gh_Exigible = behavior.UsoGhExigible ?? decimal.Zero;
                Entrada.BH_Antiguedad_Tc = behavior.AntiguedadTc ?? 0;
                Entrada.BH_Antiguedad_Cc = behavior.AntiguedadCc ?? 0;
                Entrada.BH_Cob_Status = behavior.CobStatus ?? 0;
                Entrada.BH_Hit = behavior.Hit ?? -1;
                Entrada.BH_Uso_Tc_Saldo_Fin = behavior.VUsoTcSaldoFin ?? decimal.Zero;
                Entrada.BH_IP2 = behavior.Ip2 ?? string.Empty;
                Entrada.Consultar_Veraz = behavior.ConsultarVeraz ?? string.Empty;
            }

            if (fuentes.ChequesRechazados.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out var chequesRechazados)){
                Entrada.Rech_Lev_SF_u24m_Cant = chequesRechazados.RechLevSFu24mCant ?? 0;
                Entrada.Rech_NoLev_SF_u24m_Monto = chequesRechazados.RechNoLevSFu24mMonto ?? decimal.Zero;
                Entrada.Rech_Lev_SF_u24m_Monto = chequesRechazados.RechLevSFu24mMonto ?? decimal.Zero;
            }

            if (fuentes.ScoreComportamiento.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out var scoreComportamiento)){
                Entrada.exclusion = scoreComportamiento.exclusion ?? string.Empty;
                Entrada.Score_Comp = (int)(scoreComportamiento.score ?? decimal.Zero);
                Entrada.categoria = scoreComportamiento.categoria ?? string.Empty;
            }

            if (fuentes.Tablon.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out var tablon)){
                Entrada.POL_Flag_Acuerdo_CC = tablon.POL_Flag_Acuerdo_CC ?? 0;
                Entrada.POL_Flag_Producto_Crediticio_Activo = tablon.FlagProductoCrediticioActivo ?? 0;
                Entrada.antiguedad_tc_macro = tablon.AntiguedadTc ?? decimal.Zero;
                Entrada.antiguedad_cc_macro = tablon.AntiguedadCc ?? decimal.Zero;
                Entrada.POL_Cantidad_Ocurrencia_6 = tablon.CantidadOcurrencia6 ?? 0;
            }

            if (fuentes.Excepciones.TryGetValue(integrantes.Titular?.NumeroTributario.ToString() ?? "", out var excepciones)){
                 Entrada.FECHA_VENCIMIENTO = DateOnly.FromDateTime(excepciones.FechaVencimiento ?? new DateTime(1900,1,1));
            }

            if (fuentes.ValidacionActividad.TryGetValue(integrantes.Titular?.NumeroTributario ?? -1, out var validacionActividad))
            {
                Entrada.Cuit = validacionActividad.Cuit ?? 0;
                Entrada.NombrePe = validacionActividad.NombrePe ?? string.Empty;
                Entrada.ApellidoPe = validacionActividad.ApellidoPe ?? string.Empty;
                Entrada.SexoPe = validacionActividad.SexoPe ?? string.Empty;
                Entrada.FechaNacimientoPe = DateOnly.FromDateTime(validacionActividad.FechaNacimientoPe ?? new DateTime(1900,1,1));
                Entrada.ProvinciaPe = validacionActividad.ProvinciaPe ?? string.Empty;
                Entrada.CodigoPostalPe = validacionActividad.CodigoPostalPe ?? string.Empty;
                Entrada.FechaCarga = DateOnly.FromDateTime(validacionActividad.FechaCarga ?? new DateTime(1900, 1, 1));
                Entrada.PeriodoScore = validacionActividad.PeriodoScore ?? 0;
                Entrada.Empleador = validacionActividad.Empleador ?? 0;
                Entrada.FlagCrediticio = validacionActividad.FlagCrediticio ?? 0;
                Entrada.SumScore = validacionActividad.SumScore ?? decimal.Zero;
                Entrada.RazonSocialAfip = validacionActividad.RazonSocialAfip ?? string.Empty;
                Entrada.ImpuestoGanancias = validacionActividad.ImpuestoGanancias ?? string.Empty;
                Entrada.ImpuestoIva = validacionActividad.ImpuestoIva ?? string.Empty;
                Entrada.IntegranteSociedad = validacionActividad.IntegranteSociedad ?? string.Empty;
                Entrada.EsEmpleadorAfip = validacionActividad.EsEmpleadorAfip ?? string.Empty;
                Entrada.CatMonotributo = validacionActividad.CatMonotributo ?? string.Empty;
                Entrada.ActividadMonotributo = validacionActividad.ActividadMonotributo ?? string.Empty;
                Entrada.ActividadMonotributoDescripcion = validacionActividad.ActividadMonotributoDescripcion ?? string.Empty;
                Entrada.FactMonotributo = validacionActividad.FactMonotributo ?? decimal.Zero;
                Entrada.IngresoServicios = validacionActividad.IngresoServicios ?? decimal.Zero;
                Entrada.IngresoVentas = validacionActividad.IngresoVentas ?? decimal.Zero;
                Entrada.IngresoMonotributo = validacionActividad.IngresoMonotributo ?? decimal.Zero;
                Entrada.EsEmpleado = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.EsAutonomo = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.EsMonotributista = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.EsJubilado = validacionActividad.EsJubilado ?? string.Empty;
                Entrada.IngresoTransaccionalidad = validacionActividad.IngresoTransaccionalidad ?? decimal.Zero;
                Entrada.Opcional1 = validacionActividad.Opcional1 ?? string.Empty;
                Entrada.Va1 = validacionActividad.Va1 ?? string.Empty;
                Entrada.Va2 = validacionActividad.Va2 ?? string.Empty;
                Entrada.Va3 = validacionActividad.Va3 ?? string.Empty;
                Entrada.Va4 = validacionActividad.Va4 ?? string.Empty;
                Entrada.Va5 = validacionActividad.Va5 ?? string.Empty;
                Entrada.Va6 = validacionActividad.Va6 ?? string.Empty;
                Entrada.Va7 = validacionActividad.Va7 ?? string.Empty;
                Entrada.Va8 = validacionActividad.Va8 ?? string.Empty;
                Entrada.Va9 = validacionActividad.Va9 ?? string.Empty;
                Entrada.Va10 = validacionActividad.Va10 ?? string.Empty;
                Entrada.Va11 = validacionActividad.Va11 ?? string.Empty;
                Entrada.Va12 = validacionActividad.Va12 ?? string.Empty;
                Entrada.Va13 = validacionActividad.Va13 ?? string.Empty;
                Entrada.Va14 = validacionActividad.Va14 ?? string.Empty;
                Entrada.Va15 = validacionActividad.Va15 ?? string.Empty;
                Entrada.Va16 = validacionActividad.Va16 ?? string.Empty;
                Entrada.Va17 = validacionActividad.Va17 ?? string.Empty;
                Entrada.Va18 = validacionActividad.Va18 ?? string.Empty;
                Entrada.Va19 = validacionActividad.Va19 ?? string.Empty;
                Entrada.Va20 = validacionActividad.Va20 ?? string.Empty;
                Entrada.Va21 = validacionActividad.Va21 ?? string.Empty;
                Entrada.Va22 = validacionActividad.Va22 ?? string.Empty;
                Entrada.Va23 = validacionActividad.Va23 ?? string.Empty;
                Entrada.Va24 = validacionActividad.Va24 ?? string.Empty;
                Entrada.Va25 = validacionActividad.Va25 ?? string.Empty;
                Entrada.Va26 = validacionActividad.Va26 ?? string.Empty;
                Entrada.Va27 = validacionActividad.Va27 ?? string.Empty;
                Entrada.Va28 = validacionActividad.Va28 ?? string.Empty;
                Entrada.Va29 = validacionActividad.Va29 ?? string.Empty;
                Entrada.Va30 = validacionActividad.Va30 ?? string.Empty;
                Entrada.Va31 = validacionActividad.Va31 ?? string.Empty;
                Entrada.Va32 = validacionActividad.Va32 ?? string.Empty;
                Entrada.Va33 = validacionActividad.Va33 ?? string.Empty;
                Entrada.Va34 = validacionActividad.Va34 ?? string.Empty;
                Entrada.Va35 = validacionActividad.Va35 ?? string.Empty;
                Entrada.Va36 = validacionActividad.Va36 ?? string.Empty;
                Entrada.Va37 = validacionActividad.Va37 ?? string.Empty;
                Entrada.Va38 = validacionActividad.Va38 ?? string.Empty;
                Entrada.Va39 = validacionActividad.Va39 ?? string.Empty;
                Entrada.Va40 = validacionActividad.Va40 ?? string.Empty;
                Entrada.Va41 = validacionActividad.Va41 ?? string.Empty;
                Entrada.Va42 = validacionActividad.Va42 ?? string.Empty;
                Entrada.Va43 = validacionActividad.Va43 ?? string.Empty;
                Entrada.Va44 = validacionActividad.Va44 ?? string.Empty;
                Entrada.Va45 = validacionActividad.Va45 ?? string.Empty;
                Entrada.Va46 = validacionActividad.Va46 ?? string.Empty;
                Entrada.Va47 = validacionActividad.Va47 ?? string.Empty;
                Entrada.Va48 = validacionActividad.Va48 ?? string.Empty;
                Entrada.Va49 = validacionActividad.Va49 ?? string.Empty;
                Entrada.Va50 = validacionActividad.Va50 ?? string.Empty;
            }

            if (fuentes.TablonIndividuos.TryGetValue(integrantes.Titular?.NumeroTributario.ToString() ?? "", out var tablonIndividuos))
            {
                Entrada.TindIngresoCalculadoFinal = tablonIndividuos.IngresoCalculadoFinal ?? decimal.Zero;
                Entrada.TindTipoIngreso = tablonIndividuos.TipoIngreso ?? string.Empty;
            }
        }
    }

    public class PoliticaRentaPresuntaEntrada
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
        public string prov { get; set; } = string.Empty;
        public string zona { get; set; } = string.Empty;
        public string cod_sucursal { get; set; } = string.Empty;
        public string est_civil { get; set; } = string.Empty;
        public string vivienda { get; set; } = string.Empty;
        public string alquiler { get; set; } = string.Empty;
        public string contribuyente { get; set; } = string.Empty;
        public string laboral_ppal { get; set; } = string.Empty;
        public string fech_laboral { get; set; } = string.Empty;
        public string neto_declarado { get; set; } = string.Empty;
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
        public string tipo_tasa { get; set; } = string.Empty;
        public string tna { get; set; } = string.Empty;
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
        public string PRECACOM_MONTO { get; set; } = string.Empty;
        public string field79 { get; set; } = string.Empty;
        public string field80 { get; set; } = string.Empty;
        public string ID_Interno { get; set; } = string.Empty;
        public string field82 { get; set; } = string.Empty;
        public string field83 { get; set; } = string.Empty;
        public string field84 { get; set; } = string.Empty;
        public string field85 { get; set; } = string.Empty;
        public string field86 { get; set; } = string.Empty;
        public string field87 { get; set; } = string.Empty;
        public decimal Mora_Suc_6M_90 { get; set; } = decimal.Zero;
        public decimal Mora_Suc_12M_90 { get; set; } = decimal.Zero;
        public string EA_CODIGO_CLIENTE { get; set; } = string.Empty;
        public DateOnly EA_FECHA_CARGA { get; set; } = new(1900,1,1);
        public DateOnly EA_FECHA_CABALOPE { get; set; } = new(1900, 1, 1);
        public DateOnly EA_FECHA_CONSULTA_REESTRUCTURACION { get; set; } = new(1900, 1, 1);
        public string EA_MOTIVO_REESTRUCTURACION { get; set; } = string.Empty;
        public DateOnly EA_FECHA_CONSULTA_REFI_DEU { get; set; } = new(1900, 1, 1);
        public string EA_MOTIVO_REFINANCIACION { get; set; } = string.Empty;
        public string EA_MOTIVO_DEU { get; set; } = string.Empty;
        public int CND_Cobis { get; set; } = 0;
        public string CND_Nro_Tributario { get; set; } = string.Empty;
        public string CND_Codigo_Campana { get; set; } = string.Empty;
        public DateOnly CND_Vencimiento { get; set; } = new(1900, 1, 1);
        public string CND_Motivo_Rechazo { get; set; } = string.Empty;
        public int BH_id_cobis { get; set; } = 0;
        public int BH_Riesgo_Score { get; set; } = 0;
        public int BH_Riesgo_6 { get; set; } = 0;
        public decimal BH_Uso_Tc_Limite { get; set; } = decimal.Zero;
        public decimal BH_Uso_Tc_Exigible { get; set; } = decimal.Zero;
        public decimal BH_Uso_Tc_Limite_Max { get; set; } = decimal.Zero;
        public decimal BH_Uso_Cc_Limite { get; set; } = decimal.Zero;
        public decimal BH_Uso_Cc_Promedio3 { get; set; } = decimal.Zero;
        public decimal BH_Uso_Sg_Exigible { get; set; } = decimal.Zero;
        public decimal BH_Uso_Gp_Exigible { get; set; } = decimal.Zero;
        public decimal BH_Uso_Gh_Exigible { get; set; } = decimal.Zero;
        public int BH_Antiguedad_Tc { get; set; } = 0;
        public int BH_Antiguedad_Cc { get; set; } = 0;
        public int BH_Cob_Status { get; set; } = 0;
        public int BH_Hit { get; set; } = 0;
        public decimal BH_Uso_Tc_Saldo_Fin { get; set; } = decimal.Zero;
        public string BH_IP2 { get; set; } = string.Empty;
        public string Consultar_Veraz { get; set; } = string.Empty;
        public decimal Rech_Lev_SF_u24m_Cant { get; set; } = decimal.Zero;
        public decimal Rech_NoLev_SF_u24m_Monto { get; set; } = decimal.Zero;
        public decimal Rech_Lev_SF_u24m_Monto { get; set; } = decimal.Zero;
        public string exclusion { get; set; } = string.Empty;
        public int Score_Comp { get; set; } = 0;
        public string categoria { get; set; } = string.Empty;
        public int POL_Flag_Acuerdo_CC { get; set; } = 0;
        public int POL_Flag_Producto_Crediticio_Activo { get; set; } = 0;
        public decimal antiguedad_tc_macro { get; set; } = decimal.Zero;
        public decimal antiguedad_cc_macro { get; set; } = decimal.Zero;
        public int POL_Cantidad_Ocurrencia_6 { get; set; } = 0;
        public DateOnly FECHA_VENCIMIENTO { get; set; } = new(1900, 1, 1);
        public long Cuit { get; set; } = 0;
        public string NombrePe { get; set; } = string.Empty;
        public string ApellidoPe { get; set; } = string.Empty;
        public string SexoPe { get; set; } = string.Empty;
        public DateOnly FechaNacimientoPe { get; set; } = new(1900, 1, 1);
        public string ProvinciaPe { get; set; } = string.Empty;
        public string CodigoPostalPe { get; set; } = string.Empty;
        public DateOnly FechaCarga { get; set; } = new(1900, 1, 1);
        public int PeriodoScore { get; set; } = 0;
        public long Empleador { get; set; } = 0;
        public int FlagCrediticio { get; set; } = 0;
        public decimal SumScore { get; set; } = decimal.Zero;
        public string RazonSocialAfip { get; set; } = string.Empty;
        public string ImpuestoGanancias { get; set; } = string.Empty;
        public string ImpuestoIva { get; set; } = string.Empty;
        public string IntegranteSociedad { get; set; } = string.Empty;
        public string EsEmpleadorAfip { get; set; } = string.Empty;
        public string CatMonotributo { get; set; } = string.Empty;
        public string ActividadMonotributo { get; set; } = string.Empty;
        public string ActividadMonotributoDescripcion { get; set; } = string.Empty;
        public decimal FactMonotributo { get; set; } = decimal.Zero;
        public decimal IngresoServicios { get; set; } = decimal.Zero;
        public decimal IngresoVentas { get; set; } = decimal.Zero;
        public decimal IngresoMonotributo { get; set; } = decimal.Zero;
        public string EsEmpleado { get; set; } = string.Empty;
        public string EsAutonomo { get; set; } = string.Empty;
        public string EsMonotributista { get; set; } = string.Empty;
        public string EsJubilado { get; set; } = string.Empty;
        public decimal IngresoTransaccionalidad { get; set; } = decimal.Zero;
        public string Opcional1 { get; set; } = string.Empty;
        public string Va1 { get; set; } = string.Empty;
        public string Va2 { get; set; } = string.Empty;
        public string Va3 { get; set; } = string.Empty;
        public string Va4 { get; set; } = string.Empty;
        public string Va5 { get; set; } = string.Empty;
        public string Va6 { get; set; } = string.Empty;
        public string Va7 { get; set; } = string.Empty;
        public string Va8 { get; set; } = string.Empty;
        public string Va9 { get; set; } = string.Empty;
        public string Va10 { get; set; } = string.Empty;
        public string Va11 { get; set; } = string.Empty;
        public string Va12 { get; set; } = string.Empty;
        public string Va13 { get; set; } = string.Empty;
        public string Va14 { get; set; } = string.Empty;
        public string Va15 { get; set; } = string.Empty;
        public string Va16 { get; set; } = string.Empty;
        public string Va17 { get; set; } = string.Empty;
        public string Va18 { get; set; } = string.Empty;
        public string Va19 { get; set; } = string.Empty;
        public string Va20 { get; set; } = string.Empty;
        public string Va21 { get; set; } = string.Empty;
        public string Va22 { get; set; } = string.Empty;
        public string Va23 { get; set; } = string.Empty;
        public string Va24 { get; set; } = string.Empty;
        public string Va25 { get; set; } = string.Empty;
        public string Va26 { get; set; } = string.Empty;
        public string Va27 { get; set; } = string.Empty;
        public string Va28 { get; set; } = string.Empty;
        public string Va29 { get; set; } = string.Empty;
        public string Va30 { get; set; } = string.Empty;
        public string Va31 { get; set; } = string.Empty;
        public string Va32 { get; set; } = string.Empty;
        public string Va33 { get; set; } = string.Empty;
        public string Va34 { get; set; } = string.Empty;
        public string Va35 { get; set; } = string.Empty;
        public string Va36 { get; set; } = string.Empty;
        public string Va37 { get; set; } = string.Empty;
        public string Va38 { get; set; } = string.Empty;
        public string Va39 { get; set; } = string.Empty;
        public string Va40 { get; set; } = string.Empty;
        public string Va41 { get; set; } = string.Empty;
        public string Va42 { get; set; } = string.Empty;
        public string Va43 { get; set; } = string.Empty;
        public string Va44 { get; set; } = string.Empty;
        public string Va45 { get; set; } = string.Empty;
        public string Va46 { get; set; } = string.Empty;
        public string Va47 { get; set; } = string.Empty;
        public string Va48 { get; set; } = string.Empty;
        public string Va49 { get; set; } = string.Empty;
        public string Va50 { get; set; } = string.Empty;
        public decimal TindIngresoCalculadoFinal { get; set; } = decimal.Zero;
        public string TindTipoIngreso { get; set; } = string.Empty;
    }

    public class PoliticaRentaPresuntaSalida
    {
        public string fecha { get; set; } = string.Empty;
        public string id_oportunidad { get; set; } = string.Empty;
        public string id_evaluacion { get; set; } = string.Empty;
        public string ap_origen { get; set; } = string.Empty;
        public string nro_llamado { get; set; } = string.Empty;
        public string documento { get; set; } = string.Empty;
        public string edad { get; set; } = string.Empty;
        public string VZ_domicilio { get; set; } = string.Empty;
        public string ddjj_de_salud { get; set; } = string.Empty;
        public decimal ingreso_computable_neto { get; set; } = decimal.Zero;
        public decimal afectacion_ingreso_computable_neto { get; set; } = decimal.Zero;
        public decimal compromiso_ingreso { get; set; } = decimal.Zero;
        public decimal afectacion_adicional_disponible { get; set; } = decimal.Zero;
        public decimal pm_tarj_bureau { get; set; } = decimal.Zero;
        public string sumo_pm_tj_bureau { get; set; } = string.Empty;
        public string sumo_sum_pagos_min_tarj_no_bureau { get; set; } = string.Empty;
        public decimal sprom_ac_bureau { get; set; } = decimal.Zero;
        public string sumo_sprom_ac_bureau { get; set; } = string.Empty;
        public string sumo_sum_util_cta_cte_no_bureau { get; set; } = string.Empty;
        public string tarj_activa_peerbank { get; set; } = string.Empty;
        public decimal compromiso_total_final { get; set; } = decimal.Zero;
        public decimal compromiso_total_actual_final { get; set; } = decimal.Zero;
        public decimal deuda_macro_con_garantia { get; set; } = decimal.Zero;
        public decimal deuda_macro_sin_garantia { get; set; } = decimal.Zero;
        public decimal total_deuda_macro { get; set; } = decimal.Zero;
        public string resolucion { get; set; } = string.Empty;
        public string VZ_ip { get; set; } = string.Empty;
        public string VZ_score { get; set; } = string.Empty;
        public string VZ_poblacion { get; set; } = string.Empty;
        public string explicacion { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public string score_applicant_categoria { get; set; } = string.Empty;
        public string VZ_Cliente_Macro { get; set; } = string.Empty;
        public decimal monto_prestamo { get; set; } = decimal.Zero;
        public decimal cuota_total { get; set; } = decimal.Zero;
        public decimal limite_tarjeta { get; set; } = decimal.Zero;
        public decimal monto_acuerdo_cta_cte { get; set; } = decimal.Zero;
        public string copia_lim_externo { get; set; } = string.Empty;
        public string plazo { get; set; } = string.Empty;
        public string Nivel_de_Riesgo { get; set; } = string.Empty;
        public string opcional_3 { get; set; } = string.Empty;
        public string opcional_4 { get; set; } = string.Empty;
        public string opcional_5 { get; set; } = string.Empty;
        public decimal opcional_6 { get; set; } = decimal.Zero;
        public decimal opcional_7 { get; set; } = decimal.Zero;
        public decimal opcional_8 { get; set; } = decimal.Zero;
        public decimal opcional_9 { get; set; } = decimal.Zero;
        public decimal opcional_10 { get; set; } = decimal.Zero;
    }
}