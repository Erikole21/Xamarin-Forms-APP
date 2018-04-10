using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class ONNovedadesTransporteDC
    {
        public ONNovedadesTransporteDC()
        {

        }

        public long IdManifiestoOperacionNacio { get; set; }

        public string NombreNovedad { get; set; }

        public string LugarIncidente { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaNovedad { get; set; }
        
        public string DescripcionFechaNovedad { get { return FechaNovedad.ToString("dd / MM / yyyy"); } }

        public string Tiempo { get; set; }

        public DateTime FechaEstimadaEntrega { get; set; }

        public string DescripcionFechaEstimadaEntrega { get { return FechaEstimadaEntrega.ToString("dd / MM / yyyy"); } }
    }
}
