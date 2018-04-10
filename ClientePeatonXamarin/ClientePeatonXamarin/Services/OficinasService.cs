using ClientePeatonXamarin.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientePeatonXamarin.Services
{
    public class OficinasService
    {
        private static OficinasService instancia = null;

        public static OficinasService Instancia
        {
            get
            {
                if (instancia == null)
                    return new OficinasService();
                else return instancia;
            }
        }

        public async Task<List<PUCentroServicioInfoGeneral>> ObtenerPuntosActivos()
        {
            var client = new RestClient(Configuracion.BaseServicioInterUrl);
            var request = new RestRequest(string.Format("Parametros/ObtenerInformacionGeneralCentrosServicioAPP"), Method.GET);
            var response = await client.ExecuteGetTaskAsync<List<PUCentroServicioInfoGeneral>>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    return null;
                return response.Data;
            }
            else
                return null;
        }

        public async Task<List<string>> ObtenerHorarioCentroServicio(long idCentroServicio)
        {
            var client = new RestClient(Configuracion.BaseServicioInterUrl);
            var request = new RestRequest(string.Format("Parametros/ObtenerHorariosCentroServicioAppRecogidas/{0}", idCentroServicio), Method.GET);
            var response = await client.ExecuteGetTaskAsync<List<string>>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    return null;
                return response.Data;
            }
            else
                return null;
        }

    }
}
