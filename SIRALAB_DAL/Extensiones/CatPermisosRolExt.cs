using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL.Extensiones
{
    public static class CatPermisosRolExt
    {

        public static string urlPagina(this CatPermisosRol permisoRol)
        {
            var retorno = "";
            if (permisoRol.CatPagina != null)
            {
                retorno = permisoRol.CatPagina.URL;
            }
            return retorno;
        }
        public static string NombreRol(this CatPermisosRol permisoRol)
        {
            var retorno = "";
            if (permisoRol.CatUsuarioRol != null)
            {
                retorno = permisoRol.CatUsuarioRol.Nombre;
            }
            return retorno;
        }


        public static string esActivo(this CatPermisosRol permisoRol)
        {
            var retorno = (permisoRol.Activo) ? "Activo" : "In Activo";

            return retorno;
        }

        public static string esEscritura(this CatPermisosRol permisoRol)
        {
            var retorno = (permisoRol.Escritura) ? "Escritura" : "Lectura";

            return retorno;
        }

       
    }
}
