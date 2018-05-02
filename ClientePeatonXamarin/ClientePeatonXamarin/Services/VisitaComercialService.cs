using ClientePeatonXamarin.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientePeatonXamarin.Services
{
    public class VisitaComercialService
    {
        private static VisitaComercialService instancia = null;

        public static VisitaComercialService Instancia
        {
            get
            {
                if (instancia == null)
                    return new VisitaComercialService();
                else return instancia;
            }
        }

        public async Task<long> GuardarRecogida(RegistroSolicitud registroSolicitud)
        {
            try
            {
                var client = new RestClient(Configuracion.BaseServicioRapsUrl);
                var request = new RestRequest(string.Format("SolicitudesRaps/CrearSolicitud"), Method.POST);
                request.AddJsonBody(registroSolicitud);
                request.AddHeader("usuario", "Admin");
                request.AddHeader("IdAplicativoOrigen", "7");
                request.AddHeader("IdCentroServicio", "1");
                request.AddHeader("IdUsuario", "1");
                request.AddHeader("NombreCentroServicio", "1");

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

    }
}
