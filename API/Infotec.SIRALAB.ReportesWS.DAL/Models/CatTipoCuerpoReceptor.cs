using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatTipoCuerpoReceptor
    {
        public CatTipoCuerpoReceptor()
        {
            CuerpoReceptors = new HashSet<CuerpoReceptor>();
        }

        public int TipoCuerpoReceptorID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<CuerpoReceptor> CuerpoReceptors { get; set; }
    }
}
