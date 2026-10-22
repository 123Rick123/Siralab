using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class TBitacora
    {
        public int? idUser { get; set; }
        public int Idrow { get; set; }
        public DateTime? FechaMod { get; set; }
        public string NUser { get; set; }
        public string ModuloMod { get; set; }
        public string Firma { get; set; }
        public string CadenaFirma { get; set; }
        public string TipoMov { get; set; }
        public string NSerieCert { get; set; }
    }
}
