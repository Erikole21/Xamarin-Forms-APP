using ClientePeatonXamarin.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using RestSharp;

namespace ClientePeatonXamarin.Services
{
    public class SigueEnvioService
    {
        private static SigueEnvioService instancia = null;

        public static SigueEnvioService Instancia
        {
            get
            {
                if (instancia == null)
                    return new SigueEnvioService();
                else
                    return instancia;
            }
        }

        public SigueEnvioService()
        {

        }

        public async Task<ADRastreoGuiaDC> ObtenerRastreoGuia(string numeroGuia)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioInterUrl);
                var request = new RestRequest(string.Format("Mensajeria/ObtenerRastreoGuias?guias={0}", numeroGuia), Method.GET);
                var response = await client.ExecuteGetTaskAsync<List<ADRastreoGuiaDC>>(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return null; //Guia no Existe. o no se puede conectar

                    return response.Data.FirstOrDefault();
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
