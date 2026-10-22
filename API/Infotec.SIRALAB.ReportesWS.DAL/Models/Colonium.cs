using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Colonium
    {
        public Colonium()
        {
            Direccions = new HashSet<Direccion>();
        }

        public int ColoniaID { get; set; }
        public int? ColoniaTipoID { get; set; }
        public int MunicipioID { get; set; }
        public int EstadoID { get; set; }
        public int CP { get; set; }
        public string Nombre { get; set; }

        public virtual ColoniaTipo ColoniaTipo { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
    }
}
