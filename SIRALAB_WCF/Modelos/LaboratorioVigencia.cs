using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class LaboratorioVigencia
    {
        [DataMember]
       public int LaboratorioVigenciaID { get; set; }

        [DataMember]
        public int LaboratorioID { get; set; }

        [DataMember]
        public string Numero { get; set; }

        [DataMember]
        public DateTime? FechaInicio { get; set; }

        [DataMember]
        public  DateTime? FechaFin { get; set; }

        [DataMember]
        public bool Activo { get; set; }


    }
}