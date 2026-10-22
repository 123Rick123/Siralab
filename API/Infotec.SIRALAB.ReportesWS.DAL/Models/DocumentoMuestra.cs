using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class DocumentoMuestra
    {
        public int DocumentoMuestraID { get; set; }
        public int MuestraID { get; set; }
        public int DocumentoID { get; set; }
        public int TipoDocumentoMuestraID { get; set; }

        public virtual Documento Documento { get; set; }
        public virtual Muestra Muestra { get; set; }
        public virtual CatTipoDocumentoMuestra TipoDocumentoMuestra { get; set; }
    }
}
