using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class RegistroSolicitud
    {
        public RASolicitudDC Solicitud { get; set; }
        public List<RAAdjuntoDC> Adjuntos { get; set; }
        public InformacionGestion informacionGestion { get; set; }
        public Dictionary<string, object> parametrosParametrizacion { get; set; }
        public string idCiudad { get; set; }        
        public int idSistema { get; set; }
        public int idTipoNovedad { get; set; }
    }
}
