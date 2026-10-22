using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB.Controles.AnexoE
{
    public partial class wucAgregarParametroAnalisis : System.Web.UI.UserControl
    {
        public event EventHandler<AgregarParametroArgs> Guardar;
        public event EventHandler ErrorCaptura;
        public int AnalisisID
        {
            set { Session["AnalisisID_AgregarParametro"] = value; }
            get { return Session["AnalisisID_AgregarParametro"] != null ? (int)Session["AnalisisID_AgregarParametro"] : 0; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDrops();
            }
        }

        public void Show(int analisisId)
        {
            this.AnalisisID = analisisId;
            //Acá debemos extraer todos los parámetros que existen y quitar los que ya se encuentran incluidos en las muestras simples.
            lnkBuscar_OnClick(lnkBuscar, null);

            mpeAgregarPrametro.Show();
            upAgregarPrametro.Update();
        }

        private void CargarDrops()
        {
            Laboratorio_BL labBl = new Laboratorio_BL();
            Metodo_BL metBl = new Metodo_BL();

            ddlLaboratorio.LoadDrop("Nombre", "LaboratorioID", labBl.ObtenerLaboratorios());
            ddlLaboratorio.ClearSelection();

            ddlMetodo.LoadDrop("Nombre", "MetodoID", metBl.Obten_Metodos());
            ddlMetodo.ClearSelection();
        }

        private void Buscar(int analisisId, int? laboratorioId = null, int? metodoId = null, string parametro = null)
        {
            Parametro_BL parametroBl = new Parametro_BL();

            if (analisisId == 0) analisisId = this.AnalisisID;
            var parametros = parametroBl.ObtenerTodos(analisisId, laboratorioId, metodoId, parametro);

            rptParametros.DataSource = parametros;
            rptParametros.DataBind();
        }
        private List<MuestraValor> ObtenerParametrosSeleccionados()
        {
            List<MuestraValor> lista = new List<MuestraValor>();
            foreach (RepeaterItem item in rptParametros.Items)
            {
                var check = (CheckBox) item.FindControl("chkParametro");
                var text = (TextBox)item.FindControl("txtEscalaDecimal");
                if (check != null && text != null)
                {
                    var parametroId = int.Parse(check.Attributes["data-parametro-id"]);
                    if (check.Checked && text.Text.Trim() != string.Empty)
                    {
                        lista.Add(new MuestraValor
                        {
                            ParametroID = parametroId,
                            Exenta = false,
                            Evalua = false,
                            AceptaValor = true,
                            LFD = false,
                            EscalaDecimal = int.Parse(text.Text),
                            LCMC = false
                        });
                    }
                }
            }

            if (lista.Count == 0) {
                ErrorCaptura?.Invoke(this, EventArgs.Empty);
                return null;
            }

            return lista;
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            Guardar?.Invoke(this, new AgregarParametroArgs()
            {
                AnalisisID = this.AnalisisID,
                MuestraValores = ObtenerParametrosSeleccionados()
            });
        }

        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void rptParametros_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var data = (Parametro)e.Item.DataItem;

            }
        }

        protected void lnkBuscar_OnClick(object sender, EventArgs e)
        {
            var laboratorioId = ddlLaboratorio.SelectedValue != "0" ? (int?)int.Parse(ddlLaboratorio.SelectedValue) : null;
            var metodoId = ddlMetodo.SelectedValue != "0" ? (int?)int.Parse(ddlMetodo.SelectedValue) : null;
            var parametro = txtParametro.Text.Trim() != String.Empty ? txtParametro.Text.Trim() : null;

            Buscar(this.AnalisisID, laboratorioId, metodoId, parametro);

            mpeAgregarPrametro.Show();
            upAgregarPrametro.Update();
        }
    }

    public class AgregarParametroArgs : EventArgs
    {
        public int AnalisisID { get; set; }
        public List<MuestraValor> MuestraValores { get; set; }
    }
}