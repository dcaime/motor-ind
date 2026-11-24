namespace EvaluaIndividuos.Model
{
    public sealed class Politica
    {
        public static Politica MercadoAbierto = new("Politica_MercadoAbierto", 1, 639, 87, 0, 0, 6);
        public static Politica PlanSueldoPrivado = new("Politica_Privado", 2, 642, 119, 0, 0, 6);
        public static Politica RentaPresunta = new("Politica_RentaPresunta", 3, 214, 87, 0, 0, 6);
        public static Politica PlanSueldoPublico = new("Politica_Publico", 7, 679, 125, 0, 0, 6);
        public static Politica Actividad = new("Actividad", 9, 0, 0, 0, 0, 0);
        public static Politica EvaluaIndividuos = new("Politica_EvaluaIndividuos", 10, 411, 27, 3, 4, 15);

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
}
