using Acr.UserDialogs;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Collections.ObjectModel;
using Plugin.Media;
using System.Windows.Input;
using ClientePeatonXamarin.Modelos;
using ClientePeatonXamarin.Services;

namespace ClientePeatonXamarin.ViewModels
{
    public class RecogidasViewModel : MainViewModel
    {
        public RecogidasViewModel()
        {

        }

        public RecogidasViewModel(INavigation _navegacion)
        {
            this.Navegacion = _navegacion;
            DocumentoPersona = Application.Current.Properties["UsuarioActivo"].ToString();
            CargarCiudades();
            FechaMinima = DateTime.Now;
            FechaRecogida = DateTime.Now.AddHours(2);
            HoraRecogida = DateTimeToTimeSpan(FechaRecogida);
            GuardarRecogidaCommand = new Command(Guardar);
        }

        private async void ConsultarHistorialRecogidaCliente()
        {
            RGRecogidasDC ultima = await Services.RecogidasService.Instancia.ConsultarUltimaSolicitud(documentoPersona);
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

        private async void CargarCiudades()
        {
            var lista = await Services.RecogidasService.Instancia.ConsultarCiudadesColombia();
            Ciudades = new ObservableCollection<Modelos.PALocalidadDC>(lista);
            ConsultarHistorialRecogidaCliente();
        }

        private void Guardar()
        {
            if (!string.IsNullOrEmpty(CelularPersona) && !string.IsNullOrEmpty(NombreCompletoPersona) && !string.IsNullOrEmpty(DireccionRecogida)
                && !string.IsNullOrEmpty(EmailPersona) && !string.IsNullOrEmpty(CelularPersona) && !string.IsNullOrEmpty(DescripcionEnvios)
                && !string.IsNullOrEmpty(TotalPiezas) && !string.IsNullOrEmpty(PesoAproximado) && !string.IsNullOrEmpty(PreguntarPor))
            {

                if (fechaRecogida > DateTime.Now && fechaRecogida.Subtract(DateTime.Now).TotalHours > 1)
                {
                    if (CiudadSeleccionada != null)
                    {
                        if (CelularPersona.Length == 10)
                        {
                            GuardarRecogidaValidada();
                        }
                        else
                            Configuracion.Notificar("La longitud del Número de celular no es válida debe ser de 10 caracteres)");
                    }
                    else
                        Configuracion.Notificar("Seleccione la ciudad de recogida");
                }
                else
                {
                    Configuracion.Mensaje("Debe seleccionar mínimo 1 hora despues de la hora actual para generar la recogida.");
                }
            }
            else
                Configuracion.Notificar("Complete los datos.");

        }

        private async void GuardarRecogidaValidada()
        {
            using (IProgressDialog progress = UserDialogs.Instance.Loading("Enviando información..", null, null, true, MaskType.Black))
            {
                RGRecogidasDC recogida = new RGRecogidasDC();
                recogida.TipoDocumento = "CC";
                recogida.NumeroDocumento = DocumentoPersona;
                recogida.Nombre = nombreCompletoPersona.ToUpper();
                recogida.Direccion = string.Format("{0} {1}", DireccionRecogida, complementoDireccion);
                recogida.Correo = EmailPersona;
                recogida.NumeroTelefono = CelularPersona;
                recogida.Ciudad = CiudadSeleccionada.IdLocalidad;
                recogida.FechaRecogida = FechaRecogida;
                recogida.Hora = fechaRecogida;
                recogida.TotalPiezas = Convert.ToInt32(TotalPiezas);
                recogida.PesoAproximado = Convert.ToInt32(PesoAproximado);
                recogida.DescripcionEnvios = DescripcionEnvios;
                recogida.Cantidad = 1;
                if (posicion != null)
                {
                    recogida.Latitud = posicion.Latitude.ToString();
                    recogida.Longitud = posicion.Longitude.ToString();
                }
                recogida.TipoRecogida = RGEnumTipoRecogidaDC.Esporadica;
                recogida.PreguntarPor = PreguntarPor;

                long id = await Services.RecogidasService.Instancia.GuardarRecogida(recogida, documentoPersona);
                if (id > 0)
                {
                    await Navegacion.PopAsync();
                    Configuracion.Mensaje(string.Format("Su recogida ha sido aceptada. Su clave de seguridad es: {0}  Suminístresela al mensajero, al momento de la recogida.", documentoPersona.Substring(documentoPersona.Length - 2)));
                               
                }
                else
                {
                    Configuracion.Notificar("Verifique su conexión a internet e intente de nuevo");
                }
            }
        }

        private string complementoDireccion;

        public string ComplementoDireccion
        {
            get { return complementoDireccion; }
            set
            {
                complementoDireccion = value;
                OnPropertyChanged("ComplementoDireccion");
            }
        }


        private string documentoPersona;

        public string DocumentoPersona
        {
            get { return documentoPersona; }
            set
            {
                documentoPersona = value;
                OnPropertyChanged("DocumentoPersona");
            }
        }

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

        private string descripcionEnvios;

        public string DescripcionEnvios
        {
            get { return descripcionEnvios; }
            set
            {
                descripcionEnvios = value;
                OnPropertyChanged("DescripcionEnvios");
            }
        }

        private string totalPiezas;

        public string TotalPiezas
        {
            get { return totalPiezas; }
            set
            {
                totalPiezas = value;
                OnPropertyChanged("TotalPiezas");
            }
        }

        private string pesoAproximado;

        public string PesoAproximado
        {
            get { return pesoAproximado; }
            set
            {
                pesoAproximado = value;
                OnPropertyChanged("PesoAproximado");
            }
        }

        Position posicion;

        private string ImagenCapturada;

        public async void CapturarFoto()
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
                Name = "Recogida.jpg"
            });

            if (file == null)
                return;

            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                file.GetStream().CopyTo(memory);
                ImagenCapturada = Convert.ToBase64String(memory.ToArray());
            }

            Configuracion.Mensaje("Foto capturada correctamente.");
        }





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

        public ICommand GuardarRecogidaCommand { get; set; }

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
                if (horaRecogida.HasValue)
                    fechaRecogida = new DateTime(value.Year, value.Month, value.Day, horaRecogida.Value.Hours, horaRecogida.Value.Minutes, horaRecogida.Value.Seconds);
                else
                    fechaRecogida = value;

                OnPropertyChanged("FechaRecogida");
            }
        }


        public DateTime FechaMinima { get; set; }



    }
}
