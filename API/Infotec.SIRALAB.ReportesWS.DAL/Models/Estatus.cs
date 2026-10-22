using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Estatus
    {
        public Estatus()
        {
            Analisis = new HashSet<Analisi>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
            SolicitudCancelacions = new HashSet<SolicitudCancelacion>();
        }

        public int EstatusID { get; set; }
        public int EstatusTipoID { get; set; }
        public string Nombre { get; set; }

        public virtual EstatusTipo EstatusTipo { get; set; }
        public virtual ICollection<Analisi> Analisis { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
        public virtual ICollection<SolicitudCancelacion> SolicitudCancelacions { get; set; }
    }
}
