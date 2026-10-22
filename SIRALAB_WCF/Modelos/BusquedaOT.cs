using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class BusquedaOT
    {
        [DataMember]
        public DateTime? EmisionFechaInicio { get; set; }
        [DataMember]
        public DateTime? EmisionFechaFin { get; set; }
        [DataMember]
        public DateTime? RegistroFechaInicio { get; set; }
        [DataMember]
        public DateTime? RegistroFechaFin { get; set; }
        [DataMember]
        public DateTime? ConformacionFechaInicio { get; set; }
        [DataMember]
        public DateTime? ConformacionFechaFin { get; set; }
        [DataMember]
        public int EstatusCancelacion { get; set; }
        [DataMember]
        public string NumeroInforme { get; set; }
       
        [DataMember]
        public string NombreNorma { get; set; }
        [DataMember]
        public string RFCCliente { get; set; }
        [DataMember(IsRequired =true)]       
        public string RFCLaboratorio { get; set; }
        [DataMember]
        public bool Firmados { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string Anexo { get; set; }
       
    }
}
