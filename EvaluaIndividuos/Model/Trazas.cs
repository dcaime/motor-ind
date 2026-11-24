//using System.Text.Json;
//using System.Security.Cryptography;
using Azure.Core;
using Carter.Request;
using Carter.Response;
using EvaluaIndividuos.Database.MEBI;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Serilog;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EvaluaIndividuos.Model
{
    public class Traza
    {
        public Object Datos;
        public readonly DateTime FechaHora;

        public Traza(Object datos)
        {
            Datos = datos;
            FechaHora = DateTime.Now;
        }

        public Traza(Object datos, DateTime fechaHora)
        {
            Datos = datos;
            FechaHora = fechaHora;
        }
    }

    public class Registro
    {
        private Guid _Id;
        private DateTime _Inicio;
        public string _NumeroTributario;
        private string _Servicio;
        public int? _Canal;
        public string _ReferenciaCanal;
        private bool _Error = false;
        private Dictionary<string, Traza> _TrazasInicio;
        private Dictionary<string, Traza> _TrazasFin;
        private bool _EsEncadenado = false;

        public Registro()
        {
            _Id = Guid.NewGuid();
            _TrazasInicio = new Dictionary<string, Traza>();
            _TrazasFin = new Dictionary<string, Traza>();
            _Inicio = DateTime.Now;
            Log.Information("MAPI V2.7.0 - Inicia " + Id.ToString());
        }

        public void Encadena(Guid id)
        {
            _Id = id;
            _EsEncadenado = true;
        }

        public Guid Id {  get { return _Id; } }

        public void Inicia(string servicio, Object request, string numeroTributario, int canal, string referenciaCanal = "")
        {
            _NumeroTributario = numeroTributario;
            _Canal = canal;
            _ReferenciaCanal = referenciaCanal;
            _Servicio = servicio;
            _TrazasInicio.Add(_Servicio, new Traza(request, _Inicio));
        }


        public void Inicia(HttpContext context, Object request, string numeroTributario, int? canal, string referenciaCanal)
        {
            _NumeroTributario = numeroTributario;
            _Canal = canal;
            _ReferenciaCanal = referenciaCanal;
            _Servicio = context.Request.Path.Value?.Remove(0, 1) ?? "";
            object entrada;

            if (context.Request.Method == "GET")
            {
                entrada = context.Request.QueryString.Value;
            }
            else
            {
                entrada = request;
            }
            _TrazasInicio.Add(_Servicio, new Traza(entrada, _Inicio));
        }

        public void Encadena(Guid id, string servicio, object entrada)
        {
            _Id = id;
            _EsEncadenado = true;
            _Servicio = servicio;
            _TrazasInicio.Add(_Servicio, new Traza(entrada, _Inicio));
        }

        public void RegistraInicio(String servicio, Object entrada)
        {
            _TrazasInicio.Add(servicio, new Traza(entrada));
        }

        public void RegistraFin(String servicio, Object salida)
        {
            _TrazasFin.Add(servicio, new Traza(salida));
        }

        public void RegistraError(Object error)
        {
            if (_Servicio == null || _Servicio == "") return;
            _Error = true; 
            Finaliza(error);
        }

        public void Finaliza()
        {
            Finaliza(null);
        }

        public void Finaliza(Object salida)
        {
            if (_Servicio == null || _Servicio == "") return;
            _TrazasFin.Add(_Servicio, new Traza(salida));
            new System.Threading.Tasks.Task(async () =>
            {
                try
                {
                    InsertaEnBD();
                }
                catch (Exception e) { }
            }).Start();
        }

        public string NumeroTributario
        {
            get
            {
                return _NumeroTributario;
            }
        }

        public void InsertaEnBD()
        {
            if (!_EsEncadenado) InsertaCabecera();
            InsertaTrazas();
        }

        private void InsertaCabecera()
        {
            string tabla = "Transacciones";
            string query = @"INSERT INTO [dbo].[" + tabla + "]([Id],[NumeroTributario],[Canal],[ReferenciaCanal],[Error],[Servicio]) VALUES (@id,@numeroTributario,@canal,@referenciaCanal,@error,@servicio)";
            string connStr = MEBIContext.GetConnectionString();
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@id", Id);
            comm.Parameters.AddWithValue("@numeroTributario", ((object)_NumeroTributario) ?? DBNull.Value);
            comm.Parameters.AddWithValue("@canal", ((object)_Canal) ?? DBNull.Value);
            comm.Parameters.AddWithValue("@referenciaCanal", ((object)_ReferenciaCanal) ?? DBNull.Value);
            comm.Parameters.AddWithValue("@error", _Error);
            comm.Parameters.AddWithValue("@servicio", _Servicio);

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

        private void InsertaTrazas()
        {
            if (_TrazasInicio.Count == 0) { return; }
            string tabla = "Trazas";
            using (SqlConnection oConnection = new SqlConnection(MEBIContext.GetConnectionString()))
            {
                oConnection.Open();
                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    using (SqlCommand oCommand = oConnection.CreateCommand())
                    {
                        oCommand.Transaction = oTransaction;
                        oCommand.CommandType = CommandType.Text;
                        oCommand.CommandText = "INSERT INTO [dbo].[" + tabla + "] ([Id], [Servicio], [Inicio], [Fin], [Entrada], [Salida]) VALUES (@id, @servicio, @inicio, @fin, @entrada, @salida);";
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
                                oCommand.Parameters[0].Value = Id;
                                oCommand.Parameters[1].Value = servicio;
                                TrazaInicio = _TrazasInicio.GetValueOrDefault(servicio);
                                oCommand.Parameters[2].Value = TrazaInicio.FechaHora;
                                oCommand.Parameters[4].Value = JsonConvert.SerializeObject(TrazaInicio.Datos, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                //oCommand.Parameters[4].Value = JsonSerializer.Serialize(TrazaInicio.Datos);
                                if (_TrazasFin.TryGetValue(servicio, out TrazaFin))
                                {
                                    oCommand.Parameters[3].Value = TrazaFin.FechaHora;
                                    //oCommand.Parameters[5].Value = JsonSerializer.Serialize(TrazaFin.Datos);
                                    oCommand.Parameters[5].Value = JsonConvert.SerializeObject(TrazaFin.Datos, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

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
                        catch (Exception exception)
                        {
                            oTransaction.Rollback();
                            oConnection.Close();
                            //throw new MAPIException(exception, "Error SQL MEBI", 5, 500);
                        }
                    }
                }
            }
        }
    }
}
