using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientePeatonXamarin.Modelos;
using RestSharp;

namespace ClientePeatonXamarin.Services
{
    public class CotizarService
    {
        private static CotizarService instancia = null;

        public static CotizarService Instancia
        {
            get
            {
                if (instancia == null) return new CotizarService();
                else return instancia;
            }
        }

        public async Task<List<PALocalidadDC>> ObtenerCiudades()
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
                else
                    return null;
            }
            catch
            {
                //se atrapa la excepcion generalemte por conexion con el servidor para q no totee la APP
                return null;
            }

        }

        public async Task<List<ResponseGenericoAppDC>> ObtenerTipoEntrega()
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Cotizador/ObtenerTipoEntrega"), Method.GET);
                var response = await client.ExecuteGetTaskAsync<List<ResponseGenericoAppDC>>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null;
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

        public async Task<ValorComercialResponseDC> ConsultarValorComercialPeso(int peso)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Cotizador/ConsultarValorComercialPeso?peso={0}", peso), Method.GET);
                var response = await client.ExecuteGetTaskAsync<ValorComercialResponseDC>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null;

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

        public async Task<List<TAPreciosAgrupadosDC>> ObtenerCotizacion(string LocalidadOrigen, string LocalidadDestino, decimal PesoFinal, decimal ValorComercial, string TipoEntrega, string fecha)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Cotizador/ResultadoListaCotizar/{0}/{1}/{2}/{3}/{4}/{5}", LocalidadOrigen, LocalidadDestino, PesoFinal, ValorComercial, TipoEntrega, fecha), Method.GET);
                var response = await client.ExecuteGetTaskAsync<List<TAPreciosAgrupadosDC>>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null;

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

    }
}
