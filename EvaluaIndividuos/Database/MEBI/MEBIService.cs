using Azure.Core;
using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Serilog;

namespace EvaluaIndividuos.Database.MEBI
{
    public interface IMEBIService
    {
        Task<IOrigen> GetListaNegraAsync(Integrantes integrantes, string? solicitaTarjeta);
        Task<IOrigen> GetPrecacomAsync(Integrantes integrantes, int origenId, int comercioId);
        Task<IOrigen> GetCampañasExceptuadasAsync(string origenId, string comercioId);
        Task<IOrigen> GetMoraSucursalAsync(long sucursalSource);
        Task<IOrigen> GetBehaviorAsync(Integrantes integrantes);
        Task<IOrigen> GetExcepcionesAsync(Integrantes integrantes);
        Task<Dictionary<string, CacheMapi?>> GetCacheMapiAsync(long[] clavess, string[] servicio);
        Task<IOrigen> GetConveniosCodDescuentoAsync(int numeroSubmercado);
        Task<IOrigen> GetCalificacionConveniosAsync(string numeroConvenio);

        void InsertDictamenes(IDictaminable dictaminable);
    }

    public class MEBIService : IMEBIService
    {
        private readonly MEBIFactory _mebiFactory;
        private readonly Parametros _parametros;

        public MEBIService(MEBIFactory mebiFactory, Parametros parametros)
        {
            _mebiFactory = mebiFactory;
            _parametros = parametros;
        }

        public async Task<IOrigen> GetListaNegraAsync(Integrantes integrantes, string? solicitaTarjeta)
        {
            string paramCodigos;
            solicitaTarjeta ??= "NO";

            if (solicitaTarjeta.Equals("SI", StringComparison.OrdinalIgnoreCase))
            {
                paramCodigos = _parametros.GetString("codigosLN_TC", "3, 8, 9, 10, 14, 15, 16, 17, 18, 24, 26, 27, 28, 29, 33, 34, 35, 36, 37, 41, 43, 45, 46, 47, 50, 51, 52, 53, 57, 60, 61, 62, 64, 65, 66, 67, 68, 69, 71, 72, 73, 74, 75, 76, 77, 80, 81, 82, 83, 84, 85, 86, 87, 88, 90, 91, 92, 95, 96, 122, 123, 124, 125");
            }
            else
            {
                paramCodigos = _parametros.GetString("codigosLN_PP", "3, 8, 9, 10, 14, 15, 16, 17, 18, 24, 26, 27, 28, 29, 31, 32, 33, 34, 35, 36, 37, 41, 43, 45, 46, 47, 49, 50, 51, 52, 53, 57, 60, 61, 62, 64, 65, 66, 67, 68, 69, 71, 72, 73, 74, 75, 76, 77, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 95, 96, 122, 123, 124, 125");
            }

            var codigosLnStr = paramCodigos.Split(",", StringSplitOptions.TrimEntries);
            var codigosLn = new List<int>();
            bool errorCodigos = false;

            foreach (var item in codigosLnStr)
            {
                if (int.TryParse(item, out int codigo))
                {
                    codigosLn.Add(codigo);
                }
                else
                {
                    errorCodigos = true;
                }
            }

            if (errorCodigos)
            {
                Log.Error("Los codigos de lista negra cargados en BD no tienen el formato correcto");
            }

            var numerosTributarios = integrantes.ListaNumerosTributarios.Select(n => (decimal)n).ToList();
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();

            var registros = await db.ListaNegra
                .AsNoTracking()
                .Where(ln => numerosTributarios.Contains(ln.NroDocTributario) &&
                                codigosLn.Contains(ln.MotivoObservacionKey))
                .GroupBy(ln => ln.NroDocTributario)
                .Select(grupo => new ListaNegra
                {
                    NroDocTributario = grupo.Key,
                    MotivoObservacion = grupo.Select(x => x.MotivoObservacion).FirstOrDefault()
                })
                .ToListAsync();

            var registrosMap = registros.ToDictionary(h => (long)h.NroDocTributario);

            return new Origen<long, ListaNegra>(Origenes.ListaNegra, registrosMap);
        }

