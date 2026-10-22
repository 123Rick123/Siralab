using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Transactions;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.negocio.Documentos;
using DocumentoAnexo = SIRALAB_DAL.DocumentoAnexo;
using IsolationLevel = System.Data.IsolationLevel;

namespace SIRALAB_BL
{
    public class Analisis_BL
    {
        public Siralab_Entities AnalisisContext { get; private set; }
        public Analisi Analisis { get; private set; }

        Repository<Analisi> Repositorio;

        public Analisis_BL()
        {
            AnalisisContext = new Siralab_Entities();
            Repositorio = new Repository<Analisi>(AnalisisContext);
        }

        public List<Analisi> Lista_Analisis(int OrdenTrabajoID)
        {
            var listaanalisis = Repositorio.Find(x => x.OrdenTrabajoID == OrdenTrabajoID).ToList();
            listaanalisis = listaanalisis.OrderBy(x => x.AnalisisID).ToList();
            return listaanalisis;
        }
        
        public List<Analisi> Lista_Analisis()
        {
            var listaanalisis = Repositorio.GetAll().Select(x => x).Distinct().ToList();
            return listaanalisis; //
        }

        public List<Analisi> Lista_Analisis_Analisis(int AnalisisID)
        {
            var listaanalisis = Repositorio.Find(x => x.AnalisisID == AnalisisID).ToList();
            return listaanalisis;
        }

        public Analisi Obten_Analisis_Orden(int OrdenTrabajoID)
        {
            var analisis = Repositorio.Find(x => x.OrdenTrabajoID == OrdenTrabajoID).FirstOrDefault();
            return analisis;
        }

        public Analisi Obten_Analisis(int AnalisisID)
        {
            var analisis = Repositorio.Find(x => x.AnalisisID == AnalisisID).FirstOrDefault();
            this.Analisis = analisis;
            return analisis;
        }

        public List<Analisi> Lista_Analisis_Padre(int AnalisisID)
        {
            List<Analisi> Lista_Analisis = new List<Analisi>();
            Lista_Analisis = Repositorio.Find(x => x.AnalisisPadreID == AnalisisID).ToList();
            return Lista_Analisis;
        }

