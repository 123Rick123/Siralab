using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Contacto
    {
        public int ContactoID { get; set; }
        public int PersonaID { get; set; }
        public int TipoContactoID { get; set; }
        public string Contacto1 { get; set; }
        public string Extension { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? RecibeAcuse { get; set; }

        public virtual Persona Persona { get; set; }
        public virtual CatTipoContacto TipoContacto { get; set; }
    }
}
