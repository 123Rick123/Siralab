using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class ClienteContacto
    {
        public int ClienteContactoID { get; set; }
        public int ClienteID { get; set; }
        public int PersonaID { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
