using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common.CommandTrees;
using System.Diagnostics;
using System.Linq;

namespace SIRALAB_BL
{
    public class MuestraValor_BL : Base_BL<MuestraValor>
    {
        //public Siralab_Entities MuestraValorContext { get; private set; }
        //public MuestraValor MuestraValor_ { get; private set; }
        //Repository<MuestraValor> Repositorio;
        public MuestraValor_BL() : base()
        {
            //MuestraValorContext = new Siralab_Entities();
            //Repositorio = new Repository<MuestraValor>(MuestraValorContext);
        }

        public List<MuestraValor> Lista_MuestraValor(int MuestraID)
        {
            List<MuestraValor> lista = new List<MuestraValor>();
            lista = dbContext.uspMuestraValoresOrdenada(MuestraID).ToList();
            return lista;
        }

        public List<MuestraValor> ObtenerMuestraValores(int muestraId)
        {
            return dbContext.MuestraValors.Where(m => m.MuestraID == muestraId).ToList();
        }

        public List<MuestraValor> Lista_MuestraValor_Analisis(int AnalisisID)
        {
            List<MuestraValor> lista = new List<MuestraValor>();
            lista = Repositorio.Find(x => x.Muestra.AnalisisID == AnalisisID).ToList();
            return lista.OrderBy(q => q.ParametroID).ToList();
        }

        public MuestraValor Obten(int MuestraValorID)
        {
            MuestraValor mv = new MuestraValor();
            mv = Repositorio.First(x => x.MuestraValorID == MuestraValorID);
            return mv;
        }

        public MuestraValor muestralab(int MuestraValorID)
        {
            var mv = Repositorio.First(x => x.MuestraValorID == MuestraValorID);
            return mv;
        }
        public MuestraValor Actualiza_NoAnalizado(int MuestraValorID, bool NoAnalizado)
        {
            MuestraValor muestra = new MuestraValor();
            muestra = Obten(MuestraValorID);
            muestra.NoAnalizado = NoAnalizado;

            if (muestra.Muestra.NivelID == 2 && NoAnalizado)
            {
                muestra.Valor = 0;
                muestra.LC_MC = 0;
                muestra.LimiteInferior = null;
                muestra.LimiteSuperior = 0;
                muestra.LaboratorioID = null;
                muestra.ParametroMetodoID = null;
            }

            muestra.GeneradoSP = false;

            Repositorio.Edit(muestra);
            Repositorio.SaveChanges();

            return muestra;
        }

        public MuestraValor Actualiza_Exento(int MuestraValorID, bool Exento)
        {
            MuestraValor muestra = new MuestraValor();
            muestra = Obten(MuestraValorID);
            muestra.Exento = Exento;

            if (muestra.Muestra.NivelID == 2 && Exento)
            {
                muestra.Valor = 0;
                muestra.LC_MC = 0;
                muestra.LimiteInferior = null;
                muestra.LimiteSuperior = 0;
                muestra.LaboratorioID = null;
                muestra.ParametroMetodoID = null;
            }


            muestra.GeneradoSP = false;

            Repositorio.Edit(muestra);
            Repositorio.SaveChanges();

            return muestra;
        }

        public void EliminaMuestraValor(int MuestraValorID)
        {
            MuestraValor mv = new MuestraValor();
            mv = Obten(MuestraValorID);
            Repositorio.Delete(mv);
        }

        public List<MuestraValorArgBase_BL> GuardaObten(int NormaID, int MuestraID, int NivelID, bool EsCompuesta)
        {
            var list = this.dbContext.usp_MuestraValores(NormaID, MuestraID, NivelID, EsCompuesta)
                .OrderBy(p => p.MuestraValorID).Select(i => new MuestraValorArgBase_BL
                {
                    MuestraValorID = i.MuestraValorID,
                    MuestraID = i.MuestraID,
                    ParametroID = i.ParametroID,
                    Parametro = i.Parametro,
                    ParametroMetodoID = i.ParametroMetodoID,
                    ParametroMetodo = i.ParametroMetodo,
                    LaboratorioID = i.LaboratorioID,
                    OperacionID = i.OperacionID,
                    Exenta = i.Exenta,
                    Evalua = i.Evalua,
                    Exento = i.Exento,
                    NoAnalizado = i.NoAnalizado,
                    Valor = i.Valor ?? 0,
                    ValorValidacion = i.ValorValidacion,
                    AceptaValor = i.AceptaValor,
                    LFD = i.LFD,
                    LimiteInferior = i.LimiteInferior,
                    LimiteSuperior = i.LimiteSuperior,
                    LimiteInferiorLFD = i.LimiteInferiorLFD,
                    LimiteSuperiorLFD = i.LimiteSuperiorLFD,
                    LC_MC = i.LC_MC,
                    GeneradoSP = i.GeneradoSP,
                    Grupo = i.Grupo,
                    EscalaDecimal = i.EscalaDecimal,
                    LCMC = i.LCMC,
                    Truncamiento = i.Truncamiento,
                    LimitaCaptura = i.LimitaCaptura,
                    MinAceptado = i.MinAceptado,
                    MaxAceptado = i.MaxAceptado,
                    EscalaDecimalLCM = i.EscalaDecimalLCM
                });

            return list.ToList();
        }




