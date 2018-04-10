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
    public partial class FranquiciaPage : ContentPage
    {
        public FranquiciaPage()
        {
            InitializeComponent();
        }

        public FranquiciaPage(ViewModels.VisitaComercialViewModel vM)
        {
            InitializeComponent();
            this.BindingContext = vM;
            TapGestureRecognizer tapLink = new TapGestureRecognizer();
            tapLink.Tapped += (sender, e) =>
            {
                vM.VerRequisitosFranqisia();
            };
            requisitos.GestureRecognizers.Add(tapLink);
            TapGestureRecognizer tapFoto = new TapGestureRecognizer();
            tapFoto.Tapped += (sender, e) =>
            {
                vM.CapturarFoto(1);
            };
            this.fotoCaptura1.GestureRecognizers.Add(tapFoto);
            TapGestureRecognizer tapFoto2 = new TapGestureRecognizer();
            tapFoto2.Tapped += (sender, e) =>
            {
                vM.CapturarFoto(2);
            };
            this.fotoCaptura2.GestureRecognizers.Add(tapFoto2);
            TapGestureRecognizer tapFoto3 = new TapGestureRecognizer();
            tapFoto3.Tapped += (sender, e) =>
            {
                vM.CapturarFoto(3);
            };
            this.fotoCaptura3.GestureRecognizers.Add(tapFoto3);
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (sender, e) =>
            {
                vM.ObtenerDireccionActual();
            };
            ubicacion.GestureRecognizers.Add(tap);
        }

        private async void Handle_OnSuggestionOpen(object sender, EventArgs e)
        {
            await MainScroll.ScrollToAsync((Element)sender, ScrollToPosition.End, true);
        }
    }
}