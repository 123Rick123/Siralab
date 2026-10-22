using System;
using System.Collections.Generic;
using System.Text;

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class MensajePCNOM
    {
        public int id { get; set; }
        public string Descripcion { get; set; }

        public Nullable<bool> Activo { get; set; }
    }
}
