using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class RestablecerPasswordRequest
    {
        public string IdUsuario { get; set; }
        public string Password { get; set; }
        public long Pin { get; set; }
    }
}
