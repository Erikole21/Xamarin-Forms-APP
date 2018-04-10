using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Acr.UserDialogs;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.IO;
using ClientePeatonXamarin.Modelos;

namespace ClientePeatonXamarin.ViewModels
{
    public class SigueEnvioViewModel : MainViewModel
    {

        public SigueEnvioViewModel(string _numeroGuia, INavigation navegacion)
        {
            this.NumeroGuia = _numeroGuia;
            this.Navegacion = navegacion;
            ConsultarDatos();
        }

        private async void ConsultarDatos()
        {
            if (!string.IsNullOrWhiteSpace(NumeroGuia))
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Consultando..", null, null, true, MaskType.Black))
                {
                    Modelos.ADRastreoGuiaDC rastroGuia = await Services.SigueEnvioService.Instancia.ObtenerRastreoGuia(NumeroGuia.Trim());
                    if (rastroGuia != null)
                        AsignarDatos(rastroGuia);
                    else
                    {
                        UserDialogs.Instance.Toast("El Número De Guía No Existe.", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                        await Navegacion.PopAsync(true);                     
                    }
                }
            }
        }

        private ADGuia guia;

        public ADGuia Guia
        {
            get { return guia; }
            set
            {
                guia = value;
                OnPropertyChanged("Guia");
                OnPropertyChanged("IsDHL");
                OnPropertyChanged("NoCredito");
                OnPropertyChanged("DescripcionFormaPago");
            }
        }

        public bool NoCredito { get { return guia?.FormasPago?.FirstOrDefault()?.Descripcion != "Crédito"; } }

        public bool IsDHL { get { return guia?.IdServicio == 11; } }

        private bool isSispostal;

        public bool IsSispostal
        {
            get { return isSispostal; }
            set
            {
                isSispostal = value;
                OnPropertyChanged("IsSispostal");
                OnPropertyChanged("NoSispostal");
            }
        }

        public bool NoSispostal
        {
            get { return !isSispostal; }
        }


        public string DescripcionFormaPago { get { return guia?.FormasPago?.FirstOrDefault()?.Descripcion; } }


        private void AsignarDatos(Modelos.ADRastreoGuiaDC rastroGuia)
        {
            CargarRastreoEnvio(rastroGuia);
            CargarTelemercadeo(rastroGuia);
            CargarImagenesVolantes(rastroGuia);
            CargarNovedadesTransporte(rastroGuia);
            IsSispostal = rastroGuia.EsSispostal;
            Guia = rastroGuia.Guia;
            CargarImagenPruebaEntrega(rastroGuia);
        }

        private void CargarImagenPruebaEntrega(ADRastreoGuiaDC rastroGuia)
        {
            ImagesPruebaEntrega = new ObservableCollection<Imagen>();
            if (!string.IsNullOrWhiteSpace(rastroGuia.ImagenGuia))
            {
                int borrar = 0;
                string imagen = string.Empty;
                if (rastroGuia.ImagenGuia.Contains("base64,"))
                {
                    borrar = rastroGuia.ImagenGuia.IndexOf("base64,") + 7;
                    imagen = rastroGuia.ImagenGuia.Substring(borrar, rastroGuia.ImagenGuia.Length - borrar);
                }
                else
                    imagen = rastroGuia.ImagenGuia;

                ImagesPruebaEntrega.Add(new Imagen(ImageSource.FromStream(
                         () => new MemoryStream(Convert.FromBase64String(imagen))), "Prueba de Entrega"));
            }
            else
            {
                ImagesPruebaEntrega.Add(new Imagen(ImageSource.FromResource("ClientePeatonXamarin.Resource.paraMayorInformacion.jpg", typeof(SigueEnvioViewModel).Assembly), "Contacto"));
            }

            OnPropertyChanged("ImagesPruebaEntrega");
        }

