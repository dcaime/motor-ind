using Carter;
using EvaluaIndividuos.Model;

namespace EvaluaIndividuos.Endpoints
{
    public class Admin : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/refresh", (ParametrosProvider provider) =>
            {
                provider.ClearCache();
                return Results.Ok();
            }).ExcludeFromDescription();
        }
    }
}
