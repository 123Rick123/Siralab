using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using System.Data;
using System.ComponentModel;


namespace SIRALAB_DAL.Extensiones
{
    public static class AnalisisExt
    {
        public static string Resultado(this Analisi analisis)
        {
            string resultado = string.Empty;
            string Aprobado = "Aprobado";
            string NoAprobado = "No Aprobado";

            if (analisis.VerificaBanderaMuestraCompuesta())
            {
                Norma norma = analisis.OrdenTrabajo.Norma;
                var muestras = analisis.Muestras.ToList<Muestra>();
                if ((bool)norma.Evaluacion)
                {
                    int resultadoFalse = 0;



                    muestras.ForEach(a => resultadoFalse += a.MuestraValors.Count(b => b.ValorValidacion != true));

                    if (resultadoFalse > 0)
                        resultado = NoAprobado;
                    else
                        resultado = Aprobado;
                }
                else
                {
                    string strMuestraValor = string.Empty;

                    foreach (Muestra mst in muestras)
                    {
                        foreach (MuestraValor mstVal in mst.MuestraValors)
                        {
                            string param = mstVal.Parametro.Nombre;
                            string val = mstVal.Valor.ToString();
                            strMuestraValor = "Parametro: " + param + " Valor: " + val + " , ";
                        }
                    }
                }
            }
            else
                resultado = NoAprobado;

            return resultado;
        }

        public static bool VerificaBanderaMuestraCompuesta(this Analisi analisis)
        {
            var result = false;
            var muestras = analisis.Muestras;
            var muestrasActivas = muestras.Where(p => !p.Cancelado);
            if (muestrasActivas.Count() > 0)
            {
                foreach (var muestra in muestrasActivas)
                {
                    if (muestra.NivelID == 2)
                    {
                        if (muestra.FlagCompuesta ?? false)
                            result = true;
                        if (muestra.FlagCompuesta != null && (bool)muestra.FlagCompuesta && muestra.Analisi.AnalisisFirmas.Any())
                            result = false;
                    }
                    else
                    {
                        if (muestra.FlagCompuesta != null && (bool)muestra.FlagCompuesta)
                            result = true;
                    }
                }
            }
            return result;
        }
        public static bool MuestraValorCompleto(this Analisi analisis)
        {
            bool result = false;
            Siralab_Entities db = new Siralab_Entities();

            FrecuenciaMuestreo frecuenciamuestreo = Calcula_FrecuenciaMuestreo(analisis.OrdenTrabajo.HorasProceso);

            int muestraCompuesta = 1;
            int numeroMuestras = frecuenciamuestreo.MuestrasSimples;
            var muestras = analisis.Muestras;

            if (muestras.Any(m => m.FlagCompuesta == false))
                return false;

            /// Validamos primero el análisis según su norma
            /// 
    
            var norma = analisis.OrdenTrabajo.Norma;
            var NormaFuenteAbastecimiento2021 = db.Normas.FirstOrDefault(x => x.NormaID == 17);
            if (norma.RequiereCompuesta)
            {
                numeroMuestras += muestraCompuesta;
            }

            if (!norma.RequiereCaudal)
            {
                #region No requiere caudal
                /// Sí la norma no requiere caudal entonces se requiere unicamente
                /// muestras simples en donde mínimo uno de sus parámetros se
                /// encuentre capturado de forma completa
                /// 
                var muestrasActivas = muestras.Where(p => !p.Cancelado);
                if (muestrasActivas.Any())
                {
                    /// Para poder firmar el análisis debe tener al menos una muestra vigente sin cancelación
                    ///
                    ///

                    result = !muestrasActivas.Any(m => m.MuestraValors.Any(mv => !mv.Exento && !mv.NoAnalizado && (mv.LaboratorioID == null || mv.ParametroMetodoID == null)));
                }
                else
                {
                    return false;
                }
                #endregion
            }
            else
            {
                #region Requiere caudal
                #region Alguna muestra simple falta por capturar
                /// Si existen muestras simples sin caudal y no canceladas entonces no está finalizado el análisis

                if (muestras.Any(m => m.ValHojaCampo == null && m.NivelID == 1))
                    return false;
                
                #endregion

                #endregion Muestras simples contra la frecuencia de muestreo //TODO refactor.
                if (muestras.Count() == numeroMuestras)
                {
                    foreach (Muestra muestra in muestras)
                    {
                        foreach (MuestraValor muestravalor in muestra.MuestraValors)
                        {
                            if (muestravalor.Valor != 0)
                            {
                                if (muestravalor.Exento)
                                {
                                    result = true;
                                    break;
                                }

                                if (muestravalor.NoAnalizado)
                                {
                                    result = true;
                                    break;
                                }

                                if ((muestravalor.LaboratorioID != null && muestravalor.ParametroMetodoID != null && !muestravalor.Grupo) ||
                                    (muestravalor.LaboratorioID == null && muestravalor.ParametroMetodoID == null && muestravalor.Grupo) ||
                                    (muestravalor.LaboratorioID == null && muestravalor.ParametroMetodoID == null && (muestravalor.GeneradoSP ?? true)) ||
                                    (muestravalor.OperacionID != null))
                                {
                                    if (!muestravalor.AceptaValor)
                                    {
                                        result = true;
                                    }
                                    else if (muestravalor.AceptaValor && muestra.NivelID == 1 && muestravalor.LCMC != null)
                                    {
                                        if (muestravalor.LCMC == true && muestravalor.LC_MC == 0)
                                        {
                                            return false;
                                        }
                                        result = true;
                                    }
                                    else if ((muestravalor.AceptaValor && muestra.NivelID == 2 && muestravalor.LC_MC >= 0)
                                             || muestravalor.OperacionID != null
                                             || (muestra.NivelID == 2 && muestravalor.LCMC == false)
                                             )
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if ((norma.NormaID == 16 || norma.NormaID == 15 || (NormaFuenteAbastecimiento2021 != null && norma.NormaID == NormaFuenteAbastecimiento2021.NormaID)) 
                        && analisis.NormaDQOCOT == null)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        private static FrecuenciaMuestreo Calcula_FrecuenciaMuestreo(int HorasProceso)
        {
            FrecuenciaMuestreo frecuenciamuestreo = new FrecuenciaMuestreo();
            Siralab_Entities db = new Siralab_Entities();
            var fm = db.FrecuenciaMuestreos.ToList();
            foreach (var i in fm)
            {
                if (HorasProceso <= i.HorasMaximo)
                {
                    if (HorasProceso >= i.HorasMinimo)
                    {
                        frecuenciamuestreo = i;
                        break;
                    }
                }
            }
            return frecuenciamuestreo;
        }
    }
}
