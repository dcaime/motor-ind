using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using Microsoft.Extensions.Caching.Memory;

namespace EvaluaIndividuos.Model
{
    public class ParametrosProvider
    {
        private readonly IMemoryCache _cache;
        private readonly MEBAFactory _mebaFactory;
        private static readonly SemaphoreSlim _lock = new(1, 1);
        private const string ParamCacheKey = "DatabaseParameters_MAPI";

        public ParametrosProvider(IMemoryCache cache, MEBAFactory mebaFactory)
        {
            _cache = cache;
            _mebaFactory = mebaFactory;
        }

        public async Task<Parametros> GetParametrosAsync()
        {
            if (_cache.TryGetValue(ParamCacheKey, out Parametros? cachedParams) && cachedParams != null) return cachedParams;
            await _lock.WaitAsync();
            try
            {
                if (_cache.TryGetValue(ParamCacheKey, out cachedParams) && cachedParams != null) return cachedParams;
                cachedParams = await ParametrosMotor.GetAsync(_mebaFactory);
                _cache.Set(ParamCacheKey, cachedParams);

                return cachedParams;
            }
            finally
            {
                _lock.Release();
            }
        }

        public void ClearCache()
        {
            _cache.Remove(ParamCacheKey);
        }
    }
}
