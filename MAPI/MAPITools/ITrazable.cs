using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace MAPITools
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
        readonly Guid Id;
        readonly String NumeroTributario;
        readonly String Canal;
        readonly String ReferenciaCanal;
        Object Error;
        Dictionary<String, Traza> TrazasInicio;
        Dictionary<String, Traza> TrazasFin;

        public Registro(String Servicio, Guid id, String numeroTributario, String canal, String referenciaCanal, Object Entrada)
        {
            NumeroTributario = numeroTributario;
            Canal = canal;
            Id = id;
            ReferenciaCanal = referenciaCanal;
            TrazasInicio = new Dictionary<String, Traza>();
            TrazasInicio.Add(Servicio, new Traza(Entrada));
            TrazasFin = new Dictionary<String, Traza>();
        }

        public void RegistraInicio(String servicio, Object entrada)
        {
            TrazasInicio.Add(servicio, new Traza(entrada));
        }

        public void RegistraFin(String servicio, Object salida)
        {
            TrazasFin.Add(servicio, new Traza(salida));
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

            string connStr = Herramientas.GetConnectionStringMEBI();
            string? jsonError = System.Text.Json.JsonSerializer.Serialize(Error);
            jsonError = Herramientas.TruncarString(jsonError, 100);

            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@id", Id);
            comm.Parameters.AddWithValue("@numeroTributario", NumeroTributario);
            comm.Parameters.AddWithValue("@canal", ((object)Canal) ?? DBNull.Value);
            comm.Parameters.AddWithValue("@referenciaCanal", ReferenciaCanal);
            comm.Parameters.AddWithValue("@error", jsonError);

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

        private void InsertaTrazas()
        {
            if (TrazasInicio.Count == 0) { return; }

            using (SqlConnection oConnection = new SqlConnection(Herramientas.GetConnectionStringMEBI()))
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
                            foreach (String servicio in TrazasInicio.Keys)
                            {
                                oCommand.Parameters[0].Value = Id;
                                oCommand.Parameters[1].Value = servicio;
                                TrazaInicio = TrazasInicio.GetValueOrDefault(servicio);
                                oCommand.Parameters[2].Value = TrazaInicio.FechaHora;
                                oCommand.Parameters[4].Value = JsonSerializer.Serialize(TrazaInicio.Datos);
                                if (TrazasFin.TryGetValue(servicio, out TrazaFin))
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
                            throw;
                        }
                    }
                }
            }
        }
    }
}
