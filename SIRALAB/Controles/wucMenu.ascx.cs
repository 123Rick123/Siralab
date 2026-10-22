using AjaxControlToolkit;
using SIRALAB.recursos;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.controles;
using SIRALAB_BL.negocio.login;

namespace SIRALAB.Controles
{
    public partial class wucMenu : System.Web.UI.UserControl
    {
        private StringBuilder sbMenu = new StringBuilder();
        private int childCount = 0;
        public Usuario Usuario { get { return Session["UsuarioLog"] == null ? null : (Usuario)Session["UsuarioLog"]; } }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler Salir;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptInclude(this.Page, typeof(Page), "menu-actions", ResolveClientUrl("~/js/menu-acciones.js"));
        }

        //==== Method to create dynamic menu.
        public string generateDynamicMenu()
        {
            StringBuilder sbMenu = new StringBuilder();

            sbMenu.Append(" <ul class=\"nav navbar-nav navbar-right\">");

            string childItems = getMenuItems(null);
            sbMenu.Append(childItems);

            sbMenu.Append(" </ul>");


            return sbMenu.ToString();
        }

        public string getMenuItems(int? parentId)
        {
            Usuario usr = (Usuario)Session["UsuarioLog"];
            if (usr != null)
            {
                List<SIRALAB_DAL.CatModulo> modulos = SIRALAB.Common.SeguridadPagina.Menu(usr);

                List<SIRALAB_DAL.CatModulo> menuObj = modulos.Where(a => a.ModuloPadreID == parentId).ToList();

                SIRALAB_DAL.CatModulo Salir = new SIRALAB_DAL.CatModulo();
                SIRALAB_DAL.CatModulo Inicio = new SIRALAB_DAL.CatModulo();

                Salir.URL = "Login.aspx";
                Inicio.URL = usr.URL_Default;
                Salir.Nombre = "Salir";
                Inicio.Nombre = "Inicio";
                Inicio.Orden = 0;
                Salir.Orden = menuObj.Max(p => p.Orden + 1);
                Salir.CssClass = "";
                Inicio.CssClass = "";
                menuObj.Add(Inicio);
                menuObj.Add(Salir);

                menuObj = menuObj.OrderBy(p => p.Orden).ToList();
                sbMenu.Append("<nav class=\"collapse navbar-collapse\"  >");
                sbMenu.Append("<ul id=\"menu-primary\" class=\"nav navbar-nav\" >");

                foreach (SIRALAB_DAL.CatModulo obj in menuObj)
                {
                    childCount = modulos.Count(r => r.ModuloPadreID == obj.ModuloID);
                    string cssClassM = "";
                    string cssClassMenu = (obj.CssClass != "") ? " class=\"" + obj.CssClass + "\"" : "";
                    List<SIRALAB_DAL.CatPagina> paginas = SIRALAB.Common.SeguridadPagina.paginasModulo(usr, obj.ModuloID);

                    if (obj.URL.Trim() != "")
                    {
                        if (childCount > 0 || paginas.Where(p => p.Activo && (bool)p.VisibleMenu).Count() > 0)
                        {
                            sbMenu.Append("<li class='dropdown'><a>" + obj.Nombre + "</a>");
                        }
                        else
                            sbMenu.Append("<li class='dropdown'><a href='" + Page.ResolveUrl(obj.URL) + "' >" + obj.Nombre + "</a></li>");
                    }
                    else
                    {
                        if (childCount > 0 || paginas.Where(p => p.Activo && (bool)p.VisibleMenu).Count() > 0)
                            sbMenu.Append("<li class=\"dropdown\" ><a class='dropdown-toggle' data-toggle='dropdown' role='button'>" + obj.Nombre + "</a><ul class='dropdown-menu' role='menu'>");
                    }

                    if (childCount > 0)
                    {
                        getMenuItems(obj.ModuloID);
                    }
                    else
                    {
                        paginas = paginas.Where(l => (bool) l.VisibleMenu).ToList();
                        if (paginas.Count > 0)
                        {
                            foreach (SIRALAB_DAL.CatPagina pag in paginas)
                            {
                                    string cssClass = (pag.CssClass != "") ? " class=\"" + pag.CssClass + "\"" : "";
                                    sbMenu.Append("<li class=\"menu\" ><a href='" + Page.ResolveUrl(pag.URL) + "' >" +
                                                  pag.Descripcion + "</a></li>");
                            }
                            sbMenu.Append("</ul></li>");
                        }
                    }
                }
                sbMenu.Append("</ul></nav>");
            }
            return sbMenu.ToString();
        }

        public void Show()
        {
            ApplicationSettings aS = new ApplicationSettings();
            var MenuStr = generateDynamicMenu();


            divMenu.InnerHtml = MenuStr;
        }

        protected void Salir_Click(object sender, EventArgs e)
        {
            if (Salir != null)
                Salir(this, null);
        }

        #region Cambiar password
        protected void wucCambiarPassword_Guardar(object sender, CambiarPasswordArg e)
        {
            Login_BL log = new Login_BL();

            Usuario usr = (Usuario)Session["UsuarioLog"];

            if (log.CambiarContrasena(usr, e.PasswordActual, e.PasswordNuevo, e.PasswordRepite, e.Correo))
            {
                var resp = log.CambioContrasena(e.Correo);
                this.Page.PopMensaje(log.Mensaje, "Cambio de contraseña", controles.Opciones.SoloAceptar, controles.Icono.ok, "MostrarCambioPassOk");
            }
            else
            {
                this.Page.PopMensaje(log.Mensaje, "Error", controles.Opciones.SoloAceptar, controles.Icono.error, "MostrarCambioPass");
            }
        }

        protected void wucCambiarPassword_Cerrar(object sender, EventArgs e)
        {

        }

        protected void wucCambiarPassword_Cancelar(object sender, EventArgs e)
        {

        }

      
        #endregion
    }
}