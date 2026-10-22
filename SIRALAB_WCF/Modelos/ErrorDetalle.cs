using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class ErrorDetalle
    {
        public ErrorDetalle(string MensageError, string CodigoError)
        {
            Mensage = MensageError;
            Codigo = CodigoError;
        }
            
        [DataMember]
        public string Mensage { get; set; }
        [DataMember]
        public string Codigo { get; set; }
    }
}