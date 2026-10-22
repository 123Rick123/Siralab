using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatUsuarioRol
    {
        public CatUsuarioRol()
        {
            CatPermisosRols = new HashSet<CatPermisosRol>();
            Usuarios = new HashSet<Usuario>();
        }

        public int UsuarioRolID { get; set; }
        public string Nombre { get; set; }
        public bool? Administrable { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<CatPermisosRol> CatPermisosRols { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
