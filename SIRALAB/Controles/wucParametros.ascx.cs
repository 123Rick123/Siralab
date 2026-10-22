using System.Globalization;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.ordentrabajo;
using SIRALAB_DAL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SIRALAB_BL.catalogos;
using System.Text.RegularExpressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SIRALAB.recursos;

namespace SIRALAB.Controles
{
    public partial class wucParametros : System.Web.UI.UserControl
    {
        #region Propiedades

        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_"]; }
        }

        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }

        public Laboratorio LaboratorioDatos
        {
            get { return (Laboratorio)Session["LaboratorioDatos_"]; }
        }

        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }

        public LaboratorioSignatario Signatario_Laboratorio_Persona
        {
            get { return (LaboratorioSignatario)Session["Signatario_Laboratorio_Persona_"]; }
        }

        public bool EsCompuesta
        {
            get { return (bool)Session["EsCompuesta_"]; }
        }

        public int NivelID
        {
            get { return hfNivel.Value.Trim() == "" ? 0 : int.Parse(hfNivel.Value); }
            set { hfNivel.Value = value.ToString(); }
        }


        [Browsable(true)]
        public event EventHandler<bool> DatosFaltantes;
        [Browsable(true)]
        public event EventHandler<bool> Guardado;
        [Browsable(true)]
        public event EventHandler<bool> GuardadoSimple;
        [Browsable(true)]
        public event EventHandler<Dictionary<bool, string>> CheckDQOCOT;


        #endregion Propiedades

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlRepeater.Visible = true;
        }

        protected void ddlLaboratorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            int RowID = int.Parse(ddl.Attributes["RowID"].ToString());
            int ParametroID = int.Parse(ddl.Attributes["ParametroID"].ToString());

            var ddlMetodo = (DropDownList)gvValores.Rows[RowID].FindControl("ddlMetodo");

            Metodo_BL metodo_bl = new Metodo_BL();
            var lista_Metodos = metodo_bl.Obten_Metodo(int.Parse(ddl.SelectedValue.ToString()), ParametroID);
            ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_Metodos);

            if (ddlMetodo.Items.Count == 2)
            {
                ddlMetodo.SelectedIndex = 1;
            }
        }

        protected void lnkbtnGuardarPmtos_Click(object sender, EventArgs e)
        {
            var bandera = true;
            ApplicationSettings applicationSettings = new ApplicationSettings();
            var checkCOT = (CheckBox)Parent.FindControl("chkCOT");
            var checkDQO = (CheckBox)Parent.FindControl("chkDQO");
            var hfDQOCOT = (HiddenField)Parent.FindControl("hfDQOCOT");

            if ((this.OrdenTrabajo.Norma.Nombre.Replace(" ", "") == applicationSettings.NormaCOT || this.OrdenTrabajo.NormaID == 15 
                || this.OrdenTrabajo.Norma.Nombre.ToUpper().Replace(" ", "") == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", "")) 
                && checkCOT != null && checkDQO != null)
            {
                if (hfDQOCOT != null && (checkCOT.Checked || checkDQO.Checked))
                {
                    Analisis_BL analisis_BL = new Analisis_BL();
                    var norma = checkCOT.Checked ? "COT" : "DQO";
                    var normaAnterior = hfDQOCOT.Value;
                    if (normaAnterior == norma)
                        analisis_BL.ActualizaNormaDQOCOT(this.Muestra.AnalisisID, norma);
                    else
                        bandera = false;
                }
                else
                    bandera = false;
            }

            if (bandera)
            {
                bool Faltante = false;
                MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
                List<MuestraValorArgBase_BL> list = new List<MuestraValorArgBase_BL>();

                list = this.NivelID.ConvertToNivel() == enumNivel.Simple
                    ? muestravalor_bl.GuardaObten((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID, NivelID, this.EsCompuesta)
                    : muestravalor_bl.GuardaObtenCompuesta((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID);

                int enablesCount = 0;
                List<bool> listaValida = new List<bool>();

                if (EsCompuesta)
                {
                    Muestra_BL ObternerMuestra = new Muestra_BL();
                    SIRALAB_DAL.Muestra muestraAlmacenada = ObternerMuestra.Obten_Muestra(Muestra.MuestraID);
                    muestraAlmacenada.FlagCompuesta = false;
                    ObternerMuestra.ActualizaMuestra(muestraAlmacenada);
                }
                // se valida para que se considere solo los parametros activos de lo contrario sigue funcionando igual RGG
                int? numParametros = 0;
                if (OrdenTrabajo.Norma.Nom2021 == true)
                {
                    numParametros = OrdenTrabajo?.Norma?.NormaConfiguracions.Where(nc =>
                       nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                       && nc.NormaID == this.OrdenTrabajo.Norma.NormaID
                       && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                       && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                       && nc.Activo
                   ).ToList().Count;
                }
                else
                {
                    numParametros = OrdenTrabajo?.Norma?.NormaConfiguracions.Where(nc =>
                       nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                       && nc.NormaID == this.OrdenTrabajo.Norma.NormaID
                       && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                       && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                        ).ToList().Count;
                }

                var contadorParametros = numParametros ?? 0;


                foreach (GridViewRow RowItem in gvValores.Rows)
                {
                    //bool Guarda = GuardaDatos(RowItem); 
                    bool Completo = DatosCompletos(RowItem); //TODO -> revisar código.

                    MuestraValorArgBase_BL item = new MuestraValorArgBase_BL();
                    int MuestraValorID = int.Parse(gvValores.DataKeys[RowItem.RowIndex].Value.ToString());  //Obtiene MuestraValorID
                    item = list.FirstOrDefault(p => p.MuestraValorID == (int?)MuestraValorID); //Obtiene el item con el MuestraValorID.

                    if (item != null)
                    {
                        #region Controles en la fila
                        var txtResultado = (TextBox)RowItem.FindControl("txtResultado");
                        var rev_txtResultado = (RegularExpressionValidator)RowItem.FindControl("rev_txtResultado");
                        var vce_rev_txtResultado = (ValidatorCalloutExtender)RowItem.FindControl("vce_rev_txtResultado");
                        var ddlResultado = (DropDownList)RowItem.FindControl("ddlResultado");
                        var lblResultado = (Label)RowItem.FindControl("lblResultado");
                        var txtLimiteInferior = (TextBox)RowItem.FindControl("txtLimiteInferior");
                        var txtLimiteSuperior = (TextBox)RowItem.FindControl("txtLimiteSuperior");
                        var txtLC = (TextBox)RowItem.FindControl("txtLC");
                        var ddlLaboratorio = (DropDownList)RowItem.FindControl("ddlLaboratorio");
                        var ddlMetodo = (DropDownList)RowItem.FindControl("ddlMetodo");
                        var lnkbtnExentar = (LinkButton)RowItem.FindControl("lnkbtnExentar");
                        var lnkbtnNoAnalizado = (LinkButton)RowItem.FindControl("lnkbtnNoAnalizado");
                        var lnkbtnEditar = (LinkButton)RowItem.FindControl("lnkbtnEditar");

                        #endregion

                        if ((txtResultado.Enabled && txtResultado.Visible && txtResultado.Text.Trim() != "") || (ddlResultado.Enabled && ddlResultado.Visible) || (item.Grupo == true && OrdenTrabajo.Norma.CPD))  //Si el resultado está activo y el resultado está visible y es diferente de "" o tiene un ddlResultado
                        {
                            if ((bool)item.GeneradoSP) //Si GeneradoSP
                            {
                                item.GeneradoSP = false; /* Indicamos que ya se ha modificado el registro */
                                item.LaboratorioID = ddlLaboratorio.SelectedValue == "0"
                                    ? null
                                    : (int?)int.Parse(ddlLaboratorio.SelectedValue); //Recupera el laboratorioID

                                ParametroMetodo_BL mpBL = new ParametroMetodo_BL();
                                var mp = mpBL.Obten(item.ParametroID,
                                    int.Parse(ddlMetodo
                                        .SelectedValue)); //Recupear el parámetro método: recibe ParametroID y MétodoID del ddl.
                                item.ParametroMetodoID =
                                    mp == null ? null : (int?)mp.ParametroMetodoID; //asigna parámetro método
                                item.LC_MC = txtLC.Text.Trim() == ""
                                    ? null
                                    : (decimal?)decimal.Parse(txtLC.Text.Trim()); //asigna txtLC (LCM)

                                if (item.AceptaValor) //Si acepta valor
                                {
                                    item.Valor =
                                        decimal.Parse(txtResultado.Text.Trim() == "" || txtResultado.Text.Trim() == "---"
                                            ? "0"
                                            : txtResultado.Text); //asigna valor
                                    if (this.OrdenTrabajo.Norma.CPD) // si es cpd -> 
                                    {
                                        item.LimiteInferior = item.LimiteInferior == null
                                            ? null
                                            : (txtLimiteInferior.Text.Trim() == ""
                                                ? -1
                                                : (decimal?)decimal.Parse(txtLimiteInferior
                                                    .Text)); //asigna limiete inferior
                                        item.LimiteSuperior = item.LimiteSuperior == null
                                            ? null
                                            : (txtLimiteSuperior.Text.Trim() == ""
                                                ? -1
                                                : (decimal?)decimal.Parse(txtLimiteSuperior
                                                    .Text)); //asigna límite superior.
                                    }
                                }
                                else //Si no acepta valor.
                                {
                                    item.ValorValidacion =
                                        (bool?)bool.Parse(ddlResultado.SelectedValue); //Asigna valor validación.
                                }

                                /* Validemos que esté completo el registro si no debemos enviar un mensaje diciendo donde falta información */
                                muestravalor_bl.Guardar(item);
                                //Muestra_BL ObternerMuestra = new Muestra_BL();
                                //SIRALAB_DAL.Muestra MuestraAlmacenada = ObternerMuestra.Obten_Muestra(item.MuestraID);
                                if (item.Grupo != true)
                                {
                                    var correcto = OrdenTrabajo.Norma.CPD
                                        ? (getParametroPadreId(getNormaConfiguracionPadreID(item)) == null)
                                            ? Completo
                                            : (Completo && !item.NoAnalizado.Value && !item.Exento.Value &&
                                               item.LimiteSuperior != null && item.LimiteSuperior >= 0)
                                        : Completo;

                                    lnkbtnEditar.CommandName = correcto.ToString();
                                    listaValida.Add(correcto);
                                }

                                if (item.Grupo == true && OrdenTrabajo.Norma.CPD)
                                {
                                    enablesCount++;
                                    listaValida.Add(item.LimiteSuperior != null && item.LimiteSuperior > 0);
                                }

                                saveFlagCompuesta(listaValida, item.MuestraID, contadorParametros);

                                //MuestraAlmacenada.FlagCompuesta =
                                //    (from GridViewRow r in gvValores.Rows
                                //        select (LinkButton)r.FindControl("lnkbtnEditar"))
                                //    .Count(btnedit => btnedit.CommandName == "True") >=
                                //    gvValores.Rows.Count - enablesCount && listaValida.All(p => p);

                                //ObternerMuestra.ActualizaMuestra(MuestraAlmacenada); //Actualiza la muestra.
                                //ObternerMuestra = null;
                                //MuestraAlmacenada = null;
                            }
                            else //Ya han sido modificados ???
                            {
                                /* sí no está exento y no está no analizado entones guardamos cambios */
                                if (!((bool)item.NoAnalizado || (bool)item.Exento))
                                {
                                    item.LaboratorioID = ddlLaboratorio.SelectedValue == "0" ? null : (int?)int.Parse(ddlLaboratorio.SelectedValue);

                                    ParametroMetodo_BL mpBL = new ParametroMetodo_BL();
                                    var mp = mpBL.Obten(item.ParametroID, int.Parse(ddlMetodo.SelectedValue));
                                    item.ParametroMetodoID = mp == null ? null : (int?)mp.ParametroMetodoID;
                                    item.LC_MC = txtLC.Text.Trim() == "" ? null : (decimal?)decimal.Parse(txtLC.Text.Trim());

                                    if (item.AceptaValor)
                                    {
                                        item.Valor = decimal.Parse(txtResultado.Text.Trim() == "" || txtResultado.Text.Trim() == "---" ? "0" : txtResultado.Text);
                                        if (this.OrdenTrabajo.Norma.CPD)
                                        {
                                            item.LimiteInferior = item.LimiteInferior == null ? null : (txtLimiteInferior.Text.Trim() == "" ? 0 : (decimal?)decimal.Parse(txtLimiteInferior.Text));
                                            item.LimiteSuperior = txtLimiteSuperior.Text.Trim() == "" ? 0 : (decimal?)decimal.Parse(txtLimiteSuperior.Text);
                                        }
                                    }
                                    else
                                    {
                                        item.ValorValidacion = (bool?)bool.Parse(ddlResultado.SelectedValue);
                                    }
                                    muestravalor_bl.Guardar(item);
                                    //Muestra_BL ObternerMuestra = new Muestra_BL();
                                    //SIRALAB_DAL.Muestra MuestraAlmacenada = ObternerMuestra.Obten_Muestra(item.MuestraID);

                                    if (item.Grupo != true)
                                    {
                                        var correcto = OrdenTrabajo.Norma.CPD ? (getParametroPadreId(getNormaConfiguracionPadreID(item)) == null) ? Completo
                                                : (Completo && !item.NoAnalizado.Value && !item.Exento.Value && item.LimiteSuperior != null && item.LimiteSuperior >= 0)
                                            : Completo;

                                        lnkbtnEditar.CommandName = correcto.ToString();
                                        listaValida.Add(correcto);
                                    }

                                    //Si es grupo no hay lnkbtnEditar
                                    if (item.Grupo == true && OrdenTrabajo.Norma.CPD)
                                    {
                                        enablesCount++;
                                        listaValida.Add(item.LimiteSuperior != null && item.LimiteSuperior > 0);
                                    }

                                    saveFlagCompuesta(listaValida, item.MuestraID, contadorParametros);

                                    //MuestraAlmacenada.FlagCompuesta = contador >= gvValores.Rows.Count - enablesCount && listaValida.All(p => p);

                                    //ObternerMuestra.ActualizaMuestra(MuestraAlmacenada);
                                    //ObternerMuestra = null;
                                    //MuestraAlmacenada = null;
                                }
                            }
                        }
                        else
                        {
                            enablesCount += 1;
                            listaValida.Add(true);
                            saveFlagCompuesta(listaValida, item.MuestraID, contadorParametros);
                        }
                    }
                }

                GuardaGrupos();
                CargaParametros();

                if (!EsCompuesta)
                {
                    GuardadoSimple(this, listaValida.All(b => b));
                }
                else if (Guardado != null)
                {
                    //TODO: Actualiza 
                    Muestra_BL muestra_bl = new Muestra_BL();
                    muestra_bl.RecalculaSimple(this.Muestra.MuestraID);
                    //TODO: Actualiza
                    Guardado(this, true);
                }
            }
            else
            {
                if (checkCOT.Checked || checkDQO.Checked)
                {
                    var valores = new Dictionary<bool, string>();
                    var norma = checkCOT.Checked ? "COT" : "DQO";
                    valores.Add(true, norma);
                    CheckDQOCOT(this, valores);
                }
                else if (Guardado != null)
                {
                    //TODO: Actualiza 
                    Muestra_BL muestra_bl = new Muestra_BL();
                    muestra_bl.RecalculaSimple(this.Muestra.MuestraID);
                    //TODO: Actualiza
                    Guardado(this, true);
                }
            }
        }
        protected void lnkbtnExentar_Click(object sender, EventArgs e)
        {
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            int MuestraValorID;
            int RowID = 0;
            var boton = (LinkButton)sender;
            MuestraValorID = int.Parse(boton.CommandArgument);
            RowID = int.Parse(boton.Attributes["RowID"].ToString());
            var row = gvValores.Rows[RowID];

            var muestra = muestravalor_bl.Actualiza_Exento(MuestraValorID, true);
            if (muestra != null)
            {
                #region Extracto de controles
                var txtResultado = (TextBox)row.FindControl("txtResultado");
                var ddlResultado = (DropDownList)row.FindControl("ddlResultado");
                var txtLimiteInferior = (TextBox)row.FindControl("txtLimiteInferior");
                var txtLimiteSuperior = (TextBox)row.FindControl("txtLimiteSuperior");
                var txtLC = (TextBox)row.FindControl("txtLC");
                var ddlLaboratorio = (DropDownList)row.FindControl("ddlLaboratorio");
                var ddlMetodo = (DropDownList)row.FindControl("ddlMetodo");
                var lnkbtnExentar = (LinkButton)row.FindControl("lnkbtnExentar");
                var lnkbtnNoAnalizado = (LinkButton)row.FindControl("lnkbtnNoAnalizado");
                var lblResultado = (Label)row.FindControl("lblResultado");
                var lnkbtnEditar = (LinkButton)row.FindControl("lnkbtnEditar");
                var pnlLimiteInferior = (Panel)row.FindControl("pnlLimiteInferior");
                var pnlLimiteSuperior = (Panel)row.FindControl("pnlLimiteSuperior");
                #endregion

                ddlLaboratorio.Enabled = false;
                ddlLaboratorio.SelectedIndex = 0;
                ddlMetodo.Enabled = false;
                ddlMetodo.SelectedIndex = 0;
                txtResultado.Enabled = false;
                txtResultado.Text = "0";
                txtLC.Enabled = false;
                txtLC.Text = "0";
                lblResultado.Visible = false;
                lnkbtnExentar.Visible = false;
                lnkbtnNoAnalizado.Visible = false;
                lnkbtnEditar.Visible = true;
                lnkbtnEditar.CssClass = "btn btn-default btn-xs";

                txtResultado.Attributes.Add("data-noanalizado", "true");
                if (muestra.AceptaValor && (muestra.LimitaCaptura ?? false) && (muestra.MinAceptado != null || muestra.MaxAceptado != null))
                    txtResultado.Attributes.Add("data-novalidalimites", "true");

                if (gvValores.Columns[1].Visible)
                {
                    if (this.OrdenTrabajo.Norma.CPD)
                    {
                        var parametroPadreId = getParametroPadreId(getNormaConfiguracionPadreID(muestra.ParametroID));


                        if (muestra.LimiteInferior != null)
                        {
                            pnlLimiteInferior.Visible = parametroPadreId == null;
                            pnlLimiteSuperior.Visible = parametroPadreId == null;
                            txtLimiteInferior.Enabled = false;
                            txtLimiteInferior.Text = "0";
                            txtLimiteSuperior.Enabled = false;
                            txtLimiteSuperior.Text = "0";
                        }
                        else
                        {
                            pnlLimiteSuperior.Visible = parametroPadreId == null;
                            txtLimiteSuperior.Enabled = false;
                            txtLimiteSuperior.Text = "0";
                        }
                    }
                }
            }
        }
        protected void lnkbtnNoAnalizado_Click(object sender, EventArgs e)
        {
            var boton = (LinkButton)sender;
            int indice = int.Parse(boton.Attributes["RowID"].ToString());
            int MuestraValorID = int.Parse(boton.CommandArgument);

            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            var muestra = muestravalor_bl.Actualiza_NoAnalizado(MuestraValorID, true);
            var row = gvValores.Rows[indice]; /// Extraemos la fila en la que se esta trabajando

            if (muestra != null)
            {
                #region Extracto de controles
                var txtResultado = (TextBox)row.FindControl("txtResultado");
                var ddlResultado = (DropDownList)row.FindControl("ddlResultado");
                var txtLimiteInferior = (TextBox)row.FindControl("txtLimiteInferior");
                var txtLimiteSuperior = (TextBox)row.FindControl("txtLimiteSuperior");
                var txtLC = (TextBox)row.FindControl("txtLC");
                var ddlLaboratorio = (DropDownList)row.FindControl("ddlLaboratorio");
                var ddlMetodo = (DropDownList)row.FindControl("ddlMetodo");
                var lnkbtnExentar = (LinkButton)row.FindControl("lnkbtnExentar");
                var lnkbtnNoAnalizado = (LinkButton)row.FindControl("lnkbtnNoAnalizado");
                var lblResultado = (Label)row.FindControl("lblResultado");
                var lnkbtnEditar = (LinkButton)row.FindControl("lnkbtnEditar");
                var pnlLimiteInferior = (Panel)row.FindControl("pnlLimiteInferior");
                var pnlLimiteSuperior = (Panel)row.FindControl("pnlLimiteSuperior");
                #endregion

                ddlLaboratorio.Enabled = false;
                ddlLaboratorio.SelectedIndex = 0;
                ddlMetodo.SelectedIndex = 0;
                ddlMetodo.Enabled = false;
                txtResultado.Enabled = false;
                ddlResultado.Enabled = false;
                txtResultado.Text = "0";
                txtLC.Enabled = false;
                txtLC.Text = "0";
                lblResultado.Visible = false;
                lnkbtnExentar.Visible = false;
                lnkbtnNoAnalizado.Visible = false;
                lnkbtnEditar.Visible = true;
                lnkbtnEditar.CssClass = "btn btn-default btn-xs";

                txtResultado.Attributes.Add("data-noanalizado", "true");
                if (muestra.AceptaValor && (muestra.LimitaCaptura ?? false) && (muestra.MinAceptado != null || muestra.MaxAceptado != null))
                    txtResultado.Attributes.Add("data-novalidalimites", "true");

                if (gvValores.Columns[1].Visible)
                {
                    if (this.OrdenTrabajo.Norma.CPD)
                    {
                        var parametroPadreId = getParametroPadreId(getNormaConfiguracionPadreID(muestra.ParametroID));

                        if (muestra.LimiteInferior != null)
                        {
                            pnlLimiteInferior.Visible = parametroPadreId == null;
                            pnlLimiteSuperior.Visible = parametroPadreId == null;
                            txtLimiteInferior.Enabled = false;
                            txtLimiteInferior.Text = "0";
                            txtLimiteSuperior.Enabled = false;
                            txtLimiteSuperior.Text = "0";
                        }
                        else
                        {
                            pnlLimiteSuperior.Visible = parametroPadreId == null;
                            txtLimiteSuperior.Enabled = false;
                            txtLimiteSuperior.Text = "0";
                        }
                    }
                }
            }
        }

        protected void lnkbtnEditar_Click(object sender, EventArgs e)
        {
            var boton = (LinkButton)sender;
            int indice = int.Parse(boton.Attributes["RowID"].ToString());
            int MuestraValorID = int.Parse(boton.CommandArgument);

            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            muestravalor_bl.Actualiza_NoAnalizado(MuestraValorID, false);
            muestravalor_bl.Actualiza_Exento(MuestraValorID, false);

            #region Extracto de controles
            var row = gvValores.Rows[indice];
            var txtResultado = (TextBox)row.FindControl("txtResultado");
            var ddlResultado = (DropDownList)row.FindControl("ddlResultado");
            var txtLimiteInferior = (TextBox)row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)row.FindControl("lnkbtnNoAnalizado");
            var lblResultado = (Label)row.FindControl("lblResultado");
            var lnkbtnEditar = (LinkButton)row.FindControl("lnkbtnEditar");
            var rfv_ddlLaboratorio = (RequiredFieldValidator)row.FindControl("rfv_ddlLaboratorio");
            var rfv_ddlMetodo = (RequiredFieldValidator)row.FindControl("rfv_ddlMetodo");
            var pnlLimiteInferior = (Panel)row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)row.FindControl("pnlLimiteSuperior");
            #endregion

            var muestraValor = muestravalor_bl.Obten(MuestraValorID);

            #region Habilitar y deshabilitar controles
            ddlLaboratorio.Enabled = true;
            ddlMetodo.Enabled = true;
            lnkbtnExentar.Enabled = true;
            lnkbtnExentar.Visible = true;
            lnkbtnNoAnalizado.Enabled = true;
            lnkbtnNoAnalizado.Visible = true;
            lblResultado.Visible = false;
            lnkbtnEditar.Visible = false;
            pnlLimiteInferior.Visible = false;
            pnlLimiteSuperior.Visible = false;

            //Configuramos sección de valores que pueden exentar
            if (muestraValor.Exenta)
                lnkbtnExentar.Visible = true;
            else
                lnkbtnExentar.Visible = false;

            //Configuramos parámetros que aceptan valor.
            if (muestraValor.AceptaValor)
            {
                txtResultado.Visible = true;
                txtResultado.Enabled = true;

                txtLC.Visible = true;
                txtLC.Enabled = true;

                if (this.OrdenTrabajo.Norma.CPD)
                {
                    var parametroPadreId = getParametroPadreId(getNormaConfiguracionPadreID(muestraValor.ParametroID));


                    if (muestraValor.LimiteInferior != null)
                    {
                        pnlLimiteInferior.Visible = parametroPadreId == null;
                        pnlLimiteSuperior.Visible = parametroPadreId == null;
                        txtLimiteInferior.Enabled = true;
                        txtLimiteSuperior.Enabled = true;
                    }
                    else
                    {
                        pnlLimiteSuperior.Visible = parametroPadreId == null;
                        txtLimiteSuperior.Enabled = true;
                    }
                }
            }
            else
            {
                ddlResultado.Visible = true;
                ddlResultado.Enabled = true;
            }

            txtResultado.Attributes.Add("data-noanalizado", "false");
            txtResultado.Attributes.Add("data-novalidalimites", "false");
            #endregion
        }
        protected void gvValores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            MuestraValor_BL mvBL = new MuestraValor_BL();
            var mvAB = (MuestraValorArgBase_BL)e.Row.DataItem;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //RFD 10/08/2020 Se agrega para garantizar que la etiqueta, la escala decimal y el LCM siempre se muestran correctamente.
                var etiqueta = OrdenTrabajo.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                        mvAB != null &&  nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID && (nc.ParametroID == mvAB.ParametroID &&
                                             nc.NivelID == enumNivel.Compuesta.ConvertToInt()))
                    ?.Etiqueta;

                mvAB.Etiqueta = etiqueta ?? (mvAB.Parametro.Nombre += " (mg/l)");

                var escalaDecimal = OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                    nc.Activo && nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.ParametroID == mvAB.ParametroID &&
                    nc.NormaID == OrdenTrabajo.NormaID && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID)?.EscalaDecimal;


                mvAB.EscalaDecimal = escalaDecimal ?? mvAB.EscalaDecimal;

                var divLCM = e.Row.FindControl("divLCM");
                if (divLCM != null)
                {
                    //Valida si se muestra o no el LCM dependiendo de como se configuro en la norma 2021 RGG
                    if (OrdenTrabajo.Norma.Nom2021 == true)
                    {
                        try
                        {
                            
                            divLCM.Visible = OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                             nc.Activo && nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.ParametroID == mvAB.ParametroID &&
                             nc.NormaID == OrdenTrabajo.NormaID && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID)?.LCMC ?? false;


                            var nitrogenoTotal = OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                                nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.ParametroID == mvAB.ParametroID &&
                               nc.NormaID == OrdenTrabajo.NormaID && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID).Parametro.Nombre ?? "";
                            if (nitrogenoTotal == "Nitrógeno Total")
                            {
                                divLCM.Visible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            divLCM.Visible = OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                             nc.Activo && nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.ParametroID == mvAB.ParametroID &&
                             nc.NormaID == OrdenTrabajo.NormaID)?.LCMC ?? true;

                        }
                    }
                    else
                    {

                        divLCM.Visible = OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                             nc.Activo && nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.ParametroID == mvAB.ParametroID &&
                             nc.NormaID == OrdenTrabajo.NormaID)?.LCMC ?? true;
                    }
                }
         
     
                    if (mvAB.NoAnalizado == true || mvAB.Exento==true)
                    {
                        mvAB.Valor = 0;
                    }
                
               
                MuestraValorConfig_BL mvcBL = new MuestraValorConfig_BL();
                Laboratorio_BL labBL = new Laboratorio_BL();
                Metodo_BL metBL = new Metodo_BL();
                var mvTemp = mvBL.Obten((int)mvAB.MuestraValorID);

                #region Controles en la fila
                var txtResultado = (TextBox)e.Row.FindControl("txtResultado");
                var ddlResultado = (DropDownList)e.Row.FindControl("ddlResultado");
                var lblResultado = (Label)e.Row.FindControl("lblResultado");
                var txtLimiteInferior = (TextBox)e.Row.FindControl("txtLimiteInferior");
                var txtLimiteSuperior = (TextBox)e.Row.FindControl("txtLimiteSuperior");
                var txtLC = (TextBox)e.Row.FindControl("txtLC");
                var ddlLaboratorio = (DropDownList)e.Row.FindControl("ddlLaboratorio");
                var ddlMetodo = (DropDownList)e.Row.FindControl("ddlMetodo");
                var lnkbtnExentar = (LinkButton)e.Row.FindControl("lnkbtnExentar");
                var lnkbtnNoAnalizado = (LinkButton)e.Row.FindControl("lnkbtnNoAnalizado");
                var lnkbtnEditar = (LinkButton)e.Row.FindControl("lnkbtnEditar");
                var pnlLimiteInferior = (Panel)e.Row.FindControl("pnlLimiteInferior");
                var pnlLimiteSuperior = (Panel)e.Row.FindControl("pnlLimiteSuperior");
                #endregion


                #region HabilitaSuma

                var parametroPadreId = getParametroPadreId(getNormaConfiguracionPadreID(mvAB));

                if (parametroPadreId.HasValue)
                {
                    txtResultado.Attributes.Add("data-grupoid", parametroPadreId.Value.ToString());
                    txtResultado.Attributes.Add("data-noanalizado", mvAB.NoAnalizado == true || mvAB.Exento == true ? "true" : "false");
                }

                if (mvAB.Grupo ?? false)
                {
                    txtResultado.Attributes.Add("data-grupo-padre", mvAB.ParametroID.ToString());
                }

                if (mvAB.AceptaValor && (mvAB.LimitaCaptura ?? false) && mvAB.MinAceptado != null)
                {
                    txtResultado.Attributes.Add("data-minaceptado", mvAB.MinAceptado.ToString());
                }

                if (mvAB.AceptaValor && (mvAB.LimitaCaptura ?? false) && mvAB.MaxAceptado != null)
                {
                    txtResultado.Attributes.Add("data-maxaceptado", mvAB.MaxAceptado.ToString());
                }


                if (mvAB.AceptaValor && (mvAB.LimitaCaptura ?? false) && (mvAB.MinAceptado != null || mvAB.MaxAceptado != null))
                {
                    var valida = mvAB.NoAnalizado == true || (mvAB.Exento == true);
                    txtResultado.Attributes.Add("data-novalidalimites", valida.ToString().ToLower());
                }




                #endregion

                #region Ocultar y deshabilitar controles
                /// Deshabilitamos los controles ///
                Estado_Oculto(e.Row);
                #endregion

                #region Valores en atributos
                #region Agrega la fila a los controles que la utilizan
                ddlLaboratorio.Attributes.Add("RowID", e.Row.RowIndex.ToString());
                lnkbtnEditar.Attributes.Add("RowID", e.Row.RowIndex.ToString());
                lnkbtnNoAnalizado.Attributes.Add("RowID", e.Row.RowIndex.ToString());
                lnkbtnExentar.Attributes.Add("RowID", e.Row.RowIndex.ToString());
                #endregion

                #region Agrega el ParametroID a los controles que la utilizan
                ddlLaboratorio.Attributes.Add("ParametroID", mvAB.ParametroID.ToString());
                lnkbtnEditar.Attributes.Add("ParametroID", mvAB.ParametroID.ToString());
                lnkbtnNoAnalizado.Attributes.Add("ParametroID", mvAB.ParametroID.ToString());
                lnkbtnExentar.Attributes.Add("ParametroID", mvAB.ParametroID.ToString());
                #endregion

                #region Agrega el MuestraValorID a los controles que la utilizan
                ddlLaboratorio.Attributes.Add("MuestraValorID", mvAB.MuestraValorID.ToString());
                lnkbtnEditar.Attributes.Add("MuestraValorID", mvAB.MuestraValorID.ToString());
                lnkbtnNoAnalizado.Attributes.Add("MuestraValorID", mvAB.MuestraValorID.ToString());
                lnkbtnExentar.Attributes.Add("MuestraValorID", mvAB.MuestraValorID.ToString());
                #endregion
                #endregion

                #region Oculta las filas dependiendo del tipo de Norma DQO/COT
                ApplicationSettings applicationSettings = new ApplicationSettings();
                if (this.OrdenTrabajo.Norma.Nombre.Replace(" ", "") == applicationSettings.NormaCOT || this.OrdenTrabajo.Norma.NormaID == 15 || this.OrdenTrabajo.Norma.Nombre.ToUpper().Replace(" ", "") == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                {
                    var checkCOT = (CheckBox)Parent.FindControl("chkCOT");
                    var checkDQO = (CheckBox)Parent.FindControl("chkDQO");
                    if (checkCOT.Checked && mvAB.ParametroID == 667)
                        e.Row.Visible = false;
                    else if (checkDQO.Checked && (mvAB.ParametroID == 553 || mvAB.ParametroID == 602))
                        e.Row.Visible = false;

                    var Conductividad = Muestra.Analisi.Conductividad;
                    if (Conductividad== "Escherichia")
                    {
                        if (mvAB.ParametroID== 689)
                        {
                            e.Row.Visible = false;
                        }
                        
                    }
                    else if (Conductividad== "Enterococos")
                    {
                        if (mvAB.ParametroID == 694)
                        {
                            e.Row.Visible = false;
                        }
                    }

                }

              
                #endregion

                #region Carga de Listados
                /// Cargamos los dropdown
                var listLabParametro = labBL.Obten_Laboratorio((int)this.OrdenTrabajo.NormaID, mvAB.Parametro.ParametroID);
                /*
                 * Le insertamos el valor de la fila que posteriormente se utilizará
                 * para cambiar los métodos aprobados del laboratorio en la misma fila
                 */

                ddlLaboratorio.LoadDrop("Nombre", "LaboratorioID", listLabParametro);
                try
                {
                    ddlLaboratorio.ClearSelection();
                    switch (mvTemp.ParametroID)
                    {
                        case 118:
                        case 79:
                        case 225:
                            var dt = VwOtK.Lab(mvTemp.ParametroID.ToString(CultureInfo.InvariantCulture), Muestra.AnalisisID.ToString(CultureInfo.InvariantCulture)).Tables["Reporte"];
                            var row = dt.Rows[0];
                            var labi = row["LaboratorioID"].ToString();
                            ddlLaboratorio.SelectedValue = mvTemp.LaboratorioID != null ? labi : ((int)this.Login.LaboratorioID).ToString(CultureInfo.InvariantCulture);
                            break;
                        default:
                            var labParametrosID = listLabParametro.Select(q => q.LaboratorioID).ToList();
                            ddlLaboratorio.SelectedValue = mvTemp.LaboratorioID != null
                                ?
                                mvAB.LaboratorioID.ToString()
                                :
                                !mvTemp.NoAnalizado && !mvTemp.Exento && Login.LaboratorioID != null && labParametrosID.Contains((int)Login.LaboratorioID) ? Login.LaboratorioID.ToString() : "0";
                            break;
                    }
                }
                catch (Exception ex) { }

                var listLabMetodo = metBL.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), mvAB.ParametroID);
                ddlMetodo.LoadDrop("Nombre", "MetodoID", listLabMetodo);

                //Seleccion Automatica
                if (ddlMetodo.Items.Count == 2)
                {
                    ddlMetodo.SelectedIndex = 1;
                }
                //Seleccion Automatica
                /*try
                {
                    ddlMetodo.ClearSelection();
                    ddlMetodo.SelectedValue = mvAB.ParametroMetodo.MetodoID.ToString();
                }
                catch (Exception ex) { }*/

                #region Parámetro con valor de presencia
                if (mvAB.Parametro.Nombre.Contains("Flotante"))
                {
                    ListItem li = new ListItem { Value = "false", Text = "Presente" };
                    ListItem li2 = new ListItem { Value = "true", Text = "Ausente", Selected = true };
                    ddlResultado.Items.Insert(0, li);
                    ddlResultado.Items.Insert(1, li2);
                }
                else
                {
                    ListItem li = new ListItem { Value = "true", Text = "Aprueba" };
                    ListItem li2 = new ListItem { Value = "false", Text = "No Aprueba", Selected = true };
                    ddlResultado.Items.Insert(0, li);
                    ddlResultado.Items.Insert(1, li2);
                }
                #endregion
                #endregion

                if (mvAB.GeneradoSP != null && !(bool)mvAB.Grupo)
                {
                    if ((bool)mvAB.GeneradoSP)
                    {
                        #region No ha sido modificado
                        Estado01(e.Row, mvAB);
                        #endregion
                    }
                    else
                    {
                        #region Ha sido modificado en alguna ocasión
                        Estado02(e.Row, mvAB);
                        #endregion
                    }

                    #region Fue marcado como no analizado
                    if ((bool)mvAB.NoAnalizado)
                    {
                        Estado03(e.Row, mvAB);
                    }
                    #endregion

                    #region Fue marcado como exento
                    if ((bool)mvAB.Exento)
                    {
                        Estado04(e.Row, mvAB);
                    }
                    #endregion
                }
                else
                {
                    Estado05(e.Row, mvAB);
                }
            }




        }


        public void CargaInicialParametros(int NormaID, int MuestraID, int NivelID, bool EsCompuesta)
        {
            //Se general los valores de la muestra
            var muestravalor_bl = new MuestraValor_BL();
            var list = new List<MuestraValorArgBase_BL>();

            this.NivelID = NivelID;

            //RFD 11/08/2020 -> Se cambia if por expresión 
            list = this.NivelID.ConvertToNivel() == enumNivel.Simple ? muestravalor_bl.GuardaObten(NormaID, MuestraID, NivelID, EsCompuesta) : muestravalor_bl.GuardaObtenCompuesta(NormaID, MuestraID);
            var list2 = list.OrderBy(p => p.MuestraValorID).ToList();
            Norma_BL NomBLL = new Norma_BL();
            var Norma=NomBLL.Obtener(NormaID);
            foreach (var t in list2)
            {
                switch (t.ParametroID)
                {
                    case 225:
                        t.Valor = substring0(t.Valor);
                        t.LC_MC = substring0(t.LC_MC);
                        break;
                    case 767:
                        t.Valor = substring1(t.Valor);
                        t.LC_MC = substring1(t.LC_MC);
                        break;
                    case 118:
                    case 79:
                        t.Valor = substring4(t.Valor);
                        t.LC_MC = substring4(t.LC_MC);
                        break;
                    case 298:
                        t.Valor = substring1(t.Valor);
                        t.LC_MC = substring1(t.LC_MC);
                        break;
                    case 112:
                    case 618:
                        t.Valor = substring2(t.Valor);
                        t.LC_MC = substring2(t.LC_MC);
                        break;
                    case 836:
                    case 829:
                    case 828:
                    case 835:
                    case 77:
                    case 30:
                    case 45:
                    case 556:
                    case 57:
                    case 606:
                    case 90:
                    case 93:
                    case 106:
                    case 126:
                        t.Valor = substring4(t.Valor);







                        /// Sí es una CPD se deben capturar los límites que se traen en el permiso de descarga, son obligatorios




                        //*********************************************************
                        //En esta parte se mandará la variable si corresponde a una muestra simple o corresponde a una muestra compuesta
                        //*********************************************************




                        t.LC_MC = substring4(t.LC_MC);
                        break;
                    default:
                       
                        if (Norma.Nom2021 == true)
                        {
                            t.Valor = substring4(t.Valor);
                            if (t.LCMC == true)
                            {
                                t.LC_MC = substring4(t.LC_MC);
                            }
                            else
                            {
                                t.LC_MC = null;
                            }
                           
                        }
                        else
                        {
                            t.Valor = substring4(t.Valor);
                            t.LC_MC = substring4(t.LC_MC);
                            
                        }
                        
                        break;

                }

            }

            if (EsCompuesta)
            {
                foreach (var elemento in list2)
                {
                    var etiqueta = OrdenTrabajo.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                            elemento != null && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID && (nc.ParametroID == elemento.ParametroID &&
                                                 nc.NivelID == enumNivel.Compuesta.ConvertToInt()))
                        ?.Etiqueta;

                    if (etiqueta != null)
                    {
                        elemento.Parametro.Nombre = etiqueta;
                    }
                    else
                    {
                        elemento.Parametro.Nombre =  elemento.Parametro.Nombre += " (mg/l)";
                    }
                }

            }
            if (OrdenTrabajo.Norma.Nom2021 == true)
            {

                //quita de la lista el parametro que esta inactivo RGG
                for (int i = list2.Count - 1; i >= 0; i--)
                {
                    var Activo = OrdenTrabajo.Norma?.NormaConfiguracions?.FirstOrDefault(nc => list2[i] != null && (nc.ParametroID == list2[i].ParametroID && nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID))?.Activo;

                    if (Activo == false)
                    {

                        list2.RemoveAt(i);

                    }
                }
            }
            //Se valida que solo se muestren los parametros activos para la norma 2021 de lo contrario sigue funcionando como anteriormente RGG
            List<int> listaAuxiliar = new List<int>();
            if (OrdenTrabajo.Norma.Nom2021 == true)
            {
                 listaAuxiliar = OrdenTrabajo?.Norma?.NormaConfiguracions
               ?.Where(nc => nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                             && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID
                             && nc.UsoAguaID == OrdenTrabajo.UsoAguaID
                             && nc.Activo
               ).OrderBy(q => q.Orden)
               .Select(p => p.ParametroID).ToList();
            }
            else
            {
                 listaAuxiliar = OrdenTrabajo?.Norma?.NormaConfiguracions
              ?.Where(nc => nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                            && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID
                            && nc.UsoAguaID == OrdenTrabajo.UsoAguaID
              ).OrderBy(q => q.Orden)
              .Select(p => p.ParametroID).ToList();
            }

           

            var listaOrdenada = listaAuxiliar?.Select(arg => list2.FirstOrDefault(mvArg => mvArg.ParametroID == arg)).ToList();

            if (list2.Count() > listaAuxiliar?.Count)
            {
                listaOrdenada.AddRange(list2.Where(arg => !listaAuxiliar.Contains(arg.ParametroID)));
            }


            listaOrdenada = EsCompuesta ? listaOrdenada : list2;

            gvValores.DataSource = listaOrdenada; 
            gvValores.DataBind();

            //RFD 11/08/2020 -> Se elimina código harcodeado que no hacía absolutamente nada.

            gvValores.Columns[1].Visible = this.OrdenTrabajo.Norma.CPD;
            pnl.Visible = gvValores.Rows.Count > 0;
        }

        private decimal substring0(decimal? numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    numero = Convert.ToDecimal(split[0]);
                }
            }
            else
            {
                numero = 0;
            }
            return (decimal)numero;
        }
        private decimal substring1(decimal? numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    var a = split[1].ToString(CultureInfo.InvariantCulture);
                    var b = a.Length > 1 ? a.Substring(0, 1) : a;
                    numero = Convert.ToDecimal(split[0] + "." + b);
                }
            }
            else
            {
                numero = 0;
            }
            return (decimal)numero;
        }
        private decimal substring2(decimal? numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    var a = split[1].ToString(CultureInfo.InvariantCulture);
                    var b = a.Length > 2 ? a.Substring(0, 2) : a;
                    numero = Convert.ToDecimal(split[0] + "." + b);
                }
            }
            else
            {
                numero = 0;
            }
            return (decimal)numero;
        }
        private decimal substring4(decimal? numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    var a = split[1].ToString(CultureInfo.InvariantCulture);
                    var b = a.Length > 4 ? a.Substring(0, 4) : a;
                    numero = Convert.ToDecimal(split[0] + "." + b);
                }
            }
            else
            {
                numero = 0;
            }
            return (decimal)numero;
        }

        //TODO -> Refactorizar después de octubre:
        public void CargaParametros()
        {
            var muestravalor_bl = new MuestraValor_BL();
            if (this.OrdenTrabajo.Norma.NormaConfiguracions.Count > 0)
            {
                this.NivelID = this.Muestra.NivelID;
                List<MuestraValorArgBase_BL> list = new List<MuestraValorArgBase_BL>();
                list = this.NivelID.ConvertToNivel() == enumNivel.Simple
                    ? muestravalor_bl.GuardaObten((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID, NivelID, this.EsCompuesta)
                    : muestravalor_bl.GuardaObtenCompuesta((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID);

                //RFD 05/08/2020 -> Se modifica para que mostrar el nombre que viene de la etiqueta en la configuración de la norma.
                if (EsCompuesta)
                {
                    var parametrosEnConfiguracion = OrdenTrabajo?.Norma?.NormaConfiguracions.Where(nc =>
                        nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                        && nc.NormaID == this.OrdenTrabajo.Norma.NormaID
                        && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                        && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                    ).Select(p => p.ParametroID).ToList();

                    if (parametrosEnConfiguracion.Count > list.Count)
                    {
                        var listaParametros = parametrosEnConfiguracion.Where(parametro => !list.Select(p => p.ParametroID).Contains(parametro)).ToList();

                        if (listaParametros.Any())
                        {
                            List<SIRALAB_DAL.NormaConfiguracion> normaConfiguraciones = listaParametros.Select(parametro =>
                                OrdenTrabajo?.Norma?.NormaConfiguracions.FirstOrDefault(nc =>
                                    nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                                    && nc.NormaID == this.OrdenTrabajo.Norma.NormaID
                                    && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                                    && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                                    && nc.ParametroID == parametro)).ToList();

                            muestravalor_bl.InsertaFaltantes(listaParametros, normaConfiguraciones, Muestra.MuestraID);

                            if (list.Any(p => p.Parametro != null && p.Parametro.Nombre.StartsWith("Nitrógeno Total")))
                            {
                                muestravalor_bl.ActualizaModificadosPorNormaConfiguracion(list.FirstOrDefault(p =>
                                            p.Parametro != null && p.Parametro.Nombre.StartsWith("Nitrógeno Total"))?
                                        .MuestraValorID);
                            }

                            list = muestravalor_bl.GuardaObtenCompuesta((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID);
                        }
                    }

                    if (OrdenTrabajo.Norma.Nombre.Contains("Lerma") && !list.Any(p => p.Parametro != null && p.Parametro.Nombre.StartsWith("Nitrógeno Kj")))
                    {
                        muestravalor_bl.ActualizaModificadosPorNormaConfiguracion(list.FirstOrDefault(p =>
                                p.Parametro != null && p.Parametro.Nombre.StartsWith("Nitrógeno Total"))?
                            .MuestraValorID);

                        list = muestravalor_bl.GuardaObtenCompuesta((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID);
                    }

                    foreach (var elemento in list.Where(elemento => elemento.Parametro == null))
                    {
                        elemento.Parametro = OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                            OrdenTrabajo != null
                            && nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                            && nc.NormaID == OrdenTrabajo.Norma.NormaID
                            && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                            && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                            && nc.ParametroID == elemento.ParametroID)?.Parametro;
                    }




                    foreach (var elemento in list)
                    {
                        var etiqueta = OrdenTrabajo.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                                elemento != null && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID && (nc.ParametroID == elemento.ParametroID &&
                                                     nc.NivelID == enumNivel.Compuesta.ConvertToInt()))
                            ?.Etiqueta;

                        elemento.Parametro.Nombre = etiqueta ?? (elemento.Parametro.Nombre += " (mg/l)");

                    }
                    if (OrdenTrabajo.Norma.Nom2021 == true)
                    {

                        //quita de la lista el parametro que esta inactivo RGG
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            var Activo = OrdenTrabajo.Norma?.NormaConfiguracions?.FirstOrDefault(nc => list[i] != null && (nc.ParametroID == list[i].ParametroID && nc.NivelID == enumNivel.Compuesta.ConvertToInt() && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID))?.Activo;

                            if (Activo == false)
                            {

                                list.RemoveAt(i);

                            }
                        }
                    }
                }

                if (list.Any())
                {

                    var list2 = list.OrderBy(q => q.MuestraValorID);
                    //valida que solo se apliquen los cambios para la 2021 es decir que se muestren solo los parametros activos de lo contratio sigue funcionando como anteriormente RGG
                    List<int> listaAuxiliar = new List<int>();
                    if (OrdenTrabajo.Norma.Nom2021 == true)
                    {
                         listaAuxiliar = OrdenTrabajo?.Norma?.NormaConfiguracions
                        ?.Where(nc => nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                        && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID
                        && nc.UsoAguaID == OrdenTrabajo.UsoAguaID
                        && nc.Activo
                        ).OrderBy(q => q.Orden)
                        .Select(p => p.ParametroID).ToList();
                    }
                    else
                    {
                         listaAuxiliar = OrdenTrabajo?.Norma?.NormaConfiguracions
                        ?.Where(nc => nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                        && nc.CuerpoReceptorID == OrdenTrabajo.CuerpoReceptorID
                        && nc.UsoAguaID == OrdenTrabajo.UsoAguaID
                        ).OrderBy(q => q.Orden)
                        .Select(p => p.ParametroID).ToList();
                    }
                    var listaOrdenada = listaAuxiliar.Select(arg => list2.FirstOrDefault(mvArg => mvArg.ParametroID == arg)).ToList();

                    if (list2.Count() > listaAuxiliar.Count)
                    {
                        listaOrdenada.AddRange(list2.Where(arg => !listaAuxiliar.Contains(arg.ParametroID)));
                    }

                    
                    listaOrdenada = EsCompuesta ? listaOrdenada : list;


                    gvValores.DataSource = listaOrdenada;
                    gvValores.DataBind();

                    /// Sí es una CPD se deben capturar los límites que se traen en el permiso de descarga, son obligatorios
                    gvValores.Columns[1].Visible = this.OrdenTrabajo.Norma.CPD;
                    pnl.Visible = gvValores.Rows.Count > 0;
                }
            }
        }

        public void DisableParametros()
        {
            pnlRepeater.Visible = false;
        }
        private void GuardaGrupos()
        {
            /* Acá creamos la sumatoria para todos los parámetros que sean de grupo */
            Muestra_BL mBL = new Muestra_BL();
            var muestra = mBL.Obten_Muestra(this.Muestra.MuestraID);

            var ListaGrupos = muestra.MuestraValors.Where(p => p.Grupo);
            foreach (var item in ListaGrupos)
            {
                Norma_BL nBL = new Norma_BL();
                var NormaGrupo = nBL.Obtener((int)this.OrdenTrabajo.NormaID).NormaConfiguracions.FirstOrDefault(p => p.NivelID == enumNivel.Compuesta.ConvertToInt()
                                                                                                                     && p.ParametroID == item.ParametroID
                                                                                                                     && p.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                                                                                                                     && p.UsoAguaID == this.OrdenTrabajo.UsoAguaID);

                if (NormaGrupo != null)
                {
                    var itemsGrupo = NormaGrupo.NormaConfiguracion1.Select(p => p.ParametroID);

                    var ValorGrupo = muestra.MuestraValors.Any(x => itemsGrupo.Contains(x.ParametroID) && (x.Valor == 0 || x.NoAnalizado || x.Exento) && !x.Grupo) ? 0 : muestra.MuestraValors.Where(p => itemsGrupo.Contains(p.ParametroID) && p.Valor != 0 && !p.NoAnalizado && !p.Exento && !p.Grupo).Sum(q => q.Valor);

                    /* ahora ya tenemos el valor para guardarlo en el grupo */
                    MuestraValor_BL mvBL = new MuestraValor_BL();
                    mvBL.Guardar(item.MuestraValorID, ValorGrupo);
                }
            }
        }

        private bool DatosCompletos(GridViewRow row)
        {
            bool Completo = false;

            #region Extracto de controles
            var txtResultado = (TextBox)row.FindControl("txtResultado");
            var ddlResultado = (DropDownList)row.FindControl("ddlResultado");
            var txtLimiteInferior = (TextBox)row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)row.FindControl("ddlMetodo");
            var lnkbtnEditar = (LinkButton)row.FindControl("lnkbtnEditar");
            #endregion

            int MuestraValorID = int.Parse(lnkbtnEditar.CommandArgument);
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            var mv = muestravalor_bl.Obten(MuestraValorID);

            if (mv.NoAnalizado || mv.Exento)
                return true;

            if (mv.GeneradoSP == null)
                return true;

            if (mv.Grupo)
                return true;

            /* Validamos que conforme a la configuración tenga todos los datos necesarios */
            if (this.OrdenTrabajo.Norma.CPD && mv.AceptaValor)
            {

                if (
                    (txtResultado.Visible && txtResultado.Enabled && txtResultado.Text.Trim() != "" && float.Parse(txtResultado.Text.Trim()) > 0) &&
                    ((txtLimiteInferior.Text.Trim() == "" && mv.LimiteInferior == null) || (txtLimiteInferior.Text.Trim() != "" && mv.LimiteInferior != null)) &&
                    txtLimiteSuperior.Text.Trim() != "" && ddlLaboratorio.SelectedValue != "0" && ddlMetodo.SelectedValue != "0")
                {
                    /* Quiere decir que se capturó completo el parámetro entonces es valido y se puede guardar */
                    if (txtLC.Text.Trim() != "" && float.Parse(txtLC.Text.Trim()) > 0 && Regex.IsMatch(txtLC.Text.Trim(), @"^\d+(\.\d{1,9})?$"))
                    {
                        Completo = true;
                    }
                    else if (txtLC.Text.Trim() == "")
                    {
                        Completo = true;
                    }
                    else if (mv.LCMC == false)
                    {
                        Completo = true;
                    }
                }

                if (txtResultado.Enabled == false)
                {
                    Completo = true;
                }

                var parametroPadreId = getParametroPadreId(OrdenTrabajo?.Norma?.NormaConfiguracions.FirstOrDefault(nc =>
                  nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                  && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                  && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                  && nc.ParametroID == mv.ParametroID)?.NormaConfiguracionPadreID);

                if (parametroPadreId.HasValue)
                {
                    Completo = true;
                }
            }
            else if (!this.OrdenTrabajo.Norma.CPD && mv.AceptaValor && !(this.OrdenTrabajo.FuenteAbastecimiento == null ? false : (bool)this.OrdenTrabajo.FuenteAbastecimiento))
            {
                if (txtResultado.Visible && txtResultado.Enabled && txtResultado.Text.Trim() != "" && float.Parse(txtResultado.Text.Trim()) > 0
                    && ddlLaboratorio.SelectedValue != "0" && ddlMetodo.SelectedValue != "0" && txtResultado.Text.Trim() != "")
                {

                    //TODO -> Refactorizar para tomar en cuenta max/min aceptados.
                    if (txtLC.Text.Trim() != "" && float.Parse(txtLC.Text.Trim()) > 0 && Regex.IsMatch(txtLC.Text.Trim(), @"^\d+(\.\d{1,9})?$"))
                    {
                        Completo = true;
                    }
                    else if (txtLC.Text.Trim() == "")
                    {
                        Completo = true;
                    }
                    else if (mv.LCMC==false)
                    {
                        Completo = true;
                    }
                }
            }
            else if (!this.OrdenTrabajo.Norma.CPD && mv.AceptaValor && (this.OrdenTrabajo.FuenteAbastecimiento == null ? false : (bool)this.OrdenTrabajo.FuenteAbastecimiento))
            {
                if (txtResultado.Visible && txtResultado.Enabled && txtResultado.Text.Trim() != "" && float.Parse(txtResultado.Text.Trim()) > 0
                    && ddlLaboratorio.SelectedValue != "0" && ddlMetodo.SelectedValue != "0")
                {
                    if (txtLC.Text.Trim() != "" && float.Parse(txtLC.Text.Trim()) > 0 && Regex.IsMatch(txtLC.Text.Trim(), @"^\d+(\.\d{1,9})?$"))
                    {
                        Completo = true;
                    }
                    else if (txtLC.Text.Trim() == "")
                    {
                        Completo = true;
                    }
                    else if (mv.LCMC == false)
                    {
                        Completo = true;
                    }
                }
            }
            else if (!mv.AceptaValor)
            {
                if (ddlLaboratorio.SelectedValue != "0" && ddlMetodo.SelectedValue != "0")
                {
                    /* Quiere decir que se capturó completo el parámetro entonces es valido y se puede guardar */
                    Completo = true;
                }
            }
            else if ((bool)mv.Exento || mv.NoAnalizado)
            {
                Completo = true;
            }
            return Completo;
        }
        private void Estado_Oculto(GridViewRow Row)
        {
            #region Controles de fila
            var txtResultado = (TextBox)Row.FindControl("txtResultado");
            var ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
            var lblResultado = (Label)Row.FindControl("lblResultado");
            var txtLimiteInferior = (TextBox)Row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)Row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)Row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)Row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)Row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)Row.FindControl("lnkbtnNoAnalizado");
            var lnkbtnEditar = (LinkButton)Row.FindControl("lnkbtnEditar");
            var pnlLimiteInferior = (Panel)Row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)Row.FindControl("pnlLimiteSuperior");
            #endregion

            /// Deshabilitamos los controles ///
            txtResultado.Visible = false;
            ddlResultado.Visible = false;
            lblResultado.Visible = false;

            txtLC.Enabled = false;
            ddlLaboratorio.Enabled = false;
            ddlMetodo.Enabled = false;

            lnkbtnExentar.Visible = false;
            lnkbtnNoAnalizado.Visible = false;
            lnkbtnEditar.Visible = false;

            pnlLimiteInferior.Visible = false;
            pnlLimiteSuperior.Visible = false;

            txtLimiteInferior.Enabled = false;
            txtLimiteSuperior.Enabled = false;
            txtLimiteInferior.Text = "";
            txtLimiteSuperior.Text = "";
        }

        private void Estado01(GridViewRow Row, MuestraValorArgBase_BL mvAB)
        {
            #region Controles de fila
            var txtResultado = (TextBox)Row.FindControl("txtResultado");
            var ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
            var lblResultado = (Label)Row.FindControl("lblResultado");
            var txtLimiteInferior = (TextBox)Row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)Row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)Row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)Row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)Row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)Row.FindControl("lnkbtnNoAnalizado");
            var lnkbtnEditar = (LinkButton)Row.FindControl("lnkbtnEditar");
            var pnlLimiteInferior = (Panel)Row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)Row.FindControl("pnlLimiteSuperior");
            #endregion

            /// Se encuentra como fue generado por el Stored Procedure (Estado inicial)
            /// por lo que se tiene que proceder a validar la configuración del parámetro
            lnkbtnExentar.Visible = (bool)mvAB.Exenta;
            lnkbtnNoAnalizado.Visible = true;
            txtLC.Enabled = true;
            ddlLaboratorio.Enabled = true;
            ddlMetodo.Enabled = true;

            if (mvAB.AceptaValor)
            {
                txtResultado.Visible = true;
                txtResultado.Enabled = !(bool)mvAB.Grupo;
                txtResultado.Text = mvAB.Valor.ToString();

                if (this.OrdenTrabajo.Norma.CPD /*&& (bool)mvAB.Evalua*/)
                {
                    if (mvAB.LimiteInferior != null)
                    {
                        pnlLimiteInferior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                        pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                        txtLimiteInferior.Enabled = true;
                        txtLimiteSuperior.Enabled = true;
                        txtLimiteInferior.Text = mvAB.LimiteInferior < 0 ? "0" : mvAB.LimiteInferior.ToString();
                        txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                    }
                    else
                    {
                        pnlLimiteSuperior.Visible = (getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null); ;
                        txtLimiteSuperior.Enabled = true;
                        txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                    }
                }
            }
            else
            {
                ddlResultado.Visible = true;
                txtLC.Enabled = false;

                if (this.OrdenTrabajo.Norma.CPD)
                {
                    pnlLimiteInferior.Visible = false;
                    pnlLimiteSuperior.Visible = false;
                }
            }
        }
        private void Estado02(GridViewRow Row, MuestraValorArgBase_BL mvAB)
        {
            #region Controles de fila
            var txtResultado = (TextBox)Row.FindControl("txtResultado");

            var ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
            var lblResultado = (Label)Row.FindControl("lblResultado");
            var txtLimiteInferior = (TextBox)Row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)Row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)Row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)Row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)Row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)Row.FindControl("lnkbtnNoAnalizado");
            var lnkbtnEditar = (LinkButton)Row.FindControl("lnkbtnEditar");

            var pnlLimiteInferior = (Panel)Row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)Row.FindControl("pnlLimiteSuperior");
            #endregion
            /// Ya ha sido modificado por el sistema por lo que cargamos el estado en el que se encuentra

            /// Verificamos que no se encuentre exento o no analizado
            /// Sí se encuentra exento (true) o se encuentra no analizado (true)
            /// los botones para exentar y analizar no se muestran
            lnkbtnExentar.Visible = (((bool)mvAB.Exento || !(bool)mvAB.NoAnalizado)) && (bool)mvAB.Exenta;
            lnkbtnNoAnalizado.Visible = !((bool)mvAB.Exento || (bool)mvAB.NoAnalizado);

            /// Como ya fue guardado debe contener laboratorio y método
            /// por lo que cargamos y seleccionamos los datos
            /// 
            ddlLaboratorio.ClearSelection();
            try
            {
                if (mvAB.LaboratorioID != null)
                {
                    ddlLaboratorio.Items.FindByValue(mvAB.LaboratorioID.ToString()).Selected = true;
                }
            }
            catch { }

            Metodo_BL metBL = new Metodo_BL();
            var listLabMetodo = metBL.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), mvAB.ParametroID);
            ddlMetodo.LoadDrop("Nombre", "MetodoID", listLabMetodo);

            //TODO -> Precargar método.

            try
            {
                if (mvAB.ParametroMetodo != null)
                {
                    ddlMetodo.SelectedValue = mvAB.ParametroMetodo.MetodoID.ToString();
                }
            }
            catch { }

            txtLC.Enabled = true;
            ddlLaboratorio.Enabled = true;
            ddlMetodo.Enabled = true;

            if (mvAB.AceptaValor)
            {
                txtResultado.Visible = true;
                txtResultado.Enabled = !(bool)mvAB.Grupo;
                txtResultado.Text = mvAB.Valor.ToString();

                txtResultado.Text = mvAB.Valor != 0 ? mvAB.Valor.ToString() : "";
                txtLC.Text = mvAB.LC_MC != null ? mvAB.LC_MC.ToString() : "";

                if (this.OrdenTrabajo.Norma.CPD)
                {
                    if (mvAB.LimiteInferior != null)
                    {
                        pnlLimiteInferior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                        pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                        txtLimiteInferior.Enabled = true;
                        txtLimiteSuperior.Enabled = true;
                        txtLimiteInferior.Text = mvAB.LimiteInferior < 0 ? "0" : mvAB.LimiteInferior.ToString();
                        txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                    }
                    else
                    {
                        pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                        txtLimiteSuperior.Enabled = true;
                        txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                    }
                }
            }
            else
            {
                ddlResultado.Visible = true;
                txtLC.Enabled = false;

                ddlResultado.ClearSelection();
                string valor = mvAB.ValorValidacion.ToString().ToLower();
                ddlResultado.Items.FindByValue(valor).Selected = true;
            }
        }
        private void Estado03(GridViewRow Row, MuestraValorArgBase_BL mvAB)
        {
            #region Controles de fila
            var txtResultado = (TextBox)Row.FindControl("txtResultado");

            var ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
            var lblResultado = (Label)Row.FindControl("lblResultado");
            var txtLimiteInferior = (TextBox)Row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)Row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)Row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)Row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)Row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)Row.FindControl("lnkbtnNoAnalizado");
            var lnkbtnEditar = (LinkButton)Row.FindControl("lnkbtnEditar");

            var pnlLimiteInferior = (Panel)Row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)Row.FindControl("pnlLimiteSuperior");
            #endregion

            ddlLaboratorio.Enabled = false;
            ddlMetodo.Enabled = false;
            txtResultado.Enabled = false;
            ddlResultado.Enabled = false;
            txtLC.Enabled = false;
            lblResultado.Visible = false;
            lnkbtnExentar.Visible = false;
            lnkbtnNoAnalizado.Visible = false;
            lnkbtnEditar.Visible = (bool)mvAB.Grupo ? false : true;
            lnkbtnEditar.CssClass = "btn btn-default btn-xs";
            #region Límites cuando es CPD
            if (mvAB.AceptaValor && this.OrdenTrabajo.Norma.CPD /*&& (bool)mvAB.Evalua*/)
            {
                if (mvAB.LimiteInferior != null)
                {
                    pnlLimiteInferior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                    pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                    txtLimiteInferior.Enabled = false;
                    txtLimiteSuperior.Enabled = false;
                    txtLimiteInferior.Text = mvAB.LimiteInferior < 0 ? "0" : mvAB.LimiteInferior.ToString();
                    txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                }
                else
                {
                    pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null; ;
                    txtLimiteSuperior.Enabled = false;
                    txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                }
            }
            #endregion
        }
        private void Estado04(GridViewRow Row, MuestraValorArgBase_BL mvAB)
        {
            #region Controles de fila
            var txtResultado = (TextBox)Row.FindControl("txtResultado");

            var ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
            var lblResultado = (Label)Row.FindControl("lblResultado");
            var txtLimiteInferior = (TextBox)Row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)Row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)Row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)Row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)Row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)Row.FindControl("lnkbtnNoAnalizado");
            var lnkbtnEditar = (LinkButton)Row.FindControl("lnkbtnEditar");

            var pnlLimiteInferior = (Panel)Row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)Row.FindControl("pnlLimiteSuperior");
            #endregion

            ddlLaboratorio.Enabled = false;
            ddlMetodo.Enabled = false;
            txtResultado.Enabled = false;
            txtLC.Enabled = false;
            lblResultado.Visible = false;
            lnkbtnExentar.Visible = false;
            lnkbtnNoAnalizado.Visible = false;
            lnkbtnEditar.Visible = (bool)mvAB.Grupo ? false : true;
            lnkbtnEditar.CssClass = "btn btn-default btn-xs";

            #region Límites cuando es CPD
            if (mvAB.AceptaValor && this.OrdenTrabajo.Norma.CPD /*&& (bool)mvAB.Evalua*/)
            {
                if (mvAB.LimiteInferior != null)
                {
                    pnlLimiteInferior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                    pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                    txtLimiteInferior.Enabled = false;
                    txtLimiteSuperior.Enabled = false;
                    txtLimiteInferior.Text = mvAB.LimiteInferior < 0 ? "0" : mvAB.LimiteInferior.ToString();
                    txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                }
                else
                {
                    pnlLimiteSuperior.Visible = getParametroPadreId(getNormaConfiguracionPadreID(mvAB)) == null;
                    txtLimiteSuperior.Enabled = false;
                    txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                }
            }
            #endregion
        }

        //Calculados por el sistema -> se calculan con los valores de la muestra compuesta:
        private void Estado05(GridViewRow Row, MuestraValorArgBase_BL mvAB)
        {
            #region Controles de fila
            var txtResultado = (TextBox)Row.FindControl("txtResultado");

            var ddlResultado = (DropDownList)Row.FindControl("ddlResultado");
            var lblResultado = (Label)Row.FindControl("lblResultado");
            var txtLimiteInferior = (TextBox)Row.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Row.FindControl("txtLimiteSuperior");
            var txtLC = (TextBox)Row.FindControl("txtLC");
            var ddlLaboratorio = (DropDownList)Row.FindControl("ddlLaboratorio");
            var ddlMetodo = (DropDownList)Row.FindControl("ddlMetodo");
            var lnkbtnExentar = (LinkButton)Row.FindControl("lnkbtnExentar");
            var lnkbtnNoAnalizado = (LinkButton)Row.FindControl("lnkbtnNoAnalizado");
            var lnkbtnEditar = (LinkButton)Row.FindControl("lnkbtnEditar");

            var pnlLimiteInferior = (Panel)Row.FindControl("pnlLimiteInferior");
            var pnlLimiteSuperior = (Panel)Row.FindControl("pnlLimiteSuperior");
            #endregion
            /* Este estado dice que ha sido calculado por el sistema por lo que se muestran los datos validados por el mismo */
            #region Ha sido calculado por el sistema
            /// Ya ha sido modificado por el sistema por lo que cargamos el estado en el que se encuentra

            /// Verificamos que no se encuentre exento o no analizado
            /// Sí se encuentra exento (true) o se encuentra no analizado (true)
            /// los botones para exentar y analizar no se muestran
            lnkbtnExentar.Visible = (((bool)mvAB.Exento || !(bool)mvAB.NoAnalizado)) && (bool)mvAB.Exenta;
            lnkbtnNoAnalizado.Visible = !((bool)mvAB.Exento || (bool)mvAB.NoAnalizado);

            /// Como ya fue guardado debe contener laboratorio y método
            /// por lo que cargamos y seleccionamos los datos
            /// 

            ////////ddlLaboratorio.ClearSelection();
            ////////try { ddlLaboratorio.Items.FindByValue(mvAB.LaboratorioID.ToString()).Selected = true; }
            ////////catch { }



            if (mvAB.ParametroMetodo == null)
            {

                var muestra_BL = new Muestra_BL();
                var muestra = muestra_BL.Obten_Muestra(mvAB.MuestraID);

                var metodoId = muestra?.Analisi?.Muestras
                    .FirstOrDefault(m => m.NivelID == 1)?.MuestraValors
                    .FirstOrDefault(mv => mv.ParametroID == mvAB.ParametroID)?.ParametroMetodo?.MetodoID;


                Metodo_BL metBL = new Metodo_BL();
                var listLabMetodo = metBL.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), mvAB.ParametroID);
                ddlMetodo.LoadDrop("Nombre", "MetodoID", listLabMetodo);
                try { ddlMetodo.ClearSelection(); ddlMetodo.SelectedValue = metodoId.ToString(); }
                catch { }

            }




            if (mvAB.AceptaValor)
            {
                //prueba
                lnkbtnExentar.Enabled = false;
                lnkbtnNoAnalizado.Enabled = false;
                lnkbtnNoAnalizado.Visible = false;
                lnkbtnExentar.Visible = false;
                //Prueba
                txtResultado.Visible = true;
                txtResultado.Enabled = false;

                txtResultado.Text = mvAB.Valor != 0 ? mvAB.Valor.ToString() : "";
                txtLC.Text = mvAB.LC_MC != null ? mvAB.LC_MC.ToString() : "";
                txtLC.Enabled = false;

                if (this.OrdenTrabajo.Norma.CPD /*&& (bool)mvAB.Evalua*/)
                {
                    if (mvAB.LimiteInferior != null)
                    {
                        pnlLimiteInferior.Visible = true;
                        pnlLimiteSuperior.Visible = true;
                        pnlLimiteInferior.Enabled = false;
                        pnlLimiteSuperior.Enabled = false;
                        txtLimiteInferior.Text = mvAB.LimiteInferior < 0 ? "0" : mvAB.LimiteInferior.ToString();
                        txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                    }
                    else
                    {
                        pnlLimiteSuperior.Visible = true;
                        pnlLimiteSuperior.Enabled = mvAB.Grupo ?? false;

                        if (mvAB.Grupo ?? false)
                        {
                            txtLimiteSuperior.Enabled = true;
                        }

                        txtLimiteSuperior.Text = mvAB.LimiteSuperior < 0 ? "0" : mvAB.LimiteSuperior.ToString();
                    }
                }
            }
            else
            {
                ddlResultado.Visible = true;
                ddlResultado.Enabled = false;
                ddlResultado.ClearSelection();
                string valor = mvAB.ValorValidacion.ToString().ToLower();
                ddlResultado.Items.FindByValue(valor).Selected = true;
            }

            if (mvAB.Grupo == true)
            {
                var divDdlLaboratorio = Row.FindControl("divDdlLaboratorio");
                var divDdlMetodo = Row.FindControl("divDdlMetodo");

                if (divDdlLaboratorio != null)
                {
                    divDdlLaboratorio.Visible = false;
                }

                if (divDdlMetodo != null)
                {
                    divDdlMetodo.Visible = false;
                }
            }
            #endregion
        }


        private int? getNormaConfiguracionPadreID(MuestraValorArgBase_BL args)
        {
            return OrdenTrabajo?.Norma?.NormaConfiguracions.FirstOrDefault(nc =>
                nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                && nc.ParametroID == args.ParametroID)?.NormaConfiguracionPadreID;
        }

        private int? getNormaConfiguracionPadreID(int parametroID)
        {
            return OrdenTrabajo?.Norma?.NormaConfiguracions.FirstOrDefault(nc =>
                nc.NivelID == enumNivel.Compuesta.ConvertToInt()
                && nc.CuerpoReceptorID == this.OrdenTrabajo.CuerpoReceptorID
                && nc.UsoAguaID == this.OrdenTrabajo.UsoAguaID
                && nc.ParametroID == parametroID)?.NormaConfiguracionPadreID;
        }

        private int? getParametroPadreId(int? normaConfiguracionPadreId)
        {
            return OrdenTrabajo?.Norma?.NormaConfiguracions?
                .FirstOrDefault(nc => normaConfiguracionPadreId != null && (nc.NormaConfiguracionID == normaConfiguracionPadreId.Value && nc.NivelID == enumNivel.Compuesta.ConvertToInt()))?.ParametroID;
        }


        //RFD 20/08/2020 -> Optimiza guardar flagCompuesta. 
        private void saveFlagCompuesta(List<bool> listaValida, int muestraID, int numParametros)
        {
            if (numParametros > 0 && listaValida.Count == numParametros && listaValida.All(p => p))
            {
                Muestra_BL ObternerMuestra = new Muestra_BL();
                SIRALAB_DAL.Muestra muestraAlmacenada = ObternerMuestra.Obten_Muestra(muestraID);
                muestraAlmacenada.FlagCompuesta = true;
                ObternerMuestra.ActualizaMuestra(muestraAlmacenada);
            }
        }

        public void ChkCOT(bool check)
        {
            int parametroID;

            for (int i = 0; i < gvValores.Rows.Count; i++)
            {
                DropDownList ddl = (DropDownList)gvValores.Rows[i].FindControl("ddlLaboratorio");
                parametroID = int.Parse(ddl.Attributes["ParametroID"].ToString());
                if (parametroID == 667)
                    gvValores.Rows[i].Visible = !check;
                else if (parametroID == 553 || parametroID == 602)
                    gvValores.Rows[i].Visible = true;
            }
        }

        public void ChkDQO(bool check)
        {
            int parametroID;

            for (int i = 0; i < gvValores.Rows.Count; i++)
            {
                DropDownList ddl = (DropDownList)gvValores.Rows[i].FindControl("ddlLaboratorio");
                parametroID = int.Parse(ddl.Attributes["ParametroID"].ToString());
                if (parametroID == 553 || parametroID == 602)
                    gvValores.Rows[i].Visible = !check;
                else if(parametroID == 667)
                    gvValores.Rows[i].Visible = true;
            }
        }
    }
}