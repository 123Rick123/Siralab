using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.common;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_BL.catalogos;
using System.ComponentModel;
using OfficeOpenXml.Drawing.Chart;
using SIRALAB_BL.negocio.ordentrabajo;

namespace SIRALAB.Controles
{
    public partial class wucOrdenTrabajo : System.Web.UI.UserControl
    {

        public bool InscritoNom001
        {
            get { return (bool)Session["InscritoNom001"]; }
            set { Session["InscritoNom001"] = value; }
        }
        #region Propiedades
        public Anexo Anexo
        {
            get { return (SIRALAB_DAL.Anexo)Session["AnexoConsulta_"]; }
        }
        public Cliente Cliente
        {
            get { return (SIRALAB_DAL.Cliente)Session["ClienteConsulta_"]; }
        }
        public bool ContieneAnexo
        {
            get { return (bool)Session["ContieneAnexo_"]; }
        }
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        public LaboratorioVigencia Vigencia
        {
            get { return Session["LaboratorioVigencia"] == null ? null : (LaboratorioVigencia)Session["LaboratorioVigencia"]; }
        }
        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_"]; }
            set { Session["OrdenTrabajoConsulta_"] = value; }
        }
        public int OrdenTrabajoId
        {
            get { return hfOrdenTrabajo.Value.Trim() == "" ? 0 : int.Parse(hfOrdenTrabajo.Value); }
            set { hfOrdenTrabajo.Value = value.ToString(); }
        }
        private bool? EsFuenteAbastecimiento
        {
            get { return Session["EsFuenteAbastecimiento"] == null ? null : (bool?)Session["EsFuenteAbastecimiento"]; }
            set { Session["EsFuenteAbastecimiento"] = value; }
        }
        private int? AnexoMedidorID
        {
            get { return Session["AnexoMedidorID"] == null ? null : (int?)Session["AnexoMedidorID"]; }
            set { Session["AnexoMedidorID"] = value; }
        }
        private int? CuerpoReceptorID
        {
            get { return Session["CuerpoReceptorID"] == null ? null : (int?)Session["CuerpoReceptorID"]; }
            set { Session["CuerpoReceptorID"] = value; }
        }
        private int? UsoAguaID
        {
            get { return Session["AnexoMedidorID"] == null ? null : (int?)Session["AnexoMedidorID"]; }
            set { Session["AnexoMedidorID"] = value; }
        }
        #endregion

        public event EventHandler RespuestaOrdenTrabajo;
        [Browsable(true)]
        public event EventHandler<EventArgs> Incompleto;
        [Browsable(true)]
        public event EventHandler<EventArgs> FechaErronea;
        [Browsable(true)]
        public event EventHandler<bool> RespuestaAnalisis;
        [Browsable(true)]
        public event EventHandler<NormaSeleccionadaArgs> NormaSeleccionada;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime end = DateTime.Now;
                DateTime? init = null;
                if (Vigencia != null)
                {
                    init = Vigencia.FechaInicio;

                    if (DateTime.Now < Vigencia.FechaFin)
                        end = DateTime.Now;
                    else
                        end = Vigencia.FechaFin.Value;
                }
                startTxtOrdenFechaEmision.Value = init.HasValue ? init.Value.ToShortDateString() : string.Empty;
                endTxtOrdenFechaEmision.Value = end.ToShortDateString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CargaNormas(bool flag, Int64? cuerpoReceptorID = null, Int64? usoAguaID = null)
        {
            ClearCargaNormas();

            if (ddlNorma.Enabled)
            {
                if (flag)
                {
                    Norma_BL bl = new Norma_BL();

                    ddlNorma.Items.Clear();
                    var normas = bl.ObtenerTodosFuenteAbastecimiento();

                    ddlNorma.LoadDrop("Nombre", "NormaID", normas);
                }
                else
                {
                    LoadDrops.instance.NormaCaptura(ddlNorma);
                }
                Correspondencia_CR_UA_BL correspondencia = new Correspondencia_CR_UA_BL();
                this.UsoAguaID = correspondencia.ObtenerUsoAguaID(cuerpoReceptorID, usoAguaID);
                this.CuerpoReceptorID = correspondencia.ObtenerCuerpoReceptorID(cuerpoReceptorID, usoAguaID);
                //ListItem li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
                //ddlUsoAgua.Items.Insert(0, li);
                //ddlCuerpoReceptor.Items.Insert(0, li);
            }
            else if (!IsPostBack)
            {
                LoadDrops.instance.Norma(ddlNorma);
                ListItem li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
                ddlUsoAgua.Items.Insert(0, li);

                ddlCuerpoReceptor.Items.Insert(0, li);
            }

            //Seleccion Automatica
            if (ddlNorma.Items.Count == 2)
            {
                ddlNorma.SelectedIndex = 1;
                ddlNorma_SelectedIndexChanged(null, null);
            }
            else
            {
                divHorasDescarga.Attributes.Add("hidden", "true");
            }

            //Ocultar divs
            divUsoAgua.Attributes.Add("hidden", "true");
            divCuerpoReceptor.Attributes.Add("hidden", "true");

        }


