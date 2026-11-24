//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.

//namespace MAPITools
//{
//    using System;
//    using System.Data;
//    using System.Data.SqlClient;
//    using System.Globalization;
//    using System.Text;
//    using NLog;

//    public static class Herramientas
//    {
//        public static Logger logger = LogManager.GetCurrentClassLogger();
//        public static void loguear(string mensaje)
//        {
//            logger.Info(mensaje);
//        }

//        private static string GetConnectionString(string name)
//        {
//            switch (name)
//            {
//                case "MEBA":
//                    switch (GetEnvironment())
//                    {
//                        case Ambiente.PROD:
//                            //return System.Configuration.ConfigurationManager.ConnectionStrings["MEBA_PROD"].ConnectionString;
//                            return "Data Source=SRVMEBADB-PROD,7427;Initial Catalog=MEBADB;Integrated Security=SSPI";
//                        default:
//                            return "Data Source=SRVMEBADB-PROD,7427;Initial Catalog=MEBADB;Integrated Security=SSPI";
//                    }
//                case "MEBI":
//                    switch (GetEnvironment())
//                    {
//                        case Ambiente.PROD:
//                            return "Data Source=SRVMEBIDB-PROD,7427;Initial Catalog=CESDB;Integrated Security=SSPI";
//                        case Ambiente.PREP:
//                            return "Data Source=SRVMEBIDB-PRE,7427;Initial Catalog=CESDB;Integrated Security=SSPI";
//                        default:
//                            return "Data Source=SRVMEBIDB-TEST,7427;Initial Catalog=CESDB;Integrated Security=SSPI";
//                    }
//                default:
//                    throw new Exception("Error al obtener el string de conexión al aplicativo " + name);
//            }
//        }

//        public static string GetConnectionStringMEBA()
//        {
//            return GetConnectionString("MEBA");
//        }

//        public static string GetConnectionStringMEBI()
//        {
//            return GetConnectionString("MEBI");
//        }

//        public static Ambiente GetEnvironment()
//        {
//            switch (Environment.MachineName)
//            {
//                case "SRVMEBIWB-TEST":
//                    return Ambiente.TEST;
//                case "SRVMEBIWB-PRE":
//                    return Ambiente.PREP;
//                case "SRVMEBIWB-PROD":
//                    return Ambiente.PROD;
//                default:
//                    return Ambiente.DESA;
//            }
//        }

//        public static string GetApplicationId()
//        {
//            switch (GetEnvironment())
//            {
//                case Ambiente.PROD:
//                    return "Q049R0dfTUVCSV9FU0JQUk9ELE9VPVBST0QsT1U9QXBsaWNhY2lvbmVzLE9VPUVTQixEQz1NQUNSTyxEQz1DT00sREM9QVI";
//                case Ambiente.PREP:
//                    return "Q049R0dfTUVCSV9FU0JQUkVQLE9VPVBSRVAsT1U9QXBsaWNhY2lvbmVzLE9VPUVTQixEQz1NQUNSTyxEQz1DT00sREM9QVI";
//                default:
//                    return "Q049R0dfTUVCSV9FU0JURVNULE9VPVRFU1QsT1U9QXBsaWNhY2lvbmVzLE9VPUVTQixEQz1NQUNSTyxEQz1DT00sREM9QVI";
//            }
//        }

//        public static string GetTokenESB()
//        {
//            if (GetEnvironment() == Ambiente.DESA)
//            {
//                var username = "usrmebi_test@macro.com.ar";
//                var password = "MEbitest68";
//                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
//                                               .GetBytes(username + ":" + password));
//                HttpClientHandler clientHandler = new HttpClientHandler();
//                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
//                var client = new HttpClient(clientHandler);
//                var request = new HttpRequestMessage(HttpMethod.Get, "https://esbtest.macro.com.ar/security");
//                request.Headers.Add("Authorization", "Basic " + encoded);
//                var response = client.Send(request);
//                return response.Headers.GetValues("X-Authorization-token").First();
//            }
//            else
//            {
//                string query = @"SELECT VALOR_VARCHAR FROM PARAMETROS_MOTOR WHERE VAR_ID = @VAR_ID";
//                string connStr = Herramientas.GetConnectionStringMEBI();
//                using SqlConnection conn = new SqlConnection(connStr);
//                using SqlCommand comm = new SqlCommand(query, conn);
//                comm.CommandType = CommandType.Text;
//                comm.Parameters.AddWithValue("@VAR_ID", "TokenESB");

//                try
//                {
//                    conn.Open();
//                    using (SqlDataReader reader = comm.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            return (string)reader["VALOR_VARCHAR"];
//                        }
//                    }
//                }
//                catch (SqlException)
//                {
//                    conn.Close();
//                    throw;
//                }
//            }
//            return null;
//        }

//        public static string ConvertirATexto(object o, string valorDefault = "", string formato = "")
//        {
//            if (o == null)
//            {
//                return valorDefault;
//            }
//            else if (o is DateTime)
//            {
//                var fechaAux = Convert.ToDateTime(o);
//                if (fechaAux < new DateTime(1900, 1, 1)) return "";
//                if (string.IsNullOrEmpty(formato)) formato = "yyyyMMdd";
//                return fechaAux.ToString(formato);
//            }
//            else if (o is Decimal)
//            {
//                decimal valor = (decimal)o;
//                return valor.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." });
//            }
//            else
//            {
//                return o.ToString();
//            }
//        }

//        public static object ConvertirDeTexto(string texto, DbType tipo, object valorDefault = null, string formato = "")
//        {
//            if (string.IsNullOrEmpty(texto)) return valorDefault;

//            switch (tipo)
//            {
//                case DbType.Decimal:
//                    return Convert.ToDecimal(texto);
//                case DbType.Int64:
//                    return Convert.ToInt64(texto);
//                case DbType.Int32:
//                    return Convert.ToInt32(texto);
//                case DbType.DateTime:
//                    return DateTime.ParseExact(texto, formato, System.Globalization.CultureInfo.InvariantCulture);
//                case DbType.Date:
//                    return DateOnly.ParseExact(texto, formato, System.Globalization.CultureInfo.InvariantCulture);
//                default:
//                    return texto;
//            }
//        }

//        public static string GetBaseURLMEBI()
//        {
//            switch (GetEnvironment())
//            {
//                case Ambiente.PROD:
//                    return "https://srvmebiwb-prod.macro.com.ar";
//                case Ambiente.PREP:
//                    return "https://srvmebiwb-pre.macro.com.ar";
//                default: // Testing y Desarrollo
//                    return "https://srvmebiwb-test.macro.com.ar";
//            }
//        }

//        public static string? TruncarString(string? textoATruncar, int longitud)
//        {
//            if (textoATruncar is null)
//            {
//                return null;
//            }
//            if (textoATruncar.Length > longitud)
//            {
//                return textoATruncar.Substring(0, longitud);
//            }
//            else
//            {
//                return textoATruncar;
//            }
//        }
//    }

//    public enum Ambiente
//    {
//        DESA = 0,
//        TEST = 1,
//        PREP = 2,
//        PROD = 3
//    }
//}
