using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratorioContacto
    {
        public int LaboratorioContactoID { get; set; }
        public int LaboratorioID { get; set; }
        public int PersonaID { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Activo { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
