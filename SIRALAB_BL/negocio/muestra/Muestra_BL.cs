using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;

namespace SIRALAB_BL
{
    public class Muestra_BL //: Base_BL<Muestra>
    {
        public Siralab_Entities MuestraContext { get; private set; }
        public Muestra Analisis { get; private set; }

        Repository<Muestra> Repositorio;
        Repository<MuestraValor> Repositorios;

        public Muestra_BL() //: base()
        {
            MuestraContext = new Siralab_Entities();
            Repositorio = new Repository<Muestra>(MuestraContext);
        }

        public List<Muestra> Lista_Muestra(int AnalisisID)
        {
            var listamuestra = Repositorio.Find(x => x.AnalisisID == AnalisisID).ToList();
            return listamuestra;
        }

        public Muestra Obten_Muestra(int MuestraID)
        {
            var muestra = Repositorio.Find(x => x.MuestraID == MuestraID).FirstOrDefault();
            return muestra;
        }


        public Muestra ClonarMuestra(Muestra muestraArg, int analisisID)
        {
            try
            {
                Muestra muestra = new Muestra()
                {
                    MuestraID = Repositorio.NextNewID(x => x.MuestraID),
                    AnalisisID = analisisID,
                    DireccionID = muestraArg.DireccionID,
                    NivelID = muestraArg.NivelID,
                    Numero = muestraArg.Numero,
                    Descripcion = muestraArg.Descripcion,
                    Identificacion = muestraArg.Identificacion,
                    Caudal = muestraArg.Caudal,
                    Latitud = muestraArg.Latitud,
                    Longitud = muestraArg.Longitud,
                    Datum = muestraArg.Datum,
                    Fecha = muestraArg.Fecha,
                    FechaRecepcion = muestraArg.FechaRecepcion,
                    Cancelado = false,
                    Observaciones = muestraArg.Observaciones,
                    ValMuestraC = muestraArg.ValMuestraC,
                    ValHojaCampo = muestraArg.ValHojaCampo,
                    FlagCompuesta = muestraArg.NivelID == enumNivel.Simple.ConvertToInt() ? muestraArg.FlagCompuesta : false,
                    FechaConformacion = muestraArg.FechaConformacion

                };

                Repositorio.Add(muestra);
                Repositorio.SaveChanges();
                return muestra;
            }
            catch (Exception e)
            {
                return new Muestra();
            }
        }

        public Muestra Guarda(MuestraArgBase_BL args)
        {
            Muestra muestra = new Muestra();
            //TODO -> Añadir los campos que faltan.

            try
            {
                muestra.MuestraID = args.MuestraID != null ? (int)args.MuestraID : 0;
                muestra.AnalisisID = args.AnalisisID;
                muestra.Datum = args.Datum;
                muestra.Numero = args.Numero;
                muestra.Descripcion = args.Descripcion;
                muestra.Identificacion = args.Identificacion;
                muestra.Caudal = args.Caudal;
                muestra.Latitud = args.Latitud;
                muestra.Longitud = args.Longitud;
                muestra.Fecha = args.Fecha;
                /*muestra.EsCompuesta = args.EsCompuesta;*/
                muestra.FechaRecepcion = args.FechaRecepcion;
                muestra.Cancelado = args.Cancelado;
                muestra.Observaciones = args.Observaciones;
                muestra.NivelID = args.NivelID;
                muestra.FechaConformacion = args.FechaConformacion;

                if (muestra.MuestraID == 0) //Se trata de un nuevo registro en la base de datos
                {
                    if (args.NivelID == enumNivel.Compuesta.ConvertToInt())
                    {
                        muestra.FechaConformacion = args.FechaConformacion;
                    }

                    Direccion_BL direccion_bl = new Direccion_BL();
                    if (args.dir != null)
                    {
                        direccion_bl.Guarda(args.dir);
                        if (direccion_bl.Direccion != null)
                        {
                            muestra.MuestraID = Repositorio.NextNewID(x => x.MuestraID);
                            muestra.DireccionID = direccion_bl.Direccion.DireccionID;
                            Repositorio.Add(muestra);
                            Repositorio.SaveChanges();
                            muestra = Repositorio.Single(x => x.MuestraID == muestra.MuestraID);
                        }
                    }
                    else
                    {
                        muestra.MuestraID = Repositorio.NextNewID(x => x.MuestraID);
                        muestra.DireccionID = (int)args.DireccionID;
                        Repositorio.Add(muestra);
                        Repositorio.SaveChanges();
                        muestra = Repositorio.Single(x => x.MuestraID == muestra.MuestraID);
                    }


                }
                else
                {
                    muestra.DireccionID = (int)(args.dir.DireccionID);
                    Direccion_BL direccion_bl = new Direccion_BL();
                    direccion_bl.Guarda(args.dir);

                    Repositorio.Edit(muestra);
                    Repositorio.SaveChanges();
                    muestra = Repositorio.Single(x => x.MuestraID == args.MuestraID);
                }

                return muestra;
            }

            catch (Exception ex)
            {
                muestra = null;
                return muestra;
            }
        }

