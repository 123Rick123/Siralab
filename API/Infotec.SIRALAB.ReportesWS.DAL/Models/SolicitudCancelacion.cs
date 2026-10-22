using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class SolicitudCancelacion
    {
        public SolicitudCancelacion()
        {
            SolicitudCancelacionDetalles = new HashSet<SolicitudCancelacionDetalle>();
        }

        public int SolicitudCancelacionID { get; set; }
        public int EstatusID { get; set; }
        public int LaboratorioID { get; set; }
        public string Motivo { get; set; }
        public string FirmaSolicitud { get; set; }
        public string FirmaAprobacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaAprobacion { get; set; }

        public virtual Estatus Estatus { get; set; }
        public virtual Laboratorio Laboratorio { get; set; }
        public virtual ICollection<SolicitudCancelacionDetalle> SolicitudCancelacionDetalles { get; set; }
    }
}
