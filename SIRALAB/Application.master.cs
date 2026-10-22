using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SIRALAB.Common;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using SIRALAB.controles;
using SIRALAB_BL.negocio.login;
using System.ComponentModel;
using SIRALAB_BL.catalogos;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;

namespace SIRALAB
{
    public partial class Application : System.Web.UI.MasterPage
    {
        [Browsable(true)]
        public event EventHandler<CambiarPasswordArg> Guardar;
        [Browsable(true)]
        public event EventHandler Cancelar;
        [Browsable(true)]
        public event EventHandler Cerrar;
        private int UsuarioID
        {
            get { return int.Parse(ViewState["UsuarioID"].ToString()); }
            set { ViewState["UsuarioID"] = value; }
        }
        private string PaginaIndex
        {
            get { return Session["PaginaIndex"].ToString(); }
            set { Session["PaginaIndex"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session.Count == 0)
                Response.Redirect("~/Login.aspx");

        }

        private Main main => (Main)this.Master;

        protected void Page_Load(object sender, EventArgs e)
        {
            breadcrumb();
            main.MensajeDevolucion += main_MensajeDevolucion;
            
            if (!IsPostBack)
            {
                Usuario usr = (Usuario)Session["UsuarioLog"];
                Mostrar(usr.UsuarioID);
                if (usr.PersonaID != null)
                    lblUsuario.Text = usr.Persona.NombreCompleto;
                else
                    lblUsuario.Text = usr.Laboratorio.LaboratorioNombre() + " (" + usr.Laboratorio.LaboratorioNombreEstado() + ")";

                wucMenu1.Show();
            }
        }
        public void breadcrumb()
        {
            Usuario usr = (Usuario)Session["UsuarioLog"];
            if (usr != null)
            {
                string cadena = HttpContext.Current.Request.Url.AbsoluteUri;
                string[] Separado = cadena.Split('/');
                string Final = Separado[Separado.Length - 1];
                string FinalLaboratorios = string.Empty;
                List<SIRALAB_DAL.CatModulo> modulos = SIRALAB.Common.SeguridadPagina.Menu(usr);
                string rutaxml = Server.MapPath(@"recursos\Flujo.xml");
                XDocument reader = XDocument.Load(rutaxml);  
                var flujo = reader.Descendants("flujo").Select(x => new { Ruta = x.Element("Ruta").Value, Nombre = x.Element("Nombre").Value, Rama = x.Element("rama").Value.Replace('$','<').Replace('%','>') });
                string breadcrumb = "";
                if (Final.Contains("ApercibidosYReincidentes.aspx"))
                {
                    Final = "Laboratorios apercibidos y reincidentes";
                    FinalLaboratorios = "Laboratorios apercibidos y reincidentes";
                }
                if (Final.Contains("NuevoApercibimiento.aspx"))
                {
                    Final = "Nuevo apercibimiento";
                    FinalLaboratorios = "Nuevo apercibimiento";
                }
                if (Final.Contains("NuevaReincidencia.aspx"))
                {
                    Final = "Nueva reincidencia";
                    FinalLaboratorios = "Nueva reincidencia";
                }
                if (Final.Contains("ImpugnacionReincidencia.aspx"))
                {
                    Final = "Impugnación reincidencia";
                    FinalLaboratorios = "Impugnación reincidencia";
                }
                if (Final.Contains("ImpugnacionApercibimiento.aspx"))
                {
                    Final = "Impugnación apercibimiento";
                    FinalLaboratorios = "Impugnación apercibimiento";
                }
                if (Final.Contains("ImpugnacionReincidenciaHistorico.aspx"))
                {
                    Final = "Consulta impugnación reincidencia";
                    FinalLaboratorios = "Consulta impugnación reincidencia";
                }
                if (Final.Contains("ImpugnacionApercibimientoHistorico.aspx"))
                {
                    Final = "Consulta impugnación apercibimiento";
                    FinalLaboratorios = "Consulta impugnación apercibimiento";
                }
                if (Final.Contains("DesgloseHistoricoImpugnacionApercibimiento.aspx"))
                {
                    Final = "Consulta impugnación apercibimiento";
                    FinalLaboratorios = "Consulta impugnación apercibimiento";
                }
                if (Final.Contains("DesgloseHistoricoImpugnacionApercibimiento.aspx"))
                {
                    Final = "Consulta impugnación apercibimiento";
                    FinalLaboratorios = "Consulta impugnación apercibimiento";
                }

                if (Final.Contains("ConsultaApercibimientoReincidencia.aspx"))
                {
                    Final = "Consulta apercibidos y reincidentes";
                    FinalLaboratorios = "Consulta apercibidos y reincidentes";
                }
                if (Final == "Orden.aspx" || Final== "Inicio.aspx")
                {
                    breadcrumb = @"<ol class='breadcrumb'>
                                      <li><a href = '"+ Final + @"'><i class='icon icon-home'></i></a></li>
                                      <li class='active'> Inicio</li>";
                    PaginaIndex = Final;
                }
                else
                {
                    if (!flujo.Any(x => x.Ruta == Final))
                    {
                        if (FinalLaboratorios!=string.Empty)
                        {
                            breadcrumb = @" <ol class='breadcrumb'>
                                      <li><i class='icon icon-home'></i></li>
                                      <li> Inicio </li>" +
                                               " <li class='active'>" + Final.Split('.')[0] + "</li>"
                                            + "</ol>";
                        }
                        else
                        {
                            breadcrumb = @" <ol class='breadcrumb'>
                                      <li><a href = '" + PaginaIndex + @"'><i class='icon icon-home'></i></a></li>
                                      <li><a href = '" + PaginaIndex + @"'> Inicio </a></li>" +
                                               " <li class='active'>" + Final.Split('.')[0] + "</li>"
                                            + "</ol>";

                        }
                       
                    }
                    else
                    {
                        breadcrumb = @" <ol class='breadcrumb'>
                                      <li><a href = '" + PaginaIndex + @"'><i class='icon icon-home'></i></a></li>"+
                                      flujo.Where(x=>x.Ruta==Final).Select(x=> x.Rama).First()+
                                                " <li class='active'>" + flujo.Where(x => x.Ruta == Final).Select(x => x.Nombre).First() + "</li>"
                                             + "</ol>";
                    }
                }
                divbreadcrumb.InnerHtml= breadcrumb;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ModoConsulta();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void ModoConsulta()
        {
            Usuario usr = (Usuario)Session["UsuarioLog"];
            if (usr != null)
                SeguridadPagina.PermisoPagina(usr, this.Page);
        }

        void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            switch (e.Command)
            {
                case "MostrarCambioPass":
                    var usr = (Usuario)Session["UsuarioLog"];
                   Mostrar(usr.UsuarioID);
                    break;
                case "MostrarCambioPassOk":
                    wucMenu1_Salir(null, null);
                    break;
            }

            main.MensajeDevolucion -= main_MensajeDevolucion;
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

        protected void lnkbCambiarPass_Click(object sender, EventArgs e)
        {
            Usuario usr = (Usuario)Session["UsuarioLog"];
            Mostrar(usr.UsuarioID);
        } 
        #endregion

        protected void wucMenu1_Salir(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
      

        protected void lnkbCambiarPass_Click1(object sender, EventArgs e)
        {
            Usuario usr = (Usuario)Session["UsuarioLog"];
            Mostrar(usr.UsuarioID);
        }
       
        public void Mostrar(int UsuarioID)
        {
            this.UsuarioID = UsuarioID;
            //Cargamos el correo electrónico del Usuario
            Usuario_BL uBL = new Usuario_BL();
            var usuario = uBL.Obten(UsuarioID);
            Session["correo"] = usuario.Correo;
            //    this.control_SetFocus("txtPassword");
            //Limpiar();
            txtEmail.Text = Session["correo"].ToString();           
            
        }

        private void Limpiar()
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtPasswordNuevo.Text = "";
            txtPasswordRepite.Text = "";
        }

        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            var Args = new CambiarPasswordArg
            {
                UsuarioID = this.UsuarioID,
                Correo = txtEmail.Text,
                PasswordActual = txtPassword.Text,
                PasswordNuevo = txtPasswordNuevo.Text,
                PasswordRepite = txtPasswordRepite.Text

            };
            if (Args != null)
            {
                wucCambiarPassword_Guardar(this, Args);
            }

        }

        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {
            if (Cancelar != null)
            {
                Cancelar(this, new EventArgs());
            }
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            if (Cerrar != null)
            {
                Cerrar(this, new EventArgs());
            }
        }
    }
}