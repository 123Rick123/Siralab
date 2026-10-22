using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using DL_WS_SIRALAB;
using BL_WS_SIRALAB;
using BL_WS_SIRALAB.listados;
using BL_WS_SIRALAB.negocio;
using BL_WS_SIRALAB.extend;
using COM_WS_SIRALAB;

using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using Microsoft.Reporting.WebForms;
using BL_WS_SIRALAB.clases;
using System.Diagnostics;
using BL_WS_SIRALAB.ClienteAPI;
using Cliente = BL_WS_SIRALAB.Cliente;
using Norma = BL_WS_SIRALAB.Norma;
using System.Configuration;

namespace WS_SIRALAB
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wsSIRALAB" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wsSIRALAB.svc or wsSIRALAB.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [KnownType(typeof(BL_WS_SIRALAB.PeriodResult))]
    [KnownType(typeof(List<BL_WS_SIRALAB.PeriodResult>))]
    [KnownType(typeof(BL_WS_SIRALAB.Analisis))]
    [KnownType(typeof(List<BL_WS_SIRALAB.Analisis>))]
    [KnownType(typeof(BL_WS_SIRALAB.Muestra))]
    [KnownType(typeof(List<BL_WS_SIRALAB.Muestra>))]
    [KnownType(typeof(BL_WS_SIRALAB.ParametroAnalizado))]
    [KnownType(typeof(List<BL_WS_SIRALAB.ParametroAnalizado>))]

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class wsSIRALAB : IwsSIRALAB
    {

        public static string errorMuestra = String.Empty;

        #region Operaciones del Servicio Web SVC
        ///  TODO: No está quitando el análisis con marca de falta de pago
        public List<PeriodResult> Obten_Informacion(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion)
        {

            errorMuestra = string.Empty;

            Analisis_BL aBL = new Analisis_BL();
            Norma_BL nBL = new Norma_BL();
            Cliente_BL cBL = new Cliente_BL();
            Anexo_BL axBL = new Anexo_BL();
            string mensaje = "";
            var NombreNorma = string.Empty;
            var t1 = DateTime.Now;
            try
            {
                List<PeriodResult> lPR = new List<PeriodResult>();
                /// Acá la carga de la información que se desea adquirir por medio del servicio.
                /// 
                PeriodResult pr = new PeriodResult();
                pr.EstaInscritoNom001 = EstaInscritoNom001(Anexo, Titulo, ClienteID, FechaInicio);

                if (NormaID == 3)
                {
                    if (FechaInicio.Year >= int.Parse(ConfigurationManager.AppSettings["ValidaProgramaSSTDQOCOT"]))
                    {
                        Norma2021_BL nom2021 = new Norma2021_BL();
                        NormaID = nom2021.ObtenerNormaIDByNombreSSTDQOCOT("SST, DQO / COT");
                        var nom = nom2021.ObtenerNormaByID(NormaID);
                        if (nom != null)
                        {
                            NombreNorma = nom.Nombre;
                        }
                    }
                }
                else
                {
                    if (int.Parse(ConfigurationManager.AppSettings["ValidaPrograma"]) <= FechaInicio.Year)
                    {
                        if (pr.EstaInscritoNom001 == false)
                        {
                            if (NormaID == 1)
                            {
                                Norma2021_BL nom2021 = new Norma2021_BL();
                                NormaID = nom2021.ObtenerNormaIDByNombre("NOM-001-SEMARNAT-2021");
                            }

                        }
                    }
                }
                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                var norma = nBL.Obten_Norma(NormaID);
                //Validamos que los parametros estan activos 
                NormaConfiguracion_BL Configuracion = new NormaConfiguracion_BL();
                //se comento porque la validacion cambio en declaragua
                //if (norma.Nombre == "NOM-001-SEMARNAT-2021")
                //{
                //    foreach (var item in lAnalisis)
                //    {

                //        foreach (var muestra in item.Muestras)
                //        {
                //            List<NormaConfiguracion> ConfiguracionLocalizada = Configuracion.Obten_Configuracion(NormaID, item.OrdenTrabajo.CuerpoReceptorID.Value, item.OrdenTrabajo.UsoAguaID.Value, (enumNivel)muestra.NivelID);
                //            List<MuestraValor> ListaMV = new List<MuestraValor>();
                //            foreach (var mv in muestra.MuestraValors)
                //            {
                //                var parametroActivo = ConfiguracionLocalizada.FirstOrDefault(x => x.ParametroID == mv.ParametroID).Activo;
                //                if (parametroActivo)
                //                {
                //                    if (mv.NoAnalizado)
                //                    {
                //                        mv.Evalua = false;
                //                    }
                //                    ListaMV.Add(mv);
                //                }


                //            }
                //            muestra.MuestraValors = ListaMV;
                //        }
                //    }
                //}


                pr.Fuente_Abastecimiento = false;
                List<Analisis> listadoAnalisis = new List<Analisis>();
                pr.analisis = new List<Analisis>();
                var analisisList = lAnalisis.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();
                foreach (var analisisItem in analisisList)
                {
                    listadoAnalisis.Add(analisisItem);
                }

                if (lAnalisis.Count() == 0)
                {
                    //Ya no cumple y se debe enviar el mensaje
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                    //pr.error = true;
                    //mensaje += "\nEl cliente no tiene análisis en el periodo.";
                }
                else if (lAnalisis.Count() < 2 && norma.RequiereMensual)
                {
                    //Ya no cumple y se debe enviar el mensaje
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.Incompleto;
                    //pr.error = true;                        
                    //mensaje += "\nEl cliente tiene menos de 2 promedios diarios en el periodo.";
                }
                else if (lAnalisis.Count() >= 2)
                {
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.Completo;
                }

                #region Mensual
                /// 
                /// Acá se tiene que agregar el análisis de la mensual si es requerido
                ///
                List<Analisis> lMensual = new List<Analisis>();
                if (norma.RequiereMensual)
                    lMensual = aBL.Obten_AnalisisMensual(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);

                if (lMensual.Count > 0)
                {
                    if (lMensual[0].muestras != null)
                    {
                        if (lMensual[0].muestras.Count > 0)
                        {
                            foreach (var parametroA in from parametroA in lMensual[0].muestras[0].Resultados where parametroA.grupo != null where parametroA.grupo.Count > 0 from ParA in parametroA.grupo from Analisis in analisisList from MuestraAnalisis in Analisis.muestras where MuestraAnalisis.nivel == enumNivel.Compuesta where (bool)MuestraAnalisis.Resultados.FirstOrDefault(t => t.parametro.ParametroID == ParA.ParametroID).no_analizado select parametroA)
                            {
                                parametroA.valor_validacion = false;
                            }
                        }
                    }
                }
                foreach (var mensualItem in lMensual)
                {
                    listadoAnalisis.Add(mensualItem);
                }
                #endregion




                pr.analisis = listadoAnalisis;
                bool RespuesPDValidos = false;
                pr.valida = Validacion(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion, ref RespuesPDValidos);
                pr.TodosPDValidos = RespuesPDValidos;
                pr.TieneReincidencia = TieneReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                pr.TieneReincidenciaPreventiva = TieneReincidenciaPreventiva(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                pr.TieneApercibimiento = TieneApercibimiento(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                pr.TieneReincidenciaConImpugnacion = TieneReincidenciaImpugnada(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);



                /*Agrega el error de la muestra*/
                if (errorMuestra != String.Empty)
                {
                    //pr.error = true;
                    mensaje += errorMuestra;
                }

                #region Cliente
                var pr_cliente = cBL.Obten_Cliente(ClienteID);
                if (pr_cliente == null)
                {
                    pr.error = true;
                    mensaje += "\nCliente no registrado.";
                }
                else
                {
                    pr.cliente = pr_cliente.ConvertCliente();
                }
                #endregion

                #region Consecion y Anexo
                var pr_anexo = axBL.Obten_Anexo(Titulo, Anexo, ClienteID);
                if (pr_anexo == null)
                {
                    pr.error = true;
                    mensaje += "\nTítulo o anexo no registrado.";
                }
                else
                {
                    pr.anexo = pr_anexo.ConvertAnexo();
                    pr.consecion = pr.anexo.Titulo;
                }
                pr.norma = norma.ConvertNorma();
                #endregion

                #region Laboratorio aprobacion
                /*RTG 2015 06 26*/
                vw_LaboratoriosPALA_BL BL_vw = new vw_LaboratoriosPALA_BL();
                foreach (Analisis analisis in listadoAnalisis)
                {

                    vw_LaboratoriosPALA pala = BL_vw.GetDatos(analisis.laboratorio.rfc);

                    if (pala != null)
                    {
                        analisis.laboratorio.Aprobacion = pala.Aprobacion;
                        analisis.laboratorio.AcreditacionEMA = pala.AcreditacionEMA;

                    }

                }
                /*end*/
                #endregion

                lPR.Add(pr);

                if (pr.error)
                {
                    pr.exception = new SiralabFaultException(mensaje);
                }
                //Nueva funcionalidad para norma nueva 
                if (NormaID == 15 || NormaID == 16 || NormaID == 17)
                {
                    var NormaDQOCOT = string.Empty;
                    foreach (var item in lPR)
                    {
                        if (item.analisis.Count > 0)
                        {
                            NormaDQOCOT = item.analisis.First().NormaDQOCOT;
                            foreach (var an in item.analisis)
                            {
                                if (NormaDQOCOT != null && NormaDQOCOT.Contains("COT"))
                                {
                                    foreach (var m in an.muestras)
                                    {
                                        if (m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {

                                                if (res.parametro.nombre.Contains("DQO") || res.parametro.nombre.Contains("Demanda Química de Oxigeno"))
                                                {
                                                    m.Resultados.RemoveAll(x => x.parametro.nombre.Contains("DQO") || x.parametro.nombre.Contains("Demanda Química de Oxigeno"));
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }
                                if (NormaDQOCOT != null && NormaDQOCOT.Contains("DQO"))
                                {
                                    foreach (var m in an.muestras)
                                    {
                                        if (m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.nombre.Contains("COT") || res.parametro.nombre.Contains("Carbono Orgánico Total"))
                                                {
                                                    m.Resultados.RemoveAll(x => x.parametro.nombre.Contains("COT") || x.parametro.nombre.Contains("Carbono Orgánico Total"));
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }


                            }
                        }
                    }
                }

                return lPR;
            }
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += " " + e.InnerException.Message;
                }

                List<PeriodResult> lPR = new List<PeriodResult>();
                PeriodResult pr = new PeriodResult();

                pr.Fuente_Abastecimiento = false;
                pr.TodosPDValidos = false;
                pr.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                pr.valida = false;

                pr.error = true;
                pr.exception = new SiralabFaultException(e.Message);
                lPR.Add(pr);

                return lPR;
            }

        }
        ///  TODO: No está quitando el análisis con marca de falta de pago
        public List<PeriodResult> Obten_Informacion_PD(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion)
        {
            Analisis_BL aBL = new Analisis_BL();
            Norma_BL nBL = new Norma_BL();
            Cliente_BL cBL = new Cliente_BL();
            Anexo_BL axBL = new Anexo_BL();
            try
            {
                List<PeriodResult> lPR = new List<PeriodResult>();
                /// Acá la carga de la información que se desea adquirir por medio del servicio.
                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                var norma = nBL.Obten_Norma(NormaID);

                PeriodResult pr = new PeriodResult();
                pr.Fuente_Abastecimiento = false;
                List<Analisis> listadoAnalisis = new List<Analisis>();
                pr.analisis = new List<Analisis>();
                var analisisList = lAnalisis.Select(p => p.ConvertAnalisisPD(Evaluacion)).ToList();
                if (lAnalisis.Count() == 0)
                {
                    //Ya no cumple y se debe enviar el mensaje
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                }
                else if (lAnalisis.Count() < 2 && norma.RequiereMensual)
                {
                    //Ya no cumple y se debe enviar el mensaje
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.Incompleto;
                    //pr.error = true;
                    //pr.exception = new SiralabFaultException("El cliente solo tiene menos de 2 promedios diarios en el periodo.");
                }
                else if (lAnalisis.Count() >= 2)
                {
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.Completo;
                }


                foreach (var analisisItem in analisisList)
                {
                    listadoAnalisis.Add(analisisItem);
                }

                /// 
                /// Acá se tiene que agregar el análisis de la mensual si es requerido
                ///
                List<Analisis> lMensual = new List<Analisis>();

                if (norma.RequiereMensual)
                    lMensual = aBL.Obten_AnalisisMensual(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);

                foreach (var mensualItem in lMensual)
                {
                    listadoAnalisis.Add(mensualItem);
                }

                pr.analisis = listadoAnalisis;
                bool RespuesPDValidos = false;
                pr.valida = Validacion(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion, ref RespuesPDValidos);
                pr.TodosPDValidos = RespuesPDValidos;
                pr.cliente = cBL.Obten_Cliente(ClienteID).ConvertCliente();
                pr.anexo = axBL.Obten_Anexo(Titulo, Anexo, ClienteID).ConvertAnexo();
                pr.consecion = pr.anexo.Titulo;
                pr.norma = norma.ConvertNorma();

                lPR.Add(pr);

                Trace.WriteLine(lPR);
                return lPR;
            }
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += " " + e.InnerException.Message;
                }

                List<PeriodResult> lPR = new List<PeriodResult>();
                PeriodResult pr = new PeriodResult();

                pr.error = true;
                pr.exception = new SiralabFaultException(e.Message);
                lPR.Add(pr);

                return lPR;
            }
        }
        /// <summary>
        /// Obtiene el listado de normas activas en SIRALAB
        /// </summary>
        /// <returns>Listado de Normas</returns>
        public List<BL_WS_SIRALAB.Norma> Obten_Normas()
        {
            List<BL_WS_SIRALAB.Norma> lN = new List<BL_WS_SIRALAB.Norma>();
            Normas_BL nBL = new Normas_BL();
            lN = nBL.Obten_Normas();
            return lN;
        }
        ///  TODO: No está quitando el análisis con marca de falta de pago
        public Validacion Listado_Validacion(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion)
        {
            Analisis_BL aBL = new Analisis_BL();
            Norma_BL nBL = new Norma_BL();
            Validacion respuesta = new Validacion();

            try
            {
                bool respuestaPD = false;
                respuesta.EstaInscritoNom001 = EstaInscritoNom001(Anexo, Titulo, ClienteID, FechaInicio);
                if (NormaID == 3)
                {
                    if (FechaInicio.Year >= int.Parse(ConfigurationManager.AppSettings["ValidaProgramaSSTDQOCOT"]))
                    {
                        Norma2021_BL nom2021 = new Norma2021_BL();
                        NormaID = nom2021.ObtenerNormaIDByNombreSSTDQOCOT("SST, DQO / COT");
                    }
                }
                else
                {
                    if (int.Parse(ConfigurationManager.AppSettings["ValidaPrograma"]) <= FechaInicio.Year)
                    {
                        if (respuesta.EstaInscritoNom001 == false)
                        {
                            if (NormaID == 1)
                            {
                                Norma2021_BL nom2021 = new Norma2021_BL();
                                NormaID = nom2021.ObtenerNormaIDByNombre("NOM-001-SEMARNAT-2021");
                            }

                        }
                    }
                }

                respuesta.Fuente_Abastecimiento = false;
                respuesta.Valido = Validacion(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion, ref respuestaPD);
                respuesta.TodosPDValidos = respuestaPD;
                //Banderas para mensajes en Declaragua
                respuesta.TieneReincidencia = TieneReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.TieneReincidenciaPreventiva = TieneReincidenciaPreventiva(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.TieneApercibimiento = TieneApercibimiento(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.TieneReincidenciaConImpugnacion = TieneReincidenciaImpugnada(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.Analisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID).Select(p => p.AnalisisID).ToList();
                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                var analisisList = lAnalisis.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();
                respuesta.AnalisisEvaluacionDeclaragua = analisisList;
                var norma = nBL.Obten_Norma(NormaID);

                Norma n = new Norma();
                n.id = norma.NormaID;
                n.NormaID = norma.NormaID;
                n.Nombre = norma.Nombre;
                n.Vigente = norma.Activo;
                n.Descripcion = norma.Descripcion;


                respuesta.Norma = n;
                if (norma.RequiereMensual)
                {
                    respuesta.AnalisisMensual = aBL.Obten_AnalisisMensual(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);
                }

                if (respuesta.Analisis.Count == 0)
                {
                    respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                    //respuesta.error = true;
                    //respuesta.exception = new SiralabFaultException("El cliente no tiene análisis en el periodo.");
                }
                else if (respuesta.Analisis.Count < 2 && norma.RequiereMensual)
                {
                    respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.Incompleto;
                    //respuesta.error = true;
                    //respuesta.exception = new SiralabFaultException("El cliente solo tiene menos de 2 promedios diarios en el periodo.");
                }
                else if (respuesta.Analisis.Count >= 2)
                {
                    respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.Completo;
                }
            }
            catch (Exception e)
            {
                respuesta.Fuente_Abastecimiento = false;
                respuesta.TodosPDValidos = false;
                respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                respuesta.Valido = false;

                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += " " + e.InnerException.Message;
                }
                respuesta.error = true;
                respuesta.exception = new SiralabFaultException(message);
            }
            return respuesta;
        }

        //RFD 29/01/2021: se agrega reporte generado desde web service.
        public byte[] Archivo_PDF_Analisis(int AnalisisID)
        {
            byte[] report;
            string nombreReport;
            Analisis_BL an = new Analisis_BL();
            var Compuesta = an.Compuesta(AnalisisID);
            var analisis = an.GetAnalisiByAnalisisId(AnalisisID);
            nombreReport = Compuesta ? "AnalsisisSDT" : "AnalisisFirmado";
            var fechaPublicacion = Convert.ToDateTime(ConfigurationManager.AppSettings["fechaNuevosReportes"]);

            if (analisis != null && analisis.Muestras.FirstOrDefault()?.Fecha >= fechaPublicacion)
            {
                var cliente = new ClienteApiReportes();
                report = cliente.ObtenInformeLaboratorio(AnalisisID);
            }
            else
            {
                List<ReportParameter> listParametros = new List<ReportParameter>();
                ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", AnalisisID.ToString());
                listParametros.Add(rpAnalisisID);

                report = Reports.BufferPDFRemoteReport(nombreReport, listParametros);
            }


            an.Dispose();
            return report;
        }

        //RFD 29/01/2021: se agrega reporte generado desde web service.
        public byte[] Archivo_PDF_Mensual(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion)
        {
            byte[] report = null;
            string nombreReport = null;
            Anexo_BL anx = new Anexo_BL();
            Norma_BL nor = new Norma_BL();
            var pr_anexo = anx.Obten_Anexo(Titulo, Anexo, ClienteID);
            var pr_norma = nor.Obten_Norma(NormaID);

            //RFD -> Fecha a partir de la cual se generan los nuevos reportes.
            var fechaPublicacion = Convert.ToDateTime(ConfigurationManager.AppSettings["fechaNuevosReportes"]);

            if (pr_norma.RequiereMensual)
            {
                if (FechaInicio < fechaPublicacion)
                {
                    nombreReport = "ReporteMensual";
                    List<ReportParameter> listParametros = new List<ReportParameter>();

                    ReportParameter pAnexoID = new ReportParameter("AnexoID", pr_anexo.AnexoID.ToString());
                    ReportParameter pNormaID = new ReportParameter("NormaID", NormaID.ToString());
                    ReportParameter pClienteId = new ReportParameter("Cliente", ClienteID.ToString());
                    ReportParameter pFechaInicio = new ReportParameter("FechaInicio", FechaInicio.ToShortDateString());
                    ReportParameter pFechaFin = new ReportParameter("FechaFin", FechaFin.ToShortDateString());
                    ReportParameter pTipoEvaluacion = new ReportParameter("TipoEvaluacion", Evaluacion.ToInt().ToString());

                    listParametros.Add(pClienteId);
                    listParametros.Add(pNormaID);
                    listParametros.Add(pAnexoID);
                    listParametros.Add(pFechaInicio);
                    listParametros.Add(pFechaFin);
                    listParametros.Add(pTipoEvaluacion);

                    report = Reports.BufferPDFRemoteReport(nombreReport, listParametros);
                }
                else
                {
                    var cliente = new ClienteApiReportes();
                    report = cliente.ObtenInformeMensual(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);
                }
            }

            anx.Dispose();
            nor.Dispose();
            return report;
        }
        #endregion

        #region Metodos Secundarios
        ///  TODO: No está quitando el análisis con marca de falta de pago
        public bool Validacion(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, ref bool PDValido)
        {
            try
            {

                /// Acá vamos a realizar la validación de los análisis firmados y que se encuentran dentro del periodo de análisis
                /// 
                bool ValidoSimple = false;
                bool ValidoCompuesta = false;
                bool ValidoMensual = false;
                bool Valido = false;
                /// Obtenemos los análisis firmados dentro del periodo consultado.
                /// 
                Analisis_BL aBL = new Analisis_BL();

                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                if (lAnalisis.Count > 0)
                {
                    /// Ahora validaremos por muestra los valores capturados por los laboratorios
                    /// 
                    foreach (var Analisis in lAnalisis)
                    {
                        foreach (var Muestra in Analisis.Muestras)
                        {
                            /// Ya con la muestra en memoria procedemos a generar las validaciones por parámetro
                            ///
                            var Nivel = Muestra.NivelID.ToEnumNivel();
                            switch (Nivel)
                            {
                                //se validaria aqui para mostrar cumple o no cumple en declaragua
                                case enumNivel.Simple:
                                    NormaConfiguracion_BL Configuracion = new NormaConfiguracion_BL();
                                    List<NormaConfiguracion> ConfiguracionLocalizada = Configuracion.Obten_Configuracion(NormaID, (int)Analisis.OrdenTrabajo.CuerpoReceptorID, (int)Analisis.OrdenTrabajo.UsoAguaID, enumNivel.Simple);

                                    ValidoSimple = (!Muestra.Cancelado) ? MuestraValida(Muestra, Evaluacion, ConfiguracionLocalizada) : true;
                                    if (!ValidoSimple)
                                    {
                                        errorMuestra = "\nMuestra simple no valida";
                                        PDValido = false;
                                        return false;
                                    }
                                    break;
                                case enumNivel.Compuesta:
                                    NormaConfiguracion_BL ConfiguracionCompuesta = new NormaConfiguracion_BL();
                                    List<NormaConfiguracion> ConfiguracionLocalizadaC = ConfiguracionCompuesta.Obten_Configuracion(NormaID, (int)Analisis.OrdenTrabajo.CuerpoReceptorID, (int)Analisis.OrdenTrabajo.UsoAguaID, enumNivel.Compuesta);

                                    ValidoCompuesta = (!Muestra.Cancelado) ? MuestraValida(Muestra, Evaluacion, ConfiguracionLocalizadaC) : true;
                                    if (!ValidoCompuesta)
                                    {
                                        errorMuestra = "\nMuestra compuesta no valida";
                                        PDValido = false;
                                        return false;
                                    }
                                    break;
                            }
                        }
                    }
                }
                Norma_BL nBL = new Norma_BL();
                var norma = nBL.Obten_Norma(NormaID);
                if (norma.RequiereMensual)
                    ValidoMensual = MensualValida(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);
                else
                    ValidoMensual = true;

                if (ValidoSimple && ValidoCompuesta)
                {
                    PDValido = true;
                }

                if (!ValidoSimple || !ValidoCompuesta || !ValidoMensual)
                    return false;

                Valido = ValidoSimple && ValidoCompuesta && ValidoMensual;

                return Valido;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private bool MuestraValida(DL_WS_SIRALAB.Muestra Muestra, enumTipoEvaluacion Evaluacion, List<NormaConfiguracion> Configuracion)
        {
            bool valida = false;
            var FechaLimiteParametrosNomOO1 = Convert.ToDateTime(ConfigurationManager.AppSettings["FechaLimiteParametrosNomOO1"]);
            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;


            foreach (var Parametro in Muestra.MuestraValors)
            {
                if ((Parametro.ParametroID == 931 && Muestra.Fecha.HasValue && Muestra.Fecha.Value < FechaLimiteParametrosNomOO1)
                    || ((Parametro.ParametroID == 1072 || Parametro.ParametroID == 1073 || Parametro.ParametroID == 1074)
                        && Muestra.FechaConformacion.HasValue && Muestra.FechaConformacion.Value < FechaLimiteParametrosNomOO1))
                {
                    //valida = true;
                    continue;
                }
                if ((Configuracion.First().Norma.Nombre == "NOM-001-SEMARNAT-2021" && Parametro.NoAnalizado == true) || (Configuracion.First().Norma.Nombre == "NOM-001-SEMARNAT-2021" && Muestra.Caudal == 0))
                {
                    valida = true;
                    break;
                }
                else
                {

                    if (Parametro.Grupo && Parametro.Evalua && !Parametro.NoAnalizado && !Parametro.Exento && Parametro.LFD)
                    {
                        NormaConfiguracion ConfiguracionPadre = Configuracion.Where(Z => Z.ParametroID == Parametro.ParametroID).FirstOrDefault();

                        foreach (var ParametroHijo in Muestra.NivelConfiguracion.NormaConfiguracions.Where(h => h.NormaConfiguracionPadreID == ConfiguracionPadre.NormaConfiguracionID))
                        {
                            if (Muestra.MuestraValors.Where(z => z.ParametroID == ParametroHijo.ParametroID).FirstOrDefault().NoAnalizado)
                            {
                                valida = false;
                                break;
                            }

                        }
                    }
                    else if (!Parametro.NoAnalizado && !Parametro.Exento && Parametro.Evalua)
                    {
                        //if (Parametro.ParametroID == 89)
                        //{
                        //    string x = "";
                        //}
                        if (Parametro.AceptaValor)
                        {
                            /// Primero buscamos sí se requiere evaluar por medio de LFD
                            /// 
                            switch (Evaluacion)
                            {
                                case enumTipoEvaluacion.Evaluar_Simple:
                                    #region Evaluación respecto a los límites de norma
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.LimiteInferior != null)
                                    {
                                        valida = Parametro.LimiteInferior <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperior;
                                    }
                                    else
                                    {
                                        valida = Parametro.Valor <= Parametro.LimiteSuperior;
                                    }
                                    #endregion
                                    break;
                                case enumTipoEvaluacion.Evaluar_LFD:
                                    if (Parametro.LFD)
                                    {
                                        #region Evaluación respecto a los límites de LFD
                                        ///Verificamos que tenga límite inferior para saber si es un rango
                                        ///
                                        if (Parametro.LimiteInferiorLFD != null)
                                        {
                                            valida = Parametro.LimiteInferiorLFD <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                        }
                                        else
                                        {
                                            valida = Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        valida = true;
                                    }
                                    break;
                                case enumTipoEvaluacion.Evaluar_LFD_Simple:
                                    if (Parametro.LFD)
                                    {
                                        #region Evaluación respecto a los límites de LFD
                                        ///Verificamos que tenga límite inferior para saber si es un rango
                                        ///
                                        if (Parametro.LimiteInferiorLFD != null)
                                        {
                                            valida = Parametro.LimiteInferiorLFD <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                        }
                                        else
                                        {
                                            valida = Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Evaluación respecto a los límites de norma
                                        ///Verificamos que tenga límite inferior para saber si es un rango
                                        ///
                                        if (Parametro.LimiteInferior != null)
                                        {
                                            valida = Parametro.LimiteInferior <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperior;
                                        }
                                        else
                                        {
                                            valida = Parametro.Valor <= Parametro.LimiteSuperior;
                                        }
                                        #endregion
                                    }
                                    break;
                                default:
                                    valida = true;
                                    break;
                            }
                        }
                        else
                        {
                            valida = (bool)Parametro.ValorValidacion;
                        }
                    }
                    else if (!Parametro.Evalua)
                    {
                        valida = true;
                    }
                    else if (Parametro.Evalua && Parametro.NoAnalizado && Parametro.LFD)
                    {
                        valida = false;
                    }
                    else if (Parametro.Evalua && Parametro.NoAnalizado && !Parametro.LFD)
                    {
                        valida = true;
                    }
                    else if (Parametro.Exento)
                    {
                        valida = true;
                    }
                    if (!valida)
                        break;
                }
            }



            return valida;
        }
        private bool MuestraValida(BL_WS_SIRALAB.Muestra Muestra, enumTipoEvaluacion Evaluacion)
        {
            bool valida = false;
            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;

            foreach (var Parametro in Muestra.Resultados)
            {
                if (Parametro.evalua != null ? (bool)Parametro.evalua : false)
                {
                    if (Parametro.acepta_valor != null ? (bool)Parametro.acepta_valor : false)
                    {
                        /// Primero buscamos sí se requiere evaluar por medio de LFD
                        /// 
                        switch (Evaluacion)
                        {
                            case enumTipoEvaluacion.Evaluar_Simple:
                                #region Evaluación respecto a los límites de norma
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (Parametro.limite_inferior != null)
                                {
                                    valida = Parametro.limite_inferior <= Parametro.valor && Parametro.valor <= Parametro.limite_superior;
                                }
                                else
                                {
                                    valida = Parametro.valor <= Parametro.limite_superior;
                                }
                                #endregion
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD:
                                if (Parametro.ley_federal != null ? (bool)Parametro.ley_federal : false)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.limite_inferior_LFD != null)
                                    {
                                        valida = Parametro.limite_inferior_LFD <= Parametro.valor && Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    else
                                    {
                                        valida = Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    valida = true;
                                }
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD_Simple:
                                if (Parametro.ley_federal != null ? (bool)Parametro.ley_federal : false)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.limite_inferior_LFD != null)
                                    {
                                        valida = Parametro.limite_inferior_LFD <= Parametro.valor && Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    else
                                    {
                                        valida = Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Evaluación respecto a los límites de norma
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.limite_inferior != null)
                                    {
                                        valida = Parametro.limite_inferior <= Parametro.valor && Parametro.valor <= Parametro.limite_superior;
                                    }
                                    else
                                    {
                                        valida = Parametro.valor <= Parametro.limite_superior;
                                    }
                                    #endregion
                                }
                                break;
                            default:
                                valida = true;
                                break;
                        }

                    }
                    else
                    {
                        valida = (bool)Parametro.valor_validacion;
                    }
                }
                else
                {
                    valida = true;
                }

                if (!valida)
                    break;
            }

            return valida;
        }
        private bool MensualValida(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion)
        {
            bool valida = true;

            /// Llamamos al stored procedure que consolida y valida la información
            /// Primero se debe consolidar la información y finalmente se deberá generar la validación respecto a los límites mensuales
            /// 
            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;
            Analisis_BL aBL = new Analisis_BL();

            List<Analisis> lMensual = new List<Analisis>();
            lMensual = aBL.Obten_AnalisisMensual(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);

            var muestras = lMensual.SelectMany(p => p.muestras);

            foreach (var muestra in muestras)
            {
                valida = MuestraValida(muestra, Evaluacion);
                if (!valida)
                    break;
            }

            return valida;
        }
        #endregion

        #region Fuente_de_abastecimiento
        #region Metodos_Principales_Fuente_Abastecimiento
        public Validacion Listado_Validacion_Fuente(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, List<int> MedidorID)
        {
            Analisis_BL aBL = new Analisis_BL();
            List<int> AnalisisEvaluar = new List<int>();
            Norma_BL nBL = new Norma_BL();
            Validacion respuesta = new Validacion();
            //MedidorID.Add(36594);
            try
            {
                bool respuestaPD = false;
                respuesta.EstaInscritoNom001 = EstaInscritoNom001(Anexo, Titulo, ClienteID, FechaInicio);
                //if (NormaID == 3)
                //{
                //    if (FechaInicio.Year >= int.Parse(ConfigurationManager.AppSettings["ValidaProgramaSSTDQOCOT"]))
                //    {
                //        Norma2021_BL nom2021 = new Norma2021_BL();
                //        NormaID = nom2021.ObtenerNormaIDByNombreSSTDQOCOT("SST, DQO / COT");
                //    }
                //}
                //else
                //{
                if (int.Parse(ConfigurationManager.AppSettings["ValidaPrograma"]) <= FechaInicio.Year)
                {
                    if (respuesta.EstaInscritoNom001 == false)
                    {
                        if (NormaID == 1)
                        {
                            Norma2021_BL nom2021 = new Norma2021_BL();
                            NormaID = nom2021.ObtenerNormaIDByNombre("NOM-001-SEMARNAT-2021");
                        }

                    }
                    //Se verifican y se obtienen las fuentes de abastecimiento.
                }
                //}
                respuesta.Fuente_Abastecimiento = false;

                //Se verifica que se tengan los analisis necesarios para realizar la evaluacion
                AnalisisEvaluar = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID).Select(p => p.AnalisisID).ToList();
                var norma = nBL.Obten_Norma(NormaID);
                Norma n = new Norma();
                n.id = norma.NormaID;
                n.NormaID = norma.NormaID;
                n.Nombre = norma.Nombre;
                n.Vigente = norma.Activo;
                n.Descripcion = norma.Descripcion;


                respuesta.Norma = n;
                if (norma.RequiereMensual)
                {
                    respuesta.AnalisisMensual = aBL.Obten_AnalisisMensual(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion);
                }
                if (AnalisisEvaluar.Count == 0)
                {
                    respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                }
                else if (AnalisisEvaluar.Count < 2 && norma.RequiereMensual)
                {
                    respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.Incompleto;
                }
                else if (AnalisisEvaluar.Count >= 2)
                {
                    respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.Completo;
                }

                //Se obtiene los resultados de los parametros de Fuente de Abastecimiento mensual
                List<BL_WS_SIRALAB.ParametroAnalizado> lMensualPar = new List<BL_WS_SIRALAB.ParametroAnalizado>();
                int monthFuente = 0;
                switch (FechaFin.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        {
                            monthFuente = 3;
                        }
                        break;
                    case 4:
                    case 5:
                    case 6:
                        {
                            monthFuente = 6;
                        }
                        break;
                    case 7:
                    case 8:
                    case 9:
                        {
                            monthFuente = 9;
                        }
                        break;
                    case 10:
                    case 11:
                    case 12:
                        {
                            monthFuente = 12;
                        }
                        break;
                }



                lMensualPar = aBL.Obten_AnalisisMensual_fuente(ClienteID, MedidorID, monthFuente, FechaFin.Year);

                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                var analisisList = lAnalisis.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();
                bool RestaFuenteAplicada = false;
                DateTime FechaMaximaAplicada = new DateTime();
                bool BanderaPrimeraVezResta = true;

                //Se obtiene los resultados de los parametros de Fuente de Abastecimiento mensual
                foreach (Analisis AnalisisEncontrado in analisisList)
                {
                    foreach (BL_WS_SIRALAB.Muestra MuestraEncontrado in AnalisisEncontrado.muestras)
                    {
                        if (MuestraEncontrado.nivel == enumNivel.Compuesta)
                        {
                            foreach (ParametroAnalizado ParametroResta in MuestraEncontrado.Resultados)
                            {
                                //Localizamos parametro en lista de fuente
                                decimal? ValorFuente = 0;
                                decimal? ValorEvaluado = 0;
                                ParametroAnalizado parametroOfFuente = null;
                                bool Validacion_Conformacion = false;
                                parametroOfFuente = lMensualPar.Where(p => p.parametro.ParametroID == ParametroResta.parametro.ParametroID).FirstOrDefault();
                                if (parametroOfFuente != null)
                                {
                                    ValorFuente = parametroOfFuente.valor != null ? (decimal)parametroOfFuente.valor : 0;
                                    switch (MuestraEncontrado.fecha.Value.Month)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 3)
                                                {
                                                    Validacion_Conformacion = true;
                                                }

                                            }
                                            break;
                                        case 4:
                                        case 5:
                                        case 6:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 6)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                        case 7:
                                        case 8:
                                        case 9:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 9)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 12)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                    }
                                }

                                if (parametroOfFuente != null && Validacion_Conformacion)
                                {
                                    ValorEvaluado = ParametroResta.valor - ValorFuente;
                                    if (ValorEvaluado < 0)
                                    {
                                        ValorEvaluado = 0;
                                    }
                                    ParametroResta.Diferencia = ValorEvaluado;
                                    ParametroResta.ValorFuente = ValorFuente;
                                    if (BanderaPrimeraVezResta)
                                    {
                                        FechaMaximaAplicada = parametroOfFuente.FechaConformacion;
                                        BanderaPrimeraVezResta = false;
                                    }
                                    else
                                    {
                                        if (FechaMaximaAplicada < parametroOfFuente.FechaConformacion)
                                        {
                                            FechaMaximaAplicada = parametroOfFuente.FechaConformacion;
                                        }
                                    }
                                    RestaFuenteAplicada = true;
                                    ParametroResta.AplicaFechaConformacion = true;
                                    ParametroResta.valor_validacion = (bool)AnalisisDalExtend.ValidoParametro(ParametroResta, (decimal)ValorEvaluado, Evaluacion);
                                }
                                else
                                {
                                    ParametroResta.Diferencia = ParametroResta.valor;
                                    ParametroResta.ValorFuente = 0;
                                    ParametroResta.AplicaFechaConformacion = false;
                                }
                            }
                        }
                    }
                }

                List<int> AnalisisFuente = new List<int>();
                List<int> Fuente = new List<int>();
                Fuente = aBL.Obten_AnalisisMensual_fuenteListadoFecha(ClienteID, MedidorID, FechaMaximaAplicada, monthFuente, FechaFin.Year);
                if (Fuente.Count >= 2 && RestaFuenteAplicada)
                {
                    respuesta.Fuente_Abastecimiento = true;
                    foreach (int FuenteTemp in Fuente)
                    {
                        AnalisisFuente.Add(FuenteTemp);
                    }
                }
                else
                {
                    //Si llega aqui es que la fuente no se aplico pero muestra los analisis 
                    foreach (int FuenteTemp in Fuente)
                    {
                        AnalisisFuente.Add(FuenteTemp);
                    }
                }
                //Fuentes de abastecimiento                
                respuesta.Analisis = AnalisisFuente;
                respuesta.AnalisisEvaluacionDeclaragua = analisisList;
                //Se realiza la validacion disminuyendo los resultados del mensual de fuente de abastecimiento                
                respuesta.Valido = Validacion_Fuente_listado(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion, ref respuestaPD, MedidorID);
                respuesta.TodosPDValidos = respuestaPD;

                //Banderas para mensajes en Declaragua
                respuesta.TieneReincidencia = TieneReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.TieneReincidenciaPreventiva = TieneReincidenciaPreventiva(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.TieneApercibimiento = TieneApercibimiento(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                respuesta.TieneReincidenciaConImpugnacion = TieneReincidenciaImpugnada(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);

            }
            catch (Exception e)
            {
                respuesta.Fuente_Abastecimiento = false;
                respuesta.TodosPDValidos = false;
                respuesta.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                respuesta.Valido = false;
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += " " + e.InnerException.Message;
                }
                respuesta.error = true;
                respuesta.exception = new SiralabFaultException(message);
            }
            return respuesta;
        }

        public List<PeriodResult> Obten_Informacion_Fuente(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, List<int> MedidorID)
        {
            errorMuestra = string.Empty;
            Analisis_BL aBL = new Analisis_BL();
            Norma_BL nBL = new Norma_BL();
            Cliente_BL cBL = new Cliente_BL();
            Anexo_BL axBL = new Anexo_BL();
            string mensaje = "";
            var t1 = DateTime.Now;
            var NombreNorma = string.Empty;
            try
            {
                List<PeriodResult> lPR = new List<PeriodResult>();
                PeriodResult pr = new PeriodResult();
                pr.EstaInscritoNom001 = EstaInscritoNom001(Anexo, Titulo, ClienteID, FechaInicio);
                if (NormaID == 3)
                {
                    if (FechaInicio.Year >= int.Parse(ConfigurationManager.AppSettings["ValidaProgramaSSTDQOCOT"]))
                    {
                        Norma2021_BL nom2021 = new Norma2021_BL();
                        NormaID = nom2021.ObtenerNormaIDByNombreSSTDQOCOT("SST, DQO / COT");
                        var nom = nom2021.ObtenerNormaByID(NormaID);
                        if (nom != null)
                        {
                            NombreNorma = nom.Nombre;
                        }
                    }
                }
                else
                {
                    if (int.Parse(ConfigurationManager.AppSettings["ValidaPrograma"]) <= FechaInicio.Year)
                    {
                        if (pr.EstaInscritoNom001 == false)
                        {
                            if (NormaID == 1)
                            {
                                Norma2021_BL nom2021 = new Norma2021_BL();
                                NormaID = nom2021.ObtenerNormaIDByNombre("NOM-001-SEMARNAT-2021");
                            }

                        }
                    }
                }
                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                var norma = nBL.Obten_Norma(NormaID);



                pr.Fuente_Abastecimiento = false;
                bool RestaFuenteAplicada = false;

                List<Analisis> listadoAnalisis = new List<Analisis>();
                pr.analisis = new List<Analisis>();

                var analisisList = lAnalisis.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();

                foreach (var analisisItem in analisisList)
                {
                    listadoAnalisis.Add(analisisItem);
                }

                if (lAnalisis.Count() == 0)
                {
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                }
                else if (lAnalisis.Count() < 2 && norma.RequiereMensual)
                {
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.Incompleto;
                }
                else if (lAnalisis.Count() >= 2)
                {
                    pr.ValidacionNumPromedios = enumEvaluacionPromedios.Completo;
                }

                #region Mensual
                /// 
                /// Agregar el análisis de la mensual si es requerido
                ///               

                //Se obtiene los resultados de los parametros de Fuente de Abastecimiento mensual
                List<BL_WS_SIRALAB.ParametroAnalizado> lMensualPar = new List<BL_WS_SIRALAB.ParametroAnalizado>();
                int monthFuente = 0;
                switch (FechaFin.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        {
                            monthFuente = 3;
                        }
                        break;
                    case 4:
                    case 5:
                    case 6:
                        {
                            monthFuente = 6;
                        }
                        break;
                    case 7:
                    case 8:
                    case 9:
                        {
                            monthFuente = 9;
                        }
                        break;
                    case 10:
                    case 11:
                    case 12:
                        {
                            monthFuente = 12;
                        }
                        break;
                }
                lMensualPar = aBL.Obten_AnalisisMensual_fuente(ClienteID, MedidorID, monthFuente, FechaFin.Year);

                List<BL_WS_SIRALAB.Muestra> MuestrasValidarCom = new List<BL_WS_SIRALAB.Muestra>();
                var lAnalisisRecal = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                var analisisListRecal = lAnalisisRecal.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();
                foreach (Analisis AnalisisEncontrado in analisisListRecal)
                {
                    foreach (BL_WS_SIRALAB.Muestra MuestraEncontrado in AnalisisEncontrado.muestras)
                    {
                        if (MuestraEncontrado.nivel == enumNivel.Compuesta)
                        {
                            MuestrasValidarCom.Add(MuestraEncontrado);
                            foreach (ParametroAnalizado ParametroResta in MuestraEncontrado.Resultados)
                            {
                                //Localizamos parametro en lista de fuente
                                decimal? ValorFuente = 0;
                                decimal? ValorEvaluado = 0;
                                ParametroAnalizado parametroOfFuente = null;
                                bool Validacion_Conformacion = false;
                                parametroOfFuente = lMensualPar.Where(p => p.parametro.ParametroID == ParametroResta.parametro.ParametroID).FirstOrDefault();
                                if (parametroOfFuente != null)
                                {
                                    ValorFuente = parametroOfFuente.valor != null ? (decimal)parametroOfFuente.valor : 0;
                                    switch (MuestraEncontrado.fecha.Value.Month)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 3)
                                                {
                                                    Validacion_Conformacion = true;
                                                }

                                            }
                                            break;
                                        case 4:
                                        case 5:
                                        case 6:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 6)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                        case 7:
                                        case 8:
                                        case 9:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 9)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 12)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                    }
                                }

                                if (parametroOfFuente != null && Validacion_Conformacion)
                                {
                                    ValorEvaluado = ParametroResta.valor - ValorFuente;
                                    ParametroResta.Diferencia = ValorEvaluado;
                                    ParametroResta.ValorFuente = ValorFuente;
                                    ParametroResta.valor = ValorEvaluado;
                                    ParametroResta.AplicaFechaConformacion = true;
                                    ParametroResta.valor_validacion = (bool)AnalisisDalExtend.ValidoParametro(ParametroResta, (decimal)ValorEvaluado, Evaluacion);
                                }
                                else
                                {
                                    ParametroResta.Diferencia = ParametroResta.valor;
                                    ParametroResta.ValorFuente = 0;
                                    ParametroResta.AplicaFechaConformacion = false;
                                }
                            }
                        }
                    }
                }

                //Se obtiene los resultados de los parametros de Fuente de Abastecimiento mensual
                foreach (Analisis AnalisisEncontrado in analisisList)
                {
                    foreach (BL_WS_SIRALAB.Muestra MuestraEncontrado in AnalisisEncontrado.muestras)
                    {
                        if (MuestraEncontrado.nivel == enumNivel.Compuesta)
                        {
                            foreach (ParametroAnalizado ParametroResta in MuestraEncontrado.Resultados)
                            {
                                //Localizamos parametro en lista de fuente
                                decimal? ValorFuente = 0;
                                decimal? ValorEvaluado = 0;
                                ParametroAnalizado parametroOfFuente = null;
                                bool Validacion_Conformacion = false;
                                parametroOfFuente = lMensualPar.Where(p => p.parametro.ParametroID == ParametroResta.parametro.ParametroID).FirstOrDefault();
                                if (parametroOfFuente != null)
                                {
                                    ValorFuente = parametroOfFuente.valor != null ? (decimal)parametroOfFuente.valor : 0;
                                    switch (MuestraEncontrado.fecha.Value.Month)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 3)
                                                {
                                                    Validacion_Conformacion = true;
                                                }

                                            }
                                            break;
                                        case 4:
                                        case 5:
                                        case 6:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 6)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                        case 7:
                                        case 8:
                                        case 9:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 9)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                            {
                                                if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 12)
                                                {
                                                    Validacion_Conformacion = true;
                                                }
                                            }
                                            break;
                                    }
                                }

                                if (parametroOfFuente != null && Validacion_Conformacion)
                                {
                                    ValorEvaluado = ParametroResta.valor - ValorFuente;
                                    if (ValorEvaluado < 0)
                                    { ValorEvaluado = 0; }
                                    ParametroResta.Diferencia = ValorEvaluado;
                                    ParametroResta.ValorFuente = ValorFuente;
                                    RestaFuenteAplicada = true;
                                    ParametroResta.AplicaFechaConformacion = true;
                                    ParametroResta.valor_validacion = (bool)AnalisisDalExtend.ValidoParametro(ParametroResta, (decimal)ValorEvaluado, Evaluacion);
                                }
                                else
                                {
                                    ParametroResta.Diferencia = ParametroResta.valor;
                                    ParametroResta.ValorFuente = 0;
                                    ParametroResta.AplicaFechaConformacion = false;
                                }
                            }
                        }
                    }
                }

                //aqui mandar el mensual menos la fuente de abastecimiento
                List<Analisis> lMensual = new List<Analisis>();
                if (norma.RequiereMensual)
                    lMensual = aBL.Obten_AnalisisMensual_fuente_Completo(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion, MedidorID, MuestrasValidarCom, norma);

                foreach (var mensualItem in lMensual)
                {
                    listadoAnalisis.Add(mensualItem);
                }
                #endregion

                //Si requiere por medidor obtener los numero de PuckanexoID apartir del numero de medidor.                
                List<int> Fuente = new List<int>();
                Fuente = aBL.Obten_AnalisisMensual_fuenteListado(ClienteID, MedidorID, monthFuente, FechaFin.Year);
                if (Fuente.Count >= 2 && RestaFuenteAplicada)
                {
                    pr.Fuente_Abastecimiento = true;
                }

                #region Validacion_Analisis_Parametros
                pr.analisis = listadoAnalisis;
                bool RespuesPDValidos = false;

                //Validacion_Fuente_listado
                pr.valida = Validacion_Fuente_listado(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID, Evaluacion, ref RespuesPDValidos, MedidorID);
                pr.TodosPDValidos = RespuesPDValidos;

                pr.TieneReincidencia = TieneReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                pr.TieneReincidenciaPreventiva = TieneReincidenciaPreventiva(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                pr.TieneApercibimiento = TieneApercibimiento(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
                pr.TieneReincidenciaConImpugnacion = TieneReincidenciaImpugnada(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);

                #endregion

                /*Agrega el error de la muestra*/
                if (errorMuestra != String.Empty)
                {
                    mensaje += errorMuestra;
                }

                #region Cliente
                var pr_cliente = cBL.Obten_Cliente(ClienteID);
                if (pr_cliente == null)
                {
                    pr.error = true;
                    mensaje += "\nCliente no registrado.";
                }
                else
                {
                    pr.cliente = pr_cliente.ConvertCliente();
                }
                #endregion

                #region Consecion y Anexo
                var pr_anexo = axBL.Obten_Anexo(Titulo, Anexo, ClienteID);
                if (pr_anexo == null)
                {
                    pr.error = true;
                    mensaje += "\nTítulo o anexo no registrado.";
                }
                else
                {
                    pr.anexo = pr_anexo.ConvertAnexo();
                    pr.consecion = pr.anexo.Titulo;
                }
                pr.norma = norma.ConvertNorma();
                #endregion

                #region Laboratorio aprobacion
                /*RTG 2015 06 26*/
                vw_LaboratoriosPALA_BL BL_vw = new vw_LaboratoriosPALA_BL();
                foreach (Analisis analisis in listadoAnalisis)
                {
                    vw_LaboratoriosPALA pala = BL_vw.GetDatos(analisis.laboratorio.rfc);
                    if (pala != null)
                    {
                        analisis.laboratorio.Aprobacion = pala.Aprobacion;
                        analisis.laboratorio.AcreditacionEMA = pala.AcreditacionEMA;
                    }
                }
                /*end*/
                #endregion
                lPR.Add(pr);
                if (pr.error)
                {
                    pr.exception = new SiralabFaultException(mensaje);
                }

                //Nueva funcionalidad para norma nueva 
                if (NombreNorma != string.Empty && (NombreNorma.Replace(" ", "") == norma.Nombre.Replace(" ", "")))
                {
                    var NormaDQOCOT = string.Empty;
                    foreach (var item in lPR)
                    {
                        if (item.analisis.Count > 0)
                        {
                            NormaDQOCOT = item.analisis.First().NormaDQOCOT;
                            foreach (var an in item.analisis)
                            {
                                if (NormaDQOCOT != null && NormaDQOCOT.Contains("COT"))
                                {
                                    foreach (var m in an.muestras)
                                    {
                                        if (m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {

                                                if (res.parametro.nombre.Contains("DQO") || res.parametro.nombre.Contains("Demanda Química de Oxigeno"))
                                                {
                                                    m.Resultados.RemoveAll(x => x.parametro.nombre.Contains("DQO") || x.parametro.nombre.Contains("Demanda Química de Oxigeno"));
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }
                                else if (NormaDQOCOT != null && NormaDQOCOT.Contains("DQO"))
                                {
                                    foreach (var m in an.muestras)
                                    {
                                        if (m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.nombre.Contains("COT") || res.parametro.nombre.Contains("Carbono Orgánico Total"))
                                                {
                                                    m.Resultados.RemoveAll(x => x.parametro.nombre.Contains("COT") || x.parametro.nombre.Contains("Carbono Orgánico Total"));
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                }

                return lPR;
            }
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += " " + e.InnerException.Message;
                }
                List<PeriodResult> lPR = new List<PeriodResult>();
                PeriodResult pr = new PeriodResult();
                pr.Fuente_Abastecimiento = false;
                pr.TodosPDValidos = false;
                pr.ValidacionNumPromedios = enumEvaluacionPromedios.NO_PD;
                pr.valida = false;
                pr.error = true;
                pr.exception = new SiralabFaultException(e.Message);
                lPR.Add(pr);
                return lPR;
            }
        }
        #endregion

        #region Metodos_Secundarios_Fuente_Abastecimiento
        public bool Validacion_Fuente_listado(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, ref bool PDValido, List<int> Medidor_ID)
        {
            try
            {
                bool ValidoSimple = false;
                bool ValidoCompuesta = false;
                bool ValidoMensual = false;
                bool Valido = false;

                Analisis_BL aBL = new Analisis_BL();
                var lAnalisis = aBL.Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);

                //Se obtiene los resultados de los parametros de Fuente de Abastecimiento mensual
                List<BL_WS_SIRALAB.ParametroAnalizado> lMensualPar = new List<BL_WS_SIRALAB.ParametroAnalizado>();
                int monthFuente = 0;
                switch (FechaFin.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        {
                            monthFuente = 3;
                        }
                        break;
                    case 4:
                    case 5:
                    case 6:
                        {
                            monthFuente = 6;
                        }
                        break;
                    case 7:
                    case 8:
                    case 9:
                        {
                            monthFuente = 9;
                        }
                        break;
                    case 10:
                    case 11:
                    case 12:
                        {
                            monthFuente = 12;
                        }
                        break;
                }
                lMensualPar = aBL.Obten_AnalisisMensual_fuente(ClienteID, Medidor_ID, monthFuente, FechaFin.Year);
                //Se obtiene los resultados de los parametros de Fuente de Abastecimiento mensual

                List<DL_WS_SIRALAB.Muestra> MuestrasCompuestas = new List<DL_WS_SIRALAB.Muestra>();

                if (lAnalisis.Count > 0)
                {
                    /// Ahora validaremos por muestra los valores capturados por los laboratorios
                    /// 
                    foreach (var Analisis in lAnalisis)
                    {
                        foreach (var Muestra in Analisis.Muestras)
                        {
                            /// Ya con la muestra en memoria procedemos a generar las validaciones por parámetro
                            ///
                            var Nivel = Muestra.NivelID.ToEnumNivel();
                            switch (Nivel)
                            {
                                case enumNivel.Simple:
                                    NormaConfiguracion_BL Configuracion = new NormaConfiguracion_BL();
                                    List<NormaConfiguracion> ConfiguracionLocalizada = Configuracion.Obten_Configuracion(NormaID, (int)Analisis.OrdenTrabajo.CuerpoReceptorID, (int)Analisis.OrdenTrabajo.UsoAguaID, enumNivel.Simple);

                                    ValidoSimple = (!Muestra.Cancelado) ? MuestraValida(Muestra, Evaluacion, ConfiguracionLocalizada) : true;
                                    if (!ValidoSimple)
                                    {
                                        errorMuestra = "\nMuestra simple no valida";
                                        PDValido = false;
                                        return false;
                                    }
                                    break;
                                case enumNivel.Compuesta:
                                    NormaConfiguracion_BL ConfiguracionCompuesta = new NormaConfiguracion_BL();
                                    List<NormaConfiguracion> ConfiguracionLocalizadaComp = ConfiguracionCompuesta.Obten_Configuracion(NormaID, (int)Analisis.OrdenTrabajo.CuerpoReceptorID, (int)Analisis.OrdenTrabajo.UsoAguaID, enumNivel.Compuesta);

                                    DL_WS_SIRALAB.Muestra MuestraConFuente = new DL_WS_SIRALAB.Muestra();
                                    // se deben de validar contra los parametros de fuente de abastecimiento y fecha de conformacion                                    
                                    ValidoCompuesta = (!Muestra.Cancelado) ? MuestraValida_Compuesta(Muestra, Evaluacion, lMensualPar, ref MuestraConFuente, ConfiguracionLocalizadaComp) : true;
                                    MuestrasCompuestas.Add(MuestraConFuente);
                                    if (!ValidoCompuesta)
                                    {
                                        errorMuestra = "\nMuestra compuesta no valida";
                                        PDValido = false;
                                        return false;
                                    }
                                    break;
                            }
                        }
                    }
                }
                Norma_BL nBL = new Norma_BL();
                var norma = nBL.Obten_Norma(NormaID);

                if (norma.RequiereMensual)
                {
                    ValidoMensual = MensualValida_fuente_Compuestas(MuestrasCompuestas, Evaluacion, norma);
                }
                else
                    ValidoMensual = true;

                if (ValidoSimple && ValidoCompuesta)
                {
                    PDValido = true;
                }

                if (!ValidoSimple || !ValidoCompuesta || !ValidoMensual)
                    return false;

                Valido = ValidoSimple && ValidoCompuesta && ValidoMensual;

                return Valido;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Se crea para validar las muestras compuestas mensualmente
        private bool MensualValida_fuente_Compuestas(List<DL_WS_SIRALAB.Muestra> muestras, enumTipoEvaluacion Evaluacion, DL_WS_SIRALAB.Norma Norma)
        {
            bool valida = true;
            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;
            Analisis_BL aBL = new Analisis_BL();
            List<ParametroAnalizado> ParametrosMensualesAnalizar = new List<ParametroAnalizado>();
            ParametrosMensualesAnalizar = aBL.Obten_AnalisisMensual_fuenteCompuestaFecha(muestras, Norma);
            valida = MuestraValida_fuente_compuesta(ParametrosMensualesAnalizar, Evaluacion);
            return valida;
        }

        private bool MuestraValida_fuente_compuesta(List<ParametroAnalizado> ParametrosMAnalizar, enumTipoEvaluacion Evaluacion)
        {
            bool valida = false;
            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;

            foreach (var Parametro in ParametrosMAnalizar)
            {
                if (Parametro.evalua != null ? (bool)Parametro.evalua : false)
                {
                    if (Parametro.acepta_valor != null ? (bool)Parametro.acepta_valor : false)
                    {
                        /// Primero buscamos sí se requiere evaluar por medio de LFD
                        /// 
                        switch (Evaluacion)
                        {
                            case enumTipoEvaluacion.Evaluar_Simple:
                                #region Evaluación respecto a los límites de norma
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (Parametro.limite_inferior != null)
                                {
                                    valida = Parametro.limite_inferior <= Parametro.valor && Parametro.valor <= Parametro.limite_superior;
                                }
                                else
                                {
                                    valida = Parametro.valor <= Parametro.limite_superior;
                                }
                                #endregion
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD:
                                if (Parametro.ley_federal != null ? (bool)Parametro.ley_federal : false)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    //Verificamos que tenga límite inferior para saber si es un rango
                                    if (Parametro.limite_inferior_LFD != null)
                                    {
                                        valida = Parametro.limite_inferior_LFD <= Parametro.valor && Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    else
                                    {
                                        valida = Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    valida = true;
                                }
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD_Simple:
                                if (Parametro.ley_federal != null ? (bool)Parametro.ley_federal : false)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.limite_inferior_LFD != null)
                                    {
                                        valida = Parametro.limite_inferior_LFD <= Parametro.valor && Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    else
                                    {
                                        valida = Parametro.valor <= Parametro.limite_superior_LFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Evaluación respecto a los límites de norma
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.limite_inferior != null)
                                    {
                                        valida = Parametro.limite_inferior <= Parametro.valor && Parametro.valor <= Parametro.limite_superior;
                                    }
                                    else
                                    {
                                        valida = Parametro.valor <= Parametro.limite_superior;
                                    }
                                    #endregion
                                }
                                break;
                            default:
                                valida = true;
                                break;
                        }
                    }
                    else
                    {
                        valida = (bool)Parametro.valor_validacion;
                    }
                }
                else
                {
                    valida = true;
                }

                if (!valida)
                    break;
            }
            return valida;
        }

        //se crea para validar los PD menos la fuente de abastecimiento 
        private bool MuestraValida_Compuesta(DL_WS_SIRALAB.Muestra Muestra, enumTipoEvaluacion Evaluacion, List<BL_WS_SIRALAB.ParametroAnalizado> Parametros_Fuente, ref DL_WS_SIRALAB.Muestra MuestraConFuente, List<NormaConfiguracion> Configuracion)
        {
            bool valida = false;
            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;
            var FechaLimiteParametrosNomOO1 = Convert.ToDateTime(ConfigurationManager.AppSettings["FechaLimiteParametrosNomOO1"]);
            foreach (var Parametro in Muestra.MuestraValors)
            {
                // Reemplaza la condición original por esta versión null-safe y más legible
                if ((Parametro.ParametroID == 931 && Muestra.Fecha.HasValue && Muestra.Fecha.Value <= FechaLimiteParametrosNomOO1)
                    || ((Parametro.ParametroID == 1072 || Parametro.ParametroID == 1073 || Parametro.ParametroID == 1074)
                        && Muestra.FechaConformacion.HasValue && Muestra.FechaConformacion.Value <= FechaLimiteParametrosNomOO1))
                {
                    //valida = true;
                    continue;
                }
                if (Parametro.Grupo && Parametro.Evalua && !Parametro.NoAnalizado && !Parametro.Exento && Parametro.LFD)
                {
                    NormaConfiguracion ConfiguracionPadre = Configuracion.Where(Z => Z.ParametroID == Parametro.ParametroID).FirstOrDefault();

                    foreach (var ParametroHijo in Muestra.NivelConfiguracion.NormaConfiguracions.Where(h => h.NormaConfiguracionPadreID == ConfiguracionPadre.NormaConfiguracionID))
                    {
                        if (Muestra.MuestraValors.Where(z => z.ParametroID == ParametroHijo.ParametroID).FirstOrDefault().NoAnalizado)
                        {
                            valida = false;
                            break;
                        }
                    }

                    if (valida)
                    {
                        if (Parametro.AceptaValor)
                        {
                            /// Primero buscamos sí se requiere evaluar por medio de LFD
                            /// 
                            switch (Evaluacion)
                            {
                                case enumTipoEvaluacion.Evaluar_Simple:
                                    #region Evaluación respecto a los límites de norma
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.LimiteInferior != null)
                                    {
                                        valida = Parametro.LimiteInferior <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperior;
                                    }
                                    else
                                    {
                                        valida = Parametro.Valor <= Parametro.LimiteSuperior;
                                    }
                                    #endregion
                                    break;
                                case enumTipoEvaluacion.Evaluar_LFD:
                                    if (Parametro.LFD)
                                    {
                                        #region Evaluación respecto a los límites de LFD
                                        //Localizamos parametro en lista de fuente
                                        decimal? ValorFuente = 0;
                                        decimal? ValorEvaluado = 0;
                                        ParametroAnalizado parametroOfFuente = null;
                                        parametroOfFuente = Parametros_Fuente.Where(p => p.parametro.ParametroID == Parametro.Parametro.ParametroID).FirstOrDefault();
                                        bool Validacion_Conformacion = false;
                                        if (parametroOfFuente != null)
                                        {
                                            ValorFuente = parametroOfFuente.valor != null ? (decimal)parametroOfFuente.valor : 0;
                                            switch (Muestra.Fecha.Value.Month)
                                            {
                                                case 1:
                                                case 2:
                                                case 3:
                                                    {
                                                        if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 3)
                                                        {
                                                            Validacion_Conformacion = true;
                                                        }

                                                    }
                                                    break;
                                                case 4:
                                                case 5:
                                                case 6:
                                                    {
                                                        if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 6)
                                                        {
                                                            Validacion_Conformacion = true;
                                                        }
                                                    }
                                                    break;
                                                case 7:
                                                case 8:
                                                case 9:
                                                    {
                                                        if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 9)
                                                        {
                                                            Validacion_Conformacion = true;
                                                        }
                                                    }
                                                    break;
                                                case 10:
                                                case 11:
                                                case 12:
                                                    {
                                                        if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 12)
                                                        {
                                                            Validacion_Conformacion = true;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }

                                        if (parametroOfFuente != null && Validacion_Conformacion)
                                        {
                                            ValorEvaluado = Parametro.Valor - ValorFuente;
                                        }
                                        else
                                        {
                                            ValorEvaluado = Parametro.Valor;
                                        }

                                        Parametro.Valor = ValorEvaluado;
                                        ///Verificamos que tenga límite inferior para saber si es un rango
                                        ///
                                        if (Parametro.LimiteInferiorLFD != null)
                                        {
                                            valida = Parametro.LimiteInferiorLFD <= ValorEvaluado && ValorEvaluado <= Parametro.LimiteSuperiorLFD;
                                        }
                                        else
                                        {
                                            valida = ValorEvaluado <= Parametro.LimiteSuperiorLFD;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        valida = true;
                                    }
                                    break;
                                case enumTipoEvaluacion.Evaluar_LFD_Simple:
                                    if (Parametro.LFD)
                                    {
                                        #region Evaluación respecto a los límites de LFD
                                        ///Verificamos que tenga límite inferior para saber si es un rango
                                        ///
                                        if (Parametro.LimiteInferiorLFD != null)
                                        {
                                            valida = Parametro.LimiteInferiorLFD <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                        }
                                        else
                                        {
                                            valida = Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Evaluación respecto a los límites de norma
                                        ///Verificamos que tenga límite inferior para saber si es un rango
                                        ///
                                        if (Parametro.LimiteInferior != null)
                                        {
                                            valida = Parametro.LimiteInferior <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperior;
                                        }
                                        else
                                        {
                                            valida = Parametro.Valor <= Parametro.LimiteSuperior;
                                        }
                                        #endregion
                                    }
                                    break;
                                default:
                                    valida = true;
                                    break;
                            }
                        }
                        else
                        {
                            valida = (bool)Parametro.ValorValidacion;
                        }
                    }
                }
                else if (!Parametro.NoAnalizado && !Parametro.Exento && Parametro.Evalua)
                {
                    if (Parametro.AceptaValor)
                    {
                        /// Primero buscamos sí se requiere evaluar por medio de LFD
                        /// 
                        switch (Evaluacion)
                        {
                            case enumTipoEvaluacion.Evaluar_Simple:
                                #region Evaluación respecto a los límites de norma
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (Parametro.LimiteInferior != null)
                                {
                                    valida = Parametro.LimiteInferior <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperior;
                                }
                                else
                                {
                                    valida = Parametro.Valor <= Parametro.LimiteSuperior;
                                }
                                #endregion
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD:
                                if (Parametro.LFD)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    //Localizamos parametro en lista de fuente
                                    decimal? ValorFuente = 0;
                                    decimal? ValorEvaluado = 0;
                                    ParametroAnalizado parametroOfFuente = null;
                                    parametroOfFuente = Parametros_Fuente.Where(p => p.parametro.ParametroID == Parametro.Parametro.ParametroID).FirstOrDefault();
                                    bool Validacion_Conformacion = false;
                                    if (parametroOfFuente != null)
                                    {
                                        ValorFuente = parametroOfFuente.valor != null ? (decimal)parametroOfFuente.valor : 0;
                                        switch (Muestra.Fecha.Value.Month)
                                        {
                                            case 1:
                                            case 2:
                                            case 3:
                                                {
                                                    if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 3)
                                                    {
                                                        Validacion_Conformacion = true;
                                                    }

                                                }
                                                break;
                                            case 4:
                                            case 5:
                                            case 6:
                                                {
                                                    if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 6)
                                                    {
                                                        Validacion_Conformacion = true;
                                                    }
                                                }
                                                break;
                                            case 7:
                                            case 8:
                                            case 9:
                                                {
                                                    if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 9)
                                                    {
                                                        Validacion_Conformacion = true;
                                                    }
                                                }
                                                break;
                                            case 10:
                                            case 11:
                                            case 12:
                                                {
                                                    if (parametroOfFuente.FechaConformacion.Month >= 1 && parametroOfFuente.FechaConformacion.Month <= 12)
                                                    {
                                                        Validacion_Conformacion = true;
                                                    }
                                                }
                                                break;
                                        }
                                    }

                                    if (parametroOfFuente != null && Validacion_Conformacion)
                                    {
                                        ValorEvaluado = Parametro.Valor - ValorFuente;
                                    }
                                    else
                                    {
                                        ValorEvaluado = Parametro.Valor;
                                    }

                                    Parametro.Valor = ValorEvaluado;
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.LimiteInferiorLFD != null)
                                    {
                                        valida = Parametro.LimiteInferiorLFD <= ValorEvaluado && ValorEvaluado <= Parametro.LimiteSuperiorLFD;
                                    }
                                    else
                                    {
                                        valida = ValorEvaluado <= Parametro.LimiteSuperiorLFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    valida = true;
                                }
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD_Simple:
                                if (Parametro.LFD)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.LimiteInferiorLFD != null)
                                    {
                                        valida = Parametro.LimiteInferiorLFD <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                    }
                                    else
                                    {
                                        valida = Parametro.Valor <= Parametro.LimiteSuperiorLFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Evaluación respecto a los límites de norma
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (Parametro.LimiteInferior != null)
                                    {
                                        valida = Parametro.LimiteInferior <= Parametro.Valor && Parametro.Valor <= Parametro.LimiteSuperior;
                                    }
                                    else
                                    {
                                        valida = Parametro.Valor <= Parametro.LimiteSuperior;
                                    }
                                    #endregion
                                }
                                break;
                            default:
                                valida = true;
                                break;
                        }
                    }
                    else
                    {
                        valida = (bool)Parametro.ValorValidacion;
                    }
                }
                else if (!Parametro.Evalua)
                {
                    valida = true;
                }
                else if (Parametro.Evalua && Parametro.NoAnalizado && Parametro.LFD)
                {
                    valida = false;
                }
                else if (Parametro.Evalua && Parametro.NoAnalizado && !Parametro.LFD)
                {
                    valida = true;
                }
                else if (Parametro.Exento)
                {
                    valida = true;
                }
                if (!valida)
                    break;
            }
            MuestraConFuente = Muestra;
            return valida;
        }
        #endregion
        #endregion


        #region ApercibidosYReincidentes
        public bool TieneReincidencia(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            Analisis_BL aBL = new Analisis_BL();
            var lAnalisis = aBL.Obten_AnalisisConReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            ApercibidosYReincidentes_BL BLL = new ApercibidosYReincidentes_BL();
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();

            var EstaEnReincidencia = false;
            if (lAnalisis.Count > 0)
            {
                foreach (var item in lAnalisis)
                {
                    //Valida si hay una reincidencia 
                    EstaEnReincidencia = BLL.BuscaLaboratorioEnReincidenciaByLaboratorioID(item.LaboratorioID);
                    if (EstaEnReincidencia)
                    {
                        var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                        diasHabiles = diasHabiles + 1;
                        var Reincidencia = BLL.BuscaReincidenciaByLaboratorioID(item.LaboratorioID);
                        var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                        if (item.Muestras.Select(x => x.Fecha).First().Value >= fechaLimite)
                        {
                            EstaEnReincidencia = true;
                            return EstaEnReincidencia;
                        }
                        else
                        {
                            EstaEnReincidencia = false;
                        }
                    }
                }

            }

            return EstaEnReincidencia;

        }


        public bool TieneReincidenciaPreventiva(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            Analisis_BL aBL = new Analisis_BL();
            var lAnalisis = aBL.Obten_AnalisisConReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            ApercibidosYReincidentes_BL BLL = new ApercibidosYReincidentes_BL();
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();

            var EstaEnReincidencia = false;
            if (lAnalisis.Count > 0)
            {
                foreach (var item in lAnalisis)
                {
                    //Valida si hay una reincidencia 
                    EstaEnReincidencia = BLL.EstaEnReincidenciaPreventivaByLaboratorioID(item.LaboratorioID);
                    if (EstaEnReincidencia)
                    {
                        var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                        diasHabiles = diasHabiles + 1;
                        var Reincidencia = BLL.BuscaReincidenciaByLaboratorioID(item.LaboratorioID);
                        var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                        var fechaNotificacion = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, 1);
                        if (item.Muestras.Select(x => x.Fecha).First().Value >= fechaNotificacion && item.Muestras.Select(x => x.Fecha).First().Value < fechaLimite)
                        {
                            EstaEnReincidencia = true;
                            return EstaEnReincidencia;
                        }
                        else
                        {
                            EstaEnReincidencia = false;
                        }
                    }
                }

            }

            return EstaEnReincidencia;

        }

        public bool TieneReincidenciaImpugnada(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            Analisis_BL aBL = new Analisis_BL();
            var lAnalisis = aBL.Obten_AnalisisConReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            ApercibidosYReincidentes_BL BLL = new ApercibidosYReincidentes_BL();
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();

            var EstaEnReincidencia = false;
            if (lAnalisis.Count > 0)
            {
                foreach (var item in lAnalisis)
                {
                    EstaEnReincidencia = BLL.BuscaLaboratorioEnReincidenciaImpugnadaByLaboratorioID(item.LaboratorioID);

                    if (EstaEnReincidencia)
                    {
                        var ReincidenciaImpugnada = BLL.BuscaReincidenciaImpugnadaByLaboratorioID(item.LaboratorioID);
                        var Reincidencia = BLL.BuscarReincidenciaByID(ReincidenciaImpugnada.IdReincidencia.Value);
                        var fechaNotificacion = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, 1);
                        //var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, 30);
                        if (item.Muestras.Select(x => x.Fecha).First().Value >= fechaNotificacion || item.Muestras.Select(x => x.Fecha).First().Value >= ReincidenciaImpugnada.FechaImpugnacion)
                        {
                            EstaEnReincidencia = true;
                            return EstaEnReincidencia;
                        }
                        else
                        {
                            EstaEnReincidencia = false;
                        }
                    }
                }

            }
            return EstaEnReincidencia;
        }

        public bool TieneApercibimiento(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            Analisis_BL aBL = new Analisis_BL();
            var lAnalisis = aBL.Obten_AnalisisConReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            ApercibidosYReincidentes_BL BLL = new ApercibidosYReincidentes_BL();
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();

            var TieneApercibimiento = false;
            if (lAnalisis.Count > 0)
            {
                foreach (var item in lAnalisis)
                {
                    TieneApercibimiento = BLL.BuscaLaboratorioConApercibimientoByLaboratorioID(item.LaboratorioID);

                    if (TieneApercibimiento)
                    {
                        var apercibimiento = BLL.ApercibimientoByLaboratorioID(item.LaboratorioID);
                        var fechaNotificacion = diaBLL.SiguienteDiaHabil(apercibimiento.Fecha_Notificacion.Value, 1);
                        if (item.Muestras.Select(x => x.Fecha).First().Value >= fechaNotificacion)//validar si es con la fecha de notificacion resolucion o captura
                        {
                            TieneApercibimiento = true;
                            return TieneApercibimiento;
                        }
                        else
                        {
                            TieneApercibimiento = false;
                        }

                    }
                }

            }

            return TieneApercibimiento;

        }

        public bool TieneApercibimientoImpugnado(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            Analisis_BL aBL = new Analisis_BL();
            var lAnalisis = aBL.Obten_AnalisisConReincidencia(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            ApercibidosYReincidentes_BL BLL = new ApercibidosYReincidentes_BL();


            var TieneApercibimiento = false;
            if (lAnalisis.Count > 0)
            {
                foreach (var item in lAnalisis)
                {
                    TieneApercibimiento = BLL.BuscaLaboratorioConApercibimientoImpugnadoByLaboratorioID(item.LaboratorioID);

                    if (TieneApercibimiento)
                    {
                        var apercibimiento = BLL.ApercibimientoImpugnadoByLaboratorioID(item.LaboratorioID);
                        if (item.Muestras.Select(x => x.Fecha).First().Value >= apercibimiento.FechaImpugnacion)//validar con cual fecha debemos de validar 
                        {
                            TieneApercibimiento = true;
                            return TieneApercibimiento;
                        }
                        else
                        {
                            TieneApercibimiento = false;
                        }
                    }
                }

            }

            return TieneApercibimiento;

        }
        #endregion

        #region Nom2021
        public bool EstaInscritoNom001(string Anexo, string Titulo, int clienteID, DateTime FechaMesDeclaracion)
        {
            Norma2021_BL nom2021 = new Norma2021_BL();
            Analisis_BL aBL = new Analisis_BL();
            Cliente_BL CBL = new Cliente_BL();
            var Cliente = CBL.Obten_Cliente(clienteID);
            var RFCCliente = string.Empty;
            if (Cliente != null)
            {
                RFCCliente = Cliente.RFC;
            }

            var EstaInscritoNom001 = nom2021.ObtenerDatosInscritosNom001(Anexo, Titulo, RFCCliente, FechaMesDeclaracion);

            return EstaInscritoNom001;


        }
        public bool EstaInscritoNom001SinEfectos(string Anexo, string Titulo, int clienteID)
        {
            Norma2021_BL nom2021 = new Norma2021_BL();
            Analisis_BL aBL = new Analisis_BL();
            Cliente_BL CBL = new Cliente_BL();
            var Cliente = CBL.Obten_Cliente(clienteID);
            var RFCCliente = string.Empty;
            if (Cliente != null)
            {
                RFCCliente = Cliente.RFC;
            }


            var EstaInscritoNom001SinEfectos = nom2021.EstaSinEfectos(Anexo, Titulo, RFCCliente);

            return EstaInscritoNom001SinEfectos;


        }
        #endregion
    }
}
