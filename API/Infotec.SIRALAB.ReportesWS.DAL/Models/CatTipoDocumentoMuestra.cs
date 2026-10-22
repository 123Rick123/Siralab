using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatTipoDocumentoMuestra
    {
        public CatTipoDocumentoMuestra()
        {
            DocumentoMuestras = new HashSet<DocumentoMuestra>();
        }

        public int TipoDocumentoMuestraID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<DocumentoMuestra> DocumentoMuestras { get; set; }
    }
}
