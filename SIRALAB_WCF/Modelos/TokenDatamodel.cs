using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{

    [DataContract]
    public class TokenDatamodel
    {
        [DataMember]
        public string Token { get; set; }  //<---- Este es la parte que funciona como token
        //[DataMember]
        //public string RFC { get; set; }
    }
}