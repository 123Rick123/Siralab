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
    public partial class DesgloseHistoricoImpugnacionApercibimiento : System.Web.UI.Page
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

            var lista = ApercibidosReincientesBLL.ObtenerListaImpugnacionByIdImpugnacion(int.Parse(idApercibimiento));
            DataTable dt = new DataTable();


            dt.Columns.Add("IdImpugnacion", typeof(int));
            dt.Columns.Add("MediosDeImpugnacion", typeof(string));
            dt.Columns.Add("Estatus", typeof(string));
            dt.Columns.Add("AnteQuien", typeof(string));
            dt.Columns.Add("Observaciones", typeof(string));
            dt.Columns.Add("FechaImpugnacion", typeof(string));
            dt.Columns.Add("FechaResolucion", typeof(string));
            dt.Columns.Add("Expediente", typeof(string));

            foreach (var item in lista)
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
            gvImpugnacionApercibimiento.DataSource = dt;
            gvImpugnacionApercibimiento.DataBind();
        }

        protected void lnkDEtalleImpugnacion_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            GridView gv = clickedRow.NamingContainer as GridView;
            var lbCode = gv.DataKeys[clickedRow.RowIndex].Values[0].ToString();
            llenarCampos(int.Parse(lbCode));
            PanelFormulario.Visible = true;
        }
        public void llenarCampos(int Id)
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


            txtidApercibimiento.Value = idApercibimiento;




            var listaImpugnacion = ApercibidosReincientesBLL.ObtenerImpugnacionesApercibimientoHistoricoById(Id);
            if (listaImpugnacion.Count > 0)
            {
                var ImpugnacionApercibimiento = listaImpugnacion.SingleOrDefault();
                txtAnteQuien.Enabled = false;
                ddlMedioImpugnacion.Enabled = false;
                txtFechaImpugnacion.Enabled = false;
                txtNoExpediente.Enabled = false;
                txtObservaciones.Disabled = true;
                ddlEstatus.Enabled = false;

                txtAnteQuien.Text = ImpugnacionApercibimiento.AnteQuien;
                txtFechaImpugnacion.Text = ImpugnacionApercibimiento.FechaImpugnacion.Value.ToShortDateString();
                txtNoExpediente.Text = ImpugnacionApercibimiento.Expediente;
                txtObservaciones.Value = ImpugnacionApercibimiento.Observaciones;
                ddlMedioImpugnacion.SelectedValue = ImpugnacionApercibimiento.IdMediosDeImpugnacion.ToString();
                ddlEstatus.SelectedValue = ImpugnacionApercibimiento.Estatus.ToString();



            }
        }

        protected void btnCerrarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ApercibidosYReincidentes.aspx");
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {

            if (banderaRegresar == "1")
            {
                Response.Redirect("~/ImpugnacionApercibimientoHistorico.aspx?idApercibimiento=" + idApercibimiento + "&IdReincidencia=" + IdReincidencia + "&banderaImpugnacion=" + banderaImpugnacion + "&banderaRegresar=" + banderaRegresar);
            }
            if (banderaRegresar == "2")
            {
                Response.Redirect("~/ImpugnacionReincidenciaHistorico.aspx?idApercibimiento=" + idApercibimiento + "&IdReincidencia=" + IdReincidencia + "&banderaImpugnacion=" + banderaImpugnacion + "&banderaRegresar=" + banderaRegresar);
            }
        }
    }
}