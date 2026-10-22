using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatPermisosRol
    {
        public int PermisosRolID { get; set; }
        public int UsuarioRolID { get; set; }
        public int PaginaID { get; set; }
        public bool Escritura { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual CatPagina Pagina { get; set; }
        public virtual CatUsuarioRol UsuarioRol { get; set; }
    }
}
