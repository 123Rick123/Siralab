using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL_WS_SIRALAB;

namespace BL_WS_SIRALAB.extend
{
    public static class AnalisisDalExtend
    {
        public static BL_WS_SIRALAB.Analisis ConvertAnalisis(this DL_WS_SIRALAB.Analisi analisis, enumTipoEvaluacion Evaluacion)
        {
            Analisis aConvertion = new Analisis();

            aConvertion.observaciones = analisis.Observaciones;
            aConvertion.AnalisisID = analisis.AnalisisID;
            aConvertion.falta_pago = analisis.FaltaPago;
            aConvertion.NormaDQOCOT = analisis.NormaDQOCOT;
            aConvertion.Conductividad = analisis.Conductividad;
            #region Laboratorio
            aConvertion.laboratorio = new Laboratorio
                {
                    persona_fisica = analisis.Laboratorio.Fisica,
                    nombre = analisis.Laboratorio.Nombre,
                    apellidoMaterno = analisis.Laboratorio.ApellidoMaterno,
                    apellidoPaterno = analisis.Laboratorio.ApellidoPaterno,
                    rfc = analisis.Laboratorio.RFC,
                    curp = analisis.Laboratorio.CURP,
                    direccion = analisis.Laboratorio.Direccion.ConvertDireccion()
                };
            #endregion

            #region Orden de trabajo
            aConvertion.orden_trabajo = new OrdenTrabajo
                {
                    fecha_emision = analisis.OrdenTrabajo.FechaEmision,
                    numero_informe = analisis.OrdenTrabajo.NumeroInforme,
                    folio = analisis.OrdenTrabajo.Folio,
                    horas_proceso = analisis.OrdenTrabajo.HorasProceso,
                    anio = analisis.OrdenTrabajo.Anio,
                    Anexo = analisis.OrdenTrabajo.Anexo.ConvertAnexo(),
                    Cuerpo = analisis.OrdenTrabajo.CuerpoReceptor.ConvertCuerpoReceptor(),
                    Uso = analisis.OrdenTrabajo.CatUsoAgua.ConvertUsoAgua(),
                    Norma = analisis.OrdenTrabajo.Norma.ConvertNorma()
                };
            #endregion

            #region Muestras
            var muestras = analisis.Muestras.Select(p => new Muestra
                {
                    nivel = p.NivelID.ToEnumNivel(),
                    numero = p.Numero,
                    descripcion = p.Descripcion,
                    identificacion = p.Identificacion,
                    requiere_caudal = p.Analisi.OrdenTrabajo.Norma.RequiereCaudal,
                    caudal = p.Caudal,
                    latitud = p.Latitud,
                    longitud = p.Longitud,
                    datum = p.Datum,
                    fecha = p.Fecha != null ? (DateTime?)DateTime.Parse(((DateTime)p.Fecha).ToShortDateString()) : null,
                    hora = p.Fecha != null ? (TimeSpan?)((DateTime)p.Fecha).TimeOfDay : null,
                    fecha_recepcion = p.FechaRecepcion != null ? (DateTime?)DateTime.Parse(((DateTime)p.FechaRecepcion).ToShortDateString()) : null,
                    hora_recepcion = p.FechaRecepcion != null ? (TimeSpan?)((DateTime)p.FechaRecepcion).TimeOfDay : null,
                    cancelado = p.Cancelado,
                    observaciones = p.Observaciones,
                    Muestreador = new Signatario
                    {
                        persona = p.Muestreadors.FirstOrDefault() != null ?
                            p.Muestreadors.FirstOrDefault().LaboratorioSignatario.Persona.ConvertPersona() : new BL_WS_SIRALAB.Persona(),
                        TipoSignatario = p.Muestreadors.FirstOrDefault() != null ?
                            p.Muestreadors.FirstOrDefault().LaboratorioSignatario.CatTipoSignatario.TipoSignatarioID.ToEnumSignatario() : enumSignatario.Muestreador
                    },
                    direccion = p.Direccion.ConvertDireccion(),
                    Resultados = p.MuestraValors.Select(q => q.ConvertMuestraValor(Evaluacion)).ToList()
                }).ToList();
            aConvertion.muestras = muestras;
            #endregion

            return aConvertion;
        }
        public static BL_WS_SIRALAB.Analisis ConvertAnalisisPD(this DL_WS_SIRALAB.Analisi analisis, enumTipoEvaluacion Evaluacion)
        {
            Analisis aConvertion = new Analisis();

            aConvertion.observaciones = analisis.Observaciones;
            aConvertion.AnalisisID = analisis.AnalisisID;
            aConvertion.falta_pago = analisis.FaltaPago;

            #region Laboratorio
            aConvertion.laboratorio = new Laboratorio
            {
                persona_fisica = analisis.Laboratorio.Fisica,
                nombre = analisis.Laboratorio.Nombre,
                apellidoMaterno = analisis.Laboratorio.ApellidoMaterno,
                apellidoPaterno = analisis.Laboratorio.ApellidoPaterno,
                rfc = analisis.Laboratorio.RFC,
                curp = analisis.Laboratorio.CURP,
                direccion = analisis.Laboratorio.Direccion.ConvertDireccion()
            };
            #endregion

            #region Orden de trabajo
            aConvertion.orden_trabajo = new OrdenTrabajo
            {
                fecha_emision = analisis.OrdenTrabajo.FechaEmision,
                numero_informe = analisis.OrdenTrabajo.NumeroInforme,
                folio = analisis.OrdenTrabajo.Folio,
                horas_proceso = analisis.OrdenTrabajo.HorasProceso,
                anio = analisis.OrdenTrabajo.Anio,
                Anexo = analisis.OrdenTrabajo.Anexo.ConvertAnexo(),
                Cuerpo = analisis.OrdenTrabajo.CuerpoReceptor.ConvertCuerpoReceptor(),
                Uso = analisis.OrdenTrabajo.CatUsoAgua.ConvertUsoAgua(),
                Norma = analisis.OrdenTrabajo.Norma.ConvertNorma()
            };
            #endregion

            #region Muestras
            var muestras = analisis.Muestras.Where(p => p.NivelID == enumNivel.Compuesta.ToInt()).Select(p => new Muestra
            {
                nivel = p.NivelID.ToEnumNivel(),
                numero = p.Numero,
                descripcion = p.Descripcion,
                identificacion = p.Identificacion,
                requiere_caudal = p.Analisi.OrdenTrabajo.Norma.RequiereCaudal,
                caudal = p.Caudal,
                latitud = p.Latitud,
                longitud = p.Longitud,
                datum = p.Datum,
                fecha = p.Fecha != null ? (DateTime?)DateTime.Parse(((DateTime)p.Fecha).ToShortDateString()) : null,
                hora = p.Fecha != null ? (TimeSpan?)((DateTime)p.Fecha).TimeOfDay : null,
                fecha_recepcion = p.FechaRecepcion != null ? (DateTime?)DateTime.Parse(((DateTime)p.FechaRecepcion).ToShortDateString()) : null,
                hora_recepcion = p.FechaRecepcion != null ? (TimeSpan?)((DateTime)p.FechaRecepcion).TimeOfDay : null,
                cancelado = p.Cancelado,
                observaciones = p.Observaciones,
                Muestreador = new Signatario
                {
                    persona = p.Muestreadors.FirstOrDefault() != null ?
                        p.Muestreadors.FirstOrDefault().LaboratorioSignatario.Persona.ConvertPersona() : new BL_WS_SIRALAB.Persona(),
                    TipoSignatario = p.Muestreadors.FirstOrDefault() != null ?
                        p.Muestreadors.FirstOrDefault().LaboratorioSignatario.CatTipoSignatario.TipoSignatarioID.ToEnumSignatario() : enumSignatario.Muestreador
                },
                direccion = p.Direccion.ConvertDireccion(),
                Resultados = p.MuestraValors.Select(q => q.ConvertMuestraValor(Evaluacion)).ToList()
            }).ToList();
            aConvertion.muestras = muestras;
            #endregion

            return aConvertion;
        }
        public static BL_WS_SIRALAB.Persona ConvertPersona(this DL_WS_SIRALAB.Persona persona)
        {
            BL_WS_SIRALAB.Persona param = new BL_WS_SIRALAB.Persona();

            param.nombre = persona.Nombre;
            param.apellido_paterno = persona.ApellidoPaterno;
            param.apellido_materno = persona.ApellidoMaterno;
            param.rfc = persona.RFC;
            param.curp = persona.CURP;

            return param;
        }
        public static BL_WS_SIRALAB.Direccion ConvertDireccion(this DL_WS_SIRALAB.Direccion direccion)
        {
            BL_WS_SIRALAB.Direccion param = new BL_WS_SIRALAB.Direccion();

            param.calle = direccion.Calle;
            param.colonia = direccion.Colonia.Nombre;
            param.cp = direccion.CP.ToString("{00000:0}");
            param.estado = direccion.Estado.Nombre;
            param.localidad = direccion.Localidad.Nombre;
            param.municipio = direccion.Municipio.Nombre;
            param.numero_exterior = direccion.NumeroExterior;
            param.numero_interior = direccion.NumeroInterior;
            
            return param;
        }
        public static BL_WS_SIRALAB.ParametroAnalizado ConvertMuestraValor(this DL_WS_SIRALAB.MuestraValor muestra_valor, enumTipoEvaluacion Evaluacion)
        {
            BL_WS_SIRALAB.ParametroAnalizado param = new BL_WS_SIRALAB.ParametroAnalizado();

            param.operacion = muestra_valor.OperacionID != null ? ((int)muestra_valor.OperacionID).ToEnumOperacion() : enumOperacion.No_Aplica;
            param.exenta = muestra_valor.Exenta;
            //param.evalua = muestra_valor.Evalua;
            param.exento = muestra_valor.Exento;
            param.no_analizado = muestra_valor.NoAnalizado;
            param.valor = muestra_valor.Valor;
            param.acepta_valor = muestra_valor.AceptaValor;
            param.ley_federal = muestra_valor.LFD;
            param.limite_inferior = muestra_valor.LimiteInferior;
            param.limite_superior = muestra_valor.LimiteSuperior;
            param.limite_inferior_LFD = muestra_valor.LimiteInferiorLFD;
            param.limite_superior_LFD = muestra_valor.LimiteSuperiorLFD;
            param.lc_mc = muestra_valor.LC_MC;
            param.parametro = muestra_valor.Parametro.ConvertParametro();
            param.metodo = muestra_valor.ParametroMetodo != null ? muestra_valor.ParametroMetodo.Metodo.ConvertMetodo() : null;
            param.laboratorio = (muestra_valor.OperacionID != null && !muestra_valor.Exento && !muestra_valor.NoAnalizado && muestra_valor.GeneradoSP == null) ? (muestra_valor.Laboratorio == null ? null : muestra_valor.Laboratorio.ConvertLaboratorio()) : null;
            param.grupo = muestra_valor.Grupo ? muestra_valor.Parametro.NormaConfiguracions
                .Where(z => z.NormaID == muestra_valor.Muestra.Analisi.OrdenTrabajo.NormaID)
                .SelectMany(k => k.NormaConfiguracion1)
                .Select(l => l.Parametro.ConvertParametro()).ToList() :
                new List<Parametro>();
            param.Caudal = muestra_valor.Muestra.Caudal;
            param.FechaConformacion = muestra_valor.Muestra.FechaConformacion != null ? muestra_valor.Muestra.FechaConformacion.Value : DateTime.MinValue;
            if (param.Caudal == 0 && muestra_valor.Muestra.NivelID == (int)enumNivel.Simple)
            {
                param.evalua = false;
            }
            else
            {
                param.evalua = muestra_valor.Evalua;
            }
            param.valor_validacion = muestra_valor.Valido(Evaluacion); // Validación por parámetro
           

            foreach (var ParametroHijo in param.grupo)
            {
                if (muestra_valor.Muestra.MuestraValors.Where(z => z.ParametroID == ParametroHijo.ParametroID).FirstOrDefault() != null)
                {


                    if (muestra_valor.Muestra.MuestraValors.Where(z => z.ParametroID == ParametroHijo.ParametroID).FirstOrDefault().NoAnalizado)
                    {
                        param.valor_validacion = false;
                    }
                }
            }

            return param;
        }
        public static List<BL_WS_SIRALAB.ParametroAnalizado> ConvertMuestraValorList(IEnumerable<DL_WS_SIRALAB.MuestraValor> muestra_valor, enumTipoEvaluacion Evaluacion)
        {
            List<BL_WS_SIRALAB.ParametroAnalizado> lParam = new List<BL_WS_SIRALAB.ParametroAnalizado>();
            BL_WS_SIRALAB.ParametroAnalizado param = new BL_WS_SIRALAB.ParametroAnalizado();
            object sync = new object();

            Parallel.ForEach(muestra_valor, par =>
            {
                lock (sync)
                {
                    param = new ParametroAnalizado();
                    param.operacion = par.OperacionID != null ? ((int)par.OperacionID).ToEnumOperacion() : enumOperacion.No_Aplica;
                    param.exenta = par.Exenta;
                    param.evalua = par.Evalua;
                    param.exento = par.Exento;
                    param.no_analizado = par.NoAnalizado;
                    param.valor = par.Valor;
                    param.acepta_valor = par.AceptaValor;
                    param.ley_federal = par.LFD;
                    param.limite_inferior = par.LimiteInferior;
                    param.limite_superior = par.LimiteSuperior;
                    param.limite_inferior_LFD = par.LimiteInferiorLFD;
                    param.limite_superior_LFD = par.LimiteSuperiorLFD;
                    param.lc_mc = par.LC_MC;
                    param.parametro = par.Parametro.ConvertParametro();
                    param.metodo = par.ParametroMetodo != null ? par.ParametroMetodo.Metodo.ConvertMetodo() : null;
                    param.laboratorio = (par.OperacionID != null && !par.Exento && !par.NoAnalizado && par.GeneradoSP == null) ? (par.Laboratorio == null ? null : par.Laboratorio.ConvertLaboratorio()) : null;
                    param.grupo = par.Grupo ? par.Parametro.NormaConfiguracions
                        .Where(z => z.NormaID == par.Muestra.Analisi.OrdenTrabajo.NormaID)
                        .SelectMany(k => k.NormaConfiguracion1)
                        .Select(l => l.Parametro.ConvertParametro()).ToList() :
                        new List<Parametro>();

                    param.valor_validacion = par.Valido(Evaluacion);
                    // Validación por parámetro
                    foreach (var ParametroHijo in param.grupo)
                    {
                        if (par.Muestra.MuestraValors.Where(z => z.ParametroID == ParametroHijo.ParametroID).FirstOrDefault().NoAnalizado)
                        {
                            param.valor_validacion = false;
                        }
                    }
                    lParam.Add(param);
                }
            });

            return lParam;
        }
        public static bool Valido(this DL_WS_SIRALAB.MuestraValor muestra_valor, enumTipoEvaluacion Evaluacion)
        {
            #region Validación del parámetro
            bool valida = false;

            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;

                if (!muestra_valor.NoAnalizado && !muestra_valor.Exento && muestra_valor.Evalua)
                {
                    if (muestra_valor.AceptaValor)
                    {
                        /// Primero buscamos sí se requiere evaluar por medio de LFD
                        /// 
                        switch (Evaluacion)
                        {
                            case enumTipoEvaluacion.Evaluar_Simple:
                                #region Evaluación respecto a los límites de norma
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (muestra_valor.LimiteInferior != null)
                                {
                                    valida = muestra_valor.LimiteInferior <= muestra_valor.Valor && muestra_valor.Valor <= muestra_valor.LimiteSuperior;
                                }
                                else
                                {
                                    valida = muestra_valor.Valor <= muestra_valor.LimiteSuperior;
                                }
                                #endregion
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD:
                                if (muestra_valor.LFD != null ? (bool)muestra_valor.LFD : false)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (muestra_valor.LimiteInferiorLFD != null)
                                    {
                                        valida = muestra_valor.LimiteInferiorLFD <= muestra_valor.Valor && muestra_valor.Valor <= muestra_valor.LimiteSuperiorLFD;
                                    }
                                    else
                                    {
                                        valida = muestra_valor.Valor <= muestra_valor.LimiteSuperiorLFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    valida = true;
                                }
                                break;
                            case enumTipoEvaluacion.Evaluar_LFD_Simple:
                                if (muestra_valor.LFD != null ? (bool)muestra_valor.LFD : false)
                                {
                                    #region Evaluación respecto a los límites de LFD
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (muestra_valor.LimiteInferiorLFD != null)
                                    {
                                        valida = muestra_valor.LimiteInferiorLFD <= muestra_valor.Valor && muestra_valor.Valor <= muestra_valor.LimiteSuperiorLFD;
                                    }
                                    else
                                    {
                                        valida = muestra_valor.Valor <= muestra_valor.LimiteSuperiorLFD;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Evaluación respecto a los límites de norma
                                    ///Verificamos que tenga límite inferior para saber si es un rango
                                    ///
                                    if (muestra_valor.LimiteInferior != null)
                                    {
                                        valida = muestra_valor.LimiteInferior <= muestra_valor.Valor && muestra_valor.Valor <= muestra_valor.LimiteSuperior;
                                    }
                                    else
                                    {
                                        valida = muestra_valor.Valor <= muestra_valor.LimiteSuperior;
                                    }
                                    #endregion
                                }
                                break;
                            default:
                                valida = true;
                                break;
                        }
                        //if (muestra_valor.LFD)
                        //{
                        //    #region Evaluación respecto a los límites de LFD
                        //    ///Verificamos que tenga límite inferior para saber si es un rango
                        //    ///
                        //    if (muestra_valor.LimiteInferiorLFD != null)
                        //    {
                        //        valida = muestra_valor.LimiteInferiorLFD <= muestra_valor.Valor && muestra_valor.Valor <= muestra_valor.LimiteSuperiorLFD;
                        //    }
                        //    else
                        //    {
                        //        valida = muestra_valor.Valor <= muestra_valor.LimiteSuperiorLFD;
                        //    }
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region Evaluación respecto a los límites de norma
                        //    ///Verificamos que tenga límite inferior para saber si es un rango
                        //    ///
                        //    if (muestra_valor.LimiteInferior != null)
                        //    {
                        //        valida = muestra_valor.LimiteInferior <= muestra_valor.Valor && muestra_valor.Valor <= muestra_valor.LimiteSuperior;
                        //    }
                        //    else
                        //    {
                        //        valida = muestra_valor.Valor <= muestra_valor.LimiteSuperior;
                        //    }
                        //    #endregion
                        //}
                    }
                    else
                    {
                        valida = (bool)muestra_valor.ValorValidacion;
                    }
                }
                else if (!muestra_valor.Evalua)
                {
                    valida = true;
                }
                else if (muestra_valor.Evalua && muestra_valor.NoAnalizado && muestra_valor.LFD)
                {
                    valida = false;
                }
                else if (muestra_valor.Evalua && muestra_valor.NoAnalizado && !muestra_valor.LFD)
                {
                    valida = true;
                }
                else if (muestra_valor.Exento)
                {
                    valida = true;
                }
         
            #endregion

            return valida;
        }

        public static bool ValidoParametro(ParametroAnalizado muestra_valor, decimal ValorEvaluado, enumTipoEvaluacion Evaluacion)
        {
            #region Validación del parámetro
            bool valida = false;

            if (Evaluacion == enumTipoEvaluacion.NoEvaluar) return true;

            if (!((bool)muestra_valor.no_analizado) && !((bool)muestra_valor.exento) && ((bool)muestra_valor.evalua))
            {
                if ((bool)muestra_valor.acepta_valor)
                {
                    /// Primero buscamos sí se requiere evaluar por medio de LFD
                    /// 
                    switch (Evaluacion)
                    {
                        case enumTipoEvaluacion.Evaluar_Simple:
                            { }
                            #region Evaluación respecto a los límites de norma

                            #endregion
                            break;
                        case enumTipoEvaluacion.Evaluar_LFD:
                            if (muestra_valor.ley_federal != null ? (bool)muestra_valor.ley_federal : false)
                            {
                                #region Evaluación respecto a los límites de LFD
                                ///Verificamos que tenga límite inferior para saber si es un rango
                                ///
                                if (muestra_valor.limite_inferior_LFD != null)
                                {
                                    valida = muestra_valor.limite_inferior_LFD <= ValorEvaluado && ValorEvaluado <= muestra_valor.limite_superior_LFD;
                                }
                                else
                                {
                                    valida = ValorEvaluado <= muestra_valor.limite_superior_LFD;
                                }
                                #endregion
                            }
                            else
                            {
                                valida = true;
                            }
                            break;
                        case enumTipoEvaluacion.Evaluar_LFD_Simple:
                            { }

                            break;
                        default:
                            valida = true;
                            break;
                    }
                }
                else
                {
                    valida = (bool)muestra_valor.valor_validacion;
                }
            }
            else if (!(bool)muestra_valor.evalua)
            {
                valida = true;
            }
            else if ((bool)muestra_valor.no_analizado || (bool)muestra_valor.exento)
            {
                valida = true;
            }
            else if ((bool)muestra_valor.evalua && (bool)muestra_valor.no_analizado)
            {
                valida = false;
            }
            #endregion

            return valida;
        }

        //Cambio por fuente de Abastecimiento
        public static BL_WS_SIRALAB.Parametro ConvertParametro(this DL_WS_SIRALAB.Parametro parametro)
        {
            BL_WS_SIRALAB.Parametro param = new BL_WS_SIRALAB.Parametro();

            param.nombre = parametro.Nombre;
            param.unidad = parametro.CatUnidadMedida.Nombre;
            param.descripcion = parametro.Descripcion;
            param.ParametroID = parametro.ParametroID;
            return param;
        }
        public static BL_WS_SIRALAB.Metodo ConvertMetodo(this DL_WS_SIRALAB.Metodo metodo)
        {
            BL_WS_SIRALAB.Metodo param = new BL_WS_SIRALAB.Metodo();

            param.nombre = metodo.Nombre;
            param.descripcion = metodo.Descripcion;

            return param;
        }
        public static BL_WS_SIRALAB.Laboratorio ConvertLaboratorio(this DL_WS_SIRALAB.Laboratorio laboratorio)
        {
            BL_WS_SIRALAB.Laboratorio param = new BL_WS_SIRALAB.Laboratorio();

            param.direccion = laboratorio.Direccion.ConvertDireccion();
            param.nombre = laboratorio.Nombre;
            param.apellidoPaterno = laboratorio.ApellidoPaterno;
            param.apellidoMaterno = laboratorio.ApellidoMaterno;
            param.rfc = laboratorio.RFC;
            param.curp = laboratorio.CURP;
            param.persona_fisica = laboratorio.Fisica;

            return param;
        }
        public static BL_WS_SIRALAB.Anexo ConvertAnexo(this DL_WS_SIRALAB.Anexo anexo)
        {
            BL_WS_SIRALAB.Anexo param = new BL_WS_SIRALAB.Anexo();

            param.anexo = anexo.Anexo1;
            param.latitud = anexo.Latitud;
            param.longitud = anexo.Longitud;
            param.datum = anexo.Datum;
            param.nombre_cuerpo_receptor = anexo.NombreCuerpoReceptor;
            param.fuente_abastecimiento = anexo.FuenteAbastecimiento;
            param.origen_descarga = anexo.OrigenDescarga;
            param.medidor = anexo.Medidor;
            param.marcaMedidor = anexo.MarcaMedidor;
            param.efluente = anexo.Efluente;
            param.Titulo = new Titulo { titulo = anexo.Consecion.Titulo };

            return param;
        }
        public static BL_WS_SIRALAB.CuerpoReceptor ConvertCuerpoReceptor(this DL_WS_SIRALAB.CuerpoReceptor cuerpo_receptor)
        {
            BL_WS_SIRALAB.CuerpoReceptor param = new BL_WS_SIRALAB.CuerpoReceptor();

            param.id = cuerpo_receptor.CuerpoReceptorID;
            param.nombre = cuerpo_receptor.Nombre;
            param.tipo_cuerpo_receptor = cuerpo_receptor.TipoCuerpoReceptorID.ToEnumTipoCuerpoReceptor();

            return param;
        }
        public static BL_WS_SIRALAB.UsoAgua ConvertUsoAgua(this DL_WS_SIRALAB.CatUsoAgua uso_agua)
        {
            BL_WS_SIRALAB.UsoAgua param = new BL_WS_SIRALAB.UsoAgua();

            param.id = uso_agua.UsoAguaID;
            param.nombre = uso_agua.Nombre;

            return param;
        }
        public static BL_WS_SIRALAB.Norma ConvertNorma(this DL_WS_SIRALAB.Norma norma)
        {
            BL_WS_SIRALAB.Norma param = new BL_WS_SIRALAB.Norma();

            param.id = norma.NormaID;
            param.Nombre = norma.Nombre;
            param.Descripcion = norma.Descripcion;
            param.Vigente = norma.FechaFinal != null ? (DateTime.Now <= (DateTime)norma.FechaFinal) : true;

            return param;
        }
        public static BL_WS_SIRALAB.Cliente ConvertCliente(this DL_WS_SIRALAB.Cliente cliente)
        {
            BL_WS_SIRALAB.Cliente param = new BL_WS_SIRALAB.Cliente();

            param.nombre = cliente.Nombre;
            param.apellido_paterno = cliente.ApellidoPaterno;
            param.apellido_materno = cliente.ApellidoMaterno;
            param.Correo = cliente.Correo;
            param.curp = cliente.CURP;
            param.rfc = cliente.RFC;

            return param;
        }

    }
}
