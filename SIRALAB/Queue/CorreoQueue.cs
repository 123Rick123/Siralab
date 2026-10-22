using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Reporting.WebForms;
using SIRALAB.ClienteAPI;
using SIRALAB.Common;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.Contacto;
using SIRALAB_DAL;

namespace SIRALAB.Queue
{
    public class CorreoQueue
    {
        private Queue<int> _jobs = new Queue<int>();
        private bool _delegateQueuedOrRunning = false;
        public int? ColaID { get; set; }
        public HttpServerUtility ServerUtility { get; set; }

        private Queue_BL queueBl = new Queue_BL();

        //Encola el objeto en la cola y lo agrega a un thread de pools.
        public void Enqueue(int job)
        {
            lock (_jobs)
            {
                _jobs.Enqueue(job);
                System.Diagnostics.Debug.WriteLine("Tamaño de la cola: " + _jobs.Count);
                if (!_delegateQueuedOrRunning)
                {
                    _delegateQueuedOrRunning = true;
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
                }
            }
        }


        //Procesa los items encolados.
        private void ProcessQueuedItems(object ignored)
        {
            while (true)
            {
                int item;
                lock (_jobs)
                {
                    if (_jobs.Count == 0)
                    {

                        queueBl.finalizarCola(ColaID);
                        _delegateQueuedOrRunning = false;
                        break;
                    }

                    item = _jobs.Dequeue();
                }

                try
                {
                    sendMail(item);
                }
                catch (Exception exception)
                {
                    queueBl.setEstatusAnalisisEncabezadoEnCorreo(item, ColaID.Value, enumEstatusAnalisis.EnvioFallido.ParseToInt());
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
                }
            }
        }


