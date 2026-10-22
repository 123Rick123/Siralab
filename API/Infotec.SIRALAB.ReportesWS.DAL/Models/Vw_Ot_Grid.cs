using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Vw_Ot_Grid
    {
        public int OrdenTrabajoID { get; set; }
        public string Titulo { get; set; }
        public string Anexo { get; set; }
        public string RFC { get; set; }
        public int NormaID { get; set; }
        public string Norma { get; set; }
        public int LaboratorioID { get; set; }
        public string NombreLaboratorio { get; set; }
        public string RFCLaboratorio { get; set; }
        public string Informe { get; set; }
        public bool Firma { get; set; }
        public string Firmada { get; set; }
        public int EstatusID { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaConformacion { get; set; }
    }
}
