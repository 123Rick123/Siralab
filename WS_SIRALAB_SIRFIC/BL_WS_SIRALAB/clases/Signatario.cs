using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Signatario
    {
        [DataMember]
        public Persona persona { get; set; }
        [DataMember]
        public enumSignatario? TipoSignatario { get; set; }
    }
}
