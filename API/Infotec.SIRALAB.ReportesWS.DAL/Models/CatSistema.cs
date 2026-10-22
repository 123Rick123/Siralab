using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatSistema
    {
        public CatSistema()
        {
            CatPaginas = new HashSet<CatPagina>();
        }

        public int SistemaID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<CatPagina> CatPaginas { get; set; }
    }
}
