using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Municipio
    {
        public Municipio()
        {
            Colonia = new HashSet<Colonium>();
            Direccions = new HashSet<Direccion>();
            Localidads = new HashSet<Localidad>();
        }

        public int MunicipioID { get; set; }
        public int EstadoID { get; set; }
        public string Nombre { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual ICollection<Colonium> Colonia { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
        public virtual ICollection<Localidad> Localidads { get; set; }
    }
}
