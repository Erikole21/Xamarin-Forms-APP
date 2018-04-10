using ClientePeatonXamarin.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Acr.UserDialogs;
using ClientePeatonXamarin.Services;
using Plugin.Media;
using System.Threading.Tasks;

namespace ClientePeatonXamarin.ViewModels
{
    public class VisitaComercialViewModel : ViewModelBase
    {
        public VisitaComercialViewModel(INavigation navigation)
        {
            this.Navegacion = navigation;
            GuardarPortafolioCommand = new Command(GuardarPortafolio);
            GuardarFranquiciaCommand = new Command(GuardarFranquicia);
            if (Application.Current.Properties.ContainsKey("UsuarioActivo"))
                Usuario = Application.Current.Properties["UsuarioActivo"].ToString();

            CargarCiudades();
        }

        private void Iniciarlizar()
        {
            FotosCargadas = string.Empty;
            FechaMinima = DateTime.Now;
            FechaRecogida = DateTime.Now.AddHours(2);
            HoraRecogida = DateTimeToTimeSpan(FechaRecogida);
            Observaciones = string.Empty;
        }

        public void PortafolioNavegacion()
        {
            Iniciarlizar();
            Navegacion.PushAsync(new Views.PortafolioPage(this));
        }

        public void FranquiciaNavegacion()
        {
            Iniciarlizar();
            Captura1 = Captura2 = Captura3 = string.Empty;
            Navegacion.PushAsync(new Views.FranquiciaPage(this));
        }

        public void VerRequisitosFranqisia()
        {
            Navegacion.PushAsync(new Views.RequisitosFranquiciaPage());
        }

        public ICommand GuardarPortafolioCommand { get; set; }

        public ICommand GuardarFranquiciaCommand { get; set; }