        public List<MuestraValorArgBase_BL> GuardaObtenCompuesta(int NormaID, int MuestraID)
        {
            var list = this.dbContext.usp_MuestraValoresCompuesta(NormaID, MuestraID)
                .OrderBy(p => p.MuestraValorID).Select(i => new MuestraValorArgBase_BL
                {
                    MuestraValorID = i.MuestraValorID,
                    MuestraID = i.MuestraID,
                    ParametroID = i.ParametroID,
                    Parametro = i.Parametro,
                    ParametroMetodoID = i.ParametroMetodoID,
                    ParametroMetodo = i.ParametroMetodo,
                    LaboratorioID = i.LaboratorioID,
                    OperacionID = i.OperacionID,
                    Exenta = i.Exenta,
                    Evalua = i.Evalua,
                    Exento = i.Exento,
                    NoAnalizado = i.NoAnalizado,
                    Valor = i.Valor ?? 0,
                    ValorValidacion = i.ValorValidacion,
                    AceptaValor = i.AceptaValor,
                    LFD = i.LFD,
                    LimiteInferior = i.LimiteInferior,
                    LimiteSuperior = i.LimiteSuperior,
                    LimiteInferiorLFD = i.LimiteInferiorLFD,
                    LimiteSuperiorLFD = i.LimiteSuperiorLFD,
                    LC_MC = i.LC_MC,
                    GeneradoSP = i.GeneradoSP,
                    Grupo = i.Grupo,
                    EscalaDecimal = i.EscalaDecimal,
                    LCMC = i.LCMC,
                    Truncamiento = i.Truncamiento,
                    LimitaCaptura = i.LimitaCaptura,
                    MinAceptado = i.MinAceptado,
                    MaxAceptado = i.MaxAceptado,
                    EscalaDecimalLCM = i.EscalaDecimalLCM
                });

            return list.ToList();
        }


        public void ClonarMuestraValor(MuestraValor muestraValor, int muestraID)
        {
            try
            {
                var normaConfiguracion = obtieneNormaConfiguracionAuxiliar(muestraValor);
                
                MuestraValor clon = new MuestraValor()
                {
                    MuestraValorID = Repositorio.NextNewID(x => x.MuestraValorID),
                    MuestraID = muestraID,
                    ParametroID = muestraValor.ParametroID,
                    ParametroMetodoID = muestraValor.ParametroMetodoID,
                    LaboratorioID = muestraValor.LaboratorioID,
                    OperacionID = muestraValor.OperacionID ?? normaConfiguracion.OperacionID ,
                    Exenta = muestraValor.Exenta,
                    Evalua = muestraValor.Evalua,
                    Exento = muestraValor.Exento,
                    NoAnalizado = muestraValor.NoAnalizado,
                    Valor = muestraValor.Valor,
                    ValorValidacion = muestraValor.ValorValidacion,
                    AceptaValor = muestraValor.AceptaValor,
                    LFD = muestraValor.LFD,
                    LimiteInferior = muestraValor.LimiteInferior,
                    LimiteSuperior = muestraValor.LimiteSuperior,
                    LimiteInferiorLFD = muestraValor.LimiteInferiorLFD,
                    LimiteSuperiorLFD = muestraValor.LimiteSuperiorLFD,
                    LC_MC = muestraValor.LC_MC,
                    FechaRegistro = DateTime.Now,
                    GeneradoSP = muestraValor.GeneradoSP,
                    Grupo = normaConfiguracion != null && normaConfiguracion.Grupo,
                    EscalaDecimal = muestraValor.EscalaDecimal,
                    LCMC = muestraValor.Muestra.NivelID == enumNivel.Simple.ConvertToInt() ? false : normaConfiguracion?.LCMC,
                    Truncamiento = muestraValor.Truncamiento,
                };

                Repositorio.Add(clon);
                Repositorio.SaveChanges();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }

        }

