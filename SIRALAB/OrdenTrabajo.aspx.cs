using System.Globalization;
using Microsoft.Reporting.WebForms;
using SIRALAB.Common;
using SIRALAB.Controles;
using SIRALAB.FIEL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_DAL;
using SIRALAB_DAL.Extensiones;
using System;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using SIRALAB.controles;
using SIRALAB.Queue;
using SIRALAB.Avisos;

namespace SIRALAB
{
    public partial class OrdenTrabajo : System.Web.UI.Page
    {
        #region Propiedades
        public int OrdenTrabajoID
        {
            get { return (int)Session["OrdenTrabajoID_"] == 0 ? 0 : (int)Session["OrdenTrabajoID_"]; }
            set { Session["OrdenTrabajoID_"] = value; }
        }
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        public int AnalisisID
        {
            get { return ViewState["AnalisisID"] == null ? 0 : int.Parse(ViewState["AnalisisID"].ToString()); }
            set { ViewState["AnalisisID"] = value; }
        }

        public AnalisisFirmaArgs AnalisisFirmaArgs
        {
            get { return Session["AnalisisFirmaArgs_Faltapago"] == null ? new AnalisisFirmaArgs() : (AnalisisFirmaArgs)Session["AnalisisFirmaArgs_Faltapago"]; }
            set { Session["AnalisisFirmaArgs_Faltapago"] = value; }
        }