        public async void ObtenerDireccionActual()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsGeolocationEnabled)
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Ubicando..", null, null, true, MaskType.Black))
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;
                    Geocoder geoCoder = new Geocoder();
                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(48));
                    posicion = new Position(position.Latitude, position.Longitude);
                    var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(posicion);
                    DireccionRecogida = possibleAddresses.FirstOrDefault();
                }
            }
            else
                Configuracion.Notificar("Habilite la ubicación en su dispositivo");
        }


        private async void CargarCiudades()
        {
            var lista = await Services.RecogidasService.Instancia.ConsultarCiudadesColombia();
            Ciudades = new ObservableCollection<Modelos.PALocalidadDC>(lista);
            ConsultarHistorialRecogidaCliente();
        }

        private async void ConsultarHistorialRecogidaCliente()
        {
            RGRecogidasDC ultima = await Services.RecogidasService.Instancia.ConsultarUltimaSolicitud(usuario);
            if (ultima != null)
            {
                NombreCompletoPersona = ultima.Nombre;
                EmailPersona = ultima.Correo;
                CelularPersona = ultima.NumeroTelefono;
                CiudadSeleccionada = ciudades?.FirstOrDefault(d => d.IdLocalidad == ultima.Ciudad);
                DireccionRecogida = ultima.Direccion;
                PreguntarPor = ultima.PreguntarPor;
                if (!string.IsNullOrEmpty(ultima.Latitud) && !string.IsNullOrEmpty(ultima.Longitud))
                    posicion = new Position(Convert.ToDouble(ultima.Latitud), Convert.ToDouble(ultima.Longitud));
            }
        }

        public void GuardarFranquicia()
        {
            if (ValidarFormulario())
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Enviando información..", null, null, true, MaskType.Black))
                {
                    RegistroSolicitud registroSolicitud = new RegistroSolicitud();
                    RASolicitudDC solicitudDC = new RASolicitudDC();
                    solicitudDC.IdParametrizacionRap = Configuracion.ConfigRaps.IdParamEstudioFranquicia;
                    solicitudDC.IdCargoSolicita = "0";
                    solicitudDC.IdCargoResponsable = "0";
                    solicitudDC.FechaCreacion = DateTime.Now;
                    solicitudDC.FechaInicio = DateTime.Now;
                    solicitudDC.IdEstado = RAEnumEstados.Creada;
                    solicitudDC.Descripcion = "Solicitud de Estudio de Franquicia";
                    solicitudDC.IdSolicitudPadre = 0;
                    solicitudDC.DocumentoSolicita = "0";
                    solicitudDC.idSucursal = "111";
                    solicitudDC.IdCiudad = "11001000";
                    InformacionGestion informacionGestion = new InformacionGestion();
                    informacionGestion.DocumentoGestiona = "0";
                    informacionGestion.IdCargoGestiona = "0";
                    informacionGestion.DocumentoGestiona = "0";
                    informacionGestion.IdCargoGestiona = "0";
                    informacionGestion.CorreoGestiona = emailPersona;
                    informacionGestion.IdCargoDestino = "0";
                    informacionGestion.CorreoDestino = "";
                    informacionGestion.DocumentoDestino = "0";
                    informacionGestion.IdUsuario = "ContactoComercialFranquicias";
                    informacionGestion.Comentario =
                        string.Format("Documento/Nit: No. {0}\n Nombre: {1}\n Telefono: {2}\n Email: {3}\n Ciudad: {4}\n Direccion: {5}\n FechaVisita: {6} \n PreguntarPor: {7}\n Observaciones: {8}",
                                            usuario, NombreCompletoPersona, CelularPersona, emailPersona, CiudadSeleccionada.Nombre, DireccionRecogida, FechaRecogida.ToString("yyyy-MM-dd hh:mm tt"), PreguntarPor, observaciones);

                    registroSolicitud.parametrosParametrizacion = new Dictionary<string, object>();
                    registroSolicitud.parametrosParametrizacion.Add("", "");
                    registroSolicitud.Adjuntos = new List<RAAdjuntoDC>();
                    AgregarAdjuntos(registroSolicitud.Adjuntos);
                    registroSolicitud.Solicitud = solicitudDC;
                    registroSolicitud.informacionGestion = informacionGestion;
                    registroSolicitud.idCiudad = ciudadSeleccionada.IdLocalidad;
                    GuardarSolicitud(registroSolicitud);
                }
            }           
        }

        private async void GuardarSolicitud(RegistroSolicitud registroSolicitud)
        {
            long id = await Services.VisitaComercialService.Instancia.GuardarRecogida(registroSolicitud);
            if (id > 0)
            {
                Configuracion.Mensaje(string.Format("Su solicitud ha sido realizada, en el transcurso del día uno de nuestros agentes comerciales se comunicará con usted."));
                await Navegacion.PopAsync();
            }
            else
            {
                Configuracion.Notificar("Verifique su conexión a internet e intente de nuevo");
            }
        }

        private void AgregarAdjuntos(List<RAAdjuntoDC> lista)
        {
            if (!string.IsNullOrEmpty(Captura1))
            {
                RAAdjuntoDC adjunto1 = new RAAdjuntoDC();
                adjunto1.NombreArchivo = "fotoFachadaUno";
                adjunto1.Adjunto = Captura1;
                adjunto1.AdjuntoBase64 = Captura1;
                adjunto1.Extension = "jpg";
                lista.Add(adjunto1);
            }

            if (!string.IsNullOrEmpty(Captura2))
            {
                RAAdjuntoDC adjunto2 = new RAAdjuntoDC();
                adjunto2.NombreArchivo = "fotoFachadaDos";
                adjunto2.Adjunto = Captura2;
                adjunto2.AdjuntoBase64 = Captura2;
                adjunto2.Extension = "jpg";
                lista.Add(adjunto2);
            }

            if (!string.IsNullOrEmpty(Captura3))
            {
                RAAdjuntoDC adjunto3 = new RAAdjuntoDC();
                adjunto3.NombreArchivo = "fotoFachadaTres";
                adjunto3.Adjunto = Captura3;
                adjunto3.AdjuntoBase64 = Captura3;
                adjunto3.Extension = "jpg";
                lista.Add(adjunto3);
            }
        }


        public bool ValidarFormulario()
        {
            if (!string.IsNullOrEmpty(CelularPersona) && !string.IsNullOrEmpty(NombreCompletoPersona) && !string.IsNullOrEmpty(DireccionRecogida)
               && !string.IsNullOrEmpty(EmailPersona) && !string.IsNullOrEmpty(CelularPersona) && !string.IsNullOrEmpty(observaciones))
            {
                if (fechaRecogida > DateTime.Now && fechaRecogida.Subtract(DateTime.Now).TotalHours > 1)
                {
                    if (fechaRecogida.Hour >= 8 && fechaRecogida.Hour < 17)
                    {
                        if (CiudadSeleccionada != null)
                        {
                            if (CelularPersona.Length == 10)
                                return true;
                            else
                                Configuracion.Notificar("La longitud del número de celular no es válida debe ser de 10 caracteres)");

                        }
                        else
                            Configuracion.Notificar("Seleccione la ciudad de recogida");
                    }
                    else
                        Configuracion.Notificar("La hora de visita debe estar entre las 8:00 am y las 5:00 pm");
                }
                else
                    Configuracion.Mensaje("Debe seleccionar mínimo 1 hora despues de la hora actual para generar la Solicitud");
            }
            else
                Configuracion.Notificar("Complete los datos.");

            return false;
        }

        public async void CapturarFoto(Byte numeroCaptura)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Configuracion.Notificar("No se detecta camara para capturar foto");
                return;
            }


            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "InterRapidisimo",
                Name = string.Format("Franquicia{0}.jpg", numeroCaptura)
            });

            if (file == null)
                return;

            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                file.GetStream().CopyTo(memory);
                switch (numeroCaptura)
                {
                    case 1:
                        Captura1 = Convert.ToBase64String(memory.ToArray());
                        break;
                    case 2:
                        Captura2 = Convert.ToBase64String(memory.ToArray());
                        break;
                    case 3:
                        Captura3 = Convert.ToBase64String(memory.ToArray());
                        break;
                    default:
                        break;
                }
            }

            byte cargadas = 0;

            if (!string.IsNullOrEmpty(Captura1))
                cargadas++;

            if (!string.IsNullOrEmpty(Captura2))
                cargadas++;

            if (!string.IsNullOrEmpty(Captura3))
                cargadas++;

            FotosCargadas = string.Format("# Fotos cargadas {0}", cargadas);
        }


        private string fotosCargadas;

        public string FotosCargadas
        {
            get { return fotosCargadas; }
            set
            {
                fotosCargadas = value;
                OnPropertyChanged("FotosCargadas");
            }
        }

        public string Captura1 { get; set; }

        public string Captura2 { get; set; }

        public string Captura3 { get; set; }

        public void GuardarPortafolio()
        {
            if (ValidarFormulario())
            {
                GuardarPortafolioValidado();
            }            
        }

        private void GuardarPortafolioValidado()
        {
            using (IProgressDialog progress = UserDialogs.Instance.Loading("Enviando información..", null, null, true, MaskType.Black))
            {
                RegistroSolicitud registroSolicitud = new RegistroSolicitud();
                RASolicitudDC solicitudDC = new RASolicitudDC();
                solicitudDC.IdParametrizacionRap = Configuracion.ConfigRaps.IdParamVisitaComercial;
                solicitudDC.IdCargoSolicita = "0";
                solicitudDC.IdCargoResponsable = "0";
                solicitudDC.FechaCreacion = DateTime.Now;
                solicitudDC.FechaInicio = DateTime.Now;
                solicitudDC.IdEstado = RAEnumEstados.Creada;
                solicitudDC.Descripcion = "Solicitud de Portafolio de Servicios";
                solicitudDC.IdSolicitudPadre = 0;
                solicitudDC.DocumentoSolicita = "0";
                solicitudDC.idSucursal = "111";
                solicitudDC.IdCiudad = "11001000";
                InformacionGestion informacionGestion = new InformacionGestion();
                informacionGestion.DocumentoGestiona = "0";
                informacionGestion.IdCargoGestiona = "0";
                informacionGestion.DocumentoGestiona = "0";
                informacionGestion.IdCargoGestiona = "0";
                informacionGestion.CorreoGestiona = emailPersona;
                informacionGestion.IdCargoDestino = "0";
                informacionGestion.CorreoDestino = "";
                informacionGestion.DocumentoDestino = "0";
                informacionGestion.IdUsuario = "ContactoComercial";
                informacionGestion.Comentario =
                    string.Format("Documento/Nit: No. {0}\n Nombre: {1}\n Telefono: {2}\n Email: {3}\n Ciudad: {4}\n Direccion: {5}\n FechaVisita: {6} \n PreguntarPor: {7}\n Observaciones: {8}",
                                        usuario, NombreCompletoPersona, CelularPersona, emailPersona, CiudadSeleccionada.Nombre, DireccionRecogida, FechaRecogida.ToString("yyyy-MM-dd hh:mm tt"), PreguntarPor, observaciones);

                registroSolicitud.parametrosParametrizacion = new Dictionary<string, object>();
                registroSolicitud.parametrosParametrizacion.Add("", "");
                registroSolicitud.Solicitud = solicitudDC;
                registroSolicitud.informacionGestion = informacionGestion;
                registroSolicitud.idCiudad = ciudadSeleccionada.IdLocalidad;
                GuardarSolicitud(registroSolicitud);
            }
        }

        Position posicion;

        private string nombreCompletoPersona;

        public string NombreCompletoPersona
        {
            get { return nombreCompletoPersona; }
            set
            {
                nombreCompletoPersona = value;
                OnPropertyChanged("NombreCompletoPersona");
            }
        }

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged("Usuario");
            }
        }

        private ObservableCollection<Modelos.PALocalidadDC> ciudades;

        public ObservableCollection<Modelos.PALocalidadDC> Ciudades
        {
            get { return ciudades; }
            set
            {
                ciudades = value;
                OnPropertyChanged("Ciudades");
            }
        }

        private Modelos.PALocalidadDC ciudadSeleccionada;

        public Modelos.PALocalidadDC CiudadSeleccionada
        {
            get { return ciudadSeleccionada; }
            set
            {
                ciudadSeleccionada = value;
                OnPropertyChanged("CiudadSeleccionada");
            }
        }

        private string direccionRecogida;

        public string DireccionRecogida
        {
            get { return direccionRecogida; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    direccionRecogida = value;
                    OnPropertyChanged("DireccionRecogida");
                }
            }
        }
        private string emailPersona;

        public string EmailPersona
        {
            get { return emailPersona; }
            set
            {
                emailPersona = value;
                OnPropertyChanged("EmailPersona");
            }
        }

        private string celularPersona;

        public string CelularPersona
        {
            get { return celularPersona; }
            set
            {
                celularPersona = value;
                OnPropertyChanged("CelularPersona");
            }
        }

        private string preguntarPor;

        public string PreguntarPor
        {
            get { return preguntarPor; }
            set
            {
                preguntarPor = value;
                OnPropertyChanged("PreguntarPor");
            }
        }

        private TimeSpan? horaRecogida;

        public TimeSpan? HoraRecogida
        {
            get { return horaRecogida; }
            set
            {
                horaRecogida = value;
                if (value.HasValue)
                {
                    FechaRecogida = new DateTime(fechaRecogida.Year, fechaRecogida.Month, fechaRecogida.Day, value.Value.Hours, value.Value.Minutes, value.Value.Seconds);
                }
                OnPropertyChanged("HoraRecogida");
            }
        }

        public static TimeSpan? DateTimeToTimeSpan(DateTime dt)
        {
            TimeSpan FResult;
            try
            {
                FResult = dt - dt.Date;
            }
            catch
            {
                return null;
            }
            return FResult;
        }


        private DateTime fechaRecogida;

        public DateTime FechaRecogida
        {
            get { return fechaRecogida; }
            set
            {
                fechaRecogida = value;
                OnPropertyChanged("FechaRecogida");
            }
        }

        public DateTime FechaMinima { get; set; }

        private string observaciones;

        public string Observaciones
        {
            get { return observaciones; }
            set
            {
                observaciones = value;
                OnPropertyChanged("Observaciones");
            }
        }

    }
}
