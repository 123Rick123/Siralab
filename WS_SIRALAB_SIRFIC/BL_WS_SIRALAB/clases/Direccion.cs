using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Direccion
    {
        [DataMember]
        public string calle { get; set; }
        [DataMember]
        public string numero_exterior { get; set; }
        [DataMember]
        public string numero_interior { get; set; }
        [DataMember]
        public string estado { get; set; }
        [DataMember]
        public string municipio { get; set; }
        [DataMember]
        public string localidad { get; set; }
        [DataMember]
        public string colonia { get; set; }
        [DataMember]
        public string cp { get; set; }
    }
}
