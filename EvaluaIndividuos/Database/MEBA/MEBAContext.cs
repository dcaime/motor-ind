using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Globalization;

namespace EvaluaIndividuos.Database.MEBA
{
    public class MEBAContext : DbContext
    {
        public DbSet<ClientesDepurados> ClientesDepurados { get; set; } = default!;
        public DbSet<Clientes> Clientes { get; set; } = default!;
        public DbSet<LkpClientesCompleta> LkpClientesCompleta { get; set; } = default!;
        public DbSet<ValidacionActividad> ValidacionActividad { get; set; } = default!;
        public DbSet<TablonIndividuos> TablonIndividuos { get; set; } = default!;
        public DbSet<EndeudamientoAtraso> EndeudamientoAtraso { get; set; } = default!;
        public DbSet<LimiteTransitorio> LimiteTransitorio { get; set; } = default!;
        public DbSet<ParametrosMotor> Parametros { get; set; } = default!;
        public DbSet<LkpIrregularidadesBcra> LkpIrregularidadesBcra { get; set; } = default!;
        public DbSet<CalificacionConstante> CalificacionConstante { get; set; } = default!;
        public DbSet<ChequesRechazados> ChequesRechazados { get; set; } = default!;
        public DbSet<ScoreComportamiento> ScoreComportamientos { get; set; } = default!;
        public DbSet<LkpConvenios> LkpConvenios { get; set; } = default!;
        public DbSet<Tablon> Tablon { get; set; } = default!;
        public DbSet<CpAnalisis> CpAnalisis { get; set; } = default!;
        public DbSet<CpAuxiliar> CpAuxiliar { get; set; } = default!;
        public MEBAContext(DbContextOptions<MEBAContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>().HasNoKey();
            modelBuilder.Entity<LkpClientesCompleta>().HasNoKey();
            modelBuilder.Entity<TablonIndividuos>().HasNoKey();
            modelBuilder.Entity<ClientesDepurados>().HasNoKey();
            modelBuilder.Entity<ValidacionActividad>().HasNoKey();
            modelBuilder.Entity<EndeudamientoAtraso>().HasNoKey();
            modelBuilder.Entity<LimiteTransitorio>().HasNoKey();
            modelBuilder.Entity<ParametrosMotor>().HasNoKey();
            modelBuilder.Entity<LkpIrregularidadesBcra>().HasNoKey();
            modelBuilder.Entity<ChequesRechazados>().HasNoKey();
            modelBuilder.Entity<ScoreComportamiento>().HasNoKey();
            modelBuilder.Entity<Tablon>().HasNoKey();
            modelBuilder.Entity<CpAnalisis>().HasNoKey();
            modelBuilder.Entity<CpAuxiliar>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }

        public static string GetConnectionString()
        {
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    //return System.Configuration.ConfigurationManager.ConnectionStrings["MEBA_PROD"].ConnectionString;
                    return "Data Source=SRVMEBADB-PROD,7427;Initial Catalog=MEBADB;Integrated Security=SSPI;TrustServerCertificate=True;Connect Timeout=3;Command Timeout=5";
                default:
                    return "Data Source=SRVMEBADB-TEST,7427;Initial Catalog=MEBADB;Integrated Security=SSPI;TrustServerCertificate=True;Connect Timeout=3;Command Timeout=5";
            }
        }

        public static string GetFechaCabalope()
        {
            string query = @"SELECT MENSAJE FROM MEBADB.dbo.MENSAJES WHERE TIPO = @TIPO AND ID_EJECUCION = (SELECT ID_EJECUCION FROM MEBADB.dbo.PROCESOS_VIGENTES WHERE PROCESO = @PROCESO)";
            string connStr = GetConnectionString();
            DateTime fechaActual = DateTime.Today;
            using SqlConnection conn = new SqlConnection(connStr);
            using SqlCommand comm = new SqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@TIPO", "Fecha_CABALOPE");
            comm.Parameters.AddWithValue("@PROCESO", "Endeudamiento");

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var fechaActualString = (string)reader["MENSAJE"];

                        Log.Information("Fecha CABALOPE de MEBA: " + fechaActualString);
                        try
                        {
                            fechaActual = DateTime.ParseExact(fechaActualString, "yyyyMMdd", CultureInfo.InvariantCulture);
                            fechaActual = fechaActual.AddDays(1);
                        }
                        catch (Exception)
                        {
                            throw new MAPIException("Endeudamiento", 10, "Error al convertir a Fecha el valor " + fechaActualString);
                        }
                    }
                    else
                    {
                        throw new MAPIException("Endeudamiento", 11, "No existe el parametro FechaCABALOPE");
                    }
                }
            }
            catch (Exception exception) when (exception is not MAPIException)
            {
                conn.Close();
                throw new MAPIException(exception, "Error SQL MEBA", 4, 500);

            }
            return fechaActual.ToString("yyyy-MM-dd");
        }
    }
}
