using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Localidad
    {
        public Localidad()
        {
            Direccions = new HashSet<Direccion>();
        }

        public int LocalidadID { get; set; }
        public int MunicipioID { get; set; }
        public int EstadoID { get; set; }
        public string Nombre { get; set; }

        public virtual Municipio Municipio { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
    }
}