        public bool ActualizaMuestra(Muestra args)
        {
            Muestra muestra = new Muestra();
            try
            {
                muestra = args;
                Repositorio.Edit(muestra);
                Repositorio.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                muestra = null;
            }
        }

        public void EliminarMuestra(int MuestraID)
        {
            //Obtenemos la muestra a eliminar
            Muestra mu = new Muestra();
            mu = Obten_Muestra(MuestraID);

            //Creamos lista con los objetos que se han de eliminar
            List<Muestreador> lista_muestreador = new List<Muestreador>();
            Muestreador_BL muestreador_bl = new Muestreador_BL();

            //Eliminamos el muestreador
            muestreador_bl.EliminaMuestreador(MuestraID);

            //Creamos lista con los objetos de muestra valor
            List<MuestraValor> lista_muestravalor = new List<MuestraValor>();
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

            //Obtenemos la lista de las muestra valor a eliminar
            lista_muestravalor = muestravalor_bl.ObtenerMuestraValores(MuestraID);

            foreach (var i in lista_muestravalor)
            {
                muestravalor_bl.EliminaMuestraValor(i.MuestraValorID);
            }
            Repositorio.Delete(mu);
        }

        public void EliminarMuestraValor(int MuestraID)
        {
            //Creamos lista con los objetos de muestra valor
            List<MuestraValor> lista_muestravalor = new List<MuestraValor>();
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

            //Obtenemos la lista de las muestra valor a eliminar
            lista_muestravalor = muestravalor_bl.Lista_MuestraValor(MuestraID);

            foreach (var i in lista_muestravalor)
            {
                muestravalor_bl.EliminaMuestraValor(i.MuestraValorID);
            }
        }

