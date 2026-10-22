using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.common;
using SIRALAB.Common;
using SIRALAB_BL.catalogos;
using SIRALAB_DAL;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucNormaConfiguracion : System.Web.UI.UserControl
    {
        #region Atributos
        private int? ParametroID
        {
            get { return ViewState["ParamteroIDNormaConfiguracion"] == null ? null : (int?)ViewState["ParamteroIDNormaConfiguracion"]; }
            set { ViewState["ParamteroIDNormaConfiguracion"] = value; }
        }
        private int? UsoAguaID
        {
            get { return ViewState["UsoAguaIDNormaConfiguracion"] == null ? null : (int?)ViewState["UsoAguaIDNormaConfiguracion"]; }
            set { ViewState["UsoAguaIDNormaConfiguracion"] = value; }
        }
        private int? NivelID
        {
            get { return ViewState["NivelIDNormaConfiguracion"] == null ? null : (int?)ViewState["NivelIDNormaConfiguracion"]; }
            set { ViewState["NivelIDNormaConfiguracion"] = value; }
        }
        private int? CuerpoReceptorID
        {
            get { return ViewState["CuerpoReceptorIDNormaConfiguracion"] == null ? null : (int?)ViewState["CuerpoReceptorIDNormaConfiguracion"]; }
            set { ViewState["CuerpoReceptorIDNormaConfiguracion"] = value; }
        }
        private int? NormaConfiguracionID
        {
            get { return ViewState["NormaConfiguracionIDNormaConfiguracion"] == null ? null : (int?)ViewState["NormaConfiguracionIDNormaConfiguracion"]; }
            set { ViewState["NormaConfiguracionIDNormaConfiguracion"] = value; }
        }
        private int? NormaConfiguracionPadreID
        {
            get { return ViewState["NormaConfiguracionPadreIDNormaConfiguracion"] == null ? null : (int?)ViewState["NormaConfiguracionPadreIDNormaConfiguracion"]; }
            set { ViewState["NormaConfiguracionPadreIDNormaConfiguracion"] = value; }
        }
        private int? NormaConfiguracionPadreGroupID
        {
            get { return ViewState["NormaConfiguracionPadreGroupIDNormaConfiguracion"] == null ? null : (int?)ViewState["NormaConfiguracionPadreGroupIDNormaConfiguracion"]; }
            set { ViewState["NormaConfiguracionPadreGroupIDNormaConfiguracion"] = value; }
        }
        private int? OperacionID
        {
            get { return ViewState["OperacionIDNormaConfiguracion"] == null ? null : (int?)ViewState["OperacionIDNormaConfiguracion"]; }
            set { ViewState["OperacionIDNormaConfiguracion"] = value; }
        }
        private int? Orden
        {
            get { return (int?)ViewState["OrdenNormaConfiguracion"]; }
            set
            {
                ViewState["OrdenNormaConfiguracion"] = value;
            }
        }
        private bool Grupo
        {
            get { return (bool)ViewState["GrupoNormaConfiguracion"]; }
            set
            {
                ViewState["GrupoNormaConfiguracion"] = value;
            }
        }
        private int NormaID
        {
            get { return (int)ViewState["NormaIDNormaConfiguracion"]; }
            set
            {
                ViewState["NormaIDNormaConfiguracion"] = value;
            }
        }
        private bool AllowExent
        {
            get { return ViewState["AllowExentNormaConfiguracion"] != null ? (bool)ViewState["AllowExentNormaConfiguracion"] : false; }
            set { ViewState["AllowExentNormaConfiguracion"] = value; }
        }
        [Browsable(true)]
        public bool AllowGroup
        {
            get { return ViewState["AllowGroupNormaConfiguracion"] != null ? (bool)ViewState["AllowGroupNormaConfiguracion"] : false; }
            set
            {
                ViewState["AllowGroupNormaConfiguracion"] = lblGrupo.Visible = ddlGrupo.Visible = lnkbAgregarGrupo.Visible = value;
            }
        }

        public bool EditMode { get { return ViewState["EditModeNormaConfiguracion"] != null ? (bool)ViewState["EditModeNormaConfiguracion"] : false; } set { ViewState["EditModeNormaConfiguracion"] = value; } }
        #endregion Atributos

        #region Eventos del Control
        [Browsable(true)]
        public event EventHandler<NormaConfiguracionArgs> Guardar;
        [Browsable(true)]
        public event EventHandler<NormaConfiguracionArgs> Cancelar;
        [Browsable(true)]
        public event EventHandler AgregarGrupo;
        #endregion

        #region EventosPagina
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
            }


        }
        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            if (Guardar != null)
            {
                Guardar(this, ObtenConfiguracion());
                //if (this.NormaConfiguracionID == null)
                //{
                //    txtLimiteInferior.Text="";
                //    txtLimiteSuperior.Text = "";
                //    txtLFDInferior.Text = "";
                //    txtLFDSuperior.Text = "";
                //    txtEscalaDecimal.Text = "0";
                //    txtEscalaDecimal.Text = "0";
                //    ddlGrupo.ClearSelection();

                //    rbPresencia.Enabled = true;
                //    rbValor.Enabled = true;
                //    rbRango.Enabled = true;
                //    chkNoEvalua.Enabled = true;
                //    chkLimitesLFD.Enabled = true;

                //    rbPresencia.Checked = false;
                //    rbValor.Checked = false;
                //    rbRango.Checked = true;
                //    chkNoEvalua.Checked = false;
                //    chkLimitesLFD.Checked = false;
                //    chkLimitaValoresAceptados.Checked = false;
                //    rbRango_CheckedChanged(rbRango, null);
                //    chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);
                //    this.NormaConfiguracionPadreID = null;
                //}
            }
        }
        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {
            //mpeParametro.Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalConfiguracion').modal('hide');", true);

            Actualizar();
        }
        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            //mpeParametro.Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalConfiguracion').modal({ backdrop : 'static' });", true);

            Actualizar();
        }
        protected void rbRango_CheckedChanged(object sender, EventArgs e)
        {
            lblLimiteInferior.Visible = true;
            lblLimiteSuperior.Visible = true;
            txtLimiteInferior.Visible = true;
            txtLimiteSuperior.Visible = true;
            rfvLimiteInferior.Enabled = true;
            rfvLimiteSuperior.Enabled = true;
            lblEscalaDecimal.Visible = true;
            lblEscalaDecimalLCM.Visible = true;
            txtEscalaDecimal.Visible = true;
            txtEscalaDecimalLCM.Visible = true;
            lblTrataDecimales.Visible = true;
            rbTrataDecimales.Visible = true;
            pnlValoresMinMax.Visible = true;
            //txtLimiteInferior.Text = "";

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
            lblEscalaDecimal.Visible = true;
            lblEscalaDecimalLCM.Visible = true;
            txtEscalaDecimal.Visible = true;
            txtEscalaDecimalLCM.Visible = true;
            lblTrataDecimales.Visible = true;
            rbTrataDecimales.Visible = true;
            pnlValoresMinMax.Visible = true;
            //txtLimiteInferior.Text = "";
            //txtLimiteSuperior.Text = "";

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
        protected void rbPresencia_CheckedChanged(object sender, EventArgs e)
        {
            lblLimiteInferior.Visible = false;
            lblLimiteSuperior.Visible = false;
            txtLimiteInferior.Visible = false;
            txtLimiteSuperior.Visible = false;
            rfvLimiteInferior.Enabled = false;
            rfvLimiteSuperior.Enabled = false;
            lblEscalaDecimal.Visible = false;
            txtEscalaDecimal.Visible = false;
            lblEscalaDecimal.Visible = false;
            lblEscalaDecimalLCM.Visible = false;
            txtEscalaDecimalLCM.Visible = false;
            lblTrataDecimales.Visible = false;
            rbTrataDecimales.Visible = false;

            //txtLimiteInferior.Text = "";
            //txtLimiteSuperior.Text = "";

            lblLFDInferior.Visible = false;
            lblLFDSuperior.Visible = false;
            txtLFDInferior.Visible = false;
            txtLFDSuperior.Visible = false;
            rfvLFDInferior.Enabled = false;
            rfvLFDSuperior.Enabled = false;

            txtLFDInferior.Text = txtLimiteInferior.Text;
            txtLFDSuperior.Text = txtLimiteSuperior.Text;


            pnlValoresMinMax.Visible = false;

            if (chkNoEvalua.Checked)
            {
                SinLimites();
            }

            ActualizarMostrar();
        }
        protected void lnkbAgregarGrupo_Click(object sender, EventArgs e)
        {
            if (AgregarGrupo != null)
            {
                //mpeParametro.Show();
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalConfiguracion').modal({ backdrop : 'static' });", true);

                AgregarGrupo(this, new GrupoArg { NormaID = this.NormaID, CuerpoReceptorID = (int)this.CuerpoReceptorID, UsoAguaID = (int)this.UsoAguaID, NivelID = (int)this.NivelID });
            }
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
                if (rbPresencia.Checked)
                    rbPresencia_CheckedChanged(rbValor, null);
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
                if (rbPresencia.Checked)
                    rbPresencia_CheckedChanged(rbRango, new EventArgs());
            }
        }

        
        //RFD 16/12/2020 -> Se agrega para la nueva funcionalidad que limita los valores que se pueden capturar para cada parámetro.
        protected void chkLimitaValoresAceptados_CheckedChanged(object sender, EventArgs e)
        {
            pnlInputValoresAceptados.Visible = chkLimitaValoresAceptados.Checked;

            if (chkLimitaValoresAceptados.Checked)
            {
                txtMinAceptado.Text = txtMinAceptado.Text;
                txtMaxAceptado.Text = txtMaxAceptado.Text;
            }
            ActualizarMostrar();
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* Buscamos el grupo y cargamos los mismos valores para el parámetro */
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            int NormaConfiguracionGrupoID = int.Parse(ddlGrupo.SelectedValue);
            this.NormaConfiguracionPadreGroupID = this.NormaConfiguracionPadreID;

            if (NormaConfiguracionGrupoID != 0)
            {
                var configuracionID_actual = int.Parse(ddlParametro.SelectedValue);
                var configuracionID = ncBL.Obtener(NormaConfiguracionGrupoID);
                CargarConfiguracionParaGrupo(configuracionID);

                rbPresencia.Enabled = false;
                rbPresencia.Checked = false;
                rbValor.Enabled = false;
                rbValor.Checked = configuracionID.AceptaValor && (configuracionID.LimiteInferior == -1 || configuracionID.LimiteInferior == null);
                rbRango.Enabled = false;
                rbRango.Checked = configuracionID.AceptaValor && configuracionID.LimiteInferior != -1 && configuracionID.LimiteInferior != null;
                chkLimitesLFD.Enabled = false;
                chkLimitesLFD.Checked = false;
                chkNoEvalua.Enabled = false;
                chkNoEvalua.Checked = true;
                chkNoEvalua_CheckedChanged(chkNoEvalua, null);

                this.NormaConfiguracionPadreID = NormaConfiguracionGrupoID;
                ddlParametro.ClearSelection();
                ddlParametro.Items.FindByValue(configuracionID_actual.ToString()).Selected = true;
                ddlGrupo.ClearSelection();
                ddlGrupo.Items.FindByValue(NormaConfiguracionGrupoID.ToString()).Selected = true;
            }
            else
            {
                
                LimpiarParaGrupos();
                this.NormaConfiguracionPadreID = this.NormaConfiguracionPadreGroupID;
            }

            ActualizarMostrar();
        }
        #endregion EventosPagina

        #region Metodos
        private void CargarCombos()
        {
            LoadDrops.instance.CuerpoReceptor(ddlCuerpoReceptor);
            LoadDrops.instance.UsoAgua(ddlUsoAgua);
            LoadDrops.instance.ParametroConMetodos(ddlParametro);
        }
        private void CargarCombos2021()
        {
            LoadDrops.instance.CuerpoReceptor2021(ddlCuerpoReceptor);
        }
        private void CargarCombosCOT()
        {
            LoadDrops.instance.CuerpoReceptorCOT(ddlCuerpoReceptor);
        }
        private void Limpiar()
        {
            ddlParametro.ClearSelection();
            ddlGrupo.ClearSelection();
            rbRango.Checked = true;
            rbValor.Checked = false;
            rbPresencia.Checked = false;

            rbRango.Enabled = true;
            rbValor.Enabled = true;
            rbPresencia.Enabled = true;

            txtLimiteInferior.Text = "";
            txtLimiteSuperior.Text = "";
            txtLFDInferior.Text = "";
            txtLFDSuperior.Text = "";
            chkNoEvalua.Checked = false;
            chkLimitesLFD.Checked = false;
            txtEtiqueta.Text = string.Empty;
            txtEscalaDecimal.Text = string.Empty;
            txtEscalaDecimalLCM.Text = string.Empty;
            txtMinAceptado.Text = string.Empty;
            txtMaxAceptado.Text = string.Empty;
            chkLimitaValoresAceptados.Checked = false;
            chkLimitaValoresAceptados_CheckedChanged(chkLimitaValoresAceptados, null);

            chkNoEvalua.Enabled = true;
            chkLimitesLFD.Enabled = true;
 
            chkActivo.Checked = true;

            rbRango_CheckedChanged(null, null);
            chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);
        }
        private void LimpiarParaGrupos()
        {

            ddlGrupo.ClearSelection();
            rbRango.Checked = true;
            rbValor.Checked = false;
            rbPresencia.Checked = false;

            rbRango.Enabled = true;
            rbValor.Enabled = true;
            rbPresencia.Enabled = true;

            txtLimiteInferior.Text = "";
            txtLimiteSuperior.Text = "";
            txtLFDInferior.Text = "";
            txtLFDSuperior.Text = "";
            chkNoEvalua.Checked = false;
            chkLimitesLFD.Checked = false;
            //txtEtiqueta.Text = string.Empty;
            //txtEscalaDecimal.Text = string.Empty;
            //txtEscalaDecimalLCM.Text = string.Empty;
            txtMinAceptado.Text = string.Empty;
            txtMaxAceptado.Text = string.Empty;
            chkLimitaValoresAceptados.Checked = false;
            chkLimitaValoresAceptados_CheckedChanged(chkLimitaValoresAceptados, null);

            chkNoEvalua.Enabled = true;
            chkLimitesLFD.Enabled = true;

            chkActivo.Checked = true;

            rbRango_CheckedChanged(null, null);
            chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);
        }
        public void Actualizar()
        {
            upConfiguracion.Update();
        }
        public void ActualizarMostrar()
        {
            //mpeParametro.Show();
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalConfiguracion').modal({ backdrop : 'static' });", true);
            upConfiguracion.Update();
        }
        public void ActualizarGrupo()
        {
            this.NormaID = this.NormaID;
            LoadDrops.instance.GrupoConfiguracion(ddlGrupo, NormaID, int.Parse(ddlCuerpoReceptor.SelectedValue), int.Parse(ddlUsoAgua.SelectedValue), (int)NivelID);
            ActualizarMostrar();
        }
        public void ActualizarGrupo(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            this.NormaID = NormaID;
            LoadDrops.instance.GrupoConfiguracion(ddlGrupo, NormaID, CuerpoReceptorID, UsoAguaID, NivelID);
        }
        public void Mostrar(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            Norma_BL nBL = new Norma_BL();
            ApplicationSettings applicationSettings = new ApplicationSettings();
            nBL.Obtener(NormaID);

            Limpiar();
            this.NormaID = NormaID;
            this.NormaConfiguracionID = null;
            this.NormaConfiguracionPadreID = null;
            this.CuerpoReceptorID = CuerpoReceptorID;
            this.UsoAguaID = UsoAguaID;
            this.NivelID = NivelID;

            if (ddlCuerpoReceptor.Items.Count == 0 || ddlUsoAgua.Items.Count == 0 || ddlParametro.Items.Count == 0)
                CargarCombos();

            ActualizarGrupo(NormaID, CuerpoReceptorID, UsoAguaID, NivelID);
            
            if (nBL.Entidad.Nom2021!=null && nBL.Entidad.Nom2021==true)
            {
                divUsoDelAgua.Visible = false;
                if (nBL.Entidad.Nombre.Replace(" ", "") == applicationSettings.NormaCOT)
                {
                    CargarCombosCOT();
                }
                else if (nBL.Entidad.Nombre.ToUpper().Replace(" ", "") == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ",""))
                {
                    CargarCombosCOT();
                }
                else
                {
                    CargarCombos2021();
                }
                ddlCuerpoReceptor.Enabled = false;
            }
            else
            {
                divUsoDelAgua.Visible = true;
                CargarCombos();
            }

            ddlParametro.ClearSelection();

            ddlCuerpoReceptor.ClearSelection();
            ddlCuerpoReceptor.Items.FindByValue(CuerpoReceptorID.ToString()).Selected = true;

            ddlUsoAgua.ClearSelection();
            ddlUsoAgua.Items.FindByValue(UsoAguaID.ToString()).Selected = true;
           
            rbTrataDecimales.SelectedIndex = 0;
            chkActivo.Checked = true;
            chkExenta.Visible = true;
            this.AllowExent = nBL.Entidad.ExentaParametros;
            chkExenta.Checked = this.AllowExent;
            chkExenta.Enabled = this.AllowExent;

            ActualizarMostrar();
        }
        public void Mostrar(int NormaConfiguracionID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            Norma_BL nBL = new Norma_BL();
            ApplicationSettings applicationSettings = new ApplicationSettings();
           
            ncBL.Obtener(NormaConfiguracionID); 
            nBL.Obtener(ncBL.Entidad.NormaID); 

            this.NormaConfiguracionID = NormaConfiguracionID;
            this.NormaID = ncBL.Entidad.NormaID;
            this.NivelID = ncBL.Entidad.NivelID;
            this.EditMode = true;
            this.AllowExent = ncBL.Entidad.Norma.ExentaParametros;

            var configuracion = ncBL.Obtener((int)NormaConfiguracionID);

            if (configuracion.Norma.Nom2021 != null && configuracion.Norma.Nom2021 == true)
            {
                divUsoDelAgua.Visible = false;
                if (nBL.Entidad.Nombre.Replace(" ", "") == applicationSettings.NormaCOT)
                { 
                    CargarCombosCOT(); 
                }
                else if (nBL.Entidad.Nombre.ToUpper().Replace(" ", "") == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                {
                    CargarCombosCOT();
                }
                else
                { 
                    CargarCombos2021(); 
                }
            }
            else
            {
                divUsoDelAgua.Visible = true;
                CargarCombos();
            }
            CargarConfiguracion(configuracion);

            this.NormaConfiguracionPadreID = ncBL.Entidad.NormaConfiguracionPadreID;
            ActualizarMostrar();
        }
        private void CargarConfiguracion(SIRALAB_DAL.NormaConfiguracion configuracion)
        {
            this.NormaID = configuracion.NormaID;
            this.CuerpoReceptorID = configuracion.CuerpoReceptorID;
            this.UsoAguaID = configuracion.UsoAguaID;
            this.Orden = configuracion.Orden;
            this.NormaConfiguracionPadreID = configuracion.NormaConfiguracionPadreID;
            this.OperacionID = configuracion.OperacionID;

            Limpiar();

            ddlParametro.ClearSelection();
            ddlParametro.Items.FindByValue(configuracion.ParametroID.ToString()).Selected = true;
            ddlCuerpoReceptor.ClearSelection();
            ddlCuerpoReceptor.Items.FindByValue(CuerpoReceptorID.ToString()).Selected = true;
            ddlUsoAgua.ClearSelection();
            ddlUsoAgua.Items.FindByValue(UsoAguaID.ToString()).Selected = true;
            ddlGrupo.ClearSelection();
       
                ActualizarGrupo();
            

            if (configuracion.NormaConfiguracionPadreID != null)
            {
                NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                var ConfiPadre = ncBL.Obtener((int)configuracion.NormaConfiguracionPadreID);

                if (ConfiPadre.Grupo && this.AllowGroup)
                {
                    ddlGrupo.ClearSelection();

                    ddlGrupo.Items.FindByValue(configuracion.NormaConfiguracionPadreID != null ? configuracion.NormaConfiguracionPadreID.ToString() : "0").Selected = true;

                    rbPresencia.Enabled = false;
                    rbPresencia.Checked = false;
                    rbValor.Enabled = false;
                    rbValor.Checked = ConfiPadre.AceptaValor && (ConfiPadre.LimiteInferior == -1 || ConfiPadre.LimiteInferior == null);
                    rbRango.Enabled = false;
                    rbRango.Checked = ConfiPadre.AceptaValor && ConfiPadre.LimiteInferior != -1 && ConfiPadre.LimiteInferior != null;
                    chkLimitesLFD.Enabled = false;
                    chkLimitesLFD.Checked = false;
                    chkNoEvalua.Enabled = false;
                    chkNoEvalua.Checked = true;
                    chkNoEvalua_CheckedChanged(chkNoEvalua, null);
                }
            }

            chkActivo.Checked = configuracion.Activo;
            chkExenta.Checked = configuracion.Exenta;
            chkLCMC.Checked = configuracion.LCMC ?? false;
            txtEscalaDecimal.Text = configuracion.EscalaDecimal.ToString();
            txtEscalaDecimalLCM.Text = configuracion.EscalaDecimalLCM.ToString();
            rbTrataDecimales.SelectedIndex = configuracion.Truncamiento != true ? 0 : 1;


            chkNoEvalua.Checked = !configuracion.Evalua;
            chkLimitesLFD.Checked = configuracion.LFD;
            chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);

            txtMinAceptado.Text = configuracion.MinAceptado == null ? "" : configuracion.MinAceptado.ToString();
            txtMaxAceptado.Text = configuracion.MaxAceptado == null ? "" : configuracion.MaxAceptado.ToString();

            if (configuracion.AceptaValor)
            {
                if (configuracion.LimiteInferior != null && configuracion.LimiteInferior != -1)
                {
                    rbRango.Checked = true;
                    rbRango_CheckedChanged(null, null);
                    txtLimiteInferior.Text = configuracion.LimiteInferior < 0 ? "" : configuracion.LimiteInferior.ToString();
                    txtLimiteSuperior.Text = configuracion.LimiteSuperior < 0 ? "" : configuracion.LimiteSuperior.ToString();
                    txtLFDInferior.Text = configuracion.LimiteInferiorLFD < 0 ? "" : configuracion.LimiteInferiorLFD.ToString();
                    txtLFDSuperior.Text = configuracion.LimiteSuperiorLFD < 0 ? "" : configuracion.LimiteSuperiorLFD.ToString();
                }
                else
                {
                    if (configuracion.LimiteInferior == -1)
                    {
                        rbRango.Checked = true;
                        rbRango_CheckedChanged(null, null);
                        txtLimiteInferior.Text = configuracion.LimiteInferior < 0 ? "" : configuracion.LimiteInferior.ToString();
                        txtLimiteSuperior.Text = configuracion.LimiteSuperior < 0 ? "" : configuracion.LimiteSuperior.ToString();
                        txtLFDInferior.Text = configuracion.LimiteInferiorLFD < 0 ? "" : configuracion.LimiteInferiorLFD.ToString();
                        txtLFDSuperior.Text = configuracion.LimiteSuperiorLFD < 0 ? "" : configuracion.LimiteSuperiorLFD.ToString();
                    }
                    else
                    {
                        rbValor.Checked = true;
                        rbValor_CheckedChanged(null, null);
                        txtLimiteSuperior.Text = configuracion.LimiteSuperior < 0 ? "" : configuracion.LimiteSuperior.ToString();
                        txtLFDSuperior.Text = configuracion.LimiteSuperiorLFD < 0 ? "" : configuracion.LimiteSuperiorLFD.ToString();
                    }
                }

                pnlInputValoresAceptados.Visible = configuracion.LimitaCaptura ?? false;
                chkLimitaValoresAceptados.Checked = configuracion.LimitaCaptura ?? false;
                
            }
            else
            {
                rbPresencia.Checked = true;
                rbPresencia_CheckedChanged(null, null);
                pnlValoresMinMax.Visible = false;
            }

            txtEtiqueta.Text = configuracion.Etiqueta;
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
        private NormaConfiguracionArgs ObtenConfiguracion()
        {
            NormaConfiguracionArgs resultado = new NormaConfiguracionArgs();
            if (this.NormaConfiguracionID != null)
            {
                NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                var nc = ncBL.Obtener((int)this.NormaConfiguracionID);

                if (nc != null)
                {
                    if (nc.NormaConfiguracion2 != null && !nc.NormaConfiguracion2.Grupo)
                    {
                        resultado.OperacionID = this.OperacionID;
                    }
                }
            }

            resultado.NormaConfiguracionID = this.NormaConfiguracionID;
            resultado.NormaID = this.NormaID;
            resultado.ParametroID = int.Parse(ddlParametro.SelectedValue);
            resultado.CuerpoReceptorID = int.Parse(ddlCuerpoReceptor.SelectedValue);
            resultado.UsoAguaID = int.Parse(ddlUsoAgua.SelectedValue);
            resultado.Orden = this.Orden != null ? (int)this.Orden : 0;
            resultado.AceptaValor = rbPresencia.Checked ? false : true;

            resultado.LimiteInferior = (rbRango.Checked && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLimiteInferior.Text) : null;
            resultado.LimiteSuperior = ((rbValor.Checked || rbRango.Checked) && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLimiteSuperior.Text) : null;

            resultado.Activo = chkActivo.Checked;
            resultado.Exenta = this.AllowExent ? chkExenta.Checked : false;
            resultado.LCMC = chkLCMC.Checked;
            resultado.EscalaDecimal = int.Parse(txtEscalaDecimal.Text == "" ? null : txtEscalaDecimal.Text);
            resultado.EscalaDecimalLCM = int.Parse((string.IsNullOrEmpty(txtEscalaDecimalLCM.Text) ? null : txtEscalaDecimalLCM.Text));

            /* Por la leyenda de "No aplica evaluación" se invierte el valor; por default se debe grabar en true para que siempre evalue */
            resultado.Evalua = !chkNoEvalua.Checked;

            resultado.LFD = chkLimitesLFD.Checked;
            resultado.LimiteInferiorLFD = chkLimitesLFD.Checked ? (rbRango.Checked && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLFDInferior.Text) : null : null;
            resultado.LimiteSuperiorLFD = chkLimitesLFD.Checked ? ((rbValor.Checked || rbRango.Checked) && !chkNoEvalua.Checked) ? (decimal?)decimal.Parse(txtLFDSuperior.Text) : null : null;
            resultado.Truncamiento = rbTrataDecimales.SelectedIndex == 1;

            //RFD 16/12/2020 -> valores mín/max aceptados por el parámetro.
            decimal decimalConvertido;
            resultado.LimitaCaptura = chkLimitaValoresAceptados.Checked && (decimal.TryParse(txtMinAceptado.Text, out decimalConvertido) || decimal.TryParse(txtMaxAceptado.Text, out decimalConvertido));
            resultado.MinAceptado = chkLimitaValoresAceptados.Checked ? string.IsNullOrEmpty(txtMinAceptado.Text) ? (decimal?)null : decimal.Parse(txtMinAceptado.Text) : (decimal?)null;
            resultado.MaxAceptado = chkLimitaValoresAceptados.Checked ? string.IsNullOrEmpty(txtMaxAceptado.Text) ? (decimal?)null : decimal.Parse(txtMaxAceptado.Text) : (decimal?)null;


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

            if (this.NormaConfiguracionPadreID != null)
            {
                var Id = ddlGrupo.SelectedValue == "0" ? null : (int?)int.Parse(ddlGrupo.SelectedValue);
                if (Id != null)
                {
                    resultado.NormaConfiguracionPadreID = Id;
                    resultado.OperacionID = null;/* es hijo dentro de un grupo por lo que no realiza operación de agregado */
                }
                else
                {
                    resultado.NormaConfiguracionPadreID = this.NormaConfiguracionPadreID;
                }
            }
            //resultado.NormaConfiguracionPadreID = this.AllowGroup ? ddlGrupo.SelectedValue == "0" ? null : (int?)int.Parse(ddlGrupo.SelectedValue) : this.NormaConfiguracionPadreID;
            resultado.Etiqueta = txtEtiqueta.Text;

            return resultado;
        }


        private void CargarConfiguracionParaGrupo(SIRALAB_DAL.NormaConfiguracion configuracion)
        {
            this.NormaID = configuracion.NormaID;
            this.CuerpoReceptorID = configuracion.CuerpoReceptorID;
            this.UsoAguaID = configuracion.UsoAguaID;
            this.Orden = configuracion.Orden;
            this.NormaConfiguracionPadreID = configuracion.NormaConfiguracionPadreID;
            this.OperacionID = configuracion.OperacionID;


            ddlParametro.ClearSelection();
            ddlParametro.Items.FindByValue(configuracion.ParametroID.ToString()).Selected = true;
            ddlCuerpoReceptor.ClearSelection();
            ddlCuerpoReceptor.Items.FindByValue(CuerpoReceptorID.ToString()).Selected = true;
            ddlUsoAgua.ClearSelection();
            ddlUsoAgua.Items.FindByValue(UsoAguaID.ToString()).Selected = true;
            ddlGrupo.ClearSelection();
            if (ddlGrupo.Items.Count == 0 && this.AllowGroup)
            {
                ActualizarGrupo();
            }

            if (configuracion.NormaConfiguracionPadreID != null)
            {
                NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                var ConfiPadre = ncBL.Obtener((int)configuracion.NormaConfiguracionPadreID);

                if (ConfiPadre.Grupo && this.AllowGroup)
                {
                    ddlGrupo.ClearSelection();
                    ddlGrupo.Items.FindByValue(configuracion.NormaConfiguracionPadreID != null ? configuracion.NormaConfiguracionPadreID.ToString() : "0").Selected = true;

                    rbPresencia.Enabled = false;
                    rbPresencia.Checked = false;
                    rbValor.Enabled = false;
                    rbValor.Checked = ConfiPadre.AceptaValor && (ConfiPadre.LimiteInferior == -1 || ConfiPadre.LimiteInferior == null);
                    rbRango.Enabled = false;
                    rbRango.Checked = ConfiPadre.AceptaValor && ConfiPadre.LimiteInferior != -1 && ConfiPadre.LimiteInferior != null;
                    chkLimitesLFD.Enabled = false;
                    chkLimitesLFD.Checked = false;
                    chkNoEvalua.Enabled = false;
                    chkNoEvalua.Checked = true;
                    chkNoEvalua_CheckedChanged(chkNoEvalua, null);
                }
            }

            //chkActivo.Checked = configuracion.Activo;
            //chkExenta.Checked = configuracion.Exenta;
            //chkLCMC.Checked = configuracion.LCMC ?? false;
            //txtEscalaDecimal.Text = configuracion.EscalaDecimal.ToString();
            //txtEscalaDecimalLCM.Text = configuracion.EscalaDecimalLCM.ToString();
            //rbTrataDecimales.SelectedIndex = configuracion.Truncamiento != true ? 0 : 1;


            chkNoEvalua.Checked = !configuracion.Evalua;
            chkLimitesLFD.Checked = configuracion.LFD;
            chkLimitesLFD_CheckedChanged(chkLimitesLFD, null);

            txtMinAceptado.Text = configuracion.MinAceptado == null ? "" : configuracion.MinAceptado.ToString();
            txtMaxAceptado.Text = configuracion.MaxAceptado == null ? "" : configuracion.MaxAceptado.ToString();

            if (configuracion.AceptaValor)
            {
                if (configuracion.LimiteInferior != null && configuracion.LimiteInferior != -1)
                {
                    rbRango.Checked = true;
                    rbRango_CheckedChanged(null, null);
                    txtLimiteInferior.Text = configuracion.LimiteInferior < 0 ? "" : configuracion.LimiteInferior.ToString();
                    txtLimiteSuperior.Text = configuracion.LimiteSuperior < 0 ? "" : configuracion.LimiteSuperior.ToString();
                    txtLFDInferior.Text = configuracion.LimiteInferiorLFD < 0 ? "" : configuracion.LimiteInferiorLFD.ToString();
                    txtLFDSuperior.Text = configuracion.LimiteSuperiorLFD < 0 ? "" : configuracion.LimiteSuperiorLFD.ToString();
                }
                else
                {
                    if (configuracion.LimiteInferior == -1)
                    {
                        rbRango.Checked = true;
                        rbRango_CheckedChanged(null, null);
                        txtLimiteInferior.Text = configuracion.LimiteInferior < 0 ? "" : configuracion.LimiteInferior.ToString();
                        txtLimiteSuperior.Text = configuracion.LimiteSuperior < 0 ? "" : configuracion.LimiteSuperior.ToString();
                        txtLFDInferior.Text = configuracion.LimiteInferiorLFD < 0 ? "" : configuracion.LimiteInferiorLFD.ToString();
                        txtLFDSuperior.Text = configuracion.LimiteSuperiorLFD < 0 ? "" : configuracion.LimiteSuperiorLFD.ToString();
                    }
                    else
                    {
                        rbValor.Checked = true;
                        rbValor_CheckedChanged(null, null);
                        txtLimiteSuperior.Text = configuracion.LimiteSuperior < 0 ? "" : configuracion.LimiteSuperior.ToString();
                        txtLFDSuperior.Text = configuracion.LimiteSuperiorLFD < 0 ? "" : configuracion.LimiteSuperiorLFD.ToString();
                    }
                }

                pnlInputValoresAceptados.Visible = configuracion.LimitaCaptura ?? false;
                chkLimitaValoresAceptados.Checked = configuracion.LimitaCaptura ?? false;

            }
            else
            {
                rbPresencia.Checked = true;
                rbPresencia_CheckedChanged(null, null);
                pnlValoresMinMax.Visible = false;
            }

            //txtEtiqueta.Text = configuracion.Etiqueta;
        }
        #endregion MetodosFunciones
    }

    public class GrupoArg : EventArgs
    {
        public int NormaID { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int UsoAguaID { get; set; }
        public int NivelID { get; set; }
    }
}