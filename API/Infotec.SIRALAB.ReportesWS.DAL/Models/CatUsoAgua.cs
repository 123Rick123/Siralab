using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatUsoAgua
    {
        public CatUsoAgua()
        {
            Anexos = new HashSet<Anexo>();
            NormaConfiguracions = new HashSet<NormaConfiguracion>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int UsoAguaID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Anexo> Anexos { get; set; }
        public virtual ICollection<NormaConfiguracion> NormaConfiguracions { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
