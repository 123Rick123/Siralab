using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL.common;

namespace SIRALAB_BL.seguridad
{

    public class CatPagina_BL : Base_BL<CatPagina>
    {
        public CatPagina_BL() : base() { }

        int SistemaId
        {
            get
            {
                ApplicationSettings ap = new ApplicationSettings();
                return int.Parse(ap.SistemaId);
            }
        }
        public IEnumerable<CatPagina> ObtenerTodos()
        {

            return Repositorio.Find(a=>a.SistemaID== SistemaId);
        }

        public CatPagina Obtener(int paginaID)
        {
            this.Entidad = Repositorio.Single(a => a.PaginaID == paginaID && a.SistemaID == SistemaId);
            return Entidad;
        }
        public CatPagina Obtener(string pagina)
        {
            this.Entidad = Repositorio.Single(a => a.URL == pagina && a.SistemaID== SistemaId);
            return Entidad;
        }

        public CatPagina Guardar(SIRALAB_BL.common.CatPaginaArgBase_BL argBase)
        {
            try
            {


                if (argBase.PaginaID != null)
                    Obtener((int)argBase.PaginaID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {

                    this.Entidad.ModuloID = argBase.ModuloID;
                    this.Entidad.SistemaID = SistemaId;
                    this.Entidad.Orden = argBase.Orden;
                    this.Entidad.CssClass = argBase.CssClass;
                    this.Entidad.URL = argBase.URL;
                    this.Entidad.Descripcion = argBase.Descripcion;
                    this.Entidad.VisibleMenu = argBase.VisibleMenu;
                    this.Entidad.Activo = argBase.Activo;

                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CatPagina();
                    this.Entidad.PaginaID = Repositorio.NextNewID(x => x.PaginaID);

                    this.Entidad.ModuloID = argBase.ModuloID;
                    this.Entidad.SistemaID = SistemaId;
                    this.Entidad.Orden = argBase.Orden;
                    this.Entidad.CssClass = argBase.CssClass;
                    this.Entidad.URL = argBase.URL;
                    this.Entidad.Descripcion = argBase.Descripcion;
                    this.Entidad.VisibleMenu = argBase.VisibleMenu;
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

        public bool Elimina(int PaginaID)
        {
            bool result = false;

            try
            {
                Obtener((int)PaginaID);


                if (this.Entidad != null)
                {
                    if (Entidad.CatPermisosRols.Count() > 0)
                    {
                        result = false;
                        throw new Exception("No se puede eliminar está página, porque tiene dependencias.");

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
