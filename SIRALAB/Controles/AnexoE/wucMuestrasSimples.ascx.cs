using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.WebSockets;
using AjaxControlToolkit;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using SIRALAB_BL;
using SIRALAB_BL.catalogos;
using SIRALAB_DAL;
using SIRALAB_DAL.Extensiones;
using SIRALAB_BL.common;
using System.ComponentModel;

namespace SIRALAB.Controles.AnexoE
{
    public partial class wucMuestrasSimples : System.Web.UI.UserControl
    {
        readonly Laboratorio_BL _labBl = new Laboratorio_BL();
        readonly Metodo_BL _metBl = new Metodo_BL();

        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_"]; }
        }

        public List<Columnas> Columnas
        {
            set { Session["ColumnasSimples"] = value; }
            get { return (List<Columnas>)Session["ColumnasSimples"]; }
        }

        public List<Columnas> ColumnasCompuesta
        {
            set { Session["ColumnasCompuesta"] = value; }
            get { return (List<Columnas>)Session["ColumnasCompuesta"]; }
        }

        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        public string SesionEcoliEnterococos
        {
            get { return (string)Session["SesionEcoliEnterococos"]; }
        }
        public LaboratorioVigencia Vigencia
        {
            get
            {
                return Session["LaboratorioVigencia"] == null
                    ? null
                    : (LaboratorioVigencia)Session["LaboratorioVigencia"];
            }
        }

        [Browsable(true)]
        public event EventHandler<Dictionary<bool, string>> CheckEcoliEnterococos;

        public decimal VolumenCompuesta
        {
            set { ViewState["VolumenCompuesta"] = value; }
            get { return ViewState["VolumenCompuesta"] == null ? 0 : (decimal)ViewState["VolumenCompuesta"]; }
        }

        public DateTime FechaConformacion
        {
            set { ViewState["FechaConformacion"] = value; }
            get { return ViewState["FechaConformacion"] == null ? DateTime.Now : (DateTime)ViewState["FechaConformacion"]; }
        }

        public string fechaConformacionTexto
        {
            set { ViewState["fechaConformacionTexto"] = value; }
            get { return (string)ViewState["fechaConformacionTexto"]; }
        }

        public string FechaConformacionMuestraCompuesta
        {
            set { ViewState["FechaConformacionMuestraCompuesta"] = value; }
            get { return (string)ViewState["FechaConformacionMuestraCompuesta"]; }
        }

