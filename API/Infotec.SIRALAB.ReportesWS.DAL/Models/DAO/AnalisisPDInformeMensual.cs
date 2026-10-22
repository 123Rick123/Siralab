using System;
using System.Collections.Generic;
using System.Text;

namespace Infotec.SIRALAB.ReportesWS.DAL.Models.DAO
{
    public class AnalisisPDInformeMensual
    {
        public int AnalisisId { get; set; }
        public int NivelId { get; set; }
        public string Etiqueta { get; set; }
        public string Resultado { get; set; }
        public string LMP { get; set; }
        public string Cumple { get; set; }
    }
}
