using Acr.UserDialogs;
using ClientePeatonXamarin.Modelos;
using ClientePeatonXamarin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;

namespace ClientePeatonXamarin.ViewModels
{

    public class CotizadorViewModel : MainViewModel
    {

        #region variables

        public string resultado;
        public string Resultado { get { return resultado; } set { resultado = value; OnPropertyChanged("Resultado"); } }

        private ObservableCollection<Modelos.TAPreciosAgrupadosDC> cotizaciones;

        public ObservableCollection<Modelos.TAPreciosAgrupadosDC> Cotizaciones
        {
            get { return cotizaciones; }
            set
            {
                cotizaciones = value;
                OnPropertyChanged("Cotizaciones");
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


        private Modelos.PALocalidadDC selectCiudadesOrigen;

        public Modelos.PALocalidadDC SelectCiudadesOrigen
        {
            get { return selectCiudadesOrigen; }
            set
            {
                selectCiudadesOrigen = value;
                OnPropertyChanged("SelectCiudadesOrigen");
            }
        }

        private Modelos.PALocalidadDC selectCiudadesDestino;

        public Modelos.PALocalidadDC SelectCiudadesDestino
        {
            get { return selectCiudadesDestino; }
            set
            {
                selectCiudadesDestino = value;
                OnPropertyChanged("SelectCiudadesDestino");
            }
        }


        private ObservableCollection<Modelos.ResponseGenericoAppDC> tiposEntrega;

        public ObservableCollection<Modelos.ResponseGenericoAppDC> TiposEntrega
        {
            get { return tiposEntrega; }
            set
            {
                tiposEntrega = value;
                OnPropertyChanged("TiposEntrega");
            }
        }

        private Modelos.ResponseGenericoAppDC selectTiposEntrega;

        public Modelos.ResponseGenericoAppDC SelectTiposEntrega
        {
            get { return selectTiposEntrega; }
            set
            {
                selectTiposEntrega = value;
                OnPropertyChanged("SelectTiposEntrega");
            }
        }


        private string numeroPiezas;
        public string NumeroPiezas
        {
            get { return numeroPiezas; }
            set
            {
                numeroPiezas = value;
                OnPropertyChanged("NumeroPiezas");
            }
        }

        private string pesoFinal;
        public string PesoFinal
        {
            get { return pesoFinal; }
            set
            {
                pesoFinal = value;
                OnPropertyChanged("PesoFinal");
            }
        }

        private string pesoFisico;
        public string PesoFisico
        {
            get { return pesoFisico; }
            set
            {
                pesoFisico = value;
                OnPropertyChanged("PesoFisico");
            }
        }

        private string pesoVolumetrico;
        public string PesoVolumetrico
        {
            get { return pesoVolumetrico; }
            set
            {
                pesoVolumetrico = value;
                OnPropertyChanged("PesoVolumetrico");
            }
        }

        private decimal? valorComercial;
        public decimal? ValorComercial
        {
            get { return valorComercial; }
            set
            {
                valorComercial = value;
                OnPropertyChanged("ValorComercial");
            }
        }


        private string fechaEntrega;
        public string FechaEntrega
        {
            get { return fechaEntrega; }
            set
            {
                fechaEntrega = value;
                OnPropertyChanged("FechaEntrega");
            }
        }

        private string localidadOrigen;
        public string LocalidadOrigen
        {
            get { return localidadOrigen; }
            set
            {
                localidadOrigen = value;
                OnPropertyChanged("LocalidadOrigen");
            }
        }

        private string localidadDestino;
        public string LocalidadDestino
        {
            get { return localidadDestino; }
            set
            {
                localidadDestino = value;
                OnPropertyChanged("LocalidadDestino");
            }
        }

        private string tipoEntrega;
        public string TipoEntrega
        {
            get { return tipoEntrega; }
            set
            {
                tipoEntrega = value;
                OnPropertyChanged("TipoEntrega");
            }
        }

        private bool muestraCotizacion;
        public bool MuestraCotizacion
        {
            get { return muestraCotizacion; }
            set
            {
                muestraCotizacion = value;
                OnPropertyChanged("MuestraCotizacion");
            }
        }
        private string alto;
        public string Alto
        {
            get { return alto; }
            set
            {
                alto = value;
                OnPropertyChanged("Alto");
            }
        }

        private string ancho;
        public string Ancho
        {
            get { return ancho; }
            set
            {
                ancho = value;
                OnPropertyChanged("Ancho");
            }
        }

        private string largo;
        public string Largo
        {
            get { return largo; }
            set
            {
                largo = value;
                OnPropertyChanged("Largo");
            }
        }
        #endregion variables

        public CotizadorViewModel(INavigation navegacion)
        {
            NumeroPiezas = "Número Piezas: 1";
            CotizarServicioCommand = new Command(CotizarServicio);
            CalcularPesoVolumetricoCommand = new Command(CalcularPesoVolumetrico);
            MuestraCotizacion = false;
            FechaEntrega = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            this.Navegacion = navegacion;
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            using (IProgressDialog progress = UserDialogs.Instance.Loading("consultando..", null, null, true, MaskType.Black))
            {
                ConsultarCiudades();
                ConsultarTipoEntrega();
            }
        }


        private async void ConsultarCiudades()
        {
            List<Modelos.PALocalidadDC> ciudades = await Services.RecogidasService.Instancia.ConsultarCiudadesColombia();
            Ciudades = new ObservableCollection<Modelos.PALocalidadDC>(ciudades);
            if (Ciudades == null)
            {
                await Navegacion.PushAsync(new MainPage());
                UserDialogs.Instance.Toast("No se cargaron las ciudades", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
            }

        }

        public ICommand CalcularPesoVolumetricoCommand { get; set; }

        public async void CalcularPesoVolumetrico()
        {
            decimal valorLargo = 0;
            decimal valorAncho = 0;
            decimal valorAlto = 0;

            decimal.TryParse(Largo, out valorLargo);
            decimal.TryParse(Ancho, out valorAncho);
            decimal.TryParse(Alto, out valorAlto);


            if (string.IsNullOrEmpty(Largo) || string.IsNullOrEmpty(Ancho) || string.IsNullOrEmpty(Alto))
            {
                UserDialogs.Instance.Toast("Datos incompletos", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                return;
            }

            if (valorLargo == 0 || valorAncho == 0 || valorAlto == 0)
            {
                UserDialogs.Instance.Toast("Los valores deben ser mayores a '0'", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
            }
            else
            {
                var pesoVolumetrico = Math.Ceiling((valorLargo * valorAncho * valorAlto) / 6000);
                PesoVolumetrico = pesoVolumetrico.ToString();

                using (IProgressDialog progress = UserDialogs.Instance.Loading("Calculando Valor..", null, null, true, MaskType.Black))
                {
                    if (string.IsNullOrEmpty(PesoFisico))
                    {
                        var valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoVolumetrico));
                        AsignarValorComercial(valor);
                    }
                    else
                    {
                        if (Convert.ToInt32(PesoFisico) < Convert.ToInt32(PesoVolumetrico))
                        {
                            var valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoVolumetrico));
                            AsignarValorComercial(valor);
                        }
                        else
                        {
                            var valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoFinal));
                            AsignarValorComercial(valor);
                        }
                    }
                }

                await Navegacion.PopAsync();
            }
        }

