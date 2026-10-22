using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

namespace SIRALAB.controles
{
    public partial class wucMensajes : System.Web.UI.UserControl
    {
        private event EventHandler<MensajeArgs> devolucion;
        public event EventHandler<MensajeArgs> Devolucion { add { devolucion += value; } remove { devolucion -= value; } }
        private event EventHandler<MensajeArgs> cerrar;
        public event EventHandler<MensajeArgs> Cerrar { add { cerrar += value; } remove { cerrar -= value; } }

        private string Command { get { return hndCommand.Value; } set { hndCommand.Value = value; } }
        private string Args { get { return hndArgs.Value; } set { hndArgs.Value = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Mostrar(string mensaje, string titulo, Opciones opcion = Opciones.SoloAceptar, Icono icon = Icono.ok, string command = null, string[] args = null)
        {
            /* Manejo de los hidden fields por parte de las variables del objeto wucMensajes */
            this.Args = (args != null) ? string.Join("|*", args) : null;
            this.Command = command;

            clearMessage();
            hideAllButtons();

            switch (icon)
            {
                case Icono.ok:
                    //imgMensaje.ImageUrl = "~/image/mensajes/message-ok.png";
                    AlertMensaje.Attributes.Remove("class");
                    AlertMensaje.Attributes.Add("class", "alert alert-success alert-dismissible");
                    break;
                case Icono.information:
                    //imgMensaje.ImageUrl = "~/image/mensajes/message-information.png";
                    AlertMensaje.Attributes.Remove("class");
                    AlertMensaje.Attributes.Add("class", "alert alert-info alert-dismissible");
                    break;
                case Icono.error:
                    //imgMensaje.ImageUrl = "~/image/mensajes/message-error.png";
                    AlertMensaje.Attributes.Remove("class");
                    AlertMensaje.Attributes.Add("class", "alert alert-danger alert-dismissible");
                    break;
                case Icono.warning:
                    //imgMensaje.ImageUrl = "~/image/mensajes/message-warning.png";
                    AlertMensaje.Attributes.Remove("class");
                    AlertMensaje.Attributes.Add("class", "alert alert-warning alert-dismissible");
                    break;
                case Icono.question:
                    //imgMensaje.ImageUrl = "~/image/mensajes/message-question.png";
                    break;
            }
            lblTitulo.Text = titulo;
            switch (opcion)
            {
                case Opciones.SoloAceptar:
                    lnkbAceptar.Visible = true;
                    this.control_SetFocus(lnkbAceptar.ClientID);
                    lblMensaje.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('#ModalMensaje').modal()", true);
                    break;
                case Opciones.AceptarCancelar:
                    lnkbAceptar.Visible = true;
                    lnkbCancelar.Visible = true;
                    this.control_SetFocus(lnkbCancelar.ClientID);
                    lblmensajeconfirm.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('#ModalDialog').modal();", true);
                    break;
                case Opciones.SiNo:
                    lnkbSi.Visible = true;
                    lnkbNo.Visible = true;
                    this.control_SetFocus(lnkbNo.ClientID);
                    lblmensajeconfirm.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('#ModalDialog').modal();", true);
                    break;
                case Opciones.SoloMensaje:
                    this.control_SetFocus(lnkbAceptar.ClientID);
                    lblMensaje.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('#ModalMensaje').modal()", true);
                    break;
                case Opciones.Si:
                    lnkbAceptar.Visible = true;
                    this.control_SetFocus(lnkbNo.ClientID);
                    lblmensajeconfirm.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('#ModalDialog').modal();", true);
                    break;
            }



            //string scrollPanelScript = "$(function () { $('.scroll-pane').jScrollPane({ showArrows: true }); });";
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "messageScroll", scrollPanelScript, true);
        }

        private void clearMessage()
        {
            lblMensaje.Text = "";
            lblTitulo.Text = "";
        }
        private void hideAllButtons()
        {
            lnkbAceptar.Visible = false;
            lnkbCancelar.Visible = false;
            lnkbSi.Visible = false;
            lnkbNo.Visible = false;
        }

        protected void lnkbAceptar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('body').removeClass('modal-open');$('.modal-backdrop').hide();", true);
            devolucion?.Invoke(this, new MensajeArgs
            {
                Command = this.Command,
                Args = this.Args.Split(new string[] { "|*" }, StringSplitOptions.RemoveEmptyEntries),
                Opcion = Opcion.Aceptar
            });
            if (Command == "LímiteSSTyDQO")
            {
                Response.Redirect("~/EstatusFirmado.aspx", true);
            }
            if (Command == "FirmaMasiva")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('#ModalFirma').modal();", true);
                
            }
            if (lblMensaje.Text!=string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "invocarfuncion", "$('#ModalMensaje').modal();", true);
            }
            
            //mpeMensaje.Hide();

        }
        protected void lnkbNo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('body').removeClass('modal-open');$('.modal-backdrop').hide();", true);
            if (this.Command.Contains("|"))
            {
                var arregloFirma = this.Command.Split('|');
                this.Command = arregloFirma[0];
            }
            devolucion?.Invoke(this, new MensajeArgs
            {
                Command = this.Command,
                Args = this.Args.Split(new string[] { "|*" }, StringSplitOptions.RemoveEmptyEntries),
                Opcion = Opcion.No
            });
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            cerrar?.Invoke(this, null);

            //mpeMensaje.Hide();

        }

        public void Ocultar()
        {
            //mpeMensaje.Hide();
        }

        public void Mostrar()
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "$('.alert').toggle()", true);
            //mpeMensaje.Show();
        }

    }

    #region Complemento de Mensajes
    public class MensajeArgs : EventArgs
    {
        public MensajeArgs() { }
        public string[] Args { get; set; }
        public Opcion Opcion { get; set; }
        public string Command { get; set; }
    }
    public enum Opciones
    {
        SoloAceptar = 0,
        AceptarCancelar,
        SiNo,
        SoloMensaje,
        Si
    }
    public enum Opcion
    {
        Aceptar = 0,
        Cancelar,
        Si,
        No,
        Cerrar
    }
    public enum Icono
    {
        ok = 0,
        information,
        error,
        warning,
        question
    }
    #endregion
}