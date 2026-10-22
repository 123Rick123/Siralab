using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.negocio.login;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB.Controles;
using SIRALAB_SIRFIC;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;

namespace SIRALAB
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkbAceptar_Click(object sender, EventArgs e)
        {
            Login_BL log = new Login_BL();

            if (log.TieneAcceso(txtUsuario.Text, txtPassword.Text))
            {
                Session["UsuarioLog"] = log.Entidad;
                Session["LaboratorioVigencia"] = log.Vigencia;
                //Response.Redirect(log.Entidad.URL_Default);
                if (log.Entidad.LaboratorioID != null)
                {
                    wucSucursalLogin.MostrarModal(log.Entidad.LaboratorioID.Value, log.Entidad.URL_Default);
                }
                else
                {
                    Response.Redirect(log.Entidad.URL_Default);
                }
               
            }
            else
            {
                if (log.ListaUsuario != null)
                    if (log.ListaUsuario.Count > 0)
                    {
                       
                        /* Lanzamos el listado de usuarios en pantalla para su selección */
                        var rfc = log.ListaUsuario.First().Usuario1;
                        wucSucursalLogin.Mostrar(rfc);
                    }
                    else
                    {
                        this.PopMensaje(log.Mensaje, "Análisis de Laboratorio", controles.Opciones.SoloAceptar, controles.Icono.error);
                    }
                else
                {
                    this.PopMensaje(log.Mensaje, "Análisis de Laboratorio", controles.Opciones.SoloAceptar, controles.Icono.error);
                }
            }
        }

        protected void lnkbEnviar_Click(object sender, EventArgs e)
        {
            /// Generar correo electrónico para enviar la contraseña en caso de ser un usuario que puede acceder a la aplicación
            Login_BL log = new Login_BL();

            var resp = log.RecuperaContrasena(txtCorreo.Text);
            this.PopMensaje(log.Mensaje, "Análisis de Laboratorio", controles.Opciones.SoloAceptar, resp ? controles.Icono.ok : controles.Icono.error);
        }

        protected void wucSucursalLogin_Seleccionar(object sender, LoginSucursalArg e)
        {
            /* Validamos que la contraseña sea la correcta y que tenga permisos */

            Laboratorio lab = e.laboratorio;
            Login_BL log = new Login_BL();

            if (lab.Usuarios.Count > 0)
            {
                if (log.TieneAcceso(lab, txtUsuario.Text, txtPassword.Text))
                {
                    Session["UsuarioLog"] = log.Entidad;
                    Session["LaboratorioVigencia"] = log.Vigencia;
                    wucSucursalLogin.MostrarModal(lab.LaboratorioID, log.Entidad.URL_Default);
                    //Response.Redirect(log.Entidad.URL_Default);
                }
                else
                {
                    this.PopMensaje(log.Mensaje, "Análisis de Laboratorio", controles.Opciones.SoloAceptar, controles.Icono.error);
                }
            }
        }

        
    }
}