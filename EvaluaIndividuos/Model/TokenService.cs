using EvaluaIndividuos.Database.MEBA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text;

namespace EvaluaIndividuos.Model
{
    using Microsoft.Extensions.Caching.Memory;
    using Serilog;
    using System;
    using System.Net.Http.Headers;
    using System.Runtime.Intrinsics.Arm;
    using System.Text;

    public class TokenService
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly api_oauth_access_v1_0Client _nswagClient;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        private const string TokenCacheKey = "MacroAccessToken";

        public TokenService(IMemoryCache cache, IServiceScopeFactory scopeFactory, api_oauth_access_v1_0Client nswagClient)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;
            _nswagClient = nswagClient;
        }

        public async Task<string> GetTokenAsync()
        {
            if (_cache.TryGetValue(TokenCacheKey, out string? token) && token != null) return token;

            await _lock.WaitAsync();
            try
            {
                if (_cache.TryGetValue(TokenCacheKey, out token) && token != null) return token;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var p = scope.ServiceProvider.GetRequiredService<Parametros>();
                    var clientId = CryptoService.Decrypt(p.GetString("apigee_client_id"));
                    var secret = CryptoService.Decrypt(p.GetString("apigee_secret"));
                    if (Herramientas.EsAmbienteBajo()) clientId = CryptoService.Decrypt("cvGdPayBloBSuOdKK+Xjc9ytAFgKZSmXYw3i1fQ0FKxD8VI2X0A1g7O/aprGfEGZppbjxhrO/950LRbGk08k7Q==");
                    if (Herramientas.EsAmbienteBajo()) secret = CryptoService.Decrypt("oJE0h6TJ95q6K3vPeWjOpjJpE8h7pFTQP/qGXobZJlUtFY+H6Bn4kh7lZ4MM+HSYd/hwCm7hCqV5h3n4akqyKPn2VpLVxWEgcyY+a0rusOs=");

                    var baseUrl = p.GetString("apigee_base_url");
                    if (Herramientas.EsAmbienteBajo()) baseUrl = "https://api-test.gcp.macro.com.ar";
                    var authBytes = Encoding.UTF8.GetBytes($"{clientId}:{secret}");
                    var authHeader = Convert.ToBase64String(authBytes);

                    _nswagClient.BaseUrl = $"{baseUrl}/v1/oauth";

                    var authValue = Convert.ToBase64String(authBytes);
                    _nswagClient.SetBasicAuthentication(authValue);
                    var requestBody = new Body
                    {
                        Grant_type = BodyGrant_type.Client_credentials,
                        Scope = BodyScope.Openid
                    };

                    var response = await _nswagClient.JwtAsync(new Body
                    {
                        Grant_type = BodyGrant_type.Client_credentials,
                        Scope = BodyScope.Openid
                    });

                    token = response.Access_token;

                    var expiry = TimeSpan.FromSeconds(response.Expires_in > 0 ? response.Expires_in - 60 : 3540);
                    _cache.Set(TokenCacheKey, token, expiry);
                    Log.Information("Client id: " + clientId);
                    Log.Information("Token generado: " + token);
                    return token;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
