using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBA.Model
{
    [Keyless]
    [Table("lkp_convenios")]
    public class LkpConvenios
    {
        [Column("Convenio")]
        public string? Convenio { get; set; }

        [Column("Numero_Convenio")]
        public int? NumeroConvenio { get; set; }

        [Column("Tipo_Convenio")]
        public string? TipoConvenio { get; set; }

        [Column("Tipo")]
        public string? Tipo { get; set; }

        [Column("Destino_Fondos")]
        public string? DestinoFondos { get; set; }

        [Column("Debito_Automatico")]
        public string? DebitoAutomatico { get; set; }

        [Column("Grupo_Convenio")]
        public string? GrupoConvenio { get; set; }

        [Column("Grupo")]
        public string? Grupo { get; set; }

        [Column("Grupo_Contable")]
        public string? GrupoContable { get; set; }

        [Column("Fecha_De_Inicio")]
        public DateTime? FechaDeInicio { get; set; }

        [Column("Fecha_De_Fin")]
        public DateTime? FechaDeFin { get; set; }

        [Column("Estado")]
        public string? Estado { get; set; }

        [Column("Renovacion_Automatica")]
        public string? RenovacionAutomatica { get; set; }

        [Column("Duracion_Convenio")]
        public int? DuracionConvenio { get; set; }

        [Column("Debita_Comision")]
        public string? DebitaComision { get; set; }

        [Column("Esquema_Comision")]
        public string? EsquemaComision { get; set; }

        [Column("Minimo_Mensual")]
        public int? MinimoMensual { get; set; }

        [Column("Minimo_Liquidacion")]
        public int? MinimoLiquidacion { get; set; }

        [Column("Distribucion_Comision")]
        public string? DistribucionComision { get; set; }

        [Column("Forma_Distribucion")]
        public string? FormaDistribucion { get; set; }

        [Column("Sucursal_Administradora")]
        public int? SucursalAdministradora { get; set; }

        [Column("Porcentaje_Administradora")]
        public decimal? PorcentajeAdministradora { get; set; }

        [Column("Porcentaje_Sucursal_Convenio")]
        public decimal? PorcentajeSucursalConvenio { get; set; }

        [Column("Porcentaje_Sucursal_Cobradoras")]
        public decimal? PorcentajeSucursalCobradoras { get; set; }

        [Column("Peridiocidad_Rendicion")]
        public string? PeridiocidadRendicion { get; set; }

        [Column("Desplazamiento_Central")]
        public int? DesplazamientoCentral { get; set; }

        [Column("Desplazamiento_General")]
        public string? DesplazamientoGeneral { get; set; }

        [Column("Coparticipable")]
        public string? Coparticipable { get; set; }

        [Column("Servicios_Asociados")]
        public string? ServiciosAsociados { get; set; }

        [Column("Causal_Caja_Ahorro")]
        public string? CausalCajaAhorro { get; set; }

        [Column("Comportamiento_Caja_Ahorro")]
        public string? ComportamientoCajaAhorro { get; set; }

        [Column("Causal_Cuenta_Corriente")]
        public int? CausalCuentaCorriente { get; set; }

        [Column("Comportamiento_Cuenta_Corriente")]
        public string? ComportamientoCuentaCorriente { get; set; }

        [Column("Cliente_Key")]
        public int? ClienteKey { get; set; }

        [Column("Convenio_Key")]
        public int? ConvenioKey { get; set; }

        [Column("Sucursal_Convenio_Key")]
        public int? SucursalConvenioKey { get; set; }

        [Column("Cuenta_Key")]
        public int? CuentaKey { get; set; }

        [Column("Productos_Asociados")]
        public string? ProductosAsociados { get; set; }

        [Column("Producto_Key")]
        public int? ProductoKey { get; set; }

        [Column("Especie_Key")]
        public int? EspecieKey { get; set; }

        [Column("Grupo_Convenio_Key")]
        public int? GrupoConvenioKey { get; set; }

        [Column("Calificacion")]
        public string? Calificacion { get; set; }

        [Column("Fecha_Convenio")]
        public DateTime? FechaConvenio { get; set; }

        [Column("Banco_Key")]
        public int? BancoKey { get; set; }
    }
}