        public void ClearCargaNormas()
        {
            /*Limpia Combos*/
            if (ddlNorma.Enabled)
            {
                ddlNorma.Items.Clear();
                ddlNorma.DataBind();
            }

            if (ddlCuerpoReceptor.Enabled)
            {
                ddlCuerpoReceptor.Items.Clear();
                ddlCuerpoReceptor.DataBind();
            }

            if (ddlUsoAgua.Enabled)
            {
                ddlUsoAgua.Items.Clear();
                ddlUsoAgua.DataBind();
            }
        }

        protected void lnkbtnGenerarAnalisis_Click(object sender, EventArgs e)
        {
            GeneraAnalisis();
        }

        protected void lnkbtnOrdenGuardar_Click(object sender, EventArgs e)
        {
            if (this.ContieneAnexo)
            {
                if (ValidarFecha(txtOrdenFechaEmision.Text))
                {
                    FechaErronea(this, null);
                }
                else
                {
                    if (!validaNom())
                    {
                        Incompleto(this, null);
                    }
                    else
                    {
                        GuardarAnalisis();
                    }
                }
            }
            else
            {
                Incompleto(this, null);
            }
        }

        public bool ValidarFecha(string FechaXValidarCadena)
        {
            bool Validacion = true;
            DateTime FechaActual = DateTime.Now;
            DateTime FechaXValidar = DateTime.Now;
            if (DateTime.TryParse(FechaXValidarCadena, out FechaXValidar))
            {
                if (FechaXValidar.Date <= FechaActual.Date && FechaXValidar.Year >= 2014)
                {
                    Validacion = false;
                }
            }
            return Validacion;
        }


        private bool validaNom()
        {
            var id = int.Parse(ddlNorma.SelectedValue);

            var normaBL = new Norma_BL();
            var norma = normaBL.Obtener(id);


            if (id == 0 || string.IsNullOrEmpty(txtOrdenNumero.Text) || (string.IsNullOrEmpty(txtOrdenProcesoHoras.Text) && norma.RequiereHora == true))
                return false;


            switch (id)
            {
                case 1: //NOM-001
                    return ddlCuerpoReceptor.SelectedIndex != 0 && (int.Parse(ddlCuerpoReceptor.SelectedValue) == 6 || ddlUsoAgua.SelectedIndex != 0);
                case 2: //NOM-003
                    return ddlUsoAgua.SelectedIndex != 0;
                default: //En el resto de los casos no se seleccionan los ddl.
                    return true;
            }
        }


