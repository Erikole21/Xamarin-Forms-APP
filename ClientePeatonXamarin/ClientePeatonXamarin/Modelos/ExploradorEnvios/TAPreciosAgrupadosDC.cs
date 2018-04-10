using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class TAPreciosAgrupadosDC
    {
        public int IdServicio { get; set; }

        public TAPrecioMensajeriaDC Precio { get; set; }

        public TAPrecioCargaDC PrecioCarga { get; set; }

        public string Mensaje { get; set; }

        public string NombreServicio { get; set; }

        public string TiempoEntrega { get; set; }

        public TAFormaPagoServicioDC FormaPagoServicio { get; set; }

        public DateTime fechaEntrega { get; set; }

        public string DescripcionFechaEntrega { get { return fechaEntrega.ToString("dd/MM/yyyy hh:mm tt"); } }

        public decimal Valortotal { get; set; }
    }
}
