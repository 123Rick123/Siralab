using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class OperacionConsolidado
    {
        public OperacionConsolidado()
        {
            MuestraValors = new HashSet<MuestraValor>();
            NormaConfiguracions = new HashSet<NormaConfiguracion>();
        }

        public int OperacionID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<MuestraValor> MuestraValors { get; set; }
        public virtual ICollection<NormaConfiguracion> NormaConfiguracions { get; set; }
    }
}
