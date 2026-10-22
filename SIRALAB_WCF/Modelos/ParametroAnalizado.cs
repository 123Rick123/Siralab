using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_WCF.Modelos
{
    [DataContract(IsReference = true)]
    public class ParametroAnalizado
    {
        [DataMember]
        public Parametro parametro { get; set; }
     
        [DataMember]
        public bool? exenta { get; set; }
        [DataMember]
        public bool? evalua { get; set; }
        [DataMember]
        public bool? exento { get; set; }
        [DataMember]
        public bool? no_analizado { get; set; }
        [DataMember]
        public decimal? valor { get; set; }
        [DataMember]
        public bool? valor_validacion { get; set; }
        [DataMember]
        public bool? acepta_valor { get; set; }
        [DataMember]
        public bool? ley_federal { get; set; }
        [DataMember]
        public decimal? limite_inferior { get; set; }
        [DataMember]
        public decimal? limite_superior { get; set; }
        [DataMember]
        public decimal? limite_inferior_LFD { get; set; }
        [DataMember]
        public decimal? limite_superior_LFD { get; set; }
        [DataMember]
        public decimal? lc_mc { get; set; }
        [DataMember]
        public List<Parametro> grupo { get; set; }
        [DataMember]
        public DateTime FechaConformacion { get; set; }
        [DataMember]
        public decimal? ValorFuente { get; set; }
        [DataMember]
        public decimal? Diferencia { get; set; }
        [DataMember]
        public bool? AplicaFechaConformacion { get; set; }
        [DataMember]
        public decimal? Caudal { get; set; }
    }
}