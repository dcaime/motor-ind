using EvaluaIndividuos.Database.MEBI;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security;
using System.Text.Json;
using System.Xml;

namespace EvaluaIndividuos.Model
{
    public class EvaluacionCrediticia
    {
        private string[] _ArrayEntrada;
        private string[] _ArraySalida;
        private string _TramaSalida;
        private string _Servidor;
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

        public string TramaSalida
        {
            get
            {
                return _TramaSalida;
            }
        }

        public static async Task<EvaluacionCrediticia> EvaluarAsync(IEvaluable datosPolitica, Registro registro)
        {
            var evaluacion = new EvaluacionCrediticia();
            evaluacion._Politica = datosPolitica.GetPolitica();
            evaluacion._ArrayEntrada = datosPolitica.Codifica();
            var CodigoPolitica = "Politica" + evaluacion._Politica.Codigo.ToString();

            //Genera Trama Entrada
            string tramaEntrada = GenerarTrama(evaluacion._ArrayEntrada);
            
            //Invoca Modeler
            registro.RegistraInicio(evaluacion._Politica.Nombre, tramaEntrada);
            var t = await InvocarPoliticaOldAsync(CodigoPolitica, tramaEntrada);

            //Procesa respuesta
            evaluacion._TramaSalida = t.ToString();
            registro.RegistraFin(evaluacion._Politica.Nombre, evaluacion._TramaSalida);
            evaluacion._ArraySalida = evaluacion._TramaSalida.Split(';');


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
                    throw new MAPIException("Evaluacion Crediticia", 2, errorModeler, 504);
                }
                else
                {
                    throw new MAPIException("Evaluacion Crediticia", 3, errorModeler, 409);
                }

            }

