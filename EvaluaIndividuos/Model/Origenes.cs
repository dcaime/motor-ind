using EvaluaIndividuos.Database.MEBA.Model;
using EvaluaIndividuos.Database.MEBI.Model;
using EvaluaIndividuos.Endpoints;
using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.ObjectModel;
using System.Threading;
using static EvaluaIndividuos.Model.PagosPrestamosCanales;

namespace EvaluaIndividuos.Model
{
    public enum Origenes
    {
        BcraDeudores,
        Clientes,
        ComportamientoInterno,
        ChequesRechazados,
        Request,
        Response,
        Behavior,
        Parametros,
        TablonEmpresas,
        NivelesAutorizacion,
        CodigosRechazo,
        Excepciones,
        ListaNegra,
        ClientesDepurados,
        ValidacionActividad,
        LkpIrregularidadesBcra,
        EndeudamientoAtraso,
        TablonIndividuos,
        LimiteTransitorio,
        Intradiarias,
        CalificacionConstante,
        CampañasExceptuadas,
        Precacom,
        MoraSucursal,
        ScoreComportamiento,
        Tablon,
        LkpConvenios,
        CpAnalisis,
        CpAuxiliar,
        CacheMapi,
        ConveniosCodDescuento,
        CalificacionConvenios
    }

    public record Origen<TKey, TValue> : IOrigen where TKey : notnull
    {
        public Origenes Tipo { get; init; }
        public Dictionary<TKey, TValue> Registros { get; init; }
        object IOrigen.Registros => Registros;
        public Origen(Origenes tipo, Dictionary<TKey, TValue> registros)
        {
            Tipo = tipo;
            Registros = registros;
        }
    }

    public interface IOrigen
    {
        Origenes Tipo { get; }
        object Registros { get; }
    }

    public class FuentesDeInformacion
    {
        public Dictionary<long, BcraDeudores> BcraDeudores { get; private set; } = new();
        public Dictionary<long, ListaNegra> ListaNegra { get; private set; } = new();
        public Dictionary<string, ClientesDepurados> ClientesDepurados { get; private set; } = new();
        public Dictionary<string, Clientes> Clientes { get; private set; } = new();
        public Dictionary<long, ValidacionActividad> ValidacionActividad { get; private set; } = new();
        public Dictionary<long, LkpIrregularidadesBcra> LkpIrregularidadesBcra { get; private set; } = new();
        public Dictionary<long, EndeudamientoAtraso> EndeudamientoAtraso { get; private set; } = new();
        public Dictionary<string, TablonIndividuos> TablonIndividuos { get; private set; } = new();
        public Dictionary<int, LimiteTransitorio> LimiteTransitorio { get; private set; } = new();
        public Dictionary<int, IntradiariasSumarizadas> Intradiarias { get; private set; } = new();
        public Dictionary<string, CalificacionConstante> CalificacionConstante { get; private set; } = new();
        public Dictionary<string, CampañasExceptuadas> CampañasExceptuadas { get; private set; } = new();
        public Dictionary<long, Precacom> Precacom { get; private set; } = new();
        public Dictionary<long, MoraSucursal> MoraSucursal { get; private set; } = new();
        public Dictionary<int, Behavior> Behavior { get; private set; } = new();
        public Dictionary<long, ScoreComportamiento> ScoreComportamiento { get; private set; } = new();
        public Dictionary<long, Tablon> Tablon { get; private set; } = new();
        public Dictionary<long, ChequesRechazados> ChequesRechazados { get; private set; } = new();
        public Dictionary<string, Excepciones> Excepciones { get; private set; } = new();
        public Dictionary<int, LkpConvenios> LkpConvenios { get; private set; } = new();
        public Dictionary<long, CpAuxiliar> CpAuxiliar { get; private set; } = new();
        public Dictionary<long, CpAnalisis> CpAnalisis { get; private set; } = new();
        public Dictionary<string, CacheMapi> CacheMapi { get; private set; } = new();
        public Dictionary<int, ConveniosCodDescuento> ConveniosCodDescuento { get; private set; } = new();
        public Dictionary<string, CalificacionConvenios> CalificacionConvenios { get; private set; } = new();
        public Parametros Parametros { get; private set; }
        public Registro Registro { get; private set; }
        public RequestEvaluaIndividuos RequestEvaluaIndividuos { get; set; }

        public RequestServicioLegacy RequestLegacy { get; set; }
        public Integrantes Integrantes { get; set; }

        private FuentesDeInformacion(Parametros parametros, Registro registro)
        {
            this.Parametros = parametros;
            this.Registro = registro;
        }

