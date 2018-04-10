using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class PUCentroServicioInfoGeneral
    {
        public string IdLocalidad { get; set; }
        public long IdCentroServicios { get; set; }

        public string NombreCentroServicio { get; set; }

        public string DireccionCentroServicio { get; set; }

        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }

        public decimal Longitud { get; set; }

        public decimal Latitud { get; set; }

        public string LocalidadNombre { get; set; }

        public string LocalidadNombreCompleto { get; set; }

        public string Barrio { get; set; }

        public string Tipo { get; set; }

        public bool EsReclameEnOficina { get; set; }

        public int EstadoRecogida { get; set; }

        public DateTime FechaHoraRecogida { get; set; }
        public double Distancia { get; set; }
        public List<PUHorariosCentroServicios> HorariosCentroServicios { get; set; }
    }

    public class Posicion
    {
        public Posicion(double lon, double lati)
        {
            this.Longitud = lon;
            this.Latitud = lati;
        }
        public double Longitud { get; set; }

        public double Latitud { get; set; }
    }
}
