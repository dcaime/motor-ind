using Carter;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Database.MEBI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EvaluaIndividuos.Endpoints
{
    public class Webhooks : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/webhooks/veraz_estricto", async (WebhooksVerazPostRequest request, [FromServices] MEBIContext dbContext) =>
            {
                var cacheEntry = new CacheMapi
                {
                    FechaHora = DateTime.Now,
                    Servicio = "veraz",
                    Clave = 1,
                    Cache = System.Text.Json.JsonSerializer.Serialize(request)
                };
                
                dbContext.Set<CacheMapi>().Add(cacheEntry);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }).ExcludeFromDescription();

            app.MapPost("/webhooks/veraz", async (HttpRequest httpRequest, [FromServices] MEBIContext dbContext, [FromServices] WebhookCoordinator coordinator) =>
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower
                };

                WebhooksVerazPostRequest requestObj;
                try
                {
                    requestObj = await httpRequest.ReadFromJsonAsync<WebhooksVerazPostRequest>(options);
                    if (requestObj == null) return Results.BadRequest("Error al parsear.");
                }
                catch (System.Text.Json.JsonException ex)
                {
                    return Results.BadRequest(ex.Message);
                }

                coordinator.CompleteRequest(requestObj.RequestId, requestObj);
                long clave = -1;
                long.TryParse(requestObj.CompanyTaxpayerNumber, out clave);
                var cacheEntry = new CacheMapi
                {
                    FechaHora = DateTime.Now,
                    Servicio = "veraz",
                    Clave = clave,
                    Cache = System.Text.Json.JsonSerializer.Serialize(requestObj, options)
                };

                dbContext.Set<CacheMapi>().Add(cacheEntry);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }).ExcludeFromDescription();
        }
    }
}