            //Devuelve el objeto con toda la información de la Evaluacion
            return evaluacion;
        }

        public static async Task<EvaluacionCrediticia> EvaluarFullAsync(IEvaluable datosPolitica, Registro registro, Parametros parametros)
        {
            var evaluacion = new EvaluacionCrediticia();
            string param_servers = parametros.GetString("ServidoresAplicacion", "srvmebiap1-prod,srvmebiap2-prod,srvmebiap3-prod,srvmebiap4-prod");
            string[] servers = param_servers.Split(",");
            evaluacion._Servidor = servers[Random.Shared.Next(servers.Length)];
            if (Herramientas.GetEnvironment() ==  Ambiente.PREP) evaluacion._Servidor = "srvmebiap2-pre";
            if (Herramientas.EsAmbienteBajo()) evaluacion._Servidor = "f5-modelertest.macro.com.ar";
            evaluacion._Politica = datosPolitica.GetPolitica();
            evaluacion._ArrayEntrada = datosPolitica.Codifica();
            var CodigoPolitica = "Politica" + evaluacion._Politica.Codigo.ToString();
            string param_politicaActiva = parametros.GetString(evaluacion._Politica.Nombre + "_version_activa", evaluacion._Politica.Nombre);

            //Genera Trama Entrada
            string tramaEntrada = GenerarTrama(evaluacion._ArrayEntrada);

            //Invoca Modeler
            registro.RegistraInicio(evaluacion._Politica.Nombre, tramaEntrada);
            var t = await InvocarModelerAsync(param_politicaActiva, tramaEntrada, evaluacion._Servidor);

            //Procesa respuesta
            evaluacion._TramaSalida = t.ToString();
            registro.RegistraFin(evaluacion._Politica.Nombre, evaluacion._TramaSalida);
            evaluacion._ArraySalida = evaluacion._TramaSalida.Split(';');

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
                    throw new MAPIException("Evaluacion Crediticia", 2, errorModeler, 504);
                }
                else
                {
                    throw new MAPIException("Evaluacion Crediticia", 3, errorModeler, 409);
                }
            }

            //Devuelve el objeto con toda la información de la Evaluacion
            return evaluacion;
        }

        public void RegistrarEvaluacion(Guid Id, string ReferenciaCanal = "")
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

            string connStr = MEBIContext.GetConnectionString();

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
            catch (Exception exception)
            {
                conn.Close();
                throw new MAPIException(exception, "Error SQL MEBI", 5, 500);
            }
        }

        private static string GenerarTrama(string[] ArrayCampos)
        {
            string trama = string.Empty;
            string valorAux = string.Empty;
            for (int i = 0; i < ArrayCampos.Length - 1; i++)
            {
                valorAux = ArrayCampos[i];
                valorAux = valorAux.Replace(";", "");
                valorAux = valorAux.Replace("&", "");
                trama += valorAux + ";";
            }
            trama += ArrayCampos[ArrayCampos.Length - 1];
            return trama;
        }

        public static async Task<object> InvocarPoliticaOldAsync(string politica, string entrada)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            var requestUriSTR = MEBIContext.GetBaseURL() + "/ces_ws_generic_pac/ces_ws.asmx";
            var requestUri = new Uri(requestUriSTR);
            var soapStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                        <soap12:Body>
                        <EjecutarPolitica xmlns=""http://www.spss.com.ar/ces/ws"">
                        <strRegistro>" + entrada + @"</strRegistro>
                        <strMetodo>" + politica + @"</strMetodo>
                        </EjecutarPolitica>
                        </soap12:Body>
                        </soap12:Envelope>";
            var soapBytes = System.Text.Encoding.UTF8.GetBytes(soapStr);
            return await CallSoapServiceAsync(requestUri, soapBytes);
        }

        private static readonly HttpClient _httpClient = new(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });

        public static async Task<string> InvocarPoliticaAsync(string politica, string entrada)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            var requestUriSTR = MEBIContext.GetBaseURL() + "/legacy/ces_ws.asmx";
            var requestUri = new Uri(requestUriSTR);
            string safeEntrada = System.Security.SecurityElement.Escape(entrada);
            var soapStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                        <soap12:Body>
                        <EjecutarPolitica xmlns=""http://www.spss.com.ar/ces/ws"">
                        <strRegistro>" + safeEntrada + @"</strRegistro>
                        <strMetodo>" + politica + @"</strMetodo>
                        </EjecutarPolitica>
                        </soap12:Body>
                        </soap12:Envelope>";
        
            var soapBytes = System.Text.Encoding.UTF8.GetBytes(soapStr);
            return await CallSoapServiceAsync(requestUri, soapBytes);
        }

        public static async Task<string> CallSoapServiceAsync(Uri requestUri, byte[] soapBytes)
        {
            try
            {
                using var content = new ByteArrayContent(soapBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/soap+xml")
                {
                    CharSet = "utf-8"
                };
                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Content = content;
                request.Headers.AcceptEncoding.ParseAdd("zip, gzip, deflate, br");
                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var xmlResponse = await response.Content.ReadAsStringAsync();
                var doc = new XmlDocument();
                doc.LoadXml(xmlResponse);

                return doc.InnerText;
            }
            catch (HttpRequestException ex)
            {
                throw new MAPIException(ex, "ServicioEvaluacion", 12, (int?)ex.StatusCode ?? 500);
            }
            catch (Exception ex)
            {
                throw new MAPIException(ex, "ServicioEvaluacion", 12, 500);
            }
        }

        public static async Task<object> InvocarModelerAsync(string politica, string entrada, string servidor)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://"+ servidor+":50001/EjecutaRuta");
                var content = new StringContent("{\n  \"ruta\": \"" + politica + "\",\n  \"entrada\": \"" + entrada + "\"\n}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                RespuestaModeler r = response.Content.ReadFromJsonAsync<RespuestaModeler>(new JsonSerializerOptions()).Result;
                return r.resultado;
            }
            catch (Exception ex)
            {
                return new MAPIException("ServicioEvaluacion", 12, "Error en la ejecución de la política crediticia", 500);
            }
        }
    }
}
