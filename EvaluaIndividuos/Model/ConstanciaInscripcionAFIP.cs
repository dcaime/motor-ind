//using Clientes_ConsultaConstanciaInscripcionAFIP;
using System.Data;
using Microsoft.Data.SqlClient;
using EvaluaIndividuos.Database.MEBI;

namespace EvaluaIndividuos.Model
{
    public static class ConstanciaInscripcionAFIP
    {
        public static async Task<ConsultaConstanciaInscripcionAFIPResponseType> GetConstancia(String NumeroTributario)
        {
            String cacheInscripcion = "";
            if (Herramientas.GetEnvironment() == Ambiente.DESA || Herramientas.GetEnvironment() == Ambiente.TEST)
            {
                string query = @"SELECT TOP(1) Z.Salida FROM Trazas AS Z
                                    INNER JOIN Transacciones AS T ON T.Id = Z.Id
                                    AND Z.Servicio = @servicio
                                    AND T.NumeroTributario = @numeroTributario
                                    ORDER BY T.FechaHora DESC";

                string connStr = MEBIContext.GetConnectionString();
                using SqlConnection conn = new SqlConnection(connStr);
                using SqlCommand comm = new SqlCommand(query, conn);
                comm.CommandType = CommandType.Text;
                comm.Parameters.AddWithValue("@numeroTributario", NumeroTributario);
                comm.Parameters.AddWithValue("@servicio", "Riesgos_ValidaCodigoPostal");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cacheInscripcion = (string)reader["Salida"];
                        }
                    }
                }
                catch (Exception exception)
                {
                    conn.Close();
                    throw new MAPIException(exception, "Error SQL MEBI", 5, 500);
                }
            }

            if (cacheInscripcion != "")
            {
                return System.Text.Json.JsonSerializer.Deserialize<ConsultaConstanciaInscripcionAFIPResponseType>(cacheInscripcion);
            }

            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var client = new HttpClient(clientHandler);

                //Manual
                client.DefaultRequestHeaders.Add("X-Authorization-impersonationLevel", "service");
                client.DefaultRequestHeaders.Add("X-Authorization-token", Herramientas.GetTokenESB());
                client.DefaultRequestHeaders.Add("X-Application-identity", Herramientas.GetApplicationId());
                var BaseURL = ESB.GetBaseURL();
                var request = new HttpRequestMessage(HttpMethod.Get, BaseURL + "/services/Clientes_ConsultaConstanciaInscripcionAFIP/V1/R0?NumeroTributario=" + NumeroTributario);
                request.Headers.Add("Accept", "application/json");
                var response = client.Send(request);

                //response.EnsureSuccessStatusCode();

                //Como texto
                //var jsonString = await response.Content.ReadAsStringAsync();
                //var constancias = JsonConvert.DeserializeObject<ResponseESB>(jsonString);

                //Como JSON
                ConsultaConstanciaInscripcionAFIPResponseType ConstanciaInscripcionAFIP = null;
                if (response.IsSuccessStatusCode)
                {
                    var constancias = await response.Content.ReadFromJsonAsync<ConsultaConstanciaInscripcionAFIPResponseESB>();

                    if (constancias.Elements.Count > 0)
                    {
                        ConstanciaInscripcionAFIP = constancias.Elements.First();
                    }
                }
                else
                {
                    var errorESB = await response.Content.ReadFromJsonAsync<FaultResponse>();
                    var msjError = ESB.GetErrorMessage(errorESB);
                    if (msjError == "No existe persona con ese Id")
                    {
                        throw new MAPIException("ConsultaConstanciaInscripcionAFIP", 101, msjError, 404);
                    }
                    else
                    {
                        throw new MAPIException("ConsultaConstanciaInscripcionAFIP", 100, msjError, 502);
                    }
                }

                //BD y Retorno
                return ConstanciaInscripcionAFIP;
            }
            catch (Exception exception) when (exception is not MAPIException)
            {
                throw new MAPIException(exception, "ConsultaConstanciaInscripcionAFIP", 100, 502);
            }
        }

        //private static void GuardarCacheInscripcionAFIP(String Cuil, String Cache)
        //{
        //    string query = @"INSERT INTO [dbo].[Cache_Inscripcion_AFIP] ([cuil],[cache]) VALUES (@cuil,@cache)";
        //    string connStr = MEBI.GetConnectionString();
        //    using SqlConnection conn = new SqlConnection(connStr);
        //    using SqlCommand comm = new SqlCommand(query, conn);
        //    comm.CommandType = CommandType.Text;
        //    comm.Parameters.AddWithValue("@cuil", Cuil);
        //    comm.Parameters.AddWithValue("@cache", Cache);

        //    try
        //    {
        //        conn.Open();
        //        comm.ExecuteNonQuery();
        //    }
        //    catch (SqlException)
        //    {
        //        conn.Close();
        //        throw;
        //    }
        //}
    }

    public partial class ConsultaConstanciaInscripcionAFIPResponseESB
    {
        [Newtonsoft.Json.JsonProperty("elements", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<ConsultaConstanciaInscripcionAFIPResponseType> Elements { get; set; } = new List<ConsultaConstanciaInscripcionAFIPResponseType>();
    }
}
