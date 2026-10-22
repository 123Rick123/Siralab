using SIRALAB_BL.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucFiltroSolicitudCancelacion : System.Web.UI.UserControl
    {
        public event EventHandler<FiltroSolicitudCancelacion> Buscar;
        public static int? LaboratorioID
        {
            get
            {
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];
                return usuario != null ? usuario.LaboratorioID : null;
            }
        }
        public static string Laboratorio
        {
            get
            {
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];
                return usuario != null ? usuario.Laboratorio.Nombre : null;
            }
        }
        private FiltroSolicitudCancelacion Filtro
        {
            set { Session["FiltroSolicitudBusqueda"] = value; }
            get { return (FiltroSolicitudCancelacion)Session["FiltroSolicitudBusqueda"]; }
        }
        public static enumUsuarioRol UsuarioRol
        {
            get
            {
                var result = enumUsuarioRol.Recaudacion;
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];

                return usuario != null ? usuario.UsuarioRolID.ParseToEnumUsuarioRol() : result;
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

        public void Initialize(wucFiltroSolicitudCancelacion filtro = null)
        {
            /* Cargamos los estatus, las normas y los laboratorios disponibles */
            ddlLaboratorio.Enabled = UsuarioRol == enumUsuarioRol.Recaudacion;
            if (UsuarioRol == enumUsuarioRol.Recaudacion)
                LoadDrops.instance.ListaLaboratorio(ddlLaboratorio);
            else
                ddlLaboratorio.Items.Add(new ListItem { Text = Laboratorio, Value = LaboratorioID.ToString() });
            LoadDrops.instance.Estatus(ddlEstatus, enumEstatusTipo.SolicitudCancelacion, UsuarioRol);

            if (Filtro == null)
            {
                Filtro = new FiltroSolicitudCancelacion();
            }

            {
                txtFolio.Text = Filtro.Folio.ToString();
                txtFechaFinalEmision.Text = Filtro.FechaFinalEmision?.ToString("dd/MM/yyyy");
                txtFechaInicialEmision.Text = Filtro.FechaInicialEmision?.ToString("dd/MM/yyyy");

                var itemEstatus = ddlEstatus.Items.FindByValue(Filtro.Estatus?.ParseToInt().ToString());
                if (itemEstatus != null) {
                    ddlEstatus.ClearSelection();
                    itemEstatus.Selected = true;
                }

                var itemLaboratorio = ddlLaboratorio.Items.FindByValue(Filtro.LaboratorioID.ToString());
                if (itemLaboratorio != null)
                {
                    ddlLaboratorio.ClearSelection();
                    itemLaboratorio.Selected = true;
                }
            }
        }

        protected void lnkbBuscar_Click(object sender, EventArgs e)
        {
            /* Aquí se aplica la lógica de buscar, deberá enviar el filtro y ejecutar el evento buscar */
            Buscar?.Invoke(this, ObtenerFiltro());
        }

        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            txtFolio.Text = "";
            txtFechaInicialEmision.Text = (new DateTime(DateTime.Now.Year, 1, 1)).ToString("dd/MM/yyyy");
            txtFechaFinalEmision.Text = DateTime.Now.ToString("dd/MM/yyyy");
            if (UsuarioRol == enumUsuarioRol.Recaudacion)
                ddlLaboratorio.ClearSelection();
            ddlEstatus.ClearSelection();

            upBusqueda.Update();
            Buscar?.Invoke(this, ObtenerFiltro());
        }

        public FiltroSolicitudCancelacion ObtenerFiltro()
        {
            FiltroSolicitudCancelacion filtro = Filtro ?? new FiltroSolicitudCancelacion();
            enumEstatusCancelacion estatus;
                filtro.LaboratorioID = ddlLaboratorio.SelectedValue == "0" ? null : (int?)int.Parse(ddlLaboratorio.SelectedValue);
            if (UsuarioRol == enumUsuarioRol.Recaudacion)
            {
                filtro.IncluirCapturados = false;
            }
            if (UsuarioRol == enumUsuarioRol.Laboratorio)
            {
                filtro.IncluirCapturados = true;
            }

            filtro.Folio = txtFolio.Text == "" ? null : (int?)int.Parse(txtFolio.Text);
            if(int.Parse(ddlEstatus.SelectedValue).TryParseEnum<enumEstatusCancelacion>(out estatus))
            {
                filtro.Estatus = estatus;
            }
            filtro.FechaInicialEmision = txtFechaInicialEmision.Text == "" ? null : (DateTime?)DateTime.Parse(txtFechaInicialEmision.Text);
            filtro.FechaFinalEmision = txtFechaFinalEmision.Text == "" ? null : (DateTime?)DateTime.Parse(txtFechaFinalEmision.Text);

            this.Filtro = filtro;
            return filtro;
        }
    }
    public class FiltroSolicitudCancelacion : FiltroSolicitudCancelacionArgBase
    {
        public FiltroSolicitudCancelacion()
        {
            FechaInicialEmision = (new DateTime(DateTime.Now.Year, 1, 1));
            FechaFinalEmision = DateTime.Now;
        }
    }
}