        private NormaConfiguracion obtieneNormaConfiguracionAuxiliar(MuestraValor muestraValor)
        {
            var ordenTrabajo = muestraValor.Muestra.Analisi.OrdenTrabajo;
            var cuerpoReceptorID = ordenTrabajo.CuerpoReceptorID;
            var usoAguaID = ordenTrabajo.UsoAguaID;
            var normaID = ordenTrabajo.NormaID;
            var parametroID = muestraValor.ParametroID;
            var nivelID = muestraValor.Muestra.NivelID;
            return muestraValor.Muestra?.Analisi?.OrdenTrabajo?.Norma?.NormaConfiguracions?.FirstOrDefault(nc =>
                    nc.NormaID == normaID
                    && nc.NivelID == nivelID
                    && nc.CuerpoReceptorID == cuerpoReceptorID
                    && nc.UsoAguaID == usoAguaID
                    && nc.ParametroID == parametroID);
        }

        public MuestraValor Guardar(MuestraValorArgBase_BL args)
        {
            MuestraValor mv = new MuestraValor();
            try
            {
                if (args.MuestraValorID != 0 && args.MuestraValorID != null)
                    mv = Repositorio.First(p => p.MuestraValorID == args.MuestraValorID);

                mv.MuestraValorID = args.MuestraValorID != null ? (int)args.MuestraValorID : 0;
                mv.MuestraID = args.MuestraID;
                mv.ParametroID = (int)args.ParametroID;
                mv.ParametroMetodoID = args.ParametroMetodoID;
                mv.LaboratorioID = args.LaboratorioID;
                mv.OperacionID = args.OperacionID;
                mv.Exenta = (bool)args.Exenta;
                mv.Evalua = (bool)args.Evalua;
                mv.Exento = args.Exento ?? false;
                mv.NoAnalizado = args.NoAnalizado ?? false;
                mv.Valor = args.Valor;
                mv.ValorValidacion = args.ValorValidacion;
                mv.AceptaValor = args.AceptaValor;
                mv.LFD = (bool)args.LFD;
                mv.LimiteInferior = args.LimiteInferior;
                mv.LimiteSuperior = args.LimiteSuperior;
                mv.LimiteInferiorLFD = args.LimiteInferiorLFD;
                mv.LimiteSuperiorLFD = args.LimiteSuperiorLFD;
                mv.LC_MC = args.LC_MC;
                mv.GeneradoSP = args.GeneradoSP;
                mv.Grupo = args.Grupo ?? false;
                mv.EscalaDecimal = args.EscalaDecimal;
                mv.LCMC = args.LCMC;

                if (args.MuestraValorID == 0 || args.MuestraValorID == null) //Se trata de un nuevo registro en la base de datos
                {
                    mv.MuestraValorID = Repositorio.NextNewID(x => x.MuestraValorID);
                    Repositorio.Add(mv);
                }

                Repositorio.SaveChanges();
                mv = Repositorio.Find(x => x.MuestraValorID == mv.MuestraValorID).FirstOrDefault();
                return mv;
            }
            catch (Exception ex)
            {
                mv = null;
                return mv;
            }
        }

        public MuestraValor Guardar(int MuestraValorID, decimal? valor)
        {
            MuestraValor mv = new MuestraValor();
            try
            {
                mv = Repositorio.First(p => p.MuestraValorID == MuestraValorID);
                mv.Valor = valor;

                Repositorio.SaveChanges();
                mv = Repositorio.Find(x => x.MuestraValorID == mv.MuestraValorID).FirstOrDefault();
                return mv;
            }
            catch (Exception ex)
            {
                mv = null;
                return mv;
            }
        }

