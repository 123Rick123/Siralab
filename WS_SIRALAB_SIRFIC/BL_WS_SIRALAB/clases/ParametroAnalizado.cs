using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(IsReference = true)]
    public class ParametroAnalizado
    {
        [DataMember]
        public Parametro parametro { get; set; }
        [DataMember]
        public Metodo metodo { get; set; }
        [DataMember]
        public Laboratorio laboratorio { get; set; }
        [DataMember]
        public enumOperacion? operacion { get; set; }
        [DataMember]
        public bool? exenta { get; set; }
        [DataMember]
        public bool? evalua { get; set; }
        [DataMember]
        public bool? exento { get; set; }
        [DataMember]
        public bool? no_analizado { get; set; }
        [DataMember]
        public decimal? valor { get; set; }
        [DataMember]
        public bool? valor_validacion { get; set; }
        [DataMember]
        public bool? acepta_valor { get; set; }
        [DataMember]
        public bool? ley_federal { get; set; }
        [DataMember]
        public decimal? limite_inferior { get; set; }
        [DataMember]
        public decimal? limite_superior { get; set; }
        [DataMember]
        public decimal? limite_inferior_LFD { get; set; }
        [DataMember]
        public decimal? limite_superior_LFD { get; set; }
        [DataMember]
        public decimal? lc_mc { get; set; }
        [DataMember]
        public List<Parametro> grupo { get; set; }
        [DataMember]
        public DateTime FechaConformacion { get; set; }
        [DataMember]
        public decimal? ValorFuente { get; set; }
        [DataMember]
        public decimal? Diferencia { get; set; }
        [DataMember]
        public bool? AplicaFechaConformacion { get; set; }
        [DataMember]
        public decimal? Caudal { get; set; }
    }

    public static class BL_Extend
    {
        public static bool Valido(this BL_WS_SIRALAB.ParametroAnalizado parametro_analizado, enumTipoEvaluacion Evaluacion)
        {
            #region Validación del parámetro
            bool valida = false;

            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;

            if ((bool)parametro_analizado.evalua)
            {
                if ((bool)parametro_analizado.acepta_valor)
                {
                    /// Primero buscamos sí se requiere evaluar por medio de LFD
                    /// 

                    switch (Evaluacion)
                    {
                        case enumTipoEvaluacion.Evaluar_Simple:
                            #region Evaluación respecto a los límites de norma
                            ///Verificamos que tenga límite inferior para saber si es un rango
                            ///
                            if (parametro_analizado.limite_inferior != null)
                            {
                                valida = parametro_analizado.limite_inferior <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior;
                            }
                            else
                            {
                                valida = parametro_analizado.valor <= parametro_analizado.limite_superior;
                            }
                            #endregion
                            break;
                        case enumTipoEvaluacion.Evaluar_LFD:
                            if (parametro_analizado.ley_federal != null ? (bool)parametro_analizado.ley_federal : false)
                            {
                                #region Evaluación respecto a los límites de LFD
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (parametro_analizado.limite_inferior_LFD != null)
                                {
                                    valida = parametro_analizado.limite_inferior_LFD <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                                }
                                else
                                {
                                    valida = parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                                }
                                #endregion
                            }
                            else
                            {
                                valida = true;
                            }
                            break;
                        case enumTipoEvaluacion.Evaluar_LFD_Simple:
                            if (parametro_analizado.ley_federal != null ? (bool)parametro_analizado.ley_federal : false)
                            {
                                #region Evaluación respecto a los límites de LFD
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (parametro_analizado.limite_inferior_LFD != null)
                                {
                                    valida = parametro_analizado.limite_inferior_LFD <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                                }
                                else
                                {
                                    valida = parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Evaluación respecto a los límites de norma
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (parametro_analizado.limite_inferior != null)
                                {
                                    valida = parametro_analizado.limite_inferior <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior;
                                }
                                else
                                {
                                    valida = parametro_analizado.valor <= parametro_analizado.limite_superior;
                                }
                                #endregion
                            }
                            break;
                        default:
                            valida = true;
                            break;
                    }
                    /*if ((bool)parametro_analizado.ley_federal)
                    {
                        #region Evaluación respecto a los límites de LFD
                        ///Verificamos que tenga límite inferior para saber si es un rango
                        ///
                        if (parametro_analizado.limite_inferior_LFD != null)
                        {
                            valida = parametro_analizado.limite_inferior_LFD <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                        }
                        else
                        {
                            valida = parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Evaluación respecto a los límites de norma
                        ///Verificamos que tenga límite inferior para saber si es un rango
                        ///
                        if (parametro_analizado.limite_inferior != null)
                        {
                            valida = parametro_analizado.limite_inferior <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior;
                        }
                        else
                        {
                            valida = parametro_analizado.valor <= parametro_analizado.limite_superior;
                        }
                        #endregion
                    }*/
                }
                else
                {
                    valida = (bool)parametro_analizado.valor_validacion;
                }
            }
            else if (!(bool)parametro_analizado.evalua)
            {
                valida = true;
            }
            #endregion

            return valida;
        }
        public static bool Valido_fuente(this BL_WS_SIRALAB.ParametroAnalizado parametro_analizado, enumTipoEvaluacion Evaluacion, List<BL_WS_SIRALAB.ParametroAnalizado> ParametrosFuente)
        {
            #region Validación del parámetro
            bool valida = false;

            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;

            if ((bool)parametro_analizado.evalua)
            {
                if ((bool)parametro_analizado.acepta_valor)
                {
                    /// Primero buscamos sí se requiere evaluar por medio de LFD
                    /// 

                    switch (Evaluacion)
                    {
                        case enumTipoEvaluacion.Evaluar_Simple:
                            #region Evaluación respecto a los límites de norma
                            ///Verificamos que tenga límite inferior para saber si es un rango
                            ///
                            if (parametro_analizado.limite_inferior != null)
                            {
                                valida = parametro_analizado.limite_inferior <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior;
                            }
                            else
                            {
                                valida = parametro_analizado.valor <= parametro_analizado.limite_superior;
                            }
                            #endregion
                            break;
                        case enumTipoEvaluacion.Evaluar_LFD:
                            if (parametro_analizado.ley_federal != null ? (bool)parametro_analizado.ley_federal : false)
                            {
                                #region Evaluación respecto a los límites de LFD
                                //Localizamos parametro en lista de fuente
                                decimal? ValorFuente = 0;
                                decimal? ValorEvaluado = 0;
                                ParametroAnalizado parametroOfFuente = null;
                                parametroOfFuente = ParametrosFuente.Where(p => p.parametro.ParametroID == parametro_analizado.parametro.ParametroID).FirstOrDefault();
                                if (parametroOfFuente != null)
                                {
                                    ValorFuente = parametroOfFuente.valor != null ? (decimal)parametroOfFuente.valor : 0;
                                }
                                ValorEvaluado = parametro_analizado.valor - ValorFuente;
                                //



                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (parametro_analizado.limite_inferior_LFD != null)
                                {
                                    valida = parametro_analizado.limite_inferior_LFD <= ValorEvaluado && ValorEvaluado <= parametro_analizado.limite_superior_LFD;
                                }
                                else
                                {
                                    valida = ValorEvaluado <= parametro_analizado.limite_superior_LFD;
                                }
                                #endregion
                            }
                            else
                            {
                                valida = true;
                            }
                            break;
                        case enumTipoEvaluacion.Evaluar_LFD_Simple:
                            if (parametro_analizado.ley_federal != null ? (bool)parametro_analizado.ley_federal : false)
                            {
                                #region Evaluación respecto a los límites de LFD
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (parametro_analizado.limite_inferior_LFD != null)
                                {
                                    valida = parametro_analizado.limite_inferior_LFD <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                                }
                                else
                                {
                                    valida = parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Evaluación respecto a los límites de norma
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (parametro_analizado.limite_inferior != null)
                                {
                                    valida = parametro_analizado.limite_inferior <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior;
                                }
                                else
                                {
                                    valida = parametro_analizado.valor <= parametro_analizado.limite_superior;
                                }
                                #endregion
                            }
                            break;
                        default:
                            valida = true;
                            break;
                    }
                    /*if ((bool)parametro_analizado.ley_federal)
                    {
                        #region Evaluación respecto a los límites de LFD
                        ///Verificamos que tenga límite inferior para saber si es un rango
                        ///
                        if (parametro_analizado.limite_inferior_LFD != null)
                        {
                            valida = parametro_analizado.limite_inferior_LFD <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                        }
                        else
                        {
                            valida = parametro_analizado.valor <= parametro_analizado.limite_superior_LFD;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Evaluación respecto a los límites de norma
                        ///Verificamos que tenga límite inferior para saber si es un rango
                        ///
                        if (parametro_analizado.limite_inferior != null)
                        {
                            valida = parametro_analizado.limite_inferior <= parametro_analizado.valor && parametro_analizado.valor <= parametro_analizado.limite_superior;
                        }
                        else
                        {
                            valida = parametro_analizado.valor <= parametro_analizado.limite_superior;
                        }
                        #endregion
                    }*/
                }
                else
                {
                    valida = (bool)parametro_analizado.valor_validacion;
                }
            }
            else if (!(bool)parametro_analizado.evalua)
            {
                valida = true;
            }
            #endregion

            return valida;
        }
    }
}
