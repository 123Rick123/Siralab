using SIRALAB_BL.seguridad;
using SIRALAB_DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB_BL.common
{
    public class Seguridad_BL
    {

        public Seguridad_BL()
        {
            this.lModulo = new List<CatModulo>();
            this.lPagina = new List<CatPagina>();
        }
        private List<CatPagina> lPagina { get; set; }
        private List<CatModulo> lModulo { get; set; }

        int SistemaId
        {
            get
            {
                ApplicationSettings ap = new ApplicationSettings();
                return int.Parse(ap.SistemaId);
            }
        }

        /// <summary>
        /// Valida si el usuario tiene permiso en la pagina
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public bool TienePermisoPagina(Usuario usr)
        {
            bool permiso = false;

            CatModulos_BL cmod = new CatModulos_BL();
            try
            {
                var modulos = cmod.ObtenerTodos().Where(p =>
                    p.CatPaginas.Where(q =>
                        q.CatPermisosRols.Where(r => r.UsuarioRolID == usr.UsuarioRolID && r.Activo).Count() > 0 && q.URL.ToUpper() == ObtenPagina() && q.SistemaID == SistemaId
                ).Count() > 0);


                if (modulos.Count() != 0)
                {
                    permiso = true;
                }
            }
            catch
            {
                permiso = false;
            }

            return permiso;
        }

        public List<CatModulo> ModulosMenu(Usuario usr)
        {
            List<CatModulo> modulos = new System.Collections.Generic.List<CatModulo>();
            CatModulos_BL cmod = new CatModulos_BL();

            var modulos1 = cmod.ObtenerTodos().Where(p =>
               p.CatPaginas.Where(q =>
                   q.CatPermisosRols.Where(r => r.UsuarioRolID == usr.UsuarioRolID && r.Activo && q.SistemaID == SistemaId).Count() > 0
           ).Count() > 0).Select(r => r.CatPaginas).ToList();

            return modulos;
        }

        #region Obtener Módulos

        public List<CatModulo> Obten_Modulos_Permitidos(Usuario usr)
        {
            Paginas_Permitidas(usr);
            foreach (var pag in this.lPagina)
            {
                Modulos_Permitidos(pag.CatModulo);
            }

            return this.lModulo.OrderBy(a => a.Orden).ToList();
        }


        public void Paginas_Permitidas(Usuario usr)
        {
            CatPermisosRol_BL cprg = new CatPermisosRol_BL();
            var lPagTemp = cprg.ObtenerTodos().Where(q => q.UsuarioRolID == usr.UsuarioRolID && q.Activo && q.CatPagina.Activo && q.CatPagina.SistemaID == SistemaId);

            this.lPagina = lPagTemp.Select(p => p.CatPagina).ToList();
        }

        public void Modulos_Permitidos(CatModulo modulo)
        {
            if (modulo.CatModulo1 != null)
            {
                Modulos_Permitidos(modulo.CatModulo1);
            }


            var existe = lModulo.Where(p => p.ModuloID == modulo.ModuloID);

            if (existe.Count() == 0)
                this.lModulo.Add(modulo);
        }
        #endregion

        public List<CatPagina> PaginasModulo(Usuario usr, int? moduloId)
        {
            List<CatPagina> pagina = new System.Collections.Generic.List<CatPagina>();
            CatPagina_BL cmod = new CatPagina_BL();

            pagina = cmod.ObtenerTodos().Where(q =>
                   q.CatPermisosRols.Where(r => r.UsuarioRolID == usr.UsuarioRolID && r.Activo && q.SistemaID == SistemaId).Count() > 0 && q.ModuloID == moduloId
           ).ToList();


            return pagina;
        }

        /// <summary>
        /// Valida si el usuario tiene permiso en la pagina
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public TipoPermiso TipoPermisoPagina(Usuario usr)
        {
            TipoPermiso permiso = TipoPermiso.Ninguno;

            CatModulos_BL cmod = new CatModulos_BL();
            var modulos = cmod.ObtenerTodos().Where(p =>
                p.CatPaginas.Where(q =>
                    q.CatPermisosRols.Where(r => r.UsuarioRolID == usr.UsuarioRolID && r.Activo).Count() > 0 && q.URL.ToUpper() == ObtenPagina() && q.SistemaID == SistemaId
            ).Count() > 0);

            if (modulos.Count() != 0)
            {
                var permisoTemp = modulos.Where(p =>
                    p.CatPaginas.Where(q =>
                        q.CatPermisosRols.Where(r => r.UsuarioRolID == usr.UsuarioRolID && r.Activo).Count() > 0 && q.URL.ToUpper() == ObtenPagina() && q.Activo
                    ).Count() > 0).Select(s => s.CatPaginas.Where(u => u.URL.ToUpper() == ObtenPagina() && u.SistemaID == SistemaId).Select(t => t.CatPermisosRols).SingleOrDefault()).SingleOrDefault();

                if (permisoTemp != null)
                {
                    permiso = permisoTemp.Where(r => r.UsuarioRolID == usr.UsuarioRolID).FirstOrDefault().Escritura ? TipoPermiso.Escritura : TipoPermiso.Lectura;
                }
                else
                {
                    permiso = TipoPermiso.Ninguno;
                }
            }

            return permiso;
        }

        /// <summary>
        /// Obtiene la pagina current
        /// </summary>
        /// <returns></returns>
        private string ObtenPagina()
        {
            HttpRequest request = HttpContext.Current.Request;
            string sPagePath = request.Url.AbsolutePath;
            System.IO.FileInfo oFileInfo = new System.IO.FileInfo(sPagePath);
            return oFileInfo.Name.ToUpper();
        }

        public enum TipoPermiso
        {
            Ninguno,
            Escritura,
            Lectura
        }
    }

    public static class Seguridad_BLExt
    {
        public static bool TienePermiso(this Usuario usr)
        {
            Seguridad_BL sBL = new Seguridad_BL();
            return sBL.TienePermisoPagina(usr);
        }

        public static Seguridad_BL.TipoPermiso TipoPermiso(this Usuario usr)
        {
            Seguridad_BL sBL = new Seguridad_BL();
            return sBL.TipoPermisoPagina(usr);
        }
    }
}