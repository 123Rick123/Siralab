using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratorioAprobacion
    {
        public int LaboratorioAprobacionID { get; set; }
        public int LaboratorioVigenciaID { get; set; }
        public int LaboratorioID { get; set; }
        public int ParametroMetodoID { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }
        public virtual LaboratorioVigencium LaboratorioVigencia { get; set; }
        public virtual ParametroMetodo ParametroMetodo { get; set; }
    }
}
