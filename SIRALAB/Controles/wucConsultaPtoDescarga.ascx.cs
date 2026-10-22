using AjaxControlToolkit;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_SIRFIC;
using Anexo = SIRALAB_DAL.Anexo;
using SIRALAB_BL.catalogos;

namespace SIRALAB.Controles
{
    public partial class wucConsultaPtoDescarga : System.Web.UI.UserControl
    {
        #region Propiedades

        public SIRALAB_DAL.Anexo Anexo
        {
            get { return (SIRALAB_DAL.Anexo)Session["AnexoConsulta_"]; }
            set { Session["AnexoConsulta_"] = value; }
        }

        public bool ContieneAnexo
        {
            get { return (bool)Session["ContieneAnexo_"]; }
            set { Session["ContieneAnexo_"] = value; }
        }

        public SIRALAB_DAL.Cliente Cliente
        {
            get { return (SIRALAB_DAL.Cliente)Session["ClienteConsulta_"]; }
        }

        public bool? EsFuenteAbastecimiento
        {
            get { return Session["EsFuenteAbastecimiento"] == null ? null : (bool?)Session["EsFuenteAbastecimiento"]; }
            set { Session["EsFuenteAbastecimiento"] = value; }
        }

        private int? AnexoMedidorID
        {
            get { return Session["AnexoMedidorID"] == null ? null : (int?)Session["AnexoMedidorID"]; }
            set { Session["AnexoMedidorID"] = value; }
        }

        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_P"]; }
            set { Session["OrdenTrabajoConsulta_P"] = value; }
        }
       

        #endregion Propiedades

        #region Eventos

        [Browsable(true)]
        public event EventHandler<Anexo> Seleccion;

        [Browsable(true)]
        public event EventHandler<Anexo> LimpiarNormas;

        [Browsable(true)]
        public event EventHandler<Anexo> Guardar;

        #endregion Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void lnkbtnPtoConsecion_Click(object sender, EventArgs e)
        {
            //Consecion_BL consecion_bl = new Consecion_BL();
            //consecion_bl.Obten(txtPtoConsecion.Text);
            //var consecion = consecion_bl.Consecion;

            //if (consecion != null)
            //{
            //    Anexo_BL anexo_bl = new Anexo_BL();
            //    LoadDrops.instance.Anexos(ddlPtoAnexo, consecion.ConsecionID);
            //}
        }

