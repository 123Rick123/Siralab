using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratorioSucursal
    {
        public int LaboratorioSucursalID { get; set; }
        public int LaboratorioPadreID { get; set; }
        public int LaboratorioHijoID { get; set; }

        public virtual Laboratorio LaboratorioHijo { get; set; }
        public virtual Laboratorio LaboratorioPadre { get; set; }
    }
}
