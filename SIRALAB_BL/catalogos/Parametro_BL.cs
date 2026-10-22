using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;

namespace SIRALAB_BL.catalogos
{
    public class Parametro_BL : Base_BL<Parametro>
    {
        public Parametro_BL() : base() { }

        public IEnumerable<Parametro> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public IEnumerable<Parametro> ObtenerTodos(int? analsisId, int? laboratorioId = null, int? metodoId = null, string parametro = null)
        {
            Siralab_Entities se = new Siralab_Entities();
            var resultado = se.usp_ParametrosAdicionales(analsisId, laboratorioId, metodoId, parametro);
            return resultado.Select(p => new Parametro
            {
                ParametroID = p.ParametroID,
                Nombre = p.Parametro
            });
        }

        public IQueryable<Parametro> ObtenerQueryable(Expression<Func<Parametro, bool>> predicate)
        {
            var resultado = Repositorio.FindQueryable(predicate);
            return resultado;
        }

        public Parametro Obtener(int parametroID)
        {
            this.Entidad = Repositorio.Single(a => a.ParametroID == parametroID);
            return Entidad;
        }
    }
}
