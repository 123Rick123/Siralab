using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class AnalisisColaEncabezado
    {
        public int AnalisisColaEncabezadoID { get; set; }
        public int AnalisisID { get; set; }
        public int AnalisisColaDetalleID { get; set; }
        public int LaboratorioID { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaFirma { get; set; }
        public int? EstatusID { get; set; }

        public virtual Analisi Analisis { get; set; }
        public virtual AnalisisColaDetalle AnalisisColaDetalle { get; set; }
    }
}
