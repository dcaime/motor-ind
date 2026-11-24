using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EvaluaIndividuos.Model
{
    public static class Herramientas
    {
        //public static Logger logger = LogManager.GetCurrentClassLogger();
        //public static void loguear(string mensaje)
        //{
        //    logger.Debug(mensaje);
        //}

        public static Ambiente GetEnvironment()
        {
            switch (Environment.MachineName)
            {
                case "SRVMEBIWB-TEST":
                    return Ambiente.TEST;
                case "SRVMEBIWB-PRE":
                    return Ambiente.PREP;
                case "SRVMEBIWB-PROD":
                    return Ambiente.PROD;
                default:
                    return Ambiente.DESA;
            }
        }

        public static bool EsAmbienteBajo()
        {
            return GetEnvironment() == Ambiente.DESA || GetEnvironment() == Ambiente.TEST;
        }

        public static string GetApplicationId()
        {
            switch (GetEnvironment())
            {
                case Ambiente.PROD:
                    return "Q049R0dfTUVCSV9FU0JQUk9ELE9VPVBST0QsT1U9QXBsaWNhY2lvbmVzLE9VPUVTQixEQz1NQUNSTyxEQz1DT00sREM9QVI";
                case Ambiente.PREP:
                    return "Q049R0dfTUVCSV9FU0JQUkVQLE9VPVBSRVAsT1U9QXBsaWNhY2lvbmVzLE9VPUVTQixEQz1NQUNSTyxEQz1DT00sREM9QVI";
                default:
                    return "Q049R0dfTUVCSV9FU0JURVNULE9VPVRFU1QsT1U9QXBsaWNhY2lvbmVzLE9VPUVTQixEQz1NQUNSTyxEQz1DT00sREM9QVI";
            }
        }

        public static string GetTokenESB()
        {
            if (GetEnvironment() == Ambiente.DESA)
            {
                var username = "usrmebi_test@macro.com.ar";
                var password = "MEbitest68";
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                               .GetBytes(username + ":" + password));
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var client = new HttpClient(clientHandler);
                var request = new HttpRequestMessage(HttpMethod.Get, "https://esbtest.macro.com.ar/security");
                request.Headers.Add("Authorization", "Basic " + encoded);
                var response = client.Send(request);
                return response.Headers.GetValues("X-Authorization-token").First();
            }
            else
            {
                string query = @"SELECT VALOR_VARCHAR FROM PARAMETROS_MOTOR WHERE VAR_ID = @VAR_ID";
                string connStr = MEBIContext.GetConnectionString();
                using SqlConnection conn = new SqlConnection(connStr);
                using SqlCommand comm = new SqlCommand(query, conn);
                comm.CommandType = CommandType.Text;
                comm.Parameters.AddWithValue("@VAR_ID", "TokenESB");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (string)reader["VALOR_VARCHAR"];
                        }
                    }
                }
                catch (SqlException exception)
                {
                    conn.Close();
                    throw new MAPIException(exception, "Error SQL MEBI", 5, 500);
                }
            }
            return null;
        }

        public static string ConvertirATexto(object o, string valorDefault = "", string formato = "")
        {
            if (o == null)
            {
                return valorDefault;
            }
            else if (o is DateTime)
            {
                var fechaAux = Convert.ToDateTime(o);
                if (fechaAux < new DateTime(1900, 1, 1)) return "";
                if (string.IsNullOrEmpty(formato)) formato = "yyyyMMdd";
                return fechaAux.ToString(formato);
            }
            else if (o is DateOnly)
            {
                var dateOnly = (DateOnly)o;
                return dateOnly.ToString(formato);
            }
            else if (o is Decimal)
            {
                decimal valor = (decimal)o;
                return valor.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." });
            }
            else
            {
                return string.IsNullOrEmpty(o?.ToString()) ? valorDefault : o.ToString();
            }
        }

        public static object ConvertirDeTexto(string texto, DbType tipo, object valorDefault = null, string formato = "")
        {
            if (string.IsNullOrEmpty(texto)) return valorDefault;

            try
            {
                switch (tipo)
                {
                    case DbType.Decimal:
                        return Convert.ToDecimal(texto.Replace(".", ",")) / 1.000000000000000000000000000000000m;
                    case DbType.Double:
                        return Convert.ToDouble(texto.Replace(".", ","));
                    case DbType.Int64:
                        return Convert.ToInt64(Math.Round(Convert.ToDecimal(texto.Replace(".", ",")) / 1.000000000000000000000000000000000m, MidpointRounding.ToZero));
                    case DbType.Int32:
                        return Convert.ToInt32(Math.Round(Convert.ToDecimal(texto.Replace(".", ",")) / 1.000000000000000000000000000000000m, MidpointRounding.ToZero));
                    case DbType.Int16:
                        return Convert.ToInt16(Math.Round(Convert.ToDecimal(texto.Replace(".", ",")) / 1.000000000000000000000000000000000m, MidpointRounding.ToZero));
                    case DbType.Byte:
                        return Convert.ToByte(Math.Round(Convert.ToDecimal(texto.Replace(".", ",")) / 1.000000000000000000000000000000000m, MidpointRounding.ToZero));
                    case DbType.DateTime:
                        return DateTime.ParseExact(texto, formato, System.Globalization.CultureInfo.InvariantCulture);
                    case DbType.Date:
                        return DateOnly.ParseExact(texto, formato, System.Globalization.CultureInfo.InvariantCulture);
                    default:
                        return texto;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return valorDefault;
            }
        }

        public static string? TruncarString(string? textoATruncar, int longitud)
        {
            if (textoATruncar is null)
            {
                return null;
            }
            if (textoATruncar.Length > longitud)
            {
                return textoATruncar.Substring(0, longitud);
            }
            else
            {
                return textoATruncar;
            }
        }

        public static string GetCache(string servicio, string numeroTributario)
        {
            string salida = "";
            string query = @"SELECT TOP(1) Cache FROM CacheMAPI
                            WHERE Servicio = @servicio
                            AND NumeroTributario = @numeroTributario ORDER BY Id Desc";
            string connStr = MEBIContext.GetConnectionString();
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@numeroTributario", numeroTributario);
            comm.Parameters.AddWithValue("@servicio", servicio);

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        salida = (string)reader["Cache"];
                    }
                }
            }
            catch (Exception exception)
            {
                conn.Close();
                throw new MAPIException(exception, "Error SQL MEBI", 5, 500);
            }
            return salida;
        }

        public static void InsertaBD(string connStr, string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            if (parameters != null) foreach (SqlParameter p in parameters) comm.Parameters.Add(p);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                conn.Close();
                //throw new MAPIException(exception, "Error SQL MEBI", 5, 500);
            }
        }

        public static object[]? GetDatosBD(string connStr, string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            if (parameters != null) foreach (SqlParameter p in parameters) comm.Parameters.Add(p);

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object[] salida = new object[reader.FieldCount];
                        reader.GetValues(salida);
                        conn.Close();
                        return salida;
                    }
                }
            }
            catch (Exception exception)
            {
                conn.Close();
                throw new MAPIException(exception, "Error SQL MEBA", 4, 500);
            }
            return null;
        }

        public static string NumeroATexto(decimal d)
        {
            if (d % 1 == 0) return d.ToString("0", CultureInfo.InvariantCulture);
            return d.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }

    public enum Ambiente
    {
        DESA,
        TEST,
        PREP,
        PROD
    }
}
