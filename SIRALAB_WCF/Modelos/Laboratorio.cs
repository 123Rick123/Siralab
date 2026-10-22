using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{

    [DataContract]
    public class Laboratorio
    {
        [DataMember]
        public int LaboratorioID { get; set; }
        [DataMember]
        public int DireccionID { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string RFC { get; set; }
        [DataMember]
        public bool Activo { get; set; }
       
    }
}