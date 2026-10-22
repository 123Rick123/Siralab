using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucFiltroCuerpoReceptor : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<FiltroCuerpoReceptorArg> Buscar;

        [Browsable(true)]
        public event EventHandler<FiltroCuerpoReceptorArg> Limpiar;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SIRALAB_BL.common.LoadDrops.instance.TipoCuerpoReceptor(ddlTipoCuerpoReceptor);
            }
        }

        protected void lnkbBuscar_Click(object sender, EventArgs e)
        {
            if (Buscar != null)
            {
                Buscar(this, CargarFiltro());
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ocultarAvanzado", "ocultarAvanzado();", true);

            }
        }

        protected void lnkbLimpiar_Click(object sender, EventArgs e)
        {

            txtBuscar.Text = string.Empty;
            ddlTipoCuerpoReceptor.SelectedIndex = 0;
            if (Limpiar != null)
            {
                Limpiar(this, CargarFiltro());
            }
            upBusqueda.Update();
        }

        private FiltroCuerpoReceptorArg CargarFiltro()
        {

            int tipoCuerpoReceptorID = 0;
            int.TryParse(ddlTipoCuerpoReceptor.SelectedValue.ToString(), out tipoCuerpoReceptorID);

            FiltroCuerpoReceptorArg filtro = new FiltroCuerpoReceptorArg
            {
                Nombre = txtBuscar.Text,
                TipoCuerpoReceptorID = tipoCuerpoReceptorID
            };
            return filtro;
        }
    }

    public class FiltroCuerpoReceptorArg : SIRALAB_BL.common.CuerpoReceptorArgBase_BL
    {

    }
}