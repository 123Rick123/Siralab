using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratorioCliente
    {
        public int LaboratorioClienteID { get; set; }
        public int LaboratorioID { get; set; }
        public int ClienteID { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Laboratorio Laboratorio { get; set; }
    }
}
