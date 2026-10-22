using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class ColoniaTipo
    {
        public ColoniaTipo()
        {
            Colonia = new HashSet<Colonium>();
        }

        public int ColoniaTipoID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Colonium> Colonia { get; set; }
    }
}