        private void AsignarValorComercial(ValorComercialResponseDC valor)
        {
            if (valor != null)
                ValorComercial = valor.valorComercial;
            else
                ValorComercial = 0;
        }

        private async void ConsultarTipoEntrega()
        {
            var tipoEntrega = await Services.CotizarService.Instancia.ObtenerTipoEntrega();
            TiposEntrega = new ObservableCollection<Modelos.ResponseGenericoAppDC>(tipoEntrega);
            if (TiposEntrega == null)
            {                
                await Navegacion.PushAsync(new MainPage());
                UserDialogs.Instance.Toast("No se cargaron los tipos de entrega ", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
            }
        }

        public ICommand CotizarServicioCommand { get; set; }

        public async void CotizarServicio()
        {
            using (IProgressDialog progress = UserDialogs.Instance.Loading("Consultando..", null, null, true, MaskType.Black))
            {
                if (ValidaCampos())
                {
                    var cotizacion = await Services.CotizarService.Instancia.ObtenerCotizacion(SelectCiudadesOrigen.IdLocalidad, SelectCiudadesDestino.IdLocalidad, Convert.ToDecimal(PesoFinal), Convert.ToDecimal(ValorComercial), SelectTiposEntrega.value, DateTime.Now.ToString("yyyy-MM-dd"));

                    if (cotizacion?.Count > 0)
                    {
                        foreach (var i in cotizacion)
                        {
                            i.Valortotal = i.Precio.Valor + i.Precio.ValorPrimaSeguro;

                        }
                        Cotizaciones = new ObservableCollection<Modelos.TAPreciosAgrupadosDC>(cotizacion);
                        MuestraCotizacion = true;
                    }
                    else
                        MuestraCotizacion = false;

                    if (cotizacion?.Count == 0)
                    {
                        UserDialogs.Instance.Toast("No se pudo generar cotización, No hay servicios disponibles para su envio", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                    }

                }

            }
        }

        public bool ValidaCampos()
        {
            /***************************************/
            if (SelectTiposEntrega == null)
            {
                UserDialogs.Instance.Toast("Debe seleccionar un tipo de entrega.", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                return false;
            }

            /***** Validacion numero de piezas, peso fisico y valor comercial *******/
            if (string.IsNullOrEmpty(NumeroPiezas) || string.IsNullOrEmpty(PesoFisico))
            {
                UserDialogs.Instance.Toast("El número de piezas, peso físico y valor comercial deben ser númericos.", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                return false;
            }

            /*************************** Ciudad origen y destino *********************************************/
            if (SelectCiudadesOrigen == null || SelectCiudadesOrigen == null)
            {
                UserDialogs.Instance.Toast("Utilice el autocompletar y seleccione la ciudad de origen y destino.", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                return false;
            }

            if (valorComercial < 5000)
            {
                valorComercial = 5000;
                return true;
            }


            if (PesoVolumetrico != null)
            {
                if (PesoFisico != null)
                {
                    if (Convert.ToInt32(PesoVolumetrico) < Convert.ToInt32(PesoFisico))
                    {
                        PesoFinal = pesoFisico;
                    }
                    else
                    {
                        pesoFinal = pesoVolumetrico;
                    }

                }
                else
                {
                    PesoFinal = pesoVolumetrico;
                }
                return true;
            }
            else
            {
                if (PesoFisico != null)
                {
                    PesoFinal = PesoFisico;
                    return true;
                }
                else
                {
                    UserDialogs.Instance.Toast("Debe diligenciar el peso fisico o volumétrico.", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
                    return false;
                }
            }

        }
        public async void CargarValorComercialPorPeso()
        {
            if (ValorComercial == 0 || valorComercial == null)
            {

                if (!string.IsNullOrEmpty(PesoFinal) || Convert.ToInt32(PesoFisico) != 0)
                {
                    using (IProgressDialog progress = UserDialogs.Instance.Loading("Consultando..", null, null, true, MaskType.Clear))
                    {
                        ValorComercialResponseDC valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoFisico));
                        AsignarValorComercial(valor);
                    }
                }
            }
        }

        public async void CalcularValorComercialPorPeso()
        {
            ValorComercialResponseDC valor = null;
            using (IProgressDialog progress = UserDialogs.Instance.Loading("Calculando..", null, null, true, MaskType.Clear))
            {

                if (!string.IsNullOrEmpty(PesoFisico))
                {
                    if (!string.IsNullOrEmpty(PesoVolumetrico))
                    {
                        if (Convert.ToInt32(PesoFisico) >= Convert.ToInt32(PesoVolumetrico))
                        {
                            valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoFisico));
                            AsignarValorComercial(valor);
                        }
                        else
                        {
                            valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoVolumetrico));
                            AsignarValorComercial(valor);
                        }
                    }
                    else
                    {
                        valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoFisico));
                        AsignarValorComercial(valor);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(PesoVolumetrico))
                    {
                        valor = await Services.CotizarService.Instancia.ConsultarValorComercialPeso(Convert.ToInt32(PesoVolumetrico));
                        AsignarValorComercial(valor);
                    }
                }
            }
        }




    }
}
