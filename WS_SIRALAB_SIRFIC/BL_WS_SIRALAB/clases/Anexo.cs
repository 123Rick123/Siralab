using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class Anexo
    {
        [DataMember]
        public string anexo { get; set; }
        [DataMember]
        public Direccion direccion { get; set; }
        [DataMember]
        public decimal? latitud { get; set; }
        [DataMember]
        public decimal? longitud { get; set; }
        [DataMember]
        public string datum { get; set; }
        [DataMember]
        public string nombre_cuerpo_receptor { get; set; }
        [DataMember]
        public string fuente_abastecimiento { get; set; }
        [DataMember]
        public string origen_descarga { get; set; }
        [DataMember]
        public string medidor { get; set; }
        [DataMember]
        public string marcaMedidor { get; set; }
        [DataMember]
        public bool? efluente { get; set; }
        [DataMember]
        public Titulo Titulo { get; set; }
    }
}
