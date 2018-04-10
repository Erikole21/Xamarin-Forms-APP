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
    public partial class RecogidasPage : ContentPage
    {
        public RecogidasPage()
        {
            InitializeComponent();
        }

        public RecogidasPage(ViewModels.RecogidasViewModel Vm)
        {
            InitializeComponent();
            this.BindingContext = Vm;
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (sender, e) =>
            {
                Vm.ObtenerDireccionActual();
            };
            ubicacion.GestureRecognizers.Add(tap);
            //TapGestureRecognizer tapFoto = new TapGestureRecognizer();
            //tapFoto.Tapped += (sender, e) =>
            //{
            //    Vm.CapturarFoto();
            //};
            //fotoCaptura.GestureRecognizers.Add(tapFoto);
            TapGestureRecognizer tapCotizar = new TapGestureRecognizer();
            tapCotizar.Tapped += (sender, e) =>
            {
                Navigation.PushAsync(new Views.CotizadorPage(new CotizadorViewModel(Vm.Navegacion)));
            };
            cotizar.GestureRecognizers.Add(tapCotizar);
        }

        private async void Handle_OnSuggestionOpen(object sender, EventArgs e)
        {
            await MainScroll.ScrollToAsync((Element)sender, ScrollToPosition.End, true);
        }

    }
}