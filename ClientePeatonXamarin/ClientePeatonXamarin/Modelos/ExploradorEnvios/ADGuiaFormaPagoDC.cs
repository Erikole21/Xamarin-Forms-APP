using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class ADGuiaFormaPagoDC
    {
        public ADGuiaFormaPagoDC()
        {

        }
        public short IdFormaPago { get; set; }

        public string Descripcion { get; set; }

        public decimal Valor { get; set; }
    }
}
