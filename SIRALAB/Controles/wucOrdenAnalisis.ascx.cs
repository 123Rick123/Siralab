using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using SIRALAB.Common;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB.FIEL;
using SIRALAB_DAL.Extensiones;

using System.ComponentModel;
using Microsoft.Reporting.WebForms;
using SIRALAB.controles;
using SIRALAB_BL.common;
using SIRALAB.ClienteAPI;


namespace SIRALAB.Controles
{
    public partial class wucOrdenAnalisis : System.Web.UI.UserControl
    {
        #region Propiedades

        [Browsable(true)]
        public event EventHandler<DatosFirma_Args> FirmarAnalisis;
        [Browsable(true)]
        public event EventHandler<SIRALAB_DAL.Analisi> Cancelar;
        [Browsable(true)]
        public event EventHandler<int> Eliminar;
        [Browsable(true)]
        public event EventHandler<FaltaPagoArgs> FaltaPago;
        [Browsable(true)]
        public event EventHandler<Dictionary<string,string>> MensajeSSTDQOCOT;
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        public SIRALAB_DAL.Analisi Analisis
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
            set { Session["AnalisisMuestra_"] = value; }
        }
        public int AnalisisID
        {
            get { return (int)Session["AnalisisID_"] == 0 ? 0 : (int)Session["AnalisisID_"]; }
            set { Session["AnalisisID_"] = value; }
        }
        public int OrdenTrabajoID
        {
            get { return int.Parse(hfOrdenTrabajo.Value == "" ? "0" : hfOrdenTrabajo.Value); }
            set { hfOrdenTrabajo.Value = value.ToString(); }
        }

