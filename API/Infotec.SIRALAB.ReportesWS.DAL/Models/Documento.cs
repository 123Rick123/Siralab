using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Documento
    {
        public Documento()
        {
            DocumentoAnalisis = new HashSet<DocumentoAnalisi>();
            DocumentoAnexos = new HashSet<DocumentoAnexo>();
            DocumentoMuestras = new HashSet<DocumentoMuestra>();
        }

        public int DocumentoID { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string TipoMime { get; set; }
        public string Ruta { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<DocumentoAnalisi> DocumentoAnalisis { get; set; }
        public virtual ICollection<DocumentoAnexo> DocumentoAnexos { get; set; }
        public virtual ICollection<DocumentoMuestra> DocumentoMuestras { get; set; }
    }
}
