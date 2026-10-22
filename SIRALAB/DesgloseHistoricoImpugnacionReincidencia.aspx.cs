using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class DesgloseHistoricoImpugnacionReincidencia : System.Web.UI.Page
    {
        LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();
        public string idApercibimiento;
        public string IdReincidencia;
        public string banderaImpugnacion;
        public string banderaRegresar;
        protected void Page_Load(object sender, EventArgs e)
        {
            idApercibimiento = Request.QueryString["idApercibimiento"];
            IdReincidencia = Request.QueryString["IdReincidencia"];
            banderaImpugnacion = Request.QueryString["banderaImpugnacion"];
            banderaRegresar = Request.QueryString["banderaRegresar"];

            //var lista = ReincientesBLL.ObtenerListaImpugnacionReincidencia(int.Parse(IdReincidencia));
            var lista2 = ApercibidosReincientesBLL.ObtenerListaImpugnacionReincidencia2(int.Parse(IdReincidencia));
            DataTable dt = new DataTable();


            dt.Columns.Add("IdImpugnacion", typeof(int));
            dt.Columns.Add("MediosDeImpugnacion", typeof(string));
            dt.Columns.Add("Estatus", typeof(string));
            dt.Columns.Add("AnteQuien", typeof(string));
            dt.Columns.Add("Observaciones", typeof(string));
            dt.Columns.Add("FechaImpugnacion", typeof(string));
            dt.Columns.Add("FechaResolucion", typeof(string));
            dt.Columns.Add("Expediente", typeof(string));

            foreach (var item in lista2)
            {
                DataRow rw = dt.NewRow();
                rw["IdImpugnacion"] = item.IdImpugnacion;
                rw["MediosDeImpugnacion"] = item.MediosImpungnacion;
                rw["Estatus"] = item.Estatus;
                rw["AnteQuien"] = item.AnteQuien;
                rw["Observaciones"] = item.Observaciones;
                if (item.FechaImpugnacion != null && item.FechaImpugnacion != string.Empty)
                {
                    var fImpugnacion = Convert.ToDateTime(item.FechaImpugnacion).ToShortDateString();
                    rw["FechaImpugnacion"] = fImpugnacion;
                }
                else
                {
                    rw["FechaImpugnacion"] = item.FechaImpugnacion;
                }


                if (item.FechaResolucion == null || item.FechaResolucion == string.Empty)
                {
                    rw["FechaResolucion"] = string.Empty;
                }
                else
                {
                    var fResolucion = Convert.ToDateTime(item.FechaResolucion).ToShortDateString();
                    rw["FechaResolucion"] = fResolucion;
                }
                rw["Expediente"] = item.Expediente;
                dt.Rows.Add(rw);
            }
            gvImpugnacionReincidencia.DataSource = dt;
            gvImpugnacionReincidencia.DataBind();
        }
        

        protected void lnkDetalleImpugnacion_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            GridView gv = clickedRow.NamingContainer as GridView;
            var lbCode = gv.DataKeys[clickedRow.RowIndex].Values[0].ToString();
            llenarCampos(int.Parse(lbCode));
            PanelFormulario.Visible = true;
        }
        public void llenarCampos(int id)
        {

            //llenar el ddlMedioImpugnacion
            ListItem Medio;
            Medio = new ListItem("Seleccione", "0");
            ddlMedioImpugnacion.Items.Add(Medio);

            var medios = ApercibidosReincientesBLL.ObtenerMediosDeImpugnacion();

            foreach (var item in medios)
            {
                var medioImpugnacion = new ListItem(item.Descripcion, item.id.ToString());
                ddlMedioImpugnacion.Items.Add(medioImpugnacion);
            }
            //var ultimo = medios.Select(x => x.id).LastOrDefault();
            //var ultimoid = ultimo + 1;
            //Medio = new ListItem("Otro", ultimoid.ToString());
            //ddlMedioImpugnacion.Items.Add(Medio);

            //llenar el ddlEstatus
            var EstatusList = ApercibidosReincientesBLL.ObtenerEstatusImpugnacion();
            ListItem Estatuss;
            Estatuss = new ListItem("Seleccione", "0");
            ddlEstatus.Items.Add(Estatuss);
            foreach (var item in EstatusList)
            {
                var Estatus = new ListItem(item.Descripcion, item.Id.ToString());
                ddlEstatus.Items.Add(Estatus);
            }


            txtidReincidencia.Value = IdReincidencia;


            var listaImpugnacion = ApercibidosReincientesBLL.ObtenerImpugnacionesReincidenciaHistoricoById(id);
            if (listaImpugnacion.Count > 0)
            {
                var ImpugnacionReincidencia = listaImpugnacion.SingleOrDefault();
                txtAnteQuien.Enabled = false;
                ddlMedioImpugnacion.Enabled = false;
                txtFechaImpugnacion.Enabled = false;
                txtNoExpediente.Enabled = false;
                txtObservaciones.Disabled = true;
                ddlEstatus.Enabled = false;

                txtAnteQuien.Text = ImpugnacionReincidencia.AnteQuien;
                txtFechaImpugnacion.Text = ImpugnacionReincidencia.FechaImpugnacion.Value.ToShortDateString();
                txtNoExpediente.Text = ImpugnacionReincidencia.Expediente;
                txtObservaciones.Value = ImpugnacionReincidencia.Observaciones;
                ddlMedioImpugnacion.SelectedValue = ImpugnacionReincidencia.IdMediosDeImpugnacion.ToString();
                ddlEstatus.SelectedValue = ImpugnacionReincidencia.Estatus.ToString();


            }

        }

        protected void btnCerrarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ApercibidosYReincidentes.aspx");
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ImpugnacionReincidenciaHistorico.aspx?idApercibimiento=" + idApercibimiento + "&IdReincidencia=" + IdReincidencia + "&banderaImpugnacion=" + banderaImpugnacion + "&banderaRegresar=" + banderaRegresar);
        }
    }
}