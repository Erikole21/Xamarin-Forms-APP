using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class TAFormaPagoServicioDC
    {
        public int IdServicio { get; set; }
        public List<TAFormaPagoDC> FormaPago { get; set; }
    }
}
