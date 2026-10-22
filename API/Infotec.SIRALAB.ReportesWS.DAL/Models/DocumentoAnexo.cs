using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class DocumentoAnexo
    {
        public int DocumentoAnexoID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int DocumentoID { get; set; }
        public int TipoDocumentoAnexoID { get; set; }

        public virtual Documento Documento { get; set; }
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual CatTipoDocumentoAnexo TipoDocumentoAnexo { get; set; }
    }
}
