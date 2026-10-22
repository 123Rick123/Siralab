using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class OrdenTrabajo
    {
        [DataMember]
        public DateTime? fecha_emision { get; set; }
        [DataMember]
        public string numero_informe { get; set; }
        [DataMember]
        public string folio { get; set; }
        [DataMember]
        public int? anio { get; set; }
        [DataMember]
        public int? consecutivo { get; set; }
        [DataMember]
        public int? horas_proceso { get; set; }
        [DataMember]
        public Anexo Anexo { get; set; }
        [DataMember]
        public CuerpoReceptor Cuerpo { get; set; }
        [DataMember]
        public UsoAgua Uso { get; set; }
        [DataMember]
        public Norma Norma { get; set; }
    }
}
