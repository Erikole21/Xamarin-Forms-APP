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
    public partial class VisitaComercialPage : ContentPage
    {
        public VisitaComercialPage()
        {
            InitializeComponent();
        }

        public VisitaComercialPage(ViewModels.VisitaComercialViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
            TapGestureRecognizer tapportafolio = new TapGestureRecognizer();
            tapportafolio.Tapped += (sender, e) =>
            {
                vm.PortafolioNavegacion();
            };
            portafolio.GestureRecognizers.Add(tapportafolio);
            TapGestureRecognizer tapFranquicia = new TapGestureRecognizer();
            tapFranquicia.Tapped += (sender, e) =>
              {
                  vm.FranquiciaNavegacion();
              };
            franquicia.GestureRecognizers.Add(tapFranquicia);
        }



    }
}