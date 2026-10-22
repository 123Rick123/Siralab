using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatTipoDocumentoAnexo
    {
        public CatTipoDocumentoAnexo()
        {
            DocumentoAnexos = new HashSet<DocumentoAnexo>();
        }

        public int TipoDocumentoAnexoID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<DocumentoAnexo> DocumentoAnexos { get; set; }
    }
}
