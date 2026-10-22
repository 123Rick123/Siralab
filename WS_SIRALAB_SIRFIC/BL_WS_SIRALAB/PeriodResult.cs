using BL_WS_SIRALAB.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class PeriodResult
    {
        [DataMember]
        public List<Analisis> analisis { get; set; }
        [DataMember]
        public Anexo anexo { get; set; }
        [DataMember]
        public Titulo consecion { get; set; }
        [DataMember]
        public Cliente cliente { get; set; }
        [DataMember]
        public Norma norma { get; set; }
        [DataMember]
        public bool? valida { get; set; }
        [DataMember]
        public bool TodosPDValidos { get; set; }
        [DataMember]
        public bool Fuente_Abastecimiento { get; set; }
        [DataMember]
        public enumEvaluacionPromedios ValidacionNumPromedios { get; set; }
        [DataMember]
        public bool error { get; set; }
        [DataMember]
        public SiralabFaultException exception { get; set; }
        [DataMember]
        public bool TieneReincidencia { get; set; }

        [DataMember]
        public bool TieneApercibimiento { get; set; }

        [DataMember]
        public bool TieneReincidenciaPreventiva { get; set; }

        [DataMember]
        public bool TieneReincidenciaConImpugnacion { get; set; }

        [DataMember]
        public bool EstaInscritoNom001 { get; set; }
    }
}
