using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class RASolicitudDC
    {

        public long IdSolicitud { get; set; }


        public long IdParametrizacionRap { get; set; }


        public long IdParametrizacionRapPadre { get; set; }


        public string IdCargoSolicita { get; set; }


        public string IdCargoResponsable { get; set; }


        public string IdResponsable { get; set; }


        public DateTime FechaCreacion { get; set; }


        public DateTime FechaInicio { get; set; }


        public DateTime FechaVencimiento { get; set; }


        public RAEnumEstados IdEstado { get; set; }


        public string Descripcion { get; set; }


        public long IdSolicitudPadre { get; set; }


        public string DocumentoSolicita { get; set; }


        public string DocumentoResponsable { get; set; }        


        public RACargoDC Cargo { get; set; }


        public List<RAParametrosParametrizacionDC> ParametrosSolicitud { get; set; }


        public string idSucursal { get; set; }


        public string NombreParametrizacionRapPadre { get; set; }

        public string IdCiudad { get; set; }

        public bool EsAcumulativa { get; set; }


        public String Anchor { get; set; }


        public int IdTipoEscalonamiento { get; set; }


        public bool EsRefalla { get; set; }


    }
}
