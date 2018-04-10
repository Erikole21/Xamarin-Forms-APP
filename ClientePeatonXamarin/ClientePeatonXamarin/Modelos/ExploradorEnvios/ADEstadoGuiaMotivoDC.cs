using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class ADEstadoGuiaMotivoDC
    {
        public long? IdTrazaGuia { get; set; }

        public ADTrazaGuia EstadoGuia { get; set; }

        public ADMotivoGuiaDC Motivo { get; set; }

        public DateTime FechaMotivo { get; set; }
    }
}
