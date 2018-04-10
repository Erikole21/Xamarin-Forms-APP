using ClientePeatonXamarin.ViewModels;

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
    public partial class CubicaPage : ContentPage
    {

        public CubicaPage()
        {
            InitializeComponent();
        }


        public CubicaPage(CotizadorViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;

        }

    }
}