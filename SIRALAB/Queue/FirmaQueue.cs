using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using SIRALAB.FIEL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_DAL;
using SIRALAB_DAL.Extensiones;

namespace SIRALAB.Queue
{
    public class FirmaQueue
    {


        private Queue<FIEL.Contenedor> _jobs = new Queue<FIEL.Contenedor>(); //Cola con los contenedores.
        private bool _delegateQueuedOrRunning = false;
        private CorreoQueue correoQueue = new CorreoQueue();
        private byte[] ArchivoKey { get; set; }
        private byte[] ArchivoCer { get; set; }
        private string CNT { get; set; }
        public int? ColaID { get; set; }
        public HttpServerUtility ServerUtility { get; set; }
        
        private Queue_BL queueBl = new Queue_BL();




        //Encola el objeto y lo agrega a un al pool de hilos (runtime)
        public void Enqueue(FIEL.Contenedor job)
        {
            lock (_jobs)
            {
                _jobs.Enqueue(job);

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
                FIEL.Contenedor item;
                lock (_jobs)
                {
                    if (_jobs.Count == 0)
                    {
                        //Si todos fallan hay que finalizar la cola que creamos al iniciar el proceso de firma: actualizar AnalisisColaDetalle.
                        if (ColaID != null)
                        {
                            var analisisColaEncabezados = queueBl.getAnalisisColaEncabezados(ColaID.Value);

                            if (analisisColaEncabezados.All(acd => acd.EstatusID == enumEstatusAnalisis.FirmaFallida.ParseToInt()))
                            {
                                queueBl.detenerProceso(ColaID);
                            }
                        }
                        _delegateQueuedOrRunning = false;
                        break;
                    }

                    item = _jobs.Dequeue();
                }
                try
                {
                    FirmaAnalisis(item);
                }
                catch
                {
                    //Si ocurre un error al firmar el análisis hay que ver qué estatus tiene el análisiso
                    var analisisBl = new Analisis_BL();
                    int analisisID = 0;
                    int.TryParse(item.primaryKey, out analisisID);
                    var analisisResult = analisisBl.Obten_Analisis(analisisID);

                    var analisisColaEncabezadoBL = new AnalisisColaEncabezado_BL();
                    var analisisColaEncabezado =
                        analisisColaEncabezadoBL.FindByAnalisisIDAndColaID(analisisResult.AnalisisID, ColaID.Value);

                    analisisColaEncabezado.EstatusID =
                        analisisColaEncabezado.EstatusID == enumEstatusAnalisis.EnProceso.ParseToInt()
                            ? enumEstatusAnalisis.FirmaFallida.ParseToInt()
                            : enumEstatusAnalisis.EnvioFallido.ParseToInt();

                    analisisColaEncabezadoBL.SaveOrUpdate(analisisColaEncabezado);

                    ThreadPool.QueueUserWorkItem(ProcessQueuedItems, null);
                }
            }
        }


        private void FirmaAnalisis(FIEL.Contenedor contenedor)
        {
            Thread.Sleep(50);
            var analisisBl = new Analisis_BL();
            int analisisID = 0;
            int.TryParse(contenedor.primaryKey, out analisisID);
            var analisisResult = analisisBl.Obten_Analisis(analisisID);

            var OrdenTrabajoID = analisisResult.OrdenTrabajoID;

            string[] split = contenedor.cadenaAFirmar.Split(new[] { '|' });
            var fech = split[10].Substring(35, 19);

            var provider = CultureInfo.InvariantCulture;
            const string format = "dd-MM-yyyy HH:mm:ss";
            var date = DateTime.ParseExact(fech, format, provider);

            var tabla = new LaboratoriosCNA_BL();
            var restabla = tabla.ObtenLaboratorioCna(analisisResult.LaboratorioID);

            if (analisisResult.VerificaBanderaMuestraCompuesta())
            {

                string resultPKCS7 = contenedor.cadenaCMS;
                string resultFirmaElectronica = contenedor.firmaDigital;
                string resutlCadenaOriginal = contenedor.cadenaAFirmar;
                string resultCertificado = contenedor.cetificadoDigital;

                var analisisFirmaBL = new AnalisisFirma_BL();
                var args = new AnalisisFirmaArgs
                {
                    AnalisisFirmaID = null,
                    AnalisisID = analisisID,
                    RFC = contenedor.txtRFC,
                    Firma = contenedor.firmaDigital,
                    Cadena = contenedor.cadenaAFirmar,
                    Contenedor = contenedor.cetificadoDigital,
                    FechaRegistro = date
                };

                try
                {
                    SelloDigital sello = new SelloDigital();
                    sello.cadenaOriginal = contenedor.cadenaAFirmar;
                    sello.SellarCadenaOriginal();
                    args.SelloCNA = sello.SellarCadenaOriginalSirfic();

                    // Validación: no guardar el análisis como firmado si la selladora de CONAGUA
                    // (ServicioSelladoSoap.Generar / SIRFIC) no devolvió un sello válido.
                    // Si el sello viene vacío o con el mensaje de error, se aborta el guardado
                    // lanzando una excepción; el catch marcará el análisis como FirmaFallida
                    // para que pueda reintentarse, en lugar de persistir un SelloDigital en blanco.
                    if (string.IsNullOrWhiteSpace(args.SelloCNA) ||
                        args.SelloCNA.IndexOf("Hubo algún problema", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        throw new Exception(
                            "La selladora de CONAGUA no devolvió un sello válido para el análisis " +
                            analisisID + "; se aborta el guardado para no registrar un sello en blanco.");
                    }

                    var analisis = analisisFirmaBL.Guardar(args);

                    //TODO -> Aquí enviar al web service para validación.


                    correoQueue.ColaID = this.ColaID;
                    correoQueue.ServerUtility = ServerUtility;

                    if (restabla != null)
                    {
                        if (restabla.ValExtemporaneo.ToString() == "True")
                            analisisBl.ActualizaExtemporaneo(analisisID);

                        if (restabla.ValCorreo == false)
                        {
                            queueBl.setEstatusAnalisisEncabezadoEnFirma(analisisID, ColaID.Value, enumEstatusAnalisis.Firmado.ParseToInt());
                            correoQueue.Enqueue(analisisID);
                        }
                    }
                    else
                    {
                        if (analisis != null)
                        {
                            queueBl.setEstatusAnalisisEncabezadoEnFirma(analisisID, ColaID.Value, enumEstatusAnalisis.Firmado.ParseToInt());
                            correoQueue.Enqueue(analisisID);
                        }
                    }
                }
                catch (Exception e)
                {
                    queueBl.setEstatusAnalisisEncabezadoEnFirma(analisisID, ColaID.Value, enumEstatusAnalisis.FirmaFallida.ParseToInt());
                    analisisResult.Firmada = false;
                    analisisBl.UpdateAnalisisProcesoFirma(analisisResult);
                }

            }
            else //Si el flag de la muestra compuesta no es true marcamos El AnalisisColaEncabezado como FirmaFallida
            {
                queueBl.setEstatusAnalisisEncabezadoEnFirma(analisisID, ColaID.Value, enumEstatusAnalisis.FirmaFallida.ParseToInt());
                analisisResult.Firmada = false;
                analisisBl.UpdateAnalisisProcesoFirma(analisisResult);
            }
        }
    }
}



