using EvaluaIndividuos.Database.MEBA;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;

namespace EvaluaIndividuos.Model
{
    public class Parametros : Dictionary<string, string>
    {
        //public Parametros()
        //{
        //    foreach (var p in GetParametros())
        //    {
        //        this.Add(p.Key, p.Value);
        //    }
        //}

        public Parametros(Dictionary<string, string> data) : base(data)
        {
        }

        public decimal GetDecimal(string key, decimal defaultValue = 0M)
        {
            if (this.TryGetValue(key, out var value) && decimal.TryParse(value, out var result))
            {
                return result;
            }

            Log.Warning("Parameter {Key} missing or invalid. Using default: {Default}", key, defaultValue);
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (this.TryGetValue(key, out var value) && int.TryParse(value, out var result))
            {
                return result;
            }

            Log.Warning("Parameter {Key} missing or invalid. Using default: {Default}", key, defaultValue);
            return defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (this.TryGetValue(key, out var value))
            {
                return value;
            }

            Log.Warning("Parameter {Key} missing or invalid. Using default: {Default}", key, defaultValue);
            return defaultValue;
        }

        private Dictionary<string, string> GetParametros(string grupo = "-MAPI-")
        {
            string query = @"SELECT VAR_ID, VALOR_VARCHAR FROM MEBADB.dbo.PARAMETROS_MOTOR WHERE GRUPO LIKE @GRUPO";
            string connStr = MEBAContext.GetConnectionString();
            var listaParametros = new Dictionary<string, string>();
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@GRUPO", "%" + grupo + "%");

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaParametros.Add((string)reader["VAR_ID"], (string)reader["VALOR_VARCHAR"]);
                    }
                }
            }
            catch (Exception exception)
            {
                conn.Close();
                throw new MAPIException(exception, "Error SQL MEBA", 4, 500);
            }
            return listaParametros;
        }
    }
}
