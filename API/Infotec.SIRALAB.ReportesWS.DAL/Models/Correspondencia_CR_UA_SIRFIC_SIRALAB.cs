using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Correspondencia_CR_UA_SIRFIC_SIRALAB
    {
        public long? ID_TIPO_CR { get; set; }
        public long? ID_USO_CR { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int UsoAguaID { get; set; }
    }
}
