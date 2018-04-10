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
	public partial class CotizadorPage : ContentPage
	{
        public CubicaPage cubica = new CubicaPage();
        public CotizadorPage ()
		{
			InitializeComponent ();
      	}
       

        public CotizadorPage(CotizadorViewModel vm)
        {
            InitializeComponent();
            InicializarEnlaces(vm);
            this.BindingContext = vm;

            
        }

     
        public void InicializarEnlaces(CotizadorViewModel vm)
        {
            TapGestureRecognizer tapImageVolumetrico = new TapGestureRecognizer();
            tapImageVolumetrico.Tapped += (sender, e) => { CargarPesoCubico(); };
            cubicar.GestureRecognizers.Add(tapImageVolumetrico);                      
        }

        public async void CargarPesoCubico()
        {           
            cubica = new Views.CubicaPage(this.BindingContext as CotizadorViewModel);
            await Navigation.PushAsync(cubica);
        }

        public void CalcularValorComercialPorPeso()
        {
            CotizadorViewModel vm = this.BindingContext as CotizadorViewModel;
            vm.CalcularValorComercialPorPeso();
        }

        public void CargarValorComercialPorPeso()
        {
            CotizadorViewModel vm = this.BindingContext as CotizadorViewModel;
            vm.CargarValorComercialPorPeso();
        }

        private async void Handle_OnSuggestionOpen(object sender, EventArgs e)
        {
            await Scroll.ScrollToAsync((Element)sender, ScrollToPosition.End, true);
        }

    }
}