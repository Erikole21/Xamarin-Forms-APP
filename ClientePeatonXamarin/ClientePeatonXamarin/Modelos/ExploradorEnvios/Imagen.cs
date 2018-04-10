using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClientePeatonXamarin.Modelos
{
    public class Imagen
    {
        public Imagen()
        {

        }

        public Imagen(ImageSource source, string titulo)
        {
            this.Source = source;
            this.Titulo = titulo;
        }

        public ImageSource Source { get; set; }

        public string Titulo { get; set; }

    }
}
