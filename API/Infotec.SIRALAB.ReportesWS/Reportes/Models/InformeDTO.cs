using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.Reportes.Models
{
    public class InformeDTO : InformeBase
    {
        public usp_ReporteAnalisisHeader_Result DatosGenerales { get; set; }
        public Analisi Analisis { get; set; }
        public int? numPD { get; set; }
        public string QR { get; set; }
        public string ReglaMiscelanea { get; set; }

        public string mensajeNom001 { get; set; }
    }
}
