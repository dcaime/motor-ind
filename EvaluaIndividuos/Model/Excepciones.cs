// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Serilog;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EvaluaIndividuos.Model
{
    public class MAPIException : Exception
    {
        public readonly int Codigo;
        public readonly int? Subcodigo;
        public readonly string? Tipo;
        public readonly string? Mensaje;

        public MAPIException(int Codigo = StatusCodes.Status403Forbidden)
        {
            this.Codigo = Codigo;
        }

        public MAPIException(string Tipo, int Subcodigo, string Mensaje, int Codigo = StatusCodes.Status409Conflict)
        {
            this.Subcodigo = Subcodigo;
            this.Tipo = Tipo;
            this.Codigo = Codigo;
            this.Mensaje = Mensaje;
        }

        public MAPIException(Exception InnerEx, string Tipo, int Subcodigo, int Codigo = StatusCodes.Status409Conflict)
            : base("", InnerEx)
        {
            this.Subcodigo = Subcodigo;
            this.Tipo = Tipo;
            this.Codigo = Codigo;
            this.Mensaje = GetFirstMessage(InnerEx);
        }

        public MAPIException(Exception InnerEx, string Tipo, int Subcodigo, string Mensaje, int Codigo = StatusCodes.Status409Conflict)
            : base("", InnerEx)
        {
            this.Subcodigo = Subcodigo;
            this.Tipo = Tipo;
            this.Codigo = Codigo;
            this.Mensaje = Mensaje;
        }

        public static MAPIException GetMAPIException(Exception ex)
        {
            if (ex is null)
            {
                return new MAPIException(500);
            }

            Exception inner = ex;
            while (inner is not null)
            {
                if (inner is MAPIException)
                {
                    return (MAPIException)inner;
                }
                if (inner is BadHttpRequestException || inner is System.Text.Json.JsonException)
                {
                    return new MAPIException(ex, "Bad Request", 400, "Formato de entrada inválido: " + GetFirstMessage(ex), StatusCodes.Status400BadRequest);
                }
                inner = inner.InnerException;
            }
            return new MAPIException(ex, "Internal Server Error", 100, 500);
        }

        private static string GetFirstMessage(Exception ex)
        {
            Exception inner = ex;
            while (inner.InnerException is not null)
            {
                inner = inner.InnerException;
            }
            return inner.Message;
        }

        public MAPIExceptionContent? getContent()
        {
            if (Tipo is null && Subcodigo is null && Mensaje is null) return null;
            return new MAPIExceptionContent(Tipo, Subcodigo, Mensaje);
        }
    }

    public class MAPIExceptionContent
    {
        [JsonInclude]
        public readonly int? Subcodigo;
        [JsonInclude]
        public readonly string? Tipo;
        [JsonInclude]
        public readonly string? Mensaje;

        public MAPIExceptionContent(string? Tipo, int? Subcodigo, string? Mensaje)
        {
            this.Subcodigo = Subcodigo;
            this.Tipo = Tipo;
            this.Mensaje = Mensaje;
        }
    }

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Registro registro, Parametros parametros)
        {
            try
            {
                ControlaAcceso(context, parametros);
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, registro);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, Registro registro)
        {
            MAPIException ex = MAPIException.GetMAPIException(exception);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.Codigo;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            var result = JsonSerializer.Serialize(ex.getContent(), options);

            registro.RegistraError(ex.getContent());

            if (ex.getContent() is null) return Task.CompletedTask;
            return context.Response.WriteAsync(result);
        }

        private void ControlaAcceso(HttpContext context, Parametros parametros)
        {
            if (context.GetEndpoint()?.DisplayName is "/GetSIISA" && Herramientas.GetEnvironment() == Ambiente.PREP) return;
            if (Herramientas.GetEnvironment() == Ambiente.PROD) return;
            if (parametros.GetValueOrDefault("ListaBlancaActiva", "") == "NO") return;
            Log.Information("Endpoint " + context.GetEndpoint()?.DisplayName ?? "");
            if (context.Connection.RemoteIpAddress is null)
            {
                Log.Information("Invocador sin ip");
                throw new MAPIException(403);
            }

            var ip = context.Connection.RemoteIpAddress.ToString();
            Log.Information("IP Invocador " + ip);
            if (ip == "::1") return;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(ip);
                Log.Information("Host " + host.HostName);
            }
            catch (Exception exception)
            {
                Log.Information("Host Exception " + exception.Message);
                return;
            }

            if (parametros.GetValueOrDefault("ListaBlanca", "").Contains(ip)) return;
            string[] ips;
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    ips = new string[] { "172.31.5.49", "172.31.84.41", "172.31.84.130", "172.31.152.12", "172.31.152.13", "10.54.1.43", "10.54.1.52" };
                    if (Environment.MachineName == "SRVMEBIWB-PROD") return;
                    return;
                //break;
                case Ambiente.PREP:
                    if (ip.Contains("10.54.17.")) return; //IPs rotativas APIGee PREP
                    if (Environment.MachineName == "SRVMEBIWB-PRE") return;
                    ips = new string[] { "172.31.5.31", "10.54.17.45", "10.54.17.13", "10.54.17.252", "10.54.17.52", "fe80::b53a:acdf:313d:4d9", "172.31.155.94" };
                    break;
                case Ambiente.TEST or Ambiente.DESA:
                    ips = new string[] { "172.31.219.211", "10.30.34.127", "172.31.85.88" };
                    if (Environment.MachineName == "SRVMEBIWB-TEST") return;
                    return;
                //break;
                default:
                    return;
            }

            if (ips.Contains(ip)) return;
            throw new MAPIException(403);
        }
    }
}
