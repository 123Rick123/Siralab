using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratorioVigencium
    {
        public LaboratorioVigencium()
        {
            LaboratorioAprobacions = new HashSet<LaboratorioAprobacion>();
        }

        public int LaboratorioVigenciaID { get; set; }
        public int LaboratorioID { get; set; }
        public string Numero { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }
        public virtual ICollection<LaboratorioAprobacion> LaboratorioAprobacions { get; set; }
    }
}
