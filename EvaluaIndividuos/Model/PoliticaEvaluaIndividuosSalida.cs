namespace EvaluaIndividuos.Model
{
    public class PoliticaEvaluaIndividuosSalida
    {
        public int Origen_ID { get; set; }
        public long Numero_Oportunidad { get; set; }
        public int Numero_Evaluacion { get; set; }
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public string Es_Cliente { get; set; }
        public int Cobis { get; set; }
        public long Cuil { get; set; }
        public string Rama { get; set; }
        public string Dictamen { get; set; }
        public string Codigos_Rechazo { get; set; }
        public string Explicacion { get; set; }
        public decimal Prestamo_Monto_Maximo { get; set; }
        public decimal Prestamo_Cuota_Maxima { get; set; }
        public int Prestamo_Plazo_Maximo { get; set; }
        public string Prestamo_Tipo_Tasa { get; set; }
        public decimal Prestamo_Tna { get; set; }
        public decimal Tarjeta_Credito_Limite { get; set; }
        public decimal Cuenta_Corriente_Limite { get; set; }
        public string Reporte_Politica { get; set; }
        public string Output_Adicional { get; set; }
        public string Nivel_de_Riesgo { get; set; }
        public decimal Ingreso_Computable { get; set; }
        public string Version_Orquestador { get; set; }
        public string Tipo_Prestamo { get; set; }
    }
}
