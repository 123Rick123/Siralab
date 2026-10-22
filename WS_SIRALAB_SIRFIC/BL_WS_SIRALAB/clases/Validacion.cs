using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.clases
{
    [DataContract(IsReference = true)]
    public class Validacion
    {
        [DataMember]
        public bool Valido { get; set; }
        [DataMember]
        public bool TodosPDValidos { get; set; }
        [DataMember]
        public enumEvaluacionPromedios ValidacionNumPromedios { get; set; }
        [DataMember]
        public bool Fuente_Abastecimiento { get; set; }
        [DataMember]
        public List<int> Analisis { get; set; }
        [DataMember]
        public bool error { get; set; }
        [DataMember]
        public SiralabFaultException exception { get; set; }
        [DataMember]
        public bool TieneReincidencia { get; set; }
        [DataMember]
        public bool TieneReincidenciaPreventiva { get; set; }
        [DataMember]
        public bool TieneReincidenciaConImpugnacion { get; set; }
        [DataMember]
        public bool TieneApercibimiento { get; set; }
        [DataMember]
        public bool EstaInscritoNom001 { get; set; }
        [DataMember]
        public List<Analisis> AnalisisMensual{ get; set; }
        [DataMember]
        public Norma Norma { get; set; }
        [DataMember]
        public List<Analisis> AnalisisEvaluacionDeclaragua { get; set; }

    }
}