        public bool RequiereCaudal
        {
            set { ViewState["RequiereCaudal"] = value; }
            get { return (bool)ViewState["RequiereCaudal"]; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void CargaInformacion(int OrdenTrabajoID)
        {
            rptTitulo.DataSource = null;
            rptTitulo.DataBind();

            this.OrdenTrabajoID = OrdenTrabajoID;

            Analisis_BL analisis_bl = new Analisis_BL();
            List<Analisi> ListAnalisis = new List<Analisi>();
            List<AnalisisLista> Analisis_rp = new List<AnalisisLista>();

            //Validamos si se trata de perfil de recaudación y fiscalizacion
            if (this.Login.UsuarioRolID == 2)
            {
                ListAnalisis = analisis_bl.Lista_Analisis(OrdenTrabajoID).Where(x => x.Firmada).OrderBy(x => x.AnalisisID).ToList();
                lnkbtnFirmar.Visible = false;
            }
            else
            {
                ListAnalisis = analisis_bl.Lista_Analisis(OrdenTrabajoID);
                //Validamos que se contengan analisis para hacer visible el boton de firmar
                lnkbtnFirmar.Visible = ListAnalisis.Any(a => a.VerificaBanderaMuestraCompuesta());
            }

            int Consecutivo = 1;

            foreach (var i in ListAnalisis)
            {
                Muestra FechaConformacion = null;
                FechaConformacion = i.OrdenTrabajo.Norma.RequiereCompuesta ? i.Muestras.FirstOrDefault(D => D.NivelID == 2) : i.Muestras.FirstOrDefault(D => D.NivelID == 1 && !i.OrdenTrabajo.Norma.RequiereCompuesta);

                string FechaAnalisis = string.Empty;
                DateTime ValidDatetime = new DateTime();

                //RFD - 23/07/2020: se modifica la fecha que se presenta en la tabla análisis para tomar en cuenta el nuevo campo FechaConformacion.
                if (FechaConformacion != null)
                {
                    if (FechaConformacion.NivelID == 2)
                    {
                        if (DateTime.TryParse(FechaConformacion.FechaConformacion.ToString(), out ValidDatetime)) //Cuando tiene Fecha conformación.
                        {
                            FechaAnalisis = DateTime.Parse(FechaConformacion.FechaConformacion.ToString()).ToString("dd/MM/yy");
                        }
                        else if (DateTime.TryParse(FechaConformacion.FechaRecepcion.ToString(), out ValidDatetime)) //Cuando tiene Fecha Recepción (muestras anteriores a 22/07/2020)
                        {
                            FechaAnalisis = DateTime.Parse(FechaConformacion.FechaRecepcion.ToString()).ToString("dd/MM/yy");
                        }
                        else if (DateTime.TryParse(FechaConformacion.Fecha.ToString(), out ValidDatetime))
                        {
                            FechaAnalisis = DateTime.Parse(FechaConformacion.Fecha.ToString()).ToString("dd//MM/yy");
                        }
                    }
                    else if (DateTime.TryParse(FechaConformacion.Fecha.ToString(), out ValidDatetime)) //Cuando no tiene compuesta.
                    {
                        FechaAnalisis = DateTime.Parse(FechaConformacion.Fecha.ToString()).ToString("dd/MM/yy");
                    }
                }

                Analisis_rp.Add(new AnalisisLista
                {
                    Firmado_ = i.Firmada,
                    FirmaVisible_ = i.VerificaBanderaMuestraCompuesta(),
                    AnaisisID_ = i.AnalisisID,
                    AnaisisPadreID_ = i.AnalisisPadreID,
                    EstatusID_ = i.EstatusID,
                    Procesado = i.Procesado,
                    FaltaPago = i.FaltaPago,
                    FechaRegistro_ = FechaAnalisis,
                    MuestrasSimples_ = i.OrdenTrabajo.FrecuenciaMuestreo.MuestrasSimples,
                    Enviado_ = i.Firmada == true ? "~/image/botones/Enviado.png" : "~/image/botones/NoEnviado.png",
                    EnviadoBool_ = i.Firmada,
                    Consecutivo_ = (i.OrdenTrabajo.Norma.RequiereCaudal ? "PD" : "") + Consecutivo++,
                    Complementario_ = i.Firmada && (i.OrdenTrabajo.Norma.Norma1?.Any() ?? false) && (i.Analisis1?.Any() ?? false),
                    AnalisisEncolado_ = i.AnalisisColaEncabezadoes.Any(ace => ace.AnalisisColaDetalle.Activo)
                });

                RequiereCaudal = ListAnalisis.Count > 0 && ListAnalisis.FirstOrDefault().OrdenTrabajo.Norma.RequiereCaudal;
                txtPanel.InnerText = RequiereCaudal ? "Muestra compuesta" : "Muestra";
            }

            pnlAnalisis.Visible = Analisis_rp.Count > 0;

            if (Analisis_rp.Count > 0)
            {
                rptTitulo.DataSource = Analisis_rp;
                rptTitulo.DataBind();
            }

            //RFD - 20/07/2020 -> Requiere caudal se agrega para definir si se muestra la tabla completa o si se ocultan algunas columnas.

        }
        private DatosFirma_Args ObtenerDatosAFirmar()
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FIEL.FirmaElectronica();
            var bandera = false;
            foreach (RepeaterItem dato in rptTitulo.Items)
            {
                #region Datos a firmar
                int analisisID = 0;
                int.TryParse(((HiddenField)dato.FindControl("hfAnalisisID")).Value, out analisisID);

                SIRALAB_DAL.Analisi analisisDal = new Analisi();
                Analisis_BL analisisBL = new Analisis_BL();
                analisisDal = analisisBL.Obten_Analisis(analisisID);

                //validacion para que solo funcione este codigo con la norma SST,DQO/COT
                #region SST,DQO/COT
                //ApplicationSettings applicationSettings = new ApplicationSettings();
                //var NombreNormaConfig = applicationSettings.NormaCOT;

                //OrdenTrabajo_BL OT = new OrdenTrabajo_BL();
                //var ordenTrabajo = OT.Obten(this.OrdenTrabajoID);
                //var NombreNorma = ordenTrabajo.Norma.Nombre.Replace(" ", "");

                //if (NombreNormaConfig == NombreNorma)
                //{
                //    //Datos del analisis actual evaluado
                //    var mesAnalisis = analisisDal.Muestras.First().FechaConformacion.Value.Month;
                //    var AñoAnalisis = analisisDal.Muestras.First().FechaConformacion.Value.Year;
                //    var muestraCompuesta = analisisDal.Muestras.FirstOrDefault(x => x.NivelID == 2);
                //    //lista los analisis de la orden de trabajo actual 
                //    var listaAnalisis = analisisBL.Lista_Analisis(this.OrdenTrabajoID);

                //    Siralab_Entities siralab_Entities = new Siralab_Entities();
                //    int trimestreMuestra = (int)Math.Ceiling((double)mesAnalisis / 3);
                //    var query = from ot in siralab_Entities.OrdenTrabajoes
                //                join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                //                join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                //                join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                //                where ot.NormaID == 16 && ot.ClienteID == analisisDal.OrdenTrabajo.ClienteID
                //                && ot.Anio == AñoAnalisis && _a.NormaDQOCOT != null
                //                && _a.Firmada == true && _a.EstatusID != 4 && ot.ConsecionID == ordenTrabajo.ConsecionID && ot.AnexoID == ordenTrabajo.AnexoID
                //                orderby _a.AnalisisID descending
                //                select new
                //                {
                //                    _a.AnalisisID,
                //                    Fecha = m.FechaConformacion,
                //                    Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),
                //                    _a.NormaDQOCOT
                //                };

                //    if (query != null)
                //    {
                //        var AnalisisEncontrado = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != muestraCompuesta.AnalisisID).FirstOrDefault();
                //        if (AnalisisEncontrado.NormaDQOCOT != analisisDal.NormaDQOCOT)
                //        {
                //            bandera = true;
                //            continue;
                //        }
                //    }
                //}
                //if (bandera == true)
                //{
                //    var DatosDelMensaje = new Dictionary<string, string>();
                //    var CuerpoDelMensaje = "Cuerpo del mensaje";
                //    var TituloDelMensaje = "Mensaje del sistema";
                //    DatosDelMensaje.Add("Cuerpo", CuerpoDelMensaje);
                //    DatosDelMensaje.Add("Titulo", TituloDelMensaje);
                //    MensajeSSTDQOCOT(this, DatosDelMensaje);
                //}
                #endregion

                if (analisisDal != null)
                {
                    var chkAnalisisFirmado = ((CheckBox)dato.FindControl("chkAnalisisFirmado"));

                    if (chkAnalisisFirmado != null && chkAnalisisFirmado.Visible)
                    {
                        bool seleccionado = chkAnalisisFirmado.Checked;
                        bool noGuardado = chkAnalisisFirmado.Enabled;

                        if (seleccionado && noGuardado)
                        {
                            string Llave = analisisDal.AnalisisID.ToString();
                            FIEL.CadenaOriginal cadenaOriginal = new CadenaOriginal();

                            cadenaOriginal.Datos.Add("RFC Laboratorio: " + analisisDal.Laboratorio.RFC);
                            cadenaOriginal.Datos.Add("Nombre Laboratorio: " + analisisDal.Laboratorio.Nombre);
                            cadenaOriginal.Datos.Add("RFC Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.RFC);
                            cadenaOriginal.Datos.Add("Nombre Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.Nombre);
                            cadenaOriginal.Datos.Add("Fecha Análisis: " + analisisDal.OrdenTrabajo.FechaEmision.ToShortDateString());
                            cadenaOriginal.Datos.Add("Título de concesión: " + analisisDal.Consecion.Titulo);
                            cadenaOriginal.Datos.Add("Número de anexo: " + analisisDal.Anexo.Anexo1);
                            cadenaOriginal.Datos.Add("Nombre del punto de descarga: " + analisisDal.Anexo.NombreCuerpoReceptor);
                            /*cadenaOriginal.Datos.Add("Periodo: " + analisisDal.OrdenTrabajo.FrecuenciaReporte.Nombre);*/
                            cadenaOriginal.Datos.Add("Fecha de presentación del informe: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                            cadenaOriginal.Datos.Add("Número de Informe: " + analisisDal.OrdenTrabajo.NumeroInforme);
                            cadenaOriginal.Datos.Add("Norma: " + analisisDal.OrdenTrabajo.Norma.Nombre);
                            cadenaOriginal.Datos.Add("Frecuencia de muestreo: " + analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
                            /*cadenaOriginal.Datos.Add("Resultado: " + analisisDal.Resultado());*/

                            resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);
                        }
                    }
                }
                #endregion
            }
            return resultado;
        }
        private DatosFirma_Args ObtenerDatosAMarcarFaltaDePago(int analisisID)
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FIEL.FirmaElectronica();

            foreach (RepeaterItem dato in rptTitulo.Items)
            {
                SIRALAB_DAL.Analisi analisisDal = new Analisi();
                Analisis_BL analisisBL = new Analisis_BL();
                analisisDal = analisisBL.Obten_Analisis(analisisID);

                if (analisisDal != null)
                {
                    string Llave = analisisDal.AnalisisID.ToString();
                    FIEL.CadenaOriginal cadenaOriginal = new CadenaOriginal();

                    cadenaOriginal.Datos.Add("RFC Laboratorio: " + analisisDal.Laboratorio.RFC);
                    cadenaOriginal.Datos.Add("Nombre Laboratorio: " + analisisDal.Laboratorio.Nombre);
                    cadenaOriginal.Datos.Add("RFC Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.RFC);
                    cadenaOriginal.Datos.Add("Nombre Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.Nombre);
                    cadenaOriginal.Datos.Add("Fecha Análisis: " + analisisDal.OrdenTrabajo.FechaEmision.ToShortDateString());
                    cadenaOriginal.Datos.Add("Título de concesión: " + analisisDal.Consecion.Titulo);
                    cadenaOriginal.Datos.Add("Número de anexo: " + analisisDal.Anexo.Anexo1);
                    cadenaOriginal.Datos.Add("Nombre del punto de descarga: " + analisisDal.Anexo.NombreCuerpoReceptor);
                    /*cadenaOriginal.Datos.Add("Periodo: " + analisisDal.OrdenTrabajo.FrecuenciaReporte.Nombre);*/
                    cadenaOriginal.Datos.Add("Fecha de presentación del informe: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    cadenaOriginal.Datos.Add("Número de Informe: " + analisisDal.OrdenTrabajo.NumeroInforme);
                    cadenaOriginal.Datos.Add("Norma: " + analisisDal.OrdenTrabajo.Norma.Nombre);
                    cadenaOriginal.Datos.Add("Frecuencia de muestreo: " + analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
                    /*cadenaOriginal.Datos.Add("Resultado: " + analisisDal.Resultado());*/

                    resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);
                }
            }
            return resultado;
        }



        public void Genera_Muestra(int AnalisisPadreID, int AnalisisID)
        {
            Muestra_BL muestra_bl = new Muestra_BL();

            var analisisLista = muestra_bl.Lista_Muestra(AnalisisPadreID);

            foreach (var muestra in analisisLista)
            {
                var muestraClon = muestra_bl.ClonarMuestra(muestra, AnalisisID);

                var muestreadorBL = new Muestreador_BL();
                muestreadorBL.ClonarMuestreador(muestraClon.MuestraID, muestra.Muestreadors?.FirstOrDefault(m => m.Activo == true)?.LaboratorioSignatarioID);

                if (muestraClon != null)
                {
                    Genera_MuestraValor(muestra.MuestraID, muestraClon.MuestraID);
                }
            }
        }
        public void Genera_MuestraValor(int MuestraIdOriginal, int MuestraIdClon)
        {
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            var lista = muestravalor_bl.ObtenerMuestraValores(MuestraIdOriginal);

            foreach (var muestraValor in lista)
            {
                muestravalor_bl.ClonarMuestraValor(muestraValor, MuestraIdClon);
            }
        }

        protected void rptTitulo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //RFD - 23/07/2020 Se agrega reglas para las reglas específicas de SDT.
            if (e.Item.ItemType == ListItemType.Header)
            {
                var lblThAnalisis = (Label)e.Item.FindControl("lblThAnalisis");
                lblThAnalisis.Text = RequiereCaudal ? "Muestra compuesta" : "Muestra";


                var thMuestra = (Control)e.Item.FindControl("thMuestra");
                thMuestra.Visible = RequiereCaudal;

                var lblThFechaConformacion = (Label)e.Item.FindControl("lblThFechaConformacion");
                lblThFechaConformacion.Text = RequiereCaudal ? "Fecha de conformacion" : "Fecha";

            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var itemLista = (AnalisisLista)(e.Item.DataItem);
                var lnkbtnComplementar = (LinkButton)e.Item.FindControl("lnkbtnComplementar");
                var lnkbtnFaltaPago = (LinkButton)e.Item.FindControl("lnkbtnFaltaPago");
                var lnkbtnEliminar = (LinkButton)e.Item.FindControl("lnkbtnEliminar");
                var lnkbtnSeleccionar = (LinkButton)e.Item.FindControl("lnkbtnSeleccionar");
                var lnkbtnCancelar = (LinkButton)e.Item.FindControl("lnkbtnCancelar");
                var lnkbtnAsociar = (LinkButton)e.Item.FindControl("lnkbtnAsociar");
                var lnkbtnHeredado = (LinkButton)e.Item.FindControl("lnkbtnHeredado");

                var lblAnalisis = (Label)e.Item.FindControl("lblAnalisis");
                var lblFecha = (Label)e.Item.FindControl("lblFecha");
                var lblMuestra = (Label)e.Item.FindControl("lblMuestra");

                //RFD - 23/07/2020 Se agrega para ocultar la columna Muestras simples cuando es SDT.
                var tdMuestra = (Control)e.Item.FindControl("tdMuestra");
                tdMuestra.Visible = RequiereCaudal;

                /* Muestra u oculta los botones de acción */
                var activo = itemLista.EstatusID_ == enumEstatusAnalisis.Activo.ParseToInt();
                if (activo)
                {
                    lblAnalisis.CssClass = "LabelGreen";
                    lblFecha.CssClass = "LabelGreen";
                    lblMuestra.CssClass = "LabelGreen";
                }
                else
                {
                    lblAnalisis.CssClass = "LabelRed";
                    lblFecha.CssClass = "LabelRed";
                    lblMuestra.CssClass = "LabelRed";
                }

                bool verbtnComplementar = false;
                bool verbtnFaltaPago = false;
                bool verbtnEliminar = false;
                bool verbtnCancelar = false;
                bool verbtnAsociar = false;
                bool verbtnSeleccionar = !itemLista.Firmado_;
                var verbtnHeredado = itemLista.AnaisisPadreID_ != null && itemLista.AnaisisPadreID_ > 0;

                if (this.Login.UsuarioRolID == enumUsuarioRol.Laboratorio.ParseToInt())
                {
                    lnkbtnSeleccionar.ToolTip = "Editar";
                    lnkbtnFaltaPago.CssClass = "btn btn-default btn-xs";
                    lnkbtnFaltaPago.ToolTip = "Marcar falta de pago";
                    verbtnCancelar = false;
                    verbtnComplementar = itemLista.Firmado_ && activo && itemLista.Complementario_;
                    verbtnFaltaPago = itemLista.Firmado_ && !itemLista.FaltaPago && activo;
                    verbtnEliminar = !itemLista.Firmado_ && activo;

                    verbtnAsociar = itemLista.Firmado_ && !itemLista.FaltaPago && !activo && !verbtnHeredado;
                }
                else if (this.Login.UsuarioRolID == enumUsuarioRol.Recaudacion.ParseToInt())
                {
                    lnkbtnSeleccionar.ToolTip = "Consultar";
                    lnkbtnFaltaPago.CssClass = "btn btn-default btn-xs";
                    lnkbtnFaltaPago.ToolTip = "Quitar falta de pago";
                    verbtnCancelar = activo && !itemLista.FaltaPago;
                    verbtnComplementar = false;
                    verbtnFaltaPago = itemLista.FaltaPago && activo;
                    verbtnEliminar = false;

                    verbtnAsociar = false;
                }
                else
                {
                    lnkbtnSeleccionar.ToolTip = "Consultar";

                    verbtnComplementar = false;
                    verbtnFaltaPago = false;
                    verbtnEliminar = false;
                    verbtnCancelar = false;
                    verbtnAsociar = false;
                }

                lnkbtnComplementar.Visible = verbtnComplementar;
                lnkbtnFaltaPago.Visible = verbtnFaltaPago;
                lnkbtnEliminar.Visible = verbtnEliminar;
                lnkbtnSeleccionar.Visible = verbtnSeleccionar;
                lnkbtnCancelar.Visible = verbtnCancelar;
                lnkbtnAsociar.Visible = verbtnAsociar;
                lnkbtnHeredado.Visible = verbtnHeredado;

                

                #region Comentado
                /*
                                //Validación si el análisis ya fue enviado.

                                lnkbtnEliminar.Visible = !itemLista.EnviadoBool_;

                                lnkbtnFaltaPago.Visible = true;
                                lnkbtnComplementar.Visible = true;

                                //Configuramos las acciones de los botones
                                if (this.Login.UsuarioRolID == 2)
                                {
                                    ///----------------------------------------------------------------------------------------
                                    lnkbtnSeleccionar.ToolTip = "Consultar";


                                    if (itemLista.FaltaPago)
                                    {
                                        lnkbtnFaltaPago.Visible = true;
                                        lnkbtnComplementar.Visible = false;
                                    }

                                    lnkbtnAsociar.Visible = false;


                                    if (itemLista.EstatusID_ == enumEstatusAnalisis.Cancelado.ParseToInt())
                                        lnkbtnCancelar.Visible = false;
                                    else
                                        lnkbtnCancelar.Visible = true;
                                    ///----------------------------------------------------------------------------------------
                                }
                                else
                                {
                                    lnkbtnSeleccionar.ToolTip = "Editar";
                                    lnkbtnCancelar.Visible = false;

                                    var Asociacion = (AnalisisLista) e.Item.DataItem;

                                    if (!Asociacion.Firmada)
                                    {
                                        lnkbtnFaltaPago.Visible = true;

                                        if (Asociacion.FaltaPago)
                                        {
                                            if (Login.CatUsuarioRol.UsuarioRolID == 1)
                                            {
                                                lnkbtnFaltaPago.Visible = false;
                                                lnkbtnComplementar.Visible = false;
                                            }

                                            lnkbtnFaltaPago.CssClass = "icono-faltapago-16";
                                            lnkbtnFaltaPago.ToolTip = "Quitar falta de pago";
                                        }
                                        else
                                        {
                                            lnkbtnFaltaPago.CssClass = "icono-pago-16";
                                            lnkbtnFaltaPago.ToolTip = "Marcar falta de pago";
                                        }
                                    }
                                    else lnkbtnFaltaPago.Visible = true;

                                    //Validacion realizada para mostrar o no mostrar el boton de asociar un nuevo analisis
                                    if (Asociacion.EstatusID_ == 4)
                                    {
                                        //Verificamos si ya se cuenta un registro de analisis con en ID padre
                                        Analisis_BL analisis_bl = new Analisis_BL();
                                        var an = analisis_bl.Lista_Analisis_Padre((int) Asociacion.AnaisisID_);
                                        //Si la lista tiene un numero mayor a cero significa que ya fue asociado en algun momento un nuevo análisis
                                        if (an.Count > 0)
                                            lnkbtnAsociar.Visible = false;
                                        else
                                            lnkbtnAsociar.Visible = true;

                                        lblAnalisis.CssClass = "LabelRed";
                                        lblFecha.CssClass = "LabelRed";
                                        lblMuestra.CssClass = "LabelRed";
                                    }
                                    else
                                    {
                                        lblAnalisis.CssClass = "LabelGreen";
                                        lblFecha.CssClass = "LabelGreen";
                                        lblMuestra.CssClass = "LabelGreen";

                                        lnkbtnAsociar.Visible = false;
                                    }

                                    //Verificamos el tipo de fuente, en caso que el campo PADREID este diferente de nulo se pintara en rojo, de lo contrario se pintara en verde
                                    if (Asociacion.AnaisisPadreID_ > 0)
                                        lnkbtnHeredado.Visible = true;
                                    else
                                        lnkbtnHeredado.Visible = false;

                                    if ((Asociacion.Firmado_ || Asociacion.Firmada) && this.Login.UsuarioRolID != 1 &&
                                        Asociacion.AnaisisPadreID_ == null && (!Asociacion.FaltaPago || Asociacion.FaltaPago))
                                    {
                                        lnkbtnComplementar.Visible = true;
                                    }
                                    else lnkbtnComplementar.Visible = false;

                                    //Validación utilizada para verificar si se encuentra firmado o no el análisis, en caso que se encuentre firmado
                                    //se oculta el boton de seleccionar
                                    var btnEditarVisible = (SIRALAB.Controles.AnalisisLista) e.Item.DataItem;
                                    if (btnEditarVisible.Firmado_)
                                        lnkbtnSeleccionar.Visible = false;
                                    else
                                        lnkbtnSeleccionar.Visible = true;
                                } */

                #endregion
            }
        }


        protected void lnkbtnSeleccionar_Click(object sender, EventArgs e)
        {
            int AnalisisID = int.Parse(((LinkButton)sender).CommandArgument);
            this.AnalisisID = AnalisisID;
            if (AnalisisID > 0)
            {
                Analisis_BL analisis_bl = new Analisis_BL();
                this.Analisis = analisis_bl.Obten_Analisis(AnalisisID);
                Session["BackUri"] = Request.Url.AbsoluteUri;
                Response.Redirect("~/Analisis.aspx");
            }
        }
        protected void lnkbtnFirmar_Click(object sender, EventArgs e)
        {
            FirmarAnalisis?.Invoke(this, ObtenerDatosAFirmar());
        }

        protected void lnkbtnCancelar_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(((LinkButton)sender).CommandArgument);

            Analisis_BL analisis_bl = new Analisis_BL();
            SIRALAB_DAL.Analisi analisisArgs = new SIRALAB_DAL.Analisi();
            analisisArgs = analisis_bl.Obten_Analisis(ID);
            Cancelar(this, analisisArgs);
        }
        protected void lnkbtnAsociar_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(((LinkButton)sender).CommandArgument);
            Analisis_BL an_bl = new Analisis_BL();

            var an = an_bl.Obten_Analisis(ID);

            AnalisisCapturaArgs Args = new AnalisisCapturaArgs();
            Args.AnalisisPadreID = ID;
            Args.OrdenTrabajoID = an.OrdenTrabajoID;
            Args.AnexoID = an.AnexoID;
            Args.LaboratorioID = an.LaboratorioID;
            Args.EstatusID = 3;
            Args.Firmada = false;
            Args.FechaFirmada = null;
            Args.Enviada = false;
            Args.Procesado = false;
            Args.FechaProceso = null;
            Args.FechaRegistro = DateTime.Now;
            Args.ConsecionID = an.ConsecionID;

            var analisis = an_bl.Guarda(Args);

            if (analisis != null)
                Genera_Muestra(ID, analisis.AnalisisID);

            CargaInformacion(an.OrdenTrabajoID);
        }
   protected void lnkbtnAcuse_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(((LinkButton)sender).CommandArgument);
            Analisis_BL an = new Analisis_BL();
            an.Obten_Analisis(ID);
            ApplicationSettings applicationSettings = new ApplicationSettings();
            var fechaNuevoReportes = applicationSettings.fechaNuevosReportes;

            if (an.Analisis.FechaRegistro > fechaNuevoReportes)
            {
                //var informeLabLogic = new InformeLaboratorio_Logic();

                ////Se requiere para generar la url de la imagen.
                //informeLabLogic.ServerUtility = System.Web.HttpContext.Current.Server;
                //var stream = informeLabLogic.CrearPDF(ID);
                //Session["popReport"] = stream?.ToArray();

                var clienteApiReportes = new ClienteApiReportes();
                Session["popReport"] = clienteApiReportes.ObtenInformeLaboratorioAcuse(ID);

            }

            if (Session["popReport"] != null)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Mensaje", "open('pop.aspx', 'Page','top=50,left=20, width=800,height=600,status=yes, resizable=yes,scrollbars=yes')", true);
            }
        }
        protected void lnkbtnDocumento_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(((LinkButton)sender).CommandArgument);
            Analisis_BL an = new Analisis_BL();
            an.Obten_Analisis(ID);
            ApplicationSettings applicationSettings = new ApplicationSettings();
            var fechaNuevoReportes = applicationSettings.fechaNuevosReportes;

            if (an.Analisis.FechaRegistro > fechaNuevoReportes)
            {
                //var informeLabLogic = new InformeLaboratorio_Logic();

                ////Se requiere para generar la url de la imagen.
                //informeLabLogic.ServerUtility = System.Web.HttpContext.Current.Server;
                //var stream = informeLabLogic.CrearPDF(ID);
                //Session["popReport"] = stream?.ToArray();

                var clienteApiReportes = new ClienteApiReportes();
                Session["popReport"] = clienteApiReportes.ObtenInformeLaboratorio(ID);

            }
            else
            {
                string nombreReport = an.Analisis.OrdenTrabajo.Norma.RequiereCompuesta ? "AnalisisFirmado" : "AnalsisisSDT";
                List<ReportParameter> listParametros = new List<ReportParameter>();
                ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", ID.ToString());
                listParametros.Add(rpAnalisisID);
                Session["popReport"] = OpenReports.BufferPDFRemoteReport(nombreReport, listParametros); ;
            }

            if (Session["popReport"] != null)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Mensaje", "open('pop.aspx', 'Page','top=50,left=20, width=800,height=600,status=yes, resizable=yes,scrollbars=yes')", true);
            }
        }



        protected void lnkbtnEliminar_Click(object sender, EventArgs e)
        {
            int AnalisisID = int.Parse(((LinkButton)sender).CommandArgument);

            Eliminar(this, AnalisisID);
        }
        protected void lnkbtnFaltaPago_Click(object sender, EventArgs e)
        {
            int analisisID = int.Parse(((LinkButton)sender).CommandArgument);
            DatosFirma_Args resultado = new DatosFirma_Args
            {
                DatosAFirmar = new FirmaElectronica()
            };

            Analisi analisisDal = new Analisi();
            Analisis_BL analisisBL = new Analisis_BL();
            analisisDal = analisisBL.Obten_Analisis(analisisID);

            if (analisisDal != null)
            {
                string Llave = analisisDal.AnalisisID.ToString();
                CadenaOriginal cadenaOriginal = new CadenaOriginal();

                cadenaOriginal.Datos.Add("RFC Laboratorio: " + analisisDal.Laboratorio.RFC);
                cadenaOriginal.Datos.Add("Nombre Laboratorio: " + analisisDal.Laboratorio.Nombre);
                cadenaOriginal.Datos.Add("RFC Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.RFC);
                cadenaOriginal.Datos.Add("Nombre Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.Nombre);
                cadenaOriginal.Datos.Add("Fecha Análisis: " + analisisDal.OrdenTrabajo.FechaEmision.ToShortDateString());
                cadenaOriginal.Datos.Add("Título de concesión: " + analisisDal.Consecion.Titulo);
                cadenaOriginal.Datos.Add("Número de anexo: " + analisisDal.Anexo.Anexo1);
                cadenaOriginal.Datos.Add("Nombre del punto de descarga: " + analisisDal.Anexo.NombreCuerpoReceptor);
                cadenaOriginal.Datos.Add("Número de Informe: " + analisisDal.OrdenTrabajo.NumeroInforme);
                cadenaOriginal.Datos.Add("Norma: " + analisisDal.OrdenTrabajo.Norma.Nombre);

                resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);
            }

            FaltaPago?.Invoke(this, new FaltaPagoArgs
            {
                AnalisisID = analisisID,
                Firma = resultado
            });
        }
        protected void wucAnalisisComplementario_Guardar(object sender, AnalisisComplementarioArg e)
        {
            int AnalisisID = int.Parse(((LinkButton)sender).CommandArgument);

            //if (log.CambiarContrasena(usr, e.PasswordActual, e.PasswordNuevo, e.PasswordRepite, e.Correo))
            //{
            //    var resp = log.CambioContrasena(e.Correo);
            //    this.Page.PopMensaje(log.Mensaje, "Cambio de contraseña", controles.Opciones.SoloAceptar, controles.Icono.ok, "MostrarCambioPassOk");
            //}
            //else
            //{
            //    this.Page.PopMensaje(log.Mensaje, "Error", controles.Opciones.SoloAceptar, controles.Icono.error, "MostrarCambioPass");
            //}
        }
        protected void wucAnalisisComplementario_Cerrar(object sender, EventArgs e)
        {

        }
        protected void wucAnalisisComplementario_Cancelar(object sender, EventArgs e)
        {

        }
        protected void lnkbtnComplementar_Click(object sender, EventArgs e)
        {
            int AnalisisID = int.Parse(((LinkButton)sender).CommandArgument);
            Session["AnalisisID_"] = AnalisisID;
            wucAnalisisComplementario.Mostrar();
        }

        protected void linkSubirArchivo_OnClick(object sender, EventArgs e)
        {
            var analisisId = int.Parse(((LinkButton)sender).CommandArgument);


            ScriptManager.RegisterStartupScript(this, this.GetType(), "txtAnalisisID", "setAnalisisIDModal(" + analisisId + ");", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
    }

    public class FaltaPagoArgs : EventArgs
    {
        public int AnalisisID { get; set; }
        public DatosFirma_Args Firma { get; set; }
    }
}