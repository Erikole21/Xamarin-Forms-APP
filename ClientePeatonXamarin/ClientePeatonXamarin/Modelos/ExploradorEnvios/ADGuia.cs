using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class ADGuia
    {
        public ADGuia()
        {

        }

        public long IdTrazaGuia { get; set; }

        public long NumeroGuia { get; set; }

        public string IdCiudadDestino { get; set; }

        public string NombreCiudadDestino { get; set; }

        public string CodigoPostalDestino { get; set; }

        public decimal ValorDeclarado { get; set; }

        public DateTime FechaAdmision { get; set; }

        public string DescripcionFechaAdmision { get { return FechaAdmision.ToString("dd / MM / yyyy"); } }

        public decimal Peso { get; set; }

        public string NombreServicio { get; set; }

        public string PrefijoNumeroGuia { get; set; }

        public string TelefonoDestinatario { get; set; }

        public string DireccionDestinatario { get; set; }

        public CLClienteContadoDC Destinatario { get; set; }

        public CLClienteContadoDC Remitente { get; set; }

        public string DiceContener { get; set; }

        public string NombreMensajero { get; set; }

        public short DiasDeEntrega { get; set; }

        public decimal Alto { get; set; }

        public decimal Ancho { get; set; }

        public decimal Largo { get; set; }

        public DateTime FechaEstimadaEntrega { get; set; }

        public string IdPaisDestino { get; set; }

        public string NombrePaisOrigen { get; set; }

        public string CreadoPor { get; set; }

        public decimal ValorPrimaSeguro { get; set; }

        public DateTime FechaEntrega { get; set; }

        public string Observaciones { get; set; }

        public string NombreCiudadOrigen { get; set; }

        public string NombreTipoEnvio { get; set; }

        public string NumeroBolsaSeguridad { get; set; }

        public List<ADGuiaFormaPagoDC> FormasPago { get; set; }

        public ADTrazaGuia TrazaGuiaEstado { get; set; }

        public string GuidDeChequeo { get; set; }

        public decimal ValorTotal { get; set; }

        public bool EsAlCobro { get; set; }

        public bool EstaPagada { get; set; }

        public bool EsPesoVolumetrico { get; set; }

        public decimal ValorServicio { get; set; }

        public long IdCentroServicioOrigen { get; set; }

        public string NombreCentroServicioOrigen { get; set; }

        public long IdCentroServicioDestino { get; set; }

        public string NombreCentroServicioDestino { get; set; }

        public short IdTipoEnvio { get; set; }

        public short TotalPiezas { get; set; }

        public ADEnumTipoCliente TipoCliente { get; set; }

        public bool Supervision { get; set; }

        public DateTime FechaSupervision { get; set; }

        public long IdMensajero { get; set; }

        public string DescripcionTipoEntrega { get; set; }

        public bool EsRecomendado { get; set; }

        public string DigitoVerificacion { get; set; }

        public short NumeroPieza { get; set; }

        public string IdUnidadMedida { get; set; }

        public string IdUnidadNegocio { get; set; }

        public decimal ValorTotalImpuestos { get; set; }

        public decimal ValorTotalRetenciones { get; set; }

        public string IdTipoEntrega { get; set; }

        public string IdPaisOrigen { get; set; }

        public string NombrePaisDestino { get; set; }

        public string CodigoPostalOrigen { get; set; }

        public decimal ValorAdicionales { get; set; }

        public short? IdMotivoNoUsoBolsaSegurida { get; set; }

        public string MotivoNoUsoBolsaSeguriDesc { get; set; }

        public long NumeroGuiaDHL { get; set; }

        public string NoUsoaBolsaSeguridadObserv { get; set; }

        public decimal PesoLiqMasa { get; set; }

        public decimal PesoLiqVolumetrico { get; set; }

        public decimal ValorAdmision { get; set; }

        public int CantidadIntentosEntrega { get; set; }

        public DateTime FechaEstimadaEntregaNew { get; set; }

        public string DescripcionFechaEstimadaEntregaNew { get { return FechaEstimadaEntregaNew.ToString("dd / MM / yyyy"); } }

        public string FormasPagoDescripcion { get; set; }

        public bool EsAutomatico { get; set; }

        public long IdCentroServicioEstado { get; set; }

        public string NombreCentroServicioEstado { get; set; }

        public int IdServicio { get; set; }

        public bool EsSispostal { get; set; }


    }

}