        protected void ddlNorma_SelectedIndexChanged(object sender, EventArgs e)
        {
            NormaConfiguracion_BL normaConfiguracion_BL = new NormaConfiguracion_BL();

            Norma_BL nBL = new Norma_BL();
            var NormaId = int.Parse(ddlNorma.SelectedValue);
            var Nom = nBL.Obtener(NormaId);
            //Descomentar if else cuando se aplique el candado para las normas RGG
            //if ((InscritoNom001 == true && Nom.Nom2021 == true) || (InscritoNom001 == false && Nom.Nom2021 == false) || (InscritoNom001 == true && Nom.Nom2021 == false))
            //{
            //TODO -> verificar funcionamiento.
            divCuerpoReceptor.Attributes.Remove("hidden");
                divUsoAgua.Attributes.Remove("hidden");
                divHorasDescarga.Attributes.Remove("hidden");

            //Descomentar cuando se aplique el candado para las normas RGG
                //divUsoAgua.Visible = true;
                //divHorasDescarga.Visible = true;
                //divCuerpoReceptor.Visible = true;
                if (Nom != null)
                {
                    if (!Nom.RequiereCaudal)
                    {
                        txtOrdenProcesoHoras.Text = "";
                        txtOrdenProcesoHoras.Enabled = false;
                        rfv_txtOrdenProcesoHoras.Enabled = false;
                        divHorasDescarga.Attributes.Add("hidden", "true");
                    }
                    else
                    {
                        txtOrdenProcesoHoras.Text = "";
                        txtOrdenProcesoHoras.Enabled = true;
                        rfv_txtOrdenProcesoHoras.Enabled = true;
                    }

                    var ListaCuerpo = normaConfiguracion_BL.Lista_CuerpoReceptor(NormaId).OrderBy(p => p.Nombre).ToList();
                    if (ListaCuerpo.Count > 0)
                    {
                        ddlCuerpoReceptor.LoadDrop("Nombre", "CuerpoReceptorID", ListaCuerpo);
                        ddlCuerpoReceptor.Visible = true;

                        ddlUsoAgua.Visible = true;
                        lblCReceptor.Visible = true;
                        lblUAgua.Visible = true;

                        rfv_ddlCuerpoReceptor.Enabled = true;
                        rfv_ddlUsoAgua.Enabled = true;

                        //Seleccion Automatica
                        if (ddlCuerpoReceptor.Items.Count == 2)
                        {
                            ddlCuerpoReceptor.SelectedIndex = 1;
                            ddlCuerpoReceptor_SelectedIndexChanged(null, null);

                            if (ddlCuerpoReceptor.Items[1].ToString() == "N/A")
                            {
                                ddlCuerpoReceptor.Enabled = false;
                                divCuerpoReceptor.Attributes.Add("hidden", "true");
                                if (Nom.NormaID != 2) //Es la NOM-003 (es la única que tiene cuerpo receptor 5 usos de agua)
                                    divUsoAgua.Attributes.Add("hidden", "true");
                            }

                            // para verificar  que n/a se  inhabilite
                        }
                        else
                        {
                            ddlCuerpoReceptor.Enabled = true;

                            //Seleccion Automatica
                            var itemCuerpoReceptor = ddlCuerpoReceptor.Items.FindByValue(this.CuerpoReceptorID.ToString());
                            if (itemCuerpoReceptor != null)
                            {
                                ddlCuerpoReceptor.ClearSelection();
                                ddlCuerpoReceptor.Items.FindByValue(this.CuerpoReceptorID.ToString()).Selected = true;
                                ddlCuerpoReceptor_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                    else
                    {
                        ddlCuerpoReceptor.Items.Clear();
                        ddlCuerpoReceptor.DataBind();
                        ddlCuerpoReceptor.Visible = false;
                        ddlUsoAgua.Items.Clear();
                        ddlUsoAgua.DataBind();
                        ddlUsoAgua.Visible = false;

                        lblCReceptor.Visible = false;
                        lblUAgua.Visible = false;

                        rfv_ddlCuerpoReceptor.Enabled = false;
                        rfv_ddlUsoAgua.Enabled = false;
                    }

                    // Vamos a notificar que la norma ha sido seleccionada mediante el evento NormaSeleccionada
                    NormaSeleccionada?.Invoke(this, new NormaSeleccionadaArgs(NormaId, ListaCuerpo));
                }

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeRestriccionNom001').modal();", true);
            //    ddlNorma.SelectedValue = "0";
            //    ddlUsoAgua.SelectedValue = "0";
            //    txtOrdenNumero.Text = string.Empty;
            //    txtOrdenNumero.Text = string.Empty;
            //    txtOrdenFechaEmision.Text = string.Empty;
            //    divUsoAgua.Visible = false;
            //    divHorasDescarga.Visible = false;
            //    divCuerpoReceptor.Visible = false;

            //}
        }

        protected void ddlCuerpoReceptor_SelectedIndexChanged(object sender, EventArgs e)
        {
            NormaConfiguracion_BL normaConfiguracion_BL = new NormaConfiguracion_BL();
            var ListaUsoAgua = normaConfiguracion_BL.Lista_UsoAgua(int.Parse(ddlNorma.SelectedValue), int.Parse(ddlCuerpoReceptor.SelectedValue)).OrderBy(p => p.Nombre).ToList();
            ddlUsoAgua.ClearSelection();
            ddlUsoAgua.LoadDrop("Nombre", "UsoAguaID", ListaUsoAgua);
            divUsoAgua.Attributes.Remove("hidden");

            //Seleccion Automatica
            if (ddlUsoAgua.Items.Count == 2)
            {
                ddlUsoAgua.SelectedIndex = 1;
                if (ddlUsoAgua.Items[1].ToString() == "N/A")
                {
                    ddlUsoAgua.Enabled = false;
                    divUsoAgua.Attributes.Add("hidden", "true");
                }
            }
            else
                ddlUsoAgua.Enabled = true;

            //Seleccion Automatica
            var itemUsoAgua = ddlUsoAgua.Items.FindByValue(this.UsoAguaID.ToString());
            if (itemUsoAgua != null)
            {
                ddlUsoAgua.ClearSelection();
                ddlUsoAgua.Items.FindByValue(this.UsoAguaID.ToString()).Selected = true;
            }
        }

        public void InabilitaPanel()
        {
            pnlDatosOrden.Enabled = false;
            lnkbtnOrdenGuardar.Visible = false;
        }

        public void GuardarAnalisis()
        {
            if (this.Cliente != null && this.Anexo != null)
            {
                var ordentrabajoBl = new OrdenTrabajo_BL();
                var ordentrabajo = new OrdenTrabajoCapturaArgs();

                var nBl = new Norma_BL();
                var nom = nBl.Obtener(int.Parse(ddlNorma.SelectedValue));

                ordentrabajo.ClienteID = this.Cliente.ClienteID;
                ordentrabajo.NormaID = int.Parse(ddlNorma.SelectedValue);
                ordentrabajo.EstatusID = 1;
                try
                {
                    ordentrabajo.FechaEmision = DateTime.ParseExact(txtOrdenFechaEmision.Text, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {

                }
                ordentrabajo.NumeroInforme = txtOrdenNumero.Text;
                ordentrabajo.Folio = "";
                ordentrabajo.Anio = DateTime.Now.Year;
                ordentrabajo.Consecutivo = 0;
                ordentrabajo.HorasProceso = nom.RequiereCaudal ? int.Parse(txtOrdenProcesoHoras.Text) : 25;
                ordentrabajo.LaboratorioID = Login.LaboratorioID;
                ordentrabajo.AnexoID = Anexo.AnexoID;
                ordentrabajo.ConsecionID = Anexo.ConsecionID;

                if (ddlCuerpoReceptor.Visible)
                    ordentrabajo.CuerpoReceptorID = int.Parse(ddlCuerpoReceptor.SelectedValue);
                else
                    ordentrabajo.CuerpoReceptorID = null;
                if (ddlUsoAgua.Visible)
                    ordentrabajo.UsoAguaID = int.Parse(ddlUsoAgua.SelectedValue) == 0 ? 10 : int.Parse(ddlUsoAgua.SelectedValue);
                else
                    ordentrabajo.UsoAguaID = null;

                var fm = ordentrabajoBl.Calcula_FrecuenciaMuestreo(ordentrabajo.HorasProceso);

                ordentrabajo.FrecuenciaMuestreoID = fm.FrecuenciaMuestreoID;


                /*FuenteAbastecimiento*/
                if (EsFuenteAbastecimiento ?? false)
                {

                    ordentrabajo.FuenteAbastecimiento = EsFuenteAbastecimiento;
                    ordentrabajo.AnexoMedidorID = AnexoMedidorID;
                }
                else
                {
                    ordentrabajo.FuenteAbastecimiento = false;
                    ordentrabajo.AnexoMedidorID = null;
                }

                ordentrabajoBl.Guarda(ordentrabajo);

                this.OrdenTrabajo = ordentrabajoBl.OrdenTrabajo;

                if (OrdenTrabajo != null)
                {
                    OrdenTrabajoId = OrdenTrabajo.OrdenTrabajoID;

                    var ordenArgs = new RespuestaOrdenTrabajoArgs { OrdenTrabajoId = OrdenTrabajoId };

                    pnlBoton.Enabled = true;
                    pnlBoton.Visible = true;

                    RespuestaOrdenTrabajo(this, ordenArgs);
                }
            }
            else
                RespuestaOrdenTrabajo(this, null);
        }

        public void GeneraAnalisis()
        {
            AnalisisCapturaArgs analisiscaptura = new AnalisisCapturaArgs();
            analisiscaptura.AnalisisPadreID = null;
            analisiscaptura.OrdenTrabajoID = this.OrdenTrabajo.OrdenTrabajoID;
            analisiscaptura.ConsecionID = this.Anexo.ConsecionID;
            analisiscaptura.AnexoID = this.Anexo.AnexoID;
            analisiscaptura.LaboratorioID = this.Login.Laboratorio.LaboratorioID;
            analisiscaptura.EstatusID = 3;
            analisiscaptura.Firmada = false;
            analisiscaptura.FechaFirmada = null;
            analisiscaptura.Enviada = false;
            analisiscaptura.Procesado = false;
            analisiscaptura.FechaProceso = null;
            analisiscaptura.FechaRegistro = this.OrdenTrabajo.FechaEmision;

            Analisis_BL analisis_bl = new Analisis_BL();
            var an = analisis_bl.Guarda(analisiscaptura);
            if (an == null)
            {
                lnkbtnOrdenGuardar.Visible = true;
                lnkbtnOrdenGuardar.Enabled = true;
                RespuestaAnalisis(this, false);
            }
            RespuestaAnalisis(this, true);

        }

        public void CargaOrdenTrabajo_Consulta()
        {
            pnlOrden.Visible = false;
            pnlOrdenConsulta.Visible = true;

            if (this.OrdenTrabajo.Norma != null)
                lblNormaMexicana.Text = this.OrdenTrabajo.Norma.Nombre;


            if (this.OrdenTrabajo.CuerpoReceptor != null && this.OrdenTrabajo.CatUsoAgua != null)
            {
                divCuerpoReceptorNO.Visible = OrdenTrabajo.CuerpoReceptorID != 5; //sólo se oculta cuando es N/A
                divUsoAguaNO.Visible = OrdenTrabajo.UsoAguaID != 10; //sólo se oculta cuando es N/A
                if (divCuerpoReceptorNO.Visible)
                    lblCuerpoReceptor.Text = this.OrdenTrabajo.CuerpoReceptor.Nombre;
                if (divUsoAguaNO.Visible)
                    lblUsoAgua.Text = this.OrdenTrabajo.CatUsoAgua.Nombre;

                //RFD 30/07/2020 -> Adapta dinámicamente el contenido del panel Norma Oficial Mexicana:
                if (!divCuerpoReceptorNO.Visible && !divUsoAguaNO.Visible)
                {
                    divNOM.Attributes.Remove("class");
                    divNOM.Attributes.Add("class", "col-md-12");
                }
                else if (!divCuerpoReceptorNO.Visible)
                {
                    divNOM.Attributes.Remove("class");
                    divNOM.Attributes.Add("class", "col-md-6");
                    divUsoAguaNO.Attributes.Remove("class");
                    divUsoAguaNO.Attributes.Add("class", "col-md-6");
                }
                else if (!divUsoAguaNO.Visible)
                {
                    divNOM.Attributes.Remove("class");
                    divNOM.Attributes.Add("class", "col-md-6");
                    divCuerpoReceptorNO.Attributes.Remove("class");
                    divCuerpoReceptorNO.Attributes.Add("class", "col-md-6");
                }
            }
            lblOrdenNumero.Text = this.OrdenTrabajo.NumeroInforme;
            lblOrdenFechaEmision.Text = this.OrdenTrabajo.FechaEmision.ToString("dd/MM/yy");

            //Se oculta cuando no hay horas de proceso de descarga:
            divHoraProcesoDescarga.Visible = this.OrdenTrabajo.HorasProceso <= 24;
            if (divHoraProcesoDescarga.Visible)
                lblOrdenProcesoHoras.Text = this.OrdenTrabajo.HorasProceso <= 24 ? this.OrdenTrabajo.HorasProceso.ToString() : "N/A";

            //RFD 30/07/2020 -> Adapata dinámicamente el contenido del panel Norma Oficial Mexicana:
            if (!divHoraProcesoDescarga.Visible)
            {
                divNumPrueba.Attributes.Remove("class");
                divNumPrueba.Attributes.Add("class", "col-md-6");
                divFechaEmision.Attributes.Remove("class");
                divFechaEmision.Attributes.Add("class", "col-md-6");
            }

            lblOrdenProcesoHoras.Visible = OrdenTrabajo.HorasProceso <= 24;

            Norma_BL nBL = new Norma_BL();
            Analisis_BL analisisBL = new Analisis_BL();
            var analisis = analisisBL.Lista_Analisis((int)this.OrdenTrabajo.OrdenTrabajoID).Where(a => a.AnalisisPadreID != null).ToList();
            var Nom = nBL.Obtener((int)this.OrdenTrabajo.NormaID);
            if (!Nom.RequiereCaudal)
            {
                txtOrdenProcesoHoras.Text = "";
                txtOrdenProcesoHoras.Enabled = false;
                rfv_txtOrdenProcesoHoras.Enabled = false;
            }
            else
            {
                txtOrdenProcesoHoras.Text = "";
                txtOrdenProcesoHoras.Enabled = true;
                rfv_txtOrdenProcesoHoras.Enabled = true;
            }

            lnkbtnGenerarAnalisis.Visible = analisis.Count <= 0;

            pnlBoton.Enabled = this.Login.Laboratorio != null;
            pnlBoton.Visible = this.Login.Laboratorio != null;

        }
    }

    public class RespuestaOrdenTrabajoArgs : EventArgs
    {
        public int? OrdenTrabajoId { get; set; }

        public RespuestaOrdenTrabajoArgs() { }
    }

    public class NormaSeleccionadaArgs : EventArgs
    {
        public int NormaId { get; private set; }
        public List<CuerpoReceptor> CuerposReceptores { get; private set; }

        public NormaSeleccionadaArgs(int normaId, List<CuerpoReceptor> cuerposReceptores)
        {
            NormaId = normaId;
            CuerposReceptores = cuerposReceptores;
        }
    }
}