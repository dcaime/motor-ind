using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Text.Json;
using System.Xml;

namespace EvaluaIndividuos.Database.MEBI
{
    public class MEBIContext : DbContext
    {
        public MEBIContext(DbContextOptions<MEBIContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BcraDeudores>().HasNoKey();
            modelBuilder.Entity<ListaNegra>().HasNoKey();
            modelBuilder.Entity<CampañasExceptuadas>().HasNoKey();
            modelBuilder.Entity<Precacom>().HasNoKey();
            modelBuilder.Entity<MoraSucursal>().HasNoKey();
            modelBuilder.Entity<Behavior>().HasNoKey();
            modelBuilder.Entity<Excepciones>().HasNoKey();
            modelBuilder.Entity<CacheMapi>();
            modelBuilder.Entity<DictamenesSiisa>();
            modelBuilder.Entity<ConveniosCodDescuento>().HasNoKey();
            modelBuilder.Entity<CalificacionConvenios>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }

        // Se registran las entidades
        public DbSet<DictamenesSiisa> DictamenesSiisa { get; set; } = default!;
        public DbSet<DictamenesIndividuos> DictamenesIndividuos { get; set; } = default!;
        public DbSet<DictamenesIndividuosXper> DictamenesIndividuosXper { get; set; } = default!;
        public DbSet<DictamenesRentaPresunta> DictamenesRentaPresunta { get; set; } = default!;
        public DbSet<DictamenesRentaPresuntaXper> DictamenesRentaPresuntaXper { get; set; } = default!;
        public DbSet<BcraDeudores> BcraDeudores { get; set; } = default!; 
        public DbSet<ListaNegra> ListaNegra { get; set; } = default!;
        public DbSet<CampañasExceptuadas> CampañasExceptuadas { get; set; } = default!;
        public DbSet<Precacom> Precacom { get; set; } = default!;
        public DbSet<MoraSucursal> MoraSucursal { get; set; } = default!;
        public DbSet<Behavior> Behavior { get; set; } = default!;
        public DbSet<Excepciones> Excepciones { get; set; } = default!;
        public DbSet<CacheMapi> CacheMapi { get; set; } = default!;
        public DbSet<ConveniosCodDescuento> ConveniosCodDescuentos { get; set; } = default!;
        public DbSet<CalificacionConvenios> CalificacionConvenios { get; set; } = default!;
        public static string GetConnectionString()
        {
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    return "Data Source=SRVMEBIDB-PROD,7427;Initial Catalog=CESDB;Integrated Security=SSPI;TrustServerCertificate=True;Connect Timeout=3;Command Timeout=5";
                case Ambiente.PREP:
                    return "Data Source=SRVMEBIDB-PRE,7427;Initial Catalog=CESDB;Integrated Security=SSPI;TrustServerCertificate=True;Connect Timeout=3;Command Timeout=5";
                default:
                    return "Data Source=SRVMEBIDB-TEST,7427;Initial Catalog=CESDB;Integrated Security=SSPI;TrustServerCertificate=True;Connect Timeout=3;Command Timeout=5";
            }
        }

        public static string GetBaseURL()
        {
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    return "https://srvmebiwb-prod.macro.com.ar";
                case Ambiente.PREP:
                    return "https://srvmebiwb-pre.macro.com.ar";
                default: // Testing y Desarrollo
                    return "https://srvmebiwb-test.macro.com.ar";
            }
        }
    }
}
