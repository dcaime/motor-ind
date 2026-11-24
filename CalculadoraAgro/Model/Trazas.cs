using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace CalculadoraAgro.Model
{
    public class Traza
    {
        public readonly Object Datos;
        public readonly DateTime FechaHora;

        public Traza(Object datos)
        {
            Datos = datos;
            FechaHora = DateTime.Now;
        }
    }

    public class Registro
    {
        private readonly Guid _Id;
        private readonly String _NumeroTributario;
        private readonly String _Canal;
        private readonly String _ReferenciaCanal;
        private Object _Error;
        private Dictionary<String, Traza> _TrazasInicio;
        private Dictionary<String, Traza> _TrazasFin;

        public Registro(String Servicio, Guid id, String numeroTributario, String canal, String referenciaCanal, Object Entrada)
        {
            _NumeroTributario = numeroTributario;
            _Canal = canal;
            _Id = id;
            _ReferenciaCanal = referenciaCanal;
            _TrazasInicio = new Dictionary<String, Traza>();
            _TrazasInicio.Add(Servicio, new Traza(Entrada));
            _TrazasFin = new Dictionary<String, Traza>();
        }

        public void RegistraInicio(String servicio, Object entrada)
        {
            _TrazasInicio.Add(servicio, new Traza(entrada));
        }

        public void RegistraFin(String servicio, Object salida)
        {
            _TrazasFin.Add(servicio, new Traza(salida));
        }

        public void InsertaEnBD()
        {
            InsertaCabecera();
            InsertaTrazas();
        }

        private void InsertaCabecera()
        {
            string query = @"
            INSERT INTO [dbo].[Transacciones]
           ([Id]
           ,[NumeroTributario]
           ,[Canal]
           ,[ReferenciaCanal]
           ,[Error])
     VALUES
           (@id
           ,@numeroTributario
           ,@canal
           ,@referenciaCanal
           ,@error)";

            string connStr = MEBI.GetConnectionString();
            string? jsonError = System.Text.Json.JsonSerializer.Serialize(_Error);
            jsonError = Herramientas.TruncarString(jsonError, 100);

            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@id", _Id);
            comm.Parameters.AddWithValue("@numeroTributario", _NumeroTributario);
            comm.Parameters.AddWithValue("@canal", ((object)_Canal) ?? DBNull.Value);
            comm.Parameters.AddWithValue("@referenciaCanal", _ReferenciaCanal);
            comm.Parameters.AddWithValue("@error", jsonError);

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

        private void InsertaTrazas()
        {
            if (_TrazasInicio.Count == 0) { return; }

            using (SqlConnection oConnection = new SqlConnection(MEBI.GetConnectionString()))
            {
                oConnection.Open();
                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    using (SqlCommand oCommand = oConnection.CreateCommand())
                    {
                        oCommand.Transaction = oTransaction;
                        oCommand.CommandType = CommandType.Text;
                        oCommand.CommandText = "INSERT INTO [Trazas] ([Id], [Servicio], [Inicio], [Fin], [Entrada], [Salida]) VALUES (@id, @servicio, @inicio, @fin, @entrada, @salida);";
                        oCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
                        oCommand.Parameters.Add(new SqlParameter("@servicio", SqlDbType.NVarChar));
                        oCommand.Parameters.Add(new SqlParameter("@inicio", SqlDbType.DateTime2));
                        oCommand.Parameters.Add(new SqlParameter("@fin", SqlDbType.DateTime2));
                        oCommand.Parameters.Add(new SqlParameter("@entrada", SqlDbType.NVarChar));
                        oCommand.Parameters.Add(new SqlParameter("@salida", SqlDbType.NVarChar));
                        try
                        {
                            Traza TrazaInicio;
                            Traza TrazaFin;
                            foreach (String servicio in _TrazasInicio.Keys)
                            {
                                oCommand.Parameters[0].Value = _Id;
                                oCommand.Parameters[1].Value = servicio;
                                TrazaInicio = _TrazasInicio.GetValueOrDefault(servicio);
                                oCommand.Parameters[2].Value = TrazaInicio.FechaHora;
                                oCommand.Parameters[4].Value = JsonSerializer.Serialize(TrazaInicio.Datos);
                                if (_TrazasFin.TryGetValue(servicio, out TrazaFin))
                                {
                                    oCommand.Parameters[3].Value = TrazaFin.FechaHora;
                                    oCommand.Parameters[5].Value = JsonSerializer.Serialize(TrazaFin.Datos);
                                }
                                else
                                {
                                    oCommand.Parameters[3].Value = DBNull.Value;
                                    oCommand.Parameters[5].Value = DBNull.Value;
                                }


                                if (oCommand.ExecuteNonQuery() != 1)
                                {
                                    throw new InvalidProgramException();
                                }
                            }
                            oTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            oTransaction.Rollback();
                            oConnection.Close();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
