using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using DL_WS_SIRALAB;
using BL_WS_SIRALAB;
using BL_WS_SIRALAB.extend;
using System.Configuration;

namespace BL_WS_SIRALAB.negocio
{
    public class Analisis_BL : IDisposable
    {
        #region Metodos Normas
        private Siralab_Entities se { get; set; }
        public Analisis_BL()
        {
            se = new Siralab_Entities();
            se.Configuration.AutoDetectChangesEnabled = false;
            se.Configuration.LazyLoadingEnabled = true;
            se.Configuration.ProxyCreationEnabled = true;
        }

        public Analisi GetAnalisiByAnalisisId(int analisisId)
        {
            return se.Analisis.FirstOrDefault(a => a.AnalisisID == analisisId);
        }


        public bool Compuesta(int AnalisisID)
        {
            bool respuesta = false;
            var analisis = se.Analisis.Where(p => p.AnalisisID == AnalisisID).SingleOrDefault();
            respuesta = !analisis.OrdenTrabajo.Norma.RequiereCompuesta;

            return respuesta;
        }

        public List<DL_WS_SIRALAB.Analisi> Obten_Analisis(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            int NivelID = enumNivel.Simple.ToInt();
            var lAnalisis = new List<DL_WS_SIRALAB.Analisi>();
            int EstatusID = enumAnalisisEstatus.Activo.ToInt();

            lAnalisis = se.Analisis.Where(p =>
                    p.OrdenTrabajo.ClienteID == ClienteID &&
                    p.OrdenTrabajo.Consecion.Titulo == Titulo &&
                    p.OrdenTrabajo.Anexo.Anexo1 == Anexo &&
                    p.OrdenTrabajo.NormaID == NormaID &&
                    !p.FaltaPago &&
                    (p.Muestras.Where(q => q.NivelID == NivelID && q.Fecha >= FechaInicio && q.Fecha < FechaFin).Count() > 0) &&
                    p.Firmada && !p.Procesado && p.EstatusID == EstatusID
                ).ToList();

            #region ConductividadYCloruros
            foreach (var analisis in lAnalisis)
            {
                var muestraCompuesta = analisis.Muestras.Where(muestra => muestra.NivelID == 2);
                var muestrasSimples = analisis.Muestras.Where(muestra => muestra.NivelID == 1);
                foreach (var item in muestraCompuesta)
                {
                    var listaMuestraValors = item.MuestraValors.ToList();
                    List<MuestraValor> listaNueva = new List<MuestraValor>();
                    foreach (var param in listaMuestraValors)
                    {
                        var parametroActivo = analisis.OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(x => x.ParametroID == param.Parametro.ParametroID && x.NormaID == NormaID && x.UsoAguaID == analisis.OrdenTrabajo.UsoAguaID && x.CuerpoReceptorID == analisis.OrdenTrabajo.CuerpoReceptorID && x.NivelID == 2);
                        if (parametroActivo != null && parametroActivo.Activo == true)
                        {
                            listaNueva.Add(param);
                        }
                       
                    }
                    item.MuestraValors = listaNueva;
                }

                if (NormaID == 15 || NormaID == 16 || NormaID==17)
                {
                    foreach (var muestra in muestraCompuesta)
                    {
                        var TipoDeParametro = analisis.NormaDQOCOT;
                        var Conductividad = analisis.Conductividad;
                        var listaMuestraValors = muestra.MuestraValors.ToList();
                        for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
                        {
                            //Si el tipo de parametro se hizo para cot se remueve DQO y si se hizo para DQO remueve COT RGG
                            if (TipoDeParametro != null)
                            {
                                if (TipoDeParametro == "COT")
                                {
                                    if (listaMuestraValors[i].ParametroID == 667)// el valor es de DQO
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                                else if (TipoDeParametro == "DQO")
                                {
                                    if (listaMuestraValors[i].ParametroID == 553 || listaMuestraValors[i].ParametroID == 602) //Los dos valores indican el id del parametro COT
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                            }
                            if (Conductividad != null && Conductividad != string.Empty)
                            {
                                if (Conductividad == "Escherichia")
                                {
                                    if (listaMuestraValors[i].ParametroID == 689)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }

                                }
                                else if (Conductividad == "Enterococos")
                                {
                                    if (listaMuestraValors[i].ParametroID == 694)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                            }

                        }

                        muestra.MuestraValors = listaMuestraValors;
                    }

                    foreach (var muestra in muestrasSimples)
                    {
                        var Conductividad = analisis.Conductividad;
                        var listaMuestraValors = muestra.MuestraValors.ToList();
                        for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
                        {
                            //Si el tipo de parametro se hizo para Escherichia se remueve Enterococos y si se hizo para Enterococos remueve Escherichia RGG

                            if (Conductividad != null && Conductividad != string.Empty)
                            {
                                if (Conductividad == "Escherichia")
                                {
                                    if (listaMuestraValors[i].ParametroID == 689)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }

                                }
                                else if (Conductividad == "Enterococos")
                                {
                                    if (listaMuestraValors[i].ParametroID == 694)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        muestra.MuestraValors = listaMuestraValors;
                    }
                }
            }
            #endregion

           

            //valida que las fechas esten vigentes 
            ApercibidosYReincidentes_BL BLL = new ApercibidosYReincidentes_BL();
            //Codigo para desabilitar las reincidencias cuando la fecha de aprobacion ya no este vigente
            var listaDeReincidencias = BLL.GetAllReincidencias().Where(x => x.Activo == true);
            foreach (var item in listaDeReincidencias)
            {
                var Reincidencia = BLL.BuscarReincidenciaByID(item.Id);
                var VigenciasAprobacion = BLL.ObtenerNumerosYVigenciasDeAprobacion(Reincidencia.Id_Laboratorio.Value).OrderBy(x => x.FechaRegistro).First();
                if (VigenciasAprobacion.FechaFin != null)
                {

                    if (DateTime.Now > VigenciasAprobacion.FechaFin)
                    {
                        var desactivaReincidencia = BLL.desactivaReincidencia(item.Id);
                        var Impugnacion = BLL.ObtenerImpugnacionByIdReincidencia(item.Id);

                        if (Impugnacion != null)
                        {
                            var desabilitarImpugnacion = BLL.desactivaImpugnacionById(Impugnacion.Id);
                        }
                    }
                }
            }

        
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();
           
            var EstaEnReincidencia = false;
            if (lAnalisis.Count > 0)
            {
                for (int i = lAnalisis.Count - 1; i >= 0; i--)
                {
                    var laboratorioID = lAnalisis[i].LaboratorioID;
                    //Aqui ya valida que la reincidencia corresponda al periodo de los analisis
                    EstaEnReincidencia = BLL.BuscaLaboratorioEnReincidenciaByLaboratorioID(laboratorioID);
                    if (EstaEnReincidencia)
                    {
                        var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                        diasHabiles = diasHabiles + 1;
                        var Reincidencia = BLL.BuscaReincidenciaByLaboratorioID(laboratorioID);
                        var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                        ////valida que este en la fecha limite
                        if (DateTime.Now >= fechaLimite)
                        {
                            if (lAnalisis[i].Muestras.Select(x => x.Fecha).First().Value >= fechaLimite)
                            {
                                lAnalisis.RemoveAt(i);
                            }
                        }

                    }
                }
            }
            return lAnalisis;
        }

        public List<DL_WS_SIRALAB.Analisi> Obten_AnalisisConReincidencia(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID)
        {
            int NivelID = enumNivel.Simple.ToInt();
            var lAnalisis = new List<DL_WS_SIRALAB.Analisi>();
            int EstatusID = enumAnalisisEstatus.Activo.ToInt();

            lAnalisis = se.Analisis.Where(p =>
                    p.OrdenTrabajo.ClienteID == ClienteID &&
                    p.OrdenTrabajo.Consecion.Titulo == Titulo &&
                    p.OrdenTrabajo.Anexo.Anexo1 == Anexo &&
                    p.OrdenTrabajo.NormaID == NormaID &&
                    !p.FaltaPago &&
                    (p.Muestras.Where(q => q.NivelID == NivelID && q.Fecha >= FechaInicio && q.Fecha < FechaFin).Count() > 0) &&
                    p.Firmada && !p.Procesado && p.EstatusID == EstatusID
                ).ToList();

            return lAnalisis;
        }

        public List<DL_WS_SIRALAB.Muestra> Obten_MuestrasCompuestas(List<Analisi> analisis)
        {
            int NivelID = enumNivel.Compuesta.ToInt();
            var lMuestras = new List<DL_WS_SIRALAB.Muestra>();
            lMuestras = se.Analisis.SelectMany(q => q.Muestras).Where(r => r.NivelID == NivelID).ToList();

            return lMuestras;
        }

        public List<DL_WS_SIRALAB.NormaConfiguracion> Obten_NormaConfiguracion(DL_WS_SIRALAB.Analisi analisis)
        {
            int NivelID = enumNivel.Compuesta.ToInt();
            var Configuracion = analisis.OrdenTrabajo.Norma.NormaConfiguracions.Where(q =>
                q.CuerpoReceptorID == analisis.OrdenTrabajo.CuerpoReceptorID &&
                q.UsoAguaID == analisis.OrdenTrabajo.UsoAguaID).ToList();

            return Configuracion;
        }

        public List<BL_WS_SIRALAB.Analisis> Obten_AnalisisMensual(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion)
        {
            int NivelID = enumNivel.Simple.ToInt();
            var lAnalisis = new List<BL_WS_SIRALAB.Analisis>();

           //obtiene los analisis del trimestre solo para validacion de conductividad y cloruros no afecta los calculos...
            var analisisss = Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            List<Analisis> listadoAnalisis = new List<Analisis>();
            var analisisList = analisisss.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();
            foreach (var analisisItem in analisisList)
            {
                listadoAnalisis.Add(analisisItem);
            }
            ///////////////////////////////////////////////////////////////


            var lAnexos_1 = se.Anexoes.Where(p => p.Consecion.ClienteID == ClienteID && p.Consecion.Titulo == Titulo && p.Anexo1 == Anexo).ToList();
            var AnexoObj = se.Anexoes.Where(p => p.Consecion.ClienteID == ClienteID && p.Consecion.Titulo == Titulo && p.Anexo1 == Anexo).FirstOrDefault();

            // Obtenemos las combinaciones 
            List<usp_CR_UA_Periodo_Result> lCR_UA = new List<usp_CR_UA_Periodo_Result>();
            if (AnexoObj != null)
            {
                try
                {
                    lCR_UA = se.usp_CR_UA_Periodo(ClienteID, NormaID, AnexoObj.AnexoID, FechaInicio, FechaFin).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("No se localiza el anexo {0} en el título {1} para el cliente {2}", Anexo, Titulo, ClienteID), e);
                }
            }
            else
            {
                throw new Exception(string.Format("No se localiza el anexo {0} en el título {1} ", Anexo, Titulo));
            }

            foreach (var CR_UA in lCR_UA)
            {
                BL_WS_SIRALAB.Analisis an = new BL_WS_SIRALAB.Analisis();
                var CR = se.CuerpoReceptors.Where(p => p.CuerpoReceptorID == CR_UA.CuerpoReceptorID).FirstOrDefault();
                var UA = se.CatUsoAguas.Where(p => p.UsoAguaID == CR_UA.UsoAguaID).FirstOrDefault();

                an.laboratorio = new Laboratorio();
                an.AnalisisID = null;
                an.observaciones = "";
                an.falta_pago = false;
                an.orden_trabajo = new OrdenTrabajo
                {
                    Anexo = AnexoObj.ConvertAnexo(),
                    Cuerpo = CR.ConvertCuerpoReceptor(),
                    Uso = UA.ConvertUsoAgua(),                    
                    anio = FechaFin.Year
                };

                an.muestras = new List<Muestra>();
                BL_WS_SIRALAB.Muestra muestra = new Muestra();
                muestra.nivel = enumNivel.Mensual;
                muestra.Resultados = new List<ParametroAnalizado>();

                var LResult = se.usp_Calculos_Mensual(ClienteID, NormaID, AnexoObj.AnexoID, CR_UA.CuerpoReceptorID, CR_UA.UsoAguaID, 0, FechaInicio, FechaFin).ToList();
               
                foreach (var rest in LResult)
                {
                    var esParametroActivo = CR.NormaConfiguracions.FirstOrDefault(x => x.ParametroID == rest.ParametroID && x.NivelID==2);
                    if (esParametroActivo!=null && esParametroActivo.Activo)
                    {
                        ParametroAnalizado param = new ParametroAnalizado();
                        param.acepta_valor = rest.AceptaValor;
                        param.evalua = rest.Evalua;
                        param.exenta = rest.Exenta;
                        param.ley_federal = rest.LFD;
                        param.limite_inferior = rest.LimiteInferior;
                        param.limite_superior = rest.LimiteSuperior;
                        param.limite_inferior_LFD = rest.LimiteInferiorLFD;
                        param.limite_superior_LFD = rest.LimiteSuperiorLFD;

                        if (rest.Grupo == null ? false : (bool)rest.Grupo)
                        {
                            NormaConfiguracion_BL Configuracion = new NormaConfiguracion_BL();
                            List<NormaConfiguracion> ConfiguracionLocalizada = Configuracion.Obten_Configuracion(NormaID, (int)CR_UA.CuerpoReceptorID, (int)CR_UA.UsoAguaID, enumNivel.Compuesta);
                            NormaConfiguracion configuracioPadre = ConfiguracionLocalizada.Where(z => z.ParametroID == rest.ParametroID).FirstOrDefault();
                            List<NormaConfiguracion> ConfiguracionHijos = ConfiguracionLocalizada.Where(v => v.NormaConfiguracionPadreID == configuracioPadre.NormaConfiguracionID).ToList();
                            param.grupo = rest.Grupo == null ? null : ConfiguracionHijos.Select(z => z.Parametro.ConvertParametro()).ToList();
                        }
                        var par = se.Parametroes.Where(q => q.ParametroID == rest.ParametroID).ToList().FirstOrDefault();
                        param.parametro = par.ConvertParametro();
                        param.exento = rest.Exento;
                        if (rest.ResultadoNoAnalizado == 0)
                        {
                            param.no_analizado = true;
                            param.valor = 0;
                            param.valor_validacion = false;
                        }
                        else
                        {
                            param.valor = Convert.ToDecimal(rest.Valor);
                            param.valor_validacion = param.Valido(Evaluacion);
                        }
                        muestra.Resultados.Add(param);
                    }
                }

                an.muestras.Add(muestra);
                lAnalisis.Add(an);

                #region Conductividad y Enterococos

               
                List<PeriodResult> lPR = new List<PeriodResult>();
                PeriodResult pr = new PeriodResult();
                listadoAnalisis.Add(an);

                pr.analisis = listadoAnalisis;
                lPR.Add(pr);
            
                if (NormaID == 15 || NormaID == 16 || NormaID == 17)
                {
                    var NormaDQOCOT = string.Empty;
                    var Conductividad = string.Empty;
                    foreach (var item in lPR)
                    {
                        if (item.analisis.Count > 0)
                        {
                            NormaDQOCOT = item.analisis.First().NormaDQOCOT;

                            Conductividad = item.analisis.First().Conductividad;
                            if (Conductividad == null && NormaID == 15)
                            {
                                var esch = item.analisis.First().muestras.First().Resultados.FirstOrDefault(x => x.parametro.ParametroID == 694).no_analizado;
                                var enter = item.analisis.First().muestras.First().Resultados.FirstOrDefault(x => x.parametro.ParametroID == 689).no_analizado;

                                if (esch.Value)
                                {
                                    Conductividad = "Enterococos";
                                }
                                else if (enter.Value)
                                {
                                    Conductividad = "Escherichia";
                                }
                            }
                            foreach (var anl in item.analisis)
                            {
                                if (NormaDQOCOT != null && NormaDQOCOT.Contains("COT"))
                                {
                                    foreach (var m in anl.muestras)
                                    {
                                        if (m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {

                                                if (res.parametro.nombre.Contains("DQO") || res.parametro.nombre.Contains("Demanda Química de Oxigeno"))
                                                {
                                                    m.Resultados.RemoveAll(x=> x.parametro.nombre.Contains("DQO") || x.parametro.nombre.Contains("Demanda Química de Oxigeno"));
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }
                                if (NormaDQOCOT != null && NormaDQOCOT.Contains("DQO"))
                                {
                                    foreach (var m in anl.muestras)
                                    {
                                        if (m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.nombre.Contains("COT") || res.parametro.nombre.Contains("Carbono Orgánico Total"))
                                                {
                                                    m.Resultados.RemoveAll(x=>x.parametro.nombre.Contains("COT") || x.parametro.nombre.Contains("Carbono Orgánico Total"));
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }


                                if (Conductividad != null &&  Conductividad.Contains("Enterococos"))
                                {
                                    foreach (var m in anl.muestras)
                                    {
                                        if (m.nivel == enumNivel.Simple || m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.ParametroID == 694)
                                                {
                                                    m.Resultados.Remove(res);
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }


                                if (Conductividad != null &&  Conductividad.Contains("Escherichia"))
                                {
                                    foreach (var m in anl.muestras)
                                    {
                                        if (m.nivel == enumNivel.Simple || m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.ParametroID == 689)
                                                {
                                                    m.Resultados.Remove(res);
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
                #endregion

            }

            return lAnalisis;
        }

        private bool _isDisposed = false;
        public void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            se.Dispose();
            se = null;
        }

        #endregion
        #region Fuente_De_Abastecimiento
        public List<BL_WS_SIRALAB.ParametroAnalizado> Obten_AnalisisMensual_fuente(int ClienteID, List<int> MedidorID, int monthFuente, int yearanio)
        {
            List<Analisi> AnalisisValidar = new List<Analisi>();
            List<ParametroAnalizado> ParametrosMensualesAnalizar = new List<ParametroAnalizado>();
            List<ParametroAnalizado> ResultadosPromedio = new List<ParametroAnalizado>();
          
            //Buscamos todos los analisis de todos los medidores de fuente de abastecimiento
            foreach (int Medidor_Encontrado in MedidorID)
            {
                List<Analisi> AnalisisFuentesValidos = new List<Analisi>();
               
                List<Analisi> AnalisisFuentesValidosCompuestas = new List<Analisi>();
                List<int> MedidorXEncontrar = new List<int>();
                AnexoMedidor_BL AnexoMedidor_ID = new AnexoMedidor_BL();
                MedidorXEncontrar = AnexoMedidor_ID.Obten_AnexoMedidor(Medidor_Encontrado);
                List<Analisi> AnalisisFuentesXMedidor = new List<Analisi>();
                
                AnalisisFuentesXMedidor = Obten_Analisis_Fuente(ClienteID, MedidorXEncontrar, monthFuente, yearanio).ToList();
                if (AnalisisFuentesXMedidor.Count >= 2)
                {
                    List<Analisi> AnalisisFuentes = new List<Analisi>();
                    foreach (Analisi AnalisisxMedidor in AnalisisFuentesXMedidor)
                    {
                        AnalisisFuentes.Add(AnalisisxMedidor);
                    }

                    //Validamos que los caudales sean los mismos en las muestras simples           
                    while (AnalisisFuentes.Count > 0)
                    {
                        decimal Caudal = 0;
                        decimal CaudalAnt = 0;
                        bool PrimerRegistro = true;
                        bool Valido = true;
                        var AnalisisParametro = AnalisisFuentes[0];
                        foreach (var muestras in AnalisisParametro.Muestras)
                        {
                            if (muestras.NivelID == 1)
                            {
                                if (PrimerRegistro)
                                {
                                    Caudal = muestras.Caudal;
                                    CaudalAnt = muestras.Caudal;
                                    PrimerRegistro = false;
                                }
                                else
                                {
                                    CaudalAnt = Caudal;
                                    Caudal = muestras.Caudal;
                                    if (Caudal != 0 && CaudalAnt != 0)
                                    {
                                        if (Caudal != CaudalAnt)
                                        {
                                            Valido = false;
                                          
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (Valido)
                        {
                            AnalisisFuentesValidos.Add(AnalisisParametro);
                        }
                      
                        AnalisisFuentes.Remove(AnalisisParametro);
                    }

                    //Validamos que los caudales sean los mismos en las muestras compuestas          
                    while (AnalisisFuentesValidos.Count > 0)
                    {
                        decimal Caudal = 0;
                        decimal CaudalAnt = 0;
                        bool PrimerRegistro = true;
                        bool Valido = true;
                        var AnalisisParametro = AnalisisFuentesValidos[0];
                        foreach (var muestras in AnalisisParametro.Muestras)
                        {
                            if (muestras.NivelID == 2)
                            {
                                if (PrimerRegistro)
                                {
                                    Caudal = muestras.Caudal;
                                    CaudalAnt = muestras.Caudal;
                                    PrimerRegistro = false;
                                }
                                else
                                {
                                    CaudalAnt = Caudal;
                                    Caudal = muestras.Caudal;
                                    if (Caudal != CaudalAnt)
                                    {
                                        Valido = false;
                              
                                        break;
                                    }
                                }
                            }
                        }
                        if (Valido)
                        {
                            AnalisisFuentesValidosCompuestas.Add(AnalisisParametro);
                        }
                        
                        AnalisisFuentesValidos.Remove(AnalisisParametro);
                    }
                    foreach (var Analisis_A_Validar in AnalisisFuentesValidosCompuestas)
                    {
                        AnalisisValidar.Add(Analisis_A_Validar);
                    }
                    
                }
            }

            foreach (var AnalisisParametro in AnalisisValidar)
            {
                foreach (var muestras in AnalisisParametro.Muestras)
                {
                    if (muestras.NivelID == 2)
                    {
                        foreach (var parametro in muestras.MuestraValors)
                        {
                            if ((!parametro.NoAnalizado && parametro.Valor != null && parametro.LC_MC != null) || (!parametro.NoAnalizado && parametro.Valor != null && parametro.LC_MC == null && parametro.OperacionID > 0))
                            {
                                if ((parametro.LC_MC <= parametro.Valor) || (parametro.OperacionID > 0)) //Se agrega = parametro.LC_MC <= parametro.Valor en esta condicion .. 
                                {
                                    ParametroAnalizado param = new ParametroAnalizado();
                                    param.acepta_valor = parametro.AceptaValor;
                                    param.evalua = parametro.Evalua;
                                    param.exenta = parametro.Exenta;
                                    param.ley_federal = parametro.LFD;
                                    param.limite_inferior = parametro.LimiteInferior;
                                    param.limite_superior = parametro.LimiteSuperior;
                                    param.limite_inferior_LFD = parametro.LimiteInferiorLFD;
                                    param.limite_superior_LFD = parametro.LimiteSuperiorLFD;
                                    param.FechaConformacion = (System.DateTime)muestras.Fecha;
                                    var par = se.Parametroes.Where(q => q.ParametroID == parametro.ParametroID).ToList().FirstOrDefault();
                                    param.parametro = par.ConvertParametro();
                                    param.valor = Convert.ToDecimal(parametro.Valor);
                                    param.valor_validacion = null;
                                    ParametrosMensualesAnalizar.Add(param);
                                }
                            }
                        }
                    }
                }
            }

            while (ParametrosMensualesAnalizar.Count > 0)
            {
                //variables
                List<ParametroAnalizado> Temp = new List<ParametroAnalizado>();
                int ParametroIDTemp = 0;
                ParametroAnalizado TemporalParametro = new ParametroAnalizado();

                //Parametro
                ParametroIDTemp = ParametrosMensualesAnalizar[0].parametro.ParametroID;
                Temp = ParametrosMensualesAnalizar.Where(p => p.parametro.ParametroID == ParametroIDTemp).ToList();

                TemporalParametro.acepta_valor = Temp[0].acepta_valor;
                TemporalParametro.evalua = Temp[0].evalua;
                TemporalParametro.exenta = Temp[0].exenta;
                TemporalParametro.ley_federal = Temp[0].ley_federal;
                TemporalParametro.limite_inferior = Temp[0].limite_inferior;
                TemporalParametro.limite_superior = Temp[0].limite_superior;
                TemporalParametro.limite_inferior_LFD = Temp[0].limite_inferior_LFD;
                TemporalParametro.limite_superior_LFD = Temp[0].limite_superior_LFD;               
                TemporalParametro.parametro = Temp[0].parametro;
                TemporalParametro.valor_validacion = null;

                decimal ValorPromedio = 0;
                int numero = 0;
                DateTime FechaConformacionMaxima = new DateTime();                              
                foreach (ParametroAnalizado ParametroRemove in Temp)
                {
                    ValorPromedio += ParametroRemove.valor == null ? 0 : (decimal)ParametroRemove.valor;
                    numero++;
                    if (ParametroRemove.FechaConformacion > FechaConformacionMaxima)
                        {
                            FechaConformacionMaxima = ParametroRemove.FechaConformacion;                            
                        }    
                    ParametrosMensualesAnalizar.Remove(ParametroRemove);
                }
                TemporalParametro.FechaConformacion = FechaConformacionMaxima;
                TemporalParametro.valor = ValorPromedio / numero;
                ResultadosPromedio.Add(TemporalParametro);
            }
         
            return ResultadosPromedio;
        }

        public List<int> Obten_AnalisisMensual_fuenteListado(int ClienteID, List<int> MedidorID,int monthFuente, int yearanio)
        {
            List<Analisi> AnalisisValidar = new List<Analisi>();            
            List<int> ResultadoListado = new List<int>();
            //Buscamos todos los analisis de todos los medidores de fuente de abastecimiento
            foreach (int Medidor_Encontrado in MedidorID)
            {
                List<Analisi> AnalisisFuentesValidos = new List<Analisi>();
                List<Analisi> AnalisisFuentesValidosCompuestas = new List<Analisi>();
                List<int> MedidorXEncontrar = new List<int>();
                AnexoMedidor_BL AnexoMedidor_ID = new AnexoMedidor_BL();
                MedidorXEncontrar = AnexoMedidor_ID.Obten_AnexoMedidor(Medidor_Encontrado);
                List<Analisi> AnalisisFuentesXMedidor = new List<Analisi>();
                AnalisisFuentesXMedidor = Obten_Analisis_Fuente(ClienteID, MedidorXEncontrar, monthFuente, yearanio).ToList();
                if (AnalisisFuentesXMedidor.Count >= 2)
                {
                    List<Analisi> AnalisisFuentes = new List<Analisi>();
                    foreach (Analisi AnalisisxMedidor in AnalisisFuentesXMedidor)
                    {
                        AnalisisFuentes.Add(AnalisisxMedidor);
                    }

                    //Validamos que los caudales sean los mismos en las muestras simples           
                    while (AnalisisFuentes.Count > 0)
                    {
                        decimal Caudal = 0;
                        decimal CaudalAnt = 0;
                        bool PrimerRegistro = true;
                        bool Valido = true;
                        var AnalisisParametro = AnalisisFuentes[0];
                        foreach (var muestras in AnalisisParametro.Muestras)
                        {
                            if (muestras.NivelID == 1)
                            {
                                if (PrimerRegistro)
                                {
                                    Caudal = muestras.Caudal;
                                    CaudalAnt = muestras.Caudal;
                                    PrimerRegistro = false;
                                }
                                else
                                {
                                    CaudalAnt = Caudal;
                                    Caudal = muestras.Caudal;
                                    if (Caudal != 0 && CaudalAnt != 0)
                                    {
                                        if (Caudal != CaudalAnt)
                                        {
                                            Valido = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (Valido)
                        {
                            AnalisisFuentesValidos.Add(AnalisisParametro);
                        }
                        AnalisisFuentes.Remove(AnalisisParametro);
                    }

                    //Validamos que los caudales sean los mismos en las muestras compuestas          
                    while (AnalisisFuentesValidos.Count > 0)
                    {
                        decimal Caudal = 0;
                        decimal CaudalAnt = 0;
                        bool PrimerRegistro = true;
                        bool Valido = true;
                        var AnalisisParametro = AnalisisFuentesValidos[0];
                        foreach (var muestras in AnalisisParametro.Muestras)
                        {
                            if (muestras.NivelID == 2)
                            {
                                if (PrimerRegistro)
                                {
                                    Caudal = muestras.Caudal;
                                    CaudalAnt = muestras.Caudal;
                                    PrimerRegistro = false;
                                }
                                else
                                {
                                    CaudalAnt = Caudal;
                                    Caudal = muestras.Caudal;
                                    if (Caudal != CaudalAnt)
                                    {
                                        Valido = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (Valido)
                        {
                            AnalisisFuentesValidosCompuestas.Add(AnalisisParametro);
                        }
                        AnalisisFuentesValidos.Remove(AnalisisParametro);
                    }
                    foreach (var Analisis_A_Validar in AnalisisFuentesValidosCompuestas)
                    {
                        AnalisisValidar.Add(Analisis_A_Validar);
                    }
                }
            }

            foreach (var AnalisisParametro in AnalisisValidar)
            {
                bool banderaValidacion = false;
                foreach (var muestras in AnalisisParametro.Muestras)
                {
                    if (muestras.NivelID == 2)
                    {
                        foreach (var parametro in muestras.MuestraValors)
                        {
                            if ((!parametro.NoAnalizado && parametro.Valor != null && parametro.LC_MC != null && parametro.Valor > 0 && parametro.LC_MC >= 0) || (!parametro.NoAnalizado && parametro.Valor != null && parametro.LC_MC == null && parametro.Valor > 0 && parametro.OperacionID > 0))
                            {
                                if ((parametro.LC_MC <= parametro.Valor) || (parametro.OperacionID > 0))
                                {
                                    banderaValidacion = true;
                                }
                                else
                                {
                                    banderaValidacion = false;
                                }
                            }
                        }
                    }
                }
                if (banderaValidacion)
                {
                    ResultadoListado.Add(AnalisisParametro.AnalisisID);
                }
            }
            return ResultadoListado;
        }

        public List<int> Obten_AnalisisMensual_fuenteListadoFecha(int ClienteID, List<int> MedidorID, DateTime FechaMaxima, int monthFuente, int yearanio)
        {
            List<Analisi> AnalisisValidar = new List<Analisi>();            
            List<int> ResultadoListado = new List<int>();
            List<int> ResultadoListadoDiferenteCaudal = new List<int>();
            //Buscamos todos los analisis de todos los medidores de fuente de abastecimiento
            foreach (int Medidor_Encontrado in MedidorID)
            {
                List<Analisi> AnalisisFuentesValidos = new List<Analisi>();
                List<Analisi> AnalisisFuentesValidosCompuestas = new List<Analisi>();
                List<int> MedidorXEncontrar = new List<int>();
                AnexoMedidor_BL AnexoMedidor_ID = new AnexoMedidor_BL();
                MedidorXEncontrar = AnexoMedidor_ID.Obten_AnexoMedidor(Medidor_Encontrado);
                List<Analisi> AnalisisFuentesXMedidor = new List<Analisi>();
                AnalisisFuentesXMedidor = Obten_Analisis_Fuente(ClienteID, MedidorXEncontrar, monthFuente, yearanio).ToList();
                var DiferenteCaudal = false;
                if (AnalisisFuentesXMedidor.Count >= 2)
                {
                    List<Analisi> AnalisisFuentes = new List<Analisi>();
                    foreach (Analisi AnalisisxMedidor in AnalisisFuentesXMedidor)
                    {
                        AnalisisFuentes.Add(AnalisisxMedidor);
                    }

                    //Validamos que los caudales sean los mismos en las muestras simples           
                    while (AnalisisFuentes.Count > 0)
                    {
                        decimal Caudal = 0;
                        decimal CaudalAnt = 0;
                        bool PrimerRegistro = true;
                        bool Valido = true;
                        var AnalisisParametro = AnalisisFuentes[0];
                        foreach (var muestras in AnalisisParametro.Muestras)
                        {
                            if (muestras.NivelID == 1)
                            {
                                if (PrimerRegistro)
                                {
                                    Caudal = muestras.Caudal;
                                    CaudalAnt = muestras.Caudal;
                                    PrimerRegistro = false;
                                }
                                else
                                {
                                    CaudalAnt = Caudal;
                                    Caudal = muestras.Caudal;                                    
                                        if (Caudal != CaudalAnt)
                                        {
                                            Valido = false;
                                            DiferenteCaudal = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (Valido)
                        {
                            AnalisisFuentesValidos.Add(AnalisisParametro);
                        }
                        if (DiferenteCaudal)
                        {
                            ResultadoListadoDiferenteCaudal.Add(AnalisisParametro.AnalisisID);
                        }
                        AnalisisFuentes.Remove(AnalisisParametro);
                    }

                    //Validamos que los caudales sean los mismos en las muestras compuestas          
                    while (AnalisisFuentesValidos.Count > 0)
                    {
                        decimal Caudal = 0;
                        decimal CaudalAnt = 0;
                        bool PrimerRegistro = true;
                        bool Valido = true;
                        var AnalisisParametro = AnalisisFuentesValidos[0];
                        foreach (var muestras in AnalisisParametro.Muestras)
                        {
                            if (muestras.NivelID == 2)
                            {
                                if (PrimerRegistro)
                                {
                                    Caudal = muestras.Caudal;
                                    CaudalAnt = muestras.Caudal;
                                    PrimerRegistro = false;
                                }
                                else
                                {
                                    CaudalAnt = Caudal;
                                    Caudal = muestras.Caudal;
                                    if (Caudal != CaudalAnt)
                                    {
                                        Valido = false;
                                        DiferenteCaudal = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (Valido)
                        {
                            AnalisisFuentesValidosCompuestas.Add(AnalisisParametro);
                        }
                        if (DiferenteCaudal)
                        {
                            ResultadoListadoDiferenteCaudal.Add(AnalisisParametro.AnalisisID);
                        }
                        AnalisisFuentesValidos.Remove(AnalisisParametro);
                    }
                    foreach (var Analisis_A_Validar in AnalisisFuentesValidosCompuestas)
                    {
                        AnalisisValidar.Add(Analisis_A_Validar);
                    }
                }
            }

            foreach (var AnalisisParametro in AnalisisValidar)
            {
                bool banderaValidacion = false;
                foreach (var muestras in AnalisisParametro.Muestras)
                {
                    if (muestras.NivelID == 2)
                    {
                        foreach (var parametro in muestras.MuestraValors)
                        {
                            if ((!parametro.NoAnalizado && parametro.Valor != null && parametro.LC_MC != null && parametro.Valor > 0 && parametro.LC_MC >= 0) || (!parametro.NoAnalizado && parametro.Valor != null && parametro.LC_MC == null && parametro.Valor > 0 && parametro.OperacionID > 0))
                            {
                                if ((parametro.LC_MC <= parametro.Valor && muestras.Fecha <= FechaMaxima) || (parametro.OperacionID > 0 && muestras.Fecha <= FechaMaxima))
                                {
                                    banderaValidacion = true;
                                }
                                else
                                {
                                    banderaValidacion = false;
                                }
                            }
                        }
                    }
                }
                if (banderaValidacion)
                {
                    ResultadoListado.Add(AnalisisParametro.AnalisisID);
                }
            }
            if (ResultadoListado.Count()==0)
            {
                ResultadoListado = ResultadoListadoDiferenteCaudal.Distinct().ToList();
            }
            return ResultadoListado;
        }

        public List<BL_WS_SIRALAB.ParametroAnalizado> Obten_AnalisisMensual_fuenteCompuestaFecha(List<DL_WS_SIRALAB.Muestra> Muestras, DL_WS_SIRALAB.Norma norma)
        {
            List<ParametroAnalizado> ResultadosM = new List<ParametroAnalizado>();
            decimal CaudalSumado = 0;

            if (Muestras.Count > 0)
            {
                List<MuestraValor> ListaParametrosCompuesta = new List<MuestraValor>();
                List<MuestraValor> ListaParametrosCompuestaRecon = new List<MuestraValor>();
                int? usoAguaID = 0;
                int? CuerpoReceptorID = 0;

                foreach (var Muestra in Muestras)
                {
                    var Nivel = Muestra.NivelID.ToEnumNivel();
                    switch (Nivel)
                    {
                        case enumNivel.Compuesta:
                            {
                                CaudalSumado += Muestra.Caudal;
                                foreach (MuestraValor ParametroAnalizar in Muestra.MuestraValors)
                                {
                                    ListaParametrosCompuesta.Add(ParametroAnalizar);
                                }
                            }
                            break;
                    }
                    Analisi Analisis1 = Muestra.Analisi;
                    DL_WS_SIRALAB.OrdenTrabajo orden1 = Analisis1.OrdenTrabajo;

                    usoAguaID = orden1.UsoAguaID;
                    CuerpoReceptorID = orden1.CuerpoReceptorID;
                }



                while (ListaParametrosCompuesta.Count > 0)
                {
                    //variables
                    List<MuestraValor> Temp = new List<MuestraValor>();
                    int ParametroIDTemp = 0;
                    MuestraValor TemporalParametro = new MuestraValor();

                    //Parametro
                    ParametroIDTemp = ListaParametrosCompuesta[0].ParametroID;
                    Temp = ListaParametrosCompuesta.Where(p => p.ParametroID == ParametroIDTemp).ToList();

                    TemporalParametro = ListaParametrosCompuesta[0];
                    List<NormaConfiguracion> ConfiguracionLimites = norma.NormaConfiguracions.ToList();
                    NormaConfiguracion Limites = ConfiguracionLimites.Where(P => P.ParametroID == ParametroIDTemp && P.NivelID == 3 && P.CuerpoReceptorID == CuerpoReceptorID && P.UsoAguaID == usoAguaID).FirstOrDefault();
                    
                    if (Limites != null)
                    {
                        TemporalParametro.LimiteSuperior = Limites.LimiteSuperior;
                        TemporalParametro.LimiteInferior = Limites.LimiteInferior;
                        TemporalParametro.LimiteInferiorLFD = Limites.LimiteInferiorLFD;
                        TemporalParametro.LimiteSuperiorLFD = Limites.LimiteSuperiorLFD;
                    }
                    decimal ValorPromedio = 0;
                    DateTime FechaConformacion = DateTime.Now;
                    DL_WS_SIRALAB.Muestra MuestraFinal = new DL_WS_SIRALAB.Muestra();
                    foreach (MuestraValor ParametroRemove in Temp)
                    {
                        decimal ValorSinCaudal = ParametroRemove.Valor == null ? 0 : (decimal)ParametroRemove.Valor;
                        DL_WS_SIRALAB.Muestra MuestraCaudal = ParametroRemove.Muestra;
                        if (MuestraCaudal.Fecha < FechaConformacion)
                        {
                            FechaConformacion = (DateTime)MuestraCaudal.Fecha;
                            MuestraFinal = MuestraCaudal;
                        }
                        ValorPromedio += ((ValorSinCaudal * MuestraCaudal.Caudal) / CaudalSumado);
                        ListaParametrosCompuesta.Remove(ParametroRemove);
                    }
                    TemporalParametro.Muestra = MuestraFinal;
                    TemporalParametro.Valor = ValorPromedio;
                    ListaParametrosCompuestaRecon.Add(TemporalParametro);
                }

                int[] idsAExcluir = { 931, 1072, 1073, 1074 };
                foreach (MuestraValor ParametroFinal in ListaParametrosCompuestaRecon)
                {
                    List<NormaConfiguracion> ConfiguracionLimites = norma.NormaConfiguracions.ToList();
                    var esParametroActivo = ConfiguracionLimites.FirstOrDefault(P => P.ParametroID == ParametroFinal.ParametroID && P.NivelID == 2 && P.CuerpoReceptorID == CuerpoReceptorID && P.UsoAguaID == usoAguaID);
                    if (esParametroActivo != null && esParametroActivo.Activo)
                    {
                        //Validación de exclusión activada y optimizada
                        if (idsAExcluir.Contains(esParametroActivo.ParametroID))
                        {
                            continue;
                        }
                        ParametroAnalizado param = new ParametroAnalizado();
                        param.acepta_valor = ParametroFinal.AceptaValor;
                        param.evalua = ParametroFinal.Evalua;
                        param.exenta = ParametroFinal.Exenta;
                        param.ley_federal = ParametroFinal.LFD;
                        param.limite_inferior = ParametroFinal.LimiteInferior;
                        param.limite_superior = ParametroFinal.LimiteSuperior;
                        param.limite_inferior_LFD = ParametroFinal.LimiteInferiorLFD;
                        param.limite_superior_LFD = ParametroFinal.LimiteSuperiorLFD;
                        DL_WS_SIRALAB.Muestra MuestraConformacion = ParametroFinal.Muestra;
                        param.FechaConformacion = (System.DateTime)MuestraConformacion.Fecha;
                        var par = se.Parametroes.Where(q => q.ParametroID == ParametroFinal.ParametroID).ToList().FirstOrDefault();
                        param.parametro = par.ConvertParametro();
                        param.valor = Convert.ToDecimal(ParametroFinal.Valor);
                        param.valor_validacion = null;
                        ResultadosM.Add(param);
                    }
                }
            }
            return ResultadosM;
        }

        public List<BL_WS_SIRALAB.ParametroAnalizado> Obten_AnalisisMensual_fuenteCompuestaXFecha(List<BL_WS_SIRALAB.Muestra> Muestras, DL_WS_SIRALAB.Norma norma, int? CRID, int? USOID)
        {
            List<ParametroAnalizado> ListaParametrosCompuestaRecon = new List<ParametroAnalizado>();
            int? usoAguaID = 0;
            int? CuerpoReceptorID = 0;
            decimal CaudalSumado = 0;
            usoAguaID = USOID;
            CuerpoReceptorID = CRID;

            if (Muestras.Count > 0)
            {
                List<ParametroAnalizado> ListaParametrosCompuesta = new List<ParametroAnalizado>();
                foreach (var Muestra in Muestras)
                {
                    var Nivel = Muestra.nivel;
                    switch (Nivel)
                    {
                        case enumNivel.Compuesta:
                            {
                                CaudalSumado += Muestra.caudal == null ? 0 : (decimal)Muestra.caudal;
                                foreach (ParametroAnalizado ParametroAnalizar in Muestra.Resultados)
                                {
                                        ParametroAnalizar.Caudal = Muestra.caudal;
                                        ListaParametrosCompuesta.Add(ParametroAnalizar);
                                }
                            }
                            break;
                    }                   
                }

                while (ListaParametrosCompuesta.Count > 0)
                {
                    //variables
                    List<ParametroAnalizado> Temp = new List<ParametroAnalizado>();
                    int ParametroIDTemp = 0;
                    ParametroAnalizado TemporalParametro = new ParametroAnalizado();

                    //Parametro
                    ParametroIDTemp = ListaParametrosCompuesta[0].parametro.ParametroID;
                    Temp = ListaParametrosCompuesta.Where(p => p.parametro.ParametroID == ParametroIDTemp).ToList();

                    TemporalParametro = ListaParametrosCompuesta[0];
                    List<NormaConfiguracion> ConfiguracionLimites = norma.NormaConfiguracions.ToList();
                    NormaConfiguracion Limites = ConfiguracionLimites.Where(P => P.ParametroID == ParametroIDTemp && P.NivelID == 3 && P.CuerpoReceptorID == CuerpoReceptorID && P.UsoAguaID == usoAguaID).FirstOrDefault();
                    
                    if (Limites != null)
                    {
                        TemporalParametro.limite_superior = Limites.LimiteSuperior;
                        TemporalParametro.limite_inferior = Limites.LimiteInferior;
                        TemporalParametro.limite_inferior_LFD = Limites.LimiteInferiorLFD;
                        TemporalParametro.limite_superior_LFD = Limites.LimiteSuperiorLFD;
                    }
                    decimal ValorPromedio = 0;
                    foreach (ParametroAnalizado ParametroRemove in Temp)
                    {
                        decimal ValorSinCaudal = ParametroRemove.valor == null ? 0 : (decimal)ParametroRemove.valor;
                        if (ValorSinCaudal < 0)
                        {
                            ValorSinCaudal = 0;
                        }
                        decimal CaudalParametro = ParametroRemove.Caudal == null ? 0 : (decimal)ParametroRemove.Caudal;
                        ValorPromedio += ((ValorSinCaudal * CaudalParametro) / CaudalSumado);
                        ListaParametrosCompuesta.Remove(ParametroRemove);
                    }
                    if (ValorPromedio < 0)
                    {
                        ValorPromedio = 0;
                    }
                    if (TemporalParametro.Diferencia < 0)
                    {
                        TemporalParametro.Diferencia = 0;
                    }
                    TemporalParametro.valor = ValorPromedio;

                    var esParametroActivo = ConfiguracionLimites.FirstOrDefault(P => P.ParametroID == TemporalParametro.parametro.ParametroID && P.NivelID == 2 && P.CuerpoReceptorID == CuerpoReceptorID && P.UsoAguaID == usoAguaID);
                    if (esParametroActivo != null && esParametroActivo.Activo)
                    {
                        ListaParametrosCompuestaRecon.Add(TemporalParametro);
                    }
                }
            }
            return ListaParametrosCompuestaRecon;
        }

        public List<BL_WS_SIRALAB.Analisis> Obten_AnalisisMensual_fuente_Completo(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, List<int> MedidorID, List<BL_WS_SIRALAB.Muestra> MuestrasCompuestas, DL_WS_SIRALAB.Norma norma)
        {

            int NivelID = enumNivel.Simple.ToInt();
            var lAnalisis = new List<BL_WS_SIRALAB.Analisis>();

            //obtiene los analisis del trimestre solo para validacion de conductividad y cloruros no afecta los calculos...
            var analisisss = Obten_Analisis(ClienteID, FechaInicio, FechaFin, Titulo, Anexo, NormaID);
            List<Analisis> listadoAnalisis = new List<Analisis>();
            var analisisList = analisisss.Select(p => p.ConvertAnalisis(Evaluacion)).ToList();
            foreach (var analisisItem in analisisList)
            {
                listadoAnalisis.Add(analisisItem);
            }
            ///////////////////////////////////////////////////////////////

            var lAnexos_1 = se.Anexoes.Where(p => p.Consecion.ClienteID == ClienteID && p.Consecion.Titulo == Titulo && p.Anexo1 == Anexo).ToList();
            var AnexoObj = se.Anexoes.Where(p => p.Consecion.ClienteID == ClienteID && p.Consecion.Titulo == Titulo && p.Anexo1 == Anexo).FirstOrDefault();

            // Obtenemos las combinaciones 
            List<usp_CR_UA_Periodo_Result> lCR_UA = new List<usp_CR_UA_Periodo_Result>();
            if (AnexoObj != null)
            {
                try
                {
                    lCR_UA = se.usp_CR_UA_Periodo(ClienteID, NormaID, AnexoObj.AnexoID, FechaInicio, FechaFin).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("No se localiza el anexo {0} en el título {1} para el cliente {2}", Anexo, Titulo, ClienteID), e);
                }
            }
            else
            {
                throw new Exception(string.Format("No se localiza el anexo {0} en el título {1} ", Anexo, Titulo));
            }

            foreach (var CR_UA in lCR_UA)
            {
                BL_WS_SIRALAB.Analisis an = new BL_WS_SIRALAB.Analisis();
                var CR = se.CuerpoReceptors.Where(p => p.CuerpoReceptorID == CR_UA.CuerpoReceptorID).FirstOrDefault();
                var UA = se.CatUsoAguas.Where(p => p.UsoAguaID == CR_UA.UsoAguaID).FirstOrDefault();

                an.laboratorio = new Laboratorio();
                an.AnalisisID = null;
                an.observaciones = "";
                an.falta_pago = false;
                an.orden_trabajo = new OrdenTrabajo
                {
                    Anexo = AnexoObj.ConvertAnexo(),
                    Cuerpo = CR.ConvertCuerpoReceptor(),
                    Uso = UA.ConvertUsoAgua(),                    
                    anio = FechaFin.Year
                };

                an.muestras = new List<Muestra>();
                BL_WS_SIRALAB.Muestra muestra = new Muestra();
                muestra.nivel = enumNivel.Mensual;
                muestra.Resultados = new List<ParametroAnalizado>();
                List<ParametroAnalizado> ResultadosEvaluados = new List<ParametroAnalizado>();
                muestra.Resultados = Obten_AnalisisMensual_fuenteCompuestaXFecha(MuestrasCompuestas, norma, CR_UA.CuerpoReceptorID, CR_UA.UsoAguaID);
                foreach (ParametroAnalizado ParametroAnalizarM in muestra.Resultados)
                {
                        ParametroAnalizarM.valor_validacion = ParametroAnalizarM.Valido(Evaluacion);
                        ResultadosEvaluados.Add(ParametroAnalizarM);
                }
                muestra.Resultados = ResultadosEvaluados;
                an.muestras.Add(muestra);
                lAnalisis.Add(an);

                #region Conductividad y Enterococos


                List<PeriodResult> lPR = new List<PeriodResult>();
                PeriodResult pr = new PeriodResult();
                listadoAnalisis.Add(an);


                pr.analisis = listadoAnalisis;
                if (NormaID == 15 || NormaID == 16 || NormaID == 17)
                {
                    var NormaDQOCOT = string.Empty;
                    var Conductividad = string.Empty;
                    foreach (var item in lPR)
                    {
                        if (item.analisis.Count > 0)
                        {
                            NormaDQOCOT = item.analisis.First().NormaDQOCOT;

                            Conductividad = item.analisis.First().Conductividad;

                            if (Conductividad ==null && NormaID==15)
                            {
                                var esch = item.analisis.First().muestras.First().Resultados.FirstOrDefault(x => x.parametro.ParametroID == 694).no_analizado;
                                var enter= item.analisis.First().muestras.First().Resultados.FirstOrDefault(x => x.parametro.ParametroID == 689).no_analizado;

                                if (esch.Value)
                                {
                                    Conductividad = "Enterococos";
                                }
                                else if(enter.Value)
                                {
                                    Conductividad = "Escherichia";
                                }
                            }

                            foreach (var anl in item.analisis)
                            {
                                if (NormaDQOCOT!= null && NormaDQOCOT.Contains("COT"))
                                {
                                    foreach (var m in anl.muestras)
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
                                    foreach (var m in anl.muestras)
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


                                if (Conductividad != null && Conductividad.Contains("Enterococos"))
                                {
                                    foreach (var m in anl.muestras)
                                    {
                                        if (m.nivel == enumNivel.Simple || m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.ParametroID == 694)
                                                {
                                                    m.Resultados.Remove(res);
                                                    break;
                                                    //res.valor = 0;
                                                }
                                            }
                                        }

                                    }
                                }


                                if (Conductividad != null &&  Conductividad.Contains("Escherichia"))
                                {
                                    foreach (var m in anl.muestras)
                                    {
                                        if (m.nivel == enumNivel.Simple || m.nivel == enumNivel.Compuesta || m.nivel == enumNivel.Mensual)
                                        {
                                            foreach (var res in m.Resultados)
                                            {
                                                if (res.parametro.ParametroID == 689)
                                                {
                                                    m.Resultados.Remove(res);
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
                #endregion




            }

            return lAnalisis;
        }

        public List<DL_WS_SIRALAB.Analisi> Obten_Analisis_Fuente(int ClienteID, List<int> AnexoID,int month, int year)
        {
            var IdNorma = 0;
            int Anio = int.Parse(ConfigurationManager.AppSettings["ValidaPrograma"]);
            var norma = ConfigurationManager.AppSettings["NormaFuenteAbastecimiento2021"].ToString();
            Norma2021_BL normaBLL = new Norma2021_BL();
            var normas= se.Normas.ToList();
            if (year >= Anio)
            {
                IdNorma = normas.FirstOrDefault(x => x.Nombre.ToUpper().Replace(" ", "") == norma.Replace(" ", "")).NormaID;
            }
            else
            {
                IdNorma = 13;
            }
          
            int NivelID = enumNivel.Simple.ToInt();
            var lAnalisis = new List<DL_WS_SIRALAB.Analisi>();
            int EstatusID = enumAnalisisEstatus.Activo.ToInt();
            if (year >= Anio)
            {
                var mesInicio = 0;
                var mesFin = 0;

                switch (month)
                {
                    case 3:
                        mesInicio = 1;
                        mesFin = 3;
                        break;
                    case 6:
                        mesInicio = 4;
                        mesFin = 6;
                        break;
                    case 9:
                        mesInicio = 7;
                        mesFin = 9;
                        break;
                    case 12:
                        mesInicio = 10;
                        mesFin = 12;
                        break;
                }
                lAnalisis = se.Analisis.Where(p =>
                    p.OrdenTrabajo.ClienteID == ClienteID &&
                    AnexoID.Contains(p.OrdenTrabajo.AnexoID ?? 0) &&
                    p.OrdenTrabajo.NormaID == IdNorma &&
                    !p.FaltaPago &&
                    (p.Muestras.Where(q => q.NivelID == NivelID).Count() > 0) &&
                    p.Firmada && p.EstatusID == EstatusID && p.OrdenTrabajo.FuenteAbastecimiento == true && !p.Procesado
                    && (p.Muestras.Where(R => R.NivelID == 2 && R.Fecha.Value.Year == year).Count() > 0)
                    && (p.Muestras.Where(R => R.NivelID == 2 && (R.Fecha.Value.Month>=mesInicio && R.Fecha.Value.Month <= mesFin)).Count() > 0)
                ).ToList();
             
                #region ConductividadYCloruros
                foreach (var analisis in lAnalisis)
                {
                    var muestraCompuesta = analisis.Muestras.Where(muestra => muestra.NivelID == 2);
                    var muestrasSimples = analisis.Muestras.Where(muestra => muestra.NivelID == 1);
                    foreach (var muestra in muestraCompuesta)
                    {
                        var TipoDeParametro = analisis.NormaDQOCOT;
                        var Conductividad = analisis.Conductividad;
                        var listaMuestraValors = muestra.MuestraValors.ToList();
                        for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
                        {
                            //Si el tipo de parametro se hizo para cot se remueve DQO y si se hizo para DQO remueve COT RGG
                            if (TipoDeParametro != null)
                            {
                                if (TipoDeParametro == "COT")
                                {
                                    if (listaMuestraValors[i].ParametroID == 667)// el valor es de DQO
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                                else if (TipoDeParametro == "DQO")
                                {
                                    if (listaMuestraValors[i].ParametroID == 553 || listaMuestraValors[i].ParametroID == 602) //Los dos valores indican el id del parametro COT
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                            }

                            if (Conductividad != null && Conductividad != string.Empty)
                            {
                                if (Conductividad == "Escherichia")
                                {
                                    if (listaMuestraValors[i].ParametroID == 689)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }

                                }
                                else if (Conductividad == "Enterococos")
                                {
                                    if (listaMuestraValors[i].ParametroID == 694)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                            }



                        }

                        muestra.MuestraValors = listaMuestraValors;
                    }


                    foreach (var muestra in muestrasSimples)
                    {
                        var Conductividad = analisis.Conductividad;
                        var listaMuestraValors = muestra.MuestraValors.ToList();
                        for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
                        {
                            //Si el tipo de parametro se hizo para Escherichia se remueve Enterococos y si se hizo para Enterococos remueve Escherichia RGG

                            if (Conductividad != null && Conductividad != string.Empty)
                            {
                                if (Conductividad == "Escherichia")
                                {
                                    if (listaMuestraValors[i].ParametroID == 689)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }

                                }
                                else if (Conductividad == "Enterococos")
                                {
                                    if (listaMuestraValors[i].ParametroID == 694)
                                    {
                                        listaMuestraValors.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        muestra.MuestraValors = listaMuestraValors;
                    }

                }
                #endregion
            }
            else
            {
                lAnalisis = se.Analisis.Where(p =>
                    p.OrdenTrabajo.ClienteID == ClienteID &&
                    AnexoID.Contains(p.OrdenTrabajo.AnexoID ?? 0) &&
                    p.OrdenTrabajo.NormaID == IdNorma &&
                    !p.FaltaPago &&
                    (p.Muestras.Where(q => q.NivelID == NivelID).Count() > 0) &&
                    p.Firmada && p.EstatusID == EstatusID && p.OrdenTrabajo.FuenteAbastecimiento == true && !p.Procesado
                    && (p.Muestras.Where(R => R.NivelID == 2 && R.Fecha.Value.Year == year).Count() > 0)
                    && (p.Muestras.Where(R => R.NivelID == 2 && R.Fecha.Value.Month <= month).Count() > 0)
                ).ToList();
            }
            
            return lAnalisis;
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
        }

        ~Analisis_BL()
        {
            this.Dispose(false);

        }
    }
}
