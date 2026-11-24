using EvaluaIndividuos.Database;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Model
{
    public interface IEvaluable
    {
        string[] Codifica();
        void Decodifica(string[] arraySalida);
        Politica GetPolitica();
        void GarantizaValores();
        void InsertaDictamenes(IDictaminador dictamenesService);
    }
}
