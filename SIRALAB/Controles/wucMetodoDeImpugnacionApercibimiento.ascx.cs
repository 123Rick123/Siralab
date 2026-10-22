using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using SIRALAB_BL.negocio.LaboratoriosApercibidos.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucMetodoDeImpugnacionApercibimiento : System.Web.UI.UserControl
    {
        LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();
        public string IdApercibimiento;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdApercibimiento = Request.QueryString["idApercibimiento"];
                llenarCampos();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "calendario", "Calendario()", true);
            }
           
        }

        public void llenarCampos()
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

            
                txtidApercibimiento.Value = IdApercibimiento;
            
           

            
            var listaImpugnacion = ApercibidosReincientesBLL.ObtenerImpugnacionesByIdApercibimiento(int.Parse(txtidApercibimiento.Value));
            if (listaImpugnacion.Count>0)
            {
                var ImpugnacionApercibimiento = listaImpugnacion.Where(x => x.Activo == true).SingleOrDefault();
                txtAnteQuien.Text = ImpugnacionApercibimiento.AnteQuien;
                txtFechaImpugnacion.Text = ImpugnacionApercibimiento.FechaImpugnacion.Value.ToShortDateString();
                txtNoExpediente.Text = ImpugnacionApercibimiento.Expediente;
                txtObservaciones.Value = ImpugnacionApercibimiento.Observaciones;
                ddlMedioImpugnacion.SelectedValue = ImpugnacionApercibimiento.IdMediosDeImpugnacion.ToString();
                ddlEstatus.SelectedValue = ImpugnacionApercibimiento.Estatus.ToString();
                ddlMedioImpugnacion.Enabled = false;
             

            }
           





        }

        protected void lnkbtnValidarCampos_Click(object sender, EventArgs e)
        {
            var errores = string.Empty;
            if (txtidApercibimiento.Value!=string.Empty)
            {
                var existeConApercibimiento = ApercibidosReincientesBLL.ExisteImpugnacionByIdApercibimiento(int.Parse(txtidApercibimiento.Value));
                if (existeConApercibimiento)
                {
                    txtBanderaEsConsulta.Value = "true";
                    //errores += "ErrorYaExiste";
                    //var texto = TextoAlertaSimple;
                    //texto.InnerHtml = "Ya existe una impugnacion para este apercibimiento";
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
                }
                else
                {
                    txtBanderaEsConsulta.Value = "false";
                }
            }
            if (txtidReincidencia.Value != string.Empty)
            {
                var existeConReincidencia = ApercibidosReincientesBLL.ExisteImpugnacionByIdReincidencia(int.Parse(txtidReincidencia.Value));
                if (existeConReincidencia)
                {
                   txtBanderaEsConsulta.Value=  "true";
                    //errores += "ErrorYaExiste";
                    //var texto = TextoAlertaSimple;
                    //texto.InnerHtml = "Ya existe una impugnacion para esta reincidencia";
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
                }
                else
                {
                    txtBanderaEsConsulta.Value = "false";
                }
            }

            if (ddlMedioImpugnacion.SelectedValue != "0")
            {
                errores += string.Empty;
                MensajeErrorMedioImpugnacion.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorMedioImpugnacion";
                MensajeErrorMedioImpugnacion.InnerText = "Este campo es obligatorio.";
            }
            if (ddlEstatus.SelectedValue != "0")
            {
                errores += string.Empty;
                MensajeErrorEstatus.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorEstatus";
                MensajeErrorEstatus.InnerText = "Este campo es obligatorio.";
            }
            
            if (txtFechaImpugnacion.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorFechaImpugnacion.InnerText = string.Empty;
                if (RExprFechaImpugnacion.IsValid == false)
                {
                    errores += "ErrorFechaImpugnacion";

                }
            }
            else
            {
                errores += "ErrorFechaImpugnacion";
                MensajeErrorFechaImpugnacion.InnerText = "Este campo es obligatorio.";
            }

            if (int.Parse(ddlEstatus.SelectedValue) != (int)EnumEstatusImpugnacion.EnProceso && ddlEstatus.SelectedValue != "0")
            {
                if (txtFechaResolucion.Text != string.Empty)
                {
                    errores += string.Empty;
                    MensajeErrorFechaResolucion.InnerText = string.Empty;
                    if (RExprFechaResolucion.IsValid == false)
                    {
                        errores += "ErrorFechaResolucion";

                    }
                }
                else
                {
                    errores += "ErrorFechaResolucion";
                    MensajeErrorFechaResolucion.InnerText = "Este campo es obligatorio.";
                }
            }
           
            if (txtAnteQuien.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorAnteQuien.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorAnteQuien";
                MensajeErrorAnteQuien.InnerText = "Este campo es obligatorio.";
            }
             if (txtObservaciones.Value != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorObservaciones.InnerText = string.Empty;
            }
            else
            {
                errores += "Errorobservaciones";
                MensajeErrorObservaciones.InnerText = "Este campo es obligatorio.";
            }
            if (txtNoExpediente.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorNoExpediente.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorNoExpediente";
                MensajeErrorNoExpediente.InnerText = "Este campo es obligatorio.";
            }
            if (errores.Length == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "MostrarModalConfirm", "MostrarModalConfirm()", true);
            }

        }
        protected void btnCerrarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ApercibidosYReincidentes.aspx");

        }

        protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idEstatus = int.Parse(ddlEstatus.SelectedValue);
            if (idEstatus==(int)EnumEstatusImpugnacion.EnProceso || idEstatus==0)
            {
                lblFechaResolucion.Visible = false;
                txtFechaResolucion.Visible = false;
                spanCalendarioFechaRes.Visible = false;
                txtFechaResolucion.Text = string.Empty;
                MensajeErrorFechaResolucion.InnerText = string.Empty;
            }
            else
            {
                lblFechaResolucion.Visible = true;
                txtFechaResolucion.Visible = true;
                spanCalendarioFechaRes.Visible = true;
            }
        }


        protected void ddlMedioImpugnacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idMedio = int.Parse(ddlMedioImpugnacion.SelectedValue);
            if (idMedio == (int)EnumMedioImpugnacion.Otro)
            {
                var estatus = (int)EnumEstatusImpugnacion.AFavorDeConagua;
                ddlEstatus.SelectedValue = estatus.ToString();
                lblFechaResolucion.Visible = true;
                txtFechaResolucion.Visible = true;
                spanCalendarioFechaRes.Visible = true;
                ddlEstatus.Enabled = false;
            }
            else
            {
                lblFechaResolucion.Visible = false;
                txtFechaResolucion.Visible = false;
                spanCalendarioFechaRes.Visible = false;
                txtFechaResolucion.Text = string.Empty;
                MensajeErrorFechaResolucion.InnerText = string.Empty;
                ddlEstatus.SelectedValue = "0";
                ddlEstatus.Enabled = true;
            }
        }


    }
}