using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class CatDiaInhabil_BL : Base_BL<CatDiaInhabil>
    {
        public CatDiaInhabil_BL() : base() { }

        public IEnumerable<CatDiaInhabil> ObtenerTodos()
        {
            return Repositorio.GetAll().ToList().OrderByDescending(dia => dia.DiaInhabil.Value.Date).ToList();
        }

        public CatDiaInhabil Obtener(int diaInhabilID)
        {
            this.Entidad = Repositorio.Single(a => a.DiaInhabilID == diaInhabilID);
            return Entidad;
        }

        public CatDiaInhabil Guardar(SIRALAB_BL.common.CatDiaInhabilArgBase_BL argBase)
        {
            try
            {
                if (argBase.DiaInhabilId != null)
                    this.Obtener((int)argBase.DiaInhabilId);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.DiaInhabil = argBase.DiaInhabil;

                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new CatDiaInhabil();
                    this.Entidad.DiaInhabilID = Repositorio.NextNewID(x => x.DiaInhabilID);
                    this.Entidad.DiaInhabil = argBase.DiaInhabil;
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

        public bool Elimina(int diaInhabilID)
        {
            bool result = false;

            try
            {

                this.Obtener((int)diaInhabilID);


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

      

        public bool TieneDuplicidad(DateTime diaInhabil, int? diaInhabilID)
        {
            bool result = false;

            this.Entidad = ObtenerTodos().Where(a => a.DiaInhabil == diaInhabil).SingleOrDefault();

            try
            {
                if (this.Entidad != null)
                {
                    if (diaInhabilID == null)
                        result = true;
                    else
                    {
                        if (diaInhabilID != this.Entidad.DiaInhabilID)
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
