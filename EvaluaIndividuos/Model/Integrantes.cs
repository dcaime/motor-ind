namespace EvaluaIndividuos.Model
{
    public class Integrantes
    {
        private readonly List<Integrante> _lista = new();

        public void Add(long numeroTributario, int cobis)
        {
            if (_lista.Count >= 4)
                throw new InvalidOperationException("Máximo 4 integrantes.");

            _lista.Add(new Integrante(numeroTributario, cobis, _lista.Count));
        }

        public Integrante? PorNumeroTributario(long nro) =>
            _lista.FirstOrDefault(x => x.NumeroTributario == nro);

        public Integrante? PorCobis(int cobis) =>
            _lista.FirstOrDefault(x => x.Cobis == cobis);

        public Integrante? PorIndice(int index) =>
            _lista.FirstOrDefault(x => x.Indice == index);

        public IEnumerable<Integrante> Todos => _lista;
        public Integrante? Titular => PorIndice(0);
        public IEnumerable<Integrante> Codeudores => _lista.Where(i => i.Indice > 0);
        public List<long> ListaNumerosTributarios => _lista.Select(i => i.NumeroTributario).ToList();
        public List<int> ListaCobis => _lista.Select(i => i.Cobis).ToList();
    }

    public class Integrante
    {
        public long NumeroTributario { get; init; }
        public int Indice { get; init; }
        public int Cobis { get; set; }

        public Integrante(long numeroTributario, int cobis, int indice)
        {
            NumeroTributario = numeroTributario;
            Cobis = cobis;
            Indice = indice;
        }
    }
}
