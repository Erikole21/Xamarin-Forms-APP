using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class SolicitudConMotivoRequest
    {
        public int IdActor { get; set; }
        public int IdMotivo { get; set; }

        public long IdSolicitudRecogida { get; set; }

        public string LocalidadCambio { get; set; }
    }
}