        public bool ActualizaExtemporaneo(int analisisId)
        {
            try
            {
                var analisisReg = Repositorio.Single(x => x.AnalisisID == analisisId);
                if (analisisReg != null)
                {
                    analisisReg.Procesado = true;
                    Repositorio.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool ActualizarAnalisisAFirmado(int AnalisisID)
        {
            try
            {
                Analisi AnalisisReg = Repositorio.Single(x => x.AnalisisID == AnalisisID);
                if (AnalisisReg != null)
                {
                    AnalisisReg.FechaFirma = DateTime.Now;
                    AnalisisReg.Firmada = true;

                    try
                    {
                        AnalisisReg.Procesado =
                            Analisis_Procesado(
                                (DateTime) AnalisisReg.Muestras.Where(p => p.NivelID == 1).FirstOrDefault().Fecha,
                                DateTime.Now);
                    }
                    catch
                    {
                        AnalisisReg.Procesado = true;
                    }

                    Repositorio.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public string[] BuscaLaboratorio(string Laboratorio)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            Laboratorio = string.Join("%", Laboratorio.Split(' '));
            var laboratorios = sbd.sp_BuscaLaboratorio(Laboratorio).Select(p => p.Nombre).ToArray();
            return laboratorios.ToArray();
        }
        
        public List<Laboratorio> BuscaLaboratorio()
        {
            Siralab_Entities sbd = new Siralab_Entities();
            List<Laboratorio> list = new List<Laboratorio>();

            list = sbd.sp_BuscaLaboratorio("").ToList();
            return list;
        }
        

        //TODO -> Revisar EstatusID ->
        public bool ActualizaEstatus(int AnalisisID, int EstatusID)
        {
            Analisi analisis = new Analisi();
            try
            {
                AnalisisContext.usp_ActualizaAnalisis(AnalisisID, EstatusID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //TODO -> Cancelar listado de análisis:
        public bool CancelarListado(List<SolicitudCancelacionParam> listado)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            var table = listado.ConvertToDataTable();
            var paramName = "@Listado";
            var procedureName = "usp_CancelaAnalisis";
            var typeName = "SolicitudCancelacion";
            try
            {
                //// create parameter 
                System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter(paramName, table);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = typeName;

                //// execute sp sql 
                string sql = $"EXEC {procedureName} {paramName};";

                //// execute sql 
                sbd.Database.ExecuteSqlCommand(sql, parameter);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ActualizaObservaciones(int AnalisisID, string Observaciones)
        {
            Analisi analisis = new Analisi();
            try
            {

                AnalisisContext.usp_ActualizaAnalisisObs(AnalisisID, Observaciones);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Analisi Guarda(AnalisisArgBase_BL args)
        {
            Analisi analisis = new Analisi();
            analisis.AnalisisPadreID = args.AnalisisPadreID;
            analisis.OrdenTrabajoID = args.OrdenTrabajoID;
            analisis.ConsecionID = args.ConsecionID;
            analisis.AnexoID = args.AnexoID;
            analisis.LaboratorioID = args.LaboratorioID;
            analisis.EstatusID = args.EstatusID;
            analisis.Firmada = args.Firmada;
            analisis.FechaFirma = args.FechaFirmada;
            analisis.Enviado = args.Enviada;
            analisis.Procesado = args.Procesado;
            analisis.FechaProceso = args.FechaProceso;
            analisis.FechaRegistro = args.FechaRegistro;
            analisis.NormaDQOCOT = args.NormaDQOCOT;
            try
            {
                if (args.AnaisisID != null) //Es una actualización
                {
                    analisis.AnalisisID = int.Parse(args.AnaisisID.ToString());
                    Repositorio.Edit(analisis);
                    Repositorio.SaveChanges();
                    this.Analisis = Repositorio.Single(x => x.AnalisisID == analisis.AnalisisID);

                }
                else
                {
                    analisis.AnalisisID = Repositorio.NextNewID(x => x.AnalisisID);
                    Repositorio.Add(analisis);
                    Repositorio.SaveChanges();

                    this.Analisis = Repositorio.Single(x => x.AnalisisID == analisis.AnalisisID);

                }

                return analisis = this.Analisis;
            }
            catch (Exception ex)
            {
                this.Analisis = null;
                return this.Analisis;
            }
        }

        public Analisi DuplicarAnalisi(AnalisisArgBase_BL args)
        {
            Analisi analisis = new Analisi();
            int analisisID = 0;
            var options = new TransactionOptions {IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted};
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                using (Siralab_Entities se = new Siralab_Entities())
                {
                    try
                    {
                        analisis.AnalisisID = analisisID = se.Analisis.Max(x => x.AnalisisID) + 1;
                        analisis.AnalisisPadreID = args.AnalisisPadreID;
                        analisis.OrdenTrabajoID = args.OrdenTrabajoID;
                        analisis.AnexoID = args.AnexoID;
                        analisis.LaboratorioID = args.LaboratorioID;
                        analisis.EstatusID = 3;
                        analisis.Firmada = false;
                        analisis.FechaFirma = null;
                        analisis.Enviado = false;
                        analisis.Procesado = false;
                        analisis.FechaProceso = null;
                        analisis.FechaRegistro = args.FechaRegistro;
                        analisis.Observaciones = args.Observaciones;
                        analisis.ConsecionID = args.ConsecionID;

                        se.Analisis.Add(analisis);
                        se.SaveChanges();

                        var muestras = se.Muestras.Where(p => p.AnalisisID == args.AnalisisPadreID);
                        foreach (var muestra in muestras)
                        {
                            Muestra muestraTemp = new Muestra();
                            muestraTemp.MuestraID = se.Muestras.Max(p => p.MuestraID) + 1;
                            muestraTemp.AnalisisID = analisis.AnalisisID;
                            muestraTemp.DireccionID = muestra.DireccionID;
                            muestraTemp.Numero = muestra.Numero;
                            muestraTemp.Descripcion = muestra.Descripcion;
                            muestraTemp.Identificacion = muestra.Identificacion;
                            muestraTemp.Caudal = muestra.Caudal;
                            muestraTemp.Latitud = muestra.Latitud;
                            muestraTemp.Longitud = muestra.Longitud;
                            muestraTemp.Datum = muestra.Datum;
                            muestraTemp.Fecha = muestra.Fecha;
                            muestraTemp.FechaRecepcion = muestra.FechaRecepcion;
                            muestraTemp.Cancelado = muestra.Cancelado;
                            muestraTemp.Observaciones = muestra.Observaciones;
                            muestraTemp.NivelID = muestra.NivelID;
                            muestraTemp.ValHojaCampo = muestra.ValHojaCampo;
                            muestraTemp.ValMuestraC = muestra.ValMuestraC;
                            muestraTemp.FlagCompuesta = muestra.FlagCompuesta;
                            
                            se.Muestras.Add(muestraTemp);
                            se.SaveChanges();

                            var valores = se.MuestraValors.Where(p => p.MuestraID == muestra.MuestraID);
                            foreach (var valor in valores)
                            {
                                MuestraValor valorTemp = new MuestraValor();
                                valorTemp.MuestraValorID = se.MuestraValors.Max(p => p.MuestraValorID) + 1;
                                valorTemp.MuestraID = muestraTemp.MuestraID;
                                valorTemp.ParametroID = valor.ParametroID;
                                valorTemp.ParametroMetodoID = valor.ParametroMetodoID;
                                valorTemp.LaboratorioID = valor.LaboratorioID;
                                valorTemp.OperacionID = valor.OperacionID;
                                valorTemp.Exenta = valor.Exenta;
                                valorTemp.Evalua = valor.Evalua;
                                valorTemp.Exento = valor.Exento;
                                valorTemp.NoAnalizado = valor.NoAnalizado;
                                valorTemp.Valor = valor.Valor;
                                valorTemp.ValorValidacion = valor.ValorValidacion;
                                valorTemp.AceptaValor = valor.AceptaValor;
                                valorTemp.LFD = valor.LFD;
                                valorTemp.LimiteInferior = valor.LimiteInferior;
                                valorTemp.LimiteSuperior = valor.LimiteSuperior;
                                valorTemp.LimiteInferiorLFD = valor.LimiteInferiorLFD;
                                valorTemp.LimiteSuperiorLFD = valor.LimiteSuperiorLFD;
                                valorTemp.LC_MC = valor.LC_MC;
                                valorTemp.GeneradoSP = valor.GeneradoSP;
                                valorTemp.Grupo = valor.Grupo;
                                valorTemp.Grupo = valor.Grupo;

                                se.MuestraValors.Add(valorTemp);
                                se.SaveChanges();
                            }

                            var signatarios = se.Muestreadors.Where(p => p.MuestraID == muestra.MuestraID && (p.Activo ?? false));
                            foreach (var signatario in signatarios)
                            {
                                Muestreador signatarioTemp = new Muestreador();
                                signatarioTemp.MuestreadorID = se.Muestreadors.Max(x => x.MuestreadorID) + 1;
                                signatarioTemp.MuestraID = muestraTemp.MuestraID;
                                signatarioTemp.LaboratorioSignatarioID = signatario.LaboratorioSignatarioID;
                                signatarioTemp.Activo = signatarioTemp.Activo;

                                se.Muestreadors.Add(signatarioTemp);
                                se.SaveChanges();
                            }
                        }

                        analisis = se.Analisis.FirstOrDefault(p => p.AnalisisID == analisisID);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return analisis;
        }

        public Analisi GuardaVw(AnalisisArgBase_BL args)
        {
            try
            {
                var qry = AnalisisContext.Analisis.Single(anl => anl.AnalisisID == args.AnaisisID);
                qry.Procesado = args.Procesado;
                Repositorio.Edit(qry);
                Repositorio.SaveChanges();

                return Analisis = qry;
            }
            catch (Exception ex)
            {
                this.Analisis = null;
                return this.Analisis;
            }
        }

        public void Eliminar(int AnalisisID)
        {
            Analisi an = new Analisi();
            an = Obten_Analisis(AnalisisID);

            List<Muestra> lista_muestra = new List<Muestra>();
            Muestra_BL muestra_bl = new Muestra_BL();

            lista_muestra = muestra_bl.Lista_Muestra(AnalisisID);

            foreach (var i in lista_muestra)
            {
                muestra_bl.EliminarMuestra(i.MuestraID);
            }

            Muestreador_BL muestreadorBl = new Muestreador_BL();
            


            //RFD 10/10/20 -> Se agrega para eliminar los archivos asociados a una análisis.
            DocumentoAnalisis_BL documentoAnalisisBl = new DocumentoAnalisis_BL();
            var listaDocumentosAnalisis = documentoAnalisisBl.FindAllById(AnalisisID);

            if (listaDocumentosAnalisis.Any())
            {
                //Recupera documentos anexos:
                var documentoAnexoBL = new DocumentoAnexo_BL();
                var documentosAnexosList = listaDocumentosAnalisis.Select(x => x.DocumentoID).Distinct().SelectMany(documentoId => documentoAnexoBL.FindAllByDocumentoID(documentoId)).ToList();
                documentosAnexosList = documentosAnexosList.Distinct().ToList();

                //Recupera documentos:
                var documentoBL = new Documento_BL();
                var documentosList = listaDocumentosAnalisis.Select(x => x.DocumentoID).Distinct().SelectMany(documentoId => documentoBL.FindAllByDocumentoID(documentoId)).ToList();
                documentosList = documentosList.Distinct().ToList();

                //Borrar documentoAnexo:
                if (documentosAnexosList.Any())
                    documentoAnexoBL.EliminarDocumentosAnexo(documentosAnexosList);


                //Borrar documentoAnalisis
                if (listaDocumentosAnalisis.Any())
                    documentoAnalisisBl.Eliminar(listaDocumentosAnalisis);

                //Borrar documento:
                if (listaDocumentosAnalisis.Any())
                    documentoBL.EliminarDocumentos(documentosList);
            }
            

            Repositorio.Delete(an);
        }

        public bool FaltaPago(int AnalisisID)
        {
            Analisi an = new Analisi();
            an = Obten_Analisis(AnalisisID);
            an.FaltaPago = !an.FaltaPago;

            Repositorio.SaveChanges();

            return !an.FaltaPago;
        }

        public bool Firmado()
        {
            AnalisisFirma_BL analisisFirmaBL = new AnalisisFirma_BL();
            if (analisisFirmaBL.IfAnalisisFirmado(Analisis.AnalisisID))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Esta función se utiliza bajo el campo de procesado en la tabla de análisis,
        /// el cuál muestra si el análisis debe ser tomado en cuenta para la declaración.
        /// En el caso de que el análisis no se deba tomar en cuenta por que se firmó 7 días
        /// después del cierre del periodo (trimestral).
        /// </summary>
        /// <returns>Retorna true si el análisis no debe ser procesado.</returns>
        public bool Analisis_Procesado(DateTime FechaAnalisis, DateTime FechaFirma)
        {
            /* Por el momento el análisis se verifica contra un trimestre */
            DateTime PeriodoIni = new DateTime();
            DateTime PeriodoFin = new DateTime();
            DateTime PeriodoFirmaIni = new DateTime();
            DateTime PeriodoFirmaFin = new DateTime();

            int pIni = Trimestre(FechaAnalisis, ref PeriodoIni, ref PeriodoFin);
            int pFin = Trimestre(FechaFirma, ref PeriodoFirmaIni, ref PeriodoFirmaFin);

            if (pIni == pFin)
            {
                return FechaAnalisis.Year < FechaFirma.Year;
                // return false;
            }
            else if (FechaFirma <= getFechaFinPeridoExtra(PeriodoFin))
            {
                return false;
            }

            /* Retorna true si el análisis no debe ser procesado */
            return true;
        }

        private int Trimestre(DateTime Fecha, ref DateTime PeriodoIni, ref DateTime PeriodoFin)
        {
            int anio = Fecha.Year;
            int dia = Fecha.Day;
            int mes = Fecha.Month;
            int periodo = 0;

            if (mes > 0 && mes <= 3)
            {
                PeriodoIni = new DateTime(anio, 1, 1);
                periodo = 1;
            }

            if (mes > 3 && mes <= 6)
            {
                PeriodoIni = new DateTime(anio, 4, 1);
                periodo = 2;
            }

            if (mes > 6 && mes <= 9)
            {
                PeriodoIni = new DateTime(anio, 7, 1);
                periodo = 3;
            }

            if (mes > 9 && mes <= 12)
            {
                PeriodoIni = new DateTime(anio, 10, 1);
                periodo = 4;
            }

            PeriodoFin = PeriodoIni.AddMonths(3).AddDays(-1);

            return periodo;
        }

        private DateTime UltimoDiaHabil(DateTime Fecha)
        {
            DateTime FechaRet = new DateTime();
            if (Fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                FechaRet = Fecha.AddDays(-2);
            }

            if (Fecha.DayOfWeek == DayOfWeek.Saturday)
            {
                FechaRet = Fecha.AddDays(-1);
            }

            return FechaRet;
        }

        private DateTime getFechaFinPeridoExtra(DateTime finPeriodoSinDiasHabiles)
        {
            var catDiaInhabilBL = new CatDiaInhabil_BL();
            ApplicationSettings appSetting = new ApplicationSettings();
            var diasExtra = appSetting.DiasInhabiles; 
            var fechaSuma = finPeriodoSinDiasHabiles;
            for (var i = 0; i < diasExtra; i++)
            {
                var fechaComparacion = fechaSuma.AddDays(i + 1);
                if (fechaComparacion.DayOfWeek == DayOfWeek.Saturday || fechaComparacion.DayOfWeek == DayOfWeek.Sunday)
                    diasExtra++;
            }

            var finPeriodoConDiasExtras = finPeriodoSinDiasHabiles.AddDays(diasExtra);

            //Recuperar los días inhábiles ->
            var diasInhabiles = catDiaInhabilBL.ObtenerTodos().Count(dia => dia.DiaInhabil != null
                              && dia.DiaInhabil > finPeriodoSinDiasHabiles
                              && dia.DiaInhabil <= finPeriodoConDiasExtras
                              && dia.DiaInhabil.GetValueOrDefault().DayOfWeek != DayOfWeek.Saturday
                              && dia.DiaInhabil.GetValueOrDefault().DayOfWeek != DayOfWeek.Sunday);
            var finPeriodo = finPeriodoConDiasExtras.AddDays(diasInhabiles);

            if (finPeriodo.DayOfWeek == DayOfWeek.Saturday)
                finPeriodo = finPeriodo.AddDays(2);
            if (finPeriodo.DayOfWeek == DayOfWeek.Sunday)
                finPeriodo = finPeriodo.AddDays(1);

            while (catDiaInhabilBL.ObtenerTodos().Select(dia => dia.DiaInhabil.GetValueOrDefault().Date)
                .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                .Contains(finPeriodo.Date)) //Si el fin del periodo también es inhabil se suma un día hasta que ya no sea feriado.
            {
                finPeriodo = finPeriodo.AddDays(1);
                if (finPeriodo.DayOfWeek == DayOfWeek.Saturday)
                    finPeriodo = finPeriodo.AddDays(2);
                if (finPeriodo.DayOfWeek == DayOfWeek.Sunday)
                    finPeriodo = finPeriodo.AddDays(1);
            }

            //lo pone a las 23:59:59
            finPeriodo = finPeriodo.AddHours(23).AddMinutes(59).AddSeconds(59);
            
            

            return finPeriodo;
        }

        private DateTime FechaLimite(DateTime FechaPeriodoFin)
        {
            DateTime Return = new DateTime();
            ApplicationSettings appSetting = new ApplicationSettings();
            Return = FechaPeriodoFin;
            int i = 0;
            int diasHabiles = appSetting.DiasInhabiles;

            while (i < diasHabiles)
            {
                if (Return.AddDays(1).DayOfWeek != DayOfWeek.Saturday &&
                    Return.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
                    i += 1;

                Return = Return.AddDays(1);
            }

            FechaPeriodoFin.AddHours(23);
            FechaPeriodoFin.AddMinutes(59);

            Return = Return.AddHours(23).AddMinutes(59);

            return Return;
        }

        public string AnalisisNumero(int AnalisisID)
        {
            return AnalisisContext.usp_ObtenerAnalisisEnumerado(AnalisisID).FirstOrDefault();
        }

        public bool AnalisisComplementario(int AnalisisID, int ordenTrabajoID)
        {
            Analisi analisis = new Analisi();
            var args = Repositorio.Single(x => x.AnalisisID == AnalisisID);
            int analisisID = 0;
            var options = new TransactionOptions {IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted};
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                using (Siralab_Entities se = new Siralab_Entities())
                {
                    try
                    {
                        analisis.AnalisisID = analisisID = se.Analisis.Max(x => x.AnalisisID) + 1;
                        analisis.AnalisisPadreID = AnalisisID;
                        analisis.OrdenTrabajoID = ordenTrabajoID;
                        analisis.AnexoID = args.AnexoID;
                        analisis.LaboratorioID = args.LaboratorioID;
                        analisis.EstatusID = 3;
                        analisis.Firmada = false;
                        analisis.FechaFirma = null;
                        analisis.Enviado = false;
                        analisis.Procesado = false;
                        analisis.FechaProceso = null;
                        analisis.FechaRegistro = DateTime.Now;
                        analisis.Observaciones = args.Observaciones;
                        analisis.ConsecionID = args.ConsecionID;

                        se.Analisis.Add(analisis);
                        se.SaveChanges();

                        var muestras = se.Muestras.Where(p => p.AnalisisID == AnalisisID && p.NivelID == (int)enumNivel.Simple);

                        foreach (var muestra in muestras)
                        {
                            Muestra muestraTemp = new Muestra();
                            muestraTemp.MuestraID = se.Muestras.Max(p => p.MuestraID) + 1;
                            muestraTemp.AnalisisID = analisis.AnalisisID;
                            muestraTemp.DireccionID = muestra.DireccionID;
                            muestraTemp.Numero = muestra.Numero;
                            muestraTemp.Descripcion = muestra.Descripcion;
                            muestraTemp.Identificacion = muestra.Identificacion;
                            muestraTemp.Caudal = muestra.Caudal;
                            muestraTemp.Latitud = muestra.Latitud;
                            muestraTemp.Longitud = muestra.Longitud;
                            muestraTemp.Datum = muestra.Datum;
                            muestraTemp.Fecha = muestra.Fecha;
                            muestraTemp.FechaRecepcion = muestra.FechaRecepcion;
                            muestraTemp.Cancelado = muestra.Cancelado;
                            muestraTemp.Observaciones = muestra.Observaciones;
                            muestraTemp.NivelID = muestra.NivelID;

                            se.Muestras.Add(muestraTemp);
                            se.SaveChanges();

                            var valores = se.MuestraValors.Where(p => p.MuestraID == muestra.MuestraID);
                            foreach (var valor in valores)
                            {
                                MuestraValor valorTemp = new MuestraValor();
                                valorTemp.MuestraValorID = se.MuestraValors.Max(p => p.MuestraValorID) + 1;
                                valorTemp.MuestraID = muestraTemp.MuestraID;
                                valorTemp.ParametroID = valor.ParametroID;
                                valorTemp.ParametroMetodoID = valor.ParametroMetodoID;
                                valorTemp.LaboratorioID = valor.LaboratorioID;
                                valorTemp.OperacionID = valor.OperacionID;
                                valorTemp.Exenta = valor.Exenta;
                                valorTemp.Evalua = valor.Evalua;
                                valorTemp.Exento = valor.Exento;
                                valorTemp.NoAnalizado = valor.NoAnalizado;
                                valorTemp.Valor = valor.Valor;
                                valorTemp.ValorValidacion = valor.ValorValidacion;
                                valorTemp.AceptaValor = valor.AceptaValor;
                                valorTemp.LFD = valor.LFD;
                                valorTemp.LimiteInferior = valor.LimiteInferior;
                                valorTemp.LimiteSuperior = valor.LimiteSuperior;
                                valorTemp.LimiteInferiorLFD = valor.LimiteInferiorLFD;
                                valorTemp.LimiteSuperiorLFD = valor.LimiteSuperiorLFD;
                                valorTemp.LC_MC = valor.LC_MC;
                                valorTemp.GeneradoSP = valor.GeneradoSP;
                                valorTemp.Grupo = valor.Grupo;

                                se.MuestraValors.Add(valorTemp);
                                se.SaveChanges();
                            }

                            var signatarios = se.Muestreadors.Where(p => p.MuestraID == muestra.MuestraID && (p.Activo ?? false));
                            foreach (var signatario in signatarios)
                            {
                                Muestreador signatarioTemp = new Muestreador();
                                signatarioTemp.MuestreadorID = se.Muestreadors.Max(x => x.MuestreadorID) + 1;
                                signatarioTemp.MuestraID = muestraTemp.MuestraID;
                                signatarioTemp.LaboratorioSignatarioID = signatario.LaboratorioSignatarioID;
                                signatarioTemp.Activo = true;

                                se.Muestreadors.Add(signatarioTemp);
                                se.SaveChanges();
                            }
                        }

                        analisis = se.Analisis.FirstOrDefault(p => p.AnalisisID == analisisID);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                    return true;
                }
            }
        }


        public List<OrdenAnalisis_Result> ObtenAnalisisResults(List<int?> ordenTrabajoIDList)
        {
            var listaRetorno = new List<OrdenAnalisis_Result>();

            using (Siralab_Entities sbd = new Siralab_Entities())
            {
                
                try
                {
                    {
                        //var table = ordenTrabajoIDList.ConvertToDataTable();

                        var tabla = new DataTable();
                        tabla.Columns.Add("OrdenTrabajoID");

                        foreach (var id in ordenTrabajoIDList)
                        {
                            if (id != null)
                                tabla.Rows.Add(id);
                        }
                        

                        var paramName = "@Listado";
                        var procedureName = "usp_ObtenOrdenAnalisis";
                        var typeName = "OrdenTrabajoType";
                        //// create parameter 
                        System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter(paramName, tabla);
                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = typeName;

                        //// execute sp sql 
                        string sql = $"EXEC {procedureName} {paramName};";
                        //// execute sql
                        listaRetorno = sbd.Database.SqlQuery<OrdenAnalisis_Result>(sql, parameter).ToList();

                    }

                    
                }
                catch (Exception ex)
                {
                    
                }

                return listaRetorno;
            }
        }


        public class OrdenAnalisis_Result
        {
            public int OrdenTrabajoID { get; set; }
            public int AnalisisID { get; set; }
            public bool Firmada { get; set; }
            public string Consecutivo { get; set; }
            public string Fecha { get; set; }
            public bool FirmaVisible { get; set; }
            public bool AnalisisEncolado { get; set; }
        }


        public class SolicitudCancelacionParam
        {
            public int SolicitudCancelacionID { get; set; }
            public int EstatusID { get; set; }

        }

        public class OrdenEliminarParam
        {
            public int OrdenTrabajoID { get; set; }
        }

        public void UpdateAnalisisProcesoFirma(Analisi AnalisisResult)
        {
            try
            {
                Repositorio.Edit(AnalisisResult);
                Repositorio.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException);
            }
        }

        public bool ActualizaNormaDQOCOT(int AnalisisID, string Norma)
        {
            Analisi analisis = new Analisi();
            try
            {

                AnalisisContext.usp_NormaDQOCOT(AnalisisID, Norma);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #region RegionConductividad
        public bool ActualizaConductividadElectrica(int AnalisisID, string Conductividad)
        {
            try
            {
                var analisis = AnalisisContext.Analisis.FirstOrDefault(x => x.AnalisisID == AnalisisID);
                analisis.Conductividad = Conductividad;
                AnalisisContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidaConductividad(int AnalisisID, string Conductividad, Analisi analisis)
        {
            bool resultado = false;
            try
            {
                if (Analisis.Muestras.First().FechaConformacion != null)
                {


                    var fechaConformacion = Analisis.Muestras.First().FechaConformacion.Value;
                    int trimestreMuestra = (int)Math.Ceiling((double)fechaConformacion.Month / 3);// validar que fecha poner 
                    List<Conductividad> query = (from ot in AnalisisContext.OrdenTrabajoes
                                                 join _a in AnalisisContext.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                                                 join m in AnalisisContext.Muestras on _a.AnalisisID equals m.AnalisisID
                                                 join n in AnalisisContext.Normas on ot.NormaID equals n.NormaID
                                                 where ot.NormaID == 15 && ot.ClienteID == analisis.OrdenTrabajo.ClienteID
                                                 && m.FechaConformacion.Value.Year == fechaConformacion.Year // Se puso la fecha de conformacion como indica CMIN
                                                 && _a.Conductividad != null
                                                 && _a.EstatusID != 4 && ot.ConsecionID == analisis.OrdenTrabajo.ConsecionID
                                                 && ot.AnexoID == analisis.OrdenTrabajo.AnexoID
                                                 orderby _a.AnalisisID descending
                                                 select new Conductividad
                                                 {
                                                     AnalisisID = _a.AnalisisID,
                                                     FechaConformacion = m.FechaConformacion.Value,// validar que fecha poner 
                                                     Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),// validar que fecha poner 
                                                     ConductividadElectrica = _a.Conductividad
                                                 }).ToList();

                    if (query != null)
                    {
                        var conductividadElectrica = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != AnalisisID).FirstOrDefault();
                        if (conductividadElectrica != null && conductividadElectrica.ConductividadElectrica != Conductividad)
                        {
                            resultado = false;
                        }
                        else
                        {
                            resultado = true;

                        }
                    }
                    else
                    {
                        resultado = false;
                    }

                    return resultado;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidaConductividadByFechaConformacion(int AnalisisID, string Conductividad, Analisi analisis, DateTime? FechaConformacion)
        {
            bool resultado = false;
            try
            {
                if (FechaConformacion != null)
                {
                        var fechaConformacion = FechaConformacion.Value;
                        int trimestreMuestra = (int)Math.Ceiling((double)fechaConformacion.Month / 3);// validar que fecha poner 
                        List<Conductividad> query = (from ot in AnalisisContext.OrdenTrabajoes
                                                     join _a in AnalisisContext.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                                                     join m in AnalisisContext.Muestras on _a.AnalisisID equals m.AnalisisID
                                                     join n in AnalisisContext.Normas on ot.NormaID equals n.NormaID
                                                     where ot.NormaID == 15 && ot.ClienteID == analisis.OrdenTrabajo.ClienteID
                                                     && m.FechaConformacion.Value.Year == fechaConformacion.Year // Se puso la fecha de conformacion como indica CMIN
                                                     && _a.Conductividad != null
                                                     && _a.EstatusID != 4 && ot.ConsecionID == analisis.OrdenTrabajo.ConsecionID
                                                     && ot.AnexoID == analisis.OrdenTrabajo.AnexoID
                                                     orderby _a.AnalisisID descending
                                                     select new Conductividad
                                                     {
                                                         AnalisisID = _a.AnalisisID,
                                                         FechaConformacion = m.FechaConformacion.Value,// validar que fecha poner 
                                                         Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),// validar que fecha poner 
                                                         ConductividadElectrica = _a.Conductividad
                                                     }).ToList();

                        if (query != null)
                        {
                            var conductividadElectrica = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != AnalisisID).FirstOrDefault();
                            if (conductividadElectrica != null && conductividadElectrica.ConductividadElectrica != Conductividad)
                            {
                                resultado = false;
                            }
                            else
                            {
                                resultado = true;

                            }
                        }
                        else
                        {
                            resultado = false;
                        }

                        return resultado;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }

    public class Conductividad
    {
        public int AnalisisID { get; set; }
        public DateTime FechaConformacion { get; set; }
        public int Trimestre { get; set; }
        public string ConductividadElectrica { get; set; }


    }
}