        private void sendMail(int analisisID)
        {
            ApplicationSettings ap = new ApplicationSettings();
            SIRALAB_DAL.Analisi analisisDal = new Analisi();
            Analisis_BL analisisBL = new Analisis_BL();
            analisisDal = analisisBL.Obten_Analisis(analisisID);
            var clienteApiReportes = new ClienteApiReportes();
            string nombreReportAnalisis = analisisDal.OrdenTrabajo.Norma.RequiereCompuesta
                ? "AnalisisFirmado"
                : "AnalsisisSDT";
            string nombreReportAcuse = analisisDal.Procesado ? "AcuseFirmadoVencido" : "AcuseFirmado";


            //Incidencia firma masiva
            queueBl.setEstatusAnalisisEncabezadoEnCorreo(analisisID, ColaID.Value, enumEstatusAnalisis.Finalizado.ParseToInt());

            byte[] fileAnalisisPdf = null;
            if (analisisDal.OrdenTrabajo.FechaEmision > ap.fechaNuevosReportes)
            {
                fileAnalisisPdf = clienteApiReportes.ObtenInformeLaboratorio(analisisID);
            }
            else
            {
                List<ReportParameter> listParametrosAnalisisFirmado = new List<ReportParameter>();
                ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
                listParametrosAnalisisFirmado.Add(rpAnalisisID);

                fileAnalisisPdf = OpenReports.BufferPDFRemoteReport(nombreReportAnalisis, listParametrosAnalisisFirmado);
            }


            byte[] fileAcusePdf = null;
            if (analisisDal.OrdenTrabajo.FechaEmision > ap.fechaNuevosReportes)
            {
                fileAcusePdf = clienteApiReportes.ObtenAcuseFirma(analisisID);
            }
            else
            {
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
                    !string.IsNullOrEmpty(analisisDal.Anexo.NombreCuerpoReceptor)
                        ? analisisDal.Anexo.NombreCuerpoReceptor
                        : analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
                ReportParameter paramCuerpoRece =
                    new ReportParameter("CuerpoReceptor", analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
                ReportParameter paramFrecMuestr = new ReportParameter("FrecuenciaMuestreo",
                    analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
                ReportParameter paramNormaOPmts = new ReportParameter("NormaOParametros", normaOParametros);
                ReportParameter paramUbicaMuest = new ReportParameter("UbicacionMuestreo", ubicacionDeMuestra);
                string selloDigitalCNA = analisisDal.AnalisisFirmas
                    .Last(a => a.AnalisisID == analisisDal.AnalisisID).SelloDigital;
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

                fileAcusePdf = OpenReports.BufferPDFRemoteReport(nombreReportAcuse, listParametrosAcuse);
            }



            if (fileAnalisisPdf != null && fileAcusePdf != null)
            {
                Hashtable ht = new Hashtable();
                ht.Add(nombreReportAnalisis + ".pdf", fileAnalisisPdf);
                ht.Add(nombreReportAcuse + ".pdf", fileAcusePdf);
                var dirEncabezado = Path.Combine(HttpRuntime.AppDomainAppPath, ap.MailImagePath.Replace("~", "").Replace("/", "\\").Substring(1));
                var dirFooter = Path.Combine(HttpRuntime.AppDomainAppPath, ap.MailImageFooterPath.Replace("~", "").Replace("/", "\\").Substring(1));
                ht.Add("enc_img_correo.png", dirEncabezado); //HttpContext.Current.Server.MapPath(ap.MailImagePath)
                ht.Add("Pie_Correo.png", dirFooter); //HttpContext.Current.Server.MapPath(ap.MailImageFooterPath)

                string[] ToArr = null;

                /* Llenar con las direcciones a quienes se les enviará el correo */
                ToArr = ToRecaudacion(analisisID);

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
                                     " a las " + DateTime.Now.ToString("HH:mm") + " horas, el informe número " +
                                     analisisDal.OrdenTrabajo.NumeroInforme + "/" +
                                     analisisBL.AnalisisNumero(analisisID) +
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
                            SIRALAB_BL.correo.Mailer.SendMail(to, "", "", Message,
                                "Acuse de recibo del informe del análisis de laboratorio", ht);
                        });

                        threadSendMails.IsBackground = true;
                        threadSendMails.Start();
                    }

                    //Si sale bien ponemos el estatus como finalizado.
                    //queueBl.setEstatusAnalisisEncabezadoEnCorreo(analisisID, ColaID.Value, enumEstatusAnalisis.Finalizado.ParseToInt());

                    //Pone el análisis como enviado.
                    analisisDal.Enviado = true;
                    analisisDal.FechaProceso = DateTime.Now;
                    analisisBL.UpdateAnalisisProcesoFirma(analisisDal);
                }
                catch (Exception ex)
                {
                    //Si falla el envío de correo ponemos el estatus como envío fallido:
                    queueBl.setEstatusAnalisisEncabezadoEnCorreo(analisisID, ColaID.Value, enumEstatusAnalisis.EnvioFallido.ParseToInt());
                    analisisDal.Enviado = false;
                    analisisBL.UpdateAnalisisProcesoFirma(analisisDal);
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                //Si no se generan los dos reportes correctamente ponemos el estatus como envío fallido.
                queueBl.setEstatusAnalisisEncabezadoEnCorreo(analisisID, ColaID.Value, enumEstatusAnalisis.EnvioFallido.ParseToInt());

                //Pone el análisis como NO enviado.
                analisisDal.Enviado = false;
                analisisBL.UpdateAnalisisProcesoFirma(analisisDal);
            }


        }

        //Agrega los correos que se tienen que enviar.
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

            //RFD 26/11/2020: se agrega para enviar el correo a todos los contactos que el laboratorio seleccione para recibir acuse:
            var aplicationSettings = new ApplicationSettings();
            if (analisi.OrdenTrabajo.FechaEmision >= aplicationSettings.fechaEnvioAcontactos)
            {
                var contactoBl = new Contacto_BL();
                var contactosLaboratorio = contactoBl.ObtenContactosCorreoAcuse(analisi.LaboratorioID)
                    .Where(contacto => contacto.RecibeAcuse == true).ToList();


                foreach (var contacto in contactosLaboratorio.Where(contacto => !arr.Contains(contacto.Correo)))
                {
                    arr.Add(contacto.Correo);
                }
            }

            /*cliente*/
            arr.Add(analisi.OrdenTrabajo.Cliente.Correo);

            return (String[])arr.ToArray(typeof(string));
        }

    }
}