using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class AnalisisColaDetalle
    {
        public AnalisisColaDetalle()
        {
            AnalisisColaEncabezados = new HashSet<AnalisisColaEncabezado>();
        }

        public int AnalisisColaDetalleID { get; set; }
        public bool Activo { get; set; }
        public bool Completo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }

        public virtual ICollection<AnalisisColaEncabezado> AnalisisColaEncabezados { get; set; }
    }
}
