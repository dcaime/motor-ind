// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace MAPITools
{
    public class MAPIException : Exception
    {
        public readonly int Codigo;
        public readonly int Subcodigo;
        public readonly string Tipo;
        public readonly string Mensaje;

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
                return new MAPIException("Internal Server Error", 0, "Error desconcido", 500);
            }

            Exception inner = ex;
            while (inner is not null)
            {
                if (inner is MAPIException)
                {
                    return (MAPIException)inner;
                }
                inner = inner.InnerException;
            }
            return new MAPIException(ex, "Internal Server Error", 100, 500);
        }

        private string GetFirstMessage(Exception ex)
        {
            Exception inner = ex;
            while (inner.InnerException is not null)
            {
                inner = inner.InnerException;
            }
            return inner.Message;
        }

        public MAPIExceptionContent getContent()
        {
            return new MAPIExceptionContent(Tipo, Subcodigo, Mensaje);
        }
    }

    public class MAPIExceptionContent
    {
        [JsonInclude]
        public readonly int Subcodigo;
        [JsonInclude]
        public readonly string Tipo;
        [JsonInclude]
        public readonly string Mensaje;

        public MAPIExceptionContent(string Tipo, int Subcodigo, string Mensaje)
        {
            this.Subcodigo = Subcodigo;
            this.Tipo = Tipo;
            this.Mensaje = Mensaje;
        }
    }

    public enum MAPIStatus
    {

    }
}
