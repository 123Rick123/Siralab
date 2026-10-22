using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;
using SIRALAB_WCF.Modelos;
using SIRALAB_WCF.Implementacion;
using System.Net.Security;
namespace SIRALAB_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOperacionesLaboratorio" in both code and config file together.
    [ServiceContract]
    public interface IOperacionesLaboratorio
    {
        [OperationContract]
        [WebGet(UriTemplate = "/ObtenerOrdenes", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result<OrdenTrabajo> ObtenerOrdenes(TokenDatamodel Requestt, BusquedaOT OT);

        [OperationContract]
        [WebInvoke(UriTemplate = "/InsertaOT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result<OrdenTrabajo> InsertaOT(TokenDatamodel Requestt,  InsercionOT IOT);

        [OperationContract]
        [WebInvoke(UriTemplate = "/InsertaAnalisis", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result<string> InsertaAnalisis(TokenDatamodel Requestt, InsertaAnalisis IA);

        
        [OperationContract]
        [WebInvoke(UriTemplate = "/Autenticacion", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        CuerpoMensaje Autenticacion( Cabecera Request);


       

    }
}
