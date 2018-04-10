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
    public partial class SigueEnvioPage : ContentPage
    {
        public SigueEnvioPage()
        {
            InitializeComponent();
        }

        public SigueEnvioPage(SigueEnvioViewModel vM)
        {
            InitializeComponent();
            this.BindingContext = vM;
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (sender, e) =>
            {
                vM.Compartir();
            };
            this.Compartir.GestureRecognizers.Add(tap);
        }
    }
}