        #endregion
        public string UsuarioRolID { get { return Session["UsuarioRolID"].ToString(); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)this.Master.Master;
            main.MensajeDevolucion += main_MensajeDevolucion;
            wucOrdenTrabajo1.RespuestaOrdenTrabajo += wucOrdenTrabajo1_RespuestaOrdenTrabajo;
            HttpContext.Current.Session["ExtPagerf"] = 0;
            HttpContext.Current.Session["ExtPagerNOf"] = 0;
            wucOrdenAnalisis1.MensajeSSTDQOCOT += MensajeNomSSTDQOCOT;
            if (!IsPostBack)
            {
                if (this.Login.UsuarioRolID == 2)
                    wucFirma.Visible = false;
                else
                    wucFirma.Visible = true;
                Session["UsuarioRolID"] = this.Login.UsuarioRolID;

                if (this.OrdenTrabajoID == 0)
                {
                    wucEvidencias.listaEvidencias = null;
                }

                if (this.OrdenTrabajoID > 0)
                {
                    CargaInformacion(this.OrdenTrabajoID);
                }
                else
                {
                    wucConsultaCliente1.CargaClientes();
                    pnlPtoDescarga.Enabled = false;
                    pnlNormasMexicanas.Enabled = false;
                    //TODO -> Verificar.
                    divPnlPuntoDescarga.Visible = false;
                    divNormasMexicanas.Visible = false;
                }
            }
        }
        public void CargaInformacion(int OrdenTrabajoID)
        {
            //Declaramos y obtenemos la orden de trabajo
            OrdenTrabajo_BL ordentrabajo_bl = new OrdenTrabajo_BL();
            var orden = ordentrabajo_bl.Obten(this.OrdenTrabajoID);

            //Con los datos de la orden de trabajo obtenemos el cliente 
            Cliente_BL cliente_bl = new Cliente_BL();
            //Validamos que la orden de trabajo contenga información
            if (orden != null)
            {
                lblFolio.Text = "Número: " + orden.NumeroInforme;
                cliente_bl.obtener(orden.ClienteID);
                wucConsultaCliente1.Cliente = cliente_bl.Cliente;
                wucConsultaCliente1.CargaInformacionCliente_Consulta();

                Analisis_BL analisis_bl = new Analisis_BL();
                var analisis = analisis_bl.Obten_Analisis_Orden(OrdenTrabajoID);
                if (analisis != null)
                {
                    wucOrdenAnalisis1.CargaInformacion(orden.OrdenTrabajoID);
                }
                wucConsultaPtoDescarga1.OrdenTrabajo = orden;
                wucConsultaPtoDescarga1.CargaInformacion_Consulta((int)orden.ConsecionID, (int)orden.AnexoID);
                wucOrdenTrabajo1.OrdenTrabajo = orden;
                wucOrdenTrabajo1.CargaOrdenTrabajo_Consulta();

                //wucEvidencias.Enabled(true);
                if (orden.OrdenTrabajoID == 0)
                    wucEvidencias.listaEvidencias = null;

                //wucEvidencias.Enabled(true);
                if (orden.OrdenTrabajoID == 0)
                    wucEvidencias.listaEvidencias = null;

                wucEvidencias.subirArchivosOT = orden.Analisis.Any(a => a.Firmada);
                wucEvidencias.CargarEvidencias(orden);
            }
        }
        private string[] ToRecaudacion(int AnalisisID)
        {
            ArrayList arr = new ArrayList();



            /*Laboratorio*/
            Analisis_BL anBL = new Analisis_BL();
            Analisi analisi = anBL.Obten_Analisis(AnalisisID);

            foreach (Usuario usLab in analisi.Laboratorio.Usuarios)
            {
                if (usLab.Correo != "correo@correo.com" && usLab.Correo != "corre@correo.com")
                {
                    arr.Add(usLab.Correo);
                }
            }

            /*cliente*/
            arr.Add(analisi.OrdenTrabajo.Cliente.Correo);

            return (String[])arr.ToArray(typeof(string));
        }
        private void sendMail(int AnalisisID)
        {
            ApplicationSettings ap = new ApplicationSettings();
            SIRALAB_DAL.Analisi analisisDal = new Analisi();
            Analisis_BL analisisBL = new Analisis_BL();
            analisisDal = analisisBL.Obten_Analisis(AnalisisID);

            string nombreReportAnalisis = analisisDal.OrdenTrabajo.Norma.RequiereCompuesta ? "AnalisisFirmado" : "AnalsisisSDT";
            string nombreReportAcuse = analisisDal.Procesado ? "AcuseFirmadoVencido" : "AcuseFirmado";

            List<ReportParameter> listParametrosAnalisisFirmado = new List<ReportParameter>();
            ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            listParametrosAnalisisFirmado.Add(rpAnalisisID);

            var fileAnalisisPdf = OpenReports.BufferPDFRemoteReport(nombreReportAnalisis, listParametrosAnalisisFirmado);

            List<ReportParameter> listParametrosAcuse = new List<ReportParameter>();
            string ubicacionDeMuestra = "";
            Muestra muestra = analisisDal.Muestras.FirstOrDefault();
            ubicacionDeMuestra += "Calle: " + muestra.Direccion.Calle;
            ubicacionDeMuestra += " Num: " + muestra.Direccion.NumeroExterior;
            ubicacionDeMuestra += ", " + muestra.Direccion.NumeroInterior;
            ubicacionDeMuestra += ", Colonia: " + muestra.Direccion.Colonia.Nombre;
            ubicacionDeMuestra += " Localidad: " + muestra.Direccion.Localidad.Nombre;
            ubicacionDeMuestra += " Municipio: " + muestra.Direccion.Municipio.Nombre;
            ubicacionDeMuestra += " Estado: " + muestra.Direccion.Estado.Nombre;
            ubicacionDeMuestra += " C.P. " + muestra.Direccion.CP;
            ubicacionDeMuestra += " Longitud: " + muestra.Longitud;
            ubicacionDeMuestra += " Latitud: " + muestra.Latitud;

            SIRALAB_DAL.Norma norma = analisisDal.OrdenTrabajo.Norma;
            string normaOParametros = string.Empty;
            if ((bool)norma.Evaluacion)
            {
                normaOParametros += norma.Nombre;
            }
            else
            {
                string strMuestraValor = string.Empty;
                analisisDal.Muestras.ToList().ForEach(mst => mst.MuestraValors.ToList().ForEach(mv => strMuestraValor += mv.Parametro.Nombre + " "));
                normaOParametros += strMuestraValor;
            }

            ReportParameter paramAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            ReportParameter paramNumInforme = new ReportParameter("NumeroDeInforme", analisisDal.OrdenTrabajo.NumeroInforme.ToString());
            ReportParameter paramHoraDFirma = new ReportParameter("HoraDeFirma", DateTime.Now.ToShortTimeString());
            ReportParameter paramNomPuntDes = new ReportParameter("NombrePuntoDeDescarga", analisisDal.Anexo.NombreCuerpoReceptor != null && analisisDal.Anexo.NombreCuerpoReceptor != "" ? analisisDal.Anexo.NombreCuerpoReceptor : analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
            ReportParameter paramCuerpoRece = new ReportParameter("CuerpoReceptor", analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
            ReportParameter paramFrecMuestr = new ReportParameter("FrecuenciaMuestreo", analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
            ReportParameter paramNormaOPmts = new ReportParameter("NormaOParametros", normaOParametros);
            ReportParameter paramUbicaMuest = new ReportParameter("UbicacionMuestreo", ubicacionDeMuestra);
            string selloDigitalCNA = analisisDal.AnalisisFirmas.Where(a => a.AnalisisID == analisisDal.AnalisisID).Last().SelloDigital;
            ReportParameter paramSelloDeCNA = new ReportParameter("SelloCONAGUA", selloDigitalCNA);
            ReportParameter paramAcuseRegla = new ReportParameter("AcuseRegla", string.Format(ap.AcuseTexto, ap.AcuseRegla, ap.AcuseAnio));

            listParametrosAcuse.Add(paramAnalisisID);
            listParametrosAcuse.Add(paramNumInforme);
            listParametrosAcuse.Add(paramHoraDFirma);
            listParametrosAcuse.Add(paramNomPuntDes);
            listParametrosAcuse.Add(paramCuerpoRece);
            listParametrosAcuse.Add(paramFrecMuestr);
            listParametrosAcuse.Add(paramNormaOPmts);
            listParametrosAcuse.Add(paramUbicaMuest);
            listParametrosAcuse.Add(paramSelloDeCNA);

            if (analisisDal.Procesado)
                listParametrosAcuse.Add(paramAcuseRegla);

            var fileAcusePdf = OpenReports.BufferPDFRemoteReport(nombreReportAcuse, listParametrosAcuse);

            Hashtable ht = new Hashtable();

            ht.Add(nombreReportAnalisis + ".pdf", fileAnalisisPdf);
            ht.Add(nombreReportAcuse + ".pdf", fileAcusePdf);
            ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
            ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));

            string[] ToArr = null;

            /* Llenar con las direcciones a quienes se les enviará el correo */
            ToArr = ToRecaudacion(AnalisisID);

            ApplicationSettings appSettings = new ApplicationSettings();
            if (appSettings.useTestMail)
                ToArr = appSettings.testMail;

            try
            {
                CultureInfo c = new CultureInfo("es-ES");
                var fechaOT = DateTime.Now;
                var fecha = c.DateTimeFormat.GetDayName(fechaOT.DayOfWeek) + " " +
                    fechaOT.Day.ToString() + " " +
                    c.DateTimeFormat.GetMonthName(fechaOT.Month) + " de " +
                    fechaOT.Year.ToString();
                string Message = "Se le informa que la Comisión Nacional de Agua ha recibido con fecha " + fecha +
                            " a las " + DateTime.Now.ToString("HH:mm") + " horas, el informe número " + analisisDal.OrdenTrabajo.NumeroInforme + "/" + analisisBL.AnalisisNumero(AnalisisID) +
                            " del análisis de laboratorio que su representada efectuó para el contribuyente "
                            + (analisisDal.OrdenTrabajo.Cliente.Nombre +
                            " " + (analisisDal.OrdenTrabajo.Cliente.ApellidoPaterno ?? "") +
                            " " + (analisisDal.OrdenTrabajo.Cliente.ApellidoMaterno ?? "")).Trim() +
                            ", con RFC " + analisisDal.OrdenTrabajo.Cliente.RFC +
                            ".";
                foreach (var to in ToArr)
                {
                    Thread threadSendMails = new Thread(delegate ()
                    {
                        SIRALAB_BL.correo.Mailer.SendMail(to, "", "", Message, "Acuse de recibo del informe del análisis de laboratorio", ht);
                    });

                    threadSendMails.IsBackground = true;
                    threadSendMails.Start();
                }
            }
            finally
            {
                GC.Collect();
            }
        }
        protected void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            switch (e.Command)
            {
                #region CancelarAnalisis
                case "CancelarAnalisis":

                    if (e.Opcion == controles.Opcion.Aceptar)
                    {
                        Analisis_BL analisis_bl = new Analisis_BL();

                        var an = analisis_bl.ActualizaEstatus(this.AnalisisID, 4);
                        if (an)
                        {
                            CargaInformacion(this.OrdenTrabajoID);
                            this.PopMensaje("Análisis cancelado exitosamente", "Análisis cancelado", controles.Opciones.SoloAceptar, controles.Icono.ok);
                        }
                    }
                    break;
                #endregion

                #region Eliminar Orden
                case "EliminarOrden":

                    if (e.Opcion == controles.Opcion.Aceptar)
                    {

                        //Elimina un análisis, no la orden de trabajo:
                        int ID = int.Parse(e.Args[0]);
                        Analisis_BL analisis_bl = new Analisis_BL();
                        analisis_bl.Eliminar(ID);
                        wucOrdenAnalisis1.CargaInformacion(this.OrdenTrabajoID);

                        OrdenTrabajo_BL ordenTrabajoBl = new OrdenTrabajo_BL();
                        var orden = ordenTrabajoBl.Obten(this.OrdenTrabajoID);

                        wucEvidencias.subirArchivosOT = orden.Analisis?.Any(a => a.Firmada);
                        wucEvidencias.listaEvidencias.RemoveAll(doc => doc.AnalisisID == ID);
                        wucEvidencias.CargarEvidencias(orden);
                    }
                    break;
                #endregion

                #region FaltaPago
                case "FaltaPago":
                    if (e.Opcion == controles.Opcion.Aceptar)
                    {
                        int ID = int.Parse(e.Args[0]);
                        bool Marca = bool.Parse(e.Args[1]);
                        Analisis_BL analisis_bl = new Analisis_BL();
                        var aFaltaPagoBl = new AnalisisFaltaPago_BL();
                        var result = analisis_bl.FaltaPago(ID);
                        aFaltaPagoBl.Guarda(new AnalisisFaltaPagoArgs
                        {
                            AnalisisID = ID,
                            RFC = this.AnalisisFirmaArgs.RFC,
                            Firma = this.AnalisisFirmaArgs.Firma,
                            Cadena = this.AnalisisFirmaArgs.Cadena,
                            Contenedor = this.AnalisisFirmaArgs.Contenedor,
                            SelloCNA = this.AnalisisFirmaArgs.SelloCNA,
                            Marcado = Marca,
                            FechaRegistro = this.AnalisisFirmaArgs.FechaRegistro
                        });

                        wucOrdenAnalisis1.CargaInformacion(this.OrdenTrabajoID);
                        if (result)
                            this.PopMensaje("Se ha marcado el análisis sin falta de pago correctamente.", "Falta de pago del análisis de trabajo", controles.Opciones.SoloAceptar, controles.Icono.ok, "", null);
                        else
                            this.PopMensaje("Se ha marcado el análisis con falta de pago correctamente.", "Falta de pago del análisis de trabajo", controles.Opciones.SoloAceptar, controles.Icono.ok, "", null);
                    }
                    break;
                    #endregion
            }
        }
        protected void wucOrdenAnalisis_Eliminar(object sender, int e)
        {
            var ID = e;
            string[] arr = new string[] { ID.ToString() };
            this.PopMensaje("¿Está seguro de eliminar el análisis seleccionado?, una vez eliminado no habrá forma de recuperarlo, ¿Realmente desea continuar?", "Eliminar análisis de trabajo", controles.Opciones.SiNo, controles.Icono.warning, "EliminarOrden", arr);
        }
        protected void wucOrdenAnalisis_FaltaPago(object sender, FaltaPagoArgs e)
        {
            if (e.Firma.DatosAFirmar.listaContenedores.Count > 0)
            {
                wucFirmaFaltaPago.enviarAFirma(e.Firma.DatosAFirmar);
                wucFirmaFaltaPago.Visible = true;
            }
        }
        protected void wucOrdenTrabajo1_RespuestaOrdenTrabajo(object sender, EventArgs e)
        {
            var args = (SIRALAB.Controles.RespuestaOrdenTrabajoArgs)e;
            if (args != null)
            {
                wucOrdenAnalisis1.CargaInformacion(int.Parse(args.OrdenTrabajoId.ToString()));
                wucConsultaCliente1.InabilitaPanel();
                wucConsultaPtoDescarga1.InhabilitaPanel();
                wucOrdenTrabajo1.InabilitaPanel();


                this.OrdenTrabajoID = (int)args.OrdenTrabajoId;
                CargaInformacion(this.OrdenTrabajoID);

                this.PopMensaje("Orden de trabajo generada exitosamente", "Orden de trabajo", controles.Opciones.SoloAceptar, controles.Icono.ok);
            }
            else
                this.PopMensaje("Falta proporcionar cliente y anexo", "Cliente y anexo no proporcionados", controles.Opciones.SoloAceptar, controles.Icono.warning);
        }
        protected void wucOrdenTrabajo1_RespuestaAnalisis(object sender, bool e)
        {
            var args = e;
            if (args)
            {
                wucOrdenAnalisis1.CargaInformacion(this.OrdenTrabajoID);

                this.PopMensaje("Análisis generado correctamente.", "Análisis generado", controles.Opciones.SoloAceptar, controles.Icono.ok);
            }
            else
                this.PopMensaje("Ha ocurrido un error al generar el análisis, por favor intente nuevamente.", "Análisis no generado", controles.Opciones.SoloAceptar, controles.Icono.error);
        }
        protected void wucOrdenTrabajo1_FechaErronea(object sender, EventArgs e)
        {
            if (e == null)
            {
                this.PopMensaje("La Fecha de emisión es incorrecta", "Información Incompleta", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
        protected void wucOrdenTrabajo1_Incompleto(object sender, EventArgs e)
        {
            if (e == null)
            {
                this.PopMensaje("Verifique la información capturada", "Información Incompleta", controles.Opciones.SoloAceptar, controles.Icono.warning);
            }
        }
        protected void wucOrdenTrabajo1_NormaSeleccionada(object sender, NormaSeleccionadaArgs arg)
        {
            var aviso = new MensajeLFDCR(Server.MapPath("~/recursos/avisoLFDcambioCR.json"), arg.NormaId);
            if (arg.CuerposReceptores.Any())
            {
                aviso.Show(this.Page, wucOrdenTrabajo1.Anexo);
            }
        }

        protected void wucOrdenAnalisis1_FirmarAnalisis(object sender, DatosFirma_Args e)
        {
            if (e.DatosAFirmar.listaContenedores.Count > 0)
            {
                wucFirma.enviarAFirma(e.DatosAFirmar);
                wucFirma.Visible = true;
            }
        }
        protected void wucFirma_FirmarColeccion(object sender, DatosFirma_Args e)
        {
            FIEL.FirmaElectronica datosFirma;
            datosFirma = e.DatosAFirmar;
            byte[] ArchivoKey = e.ArchivoKey;
            byte[] ArchivoCer = e.ArchivoCer;
            string CNT = e.txtContra;
            List<string> listaErrorFirma = new List<string>();
            var contenedores = new List<Contenedor>();
            var limiteSSTDQO = false;
            var ClorurosConductividad = false;
            var Cloruros = false;

            if (Login.Laboratorio.RFCValidoParaFirmar(e.RFCFirma))
            {
                try
                {
                    FirmaQueue queue = new FirmaQueue();
                    var banderaDetieneCola = true;
                    var banderaCreaRegistros = false;

                    var analisisColaDetalleBL = new AnalisisColaDetalle_BL();
                    var analisisColaEncabezadoBL = new AnalisisColaEncabezado_BL();


                    var date = DateTime.Now;

                    var colaDetalle = new AnalisisColaDetalle()
                    {
                        AnalisisColaDetalleID = 0,
                        Activo = true,
                        Completo = false,
                        FechaCreacion = date,
                    };

                    foreach (var contenedor in datosFirma.listaContenedores)
                    {
                        contenedor.ArchivoCer = ArchivoCer;
                        contenedor.ArchivoKey = ArchivoKey;
                        contenedor.txtRFC = e.RFCFirma;
                        contenedor.txtContra = CNT;

                        if (contenedor.valido) //Como los datos son siempre iguales, si falla el primero fallan todos:
                        {
                            #region ValidacionLimiteNormaSSTDQO
                            try
                            {

                                ApplicationSettings applicationSettings = new ApplicationSettings();
                                var idAnalisis = int.Parse(contenedor.primaryKey);
                                Analisis_BL analisisBL = new Analisis_BL();
                                var analisisDal = analisisBL.Obten_Analisis(idAnalisis);
                                if (analisisDal.OrdenTrabajo.NormaID == 3 || analisisDal.OrdenTrabajo.NormaID == 13)
                                {
                                    var fechaUltimaMuestra = analisisDal.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha;
                                    var fechaLimiteAnalisisSSTDQO = Convert.ToDateTime(applicationSettings.LimiteNormaSSTDQO);
                                    if (fechaUltimaMuestra >= fechaLimiteAnalisisSSTDQO)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que se encuentran fuera de la vigencia de la norma");
                                        limiteSSTDQO = true;
                                        continue;
                                    }
                                }
                                else if (analisisDal.OrdenTrabajo.Norma.Nombre.Replace(" ", "") == applicationSettings.NormaCOT || analisisDal.OrdenTrabajo.Norma.Nombre.Replace(" ", "").ToUpper() == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                                {
                                    var fechaUltimaMuestra = analisisDal.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha;
                                    var fechaLimiteAnalisisSSTDQO = Convert.ToDateTime(applicationSettings.LimiteNormaSSTDQO);
                                    if (fechaUltimaMuestra < fechaLimiteAnalisisSSTDQO)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que se encuentran fuera de la vigencia de la norma");
                                        limiteSSTDQO = true;
                                        continue;
                                    }
                                }


                                if (analisisDal.OrdenTrabajo.NormaID == 15)//valida que sea la norma 2021
                                {
                                    if (analisisDal.NormaDQOCOT == null || analisisDal.NormaDQOCOT == string.Empty || analisisDal.Conductividad == null || analisisDal.Conductividad == string.Empty)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros y/o conductividad eléctrica.");
                                        ClorurosConductividad = true;
                                        continue;
                                    }

                                }
                                else if (analisisDal.OrdenTrabajo.Norma.Nombre.Replace(" ", "").ToUpper() == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))//valida la norma agua clara
                                {
                                    if (analisisDal.NormaDQOCOT == null || analisisDal.NormaDQOCOT == string.Empty)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros.");
                                        Cloruros = true;
                                        continue;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                listaErrorFirma.Add("Ocurrió un error al intentar crear el proceso de firma.Inténtelo de nuevo en unos minutos");
                                break;
                            }

                            #endregion
                            if (banderaDetieneCola) //En la primera iteración crea la cola. Si ocurre un error al crear el registro, se regresa un mensaje de error (no se persiste nada y no se abre un hilo para las firmas).
                            {
                                colaDetalle = analisisColaDetalleBL.Guarda(colaDetalle);
                                banderaDetieneCola = colaDetalle == null;
                                queue.ColaID = !banderaDetieneCola ? (int)colaDetalle.AnalisisColaDetalleID : 0;
                                queue.ServerUtility = System.Web.HttpContext.Current.Server;

                                if (banderaDetieneCola)
                                {
                                    listaErrorFirma.Add("Ocurrió un error al intentar crear el proceso de firma. Inténtelo de nuevo en unos minutos");
                                    break;
                                }
                            }

                            var analisisID = 0;
                            int.TryParse(contenedor.primaryKey, out analisisID);
                            var analisisColaEncabezado = new AnalisisColaEncabezado()
                            {
                                AnalisisColaEncabezadoID = 0,
                                AnalisisColaDetalleID = colaDetalle.AnalisisColaDetalleID,
                                AnalisisID = analisisID,
                                FechaEnvio = DateTime.Now,
                                EstatusID = enumEstatusAnalisis.EnProceso.ParseToInt(),
                                LaboratorioID = Login.LaboratorioID ?? 0
                            };

                            //Si ocurre algún error al persistir el registro se eliminan todos los registros de la cola:
                            banderaCreaRegistros = analisisColaEncabezadoBL.SaveOrUpdate(analisisColaEncabezado) == null;

                            if (banderaCreaRegistros)
                            {
                                analisisColaEncabezadoBL.DeleteAllByColaID((int)colaDetalle.AnalisisColaDetalleID);
                                analisisColaDetalleBL.DeleteById((int)colaDetalle.AnalisisColaDetalleID);
                                listaErrorFirma.Add("Ocurrió un error al intentar crear el proceso de firma. Inténtelo de nuevo en unos minutos");
                                break;
                            }

                            contenedores.Add(contenedor);
                        }
                        else //Si alguno de los datos es erróneo finaliza la firma.
                        {
                            listaErrorFirma.Add(contenedor.mensajeError);
                            System.Diagnostics.Debug.WriteLine(contenedor.mensajeError);
                            break;
                        }
                    }

                    //Si los registros se crean correctamente, entonces se añaden todos a la cola.
                    if (!banderaDetieneCola && !banderaCreaRegistros)
                        contenedores.ForEach(contenedor => queue.Enqueue(contenedor));



                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.InnerException);
                }

                if (limiteSSTDQO && contenedores.Count > 0)
                {
                    this.PopMensaje("Algunos análisis no pudieron ser firmados debido a que se encuentran fuera de la vigencia de la norma", "Firma de análisis", Opciones.Si, Icono.question, "LímiteSSTyDQO", null);
                }
                else if (ClorurosConductividad && contenedores.Count > 0)
                {
                    this.PopMensaje("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros y/o conductividad eléctrica.", "Firma de análisis", Opciones.Si, Icono.question, "LímiteSSTyDQO", null);
                }
                else if (Cloruros && contenedores.Count > 0)
                {
                    this.PopMensaje("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros.", "Firma de análisis", Opciones.Si, Icono.question, "LímiteSSTyDQO", null);
                }
                else if (!listaErrorFirma.Any())
                {
                    //Page_Load(sender, e);
                    Response.Redirect("~/EstatusFirmado.aspx", true);
                    this.PopMensaje("Análisis enviados a firma", "Firmado");
                }
                else
                    this.PopMensaje(listaErrorFirma.FirstOrDefault().ToString(), "Error al firma", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else
            {
                this.PopMensaje("El RFC autorizado para firmar no coincide con el del certificado. Favor de verificarlo.", "RFC Diferentes.", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
        protected void wucFirmaFaltaPago_Firmar(object sender, DatosFirma_Args e)
        {
            FIEL.FirmaElectronica datosFirma = e.DatosAFirmar;
            byte[] ArchivoKey = e.ArchivoKey;
            byte[] ArchivoCer = e.ArchivoCer;
            string CNT = e.txtContra;
            List<string> listaErrorFirma = new List<string>();
            List<string> ListaErrorCertificado = new List<string>();

            if (Login.Laboratorio.RFCValidoParaFirmar(e.RFCFirma))
            {
                foreach (FIEL.Contenedor contenedor in datosFirma.listaContenedores)
                {
                    contenedor.ArchivoCer = ArchivoCer;
                    contenedor.ArchivoKey = ArchivoKey;
                    contenedor.txtRFC = e.RFCFirma;
                    contenedor.txtContra = CNT;

                    var analisisBl = new Analisis_BL();
                    int analisisID = 0;
                    int.TryParse(contenedor.primaryKey, out analisisID);
                    var analisisResult = analisisBl.Obten_Analisis(analisisID);
                    OrdenTrabajoID = analisisResult.OrdenTrabajoID;
                    string[] split = contenedor.cadenaAFirmar.Split(new[] { '|' });
                    //var fech = split[10].Substring(35, 19);

                    //var provider = CultureInfo.InvariantCulture;
                    //const string format = "dd-MM-yyyy HH:mm:ss";
                    //var date = DateTime.ParseExact(fech, format, provider);

                    if (contenedor.valido)
                    {
                        var args = new AnalisisFirmaArgs
                        {
                            AnalisisFirmaID = null,
                            AnalisisID = analisisID,
                            RFC = e.RFCFirma,
                            Firma = contenedor.firmaDigital,
                            Cadena = contenedor.cadenaAFirmar,
                            Contenedor = contenedor.cetificadoDigital,
                            FechaRegistro = DateTime.Now
                        };

                        try
                        {
                            ApplicationSettings aps = new ApplicationSettings();
                            SelloDigital sello = new SelloDigital();
                            sello.cadenaOriginal = contenedor.cadenaAFirmar;
                            sello.SellarCadenaOriginal();
                            //args.SelloCNA = sello.SellarCadenaOriginalSirfic();
                            args.SelloCNA = sello.sello;

                            // Validación: no continuar si el servicio de sellado de CONAGUA no
                            // devolvió un sello válido (sello.sello regresa cadena vacía cuando
                            // codigoError != 0, sin lanzar excepción). Se lanza la excepción para
                            // caer al catch y mostrar el error, en lugar de guardar un sello en blanco.
                            if (string.IsNullOrWhiteSpace(args.SelloCNA))
                            {
                                throw new Exception(
                                    "La selladora de CONAGUA no devolvió un sello válido (" +
                                    (sello.mensajeError ?? "sin respuesta") + ").");
                            }

                            this.AnalisisFirmaArgs = args;
                            //Debemos solicitar la firma electrónica
                            bool faltaPago = analisisBl.Obten_Analisis(analisisID).FaltaPago;
                            var mensaje = "¿Se encuentra seguro de marcar el análisis con falta de pago? <br /><br />" + aps.LeyendaFaltaPago.Replace("|", "<br />");
                            if (faltaPago)
                                mensaje = "¿Se encuentra seguro de retirar la marca de falta de pago del análisis?";

                            string[] param = { analisisID.ToString(), faltaPago.ToString() };
                            this.PopMensaje(mensaje, "Falta de pago del análisis de trabajo", controles.Opciones.SiNo, controles.Icono.warning, "FaltaPago", param);
                        }
                        catch (Exception Ex)
                        {
                            args.SelloCNA = "Hubo algún problema al solicitar el sello de CONAGUA.";
                            string errorStr = "<br/>";
                            errorStr += "Error al sellar el acuse del Analisis ID: ";
                            errorStr += analisisResult.AnalisisID;
                            errorStr += " ";
                            errorStr += "Error: ";
                            errorStr += "";
                            errorStr += " ";
                            errorStr += "Descripcion: ";
                            errorStr += Ex.Message;
                            listaErrorFirma.Add(errorStr);
                        }
                    }
                    else//añadir lista de errores
                    {
                        string errorStr = "<br/>";
                        errorStr += "Descripción: ";
                        errorStr += contenedor.mensajeError;
                        ListaErrorCertificado.Add(errorStr);
                    }
                }
            }
            else
            {
                this.PopMensaje("El RFC autorizado para firmar no coincide con el del certificado. Favor de verificarlo.", "RFC Diferentes.", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
        protected void lnkbtnback_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(1);
            Response.Redirect(regreso);
        }
        protected void wucOrdenAnalisis_Cancelar(object sender, SIRALAB_DAL.Analisi e)
        {
            var args = (SIRALAB_DAL.Analisi)e;
            if (args != null)
            {
                this.AnalisisID = args.AnalisisID;
                this.PopMensaje("¿Esta seguro de cancelar el analisis seleccionado?", "Cancelar analisis", controles.Opciones.SiNo, controles.Icono.warning, "CancelarAnalisis");
            }
        }
        protected void wucConsultaCliente1_Seleccion(object sender, Cliente e)
        {
            if (e != null)
            {
                pnlPtoDescarga.Enabled = true;
                wucConsultaPtoDescarga1.ObtenConseciones();
                //TODO -> verificar funcionamiento.
                divPnlPuntoDescarga.Visible = true;
            }
            else
            {
                pnlPtoDescarga.Enabled = false;
                pnlPtoDescarga.Visible = false;
            }
        }
        protected void wucConsultaPtoDescarga1_Seleccion(object sender, Anexo e)
        {
            if (e != null)
            {
                pnlPtoDescarga.Enabled = true;
                pnlNormasMexicanas.Enabled = true;
                divNormasMexicanas.Visible = true;

                wucConsultaPtoDescarga1_Limpiar(null, null);
                wucOrdenTrabajo1.CargaNormas(wucConsultaPtoDescarga1.EsFuenteAbastecimiento ?? false, e.ID_TIPO_CR, e.ID_USO_CR);
            }
            else
                pnlPtoDescarga.Enabled = false;
        }
        protected void wucConsultaPtoDescarga1_Limpiar(object sender, Anexo e)
        {
            wucOrdenTrabajo1.ClearCargaNormas();

            var ddlPtoAnexo = (DropDownList)wucConsultaPtoDescarga1.FindControl("ddlPtoAnexo");

            if (this.OrdenTrabajoID == 0)
            {
                wucOrdenTrabajo1.Visible = ddlPtoAnexo != null && ddlPtoAnexo.SelectedIndex > 0;
                divNormasMexicanas.Visible = ddlPtoAnexo != null && ddlPtoAnexo.SelectedIndex > 0;
            }

        }
        protected void wucConsultaPtoDescarga1_Guardar(object sender, Anexo e)
        {
            if (e.AnexoID > 0)
                this.PopMensaje("Se ha actualizado la información de fuente de abastecimiento y origen de descarga exitosamente", "Datos actualizados exitosamente", controles.Opciones.SoloAceptar, controles.Icono.ok);
        }
        protected void wucConsultaCliente1_Error(object sender, ClienteErrorArg e)
        {
            this.PopMensaje(e.Mensaje, "Error Cliente", controles.Opciones.SoloAceptar, controles.Icono.warning);
        }
        protected void MensajeNomSSTDQOCOT(object sender, Dictionary<string, string> DatosDelMensaje)
        {
            //var mensaje = "Se observa que en la muestra compuesta anterior reportó " + normaAnterior + " y actualmente se está reportando " + norma + ", lo cual afectará la aplicación por parte de su " + "cliente del beneficio fiscal a que se refiere el artículo 278 de la Ley Federal de Derechos.";
            List<string> listaValores = new List<string>(DatosDelMensaje.Values);
            this.PopMensaje(listaValores[0], listaValores[1], controles.Opciones.SoloAceptar, controles.Icono.warning);
        }
    }

    public class AnalisisFirmaArgs : SIRALAB_BL.common.AnalisisFirmaArgBase_BL
    {

    }
    public class AnalisisFaltaPagoArgs : AnalisisFaltaPagoArgBase_BL { }
}