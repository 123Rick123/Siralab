using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using SIRALAB_BL.common;

namespace SIRALAB.ClienteAPI
{
    public class ClienteApiReportes
    {
        public byte[] ObtenAcuseFirma(int analisisID)
        {
            try
            {
                var client = new RestClient(ObtenerURL(TipoReporte.AcuseFirma));
                var request = new RestRequest($"{analisisID}", Method.GET);
                var response = client.Execute(request);
                var respuesta = JsonConvert.DeserializeObject<ReporteRespuesta>(response.Content);
                return respuesta.Respuesta;
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public byte[] ObtenInformeLaboratorio(int analisisID)
        {
            try
            {
                var client = new RestClient(ObtenerURL(TipoReporte.InformeLaboratorio));
                var request = new RestRequest($"{analisisID}", Method.GET);
                var response = client.Execute(request);
                var respuesta = JsonConvert.DeserializeObject<ReporteRespuesta>(response.Content);
                return respuesta.Respuesta;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public byte[] ObtenInformeLaboratorioAcuse(int analisisID)
        {
            try
            {
                var client = new RestClient(ObtenerURL(TipoReporte.AcuseFirma));
                var request = new RestRequest($"{analisisID}", Method.GET);
                var response = client.Execute(request);
                var respuesta = JsonConvert.DeserializeObject<ReporteRespuesta>(response.Content);
                return respuesta.Respuesta;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string ObtenerURL(TipoReporte tipoReporte)
        {
            var aplicationSettings = new ApplicationSettings();
            var url = aplicationSettings.UrlApiReportes;

            switch (tipoReporte)
            {
                case TipoReporte.InformeLaboratorio:
                    return url + aplicationSettings.UrlInformeLaboratorio;
                case TipoReporte.AcuseFirma:
                    return url + aplicationSettings.UrlAcuseFirma;
                case TipoReporte.InformeMensual:
                    return url;
                default:
                    return url;
            }
        }

    }
}