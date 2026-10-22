using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class ParametroMetodo_BL : Base_BL<ParametroMetodo>
    {
        public ParametroMetodo_BL() : base() { }

        public IEnumerable<ParametroMetodo> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public List<ParametroMetodo> ObtenerTodosByParametroId(int ParametroId)
        {
            List<ParametroMetodo> listaPMN = new List<ParametroMetodo>();

            try
            {
                Siralab_Entities db = new Siralab_Entities();

                listaPMN = db.ParametroMetodoes.Where(a => a.ParametroID == ParametroId).ToList(); ;
            }
            catch (Exception ex)
            { 
            
            }

            return listaPMN;
        }

        public ParametroMetodo Obtener(int parametroMetodoID)
        {
            this.Entidad = Repositorio.Single(a => a.ParametroMetodoID == parametroMetodoID);
            return Entidad;
        }

        public ParametroMetodo Obten(int ParametroID, int MetodoID)
        {
            var parametro_metodo = Repositorio.Find(x => x.ParametroID == ParametroID && x.MetodoID == MetodoID).FirstOrDefault();
            return parametro_metodo;
        }

    }
}