        private void CargarRastreoEnvio(ADRastreoGuiaDC rastroGuia)
        {
            var estadoGuia = rastroGuia.EstadosGuia.LastOrDefault();
            if (estadoGuia != null)
            {
                EstadoGuia = estadoGuia.EstadoGuia.DescripcionEstadoGuia;
                FechaEstado = estadoGuia.EstadoGuia.DescripcionFechaGrabacion;
            }

            if (rastroGuia.EstadosGuia?.Count > 0)
            {
                RastreoGuiaVisible = true;
                EstadosGuia = new ObservableCollection<ADEstadoGuiaMotivoDC>(rastroGuia.EstadosGuia);
            }
        }

        public void Compartir()
        {
            MessagingCenter.Send<ImageSource>(ImagesPruebaEntrega?.FirstOrDefault()?.Source, "Share");
        }

        private void CargarNovedadesTransporte(ADRastreoGuiaDC rastroGuia)
        {
            if (rastroGuia.NovedadesTransporte?.Count > 0)
            {
                NovedadesTransporte = new ObservableCollection<ONNovedadesTransporteDC>(rastroGuia.NovedadesTransporte);
                NovedadesTransporteVisible = true;
            }
            ////para ver como se ve en ejecucion con datos
            //else
            //{
            //    NovedadesTransporteVisible = true;
            //    NovedadesTransporte = new ObservableCollection<ONNovedadesTransporteDC>();
            //    NovedadesTransporte.Add(new ONNovedadesTransporteDC() { Descripcion = "Peruebna asdas asdasd asdasdasd asdasdasd asdsadasd asdasd", FechaEstimadaEntrega = DateTime.Now, FechaNovedad = DateTime.Now.AddDays(-1), NombreNovedad = "Perdida", LugarIncidente = "Bogota", Tiempo = "asdasd 214 horas" });
            //    NovedadesTransporte.Add(new ONNovedadesTransporteDC() { Descripcion = "Peruebna asdas asdasd asdasdasd asdasdasd asdsadasd asdasd", FechaEstimadaEntrega = DateTime.Now, FechaNovedad = DateTime.Now.AddDays(-1), NombreNovedad = "Perdida", LugarIncidente = "Bogota", Tiempo = "asdasd 214 horas" });
            //}
        }

        private void CargarTelemercadeo(ADRastreoGuiaDC rastroGuia)
        {
            if (rastroGuia.Telemercadeo?.Count > 0)
            {
                TelemercadeoVisible = true;
                Telemercadeos = new ObservableCollection<Modelos.LIGestionesDC>(rastroGuia.Telemercadeo);
            }
            //para ver como se ve en ejecucion con datos
            //else
            //{
            //    TelemercadeoVisible = true;
            //    Modelos.LIGestionesDC lIGestionesDC = new Modelos.LIGestionesDC()
            //    {
            //        FechaGestion = DateTime.Now,
            //        NuevaDireccion = "La nueva direccion prueba",
            //        Observaciones = "La persona afsafasd afsaafasf afasfafaf afasfafas afasfasfasf asfasfasf af",
            //        PersonaContesta = "Erik Manolo Rodriguez Lopez",
            //        Resultado = new Modelos.LIResultadoTelemercadeoDC() { Ciudad = "Bogota", Descripcion = "oasd asdasdsad asds", Estado = "asdsad" },
            //        Telefono = "213213213 - 123213"
            //    };
            //    Modelos.LIGestionesDC lIGestiones1DC = new Modelos.LIGestionesDC()
            //    {
            //        FechaGestion = DateTime.Now,
            //        NuevaDireccion = "La nueva direccion prueba",
            //        Observaciones = "La persona afsafasd afsaafasf afasfafaf afasfafas afasfasfasf asfasfasf af",
            //        PersonaContesta = "Erik Manolo Rodriguez Lopez",
            //        Resultado = new Modelos.LIResultadoTelemercadeoDC() { Ciudad = "Bogota", Descripcion = "oasd asdasdsad asds", Estado = "asdsad" },
            //        Telefono = "213213213 - 123213",
            //    };

            //    telemercadeos = new ObservableCollection<Modelos.LIGestionesDC>();
            //    telemercadeos.Add(lIGestionesDC);
            //    telemercadeos.Add(lIGestiones1DC);
            //    OnPropertyChanged("Telemercadeos");
            //}
        }

