namespace EvaluaIndividuos.Database.MEBI.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Estructura de la tabla DICTAMENES_INDIVIDUOS de MEBI
    /// </summary>
    [Table("DICTAMENES_INDIVIDUOS")]
    public class DictamenesIndividuos : IIdentity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int? Origen_ID { get; set; }
        public long? Numero_Oportunidad { get; set; }
        public int? Numero_Evaluacion { get; set; }
        public DateTime? Fecha_Hora { get; set; }
        public long? Comercio_ID { get; set; }
        public int? Comercio_Sucursal { get; set; }
        [MaxLength(50)]
        public string? Comercio_Usuario { get; set; }
        [MaxLength(50)]
        public string? Provincia { get; set; }
        public int? Sucursal_Macro_Asociada { get; set; }
        [MaxLength(20)]
        public string? Canal { get; set; }
        [MaxLength(100)]
        public string? Productos_Precancelados { get; set; }
        [MaxLength(2)]
        public string? Preca_Utiliza { get; set; }
        [MaxLength(30)]
        public string? Preca_Codigo { get; set; }
        [MaxLength(40)]
        public string? Preca_Oferta { get; set; }
        [MaxLength(20)]
        public string? Circuito { get; set; }
        [MaxLength(30)]
        public string? Preca_Nombre { get; set; }
        [MaxLength(30)]
        public string? Preca_Consultar { get; set; }
        public int? Integrantes { get; set; }
        [MaxLength(50)]
        public string? Vivienda { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Alquiler_Monto { get; set; }
        [MaxLength(30)]
        public string? Categoria_Cliente { get; set; }
        [MaxLength(30)]
        public string? Numero_Convenio { get; set; }
        [MaxLength(100)]
        public string? Nombre_Convenio { get; set; }
        [MaxLength(50)]
        public string? Calificacion { get; set; }
        [MaxLength(30)]
        public string? Submercado { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Monto_Inversor { get; set; }
        [MaxLength(30)]
        public string? Tipo_Contribuyente { get; set; }
        [MaxLength(2)]
        public string? Tarjeta_Solicita { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Tarjeta_Limite { get; set; }
        [MaxLength(30)]
        public string? COD_TC { get; set; }
        [MaxLength(2)]
        public string? Prestamo_Solicita { get; set; }
        [MaxLength(50)]
        public string? Prestamo_Tipo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Prestamo_Monto { get; set; }
        [MaxLength(50)]
        public string? Prestamo_Destino { get; set; }
        public int? Prestamo_Plazo { get; set; }
        [MaxLength(20)]
        public string? Prestamo_Tipo_Tasa { get; set; }
        [Column(TypeName = "numeric(19, 2)")]
        public decimal? Prestamo_TNA { get; set; }
        [MaxLength(30)]
        public string? COD_PP { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Con_Garantia_Valor_Bien { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Con_Garantia_Subsidio { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Con_Garantia_Tasacion { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Hipotecario_Valor_Terreno { get; set; }
        public int? Modelo_Automovil { get; set; }
        [MaxLength(2)]
        public string? Acuerdo_Solicita { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Acuerdo_Monto { get; set; }
        [MaxLength(30)]
        public string? COD_CC { get; set; }
        [MaxLength(1000)]
        public string? Input_Adicional { get; set; }
        [MaxLength(1000)]
        public string? SAL_Output_Adicional { get; set; }
        [MaxLength(20)]
        public string? SAL_Status { get; set; }
        [MaxLength(1000)]
        public string? SAL_Mensaje { get; set; }
        [MaxLength(20)]
        public string? SAL_Tipo_Cliente { get; set; }
        [MaxLength(50)]
        public string? SAL_Rama { get; set; }
        [MaxLength(20)]
        public string? SAL_Dictamen { get; set; }
        [MaxLength(1000)]
        public string? SAL_Codigos_Rechazo { get; set; }
        [MaxLength(1000)]
        public string? SAL_Explicacion { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SAL_Prestamo_Monto_Maximo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SAL_Prestamo_Cuota_Maxima { get; set; }
        public int? SAL_Prestamo_Plazo_Maximo { get; set; }
        [Column(TypeName = "numeric(19, 2)")]
        public decimal? SAL_Prestamo_TNA { get; set; }
        [MaxLength(20)]
        public string? SAL_Prestamo_Tipo_Tasa { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SAL_Tarjeta_Credito_Limite { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SAL_Cuenta_Corriente_Limite { get; set; }
        [MaxLength(20)]
        public string? BP_Categoria { get; set; }
        [MaxLength(20)]
        public string? BP_Clase { get; set; }
        [MaxLength(50)]
        public string? CAL_Version { get; set; }
        [MaxLength(50)]
        public string? CAL_Codigo_Campana { get; set; }
        [MaxLength(50)]
        public string? CAL_Nombre_Campana { get; set; }
        [MaxLength(50)]
        public string? CAL_Tipo_Campana { get; set; }
        [MaxLength(500)]
        public string? CAL_Descripcion { get; set; }
        public DateTime? CAL_Fecha_Inicio { get; set; }
        public DateTime? CAL_Fecha_Fin { get; set; }
        [MaxLength(30)]
        public string? CAL_Segmento { get; set; }
        [MaxLength(30)]
        public string? CAL_Subsegmento { get; set; }
        [MaxLength(30)]
        public string? CAL_Sucursal_Asignada_Venta { get; set; }
        [MaxLength(30)]
        public string? CAL_Canal { get; set; }
        [MaxLength(30)]
        public string? CAL_Objetivo { get; set; }
        [MaxLength(30)]
        public string? CAL_Convenios_Beneficios { get; set; }
        [MaxLength(30)]
        public string? CAL_TC_Producto { get; set; }
        [MaxLength(30)]
        public string? CAL_TC_Tipo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CAL_TC_Monto_Maximo { get; set; }
        public DateTime? CAL_TC_Fecha_Inicio { get; set; }
        public DateTime? CAL_TC_FechaVencimiento { get; set; }
        [MaxLength(30)]
        public string? CAL_P_Producto { get; set; }
        [MaxLength(30)]
        public string? CAL_P_Tipo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CAL_P_Monto_Maximo { get; set; }
        [Column(TypeName = "numeric(19, 2)")]
        public decimal? CAL_P_Tasa { get; set; }
        public int? CAL_P_Plazo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CAL_P_Afectacion { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CAL_P_Cuota_Inicial { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CAL_P_CuotaPromedio { get; set; }
        public DateTime? CAL_P_Fecha_Inicio { get; set; }
        public DateTime? CAL_P_Fecha_Vencimiento { get; set; }
        [MaxLength(30)]
        public string? CAL_AC_Producto { get; set; }
        [MaxLength(30)]
        public string? CAL_AC_Tipo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CAL_AC_Monto_Maximo { get; set; }
        public DateTime? CAL_AC_Fecha_Inicio { get; set; }
        public DateTime? CAL_AC_Fecha_Vencimiento { get; set; }
        [MaxLength(50)]
        public string? PRECACOM_Version { get; set; }
        public long? PRECACOM_Cuil { get; set; }
        public int? PRECACOM_Origen_ID { get; set; }
        public long? PRECACOM_Comercio_ID { get; set; }
        [MaxLength(30)]
        public string? PRECACOM_ID_Alternativo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PRECACOM_P_Monto_Maximo { get; set; }
        public int? PRECACOM_P_Plazo_Maximo { get; set; }
        [Column(TypeName = "numeric(19, 2)")]
        public decimal? PRECACOM_P_TNA { get; set; }
        [MaxLength(20)]
        public string? PRECACOM_P_Tipo_Tasa { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PRECACOM_P_Cuota_Maxima { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PRECACOM_TC_Limite { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PRECACOM_CC_Limite { get; set; }
        public DateTime? PRECACOM_Vigente_Desde { get; set; }
        public DateTime? PRECACOM_Vigente_Hasta { get; set; }
        public int? PRECACOM_Vigencia { get; set; }
        [MaxLength(100)]
        public string? PRECACOM_Adicional1 { get; set; }
        [MaxLength(100)]
        public string? PRECACOM_Adicional2 { get; set; }
        [MaxLength(100)]
        public string? PRECACOM_Adicional3 { get; set; }
        [MaxLength(100)]
        public string? Info_Liquidacion { get; set; }
        public int? Dias_Atraso { get; set; }
        [MaxLength(200)]
        public string? Duracion_Servicios { get; set; }
        [MaxLength(50)]
        public string? Version { get; set; }
        [MaxLength(17)]
        public string? CC_ID_OFERTAS { get; set; }
        [MaxLength(36)]
        public string? ID_Interno { get; set; }
        [MaxLength(50)]
        public string? ENT_NUC_Numero { get; set; }
        [MaxLength(50)]
        public string? ENT_NUC_Tipo { get; set; }
        [MaxLength(50)]
        public string? ENT_NUC_Medio { get; set; }
        [MaxLength(50)]
        public string? ENT_NUC_Adicional { get; set; }
    }
}
