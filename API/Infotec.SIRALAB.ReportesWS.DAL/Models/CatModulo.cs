using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatModulo
    {
        public CatModulo()
        {
            CatPaginas = new HashSet<CatPagina>();
            InverseModuloPadre = new HashSet<CatModulo>();
        }

        public int ModuloID { get; set; }
        public int? ModuloPadreID { get; set; }
        public int? Orden { get; set; }
        public string CssClass { get; set; }
        public string URL { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual CatModulo ModuloPadre { get; set; }
        public virtual ICollection<CatPagina> CatPaginas { get; set; }
        public virtual ICollection<CatModulo> InverseModuloPadre { get; set; }
    }
}
