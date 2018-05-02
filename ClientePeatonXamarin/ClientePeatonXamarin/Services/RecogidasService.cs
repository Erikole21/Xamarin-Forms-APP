using Acr.UserDialogs;
using ClientePeatonXamarin.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientePeatonXamarin.Services
{
    public class RecogidasService
    {
        private static RecogidasService instancia = null;

        public static RecogidasService Instancia
        {
            get
            {
                if (instancia == null)
                    return new RecogidasService();
                else
                    return instancia;
            }
        }

        public RecogidasService()
        {

        }

        public async Task<EnumLoginEstados> IngresarUsuarioExterno(CredencialUsuarioRequest login)
        {
            try
            {

                var client = new RestClient(Configuracion.BaseServicioSeguridadInterUrl);
                var request = new RestRequest(string.Format("Seguridad/ValidarPasswordUsuarioExternoApp"), Method.POST);
                request.AddObject(login);
                var response = await client.ExecutePostTaskAsync<EnumLoginEstados>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return EnumLoginEstados.FalloCreacionSesion;

                    return response.Data;
                }
                else
                    return EnumLoginEstados.FalloCreacionSesion;
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return EnumLoginEstados.FalloCreacionSesion;
            }

        }

        public async Task<UsuarioContactoResponse> ConsultarUsuario(string documento)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioSeguridadInterUrl);
                var request = new RestRequest(string.Format("Administracion/ConsultarUsuario?idDocumento={0}", documento), Method.GET);
                var response = await client.ExecuteGetTaskAsync<UsuarioContactoResponse>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    return response.Data;
                }
                else
                    return null;
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return null;
            }

        }

        public async Task<bool> ValidaExisteDatosContacto(string documento, int tipoEnvio)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioSeguridadInterUrl);
                var request = new RestRequest(string.Format("Administracion/ValidaExisteDatosContacto/{0}/{1}", documento, tipoEnvio), Method.GET);
                var response = await client.ExecuteGetTaskAsync(request);
                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    if (response.StatusDescription.Contains("No tiene relaciono datos contacto."))
                    {
                        UserDialogs.Instance.Toast("Error de Conexión, Intente de nuevo", TimeSpan.FromMilliseconds(Configuracion.TiempoMensaje));
                        return true;
                    }
                }
                else
                    return true;

                return false;

            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return false;
            }


        }

        public async Task<RGRecogidasDC> ConsultarUltimaSolicitud(string numeroDocumento)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Recogidas/ObtenerUltimaSolicitud?numeroDocumento={0}", numeroDocumento), Method.GET);
                var response = await client.ExecuteTaskAsync<RGRecogidasDC>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null;

                    return response.Data;
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return null;
            }
        }

        public async Task<long> GuardarRecogida(RGRecogidasDC recogida, string usuario)
        {
            try
            {

                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Recogidas/InsertarRecogidaEsporadica"), Method.POST);
                request.AddJsonBody(recogida);
                request.AddHeader("usuario", usuario);
                request.AddHeader("IdAplicativoOrigen", "7");
                var response = await client.ExecutePostTaskAsync<long>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return 0;

                    return response.Data;
                }
                else
                {
                    return 0;
                }

            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return 0;
            }
        }

        public async Task<List<PALocalidadDC>> ConsultarCiudadesColombia()
        {

            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Parametros/ObtenerLocalidadesColombia"), Method.GET);
                var response = await client.ExecuteGetTaskAsync<List<PALocalidadDC>>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null;

                    return response.Data;
                }
                return null;
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return null;
            }
        }


        public async Task<bool> RestablecerPasswordPin(Modelos.RestablecerPasswordRequest credencial)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioSeguridadInterUrl);
                var request = new RestRequest(string.Format("Seguridad/RestablecerPasswordPin"), Method.POST);
                request.AddObject(credencial);
                var response = await client.ExecutePostTaskAsync<bool>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return false;

                    return response.Data;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return false;
            }
        }

        public async Task<List<RGRecogidaEsporadicaDC>> ConsultarRecogidasUsuario(string idUsuario)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Recogidas/ObtenerMisRecogidasClientePeaton/{0}", idUsuario), Method.GET);
                var response = await client.ExecuteGetTaskAsync<List<RGRecogidaEsporadicaDC>>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null;

                    return response.Data;
                }
                return null;
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return null;
            }
        }

        public async Task<bool> CancelarServicio(SolicitudConMotivoRequest cancelacion)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Recogidas/CancelarConMotivoSolRecogida"), Method.POST);
                request.AddObject(cancelacion);
                var response = await client.ExecutePostTaskAsync(request);

                return true;
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return false;
            }
        }


    }
}
