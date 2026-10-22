using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.analisis
{
    public class AnalisisFirma_BL : Base_BL<AnalisisFirma>
    {
        public AnalisisFirma_BL() : base() { }

        public IEnumerable<AnalisisFirma> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public AnalisisFirma Obtener(int analisisFirmaID)
        {
            this.Entidad = Repositorio.Single(a => a.AnalisisFirmaID == analisisFirmaID);
            return Entidad;
        }

        public AnalisisFirma Guardar(SIRALAB_BL.common.AnalisisFirmaArgBase_BL argBase)
        {
            AnalisisFirma analisisFirmaReg = null;
            try
            {
                var analisis = new Analisis_BL();
                var result = analisis.Obten_Analisis(argBase.AnalisisID);
                //if (argBase.AnalisisFirmaID != null)
                //    this.Obtener((int)argBase.AnalisisFirmaID);
                //else
                //    this.Entidad = null;

                //if (this.Entidad != null) //Actualiza
                //{
                //    this.Entidad.AnalisisID = argBase.AnalisisID;
                //    this.Entidad.RFC = argBase.RFC;
                //    this.Entidad.Firma = argBase.Firma;
                //    this.Entidad.Cadena = argBase.Cadena;
                //    this.Entidad.Contenedor = argBase.Contenedor;
                //    this.Entidad.FechaRegistro = argBase.FechaRegistro;
                //    this.Entidad.SelloDigital = argBase.SelloCNA;
                //    Repositorio.SaveChanges();
                //}
                //else  //Nuevo
                //{
                if (result.AnalisisFirmas.Count == 0)
                {
                     analisisFirmaReg = new AnalisisFirma
                    {
                        AnalisisFirmaID = Repositorio.NextNewID(x => x.AnalisisFirmaID),
                        AnalisisID = argBase.AnalisisID,
                        RFC = argBase.RFC,
                        Firma = argBase.Firma,
                        Cadena = argBase.Cadena,
                        Contenedor = argBase.Contenedor,
                        FechaRegistro = argBase.FechaRegistro,
                        SelloDigital = argBase.SelloCNA
                    };

                    Repositorio.Add(analisisFirmaReg);
                Repositorio.SaveChanges();

                        var analisisBl = new Analisis_BL();
                      analisisBl.ActualizarAnalisisAFirmado(argBase.AnalisisID);
                }
                else
                {
                    analisisFirmaReg = null;
                }
                //}
            }

            catch (Exception ex)
            {
                this.Entidad = null;
                throw new Exception(ex.Message, ex.InnerException);
            }
            return analisisFirmaReg;
           // return this.Entidad;
        }
        public AnalisisFirma Delete(SIRALAB_BL.common.AnalisisFirmaArgBase_BL argBase)
        {
            AnalisisFirma analisisFirmaReg = null;
            try
            {
                var analisis = new Analisis_BL();
                var result = analisis.Obten_Analisis(argBase.AnalisisID);
               
                if (result.AnalisisFirmas.Count > 0)
                {
                    analisisFirmaReg = new AnalisisFirma
                    {
                        AnalisisFirmaID = result.AnalisisFirmas.Where(x=>x.AnalisisID==argBase.AnalisisID).First().AnalisisFirmaID,
                        AnalisisID = argBase.AnalisisID,
                        RFC = argBase.RFC,
                        Firma = argBase.Firma,
                        Cadena = argBase.Cadena,
                        Contenedor = argBase.Contenedor,
                        FechaRegistro = argBase.FechaRegistro,
                        SelloDigital = argBase.SelloCNA
                    };

                    Repositorio.Delete(analisisFirmaReg);
                    Repositorio.SaveChanges();

                    var analisisBl = new Analisis_BL();
                
                }
                else
                {
                    analisisFirmaReg = null;
                }
                //}
            }

            catch (Exception ex)
            {
                this.Entidad = null;
                throw new Exception(ex.Message, ex.InnerException);
            }
            return analisisFirmaReg;
            // return this.Entidad;
        }

        public bool IfAnalisisFirmado(int AnalisisID)
        {

            var result = Repositorio.Single(a => a.AnalisisID == AnalisisID);
            if (result != null && result != (new AnalisisFirma()))
                return true;
            return false;
        
        }


        public AnalisisFirma FindByAnalisisID(int analisisID)
        {
            try
            {
                return Repositorio.Find(a => a.AnalisisID == analisisID).FirstOrDefault();
            }
            catch (Exception e)
            {
                return new AnalisisFirma(); ;
            }
        }

        public void DeleteByAnalisisID(int analisisID)
        {
            AnalisisFirma analisisFirmaReg = null;
            try
            {
                var analisis = FindByAnalisisID(analisisID);
                Repositorio.Delete(analisis);
                Repositorio.SaveChanges();
            }

            catch (Exception ex)
            {
                this.Entidad = null;
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
