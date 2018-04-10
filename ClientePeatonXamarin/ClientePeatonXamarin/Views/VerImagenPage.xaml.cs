using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientePeatonXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerImagenPage : ContentPage
    {
        public VerImagenPage()
        {
            InitializeComponent();
        }

        public VerImagenPage(Modelos.Imagen source)
        {
            InitializeComponent();
            this.Title = source.Titulo;
            BindingContext = source;
        }
    }
}