using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class ADTrazaGuia
    {
        public ADTrazaGuia()
        {

        }

        public long? IdAdmision { get; set; }

        public string Observaciones { get; set; }

        public long IdCentroServicioEstado { get; set; }

        public string NombreCentroServicioEstado { get; set; }

        public long? NumeroGuia { get; set; }

        public short? IdEstadoGuia { get; set; }

        public string DescripcionEstadoGuia { get; set; }

        public string IdCiudad { get; set; }

        public string Ciudad { get; set; }

        public DateTime FechaAdmisionGuia { get; set; }

        public DateTime FechaGrabacion { get; set; }

        public string DescripcionFechaGrabacion { get { return FechaGrabacion.ToString("dd / MM / yyyy"); } }

        public DateTime? FechaEntrega { get; set; }

        public int IdOrigenApliacion { get; set; }

        public string ClaseEstado { get; set; }

        public string Usuario { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }
    }
}
