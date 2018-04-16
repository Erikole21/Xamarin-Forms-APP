using Acr.UserDialogs;
using ClientePeatonXamarin.Services;
using ClientePeatonXamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ClientePeatonXamarin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {

        }

        public LoginViewModel(INavigation _navegacion, bool misRecogidas)
        {
            this.MisRecogidas = misRecogidas;
            this.Navegacion = _navegacion;
            IngesarComand = new Command(Ingresar);
            ValidarComand = new Command(ValidarUsuario);
            RecordarClaveComand = new Command(RecordarClave);
            EnviarComand = new Command(EnviarRecuperacion);
            CambiarClaveComand = new Command(CambiarClave);

        }

        public bool MisRecogidas { get; set; }

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

        private string clave;

        public string Clave
        {
            get { return clave; }
            set
            {
                clave = value;
                OnPropertyChanged("Clave");
            }
        }

        public ICommand IngesarComand { set; get; }

        public ICommand ValidarComand { get; set; }

        public ICommand RecordarClaveComand { get; set; }

        public ICommand EnviarComand { get; set; }

        public ICommand CambiarClaveComand { get; set; }


        private bool opcionesRecuperaVisible;

        public bool OpcionesRecuperaVisible
        {
            get { return opcionesRecuperaVisible; }
            set
            {
                opcionesRecuperaVisible = value;
                OnPropertyChanged("OpcionesRecuperaVisible");
            }
        }

        private bool opcionesRestablecerVisible;

        public bool OpcionesRestablecerVisible
        {
            get { return opcionesRestablecerVisible; }
            set
            {
                opcionesRestablecerVisible = value;
                OnPropertyChanged("OpcionesRestablecerVisible");
            }
        }


        private bool correo;

        public bool Correo
        {
            get { return correo; }
            set
            {
                correo = value;
                mensajeTexto = !correo;
                OnPropertyChanged("MensajeTexto");
                OnPropertyChanged("Correo");
            }
        }

        private bool mensajeTexto;

        public bool MensajeTexto
        {
            get { return mensajeTexto; }
            set
            {
                mensajeTexto = value;
                correo = !mensajeTexto;
                OnPropertyChanged("Correo");
                OnPropertyChanged("MensajeTexto");
            }
        }

        private string textoMensaje;

        public string TextoMensaje
        {
            get { return textoMensaje; }
            set
            {
                textoMensaje = value;
                OnPropertyChanged("TextoMensaje");
            }
        }

        private string textoEmail;

        public string TextoEmail
        {
            get { return textoEmail; }
            set
            {
                textoEmail = value;
                OnPropertyChanged("TextoEmail");
            }
        }

        private string nuevaClave;

        public string NuevaClave
        {
            get { return nuevaClave; }
            set
            {
                nuevaClave = value;
                OnPropertyChanged("NuevaClave");
            }
        }

        private string confirmarClave;

        public string ConfirmarClave
        {
            get { return confirmarClave; }
            set
            {
                confirmarClave = value;
                OnPropertyChanged("ConfirmarClave");
            }
        }

        private string pingConfirmacion;

        public string PingConfirmacion
        {
            get { return pingConfirmacion; }
            set
            {
                pingConfirmacion = value;
                OnPropertyChanged("PingConfirmacion");
            }
        }


        private async void CambiarClave()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(nuevaClave) && !string.IsNullOrEmpty(confirmarClave) && !string.IsNullOrEmpty(pingConfirmacion))
            {
                if (nuevaClave.Trim() == confirmarClave.Trim())
                {
                    if (nuevaClave.Trim().Length > 7)
                    {
                        bool completo = await RecogidasService.Instancia.RestablecerPasswordPin
                        (new Modelos.RestablecerPasswordRequest() { IdUsuario = usuario, Password = nuevaClave, Pin = Convert.ToInt32(pingConfirmacion) });

                        if (completo)
                        {
                            Notificar("Contraseña reestablecida correctamente!");
                            Clave = string.Empty;
                            await Navegacion.PopAsync();
                        }
                        else
                            Mensaje("Pin Incorrecto o Contraseña Insegura, Valide la Información");
                    }
                    else
                        Mensaje("La nueva clave debe ser de mínimo 8 caracteres.");
                }
                else
                    Notificar("Los campos de nueva clave y confirmación no concuerdan..");
            }
            else
                Notificar("Datos Incompletos..");
        }

        private async void RecordarClave()
        {
            await Navegacion.PushAsync(new RecuperarClavePage(this));
        }

        private async void EnviarRecuperacion()
        {
            bool valido = await RecogidasService.Instancia.ValidaExisteDatosContacto(usuario, correo ? 2 : 1);
            if (valido)
            {
                OpcionesRestablecerVisible = true;
            }
        }

        private async void ValidarUsuario()
        {
            if (!string.IsNullOrEmpty(usuario))
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Validando..", null, null, true, MaskType.Black))
                {
                    Modelos.UsuarioContactoResponse usuarioExiste = await RecogidasService.Instancia.ConsultarUsuario(usuario);
                    //No tiene relaciono datos contacto pero existe en el sistema.
                    if (usuarioExiste?.MensajeResultado == Modelos.EnumLoginEstados.UsuarioValido
                        && string.IsNullOrEmpty(usuarioExiste.Correo) && string.IsNullOrEmpty(usuarioExiste.Celular))
                    {
                        Notificar("Sin datos de contato, Guarda una recogida para almacenar datos de contaco y recuperar tu clave");
                        GuardarUsuarioValidado(); // para q ingrese informacion de recogida y datos para proxima ingreso
                    }
                    else
                    {
                        if (usuarioExiste?.MensajeResultado == Modelos.EnumLoginEstados.UsuarioValido)
                        {
                            OpcionesRecuperaVisible = true;
                            if (!string.IsNullOrEmpty(usuarioExiste.Correo) || !string.IsNullOrEmpty(usuarioExiste.Celular))
                            {
                                if (!string.IsNullOrEmpty(usuarioExiste.Correo))
                                {
                                    Correo = true;
                                    TextoEmail = string.Format("Correo Electronico : {0}", usuarioExiste.Correo);
                                }

                                if (!string.IsNullOrEmpty(usuarioExiste.Celular))
                                {
                                    MensajeTexto = true;
                                    TextoMensaje = string.Format("Mensaje de Texto : ******{0}", usuarioExiste.Celular?.Length >= 5 ? usuarioExiste.Celular.Substring(usuarioExiste.Celular.Length - 4, 4) : usuarioExiste.Celular);
                                }
                            }
                        }
                        else
                        {
                            // para q ingrese informacion de recogida y datos para proxima ingreso
                            Notificar("Usuario no existe, Ingresa una clave inicial de ingreso para realizar una recogida");
                            Clave = string.Empty;
                            await Navegacion.PopAsync();
                        }
                    }
                }
            }
            else
                Notificar("Complete los datos!");
        }

        private static void Notificar(string mensaje)
        {
            UserDialogs.Instance.Toast(mensaje, TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
        }

        private async void Ingresar()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(clave))
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Ingresando..", null, null, true, MaskType.Black))
                {
                    Modelos.EnumLoginEstados resultado = await RecogidasService.Instancia.IngresarUsuarioExterno(new Modelos.CredencialUsuarioRequest() { Password = clave, Usuario = usuario });
                    switch (resultado)
                    {
                        case Modelos.EnumLoginEstados.UsuarioOPasswordInvalido:
                            Mensaje("Usuario o contraseña incorrecta.");
                            break;
                        case Modelos.EnumLoginEstados.UsuarioBloqueado:
                            Mensaje("Usuario bloqueado.");
                            break;
                        case Modelos.EnumLoginEstados.UsuarioPasswordValidos:
                            GuardarUsuarioValidado();
                            break;
                        case Modelos.EnumLoginEstados.UsuarioNoExiste:
                            Mensaje("Usuario no existe.");
                            break;
                        case Modelos.EnumLoginEstados.PasswordVencida:
                            Mensaje("Contraseña vencida.");
                            break;
                        case Modelos.EnumLoginEstados.EmpleadoNoExisteOEmpleadoInactivo:
                            Mensaje("Usuario inactivo.");
                            break;
                        case Modelos.EnumLoginEstados.UsuarioSinPermisos:
                            Mensaje("Usuario sin permisos.");
                            break;
                        case Modelos.EnumLoginEstados.UsuarioSinUbicacion:
                            Mensaje("Usuario sin ubicación.");
                            break;
                        case Modelos.EnumLoginEstados.FalloCreacionSesion:
                            Mensaje("Error en inicio de sesión.");
                            break;
                        case Modelos.EnumLoginEstados.UsuarioNuevo:
                            GuardarUsuarioValidado();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
                UserDialogs.Instance.Toast("Complete los datos!.", TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
        }

        private static void Mensaje(string mensaje)
        {
            UserDialogs.Instance.Alert(new AlertConfig() { Message = mensaje, Title = "Inter Rapidísimo" });
        }

        private async void GuardarUsuarioValidado()
        {
            Application.Current.Properties["UsuarioActivo"] = usuario;
            await Navegacion.PopAsync();
            if (MisRecogidas)
                await Navegacion.PushAsync(new MisRecogidas(new ViewModels.MisRecogidasViewModel(Navegacion)));
            else
                await Navegacion.PushAsync(new RecogidasPage(new ViewModels.RecogidasViewModel(Navegacion)));
        }
    }
}
