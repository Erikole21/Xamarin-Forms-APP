using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class UsuarioContactoResponse
    {
        public string Celular { get; set; }
        public string Correo { get; set; }
        public EnumLoginEstados MensajeResultado { get; set; }
    }
}
