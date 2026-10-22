using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class Result<t>
    {
        [DataMember]
        public List<t> ListaResultado { get; set; }
        [DataMember]
        public ErrorDetalle Error { get; set; }
        [DataMember]
        public bool Executenonquery { get; set; }
        [DataMember]
        public List<ErrorDetalle> LstErrores { get; set; }
    }
}