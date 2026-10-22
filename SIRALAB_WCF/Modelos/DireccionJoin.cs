using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract(IsReference = true)]
    public class DireccionJoin
    {
        [DataMember]
        public string EstadoID { get; set; }

        [DataMember]
        public string MunicipioID { get; set; }


        [DataMember]
        public string LocalidadID { get; set; }

        [DataMember]
        public string ColoniaID { get; set; }

        
        [DataMember]
        public int cp { get; set; }
    }
}