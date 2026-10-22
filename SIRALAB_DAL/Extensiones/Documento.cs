using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL
{
    public partial class Documento
    {
        public Guid UniqueId { get; set; }
        public byte[] BufferData { get; set; }
        public string ImagePreView { get; set; }
        public int? TipoDocumentoID { get; set; }
        public string TipoDocumento { get; set; }
        public int? AnalisisID { get; set; }
        public bool? eliminar { get; set; }
    }

    public class DocumentoEqualityComparer : IEqualityComparer<Documento>
    {
        public bool Equals(Documento documento1, Documento documento2)
        {
            if (documento1.DocumentoID == documento2.DocumentoID)
                return true;
            return false;
        }
        public int GetHashCode(Documento documento)
        {
            return new { documento.DocumentoID, documento.Nombre, documento.TipoMime }.GetHashCode();
        }
    }
}