        public int AnalisisID
        {
            set { ViewState["AnalisisID"] = value; }
            get { return (int)ViewState["AnalisisID"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(int analisisId)
        {
            AnalisisID = analisisId;
            NormaConfiguracion_BL _configuracionBl = new NormaConfiguracion_BL();
            var configuracion = _configuracionBl.ObtenerNormaConfiguracion(OrdenTrabajo.NormaID ?? 0,
                    OrdenTrabajo.CuerpoReceptorID ?? 0,
                    OrdenTrabajo.UsoAguaID ?? 0,
                    enumNivel.Compuesta.ConvertToInt()).AsQueryable()
                .Include(r => r.NormaConfiguracion2.Parametro.CatUnidadMedida)
                .Include(r => r.Parametro.CatUnidadMedida)
                .Where(p => p.NormaConfiguracionPadreID != null &&
                            p.NormaConfiguracion2.Parametro.EsCompuesta == false);

            Muestra_BL _muestraBl = new Muestra_BL();
            MuestraValor_BL _muestravalorBl = new MuestraValor_BL();
            Analisis_BL _analisisBl = new Analisis_BL();
            var analisisPadre = _analisisBl.Obten_Analisis(analisisId).Analisi1?.Muestras
                .FirstOrDefault(q => q.NivelID == (int)enumNivel.Simple);
            var parametrosAnalisisPadre = new List<int>();
            if (analisisPadre != null)
            {
                parametrosAnalisisPadre = analisisPadre.MuestraValors.Select(p => p.ParametroID).ToList();
            }

            var indiceMuestra = 0;
            var muestras = _muestraBl.Lista_Muestra(analisisId);
            foreach (var muestra in muestras.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()))
            {
                indiceMuestra++;
                if (!muestra.MuestraValors.Any())
                {
                    _muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, muestra.MuestraID, muestra.NivelID, false);
                    muestra.MuestraValors = _muestravalorBl.Lista_MuestraValor(muestra.MuestraID);
                }
                else //RFD 08/06/2020 -> Agrego método para odenar los valores de la muestra de acuerdo a la configuración.
                {
                    muestra.MuestraValors = _muestravalorBl.Lista_MuestraValor(muestra.MuestraID);
                }

                muestra.Index = indiceMuestra;
            }

            var primerMuestra = muestras.FirstOrDefault();
            hfAnalisisID.Value = analisisId.ToString();
            hfNormaId.Value = OrdenTrabajo.NormaID.ToString();
            VolumenCompuesta = primerMuestra.ValMuestraC != null ? (decimal)primerMuestra.ValMuestraC : 0;

            FechaConformacion = primerMuestra.FechaRecepcion != null ? (DateTime)primerMuestra.FechaRecepcion : DateTime.Now;
            fechaConformacionTexto = primerMuestra.FechaRecepcion == null ? "" : primerMuestra.FechaRecepcion.ToString();
            FechaConformacionMuestraCompuesta = primerMuestra.FechaConformacion == null ? "" : primerMuestra.FechaConformacion.ToString();

            var muestraCompuesta = new List<MuestraValor>();
            var compuesta = configuracion.Select(p => new MuestraValor
            {
                MuestraValorID = 0,
                MuestraID = 0,
                Parametro = p.Parametro,
                ParametroID = p.ParametroID,
                ParametroMetodoID = 0,
                LaboratorioID = 0,
                OperacionID = p.OperacionID,
                Exenta = p.Exenta,
                Evalua = p.Evalua,
                Exento = false,
                NoAnalizado = false,
                Valor = 0,
                ValorValidacion = null,
                AceptaValor = p.AceptaValor,
                LFD = p.LFD,
                LimiteInferior = p.LimiteInferior,
                LimiteSuperior = p.LimiteSuperior,
                LimiteInferiorLFD = p.LimiteInferiorLFD,
                LimiteSuperiorLFD = p.LimiteSuperiorLFD,
                LC_MC = 0,
                EscalaDecimal = p.EscalaDecimal,
                LCMC = p.LCMC,
                Truncamiento = p.Truncamiento
            });
            muestraCompuesta = compuesta.ToList();


            muestras.ForEach(p =>
            {
                foreach (var objMuestraValor in p.MuestraValors)
                {
                    var paramConfig =
                        muestraCompuesta.FirstOrDefault(q => q.ParametroID == objMuestraValor.ParametroID);
                    if (paramConfig != null)
                    {
                        objMuestraValor.OperacionID = paramConfig.OperacionID;
                        objMuestraValor.LCMC = paramConfig.LCMC;
                    }

                    objMuestraValor.Editable = true;
                    if (parametrosAnalisisPadre.Count > 0)
                    {
                        /// TODO: Consultar sí el análisis es un análisis hijo, en caso de que sí se debe proceder a
                        /// TODO: bloquear los controles que ya han sido capturados en el análisis padre
                        if (parametrosAnalisisPadre.Contains(objMuestraValor.ParametroID))
                            objMuestraValor.Editable = false;
                    }
                }
            });

            var columnas =
                agregaColumnasVirtuales(
                    ObtenerColumnas(muestras.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()).ToList()),
                    (int)OrdenTrabajo.NormaID);
            Columnas = columnas;

            var columnasCompuesta = ObtenerColumnasCompuesta(compuesta.ToList());
            ColumnasCompuesta = columnasCompuesta;



            #region RegionConductividad
            var muestra1 = muestras.FirstOrDefault();
            if (muestra1.Analisi.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")
            {
                var EcoliEnterococos = string.Empty;
                if (Session["SesionEcoliEnterococos"] != null && Session["SesionEcoliEnterococos"].ToString() != string.Empty)
                {
                    EcoliEnterococos = Session["SesionEcoliEnterococos"].ToString();
                }
                else
                {
                    EcoliEnterococos = muestra1.Analisi.Conductividad;
                }
                foreach (var item in muestras)
                {

                    if (EcoliEnterococos == "Escherichia")
                    {
                        var borrar = item.MuestraValors.FirstOrDefault(x => x.Parametro.ParametroID == 689);
                        var eliminada = item.MuestraValors.Remove(borrar);

                    }
                    else if (EcoliEnterococos == "Enterococos")
                    {
                        var borrar = item.MuestraValors.FirstOrDefault(x => x.Parametro.ParametroID == 694);
                        var eliminada = item.MuestraValors.Remove(borrar);
                    }
                }
            }
            #endregion
            rptMuestrasSimples.DataSource = muestras.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt());
            rptMuestrasSimples.DataBind();
            udpMuestrasSimples.Update();

        }

        protected void rptMuestrasSimples_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Header:
                    var rptHeader = (Repeater)e.Item.FindControl("rptHeader");
                    var thObservaciones = (HtmlTableCell)e.Item.FindControl("thObservaciones");
                    var thIndex = (HtmlTableCell)e.Item.FindControl("thIndex");
                    var thHoras = (HtmlTableCell)e.Item.FindControl("thHoras");

