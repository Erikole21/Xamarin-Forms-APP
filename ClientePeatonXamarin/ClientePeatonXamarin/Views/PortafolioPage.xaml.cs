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
	public partial class PortafolioPage : ContentPage
	{
		public PortafolioPage ()
		{
			InitializeComponent ();
		}

        public PortafolioPage(ViewModels.VisitaComercialViewModel vM)
        {
            InitializeComponent();
            this.BindingContext = vM;
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