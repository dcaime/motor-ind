using Azure.Core;
using EvaluaIndividuos.Database;
using EvaluaIndividuos.Database.MEBA.Model;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EvaluaIndividuos.Model
{
    public class PoliticaEvaluaIndividuos : IEvaluable, IDictaminable
    {
        public PoliticaEvaluaIndividuosEntrada Entrada { get; set; } = new();

        private PoliticaEvaluaIndividuosSalida _Salida;

        private Politica _Politica;
         
        public PoliticaEvaluaIndividuos()
        {
            _Politica = Politica.EvaluaIndividuos;
        }

        public PoliticaEvaluaIndividuos(PoliticaEvaluaIndividuosEntrada entrada) : this()
        {
            Entrada = entrada;
            GarantizaValores();
        }

        public PoliticaEvaluaIndividuosSalida Salida
        {
            get { return _Salida; }
        }

        public Politica GetPolitica()
        {
            return _Politica;
        }

        public async Task<bool> EvaluaPrecalificado(Parametros parametros, Registro registro)
        {
            if (parametros.GetInt("Switch_Precalificados") < Random.Shared.Next(1, 100)) return false;
            var aux = new Dictionary<string, object>();
            aux["parametros"] = parametros;
            aux["registro"] = registro;
            aux["errores"] = Validar();
            if (!(Entrada.COT_3_Cobis is 0 or null && Entrada.COT_3_Cuil is 0 or null && Entrada.COT_3_Numero_Documento is 0 or null)) aux["Integrantes"] = 4;
            else if (!(Entrada.COT_2_Cobis is 0 or null && Entrada.COT_2_Cuil is 0 or null && Entrada.COT_2_Numero_Documento is 0 or null)) aux["Integrantes"] = 3;
            else if (!(Entrada.COT_1_Cobis is 0 or null && Entrada.COT_1_Cuil is 0 or null && Entrada.COT_1_Numero_Documento is 0 or null)) aux["Integrantes"] = 2;
            else aux["Integrantes"] = 1;
            if (Entrada.Comercio_Usuario is null) Entrada.Comercio_Usuario = "";
            aux["Multiscoring"] = (Entrada.Comercio_Usuario.Contains("externalappusermultiscoring")) ? "Si" : "No";
            aux["Preca_Nombre"] = ((Entrada.Solicita_Acuerdo == "SI" && Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE > 0) || (Entrada.Solicita_Prestamo == "SI" && Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO > 0) || (Entrada.Solicita_Tarjeta == "SI" && Entrada.PRECACOM_TARJETA_CREDITO_LIMITE > 0)) ? "PRECACOM" : "";
            if (Entrada.Solicita_Prestamo is "SI" && Entrada.Prestamo_Destino is "Auto uso particular") aux["SubProducto"] = "PRESTAMO PRENDARIO";
            else if (Entrada.Solicita_Prestamo is "SI") aux["SubProducto"] = "PRESTAMO PERSONAL";
            else if (Entrada.Solicita_Tarjeta is "SI") aux["SubProducto"] = "TARJETA DE CREDITO";
            else if (Entrada.Solicita_Acuerdo is "SI") aux["SubProducto"] = "ACUERDO EN CUENTA CORRIENTE";
            else aux["SubProducto"] = "";

            _Salida = new PoliticaEvaluaIndividuosSalida();
            Salida.Origen_ID = 0;
            Salida.Numero_Oportunidad = 0;
            Salida.Numero_Evaluacion = 0;
            Salida.Status = string.Empty;
            Salida.Mensaje = string.Empty;
            Salida.Es_Cliente = string.Empty;
            Salida.Cobis = 0;
            Salida.Cuil = 0;
            Salida.Rama = string.Empty;
            Salida.Dictamen = string.Empty;
            Salida.Codigos_Rechazo = string.Empty;
            Salida.Explicacion = string.Empty;
            Salida.Prestamo_Monto_Maximo = 0;
            Salida.Prestamo_Cuota_Maxima = 0;
            Salida.Prestamo_Plazo_Maximo = 0;
            Salida.Prestamo_Tipo_Tasa = string.Empty;
            Salida.Prestamo_Tna = 0;
            Salida.Tarjeta_Credito_Limite = 0;
            Salida.Cuenta_Corriente_Limite = 0;
            Salida.Reporte_Politica = string.Empty;
            Salida.Output_Adicional = string.Empty;
            Salida.Nivel_de_Riesgo = string.Empty;
            Salida.Ingreso_Computable = 0;
            Salida.Version_Orquestador = string.Empty;
            Salida.Tipo_Prestamo = string.Empty;

            if (aux["errores"] is not "" || !SuperaControlTitular())
            {
                Salida.Rama = "VALIDACION";
                Salida.Dictamen = "Rechazar";
                if (Salida.Explicacion is "")
                {
                    Salida.Status = "ERROR";
                    if (aux["errores"] is "" or "||" && Salida.Mensaje is "" && Salida.Mensaje is "") aux["errores"] = "Error desconocido.";
                    if (Salida.Mensaje is "") Salida.Mensaje = (string)aux["errores"];
                    Salida.Mensaje = "|" + Salida.Codigos_Rechazo + "|" + Salida.Mensaje;
                    throw new MAPIException("Evaluacion Crediticia", 3, Salida.Mensaje, 409);
                }
                else
                {
                    Salida.Status = "OK";
                    Salida.Explicacion = "|" + Salida.Codigos_Rechazo + "| " + Salida.Explicacion;
                }
                return true;
            }

            //Verifica Intradiarias
            if (Entrada.tiene_intradiarias is "SI") return false;

            aux["nacimiento"] = Entrada.CLI_fecha_nacimiento ?? new DateTime(1900, 1, 1);
            aux["fecha_nacimiento_formateada"] = ((DateTime)aux["nacimiento"]).ToString("dd/MM/yyyy");

            if (Entrada.Origen_ID is 11 && Entrada.CC_CODIGO_CAMPANA is "" or null)
            {
                Salida.Origen_ID = Entrada.Origen_ID ?? 0;
                Salida.Numero_Oportunidad = Entrada.Numero_Oportunidad ?? 0;
                Salida.Numero_Evaluacion = Entrada.Numero_Evaluacion ?? 0;
                if (int.TryParse(Entrada.CLI_codigo_cliente, out var auxInt)) Salida.Cobis = auxInt;
                if (long.TryParse(Entrada.CLI_nro_doc_tributario, out var auxLong)) Salida.Cuil = auxLong;
                Salida.Rama = "CLIENTE NOPRECA";
                Salida.Dictamen = "Rechazar";
                Salida.Codigos_Rechazo = "700";
                Salida.Explicacion = "|700| Cliente no precalificado.";
                Salida.Output_Adicional = "[Categoria:" + Entrada.CLI_categoria_source + "]"
                    + "[SegmentoCRM:" + Entrada.CLI_segmento_source + "]"
                    + "[SubSegmentoCRM:" + Entrada.CLI_subsegmento_source + "]"
                    + "[FechaNacimiento:" + aux["fecha_nacimiento_formateada"] + "]"
                    + "[MarcaPEP:" + Entrada.CLI_marca_pep + "]"
                    + "[SujetoFATCAOCDE:" + "]"
                    + "[SujetoObligado:" + "]";
                return true;
            }

            if (Salida.Es_Cliente is "NO") return false; //await EvaluaNoClientePrecalificado(aux);
            
            if (Entrada.CC_CODIGO_CAMPANA is "" or null) return false;
            
            //Aplica evaluacion
            int edad = DateTime.Now.Year - ((DateTime)aux["nacimiento"]).Year;
            int segmento = CalcularSegmento();
            int subsegmento = CalcularSubsegmento(segmento, edad);
            string control_Integrante = ControlarIntegrante(segmento, subsegmento);
            string resolucion_Black_List = (Entrada.LN_Cant is not 0 and not null) ? "Rechazado" : "Aprobado";
            string explica_Black_List = Entrada.LN_Motivo + ".";
            if (resolucion_Black_List is "Rechazado") explica_Black_List = "|50| " + explica_Black_List;

            //Control_BCRA
            string control_BCRA_ult = Controlar_BCRA_ult(segmento, subsegmento);
            string control_BCRA_ult_3 = Controlar_BCRA_ult_3(segmento, subsegmento, control_BCRA_ult);
            string control_BCRA_ult_12 = "";
            string control_BCRA_ex_entidad = (Entrada.BCRADEU_origen is "MOROSOS") ? "Moroso de ex-entidades liquidadas." : "";
            string control_BCRA_producto = (Entrada.Prestamo_Destino is "PROCREAR" or "PROCREAR JOVEN" && (Entrada.BCRADEU_Peor_sit_ult_6m is > 1 and < 7 || Entrada.BCRADEU_Cant_de_sit_ireg_ult_24m is > 0)) ? "Situacion no admitida para el producto seleccionado." : "";

            //Monto_Sit_2
            var Suma_Sit_2_Ent1 = (Entrada.BCRADEU_situacion_entidad1 is "2") ? "moroso_1" : "No_moroso_1";
            var Suma_Sit_2_Ent2 = (Entrada.BCRADEU_situacion_entidad2 is "2") ? "moroso_2" : "No_moroso_2";
            var Suma_Sit_2_Ent3 = (Entrada.BCRADEU_situacion_entidad3 is "2") ? "moroso_3" : "No_moroso_3";
            var Suma_Sit_2_Ent4 = (Entrada.BCRADEU_situacion_entidad4 is "2") ? "moroso_4" : "No_moroso_4";
            var Suma_Sit_2_Ent5 = (Entrada.BCRADEU_situacion_entidad5 is "2") ? "moroso_5" : "No_moroso_5";
            var Suma_Sit_2 = (Suma_Sit_2_Ent1 is "moroso_1") ? Entrada.BCRADEU_monto_entidad1 : 0;
            Suma_Sit_2 += (Suma_Sit_2_Ent2 is "moroso_2") ? Entrada.BCRADEU_monto_entidad2 : 0;
            Suma_Sit_2 += (Suma_Sit_2_Ent3 is "moroso_3") ? Entrada.BCRADEU_monto_entidad3 : 0;
            Suma_Sit_2 += (Suma_Sit_2_Ent4 is "moroso_4") ? Entrada.BCRADEU_monto_entidad4 : 0;
            Suma_Sit_2 += (Suma_Sit_2_Ent5 is "moroso_5") ? Entrada.BCRADEU_monto_entidad5 : 0;
            //Fin Monto_Sit_2

            string Suma_Irregular_Ent1 = (Entrada.BCRADEU_situacion_entidad1 is "2" or "3" or "4" or "5") ? "Irreg_1" : "No_Irreg1";
            string Suma_Irregular_Ent2 = (Entrada.BCRADEU_situacion_entidad2 is "2" or "3" or "4" or "5") ? "Irreg_2" : "No_Irreg2";
            string Suma_Irregular_Ent3 = (Entrada.BCRADEU_situacion_entidad3 is "2" or "3" or "4" or "5") ? "Irreg_3" : "No_Irreg3";
            string Suma_Irregular_Ent4 = (Entrada.BCRADEU_situacion_entidad4 is "2" or "3" or "4" or "5") ? "Irreg_4" : "No_Irreg4";
            string Suma_Irregular_Ent5 = (Entrada.BCRADEU_situacion_entidad5 is "2" or "3" or "4" or "5") ? "Irreg_5" : "No_Irreg5";
            decimal Suma_Irregular = (Suma_Irregular_Ent1 is "Irreg_1") ? Entrada.BCRADEU_monto_entidad1 ?? decimal.Zero : decimal.Zero;
            Suma_Irregular += (Suma_Irregular_Ent2 is "Irreg_2") ? Entrada.BCRADEU_monto_entidad2 ?? decimal.Zero : decimal.Zero;
            Suma_Irregular += (Suma_Irregular_Ent3 is "Irreg_3") ? Entrada.BCRADEU_monto_entidad3 ?? decimal.Zero : decimal.Zero;
            Suma_Irregular += (Suma_Irregular_Ent4 is "Irreg_4") ? Entrada.BCRADEU_monto_entidad4 ?? decimal.Zero : decimal.Zero;
            Suma_Irregular += (Suma_Irregular_Ent5 is "Irreg_5") ? Entrada.BCRADEU_monto_entidad5 ?? decimal.Zero : decimal.Zero;
            decimal Deuda_Total = Entrada.BCRADEU_monto_entidad1 ?? 0 + Entrada.BCRADEU_monto_entidad2 ?? 0 + Entrada.BCRADEU_monto_entidad3 ?? 0 + Entrada.BCRADEU_monto_entidad4 ?? 0 + Entrada.BCRADEU_monto_entidad5 ?? 0;
            string Flexibiliza_BCRA = (segmento is 7 && subsegmento is 36 or 65
                && (control_BCRA_ult is not "" || control_BCRA_ult_12 is not "" || control_BCRA_ult_3 is not "")
                && Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m is 1
                && Suma_Irregular is <= 100
                ) ? "Aplica flexibilizacion" : "No Aplica";
            string Flexibiliza_BCRA_Privados = (
                (
                    (segmento is 6 && subsegmento is 32 or 33)
                    || (segmento is 7 && subsegmento is 35)
                    || (segmento is 8 && subsegmento is 38 or 47 or 48 or 60)
                    || (segmento is 8 && subsegmento is 1 or 2 or 9)
                )
                && (control_BCRA_ult is not "" || control_BCRA_ult_3 is not "")
                && (Entrada.BCRADEU_Peor_sit_ult_3m <= 2)
                && (Suma_Sit_2 <= 100)
                ) ? "Aplica flexibilizacion" : "No Aplica";

            if (Flexibiliza_BCRA_Privados is "Aplica flexibilizacion") Flexibiliza_BCRA = "Aplica flexibilizacion";
            
            if (Flexibiliza_BCRA is "Aplica flexibilizacion")
            {
                control_BCRA_ult = "";
                control_BCRA_ult_3 = "";
                control_BCRA_ult_12 = "";
                control_BCRA_ex_entidad = "";
                control_BCRA_producto = "";
            }
            Entrada.BCRADEU_monto_entidad1 = decimal.Round(Entrada.BCRADEU_monto_entidad1 ?? decimal.Zero, 2);
            Entrada.BCRADEU_monto_entidad2 = decimal.Round(Entrada.BCRADEU_monto_entidad2 ?? decimal.Zero, 2);
            Entrada.BCRADEU_monto_entidad3 = decimal.Round(Entrada.BCRADEU_monto_entidad3 ?? decimal.Zero, 2);
            Entrada.BCRADEU_monto_entidad4 = decimal.Round(Entrada.BCRADEU_monto_entidad4 ?? decimal.Zero, 2);
            Entrada.BCRADEU_monto_entidad5 = decimal.Round(Entrada.BCRADEU_monto_entidad5 ?? decimal.Zero, 2);
            string con_campaña = "";
            DateTime fecha_preevaluacion_bcra = DateTime.Now.Date;
            if (Entrada.CC_CODIGO_CAMPANA is not "" and not null && Entrada.CAMPEXCEP_Codigo_campana is "" or null) con_campaña = "No";
            else if (Entrada.CC_CODIGO_CAMPANA is not "" and not null && Entrada.CAMPEXCEP_Codigo_campana == Entrada.CC_CODIGO_CAMPANA) con_campaña = "Si";
            string codigo_bcra = "";
            codigo_bcra += (control_Integrante is not "") ? "-000" : "";
            codigo_bcra += (control_BCRA_ult is not "") ? "-500" : "";
            codigo_bcra += (control_BCRA_ult_12 is not "") ? "-501" : "";
            codigo_bcra += (control_BCRA_ex_entidad is not "") ? "-502" : "";
            codigo_bcra += (control_BCRA_producto is not "") ? "-503" : "";
            codigo_bcra += (control_BCRA_ult_3 is not "") ? "-504" : "";
            codigo_bcra = (codigo_bcra is "") ? codigo_bcra : codigo_bcra.Remove(0, 1);
            string explicacion_bcra = (control_Integrante + control_BCRA_ult + control_BCRA_ult_12 + control_BCRA_ult_3 + control_BCRA_ex_entidad + control_BCRA_producto).Trim();
            string bcra_exceptuado = (con_campaña is "Si"
                && Entrada.CAMPEXCEP_BCRA_EXCEPTUADO?.ToLower().Trim() is "si"
                && Entrada.CAMPEXCEP_ESTADO?.ToLower().Trim() is "vigente"
                && fecha_preevaluacion_bcra < (Entrada.CAMPEXCEP_FECHA_FIN_PREVISTO ?? new DateTime(1900, 1, 1))) ? "Si" : "No";
            string resolucion_bcra = (codigo_bcra is "" || bcra_exceptuado is "Si") ? "Aprobado" : "Rechazado";
            if (Entrada.Origen_ID is not 1 && resolucion_bcra is "Rechazado") explicacion_bcra = "|" + codigo_bcra + "|" + " " + explicacion_bcra;
            //Fin Control_BCRA

            //Control_Actividad
            string actividad_exceptuado = (con_campaña is "Si" 
                && Entrada.CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO?.ToLower().Trim() is "si"
                && Entrada.CAMPEXCEP_ESTADO?.ToLower().Trim() is "vigente"
                && fecha_preevaluacion_bcra < (Entrada.CAMPEXCEP_FECHA_FIN_PREVISTO ?? new DateTime(1900, 1, 1)))
                ? "Si" : "No";
            string requiere_validacion_ppal = ((segmento is 1 && subsegmento is not 14 and not 69)
                || segmento is 2
                || segmento is 9
                || (segmento is 3 && subsegmento is 23 or 26)
                || (segmento is 4 && subsegmento is 27 or 30)
                || (segmento is 8 && subsegmento is 41 or 49 or 50 or 61))
                ? "Si" : "No";
            string requiere_validacion = (requiere_validacion_ppal is "Si") ? "Si" : "No";
            string control_Actividad_ppal = (Entrada.VA_ES_EMPLEADO is "Si"
                || Entrada.VA_ES_AUTONOMO is "Si"
                || Entrada.VA_ES_MONOTRIBUTISTA is "Si")
                ? "" : "Actividad en canal digital no validado.";
            string codigo_Control = (control_Actividad_ppal is "") ? "" : "600";
            string explica_Control = control_Actividad_ppal;
            int control_KO = (explica_Control is not "" && segmento is not 3 and not 4) ? 1 : 0;
            string explicacion_actividad = explica_Control;
            string explicacion_actividad_aux = explicacion_actividad;
            string codigo_actividad = codigo_Control;
            string resolucion_actividad = "";
            if (requiere_validacion is "No") resolucion_actividad = "No Aplica";
            else if (control_KO is 0 || actividad_exceptuado is "Si") resolucion_actividad = "Validado";
            else resolucion_actividad = "Rechazado";
            explicacion_actividad = (Entrada.Origen_ID is not 1 && resolucion_actividad is "No Validado") ? "|" + codigo_actividad + "| " + explicacion_actividad : explicacion_actividad;
            //Fin Control_Actividad

            string ACTI_Resolucion_Final = (resolucion_bcra is "Aprobado" && resolucion_actividad is "Validado" or "No Aplica") ? "Aprobado" : "Rechazado";
            string ACTI_Resolucion = resolucion_actividad;
            string ACTI_Explicacion = explicacion_actividad;
            string BCRA_Resolucion = resolucion_bcra;
            string BCRA_Explicacion = explicacion_bcra;
            string BL_Resolucion = resolucion_Black_List;
            string BL_Explicacion = explica_Black_List;
            string BLBC_Resolucion = (BCRA_Resolucion is "Aprobado" && BL_Resolucion is "" or "Aprobado") ? "Aprobado" : "Rechazado";
            string BLBC_Explicacion;
            if (BCRA_Explicacion is not "" && BL_Explicacion is not "" and not ".") BLBC_Explicacion = BCRA_Explicacion + ". " + BL_Explicacion;
            else if (BCRA_Explicacion is not "") BLBC_Explicacion = BCRA_Explicacion;
            else if (BL_Explicacion is not "" and not ".") BLBC_Explicacion = BL_Explicacion;
            else BLBC_Explicacion = "";

            //Define rama
            string rama;
            if (BLBC_Resolucion is "Aprobado")
            {
                if (ACTI_Resolucion is "Validado" or "No Aplica")
                {
                    rama = "CLIENTE PRECA";
                }
                else
                {
                    rama = "CLIENTE NOVALAC";
                }
            }
            else
            {
                rama = "CLIENTE BL_BC";
            }

            switch (rama)
            {
                case "CLIENTE PRECA":
                    Salida.Rama = rama;
                    Salida.Dictamen = "Aprobar";
                    Salida.Explicacion = "";
                    Salida.Prestamo_Monto_Maximo = Entrada.CC_PRESTAMO_MONTO ?? decimal.Zero;
                    Salida.Prestamo_Cuota_Maxima = Entrada.CC_PRESTAMO_AFECTACION ?? decimal.Zero;
                    if ((Entrada.CC_SEGMENTO_CRM is "01" && Entrada.CC_SUBSEGMENTO_CRM is "02")
                        || (Entrada.CC_SEGMENTO_CRM is "01" && Entrada.CC_SUBSEGMENTO_CRM is "04")
                        || (Entrada.CC_SEGMENTO_CRM is "01" && Entrada.CC_SUBSEGMENTO_CRM is "56")
                        || (Entrada.CC_SEGMENTO_CRM is "01" && Entrada.CC_SUBSEGMENTO_CRM is "995")
                        || (Entrada.CC_SEGMENTO_CRM is "02" && Entrada.CC_SUBSEGMENTO_CRM is "11")
                        ) Salida.Prestamo_Cuota_Maxima = Salida.Prestamo_Cuota_Maxima * (Entrada.PM_Ajuste_cuota_maxima ?? 1);
                    if ((Entrada.CC_SEGMENTO_CRM is "01" && Entrada.CC_SUBSEGMENTO_CRM is "58")
                        || (Entrada.CC_SEGMENTO_CRM is "01" && Entrada.CC_SUBSEGMENTO_CRM is "05")
                        || (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.CC_SUBSEGMENTO_CRM is "14")
                        || (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.CC_SUBSEGMENTO_CRM is "500")
                        || (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.CC_SUBSEGMENTO_CRM is "16")
                        || (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.CC_SUBSEGMENTO_CRM is "17")
                        || (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.CC_SUBSEGMENTO_CRM is "18")
                        || (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.CC_SUBSEGMENTO_CRM is "23")
                        || (Entrada.CC_SEGMENTO_CRM is "19" && Entrada.CC_SUBSEGMENTO_CRM is "17")
                        || (Entrada.CC_SEGMENTO_CRM is "19" && Entrada.CC_SUBSEGMENTO_CRM is "23")
                        || (Entrada.CC_SEGMENTO_CRM is "19" && Entrada.CC_SUBSEGMENTO_CRM is "16")
                        || (Entrada.CC_SEGMENTO_CRM is "19" && Entrada.CC_SUBSEGMENTO_CRM is "18")
                        ) Salida.Prestamo_Monto_Maximo = Salida.Prestamo_Monto_Maximo * (Entrada.PM_Ajuste_monto ?? 1);
                    if (Entrada.Comercio_ID is 528 or 529
                        && (Salida.Prestamo_Monto_Maximo > (Entrada.Prestamo_Monto ?? 0) * (Entrada.PM_Ajuste_monto_Camuzzi ?? 1)))
                        Salida.Prestamo_Monto_Maximo = decimal.Round((Entrada.Prestamo_Monto ?? 0) * (Entrada.PM_Ajuste_monto_Camuzzi ?? 1), 2);
                    Salida.Prestamo_Plazo_Maximo = Entrada.CC_PRESTAMO_PLAZO ?? 0;
                    Salida.Prestamo_Tna = Entrada.CC_PRESTAMO_TASA ?? 0;
                    Salida.Tarjeta_Credito_Limite = Entrada.CC_TARJETA_MONTO ?? 0;
                    Salida.Cuenta_Corriente_Limite = 0;
                    Salida.Output_Adicional = "[Descripcion:" + Entrada.CC_TIPO_PRECALIFICACION + "]"
                        + "[ConveniosBeneficios:" + Entrada.CC_CONVENIO + "]"
                        + "[Segmento:" + Entrada.CC_SEGMENTO_CRM + "]"
                        + "[Subsegmento:" + Entrada.CC_SUBSEGMENTO_CRM + "]"
                        + "[Categoria:" + Entrada.CLI_categoria_source + "]"
                        + "[SegmentoCRM:" + Entrada.CLI_segmento_source + "]"
                        + "[SubSegmentoCRM:" + Entrada.CLI_subsegmento_source + "]"
                        + "[FechaNacimiento:" + aux["fecha_nacimiento_formateada"] + "]"
                        + "[MarcaPEP:" + Entrada.CLI_marca_pep + "]"
                        + "[SujetoFATCAOCDE:" + "]"
                        + "[SujetoObligado:" + "]"
                        + "[FechaVencimiento:" + Entrada.CC_FECHA_VENCIMIENTO?.ToString("yyyy-MM-dd") + "]";
                    var parts = Entrada.CC_TIPO_PRECALIFICACION.Split('|');
                    if (Entrada.Solicita_Prestamo is "SI") Salida.Nivel_de_Riesgo = parts.Length > 6 ? parts[6] : ""; ;
                    decimal ingresoComputable = 0;
                    if(parts.Length > 5)
                    {
                        decimal.TryParse(parts[5], out ingresoComputable);
                    }
                    Salida.Ingreso_Computable = ingresoComputable;
                    break;
                case "CLIENTE NOVALAC":
                    Salida.Rama = rama;
                    Salida.Dictamen = "Rechazar";
                    Salida.Codigos_Rechazo = explicacion_actividad_aux;
                    Salida.Explicacion = explicacion_actividad;
                    Salida.Output_Adicional = "[Categoria:" + Entrada.CLI_categoria_source + "]"
                        + "[SegmentoCRM:" + Entrada.CLI_segmento_source + "]"
                        + "[SubSegmentoCRM:" + Entrada.CLI_subsegmento_source + "]"
                        + "[FechaNacimiento:" + aux["fecha_nacimiento_formateada"] + "]"
                        + "[MarcaPEP:" + Entrada.CLI_marca_pep + "]"
                        + "[SujetoFATCAOCDE:" + "]"
                        + "[SujetoObligado:" + "]";
                    break;
                case "CLIENTE BL_BC":
                    Salida.Rama = rama;
                    Salida.Dictamen = "Rechazar";
                    var partes = BLBC_Explicacion?.Split('|');
                    Salida.Codigos_Rechazo = (partes?.Length > 1) ? partes[1] : "";
                    Salida.Explicacion = BLBC_Explicacion;
                    Salida.Output_Adicional = "[Descripcion:" + Entrada.CC_TIPO_PRECALIFICACION + "]"
                        + "[ConveniosBeneficios:" + Entrada.CC_CONVENIO + "]"
                        + "[Segmento:" + Entrada.CC_SEGMENTO_CRM + "]"
                        + "[Subsegmento:" + Entrada.CC_SUBSEGMENTO_CRM + "]"
                        + "[Categoria:" + Entrada.CLI_categoria_source + "]"
                        + "[SegmentoCRM:" + Entrada.CLI_segmento_source + "]"
                        + "[SubSegmentoCRM:" + Entrada.CLI_subsegmento_source + "]"
                        + "[FechaNacimiento:" + aux["fecha_nacimiento_formateada"] + "]"
                        + "[MarcaPEP:" + Entrada.CLI_marca_pep + "]"
                        + "[SujetoFATCAOCDE:" + "]"
                        + "[SujetoObligado:" + "]";
                    break;
            }

            try
            {
                if (Entrada.Solicita_Prestamo is "SI")
                {
                    Salida.Tipo_Prestamo = (Entrada.Prestamo_Destino is "Auto uso particular") ? "PRESTAMO PRENDARIO" : "PRESTAMO PERSONAL";
                }
                Salida.Status = "OK";
                Salida.Origen_ID = Entrada.Origen_ID ?? 0;
                Salida.Numero_Oportunidad = Entrada.Numero_Oportunidad ?? 0;
                Salida.Numero_Evaluacion = Entrada.Numero_Evaluacion ?? 0;
                Salida.Cobis = int.Parse(Entrada.CLI_codigo_cliente);
                Salida.Cuil = long.Parse(Entrada.CLI_nro_doc_tributario);
            }
            catch (Exception)
            {
                Log.Error("Origen, Oporunidad, Evaluacion o Cobis no pudieron ser convertidos a Entero");
            }

            return true;
            
        }

        private async Task<bool> EvaluaNoClientePrecalificado(Dictionary<string, object> aux)
        {
            DateTime fechaDefault = new DateTime();
            Registro registro = (Registro)aux["registro"];

            aux["COT_1_Neto_Fijo"] = Entrada.COT_1_Actividad_1_Neto_Fijo ?? 0 + Entrada.COT_1_Actividad_2_Neto_Fijo ?? 0;
            aux["COT_2_Neto_Fijo"] = Entrada.COT_2_Actividad_1_Neto_Fijo ?? 0 + Entrada.COT_2_Actividad_2_Neto_Fijo ?? 0;
            aux["COT_3_Neto_Fijo"] = Entrada.COT_3_Actividad_1_Neto_Fijo ?? 0 + Entrada.COT_3_Actividad_2_Neto_Fijo ?? 0;
            aux["COT_1_Neto_Variable"] = Entrada.COT_1_Actividad_1_Neto_Variable ?? 0 + Entrada.COT_1_Actividad_2_Neto_Variable ?? 0;
            aux["COT_2_Neto_Variable"] = Entrada.COT_2_Actividad_1_Neto_Variable ?? 0 + Entrada.COT_2_Actividad_2_Neto_Variable ?? 0;
            aux["COT_3_Neto_Variable"] = Entrada.COT_3_Actividad_1_Neto_Variable ?? 0 + Entrada.COT_3_Actividad_2_Neto_Variable ?? 0;

            //Seleccionar NO Precalificado
            if ((aux["Preca_Nombre"] is "" && Entrada.Comercio_ID is 528 or 529)
                || (aux["Preca_Nombre"] is not "" && Entrada.Comercio_ID is 528 or 529 && Entrada.PRECACOM_COMERCIO_ID is 0)
                || (aux["Preca_Nombre"] is "" && aux["Multiscoring"] is "Si" && Entrada.Origen_ID is 2))
            {
                Salida.Rama = "NO_CLIENTE NOPRECA";
                Salida.Dictamen = "Rechazar";
                Salida.Codigos_Rechazo = (Salida.Codigos_Rechazo is "" or null) ? "700" : "700-" + Salida.Codigos_Rechazo;
                Salida.Explicacion = (Salida.Codigos_Rechazo is "" or null) ? "No Cliente no precalificado." : "No Cliente no precalificado."+ Salida.Explicacion;
                Salida.Explicacion = "|" + Salida.Codigos_Rechazo + "| " + Salida.Explicacion;
                return true;
            }
            aux["Preca_Codigo"] = (aux["Preca_Nombre"] is "") ? "" : "PRECACOM";

            //Seleccionar Precalificado
            var acti_ent = new string[93];
            acti_ent[0] = Entrada.Numero_Oportunidad?.ToString() ?? string.Empty;
            acti_ent[1] = string.Empty;
            acti_ent[2] = Entrada.Numero_Evaluacion?.ToString() ?? string.Empty;
            acti_ent[3] = DateTime.Now.ToString("dd/MM/yyyy");
            acti_ent[4] = string.Empty;
            acti_ent[5] = string.Empty;
            acti_ent[6] = Entrada.Origen_ID?.ToString() ?? string.Empty;
            if (aux["SubProducto"] is "PRESTAMO PERSONAL") acti_ent[7] = "Prestamos Personales";
            else if (aux["SubProducto"] is "ACUERDO EN CUENTA CORRIENTE") acti_ent[7] = "Cuentas Corrientes";
            else if (aux["SubProducto"] is "TARJETA DE CREDITO") acti_ent[7] = "Prestamos Personales";
            else acti_ent[7] = string.Empty;
            if (aux["SubProducto"] is "PRESTAMO PERSONAL") acti_ent[8] = "Otros Prestamos";
            else if (aux["SubProducto"] is "ACUERDO EN CUENTA CORRIENTE") acti_ent[8] = "Cuentas Corrientes";
            else if (aux["SubProducto"] is "TARJETA DE CREDITO") acti_ent[8] = "Prestamos Personales";
            else acti_ent[8] = string.Empty;
            acti_ent[9] = aux["SubProducto"].ToString();
            if (Entrada.Solicita_Prestamo is "NO") acti_ent[10] = string.Empty;
            else if (Entrada.Prestamo_Destino is "") acti_ent[10] = "Consumo Personal";
            else acti_ent[10] = Entrada.Prestamo_Destino;
            acti_ent[11] = (Salida.Es_Cliente is "NO") ? "" : Salida.Cobis.ToString();
            acti_ent[12] = (Entrada.TIT_Tipo_Documento is "" or null) ? "DNI" : Entrada.TIT_Tipo_Documento;
            acti_ent[13] = Entrada.TIT_Version_Documento;
            acti_ent[14] = Entrada.TIT_Cuil.ToString().Substring(2, 8);
            acti_ent[15] = Entrada.TIT_Cuil.ToString();
            acti_ent[16] = Entrada.TIT_Nombre;
            acti_ent[17] = Entrada.TIT_Apellido;
            acti_ent[18] = Entrada.TIT_Sexo;
            acti_ent[19] = Entrada.TIT_Nacimiento?.ToString("dd/MM/yyyy") ?? "";
            acti_ent[20] = "";
            acti_ent[21] = (Entrada.Sucursal_Macro_Asociada is 0 or null) ? "811" : Entrada.Sucursal_Macro_Asociada.ToString();
            acti_ent[22] = "ORQUESTADOR";
            acti_ent[23] = Entrada.Comercio_ID?.ToString() ?? "";
            acti_ent[24] = (aux["Preca_Nombre"] is "") ? "Otras fuentes" : "Precalificacion";
            acti_ent[25] = "";
            acti_ent[26] = (aux["Preca_Codigo"] is "") ? "" : aux["Preca_Codigo"].ToString();
            acti_ent[27] = aux["Preca_Nombre"].ToString();
            acti_ent[28] = "MERCADO ABIERTO";
            acti_ent[29] = "";
            acti_ent[30] = Entrada.Comercio_Sucursal ?? "" + ":" + Entrada.Comercio_Usuario;
            acti_ent[31] = Entrada.TIT_Actividad_1;
            acti_ent[32] = Entrada.TIT_Actividad_2;
            acti_ent[33] = (Entrada.TIT_Actividad_1_Neto_Fijo ?? 0 + Entrada.TIT_Actividad_2_Neto_Fijo ?? 0).ToString();
            acti_ent[34] = (Entrada.TIT_Actividad_1_Neto_Variable ?? 0 + Entrada.TIT_Actividad_2_Neto_Variable ?? 0).ToString();
            acti_ent[35] = ((decimal)aux["COT_1_Neto_Fijo"] + (decimal)aux["COT_2_Neto_Fijo"] + (decimal)aux["COT_3_Neto_Fijo"]).ToString();
            acti_ent[36] = ((decimal)aux["COT_1_Neto_Variable"] + (decimal)aux["COT_2_Neto_Variable"] + (decimal)aux["COT_3_Neto_Variable"]).ToString();
            acti_ent[37] = ((int)aux["Integrantes"] > 1) ? "SI" : "NO";
            acti_ent[38] = Entrada.COT_1_Cobis?.ToString() ?? "";
            acti_ent[39] = Entrada.COT_1_Tipo_Documento?.ToString() ?? "";
            acti_ent[40] = Entrada.COT_1_Version_Documento;
            acti_ent[41] = Entrada.COT_1_Numero_Documento?.ToString() ?? "";
            acti_ent[42] = Entrada.COT_1_Nombre;
            acti_ent[43] = Entrada.COT_1_Apellido;
            acti_ent[44] = Entrada.COT_1_Sexo;
            acti_ent[45] = Entrada.COT_1_Cuil?.ToString() ?? "";
            acti_ent[46] = (Entrada.COT_1_Segmento is "") ? "MERCADO ABIERTO" : "";
            acti_ent[47] = Entrada.COT_1_Subsegmento;
            acti_ent[48] = Entrada.COT_1_Actividad_1;
            acti_ent[49] = Entrada.COT_1_Actividad_2;
            acti_ent[50] = (Entrada.COT_1_Actividad_1_Fecha is null || Entrada.COT_1_Actividad_1_Fecha == fechaDefault) ? "" : Entrada.COT_1_Actividad_1_Fecha?.ToString("dd/MM/yyyy");
            acti_ent[51] = aux["COT_1_Neto_Fijo"].ToString();
            acti_ent[52] = aux["COT_1_Neto_Variable"].ToString();
            acti_ent[53] = Entrada.COT_2_Cobis?.ToString() ?? "";
            acti_ent[54] = Entrada.COT_2_Tipo_Documento?.ToString() ?? "";
            acti_ent[55] = Entrada.COT_2_Version_Documento;
            acti_ent[56] = Entrada.COT_2_Numero_Documento?.ToString() ?? "";
            acti_ent[57] = Entrada.COT_2_Nombre;
            acti_ent[58] = Entrada.COT_2_Apellido;
            acti_ent[59] = Entrada.COT_2_Sexo;
            acti_ent[60] = Entrada.COT_2_Cuil?.ToString() ?? "";
            acti_ent[61] = (Entrada.COT_2_Segmento is "") ? "MERCADO ABIERTO" : "";
            acti_ent[62] = Entrada.COT_2_Subsegmento;
            acti_ent[63] = Entrada.COT_2_Actividad_1;
            acti_ent[64] = Entrada.COT_2_Actividad_2;
            acti_ent[65] = (Entrada.COT_2_Actividad_1_Fecha is null || Entrada.COT_2_Actividad_1_Fecha == fechaDefault) ? "" : Entrada.COT_2_Actividad_1_Fecha?.ToString("dd/MM/yyyy");
            acti_ent[66] = aux["COT_2_Neto_Fijo"].ToString();
            acti_ent[67] = aux["COT_2_Neto_Variable"].ToString();
            acti_ent[68] = Entrada.COT_3_Cobis?.ToString() ?? "";
            acti_ent[69] = Entrada.COT_3_Tipo_Documento?.ToString() ?? "";
            acti_ent[70] = Entrada.COT_3_Version_Documento;
            acti_ent[71] = Entrada.COT_3_Numero_Documento?.ToString() ?? "";
            acti_ent[72] = Entrada.COT_3_Nombre;
            acti_ent[73] = Entrada.COT_3_Apellido;
            acti_ent[74] = Entrada.COT_3_Sexo;
            acti_ent[75] = Entrada.COT_3_Cuil?.ToString() ?? "";
            acti_ent[76] = (Entrada.COT_3_Segmento is "") ? "MERCADO ABIERTO" : "";
            acti_ent[77] = Entrada.COT_3_Subsegmento;
            acti_ent[78] = Entrada.COT_3_Actividad_1;
            acti_ent[79] = Entrada.COT_3_Actividad_2;
            acti_ent[80] = (Entrada.COT_3_Actividad_1_Fecha is null || Entrada.COT_3_Actividad_1_Fecha == fechaDefault) ? "" : Entrada.COT_3_Actividad_1_Fecha?.ToString("dd/MM/yyyy");
            acti_ent[81] = aux["COT_3_Neto_Fijo"].ToString();
            acti_ent[82] = aux["COT_3_Neto_Variable"].ToString();
            acti_ent[83] = registro.Id.ToString();
            acti_ent[84] = "";
            acti_ent[85] = "";
            acti_ent[86] = "";
            acti_ent[87] = "";
            acti_ent[88] = "";
            acti_ent[89] = "";
            acti_ent[90] = "";
            acti_ent[91] = "";
            acti_ent[92] = "";
            var acti_ent_trama = string.Join(";", acti_ent);
            registro.RegistraInicio("ACTI", acti_ent_trama);
            string acti_sal_trama = "";
            string Politica_Mensaje = "";
            int Politica_Estado = 0;

            try
            {
                acti_sal_trama = await EvaluacionCrediticia.InvocarPoliticaAsync("Politica9", acti_ent_trama)
                                                 .WaitAsync(TimeSpan.FromSeconds(25));
                registro.RegistraFin("ACTI", acti_sal_trama);
                Politica_Estado = 0;
            }
            catch (TimeoutException)
            {
                Politica_Mensaje = "Timeout";
                Politica_Estado = 7;
                registro.RegistraFin("ACTI", Politica_Mensaje);
            }
            catch (Exception ex)
            {
                Politica_Mensaje = "Error desconocido en Politica Actividad.";
                Politica_Estado = 7;
                registro.RegistraFin("ACTI", ex.Message);
            }

            if (Politica_Estado is not 0)
            {
                Salida.Codigos_Rechazo = Politica_Mensaje.ToString();
                Salida.Mensaje = Politica_Mensaje;
                Salida.Rama = "VALIDACION";
                Salida.Dictamen = "Rechazar";
                Salida.Status = "ERROR";
                throw new MAPIException("Evaluacion Crediticia", 3, Salida.Mensaje, 409);
            }

            var acti_sal = acti_sal_trama.Split(";");
            if(acti_sal.Length is 1)
            {
                throw new MAPIException("Actividad", 0, acti_sal[0], 500);
            }

            aux["ACTI_Resolucion_Final"] = acti_sal[12];
            aux["ACTI_Resolucion"] = acti_sal[24];
            aux["ACTI_Resolucion_Cotitu1"] = acti_sal[40];
            aux["ACTI_Explicacion"] = acti_sal[23] ?? "";
            aux["ACTI_Codigo_Cotitu1"] = acti_sal[34];
            aux["ACTI_Explicacion_Cotitu1"] = acti_sal[39] ?? "";
            if (aux["ACTI_Resolucion_Cotitu1"] is "No Validado" && aux["ACTI_Explicacion_Cotitu1"] is not "") aux["ACTI_Explicacion_Cotitu1"] = "|"+ aux["ACTI_Codigo_Cotitu1"]+ "| Cotitular: " + aux["ACTI_Explicacion_Cotitu1"];
            if (aux["ACTI_Explicacion_Cotitu1"] is not "") aux["ACTI_Explicacion"] = aux["ACTI_Explicacion"].ToString() + aux["ACTI_Explicacion_Cotitu1"].ToString();
            aux["BCRA_Resolucion"] = acti_sal[19];
            aux["BCRA_Resolucion_Cotitu1"] = acti_sal[35];
            aux["BCRA_Explicacion"] = acti_sal[16];
            aux["BCRA_Codigo_Cotitu1"] = acti_sal[33];
            aux["BCRA_Explicacion_Cotitu1"] = acti_sal[36];
            if (aux["BCRA_Resolucion_Cotitu1"] is "Rechazado" && aux["BCRA_Explicacion_Cotitu1"] is not "") aux["BCRA_Explicacion_Cotitu1"] = "|" + aux["BCRA_Codigo_Cotitu1"] + "| Cotitular: " + aux["BCRA_Explicacion_Cotitu1"];
            if (aux["BCRA_Explicacion_Cotitu1"] is not "") aux["BCRA_Explicacion"] = aux["BCRA_Explicacion"].ToString() + aux["BCRA_Explicacion_Cotitu1"].ToString();
            aux["BL_Resolucion"] = acti_sal[69];
            aux["BL_Explicacion"] = acti_sal[70];
            aux["BLBC_Resolucion"] = (aux["BCRA_Resolucion"] is "Aprobado"
                && aux["BCRA_Resolucion_Cotitu1"] is "Aprobado" or ""
                && aux["BL_Resolucion"] is "Aprobado" or "") ? "Aprobado" : "Rechazado";
            if (aux["BCRA_Explicacion"] is not "" && aux["BL_Explicacion"] is not "" and not ".") aux["BLBC_Explicacion"] = aux["BCRA_Explicacion"] + ". " + aux["BL_Explicacion"];
            else if (aux["BCRA_Explicacion"] is not "") aux["BLBC_Explicacion"] = aux["BCRA_Explicacion"];
            else if (aux["BL_Explicacion"] is not "" and not ".") aux["BLBC_Explicacion"] = aux["BL_Explicacion"];
            else aux["BLBC_Explicacion"] = "";
            aux["CUIT_EMPLEADOR"] = acti_sal[71];
            aux["DOMICILIO"] = acti_sal[72];
            aux["ADICIONAL_CONCATENADO"] = acti_sal[73];
            aux["SUM_SCORE"] = int.TryParse(acti_sal[74], out int score) ? score : 0;
            
            if(aux["BLBC_Resolucion"] is not "Aprobado")
            {
                Salida.Codigos_Rechazo = aux["BLBC_Explicacion"].ToString().Split("|")[3];
                Salida.Explicacion = aux["BLBC_Explicacion"].ToString() + aux["ACTI_Explicacion"].ToString();
                Salida.Mensaje = Politica_Mensaje;
                Salida.Rama = "NO_CLIENTE BL_BC";
                Salida.Dictamen = "Rechazar";
                Salida.Status = "ERROR";
                return true;
            }

            if (aux["BLBC_Resolucion"] is "Aprobado" && aux["Preca_Nombre"] is not "" && aux["ACTI_Resolucion"] is "Validado" or "No Aplica")
            {
                Salida.Rama = "NO_CLIENTE PRECACOM";
                Salida.Dictamen = "Aprobar";
                Salida.Explicacion = "";
                Salida.Prestamo_Monto_Maximo = Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO ?? 0;
                if(Entrada.Comercio_ID is 528 or 529
                    && Salida.Prestamo_Monto_Maximo > Entrada.Prestamo_Monto * Entrada.PM_Ajuste_monto_Camuzzi) Salida.Prestamo_Monto_Maximo = Entrada.Prestamo_Monto ?? 0 * Entrada.PM_Ajuste_monto_Camuzzi ?? 1;
                Salida.Prestamo_Cuota_Maxima = Entrada.PRECACOM_PRESTAMO_CUOTA_MAXIMO ?? 0;
                Salida.Prestamo_Plazo_Maximo = (int)(Entrada.PRECACOM_PRESTAMO_PLAZO_MAXIMO ?? 0);
                Salida.Prestamo_Tipo_Tasa = Entrada.PRECACOM_PRESTAMO_TIPO_TASA;
                Salida.Prestamo_Tna = Entrada.PRECACOM_PRESTAMO_TNA ?? 0;
                Salida.Tarjeta_Credito_Limite = Entrada.PRECACOM_TARJETA_CREDITO_LIMITE ?? 0;
                Salida.Cuenta_Corriente_Limite = Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE ?? 0;
                Salida.Output_Adicional = "[IngresoComputableNeto:" + Entrada.PRECACOM_COM1 + "][CodigoCampana:" + Entrada.PRECACOM_CODIGO + "]";
                if (Entrada.Origen_ID is 5) Salida.Output_Adicional += "[CuitEmpleador:" + Entrada.PRECACOM_COM3 + "]";
                return true;
            }

            return false;
        }
        private string Controlar_BCRA_ult_3(int segmento, int subsegmento, string control_BCRA_ult)
        {
            if (control_BCRA_ult is not "") return "";
            else if (segmento is 1 or 2 or 9 && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento ultimos 3m.";
            else if (segmento is 6 && (((ReadOnlySpan<int>)[32, 33]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 6 && subsegmento is 34 && Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m is > 1) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 6 && subsegmento is 34 && Entrada.BCRADEU_Peor_sit_ult_3m is 6) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 7 && subsegmento is 35 && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 7 && subsegmento is 36 or 65 && Entrada.BCRADEU_Peor_sit_ult_3m is > 2 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 7 && subsegmento is 66 && Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m is > 1) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 7 && subsegmento is 66 && Entrada.BCRADEU_Peor_sit_ult_3m is 6) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 7 && subsegmento is 37 && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[57, 58, 59, 64, 67]).Contains(subsegmento)) && Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m is > 1) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[57, 58, 59, 64, 67]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_ult_3m is 6) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 8 && subsegmento is 63 && Entrada.BCRADEU_Peor_sit_ult_3m is > 2 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[38, 47, 48, 60]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[39, 43, 44, 46, 56, 53, 54, 68, 70]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[41, 49, 50, 61]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_ult_3m is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento ultimos 3m.";
            else return "";
        }

        private string Controlar_BCRA_ult(int segmento, int subsegmento)
        {
            if (((ReadOnlySpan<int>)[1, 2, 9]).Contains(segmento) && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento.";
            else if (segmento is 6 && (((ReadOnlySpan<int>)[32, 33]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 6 && subsegmento is 34 && Entrada.BCRADEU_Cant_de_sit_irreg_ult_periodo is > 1) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 6 && subsegmento is 34 && Entrada.BCRADEU_Peor_sit_vigente is 6) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 7 && subsegmento is 35 && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 7 && (((ReadOnlySpan<int>)[36, 65]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_vigente is > 2 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 7 && subsegmento is 66 && Entrada.BCRADEU_Cant_de_sit_irreg_ult_periodo is > 1) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 7 && subsegmento is 66 && Entrada.BCRADEU_Peor_sit_vigente is 6) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 7 && subsegmento is 37 && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[57, 58, 59, 64, 67]).Contains(subsegmento)) && Entrada.BCRADEU_Cant_de_sit_irreg_ult_periodo is > 1) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[57, 58, 59, 64, 67]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_vigente is 6) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 8 && subsegmento is 63 && Entrada.BCRADEU_Peor_sit_vigente is > 2 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[38, 47, 48, 60]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[39, 43, 44, 46, 56, 53, 54, 68, 70]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            else if (segmento is 8 && (((ReadOnlySpan<int>)[41, 49, 50, 51]).Contains(subsegmento)) && Entrada.BCRADEU_Peor_sit_vigente is > 1 and < 7) return "Situacion superior a la permitida para el segmento/subsegmento.";
            return "";
        }


        private string ControlarIntegrante(int segmento, int subsegmento)
        {
            if (segmento < 0) return "Segmento.";
            else if (subsegmento < 0) return "Subsegmento.";
            else if (segmento is 1 && !((ReadOnlySpan<int>)[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 69, 998, 999]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 1.";
            else if (segmento is 2 && !((ReadOnlySpan<int>)[16, 17, 18, 19, 20, 21]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 2.";
            else if (segmento is 3 && !((ReadOnlySpan<int>)[22, 23, 24, 25, 26]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 3.";
            else if (segmento is 4 && !((ReadOnlySpan<int>)[27, 28, 29, 30]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 4.";
            else if (segmento is 5) return "Segmento 5 no aplica.";
            else if (segmento is 6 && !((ReadOnlySpan<int>)[32, 33, 34]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 6.";
            else if (segmento is 7 && !((ReadOnlySpan<int>)[35, 36, 37, 65, 66]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 7.";
            else if (segmento is 8 && !((ReadOnlySpan<int>)[38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 67, 68, 70]).Contains(subsegmento)) return "Subsegmento incorrecto para segmento 8.";
            else if (segmento is 9 && subsegmento is not 71) return "Subsegmento incorrecto para segmento 9.";
            else if (((ReadOnlySpan<int>)[45, 51, 52, 55, 10, 11, 12, 13, 62]).Contains(subsegmento)) return "Subsegmento no válido.";
            else return "";
        }

        private int CalcularSegmento()
        {
            if (Entrada.CC_SEGMENTO_CRM is "MERCADO ABIERTO") return 1;
            else if (Entrada.CC_SEGMENTO_CRM is "PROFESIONALES Y NEGOCIOS") return 2;
            else if (Entrada.CC_SEGMENTO_CRM is "SIN CONTROLES") return 3;
            else if (Entrada.CC_SEGMENTO_CRM is "SELECTA PRIVADO") return 4;
            else if (Entrada.CC_SEGMENTO_CRM is "CLIENTES DE CARTERAS COMPRADAS") return 5;
            else if (Entrada.CC_SEGMENTO_CRM is "CLTES CONV. CON COD. DE DESC") return 6;
            else if (Entrada.CC_SEGMENTO_CRM is "JUBILADOS/PENSIONADOS") return 7;
            else if (Entrada.CC_SEGMENTO_CRM is "PLAN SUELDO") return 8;
            else if (Entrada.CC_SEGMENTO_CRM is "NEGOCIOS Y PROFESIONALES") return 9;
            else if (Entrada.CC_SEGMENTO_CRM is "02" && Entrada.Origen_ID is not 1) return 7;
            else if (Entrada.CC_SEGMENTO_CRM is "04" && Entrada.Origen_ID is not 1) return 1;
            else if (Entrada.CC_SEGMENTO_CRM is "01" && Entrada.Origen_ID is not 1) return 8;
            else if (Entrada.CC_SEGMENTO_CRM is "19" && Entrada.Origen_ID is not 1) return 2;
            else return -1;
        }

        private int CalcularSubsegmento(int segmento, int edad)
        {
            int subsegmento = Entrada.CC_SUBSEGMENTO_CRM switch
            {
                "RENTA PRESUNTA - Aut" => 1,
                "RENTISTAS" => 2,
                "SOCIO O DIRECT. EMP. CLIENTE" => 3,
                "TRABAJADOR INDEPENDIENTE" => 4,
                "JUBILADOS NO MACRO" => 5,
                "RENTA PRESUNTA - Jub" => 6,
                "EMPL. DE EMPRESA SIN CONVENIO" => 7,
                "RENTA PRESUNTA - Rel" => 8,
                "SMSV-SOCIEDAD MILITAR SEG.VIDA" => 9,
                "AMAS DE CASA" => 10,
                "ASIG. FAMILIARES" => 11,
                "ESTUDIANTES" => 12,
                "OTROS SIN SEGMENTAR - 24" => 13,
                "RP - SIN ACTIVIDAD VERIFICADA" => 14,
                "EX EMPLEADO BCO MACRO" => 15,
                "COMERCIANTE" => 16,
                "PROFESIONAL INDEPENDIENTE" => 17,
                "PROVEEDORES DE SERVICIOS" => 18,
                "PROD. TABACALEROS, YERBATEROS" => 19,
                "EVAL. POR MICROEMPRENDIMIENTO" => 20,
                "EMPL. RELACION DE DEPENDENCIA" => 21,
                "SIN CONTROLES" => 22,
                "SIN CONTROLES - MA" => 23,
                "SIN CONTROLES - PLS PRIV" => 24,
                "SIN CONTROLES - PS PUB Y JUB" => 25,
                "SIN CONTROLES - RENTA PRES." => 26,
                "SELECTA PRIVADO - MA" => 27,
                "SELECTA PRIVADO - PLS PRIV" => 28,
                "SELECTA PRIVADO - PS PUB Y JUB" => 29,
                "SELECTA PRIVADO - RENTA PRES." => 30,
                "CLIENTES DE CARTERAS COMPRADAS" => 31,
                "PRIVADOS" => 32,
                "JUB/PENS AFJP-EXTERIOR" => 33,
                "PUBLICOS" => 34,
                "JUBILADOS/PENSIONADOS AFJP" => 35,
                "JUBILADOS/PENSIONADOS ANSES" => 36,
                "OTROS BANCOS" => 37,
                "ACRED. CTA Y ORD. DE 3OS A o B" => 38,
                "ACRED. CTA Y ORD. DE 3OS EB" => 39,
                "ACRED. CUENTA Y ORDEN DE 3OS" => 40,
                "ACRED. POR CBU SIN CONVENIO" => 41,
                "ACREDITACION POR CBU" => 42,
                "ASIG FAMILIARES AUH" => 43,
                "ASIG. FAMILIARES (AUH)" => 44,
                "ASIG. FAMILIARES (PROGRESAR)" => 45,
                "EMPLEADO DE MACRO" => 46,
                "EMPRESAS PRIVADAS A" => 47,
                "EMPRESAS PRIVADAS B" => 48,
                "EMPRESAS PRIVADAS C" => 49,
                "EMPRESAS PRIVADAS CB" => 50,
                "EMPRESAS PRIVADAS D" => 51,
                "EMPRESAS PRIVADAS E" => 52,
                "EMPRESAS PRIVADAS EA" => 53,
                "EMPRESAS PRIVADAS EB" => 54,
                "EMPRESAS PRIVADAS EE" => 55,
                "FUNCIONARIOS DE MACRO" => 56,
                "ORG. PUBLICOS MUNICIPALES" => 57,
                "ORG. PUBLICOS NACIONALES" => 58,
                "ORG. PUBLICOS PROVINCIALES" => 59,
                "PAGO DE HONORARIOS AB" => 60,
                "PAGO DE HONORARIOS C" => 61,
                "PAGO DE HONORARIOS E" => 62,
                "REESTRUCTURACION PS PRIV" => 63,
                "REESTRUCTURACION PS PUB" => 64,
                "PENSION GRACIABLE" => 65,
                "REESTRUCTURACION JUB" => 66,
                "FUERZA POLICIAL" => 67,
                "REESTRUCTURACION COLABORADORES" => 68,
                "REESTRUCTURACION MA" => 69,
                "CUNA -ASIG. FAMILIARES" => 70,
                "NEGOCIOS Y PROFESIONALES" => 71,

                // Case for Empty/Null CC_SUBSEGMENTO_CRM with complex logic
                null or "" or " " when segmento == 1 && Entrada.Origen_ID != 1 => Entrada.TIT_Actividad_1 switch
                {
                    "Autonomo DDJJ" or "Monotributista" => 2,
                    "Relacion de dependencia" => 998,
                    "Jubilado" => 999,
                    // Mixed logic for dependency/jubilado + age
                    _ when (Entrada.TIT_Actividad_1 is null or "" or " " or "Relacion de dependencia|Jubilado") && edad < 66 => 998,
                    _ when (Entrada.TIT_Actividad_1 is null or "" or " " or "Relacion de dependencia|Jubilado") && edad >= 66 => 999,
                    _ => -1
                },

                // Numeric codes as strings
                "02" when Entrada.Origen_ID != 1 => 59,
                "04" when Entrada.Origen_ID != 1 => 47,
                "05" when Entrada.Origen_ID != 1 => 49,
                "11" when Entrada.Origen_ID != 1 => 36,
                "14" when Entrada.Origen_ID != 1 => 7,
                "16" when Entrada.Origen_ID != 1 => 17,
                "17" when Entrada.Origen_ID != 1 => 16,
                "18" when Entrada.Origen_ID != 1 => 18,
                "23" when Entrada.Origen_ID != 1 => 19,
                "56" when Entrada.Origen_ID != 1 => 48,
                "58" when Entrada.Origen_ID != 1 => 44,
                "500" when Entrada.Origen_ID != 1 => 4,
                "995" when Entrada.Origen_ID != 1 => 41,

                // Default case (else)
                _ => -1
            };
            return subsegmento;
        }

        public bool SuperaControlTitular()
        {
            string tipoCliente;
            if (Entrada.CLI_codigo_cliente is not "0" and not "" and not null
                && Entrada.TIT_Nacimiento?.Year != Entrada.CLI_fecha_nacimiento?.Year
                && Entrada.TIT_Nacimiento?.Year != 1900)
            {
                Salida.Codigos_Rechazo += "-604";
                Salida.Explicacion += "Inconsistencia en fecha de nacimiento.";
            }
            if (Entrada.CLI_clase is "EVENTUAL" or "EVENTUAL MESA CBIO" or "HABITUAL" && Entrada.CLI_codigo_cliente is "0" or "")
            {
                Salida.Codigos_Rechazo += "-602";
                Salida.Explicacion += "Cliente habitual sin Cobis.";
            }
            if (Entrada.CND_Codigo_Campana is "TODAS")
            {
                Salida.Codigos_Rechazo += "-499";
                Salida.Explicacion += "Cliente No Deseado.";
            }
            if (Entrada.CLI_fallecido is "S")
            {
                Salida.Codigos_Rechazo += "-603";
                Salida.Explicacion += "Fallecido.";
            }
            if (Entrada.CLI_restringido is "S")
            {
                Salida.Codigos_Rechazo += "-601";
                Salida.Explicacion += "Cliente Restringido.";
            }


            //Rellenar SAL_Tipo_Cliente
            if (Entrada.CLI_clase is "HABITUAL") tipoCliente = "SI";
            else if (Entrada.CLI_clase is "EVENTUAL" or "EVENTUAL MESA CBIO") tipoCliente = "EVENTUAL";
            else if (Entrada.CLI_clase is "PROSPECTO") tipoCliente = "PROSPECTO";
            else if (Entrada.CLI_codigo_cliente is not "0" and not "" and not null) tipoCliente = "SI";
            else tipoCliente = "NO";

            if (tipoCliente is "SI" or "EVENTUAL") Salida.Es_Cliente = "SI";
            else Salida.Es_Cliente = "NO";
            if (Salida.Es_Cliente is "SI")
            {
                if (Entrada.TIT_Cobis is not null and not 0 && Entrada.TIT_Cobis?.ToString() != Entrada.CLI_codigo_cliente) Salida.Mensaje = "|802|El Cobis informado no coincide con el recuperado";
                if (Entrada.TIT_Cuil is not null and not 0 && Entrada.TIT_Cuil?.ToString() != Entrada.CLI_nro_doc_tributario) Salida.Mensaje = "|802|El Cuil informado no coincide con el recuperado";
            }
            else
            {
                if (Entrada.TIT_Cobis is not null and not 0) Salida.Mensaje = "|801|El Cobis informado no fue recuperado";
                if (Entrada.Origen_ID is 11) Salida.Mensaje = "|802|Canal no habilitado para No clientes";
            }

            if (Salida.Codigos_Rechazo is not "" and not null) Salida.Codigos_Rechazo = Salida.Codigos_Rechazo.Remove(0, 1);
            return Salida.Codigos_Rechazo is "" && Salida.Mensaje is "";
        }

        public string Validar()
        {
            string errores = "";
            int cantidadProductos = 0;
            ReadOnlySpan<string> viviendasAdmitidas = ["", "Propia", "Alquilada", "Familia", "Otros"];
            ReadOnlySpan<string> estadosCivilesAdmitidas = ["", "CASADO", "SOLTERO", "DIVORCIADO", "VIUDO", "OTRO"];
            ReadOnlySpan<string> actividadesAdmitidas = ["", "Autonomo DDJJ", "Relacion de dependencia", "Jubilado", "Monotributista", "Relacion de dependencia|Jubilado"];
            ReadOnlySpan<string> provinciasAdmitidas = ["", "Caba", "Capital Federal", "Buenos Aires", "Catamarca", "Cordoba",
                "Corrientes", "Entre Rios", "Jujuy", "Mendoza", "La Rioja", "Salta",
                "San Juan", "San Luis", "Santa Fe", "Santiago del Estero", "Tucuman",
                "Chaco", "Chubut", "Formosa", "Misiones", "Neuquen", "La Pampa",
                "Rio Negro", "Santa Cruz", "Tierra del Fuego"];
            ReadOnlySpan<string> versionesDocumentoAdmitidas = [
                "","Original","Duplicado","Triplicado","Cuatriplicado","Quintuplicado","Sextuplicado","Septuplicado","Octuplicado","Version 9"
                ,"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q"
                ,"R","S","T","U","V","W","X","Y","Z","AA","AB","AC","AD","AE","AF"
                ,"AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT"
                ,"AU","AV","AW","AX","AY","AZ"
                ];

            if (Entrada.Origen_ID is null or 0) errores += "Aplicativo Origen debe ser un número entero. ";
            if (Entrada.Numero_Oportunidad is null or 0) errores += "Oportunidad debe ser un número entero. ";
            if (Entrada.Origen_ID is not 2 and not 3 and not 5 and not 8 and not 10 and not 11 and not 12) errores += "Origen_ID no admitido. ";
            if (Entrada.Numero_Oportunidad is null or 0) errores += "Oportunidad debe ser un número entero. ";
            if (Entrada.Numero_Evaluacion is (< 1 or > 999)) errores += "Numero_Evaluacion debe estar entre 1 y 999. ";
            if (Entrada.Solicita_Acuerdo is not "SI" and not "NO" && Entrada.Solicita_Tarjeta is not "SI" and not "NO" && Entrada.Solicita_Prestamo is not "SI" and not "NO") errores += "La Solicitud de Productos debe ser SI o NO. ";
            if (Entrada.Solicita_Acuerdo is "SI") cantidadProductos++;
            if (Entrada.Solicita_Prestamo is "SI") cantidadProductos++;
            if (Entrada.Solicita_Tarjeta is "SI") cantidadProductos++;
            if (cantidadProductos is 0) errores += "Debe solicitar al menos un producto. ";
            if (cantidadProductos is > 1) errores += "Debe solicitar uno solo producto. ";
            if (!provinciasAdmitidas.Contains(Entrada.Provincia ?? "")) errores += "Provincia no admitida. ";
            if (!viviendasAdmitidas.Contains(Entrada.TIT_Vivienda ?? "")) errores += "TIT_Vivienda no admitida. ";
            if (Entrada.TIT_Cobis is null or 0 && Entrada.TIT_Cuil is null or 0 && Entrada.TIT_Numero_Documento is null or 0) errores += "Información faltante del Titu: Cobis, Cuil o DNI deben ser informados. ";
            if (Entrada.TIT_Cuil is < 20_000_000_000 or > 29_999_999_999) errores += "TIT_Cuil debe contener 11 digitos y comenzar en 2. ";
            if (!versionesDocumentoAdmitidas.Contains(Entrada.TIT_Version_Documento ?? "")) errores += "TIT_Version_Documento no admitida. ";
            if (!estadosCivilesAdmitidas.Contains(Entrada.TIT_Estado_Civil ?? "")) errores += "TIT_Estado_Civil no admitido. ";
            if (!actividadesAdmitidas.Contains(Entrada.TIT_Actividad_1 ?? "")) errores += "TIT_Actividad_1 no admitida. ";
            if (!actividadesAdmitidas.Contains(Entrada.TIT_Actividad_2 ?? "")) errores += "TIT_Actividad_2 no admitida. ";
            if (!((ReadOnlySpan<string>)["", "Consumidor Final", "Responsable Inscripto", "Responsable no categorizado"]).Contains(Entrada.TIT_Tipo_Contribuyente ?? "")) errores += "TIT_Tipo_Contribuyente no admitido. ";
            if (Entrada.COT_1_Cuil is not null and not 0)
            {
                if (!versionesDocumentoAdmitidas.Contains(Entrada.COT_1_Version_Documento ?? "")) errores += "COT_1_Version_Documento no admitida. ";
                if (!estadosCivilesAdmitidas.Contains(Entrada.COT_1_Estado_Civil ?? "")) errores += "COT_1_Estado_Civil no admitido. ";
                if (!actividadesAdmitidas.Contains(Entrada.COT_1_Actividad_1 ?? "")) errores += "COT_1_Actividad_1 no admitida. ";
                if (!actividadesAdmitidas.Contains(Entrada.COT_1_Actividad_2 ?? "")) errores += "COT_1_Actividad_2 no admitida. ";
            }
            if (Entrada.COT_2_Cuil is not null and not 0)
            {
                if (!versionesDocumentoAdmitidas.Contains(Entrada.COT_2_Version_Documento ?? "")) errores += "COT_2_Version_Documento no admitida. ";
                if (!estadosCivilesAdmitidas.Contains(Entrada.COT_2_Estado_Civil ?? "")) errores += "COT_2_Estado_Civil no admitido. ";
                if (!actividadesAdmitidas.Contains(Entrada.COT_2_Actividad_1 ?? "")) errores += "COT_2_Actividad_1 no admitida. ";
                if (!actividadesAdmitidas.Contains(Entrada.COT_2_Actividad_2 ?? "")) errores += "COT_2_Actividad_2 no admitida. ";
            }
            if (Entrada.COT_3_Cuil is not null and not 0)
            {
                if (!versionesDocumentoAdmitidas.Contains(Entrada.COT_3_Version_Documento ?? "")) errores += "COT_3_Version_Documento no admitida. ";
                if (!estadosCivilesAdmitidas.Contains(Entrada.COT_3_Estado_Civil ?? "")) errores += "COT_3_Estado_Civil no admitido. ";
                if (!actividadesAdmitidas.Contains(Entrada.COT_3_Actividad_1 ?? "")) errores += "COT_3_Actividad_1 no admitida. ";
                if (!actividadesAdmitidas.Contains(Entrada.COT_3_Actividad_2 ?? "")) errores += "COT_3_Actividad_2 no admitida. ";
            }

            return errores;
        }

        public string[] Codifica()
        {
            //var diccionarioEntrada = new Dictionary<int, string>();
            var diccionarioEntrada = new string[_Politica.CamposEntrada+1];
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
            diccionarioEntrada[46] = Herramientas.ConvertirATexto(Entrada.TIT_Tipo_Contribuyente, "Consumidor Final");
            diccionarioEntrada[47] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_PP, "0");
            diccionarioEntrada[48] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_PP, "0");
            diccionarioEntrada[49] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_PR, "0");
            diccionarioEntrada[50] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_PR, "0");
            diccionarioEntrada[51] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_PH, "0");
            diccionarioEntrada[52] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_PH, "0");
            diccionarioEntrada[53] = Herramientas.ConvertirATexto(Entrada.SUM_cuota_actual_OP, "0");
            diccionarioEntrada[54] = Herramientas.ConvertirATexto(Entrada.SUM_monto_deuda_actual_OP, "0");
            diccionarioEntrada[55] = Herramientas.ConvertirATexto(Entrada.SUM_limites_TC_Macro, "0");
            diccionarioEntrada[56] = Herramientas.ConvertirATexto(Entrada.SUM_pago_min_prom_ult2_TC_Macro, "0");
            diccionarioEntrada[57] = Herramientas.ConvertirATexto(Entrada.SUM_acuerdos_cc_Macro, "0");
            diccionarioEntrada[58] = Herramientas.ConvertirATexto(Entrada.SUM_uso_prom_ult2_acuerdos_cc_Macro, "0");
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
            diccionarioEntrada[153] = Herramientas.ConvertirATexto(Entrada.NUC_Numero);
            diccionarioEntrada[154] = Herramientas.ConvertirATexto(Entrada.NUC_Tipo);
            diccionarioEntrada[155] = Herramientas.ConvertirATexto(Entrada.NUC_Medio);
            diccionarioEntrada[156] = Herramientas.ConvertirATexto(Entrada.NUC_Adicional);
            diccionarioEntrada[157] = Herramientas.ConvertirATexto(Entrada.CND_Codigo_Campana);
            diccionarioEntrada[158] = Herramientas.ConvertirATexto(Entrada.CLI_nombre);
            diccionarioEntrada[159] = Herramientas.ConvertirATexto(Entrada.CLI_apellido);
            diccionarioEntrada[160] = Herramientas.ConvertirATexto(Entrada.CLI_codigo_cliente);
            diccionarioEntrada[161] = Herramientas.ConvertirATexto(Entrada.CLI_nro_doc_tributario);
            diccionarioEntrada[162] = Herramientas.ConvertirATexto(Entrada.CLI_fecha_nacimiento, "19000101", "yyyyMMdd");
            diccionarioEntrada[163] = Herramientas.ConvertirATexto(Entrada.CLI_categoria_source);
            diccionarioEntrada[164] = Herramientas.ConvertirATexto(Entrada.CLI_clase);
            diccionarioEntrada[165] = Herramientas.ConvertirATexto(Entrada.CLI_restringido);
            diccionarioEntrada[166] = Herramientas.ConvertirATexto(Entrada.CLI_marca_pep);
            diccionarioEntrada[167] = Herramientas.ConvertirATexto(Entrada.CLI_fallecido);
            diccionarioEntrada[168] = Herramientas.ConvertirATexto(Entrada.CLI_banca);
            diccionarioEntrada[169] = Herramientas.ConvertirATexto(Entrada.CLI_segmento_source);
            diccionarioEntrada[170] = Herramientas.ConvertirATexto(Entrada.CLI_subsegmento_source);
            diccionarioEntrada[171] = Herramientas.ConvertirATexto(Entrada.CC_ID_CAMPANA);
            diccionarioEntrada[172] = Herramientas.ConvertirATexto(Entrada.CC_CODIGO_CAMPANA);
            diccionarioEntrada[173] = Herramientas.ConvertirATexto(Entrada.CC_SEGMENTO_CRM);
            diccionarioEntrada[174] = Herramientas.ConvertirATexto(Entrada.CC_SUBSEGMENTO_CRM);
            diccionarioEntrada[175] = Herramientas.ConvertirATexto(Entrada.CC_SUCURSAL_MIEMBRO);
            diccionarioEntrada[176] = Herramientas.ConvertirATexto(Entrada.CC_CANAL);
            diccionarioEntrada[177] = Herramientas.ConvertirATexto(Entrada.CC_TIPO_PRECALIFICACION);
            diccionarioEntrada[178] = Herramientas.ConvertirATexto(Entrada.CC_FECHA_CALIFICACION, "", "yyyyMMdd");
            diccionarioEntrada[179] = Herramientas.ConvertirATexto(Entrada.CC_FECHA_VENCIMIENTO, "", "yyyyMMdd");
            diccionarioEntrada[180] = Herramientas.ConvertirATexto(Entrada.CC_CONVENIO);
            diccionarioEntrada[181] = Herramientas.ConvertirATexto(Entrada.CC_PRODUCTO_PRESTAMO);
            diccionarioEntrada[182] = Herramientas.ConvertirATexto(Entrada.CC_PRODUCTO_TARJETA);
            diccionarioEntrada[183] = Herramientas.ConvertirATexto(Entrada.CC_PRODUCTO_ACUERDO);
            diccionarioEntrada[184] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_TASA, decimal.Zero.ToString());
            diccionarioEntrada[185] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_PLAZO, "0");
            diccionarioEntrada[186] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_MONTO, decimal.Zero.ToString());
            diccionarioEntrada[187] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_AFECTACION, decimal.Zero.ToString());
            diccionarioEntrada[188] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_CUOTA_PROMEDIO, decimal.Zero.ToString());
            diccionarioEntrada[189] = Herramientas.ConvertirATexto(Entrada.CC_PRESTAMO_CUOTA_INICIAL, decimal.Zero.ToString());
            diccionarioEntrada[190] = Herramientas.ConvertirATexto(Entrada.CC_TARJETA_MONTO, decimal.Zero.ToString());
            diccionarioEntrada[191] = Herramientas.ConvertirATexto(Entrada.CC_ACUERDO_MONTO, decimal.Zero.ToString());
            diccionarioEntrada[192] = Herramientas.ConvertirATexto(Entrada.CC_ID_OFERTAS);
            diccionarioEntrada[193] = Herramientas.ConvertirATexto(Entrada.PRECACOM_CODIGO);
            diccionarioEntrada[194] = Herramientas.ConvertirATexto(Entrada.PRECACOM_CUIL);
            diccionarioEntrada[195] = Herramientas.ConvertirATexto(Entrada.PRECACOM_ORIGEN_ID);
            diccionarioEntrada[196] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COMERCIO_ID);
            diccionarioEntrada[197] = Herramientas.ConvertirATexto(Entrada.PRECACOM_ID_ALTERNATIVO);
            diccionarioEntrada[198] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO, decimal.Zero.ToString());
            diccionarioEntrada[199] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_PLAZO_MAXIMO, decimal.Zero.ToString());
            diccionarioEntrada[200] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_TNA, decimal.Zero.ToString());
            diccionarioEntrada[201] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_TIPO_TASA);
            diccionarioEntrada[202] = Herramientas.ConvertirATexto(Entrada.PRECACOM_PRESTAMO_CUOTA_MAXIMO, decimal.Zero.ToString());
            diccionarioEntrada[203] = Herramientas.ConvertirATexto(Entrada.PRECACOM_TARJETA_CREDITO_LIMITE, decimal.Zero.ToString());
            diccionarioEntrada[204] = Herramientas.ConvertirATexto(Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE, decimal.Zero.ToString());
            diccionarioEntrada[205] = Herramientas.ConvertirATexto(Entrada.PRECACOM_VIGENTE_DESDE, "19000101", "yyyyMMdd");
            diccionarioEntrada[206] = Herramientas.ConvertirATexto(Entrada.PRECACOM_VIGENTE_HASTA, "19000101", "yyyyMMdd");
            diccionarioEntrada[207] = Herramientas.ConvertirATexto(Entrada.PRECACOM_VIGENCIA, "0");
            diccionarioEntrada[208] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COM1);
            diccionarioEntrada[209] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COM2);
            diccionarioEntrada[210] = Herramientas.ConvertirATexto(Entrada.PRECACOM_COM3);
            diccionarioEntrada[211] = Herramientas.ConvertirATexto(Entrada.TIND_CODIGO_CLIENTE);
            diccionarioEntrada[212] = Herramientas.ConvertirATexto(Entrada.TIND_SEGMENTO);
            diccionarioEntrada[213] = Herramientas.ConvertirATexto(Entrada.TIND_CALIFICACION_CONV_PRINCIPAL);
            diccionarioEntrada[214] = Herramientas.ConvertirATexto(Entrada.TIND_NRO_CUENTA_PRINCIPAL);
            diccionarioEntrada[215] = Herramientas.ConvertirATexto(Entrada.TIND_COD_CATEGORIA_CUENTA);
            diccionarioEntrada[216] = Herramientas.ConvertirATexto(Entrada.TIND_MARCA_NO_PRECALIFICAR, "0");
            diccionarioEntrada[217] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_SITUACION_BM, "0");
            diccionarioEntrada[218] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_SIT_CART_CEDIDA, "0");
            diccionarioEntrada[219] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_MORA_HISTORICA, "0");
            diccionarioEntrada[220] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_BANCA_CLIENTE, "0");
            diccionarioEntrada[221] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_REFINANCIACION, "0");
            diccionarioEntrada[222] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_REESTRUCTURACION, "0");
            diccionarioEntrada[223] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_DIFERIMIENTO, "0");
            diccionarioEntrada[224] = Herramientas.ConvertirATexto(Entrada.TIND_CATEGORIA_MONOTRIBUTO);
            diccionarioEntrada[225] = Herramientas.ConvertirATexto(Entrada.TIND_ACTIVIDAD_MONOTRIBUTO);
            diccionarioEntrada[226] = Herramientas.ConvertirATexto(Entrada.TIND_MONTO_INVERSION, decimal.Zero.ToString());
            diccionarioEntrada[227] = Herramientas.ConvertirATexto(Entrada.TIND_NO_ACREDITO_ULT_MES, "0");
            diccionarioEntrada[228] = Herramientas.ConvertirATexto(Entrada.TIND_NO_ACREDITO_ULT_2M, "0");
            diccionarioEntrada[229] = Herramientas.ConvertirATexto(Entrada.TIND_INGRESO_CALCULADO_FINAL, decimal.Zero.ToString());
            diccionarioEntrada[230] = Herramientas.ConvertirATexto(Entrada.TIND_TIPO_INGRESO);
            diccionarioEntrada[231] = Herramientas.ConvertirATexto(Entrada.TIND_CONVENIO_LIQUI_PRESTAMO);
            diccionarioEntrada[232] = Herramientas.ConvertirATexto(Entrada.TIND_ENTE_CONVENIO);
            diccionarioEntrada[233] = Herramientas.ConvertirATexto(Entrada.TIND_MERCADO);
            diccionarioEntrada[234] = Herramientas.ConvertirATexto(Entrada.TIND_SUBMERCADO);
            diccionarioEntrada[235] = Herramientas.ConvertirATexto(Entrada.TIND_SIN_MERC_SUBM, "0");
            diccionarioEntrada[236] = Herramientas.ConvertirATexto(Entrada.TIND_COD_SEGMENTO_CRM);
            diccionarioEntrada[237] = Herramientas.ConvertirATexto(Entrada.TIND_COD_SUBSEGMENTO_CRM);
            diccionarioEntrada[238] = Herramientas.ConvertirATexto(Entrada.TIND_SEXO);
            diccionarioEntrada[239] = Herramientas.ConvertirATexto(Entrada.EA_CODIGO_CLIENTE);
            diccionarioEntrada[240] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_PP, decimal.Zero.ToString());
            diccionarioEntrada[241] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_PP, decimal.Zero.ToString());
            diccionarioEntrada[242] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO, decimal.Zero.ToString());
            diccionarioEntrada[243] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO, decimal.Zero.ToString());
            diccionarioEntrada[244] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO, decimal.Zero.ToString());
            diccionarioEntrada[245] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO, decimal.Zero.ToString());
            diccionarioEntrada[246] = Herramientas.ConvertirATexto(Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS, decimal.Zero.ToString());
            diccionarioEntrada[247] = Herramientas.ConvertirATexto(Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS, decimal.Zero.ToString());
            diccionarioEntrada[248] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_ATRASO_PRESTAMOS, decimal.Zero.ToString());
            diccionarioEntrada[249] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[250] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_REFINANCIACION);
            diccionarioEntrada[251] = Herramientas.ConvertirATexto(Entrada.EA_MOTIVO_DEU);
            diccionarioEntrada[252] = Herramientas.ConvertirATexto(Entrada.EA_DIAS_ATRASO_TC, "0");
            diccionarioEntrada[253] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_DESC_SIN_ACUERDO);
            diccionarioEntrada[254] = Herramientas.ConvertirATexto(Entrada.EA_LIMITE_ACUERDO, decimal.Zero.ToString());
            diccionarioEntrada[255] = Herramientas.ConvertirATexto(Entrada.EA_UTILIZACION_ACUERDO, decimal.Zero.ToString());
            diccionarioEntrada[256] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_CLEAN_UP, "0");
            diccionarioEntrada[257] = Herramientas.ConvertirATexto(Entrada.EA_MAX_DIAS_EXCESO_ACUERDO, "0");
            diccionarioEntrada[258] = Herramientas.ConvertirATexto(Entrada.EA_LIMITE_COMPRA_TC, decimal.Zero.ToString());
            diccionarioEntrada[259] = Herramientas.ConvertirATexto(Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES, decimal.Zero.ToString());
            diccionarioEntrada[260] = Herramientas.ConvertirATexto(Entrada.LIMITE_TRANSITORIO_TOTAL, decimal.Zero.ToString());
            diccionarioEntrada[261] = Herramientas.ConvertirATexto(Entrada.LIMITE_TRANSITORIO_DIFERENCIAL, decimal.Zero.ToString());
            diccionarioEntrada[262] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad1, "0");
            diccionarioEntrada[263] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad1, decimal.Zero.ToString());
            diccionarioEntrada[264] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad2, "0");
            diccionarioEntrada[265] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad2, decimal.Zero.ToString());
            diccionarioEntrada[266] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad3, "0");
            diccionarioEntrada[267] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad3, decimal.Zero.ToString());
            diccionarioEntrada[268] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad4, "0");
            diccionarioEntrada[269] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad4, decimal.Zero.ToString());
            diccionarioEntrada[270] = Herramientas.ConvertirATexto(Entrada.BCRADEU_situacion_entidad5, "0");
            diccionarioEntrada[271] = Herramientas.ConvertirATexto(Entrada.BCRADEU_monto_entidad5, decimal.Zero.ToString());
            diccionarioEntrada[272] = Herramientas.ConvertirATexto(Entrada.BCRADEU_origen);
            diccionarioEntrada[273] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_ireg_ult_24m, "0");
            diccionarioEntrada[274] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Peor_sit_vigente, "0");
            diccionarioEntrada[275] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Peor_sit_ult_6m, "0");
            diccionarioEntrada[276] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_irreg_ult_periodo, "0");
            diccionarioEntrada[277] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Peor_sit_ult_3m, "0");
            diccionarioEntrada[278] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_2_ult_3m, "0");
            diccionarioEntrada[279] = Herramientas.ConvertirATexto(Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m, "0");
            diccionarioEntrada[280] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_Codigo_campana);
            diccionarioEntrada[281] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_FECHA_FIN_PREVISTO, "19000101", "yyyyMMdd");
            diccionarioEntrada[282] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_ESTADO);
            diccionarioEntrada[283] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_BCRA_EXCEPTUADO);
            diccionarioEntrada[284] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO);
            diccionarioEntrada[285] = Herramientas.ConvertirATexto(Entrada.CAMPEXCEP_SUCURSAL_EXCLUSIVA);
            diccionarioEntrada[286] = Herramientas.ConvertirATexto(Entrada.LN_Cant, "0");
            diccionarioEntrada[287] = Herramientas.ConvertirATexto(Entrada.LN_Motivo);
            diccionarioEntrada[288] = Herramientas.ConvertirATexto(Entrada.VA_ES_AUTONOMO);
            diccionarioEntrada[289] = Herramientas.ConvertirATexto(Entrada.VA_ES_EMPLEADO);
            diccionarioEntrada[290] = Herramientas.ConvertirATexto(Entrada.VA_ES_MONOTRIBUTISTA);
            diccionarioEntrada[291] = Herramientas.ConvertirATexto(Entrada.dias_vigencia, "0");
            diccionarioEntrada[292] = Herramientas.ConvertirATexto(Entrada.PM_Ajuste_monto_Camuzzi, decimal.Zero.ToString());
            diccionarioEntrada[293] = Herramientas.ConvertirATexto(Entrada.PM_Ajuste_monto, decimal.Zero.ToString());
            diccionarioEntrada[294] = Herramientas.ConvertirATexto(Entrada.PM_Ajuste_cuota_maxima, decimal.Zero.ToString());
            diccionarioEntrada[295] = Herramientas.ConvertirATexto(Entrada.PM_PrecaAConsultar);
            diccionarioEntrada[296] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_codigo_cliente);
            diccionarioEntrada[297] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_fecha_nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[298] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_clase);
            diccionarioEntrada[299] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_restringido);
            diccionarioEntrada[300] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_fallecido);
            diccionarioEntrada[301] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_banca);
            diccionarioEntrada[302] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_codigo_cliente);
            diccionarioEntrada[303] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_fecha_nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[304] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_clase);
            diccionarioEntrada[305] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_restringido);
            diccionarioEntrada[306] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_fallecido);
            diccionarioEntrada[307] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_banca);
            diccionarioEntrada[308] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_codigo_cliente);
            diccionarioEntrada[309] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_fecha_nacimiento, "", "yyyyMMdd");
            diccionarioEntrada[310] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_clase);
            diccionarioEntrada[311] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_restringido);
            diccionarioEntrada[312] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_fallecido);
            diccionarioEntrada[313] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_banca);
            diccionarioEntrada[314] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_SEGMENTO);
            diccionarioEntrada[315] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MARCA_NO_PRECALIFICAR, "0");
            diccionarioEntrada[316] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_SITUACION_BM, "0");
            diccionarioEntrada[317] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_SIT_CART_CEDIDA, "0");
            diccionarioEntrada[318] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_MORA_HISTORICA, "0");
            diccionarioEntrada[319] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_REFINANCIACION, "0");
            diccionarioEntrada[320] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_REESTRUCTURACION, "0");
            diccionarioEntrada[321] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_DIFERIMIENTO, "0");
            diccionarioEntrada[322] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_NO_ACREDITO_ULT_MES, "0");
            diccionarioEntrada[323] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_NO_ACREDITO_ULT_2M, "0");
            diccionarioEntrada[324] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_INGRESO_CALCULADO_FINAL, decimal.Zero.ToString());
            diccionarioEntrada[325] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_TIPO_INGRESO);
            diccionarioEntrada[326] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_SEGMENTO);
            diccionarioEntrada[327] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MARCA_NO_PRECALIFICAR, "0");
            diccionarioEntrada[328] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_SITUACION_BM, "0");
            diccionarioEntrada[329] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_SIT_CART_CEDIDA, "0");
            diccionarioEntrada[330] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_MORA_HISTORICA, "0");
            diccionarioEntrada[331] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_REFINANCIACION, "0");
            diccionarioEntrada[332] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_REESTRUCTURACION, "0");
            diccionarioEntrada[333] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_DIFERIMIENTO, "0");
            diccionarioEntrada[334] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_NO_ACREDITO_ULT_MES, "0");
            diccionarioEntrada[335] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_NO_ACREDITO_ULT_2M, "0");
            diccionarioEntrada[336] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_INGRESO_CALCULADO_FINAL, decimal.Zero.ToString());
            diccionarioEntrada[337] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_TIPO_INGRESO);
            diccionarioEntrada[338] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_SEGMENTO);
            diccionarioEntrada[339] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MARCA_NO_PRECALIFICAR, "0");
            diccionarioEntrada[340] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_SITUACION_BM, "0");
            diccionarioEntrada[341] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_SIT_CART_CEDIDA, "0");
            diccionarioEntrada[342] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_MORA_HISTORICA, "0");
            diccionarioEntrada[343] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_REFINANCIACION, "0");
            diccionarioEntrada[344] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_REESTRUCTURACION, "0");
            diccionarioEntrada[345] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_DIFERIMIENTO, "0");
            diccionarioEntrada[346] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_NO_ACREDITO_ULT_MES, "0");
            diccionarioEntrada[347] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_NO_ACREDITO_ULT_2M, "0");
            diccionarioEntrada[348] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_INGRESO_CALCULADO_FINAL, decimal.Zero.ToString());
            diccionarioEntrada[349] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_TIPO_INGRESO);
            diccionarioEntrada[350] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MAX_DIAS_ATRASO_PRESTAMOS, "0");
            diccionarioEntrada[351] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[352] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MOTIVO_REFINANCIACION);
            diccionarioEntrada[353] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MOTIVO_DEU);
            diccionarioEntrada[354] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_DIAS_ATRASO_TC, "0");
            diccionarioEntrada[355] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MAX_DIAS_DESC_SIN_ACUERDO, "0");
            diccionarioEntrada[356] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MAX_DIAS_CLEAN_UP, "0");
            diccionarioEntrada[357] = Herramientas.ConvertirATexto(Entrada.EA_COT_1_MAX_DIAS_EXCESO_ACUERDO, "0");
            diccionarioEntrada[358] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MAX_DIAS_ATRASO_PRESTAMOS, "0");
            diccionarioEntrada[359] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[360] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MOTIVO_REFINANCIACION);
            diccionarioEntrada[361] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MOTIVO_DEU);
            diccionarioEntrada[362] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_DIAS_ATRASO_TC, "0");
            diccionarioEntrada[363] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MAX_DIAS_DESC_SIN_ACUERDO, "0");
            diccionarioEntrada[364] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MAX_DIAS_CLEAN_UP, "0");
            diccionarioEntrada[365] = Herramientas.ConvertirATexto(Entrada.EA_COT_2_MAX_DIAS_EXCESO_ACUERDO, "0");
            diccionarioEntrada[366] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MAX_DIAS_ATRASO_PRESTAMOS, "0");
            diccionarioEntrada[367] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MOTIVO_REESTRUCTURACION);
            diccionarioEntrada[368] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MOTIVO_REFINANCIACION);
            diccionarioEntrada[369] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MOTIVO_DEU);
            diccionarioEntrada[370] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_DIAS_ATRASO_TC, "0");
            diccionarioEntrada[371] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MAX_DIAS_DESC_SIN_ACUERDO, "0");
            diccionarioEntrada[372] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MAX_DIAS_CLEAN_UP, "0");
            diccionarioEntrada[373] = Herramientas.ConvertirATexto(Entrada.EA_COT_3_MAX_DIAS_EXCESO_ACUERDO, "0");
            diccionarioEntrada[374] = Herramientas.ConvertirATexto(Entrada.BCRADEU_COT_1_Peor_sit_vigente, "0");
            diccionarioEntrada[375] = Herramientas.ConvertirATexto(Entrada.BCRADEU_COT_1_Peor_sit_ult_3m, "0");
            diccionarioEntrada[376] = Herramientas.ConvertirATexto(Entrada.BCRADEU_COT_2_Peor_sit_vigente, "0");
            diccionarioEntrada[377] = Herramientas.ConvertirATexto(Entrada.BCRADEU_COT_2_Peor_sit_ult_3m, "0");
            diccionarioEntrada[378] = Herramientas.ConvertirATexto(Entrada.BCRADEU_COT_3_Peor_sit_vigente, "0");
            diccionarioEntrada[379] = Herramientas.ConvertirATexto(Entrada.BCRADEU_COT_3_Peor_sit_ult_3m, "0");
            diccionarioEntrada[380] = Herramientas.ConvertirATexto(Entrada.LN_COT_1_Cant, "0");
            diccionarioEntrada[381] = Herramientas.ConvertirATexto(Entrada.LN_COT_1_Motivo);
            diccionarioEntrada[382] = Herramientas.ConvertirATexto(Entrada.LN_COT_2_Cant, "0");
            diccionarioEntrada[383] = Herramientas.ConvertirATexto(Entrada.LN_COT_2_Motivo);
            diccionarioEntrada[384] = Herramientas.ConvertirATexto(Entrada.LN_COT_3_Cant, "0");
            diccionarioEntrada[385] = Herramientas.ConvertirATexto(Entrada.LN_COT_3_Motivo);
            diccionarioEntrada[386] = Herramientas.ConvertirATexto(Entrada.VA_COT_1_ES_AUTONOMO);
            diccionarioEntrada[387] = Herramientas.ConvertirATexto(Entrada.VA_COT_1_ES_EMPLEADO);
            diccionarioEntrada[388] = Herramientas.ConvertirATexto(Entrada.VA_COT_1_ES_MONOTRIBUTISTA);
            diccionarioEntrada[389] = Herramientas.ConvertirATexto(Entrada.VA_COT_2_ES_AUTONOMO);
            diccionarioEntrada[390] = Herramientas.ConvertirATexto(Entrada.VA_COT_2_ES_EMPLEADO);
            diccionarioEntrada[391] = Herramientas.ConvertirATexto(Entrada.VA_COT_2_ES_MONOTRIBUTISTA);
            diccionarioEntrada[392] = Herramientas.ConvertirATexto(Entrada.VA_COT_3_ES_AUTONOMO);
            diccionarioEntrada[393] = Herramientas.ConvertirATexto(Entrada.VA_COT_3_ES_EMPLEADO);
            diccionarioEntrada[394] = Herramientas.ConvertirATexto(Entrada.VA_COT_3_ES_MONOTRIBUTISTA);
            diccionarioEntrada[395] = Herramientas.ConvertirATexto(Entrada.VA_SUM_SCORE, "0");
            diccionarioEntrada[396] = Herramientas.ConvertirATexto(Entrada.VA_COT_1_SUM_SCORE, "0");
            diccionarioEntrada[397] = Herramientas.ConvertirATexto(Entrada.VA_COT_2_SUM_SCORE, "0");
            diccionarioEntrada[398] = Herramientas.ConvertirATexto(Entrada.VA_COT_3_SUM_SCORE, "0");
            diccionarioEntrada[399] = Herramientas.ConvertirATexto(Entrada.tiene_intradiarias, "");
            diccionarioEntrada[400] = Herramientas.ConvertirATexto(Entrada.TIND_MOTIVO_CHEQ_RECH, "0");
            diccionarioEntrada[401] = Herramientas.ConvertirATexto(Entrada.TIND_COT_1_MOTIVO_CHEQ_RECH, "0");
            diccionarioEntrada[402] = Herramientas.ConvertirATexto(Entrada.TIND_COT_2_MOTIVO_CHEQ_RECH, "0");
            diccionarioEntrada[403] = Herramientas.ConvertirATexto(Entrada.TIND_COT_3_MOTIVO_CHEQ_RECH, "0");
            diccionarioEntrada[404] = Herramientas.ConvertirATexto(Entrada.CLI_FechaAlta, "19000101", "yyyyMMdd");
            diccionarioEntrada[405] = Herramientas.ConvertirATexto(Entrada.CLI_COT_1_FechaAlta, "19000101", "yyyyMMdd");
            diccionarioEntrada[406] = Herramientas.ConvertirATexto(Entrada.CLI_COT_2_FechaAlta, "19000101", "yyyyMMdd");
            diccionarioEntrada[407] = Herramientas.ConvertirATexto(Entrada.CLI_COT_3_FechaAlta, "19000101", "yyyyMMdd");
            diccionarioEntrada[408] = Herramientas.ConvertirATexto(Entrada.CCD_estado_convenio, "");
            diccionarioEntrada[409] = Herramientas.ConvertirATexto(Entrada.CCD_calificacion_permanente, "");
            diccionarioEntrada[410] = Herramientas.ConvertirATexto(Entrada.CCD_calificacion_otra_planta, "");
            diccionarioEntrada[diccionarioEntrada.Length - 1] = "FIN";
            return diccionarioEntrada;
        }

        public void Decodifica(string[] arraySalida)
        {
            _Salida = new PoliticaEvaluaIndividuosSalida();
            _Salida.Origen_ID = (int)Herramientas.ConvertirDeTexto(arraySalida[0], DbType.Int32, 0);
            _Salida.Numero_Oportunidad = (long)Herramientas.ConvertirDeTexto(arraySalida[1], DbType.Int64, 0);
            _Salida.Numero_Evaluacion = (int)Herramientas.ConvertirDeTexto(arraySalida[2], DbType.Int32, 0);
            _Salida.Status = (string)Herramientas.ConvertirDeTexto(arraySalida[3], DbType.String, "");
            _Salida.Mensaje = (string)Herramientas.ConvertirDeTexto(arraySalida[4], DbType.String, "");
            _Salida.Es_Cliente = (string)Herramientas.ConvertirDeTexto(arraySalida[5], DbType.String, "");
            _Salida.Cobis = (int)Herramientas.ConvertirDeTexto(arraySalida[6], DbType.Int32, 0);
            _Salida.Cuil = (long)Herramientas.ConvertirDeTexto(arraySalida[7], DbType.Int64, 0);
            _Salida.Rama = (string)Herramientas.ConvertirDeTexto(arraySalida[8], DbType.String, "");
            _Salida.Dictamen = (string)Herramientas.ConvertirDeTexto(arraySalida[9], DbType.String, "");
            _Salida.Codigos_Rechazo = (string)Herramientas.ConvertirDeTexto(arraySalida[10], DbType.String, "");
            _Salida.Explicacion = (string)Herramientas.ConvertirDeTexto(arraySalida[11], DbType.String, "");
            _Salida.Prestamo_Monto_Maximo = (decimal)Herramientas.ConvertirDeTexto(arraySalida[12], DbType.Decimal, decimal.Zero);
            _Salida.Prestamo_Cuota_Maxima = (decimal)Herramientas.ConvertirDeTexto(arraySalida[13], DbType.Decimal, decimal.Zero);
            _Salida.Prestamo_Plazo_Maximo = (int)Herramientas.ConvertirDeTexto(arraySalida[14], DbType.Int32, 0);
            _Salida.Prestamo_Tipo_Tasa = (string)Herramientas.ConvertirDeTexto(arraySalida[15], DbType.String, "");
            _Salida.Prestamo_Tna = (decimal)Herramientas.ConvertirDeTexto(arraySalida[16], DbType.Decimal, decimal.Zero);
            _Salida.Tarjeta_Credito_Limite = (decimal)Herramientas.ConvertirDeTexto(arraySalida[17], DbType.Decimal, decimal.Zero);
            _Salida.Cuenta_Corriente_Limite = (decimal)Herramientas.ConvertirDeTexto(arraySalida[18], DbType.Decimal, decimal.Zero);
            _Salida.Reporte_Politica = (string)Herramientas.ConvertirDeTexto(arraySalida[19], DbType.String, "");
            _Salida.Output_Adicional = (string)Herramientas.ConvertirDeTexto(arraySalida[20], DbType.String, "");
            _Salida.Nivel_de_Riesgo = (string)Herramientas.ConvertirDeTexto(arraySalida[21], DbType.String, "");
            _Salida.Ingreso_Computable = (decimal)Herramientas.ConvertirDeTexto(arraySalida[22], DbType.Decimal, decimal.Zero);
                _Salida.Version_Orquestador = (string)Herramientas.ConvertirDeTexto(arraySalida[23], DbType.String, "");
                _Salida.Tipo_Prestamo = (string)Herramientas.ConvertirDeTexto(arraySalida[24], DbType.String, "");
        }

        public void InsertaDictamenes(IDictaminador dictamenes)
        {
            var cab = MapeaCabecera();
            var id = dictamenes.InsertRow(cab);
            var xper = MapeaXPER();
            foreach (var i in xper) i.ID_Cabecera = id;
            dictamenes.InsertRows(xper);
        }

        private DictamenesIndividuos MapeaCabecera()
        {
            var registro = new DictamenesIndividuos();
            registro.Origen_ID = Entrada.Origen_ID;
            registro.Numero_Oportunidad = Entrada.Numero_Oportunidad;
            registro.Numero_Evaluacion = Entrada.Numero_Evaluacion;
            registro.Fecha_Hora = DateTime.Now;
            registro.Comercio_ID = Entrada.Comercio_ID;
            registro.Comercio_Sucursal = (int?)Herramientas.ConvertirDeTexto(Entrada.Comercio_Sucursal ?? "0", DbType.Int32);
            registro.Comercio_Usuario = Entrada.Comercio_Usuario;
            registro.Provincia = Entrada.Provincia;
            registro.Sucursal_Macro_Asociada = Entrada.Sucursal_Macro_Asociada;
            registro.Canal = Entrada.Canal ?? string.Empty;
            registro.Productos_Precancelados = Entrada.Productos_Precancelados ?? string.Empty;
            registro.Preca_Utiliza = Entrada.Preca_Utiliza ?? string.Empty;
            registro.Preca_Codigo = Entrada.Preca_Codigo ?? string.Empty;
            registro.Preca_Oferta = Entrada.Preca_Oferta ?? string.Empty;
            registro.Circuito = Entrada.Circuito ?? string.Empty;
            registro.Integrantes = 1 + ((Entrada.COT_1_Cuil > 0) ? 1 : 0) + ((Entrada.COT_2_Cuil > 0) ? 1 : 0) + ((Entrada.COT_3_Cuil > 0) ? 1 : 0);
            registro.Vivienda = Entrada.TIT_Vivienda ?? string.Empty;
            registro.Alquiler_Monto = Entrada.TIT_Alquiler_Monto;
            registro.Categoria_Cliente = Entrada.TIT_Categoria_Cliente ?? "";
            registro.Numero_Convenio = Entrada.TIT_Numero_Convenio?.ToString() ?? "";
            registro.Nombre_Convenio = Entrada.TIT_Nombre_Convenio ?? "";
            registro.Calificacion = Entrada.TIT_Calificacion ?? "";
            registro.Submercado = Entrada.TIT_Submercado ?? "";
            registro.Monto_Inversor = Entrada.TIT_Monto_Inversor;
            registro.Tipo_Contribuyente = Entrada.TIT_Tipo_Contribuyente ?? "Consumidor Final";
            registro.Tarjeta_Solicita = Entrada.Solicita_Tarjeta;
            registro.Tarjeta_Limite = Entrada.Tarjeta_Limite;
            registro.Prestamo_Solicita = Entrada.Solicita_Prestamo;
            registro.Prestamo_Tipo = Salida.Tipo_Prestamo;
            registro.Prestamo_Monto = Entrada.Prestamo_Monto;
            registro.Prestamo_Destino = Entrada.Prestamo_Destino ?? "";
            registro.Prestamo_Plazo = Entrada.Prestamo_Plazo;
            registro.Prestamo_Tipo_Tasa = Entrada.Prestamo_Tipo_Tasa ?? "";
            registro.Prestamo_TNA = Entrada.Prestamo_TNA;
            registro.Con_Garantia_Valor_Bien = Entrada.Con_Garantia_Valor_Bien;
            registro.Con_Garantia_Subsidio = Entrada.Con_Garantia_Subsidio;
            registro.Con_Garantia_Tasacion = Entrada.Con_Garantia_Tasacion;
            registro.Hipotecario_Valor_Terreno = Entrada.Hipotecario_Valor_Terreno;
            registro.Modelo_Automovil = Entrada.Modelo_Automovil;
            registro.Acuerdo_Solicita = Entrada.Solicita_Acuerdo;
            registro.Acuerdo_Monto = Entrada.Acuerdo_Monto;
            registro.Input_Adicional = Entrada.Extra ?? "";
            registro.SAL_Status = Salida.Status;
            registro.SAL_Mensaje = Salida.Mensaje;
            registro.SAL_Tipo_Cliente = Salida.Es_Cliente;
            registro.SAL_Rama = Salida.Rama;
            registro.SAL_Dictamen = Salida.Dictamen;
            registro.SAL_Codigos_Rechazo = Salida.Codigos_Rechazo;
            registro.SAL_Explicacion = Salida.Explicacion;
            registro.SAL_Prestamo_Monto_Maximo = Salida.Prestamo_Monto_Maximo;
            registro.SAL_Prestamo_Cuota_Maxima = Salida.Prestamo_Cuota_Maxima;
            registro.SAL_Prestamo_Plazo_Maximo = Salida.Prestamo_Plazo_Maximo;
            registro.SAL_Prestamo_TNA = Salida.Prestamo_Tna;
            registro.SAL_Prestamo_Tipo_Tasa = Salida.Prestamo_Tipo_Tasa;
            registro.SAL_Tarjeta_Credito_Limite = Salida.Tarjeta_Credito_Limite;
            registro.SAL_Cuenta_Corriente_Limite = Salida.Cuenta_Corriente_Limite;
            registro.SAL_Output_Adicional = Salida.Output_Adicional;
            registro.BP_Categoria = Entrada.CLI_categoria_source;
            registro.BP_Clase = Entrada.CLI_clase;
            registro.Preca_Nombre = (Salida.Cobis > 0 && ((Entrada.Solicita_Acuerdo == "SI" && Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE > 0) || (Entrada.Solicita_Prestamo == "SI" && Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO > 0) || (Entrada.Solicita_Tarjeta == "SI" && Entrada.PRECACOM_TARJETA_CREDITO_LIMITE > 0))) ? "PRECACOM" : "";
            registro.Preca_Consultar = Entrada.PM_PrecaAConsultar;
            registro.COD_TC = "028-TARJCRED";
            registro.COD_CC = "003-ACUERDOS";
            registro.COD_PP = "007-PERSTECO";
            registro.CAL_Version =  Entrada.CC_TIPO_PRECALIFICACION?.Split("|").ElementAtOrDefault(7) ?? "";
            registro.CAL_Codigo_Campana = Entrada.CC_CODIGO_CAMPANA ?? "";
            registro.CAL_Nombre_Campana = Entrada.CC_CODIGO_CAMPANA + "_" + Entrada.CC_ID_CAMPANA;
            registro.CAL_Tipo_Campana = "";
            registro.CAL_Descripcion = Entrada.CC_TIPO_PRECALIFICACION ?? "";
            registro.CAL_Fecha_Inicio = Entrada.CC_FECHA_CALIFICACION ?? new DateTime(1900,1,1);
            registro.CAL_Fecha_Fin = Entrada.CC_FECHA_VENCIMIENTO ?? new DateTime(1900, 1, 1);
            registro.CAL_Segmento = Entrada.CC_SEGMENTO_CRM ?? "";
            registro.CAL_Subsegmento = Entrada.CC_SUBSEGMENTO_CRM ?? "";
            registro.CAL_Sucursal_Asignada_Venta = Entrada.CC_SUCURSAL_MIEMBRO?.ToString() ?? "";
            registro.CAL_Canal = Entrada.CC_CANAL ?? "";
            registro.CAL_Objetivo = "";
            registro.CAL_Convenios_Beneficios = Entrada.CC_CONVENIO ?? "";
            registro.CAL_TC_Producto = Entrada.CC_PRODUCTO_TARJETA ?? "";
            registro.CAL_TC_Tipo = "";
            registro.CAL_TC_Monto_Maximo = Entrada.CC_TARJETA_MONTO;
            registro.CAL_TC_Fecha_Inicio = Entrada.CC_FECHA_CALIFICACION ?? new DateTime(1900, 1, 1);
            registro.CAL_TC_FechaVencimiento = Entrada.CC_FECHA_VENCIMIENTO ?? new DateTime(1900, 1, 1);
            registro.CAL_P_Producto = Entrada.CC_PRODUCTO_PRESTAMO ?? "";
            registro.CAL_P_Tipo = "";
            registro.CAL_P_Monto_Maximo = Entrada.CC_PRESTAMO_MONTO;
            registro.CAL_P_Tasa = Entrada.CC_PRESTAMO_TASA ?? decimal.Zero;
            registro.CAL_P_Plazo = Entrada.CC_PRESTAMO_PLAZO;
            registro.CAL_P_Afectacion = Entrada.CC_PRESTAMO_AFECTACION;
            registro.CAL_P_Cuota_Inicial = Entrada.CC_PRESTAMO_CUOTA_INICIAL;
            registro.CAL_P_CuotaPromedio = Entrada.CC_PRESTAMO_CUOTA_PROMEDIO;
            registro.CAL_P_Fecha_Inicio = Entrada.CC_FECHA_CALIFICACION ?? new DateTime(1900, 1, 1);
            registro.CAL_P_Fecha_Vencimiento = Entrada.CC_FECHA_VENCIMIENTO ?? new DateTime(1900, 1, 1);
            registro.CAL_AC_Producto = Entrada.CC_PRODUCTO_ACUERDO ?? "";
            registro.CAL_AC_Tipo = "";
            registro.CAL_AC_Monto_Maximo = Entrada.CC_ACUERDO_MONTO;
            registro.CAL_AC_Fecha_Inicio = Entrada.CC_FECHA_CALIFICACION ?? new DateTime(1900, 1, 1);
            registro.CAL_AC_Fecha_Vencimiento = Entrada.CC_FECHA_VENCIMIENTO ?? new DateTime(1900, 1, 1);
            registro.PRECACOM_Version = Entrada.PRECACOM_CODIGO ?? "";
            registro.PRECACOM_Cuil = Entrada.PRECACOM_CUIL;
            registro.PRECACOM_Origen_ID = (int?)Entrada.PRECACOM_ORIGEN_ID;
            registro.PRECACOM_Comercio_ID = Entrada.PRECACOM_COMERCIO_ID;
            registro.PRECACOM_ID_Alternativo = Entrada.PRECACOM_ID_ALTERNATIVO?.ToString() ?? "";
            registro.PRECACOM_P_Monto_Maximo = Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO;
            registro.PRECACOM_P_Plazo_Maximo = (int?)Entrada.PRECACOM_PRESTAMO_PLAZO_MAXIMO;
            registro.PRECACOM_P_TNA = Entrada.PRECACOM_PRESTAMO_TNA;
            registro.PRECACOM_P_Tipo_Tasa = Entrada.PRECACOM_PRESTAMO_TIPO_TASA ?? "";
            registro.PRECACOM_P_Cuota_Maxima = Entrada.PRECACOM_PRESTAMO_CUOTA_MAXIMO;
            registro.PRECACOM_TC_Limite = Entrada.PRECACOM_TARJETA_CREDITO_LIMITE;
            registro.PRECACOM_CC_Limite = Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE;
            registro.PRECACOM_Vigente_Desde = Entrada.PRECACOM_VIGENTE_DESDE ?? new DateTime(1900,1,1);
            registro.PRECACOM_Vigente_Hasta = Entrada.PRECACOM_VIGENTE_HASTA ?? new DateTime(1900, 1, 1);
            registro.PRECACOM_Vigencia = Entrada.PRECACOM_VIGENCIA;
            registro.PRECACOM_Adicional1 = Entrada.PRECACOM_COM1 ?? "";
            registro.PRECACOM_Adicional2 = Entrada.PRECACOM_COM2 ?? "";
            registro.PRECACOM_Adicional3 = Entrada.PRECACOM_COM3 ?? "";
            registro.Info_Liquidacion = "";
            registro.Dias_Atraso = 0;
            registro.Duracion_Servicios = "";
            registro.Version = Salida.Version_Orquestador;
            registro.CC_ID_OFERTAS = Entrada.CC_ID_OFERTAS ?? "";
            registro.ID_Interno = Entrada.ID_Transaccion;
            registro.ENT_NUC_Numero = (Entrada.NUC_Numero == "") ? null : Entrada.NUC_Numero?.Substring(0, Math.Min(50, Entrada.NUC_Numero.Length));
            registro.ENT_NUC_Tipo = Entrada.NUC_Tipo.Substring(0, Math.Min(50, Entrada.NUC_Tipo.Length));
            registro.ENT_NUC_Medio = Entrada.NUC_Medio.Substring(0, Math.Min(50, Entrada.NUC_Medio.Length));
            registro.ENT_NUC_Adicional = Entrada.NUC_Adicional.Substring(0, Math.Min(50, Entrada.NUC_Adicional.Length));
            return registro;
        }

        private DictamenesIndividuosXper[] MapeaXPER()
        {
            var integrantes = 1+((Entrada.COT_1_Cuil > 0) ? 1 : 0) + ((Entrada.COT_2_Cuil > 0) ? 1 : 0) + ((Entrada.COT_3_Cuil > 0) ? 1 : 0);
            var registros = new DictamenesIndividuosXper[integrantes];
            int intAux;
            DictamenesIndividuosXper registro = new();
            //registro.Id = 0;
            //registro.IdCabecera = 0;
            registro.Origen_ID = Entrada.Origen_ID;
            registro.Numero_Oportunidad = Entrada.Numero_Oportunidad;
            registro.Numero_Evaluacion = Entrada.Numero_Evaluacion;
            registro.Fecha_Hora = DateTime.Now;
            registro.Integrante = 1;
            registro.Cobis = int.TryParse(Entrada.CLI_codigo_cliente, out intAux) ? intAux : null;
            registro.Cuil = Entrada.TIT_Cuil;
            registro.Numero_Documento = Entrada.TIT_Numero_Documento ?? int.Parse(Entrada.TIT_Cuil.Value.ToString().Substring(2,8));
            registro.Tipo_Documento = Entrada.TIT_Tipo_Documento ?? string.Empty;
            registro.Version_Documento = Entrada.TIT_Version_Documento ?? string.Empty;
            registro.Sexo = Entrada.TIT_Sexo ?? string.Empty;
            registro.Apellido = Entrada.TIT_Apellido ?? string.Empty;
            registro.Nombre = Entrada.TIT_Nombre ?? string.Empty;
            registro.Nacimiento = Entrada.TIT_Nacimiento ?? new DateTime(1900,1,1);
            registro.Estado_Civil = Entrada.TIT_Estado_Civil ?? string.Empty;
            registro.Segmento = Entrada.TIT_Segmento ?? string.Empty;
            registro.Subsegmento = Entrada.TIT_Subsegmento ?? string.Empty;
            registro.Actividad_1 = Entrada.TIT_Actividad_1 ?? string.Empty;
            registro.Actividad_1_Fecha_Inicio = Entrada.TIT_Actividad_1_Fecha_Inicio ?? new DateTime(1900,1,1);
            registro.Actividad_1_Neto_Fijo = Entrada.TIT_Actividad_1_Neto_Fijo ?? decimal.Zero;
            registro.Actividad_1_Neto_Variable = Entrada.TIT_Actividad_1_Neto_Variable ?? decimal.Zero;
            registro.Actividad_1_Categoria_Monotributo = Entrada.TIT_Actividad_1_Categoria_Monotributo ?? string.Empty;
            registro.Actividad_1_Actividad_Monotributo = Entrada.TIT_Actividad_1_Actividad_Monotributo ?? string.Empty;
            registro.Actividad_2 = Entrada.TIT_Actividad_2 ?? string.Empty;
            registro.Actividad_2_Fecha_Inicio = Entrada.TIT_Actividad_2_Fecha_Inicio ?? new DateTime(1900, 1, 1);
            registro.Actividad_2_Neto_Fijo = Entrada.TIT_Actividad_2_Neto_Fijo ?? decimal.Zero;
            registro.Actividad_2_Neto_Variable = Entrada.TIT_Actividad_2_Neto_Variable ?? decimal.Zero;
            registro.Actividad_2_Categoria_Monotributo = Entrada.TIT_Actividad_2_Categoria_Monotributo ?? string.Empty;
            registro.Actividad_2_Actividad_Monotributo = Entrada.TIT_Actividad_2_Actividad_Monotributo ?? string.Empty;
            registro.ID_Interno = Entrada.ID_Transaccion;
            registros[0] = registro;

            if (integrantes > 1)
            {
                registro = new();
                //registro.Id = 0;
                //registro.IdCabecera = 0;
                registro.Origen_ID = Entrada.Origen_ID;
                registro.Numero_Oportunidad = Entrada.Numero_Oportunidad;
                registro.Numero_Evaluacion = Entrada.Numero_Evaluacion;
                registro.Fecha_Hora = DateTime.Now;
                registro.Integrante = 2;
                registro.Cobis = int.TryParse(Entrada.CLI_COT_1_codigo_cliente, out intAux) ? intAux : null;
                registro.Cuil = Entrada.COT_1_Cuil;
                registro.Numero_Documento = Entrada.COT_1_Numero_Documento;
                registro.Tipo_Documento = Entrada.COT_1_Tipo_Documento;
                registro.Version_Documento = Entrada.COT_1_Version_Documento;
                registro.Sexo = Entrada.COT_1_Sexo;
                registro.Apellido = Entrada.COT_1_Apellido;
                registro.Nombre = Entrada.COT_1_Nombre;
                registro.Nacimiento = Entrada.COT_1_Nacimiento;
                registro.Estado_Civil = Entrada.COT_1_Estado_Civil;
                registro.Segmento = Entrada.COT_1_Segmento;
                registro.Subsegmento = Entrada.COT_1_Subsegmento;
                registro.Actividad_1 = Entrada.COT_1_Actividad_1;
                registro.Actividad_1_Fecha_Inicio = Entrada.COT_1_Actividad_1_Fecha ?? new DateTime(1900, 1, 1);
                registro.Actividad_1_Neto_Fijo = Entrada.COT_1_Actividad_1_Neto_Fijo;
                registro.Actividad_1_Neto_Variable = Entrada.COT_1_Actividad_1_Neto_Variable;
                registro.Actividad_1_Categoria_Monotributo = Entrada.COT_1_Actividad_1_Categoria_Monotributo;
                registro.Actividad_1_Actividad_Monotributo = Entrada.COT_1_Actividad_1_Actividad_Monotributo;
                registro.Actividad_2 = Entrada.COT_1_Actividad_2;
                registro.Actividad_2_Fecha_Inicio = Entrada.COT_1_Actividad_2_Fecha ?? new DateTime(1900, 1, 1);
                registro.Actividad_2_Neto_Fijo = Entrada.COT_1_Actividad_2_Neto_Fijo;
                registro.Actividad_2_Neto_Variable = Entrada.COT_1_Actividad_2_Neto_Variable;
                registro.Actividad_2_Categoria_Monotributo = Entrada.COT_1_Actividad_2_Categoria_Monotributo;
                registro.Actividad_2_Actividad_Monotributo = Entrada.COT_1_Actividad_2_Actividad_Monotributo;
                registro.ID_Interno = Entrada.ID_Transaccion;
                registros[1] = registro;
            }

            if (integrantes > 2)
            {
                registro = new();
                //registro.Id = 0;
                //registro.IdCabecera = 0;
                registro.Origen_ID = Entrada.Origen_ID;
                registro.Numero_Oportunidad = Entrada.Numero_Oportunidad;
                registro.Numero_Evaluacion = Entrada.Numero_Evaluacion;
                registro.Fecha_Hora = DateTime.Now;
                registro.Integrante = 3;
                registro.Cobis = int.TryParse(Entrada.CLI_COT_2_codigo_cliente, out intAux) ? intAux : null;
                registro.Cuil = Entrada.COT_2_Cuil;
                registro.Numero_Documento = Entrada.COT_2_Numero_Documento;
                registro.Tipo_Documento = Entrada.COT_2_Tipo_Documento;
                registro.Version_Documento = Entrada.COT_2_Version_Documento;
                registro.Sexo = Entrada.COT_2_Sexo;
                registro.Apellido = Entrada.COT_2_Apellido;
                registro.Nombre = Entrada.COT_2_Nombre;
                registro.Nacimiento = Entrada.COT_2_Nacimiento;
                registro.Estado_Civil = Entrada.COT_2_Estado_Civil;
                registro.Segmento = Entrada.COT_2_Segmento;
                registro.Subsegmento = Entrada.COT_2_Subsegmento;
                registro.Actividad_1 = Entrada.COT_2_Actividad_1;
                registro.Actividad_1_Fecha_Inicio = Entrada.COT_2_Actividad_1_Fecha ?? new DateTime(1900, 1, 1);
                registro.Actividad_1_Neto_Fijo = Entrada.COT_2_Actividad_1_Neto_Fijo;
                registro.Actividad_1_Neto_Variable = Entrada.COT_2_Actividad_1_Neto_Variable;
                registro.Actividad_1_Categoria_Monotributo = Entrada.COT_2_Actividad_1_Categoria_Monotributo;
                registro.Actividad_1_Actividad_Monotributo = Entrada.COT_2_Actividad_1_Actividad_Monotributo;
                registro.Actividad_2 = Entrada.COT_2_Actividad_2;
                registro.Actividad_2_Fecha_Inicio = Entrada.COT_2_Actividad_2_Fecha ?? new DateTime(1900, 1, 1);
                registro.Actividad_2_Neto_Fijo = Entrada.COT_2_Actividad_2_Neto_Fijo;
                registro.Actividad_2_Neto_Variable = Entrada.COT_2_Actividad_2_Neto_Variable;
                registro.Actividad_2_Categoria_Monotributo = Entrada.COT_2_Actividad_2_Categoria_Monotributo;
                registro.Actividad_2_Actividad_Monotributo = Entrada.COT_2_Actividad_2_Actividad_Monotributo;
                registro.ID_Interno = Entrada.ID_Transaccion;
                registros[2] = registro;
            }

            if (integrantes > 3)
            {
                registro = new();
                //registro.Id = 0;
                //registro.IdCabecera = 0;
                registro.Origen_ID = Entrada.Origen_ID;
                registro.Numero_Oportunidad = Entrada.Numero_Oportunidad;
                registro.Numero_Evaluacion = Entrada.Numero_Evaluacion;
                registro.Fecha_Hora = DateTime.Today;
                registro.Integrante = 4;
                registro.Cobis = int.TryParse(Entrada.CLI_COT_3_codigo_cliente, out intAux) ? intAux : null;
                registro.Cuil = Entrada.COT_3_Cuil;
                registro.Numero_Documento = Entrada.COT_3_Numero_Documento;
                registro.Tipo_Documento = Entrada.COT_3_Tipo_Documento;
                registro.Version_Documento = Entrada.COT_3_Version_Documento;
                registro.Sexo = Entrada.COT_3_Sexo;
                registro.Apellido = Entrada.COT_3_Apellido;
                registro.Nombre = Entrada.COT_3_Nombre;
                registro.Nacimiento = Entrada.COT_3_Nacimiento;
                registro.Estado_Civil = Entrada.COT_3_Estado_Civil;
                registro.Segmento = Entrada.COT_3_Segmento;
                registro.Subsegmento = Entrada.COT_3_Subsegmento;
                registro.Actividad_1 = Entrada.COT_3_Actividad_1;
                registro.Actividad_1_Fecha_Inicio = Entrada.COT_3_Actividad_1_Fecha ?? new DateTime(1900, 1, 1);
                registro.Actividad_1_Neto_Fijo = Entrada.COT_3_Actividad_1_Neto_Fijo;
                registro.Actividad_1_Neto_Variable = Entrada.COT_3_Actividad_1_Neto_Variable;
                registro.Actividad_1_Categoria_Monotributo = Entrada.COT_3_Actividad_1_Categoria_Monotributo;
                registro.Actividad_1_Actividad_Monotributo = Entrada.COT_3_Actividad_1_Actividad_Monotributo;
                registro.Actividad_2 = Entrada.COT_3_Actividad_2;
                registro.Actividad_2_Fecha_Inicio = Entrada.COT_3_Actividad_2_Fecha ?? new DateTime(1900, 1, 1);
                registro.Actividad_2_Neto_Fijo = Entrada.COT_3_Actividad_2_Neto_Fijo;
                registro.Actividad_2_Neto_Variable = Entrada.COT_3_Actividad_2_Neto_Variable;
                registro.Actividad_2_Categoria_Monotributo = Entrada.COT_3_Actividad_2_Categoria_Monotributo;
                registro.Actividad_2_Actividad_Monotributo = Entrada.COT_3_Actividad_2_Actividad_Monotributo;
                registro.ID_Interno = Entrada.ID_Transaccion;
                registros[3] = registro;
            }

            return registros;
        }

        public void GarantizaValores()
        {
            Entrada.TIT_Nacimiento ??= new DateTime(1900, 1, 1);
            Entrada.TIT_Actividad_1_Fecha_Inicio ??= new DateTime(1900, 1, 1);
            Entrada.TIT_Actividad_1_Neto_Fijo ??= Decimal.Zero;
            Entrada.TIT_Actividad_1_Neto_Variable ??= Decimal.Zero;
            Entrada.TIT_Actividad_2_Fecha_Inicio ??= new DateTime(1900, 1, 1);
            Entrada.TIT_Actividad_2_Neto_Fijo ??= Decimal.Zero;
            Entrada.TIT_Actividad_2_Neto_Variable ??= Decimal.Zero;
            Entrada.TIT_Alquiler_Monto ??= Decimal.Zero;
            Entrada.TIT_Monto_Inversor ??= Decimal.Zero;
            Entrada.SUM_cuota_actual_PP ??= Decimal.Zero;
            Entrada.SUM_monto_deuda_actual_PP ??= Decimal.Zero;
            Entrada.SUM_cuota_actual_PR ??= Decimal.Zero;
            Entrada.SUM_monto_deuda_actual_PR ??= Decimal.Zero;
            Entrada.SUM_cuota_actual_PH ??= Decimal.Zero;
            Entrada.SUM_monto_deuda_actual_PH ??= Decimal.Zero;
            Entrada.SUM_cuota_actual_OP ??= Decimal.Zero;
            Entrada.SUM_monto_deuda_actual_OP ??= Decimal.Zero;
            Entrada.SUM_limites_TC_Macro ??= Decimal.Zero;
            Entrada.SUM_pago_min_prom_ult2_TC_Macro ??= Decimal.Zero;
            Entrada.SUM_acuerdos_cc_Macro ??= Decimal.Zero;
            Entrada.SUM_uso_prom_ult2_acuerdos_cc_Macro ??= Decimal.Zero;
            Entrada.Cuota_prest_sin_gtia_no_bureau ??= Decimal.Zero;
            Entrada.Cuota_prest_con_gtia_no_bureau ??= Decimal.Zero;
            Entrada.Pago_min_tarj_no_bureau ??= Decimal.Zero;
            Entrada.Util_cta_cte_no_bureau ??= Decimal.Zero;
            Entrada.Tarjeta_Limite ??= Decimal.Zero;
            Entrada.Prestamo_Monto ??= Decimal.Zero;
            Entrada.Prestamo_Plazo ??= 0;
            Entrada.Prestamo_TNA ??= Decimal.Zero;
            Entrada.Con_Garantia_Valor_Bien ??= Decimal.Zero;
            Entrada.Con_Garantia_Subsidio ??= Decimal.Zero;
            Entrada.Con_Garantia_Tasacion ??= Decimal.Zero;
            Entrada.Hipotecario_Valor_Terreno ??= Decimal.Zero;
            Entrada.Acuerdo_Monto ??= Decimal.Zero;
            Entrada.COT_1_Nacimiento ??= new DateTime(1900, 1, 1);
            Entrada.COT_1_Actividad_1_Fecha ??= new DateTime(1900, 1, 1);
            Entrada.COT_1_Actividad_1_Neto_Fijo ??= Decimal.Zero;
            Entrada.COT_1_Actividad_1_Neto_Variable ??= Decimal.Zero;
            Entrada.COT_1_Actividad_2_Fecha ??= new DateTime(1900, 1, 1);
            Entrada.COT_1_Actividad_2_Neto_Fijo ??= Decimal.Zero;
            Entrada.COT_1_Actividad_2_Neto_Variable ??= Decimal.Zero;
            Entrada.COT_2_Nacimiento ??= new DateTime(1900, 1, 1);
            Entrada.COT_2_Actividad_1_Fecha ??= new DateTime(1900, 1, 1);
            Entrada.COT_2_Actividad_1_Neto_Fijo ??= Decimal.Zero;
            Entrada.COT_2_Actividad_1_Neto_Variable ??= Decimal.Zero;
            Entrada.COT_2_Actividad_2_Fecha ??= new DateTime(1900, 1, 1);
            Entrada.COT_2_Actividad_2_Neto_Fijo ??= Decimal.Zero;
            Entrada.COT_2_Actividad_2_Neto_Variable ??= Decimal.Zero;
            Entrada.COT_3_Nacimiento ??= new DateTime(1900, 1, 1);
            Entrada.COT_3_Actividad_1_Fecha ??= new DateTime(1900, 1, 1);
            Entrada.COT_3_Actividad_1_Neto_Fijo ??= Decimal.Zero;
            Entrada.COT_3_Actividad_1_Neto_Variable ??= Decimal.Zero;
            Entrada.COT_3_Actividad_2_Fecha ??= new DateTime(1900, 1, 1);
            Entrada.COT_3_Actividad_2_Neto_Fijo ??= Decimal.Zero;
            Entrada.COT_3_Actividad_2_Neto_Variable ??= Decimal.Zero;
            Entrada.Seguro_Automovil ??= Decimal.Zero;
        }

        public void MapeaEntradaPolitica(FuentesDeInformacion fuentes, Integrantes integrantes)
        {
            var request = fuentes.RequestEvaluaIndividuos;
            var parametros = fuentes.Parametros;
            long numeroTributario;
            int cobis;

            MapeaRequest(fuentes.RequestEvaluaIndividuos);
            Entrada.ID_Transaccion = fuentes.Registro.Id.ToString();

            ClientesDepurados clientesDepurado;
            if (fuentes.ClientesDepurados.TryGetValue(integrantes?.Titular?.NumeroTributario.ToString() ?? string.Empty, out clientesDepurado))
            {
                Entrada.CND_Codigo_Campana = clientesDepurado.Depuracion ?? string.Empty;
            }

            Clientes cliente;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (fuentes.Clientes.TryGetValue(numeroTributario.ToString(), out cliente))
            {
                Entrada.CLI_nombre = cliente?.Nombre ?? string.Empty;
                Entrada.CLI_apellido = cliente?.Apellido ?? string.Empty;
                Entrada.CLI_codigo_cliente = cliente?.CodigoCliente ?? string.Empty;
                Entrada.CLI_nro_doc_tributario = cliente?.NroDocTributario ?? string.Empty;
                Entrada.CLI_fecha_nacimiento = cliente?.FechaNacimiento ?? new DateTime(1900, 1, 1);
                Entrada.CLI_categoria_source = cliente?.CategoriaSource ?? string.Empty;
                Entrada.CLI_clase = cliente?.Clase ?? string.Empty;
                Entrada.CLI_restringido = cliente?.Restringido ?? string.Empty;
                Entrada.CLI_marca_pep = cliente?.MarcaPep ?? string.Empty;
                Entrada.CLI_fallecido = cliente?.Fallecido ?? string.Empty;
                Entrada.CLI_banca = cliente?.Banca ?? string.Empty;
                Entrada.CLI_segmento_source = cliente?.SegmentoSource ?? string.Empty;
                Entrada.CLI_subsegmento_source = cliente?.SubsegmentoSource ?? string.Empty;
                Entrada.CLI_FechaAlta = DateOnly.FromDateTime(cliente?.FechaAlta ?? new DateTime(1900, 1, 1));
            }

            numeroTributario = integrantes.PorIndice(1)?.NumeroTributario ?? 0;
            if (fuentes.Clientes.TryGetValue(numeroTributario.ToString(), out cliente))
            {
                Entrada.CLI_COT_1_codigo_cliente = cliente?.CodigoCliente ?? string.Empty;
                Entrada.CLI_COT_1_fecha_nacimiento = cliente?.FechaNacimiento ?? new DateTime(1900, 1, 1);
                Entrada.CLI_COT_1_clase = cliente?.Clase ?? string.Empty;
                Entrada.CLI_COT_1_restringido = cliente?.Restringido ?? string.Empty;
                Entrada.CLI_COT_1_fallecido = cliente?.Fallecido ?? string.Empty;
                Entrada.CLI_COT_1_banca = cliente?.Banca ?? string.Empty;
                Entrada.CLI_COT_1_FechaAlta = DateOnly.FromDateTime(cliente?.FechaAlta ?? new DateTime(1900, 1, 1));
            }

            numeroTributario = integrantes.PorIndice(2)?.NumeroTributario ?? 0;
            if (fuentes.Clientes.TryGetValue(numeroTributario.ToString(), out cliente))
            {
                Entrada.CLI_COT_2_codigo_cliente = cliente?.CodigoCliente ?? string.Empty;
                Entrada.CLI_COT_2_fecha_nacimiento = cliente?.FechaNacimiento ?? new DateTime(1900, 1, 1);
                Entrada.CLI_COT_2_clase = cliente?.Clase ?? string.Empty;
                Entrada.CLI_COT_2_restringido = cliente?.Restringido ?? string.Empty;
                Entrada.CLI_COT_2_fallecido = cliente?.Fallecido ?? string.Empty;
                Entrada.CLI_COT_2_banca = cliente?.Banca ?? string.Empty;
                Entrada.CLI_COT_2_FechaAlta = DateOnly.FromDateTime(cliente?.FechaAlta ?? new DateTime(1900, 1, 1));
            }

            numeroTributario = integrantes.PorIndice(3)?.NumeroTributario ?? 0;
            if (fuentes.Clientes.TryGetValue(numeroTributario.ToString(), out cliente))
            {
                Entrada.CLI_COT_3_codigo_cliente = cliente?.CodigoCliente ?? string.Empty;
                Entrada.CLI_COT_3_fecha_nacimiento = cliente?.FechaNacimiento ?? new DateTime(1900, 1, 1);
                Entrada.CLI_COT_3_clase = cliente?.Clase ?? string.Empty;
                Entrada.CLI_COT_3_restringido = cliente?.Restringido ?? string.Empty;
                Entrada.CLI_COT_3_fallecido = cliente?.Fallecido ?? string.Empty;
                Entrada.CLI_COT_3_banca = cliente?.Banca ?? string.Empty;
                Entrada.CLI_COT_3_FechaAlta = DateOnly.FromDateTime(cliente?.FechaAlta ?? new DateTime(1900, 1, 1));
            }

            ValidacionActividad validacionActividad;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (fuentes.ValidacionActividad.TryGetValue(numeroTributario, out validacionActividad))
            {
                Entrada.VA_ES_AUTONOMO = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_ES_EMPLEADO = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_ES_MONOTRIBUTISTA = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_SUM_SCORE = validacionActividad.SumScore ?? Decimal.Zero;
            }

            numeroTributario = integrantes.PorIndice(1)?.NumeroTributario ?? 0;
            if (fuentes.ValidacionActividad.TryGetValue(numeroTributario, out validacionActividad))
            {
                Entrada.VA_COT_1_ES_AUTONOMO = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_COT_1_ES_EMPLEADO = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_COT_1_ES_MONOTRIBUTISTA = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_COT_1_SUM_SCORE = validacionActividad.SumScore ?? Decimal.Zero;
            }

            numeroTributario = integrantes.PorIndice(2)?.NumeroTributario ?? 0;
            if (fuentes.ValidacionActividad.TryGetValue(numeroTributario, out validacionActividad))
            {
                Entrada.VA_COT_2_ES_AUTONOMO = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_COT_2_ES_EMPLEADO = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_COT_2_ES_MONOTRIBUTISTA = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_COT_2_SUM_SCORE = validacionActividad.SumScore ?? Decimal.Zero;
            }

            numeroTributario = integrantes.PorIndice(3)?.NumeroTributario ?? 0;
            if (fuentes.ValidacionActividad.TryGetValue(numeroTributario, out validacionActividad))
            {
                Entrada.VA_COT_3_ES_AUTONOMO = validacionActividad.EsAutonomo ?? string.Empty;
                Entrada.VA_COT_3_ES_EMPLEADO = validacionActividad.EsEmpleado ?? string.Empty;
                Entrada.VA_COT_3_ES_MONOTRIBUTISTA = validacionActividad.EsMonotributista ?? string.Empty;
                Entrada.VA_COT_3_SUM_SCORE = validacionActividad.SumScore ?? Decimal.Zero;
            }

            LkpIrregularidadesBcra lkpIrregularidadesBcra;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (fuentes.LkpIrregularidadesBcra.TryGetValue(numeroTributario, out lkpIrregularidadesBcra))
            {
                Entrada.BCRADEU_situacion_entidad1 = lkpIrregularidadesBcra.BcraDeuSituacionEntidad1 ?? string.Empty;
                Entrada.BCRADEU_monto_entidad1 = lkpIrregularidadesBcra.BcraDeuMontoEntidad1 ?? Decimal.Zero;
                Entrada.BCRADEU_situacion_entidad2 = lkpIrregularidadesBcra.BcraDeuSituacionEntidad2 ?? string.Empty;
                Entrada.BCRADEU_monto_entidad2 = lkpIrregularidadesBcra.BcraDeuMontoEntidad2 ?? Decimal.Zero;
                Entrada.BCRADEU_situacion_entidad3 = lkpIrregularidadesBcra.BcraDeuSituacionEntidad3 ?? string.Empty;
                Entrada.BCRADEU_monto_entidad3 = lkpIrregularidadesBcra.BcraDeuMontoEntidad3 ?? Decimal.Zero;
                Entrada.BCRADEU_situacion_entidad4 = lkpIrregularidadesBcra.BcraDeuSituacionEntidad4 ?? string.Empty;
                Entrada.BCRADEU_monto_entidad4 = lkpIrregularidadesBcra.BcraDeuMontoEntidad4 ?? Decimal.Zero;
                Entrada.BCRADEU_situacion_entidad5 = lkpIrregularidadesBcra.BcraDeuSituacionEntidad5 ?? string.Empty;
                Entrada.BCRADEU_monto_entidad5 = lkpIrregularidadesBcra.BcraDeuMontoEntidad5 ?? Decimal.Zero;
                Entrada.BCRADEU_origen = lkpIrregularidadesBcra.BcraDeuOrigen ?? string.Empty;
                Entrada.BCRADEU_Cant_de_sit_ireg_ult_24m = lkpIrregularidadesBcra.BcraDeuCantDeSitIrregUlt24m ?? 0;
                Entrada.BCRADEU_Peor_sit_vigente = lkpIrregularidadesBcra.BcraDeuPeorSitVigente ?? 0;
                Entrada.BCRADEU_Peor_sit_ult_6m = lkpIrregularidadesBcra.BcraDeuPeorSitUlt6m ?? 0;
                Entrada.BCRADEU_Cant_de_sit_irreg_ult_periodo = lkpIrregularidadesBcra.BcraDeuCantDeSitIrregUltPeriodo ?? 0;
                Entrada.BCRADEU_Peor_sit_ult_3m = lkpIrregularidadesBcra.BcraDeuPeorSitUlt3m ?? 0;
                Entrada.BCRADEU_Cant_de_sit_2_ult_3m = lkpIrregularidadesBcra.BcraDeuCantDeSit2Ult3m ?? 0;
                Entrada.BCRADEU_Cant_de_sit_irreg_ult_3m = lkpIrregularidadesBcra.BcraDeuCantDeSitIrregUlt3m ?? 0;
            }

            numeroTributario = integrantes.PorIndice(1)?.NumeroTributario ?? 0;
            if (fuentes.LkpIrregularidadesBcra.TryGetValue(numeroTributario, out lkpIrregularidadesBcra))
            {
                Entrada.BCRADEU_COT_1_Peor_sit_vigente = lkpIrregularidadesBcra.BcraDeuPeorSitVigente ?? 0;
                Entrada.BCRADEU_COT_1_Peor_sit_ult_3m = lkpIrregularidadesBcra.BcraDeuPeorSitUlt3m ?? 0;
            }

            numeroTributario = integrantes.PorIndice(2)?.NumeroTributario ?? 0;
            if (fuentes.LkpIrregularidadesBcra.TryGetValue(numeroTributario, out lkpIrregularidadesBcra))
            {
                Entrada.BCRADEU_COT_2_Peor_sit_vigente = lkpIrregularidadesBcra.BcraDeuPeorSitVigente ?? 0;
                Entrada.BCRADEU_COT_2_Peor_sit_ult_3m = lkpIrregularidadesBcra.BcraDeuPeorSitUlt3m ?? 0;
            }

            numeroTributario = integrantes.PorIndice(3)?.NumeroTributario ?? 0;
            if (fuentes.LkpIrregularidadesBcra.TryGetValue(numeroTributario, out lkpIrregularidadesBcra))
            {
                Entrada.BCRADEU_COT_3_Peor_sit_vigente = lkpIrregularidadesBcra.BcraDeuPeorSitVigente ?? 0;
                Entrada.BCRADEU_COT_3_Peor_sit_ult_3m = lkpIrregularidadesBcra.BcraDeuPeorSitUlt3m ?? 0;
            }


            //Inicializacion EndeudamientoAtraso
            EndeudamientoAtraso endeudamientoAtraso;
            Entrada.EA_SALDO_CONTABLE_PP = decimal.Zero;
            Entrada.EA_MONTO_PROXIMA_CUOTA_PP = decimal.Zero;
            Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO = decimal.Zero;
            Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO = decimal.Zero;
            Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO = decimal.Zero;
            Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO = decimal.Zero;
            Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS = decimal.Zero;
            Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS = decimal.Zero;
            Entrada.EA_LIMITE_ACUERDO = decimal.Zero;
            Entrada.EA_UTILIZACION_ACUERDO = decimal.Zero;
            Entrada.EA_LIMITE_COMPRA_TC = decimal.Zero;
            Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES = decimal.Zero;

            cobis = integrantes.PorIndice(0)?.Cobis ?? 0;
            if (fuentes.EndeudamientoAtraso.TryGetValue(cobis, out endeudamientoAtraso))
            {
                Entrada.EA_CODIGO_CLIENTE = endeudamientoAtraso.CodigoCliente;
                Entrada.EA_MAX_DIAS_ATRASO_PRESTAMOS = endeudamientoAtraso.MaxDiasAtrasoPrestamos;
                Entrada.EA_MOTIVO_REESTRUCTURACION = endeudamientoAtraso.MotivoReestructuracion;
                Entrada.EA_MOTIVO_REFINANCIACION = endeudamientoAtraso.MotivoRefinanciacion;
                Entrada.EA_MOTIVO_DEU = endeudamientoAtraso.MotivoDeu;
                Entrada.EA_DIAS_ATRASO_TC = endeudamientoAtraso.DiasAtrasoTc;
                Entrada.EA_MAX_DIAS_DESC_SIN_ACUERDO = endeudamientoAtraso.MaxDiasDescSinAcuerdo;
                Entrada.EA_MAX_DIAS_CLEAN_UP = endeudamientoAtraso.MaxDiasCleanUp;
                Entrada.EA_MAX_DIAS_EXCESO_ACUERDO = endeudamientoAtraso.MaxDiasExcesoAcuerdo;
                //Sumarizados
                Entrada.EA_SALDO_CONTABLE_PP += endeudamientoAtraso.SaldoContablePp;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PP += endeudamientoAtraso.MontoProximaCuotaPp;
                Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO += endeudamientoAtraso.SaldoContablePrestPrendario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO += endeudamientoAtraso.MontoProximaCuotaPrestPrendario;
                Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO += endeudamientoAtraso.SaldoContablePrestHipotecario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO += endeudamientoAtraso.MontoProximaCuotaPrestHipotecario;
                Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS += endeudamientoAtraso.SaldoContableOtrosPrestamos;
                Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS += endeudamientoAtraso.MontoProximaCuotaOtrosPrestamos;
                Entrada.EA_LIMITE_ACUERDO += endeudamientoAtraso.LimiteAcuerdo;
                Entrada.EA_UTILIZACION_ACUERDO += endeudamientoAtraso.UtilizacionAcuerdo;
                Entrada.EA_LIMITE_COMPRA_TC += endeudamientoAtraso.LimiteCompraTc;
                Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES += endeudamientoAtraso.PagoMinimoPromUlt2Meses;
            }

            cobis = integrantes.PorIndice(1)?.Cobis ?? 0;
            if (fuentes.EndeudamientoAtraso.TryGetValue(cobis, out endeudamientoAtraso))
            {
                Entrada.EA_COT_1_MAX_DIAS_ATRASO_PRESTAMOS = endeudamientoAtraso.MaxDiasAtrasoPrestamos;
                Entrada.EA_COT_1_MOTIVO_REESTRUCTURACION = endeudamientoAtraso.MotivoReestructuracion;
                Entrada.EA_COT_1_MOTIVO_REFINANCIACION = endeudamientoAtraso.MotivoRefinanciacion;
                Entrada.EA_COT_1_MOTIVO_DEU = endeudamientoAtraso.MotivoDeu;
                Entrada.EA_COT_1_DIAS_ATRASO_TC = endeudamientoAtraso.DiasAtrasoTc;
                Entrada.EA_COT_1_MAX_DIAS_DESC_SIN_ACUERDO = endeudamientoAtraso.MaxDiasDescSinAcuerdo;
                Entrada.EA_COT_1_MAX_DIAS_CLEAN_UP = endeudamientoAtraso.MaxDiasCleanUp;
                Entrada.EA_COT_1_MAX_DIAS_EXCESO_ACUERDO = endeudamientoAtraso.MaxDiasExcesoAcuerdo;
                //Sumarizados
                Entrada.EA_SALDO_CONTABLE_PP += endeudamientoAtraso.SaldoContablePp;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PP += endeudamientoAtraso.MontoProximaCuotaPp;
                Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO += endeudamientoAtraso.SaldoContablePrestPrendario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO += endeudamientoAtraso.MontoProximaCuotaPrestPrendario;
                Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO += endeudamientoAtraso.SaldoContablePrestHipotecario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO += endeudamientoAtraso.MontoProximaCuotaPrestHipotecario;
                Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS += endeudamientoAtraso.SaldoContableOtrosPrestamos;
                Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS += endeudamientoAtraso.MontoProximaCuotaOtrosPrestamos;
                Entrada.EA_LIMITE_ACUERDO += endeudamientoAtraso.LimiteAcuerdo;
                Entrada.EA_UTILIZACION_ACUERDO += endeudamientoAtraso.UtilizacionAcuerdo;
                Entrada.EA_LIMITE_COMPRA_TC += endeudamientoAtraso.LimiteCompraTc;
                Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES += endeudamientoAtraso.PagoMinimoPromUlt2Meses;
            }

            cobis = integrantes.PorIndice(2)?.Cobis ?? 0;
            if (fuentes.EndeudamientoAtraso.TryGetValue(cobis, out endeudamientoAtraso))
            {
                Entrada.EA_COT_2_MAX_DIAS_ATRASO_PRESTAMOS = endeudamientoAtraso.MaxDiasAtrasoPrestamos;
                Entrada.EA_COT_2_MOTIVO_REESTRUCTURACION = endeudamientoAtraso.MotivoReestructuracion;
                Entrada.EA_COT_2_MOTIVO_REFINANCIACION = endeudamientoAtraso.MotivoRefinanciacion;
                Entrada.EA_COT_2_MOTIVO_DEU = endeudamientoAtraso.MotivoDeu;
                Entrada.EA_COT_2_DIAS_ATRASO_TC = endeudamientoAtraso.DiasAtrasoTc;
                Entrada.EA_COT_2_MAX_DIAS_DESC_SIN_ACUERDO = endeudamientoAtraso.MaxDiasDescSinAcuerdo;
                Entrada.EA_COT_2_MAX_DIAS_CLEAN_UP = endeudamientoAtraso.MaxDiasCleanUp;
                Entrada.EA_COT_2_MAX_DIAS_EXCESO_ACUERDO = endeudamientoAtraso.MaxDiasExcesoAcuerdo;
                //Sumarizados
                Entrada.EA_SALDO_CONTABLE_PP += endeudamientoAtraso.SaldoContablePp;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PP += endeudamientoAtraso.MontoProximaCuotaPp;
                Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO += endeudamientoAtraso.SaldoContablePrestPrendario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO += endeudamientoAtraso.MontoProximaCuotaPrestPrendario;
                Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO += endeudamientoAtraso.SaldoContablePrestHipotecario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO += endeudamientoAtraso.MontoProximaCuotaPrestHipotecario;
                Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS += endeudamientoAtraso.SaldoContableOtrosPrestamos;
                Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS += endeudamientoAtraso.MontoProximaCuotaOtrosPrestamos;
                Entrada.EA_LIMITE_ACUERDO += endeudamientoAtraso.LimiteAcuerdo;
                Entrada.EA_UTILIZACION_ACUERDO += endeudamientoAtraso.UtilizacionAcuerdo;
                Entrada.EA_LIMITE_COMPRA_TC += endeudamientoAtraso.LimiteCompraTc;
                Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES += endeudamientoAtraso.PagoMinimoPromUlt2Meses;
            }

            cobis = integrantes.PorIndice(3)?.Cobis ?? 0;
            if (fuentes.EndeudamientoAtraso.TryGetValue(cobis, out endeudamientoAtraso))
            {
                Entrada.EA_COT_3_MAX_DIAS_ATRASO_PRESTAMOS = endeudamientoAtraso.MaxDiasAtrasoPrestamos;
                Entrada.EA_COT_3_MOTIVO_REESTRUCTURACION = endeudamientoAtraso.MotivoReestructuracion;
                Entrada.EA_COT_3_MOTIVO_REFINANCIACION = endeudamientoAtraso.MotivoRefinanciacion;
                Entrada.EA_COT_3_MOTIVO_DEU = endeudamientoAtraso.MotivoDeu;
                Entrada.EA_COT_3_DIAS_ATRASO_TC = endeudamientoAtraso.DiasAtrasoTc;
                Entrada.EA_COT_3_MAX_DIAS_DESC_SIN_ACUERDO = endeudamientoAtraso.MaxDiasDescSinAcuerdo;
                Entrada.EA_COT_3_MAX_DIAS_CLEAN_UP = endeudamientoAtraso.MaxDiasCleanUp;
                Entrada.EA_COT_3_MAX_DIAS_EXCESO_ACUERDO = endeudamientoAtraso.MaxDiasExcesoAcuerdo;
                //Sumarizados
                Entrada.EA_SALDO_CONTABLE_PP += endeudamientoAtraso.SaldoContablePp;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PP += endeudamientoAtraso.MontoProximaCuotaPp;
                Entrada.EA_SALDO_CONTABLE_PREST_PRENDARIO += endeudamientoAtraso.SaldoContablePrestPrendario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_PRENDARIO += endeudamientoAtraso.MontoProximaCuotaPrestPrendario;
                Entrada.EA_SALDO_CONTABLE_PREST_HIPOTECARIO += endeudamientoAtraso.SaldoContablePrestHipotecario;
                Entrada.EA_MONTO_PROXIMA_CUOTA_PREST_HIPOTECARIO += endeudamientoAtraso.MontoProximaCuotaPrestHipotecario;
                Entrada.EA_SALDO_CONTABLE_OTROS_PRESTAMOS += endeudamientoAtraso.SaldoContableOtrosPrestamos;
                Entrada.EA_MONTO_PROXIMA_CUOTA_OTROS_PRESTAMOS += endeudamientoAtraso.MontoProximaCuotaOtrosPrestamos;
                Entrada.EA_LIMITE_ACUERDO += endeudamientoAtraso.LimiteAcuerdo;
                Entrada.EA_UTILIZACION_ACUERDO += endeudamientoAtraso.UtilizacionAcuerdo;
                Entrada.EA_LIMITE_COMPRA_TC += endeudamientoAtraso.LimiteCompraTc;
                Entrada.EA_PAGO_MINIMO_PROM_ULT_2_MESES += endeudamientoAtraso.PagoMinimoPromUlt2Meses;
            }

            TablonIndividuos tablonIndividuos;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (fuentes.TablonIndividuos.TryGetValue(numeroTributario.ToString(), out tablonIndividuos))
            {
                Log.Information("Match Tablon Titu");
                Entrada.TIND_CODIGO_CLIENTE = tablonIndividuos.CodigoCliente;
                Entrada.TIND_SEGMENTO = tablonIndividuos.Segmento;
                Entrada.TIND_CALIFICACION_CONV_PRINCIPAL = tablonIndividuos.CalificacionConvPrincipal;
                Entrada.TIND_NRO_CUENTA_PRINCIPAL = tablonIndividuos.NroCuentaPrincipal;
                Entrada.TIND_COD_CATEGORIA_CUENTA = tablonIndividuos.CodCategoriaCuenta;
                Entrada.TIND_MARCA_NO_PRECALIFICAR = tablonIndividuos.MarcaObsClienteNoPrecalificar;
                Entrada.TIND_MOTIVO_SITUACION_BM = tablonIndividuos.MotivoSituacionBm;
                Entrada.TIND_MOTIVO_SIT_CART_CEDIDA = tablonIndividuos.MotivoSitCartCedida;
                Entrada.TIND_MOTIVO_MORA_HISTORICA = tablonIndividuos.MotivoMoraHistorica;
                Entrada.TIND_MOTIVO_BANCA_CLIENTE = tablonIndividuos.MotivoBancaCliente;
                Entrada.TIND_MOTIVO_REFINANCIACION = tablonIndividuos.MotivoRefinanciacion;
                Entrada.TIND_MOTIVO_REESTRUCTURACION = tablonIndividuos.MotivoReestructuracion;
                Entrada.TIND_MOTIVO_DIFERIMIENTO = tablonIndividuos.MotivoDiferimiento;
                Entrada.TIND_CATEGORIA_MONOTRIBUTO = tablonIndividuos.CategoriaMonotributo;
                Entrada.TIND_ACTIVIDAD_MONOTRIBUTO = tablonIndividuos.ActividadMonotributo;
                Entrada.TIND_MONTO_INVERSION = tablonIndividuos.MontoInversion;
                Entrada.TIND_NO_ACREDITO_ULT_MES = tablonIndividuos.NoAcreditoUltMes;
                Entrada.TIND_NO_ACREDITO_ULT_2M = tablonIndividuos.NoAcreditoUlt2m;
                Entrada.TIND_INGRESO_CALCULADO_FINAL = tablonIndividuos.IngresoCalculadoFinal;
                Entrada.TIND_TIPO_INGRESO = tablonIndividuos.TipoIngreso;
                Entrada.TIND_CONVENIO_LIQUI_PRESTAMO = tablonIndividuos.ConvenioLiquiPrestamo;
                Entrada.TIND_ENTE_CONVENIO = tablonIndividuos.EnteConvenio;
                Entrada.TIND_MERCADO = tablonIndividuos.Mercado;
                Entrada.TIND_SUBMERCADO = tablonIndividuos.Submercado;
                Entrada.TIND_SIN_MERC_SUBM = tablonIndividuos.SinMercSubm;
                Entrada.TIND_COD_SEGMENTO_CRM = tablonIndividuos.CodSegmento;
                Entrada.TIND_COD_SUBSEGMENTO_CRM = tablonIndividuos.CodSubsegmento;
                Entrada.TIND_SEXO = tablonIndividuos.Sexo;
                Entrada.TIND_MOTIVO_CHEQ_RECH = tablonIndividuos.MotivoCheqRech ?? 0;
            }

            numeroTributario = integrantes.PorIndice(1)?.NumeroTributario ?? 0;
            if (fuentes.TablonIndividuos.TryGetValue(numeroTributario.ToString(), out tablonIndividuos))
            {
                Log.Information("Match Tablon Cot1");
                Entrada.TIND_COT_1_SEGMENTO = tablonIndividuos.Segmento;
                Entrada.TIND_COT_1_MARCA_NO_PRECALIFICAR = tablonIndividuos.MarcaObsClienteNoPrecalificar;
                Entrada.TIND_COT_1_MOTIVO_SITUACION_BM = tablonIndividuos.MotivoSituacionBm;
                Entrada.TIND_COT_1_MOTIVO_SIT_CART_CEDIDA = tablonIndividuos.MotivoSitCartCedida;
                Entrada.TIND_COT_1_MOTIVO_MORA_HISTORICA = tablonIndividuos.MotivoMoraHistorica;
                Entrada.TIND_COT_1_MOTIVO_REFINANCIACION = tablonIndividuos.MotivoRefinanciacion;
                Entrada.TIND_COT_1_MOTIVO_REESTRUCTURACION = tablonIndividuos.MotivoReestructuracion;
                Entrada.TIND_COT_1_MOTIVO_DIFERIMIENTO = tablonIndividuos.MotivoDiferimiento;
                Entrada.TIND_COT_1_NO_ACREDITO_ULT_MES = tablonIndividuos.NoAcreditoUltMes;
                Entrada.TIND_COT_1_NO_ACREDITO_ULT_2M = tablonIndividuos.NoAcreditoUlt2m;
                Entrada.TIND_COT_1_INGRESO_CALCULADO_FINAL = tablonIndividuos.IngresoCalculadoFinal;
                Entrada.TIND_COT_1_TIPO_INGRESO = tablonIndividuos.TipoIngreso;
                Entrada.TIND_COT_1_MOTIVO_CHEQ_RECH = tablonIndividuos.MotivoCheqRech ?? 0;
            }

            numeroTributario = integrantes.PorIndice(2)?.NumeroTributario ?? 0;
            if (fuentes.TablonIndividuos.TryGetValue(numeroTributario.ToString(), out tablonIndividuos))
            {
                Log.Information("Match Tablon Cot2");
                Entrada.TIND_COT_2_SEGMENTO = tablonIndividuos.Segmento;
                Entrada.TIND_COT_2_MARCA_NO_PRECALIFICAR = tablonIndividuos.MarcaObsClienteNoPrecalificar;
                Entrada.TIND_COT_2_MOTIVO_SITUACION_BM = tablonIndividuos.MotivoSituacionBm;
                Entrada.TIND_COT_2_MOTIVO_SIT_CART_CEDIDA = tablonIndividuos.MotivoSitCartCedida;
                Entrada.TIND_COT_2_MOTIVO_MORA_HISTORICA = tablonIndividuos.MotivoMoraHistorica;
                Entrada.TIND_COT_2_MOTIVO_REFINANCIACION = tablonIndividuos.MotivoRefinanciacion;
                Entrada.TIND_COT_2_MOTIVO_REESTRUCTURACION = tablonIndividuos.MotivoReestructuracion;
                Entrada.TIND_COT_2_MOTIVO_DIFERIMIENTO = tablonIndividuos.MotivoDiferimiento;
                Entrada.TIND_COT_2_NO_ACREDITO_ULT_MES = tablonIndividuos.NoAcreditoUltMes;
                Entrada.TIND_COT_2_NO_ACREDITO_ULT_2M = tablonIndividuos.NoAcreditoUlt2m;
                Entrada.TIND_COT_2_INGRESO_CALCULADO_FINAL = tablonIndividuos.IngresoCalculadoFinal;
                Entrada.TIND_COT_2_TIPO_INGRESO = tablonIndividuos.TipoIngreso;
                Entrada.TIND_COT_2_MOTIVO_CHEQ_RECH = tablonIndividuos.MotivoCheqRech ?? 0;
            }

            numeroTributario = integrantes.PorIndice(3)?.NumeroTributario ?? 0;
            if (fuentes.TablonIndividuos.TryGetValue(numeroTributario.ToString(), out tablonIndividuos))
            {
                Log.Information("Match Tablon Cot3");
                Entrada.TIND_COT_3_SEGMENTO = tablonIndividuos.Segmento;
                Entrada.TIND_COT_3_MARCA_NO_PRECALIFICAR = tablonIndividuos.MarcaObsClienteNoPrecalificar;
                Entrada.TIND_COT_3_MOTIVO_SITUACION_BM = tablonIndividuos.MotivoSituacionBm;
                Entrada.TIND_COT_3_MOTIVO_SIT_CART_CEDIDA = tablonIndividuos.MotivoSitCartCedida;
                Entrada.TIND_COT_3_MOTIVO_MORA_HISTORICA = tablonIndividuos.MotivoMoraHistorica;
                Entrada.TIND_COT_3_MOTIVO_REFINANCIACION = tablonIndividuos.MotivoRefinanciacion;
                Entrada.TIND_COT_3_MOTIVO_REESTRUCTURACION = tablonIndividuos.MotivoReestructuracion;
                Entrada.TIND_COT_3_MOTIVO_DIFERIMIENTO = tablonIndividuos.MotivoDiferimiento;
                Entrada.TIND_COT_3_NO_ACREDITO_ULT_MES = tablonIndividuos.NoAcreditoUltMes;
                Entrada.TIND_COT_3_NO_ACREDITO_ULT_2M = tablonIndividuos.NoAcreditoUlt2m;
                Entrada.TIND_COT_3_INGRESO_CALCULADO_FINAL = tablonIndividuos.IngresoCalculadoFinal;
                Entrada.TIND_COT_3_TIPO_INGRESO = tablonIndividuos.TipoIngreso;
                Entrada.TIND_COT_3_MOTIVO_CHEQ_RECH = tablonIndividuos.MotivoCheqRech ?? 0;
            }

            Entrada.LIMITE_TRANSITORIO_TOTAL = Decimal.Zero;
            Entrada.LIMITE_TRANSITORIO_TOTAL = Decimal.Zero;
            foreach (var limiteTransitorio in fuentes.LimiteTransitorio.Values)
            {
                Entrada.LIMITE_TRANSITORIO_TOTAL += limiteTransitorio?.LimiteTransitorioTotal ?? Decimal.Zero;
                Entrada.LIMITE_TRANSITORIO_DIFERENCIAL += limiteTransitorio?.LimiteTransitorioDiferencial ?? Decimal.Zero;
            }

            foreach (var intradiaria in fuentes.Intradiarias.Values)
            {
                Entrada.SUM_cuota_actual_OP += intradiaria.SUM_cuota_actual_OP;
                Entrada.SUM_cuota_actual_PH += intradiaria.SUM_cuota_actual_PH;
                Entrada.SUM_cuota_actual_PR += intradiaria.SUM_cuota_actual_PR;
                Entrada.SUM_cuota_actual_PP += intradiaria.SUM_cuota_actual_PP;
                Entrada.SUM_monto_deuda_actual_OP += intradiaria.SUM_monto_deuda_actual_OP;
                Entrada.SUM_monto_deuda_actual_PH += intradiaria.SUM_monto_deuda_actual_PH;
                Entrada.SUM_monto_deuda_actual_PP += intradiaria.SUM_monto_deuda_actual_PP;
                Entrada.SUM_monto_deuda_actual_PR += intradiaria.SUM_monto_deuda_actual_PR;

                if (intradiaria.SUM_cuota_actual_OP > 0
                    || intradiaria.SUM_cuota_actual_PH > 0
                    || intradiaria.SUM_cuota_actual_PR > 0
                    || intradiaria.SUM_cuota_actual_PP > 0
                    || intradiaria.SUM_monto_deuda_actual_OP > 0
                    || intradiaria.SUM_monto_deuda_actual_PH > 0
                    || intradiaria.SUM_monto_deuda_actual_PP > 0
                    || intradiaria.SUM_monto_deuda_actual_PR > 0) Entrada.tiene_intradiarias = "SI";
            }
            if(Entrada.tiene_intradiarias is "") Entrada.tiene_intradiarias = "NO";

            CalificacionConstante calificacionConstante;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (request.OrigenId is 13)
            { 
                if(fuentes.CpAnalisis.TryGetValue(numeroTributario, out var cpAnalisis)
                    && fuentes.CpAuxiliar.TryGetValue(numeroTributario, out var cpAuxiliar) 
                    && Entrada.Solicita_Tarjeta is "SI")
                {
                    if(cpAnalisis.LimiteAOtorgarTc > 0)
                    {
                        Entrada.CC_ID_CAMPANA = "CP";
                        Entrada.CC_CODIGO_CAMPANA = cpAnalisis.IdCampaña ?? string.Empty;
                        Entrada.CC_SEGMENTO_CRM = cpAuxiliar.SegmentoCod ?? string.Empty;
                        Entrada.CC_SUBSEGMENTO_CRM = cpAuxiliar.SubsegmentoCod ?? string.Empty;
                        Entrada.CC_SUCURSAL_MIEMBRO = cpAnalisis.Sucursal;
                        Entrada.CC_CANAL = "PERSONAL";
                        Entrada.CC_TIPO_PRECALIFICACION = string.Empty;
                        Entrada.CC_FECHA_CALIFICACION = cpAnalisis.FechaCalificacion;
                        Entrada.CC_FECHA_VENCIMIENTO = DateTime.ParseExact(cpAuxiliar.FechaVencimiento ?? "1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        Entrada.CC_CONVENIO = cpAnalisis.Convenio?.ToString() ?? string.Empty;
                        Entrada.CC_PRODUCTO_TARJETA = "028-TARJCRED";
                        Entrada.CC_TARJETA_MONTO = cpAnalisis.LimiteAOtorgarTc;                        
                        Entrada.CC_ID_OFERTAS = cpAnalisis.IdEjecucion ?? string.Empty;
                    }
                }


            } else if (fuentes.CalificacionConstante.TryGetValue(numeroTributario.ToString(), out calificacionConstante))
            {
                if ((Entrada.Solicita_Prestamo is "SI" && calificacionConstante.CcPrestamoMonto is not 0)
                    || (Entrada.Solicita_Tarjeta is "SI" && calificacionConstante.CcTarjetaMonto is not 0))

                    Entrada.CC_ID_CAMPANA = calificacionConstante.CcIdCampana;
                Entrada.CC_CODIGO_CAMPANA = calificacionConstante.CcCodigoCampana;
                Entrada.CC_SEGMENTO_CRM = calificacionConstante.CcSegmentoCrm;
                Entrada.CC_SUBSEGMENTO_CRM = calificacionConstante.CcSubsegmentoCrm;
                Entrada.CC_SUCURSAL_MIEMBRO = calificacionConstante.CcSucursalMiembro;
                Entrada.CC_CANAL = calificacionConstante.CcCanal;
                Entrada.CC_TIPO_PRECALIFICACION = calificacionConstante.CcTipoPrecalificacion;
                Entrada.CC_FECHA_CALIFICACION = calificacionConstante.CcFechaCalificacion;
                Entrada.CC_FECHA_VENCIMIENTO = calificacionConstante.CcFechaVencimiento;
                Entrada.CC_CONVENIO = calificacionConstante.CcConvenio;
                Entrada.CC_PRODUCTO_PRESTAMO = calificacionConstante.CcProductoPrestamo;
                Entrada.CC_PRODUCTO_TARJETA = calificacionConstante.CcProductoTarjeta;
                Entrada.CC_PRODUCTO_ACUERDO = calificacionConstante.CcProductoAcuerdo;
                Entrada.CC_PRESTAMO_TASA = calificacionConstante.CcPrestamoTasa;
                Entrada.CC_PRESTAMO_PLAZO = calificacionConstante.CcPrestamoPlazo;
                Entrada.CC_PRESTAMO_MONTO = calificacionConstante.CcPrestamoMonto;
                Entrada.CC_PRESTAMO_AFECTACION = calificacionConstante.CcPrestamoAfectacion;
                Entrada.CC_PRESTAMO_CUOTA_PROMEDIO = calificacionConstante.CcPrestamoCuotaPromedio;
                Entrada.CC_PRESTAMO_CUOTA_INICIAL = calificacionConstante.CcPrestamoCuotaInicial;
                Entrada.CC_TARJETA_MONTO = calificacionConstante.CcTarjetaMonto;
                Entrada.CC_ACUERDO_MONTO = calificacionConstante.CcAcuerdoMonto;
                Entrada.CC_ID_OFERTAS = calificacionConstante.CcIdOfertas;
            }

            if (Entrada.CC_CODIGO_CAMPANA is not "")
            {
                if (fuentes.CampañasExceptuadas.Count() > 0)
                {
                    CampañasExceptuadas campañasExceptuadas = fuentes.CampañasExceptuadas.Values.First();
                    Entrada.CAMPEXCEP_Codigo_campana = campañasExceptuadas.CodigoCampana;
                    Entrada.CAMPEXCEP_FECHA_FIN_PREVISTO = campañasExceptuadas.FechaFinPrevisto;
                    Entrada.CAMPEXCEP_ESTADO = campañasExceptuadas.Estado;
                    Entrada.CAMPEXCEP_BCRA_EXCEPTUADO = campañasExceptuadas.BcraExceptuado;
                    Entrada.CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO = campañasExceptuadas.ActividadExceptuado;
                    Entrada.CAMPEXCEP_SUCURSAL_EXCLUSIVA = campañasExceptuadas.SucursalExclusiva;
                }
            }

            Entrada.PM_Ajuste_monto_Camuzzi = parametros.GetDecimal("Ajuste_monto_Camuzzi", 1.04M);
            Entrada.PM_Ajuste_monto = parametros.GetDecimal("Ajuste_monto", 1);
            Entrada.PM_Ajuste_cuota_maxima = parametros.GetDecimal("Ajuste_cuota_maxima", 1);

            Precacom precacom;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (fuentes.Precacom.TryGetValue(numeroTributario, out precacom))
            {
                Entrada.PRECACOM_CODIGO = precacom.PrecacomCodigo;
                Entrada.PRECACOM_CUIL = precacom.PrecacomCuil;
                Entrada.PRECACOM_ORIGEN_ID = precacom.PrecacomOrigenId;
                Entrada.PRECACOM_COMERCIO_ID = precacom.PrecacomComercioId;
                Entrada.PRECACOM_ID_ALTERNATIVO = precacom.PrecacomIdAlternativo;
                Entrada.PRECACOM_PRESTAMO_MONTO_MAXIMO = precacom.PrecacomPrestamoMontoMaximo;
                Entrada.PRECACOM_PRESTAMO_PLAZO_MAXIMO = precacom.PrecacomPrestamoPlazoMaximo;
                Entrada.PRECACOM_PRESTAMO_TNA = precacom.PrecacomPrestamoTna;
                Entrada.PRECACOM_PRESTAMO_TIPO_TASA = precacom.PrecacomPrestamoTipoTasa;
                Entrada.PRECACOM_PRESTAMO_CUOTA_MAXIMO = precacom.PrecacomPrestamoCuotaMaximo;
                Entrada.PRECACOM_TARJETA_CREDITO_LIMITE = precacom.PrecacomTarjetaCreditoLimite;
                Entrada.PRECACOM_CUENTA_CORRIENTE_LIMITE = precacom.PrecacomTarjetaCuentaCorrienteLimite;
                Entrada.PRECACOM_VIGENTE_DESDE = precacom.PrecacomVigenteDesde;
                Entrada.PRECACOM_VIGENTE_HASTA = precacom.PrecacomVigenteHasta;
                Entrada.PRECACOM_VIGENCIA = precacom.PrecacomVigencia;
                Entrada.PRECACOM_COM1 = precacom.PrecacomCom1;
                Entrada.PRECACOM_COM2 = precacom.PrecacomCom2;
                Entrada.PRECACOM_COM3 = precacom.PrecacomCom3;
                Entrada.CAMPEXCEP_Codigo_campana = precacom.CampexcepCodigoCampana;
                Entrada.CAMPEXCEP_FECHA_FIN_PREVISTO = precacom.CampexcepFechaFinPrevisto;
                Entrada.CAMPEXCEP_ESTADO = precacom.CampexcepEstado;
                Entrada.CAMPEXCEP_BCRA_EXCEPTUADO = precacom.CampexcepBcraExceptuado;
                Entrada.CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO = precacom.CampexcepCtrlActividadExceptuado;
                Entrada.CAMPEXCEP_SUCURSAL_EXCLUSIVA = precacom.CampexcepSucursalExclusiva;
            }

            ListaNegra listaNegra;
            numeroTributario = integrantes.PorIndice(0)?.NumeroTributario ?? 0;
            if (fuentes.ListaNegra.TryGetValue(numeroTributario, out listaNegra))
            {
                Entrada.LN_Cant = 1;
                Entrada.LN_Motivo = listaNegra.MotivoObservacion ?? string.Empty;
            }
            numeroTributario = integrantes.PorIndice(1)?.NumeroTributario ?? 0;
            if (fuentes.ListaNegra.TryGetValue(numeroTributario, out listaNegra))
            {
                Entrada.LN_COT_1_Cant = 1;
                Entrada.LN_COT_1_Motivo = listaNegra.MotivoObservacion ?? string.Empty;
            }
            numeroTributario = integrantes.PorIndice(2)?.NumeroTributario ?? 0;
            if (fuentes.ListaNegra.TryGetValue(numeroTributario, out listaNegra))
            {
                Entrada.LN_COT_2_Cant = 1;
                Entrada.LN_COT_2_Motivo = listaNegra.MotivoObservacion ?? string.Empty;
            }
            numeroTributario = integrantes.PorIndice(3)?.NumeroTributario ?? 0;
            if (fuentes.ListaNegra.TryGetValue(numeroTributario, out listaNegra))
            {
                Entrada.LN_COT_3_Cant = 1;
                Entrada.LN_COT_3_Motivo = listaNegra.MotivoObservacion ?? string.Empty;
            }

            if(request.Titular?.Calificacion is null or "" && fuentes.LkpConvenios.Count > 0)
            {
                if (fuentes.LkpConvenios.TryGetValue(request.Titular?.NumeroConvenio ?? -1, out var lkpConvenios)){
                    Entrada.TIT_Calificacion = lkpConvenios.Calificacion ?? string.Empty;
                }                
            }

            if (fuentes.ConveniosCodDescuento.Count > 0)
            {
                var conveniosCodDescuento = fuentes.ConveniosCodDescuento.FirstOrDefault().Value;
                Entrada.CCD_estado_convenio = conveniosCodDescuento.Estado ?? string.Empty;
                Entrada.CCD_calificacion_permanente = conveniosCodDescuento.CalificacionPermanente ?? string.Empty;
                Entrada.CCD_calificacion_otra_planta = conveniosCodDescuento.CalificacionOtraPlanta ?? string.Empty;
            }
        }

        public void MapeaRequest(RequestEvaluaIndividuosArray requestArray)
        {
            MapeaRequest(requestArray.element);
        }

        public void MapeaRequest(RequestEvaluaIndividuos request)
        {
            Entrada.Origen_ID = request.OrigenId;
            Entrada.Numero_Oportunidad = request.NumeroOportunidad;
            Entrada.Numero_Evaluacion = request.NumeroEvaluacion;
            Entrada.Comercio_ID = request.ComercioId;
            Entrada.Comercio_Sucursal = request.ComercioSucursal;
            Entrada.Comercio_Usuario = request.ComercioUsuario;
            Entrada.Provincia = request.Provincia;
            Entrada.Sucursal_Macro_Asociada = request.SucursalMacroAsociada;
            Entrada.Canal = request.Canal;
            if (request.ProductosPrecancelados is not null) Entrada.Productos_Precancelados = string.Join("|", request.ProductosPrecancelados);
            Entrada.Preca_Utiliza = request.PrecalificacionUtiliza;
            Entrada.Preca_Codigo = request.PrecalificacionCodigo;
            Entrada.Preca_Oferta = request.PrecalificacionOferta;
            Entrada.Circuito = request.Circuito;
            Entrada.TIT_Cobis = request.Titular?.Cobis;
            Entrada.TIT_Cuil = request.Titular?.NumeroCUIL;
            Entrada.TIT_Numero_Documento = request.Titular?.NumeroDocumento;
            Entrada.TIT_Tipo_Documento = request.Titular?.TipoDocumento;
            Entrada.TIT_Version_Documento = request.Titular?.VersionDocumento;
            Entrada.TIT_Sexo = request.Titular?.Sexo;
            Entrada.TIT_Apellido = request.Titular?.Apellido;
            Entrada.TIT_Nombre = request.Titular?.Nombre;
            Entrada.TIT_Nacimiento = request.Titular?.FechaNacimiento;
            Entrada.TIT_Estado_Civil = request.Titular?.EstadoCivil;
            Entrada.TIT_Actividad_1 = request.Titular?.Actividad1?.Actividad;
            Entrada.TIT_Actividad_1_Fecha_Inicio = request.Titular?.Actividad1?.FechaInicio;
            Entrada.TIT_Actividad_1_Neto_Fijo = request.Titular?.Actividad1?.NetoFijo;
            Entrada.TIT_Actividad_1_Neto_Variable = request.Titular?.Actividad1?.NetoVariable;
            Entrada.TIT_Actividad_1_Categoria_Monotributo = request.Titular?.Actividad1?.CategoriaMonotributo;
            Entrada.TIT_Actividad_1_Actividad_Monotributo = request.Titular?.Actividad1?.ActividadMonotributo;
            Entrada.TIT_Actividad_2 = request.Titular?.Actividad2?.Actividad;
            Entrada.TIT_Actividad_2_Fecha_Inicio = request.Titular?.Actividad2?.FechaInicio;
            Entrada.TIT_Actividad_2_Neto_Fijo = request.Titular?.Actividad2?.NetoFijo;
            Entrada.TIT_Actividad_2_Neto_Variable = request.Titular?.Actividad2?.NetoVariable;
            Entrada.TIT_Actividad_2_Categoria_Monotributo = request.Titular?.Actividad2?.CategoriaMonotributo;
            Entrada.TIT_Actividad_2_Actividad_Monotributo = request.Titular?.Actividad2?.ActividadMonotributo;
            Entrada.TIT_Vivienda = request.Titular?.Vivienda;
            Entrada.TIT_Alquiler_Monto = request.Titular?.AlquilerMonto;
            Entrada.TIT_Segmento = request.Titular?.Segmento;
            Entrada.TIT_Subsegmento = request.Titular?.Subsegmento;
            Entrada.TIT_Categoria_Cliente = request.Titular?.CategoriaCliente;
            Entrada.TIT_Numero_Convenio = request.Titular?.NumeroConvenio;
            Entrada.TIT_Nombre_Convenio = request.Titular?.NombreConvenio;
            Entrada.TIT_Calificacion = request.Titular?.Calificacion;
            Entrada.TIT_Submercado = request.Titular?.Submercado;
            Entrada.TIT_Monto_Inversor = request.Titular?.MontoInversor;
            Entrada.TIT_Tipo_Contribuyente = request.Titular?.TipoContribuyente;
            Entrada.SUM_cuota_actual_PP = request.Sumatoria?.CuotaActualPrestamoPersonal ?? 0M;
            Entrada.SUM_monto_deuda_actual_PP = request.Sumatoria?.MontoDeudaActualPrestamoPersonal ?? 0M;
            Entrada.SUM_cuota_actual_PR = request.Sumatoria?.CuotaActualPrestamoPrendario ?? 0M;
            Entrada.SUM_monto_deuda_actual_PR = request.Sumatoria?.MontoDeudaActualPrestamoPrendario ?? 0M;
            Entrada.SUM_cuota_actual_PH = request.Sumatoria?.CuotaActualPrestamoHipotecario ?? 0M;
            Entrada.SUM_monto_deuda_actual_PH = request.Sumatoria?.MontoDeudaActualPrestamoHipotecario ?? 0M;
            Entrada.SUM_cuota_actual_OP = request.Sumatoria?.CuotaActualOtrosPrestamos ?? 0M;
            Entrada.SUM_monto_deuda_actual_OP = request.Sumatoria?.MontoDeudaActualOtrosPrestamos ?? 0M;
            Entrada.SUM_limites_TC_Macro = request.Sumatoria?.LimitesTarjetaCreditoMacro ?? 0M;
            Entrada.SUM_pago_min_prom_ult2_TC_Macro = request.Sumatoria?.PagoMinPromUltimos2MesesTCMacro ?? 0M;
            Entrada.SUM_acuerdos_cc_Macro = request.Sumatoria?.AcuerdosCuentaCorrienteMacro ?? 0M;
            Entrada.SUM_uso_prom_ult2_acuerdos_cc_Macro = request.Sumatoria?.UsoPromUltimos2MesesAcuerdosCCMacro ?? 0M;
            Entrada.Cuota_prest_sin_gtia_no_bureau = request.CuotaPrestamoSinGarantiaNoBureau ?? 0M;
            Entrada.Cuota_prest_con_gtia_no_bureau = request.CuotaPrestamoConGarantiaNoBureau ?? 0M;
            Entrada.Pago_min_tarj_no_bureau = request.PagoMinimoTarjetaNoBureau ?? 0M;
            Entrada.Util_cta_cte_no_bureau = request.UtilidadCuentaCorrienteNoBureau ?? 0M;
            Entrada.Solicita_Tarjeta = request.SolicitaTarjeta?.ToUpper() ?? "NO";
            Entrada.Tarjeta_Limite = request.TarjetaLimite;
            Entrada.Solicita_Prestamo = request.SolicitaPrestamo?.ToUpper() ?? "NO";
            Entrada.Prestamo_Monto = request.PrestamoMonto;
            Entrada.Prestamo_Destino = request.PrestamoDestino;
            Entrada.Prestamo_Plazo = request.PrestamoPlazo;
            Entrada.Prestamo_Tipo_Tasa = request.PrestamoTipoTasa;
            Entrada.Prestamo_TNA = request.PrestamoTNA;
            Entrada.Con_Garantia_Valor_Bien = request.ConGarantiaValorBien;
            Entrada.Con_Garantia_Subsidio = request.ConGarantiaSubsidio;
            Entrada.Con_Garantia_Tasacion = request.ConGarantiaTasacion;
            Entrada.Hipotecario_Valor_Terreno = request.HipotecarioValorTerreno;
            Entrada.Modelo_Automovil = request.ModeloAutomovil;
            Entrada.Solicita_Acuerdo = request.SolicitaAcuerdo?.ToUpper() ?? "NO";
            Entrada.Acuerdo_Monto = request.AcuerdoMonto;
            Entrada.COT_1_Cobis = request.Cotitular1?.Cobis;
            Entrada.COT_1_Cuil = request.Cotitular1?.NumeroCUIL;
            Entrada.COT_1_Numero_Documento = request.Cotitular1?.NumeroDocumento;
            Entrada.COT_1_Tipo_Documento = request.Cotitular1?.TipoDocumento;
            Entrada.COT_1_Version_Documento = request.Cotitular1?.VersionDocumento;
            Entrada.COT_1_Sexo = request.Cotitular1?.Sexo;
            Entrada.COT_1_Apellido = request.Cotitular1?.Apellido;
            Entrada.COT_1_Nombre = request.Cotitular1?.Nombre;
            Entrada.COT_1_Nacimiento = request.Cotitular1?.FechaNacimiento;
            Entrada.COT_1_Estado_Civil = request.Cotitular1?.EstadoCivil;
            Entrada.COT_1_Segmento = request.Cotitular1?.Segmento;
            Entrada.COT_1_Subsegmento = request.Cotitular1?.Subsegmento;
            Entrada.COT_1_Actividad_1 = request.Cotitular1?.Actividad1?.Actividad;
            Entrada.COT_1_Actividad_1_Fecha = request.Cotitular1?.Actividad1?.FechaInicio;
            Entrada.COT_1_Actividad_1_Neto_Fijo = request.Cotitular1?.Actividad1?.NetoFijo;
            Entrada.COT_1_Actividad_1_Neto_Variable = request.Cotitular1?.Actividad1?.NetoVariable;
            Entrada.COT_1_Actividad_1_Categoria_Monotributo = request.Cotitular1?.Actividad1?.CategoriaMonotributo;
            Entrada.COT_1_Actividad_1_Actividad_Monotributo = request.Cotitular1?.Actividad1?.ActividadMonotributo;
            Entrada.COT_1_Actividad_2 = request.Cotitular1?.Actividad2?.Actividad;
            Entrada.COT_1_Actividad_2_Fecha = request.Cotitular1?.Actividad2?.FechaInicio;
            Entrada.COT_1_Actividad_2_Neto_Fijo = request.Cotitular1?.Actividad2?.NetoFijo;
            Entrada.COT_1_Actividad_2_Neto_Variable = request.Cotitular1?.Actividad2?.NetoVariable;
            Entrada.COT_1_Actividad_2_Categoria_Monotributo = request.Cotitular1?.Actividad2?.CategoriaMonotributo;
            Entrada.COT_1_Actividad_2_Actividad_Monotributo = request.Cotitular1?.Actividad2?.ActividadMonotributo;
            Entrada.COT_2_Cobis = request.Cotitular2?.Cobis;
            Entrada.COT_2_Cuil = request.Cotitular2?.NumeroCUIL;
            Entrada.COT_2_Numero_Documento = request.Cotitular2?.NumeroDocumento;
            Entrada.COT_2_Tipo_Documento = request.Cotitular2?.TipoDocumento;
            Entrada.COT_2_Version_Documento = request.Cotitular2?.VersionDocumento;
            Entrada.COT_2_Sexo = request.Cotitular2?.Sexo;
            Entrada.COT_2_Apellido = request.Cotitular2?.Apellido;
            Entrada.COT_2_Nombre = request.Cotitular2?.Nombre;
            Entrada.COT_2_Nacimiento = request.Cotitular2?.FechaNacimiento;
            Entrada.COT_2_Estado_Civil = request.Cotitular2?.EstadoCivil;
            Entrada.COT_2_Segmento = request.Cotitular2?.Segmento;
            Entrada.COT_2_Subsegmento = request.Cotitular2?.Subsegmento;
            Entrada.COT_2_Actividad_1 = request.Cotitular2?.Actividad1?.Actividad;
            Entrada.COT_2_Actividad_1_Fecha = request.Cotitular2?.Actividad1?.FechaInicio;
            Entrada.COT_2_Actividad_1_Neto_Fijo = request.Cotitular2?.Actividad1?.NetoFijo;
            Entrada.COT_2_Actividad_1_Neto_Variable = request.Cotitular2?.Actividad1?.NetoVariable;
            Entrada.COT_2_Actividad_1_Categoria_Monotributo = request.Cotitular2?.Actividad1?.CategoriaMonotributo;
            Entrada.COT_2_Actividad_1_Actividad_Monotributo = request.Cotitular2?.Actividad1?.ActividadMonotributo;
            Entrada.COT_2_Actividad_2 = request.Cotitular2?.Actividad2?.Actividad;
            Entrada.COT_2_Actividad_2_Fecha = request.Cotitular2?.Actividad2?.FechaInicio;
            Entrada.COT_2_Actividad_2_Neto_Fijo = request.Cotitular2?.Actividad2?.NetoFijo;
            Entrada.COT_2_Actividad_2_Neto_Variable = request.Cotitular2?.Actividad2?.NetoVariable;
            Entrada.COT_2_Actividad_2_Categoria_Monotributo = request.Cotitular2?.Actividad2?.CategoriaMonotributo;
            Entrada.COT_2_Actividad_2_Actividad_Monotributo = request.Cotitular2?.Actividad2?.ActividadMonotributo;
            Entrada.COT_3_Cobis = request.Cotitular3?.Cobis;
            Entrada.COT_3_Cuil = request.Cotitular3?.NumeroCUIL;
            Entrada.COT_3_Numero_Documento = request.Cotitular3?.NumeroDocumento;
            Entrada.COT_3_Tipo_Documento = request.Cotitular3?.TipoDocumento;
            Entrada.COT_3_Version_Documento = request.Cotitular3?.VersionDocumento;
            Entrada.COT_3_Sexo = request.Cotitular3?.Sexo;
            Entrada.COT_3_Apellido = request.Cotitular3?.Apellido;
            Entrada.COT_3_Nombre = request.Cotitular3?.Nombre;
            Entrada.COT_3_Nacimiento = request.Cotitular3?.FechaNacimiento;
            Entrada.COT_3_Estado_Civil = request.Cotitular3?.EstadoCivil;
            Entrada.COT_3_Segmento = request.Cotitular3?.Segmento;
            Entrada.COT_3_Subsegmento = request.Cotitular3?.Subsegmento;
            Entrada.COT_3_Actividad_1 = request.Cotitular3?.Actividad1?.Actividad;
            Entrada.COT_3_Actividad_1_Fecha = request.Cotitular3?.Actividad1?.FechaInicio;
            Entrada.COT_3_Actividad_1_Neto_Fijo = request.Cotitular3?.Actividad1?.NetoFijo;
            Entrada.COT_3_Actividad_1_Neto_Variable = request.Cotitular3?.Actividad1?.NetoVariable;
            Entrada.COT_3_Actividad_1_Categoria_Monotributo = request.Cotitular3?.Actividad1?.CategoriaMonotributo;
            Entrada.COT_3_Actividad_1_Actividad_Monotributo = request.Cotitular3?.Actividad1?.ActividadMonotributo;
            Entrada.COT_3_Actividad_2 = request.Cotitular3?.Actividad2?.Actividad;
            Entrada.COT_3_Actividad_2_Fecha = request.Cotitular3?.Actividad2?.FechaInicio;
            Entrada.COT_3_Actividad_2_Neto_Fijo = request.Cotitular3?.Actividad2?.NetoFijo;
            Entrada.COT_3_Actividad_2_Neto_Variable = request.Cotitular3?.Actividad2?.NetoVariable;
            Entrada.COT_3_Actividad_2_Categoria_Monotributo = request.Cotitular3?.Actividad2?.CategoriaMonotributo;
            Entrada.COT_3_Actividad_2_Actividad_Monotributo = request.Cotitular3?.Actividad2?.ActividadMonotributo;
            Entrada.Extra = request.Extra;
            Entrada.Seguro_Automovil = request.SeguroAutomovil;
            Entrada.NUC_Numero = request.Nucleador?.Numero ?? "";
            Entrada.NUC_Tipo = request.Nucleador?.Tipo ?? "";
            Entrada.NUC_Medio = request.Nucleador?.Medio ?? "";
            Entrada.NUC_Adicional = request.Nucleador?.Adicional ?? "";
        }
        public ResponseEvaluaIndividuos MapeaSalida(PoliticaEvaluaIndividuosSalida salida)
        {
            var response = new ResponseEvaluaIndividuos
            {
                OrigenId = salida.Origen_ID,
                NumeroOportunidad = salida.Numero_Oportunidad,
                NumeroEvaluacion = salida.Numero_Evaluacion,
                EsCliente = salida.Es_Cliente,
                Cobis = salida.Cobis,
                NumeroCUIL = salida.Cuil,
                Rama = salida.Rama,
                Dictamen = salida.Dictamen,
                Codigos_Rechazos = salida.Codigos_Rechazo,
                Explicacion = salida.Explicacion,
                PrestamoMontoMaximo = salida.Prestamo_Monto_Maximo,
                PrestamoCuotaMaxima = salida.Prestamo_Cuota_Maxima,
                PrestamoPlazoMaximo = salida.Prestamo_Plazo_Maximo,
                PrestamoTipoTasa = salida.Prestamo_Tipo_Tasa,
                PrestamoTNA = salida.Prestamo_Tna,
                TarjetaCreditoLimite = salida.Tarjeta_Credito_Limite,
                CuentaCorrienteLimite = salida.Cuenta_Corriente_Limite,
                ReportePolitica = salida.Reporte_Politica,
                OutputAdicional = salida.Output_Adicional,
                NivelDeRiesgo = salida.Nivel_de_Riesgo,
                IngresoComputableNeto = salida.Ingreso_Computable
            };
            return response;
        }
    }
}
