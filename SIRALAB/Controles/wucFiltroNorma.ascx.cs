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
    public partial class wucFiltroNorma : System.Web.UI.UserControl
    {

        [Browsable(true)]
        public event EventHandler<FiltroNormaArg> Buscar;

        [Browsable(true)]
        public event EventHandler<FiltroNormaArg> Limpiar;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void lnkbBuscar_Click(object sender, EventArgs e)
        {
            if (Buscar != null)
            {
                Buscar(this, CargarFiltro());
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ocultarFiltroNormaAvanzado", "ocultarFiltroNormaAvanzado();", true);
            }
        }

        protected void lnkbLimpiar_Click(object sender, EventArgs e)
        {

            txtBuscar.Text = string.Empty;
            txtNormaDescripcion.Text = string.Empty;
            txtNormaFechaInicio.Text = string.Empty;

            if (Limpiar != null)
            {
                Limpiar(this, CargarFiltro());
            }
            upBusqueda.Update();
        }


        private FiltroNormaArg CargarFiltro()
        {
            FiltroNormaArg filtro = new FiltroNormaArg();
            filtro.Nombre = txtBuscar.Text.Trim();
            filtro.Descripcion = txtNormaDescripcion.Text.Trim();
            DateTime fechaInicioNorma;
            if (DateTime.TryParse(txtNormaFechaInicio.Text, out fechaInicioNorma))
                filtro.FechaInicio = fechaInicioNorma;
            else
                filtro.FechaInicio = new DateTime();

            return filtro;
        }
    }
    public class FiltroNormaArg : SIRALAB_BL.common.NormaArgBase_BL
    {
       
    }


}