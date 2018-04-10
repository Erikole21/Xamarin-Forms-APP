using Acr.UserDialogs;
using ClientePeatonXamarin.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using ClientePeatonXamarin.Modelos;
using System.Threading.Tasks;

namespace ClientePeatonXamarin.ViewModels
{
    public class OficinasViewModel : MainViewModel
    {
        public OficinasViewModel()
        {

        }

        public OficinasViewModel(INavigation navegacion)
        {
            this.Navegacion = navegacion;
            Ubicacion = true;
            CargarCiudades();
        }

        private async Task ConsultarPuntos()
        {
            using (IProgressDialog progress = UserDialogs.Instance.Loading("Consultando puntos..", null, null, true, MaskType.Black))
            {
                if (Application.Current.Properties.ContainsKey("Puntos") && Application.Current.Properties["Puntos"] is List<PUCentroServicioInfoGeneral>)
                {
                    var puntos = Application.Current.Properties["Puntos"] as List<PUCentroServicioInfoGeneral>;
                    PuntosInter = new ObservableCollection<Modelos.PUCentroServicioInfoGeneral>(puntos);
                }
                else
                {
                    var puntos = await Services.OficinasService.Instancia.ObtenerPuntosActivos();
                    if (puntos != null)
                        PuntosInter = new ObservableCollection<Modelos.PUCentroServicioInfoGeneral>(puntos);
                }
            }

        }

        private Plugin.Geolocator.Abstractions.Position PosicionActual;

