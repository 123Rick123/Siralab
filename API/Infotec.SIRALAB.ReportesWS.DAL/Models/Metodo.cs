using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Metodo
    {
        public Metodo()
        {
            ConfiguracionGrupoMetodos = new HashSet<ConfiguracionGrupoMetodo>();
            ParametroMetodos = new HashSet<ParametroMetodo>();
        }

        public int MetodoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<ConfiguracionGrupoMetodo> ConfiguracionGrupoMetodos { get; set; }
        public virtual ICollection<ParametroMetodo> ParametroMetodos { get; set; }
    }
}
