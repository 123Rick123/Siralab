using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Muestra
    {
        [DataMember]
        public Signatario Muestreador { get; set; }
        [DataMember]
        public List<ParametroAnalizado> Resultados { get; set; }
        [DataMember]
        public Direccion direccion { get; set; }
        [DataMember]
        public enumNivel? nivel { get; set; }
        [DataMember]
        public string numero { get; set; }
        [DataMember]
        public string descripcion { get; set; }
        [DataMember]
        public string identificacion { get; set; }
        [DataMember]
        public bool? requiere_caudal { get; set; }
        [DataMember]
        public decimal? caudal { get; set; }
        [DataMember]
        public decimal? latitud { get; set; }
        [DataMember]
        public decimal? longitud { get; set; }
        [DataMember]
        public string datum { get; set; }
        [DataMember]
        public DateTime? fecha { get; set; }
        [DataMember]
        public TimeSpan? hora { get; set; }
        [DataMember]
        public DateTime? fecha_recepcion { get; set; }
        [DataMember]
        public TimeSpan? hora_recepcion { get; set; }
        [DataMember]
        public bool? cancelado { get; set; }
        [DataMember]
        public string observaciones { get; set; }
    }
}
