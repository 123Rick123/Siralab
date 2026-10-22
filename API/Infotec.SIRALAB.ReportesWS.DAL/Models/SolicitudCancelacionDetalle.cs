using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class SolicitudCancelacionDetalle
    {
        public int SolicitudCancelacionDetalleID { get; set; }
        public int SolicitudCancelacionID { get; set; }
        public int AnalisisID { get; set; }

        public virtual Analisi Analisis { get; set; }
        public virtual SolicitudCancelacion SolicitudCancelacion { get; set; }
    }
}
