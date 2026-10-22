using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL.Extensiones
{
    public static class CatModuloExt
    {
        public static string NombreModuloPadre(this CatModulo modulo)
        {
            var retorno = "";
            if (modulo.CatModulo1 != null)
            {
                retorno = modulo.CatModulo1.Nombre;
            }
            return retorno;
        }
    }
}
