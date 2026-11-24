using System.Data;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml;

namespace CalculadoraAgro.Model
{
    public static class MEBI
    {
        public static string GetConnectionString()
        {
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    return "Data Source=SRVMEBIDB-PROD,7427;Initial Catalog=CESDB;Integrated Security=SSPI";
                case Ambiente.PREP:
                    return "Data Source=SRVMEBIDB-PRE,7427;Initial Catalog=CESDB;Integrated Security=SSPI";
                default:
                    return "Data Source=SRVMEBIDB-TEST,7427;Initial Catalog=CESDB;Integrated Security=SSPI";
            }
        }

        public static string GetBaseURL()
        {
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    return "https://srvmebiwb-prod.macro.com.ar";
                case Ambiente.PREP:
                    return "https://srvmebiwb-pre.macro.com.ar";
                default: // Testing y Desarrollo
                    return "https://srvmebiwb-test.macro.com.ar";
            }
        }
    }

    public interface IEvaluable
    {
        string[] codificaEntrada();
        void decodificaSalida(string[] ArraySalida);
        Politica GetPolitica();
    }

    public class EvaluacionCrediticia
    {
        private string[] _ArrayEntrada;
        private string[] _ArraySalida;
        private Politica _Politica;
        private DateTime _TimestampRequest;
        private DateTime _TimestampResponse;

        private EvaluacionCrediticia() { }

        public string[] ArraySalida
        {
            get
            {
                return _ArraySalida;
            }
        }

        public static async Task<EvaluacionCrediticia> EvaluarAsync(IEvaluable datosPolitica, Registro registro)
        {
            var evaluacion = new EvaluacionCrediticia();
            evaluacion._Politica = datosPolitica.GetPolitica();
            evaluacion._ArrayEntrada = datosPolitica.codificaEntrada();
            var CodigoPolitica = "Politica" + (evaluacion._Politica.Codigo).ToString();

            //Genera Trama Entrada
            string tramaEntrada = GenerarTrama(evaluacion._ArrayEntrada);

            //Invoca Modeler
            registro.RegistraInicio(evaluacion._Politica.Nombre, tramaEntrada);
            var t = await evaluacion.InvocarPoliticaAsync(CodigoPolitica, tramaEntrada);

            //Procesa respuesta
            string tramaSalida = t.ToString();
            registro.RegistraFin(evaluacion._Politica.Nombre, tramaSalida);
            evaluacion._ArraySalida = tramaSalida.Split(';');


            //Interrumpe ejecucion si hubo error
            string errorModeler = "";
            if (evaluacion._ArraySalida.Length == 1)
            {
                errorModeler = evaluacion._ArraySalida[0];
            }
            else if (evaluacion._ArraySalida[evaluacion._Politica.IdxError] == "ERROR")
            {
                errorModeler = evaluacion._ArraySalida[evaluacion._Politica.IdxDescripcionError];
            }
            if (errorModeler != "")
            {
                if (errorModeler == "Error: Error en el envío de datos al servidor." || errorModeler == "CES no respondió en el tiempo establecido.")
                {
                    throw new MAPIException("EvaluacionCrediticia", 2, errorModeler, 504);
                }
                else
                {
                    throw new MAPIException("EvaluacionCrediticia", 3, errorModeler, 409);
                }
            }

            //Devuelve el objeto con toda la información de la Evaluacion
            return evaluacion;
        }

        public void RegistrarEvaluacion(Guid Id, String ReferenciaCanal = "")
        {
            string query = @"
            INSERT INTO [dbo].[Evaluaciones]
           ([id]
           ,[nro_tributario]
           ,[inicio]
           ,[fin]
           ,[politica]
           ,[entrada]
           ,[salida]
           ,[referencia_canal])
     VALUES
           (@id
           ,@nro_tributario
           ,@inicio
           ,@fin
           ,@politica
           ,@entrada
           ,@salida
           ,@referencia_canal)";

            string connStr = MEBI.GetConnectionString();

            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@id", Id);
            comm.Parameters.AddWithValue("@nro_tributario", _ArrayEntrada[_Politica.IdxNroTributario]);
            comm.Parameters.AddWithValue("@inicio", _TimestampRequest);
            comm.Parameters.AddWithValue("@fin", _TimestampResponse);
            comm.Parameters.AddWithValue("@politica", _Politica.Codigo);
            comm.Parameters.AddWithValue("@entrada", GenerarTrama(_ArrayEntrada));
            comm.Parameters.AddWithValue("@salida", GenerarTrama(_ArraySalida));
            comm.Parameters.AddWithValue("@referencia_canal", ReferenciaCanal);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                conn.Close();
                // Manejar excepción
            }
        }

        private static string GenerarTrama(string[] ArrayCampos)
        {
            string trama = string.Empty;
            for (int i = 0; i < ArrayCampos.Length - 1; i++)
            {
                trama += ArrayCampos[i] + ";";
            }
            trama += ArrayCampos[ArrayCampos.Length - 1];
            return trama;
        }

        private string GetRequestPolitica(string numPolitica, string trama)
        {
            var soapStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                        <soap12:Body>
                        <EjecutarPolitica xmlns=""http://www.spss.com.ar/ces/ws"">
                        <strRegistro>" + trama + @"</strRegistro>
                        <strMetodo>" + numPolitica + @"</strMetodo>
                        </EjecutarPolitica>
                        </soap12:Body>
                        </soap12:Envelope>";
            return soapStr;
        }

        private async Task<object> InvocarPoliticaAsync(string politica, string entrada)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            var requestUri = MEBI.GetBaseURL() + "/ces_ws_generic_pac/ces_ws.asmx";
            var soapStr = GetRequestPolitica(politica, entrada);
            var soapBytes = System.Text.Encoding.UTF8.GetBytes(soapStr);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Headers.Add("Accept-Encoding", "zip, gzip, deflate, br");
                request.ContentType = "application/soap+xml";
                request.ContentLength = soapBytes.Length;
                request.Method = "POST";

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(soapBytes, 0, soapBytes.Length);
                }

                var t = await request.GetResponseAsync();
                using (var dataStream = t.GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    var sd2Request = reader.ReadToEnd();
                    var doc = new XmlDocument();
                    doc.LoadXml(sd2Request);
                    var tramaSalida = doc.InnerText;
                    return tramaSalida;
                }
            }
            catch (WebException ex)
            {
                // Manejar excepción
                return ex;
            }
        }
    }

    public sealed class Politica
    {

        public static Politica EvaluaIndividuos = new Politica("EvaluaIndividuos", 10, 151, 21, 3, 4, 15);
        public static Politica CalculadoraAgro = new Politica("CalculadoraAgro", 11, 273, 129, 3, 4, 5);
        public static Politica CalculadoraAgroV2 = new Politica("CalculadoraAgroV2", 15, 273, 129, 3, 4, 5);
        //        MercadoAbierto = 1,
        //        PlanSueldoPrivado = 2,
        //        RentaPresunta = 3,
        //        PrimerFiltro = 4,
        //        CreditoAlInstante = 5,
        //        ScoringMepy = 6,
        //        PlanSueldoPublicoJubilados = 7,
        //        Actividad = 9,
        //        EvaluaIndividuos = 10,
        //        CalculadoraAgro = 11,
        //        ScoringAgro = 12,
        //        CreditoSimple = 13,
        //        CreditoSimpleSISA = 14,
        //        CalculadoraAgroV2 = 15
        public readonly string Nombre;
        public readonly int Codigo;
        public readonly int CamposEntrada;
        public readonly int CamposSalida;
        public readonly int IdxError;
        public readonly int IdxDescripcionError;
        public readonly int IdxNroTributario;

        private Politica(string nombre, int codigo, int camposEntrada, int camposSalida, int idxError, int idxDescripcionError, int idxNroTributario)
        {
            Nombre = nombre;
            Codigo = codigo;
            CamposEntrada = camposEntrada;
            CamposSalida = camposSalida;
            IdxDescripcionError = idxDescripcionError;
            IdxError = idxError;
            IdxNroTributario = idxNroTributario;
        }
    }
}
