using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class CatTipoCuerpoReceptor_BL : Base_BL<CatTipoCuerpoReceptor> 
    {
        public CatTipoCuerpoReceptor_BL() : base() { }

        public IEnumerable<CatTipoCuerpoReceptor> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList();
        }

        public CatTipoCuerpoReceptor Obtener(int tipoCuerpoReceptorID)
        {
            this.Entidad = Repositorio.Single(a => a.TipoCuerpoReceptorID == tipoCuerpoReceptorID);
            return Entidad;
        }

        public CatTipoCuerpoReceptor Guardar(SIRALAB_BL.common.CatTipoCuerpoReceptorArgBase_BL argBase)
        {
            try
            {
                if (argBase.TipoCuerpoReceptorID != null)
                    this.Obtener((int)argBase.TipoCuerpoReceptorID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.Nombre = argBase.Nombre;
                   
                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CatTipoCuerpoReceptor();
                    this.Entidad.TipoCuerpoReceptorID = Repositorio.NextNewID(x => x.TipoCuerpoReceptorID);
                    this.Entidad.Nombre = argBase.Nombre;
                    Repositorio.Add(Entidad);
                    Repositorio.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                this.Entidad = null;
                throw new Exception(ex.Message, ex.InnerException);
            }

            return this.Entidad;
        }

        public bool Elimina(int tipoCuerpoReceptorID)
        {
            bool result = false;
            try
            {
                
                    this.Obtener((int)tipoCuerpoReceptorID);
               

                if (this.Entidad != null )
                {
                    if (Entidad.CuerpoReceptors.Select(c => c).Count() != 0)
                        throw new Exception("No se puede eliminar el registro, porque tiene depedencias.");
                    else
                    {
                        Repositorio.Delete(this.Entidad);
                        Repositorio.SaveChanges();
                        result = true;
                    }
                }
                else 
                    result = false; 
                
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public bool TieneDependencia(int tipoCuerpoReceptorId)
        {
            bool result = false;

            this.Entidad = ObtenerTodos().Where(a => a.TipoCuerpoReceptorID == tipoCuerpoReceptorId).SingleOrDefault();

            try
            {
                if (this.Entidad != null)
                {
                    if (this.Entidad.CuerpoReceptors != null)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public bool TieneDuplicidad(string nombre, int? tipoCuerpoReceptorId)
        {
            bool result = false;

            this.Entidad = ObtenerTodos().Where(a => a.Nombre.ToUpper().Equals(nombre.ToUpper())).SingleOrDefault();

            try
            {
                if (this.Entidad != null)
                {
                    if (tipoCuerpoReceptorId == null)
                        result = true;
                    else
                    {
                        if (tipoCuerpoReceptorId != this.Entidad.TipoCuerpoReceptorID)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }
       
    }
}