                    //TODO -> refactorizar para que tome en cuenta si requiere observaciones/hora.
                    if (OrdenTrabajo.Norma?.RequiereHora == false && OrdenTrabajo.Norma.RequiereObservaciones == false)
                    {
                        thIndex?.Attributes.Add("hidden", "true");
                    }


                    if (OrdenTrabajo.Norma?.RequiereHora == false)
                    {
                        thHoras?.Attributes.Add("hidden", "hidden");
                    }

                    if (OrdenTrabajo.Norma?.RequiereObservaciones == false)
                    {
                        thObservaciones?.Attributes.Add("hidden", "true");
                    }

                    if (rptHeader != null)
                    {
                        rptHeader.DataSource = Columnas;
                        rptHeader.DataBind();
                    }
                    break;
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    var ceFecha = (CalendarExtender)e.Item.FindControl("ceFecha");
                    var txtFecha = (TextBox)e.Item.FindControl("txtFecha");
                    var txtHora = (TextBox)e.Item.FindControl("txtHora");


                    var txtCaudal = (TextBox)e.Item.FindControl("txtCaudal");
                    var txtObservaciones = (TextBox)e.Item.FindControl("txtObservaciones");

                    var rptBody = (Repeater)e.Item.FindControl("rptBody");
                    var valores = (Muestra)e.Item.DataItem; //Aquí saca los valores de la muestra

                    var tdIndex = (HtmlTableCell)e.Item.FindControl("tdIndex");
                    var tdHoras = (HtmlTableCell)e.Item.FindControl("tdHoras");
                    var tdObservaciones = (HtmlTableCell)e.Item.FindControl("tdObservaciones");


                    if (OrdenTrabajo.Norma?.RequiereHora == false && OrdenTrabajo.Norma?.RequiereObservaciones == false)
                    {
                        tdIndex?.Attributes.Add("hidden", "true");
                    }

                    if (OrdenTrabajo.Norma?.RequiereHora == false)
                    {
                        tdHoras?.Attributes.Add("hidden", "hidden");
                    }

                    if (OrdenTrabajo.Norma?.RequiereObservaciones == false)
                    {
                        tdObservaciones?.Attributes.Add("hidden", "hidden");
                    }

                    if (this.OrdenTrabajo.Norma.Norma2 != null)
                    {
                        if (txtFecha != null) txtFecha.Enabled = false;
                        if (ceFecha != null) ceFecha.Enabled = false;
                        if (txtHora != null) txtHora.Enabled = false;
                        if (txtCaudal != null) txtCaudal.Enabled = false;
                        if (txtObservaciones != null) txtObservaciones.Enabled = false;
                    }

                    if (rptBody != null)
                    {
                        if (ceFecha != null)
                        {
                            ceFecha.StartDate = Vigencia.FechaInicio;
                            ceFecha.EndDate = Vigencia.FechaFin;
                        }


                        rptBody.ItemDataBound += RptBody_ItemDataBound;
                        rptBody.DataSource = agregarParametrosARptBody(valores.MuestraValors.ToList(), e, (int)OrdenTrabajo.NormaID);
                        rptBody.DataBind();
                    }

                    break;
                case ListItemType.Footer:



                    var rptLaboratorio = (Repeater)e.Item.FindControl("rptLaboratorio");
                    var parametros = Columnas;

                    var txtVolumen = (TextBox)e.Item.FindControl("txtVolumen");
                    var txtFechaConformacion = (TextBox)e.Item.FindControl("txtFechaConformacion");
                    var txtHoraConformacion = (TextBox)e.Item.FindControl("txtHoraConformacion");
                    var txtFechaConformacionMuestraCompuesta = (TextBox)e.Item.FindControl("txtFechaConformacionMuestraCompuesta");
                    var txtHoraConformacionMuestraCompueta = (TextBox)e.Item.FindControl("txtHoraConformacionMuestraCompuesta");




                    txtVolumen.Text = VolumenCompuesta.ToString();
                    txtFechaConformacion.Text = fechaConformacionTexto != "" ? FechaConformacion.ToString("dd/MM/yyyy") : "";
                    txtHoraConformacion.Text = fechaConformacionTexto != "" ? FechaConformacion.ToString("HH:mm") : "";
                    txtFechaConformacionMuestraCompuesta.Text = FechaConformacionMuestraCompuesta != ""
                        ? Convert.ToDateTime(FechaConformacionMuestraCompuesta).ToString("dd/MM/yyyy")
                        : "";
                    txtHoraConformacionMuestraCompueta.Text = FechaConformacionMuestraCompuesta != ""
                        ? Convert.ToDateTime(FechaConformacionMuestraCompuesta).ToString("HH:mm")
                        : "";

