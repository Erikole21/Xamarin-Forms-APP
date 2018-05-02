using ClientePeatonXamarin.Code;
using ClientePeatonXamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ClientePeatonXamarin.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            MisRecogidasCommand = new Command(MisRecogidas);
            verOpciones = true;
        }

        public void Buscar()
        {
            if (!string.IsNullOrEmpty(numeroGuia))
            {
                Navegacion.PushAsync(new Views.SigueEnvioPage(new SigueEnvioViewModel(numeroGuia, Navegacion)));
            }
        }

        public async void LeerCodigoBarras()
        {
            var scanner = DependencyService.Get<IQrCodeScanningService>();
            if (scanner != null)
            {
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    NumeroGuia = result;
                    Buscar();
                }
            }
        }

        private async void MisRecogidas()
        {
            if (Application.Current.Properties.ContainsKey("UsuarioActivo") && Application.Current.Properties["UsuarioActivo"] != null)
                await Navegacion.PushAsync(new Views.MisRecogidas(new MisRecogidasViewModel(Navegacion)));
            else
            {
                await Navegacion.PushAsync(new Views.LoginRecogidasPage(new LoginViewModel(Navegacion, true)));
                Configuracion.Mensaje("Ingrese sus datos, e intente de nuevo para realizar la consulta en el sistema");

            }
        }

        private bool verOpciones;

        public bool VerOpciones
        {
            get { return verOpciones; }
            set
            {
                verOpciones = value;
                OnPropertyChanged("VerOpciones");
            }
        }


        private string numeroGuia;

        public string NumeroGuia
        {
            get { return numeroGuia; }
            set
            {
                numeroGuia = value;
                OnPropertyChanged("NumeroGuia");
            }
        }

        public ICommand MisRecogidasCommand { get; set; }

        public void NavegarRecogidas()
        {
            if (Application.Current.Properties.ContainsKey("UsuarioActivo") && Application.Current.Properties["UsuarioActivo"] != null)
            {
                Navegacion.PushAsync(new Views.RecogidasPage(new RecogidasViewModel(Navegacion)));
            }
            else
                Navegacion.PushAsync(new Views.LoginRecogidasPage(new LoginViewModel(Navegacion, false)));
        }


        public void NavegarCotizar()
        {
            Navegacion.PushAsync(new Views.CotizadorPage(new CotizadorViewModel(Navegacion)));
        }

        public void NavegarOficinas()
        {
            Navegacion.PushAsync(new Views.OficinasPage(new OficinasViewModel(Navegacion)));
        }

        public void NavegarVisitaComercial()
        {
            Navegacion.PushAsync(new Views.VisitaComercialPage(new VisitaComercialViewModel(Navegacion)));
        }

    }
}
