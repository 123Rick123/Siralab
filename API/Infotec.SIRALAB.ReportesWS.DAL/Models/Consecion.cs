using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Consecion
    {
        public Consecion()
        {
            Analisis = new HashSet<Analisi>();
            Anexos = new HashSet<Anexo>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int ConsecionID { get; set; }
        public int ClienteID { get; set; }
        public string Titulo { get; set; }
        public long? PukConsecionID { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Analisi> Analisis { get; set; }
        public virtual ICollection<Anexo> Anexos { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
