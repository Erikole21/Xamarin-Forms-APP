using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class RAParametrosParametrizacionDC
    {
        
        public int idParametro { get; set; }

        
        public long idParametrizacionRap { get; set; }

        
        public string descripcionParametro { get; set; }

        
        public int idTipoDato { get; set; }

        
        public int longitud { get; set; }

        
        public int? idTipoNovedad { get; set; }

        
        public string Valor { get; set; }

        
        public bool EsAgrupamiento { get; set; }

        
        public long IdSolicitud { get; set; }

        
        public string DescripcionSolicitud { get; set; }

        
        public bool EsEncabezadoDescripcion { get; set; }

        
        public bool DescripcionReporte { get; set; }

        
        public int IdTipoParametro { get; set; }
    }
}
