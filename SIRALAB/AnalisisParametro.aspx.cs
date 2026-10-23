using SIRALAB.Controles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Reporting.WebForms;
using SIRALAB.controles;
using SIRALAB.Common;
using SIRALAB.FIEL;
using SIRALAB.Queue;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_DAL;
using SIRALAB_DAL.Extensiones;
using System.Web.UI;

namespace SIRALAB
{
    public partial class AnalisisParametro : System.Web.UI.Page
    {
        public int OrdenTrabajoID
        {
            get { return (int)Session["OrdenTrabajoID_"] == 0 ? 0 : (int)Session["OrdenTrabajoID_"]; }
            set { Session["OrdenTrabajoID_"] = value; }
        }
        public SIRALAB_DAL.Analisi Analisis_
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)this.Master.Master;
            main.MensajeDevolucion += main_MensajeDevolucion;

            var ordentrabajoBl = new OrdenTrabajo_BL();
            var orden = ordentrabajoBl.Obten(this.OrdenTrabajoID);
            if (orden != null)
            {
                lblFolio.Text = "Número: " + orden.NumeroInforme;
            }

            wucMuestraDatos1.CheckDQOCOT += wucMuestraDatos1_CheckDQOCOT;

            if (!IsPostBack)
                CargaInformacion();
        }

        protected void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            if (e.Command.Contains("|"))
            {
               var arregloFirma= e.Command.Split('|');
                e.Command = arregloFirma[1];
            }
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
                case "msjNomSSTDQO":
     
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeNomSSTDQO').modal();", true);
                    //this.PopMensaje("El análisis no puede ser firmado debido a que se encuentra fuera de la vigencia de la norma", "Firma de análisis", Opciones.SoloAceptar, Icono.question);
                    break;
                case "Cloruros":

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeCloruros').modal();", true);
                    //this.PopMensaje("El análisis no puede ser firmado debido a que se encuentra fuera de la vigencia de la norma", "Firma de análisis", Opciones.SoloAceptar, Icono.question);
                    break;
                case "Conductividad":
                    //divMensajesSSTDQO.Visible = true;

                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeConductividad').modal();", true);
                    //this.PopMensaje("El análisis no puede ser firmado debido a que se encuentra fuera de la vigencia de la norma", "Firma de análisis", Opciones.SoloAceptar, Icono.question);
                    break;
            }
        }

        protected void CargaInformacion()
        {
            wucMuestraDatos1.CargaDatos();
            wucMuestreador1.ObtenInformacionMuestreador();
        }

        protected void wucMuestraDatos1_Guardar(object sender, EventArgs e)
        {

            if (((MuestraCapturaArgs)e).Descripcion == "SinParametros")
            {
                this.PopMensaje("La norma que ha seleccionado para realizar el análisis no contiene parámetros, por favor contacte al administrador del sistema para informarle la situación presentada", "Norma sin parámetros", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else if (((MuestraCapturaArgs)e).Descripcion == "SinMuestreador")
            {
                this.PopMensaje("Falta seleccionar el laboratorio que realiza el muestreo", "Datos incompletos", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else if (((MuestraCapturaArgs)e).Descripcion == "ErrorFecha")
            {
                this.PopMensaje("La fecha de muestreo y/o la fecha de recepción en laboratorio son incorrectas", "Datos incompletos", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else if (((MuestraCapturaArgs)e).Descripcion == "CaudalCero")
            {
                this.PopMensaje("El caudal debe ser mayor a cero", "Datos incompletos", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else
            {
                this.PopMensaje("Información guardada/actualizada correctamente", "Información guardada", controles.Opciones.SoloAceptar, controles.Icono.ok);
            }
        }


        protected void lnkbtnBack_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(3);
            Response.Redirect(regreso);
        }

        protected void wucMuestraDatos1_TipoMuestra(object sender, bool e)
        {
            if (e)
                liSignatario.Visible = false;
            else
                liSignatario.Visible = true;
        }
        protected void wucMuestraDatos1_DatosIncompletos(object sender, bool e)
        {
            if (e)
                this.PopMensaje("Falta seleccionar el laboratorio y/o el método de análisis", "Información incompleta", controles.Opciones.SoloAceptar, controles.Icono.warning);
        }

        protected void wucMuestraDatos1_Simple_DatosIncompletos(object sender, bool e)
        {
            if (e)
                this.PopMensaje("Existen datos incompletos en los campos de información del parametro", "Información Incompleta", controles.Opciones.SoloAceptar, controles.Icono.information);
        }

        protected void wucMuestraDatos1_Compuesta_DatosIncompletos(object sender, bool e)
        {
            if (e)
                this.PopMensaje("Existen datos incompletos en los campos de información del parametro", "Información Incompleta", controles.Opciones.SoloAceptar, controles.Icono.warning);
        }

        protected void wucMuestraDatos1_GuardaParametros(object sender, bool e)
        {
            if (e)
            {
                /// TODO: Debemos comprobar que el análisis se haya concluido, esto es que todas las muestras tengan
                var analisisBl = new Analisis_BL();
                var analisisTemp = analisisBl.Obten_Analisis(this.Analisis_.AnalisisID);
                var finalizado = analisisTemp.MuestraValorCompleto();



                if (finalizado)
                {
                    /// Ya está finalizado y se puede firmar el análisis
                    ///
                    #region ValidacionLimiteNormaSSTDQO
                    var msjNomSSTDQO = string.Empty;
                    try
                    {

                        ApplicationSettings applicationSettings = new ApplicationSettings();
                        
                        if (analisisTemp.OrdenTrabajo.NormaID == 3 || analisisTemp.OrdenTrabajo.Norma.NormaID == 13)
                        {
                            var fechaUltimaMuestra = analisisTemp.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha;
                            var fechaLimiteAnalisisSSTDQO = Convert.ToDateTime(applicationSettings.LimiteNormaSSTDQO);
                            if (fechaUltimaMuestra >= fechaLimiteAnalisisSSTDQO)
                            {
                                msjNomSSTDQO = "|msjNomSSTDQO";
                                
                            }
                        }
                        else if (analisisTemp.OrdenTrabajo.Norma.Nombre.Replace(" ", "") == applicationSettings.NormaCOT || analisisTemp.OrdenTrabajo.Norma.Nombre.Replace(" ", "").ToUpper() == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                        {
                            var fechaUltimaMuestra = analisisTemp.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha;
                            var fechaLimiteAnalisisSSTDQO = Convert.ToDateTime(applicationSettings.LimiteNormaSSTDQO);
                            if (fechaUltimaMuestra < fechaLimiteAnalisisSSTDQO)
                            {
                                msjNomSSTDQO = "|msjNomSSTDQO";
                            }
                        }

                        if (analisisTemp.OrdenTrabajo.NormaID==15)//valida que sea la norma 2021
                        {
                            if (analisisTemp.NormaDQOCOT==null || analisisTemp.NormaDQOCOT == string.Empty)
                            {
                                msjNomSSTDQO = "|Cloruros";
                            }
                            if (analisisTemp.Conductividad == null || analisisTemp.Conductividad == string.Empty)
                            {
                                msjNomSSTDQO = "|Conductividad";
                            }
                            

                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }

                    #endregion
                    this.PopMensaje("Se ha guardado la información, el análisis se encuentra completo, ¿Desea proceder a firmar el análisis?", "Muestras de análisis", controles.Opciones.SiNo, controles.Icono.information, "FirmarAnalisis"+ msjNomSSTDQO, new string[] { this.Analisis_.AnalisisID.ToString() });
                }
                else
                {
                    ApplicationSettings applicationSettings = new ApplicationSettings();
                    string mensaje = $"Se ha guardado la información, el análisis aún no se ha completado, los datos que se evalúan para que la muestra se considere como completa son: <br />"
                                     + $"<br />Sí el parámetro no se marcó como exento o no analizado<br />"
                                     + $"<ul style=\"list-style: none; padding: 2px; margin: 0;\">"
                                     + $"<li>Contener un valor numérico en todos los registros en los campos \"Resultado\" y \"LC/MC\".</li>"
                                     + $"<li>Contener seleccionado un \"Laboratorio\" y el \"Método\" analítico en todos los parámetros analizados.</li>";

                    if (this.Analisis_.OrdenTrabajo.NormaID == 15 || this.Analisis_.OrdenTrabajo.NormaID == 16
                        || this.Analisis_.OrdenTrabajo.Norma.Nombre.ToUpper().Replace(" ", "") == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                        mensaje += $"<li>Seleccionar el parámetro \"DQO/COT\" a evaluar</li>";

                    mensaje += $"</ul>";
                    this.PopMensaje(mensaje, "Muestras de análisis", controles.Opciones.SoloAceptar, controles.Icono.information);
                }
            }
        }

        protected void wucMuestraDatos1_GuardadoSimple(object sender, bool e)
        {
            if (e)
            {
                this.PopMensaje("Muestra guardada con éxito", "Muestras de análisis", controles.Opciones.SoloAceptar, controles.Icono.ok);
            }
            else
            {
                string mensaje = $"Se ha guardado la información, el análisis aún no se ha completado, los datos que se evalúan para que la muestra se considere como completa son: <br />"
                                 + $"<br />Sí el parámetro no se marcó como exento o no analizado<br />"
                                 + $"<ul style=\"list-style: none; padding: 2px; margin: 0;\">"
                                 + $"<li>Contener un valor numérico en todos los registros en los campos \"Resultado\" y \"LC/MC\".</li>"
                                 + $"<li>Contener seleccionado un \"Laboratorio\" y el \"Método\" analítico en todos los parámetros analizados.</li>"
                                 + $"</ul>";
                this.PopMensaje(mensaje, "Muestras de análisis", controles.Opciones.SoloAceptar, controles.Icono.information);
            }

        }


        protected void wucMuestraDatos1_RecalculaCompuesta(object sender, bool e)
        {
            if (e)
                this.PopMensaje("Se ha actualizado los valores de las muestras simples.", "Muestras de análisis", controles.Opciones.SoloAceptar, controles.Icono.information);

        }

        protected void wucMuestraDatos1_CheckDQOCOT(object sender, Dictionary<bool, string> valuePairs )
        {
            if (valuePairs.First().Key)
            {
                var norma = valuePairs.First().Value;
                var articulo = this.Analisis_.OrdenTrabajo.NormaID == 16 ? "278" : this.Analisis_.OrdenTrabajo.NormaID == 15 ? "282 fracción I" : "278 B fracción VI";
                var normaAnterior = norma == "COT" ? "DQO" : "COT";
                var mensaje = "Se observa que en la muestra compuesta anterior reportó " + normaAnterior +
                    " y actualmente se está reportando " + norma +", lo cual afectará la aplicación por parte de su " +
                    "cliente del beneficio fiscal a que se refiere el artículo " + articulo + " de la Ley Federal de Derechos.";
                this.PopMensaje(mensaje, "Muestras de análisis", controles.Opciones.SoloAceptar, controles.Icono.warning);
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
        protected void wucAppletFIEL_OnFirmarColeccion(object sender, DatosFirma_Args e)
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
                        "El servicio de sellado de CONAGUA no está disponible en este momento. Por favor, intente firmar más tarde.",
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

                            if (banderaDetieneCola) //En la primera iteración crea la cola. Si ocurre un error al crear el registro, se regresa un mensaje de error (no se persiste nada y no se abre un hilo para las firmas).
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
                        contenedores.ForEach(contenedor =>
                        {
                            queue.Enqueue(contenedor);
                        });



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

            //Prueba funcionamiento:
            byte[] fileAnalisisPdf = null;
            try
            {
                fileAnalisisPdf = OpenReports.BufferPDFRemoteReport(nombreReportAnalisis, listParametrosAnalisisFirmado);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.ToString());
            }

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
            ReportParameter paramNomPuntDes = new ReportParameter("NombrePuntoDeDescarga", analisisDal.Anexo.NombreCuerpoReceptor);
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

            //Prueba funcionamiento:
            byte[] fileAcusePdf = null;
            try
            {
                fileAcusePdf = OpenReports.BufferPDFRemoteReport(nombreReportAcuse, listParametrosAcuse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.ToString());
            }


            Hashtable ht = new Hashtable();

            ht.Add(nombreReportAnalisis + ".pdf", fileAnalisisPdf);
            ht.Add(nombreReportAcuse + ".pdf", fileAcusePdf);
            ht.Add("enc_img_correo", HttpContext.Current.Server.MapPath(ap.MailImagePath));
            ht.Add("Pie_Correo", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));

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
    }

    public class ContenedorExt : Contenedor
    {

    }
}