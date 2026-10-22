using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class AnexoMedidor
    {
        public int AnexoMedidorID { get; set; }
        public int AnexoID { get; set; }
        public long? ID_SERIE_MEDIDOR { get; set; }
        public string numeroMedidor { get; set; }
        public long? ID_MARCA_MEDIDOR { get; set; }
        public string MARCA_MEDIDOR { get; set; }
        public bool? FuenteAbastecimiento { get; set; }
        public long? idUman { get; set; }

        public virtual Anexo Anexo { get; set; }
    }
}
