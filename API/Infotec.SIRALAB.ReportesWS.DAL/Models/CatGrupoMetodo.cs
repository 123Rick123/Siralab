using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatGrupoMetodo
    {
        public CatGrupoMetodo()
        {
            ConfiguracionGrupoMetodos = new HashSet<ConfiguracionGrupoMetodo>();
        }

        public int IdGrupoMetodo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<ConfiguracionGrupoMetodo> ConfiguracionGrupoMetodos { get; set; }
    }
}
