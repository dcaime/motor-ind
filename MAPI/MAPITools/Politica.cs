// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

public sealed class Politica
{

    public static Politica EvaluaIndividuos = new Politica("EvaluaIndividuos", 10, 151, 21, 3, 4, 15);
    public static Politica CalculadoraAgro = new Politica("CalculadoraAgro", 11, 273, 129, 3, 4, 5);
    public static Politica CalculadoraAgroV2 = new Politica("CalculadoraAgroV2", 15, 273, 129, 3, 4, 5);
    //        MercadoAbierto = 1,
    //        PlanSueldoPrivado = 2,
    //        RentaPresunta = 3,
    //        PrimerFiltro = 4,
    //        CreditoAlInstante = 5,
    //        ScoringMepy = 6,
    //        PlanSueldoPublicoJubilados = 7,
    //        Actividad = 9,
    //        EvaluaIndividuos = 10,
    //        CalculadoraAgro = 11,
    //        ScoringAgro = 12,
    //        CreditoSimple = 13,
    //        CreditoSimpleSISA = 14,
    //        CalculadoraAgroV2 = 15
    public readonly string Nombre;
    public readonly int Codigo;
    public readonly int CamposEntrada;
    public readonly int CamposSalida;
    public readonly int IdxError;
    public readonly int IdxDescripcionError;
    public readonly int IdxNroTributario;

    private Politica(string nombre, int codigo, int camposEntrada, int camposSalida, int idxError, int idxDescripcionError, int idxNroTributario)
    {
        Nombre = nombre;
        Codigo = codigo;
        CamposEntrada = camposEntrada;
        CamposSalida = camposSalida;
        IdxDescripcionError = idxDescripcionError;
        IdxError = idxError;
        IdxNroTributario = idxNroTributario;
    }
}