        protected void rdConcesion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = rdConcesion.SelectedIndex;
            var flag = (i == 0) ? false : true;
            EsFuenteAbastecimiento = i == 1;
            ObtenConseciones();
        }

        #region titulos

        /// <summary>
        /// Obtiene los títulos
        /// </summary>
        public void ObtenConseciones()
        {

            var i = rdConcesion.SelectedIndex;
            var flag = i == 0;
            var sirficContext = new CNA_SIRFICEntities();
            var titulos = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                           where
                               clie.rfc == Cliente.RFC &&
                               clie.FUENTE_ABASTECIMIENTO == (flag ? clie.FUENTE_ABASTECIMIENTO : 1)
                           select new { clie.ID_CONTRIBUYENTE, clie.titulo, clie.ID_TITULO }).Distinct().ToList();

            ddlConsecion.LoadDrop("titulo", "ID_TITULO", titulos);
            ddlConsecion.ClearSelection();
            ddlConsecion.SelectedIndex = ddlConsecion.Items.Count == 2 ? 1 : 0;

            limpiar();

            /*Limpia Anexo*/
            ddlPtoAnexo.Items.Clear();
            ddlPtoAnexo.DataBind();

            /*Limpia Medidor*/
            ddlMedidorFuente.Items.Clear();
            ddlMedidorFuente.DataBind();
            ddlMedidorFuente.Enabled = !flag;
            //Solo debe ser visible el medidor con fuente de abastecimiento
            ddlMedidorFuente.Visible = !flag;
            lblMedidorTag.Visible = !flag;

            //Limpiar normas
            LimpiarNormas(null, null);

            if (ddlConsecion.Items.Count == 2)
                ddlConsecion_SelectedIndexChanged(null, null);
        }

        protected void ddlConsecion_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*Limpia Medidor*/
            ddlMedidorFuente.Items.Clear();
            ddlMedidorFuente.DataBind();

            

            if (ddlConsecion.SelectedItem.Value != "0")
            {
                
                var consecionBl = new Consecion_BL();
                var consecion = new ConsecionCapturaArgs();
                var id = Convert.ToInt32(ddlConsecion.SelectedValue);
                consecionBl.ObtenConsecion(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);

                if (consecionBl.Consecion != null)
                {
                    if (ddlConsecion.SelectedItem.Text != consecionBl.Consecion.Titulo || consecionBl.Consecion.PukConsecionID != id)
                    {
                        consecion.ConsecionID = consecionBl.Consecion.ConsecionID;
                        consecion.ClienteID = Cliente.ClienteID;
                        consecion.Titulo = ddlConsecion.SelectedItem.Text;
                        consecion.PukConsecionID = id;
                        consecionBl.GuardaTitulos(consecion);

                    }
                }
                else
                {
                    consecion.ClienteID = Cliente.ClienteID;
                    consecion.Titulo = ddlConsecion.SelectedItem.Text;
                    consecion.PukConsecionID = id;
                    consecionBl.GuardaTitulos(consecion);
                    consecionBl.ObtenConsecion(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);
                }
                ObtenAnexos();
                LimpiarNormas(null, null);
            }
            else
            {
                ddlPtoAnexo.Items.Clear();
                var li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
                ddlPtoAnexo.Items.Insert(0, li);
                limpiar();
                LimpiarNormas(null, null);
                this.ContieneAnexo = false;
            }
        }

        #endregion titulos

        #region Anexo

        public void ObtenAnexos()
        {
            var i = rdConcesion.SelectedIndex;
            var flag = (i == 0) ? false : true;
            var ddlconse = Convert.ToInt32(ddlConsecion.SelectedValue);
            var sirficContext = new CNA_SIRFICEntities();

            List<SIRFIC_Anexo> anexos = new List<SIRFIC_Anexo>();

            anexos = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                      where
                          clie.rfc == Cliente.RFC &&
                          clie.FUENTE_ABASTECIMIENTO == (i == 0 ? clie.FUENTE_ABASTECIMIENTO : 1) &&
                          clie.ID_TITULO == ddlconse
                      select new SIRFIC_Anexo
                      {
                          numeroAnexo = clie.numeroAnexo,
                          titulo = clie.titulo,
                          ID_TITULO = clie.ID_TITULO,
                          ID_APROVECHAMIENTO = clie.ID_APROVECHAMIENTO,
                          ID_USO_CR = clie.ID_USO_CR,
                          ID_TIPO_CR = clie.ID_TIPO_CR
                      }).Distinct().ToList();

            ddlPtoAnexo.LoadDrop("numeroAnexo", "ID_APROVECHAMIENTO", anexos);
        }

        protected void ddlPtoAnexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Codigo comentado el 16-07
            lnkbtnPtoConsulta.Visible = true;
            /*Limpia Medidor*/
            ddlMedidorFuente.Items.Clear();
            ddlMedidorFuente.DataBind();
            if (ddlPtoAnexo.SelectedItem.Value != "0")
            {
                if (rdConcesion.SelectedIndex == 1)
                {
                    CargaMedidor(0);
                    EsFuenteAbastecimiento = true;
                }
                else
                {
                    EsFuenteAbastecimiento = false;
                }
                this.ContieneAnexo = true;
                CargaInformacionPunto(int.Parse(ddlPtoAnexo.SelectedItem.Value));
                txtPtoAbastecimiento.Enabled = true;
                txtPtoDescarga.Enabled = true;
                lnkbtnPtoAbasDes.Enabled = true;
                ddlMedidorFuente_SelectedIndexChanged(null, null);
                Norma_BL norma2021 = new Norma_BL();
                var aplicationSettings = new ApplicationSettings();
                var ValidaPrograma = aplicationSettings.ValidaPrograma;
                //var RequierePrograma = norma2021.RequierePrograma();
                if (ValidaPrograma=="true")
                {
                    var EstaInscritoNom001 = norma2021.ObtieneDatosInscritosNom001(ddlConsecion.SelectedItem.Text.Trim(), ddlPtoAnexo.SelectedItem.Text.Trim(), Cliente.RFC);

                    if (EstaInscritoNom001)
                    {
                        var rfcContribuyente = Cliente.RFC;
                        var NombreCompleto = string.Empty;
                        var NombreContribuyente = string.Empty;
                        if ((Cliente.ApellidoPaterno != string.Empty && Cliente.ApellidoPaterno != null) || (Cliente.ApellidoMaterno != string.Empty && Cliente.ApellidoMaterno != null))
                        {
                            NombreCompleto = Cliente.Nombre.Trim() + " " + Cliente.ApellidoPaterno.Trim() + " " + Cliente.ApellidoMaterno.Trim();
                            NombreContribuyente = NombreCompleto.Trim().ToUpper();
                            //var arrayNombre = NombreCompleto.Trim().Split(' ');

                            //foreach (var item in arrayNombre)
                            //{
                            //    if (item != string.Empty)
                            //    {
                            //        var LetraMayuzcula = item.Substring(0, 1);
                            //        LetraMayuzcula = LetraMayuzcula.ToUpper();
                            //        var restoPalabra = item.Substring(1);
                            //        restoPalabra = restoPalabra.ToLower();
                            //        NombreContribuyente = NombreContribuyente + " " + LetraMayuzcula + restoPalabra;
                            //    }
                            //}
                        }
                        else
                        {
                            NombreContribuyente = Cliente.Nombre.ToUpper();
                        }
                        Session["InscritoNom001"] = true;
                        txtMensajeModalNom001.InnerText = "El contribuyente " + NombreContribuyente + " con RFC " + rfcContribuyente + " CUENTA con programa para el cumplimiento vigente, establecido en el artículo cuarto transitorio de la Norma Oficial Mexicana “NOM-001-SEMARNAT-2021, Que establece los límites permisibles de contaminantes en las descargas de aguas residuales en cuerpos receptores propiedad de la nación.”";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeNom001').modal();", true);
                    }
                    else
                    {
                        var rfcContribuyente = Cliente.RFC;
                        var NombreCompleto = string.Empty;
                        var NombreContribuyente = string.Empty;
                        if ((Cliente.ApellidoPaterno !=string.Empty && Cliente.ApellidoPaterno!=null) || (Cliente.ApellidoMaterno!=string.Empty && Cliente.ApellidoMaterno!=null))
                        {
                            NombreCompleto = Cliente.Nombre.Trim() + " " + Cliente.ApellidoPaterno.Trim() + " " + Cliente.ApellidoMaterno.Trim();

                            NombreContribuyente = NombreCompleto.Trim().ToUpper();
                            //var arrayNombre = NombreCompleto.Trim().Split(' ');
                            
                            //foreach (var item in arrayNombre)
                            //{
                            //    if (item != string.Empty)
                            //    {
                            //        var LetraMayuzcula = item.Substring(0, 1);
                            //        LetraMayuzcula = LetraMayuzcula.ToUpper();
                            //        var restoPalabra = item.Substring(1);
                            //        restoPalabra = restoPalabra.ToLower();
                            //        NombreContribuyente = NombreContribuyente + " " + LetraMayuzcula + restoPalabra;
                            //    }
                            //}
                        }
                        else
                        {
                            NombreContribuyente = Cliente.Nombre.ToUpper();
                        }
                        
                         
                        Session["InscritoNom001"] = false;
                        txtMensajeModalNom001.InnerText = "El contribuyente " + NombreContribuyente + " con RFC " + rfcContribuyente + " NO CUENTA con programa para el cumplimiento vigente, establecido en el artículo cuarto transitorio de la Norma Oficial Mexicana “NOM-001-SEMARNAT-2021, Que establece los límites permisibles de contaminantes en las descargas de aguas residuales en cuerpos receptores propiedad de la nación.”";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeNom001').modal();", true);
                    }
                }
                
            }
            else
            {
                LimpiarNormas(null, null);
                limpiar();
                this.ContieneAnexo = false;
            }
        }

        #endregion Anexo

        #region Medidor

        private void CargaMedidor(int AnexoId)
        {
            var anexoBl = new Anexo_BL();
            anexoBl.Obten(AnexoId);
            if (AnexoId != 0)
                Session["ANEXO"] = anexoBl.Anexo.Anexo1;
            else
                Session["ANEXO"] = ddlPtoAnexo.SelectedItem.ToString();

            var sirficContext = new CNA_SIRFICEntities();
            var bl = new AnexoMedidor_BL();
            int i = rdConcesion.SelectedIndex;
            int ddlconse = Convert.ToInt32(ddlConsecion.SelectedValue);

            object medidor = null;
            EsFuenteAbastecimiento = (i == 0) ? false : true;

            if (AnexoId > 0)
                medidor = bl.Obten(AnexoId, true);
            else
            {
                int ddlanex;
                if (ddlPtoAnexo.SelectedValue != "" || ddlPtoAnexo.SelectedValue != null)
                {
                    ddlanex = Convert.ToInt32(ddlPtoAnexo.SelectedValue);
                    medidor = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                               where clie.rfc == Cliente.RFC && clie.FUENTE_ABASTECIMIENTO == i && clie.ID_TITULO == ddlconse && clie.ID_APROVECHAMIENTO == ddlanex
                               select new { clie.numeroMedidor, clie.ID_SERIE_MEDIDOR }).Distinct().ToList();
                }
            }
            if (EsFuenteAbastecimiento ?? false)
            {
                ddlMedidorFuente.DataSource = medidor;
                ddlMedidorFuente.DataTextField = "numeroMedidor";
                ddlMedidorFuente.DataValueField = "ID_SERIE_MEDIDOR";
                ddlMedidorFuente.DataBind();

                var li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
                ddlMedidorFuente.Items.Insert(0, li);

                ddlMedidorFuente.SelectedIndex = 0;

                //Seleccion Automatica
                if (ddlMedidorFuente.Items.Count == 2)
                {
                    ddlMedidorFuente.SelectedIndex = 1;
                }

                if (ddlMedidorFuente.Items.Count >= 3)
                {
                    if (anexoBl.Anexo != null)
                    {
                        if (anexoBl.Anexo.Medidor != "" || anexoBl.Anexo.Medidor != null)
                            ddlMedidorFuente.SelectedItem.Text = anexoBl.Anexo.Medidor;
                        else
                            ddlMedidorFuente.SelectedIndex = 1;
                    }
                    else
                        ddlMedidorFuente.SelectedIndex = 1;
                }

                //Seleccion Automatica
                ddlMedidorFuente.Enabled = true;
                ddlMedidorFuente_SelectedIndexChanged(null, null);
            }
            else
            {
                ddlMedidorFuente.Items.Clear();
                ddlMedidorFuente.Enabled = false;
            }
            rdConcesion.SelectedIndex = i;
            ScriptManager.RegisterStartupScript(lnkbtnPtoConsulta, lnkbtnPtoConsulta.GetType(), "moveScript", "ve(3);", true);
        }

        protected void ddlMedidorFuente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMedidorFuente.Items.Count > 0)
            {
                var sirficContext = new CNA_SIRFICEntities();
                const int i = 1;
                var ddlconse = Convert.ToInt32(ddlConsecion.SelectedValue);
                if (Anexo != null)
                {
                    var medidor = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                                   where clie.rfc == Cliente.RFC && clie.FUENTE_ABASTECIMIENTO == i /*&& clie.ID_TITULO == ddlconse*/
                                   select new { clie.ID_SERIE_MEDIDOR, clie.numeroMedidor, clie.ID_MARCA_MEDIDOR, clie.MARCA_MEDIDOR, clie.FUENTE_ABASTECIMIENTO }).Distinct().ToList();

                    foreach (var t in medidor)
                    {
                        var anexoMedidorBl = new AnexoMedidor_BL();
                        var anexoMedidorArg = new AnexoMedidorArgs();

                        anexoMedidorBl.Obten(Anexo.AnexoID, Convert.ToInt32(t.ID_SERIE_MEDIDOR));
                        if (anexoMedidorBl.AnexoMedidor != null)
                        {
                            if (anexoMedidorBl.AnexoMedidor.MARCA_MEDIDOR != t.MARCA_MEDIDOR ||
                                anexoMedidorBl.AnexoMedidor.ID_MARCA_MEDIDOR != t.ID_MARCA_MEDIDOR ||
                                anexoMedidorBl.AnexoMedidor.ID_SERIE_MEDIDOR != t.ID_SERIE_MEDIDOR ||
                                anexoMedidorBl.AnexoMedidor.FuenteAbastecimiento != Convert.ToBoolean(t.FUENTE_ABASTECIMIENTO) ||
                                anexoMedidorBl.AnexoMedidor.numeroMedidor != t.numeroMedidor)
                            {
                                anexoMedidorArg.AnexoMedidorID = anexoMedidorBl.AnexoMedidor.AnexoMedidorID;
                                anexoMedidorArg.AnexoID = Anexo.AnexoID;
                                anexoMedidorArg.ID_SERIE_MEDIDOR = t.ID_SERIE_MEDIDOR;
                                anexoMedidorArg.numeroMedidor = t.numeroMedidor;
                                anexoMedidorArg.ID_MARCA_MEDIDOR = t.ID_MARCA_MEDIDOR;
                                anexoMedidorArg.MARCA_MEDIDOR = t.MARCA_MEDIDOR;
                                anexoMedidorArg.FuenteAbastecimiento = i == 1;
                                anexoMedidorBl.Guarda(anexoMedidorArg);
                            }

                        }
                        else
                        {
                            anexoMedidorArg.AnexoID = Anexo.AnexoID;
                            anexoMedidorArg.ID_SERIE_MEDIDOR = t.ID_SERIE_MEDIDOR;
                            anexoMedidorArg.numeroMedidor = t.numeroMedidor;
                            anexoMedidorArg.ID_MARCA_MEDIDOR = t.ID_MARCA_MEDIDOR;
                            anexoMedidorArg.MARCA_MEDIDOR = t.MARCA_MEDIDOR;
                            anexoMedidorArg.FuenteAbastecimiento = (i == 1) ? true : false;
                            anexoMedidorBl.Guarda(anexoMedidorArg);
                        }

                    }
                }

                AnexoMedidorID = Convert.ToInt32(ddlMedidorFuente.SelectedItem.Value);

                var anexoBl = new Anexo_BL();
                var consecionBl = new Consecion_BL();
                // var ddlanexo = Session["ANEXO"] != null ? Session["ANEXO"].ToString() : ddlPtoAnexo.SelectedItem.ToString();

                consecionBl.ObtenConsecion(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);
                anexoBl.Obtanexnombre(consecionBl.Consecion.ConsecionID, Session["ANEXO"].ToString(), ddlMedidorFuente.SelectedItem.ToString());
                this.Anexo = anexoBl.Anexo;

                ScriptManager.RegisterStartupScript(lnkbtnPtoConsulta, lnkbtnPtoConsulta.GetType(), "moveScript",
                    "ve(3);", true);

            }
        }

        #endregion Medidor

        public void limpiar()
        {
            lblPtoCalle.Text = "";
            lblPtoNE.Text = "";
            lblPtoNI.Text = "";
            lblPtoCP.Text = "";
            lblPtoEstado.Text = "";
            lblPtoMunicipio.Text = "";
            lblPtoLocalidad.Text = "";
            lblPtoColonia.Text = "";
            lblPtoGraLat.Text = "";
            lblPtoMinLat.Text = "";
            lblPtoSegLat.Text = "";
            lblPtoDirLat.Text = "";
            lblPtoGraLong.Text = "";
            lblPtoMinLong.Text = "";
            lblPtoSegLong.Text = "";
            lblPtoDirLong.Text = "";
            lblPtoDatum.Text = "";
            lblPtoCuerpoReceptor.Text = "";
            txtPtoAbastecimiento.Text = "";
            txtPtoDescarga.Text = "";
        }

        public void InhabilitaPanel()
        {
            pnlDatosPto.Enabled = false;
        }

        public void CargaInformacionPunto(int AnexoID)
        {
            int i = rdConcesion.SelectedIndex;
            var consecionId = Convert.ToInt32(ddlConsecion.SelectedValue);
            string numeroAnexo = ddlPtoAnexo.SelectedItem.ToString();
            var sirficContext = new CNA_SIRFICEntities();
            var anexos = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                where
                    clie.rfc == Cliente.RFC &&
                    clie.ID_TITULO == consecionId &&
                    clie.numeroAnexo == numeroAnexo &&
                    clie.FUENTE_ABASTECIMIENTO == (i == 0 ? clie.FUENTE_ABASTECIMIENTO : 1)
                orderby clie.numeroMedidor
                select new
                {
                    clie.vialidad_PD,
                    clie.numeroExterior1_PD,
                    clie.numeroExterior2_PD,
                    clie.numeroInterior_PD,
                    clie.CODIGO_POSTAL_PD,
                    clie.CLAVE_ESTADO_PD,
                    clie.ESTADO_PD,
                    clie.CLAVE_MUNICIPIO_PD,
                    clie.MUNICIPIO_PD,
                    clie.LOCALIDAD_PD,
                    clie.CLAVE_LOCALIDAD_PD,
                    clie.ID_ASENTAMIENTO_HUMANO_PD,
                    clie.ASENTAMIENTO_HUMANO_PD,
                    clie.latitud,
                    clie.longitud,
                    clie.ID_APROVECHAMIENTO,
                    clie.NOM_APROVECHAMIENTO,
                    clie.idCuerpoReceptor,
                    clie.NOMBRE_CR,
                    clie.idUman,
                    clie.numeroMedidor,
                    clie.ID_TIPO_CR,
                    clie.ID_USO_CR,

                    clie.codigoPostal,
                    clie.CLAVE_ESTADO,
                    clie.CLAVE_MUNICIPIO,
                    clie.vialidad,
                    clie.numeroExterior1,
                    clie.numeroInterior,
                    clie.ESTADO,
                    clie.CLAVE_LOCALIDAD,
                    clie.LOCALIDAD,
                    clie.TipoAsentamiento,
                    clie.ASENTAMIENTO_HUMANO,
                  clie.ID_ASENTAMIENTO_HUMANO
                }).Distinct().ToList();
            //if (i == 0)
            //{
  
        
            

   
         
            var anex = 0;
                var count = 0;
            foreach (var tan in anexos)
            {
                count = count + 1;
                lblPtoCalle.Text = tan.vialidad_PD;
                lblPtoNE.Text = tan.numeroExterior1_PD;
                lblPtoNI.Text = tan.numeroInterior_PD;
                lblPtoCP.Text = tan.CODIGO_POSTAL_PD.ToString();
                lblPtoEstado.Text = tan.ESTADO_PD;
                lblPtoMunicipio.Text = tan.MUNICIPIO_PD;
                lblPtoLocalidad.Text = tan.LOCALIDAD_PD;
                lblPtoColonia.Text = tan.ASENTAMIENTO_HUMANO_PD;
                anex = Convert.ToInt32(tan.ID_APROVECHAMIENTO);
                var latitud = tan.latitud.Split('|');
                lblPtoGraLat.Text = latitud[0];
                lblPtoMinLat.Text = latitud[1];
                lblPtoSegLat.Text = latitud[2];
                lblPtoDirLat.Text = "Norte";
                var longitud = tan.longitud.Split('|');
                lblPtoGraLong.Text = longitud[0];
                lblPtoMinLong.Text = longitud[1];
                lblPtoSegLong.Text = longitud[2];
                lblPtoDirLong.Text = "Oeste";
                lblPtoDatum.Text = "";
                lblPtoEPTratamiento.Text = "No";
                var anexoBl = new Anexo_BL();
                var anexoArg = new AnexoCapturaArgs();
                var direccionAnexo = new DireccionCapturaArgs();
                /*se busca id deconsesion*/
                var consecionBl = new Consecion_BL();

                consecionBl.ObtenConsecion(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);
                anexoBl.Obtanexsolo(consecionBl.Consecion.ConsecionID, numeroAnexo);

                HttpContext.Current.Session["ColTipo"] = tan.TipoAsentamiento;
                if (tan.LOCALIDAD_PD==null || tan.LOCALIDAD_PD==string.Empty)
                {
                    HttpContext.Current.Session["LocalidadNombre"] = tan.LOCALIDAD;
                    HttpContext.Current.Session["NombreColonia"] = tan.ASENTAMIENTO_HUMANO;
                }
                else
                {
                    HttpContext.Current.Session["LocalidadNombre"] = tan.LOCALIDAD_PD;
                    HttpContext.Current.Session["NombreColonia"] = tan.ASENTAMIENTO_HUMANO_PD;
                }
                if (count >= 2)
                    anexoBl.ObtenAnexo(consecionBl.Consecion.ConsecionID, numeroAnexo, tan.idUman);

                if (anexoBl.Anexo != null)
                {
                    if (anexoBl.Anexo.Anexo1 != numeroAnexo || anexoBl.Anexo.PukAnexoID != anex ||
                        anexoBl.Anexo.Medidor != tan.numeroMedidor)
                    {
                        var latitudg = tan.latitud.Split('|');
                        var longitudg = tan.longitud.Split('|');
                        var coordenada = new Coordenadas_BL(
                            decimal.Parse(latitudg[0]),
                            decimal.Parse(latitudg[1]),
                            decimal.Parse(latitudg[2]), EnumCoordinatesLat.Norte,
                            decimal.Parse(longitudg[0]),
                            decimal.Parse(longitudg[1]),
                            decimal.Parse(longitudg[2]), EnumCoordinatesLng.Oeste);
                        anexoArg.AnexoID = anexoBl.Anexo.AnexoID;
                        direccionAnexo.DireccionID = anexoBl.Anexo.DireccionID;

                        if ((tan.CODIGO_POSTAL_PD == null || tan.CODIGO_POSTAL_PD == 0) && (tan.vialidad_PD == null || tan.vialidad_PD == string.Empty) && (tan.CLAVE_ESTADO_PD == null || tan.CLAVE_ESTADO_PD == 0) && (tan.CLAVE_LOCALIDAD_PD == null || tan.CLAVE_LOCALIDAD_PD == 0) && (tan.CLAVE_MUNICIPIO_PD == null || tan.CLAVE_MUNICIPIO_PD == 0))
                        {
                            direccionAnexo.Calle = tan.vialidad;
                            direccionAnexo.NumeroExteior = tan.numeroExterior1;
                            direccionAnexo.NumeroInterior = tan.numeroInterior;
                            direccionAnexo.EstadoID = Convert.ToInt32(tan.CLAVE_ESTADO);
                            direccionAnexo.MunicipioID = Convert.ToInt32(tan.CLAVE_MUNICIPIO);
                            direccionAnexo.LocalidadID = tan.CLAVE_LOCALIDAD;
                            direccionAnexo.ColoniaID = Convert.ToInt32(tan.ID_ASENTAMIENTO_HUMANO);
                            direccionAnexo.CP = tan.codigoPostal;
                        }
                        else
                        {
                            direccionAnexo.Calle = tan.vialidad_PD;
                            direccionAnexo.NumeroExteior = tan.numeroExterior1_PD;
                            direccionAnexo.NumeroInterior = tan.numeroInterior_PD;
                            direccionAnexo.EstadoID = Convert.ToInt32(tan.CLAVE_ESTADO_PD);
                            direccionAnexo.MunicipioID = Convert.ToInt32(tan.CLAVE_MUNICIPIO_PD);
                            direccionAnexo.LocalidadID = tan.CLAVE_LOCALIDAD_PD;
                            direccionAnexo.ColoniaID = Convert.ToInt32(tan.ID_ASENTAMIENTO_HUMANO_PD);
                            direccionAnexo.CP = tan.CODIGO_POSTAL_PD;
                        }

                        consecionBl.Obten(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);

                        anexoArg.ConsecionID = consecionBl.Consecion.ConsecionID;
                        anexoArg.Anexo = numeroAnexo;
                        anexoArg.Latitud = coordenada.Latitude;
                        anexoArg.Longitud = coordenada.Longitude;
                        anexoArg.Datum = "";
                        anexoArg.CuerpoReceptorID = 1;
                        anexoArg.UsoAguaID = 1;
                        anexoArg.NumeroMedidor = tan.numeroMedidor;
                        anexoArg.NombreCuerpoReceptor = tan.NOMBRE_CR;
                        anexoArg.FuenteAbastecimiento = "";
                        anexoArg.OrigenDescarga = tan.NOM_APROVECHAMIENTO;
                        anexoArg.Efluente = false;
                        anexoArg.PukAnexoID = tan.ID_APROVECHAMIENTO;
                        anexoArg.iduman = tan.idUman;
                        anexoArg.dir = direccionAnexo;
                        anexoBl.Guarda(anexoArg);
                    }
                }
                else
                {
                    var latitudg = tan.latitud.Split('|');
                    var longitudg = tan.longitud.Split('|');
                    var coordenada = new Coordenadas_BL(
                        decimal.Parse(latitudg[0]),
                        decimal.Parse(latitudg[1]),
                        decimal.Parse(latitudg[2]), EnumCoordinatesLat.Norte,
                        decimal.Parse(longitudg[0]),
                        decimal.Parse(longitudg[1]),
                        decimal.Parse(longitudg[2]), EnumCoordinatesLng.Oeste);
                    anexoArg.AnexoID = 0;
                    if ((tan.CODIGO_POSTAL_PD == null || tan.CODIGO_POSTAL_PD == 0) && (tan.vialidad_PD == null || tan.vialidad_PD == string.Empty) && (tan.CLAVE_ESTADO_PD == null || tan.CLAVE_ESTADO_PD == 0) && (tan.CLAVE_LOCALIDAD_PD == null || tan.CLAVE_LOCALIDAD_PD == 0) && (tan.CLAVE_MUNICIPIO_PD == null || tan.CLAVE_MUNICIPIO_PD == 0))
                    {
                        direccionAnexo.DireccionID = null;
                        direccionAnexo.Calle = tan.vialidad;
                        direccionAnexo.NumeroExteior = tan.numeroExterior1;
                        direccionAnexo.NumeroInterior = tan.numeroInterior;
                        direccionAnexo.EstadoID = Convert.ToInt32(tan.CLAVE_ESTADO);
                        direccionAnexo.MunicipioID = Convert.ToInt32(tan.CLAVE_MUNICIPIO);
                        direccionAnexo.LocalidadID = tan.CLAVE_LOCALIDAD;
                        direccionAnexo.ColoniaID = Convert.ToInt32(tan.ID_ASENTAMIENTO_HUMANO);
                        direccionAnexo.CP = tan.codigoPostal;
                    }
                    else
                    {
                        direccionAnexo.DireccionID = null;
                        direccionAnexo.Calle = tan.vialidad_PD ?? "";
                        direccionAnexo.NumeroExteior = tan.numeroExterior1_PD ?? "";
                        direccionAnexo.NumeroInterior = tan.numeroInterior_PD ?? "";
                        direccionAnexo.EstadoID = Convert.ToInt32(tan.CLAVE_ESTADO_PD);
                        direccionAnexo.MunicipioID = Convert.ToInt32(tan.CLAVE_MUNICIPIO_PD);
                        direccionAnexo.LocalidadID = tan.CLAVE_LOCALIDAD_PD;
                        direccionAnexo.ColoniaID = Convert.ToInt32(tan.ID_ASENTAMIENTO_HUMANO_PD);
                        direccionAnexo.CP = tan.CODIGO_POSTAL_PD;
                    }

                    consecionBl.Obten(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);

                    anexoArg.ConsecionID = consecionBl.Consecion.ConsecionID;
                    anexoArg.Anexo = numeroAnexo;
                    anexoArg.Latitud = coordenada.Latitude;
                    anexoArg.Longitud = coordenada.Longitude;
                    anexoArg.Datum = "";
                    anexoArg.CuerpoReceptorID = 1;
                    anexoArg.UsoAguaID = 1;
                    anexoArg.NumeroMedidor = tan.numeroMedidor;
                    anexoArg.NombreCuerpoReceptor = tan.NOMBRE_CR;
                    anexoArg.FuenteAbastecimiento = "";
                    anexoArg.OrigenDescarga = tan.NOM_APROVECHAMIENTO;
                    anexoArg.Efluente = false;
                    anexoArg.PukAnexoID = tan.ID_APROVECHAMIENTO;
                    anexoArg.iduman = tan.idUman;

                    anexoArg.dir = direccionAnexo;
                    anexoBl.Guarda(anexoArg);
                }
                anexoBl.Anexo.ID_USO_CR = tan.ID_USO_CR;
                anexoBl.Anexo.ID_TIPO_CR = tan.ID_TIPO_CR;

                this.Anexo = anexoBl.Anexo;
            }

            #region Comentado
            //}
            //else
            //{
            //    anexos = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
            //                  where clie.rfc == Cliente.RFC && clie.ID_TITULO == consecionId && clie.numeroAnexo == numeroAnexo
            //                  orderby clie.numeroMedidor
            //                  select new { clie.vialidad_PD,
            //                      clie.numeroExterior1_PD,
            //                      clie.numeroExterior2_PD,
            //                      clie.numeroInterior_PD,
            //                      clie.CODIGO_POSTAL_PD,
            //                      clie.CLAVE_ESTADO_PD,
            //                      clie.ESTADO_PD,
            //                      clie.CLAVE_MUNICIPIO_PD,
            //                      clie.MUNICIPIO_PD,
            //                      clie.LOCALIDAD_PD,
            //                      clie.CLAVE_LOCALIDAD_PD,
            //                      clie.ID_ASENTAMIENTO_HUMANO_PD,
            //                      clie.ASENTAMIENTO_HUMANO_PD,
            //                      clie.latitud,
            //                      clie.longitud,
            //                      clie.ID_APROVECHAMIENTO,
            //                      clie.NOM_APROVECHAMIENTO,
            //                      clie.idCuerpoReceptor,
            //                      clie.NOMBRE_CR,
            //                      clie.idUman,
            //                      clie.numeroMedidor,
            //                      clie.ID_TIPO_CR,
            //                      clie.ID_USO_CR
            //                  }).Distinct().ToList();

            //    int anex;
            //    var count = 0;

            //    foreach (var tan in anexos)
            //    {
            //        count = count + 1;
            //        lblPtoCalle.Text = tan.vialidad_PD;
            //        lblPtoNE.Text = tan.numeroExterior1_PD;
            //        lblPtoNI.Text = tan.numeroInterior_PD;
            //        lblPtoCP.Text = tan.CODIGO_POSTAL_PD.ToString();
            //        lblPtoEstado.Text = tan.ESTADO_PD;
            //        lblPtoMunicipio.Text = tan.MUNICIPIO_PD;
            //        lblPtoLocalidad.Text = tan.LOCALIDAD_PD;
            //        lblPtoColonia.Text = tan.ASENTAMIENTO_HUMANO_PD;
            //        anex = Convert.ToInt32(tan.ID_APROVECHAMIENTO);
            //        var latitud = tan.latitud.Split('|');
            //        lblPtoGraLat.Text = latitud[0];
            //        lblPtoMinLat.Text = latitud[1];
            //        lblPtoSegLat.Text = latitud[2];
            //        lblPtoDirLat.Text = "Norte";
            //        var longitud = tan.longitud.Split('|');
            //        lblPtoGraLong.Text = longitud[0];
            //        lblPtoMinLong.Text = longitud[1];
            //        lblPtoSegLong.Text = longitud[2];
            //        lblPtoDirLong.Text = "Oeste";
            //        lblPtoDatum.Text = "";
            //        lblPtoEPTratamiento.Text = "No";
            //        var anexoBl = new Anexo_BL();
            //        var anexoArg = new AnexoCapturaArgs();
            //        var direccionAnexo = new DireccionCapturaArgs();
            //        /*se busca id deconsesion*/
            //        var consecionBl = new Consecion_BL();

            //        consecionBl.Obtenconse(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);
            //        anexoBl.Obtanexsolo(consecionBl.Consecion.ConsecionID, numeroAnexo);

            //        if (count >= 2)
            //            anexoBl.Obtanex(consecionBl.Consecion.ConsecionID, numeroAnexo, tan.idUman);

            //        if (anexoBl.Anexo != null)
            //        {
            //            if (anexoBl.Anexo.Anexo1 != numeroAnexo || anexoBl.Anexo.PukAnexoID != anex || anexoBl.Anexo.Medidor != tan.numeroMedidor)
            //            {
            //                var latitudg = tan.latitud.Split('|');
            //                var longitudg = tan.longitud.Split('|');
            //                var coordenada = new Coordenadas_BL(
            //                   decimal.Parse(latitudg[0]),
            //                   decimal.Parse(latitudg[1]),
            //                   decimal.Parse(latitudg[2]), EnumCoordinatesLat.Norte,
            //                   decimal.Parse(longitudg[0]),
            //                   decimal.Parse(longitudg[1]),
            //                   decimal.Parse(longitudg[2]), EnumCoordinatesLng.Oeste);
            //                anexoArg.AnexoID = anexoBl.Anexo.AnexoID;
            //                direccionAnexo.DireccionID = anexoBl.Anexo.DireccionID;
            //                direccionAnexo.Calle = tan.vialidad_PD;
            //                direccionAnexo.NumeroExteior = tan.numeroExterior1_PD;
            //                direccionAnexo.NumeroInterior = tan.numeroInterior_PD;
            //                direccionAnexo.EstadoID = Convert.ToInt32(tan.CLAVE_ESTADO_PD);
            //                direccionAnexo.MunicipioID = Convert.ToInt32(tan.CLAVE_MUNICIPIO_PD);
            //                direccionAnexo.LocalidadID = tan.CLAVE_LOCALIDAD_PD;
            //                direccionAnexo.ColoniaID = Convert.ToInt32(tan.ID_ASENTAMIENTO_HUMANO_PD);
            //                direccionAnexo.CP = tan.CODIGO_POSTAL_PD;


            //                consecionBl.Obten(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);

            //                anexoArg.ConsecionID = consecionBl.Consecion.ConsecionID;
            //                anexoArg.Anexo = numeroAnexo;
            //                anexoArg.Latitud = coordenada.Latitude;
            //                anexoArg.Longitud = coordenada.Longitude;
            //                anexoArg.Datum = "";
            //                anexoArg.CuerpoReceptorID = 1;
            //                anexoArg.UsoAguaID = 1;
            //                anexoArg.NumeroMedidor = tan.numeroMedidor;
            //                anexoArg.NombreCuerpoReceptor = tan.NOMBRE_CR;
            //                anexoArg.FuenteAbastecimiento = "";
            //                anexoArg.OrigenDescarga = tan.NOM_APROVECHAMIENTO;
            //                anexoArg.Efluente = false;
            //                anexoArg.PukAnexoID = tan.ID_APROVECHAMIENTO;
            //                anexoArg.iduman = tan.idUman;

            //                anexoArg.dir = direccionAnexo;
            //                anexoBl.Guarda(anexoArg);

            //            }
            //        }
            //        else
            //        {
            //            var latitudg = tan.latitud.Split('|');
            //            var longitudg = tan.longitud.Split('|');
            //            var coordenada = new Coordenadas_BL(
            //               decimal.Parse(latitudg[0]),
            //               decimal.Parse(latitudg[1]),
            //               decimal.Parse(latitudg[2]), EnumCoordinatesLat.Norte,
            //               decimal.Parse(longitudg[0]),
            //               decimal.Parse(longitudg[1]),
            //               decimal.Parse(longitudg[2]), EnumCoordinatesLng.Oeste);
            //            anexoArg.AnexoID = 0;
            //            direccionAnexo.DireccionID = null;
            //            direccionAnexo.Calle = tan.vialidad_PD ?? "";
            //            direccionAnexo.NumeroExteior = tan.numeroExterior1_PD ?? "";
            //            direccionAnexo.NumeroInterior = tan.numeroInterior_PD ?? "";
            //            direccionAnexo.EstadoID = Convert.ToInt32(tan.CLAVE_ESTADO_PD);
            //            direccionAnexo.MunicipioID = Convert.ToInt32(tan.CLAVE_MUNICIPIO_PD);
            //            direccionAnexo.LocalidadID = tan.CLAVE_LOCALIDAD_PD;
            //            direccionAnexo.ColoniaID = Convert.ToInt32(tan.ID_ASENTAMIENTO_HUMANO_PD);
            //            direccionAnexo.CP = tan.CODIGO_POSTAL_PD;

            //            consecionBl.Obten(ddlConsecion.SelectedItem.Text, Cliente.ClienteID);

            //            anexoArg.ConsecionID = consecionBl.Consecion.ConsecionID;
            //            anexoArg.Anexo = numeroAnexo;
            //            anexoArg.Latitud = coordenada.Latitude;
            //            anexoArg.Longitud = coordenada.Longitude;
            //            anexoArg.Datum = "";
            //            anexoArg.CuerpoReceptorID = 1;
            //            anexoArg.UsoAguaID = 1;
            //            anexoArg.NumeroMedidor = tan.numeroMedidor;
            //            anexoArg.NombreCuerpoReceptor = tan.NOMBRE_CR;
            //            anexoArg.FuenteAbastecimiento = "";
            //            anexoArg.OrigenDescarga = tan.NOM_APROVECHAMIENTO;
            //            anexoArg.Efluente = false;
            //            anexoArg.PukAnexoID = tan.ID_APROVECHAMIENTO;
            //            anexoArg.iduman = tan.idUman;

            //            anexoArg.dir = direccionAnexo;
            //            anexoBl.Guarda(anexoArg);
            //        }
            //this.Anexo = anexoBl.Anexo;
            //}
            //} 
            #endregion
            HttpContext.Current.Session["anexok"] = 0;

            /*Fuente Abastecimiento*/
            if (OrdenTrabajo != null && (OrdenTrabajo.FuenteAbastecimiento ?? false))
            {
                rdConcesion.SelectedIndex = (OrdenTrabajo.FuenteAbastecimiento ?? false) ? 0 : 1;
                AnexoMedidorID = OrdenTrabajo.AnexoMedidorID;
            }
            else
            {
                AnexoMedidorID = null;
            }

            ScriptManager.RegisterStartupScript(lnkbtnPtoConsulta, lnkbtnPtoConsulta.GetType(), "moveScript", "ve(3);", true);

            Seleccion(this, this.Anexo);
        }

        public void CargaInformacionPunto_Consulta(int AnexoID)
        {
            Anexo_BL anexo_bl = new Anexo_BL();
            anexo_bl.Obten(AnexoID);
            var anexo = anexo_bl.Anexo;
            this.Anexo = anexo_bl.Anexo;

            lblPtoCalle.Text = anexo.Direccion.Calle;
            lblPtoNE.Text = anexo.Direccion.NumeroExterior;
            lblPtoNI.Text = anexo.Direccion.NumeroInterior;
            lblPtoCP.Text = anexo.Direccion.CP.ToString();
            lblPtoEstado.Text = anexo.Direccion.Estado.Nombre;
            lblPtoMunicipio.Text = anexo.Direccion.Municipio.Nombre;
            lblPtoLocalidad.Text = anexo.Direccion.Localidad.Nombre;
            lblPtoColonia.Text = anexo.Direccion.Colonia.Nombre;
          //Sección de coordenas
          Coordenadas_BL Coordenada = new Coordenadas_BL(anexo.Latitud, anexo.Longitud);

            lblPtoGraLat.Text = Coordenada.GradosLat.ToString();
            lblPtoMinLat.Text = Coordenada.MinutosLat.ToString();
            lblPtoSegLat.Text = Coordenada.SegundosLat.ToString().Substring(0, 5);
            lblPtoDirLat.Text = (Coordenada.PosicionLat.ToString() == "North" || Coordenada.PosicionLat.ToString() == "Norte") ? "Norte" : "Sur";

            lblPtoGraLong.Text = Coordenada.GradosLng.ToString();
            lblPtoMinLong.Text = Coordenada.MinutosLng.ToString();
            lblPtoSegLong.Text = Coordenada.SegundosLng.ToString().Substring(0, 5);
            lblPtoDirLong.Text = (Coordenada.PosicionLng.ToString() == "East" || Coordenada.PosicionLng.ToString() == "Este") ? "Este" : "Oeste";
          
            lblPtoDatum.Text = anexo.Datum;
            lblPtoCuerpoReceptor.Text = anexo.NombreCuerpoReceptor;
            txtPtoAbastecimiento.Text = anexo.FuenteAbastecimiento;
            txtPtoDescarga.Text = anexo.OrigenDescarga;
            lblPtoEPTratamiento.Text = anexo.Efluente == true ? "Si" : "No";

            /*Fuente Abastecimiento*/

            if (OrdenTrabajo.FuenteAbastecimiento ?? false)
            {
                AnexoMedidorID = OrdenTrabajo.AnexoMedidorID;
                CargaMedidor(OrdenTrabajo.AnexoID ?? 0);
            }
            else
            {
                AnexoMedidorID = null;
                ddlMedidorFuente.Visible = false;
                lblMedidorTag.Visible = false;
            }
            ddlMedidorFuente.Enabled = false;
            Seleccion(this, this.Anexo);
        }

        public void CargaInformacion_Consulta(int ConsecionID, int AnexoID)
        {
            lnkbtnPtoConsulta.Visible = true;
            Consecion_BL consecion_bl = new Consecion_BL();
            consecion_bl.Obten(ConsecionID);

            //Titulos
            //LoadDrops.instance.Conseciones(ddlConsecion, this.Cliente.ClienteID);
            if (OrdenTrabajo.FuenteAbastecimiento ?? false)
            {
                Consecion_BL bl = new Consecion_BL();
                var conseciones = bl.Listado_Consecion(this.Cliente.ClienteID, true);
                ddlConsecion.DataSource = conseciones;
                ddlConsecion.DataTextField = "Titulo";
                ddlConsecion.DataValueField = "ConsecionID";
                ddlConsecion.DataBind();
                rdConcesion.SelectedIndex = 1;
            }
            else
            {
                LoadDrops.instance.Conseciones(ddlConsecion, this.Cliente.ClienteID);
                rdConcesion.SelectedIndex = 0;
            }
            rdConcesion.Enabled = false;

            ddlConsecion.SelectedValue = consecion_bl.Consecion.ConsecionID.ToString();
            ddlConsecion.Enabled = false;

            /*Anexo*/
            CargaInformacionPunto_Consulta(AnexoID);

            ListItem item = new ListItem();
            item.Text = this.Anexo.Anexo1.ToString();
            ddlPtoAnexo.Items.Add(item);
            ddlPtoAnexo.Enabled = false;

            lnkbtnPtoConsecion.Visible = false;
        }

        protected void lnkbtnPtoConsulta_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(lnkbtnPtoConsulta, lnkbtnPtoConsulta.GetType(), "moveScript", "ve(3);", true);
        }

        protected void lnkbtnPtoAbasDes_Click(object sender, EventArgs e)
        {
            //this.Anexo.FuenteAbastecimiento=txtPtoAbastecimiento.Text;
            this.Anexo.OrigenDescarga = txtPtoDescarga.Text;
            Anexo_BL anexo_bl = new Anexo_BL();
            var an = anexo_bl.Guarda_PDES(this.Anexo);
            Guardar(this, an);
        }
    }

    public class SIRFIC_Anexo
    {
        public string numeroAnexo { get; set; }
        public string titulo { get; set; }
        public long ID_TITULO { get; set; }
        public long ID_APROVECHAMIENTO { get; set; }
        public long? ID_TIPO_CR { get; set; }
        public long? ID_USO_CR { get; set; }
    }
}