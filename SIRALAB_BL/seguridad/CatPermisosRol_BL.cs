using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL.common;

namespace SIRALAB_BL.seguridad
{
    public class CatPermisosRol_BL : Base_BL<CatPermisosRol>
    {
        public CatPermisosRol_BL() : base() { }

        public IEnumerable<CatPermisosRol> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList().Where(a=> a.CatPagina.SistemaID == SistemaId);
        }

        int SistemaId
        {
            get
            {
                ApplicationSettings ap = new ApplicationSettings();
                return int.Parse(ap.SistemaId);
            }
        }

        public CatPermisosRol Obtener(int permisorolID)
        {
            this.Entidad = Repositorio.Single(a => a.PermisosRolID == permisorolID && a.CatPagina.SistemaID == SistemaId );
            return Entidad;
        }


        public CatPermisosRol Obtener(int UsuarioRolID,int PaginaID)
        {
            this.Entidad = Repositorio.Single(a => a.UsuarioRolID == UsuarioRolID && a.PaginaID == PaginaID && a.CatPagina.SistemaID == SistemaId);
            return Entidad;
        }

        public CatPermisosRol Guardar(SIRALAB_BL.common.CatPermisosRolArgBase_BL argBase)
        {
            try
            {
                if (argBase.PermisosRolID != null)
                    Obtener((int)argBase.PermisosRolID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) 
                {
                    this.Entidad.UsuarioRolID = argBase.UsuarioRolID;
                    this.Entidad.PaginaID = argBase.PaginaID;
                    this.Entidad.Escritura = argBase.Escritura;
                    this.Entidad.Activo = argBase.Activo;
                    Repositorio.SaveChanges();
                }
                else  
                {
                    this.Entidad = new CatPermisosRol();
                    this.Entidad.PermisosRolID = Repositorio.NextNewID(x => x.PermisosRolID);

                    this.Entidad.UsuarioRolID = argBase.UsuarioRolID;
                    this.Entidad.PaginaID = argBase.PaginaID;
                    this.Entidad.Escritura = argBase.Escritura;
                    this.Entidad.Activo = true;
                    this.Entidad.FechaRegistro = DateTime.Now;

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

        public bool Elimina(int PermisosRolID)
        {
            bool result = false;

            try
            {
                Obtener((int)PermisosRolID);


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
