using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Laboratorio
    {
        [DataMember]
        public int LaboratorioID { get; set; }
        [DataMember]
        public bool Activo { get; set; }

        [DataMember]
        public Direccion direccion { get; set; }
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string apellidoPaterno { get; set; }
        [DataMember]
        public string apellidoMaterno { get; set; }
        [DataMember]
        public string rfc { get; set; }
        [DataMember]
        public string curp { get; set; }
        [DataMember]
        public bool? persona_fisica { get; set; }

        [DataMember]
        public string AcreditacionEMA { get; set; }
        [DataMember]
        public string Aprobacion { get; set; }

    }
}
