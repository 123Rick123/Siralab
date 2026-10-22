using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratorioSignatario
    {
        public LaboratorioSignatario()
        {
            Muestreadors = new HashSet<Muestreador>();
        }

        public int LaboratorioSignatarioID { get; set; }
        public int LaboratorioID { get; set; }
        public int PersonaID { get; set; }
        public int TipoSignatarioID { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool EsInterno { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual CatTipoSignatario TipoSignatario { get; set; }
        public virtual ICollection<Muestreador> Muestreadors { get; set; }
    }
}
