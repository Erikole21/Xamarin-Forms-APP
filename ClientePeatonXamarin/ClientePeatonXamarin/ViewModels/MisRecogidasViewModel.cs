using Acr.UserDialogs;
using ClientePeatonXamarin.Modelos;
using ClientePeatonXamarin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ClientePeatonXamarin.ViewModels
{
    public class MisRecogidasViewModel : MainViewModel
    {
        public MisRecogidasViewModel()
        {
        }

        private async void ConsultarRecogidas()
        {
            if (Application.Current.Properties.ContainsKey("UsuarioActivo") && Application.Current.Properties["UsuarioActivo"] != null)
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Consultando..", null, null, true, MaskType.Black))
                {
                    var listaRecogidas = await Services.RecogidasService.Instancia.ConsultarRecogidasUsuario(Application.Current.Properties["UsuarioActivo"].ToString());
                    Recogidas = new ObservableCollection<Modelos.RGRecogidaEsporadicaDC>(listaRecogidas);
                }

            }
            else            
                Configuracion.Mensaje("Ingrese sus datos en solicitar recogidas, e intente de nuevo para realizar la consulta en el sistema");                            
        }

        public MisRecogidasViewModel(INavigation _navegacion)
        {
            this.Navegacion = _navegacion;
            CancelarRecogidaCommand = new Command<Modelos.RGRecogidaEsporadicaDC>(CancelarRecogida);
            ConsultarRecogidas();
        }

        private ObservableCollection<Modelos.RGRecogidaEsporadicaDC> recogidas;

        public ObservableCollection<Modelos.RGRecogidaEsporadicaDC> Recogidas
        {
            get { return recogidas; }
            set
            {
                recogidas = value;
                OnPropertyChanged("Recogidas");
            }
        }


        private async void CancelarRecogida(Modelos.RGRecogidaEsporadicaDC sender)
        {
            SolicitudConMotivoRequest cancelacion = new SolicitudConMotivoRequest();
            cancelacion.IdMotivo = 2;
            cancelacion.IdActor = 3;
            cancelacion.IdSolicitudRecogida = sender.IdSolRecogida.Value;
            cancelacion.LocalidadCambio = sender.IdLocalidad;

            using (IProgressDialog progress = UserDialogs.Instance.Loading("Cancelando..", null, null, true, MaskType.Black))
                await Services.RecogidasService.Instancia.CancelarServicio(cancelacion);

            ConsultarRecogidas();
            Configuracion.Notificar("La solicitud fué cancelada.");

        }


        public ICommand CancelarRecogidaCommand { get; set; }
    }
}