        public static async Task<FuentesDeInformacion> CreateAsync(Parametros parametros, Registro registro)
        {
            var fuentesInformacion = new FuentesDeInformacion(parametros, registro);
            return fuentesInformacion;
        }

        public static async Task<FuentesDeInformacion> CreateAsync(List<Task<IOrigen>> tareas, Parametros parametros, Registro registro)
        {
            var fuentesInformacion = new FuentesDeInformacion(parametros, registro);
            await fuentesInformacion.MapeaTareas(tareas);
            return fuentesInformacion;
        }

        public async Task MapeaTareas(List<Task<IOrigen>> tareas)
        {
            var resultados = await Task.WhenAll(tareas);

            foreach (var resultado in resultados)
            {

                switch (resultado.Tipo)
                {
                    case Origenes.BcraDeudores:
                        BcraDeudores = (Dictionary<long, BcraDeudores>)resultado.Registros;
                        break;
                    case Origenes.ListaNegra:
                        ListaNegra = (Dictionary<long, ListaNegra>)resultado.Registros;
                        break;
                    case Origenes.ClientesDepurados:
                        ClientesDepurados = (Dictionary<string, ClientesDepurados>)resultado.Registros;
                        break;
                    case Origenes.Clientes:
                        Clientes = (Dictionary<string, Clientes>)resultado.Registros;
                        break;
                    case Origenes.ValidacionActividad:
                        ValidacionActividad = (Dictionary<long, ValidacionActividad>)resultado.Registros;
                        break;
                    case Origenes.LkpIrregularidadesBcra:
                        LkpIrregularidadesBcra = (Dictionary<long, LkpIrregularidadesBcra>)resultado.Registros;
                        break;
                    case Origenes.EndeudamientoAtraso:
                        EndeudamientoAtraso = (Dictionary<long, EndeudamientoAtraso>)resultado.Registros;
                        break;
                    case Origenes.TablonIndividuos:
                        TablonIndividuos = (Dictionary<string, TablonIndividuos>)resultado.Registros;
                        break;
                    case Origenes.LimiteTransitorio:
                        LimiteTransitorio = (Dictionary<int, LimiteTransitorio>)resultado.Registros;
                        break;
                    case Origenes.Intradiarias:
                        var intradiarias = (Dictionary<int, IntradiariasSumarizadas>)resultado.Registros;
                        foreach (var i in intradiarias)
                        {
                            Intradiarias[i.Key] = i.Value;
                        }
                        break;
                    case Origenes.CalificacionConstante:
                        CalificacionConstante = (Dictionary<string, CalificacionConstante>)resultado.Registros;
                        break;
                    case Origenes.CampañasExceptuadas:
                        CampañasExceptuadas = (Dictionary<string, CampañasExceptuadas>)resultado.Registros;
                        break;
                    case Origenes.Precacom:
                        Precacom = (Dictionary<long, Precacom>)resultado.Registros;
                        break;
                    case Origenes.MoraSucursal:
                        MoraSucursal = (Dictionary<long, MoraSucursal>)resultado.Registros;
                        break;
                    case Origenes.Behavior:
                        Behavior = (Dictionary<int, Behavior>)resultado.Registros;
                        break;
                    case Origenes.ScoreComportamiento:
                        ScoreComportamiento = (Dictionary<long, ScoreComportamiento>)resultado.Registros;
                        break;
                    case Origenes.Tablon:
                        Tablon = (Dictionary<long, Tablon>)resultado.Registros;
                        break;
                    case Origenes.ChequesRechazados:
                        ChequesRechazados = (Dictionary<long, ChequesRechazados>)resultado.Registros;
                        break;
                    case Origenes.Excepciones:
                        Excepciones = (Dictionary<string, Excepciones>)resultado.Registros;
                        break;
                    case Origenes.LkpConvenios:
                        LkpConvenios = (Dictionary<int, LkpConvenios>)resultado.Registros;
                        break;
                    case Origenes.CpAnalisis:
                        CpAnalisis = (Dictionary<long, CpAnalisis>)resultado.Registros;
                        break;
                    case Origenes.CpAuxiliar:
                        CpAuxiliar = (Dictionary<long, CpAuxiliar>)resultado.Registros;
                        break;
                    case Origenes.CacheMapi:
                        CacheMapi = (Dictionary<string, CacheMapi>)resultado.Registros;
                        break;
                    case Origenes.ConveniosCodDescuento:
                        ConveniosCodDescuento = (Dictionary<int, ConveniosCodDescuento>)resultado.Registros;
                        break;
                    case Origenes.CalificacionConvenios:
                        CalificacionConvenios = (Dictionary<string, CalificacionConvenios>)resultado.Registros;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
