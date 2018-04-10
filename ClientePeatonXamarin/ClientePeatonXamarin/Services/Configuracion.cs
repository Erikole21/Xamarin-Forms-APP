using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Services
{
    public static class Configuracion
    {

        public static string BaseServicioInterUrl = "http://172.16.129.28/ServiciosInter.Api/api/";
        public static string BaseServicioSeguridadInterUrl = "http://172.16.129.28/WebApi.Seguridad/api/";
        public static string BaseServicioRapsUrl = "http://172.16.129.28/CO.Servidor.Servicios.RapsWebApi/api/";        

        public static int TiempoMensaje = 4000;

        public static void Notificar(string mensaje)
        {
            UserDialogs.Instance.Toast(mensaje, TimeSpan.FromMilliseconds(Services.Configuracion.TiempoMensaje));
        }

        public static void Mensaje(string mensaje)
        {
            UserDialogs.Instance.Alert(new AlertConfig() { Message = mensaje, Title = "Inter Rapidísimo" });
        }

        public static class ConfigRaps
        {
            private static long idParamVisitaComercial = 596;
            //15924 desarrolo, produccion 36133,  (pre-produccion) 15924; //596 (pruebas); //Clientes corporativos
            public static long IdParamVisitaComercial { get { return idParamVisitaComercial; } }

            private static long idParamEstudioFranquicia = 597;
            //15925 desarrollo, produccion 36131, //15925(pre-produccion); //597(pruebas); //Franquicia
            public static long IdParamEstudioFranquicia { get { return idParamEstudioFranquicia; } }
        }
    }

   
}

