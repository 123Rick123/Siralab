using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class ParametroMetodo
    {
        public ParametroMetodo()
        {
            LaboratorioAprobacions = new HashSet<LaboratorioAprobacion>();
            MuestraValors = new HashSet<MuestraValor>();
        }

        public int ParametroMetodoID { get; set; }
        public int ParametroID { get; set; }
        public int MetodoID { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public virtual Metodo Metodo { get; set; }
        public virtual Parametro Parametro { get; set; }
        public virtual ICollection<LaboratorioAprobacion> LaboratorioAprobacions { get; set; }
        public virtual ICollection<MuestraValor> MuestraValors { get; set; }
    }
}