        public bool XMLActualizaMuestraValor(MuestraValor muestraV)
        {
            MuestraValor mv = new MuestraValor();
            try
            {
                mv = Repositorio.First(p => p.MuestraValorID == muestraV.MuestraValorID);
                mv.AceptaValor = muestraV.AceptaValor;
                mv.EscalaDecimal = muestraV.EscalaDecimal;
                mv.Evalua = muestraV.Evalua;
                mv.Exenta = muestraV.Exenta;
                mv.Exento = muestraV.Exento;
                mv.FechaRegistro = muestraV.FechaRegistro;
                mv.GeneradoSP = muestraV.GeneradoSP;
                mv.Grupo = muestraV.Grupo;
                mv.LaboratorioID = muestraV.LaboratorioID;
                mv.LCMC = muestraV.LCMC;
                mv.LC_MC = muestraV.LC_MC;
                mv.LFD = muestraV.LFD;
                mv.LimiteInferior = muestraV.LimiteInferior;
                mv.LimiteInferiorLFD = muestraV.LimiteInferiorLFD;
                mv.LimiteSuperior = muestraV.LimiteSuperior;
                mv.LimiteSuperiorLFD = muestraV.LimiteSuperiorLFD;
                mv.MuestraID = muestraV.MuestraID;
                mv.MuestraValorID = muestraV.MuestraValorID;
                mv.NoAnalizado = muestraV.NoAnalizado;
                mv.OperacionID = muestraV.OperacionID;
                mv.ParametroID = muestraV.ParametroID;
                mv.ParametroMetodoID = muestraV.ParametroMetodoID;
                mv.Valor = muestraV.Valor;
                mv.ValorValidacion = muestraV.ValorValidacion;

                Repositorio.SaveChanges();
                mv = Repositorio.Find(x => x.MuestraValorID == mv.MuestraValorID).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                mv = null;
                return false;
            }
        }

        public void Dispose()
        {
            this.Repositorio.Dispose();
        }

        public void InsertaFaltantes(List<int> parametros, List<NormaConfiguracion> normaConfiguraciones, int muestraID)
        {
            try
            {
                foreach (var parametro in parametros)
                {
                    var normaConfiguracion = normaConfiguraciones.FirstOrDefault(nc => nc.ParametroID == parametro);
                    var muestraValor = new MuestraValor
                    {
                        MuestraValorID = Repositorio.NextNewID(x => x.MuestraValorID),
                        MuestraID = muestraID,
                        ParametroID = parametro,
                        ParametroMetodoID = null,
                        LaboratorioID = null,
                        OperacionID = null,
                        Exenta = normaConfiguracion.Exenta,
                        Evalua = normaConfiguracion.Evalua,
                        Exento = false,
                        NoAnalizado = false,
                        Valor = 0,
                        ValorValidacion = null,
                        AceptaValor = normaConfiguracion.AceptaValor,
                        LFD = normaConfiguracion.LFD,
                        LimiteInferior = normaConfiguracion.LimiteInferior,
                        LimiteSuperior = normaConfiguracion.LimiteSuperior,
                        LimiteInferiorLFD = normaConfiguracion.LimiteInferiorLFD,
                        LimiteSuperiorLFD = normaConfiguracion.LimiteSuperiorLFD,
                        LC_MC = null,
                        FechaRegistro = DateTime.Now,
                        GeneradoSP = false,
                        LCMC = normaConfiguracion.LCMC ?? true,
                        EscalaDecimal = normaConfiguracion.EscalaDecimal
                    };

                    Repositorio.Add(muestraValor);
                }

                //Repositorio.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }


        //TODO => Refactorizar después de octubre:
        //RFD -> se añade para evitar que truene la captura de muestra compuesta en las normas en las que se han agregado parámetros.
        public void ActualizaModificadosPorNormaConfiguracion(int? muestraValorID)
        {
            try
            {
                if (muestraValorID.HasValue)
                {
                    MuestraValor muestraValor = Obten(muestraValorID.Value);

                    if (!muestraValor.Muestra.Analisi.OrdenTrabajo.Norma.Nombre.Contains("Lerma") && muestraValor.Parametro.Nombre.StartsWith("Nitrógeno Total"))
                    {
                        muestraValor.OperacionID = 1;
                        muestraValor.Grupo = true;
                        muestraValor.LCMC = false;
                    }
                    else if (muestraValor.Muestra.Analisi.OrdenTrabajo.Norma.Nombre.Contains("Lerma"))
                    {
                        muestraValor.ParametroID = 836;
                        muestraValor.OperacionID = null;
                        muestraValor.Grupo = false;
                        muestraValor.LCMC = true;
                    }

                    Repositorio.Edit(muestraValor);
                    Repositorio.SaveChanges();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }


}