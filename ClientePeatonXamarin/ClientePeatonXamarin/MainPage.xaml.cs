using ClientePeatonXamarin.Services;
using ClientePeatonXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClientePeatonXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            if (Application.Current.Properties.ContainsKey("Puntos"))
                Application.Current.Properties.Remove("Puntos");

            InitializeComponent();
            InicializarEnlaces();
            ConsultarPuntos();
        }

        private async void ConsultarPuntos()
        {
            var puntos = await Services.OficinasService.Instancia.ObtenerPuntosActivos();
            if (puntos != null)
                Application.Current.Properties["Puntos"] = puntos;
            else
            {
                Configuracion.Mensaje("Revise la conexion a internet o intente de nuevo.");
                Application.Current.Properties["Puntos"] = false;
            }

        }

        private void InicializarEnlaces()
        {
            MainViewModel vm = this.BindingContext as MainViewModel;
            vm.Navegacion = Navigation;
            TapGestureRecognizer tapImage = new TapGestureRecognizer();
            tapImage.Tapped += (sender, e) =>
            {
                vm.Buscar();
            };
            buscar.GestureRecognizers.Add(tapImage);
            TapGestureRecognizer tapImageCodigo = new TapGestureRecognizer();
            tapImageCodigo.Tapped += (sender, e) =>
            {
                vm.LeerCodigoBarras();
            };
            codigoBarras.GestureRecognizers.Add(tapImageCodigo);
            TapGestureRecognizer tapRecogidas = new TapGestureRecognizer();
            tapRecogidas.Tapped += (sender, e) =>
            {
                vm.NavegarRecogidas();
            };
            recogida.GestureRecognizers.Add(tapRecogidas);
            TapGestureRecognizer tapImageCotiza = new TapGestureRecognizer();
            tapImageCotiza.Tapped += (sender, e) =>
            {
                vm.NavegarCotizar();
            };
            cotiza.GestureRecognizers.Add(tapImageCotiza);
            TapGestureRecognizer tapImageoficina = new TapGestureRecognizer();
            tapImageoficina.Tapped += (sender, e) =>
            {
                vm.NavegarOficinas();
            };
            oficina.GestureRecognizers.Add(tapImageoficina);
            TapGestureRecognizer tapVisita = new TapGestureRecognizer();
            tapVisita.Tapped += (sender, e) =>
              {
                  vm.NavegarVisitaComercial();

              };
            comercial.GestureRecognizers.Add(tapVisita);
            TapGestureRecognizer tapFacebook = new TapGestureRecognizer();
            tapFacebook.Tapped += (sender, e) =>
            {
                Device.OpenUri(new Uri("https://www.facebook.com/interrapidisimo"));
            };
            facebook.GestureRecognizers.Add(tapFacebook);
            TapGestureRecognizer tapTwitter = new TapGestureRecognizer();
            tapTwitter.Tapped += (sender, e) =>
            {
                Device.OpenUri(new Uri("https://twitter.com/Interrapidisimo"));
            };
            Twitter.GestureRecognizers.Add(tapTwitter);

            TapGestureRecognizer tapInstagram = new TapGestureRecognizer();
            tapInstagram.Tapped += (sender, e) =>
            {
                Device.OpenUri(new Uri("https://www.instagram.com/interrapidisimo_co/"));
            };
            Instagram.GestureRecognizers.Add(tapInstagram);
        }


    }
}
