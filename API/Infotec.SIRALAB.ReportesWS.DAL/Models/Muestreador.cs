using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Muestreador
    {
        public int MuestreadorID { get; set; }
        public int MuestraID { get; set; }
        public int LaboratorioSignatarioID { get; set; }
        public bool? Activo { get; set; }

        public virtual LaboratorioSignatario LaboratorioSignatario { get; set; }
        public virtual Muestra Muestra { get; set; }
    }
}
