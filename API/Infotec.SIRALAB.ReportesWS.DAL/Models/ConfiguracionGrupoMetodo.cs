using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class ConfiguracionGrupoMetodo
    {
        public int IdGrupoMetodo { get; set; }
        public int IdMetodo { get; set; }
        public bool Activo { get; set; }

        public virtual CatGrupoMetodo IdGrupoMetodoNavigation { get; set; }
        public virtual Metodo IdMetodoNavigation { get; set; }
    }
}
