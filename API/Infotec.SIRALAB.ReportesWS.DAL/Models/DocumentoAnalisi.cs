using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class DocumentoAnalisi
    {
        public int DocumentoAnalisisID { get; set; }
        public int DocumentoID { get; set; }
        public int AnalisisID { get; set; }

        public virtual Analisi Analisis { get; set; }
        public virtual Documento Documento { get; set; }
    }
}
