using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;

namespace SIRALAB_BL.catalogos
{
    public class CuerpoReceptor_BL : Base_BL<CuerpoReceptor>
    {
        public CuerpoReceptor_BL() : base() { }

        public IEnumerable<CuerpoReceptor> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList();
        }

        public CuerpoReceptor Obtener(int usoAguaID)
        {
            this.Entidad = Repositorio.Single( a => a.CuerpoReceptorID == usoAguaID);
            return Entidad;
        }

        public CuerpoReceptor Guardar(SIRALAB_BL.common.CuerpoReceptorArgBase_BL argBase)
        {
            try
            {
                if (argBase.CuerpoReceptorID != null)
                   Obtener ((int)argBase.CuerpoReceptorID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.Nombre = argBase.Nombre;
                    this.Entidad.TipoCuerpoReceptorID = argBase.TipoCuerpoReceptorID;
                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CuerpoReceptor();
                    this.Entidad.CuerpoReceptorID = Repositorio.NextNewID(x => x.CuerpoReceptorID);
                    this.Entidad.Nombre = argBase.Nombre;
                    this.Entidad.TipoCuerpoReceptorID = argBase.TipoCuerpoReceptorID;
                    Repositorio.Add(this.Entidad);
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
                this.Obtener(usoAguaID);
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

        public bool TieneDependencia(int cuerpoReceptorId)
        {
            bool result = false;

            this.Entidad = ObtenerTodos().Where(a => a.CuerpoReceptorID == cuerpoReceptorId).SingleOrDefault();

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

        public bool TieneDuplicidad(string nombre, int? cuerpoReceptorID)
        {
            bool result = false;


            this.Entidad = ObtenerTodos().Where(a => a.Nombre.ToUpper().Equals(nombre.ToUpper())).SingleOrDefault();

            try
            {
                if (this.Entidad != null)
                {
                    if (cuerpoReceptorID == null)
                        result = true;
                    else
                    {
                        if (cuerpoReceptorID != this.Entidad.CuerpoReceptorID)
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

        public List<CuerpoReceptor> Filtrar(common.CuerpoReceptorArgBase_BL filtro)
        {
            List<CuerpoReceptor> cuerpoReceptorResp = Repositorio.GetAll().ToList();
                       
            cuerpoReceptorResp = cuerpoReceptorResp.Where(p =>
                ((filtro.Nombre == null || filtro.Nombre == "") ? true : p.Nombre.Like(filtro.Nombre.FilterSQLData())) &&
                ((filtro.TipoCuerpoReceptorID == 0)? true: p.TipoCuerpoReceptorID == filtro.TipoCuerpoReceptorID)
                ).ToList();

            return cuerpoReceptorResp;
        }

    }
}
