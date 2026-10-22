using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatPagina
    {
        public CatPagina()
        {
            CatPermisosRols = new HashSet<CatPermisosRol>();
        }

        public int PaginaID { get; set; }
        public int ModuloID { get; set; }
        public int SistemaID { get; set; }
        public int? Orden { get; set; }
        public string CssClass { get; set; }
        public string URL { get; set; }
        public string Descripcion { get; set; }
        public bool? VisibleMenu { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual CatModulo Modulo { get; set; }
        public virtual CatSistema Sistema { get; set; }
        public virtual ICollection<CatPermisosRol> CatPermisosRols { get; set; }
    }
}
