using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_WCF.Modelos
{
    [DataContract(IsReference = true)]
    public class Direccion
    {
        [DataMember]
        public string Calle { get; set; }
        [DataMember]
        public string Numero_exterior { get; set; }
        [DataMember]
        public string Numero_interior { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Municipio { get; set; }
        [DataMember]
        public string Localidad { get; set; }
        [DataMember]
        public string Colonia { get; set; }
        [DataMember]
        public string CP { get; set; }
     
    }
}
