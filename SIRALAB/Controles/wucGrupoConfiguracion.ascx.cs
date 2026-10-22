using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB_DAL;
using SIRALAB_BL.negocio;
using System.ComponentModel;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucGrupoConfiguracion : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<GrupoConfiguracionArgs> Guardar;
        [Browsable(true)]
        public event EventHandler Cerrar;

        #region Propiedades
        private int NormaID
        {
            set { ViewState["NormaIDGrupo"] = value; }
            get { return ViewState["NormaIDGrupo"] != null ? (int)ViewState["NormaIDGrupo"] : 0; }
        }
        private int? NormaConfiguracionID
        {
            set { ViewState["NormaConfiguracionIDGrupo"] = value; }
            get { return (int?)ViewState["NormaConfiguracionIDGrupo"]; }
        }
        private int CuerpoReceptorID
        {
            set { ViewState["CuerpoReceptorIDGrupo"] = value; }
            get { return ViewState["CuerpoReceptorIDGrupo"] != null ? (int)ViewState["CuerpoReceptorIDGrupo"] : 0; }
        }
        private int UsoAguaID
        {
            set { ViewState["UsoAguaIDGrupo"] = value; }
            get { return ViewState["UsoAguaIDGrupo"] != null ? (int)ViewState["UsoAguaIDGrupo"] : 0; }
        }
        private int NivelID
        {
            set { ViewState["NivelIDGrupo"] = value; }
            get { return ViewState["NivelIDGrupo"] != null ? (int)ViewState["NivelIDGrupo"] : 0; }
        }
        private bool AllowExent
        {
            get { return ViewState["AllowExentGrupo"] != null ? (bool)ViewState["AllowExentGrupo"] : false; }
            set { ViewState["AllowExentGrupo"] = chkExenta.Visible = value; }
        }
        private int Orden
        {
            set { ViewState["OrdenGrupo"] = value; }
            get { return ViewState["OrdenGrupo"] != null ? (int)ViewState["OrdenGrupo"] : 0; }
        }
        public bool Lista
        {
            set { ViewState["ListaGrupo"] = value; }
            get { return ViewState["ListaGrupo"] != null ? (bool)ViewState["ListaGrupo"] : false; }
        }
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDrops();
            }
        }

        #region Métodos
        private void CargarDrops()
        {
            LoadDrops.instance.OperacionConsolidado(ddlOperacion);
            LoadDrops.instance.ParametroConMetodos(ddlParametroGrupo);
        }
        private void Limpiar()
        {
            ddlParametroGrupo.ClearSelection();
            chkActivo.Checked = true;
            ddlOperacion.ClearSelection();
            rbRango.Checked = true;
            rbValor.Checked = false;
            txtLimiteInferior.Text = "";
            txtLimiteSuperior.Text = "";
            txtLFDInferior.Text = "";
            txtLFDSuperior.Text = "";
            chkNoEvalua.Checked = false;
            chkLimitesLFD.Checked = false;
            chkActivo.Checked = true;
            this.Orden = 0;

            rbRango_CheckedChanged(null, null);
            this.NormaConfiguracionID = null;
            chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);
        }
        public void Mostrar(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            Norma_BL nBL = new Norma_BL();
            var norma = nBL.Obtener(NormaID);

            this.NormaID = NormaID;
            this.CuerpoReceptorID = CuerpoReceptorID;
            this.UsoAguaID = UsoAguaID;
            this.NivelID = NivelID;
            this.AllowExent = norma.ExentaParametros;
            this.Lista = false;

            Limpiar();
            Cargar(NormaID, CuerpoReceptorID, UsoAguaID, NivelID);
            ActualizarMostrar();
        }
        public void Mostrar(int NormaConfiguracionID, bool listado)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var nc = ncBL.Obtener(NormaConfiguracionID);
            this.NormaID = nc.NormaID;
            this.CuerpoReceptorID = nc.CuerpoReceptorID;
            this.UsoAguaID = nc.UsoAguaID;
            this.NivelID = nc.NivelID;
            this.Lista = listado;
            this.AllowExent = nc.Norma.ExentaParametros;

            Limpiar();
            Cargar(NormaID, CuerpoReceptorID, UsoAguaID, NivelID);
            CargarGrupo(NormaConfiguracionID);

            ActualizarMostrar();
        }
        private void Cargar(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var Listado = ncBL.Obtener_Grupos(NormaID, CuerpoReceptorID, UsoAguaID, NivelID).Select(q => new GrupoConfiguracionItem
            {
                NormaID = q.NormaID,
                NormaConfiguracionID = q.NormaConfiguracionID,
                OperacionID = (int)q.OperacionID,
                Limite = q.Evalua ? q.AceptaValor ? (q.LimiteInferior == null ? q.LimiteSuperior.ToString() : string.Format("[{0} - {1}]", q.LimiteInferior, q.LimiteSuperior)) : "Presencia" : "No evalúa",
                LimiteLFD = q.LFD ? q.Evalua ? q.AceptaValor ? (q.LimiteInferiorLFD == null ? q.LimiteSuperiorLFD.ToString() : string.Format("[{0} - {1}]", q.LimiteInferiorLFD, q.LimiteSuperiorLFD)) : "Presencia" : "No evalúa" : "-",
                NombreGrupo = q.Parametro.Nombre,
                NombreOperacion = q.OperacionConsolidado.Nombre
            });

            rptGrupoConfiguracion.DataSource = Listado;
            rptGrupoConfiguracion.DataBind();
        }
        private void CargarGrupo(int NormaConfiguracionID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var grupo = ncBL.Obtener(NormaConfiguracionID);

            Limpiar();
            this.AllowExent = grupo.Norma.ExentaParametros;

            ddlParametroGrupo.ClearSelection();
            ddlParametroGrupo.Items.FindByValue(grupo.ParametroID.ToString()).Selected = true;
            ddlOperacion.ClearSelection();
            ddlOperacion.Items.FindByValue(grupo.OperacionID.ToString()).Selected = true;
            chkActivo.Checked = grupo.Activo;
            chkExenta.Checked = grupo.Exenta;
            this.Orden = grupo.Orden;

            chkNoEvalua.Checked = !grupo.Evalua;
            chkLimitesLFD.Checked = grupo.LFD;
            chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);

            if (grupo.LimiteInferior != null)
            {
                rbRango.Checked = true;
                rbRango_CheckedChanged(null, null);
                txtLimiteInferior.Text = grupo.LimiteInferior.ToString();
                txtLimiteSuperior.Text = grupo.LimiteSuperior.ToString();

                txtLFDInferior.Text = grupo.LimiteInferiorLFD.ToString();
                txtLFDSuperior.Text = grupo.LimiteSuperiorLFD.ToString();
            }
            else
            {
                rbValor.Checked = true;
                rbValor_CheckedChanged(null, null);
                txtLimiteSuperior.Text = grupo.LimiteSuperior.ToString();
                txtLFDSuperior.Text = grupo.LimiteSuperiorLFD.ToString();
            }

            this.NormaConfiguracionID = grupo.NormaConfiguracionID;
        }
        public void Actualizar()
        {
            upGrupoConfiguracion.Update();
        }
        public void ActualizarMostrar()
        {
            //string scrollPanelScript = "$('#modalGrupoConfiguracion').modal()";
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalGrupoConfiguracion').modal({ backdrop : 'static' });", true);

            upGrupoConfiguracion.Update();
        }
        private void SinLimites()
        {
            lblLimiteInferior.Visible = false;
            lblLimiteSuperior.Visible = false;
            txtLimiteInferior.Visible = false;
            txtLimiteSuperior.Visible = false;
            rfvLimiteInferior.Enabled = false;
            rfvLimiteSuperior.Enabled = false;
            lblLFDInferior.Visible = false;
            lblLFDSuperior.Visible = false;
            txtLFDInferior.Visible = false;
            txtLFDSuperior.Visible = false;
            rfvLFDInferior.Enabled = false;
            rfvLFDSuperior.Enabled = false;
        }
        private GrupoConfiguracionArgs ObtenConfiguracion()
        {
            GrupoConfiguracionArgs resultado = new GrupoConfiguracionArgs();

            resultado.NormaConfiguracionID = this.NormaConfiguracionID;
            resultado.NormaID = this.NormaID;
            resultado.ParametroID = int.Parse(ddlParametroGrupo.SelectedValue);
            resultado.CuerpoReceptorID = this.CuerpoReceptorID;
            resultado.UsoAguaID = this.UsoAguaID;
            resultado.OperacionID = int.Parse(ddlOperacion.SelectedValue);
            resultado.Orden = this.Orden;
            resultado.AceptaValor = true;
            resultado.LimiteInferior = (rbRango.Checked && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLimiteInferior.Text) : null;
            resultado.LimiteSuperior = ((rbValor.Checked || rbRango.Checked) && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLimiteSuperior.Text) : null;
            resultado.Activo = chkActivo.Checked;
            resultado.Exenta = this.AllowExent ? chkExenta.Checked : false;

            /* Por la leyenda de "No aplica evaluación" se invierte el valor; por default se debe grabar en true para que siempre evalue */
            resultado.Evalua = !chkNoEvalua.Checked;
            resultado.LFD = chkLimitesLFD.Checked;
            resultado.LimiteInferiorLFD = chkLimitesLFD.Checked ? (rbRango.Checked && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLFDInferior.Text) : null : null;
            resultado.LimiteSuperiorLFD = chkLimitesLFD.Checked ? ((rbValor.Checked || rbRango.Checked) && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLFDSuperior.Text) : null : null;
            resultado.FechaDesactivacion = chkActivo.Checked ? null : (DateTime?)DateTime.Now;
            resultado.NormaConfiguracionPadreID = null;

            if (chkNoEvalua.Checked)
            {
                /// Entonces no evalúa y no pide límites pero los guardamos en -1
                /// para saber sí es un rango o un valor
                resultado.LimiteInferior = rbRango.Checked ? (decimal?)-1 : null;
                resultado.LimiteSuperior = rbValor.Checked || rbRango.Checked ? (decimal?)-1 : null;
                resultado.LimiteInferiorLFD = chkLimitesLFD.Checked ? rbRango.Checked ? (decimal?)-1 : null : null;
                resultado.LimiteSuperiorLFD = chkLimitesLFD.Checked ? rbValor.Checked || rbRango.Checked ? (decimal?)-1 : null : null;
            }

            resultado.FechaDesactivacion = chkActivo.Checked ? null : (DateTime?)DateTime.Now;
            resultado.Grupo = true;

            return resultado;
        }
        #endregion

        #region Eventos de botones
        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            Actualizar();
            if (Cerrar != null)
                Cerrar(this, new EventArgs());
        }
        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {
            this.NormaConfiguracionID = null;
            Limpiar();

            ActualizarMostrar();
        }
        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            if (Guardar != null)
            {
                Guardar(this, ObtenConfiguracion());
            }

            Limpiar();
            Cargar(this.NormaID, this.CuerpoReceptorID, this.UsoAguaID, this.NivelID);
            //ActualizarMostrar();
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "HideModal", "$('#modalGrupoConfiguracion').modal('hide');", true);

        }
        protected void lnkbEditar_Click(object sender, EventArgs e)
        {
            int NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            CargarGrupo(NormaConfiguracionID);
        }
        protected void lnkbActivar_Click(object sender, EventArgs e)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            var Grupo = ncBL.Obtener(NormaConfiguracionID);

            if (Guardar != null)
            {
                Guardar(this, ObtenConfiguracion());
            }

            Cargar(this.NormaID, this.CuerpoReceptorID, this.UsoAguaID, this.NivelID);
            ActualizarMostrar();

        }
        protected void rbRango_CheckedChanged(object sender, EventArgs e)
        {
            lblLimiteInferior.Visible = true;
            lblLimiteSuperior.Visible = true;
            txtLimiteInferior.Visible = true;
            txtLimiteSuperior.Visible = true;
            rfvLimiteInferior.Enabled = true;
            rfvLimiteSuperior.Enabled = true;
            
            lblLFDInferior.Visible = true;
            lblLFDSuperior.Visible = true;
            txtLFDInferior.Visible = true;
            txtLFDSuperior.Visible = true;
            rfvLFDInferior.Enabled = true;
            rfvLFDSuperior.Enabled = true;

            txtLFDInferior.Text = txtLimiteInferior.Text;
            txtLFDSuperior.Text = txtLimiteSuperior.Text;

            if (chkNoEvalua.Checked)
            {
                SinLimites();
            }

            ActualizarMostrar();
        }
        protected void rbValor_CheckedChanged(object sender, EventArgs e)
        {
            lblLimiteInferior.Visible = false;
            lblLimiteSuperior.Visible = true;
            txtLimiteInferior.Visible = false;
            txtLimiteSuperior.Visible = true;
            rfvLimiteInferior.Enabled = false;
            rfvLimiteSuperior.Enabled = true;
            
            lblLFDInferior.Visible = false;
            lblLFDSuperior.Visible = true;
            txtLFDInferior.Visible = false;
            txtLFDSuperior.Visible = true;
            rfvLFDInferior.Enabled = false;
            rfvLFDSuperior.Enabled = true;

            txtLFDInferior.Text = txtLimiteInferior.Text;
            txtLFDSuperior.Text = txtLimiteSuperior.Text;

            if (chkNoEvalua.Checked)
            {
                SinLimites();
            }

            ActualizarMostrar();
        }
        protected void chkLimitesLFD_CheckedChanged(object sender, EventArgs e)
        {
            pnlLimitesLFD.Visible = chkLimitesLFD.Checked;
            if (chkLimitesLFD.Checked)
            {
                txtLFDInferior.Text = txtLimiteInferior.Text;
                txtLFDSuperior.Text = txtLimiteSuperior.Text;

                if (rbValor.Checked)
                    rbValor_CheckedChanged(rbValor, null);
                if (rbRango.Checked)
                    rbRango_CheckedChanged(rbValor, null);
            }
            else
            {
                txtLFDInferior.Text = "";
                txtLFDSuperior.Text = "";
            }

            ActualizarMostrar();
        }
        protected void chkNoEvalua_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoEvalua.Checked)
            {
                SinLimites();
                ActualizarMostrar();
            }
            else
            {
                if (rbRango.Checked)
                    rbRango_CheckedChanged(rbRango, new EventArgs());
                if (rbValor.Checked)
                    rbValor_CheckedChanged(rbRango, new EventArgs());
            }
        }
        #endregion
    }

    #region Clases Auxiliares
    public class GrupoConfiguracionItem
    {
        public int NormaID { get; set; }
        public int NormaConfiguracionID { get; set; }
        public int OperacionID { get; set; }
        public string Limite { get; set; }
        public string LimiteLFD { get; set; }
        public string NombreGrupo { get; set; }
        public string NombreOperacion { get; set; }
    }

    public class GrupoConfiguracionArgs : GrupoConfiguracionArgBase
    {

    } 
    #endregion
}