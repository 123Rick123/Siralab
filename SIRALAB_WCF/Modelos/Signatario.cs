using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_WCF.Modelos
{
    [DataContract(IsReference = true)]
    public class Signatario
    {
        [DataMember]
        public  string RFCLaboratorio { get; set; }
        [DataMember]
        public string NombreSignatario { get; set; }
    }
}
