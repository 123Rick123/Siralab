using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL.Extensiones
{
    public static class CatPaginaExt
    {
        /// <summary>
        /// Nombre del modulo
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public static string NombreModulo(this CatPagina pagina)
        {
            var retorno = "";
            if (pagina.CatModulo != null)
            {
                retorno = pagina.CatModulo.Nombre;
            }
            return retorno;
        }

        /// <summary>
        /// Está activa la pagina
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public static string esActivo(this CatPagina pagina)
        {
            var retorno = (pagina.Activo) ? "Activa" : "No Activa";

            return retorno;
        }

        /// <summary>
        /// Se muestra en el menú
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public static string esVisibleMenu(this CatPagina pagina)
        {
            var retorno = (pagina.VisibleMenu??false) ? "Visible en Menú" : "Invisible en Menú";

            return retorno;
        }

       
    }
}
