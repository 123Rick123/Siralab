using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class LaboratoriosCNA
    {
        public int Id { get; set; }
        public int? IdLaboratorio { get; set; }
        public string RfcLaboratorio { get; set; }
        public bool? ValCorreo { get; set; }
        public bool? ValExtemporaneo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
