using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;

namespace SIRALAB.Common
{
    public class SeguridadPagina
    {


        /// <summary>
        /// Valida si el usuario tiene permiso en la pagina
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        static public void PermisoPagina(Usuario usr, System.Web.UI.Page pagina)
        {
            Seguridad_BL sec = new Seguridad_BL();
            SIRALAB_BL.common.Seguridad_BL.TipoPermiso permiso = new SIRALAB_BL.common.Seguridad_BL.TipoPermiso();
            permiso = usr.TipoPermiso();

            if (permiso == SIRALAB_BL.common.Seguridad_BL.TipoPermiso.Lectura)
            {
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(pagina, typeof(System.Web.UI.Page), "Mensaje", "ModoConsulta();", true);
            }
            if (permiso == SIRALAB_BL.common.Seguridad_BL.TipoPermiso.Ninguno)
            {
                pagina.Response.Redirect(usr.URL_Default);
            }
        }

        static public List<SIRALAB_DAL.CatModulo> Menu(Usuario usr)
        {
            List<SIRALAB_DAL.CatModulo> modulos = new List<SIRALAB_DAL.CatModulo>();
            Seguridad_BL seguridad = new Seguridad_BL();
            modulos = seguridad.Obten_Modulos_Permitidos(usr);

            return modulos;

        }

        static public List<SIRALAB_DAL.CatPagina > paginasModulo(Usuario usr,int ? moduloId)
        {
            List<SIRALAB_DAL.CatPagina> pag = new List<SIRALAB_DAL.CatPagina>();
            Seguridad_BL seguridad = new Seguridad_BL();
            pag = seguridad.PaginasModulo(usr, moduloId).OrderBy(a=>a.Orden).ToList();
            return pag;

        }
    }
}