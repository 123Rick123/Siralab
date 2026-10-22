using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class ModelNormaConfiguracion
    {
        [DataMember]
        public int NormaID { get; set; }
        [DataMember]
        public int ParametroID { get; set; }
        [DataMember]
        public int CuerpoReceptorID { get; set; }
        [DataMember]
        public int UsoAguaID { get; set; }
        [DataMember]
        public int NivelID { get; set; }


    }
}