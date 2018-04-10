using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class RAAdjuntoDC
    {
        
        public int IdAdjunto { get; set; }

        
        public long IdSolicitud { get; set; }

        
        public string NombreArchivo { get; set; }

        
        public long IdGestion { get; set; }

        
        public Decimal Tamaño { get; set; }

        
        public string Adjunto { get; set; }

        
        public string Extension { get; set; }

        
        public string UbicacionNombre { get; set; }

        
        public string AdjuntoBase64 { get; set; }

        
        public long IdCita { get; set; }

        
        public DateTime? FechaCreacion { get; set; }

        
        public string CreadoPor { get; set; }
    }
}
