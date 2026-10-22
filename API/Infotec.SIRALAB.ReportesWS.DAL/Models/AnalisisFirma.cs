using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class AnalisisFirma
    {
        public int AnalisisFirmaID { get; set; }
        public int AnalisisID { get; set; }
        public string RFC { get; set; }
        public string Firma { get; set; }
        public string Cadena { get; set; }
        public string Contenedor { get; set; }
        public string SelloDigital { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Analisi Analisis { get; set; }
    }
}
