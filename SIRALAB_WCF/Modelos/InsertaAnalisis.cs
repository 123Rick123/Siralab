using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class InsertaAnalisis
    {
        public InsertaAnalisis()
        {
            Analisis = new List<Modelos.Analisis>();
        }
        [DataMember]
        public int OrdenTrabajoID { get; set; }
        [DataMember]
        public List<Analisis> Analisis { get; set; }
       
    }
}