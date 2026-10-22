using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRALAB_WCF.Modelos;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB.Controles;
using System.ServiceModel.Web;
using System.Net;
using SIRALAB_BL.catalogos;
using SIRALAB_SIRFIC;
using SIRALAB_DAL;
using System.Configuration;
using System.IO;
using static SIRALAB_BL.Archivos;
using System.Transactions;
using System.Data.Entity.Validation;
using SIRALAB.Common;

namespace SIRALAB_WCF.Implementacion
{
    public class IOrdenTrabajo
    {

        public class FiltroOrdenTrabajo : FiltroOrdenTrabajoArgBase
        {
            public FiltroOrdenTrabajo()
            {
                EstatusCancelacion = 2;
            }

        }
        int pobjOrdendeTrabajo;
        string pathFolder = string.Empty;
        string tipo = string.Empty;
        public void Buscar(ref List<Modelos.OrdenTrabajo> lstOrdenes, FiltroOrdenTrabajo filtro)
        {

            var oT = new vw_oT();
            IEnumerable<Vw_Ot_Grid> ordenesT = oT.ListaOrdenTrabajo(filtro);
            var lOt = ordenesT.Select(p => new OrdenTrabajoLista
            {
                OrdenTrabajoId = p.OrdenTrabajoID,
                Folio = p.Informe,
                Titulo = p.Titulo,
                Anexo = p.Anexo.ToString(),
                RFCCliente = p.RFC,
                Registro = p.FechaEmision.ToShortDateString(),
                muestreado = p.EstatusID.ToString(),
                NombreLaboratorio = p.NombreLaboratorio,
                Norma = p.Norma
            }).ToList();
            var Analisis = oT.ListaAnalisis(lOt.Select(p => p.OrdenTrabajoId ?? 0).ToList());

            IEnumerable<uspBusquedaOrdenesTrabajoExcel_Result> ordenesE = oT.ListaOrdenTrabajoExcel(filtro);
            var ordenes = ordenesE.Select(p => new ExcelExportModel
            {
                Firmada = p.Firmada,
                EstatusAnalisis = p.EstatusAnalisis,
                LaboratorioRegistraOT = p.LaboratorioRegistraOT,
                RFCLaboratorio = p.RFCLaboratorio,
                LaboratorioMuestreo = p.LaboratorioMuestreo,
                Muestreador = p.Muestreador,
                LaboratorioAnalizo = p.LaboratorioAnalizo,
                NumeroInforme = p.NumeroInforme,
                FolioCONAGUA = p.FolioCONAGUA,
                RFCUsuarioContribuyente = p.RFCUsuarioContribuyente,
                NombreUsuarioContribuyente = p.NombreUsuarioContribuyente,
                Titulo = p.Titulo,
                Anexo = p.Anexo,
                Estado = p.Estado,
                Municipio = p.Municipio,
                TipoCuerpoReceptor = p.TipoCuerpoReceptor,
                UsoCuerpoReceptor = p.UsoCuerpoReceptor,
                Norma = p.Norma,
                FechaRecepcion = p.FechaRecepcion,
                FechaConformacion = p.FechaConformacion,
                FechaEmision = p.FechaEmision,
                FechaFirma = p.FechaFirma,
                EstaHabilitado = p.EstaHabilitado,
                HorasProceso = p.HorasProceso,
                TipoMuestra = p.TipoMuestra,
                Caudal = p.Caudal,
                Parametro = p.Parametro,
                Resultado = p.Resultado,
                Exento = p.Exento,
                NoAnalizado = p.NoAnalizado,
                Unidad = p.Unidad,
                LC_MC = p.LC_MC,
                Metodo = p.Metodo
            }).ToList();


            lOt.ForEach(f =>
            {
                f.Analisis = Analisis.Where(p => p.OrdenTrabajoID == f.OrdenTrabajoId)
                    .Select((q, i) =>
                    {
                        var analisisLista = new AnalisisLista();
                        analisisLista.AnaisisID = q.AnalisisID;
                        analisisLista.Firmado_ = q.Firmada;
                        analisisLista.Consecutivo_ =
                            $"{(q.OrdenTrabajo.Norma.RequiereCaudal ? "Promedio diario" : "Muestra puntual")} {i + 1}";

                        if (q.Muestras.FirstOrDefault(r => r.NivelID == 2) != null)
                        {
                            var muestraCompuestaFormacionFecha = q.Muestras.First(r => r.NivelID == 2)?.Fecha;
                            analisisLista.FechaRegistro_ = muestraCompuestaFormacionFecha != null
                                ? ((DateTime)muestraCompuestaFormacionFecha).ToString("dd/MM/yyyy")
                                : "";
                        }
                        else
                        {
                            analisisLista.FechaRegistro_ = "";
                        }

                        analisisLista.FirmaVisible_ = q.Muestras.Any(r => r.NivelID == 2 && (r.FlagCompuesta ?? false));
                        return analisisLista;
                    }).ToList();
            });
            lstOrdenes = lOt.Select(x => new Modelos.OrdenTrabajo()
            {
                NumeroInforme = x.Folio,
                NombreNorma = x.Norma,
                RFCCliente = x.RFCCliente,
                OrdenTrabajoID = x.OrdenTrabajoId,
                Anexo = x.Anexo,
                Titulo = x.Titulo,
                FechaEmision = Convert.ToDateTime(x.Registro),
                LaboratorioRegistraOT = x.NombreLaboratorio

            }).ToList();
            var lAnalisis = lOt.Select(p => p.Analisis).ToList();

        }
        /// <summary>
        /// Metodo que obtiene los filtros para realisar la consulta de una orden de trabajo
        /// </summary>
        /// <param name="OT">parametro de contrato BusquedaOT</param>
        /// <returns></returns>
        public FiltroOrdenTrabajo ObtenerFiltro(BusquedaOT OT)
        {


            var emisionFI_ = OT.EmisionFechaInicio;
            var emisionFF_ = OT.EmisionFechaFin;
            var registroFI_ = OT.RegistroFechaInicio;
            var registroFF_ = OT.RegistroFechaFin;
            var conformacionFI_ = OT.ConformacionFechaInicio;
            var conformacionFF_ = OT.ConformacionFechaFin;
            var emisionFI = emisionFI_;
            var emisionFF = emisionFF_;
            var registroFI = registroFI_;
            var registroFF = registroFF_;
            var conformacionFI = conformacionFI_;
            var conformacionFF = conformacionFF_;
            var estatusCancelacion = OT.EstatusCancelacion;

            var filter = new FiltroOrdenTrabajo
            {
                NumeroInforme = OT.NumeroInforme,

                Firma = OT.Firmados,
                RFC = OT.RFCCliente,
                Titulo = OT.Titulo,
                Anexo = OT.Anexo,
                EstatusCancelacion = estatusCancelacion,
                FechaEmisionInicio = emisionFI,
                FechaEmisionFin = emisionFF,
                FechaRegistroInicio = registroFI,
                FechaRegistroFin = registroFF,
                FechaConformacionInicio = conformacionFI,
                FechaConformacionFin = conformacionFF,
                CurrentPage = 1,
                PageSize = 1000000000,
                OrderBy = "FechaEmision",
                OrderByDirection = "ASC"
            };

            if (OT.EstatusCancelacion == 0) filter.EstatusCancelacion = 2;
            using (Siralab_Entities sdb = new Siralab_Entities())
            {
                if (OT.NombreNorma != null && OT.NombreNorma != "")
                {
                    var r = from q in sdb.Normas
                            where q.Nombre == OT.NombreNorma
                            select new
                            {
                                NormaID = q.NormaID
                            };

                    //filter.NormaID = 
                    string result = r.Select(x => x.NormaID).First().ToString();
                    filter.NormaID = Convert.ToInt32(result);

                }

                var j = from q in sdb.Usuarios
                        where q.Usuario1 == OT.RFCLaboratorio && q.UsuarioRolID == 1
                        select new { LaboratorioID = q.LaboratorioID };
                if (j != null)
                {
                    filter.LaboratorioID = Convert.ToInt32(j.ToList().First().LaboratorioID);
                }
            }
            return filter;

        }
        /// <summary>
        /// Funcion que genera el alta de una orden de trabajo
        /// </summary>
        /// <param name="IOT">parametro de contrato InsercionOT</param>
        /// <returns></returns>
        public Result<Modelos.OrdenTrabajo> AltaOT(InsercionOT IOT)
        {
            Result<Modelos.OrdenTrabajo> RESULT = new Result<Modelos.OrdenTrabajo>();
            try
            {

                Modelos.OrdenTrabajo resOT = GuardarAnalisis(IOT);
                if (resOT.OrdenTrabajoID != null && resOT.OrdenTrabajoID != 0)
                {
                    RESULT.ListaResultado = new List<Modelos.OrdenTrabajo>();
                    RESULT.Executenonquery = true;
                    RESULT.ListaResultado.Add(resOT);
                }
                else
                {
                    RESULT.Executenonquery = false;
                    RESULT.Error = new ErrorDetalle("Ocurrio un error al momento de cargar la orden", "0002");
                }


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("|"))
                {
                    if (ex.Message.Split('|').ToList().Where(x => x != "").Count() > 1)
                    {
                        RESULT.LstErrores = new List<ErrorDetalle>();
                        RESULT.LstErrores = ex.Message.Split('|').ToList().Where(x => x != "").Select(x => new ErrorDetalle(x, "5560")).ToList();
                    }
                    else
                    {
                        RESULT.Error = new ErrorDetalle(ex.Message.Split('|').First(), ex.HResult.ToString());
                    }
                }
                else
                {
                    RESULT.Error = new ErrorDetalle(ex.Message, ex.HResult.ToString());
                }
            }
            return RESULT;

        }
        private Modelos.OrdenTrabajo GuardarAnalisis(InsercionOT IOT)
        {
            try
            {
                var sirficContext = new CNA_SIRFICEntities();
                var SiralabContext = new Siralab_Entities();
                var ordentrabajoBl = new OrdenTrabajo_BL();
                var ordentrabajo = new OrdenTrabajoCapturaArgs();
                var nBl = new Norma_BL();

                var contrib = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                               where clie.rfc == IOT.RFC
                               select new { clie.ID_CONTRIBUYENTE, clie.nombre, clie.apellidoPaterno, clie.apellidoMaterno, clie.rfc, clie.curp, clie.email, clie.ID_APROVECHAMIENTO }).Distinct().ToList();
                if (contrib != null && contrib.Count() > 0)
                {
                    int ClienteID = Convert.ToInt32(contrib.First().ID_CONTRIBUYENTE);
                    var consesiones = from conse in SiralabContext.Consecions
                                      where conse.ClienteID == ClienteID && conse.Titulo == IOT.Titulo
                                      select new { ConsecionID = conse.ConsecionID, ClienteID = conse.ClienteID };
                    if (consesiones != null && consesiones.Count() > 0)
                    {
                        var Anexo = from conse in consesiones
                                    join anexo in SiralabContext.Anexoes on conse.ConsecionID equals anexo.ConsecionID
                                    where anexo.Anexo1 == IOT.Anexo
                                    select new { AnexoID = anexo.AnexoID, ConsecionID = anexo.ConsecionID };
                        string IsError = "";
                        if (Anexo != null && Anexo.Count() > 0)
                        {


                            int NormaID = 0;
                            int CuerpoReceptorID = 0;
                            int UsoAguaID = 0;


                            ExisteNorma(ref IsError, ref NormaID, IOT.NombreNorma);
                            if (IsError == "")
                            {
                                // validamos si por omision se mete null o vacio se agrega en ambos el N/A  a CuerpoReceptor y UsoAgua
                                if (IOT.CuerpoReceptor==null&& IOT.UsoAgua==null) {

                                    IOT.CuerpoReceptor = "N/A";
                                    IOT.UsoAgua = "N/A";

                                }
                                ExisteCuerporeceptor(ref IsError, ref CuerpoReceptorID, IOT.CuerpoReceptor);
                                ExisteUsoAgua(ref IsError, ref UsoAguaID, IOT.UsoAgua);
                                ValidarFecha(ref IsError, IOT.HorasProceso, IOT.FechaEmision);
                                ValidaRelacionNormaCuerpoUsoAguaID(ref IsError, NormaID, CuerpoReceptorID, UsoAguaID);
                                int? LaboratorioID = SiralabContext.Usuarios.ToList().Where(x => x.Usuario1 == IOT.RFCLaboratorio && x.UsuarioRolID == 1).Select(x => x.LaboratorioID).First();
                                var nom = nBl.Obtener(NormaID);
                                if (nom.RequiereCaudal && IOT.HorasProceso == 0)
                                {
                                    throw new ApplicationException("Favor de ingresar la hora de proceso");
                                }
                                ordentrabajo.ClienteID = Convert.ToInt32(contrib.First().ID_CONTRIBUYENTE);
                                ordentrabajo.NormaID = NormaID;
                                ordentrabajo.EstatusID = 1;
                                ordentrabajo.FechaEmision = IOT.FechaEmision;
                                ordentrabajo.NumeroInforme = IOT.NumeroInforme;
                                ordentrabajo.Folio = "";
                                ordentrabajo.Anio = DateTime.Now.Year;
                                ordentrabajo.Consecutivo = 0;
                                ordentrabajo.HorasProceso = nom.RequiereCaudal ? IOT.HorasProceso : 25;
                                ordentrabajo.LaboratorioID = LaboratorioID;
                                ordentrabajo.AnexoID = Anexo.First().AnexoID;
                                ordentrabajo.ConsecionID = Anexo.First().ConsecionID;
                                ordentrabajo.CuerpoReceptorID = CuerpoReceptorID;
                                ordentrabajo.UsoAguaID = UsoAguaID;
                                var fm = ordentrabajoBl.Calcula_FrecuenciaMuestreo(ordentrabajo.HorasProceso);
                                ordentrabajo.FrecuenciaMuestreoID = fm.FrecuenciaMuestreoID;
                                bool? EsFuenteAbastecimiento = !string.IsNullOrEmpty(IOT.Medidor) ? true : false;
                                /*FuenteAbastecimiento*/
                                if (EsFuenteAbastecimiento ?? false)
                                {
                                    ordentrabajo.FuenteAbastecimiento = EsFuenteAbastecimiento;
                                    int AnexoMedidorID = 0;
                                    ExisteAnexoMedidor(ref IsError, ref AnexoMedidorID, Anexo.First().AnexoID, IOT.Medidor);
                                    ordentrabajo.AnexoMedidorID = AnexoMedidorID;
                                }
                                else
                                {
                                    ordentrabajo.FuenteAbastecimiento = false;
                                    ordentrabajo.AnexoMedidorID = null;
                                }
                                if (IsError == "")
                                {
                                    ordentrabajoBl.Guarda(ordentrabajo);
                                    if (IOT.Evidencias.Count > 0)
                                    {
                                        if (ordentrabajoBl.OrdenTrabajo.OrdenTrabajoID != 0)
                                        {
                                            pobjOrdendeTrabajo = ordentrabajoBl.OrdenTrabajo.OrdenTrabajoID;

                                        }
                                        List<SIRALAB_DAL.Documento> lstDocumentos = IOT.Evidencias.Select(x => new SIRALAB_DAL.Documento() { Nombre = x.Nombre, BufferData = x.Archivo, TipoDocumento = x.TipoDocumento.ToString(), TipoDocumentoID = Convert.ToInt32(x.TipoDocumento), Extension = x.Extension, TipoMime = x.Mime }).ToList();
                                        GuardarEvidencias(lstDocumentos);
                                    }


                                    return new Modelos.OrdenTrabajo() { OrdenTrabajoID = ordentrabajoBl.OrdenTrabajo.OrdenTrabajoID, NumeroInforme = ordentrabajo.NumeroInforme };
                                }
                                else
                                {
                                    throw new ApplicationException(IsError);
                                }
                            }
                            else
                            {
                                throw new ApplicationException(IsError);
                            }
                        }
                        else
                        {
                            throw new ApplicationException("Anexo no es valido: " + IOT.Anexo);
                        }


                    }
                    else
                    {
                        throw new ApplicationException("El titulo no se encuentra registrado: " + IOT.Titulo);
                    }
                }
                else
                {
                    throw new ApplicationException("No se encontro el contribuyente: " + IOT.RFC);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        private void ValidarFecha(ref string IsError, int HorasDescarga, DateTime FechaXValidarCadena)
        {
            DateTime FechaActual = DateTime.Now;
            if (FechaXValidarCadena.Date <= FechaActual.Date && FechaXValidarCadena.Year >= 2014)
            {

            }
            else
            {
                IsError = IsError + "La fecha debe de ser menero o igual a " + DateTime.Now.ToShortDateString() + "|";
            }
            if (HorasDescarga > 24 || HorasDescarga < 0)
            {
                IsError = IsError + "La la hora no debe ser mayor que 24 ni menor que 1|";
            }

        }
        private void ExisteNorma(ref string IsError, ref int NormaID, string Norma)
        {
            try
            {
                var sirficContext = new CNA_SIRFICEntities();
                var SiralabContext = new Siralab_Entities();
                var q = SiralabContext.Normas.ToList().Where(x => x.Nombre == Norma);
                if (q != null)
                {
                    IsError = "";
                    NormaID = SiralabContext.Normas.ToList().Where(x => x.Nombre == Norma).Select(x => x.NormaID).First();

                }
                else
                {
                    IsError = IsError + "Norma no fue encontrada: " + Norma + "|";

                }

            }
            catch (Exception ex)
            {
                IsError = IsError + "Norma no fue encontrada: " + Norma + "|";

            }

        }
        private void ExisteCuerporeceptor(ref string IsError, ref int CuerpoReceptorID, string CuerpoReceptor)
        {
            try
            {
                var sirficContext = new CNA_SIRFICEntities();
                var SiralabContext = new Siralab_Entities();
                var q = SiralabContext.CuerpoReceptors.Where(x => x.Nombre == CuerpoReceptor).First();
                if (q != null)
                {
                    IsError = "";
                    CuerpoReceptorID = SiralabContext.CuerpoReceptors.Where(x => x.Nombre == CuerpoReceptor).First().CuerpoReceptorID;

                }
                else
                {
                    IsError = IsError + "Cuerpo receptor no fue encontrado: " + CuerpoReceptor + "|";

                }

            }
            catch (Exception ex)
            {
                IsError = IsError + "Cuerpo receptor no fue encontrado: " + CuerpoReceptor + "|";

            }
        }
        private void ExisteUsoAgua(ref string IsError, ref int UsoAguaID, string UsoAgua)
        {
            try
            {
                var sirficContext = new CNA_SIRFICEntities();
                var SiralabContext = new Siralab_Entities();
                var q = SiralabContext.CatUsoAguas.Where(x => x.Nombre == UsoAgua).First();
                if (q != null)
                {
                    UsoAguaID = SiralabContext.CatUsoAguas.Where(x => x.Nombre == UsoAgua).First().UsoAguaID;
                }
                else
                {
                    IsError = IsError + "Uso del agua no fue encontrado: " + UsoAgua + "|";

                }

            }
            catch (Exception ex)
            {
                IsError = IsError + "Uso del agua no fue encontrado: " + UsoAgua + "|";

            }
        }
        private void ExisteAnexoMedidor(ref string IsError, ref int AnexoMedidorID, int AnexoID, string Medidor)
        {
            try
            {
                var SiralabContext = new Siralab_Entities();
                var q = SiralabContext.CatUsoAguas.Where(x => x.Nombre == Medidor).First();
                if (q != null)
                {
                    IsError = "";
                    AnexoMedidorID = SiralabContext.AnexoMedidors.Where(x => x.AnexoID == AnexoID && x.numeroMedidor == Medidor).First().AnexoMedidorID;

                }
                else
                {
                    IsError = IsError + "Medidor no fue encontrado: " + Medidor + "|";

                }

            }
            catch (Exception ex)
            {
                IsError = IsError + "Medidor no fue encontrado: " + Medidor + "|";

            }
        }
        private void ValidaRelacionNormaCuerpoUsoAguaID(ref string IsError, int NormaID, int CuerpoReceptorID, int usoAguaID)
        {
            var SiralabContext = new Siralab_Entities();
            var rel = from nomconf in SiralabContext.NormaConfiguracions
                      where nomconf.NormaID == NormaID && nomconf.CuerpoReceptorID == CuerpoReceptorID && nomconf.UsoAguaID == usoAguaID
                      select new { NormaID, CuerpoReceptorID, usoAguaID };
            string nomNorma = SiralabContext.Normas.ToList().Where(x => x.NormaID == NormaID).Select(x => x.Nombre).First();
            if (rel == null || rel.Count() == 0)
            {
                IsError = IsError + "Los parametros recibidos deacuerdo a la norma no concuerdan con el estandar establecido.  Norma: " + nomNorma + "|";
            }
        }
        public bool ValidaToken(ref string IsError, string RFCLaboratorio, string Token)
        {
            try
            {
                bool isValido = true;
                var SiralabContext = new Siralab_Entities();

                var rel = from la in SiralabContext.LaboratorioAcreditacions

                          select la;
                return isValido;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }

        }



        #region Subida de Archivos



        #region Para la Subida de Archivos
        public List<ResultOperacionArchivo> GuardarEvidencias(List<SIRALAB_DAL.Documento> listaEvidencia)
        {

            List<ResultOperacionArchivo> listaResultados = new List<ResultOperacionArchivo>();

            List<SIRALAB_DAL.Documento> listaEvidenciaAdd;

            Int16 pobjInt;




            if (listaEvidencia != null && listaEvidencia.Count > 0)
            {
                //listaEvidenciaAdd = listaEvidencia.Except(listaEvidencia, rel => rel.DocumentoID).ToList();
                // listaEvidenciaAdd = listaEvidencia.Intersect(listaEvidencia, new EvidenciaEqualityComparer()).ToList();
                foreach (SIRALAB_DAL.Documento evidenciaInserta in listaEvidencia)
                {
                    ResultOperacionArchivo result = new ResultOperacionArchivo();
                    result.evidencia = evidenciaInserta;
                    try
                    {
                        tipo = TipoDocumento(evidenciaInserta.TipoDocumento, ref pathFolder);
                        evidenciaInserta.Nombre = pathFolder + "\\" + Path.GetFileName(evidenciaInserta.Nombre);

                        pobjInt = InsertFile(evidenciaInserta.Nombre, evidenciaInserta.BufferData);
                        // listaEvidencia.Add(evidenciaInserta);
                        GuardarDocumentos(listaEvidencia);
                        result.comentarios = "Insertado correctamente";
                    }
                    catch (Exception ex)
                    {
                        result.codeError = -1;
                        result.comentarios = ex.Message;
                    }
                    finally
                    {
                        listaResultados.Add(result);
                    }

                }

            }


            return listaResultados;
        }


        public static DocumentoEntidad GetOrdenTrabajo<T>(T claseAccion)
        {
            DocumentoEntidad docEntidad = new DocumentoEntidad();
            string tipo = claseAccion.GetType().BaseType.Name;
            int id = 0;

            try
            {
                if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
                {
                    Muestra_BL mtBL = new Muestra_BL();

                    SIRALAB_DAL.Muestra muestra = (claseAccion as SIRALAB_DAL.Muestra);
                    id = (int)muestra.MuestraID;

                    SIRALAB_DAL.Muestra muestraBusqueda = mtBL.Obten_Muestra(id);
                    if (muestraBusqueda != null)
                    {
                        docEntidad.OrdenTrabajoID = muestraBusqueda.Analisi.OrdenTrabajo.OrdenTrabajoID;
                        docEntidad.AnalisisID = muestraBusqueda.Analisi.AnalisisID;
                        docEntidad.MuestraID = muestraBusqueda.MuestraID;
                    }
                }
                else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
                {
                    Analisis_BL anlsBL = new Analisis_BL();

                    SIRALAB_DAL.Analisi analisis = (claseAccion as SIRALAB_DAL.Analisi);
                    id = (int)analisis.AnalisisID;

                    SIRALAB_DAL.Analisi analisisBusqueda = anlsBL.Obten_Analisis(id);

                    if (analisisBusqueda != null)
                    {
                        docEntidad.OrdenTrabajoID = analisisBusqueda.OrdenTrabajoID;
                        docEntidad.AnalisisID = analisisBusqueda.AnalisisID;
                        docEntidad.MuestraID = null;
                    }
                }
                else
                {
                    docEntidad = new DocumentoEntidad
                    {
                        AnalisisID = 0,
                        MuestraID = 0,
                        OrdenTrabajoID = 0
                    };
                }
            }
            catch (Exception ex)
            {

            }

            return docEntidad;
        }




        public int GetIdObject(int IdOrdenDocumento)
        {
            int id = 0;
            // string tipo = claseAccion.GetType().BaseType.Name;
            var contexto = new Siralab_Entities();

            if (tipo == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
            {

                var Resultado = contexto.OrdenTrabajoes.Select(x => x.OrdenTrabajoID == IdOrdenDocumento).First();
                id = Convert.ToInt32(Resultado);

            }
            else if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
            {

                var Resultado = contexto.Muestras.Select(x => x.MuestraID == IdOrdenDocumento).First();

                id = Convert.ToInt32(Resultado);

            }
            else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
            {
                Analisis_BL anlsBL = new Analisis_BL();

                SIRALAB_DAL.Analisi analisisBusqueda = anlsBL.Obten_Analisis(IdOrdenDocumento);
                id = analisisBusqueda.AnalisisID;
            }

            return id;
        }




        public string TipoDocumento(string NombreDocumento, ref string Path)
        {
            ApplicationSettings aS = new ApplicationSettings();
            string pathFinal = string.Empty;
            string pathEvidencia = aS.NombreCarpetaEvidencia;
            string PathOrdenTrabajo = aS.PathOrdenTrabajo;
            string PathMuestra = aS.PathMuestra;
            string PathAnalisis = aS.PathAnalisis;

            string tipo = string.Empty;
            CatTipoDocumentoAnexo_BL CatAnexo = new CatTipoDocumentoAnexo_BL();
            CatTipoDocumentoMuestra_BL CatMuestra = new CatTipoDocumentoMuestra_BL();
            Analisi o = new Analisi();

            SIRALAB_DAL.OrdenTrabajo p = new SIRALAB_DAL.OrdenTrabajo();

            if (!string.IsNullOrEmpty(NombreDocumento))
            {
                var VarTipoDocumento = CatAnexo.ObtenerTodos().Where(cat => cat.Nombre == NombreDocumento);
                tipo = "OrdenTrabajo";
                if (tipo == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
                {
                    Path = string.Empty;
                    pathFinal += PathOrdenTrabajo;
                    pathFinal += "\\" + pobjOrdendeTrabajo.ToString().PadLeft(6, '0');
                    pathFinal += pathEvidencia;
                    Path = pathFinal;
                }
            }
            else if (!string.IsNullOrEmpty(NombreDocumento))
            {
                var VarTipoDocumento = CatMuestra.ObtenerTodos().Where(cat => cat.Nombre == NombreDocumento);
                tipo = "Muestra";
                if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
                {
                    Path = string.Empty;
                    //  SIRALAB_DAL.Muestra m = (claseAccion as SIRALAB_DAL.Muestra);
                    //string mPath = (m.NivelID == 2) ? "Compuesta" : "Simple";
                    /*
                     * hay que obtener el nivel y referenciarlo para los directorios de los archivos
                     */
                    pathFinal += PathOrdenTrabajo;
                    //     pathFinal += "\\" + ot.OrdenTrabajoID.ToString().PadLeft(6, '0');
                    pathFinal += PathAnalisis;
                    //   pathFinal += "\\" + ot.AnalisisID.ToString().PadLeft(6, '0');
                    pathFinal += PathMuestra;
                    // pathFinal += "\\" + mPath;
                    //pathFinal += "\\" + id.ToString().PadLeft(6, '0');

                    pathFinal += pathEvidencia;
                    Path = pathFinal;
                }

            }
            else if (!string.IsNullOrEmpty(NombreDocumento))
            {
                o.DocumentoAnalisis.Select(cat => cat.Documento.Nombre == NombreDocumento);
                tipo = "Analisi";
                if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
                {
                    Path = string.Empty;
                    pathFinal += PathOrdenTrabajo;
                    //  pathFinal += "\\" + ot.OrdenTrabajoID.ToString().PadLeft(6, '0');
                    pathFinal += PathAnalisis;
                    //pathFinal += "\\" + id.ToString().PadLeft(6, '0');
                    pathFinal += pathEvidencia;
                    Path = pathFinal;
                }
            }



            return tipo;
        }






        public void GuardarDocumentos(List<SIRALAB_DAL.Documento> doctosActualizados)
        {
            int id = 0;

            //List<SIRALAB_DAL.Documento> listaEvidenciasGuardadas = GetEvidencias(entidad);

            List<SIRALAB_DAL.Documento> listaDocumentosAdd = null;
            List<SIRALAB_DAL.Documento> listaDocumentosUpd = null;
            List<SIRALAB_DAL.Documento> listaDocumentosDel = null;
            List<SIRALAB_DAL.Documento> listaDocumentosActual = new List<SIRALAB_DAL.Documento>();
            using (var db = new Siralab_Entities())
            {
                try
                {

                    if (doctosActualizados != null && doctosActualizados.Count > 0)
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {

                            #region ORDENTRABAJO
                            //foreach (SIRALAB_DAL.Documento D in doctosActualizados)
                            //{
                            //    tipo = TipoDocumento(D.TipoDocumento, ref pathFolder);
                            //}
                            if (tipo == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
                            {
                                SIRALAB_DAL.OrdenTrabajo ordenTrabajo = db.OrdenTrabajoes.Where(a => a.OrdenTrabajoID == pobjOrdendeTrabajo).FirstOrDefault();
                                listaDocumentosActual = ordenTrabajo.DocumentoAnexoes.Select(a => a.Documento).ToList();

                                if (ordenTrabajo != null)
                                {
                                    listaDocumentosAdd = doctosActualizados.Where(a => a.DocumentoID == 0).ToList();
                                    // listaDocumentosUpd = doctosActualizados.Intersect(listaDocumentosActual).ToList();
                                    // listaDocumentosDel = listaDocumentosActual.Except(doctosActualizados, rel => rel.DocumentoID).ToList();

                                    #region DocumentosNuevos
                                    if (listaDocumentosAdd != null && listaDocumentosAdd.Count > 0)
                                    {

                                        int DocumentoIDMax = (db.Documentoes.Count() == 0) ? 0 : db.Documentoes.Max(a => a.DocumentoID);
                                        int DocumentoAnexoIDMax = (db.DocumentoAnexoes.Count() == 0) ? 0 : db.DocumentoAnexoes.Max(a => a.DocumentoID);

                                        foreach (SIRALAB_DAL.Documento documentoInserta in listaDocumentosAdd)
                                        {
                                            SIRALAB_DAL.Documento documentoNueva = new SIRALAB_DAL.Documento();

                                            DocumentoIDMax += 1;
                                            DocumentoAnexoIDMax += 1;

                                            documentoNueva.DocumentoID = DocumentoIDMax;
                                            documentoNueva.Nombre = documentoInserta.Nombre;
                                            documentoNueva.Ruta = documentoInserta.Ruta ?? "ruta";
                                            documentoNueva.TipoMime = documentoInserta.TipoMime;
                                            documentoNueva.Extension = documentoInserta.Extension;
                                            documentoNueva.FechaRegistro = DateTime.Now;

                                            db.Documentoes.Add(documentoNueva);

                                            SIRALAB_DAL.DocumentoAnexo documentoAnexoNueva = new SIRALAB_DAL.DocumentoAnexo();

                                            documentoAnexoNueva.DocumentoAnexoID = DocumentoAnexoIDMax;
                                            documentoAnexoNueva.DocumentoID = documentoNueva.DocumentoID;
                                            documentoAnexoNueva.TipoDocumentoAnexoID = (int)documentoInserta.TipoDocumentoID;
                                            documentoAnexoNueva.OrdenTrabajoID = ordenTrabajo.OrdenTrabajoID;

                                            db.DocumentoAnexoes.Add(documentoAnexoNueva);

                                        }
                                    }
                                    #endregion DocumentosNuevos

                                }
                            }//

                            #endregion ORDENTRABAJO

                            #region MUESTRA
                            else if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
                            {
                                SIRALAB_DAL.Muestra muestra = db.Muestras.Where(a => a.MuestraID == id).FirstOrDefault();

                                listaDocumentosActual = muestra.DocumentoMuestras.Select(a => a.Documento).ToList();

                                if (muestra != null)
                                {
                                    listaDocumentosAdd = doctosActualizados.Where(a => a.DocumentoID == 0).ToList();
                                    listaDocumentosUpd = doctosActualizados.Intersect(listaDocumentosActual).ToList();
                                    listaDocumentosDel = listaDocumentosActual.Except(doctosActualizados, rel => rel.DocumentoID).ToList();


                                    #region DocumentosNuevos
                                    if (listaDocumentosAdd != null && listaDocumentosAdd.Count > 0)
                                    {

                                        int DocumentoIDMax = (db.Documentoes.Count() == 0) ? 0 : db.Documentoes.Max(a => a.DocumentoID);
                                        int DocumentoMuestraIDMax = (db.DocumentoMuestras.Count() == 0) ? 0 : db.DocumentoMuestras.Max(a => a.DocumentoID);

                                        foreach (SIRALAB_DAL.Documento documentoInserta in listaDocumentosAdd)
                                        {
                                            SIRALAB_DAL.Documento documentoNueva = new SIRALAB_DAL.Documento();

                                            DocumentoIDMax += 1;
                                            DocumentoMuestraIDMax += 1;

                                            documentoNueva.DocumentoID = DocumentoIDMax;
                                            documentoNueva.Nombre = documentoInserta.Nombre;
                                            documentoNueva.Ruta = documentoInserta.Ruta ?? "ruta";
                                            documentoNueva.TipoMime = documentoInserta.TipoMime;
                                            documentoNueva.Extension = documentoInserta.Extension;
                                            documentoNueva.FechaRegistro = DateTime.Now;

                                            db.Documentoes.Add(documentoNueva);

                                            SIRALAB_DAL.DocumentoMuestra documentoAnexoNueva = new SIRALAB_DAL.DocumentoMuestra();

                                            documentoAnexoNueva.DocumentoMuestraID = DocumentoMuestraIDMax;
                                            documentoAnexoNueva.DocumentoID = documentoNueva.DocumentoID;
                                            documentoAnexoNueva.TipoDocumentoMuestraID = (int)documentoInserta.TipoDocumentoID;
                                            documentoAnexoNueva.MuestraID = muestra.MuestraID;

                                            db.DocumentoMuestras.Add(documentoAnexoNueva);

                                        }
                                    }
                                    #endregion DocumentosNuevos

                                }

                            }

                            #endregion MUESTRA

                            #region ANALISIS
                            else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
                            {
                                SIRALAB_DAL.Analisi analisis = db.Analisis.Where(a => a.AnalisisID == id).FirstOrDefault();

                                listaDocumentosActual = analisis.DocumentoAnalisis.Select(a => a.Documento).ToList();

                                if (analisis != null)
                                {
                                    listaDocumentosAdd = doctosActualizados.Where(a => a.DocumentoID == 0).ToList();
                                    listaDocumentosDel = listaDocumentosActual.Except(doctosActualizados, rel => rel.DocumentoID).ToList();



                                    #region DocumentosNuevos
                                    if (listaDocumentosAdd != null && listaDocumentosAdd.Count > 0)
                                    {

                                        int DocumentoIDMax = (db.Documentoes.Count() == 0) ? 0 : db.Documentoes.Max(a => a.DocumentoID);
                                        int DocumentoAnexoIDMax = (db.DocumentoAnalisis.Count() == 0) ? 0 : db.DocumentoAnalisis.Max(a => a.DocumentoID);

                                        foreach (SIRALAB_DAL.Documento documentoInserta in listaDocumentosAdd)
                                        {
                                            SIRALAB_DAL.Documento documentoNueva = new SIRALAB_DAL.Documento();

                                            DocumentoIDMax += 1;
                                            DocumentoAnexoIDMax += 1;

                                            documentoNueva.DocumentoID = DocumentoIDMax;
                                            documentoNueva.Nombre = Path.GetFileName(documentoInserta.Nombre);// documentoInserta.Nombre;
                                            documentoNueva.Ruta = documentoInserta.Nombre ?? "ruta";
                                            documentoNueva.TipoMime = documentoInserta.TipoMime;
                                            documentoNueva.Extension = documentoInserta.Extension;
                                            documentoNueva.FechaRegistro = DateTime.Now;

                                            db.Documentoes.Add(documentoNueva);

                                            SIRALAB_DAL.DocumentoAnalisi documentoAnexoNueva = new SIRALAB_DAL.DocumentoAnalisi();

                                            documentoAnexoNueva.DocumentoAnalisisID = DocumentoAnexoIDMax;
                                            documentoAnexoNueva.DocumentoID = documentoNueva.DocumentoID;
                                            documentoAnexoNueva.AnalisisID = analisis.AnalisisID;

                                            db.DocumentoAnalisis.Add(documentoAnexoNueva);

                                        }
                                    }
                                    #endregion DocumentosNuevos     



                                }

                            }

                            #endregion ANALISIS

                            db.SaveChanges();
                            transaction.Complete();
                        }

                        db.SaveChanges();
                    }//

                }
                catch (DbEntityValidationException ex)
                {
                    //throw ex;
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                    //throw ex;
                }
            }
        }


        #endregion


        public Int16 InsertFile(string fileName, byte[] bufferData)
        {
            string ruta = "";
            try
            {
                ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;

                FileInfo file = new System.IO.FileInfo(ruta);

                if (!file.Exists)
                {

                    DirectoryInfo dir = new DirectoryInfo(file.DirectoryName);
                    if (!dir.Exists)
                        dir.Create();
                    FileStream stream = file.Create();

                    stream.Write(bufferData, 0, bufferData.Length);

                    stream.Close();

                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("No se pudo guardar el archivo.  " + ex.Message);
            }
        }


        public byte[] GetFile(string fileName)
        {

            byte[] buffer = null;
            string ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;

            FileInfo existingFile = null;
            try
            {
                existingFile = new FileInfo(ruta);
                if (existingFile.Exists)
                {
                    buffer = new byte[existingFile.Length];
                    FileStream streamFile = new FileStream(ruta, FileMode.Open);
                    streamFile.Read(buffer, 0, buffer.Length);
                    streamFile.Close();
                    return buffer;
                }
                throw new ArgumentException("No existe el archivo que solicitado");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("No se solicitar el archivo.  " + ex.Message);
            }
        }


        public int DeleteFile(string fileName)
        {
            string ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;
            FileInfo existingFile = null;
            try
            {
                existingFile = new FileInfo(ruta);
                if (existingFile.Exists)
                {
                    existingFile.Delete();
                    return 0;
                }
                else
                {
                    throw new ArgumentException("No se puedo eliminar el archivo");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("No se eliminar el archivo.  " + ex.Message);
            }
        }


        public bool IfFileExists(string fileName)
        {
            string ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;
            FileInfo existingFile = null;
            existingFile = new FileInfo(ruta);
            if (existingFile.Exists)
                return true;
            else
                return false;
        }

        #endregion


    }
}