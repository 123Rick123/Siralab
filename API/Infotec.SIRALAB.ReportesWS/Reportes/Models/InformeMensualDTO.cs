using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Infotec.SIRALAB.ReportesWS.DAL.Models.DAO;
using Infotec.SIRALAB.ReportesWS.DAL.Models.Enums;
using Infotec.SIRALAB.ReportesWS.DAL.Services;

namespace Infotec.SIRALAB.ReportesWS.Reportes.Models
{
    public class InformeMensualDTO : InformeBase
    {
        public usp_ReporteMensualHeader UspReporteMensualHeader { get; set; }
        public List<Analisi> Analisis { get; set; }
        public List<AnalisisPDInformeMensual> AnalisisPD { get; set; }
        public List<AnalisisPDInformeMensual> CalculoMensual { get; set; }
        public TipoEvaluacion TipoEvaluacion { get; set; }

    }
}