        public async Task<IOrigen> GetPrecacomAsync(Integrantes integrantes, int origenId, int comercioId)
        {
            var registrosMap = new Dictionary<long, Precacom>();

            long cuil = integrantes.Titular?.NumeroTributario ?? 0;

            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();

            var cuilParam = new Microsoft.Data.SqlClient.SqlParameter("@cuil", System.Data.SqlDbType.BigInt) { Value = cuil };
            var origenIdParam = new Microsoft.Data.SqlClient.SqlParameter("@origenId", System.Data.SqlDbType.BigInt) { Value = origenId };
            var comercioIdParam = new Microsoft.Data.SqlClient.SqlParameter("@comercioId", System.Data.SqlDbType.BigInt) { Value = comercioId };

            string query = @"
                SELECT TOP 1 
                    P.PRECACOM_CODIGO, 
                    P.PRECACOM_CUIL, 
                    P.PRECACOM_ORIGEN_ID, 
                    P.PRECACOM_COMERCIO_ID, 
                    P.PRECACOM_ID_ALTERNATIVO, 
                    P.PRECACOM_PRESTAMO_MONTO_MAXIMO, 
                    P.PRECACOM_PRESTAMO_PLAZO_MAXIMO, 
                    P.PRECACOM_PRESTAMO_TNA, 
                    P.PRECACOM_PRESTAMO_TIPO_TASA, 
                    P.PRECACOM_PRESTAMO_CUOTA_MAXIMO, 
                    P.PRECACOM_TARJETA_CREDITO_LIMITE, 
                    P.PRECACOM_TARJETA_CUENTA_CORRIENTE_LIMITE, 
                    P.PRECACOM_VIGENTE_DESDE, 
                    P.PRECACOM_VIGENTE_HASTA, 
                    P.PRECACOM_VIGENCIA, 
                    P.PRECACOM_COM1, 
                    P.PRECACOM_COM2, 
                    P.PRECACOM_COM3,
                    CE.CAMPEXCEP_Codigo_campana,
                    CE.CAMPEXCEP_FECHA_FIN_PREVISTO,
                    CE.CAMPEXCEP_ESTADO,
                    CE.CAMPEXCEP_BCRA_EXCEPTUADO,
                    CE.CAMPEXCEP_CTRL_ACTIVIDAD_EXCEPTUADO,
                    CE.CAMPEXCEP_SUCURSAL_EXCLUSIVA
                FROM dbo.PRECACOM P WITH (NOLOCK)
                LEFT JOIN dbo.BD_CAMP_EXCEPTUADAS CE WITH (NOLOCK) 
                    ON CE.CAMPEXCEP_Codigo_campana = P.PRECACOM_CODIGO
                WHERE (P.PRECACOM_CUIL = @cuil OR (P.PRECACOM_Cuil = 0 AND P.PRECACOM_ID_ALTERNATIVO = 0)) 
                  AND (P.PRECACOM_ORIGEN_ID = @origenId OR P.PRECACOM_ORIGEN_ID = 0) 
                  AND (P.PRECACOM_COMERCIO_ID = @comercioId OR P.PRECACOM_COMERCIO_ID = 0) 
                  AND P.PRECACOM_VIGENTE_DESDE <= GETDATE() 
                  AND (
                        (P.PRECACOM_VIGENCIA > 0 AND GETDATE() <= DATEADD(day, P.PRECACOM_VIGENCIA, P.PRECACOM_VIGENTE_DESDE))
                        OR 
                        (P.PRECACOM_VIGENCIA = 0 AND CAST(GETDATE() AS date) <= P.PRECACOM_VIGENTE_HASTA)
                      )
                ORDER BY 
                    P.PRECACOM_Cuil DESC, 
                    P.PRECACOM_COMERCIO_ID DESC, 
                    P.PRECACOM_ORIGEN_ID DESC, 
                    P.PRECACOM_FECHA_CARGA DESC";

            var registros = await db.Set<Precacom>()
                .FromSqlRaw(query, cuilParam, origenIdParam, comercioIdParam)
                .AsNoTracking()
                .ToListAsync();

            if (registros.Any()) registrosMap.Add(cuil, registros.First());
            return new Origen<long, Precacom>(Origenes.Precacom, registrosMap);
        }

