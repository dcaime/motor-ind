// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MAPITools
{
    public interface IEvaluable
    {
        string[] codificaEntrada();
        void decodificaSalida(string[] ArraySalida);
        Politica GetPolitica();
    }
}
