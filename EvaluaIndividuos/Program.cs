using Carter;
using Carter.Request;
using EvaluaIndividuos;
using EvaluaIndividuos.Database.MEBA;
using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI;
using EvaluaIndividuos.Model;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddScoped<Registro>();
builder.Services.AddHttpClient<api_oauth_access_v1_0Client>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ParametrosProvider>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddTransient<ApigeeAuthHandler>();
builder.Services.AddScoped<Parametros>(sp =>
{
    var provider = sp.GetRequiredService<ParametrosProvider>();
    return provider.GetParametrosAsync().GetAwaiter().GetResult();
});

builder.Services.AddHttpClient<api_fp_credit_assessment_information_Client>()
    .AddHttpMessageHandler<ApigeeAuthHandler>();

builder.Services.AddHttpClient<api_webhooks_v1_9_1Client>
    (httpClient =>
    {
        httpClient.DefaultRequestHeaders.Add("x-callback-url", "https://"+ Environment.MachineName + ".macro.com.ar:50008/EvaluaIndividuos/Webhooks/Veraz");
    })
    .AddHttpMessageHandler<ApigeeAuthHandler>();

builder.Services.AddHttpClient<api_bureaus_v1_0_6_Client>
    (httpClient =>
    {
        httpClient.DefaultRequestHeaders.Add("x-callback-url", "https://" + Environment.MachineName + ".macro.com.ar:50008/EvaluaIndividuos/Webhooks/Veraz");
    })
    .AddHttpMessageHandler<ApigeeAuthHandler>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.File(
        path: "D:\\MAPI\\MAPI_Logs\\EvaluaIndividuos\\log_.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        fileSizeLimitBytes: 100_000_000,
        rollOnFileSizeLimit: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{TraceId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    ))
    .CreateLogger();

builder.Host.UseSerilog();
builder.WebHost.UseUrls("http://localhost:50008");

builder.Services.AddDbContextFactory<MEBAContext>(options =>
    options.UseSqlServer(MEBAContext.GetConnectionString()));
builder.Services.AddSingleton<MEBAFactory>();

builder.Services.AddDbContextFactory<MEBIContext>(options =>
    options.UseSqlServer(MEBIContext.GetConnectionString()));
builder.Services.AddSingleton<MEBIFactory>();

builder.Services.AddTransient<IMEBAService, MEBAService>();
builder.Services.AddTransient<IMEBIService, MEBIService>();
builder.Services.AddScoped<IEvaluador, Evaluador>();
builder.Services.AddSingleton<WebhookCoordinator>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapGet("/version", () => new {
    // version = Assembly.GetEntryAssembly()!
    //     .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
    version = Assembly.GetExecutingAssembly().GetName().Version,
    compilado = File.GetLastWriteTimeUtc(Assembly.GetEntryAssembly()!.Location)
});

app.MapGet("/health", async (IDbContextFactory<MEBAContext> mebaFactory, IDbContextFactory<MEBIContext> mebiFactory, CancellationToken ct) =>
{
    async Task<string> Check(Func<Task<bool>> canConnect)
    {
        try { return await canConnect() ? "Healthy" : "Unhealthy"; }
        catch { return "Unhealthy"; }
    }

    using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
    timeout.CancelAfter(TimeSpan.FromSeconds(5));

    var meba = Check(async () => { await using var db = await mebaFactory.CreateDbContextAsync(timeout.Token); return await db.Database.CanConnectAsync(timeout.Token); });
    var mebi = Check(async () => { await using var db = await mebiFactory.CreateDbContextAsync(timeout.Token); return await db.Database.CanConnectAsync(timeout.Token); });
    await Task.WhenAll(meba, mebi);

    var healthy = meba.Result == "Healthy" && mebi.Result == "Healthy";
    var body = new { status = healthy ? "Healthy" : "Unhealthy", meba = meba.Result, mebi = mebi.Result };
    return Results.Json(body, statusCode: healthy ? StatusCodes.Status200OK : StatusCodes.Status503ServiceUnavailable);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    if (app.Environment.IsDevelopment())
    {
        app.UsePathBase("/EvaluaIndividuos");
        app.UseRouting();
    }
}

app.MapCarter();
app.Run();