                    //Ocultar select, si no hay ningún parámetro seleccionable.
                    var trsSelects = e.Item.FindControl("trsSelects");
                    trsSelects.Visible = parametros.Any(parametro => parametro.ParametroID != null);

                    //Ocultar tr para capturar volumen de muestra simple y fecha/hora de conformación de la muestra.
                    var divDatos = e.Item.FindControl("divDatos");
                    divDatos.Visible = OrdenTrabajo.Norma.RequiereCompuesta;

                    if (rptLaboratorio != null)
                    {
                        rptLaboratorio.DataSource = parametros;
                        rptLaboratorio.DataBind();
                    }

                    //TODO 

                    var rptMetodos = (Repeater)e.Item.FindControl("rptMetodos");
                    if (rptMetodos != null)
                    {
                        rptMetodos.DataSource = parametros;
                        rptMetodos.DataBind();
                    }

                    var rptAccion = (Repeater)e.Item.FindControl("rptAccion");

                    if (rptAccion != null)
                    {
                        rptAccion.DataSource = parametros;
                        rptAccion.DataBind();
                        rptAccion.Visible = OrdenTrabajo.NormaID != 4;
                    }

                    var rptDatos = (Repeater)e.Item.FindControl("rptDatos");
                    if (rptDatos != null)
                    {
                        rptDatos.DataSource = parametros;
                        rptDatos.DataBind();
                    }

                    var trsCompuesta = (Control)e.Item.FindControl("trsCompuesta");
                    trsCompuesta.Visible = this.OrdenTrabajo.Norma.RequiereCompuesta;

                    List<Columnas> columnasLMP = new List<Columnas>();
                    if (this.OrdenTrabajo.Norma.RequiereCompuesta)
                    {
                        /* Oculta el boton de generar compuesta sí ya existe una compuesta o si no se requiere de la compuesta */
                        var rptHeaderCompuesta = (Repeater)e.Item.FindControl("rptHeaderCompuesta");
                        var rptCamposCompuesta = (Repeater)e.Item.FindControl("rptCamposCompuesta");
                        var chkValidaHoja = (CheckBox)e.Item.FindControl("chkValidaHoja");
                        chkValidaHoja.Checked = false;
                        chkValidaHoja.Visible = true;

                        var variablePrueba = agregarVirtualesARtpHeaderCompuesto(ColumnasCompuesta, Columnas);
                        columnasLMP = agregaEscalaDecimal(variablePrueba);
                        if (rptHeaderCompuesta != null)
                        {
                            rptHeaderCompuesta.DataSource = variablePrueba;
                            rptHeaderCompuesta.DataBind();
                        }

                        if (rptCamposCompuesta != null)
                        {
                            rptCamposCompuesta.DataSource = variablePrueba;
                            rptCamposCompuesta.DataBind();
                        }

                    }

                    var divLMP = (Control)e.Item.FindControl("divLMP");
                    divLMP.Visible = OrdenTrabajo.Norma.CPD;