        public void EliminarMuestraCompuesta(int MuestraIDSimple)
        {
            //banderas para poder borrar la muestra compuesta
            bool Avanza_Compuesta = false;
            bool EliminaCompuesta = false;

            //Obtenemos la muestra simple para ver a que analisis pertenece
            Muestra muSimple = new Muestra();
            muSimple = Obten_Muestra(MuestraIDSimple);

            //Obtenemos las muestras simples de analisis 
            List<Muestra> listamuestra = Repositorio.Find(x => x.AnalisisID == muSimple.AnalisisID && x.NivelID == 1).ToList();
            SIRALAB_BL.catalogos.Norma_BL ncBL = new SIRALAB_BL.catalogos.Norma_BL();
            Analisis_BL analisis = new Analisis_BL();
            Analisi analisisGeneral = analisis.Obten_Analisis(muSimple.AnalisisID);
            var lista_valoresConfig = ncBL.Obtener((int)analisisGeneral.OrdenTrabajo.NormaID);

            //Validamos que esten listas las muestras para crear la compuesta
            if (listamuestra.Count > 0)
            {
                foreach (var muestra in listamuestra)
                {
                    MuestraValor_BL muestravalor_blSimples = new MuestraValor_BL();
                    var lista_valores = muestravalor_blSimples.Lista_MuestraValor(muestra.MuestraID);

                    //Validación utilizada para comprobar que existen registros en muestra valor
                    if (lista_valores.Count > 0 && lista_valoresConfig.NormaConfiguracions.Where(p => p.NivelID == 1).Count() > 0)
                    {
                        foreach (var val in lista_valores)
                        {
                            if (val.GeneradoSP == false)
                            {
                                Avanza_Compuesta = true;
                            }
                            else
                            {
                                Avanza_Compuesta = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (!analisisGeneral.OrdenTrabajo.Norma.RequiereCaudal && muestra.Caudal == -1)
                        {
                            Avanza_Compuesta = true;
                        }
                        else
                        {
                            if (muestra.Cancelado)
                            {
                                Avanza_Compuesta = true;
                            }
                            else
                            {
                                Avanza_Compuesta = lista_valoresConfig.NormaConfiguracions.Where(p => p.NivelID == 1).Count() == 0;
                                break;
                            }
                        }
                    }
                }
            }

            if (Avanza_Compuesta)
            {
                int suma = 0;
                foreach (Muestra VerifCaudal in listamuestra)
                {
                    if (VerifCaudal.Caudal <= 0)
                    {
                        suma += 1;
                    }
                }
                if (suma == listamuestra.Count)
                {
                    EliminaCompuesta = true;
                }
            }

            if (EliminaCompuesta)
            {
                Muestra MuestraCompuesta = Repositorio.Find(x => x.AnalisisID == muSimple.AnalisisID && x.NivelID == 2).FirstOrDefault();
                if (MuestraCompuesta != null)
                {
                    int MuestraID = MuestraCompuesta.MuestraID;
                    //Obtenemos la muestra a eliminar
                    Muestra mu = new Muestra();
                    mu = Obten_Muestra(MuestraID);

                    //Creamos lista con los objetos que se han de eliminar
                    List<Muestreador> lista_muestreador = new List<Muestreador>();
                    Muestreador_BL muestreador_bl = new Muestreador_BL();

                    //Eliminamos el muestreador
                    muestreador_bl.EliminaMuestreador(MuestraID);

                    //Creamos lista con los objetos de muestra valor
                    List<MuestraValor> lista_muestravalor = new List<MuestraValor>();
                    MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

                    //Obtenemos la lista de las muestra valor a eliminar
                    lista_muestravalor = muestravalor_bl.Lista_MuestraValor(MuestraID);

                    foreach (var i in lista_muestravalor)
                    {
                        muestravalor_bl.EliminaMuestraValor(i.MuestraValorID);
                    }
                    Repositorio.Delete(mu);
                }
            }
        }

        public Muestra Actualiza(MuestraArgBase_BL args)
        {
            Muestra muestra = new Muestra();

            try
            {
                muestra.MuestraID = args.MuestraID != null ? (int)args.MuestraID : 0;
                muestra.AnalisisID = args.AnalisisID;
                muestra.DireccionID = (int)args.DireccionID;
                muestra.Datum = args.Datum;
                muestra.Numero = args.Numero;
                muestra.Descripcion = args.Descripcion;
                muestra.Identificacion = args.Identificacion;
                muestra.Caudal = args.Caudal;
                muestra.Latitud = args.Latitud;
                muestra.Longitud = args.Longitud;
                muestra.Fecha = args.Fecha;
                /*muestra.EsCompuesta = args.EsCompuesta;*/
                muestra.FechaRecepcion = args.FechaRecepcion;
                muestra.Cancelado = args.Cancelado;
                muestra.Observaciones = args.Observaciones;
                muestra.NivelID = args.NivelID;
                muestra.ValMuestraC = args.ValMuestraC;
                muestra.ValHojaCampo = args.ValHojaCampo;
                muestra.FechaConformacion = args.FechaConformacion;

                if (muestra.MuestraID > 0) //Se trata de un nuevo registro en la base de datos
                {
                    Repositorio.Edit(muestra);
                    Repositorio.SaveChanges();
                    muestra = Repositorio.Single(x => x.MuestraID == args.MuestraID);
                }

                return muestra;
            }
            catch (Exception ex)
            {
                muestra = null;
                return muestra;
            }
        }

        public bool Recalcula(int NormaID, int MuestraID)
        {
            Siralab_Entities se = new Siralab_Entities();
            bool respuesta = false;

            try
            {
                se.usp_MuestraValoresCompuestaRecalculo(NormaID, MuestraID);
                respuesta = true;
            }
            catch
            {
                respuesta = false;
            }
            finally
            {
                se.Dispose();
                se = null;
            }

            return respuesta;
        }

        public bool RecalculaSimple(int MuestraID)
        {
            Siralab_Entities se = new Siralab_Entities();
            bool respuesta = false;

            try
            {
                se.usp_MuestraValoresSimpleRecalculo(MuestraID);
                respuesta = true;
            }
            catch
            {
                respuesta = false;
            }
            finally
            {
                se.Dispose();
                se = null;
            }

            return respuesta;
        }

        public bool XMLActualizaMuestras(List<Muestra> xml, int AnalisisID)
        {
            bool result = false;
            var list = Repositorio.Find(x => x.AnalisisID == AnalisisID).ToList();
            MuestraValor_BL muestraValorBL = new MuestraValor_BL();
            foreach (var item in list)
            {
                var parametros = item.MuestraValors.Select(c => c.ParametroID).ToArray();
                var parametrometodos = item.MuestraValors.Select(c => c.ParametroMetodoID).ToArray();
                var x = xml.Where(a =>
                        a.MuestraValors.Any(b => parametros.Contains(b.ParametroID) && parametrometodos.Contains(b.ParametroMetodoID))).FirstOrDefault();
                var y = x == null ? new List<MuestraValor>() : x.MuestraValors.ToList();
                foreach (var e in item.MuestraValors)
                {
                    var v = y.Where(w1 => w1.ParametroID == e.ParametroID && w1.ParametroMetodoID == e.ParametroMetodoID).FirstOrDefault();
                    MuestraValor valor = new MuestraValor();
                    valor.AceptaValor = e.AceptaValor;
                    valor.EscalaDecimal = e.EscalaDecimal;
                    valor.Evalua = e.Evalua;
                    valor.Exenta = e.Exenta;
                    valor.Exento = v?.Exento ?? e.Exento;
                    valor.FechaRegistro = v?.FechaRegistro ?? e.FechaRegistro;
                    valor.GeneradoSP = e.GeneradoSP;
                    valor.Grupo = e.Grupo;
                    valor.LaboratorioID = e.LaboratorioID;
                    valor.LCMC = e.LCMC;
                    valor.LC_MC = v?.LC_MC ?? e.LC_MC;
                    valor.LFD = e.LFD;
                    valor.LimiteInferior = e.LimiteInferior;
                    valor.LimiteInferiorLFD = e.LimiteInferiorLFD;
                    valor.LimiteSuperior = e.LimiteSuperior;
                    valor.LimiteSuperiorLFD = e.LimiteSuperiorLFD;
                    valor.MuestraID = e.MuestraID;
                    valor.MuestraValorID = e.MuestraValorID;
                    valor.NoAnalizado = v?.NoAnalizado ?? e.NoAnalizado;
                    valor.OperacionID = e.OperacionID;
                    valor.ParametroID = v?.ParametroID ?? e.ParametroID;
                    valor.ParametroMetodoID = v?.ParametroMetodoID ?? e.ParametroMetodoID;
                    valor.Valor = v?.Valor ?? e.Valor;
                    valor.ValorValidacion = e.ValorValidacion;
                    valor.Laboratorio = e.Laboratorio;
                    valor.OperacionConsolidado = e.OperacionConsolidado;
                    valor.Muestra = e.Muestra;
                    valor.Parametro = e.Parametro;
                    valor.ParametroMetodo = e.ParametroMetodo;

                    result = muestraValorBL.XMLActualizaMuestraValor(valor);
                }
                Muestra muestra = new Muestra();

                muestra.MuestraID = item.MuestraID;
                muestra.AnalisisID = item.AnalisisID;
                muestra.Cancelado = item.Cancelado;
                muestra.Caudal = x?.Caudal ?? item.Caudal;
                muestra.Datum = item.Datum;
                muestra.Descripcion = x?.Descripcion ?? item.Descripcion;
                muestra.DireccionID = item.DireccionID;
                muestra.Fecha = x?.Fecha ?? item.Fecha;
                muestra.FechaRecepcion = x?.FechaRecepcion ?? item.FechaRecepcion;
                muestra.FlagCompuesta = item.FlagCompuesta;
                muestra.Identificacion = x?.Identificacion ?? item.Identificacion;
                muestra.Latitud = item.Latitud;
                muestra.Longitud = item.Longitud;
                muestra.NivelID = item.NivelID;
                muestra.Numero = x?.Numero ?? item.Numero;
                muestra.Observaciones = item.Observaciones;
                muestra.ValHojaCampo = item.ValHojaCampo;
                muestra.ValMuestraC = item.ValMuestraC;

                result = XMLActualizaMuestra(muestra);
            }

            return result;
        }

        public bool XMLActualizaMuestra(Muestra args)
        {
            Muestra muestra = new Muestra();

            try
            {
                muestra = Repositorio.First(p => p.MuestraID == args.MuestraID);
                muestra.MuestraID = args.MuestraID;
                muestra.AnalisisID = args.AnalisisID;
                muestra.Cancelado = args.Cancelado;
                muestra.Caudal = args.Caudal;
                muestra.Datum = args.Datum;
                muestra.Descripcion = args.Descripcion;
                muestra.DireccionID = args.DireccionID;
                muestra.Fecha = args.Fecha;
                muestra.FechaRecepcion = args.FechaRecepcion;
                muestra.FlagCompuesta = args.FlagCompuesta;
                muestra.Identificacion = args.Identificacion;
                muestra.Latitud = args.Latitud;
                muestra.Longitud = args.Longitud;
                muestra.NivelID = args.NivelID;
                muestra.Numero = args.Numero;
                muestra.Observaciones = args.Observaciones;
                muestra.ValHojaCampo = args.ValHojaCampo;
                muestra.ValMuestraC = args.ValMuestraC;

                Repositorio.SaveChanges();
                muestra = Repositorio.Find(x => x.MuestraID == muestra.MuestraID).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Muestra ObtenerMuestaCompuestaByAnalisisId(int analisisID)
        {
            var resultado = Repositorio.First(x => x.AnalisisID == analisisID && x.NivelID == (int)enumNivel.Compuesta);

            return resultado;
        }

        public bool ActualizaFechaMuestra(int muestraID, DateTime Fecha)
        {
            bool resultado = false;
            var muestraC = Repositorio.First(x => x.MuestraID == muestraID);

            if (muestraC != null)
            {
                try
                {
                    muestraC.Fecha = Fecha;
                    muestraC.FechaConformacion = Fecha;
                    Repositorio.SaveChanges();
                    resultado = true;
                }
                catch (Exception ex)
                {
                    resultado = false;
                }
                
            }

            return resultado;
        }
    }
}