using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Direccions = new HashSet<Direccion>();
            Municipios = new HashSet<Municipio>();
        }

        public int EstadoID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Direccion> Direccions { get; set; }
        public virtual ICollection<Municipio> Municipios { get; set; }
    }
}