        private async Task<bool> CargarPosicionActual()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsGeolocationEnabled)
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Ubicando..", null, null, true, MaskType.Black))
                {
                    if (PosicionActual == null)
                    {
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 50;
                        PosicionActual = await locator.GetPositionAsync(TimeSpan.FromSeconds(20));
                    }                    
                }
            }

            return true;
        }

        private async Task<bool> CargarPosicionDireccion()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsGeolocationEnabled)
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Ubicando..", null, null, true, MaskType.Black))
                {
                    Geocoder geoCoder = new Geocoder();
                    if (!string.IsNullOrWhiteSpace(DireccionRecogida))
                    {
                        var posiciones = await geoCoder.GetPositionsForAddressAsync(DireccionRecogida);
                        if (posiciones.Count() > 0)
                            PosicionActual = new Plugin.Geolocator.Abstractions.Position(posiciones.FirstOrDefault().Latitude, posiciones.FirstOrDefault().Longitude);
                        else
                            await UbicarPorCiudad(geoCoder);

                    }
                    else
                        await UbicarPorCiudad(geoCoder);
                }
            }

            return true;
        }

        private async Task<bool> UbicarPorCiudad(Geocoder geoCoder)
        {
            if (puntosInter != null)
            {
                var agenciaDepartamento = puntosInter.FirstOrDefault(p => p.IdLocalidad == ciudadSeleccionada.IdLocalidad && p.Tipo == "AGE");
                if (agenciaDepartamento != null)
                {
                    if (agenciaDepartamento.Longitud != 0 && agenciaDepartamento.Latitud != 0)
                        PosicionActual = new Plugin.Geolocator.Abstractions.Position(Convert.ToDouble(agenciaDepartamento.Latitud), Convert.ToDouble(agenciaDepartamento.Longitud));
                    else
                    {
                        var posiciones = await geoCoder.GetPositionsForAddressAsync(agenciaDepartamento.LocalidadNombre);
                        if (posiciones.Count() > 0)
                            PosicionActual = new Plugin.Geolocator.Abstractions.Position(posiciones.FirstOrDefault().Latitude, posiciones.FirstOrDefault().Longitude);
                        else
                            Configuracion.Notificar("No se pudo encontrar la ubicación ingresada");
                    }
                }
                else
                    Configuracion.Notificar("No se pudo encontrar la ubicación ingresada");
            }


            return true;
        }

        private bool ubicacion;

        public bool Ubicacion
        {
            get { return ubicacion; }
            set
            {

                ubicacion = value;
                CiudadSeleccionada = null;
                otroLugar = !ubicacion;
                OnPropertyChanged("OtroLugar");
                OnPropertyChanged("Ubicacion");
            }
        }

        private bool otroLugar;

        public bool OtroLugar
        {
            get { return otroLugar; }
            set
            {
                PosicionActual = null;
                otroLugar = value;
                ubicacion = !value;
                OnPropertyChanged("Ubicacion");
                OnPropertyChanged("OtroLugar");
            }
        }

        private string direccionRecogida;

        public string DireccionRecogida
        {
            get { return direccionRecogida; }
            set
            {
                direccionRecogida = value;
                OnPropertyChanged("DireccionRecogida");
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

        private ObservableCollection<Modelos.PUCentroServicioInfoGeneral> puntosInter;

        public ObservableCollection<Modelos.PUCentroServicioInfoGeneral> PuntosInter
        {
            get { return puntosInter; }
            set
            {
                puntosInter = value;
                OnPropertyChanged("PuntosInter");
            }
        }

        private ObservableCollection<Modelos.PUCentroServicioInfoGeneral> puntosCercanosRango;

        public ObservableCollection<Modelos.PUCentroServicioInfoGeneral> PuntosCercanosRango
        {
            get { return puntosCercanosRango; }
            set
            {
                puntosCercanosRango = value;
                OnPropertyChanged("PuntosCercanosRango");
            }
        }

        private bool verDatos;

        public bool VerDatos
        {
            get { return verDatos; }
            set
            {
                verDatos = value;
                OnPropertyChanged("VerDatos");
            }
        }

        private string departamentoBusqueda;

        public string DepartamentoBusqueda
        {
            get { return departamentoBusqueda; }
            set
            {
                departamentoBusqueda = value;
                OnPropertyChanged("DepartamentoBusqueda");
            }
        }

        private async void CargarCiudades()
        {
            var lista = await Services.RecogidasService.Instancia.ConsultarCiudadesColombia();
            if (lista != null)
                Ciudades = new ObservableCollection<Modelos.PALocalidadDC>(lista);
            else
                Configuracion.Mensaje("No fue posible conectar con el servidor, Intente de nuevo");
        }

        public async void Consultar()
        {
            VerDatos = false;
            await EstablecerPuntoCercanos(false);
        }

        private async Task EstablecerPuntoCercanos(bool verMapa)
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsGeolocationEnabled)
            {
                if (PuntosInter == null)
                    await ConsultarPuntos();

                if (puntosInter != null)
                {
                    puntosCercanosRango = new ObservableCollection<PUCentroServicioInfoGeneral>();
                    if (Ubicacion)
                    {
                        await CargarPosicionActual();
                        await CalcularPuntosRangoMetros(4000, verMapa);
                    }
                    else
                    {
                        //llenar segun ubicacion seleccionada   
                        if (CiudadSeleccionada != null)
                        {
                            await CargarPosicionDireccion();
                            await CalcularPuntosRangoMetros(8000, verMapa);
                        }
                        else
                            Configuracion.Notificar("Seleccione ciudad e ingrese dirección");
                    }
                }
            }
            else
                Configuracion.Notificar("Habilite la ubicación en su dispositivo");
        }

        private async Task CalcularPuntosRangoMetros(int rangoMetros, bool verMapa)
        {
            await Task.Run(() =>
            {
                if (PosicionActual != null && puntosInter != null)
                {
                    using (IProgressDialog progress = UserDialogs.Instance.Loading("Calculando puntos cercanos..", null, null, true, MaskType.Black))
                    {
                        double latitudRadio, longitudRadio, distanciaMetros, distancia, aproximacion;
                        int radioTierra = 6378137;
                        ObservableCollection<PUCentroServicioInfoGeneral> puntosFiltrados = null;

                        if (CiudadSeleccionada != null)
                            puntosFiltrados = new ObservableCollection<PUCentroServicioInfoGeneral>(puntosInter.Where(p => p.IdLocalidad == ciudadSeleccionada.IdLocalidad));
                        else
                            puntosFiltrados = puntosInter;

                        foreach (var punto in puntosFiltrados)
                        {
                            latitudRadio = Rad(Convert.ToDouble(punto.Latitud) - PosicionActual.Latitude);
                            longitudRadio = Rad(Convert.ToDouble(punto.Longitud) - PosicionActual.Longitude);
                            distancia = Math.Sin(latitudRadio / 2) *
                                        Math.Sin(latitudRadio / 2) +
                                        Math.Cos(Rad(PosicionActual.Latitude)) *
                                        Math.Cos(Rad(Convert.ToDouble(punto.Latitud))) *
                                        Math.Sin(longitudRadio / 2) *
                                        Math.Sin(longitudRadio / 2);
                            aproximacion = 2 * Math.Atan2(Math.Sqrt(distancia), Math.Sqrt(1 - distancia));
                            distanciaMetros = radioTierra * aproximacion;
                            if (distanciaMetros <= rangoMetros)
                            {
                                punto.Distancia = distanciaMetros;
                                puntosCercanosRango.Add(punto);

                                if (PuntosCercanosRango.Count >= 80)
                                    break;
                            }
                        }

                        CargarInformacionProcesada(verMapa);
                    }
                }
            });
        }

        private string contactoAgencia;

        public string ContactoAgencia
        {
            get { return contactoAgencia; }
            set
            {
                contactoAgencia = value;
                OnPropertyChanged("ContactoAgencia");
            }
        }

        private ObservableCollection<string> horariosAgencia;

        public ObservableCollection<string> HorariosAgencia
        {
            get { return horariosAgencia; }
            set
            {
                horariosAgencia = value;
                OnPropertyChanged("HorariosAgencia");
            }
        }

        private async void CargarInformacionProcesada(bool verMapa)
        {
            if (puntosCercanosRango?.Count > 0)
            {
                if (!verMapa)
                {
                    using (IProgressDialog progress = UserDialogs.Instance.Loading("Cargando información..", null, null, true, MaskType.Black))
                    {
                        var agenciaDepartamento = puntosInter.FirstOrDefault(p => p.IdLocalidad == puntosCercanosRango.FirstOrDefault().IdLocalidad && p.Tipo == "AGE");
                        ContactoAgencia = string.Format("DIR: {0}, TEL: {1}-{2}", agenciaDepartamento?.DireccionCentroServicio, agenciaDepartamento?.Telefono1, agenciaDepartamento?.Telefono2);
                        DepartamentoBusqueda = string.Format("LA OFICINA PRINCIPAL EN: {0}", ciudades.FirstOrDefault(c => c.IdLocalidad == puntosCercanosRango.FirstOrDefault().IdLocalidad)?.Nombre);
                        var horarios = await Services.OficinasService.Instancia.ObtenerHorarioCentroServicio(agenciaDepartamento.IdCentroServicios);
                        HorariosAgencia = new ObservableCollection<string>(horarios);

                        puntosCercanosRango = new ObservableCollection<PUCentroServicioInfoGeneral>(puntosCercanosRango.OrderBy(p => p.Distancia));
                        OnPropertyChanged("PuntosCercanosRango");

                        VerDatos = true;
                    }
                }
            }
            else
                Configuracion.Notificar("No se encontraron puntos cercanos al punto enviado.");
        }


        private double Rad(double valor)
        {
            return valor * Math.PI / 180;
        }

        private ObservableCollection<Pin> puntos;

        public ObservableCollection<Pin> Puntos
        {
            get { return puntos; }
            set
            {
                puntos = value;
                OnPropertyChanged("Puntos");
            }
        }

        private Posicion ubicacioInicial;

        public Posicion UbicacioInicial
        {
            get { return ubicacioInicial; }
            set
            {
                ubicacioInicial = value;
                OnPropertyChanged("UbicacioInicial");
            }
        }

        private void CargarInformacionMapa()
        {
            if (puntosInter != null)
            {
                UbicacioInicial = new Posicion(PosicionActual.Longitude, PosicionActual.Latitude);
                puntos = new ObservableCollection<Pin>(puntosCercanosRango.Select(s => new Pin()
                {
                    Address = s.DireccionCentroServicio,
                    Position = new Position(Convert.ToDouble(s.Latitud), Convert.ToDouble(s.Longitud)),
                    Type = PinType.SearchResult,
                    Label = string.Format("Telefonos:{0}-{1}", s.Telefono1, s.Telefono2)
                }));
                puntos.Add(new Pin()
                {
                    Address = "Tu Ubicación",
                    Position = new Position(UbicacioInicial.Latitud, UbicacioInicial.Longitud),
                    Type = PinType.SearchResult,
                    Label = "Cliente Inter Rapidísimo"
                });
                OnPropertyChanged("Puntos");
            }
            else
                Configuracion.Mensaje("No fue posible establecer conexión con el servidor, intente de nuevo.");
        }

        public async void VerMapa()
        {
            await EstablecerPuntoCercanos(true);
            await Navegacion.PushAsync(new Views.MapaPage(this));
            CargarInformacionMapa();
        }
    }
}
