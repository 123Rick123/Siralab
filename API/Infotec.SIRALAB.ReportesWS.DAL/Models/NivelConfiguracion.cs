using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class NivelConfiguracion
    {
        public NivelConfiguracion()
        {
            Muestras = new HashSet<Muestra>();
            NormaConfiguracions = new HashSet<NormaConfiguracion>();
        }

        public int NivelID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Muestra> Muestras { get; set; }
        public virtual ICollection<NormaConfiguracion> NormaConfiguracions { get; set; }
    }
}
