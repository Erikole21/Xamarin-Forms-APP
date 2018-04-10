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
    public partial class OficinasPage : ContentPage
    {
        public OficinasPage()
        {
            InitializeComponent();
        }

        public OficinasPage(ViewModels.OficinasViewModel vM)
        {
            InitializeComponent();
            this.BindingContext = vM;
            TapGestureRecognizer tapConsultar = new TapGestureRecognizer();
            tapConsultar.Tapped += (sender, e) =>
            {
                vM.Consultar();
            };
            Consultar.GestureRecognizers.Add(tapConsultar);
            TapGestureRecognizer tapVermapa = new TapGestureRecognizer();
            tapVermapa.Tapped += (sender, e) =>
              {
                  vM.VerMapa();
              };
            VerMapa.GestureRecognizers.Add(tapVermapa);

        }

        private async void Handle_OnSuggestionOpen(object sender, EventArgs e)
        {
            await MainScroll.ScrollToAsync((Element)sender, ScrollToPosition.End, true);
        }
    }
}