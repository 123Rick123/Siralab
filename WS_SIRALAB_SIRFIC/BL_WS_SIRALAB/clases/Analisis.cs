using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Analisis
    {
        [DataMember]
        public int? AnalisisID { get; set; }
        [DataMember]
        public List<Muestra> muestras { get; set; }
        [DataMember]
        public Laboratorio laboratorio { get; set; }
        [DataMember]
        public OrdenTrabajo orden_trabajo { get; set; }
        [DataMember]
        public string observaciones { get; set; }
        [DataMember]
        public bool falta_pago { get; set; }
        [DataMember]
        public string NormaDQOCOT { get; set; }
        [DataMember]
        public string Conductividad { get; set; }
    }
}
