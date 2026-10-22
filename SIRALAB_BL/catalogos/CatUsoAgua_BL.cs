using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class CatUsoAgua_BL : Base_BL<CatUsoAgua>
    {
        public CatUsoAgua_BL() : base() { }

        public IEnumerable<CatUsoAgua> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList();
        }

        public CatUsoAgua Obtener(int usoAguaID)
        {
            this.Entidad = Repositorio.Single(a => a.UsoAguaID == usoAguaID);
            return Entidad;
        }

        public CatUsoAgua Guardar(SIRALAB_BL.common.CatUsoAguaArgBase_BL argBase)
        {
            try
            {
                if (argBase.UsoAguaID != null)
                    this.Obtener((int)argBase.UsoAguaID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.Nombre = argBase.Nombre;
                    
                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CatUsoAgua();
                    this.Entidad.UsoAguaID = Repositorio.NextNewID(x => x.UsoAguaID);
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

        public bool Elimina(int usoAguaID)
        {
            bool result = false;
            
            try
            {
              
                    this.Obtener((int)usoAguaID);
               

                if (this.Entidad != null)
                {
                    Repositorio.Delete(this.Entidad);
                    Repositorio.SaveChanges();
                    result = true;
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

        public bool TieneDependencia(int usoAguaId)
        {
            bool result = false;

            this.Entidad = ObtenerTodos().Where(a => a.UsoAguaID == usoAguaId).SingleOrDefault();

            try
            {
                if (this.Entidad != null)
                {
                    if (this.Entidad.NormaConfiguracions != null)
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

        public bool TieneDuplicidad(string nombre, int? usoAguaID)
        {
            bool result = false;

            this.Entidad = ObtenerTodos().Where(a => a.Nombre.ToUpper().Equals(nombre.ToUpper())).SingleOrDefault();

            try
            {
                if (this.Entidad != null)
                {
                    if (usoAguaID == null)
                        result = true;
                    else
                    {
                        if (usoAguaID != this.Entidad.UsoAguaID)
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