                    //TODO refactorizar para tomar en cuenta si requiere LMP ->
                    if (OrdenTrabajo.Norma.CPD)
                    {
                        var rptLimiteMaximoPermisible = (Repeater)e.Item.FindControl("rptLimiteMaximoPermisible");
                        if (rptLimiteMaximoPermisible != null)
                        {
                            rptLimiteMaximoPermisible.DataSource = columnasLMP;
                            rptLimiteMaximoPermisible.DataBind();
                        }
                    }
                    break;
            }
        }

        private void RptBody_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var dataItem = (MuestraValor)e.Item.DataItem;
                var txtValue = (TextBox)e.Item.FindControl("txtValue");
                var ddlValue = (DropDownList)e.Item.FindControl("ddlValue");
                var txtLcmc = (TextBox)e.Item.FindControl("txtLcmc");

                var muestraParticipacion = e.Item.FindControl("muestraParticipacion");
                muestraParticipacion.Visible = false;
                var muestraVolumen = e.Item.FindControl("muestraVolumen");
                muestraVolumen.Visible = false;
                var muestraParametro = e.Item.FindControl("muestraParametro");
                muestraParametro.Visible = true;

                if (txtValue != null)
                {
                    txtValue.Enabled = dataItem.Editable;
                    txtValue.Attributes.Add("data-editable", dataItem.Editable.ToString());
                }

                if (ddlValue != null)
                {
                    ddlValue.Enabled = dataItem.Editable;
                    ddlValue.Attributes.Add("data-editable", dataItem.Editable.ToString());
                }

                if (txtLcmc != null)
                {
                    txtLcmc.Enabled = dataItem.Editable;
                    txtLcmc.Attributes.Add("data-editable", dataItem.Editable.ToString());
                }

                if (dataItem.requiereCaudal != null)
                {
                    muestraParametro.Visible = false;
                }

                if (dataItem.requiereCompuesta != null)
                {
                    muestraParticipacion.Visible = true;
                    muestraVolumen.Visible = true;
                    muestraParametro.Visible = false;
                }

                if (txtValue != null && dataItem.AceptaValor && (dataItem.LimitaCaptura ?? false))
                {
                    if (dataItem.MinAceptado != null)
                    {
                        txtValue.Attributes.Add("data-minaceptado", dataItem.MinAceptado.ToString());
                    }

                    if (dataItem.MaxAceptado != null)
                    {
                        txtValue.Attributes.Add("data-maxaceptado", dataItem.MaxAceptado.ToString());
                    }
                }
            }
        }

        protected void rptLaboratorio_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    var ddlLaboratorio = (DropDownList)e.Item.FindControl("ddlLaboratorio");
                    if (ddlLaboratorio != null)
                    {
                        var parametroID = ddlLaboratorio.Attributes["data-parameter-id"] == ""
                            ? 0
                            : int.Parse(ddlLaboratorio.Attributes["data-parameter-id"]);
                        var laboratorioID = ddlLaboratorio.Attributes["data-laboratorio-id"];

                        ddlLaboratorio.LoadDrop("Nombre", "LaboratorioID",
                            _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, parametroID));
                        ddlLaboratorio.ClearSelection();

                        if (!(laboratorioID != "" && laboratorioID != "0"))
                        {
                            laboratorioID = Login.LaboratorioID != null ? Login.LaboratorioID.Value.ToString() : "0";
                        }

                        var ddlItem = ddlLaboratorio.Items.FindByValue(laboratorioID);
                        if (ddlItem != null)
                            ddlLaboratorio.Items.FindByValue(laboratorioID).Selected = true;
                    }

                    break;

                case ListItemType.Footer:
                    if (OrdenTrabajo.Norma?.RequiereObservaciones == false)
                    {
                        var tdLaboratorios = (HtmlTableCell)e.Item.FindControl("tdLaboratorios");
                        tdLaboratorios?.Attributes.Add("hidden", "hidden");
                    }
                    break;

            }
        }

        protected void rptMetodos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    var ddlMetodo = (DropDownList)e.Item.FindControl("ddlMetodo");
                    if (ddlMetodo != null)
                    {
                        var parametroID = ddlMetodo.Attributes["data-parameter-id"] == ""
                            ? 0
                            : int.Parse(ddlMetodo.Attributes["data-parameter-id"]);
                        var laboratorioID = ddlMetodo.Attributes["data-laboratorio-id"];

                        if (laboratorioID == "" || laboratorioID == "0")
                            laboratorioID = Login.LaboratorioID != null ? Login.LaboratorioID.Value.ToString() : "0";

                        var metodoID = ddlMetodo.Attributes["data-metodo-id"];

                        var lista = _metBl.Obten_Metodo(int.Parse(laboratorioID), parametroID);


                        ddlMetodo.LoadDrop("Nombre", "MetodoID",
                            lista);
                        ddlMetodo.ClearSelection();

                        if (metodoID != "" && metodoID != "0")
                        {
                            var ddlItem = ddlMetodo.Items.FindByValue(metodoID);
                            if (ddlItem != null)
                                ddlMetodo.Items.FindByValue(metodoID).Selected = true;
                        }
                        else
                        {
                            if (ddlMetodo.Items.Count == 2)
                            {
                                ddlMetodo.Items[1].Selected = true;
                            }
                        }
                    }

                    break;
                case ListItemType.Footer:
                    if (OrdenTrabajo.Norma.RequiereObservaciones == false)
                    {
                        var tdMetodos = (HtmlTableCell)e.Item.FindControl("tdMetodos");
                        tdMetodos?.Attributes.Add("hidden", "hidden");
                    }
                    break;
            }
        }

        protected void rptAccion_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Footer:
                    if (OrdenTrabajo.Norma.RequiereObservaciones == false)
                    {
                        var tdAcciones = (HtmlTableCell)e.Item.FindControl("tdAcciones");
                        tdAcciones?.Attributes.Add("hidden", "hidden");
                    }
                    break;
            }
        }

        protected void rptDatos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        //RFD 03/07/2020 se modifica el método para obtener el nombre del campo etiqueta de la tabla norma configuración.
        private List<Columnas> ObtenerColumnas(List<Muestra> muestras)
        {
            List<Columnas> columnas = new List<Columnas>();
            var muestra = muestras.FirstOrDefault();
            #region RegionConductividad
            if (muestra.Analisi.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")
            {
                var EcoliEnterococos = string.Empty;
                if (Session["SesionEcoliEnterococos"] != null && Session["SesionEcoliEnterococos"].ToString() != string.Empty)
                {
                    EcoliEnterococos = Session["SesionEcoliEnterococos"].ToString();
                }
                else
                {
                    EcoliEnterococos = muestra.Analisi.Conductividad;
                }
                if (EcoliEnterococos == "Escherichia")
                {
                    var borrar = muestra.MuestraValors.FirstOrDefault(x => x.Parametro.ParametroID == 689);
                    var eliminada = muestra.MuestraValors.Remove(borrar);

                }
                else if (EcoliEnterococos == "Enterococos")
                {
                    var borrar = muestra.MuestraValors.FirstOrDefault(x => x.Parametro.ParametroID == 694);
                    var eliminada = muestra.MuestraValors.Remove(borrar);
                }
            }
            #endregion
            foreach (var parametro in muestra.MuestraValors)
            {
                var accion = parametro.Exento ? 1 : parametro.NoAnalizado ? 2 : 0;

                columnas.Add(new Columnas
                {
                    ParametroID = parametro.ParametroID,
                    Nombre = OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(nc => nc.ParametroID == parametro.ParametroID)?.Etiqueta,
                    LaboratorioID = parametro.LaboratorioID,
                    MetodoID = parametro.ParametroMetodo?.MetodoID,
                    Accion = accion,
                    RequiereLC_MC = parametro.LCMC ?? false,
                    OperacionID = parametro.OperacionID,

                });



                if (parametro.LCMC ?? false)
                    columnas.Add(new Columnas
                    { Nombre = "LC/MC" }); //RFD 08/06/2020 se cambia el nombre de la columna por petición del usuario.

                //RFD 09/06/2020 Se modifica el orden en el que se inserta la columna Promedio ponderado
                if (parametro.OperacionID != null)
                {
                    var operacionHeader = "";
                    switch (parametro.OperacionID)
                    {
                        case 3:
                            //Se harcodea el id del parámetro Grasas y Aceites:
                            operacionHeader = parametro.Parametro.ParametroID == OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(nc => nc.Parametro.Nombre.Contains("Grasas y Aceites ") && nc.Activo)?.Parametro.ParametroID ? "Valor ponderado G&A" : "Valor Ponderado";
                            columnas.Add(new Columnas { Nombre = operacionHeader });
                            break;
                    }
                }
            }

            return columnas;
        }

        //RFD 03/07/2020 se modifica el método para obtener el nombre del campo etiqueta de la tabla norma configuración.
        private List<Columnas> ObtenerColumnasCompuesta(List<MuestraValor> muestras)
        {
            List<Columnas> columnas = new List<Columnas>();
            foreach (var parametro in muestras)
            {
                var accion = parametro.Exento ? 1 : parametro.NoAnalizado ? 2 : 0;
                if (parametro.OperacionID != null)
                {
                    var operacionHeader = "";
                    switch (parametro.OperacionID)
                    {
                        case 1:
                            operacionHeader = "Sumatoria";
                            break;
                        case 2:
                            operacionHeader = "Promedio aritmético";
                            break;
                        case 3:
                            operacionHeader = "Promedio ponderado";
                            break;
                        case 4:
                            operacionHeader = "Media geométrica";
                            break;
                    }

                    columnas.Add(new Columnas
                    {
                        ParametroID = parametro.ParametroID,
                        Nombre = operacionHeader,
                        LaboratorioID = parametro.LaboratorioID,
                        OperacionID = parametro.OperacionID,
                        Accion = accion,
                        EscalaDecimal = parametro.EscalaDecimal,
                        RequiereCompuesta = true,
                        Truncamiento = parametro.Truncamiento
                    });
                }
                else
                {
                    columnas.Add(new Columnas
                    {
                        EscalaDecimal = parametro.EscalaDecimal
                    });
                }

            }

            return columnas;
        }





        //Métodos auxiliares para ordenar la tabla.
        //RFD 10/06/2020 genera los valores que se cargan en el encabezado de la tabla.
        private List<Columnas> agregaColumnasVirtuales(List<Columnas> columnas, int normaId)
        {
            Norma_BL normaBL = new Norma_BL();
            SIRALAB_DAL.Norma norma = normaBL.Obtener((int)normaId);

            //Crear parámetros virtuales.
            var caudal = new Columnas() { Nombre = "Caudal (l/s)", RequiereSelect = true, RequiereCaudal = true };
            var participacionCaudal = new Columnas() { Nombre = "Participación del Caudal", RequiereSelect = true, RequiereParticipacion = true };
            var volMuestraSimple = new Columnas() { Nombre = "Volumen de Muestra Simple (l)", RequiereSelect = true, RequiereVolumen = true };

            var columnasConVirtuales = new List<Columnas>();

            if (columnas.Any())
            {

                var desplazamiento = 0;
                for (var i = 0; i < columnas.Count;)
                {
                    if (i == 0 && norma.RequiereCaudal) //Agrega caudal a la lista. Siempre es el primer elemento de la lista.
                        columnasConVirtuales.Add(caudal);



                    if (norma.RequiereCompuesta && norma.Desplazamiento != null && desplazamiento == norma.Desplazamiento) //Agrega participación del caudal y volumen de muestra simple
                    {
                        columnasConVirtuales.Add(participacionCaudal);
                        columnasConVirtuales.Add(volMuestraSimple);
                    }

                    columnasConVirtuales.Add(columnas[i]);

                    if (columnas[i].RequiereLC_MC)
                    {
                        columnasConVirtuales.Add(columnas[i + 1]);
                        i++;

                    }

                    i++;
                    desplazamiento++;
                }

                //Si requiere compuesta y no se han agregado.
                if (norma.RequiereCompuesta && !columnasConVirtuales.Any(col => col.RequiereParticipacion == true))
                {
                    columnasConVirtuales.Add(participacionCaudal);
                    columnasConVirtuales.Add(volMuestraSimple);
                }

            }
            else
            {
                if (norma.RequiereCaudal)
                {
                    columnasConVirtuales.Add(caudal);

                    if (norma.RequiereCompuesta)
                    {
                        columnasConVirtuales.Add(participacionCaudal);
                        columnasConVirtuales.Add(volMuestraSimple);
                    }
                }
            }

            return columnasConVirtuales;
        }

        //RFD 10/06/2020 agrega y ordena los valores que se cargan en la tabla.
        private List<MuestraValor> agregarParametrosARptBody(List<MuestraValor> muestraValores, RepeaterItemEventArgs e,
            int normaId)
        {
            Norma_BL normaBL = new Norma_BL();
            SIRALAB_DAL.Norma norma = normaBL.Obtener((int)normaId);

            var tamanhoLista = muestraValores.Count();
            norma.Desplazamiento = norma.Desplazamiento >= tamanhoLista
                ? tamanhoLista
                : norma.Desplazamiento;


            var parametrosOrdenados = new List<MuestraValor>();

            if (muestraValores.Any())
            {
                for (var i = 0; i < tamanhoLista; i++)
                {


                    Muestra muestra = muestraValores[i].Muestra;

                    if (i == 0 && norma.RequiereCaudal)
                    {
                        parametrosOrdenados.Add(new MuestraValor()
                        {
                            caudal = (float?)muestra.Caudal,
                            requiereCaudal = true
                        });
                    }

                    if (norma.RequiereCompuesta && norma.Desplazamiento != null && i == norma.Desplazamiento
                    ) //Agrega participación del caudal y volumen de muestra simple
                    {
                        parametrosOrdenados.Add(new MuestraValor()
                        {
                            requiereCompuesta = true
                        });
                    }

                    parametrosOrdenados.Add(muestraValores[i]);

                }

                //Si requiere compuesta y no se ha agregado.
                if (norma.RequiereCompuesta && !parametrosOrdenados.Any(par => par.requiereCompuesta == true))
                {
                    parametrosOrdenados.Add(new MuestraValor()
                    {
                        requiereCompuesta = true
                    });
                }
            }
            else
            {
                if (norma.RequiereCaudal)
                {
                    var muestra = (Muestra)e.Item.DataItem;

                    parametrosOrdenados.Add(new MuestraValor()
                    {
                        caudal = (float)muestra.Caudal,
                        requiereCaudal = true
                    });

                    if (norma.RequiereCompuesta)
                    {
                        parametrosOrdenados.Add(new MuestraValor()
                        {
                            requiereCompuesta = true
                        });
                    }
                }
            }

            return parametrosOrdenados;
        }

        private List<Columnas> agregarVirtualesARtpHeaderCompuesto(List<Columnas> muestraValores,
            List<Columnas> columnas)
        {
            var listaOrdenada = new List<Columnas>();
            foreach (var columna in columnas)
            {
                var muestraTmp = muestraValores.FirstOrDefault(mv => mv.ParametroID == columna.ParametroID);
                if (muestraTmp != null)
                    muestraTmp.RequiereParametro = true;

                listaOrdenada.Add(muestraTmp ?? columna);
            }


            //Lo itero otra vez para evitar problemas con la variable global Columnas (por inmutabilidad).
            foreach (var elemento in listaOrdenada)
            {
                if (elemento.Nombre.Equals("Caudal (l/s)"))
                {
                    elemento.Nombre = "Promedio Aritmético";
                }
                else if (elemento.Nombre.Equals("Participación del Caudal"))
                {
                    elemento.Nombre = "Total (Qi/Qt)";
                }
                else if (elemento.Nombre == "Volumen de Muestra Simple (l)")
                {
                    elemento.Nombre = "Total MS";
                }
                else if (elemento.ParametroID == null || elemento.RequiereCompuesta == null)
                {
                    elemento.Nombre = "";
                }


                elemento.muestraTotalNoRequerido = elemento.RequiereCaudal == null &&
                                                   elemento.RequiereParticipacion == null &&
                                                   elemento.RequiereVolumen == null &&
                                                   elemento.RequiereParametro == null;

            }


            return ordenaEncabezadoResultadosCompuesta(listaOrdenada);
        }

        private List<Columnas> ordenaEncabezadoResultadosCompuesta(List<Columnas> columnas)
        {
            for (var i = 0; i < columnas.Count; i++)
            {
                if (columnas[i].ParametroID != null && columnas[i].ParametroID > 0 && columnas[i].OperacionID != null &&
                    columnas[i].OperacionID == 3)
                {
                    var encabezadoParametro = columnas[i];
                    if (OrdenTrabajo != null && OrdenTrabajo.Norma.NormaConfiguracions
                        .Any(nc => nc.ParametroID == columnas[i].ParametroID && nc.LCMC != null && nc.LCMC == true))
                    {

                        var encabezadoPromedioPar = columnas[i + 2];

                        columnas[i] = encabezadoPromedioPar;
                        columnas[i + 2] = encabezadoParametro;
                        i += 2;
                    }
                    else
                    {
                        var encabezadoPromedioPar = columnas[i + 1];

                        columnas[i] = encabezadoPromedioPar;
                        columnas[i + 1] = encabezadoParametro;
                        i += 1;
                    }

                }
            }
            return columnas;
        }

        private List<Columnas> agregaEscalaDecimal(List<Columnas> columnas)
        {
            var _muestraBl = new Muestra_BL();
            var muestras = _muestraBl.Lista_Muestra(AnalisisID);
            var muestra = muestras.FirstOrDefault(m => m.NivelID == enumNivel.Simple.ConvertToInt());
            foreach (var elemento in columnas)
            {
                if (elemento.ParametroID != null)
                {
                    elemento.EscalaDecimal = OrdenTrabajo.Norma.NormaConfiguracions
                        .FirstOrDefault(nc => elemento != null && nc.ParametroID == elemento.ParametroID).EscalaDecimal;

                    elemento.capturaLMP = OrdenTrabajo.Norma.NormaConfiguracions
                        .FirstOrDefault(nc => nc.ParametroID == elemento.ParametroID)?.AceptaValor;

                    if (muestra != null)
                    {
                        var limiteSuperior = muestra.MuestraValors.FirstOrDefault(mv => mv.ParametroID == elemento.ParametroID)?.LimiteSuperior;
                        elemento.limiteSuperior = limiteSuperior != null && limiteSuperior > 0 ? limiteSuperior : 0;
                    }

                    if (elemento.ParametroID == OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(nc => nc.Parametro.Nombre.Contains("pH") && nc.Parametro.Activo)?.Parametro?.ParametroID)//sólo se captura el limite inferior cuando el parámetro es ph:
                    {
                        elemento.requiereLimiteInferior = true;
                        var limiteInferior = muestra.MuestraValors.FirstOrDefault(mv => mv.ParametroID == elemento.ParametroID)?.LimiteInferior;
                        elemento.limiteInferior = limiteInferior != null && limiteInferior > 0 ? limiteInferior : 0;

                    }
                }
            }
            return columnas;
        }
        #region RegionConductividad
        public void ChkEcoli(bool check)
        {
            Session["SesionEcoliEnterococos"] = "Escherichia";
        }

        public void ChkEnterococos(bool check)
        {
            Session["SesionEcoliEnterococos"] = "Enterococos";
        }
        #endregion
    }
}
