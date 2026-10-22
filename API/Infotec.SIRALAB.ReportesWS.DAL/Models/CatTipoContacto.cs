using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatTipoContacto
    {
        public CatTipoContacto()
        {
            Contactos = new HashSet<Contacto>();
        }

        public int TipoContactoID { get; set; }
        public string Nombre { get; set; }
        public bool RequiereInfo { get; set; }

        public virtual ICollection<Contacto> Contactos { get; set; }
    }
}
