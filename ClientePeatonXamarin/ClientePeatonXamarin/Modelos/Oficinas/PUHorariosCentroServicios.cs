using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class PUHorariosCentroServicios
    {
        public string IdLocalidad { get; set; }
        public int IdHorarioCentroServicios { get; set; }
        public long IdCentroServicios { get; set; }
        public string IdDia { get; set; }
        public string NombreDia { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }        

        public string Descripcion { get { return string.Format("{0} de {1} a {2}", NombreDia, HoraInicio.ToString("hh:mm tt"), HoraFin.ToString("hh:mm tt")); } }

    }
}
