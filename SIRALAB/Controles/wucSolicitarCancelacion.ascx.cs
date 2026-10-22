using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.common;

namespace SIRALAB.Controles
{
    public partial class wucSolictarCancelacion : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<SolicitudCancelacionArg> Guardar;
        [Browsable(true)]
        public event EventHandler Cerrar;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Actualizar()
        {
            upCancelarSolicitud.Update();
        }
        public void ActualizarMostrar(List<SolicitudCancelacionAnalisisItem> list)
        {
            rptDetalle.DataSource = list;
            rptDetalle.DataBind();

            mpeCancelarSolicitud.Show();
            Actualizar();
        }
        protected void Limpiar()
        {
            txtMotivo.Text = "";
        }
        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            if (Guardar != null)
            {
                var arg = new SolicitudCancelacionArg();
                arg.Motivo = txtMotivo.Text;
                Guardar(this, arg);
            }
            else
            {
                Actualizar();
            }
            Limpiar();
        }
        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {
            if (Cerrar != null)
            {
                Cerrar(this, new EventArgs());
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