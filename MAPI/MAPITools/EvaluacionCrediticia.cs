// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Xml;

namespace MAPITools
{
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
                throw new MAPIException(evaluacion._Politica.Nombre, 1, errorModeler);
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

            string connStr = Herramientas.GetConnectionStringMEBI();

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

            var requestUri = Herramientas.GetBaseURLMEBI() + "/ces_ws_generic_pac/ces_ws.asmx";
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
}