        public async Task<IOrigen> GetCampañasExceptuadasAsync(string origenId, string comercioId)
        {
            CampañasExceptuadas instancia = new();
            string precaAConsultar = string.Empty;
            string keyEspecifica = $"OFERTA ORIGEN={origenId} COMERCIO={comercioId}";
            string keyGeneral = $"OFERTA ORIGEN={origenId}";
            var registrosMap = new Dictionary<string, CampañasExceptuadas>();

            if (_parametros.TryGetValue(keyEspecifica, out string? valorEspecifico) && !string.IsNullOrEmpty(valorEspecifico))
            {
                precaAConsultar = valorEspecifico;
            }
            else if (_parametros.TryGetValue(keyGeneral, out string? valorGeneral) && !string.IsNullOrEmpty(valorGeneral))
            {
                precaAConsultar = valorGeneral;
            }

            if (string.IsNullOrEmpty(precaAConsultar))
            {
                return new Origen<string, CampañasExceptuadas>(Origenes.CampañasExceptuadas, registrosMap);
            }

            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.CampañasExceptuadas
                .AsNoTracking()
                .Where(r => precaAConsultar.Equals(r.CodigoCampana))
                .ToListAsync();
            registrosMap = registros.ToDictionary(c => c.CodigoCampana ?? string.Empty);
            return new Origen<string, CampañasExceptuadas>(Origenes.CampañasExceptuadas, registrosMap);
        }

        public async Task<IOrigen> GetMoraSucursalAsync(long sucursalSource)
        {
            var registrosMap = new Dictionary<long, MoraSucursal>();
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.MoraSucursal
                .AsNoTracking()
                .Where(r => sucursalSource.Equals(r.SucursalSource))
                .ToListAsync();
            registrosMap = registros.ToDictionary(c => c.SucursalSource);
            return new Origen<long, MoraSucursal>(Origenes.MoraSucursal, registrosMap);
        }

        public async Task<IOrigen> GetBehaviorAsync(Integrantes integrantes)
        {
            var registrosMap = new Dictionary<int, Behavior>();
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var listaCobis = integrantes.ListaCobis.Select(cobis => (int?)cobis).ToList();
            var registros = await db.Behavior
                .AsNoTracking()
                .Where(r => listaCobis.Contains(r.ClienteCobis))
                .ToListAsync();
            registrosMap = registros.ToDictionary(c => c.ClienteCobis ?? -1);
            return new Origen<int, Behavior>(Origenes.Behavior, registrosMap);
        }
        public async Task<IOrigen> GetExcepcionesAsync(Integrantes integrantes)
        {
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var cuilsString = integrantes.ListaNumerosTributarios.Select(n => n.ToString()).ToList();
            var currentDate = DateTime.Today;
            var registros = await db.Excepciones
                .AsNoTracking()
                .Where(r => cuilsString.Contains(r.Documento) && r.FechaVencimiento >= currentDate)
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.Documento);
            return new Origen<string, Excepciones>(Origenes.Excepciones, registrosMap);
        }

        public async Task<Dictionary<string, CacheMapi?>> GetCacheMapiAsync(long[] claves, string[] servicios)
        {
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.CacheMapi
                .AsNoTracking()
                .Where(c => claves.Contains(c.Clave) && servicios.Contains(c.Servicio))
                .GroupBy(c => new { c.Clave, c.Servicio })
                .Select(g => g.OrderByDescending(c => c.FechaHora).FirstOrDefault())
                .ToListAsync();

            return registros.ToDictionary(c => c.Clave + c.Servicio);
        }

        public async Task<IOrigen> GetConveniosCodDescuentoAsync(int numeroSubmercado)
        {
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.ConveniosCodDescuentos
                .AsNoTracking()
                .Where(r => numeroSubmercado.Equals(r.NumeroSubmercado))
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.NumeroSubmercado ?? -1);
            return new Origen<int, ConveniosCodDescuento>(Origenes.ConveniosCodDescuento, registrosMap);
        }

        public async Task<IOrigen> GetCalificacionConveniosAsync(string numeroConvenio) 
        {
            using var db = await _mebiFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.CalificacionConvenios
                .AsNoTracking()
                .Where(r => numeroConvenio.Equals(r.NumeroConvenio))
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.NumeroConvenio ?? "");
            return new Origen<string, CalificacionConvenios>(Origenes.CalificacionConvenios, registrosMap);
        }

        public void InsertDictamenes(IDictaminable dictaminable)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    dictaminable.InsertaDictamenes(_mebiFactory);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error en la inserción de dictamenes");
                }
            });
        }
    }
}
