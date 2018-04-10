using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class RGRecogidaEsporadicaDC
    {
        public long? IdSolRecogida { get; set; }

        public DateTime FechaHoraRecogida { get; set; }

        public string DescripcionFechaHoraRecogida { get { return FechaHoraRecogida.ToString("dd/MM/yyyy hh:mm tt"); } }

        public DateTime FechaGrabacion { get; set; }

        public string DescripcionFechaGrabacion { get { return FechaGrabacion.ToString("dd/MM/yyyy hh:mm tt"); } }

        public string DireccionRecogida { get; set; }

        public string EstadoRecogida { get; set; }

        public string DescripcionEstado { get; set; }

        public string Mensajero { get; set; }

        public string IdLocalidad { get; set; }

        public string Longitud { get; set; }

        public string Latitud { get; set; }

        public string UrlImage { get; set; }

        public int IdEstadoRecogida { get; set; }

        public bool CancelarVisible
        {
            get
            {
                if (IdEstadoRecogida != 4
                    && IdEstadoRecogida != 11
                    && IdEstadoRecogida != 7
                    && IdEstadoRecogida != 8)
                    return true;
                else
                    return false;
            }
        }
    }
}
