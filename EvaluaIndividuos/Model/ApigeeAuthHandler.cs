using EvaluaIndividuos.Database.MEBA.Model;
using Microsoft.AspNetCore.DataProtection;
using Serilog;
using System.Net.Http.Headers;

namespace EvaluaIndividuos.Model
{
    public class ApigeeAuthHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;
        private readonly IServiceScopeFactory _scopeFactory;

        public ApigeeAuthHandler(TokenService tokenService, IServiceScopeFactory scopeFactory)
        {
            _tokenService = tokenService;
            _scopeFactory = scopeFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = await _tokenService.GetTokenAsync();
            Log.Information("Token: "+token);
            using (var scope = _scopeFactory.CreateScope())
            {
                var p = scope.ServiceProvider.GetRequiredService<Parametros>();

                if (request.RequestUri != null && !request.RequestUri.IsAbsoluteUri)
                {
                    var baseUri = new Uri(p.GetString("apigee_base_url"));
                    if(Herramientas.EsAmbienteBajo()) baseUri = new Uri("https://api-test.gcp.macro.com.ar");
                    request.RequestUri = new Uri(baseUri, request.RequestUri);
                }

                var apiKey = CryptoService.Decrypt(p.GetString("apigee_client_id"));
                if (Herramientas.EsAmbienteBajo()) apiKey = CryptoService.Decrypt("cvGdPayBloBSuOdKK+Xjc9ytAFgKZSmXYw3i1fQ0FKxD8VI2X0A1g7O/aprGfEGZppbjxhrO/950LRbGk08k7Q==");

                if (!request.Headers.Contains("apikey")) request.Headers.Add("apikey", apiKey);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
