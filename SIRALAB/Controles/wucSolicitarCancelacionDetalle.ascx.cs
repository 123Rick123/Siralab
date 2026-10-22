using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.controles;
using SIRALAB_BL.common;

namespace SIRALAB.Controles
{
    public partial class wucSolicitarCancelacionDetalle : System.Web.UI.UserControl
    {
        public event EventHandler<DuplicarAnalisisItem> Duplicar;
        public static enumUsuarioRol UsuarioRol
        {
            get
            {
                var result = enumUsuarioRol.Recaudacion;
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];

                return usuario != null ? usuario.UsuarioRolID.ParseToEnumUsuarioRol() : result;
            }
        }
        public SolicitudCancelacionItem Solicitud
        {
            get { return (SolicitudCancelacionItem) ViewState["Solicitud"]; }
            set { ViewState["Solicitud"] = value; }
        }
        private List<SolicitudCancelacionDetalleItem> Listado
        {
            get { return (List<SolicitudCancelacionDetalleItem>)ViewState["SolicitudListado"]; }
            set { ViewState["SolicitudListado"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)Page.Master.Master;
            main.MensajeDevolucion += Main_MensajeDevolucion;
        }
        private void Main_MensajeDevolucion(object sender, MensajeArgs e)
        {
            if (e.Opcion == Opcion.Aceptar)
            {
                switch (e.Command)
                {
                    case "Duplicar":
                        var arg = int.Parse(e.Args[0]);
                        Duplicar?.Invoke(this, new DuplicarAnalisisItem { Solicitud = Solicitud, Listado = Listado, AnalisisID = arg});
                        
                        break;
                    default:
                        break;
                }
            }
        }
        public void Actualizar()
        {
            upCancelarSolicitud.Update();
        }
        public void ActualizarMostrar(SolicitudCancelacionItem solicitud, List<SolicitudCancelacionDetalleItem> list)
        {
            if (solicitud != null)
            {
                this.Solicitud = solicitud;
                this.Listado = list;

                lblFolio.Text = solicitud.Folio.ToString();
                lblLaboratorio.Text = solicitud.Laboratorio;
                lblFechaSolicitud.Text = solicitud.FechaRegistro.ToString("dd/MM/yyyy");
                lblFechaEnvio.Text = solicitud.FechaRegistro.ToString("dd/MM/yyyy");
                lblMotivo.Text = solicitud.Motivo;

                rptDetalle.DataSource = list;
                rptDetalle.DataBind();

                mpeCancelarSolicitud.Show();
                Actualizar();
            }
        }

        public void Hide()
        {
            mpeCancelarSolicitud.Hide();
            Actualizar();
        }
        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {

        }
        protected void lnkbAceptar_Click(object sender, EventArgs e)
        {

        }
        protected void lnkEditarAnalisis_OnClick(object sender, EventArgs e)
        {
            
            string arg = ((LinkButton)sender).Attributes["data-analisis-id"];
            this.Page.PopMensaje("Se realizará la copia del análisis y quedará editable para la corrección del mismo. <br /><br />¿Desea continuar?", "Solicitud de Cancelación", Opciones.SiNo, Icono.question, "Duplicar", new [] { arg });
            mpeCancelarSolicitud.Hide();
        }
    }
    public class DuplicarAnalisisItem : EventArgs
    {
        public SolicitudCancelacionItem Solicitud { get; set; }
        public List<SolicitudCancelacionDetalleItem> Listado { get; set; }
        public int AnalisisID { get; set; }
    }
}