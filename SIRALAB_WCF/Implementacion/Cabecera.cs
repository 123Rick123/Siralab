using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Net.Security;

namespace SIRALAB_WCF.Implementacion
{
    [MessageContract]
    public class Cabecera
    {
        [MessageHeader]
        public string Token { get; set; }  //<---- Este es la parte que funciona como token
        [MessageHeader]
        public string RFC { get; set; }

    }

    [MessageContract(IsWrapped = true)]
    public class CuerpoMensaje
    {

        public CuerpoMensaje()
        {
        }

        public CuerpoMensaje(string mensaje)
        {
            contruirMensajeOK = mensaje;
        }


        [MessageBodyMember]
        public string contruirMensajeOK { get; set; }

    }


}