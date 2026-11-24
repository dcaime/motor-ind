using Azure.Core;
using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Endpoints;
using EvaluaIndividuos.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EvaluaIndividuos.Database.MEBA
{
    public interface IMEBAService
    {
        Task<IOrigen> GetCalificacionConstanteAsync(Integrantes integrantes, string origenId, string comercioId);
        Task<IOrigen> GetClientesAsync(Integrantes integrantes);
        Task<IOrigen> GetClientesDepuradosAsync(Integrantes integrantes);
        Task<IOrigen> GetEndeudamientoAtrasoAsync(Integrantes integrantes);
        Task<IOrigen> GetLimiteTransitorioAsync(Integrantes integrantes);
        Task<IOrigen> GetLkpIrregularidadesBcraAsync(Integrantes integrantes);
        Task<IOrigen> GetTablonIndividuosAsync(Integrantes integrantes);
        Task<IOrigen> GetValidacionActividadAsync(Integrantes integrantes);
        Task<IOrigen> GetScoreComportamientoAsync(Integrantes integrantes);
        Task<Integrantes> GetIntegrantesPorCuilAsync(RequestEvaluaIndividuos request);
        Task<Integrantes> GetIntegrantesPorCobisAsync(int[] cobis);
        Task<IOrigen> GetChequesRechazadosAsync(Integrantes integrantes);
        Task<IOrigen> GetTablonAsync(Integrantes integrantes);
        Task<IOrigen> GetLkpConveniosAsync(int numeroConvenio);
        Task<IOrigen> GetCpAnalisisAsync(Integrantes integrantes);
        Task<IOrigen> GetCpAuxiliarAsync(Integrantes integrantes);
    }

    public class MEBAService : IMEBAService
    {
        private readonly MEBAFactory _mebaFactory;
        private readonly Parametros _parametros;

        public MEBAService(MEBAFactory mebaFactory, Parametros parametros)
        {
            _mebaFactory = mebaFactory;
            _parametros = parametros;
        }

        public async Task<IOrigen> GetCalificacionConstanteAsync(Integrantes integrantes, string origenId, string comercioId)
        {
            var registrosMap = new Dictionary<string, CalificacionConstante>();
            string numeroTributario = integrantes.Titular.NumeroTributario.ToString() ?? string.Empty;

            string precaAConsultar = string.Empty;
            string keyEspecifica = $"OFERTA ORIGEN={origenId} COMERCIO={comercioId}";
            string keyGeneral = $"OFERTA ORIGEN={origenId}";

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
                return new Origen<string, CalificacionConstante>(Origenes.CalificacionConstante, registrosMap);
            }
            var cuilParam = new SqlParameter("@cuil", System.Data.SqlDbType.NVarChar, 11) { Value = numeroTributario };
            var ofertaParam = new SqlParameter("@oferta", System.Data.SqlDbType.NVarChar, 30) { Value = precaAConsultar };
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            string query = @"
                SELECT TOP(1) 
                       CV.CUIL,
                       'CC' AS CC_ID_CAMPANA, 
                       CV.NOMBRE_OFERTA AS CC_CODIGO_CAMPANA,
                       TI.COD_SEGMENTO AS CC_SEGMENTO_CRM,
                       TI.COD_SUBSEGMENTO AS CC_SUBSEGMENTO_CRM,
                       AD.SUCURSAL_RIESGO AS CC_SUCURSAL_MIEMBRO,
                       'PERSONAL' AS CC_CANAL, 
                       CV.MENSAJE_OFERTA AS CC_TIPO_PRECALIFICACION,
                       AD.FECHA_INICIO AS CC_FECHA_CALIFICACION,
                       AD.FECHA_VENCIMIENTO AS CC_FECHA_VENCIMIENTO,
                       AD.CONVENIO_LIQUI_PRESTAMO AS CC_CONVENIO,
                       '007-PERSTECO' AS CC_PRODUCTO_PRESTAMO,
                       '028-TARJCRED' AS CC_PRODUCTO_TARJETA,
                       '003-ACUERDOS' AS CC_PRODUCTO_ACUERDO, 
                       CV.TNA_PRESTAMO AS CC_PRESTAMO_TASA,
                       CV.PLAZO_PRESTAMO AS CC_PRESTAMO_PLAZO,
                       TRY_CAST(CV.OFERTA_PRESTAMO AS BIGINT) AS CC_PRESTAMO_MONTO,
                       TRY_CAST(CV.AFECTACION_PRESTAMO AS BIGINT) AS CC_PRESTAMO_AFECTACION, 
                       TRY_CAST(CV.CUOTA_PROM_PRESTAMO AS BIGINT) AS CC_PRESTAMO_CUOTA_PROMEDIO, 
                       TRY_CAST(CV.CUOTA_INI_PRESTAMO AS BIGINT) AS CC_PRESTAMO_CUOTA_INICIAL, 
                       TRY_CAST(CV.OFERTA_TARJETA AS BIGINT) AS CC_TARJETA_MONTO, 
                       TRY_CAST(CV.OFERTA_ACUERDO AS BIGINT) AS CC_ACUERDO_MONTO,
                       CV.ID_EJECUCION AS CC_ID_OFERTAS 
                FROM MEBADB.dbo.CALIFICACIONES_VIGENTES CV WITH (NOLOCK)
                INNER JOIN MEBADB.dbo.AFECTACION_DICTAMEN_CC_BI AD WITH (NOLOCK)
                    ON AD.CODIGO_CLIENTE = CV.CODIGO_CLIENTE AND AD.RAMA_POLITICA = CV.RAMA_POLITICA  
                INNER JOIN MEBADB.dbo.TABLON_INDIVIDUOS TI WITH (NOLOCK)
                    ON TI.CODIGO_CLIENTE = CV.CODIGO_CLIENTE 
                WHERE CV.CUIL = @cuil AND CV.NOMBRE_OFERTA = @oferta
                ORDER BY CV.ID_EJECUCION DESC";

            var registros = await db.Set<CalificacionConstante>()
                .FromSqlRaw(query, cuilParam, ofertaParam)
                .AsNoTracking()
                .ToListAsync();

            registrosMap = registros.ToDictionary(c => c.Cuil ?? string.Empty);

            return new Origen<string, CalificacionConstante>(Origenes.CalificacionConstante, registrosMap);
        }

        public async Task<IOrigen> GetClientesAsync(Integrantes integrantes)
        {
            var cuilsString = integrantes.ListaNumerosTributarios.Select(n => (string?)n.ToString()).ToList();
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.Clientes
                .AsNoTracking()
                .Where(r => cuilsString.Contains(r.NroDocTributario))
                .ToListAsync();
            var registrosMap = registros.ToDictionary(c => c.NroDocTributario ?? string.Empty);
            return new Origen<string, Clientes>(Origenes.Clientes, registrosMap);
        }

        public async Task<IOrigen> GetClientesDepuradosAsync(Integrantes integrantes)
        {
            var cuilsString = integrantes.ListaNumerosTributarios.Select(n => (string?)n.ToString()).ToList();
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.ClientesDepurados
                .AsNoTracking()
                .Where(r => cuilsString.Contains(r.Cuil) && r.DepuracionHasta > DateTime.Now)
                .ToListAsync();
            var registrosMap = registros.ToDictionary(c => c.Cuil ?? "");
            return new Origen<string, ClientesDepurados>(Origenes.ClientesDepurados, registrosMap);
        }

        public async Task<IOrigen> GetEndeudamientoAtrasoAsync(Integrantes integrantes)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var listaCobisLong = integrantes.ListaCobis.Select(cobis => (long?)cobis).ToList();
            var registros = await db.EndeudamientoAtraso
                .AsNoTracking()
                .Where(r => listaCobisLong.Contains(r.CodigoCliente))
                .ToListAsync();
            var registrosMap = registros.ToDictionary(c => c.CodigoCliente ?? 0);
            return new Origen<long, EndeudamientoAtraso>(Origenes.EndeudamientoAtraso, registrosMap);
        }

        public async Task<IOrigen> GetLimiteTransitorioAsync(Integrantes integrantes)
        {
            var cuilsString = integrantes.ListaNumerosTributarios.Select(n => n.ToString()).ToList();
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.LimiteTransitorio
                .AsNoTracking()
                .Where(r => cuilsString.Contains(r.NumeroTributario))
                .ToListAsync();
            var registrosMap = registros
                .Select((registro, index) => new { index, registro })
                .ToDictionary(x => x.index, x => x.registro);
            return new Origen<int, LimiteTransitorio>(Origenes.LimiteTransitorio, registrosMap);
        }
        public async Task<IOrigen> GetLkpIrregularidadesBcraAsync(Integrantes integrantes)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.LkpIrregularidadesBcra
                .AsNoTracking()
                .Where(r => integrantes.ListaNumerosTributarios.Contains(r.BcraDeuDocTributario))
                .ToListAsync();
            var registrosMap = registros.ToDictionary(c => c.BcraDeuDocTributario);
            return new Origen<long, LkpIrregularidadesBcra>(Origenes.LkpIrregularidadesBcra, registrosMap);
        }

        public async Task<IOrigen> GetTablonIndividuosAsync(Integrantes integrantes)
        {
            var cuilsString = integrantes.ListaNumerosTributarios.Select(n => (string?)n.ToString()).ToList();
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.TablonIndividuos
                .AsNoTracking()
                .Where(r => cuilsString.Contains(r.Cuil))
                .ToListAsync();
            var registrosMap = registros.ToDictionary(c => c.Cuil ?? "");
            return new Origen<string, TablonIndividuos>(Origenes.TablonIndividuos, registrosMap);
        }

        public async Task<IOrigen> GetValidacionActividadAsync(Integrantes integrantes)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var cuils = integrantes.ListaNumerosTributarios.Select(cuil => (long?)cuil).ToList();
            var registros = await db.ValidacionActividad
                .AsNoTracking()
                .Where(r => cuils.Contains(r.Cuit))
                .ToListAsync();
            var registrosMap = registros.ToDictionary(c => c.Cuit ?? 0);
            return new Origen<long, ValidacionActividad>(Origenes.ValidacionActividad, registrosMap);
        }

        public async Task<Integrantes> GetIntegrantesPorCuilAsync(RequestEvaluaIndividuos request)
        {
            var cuilsList = new List<long>(4);

            if (request.Titular?.NumeroCUIL is long c0 && c0 > 0) cuilsList.Add(c0);
            if (request.Cotitular1?.NumeroCUIL is long c1 && c1 > 0) cuilsList.Add(c1);
            if (request.Cotitular2?.NumeroCUIL is long c2 && c2 > 0) cuilsList.Add(c2);
            if (request.Cotitular3?.NumeroCUIL is long c3 && c3 > 0) cuilsList.Add(c3);

            long[] cuils = cuilsList.ToArray();
            return await GetIntegrantesPorCuilAsync(cuils);
        }

        public async Task<Integrantes> GetIntegrantesPorCuilAsync(long[] cuils)
        {
            var integrantes = new Integrantes();
            var listaCuils = cuils.Select(cuil => (long?)cuil).ToList();
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.LkpClientesCompleta
                .AsNoTracking()
                .Where(r => listaCuils.Contains(r.NumeroTributario))
                .Select(r => new LkpClientesCompleta
                {
                    NumeroTributario = r.NumeroTributario,
                    CodigoCliente = r.CodigoCliente
                })
                .ToListAsync();

            var clientesMap = registros.ToDictionary(c => c.NumeroTributario ?? 0);
            foreach (var c in cuils) integrantes.Add(c, clientesMap.GetValueOrDefault(c)?.CodigoCliente ?? 0);
            return integrantes;
        }

        public async Task<Integrantes> GetIntegrantesPorCobisAsync(int[] cobis)
        {
            var integrantes = new Integrantes();
            var listaCobis = cobis.Select(cobis => (int?)cobis).ToList();
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.LkpClientesCompleta
                .AsNoTracking()
                .Where(r => listaCobis.Contains(r.CodigoCliente))
                .Select(r => new LkpClientesCompleta
                {
                    NumeroTributario = r.NumeroTributario,
                    CodigoCliente = r.CodigoCliente
                })
                .ToListAsync();

            var clientesMap = registros.ToDictionary(c => c.CodigoCliente ?? 0);
            foreach (var c in cobis) integrantes.Add(clientesMap.GetValueOrDefault(c)?.NumeroTributario ?? 0, c);
            return integrantes;
        }

        public async Task<IOrigen> GetChequesRechazadosAsync(Integrantes integrantes)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.ChequesRechazados
                .AsNoTracking()
                .Where(r => integrantes.ListaNumerosTributarios.Contains(r.numeroTributario))
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.numeroTributario);
            return new Origen<long, ChequesRechazados>(Origenes.ChequesRechazados, registrosMap);
        }

        public async Task<IOrigen> GetScoreComportamientoAsync(Integrantes integrantes)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.ScoreComportamientos
                .AsNoTracking()
                .Where(r => integrantes.ListaNumerosTributarios.Contains(r.nro_doc_tributario))
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.nro_doc_tributario);
            return new Origen<long, ScoreComportamiento>(Origenes.ScoreComportamiento, registrosMap);
        }

        public async Task<IOrigen> GetTablonAsync(Integrantes integrantes)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.Tablon
                .AsNoTracking()
                .Where(r => integrantes.ListaNumerosTributarios.Contains(r.NumeroTributario))
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.NumeroTributario);
            return new Origen<long, Tablon>(Origenes.Tablon, registrosMap);
        }

        public async Task<IOrigen> GetLkpConveniosAsync(int numeroConvenio)
        {
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.LkpConvenios
                .AsNoTracking()
                .Where(r => numeroConvenio.Equals(r.NumeroConvenio))
                .ToListAsync();

            var registrosMap = registros.ToDictionary(c => c.NumeroConvenio ?? -1);
            return new Origen<int, LkpConvenios>(Origenes.LkpConvenios, registrosMap);
        }

        public async Task<IOrigen> GetCpAnalisisAsync(Integrantes integrantes)
        {
            long cuil = integrantes.Titular?.NumeroTributario ?? -1;
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.CpAnalisis
                .AsNoTracking()
                .Where(r => cuil.Equals(r.Cuil))
                .ToListAsync();

            var registrosMap = new Dictionary<long, CpAnalisis>();
            foreach (var item in registros)
            {
                registrosMap.TryAdd(item.Cuil ?? -1, item);
            }
            return new Origen<long, CpAnalisis>(Origenes.CpAnalisis, registrosMap);
        }

        public async Task<IOrigen> GetCpAuxiliarAsync(Integrantes integrantes)
        {
            long cuil = integrantes.Titular?.NumeroTributario ?? -1;
            using var db = await _mebaFactory._dbContextFactory.CreateDbContextAsync();
            var registros = await db.CpAuxiliar
                .AsNoTracking()
                .Where(r => cuil.Equals(r.Cuil))
                .ToListAsync();

            var registrosMap = new Dictionary<long, CpAuxiliar>();
            foreach (var item in registros)
            {
                registrosMap.TryAdd(item.Cuil ?? -1, item);
            }

            return new Origen<long, CpAuxiliar>(Origenes.CpAuxiliar, registrosMap);
        }
    }
}
