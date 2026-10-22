using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIRALAB_BL.seguridad
{
    public class CatModulos_BL : Base_BL<CatModulo>
    {
        public CatModulos_BL() : base() { }

        public IEnumerable <CatModulo> ObtenerTodos()
        {
            return  Repositorio.GetAll().ToList();
        }

        public CatModulo Obtener(int moduloID)
        {
            this.Entidad = Repositorio.Single(a => a.ModuloID == moduloID);
            return Entidad;
        }


        public CatModulo Guardar(SIRALAB_BL.common.CatModulosArgBase_BL argBase)
        {
            try
            {
                if (argBase.ModuloID != null)
                    Obtener((int)argBase.ModuloID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    
                    this.Entidad.ModuloPadreID = argBase.ModuloPadreID;
                    this.Entidad.Nombre = argBase.Nombre;
                    this.Entidad.Descripcion = argBase.Descripcion;
                    this.Entidad.Orden = argBase.Orden;
                    this.Entidad.URL = argBase.URL;
                    this.Entidad.CssClass = argBase.CssClass;

                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CatModulo();
                    this.Entidad.ModuloID = Repositorio.NextNewID(x => x.ModuloID);
                    this.Entidad.ModuloPadreID = argBase.ModuloPadreID;
                    this.Entidad.Nombre = argBase.Nombre;
                    this.Entidad.Descripcion = argBase.Descripcion;
                    this.Entidad.Orden = argBase.Orden;
                    this.Entidad.URL = argBase.URL;
                    this.Entidad.CssClass = argBase.CssClass;

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

        public bool Elimina(int moludoID)
        {
            bool result = false;

            try
            {
                    Obtener((int)moludoID);
                

                if (this.Entidad != null)
                {
                    if (Entidad.CatModulos1.Count() > 0)
                    {
                        result = false;
                        throw new Exception("No se puede eliminar el modulo, porque tiene dependencias.");
                        
                    }
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
    }
}
