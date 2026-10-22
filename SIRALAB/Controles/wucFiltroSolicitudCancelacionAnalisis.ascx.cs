using SIRALAB_BL.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucFiltroSolicitudCancelacionAnalisis : System.Web.UI.UserControl
    {
        public event EventHandler<FiltroSolicitudCancelacionAnalisis> Buscar;
        private int? LaboratorioID {
            get {
                return ViewState["LaboratorioID"] == null ? null : (int?)int.Parse(ViewState["LaboratorioID"].ToString());
            }
            set {
                ViewState["LaboratorioID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaInicialEmision.Text = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
                txtFechaFinalEmision.Text = DateTime.Now.ToShortDateString();

                endTxtFechaInicialEmision.Value = DateTime.Now.ToShortDateString();
                endTxtFechaFinalEmision.Value = DateTime.Now.ToShortDateString();
            }
        }

        public void Initialize(FiltroSolicitudCancelacionAnalisis filtro = null)
        {
            this.LaboratorioID = filtro?.LaboratorioID;
            /* Cargamos las normas */
            LoadDrops.instance.Norma(ddlNorma);
        }

        protected void lnkbBuscar_Click(object sender, EventArgs e)
        {
            /* Aquí se aplica la lógica de buscar, deberá enviar el filtro y ejecutar el evento buscar */
            Buscar?.Invoke(this, ObtenerFiltro());
        }

        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            txtNumeroInforme.Text = "";
            txtFechaInicialEmision.Text = (new DateTime(DateTime.Now.Year, 1, 1)).ToString("dd/MM/yyyy");
            txtFechaFinalEmision.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddlNorma.ClearSelection();
            txtRfc.Text = "";
            txtTitulo.Text = "";

            upBusqueda.Update();

            Buscar?.Invoke(this, ObtenerFiltro());
        }

        public FiltroSolicitudCancelacionAnalisis ObtenerFiltro()
        {
            FiltroSolicitudCancelacionAnalisis filtro = new FiltroSolicitudCancelacionAnalisis();

            filtro.LaboratorioID = LaboratorioID;
            filtro.NormaID = ddlNorma.SelectedValue == "0" ? null : (int?)int.Parse(ddlNorma.SelectedValue);
            filtro.Numero = txtNumeroInforme.Text == "" ? null : txtNumeroInforme.Text;
            filtro.FechaInicialEmision = txtFechaInicialEmision.Text == "" ? null : (DateTime?)DateTime.Parse(txtFechaInicialEmision.Text);
            filtro.FechaFinalEmision = txtFechaFinalEmision.Text == "" ? null : (DateTime?)DateTime.Parse(txtFechaFinalEmision.Text);
            filtro.Rfc = txtRfc.Text == "" ? null : txtRfc.Text;
            filtro.Titulo = txtTitulo.Text == "" ? null : txtTitulo.Text;

            return filtro;
        }
    }
    public class FiltroSolicitudCancelacionAnalisis : FiltroSolicitudCancelacionAnalisisArgBase
    {

    }
}