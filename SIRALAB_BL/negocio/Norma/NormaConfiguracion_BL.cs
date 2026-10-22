using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIRALAB_BL.catalogos
{
    public class NormaConfiguracion_BL : Base_BL<NormaConfiguracion>
    {

        public NormaConfiguracion_BL() : base() { }

        public IEnumerable<NormaConfiguracion> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public IEnumerable<NormaConfiguracion> ObtenerNormaConfiguracion(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            return Repositorio.Find(a =>
                a.NormaID == NormaID &&
                a.CuerpoReceptorID == CuerpoReceptorID &&
                a.UsoAguaID == UsoAguaID &&
                a.NivelID == NivelID).OrderBy(b => b.Orden);
        }

        public NormaConfiguracion Obtener(int normaConfiguracionID)
        {
            this.Entidad = Repositorio.Single(a => a.NormaConfiguracionID == normaConfiguracionID);
            return Entidad;
        }

        public IEnumerable<NormaConfiguracion> Obtener_Grupos(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            IEnumerable<NormaConfiguracion> Listado = new List<NormaConfiguracion>();
            Listado = Repositorio.Find(a => a.NormaID == NormaID && a.CuerpoReceptorID == CuerpoReceptorID && a.UsoAguaID == UsoAguaID && a.NivelID == NivelID && a.Grupo);
            return Listado;
        }

        public NormaConfiguracion Obtener_ParametroMetodo(int ParametroID, int NormaID, int CuerpoReceptorID, int UsoAguaID, int PromedioID)
        {
            this.Entidad = Repositorio.First(a => a.ParametroID == ParametroID && a.NormaID == NormaID && 
                                              a.CuerpoReceptorID == CuerpoReceptorID && a.UsoAguaID == UsoAguaID && a.NivelID == PromedioID);
            return Entidad;
        }

        public NormaConfiguracion Obtener_NormaConfiguracion(int NormaID, int ParametroID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            /*
            this.Entidad = Repositorio.First(a => a.ParametroMetodo.ParametroID == ParametroID && a.NormaID == NormaID && 
                                                   a.CuerpoReceptorID == CuerpoReceptorID && a.UsoAguaID == UsoAguaID && a.PromedioID == PromedioID);
            return Entidad;
             */
            return new NormaConfiguracion();
        }

        public List<CuerpoReceptor> Lista_CuerpoReceptor(int NormaID)
        {
            var listaNC = Repositorio.Find(x => x.NormaID == NormaID).Select(p => p.CuerpoReceptor).Distinct().ToList();
            return listaNC;
        }

        public List<CatUsoAgua> Lista_UsoAgua(int NormaID, int CuerpoReceptorID)
        {
            var listaUsoAgua = Repositorio.Find(x => x.NormaID == NormaID && x.CuerpoReceptorID == CuerpoReceptorID).Select(p => p.CatUsoAgua).Distinct().ToList();
            return listaUsoAgua;
        }

        public List<usp_ParametrosMetodosNorma_Result> ObtenerNormaConfiguracionByNormaID(int NormaId)
        {
            Siralab_Entities db = new Siralab_Entities();
            List<usp_ParametrosMetodosNorma_Result> listaPMN = new List<usp_ParametrosMetodosNorma_Result>();
            
            try
            {
                listaPMN = db.usp_ParametrosMetodosNorma(NormaId).ToList();
            }
            catch (Exception ex)
            {

            }

            return listaPMN;
        }

        public NormaConfiguracion Guardar(SIRALAB_BL.common.NormaConfiguracionArgBase_BL argBase)
        {
            try
            {
                if (argBase.NormaConfiguracionID != null)
                    Obtener((int)argBase.NormaConfiguracionID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.NormaID = argBase.NormaID;
                    this.Entidad.NormaConfiguracionPadreID = argBase.NormaConfiguracionPadreID;
                    this.Entidad.CuerpoReceptorID = argBase.CuerpoReceptorID;
                    this.Entidad.UsoAguaID = argBase.UsoAguaID;
                    this.Entidad.NivelID = argBase.NivelID;
                    this.Entidad.OperacionID = argBase.OperacionID;
                    this.Entidad.Orden = argBase.Orden == 0 ? Siguiente_Orden(argBase.UsoAguaID, argBase.CuerpoReceptorID, argBase.NivelID, argBase.NormaID) : argBase.Orden;
                    this.Entidad.AceptaValor = argBase.AceptaValor;
                    this.Entidad.LimiteInferior = argBase.LimiteInferior;
                    this.Entidad.LimiteSuperior = argBase.LimiteSuperior;
                    this.Entidad.Activo = argBase.Activo;
                    this.Entidad.Grupo = argBase.Grupo;
                    this.Entidad.Exenta = argBase.Exenta;
                    this.Entidad.LFD = argBase.LFD;
                    this.Entidad.LimiteInferiorLFD = argBase.LimiteInferiorLFD;
                    this.Entidad.LimiteSuperiorLFD = argBase.LimiteSuperiorLFD;
                    this.Entidad.Evalua = argBase.Evalua;
                    this.Entidad.FechaDesactivacion = argBase.FechaDesactivacion;
                    this.Entidad.EscalaDecimal = argBase.EscalaDecimal;
                    this.Entidad.EscalaDecimalLCM = argBase.EscalaDecimalLCM;
                    this.Entidad.LCMC = argBase.LCMC;
                    this.Entidad.Etiqueta = argBase.Etiqueta;
                    this.Entidad.Truncamiento = argBase.Truncamiento;
                    this.Entidad.LimitaCaptura = argBase.LimitaCaptura;
                    this.Entidad.MinAceptado = argBase.MinAceptado;
                    this.Entidad.MaxAceptado = argBase.MaxAceptado;
                    
                    Repositorio.SaveChanges();
                    this.Mensaje = "Exito";
                }
                else  //Nuevo
                {
                    if (!Existe(argBase.ParametroID, argBase.UsoAguaID, argBase.CuerpoReceptorID, argBase.NivelID, argBase.NormaID))
                    {

                        this.Entidad = new NormaConfiguracion();
                        this.Entidad.NormaConfiguracionID = Repositorio.NextNewID(x => x.NormaConfiguracionID);
                        this.Entidad.NormaConfiguracionPadreID = argBase.NormaConfiguracionPadreID;
                        this.Entidad.NormaID = argBase.NormaID;
                        this.Entidad.ParametroID = argBase.ParametroID;
                        this.Entidad.CuerpoReceptorID = argBase.CuerpoReceptorID;
                        this.Entidad.UsoAguaID = argBase.UsoAguaID;
                        this.Entidad.NivelID = argBase.NivelID;
                        this.Entidad.OperacionID = argBase.OperacionID;
                        this.Entidad.Orden = Siguiente_Orden(argBase.UsoAguaID, argBase.CuerpoReceptorID, argBase.NivelID, argBase.NormaID);
                        this.Entidad.AceptaValor = argBase.AceptaValor;
                        this.Entidad.LimiteInferior = argBase.LimiteInferior;
                        this.Entidad.LimiteSuperior = argBase.LimiteSuperior;
                        this.Entidad.Activo = argBase.Activo;
                        this.Entidad.Grupo = argBase.Grupo;
                        this.Entidad.Exenta = argBase.Exenta;
                        this.Entidad.LFD = argBase.LFD;
                        this.Entidad.LimiteInferiorLFD = argBase.LimiteInferiorLFD;
                        this.Entidad.LimiteSuperiorLFD = argBase.LimiteSuperiorLFD;
                        this.Entidad.Evalua = argBase.Evalua;
                        this.Entidad.FechaDesactivacion = argBase.FechaDesactivacion;
                        this.Entidad.FechaRegistro = DateTime.Now;
                        this.Entidad.EscalaDecimal = argBase.EscalaDecimal;
                        this.Entidad.EscalaDecimalLCM = argBase.EscalaDecimalLCM;
                        this.Entidad.LCMC = argBase.LCMC;
                        this.Entidad.Etiqueta = argBase.Etiqueta;
                        this.Entidad.Truncamiento = argBase.Truncamiento;
                        this.Entidad.LimitaCaptura = argBase.LimitaCaptura;
                        this.Entidad.MinAceptado = argBase.MinAceptado;
                        this.Entidad.MaxAceptado = argBase.MaxAceptado;

                        Repositorio.Add(Entidad);
                        Repositorio.SaveChanges();
                        this.Mensaje = "Exito";
                    }
                    else
                    {
                        this.Entidad = null;
                        if (argBase.Grupo)
                        {
                            this.Mensaje = "El grupo o parámetro ya se encuentra registrado en esta norma.";
                        }
                        else
                        {
                            this.Mensaje = "El parámetro ya se encuentra registrado en esta norma.";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                this.Mensaje = "Error "+ex.Message;
                this.Entidad = null;
               
            }

            return this.Entidad;
        }

        public NormaConfiguracion Guardar(int NormaConfiguracionID)
        {
            Obtener(NormaConfiguracionID);

            this.Entidad.Activo = !this.Entidad.Activo;
            this.Entidad.FechaDesactivacion = this.Entidad.Activo ? null : (DateTime?)DateTime.Now;
            
            Repositorio.SaveChanges();
            return this.Entidad;
        }

        public void Eliminar(int NormaConfiguracionID)
        {
            var delElement = Repositorio.First(p => p.NormaConfiguracionID == NormaConfiguracionID);
            this.Mensaje = "";

            if (delElement.NormaConfiguracion1.Count > 0)
            {
                this.Mensaje = "El parámetro tiene asociaciones activas y no se puede eliminar.";
                return;
            }
            try
            {
                Repositorio.Delete(delElement);
            }
            catch (Exception e)
            {
                this.Mensaje = "El registro no se ha podido eliminar. <br /><br />" + e.Message;
            }
        }

        public void CambiarOrden(int NormaConfiguracionID, bool Subir)/* Subir true sí sube, false sí baja */
        {
            Siralab_Entities se = new Siralab_Entities();
            se.usp_OrdenNormaConfiguracion(NormaConfiguracionID, Subir);
        }
        
        public bool Existe(int parametroID, int usoAguaID,int cuerpoReceptorID, int nivelID, int normaID)
        {
            bool result = false;

            try
            {
                this.Entidad = ObtenerTodos().Where(a => a.UsoAguaID == usoAguaID && a.CuerpoReceptorID == cuerpoReceptorID &&
                                                     a.NivelID == nivelID && a.NormaID == normaID &&
                                                     a.ParametroID == parametroID).FirstOrDefault();/**/

                if (this.Entidad != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public int Siguiente_Orden(int usoAguaID, int cuerpoReceptorID, int nivelID, int normaID)
        {
            int Orden = 0;
            var Listado = ObtenerTodos().Where(a => a.UsoAguaID == usoAguaID && a.CuerpoReceptorID == cuerpoReceptorID &&
                                                 a.NivelID == nivelID && a.NormaID == normaID);

            if (Listado.Count() > 0)
            {
                Orden = Listado.Max(p => p.Orden);
            }

            return Orden + 1;
        }
    }
}
