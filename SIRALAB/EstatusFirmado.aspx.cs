using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SIRALAB.ClienteAPI;
using SIRALAB.Common;
using SIRALAB.controles;
using SIRALAB.Controles;
using SIRALAB.FIEL;
using SIRALAB.Queue;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_DAL;

namespace SIRALAB
{



    public partial class EstatusFirmado : System.Web.UI.Page
    {

        private static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)this.Master.Master;
            main.MensajeDevolucion += main_MensajeDevolucion;

            if (!IsPostBack)
            {
                Initialize(Login.LaboratorioID ?? 0);
                hfLaboratorioID.Value = Login.LaboratorioID.ToString();
            }
        }



        public void Initialize(int LaboratorioID)
        {
            var analisisColaEncabezado_BL = new AnalisisColaEncabezado_BL();
            var analisisActivos =
                analisisColaEncabezado_BL.FindAllByLabIDAndColaActiva(LaboratorioID); //Recupera analisis Encolados.
            btnLimpiar.Visible = analisisActivos.Any();

            var analisisFallido =
                analisisColaEncabezado_BL.FindAllByLabIdFallido(LaboratorioID).OrderBy(x => x.AnalisisID); //Recupera analisis que han fallado.

            foreach (var ace in analisisFallido.Where(ace =>
                !analisisActivos.Select(a => a.AnalisisID).Contains(ace.AnalisisID))
            ) //Filtra y añade los análisis que han fallado.
            {
                analisisActivos.Add(ace);
            }

            if (analisisActivos.Any())
                analisisActivos.OrderBy(x => x.AnalisisID);

            hfAnalisisActivos.Value = analisisActivos.Count.ToString();
            divTablaAnalisis.Visible = analisisActivos.Any();

            rptEstatusFirma.DataSource = analisisActivos;
            rptEstatusFirma.DataBind();
        }


        protected void lnkbtnBack_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(1);
            Response.Redirect(regreso);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Resultado ActualizaBarraProgreso(int laboratorioId, List<int> listaColas)
        {

            var resultado = new Resultado();
            var analisisColaEncabezado = new AnalisisColaEncabezado_BL();

            var analisisActivos = analisisColaEncabezado.FindAllByLabIDAndColaActiva(laboratorioId);

            float total = analisisActivos.Count;
            float enProceso = total > 0 ? analisisActivos.Count(a =>
                a.EstatusID == enumEstatusAnalisis.EnProceso.ParseToInt() ||
                a.EstatusID == enumEstatusAnalisis.Firmado.ParseToInt()) : 0;

            var analisisEnPantalla = new List<AnalisisColaEncabezado>();

            foreach (var colaId in listaColas)
            {
                analisisEnPantalla.AddRange(analisisColaEncabezado.FindAllByAnalisisColaDetalleID(colaId));
            }


            var analisisString = "";
            var analisisStringList = analisisEnPantalla.Select(ace => "{\"AnalisisId\":" + ace.AnalisisID + ",\"EstatusId\":" + ace.EstatusID + ",\"ColaId\":" + ace.AnalisisColaDetalleID + ",\"FechaConformacion\":\"" + (ace.FechaFirma != null ? ((DateTime)ace.FechaFirma).ToString("dd/MM/yyyy") : "") + "\", \"HoraConformacion\":\"" + (ace.FechaFirma != null ? ((DateTime)ace.FechaFirma).ToString("HH:mm") : "") + "\"}").ToList();

            if (analisisStringList.Count == 1)
            {
                analisisString += "\"Analisis\": [" + analisisStringList[0] + "]";
            }
            else
            {
                for (var i = 0; i < analisisStringList.Count; i++)
                {
                    if (i == 0)
                    {
                        analisisString += "\"Analisis\": [" + analisisStringList[i] + ",";
                    }
                    else if (i == analisisStringList.Count - 1)
                    {
                        analisisString += analisisStringList[i] + "]";
                    }
                    else
                    {
                        analisisString += analisisStringList[i] + ",";
                    }
                }
            }




            int porcentaje = Convert.ToInt32( total > 0 ? 100 - (enProceso / total) * 100 : 100);
            resultado.Message = analisisStringList.Any()
                ? "{\"Total\":" + total + ",\"EnProceso\":" + enProceso + ",\"Porcentaje\":" + porcentaje + "," + analisisString + " }"
                : "{\"Total\":" + total + ",\"EnProceso\":" + enProceso + ",\"Porcentaje\":" + porcentaje + " }";

            resultado.Result = true;
            resultado.Finalized = true;
            return resultado;

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Resultado FinalizaResultadoBarraProgreso()
        {
            var resultado = new Resultado();
            resultado.Result = true;
            resultado.Finalized = true;
            return resultado;
        }



        protected void wucAppletFIEL_FirmarColeccion(object sender, DatosFirma_Args e)
        {
            FIEL.FirmaElectronica datosFirma;
            datosFirma = e.DatosAFirmar;
            byte[] ArchivoKey = e.ArchivoKey;
            byte[] ArchivoCer = e.ArchivoCer;
            string CNT = e.txtContra;
            List<string> listaErrorFirma = new List<string>();


            if (Login.Laboratorio.RFCValidoParaFirmar(e.RFCFirma))
            {

                //
                // Health-check: si la selladora de CONAGUA no está disponible, no se encola
                // nada y se avisa al usuario en el momento para que reintente más tarde.
                if (!SelloDigital.SelladoraDisponible())
                {
                    this.PopMensaje(
                        "Estamos teniendo una breve interrupción en el servicio de sellado. No te preocupes, tus datos no se han perdido. Te sugerimos reintentar el proceso un poco más tarde.",
                        "Servicio de sellado no disponible",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    return;
                }

                try
                {
                    FirmaQueue queue = new FirmaQueue();
                    var banderaDetieneCola = true;
                    var banderaCreaRegistros = false;

                    var analisisColaDetalleBL = new AnalisisColaDetalle_BL();
                    var analisisColaEncabezadoBL = new AnalisisColaEncabezado_BL();

                    //var provider = CultureInfo.InvariantCulture;
                    //const string format = "dd-MM-yyyy HH:mm:ss";
                    var date = DateTime.Now;

                    var colaDetalle = new AnalisisColaDetalle()
                    {
                        AnalisisColaDetalleID = 0,
                        Activo = true,
                        Completo = false,
                        FechaCreacion = date,
                    };

                    var contenedores = new List<Contenedor>();

                    foreach (var contenedor in datosFirma.listaContenedores)
                    {
                        contenedor.ArchivoCer = ArchivoCer;
                        contenedor.ArchivoKey = ArchivoKey;
                        contenedor.txtRFC = e.RFCFirma;
                        contenedor.txtContra = CNT;

                        if (contenedor.valido) //Como los datos son siempre iguales, si falla el primero fallan todos:
                        {

                            if (banderaDetieneCola
                            ) //En la primera iteración crea la cola. Si ocurre un error al crear el registro, se regresa un mensaje de error (no se persiste nada y no se abre un hilo para las firmas).
                            {

                                colaDetalle = analisisColaDetalleBL.Guarda(colaDetalle);
                                banderaDetieneCola = colaDetalle == null;
                                queue.ColaID = !banderaDetieneCola ? (int)colaDetalle.AnalisisColaDetalleID : 0;
                                queue.ServerUtility = System.Web.HttpContext.Current.Server;

                                if (banderaDetieneCola)
                                {
                                    listaErrorFirma.Add(
                                        "Ocurrió un error al intentar crear el proceso de firma. Inténtelo de nuevo en unos minutos");
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


                            //TODO -> Crear validación para que si hay algún análisis que ya se está firmando detenga todo el proceso.


                            //Si ocurre algún error al persistir el registro se eliminan todos los registros de la cola:
                            banderaCreaRegistros =
                                analisisColaEncabezadoBL.SaveOrUpdate(analisisColaEncabezado) == null;

                            if (banderaCreaRegistros)
                            {
                                analisisColaEncabezadoBL.DeleteAllByColaID((int)colaDetalle.AnalisisColaDetalleID);
                                analisisColaDetalleBL.DeleteById((int)colaDetalle.AnalisisColaDetalleID);
                                listaErrorFirma.Add(
                                    "Ocurrió un error al intentar crear el proceso de firma. Inténtelo de nuevo en unos minutos");
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

                if (!listaErrorFirma.Any())
                {
                    //Page_Load(sender, e);
                    Response.Redirect("~/EstatusFirmado.aspx", true);
                    this.PopMensaje("Análisis enviados a firma", "Firmado");
                }
                else
                    this.PopMensaje(listaErrorFirma.FirstOrDefault().ToString(), "Error al firma",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else
            {
                this.PopMensaje(
                    "El RFC autorizado para firmar no coincide con el del certificado. Favor de verificarlo.",
                    "RFC Diferentes.", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }




        protected void rptEstatusFirma_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }


        //Redirige a la OT.
        protected void btnInfoAnalisis_OnClick(object sender, EventArgs e)
        {
            var ordenTrabajoId = int.Parse(((LinkButton)sender).CommandArgument);
            Session["OrdenTrabajoID_"] = ordenTrabajoId;
            Response.Redirect("~/OrdenTrabajo.aspx");
        }

        //Descarga el informe del análisis.
        protected void btnDescargaReporte_OnClick(object sender, EventArgs e)
        {
            int ID = int.Parse(((LinkButton)sender).CommandArgument);
            Analisis_BL an = new Analisis_BL();
            an.Obten_Analisis(ID);
            ApplicationSettings applicationSettings = new ApplicationSettings();
            var fechaNuevoReportes = applicationSettings.fechaNuevosReportes;

            if (an.Analisis.FechaRegistro > fechaNuevoReportes)
            {
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

        //Genera el informe del análisis.
        private byte[] generaInformeAnalisis(int analisisId, string nombreReport)
        {
            List<ReportParameter> listParametros = new List<ReportParameter>();
            ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", analisisId.ToString());
            listParametros.Add(rpAnalisisID);
            return OpenReports.BufferPDFRemoteReport(nombreReport, listParametros);
        }


        //Genera el acuse del análisis.
        private byte[] generaAcuse(int analisisId, string nombreReportAcuse)
        {

            ApplicationSettings ap = new ApplicationSettings();
            SIRALAB_DAL.Analisi analisisDal = new Analisi();
            Analisis_BL analisisBL = new Analisis_BL();
            analisisDal = analisisBL.Obten_Analisis(analisisId);


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
                analisisDal.Muestras.ToList().ForEach(mst =>
                    mst.MuestraValors.ToList().ForEach(mv => strMuestraValor += mv.Parametro.Nombre + " "));
                normaOParametros += strMuestraValor;
            }

            ReportParameter paramAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            ReportParameter paramNumInforme =
                new ReportParameter("NumeroDeInforme", analisisDal.OrdenTrabajo.NumeroInforme.ToString());
            ReportParameter paramHoraDFirma = new ReportParameter("HoraDeFirma", DateTime.Now.ToShortTimeString());
            ReportParameter paramNomPuntDes = new ReportParameter("NombrePuntoDeDescarga",
                analisisDal.Anexo.NombreCuerpoReceptor != null && analisisDal.Anexo.NombreCuerpoReceptor != ""
                    ? analisisDal.Anexo.NombreCuerpoReceptor
                    : analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
            ReportParameter paramCuerpoRece =
                new ReportParameter("CuerpoReceptor", analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
            ReportParameter paramFrecMuestr = new ReportParameter("FrecuenciaMuestreo",
                analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
            ReportParameter paramNormaOPmts = new ReportParameter("NormaOParametros", normaOParametros);
            ReportParameter paramUbicaMuest = new ReportParameter("UbicacionMuestreo", ubicacionDeMuestra);
            string selloDigitalCNA = analisisDal.AnalisisFirmas.Where(a => a.AnalisisID == analisisDal.AnalisisID)
                .Last().SelloDigital;
            ReportParameter paramSelloDeCNA = new ReportParameter("SelloCONAGUA", selloDigitalCNA);
            ReportParameter paramAcuseRegla = new ReportParameter("AcuseRegla",
                string.Format(ap.AcuseTexto, ap.AcuseRegla, ap.AcuseAnio));

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

            return OpenReports.BufferPDFRemoteReport(nombreReportAcuse, listParametrosAcuse);
        }

        protected void btnFirmar_OnClick(object sender, EventArgs e)
        {
            int analisisId = int.Parse(((LinkButton)sender).CommandArgument);
            this.PopMensaje("¿Desea proceder a firmar el análisis?", "Muestras de análisis", controles.Opciones.SiNo, controles.Icono.information, "FirmarAnalisis", new string[] { analisisId.ToString() });
            hfMostrarModal.Value = true.ToString();
        }



        //Sólo se muestra opción si álgo en un proceso anterior ha fallado.
        protected void btnEnvio_OnClick(object sender, EventArgs e)
        {
            int analisisColaEncabezadoId = int.Parse(((LinkButton)sender).CommandArgument);
            var analisisColaEncabezadoBL = new AnalisisColaEncabezado_BL();
            var aceOriginal = analisisColaEncabezadoBL.FinAnalisisColaEncabezadoById(analisisColaEncabezadoId);

            //Verificar estatus del análisisColaEncabezado.
            if (aceOriginal.EstatusID == enumEstatusAnalisis.EnvioFallido.ParseToInt() ||
                aceOriginal.EstatusID == enumEstatusAnalisis.Firmado.ParseToInt())
            {
                var acd = new AnalisisColaDetalle()
                {
                    AnalisisColaDetalleID = 0,
                    Activo = true,
                    Completo = false,
                    FechaCreacion = DateTime.Now
                };

                var acdBL = new AnalisisColaDetalle_BL();
                acd = acdBL.Guarda(acd);


                var ace = new AnalisisColaEncabezado()
                {
                    AnalisisColaEncabezadoID = 0,
                    AnalisisColaDetalleID = acd.AnalisisColaDetalleID,
                    AnalisisID = aceOriginal.AnalisisID,
                    FechaEnvio = DateTime.Now,
                    EstatusID = enumEstatusAnalisis.Firmado.ParseToInt(),
                    LaboratorioID = Login.LaboratorioID ?? 0,

                };

                analisisColaEncabezadoBL.SaveOrUpdate(ace);

                CorreoQueue correoQueue = new CorreoQueue();
                correoQueue.ColaID = ace.AnalisisColaDetalleID;
                
                var serverUtility = System.Web.HttpContext.Current.Server;
                correoQueue.ServerUtility = serverUtility;
                correoQueue.Enqueue(ace.AnalisisID);

                Initialize(Login.LaboratorioID ?? 0);
            }
        }


        protected void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            switch (e.Command)
            {
                case "FirmarAnalisis":
                    /// Mostrar la Firma electrónica
                    ///
                    if (e.Opcion == Opcion.Si || e.Opcion == Opcion.Aceptar)
                    {
                        var analisisIdFirma = int.Parse(e.Args[0]);
                        var datosFirma = ObtenerDatosAFirmar(analisisIdFirma);

                        wucAppletFIEL.enviarAFirma(datosFirma.DatosAFirmar);
                        wucAppletFIEL.Visible = true;
                    }
                    break;
                case "FirmadoCorrectamente":
                    if (e.Opcion == Opcion.Si || e.Opcion == Opcion.Aceptar)
                        Response.Redirect("~/OrdenTrabajo.aspx");
                    break;
            }
        }


        private DatosFirma_Args ObtenerDatosAFirmar(int analisisID)
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FIEL.FirmaElectronica();

            #region Datos a firmar

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
            #endregion

            return resultado;
        }


        //public void Firmar()
        //{
        //    if (FirmarAnalisis != null)
        //    {
        //        FirmarAnalisis(this, ObtenerDatosAFirmar());
        //    }
        //}

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            Initialize(Login.LaboratorioID ?? 0);
        }
    }
}