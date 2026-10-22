using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class EstatusTipo
    {
        public EstatusTipo()
        {
            Estatuses = new HashSet<Estatus>();
        }

        public int EstatusTipoID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Estatus> Estatuses { get; set; }
    }
}
