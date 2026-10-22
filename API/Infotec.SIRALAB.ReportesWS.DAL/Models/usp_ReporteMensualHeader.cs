using System;
using System.Collections.Generic;
using System.Text;

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public class usp_ReporteMensualHeader
    {
        public  string ClienteNombre { get; set; }
        public string ClienteRFC { get; set; }
        public string Anexo { get; set; }
        public string AnexoCuerpoReceptorNombre { get; set; }
        public string PuntoDescargaNombreCuerpoReceptor { get; set; }
        public string PuntoDescargaTitulo { get; set; }
        public string NormaNombre { get; set; }
        public string UsoAguaNombre { get; set; }
        public string CuerpoReceptorNombre { get; set; }
    }
}