        private void CargarImagenesVolantes(ADRastreoGuiaDC rastroGuia)
        {
            //para probar como se ve visualmente
            //rastroGuia.Volantes = new List<string>();
            //rastroGuia.Volantes.Add("data:image/gif;base64,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");
            //rastroGuia.Volantes.Add("data:image/gif;base64,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");

            if (rastroGuia.Volantes?.Count > 0)
            {
                int borrar = 0;
                string imagen = string.Empty;
                images = new ObservableCollection<Imagen>();
                foreach (var imageBase64 in rastroGuia.Volantes)
                {
                    if (imageBase64.Contains("base64,"))
                    {
                        borrar = imageBase64.IndexOf("base64,") + 7;
                        imagen = imageBase64.Substring(borrar, imageBase64.Length - borrar);
                    }
                    else
                        imagen = imageBase64;

                    images.Add(new Imagen(Xamarin.Forms.ImageSource.FromStream(
                        () => new MemoryStream(Convert.FromBase64String(imagen))), "Intento Fallido Entrega"));
                }
                FallidosVisible = true;
                OnPropertyChanged("ImagesFallidos");
            }
        }

        private bool rastreoGuiaVisible;

        public bool RastreoGuiaVisible
        {
            get { return rastreoGuiaVisible; }
            set
            {
                rastreoGuiaVisible = value;
                OnPropertyChanged("RastreoGuiaVisible");
            }
        }


        private bool novedadesTransporteVisible;

        public bool NovedadesTransporteVisible
        {
            get { return novedadesTransporteVisible; }
            set
            {
                novedadesTransporteVisible = value;
                OnPropertyChanged("NovedadesTransporteVisible");
            }
        }


        private bool telemercadeoVisible;

        public bool TelemercadeoVisible
        {
            get { return telemercadeoVisible; }
            set
            {
                telemercadeoVisible = value;
                OnPropertyChanged("TelemercadeoVisible");
            }
        }

        private bool fallidosVisible;

        public bool FallidosVisible
        {
            get { return fallidosVisible; }
            set
            {
                fallidosVisible = value;
                OnPropertyChanged("FallidosVisible");
            }
        }

        private ObservableCollection<ADEstadoGuiaMotivoDC> estadosGuia;

        public ObservableCollection<ADEstadoGuiaMotivoDC> EstadosGuia
        {
            get { return estadosGuia; }
            set
            {
                estadosGuia = value;
                OnPropertyChanged("EstadosGuia");
            }
        }

        private ObservableCollection<Imagen> imagesPruebaEntrega;

        public ObservableCollection<Imagen> ImagesPruebaEntrega
        {
            get { return imagesPruebaEntrega; }
            set
            {
                imagesPruebaEntrega = value;
                OnPropertyChanged("ImagesPruebaEntrega");
            }
        }




        private ObservableCollection<Imagen> images;
        public ObservableCollection<Imagen> ImagesFallidos
        {
            get
            {
                return images;
            }
        }

        private ObservableCollection<Modelos.LIGestionesDC> telemercadeos;

        public ObservableCollection<Modelos.LIGestionesDC> Telemercadeos
        {
            get { return telemercadeos; }
            set
            {
                telemercadeos = value;
                OnPropertyChanged("Telemercadeos");
            }
        }

        private ObservableCollection<ONNovedadesTransporteDC> novedadesTransporte;

        public ObservableCollection<ONNovedadesTransporteDC> NovedadesTransporte
        {
            get { return novedadesTransporte; }
            set
            {
                novedadesTransporte = value;
                OnPropertyChanged("NovedadesTransporte");
            }
        }


        private string estadoGuia;

        public string EstadoGuia
        {
            get { return estadoGuia; }
            set
            {
                estadoGuia = value;
                OnPropertyChanged("EstadoGuia");
            }
        }

        private string fechaEstado;

        public string FechaEstado
        {
            get { return fechaEstado; }
            set
            {
                fechaEstado = value;
                OnPropertyChanged("FechaEstado");
            }
        }
    }
}
