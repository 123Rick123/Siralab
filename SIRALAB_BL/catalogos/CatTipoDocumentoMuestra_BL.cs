using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class CatTipoDocumentoMuestra_BL : Base_BL<CatTipoDocumentoMuestra>
    {
        public CatTipoDocumentoMuestra_BL() : base() { }
        public IEnumerable<CatTipoDocumentoMuestra> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public CatTipoDocumentoMuestra Obtener(int tipoDocumentoAnexoID)
        {
            this.Entidad = Repositorio.Single(a => a.TipoDocumentoMuestraID == tipoDocumentoAnexoID);
            return Entidad;
        }

    }
}
