using CalculadoraAgro.Model;
using Carter;
using Microsoft.AspNetCore.HttpLogging;
using NLog.Web;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
});
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});
builder.WebHost.UseNLog();

var app = builder.Build();

// Exceptions
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapGet("/version", () => new {
    version = System.Reflection.Assembly.GetEntryAssembly()!
        .GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()?.InformationalVersion,
    compilado = File.GetLastWriteTimeUtc(System.Reflection.Assembly.GetEntryAssembly()!.Location)
}).ExcludeFromDescription();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || Herramientas.GetEnvironment() == Ambiente.TEST)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    if (app.Environment.IsDevelopment())
    {
        app.UsePathBase("/CalculadoraAgro");
        app.UseRouting();
    }
}

//app.UseMiddleware<NLog.Web.NLogRequestPostedBodyMiddleware>(new NLog.Web.NLogRequestPostedBodyMiddlewareOptions());
app.UseHttpLogging();
app.MapCarter();
app.Run();
