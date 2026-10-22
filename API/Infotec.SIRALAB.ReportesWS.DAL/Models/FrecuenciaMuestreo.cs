using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class FrecuenciaMuestreo
    {
        public FrecuenciaMuestreo()
        {
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int FrecuenciaMuestreoID { get; set; }
        public string Nombre { get; set; }
        public int OperadorIDMinimo { get; set; }
        public int HorasMinimo { get; set; }
        public int OperadorIDMaximo { get; set; }
        public int HorasMaximo { get; set; }
        public int MuestrasSimples { get; set; }

        public virtual Operador OperadorIDMaximoNavigation { get; set; }
        public virtual Operador OperadorIDMinimoNavigation { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
