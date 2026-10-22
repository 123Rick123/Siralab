using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.seguridad
{

    public class CatUsuarioRol_BL : Base_BL<CatUsuarioRol>
    {
        public CatUsuarioRol_BL() : base() { }

        public IEnumerable<CatUsuarioRol> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList();
        }

        public CatUsuarioRol Obtener(int usuarioRolID)
        {
            this.Entidad = Repositorio.Single(a => a.UsuarioRolID == usuarioRolID);
            return Entidad;
        }

        public CatUsuarioRol Guardar(SIRALAB_BL.common.CatUsuarioRolArgBase_BL argBase)
        {
            try
            {
                if (argBase.UsuarioRolID != null)
                    Obtener((int)argBase.UsuarioRolID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.Nombre = argBase.Nombre;
                    this.Entidad.Administrable = argBase.Administrable;
                    this.Entidad.FechaRegistro = DateTime.Now;
                    this.Entidad.Activo = argBase.Activo;
                    
                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CatUsuarioRol();
                    this.Entidad.UsuarioRolID = Repositorio.NextNewID(x => x.UsuarioRolID);
                    this.Entidad.Nombre = argBase.Nombre;
                    this.Entidad.Administrable = argBase.Administrable;
                    this.Entidad.FechaRegistro = DateTime.Now;
                    this.Entidad.Activo = argBase.Activo;
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

        public bool Elimina(int usuarioRolID)
        {
            bool result = false;

            try
            {
                Obtener((int)usuarioRolID);
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
    }
}
