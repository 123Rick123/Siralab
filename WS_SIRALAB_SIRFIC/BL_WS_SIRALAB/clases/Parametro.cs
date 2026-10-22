using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Parametro
    {
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string unidad { get; set; }
        [DataMember]
        public string descripcion { get; set; }
        [DataMember]
        public int ParametroID { get; set; }
    }
}
