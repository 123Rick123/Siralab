using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class CatTipoDocumentoAnexo_BL: Base_BL<CatTipoDocumentoAnexo>
    {
        public CatTipoDocumentoAnexo_BL() : base() { }
        public IEnumerable<CatTipoDocumentoAnexo> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList();
        }

        public CatTipoDocumentoAnexo Obtener(int tipoDocumentoAnexoID)
        {
            this.Entidad = Repositorio.Single(a => a.TipoDocumentoAnexoID == tipoDocumentoAnexoID);
            return Entidad;
        }

    }
}
