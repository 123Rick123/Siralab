using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class InsercionOT
    {
       
        [DataMember]
        public string RFC { get; set; }
        [DataMember]
        public string RFCLaboratorio { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string Anexo { get; set; }
        [DataMember]
        public string Medidor { get; set; }
        [DataMember]
        public string NombreNorma { get; set; }
        [DataMember]
        public string CuerpoReceptor { get; set; }
        [DataMember]
        public string UsoAgua { get; set; }
        [DataMember]
        public string NumeroInforme { get; set; }
        [DataMember]
        public int HorasProceso { get; set; }
        [DataMember]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        public List<Documento> Evidencias { get; set; }
    }
}