using System;
using System.Net;
using System.Resources;
using Newtonsoft.Json;
using RestSharp;

namespace BL_WS_SIRALAB.ClienteAPI
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


        public byte[] ObtenInformeMensual(int clienteId, DateTime fechaInicio, DateTime fechaFin, string titulo,
            string anexo, int normaId, enumTipoEvaluacion evaluacion)
        {
            try
            {
                var client = new RestClient(ObtenerURL(TipoReporte.InformeMensual) + $"?clienteId={clienteId}&fechaInicio={fechaInicio:yyyy/MM/dd}&fechaFin={fechaFin:yyyy/MM/dd}&titulo={titulo}&anexo={anexo}&normaId={normaId}&evaluacion={evaluacion}");
                var request = new RestRequest(Method.GET);
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
           var url = ConfigApiReportes.CadenaConexionEndPoint;

            switch (tipoReporte)
            {
                case TipoReporte.InformeLaboratorio:
                    return url + ConfigApiReportes.CadenaInformeLaboratorio; 
                case TipoReporte.AcuseFirma:
                    return url + ConfigApiReportes.CadenaAcuseFirma;
                case TipoReporte.InformeMensual:
                    return url + ConfigApiReportes.CadenaInformeMensual;
                default:
                    return url;
            }
        }
    }
}