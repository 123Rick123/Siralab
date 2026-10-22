using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;

namespace SIRALAB_BL
{
    public class Metodo_BL : Base_BL<Metodo>
    {
        //public Siralab_Entities MetodoValorContext { get; private set; }
        //public MuestraValor Metodo_ { get; private set; }

        //Repository<Metodo> Repositorio;

        public Metodo_BL() : base()
        {
            //MetodoValorContext = new Siralab_Entities();
            //Repositorio = new Repository<Metodo>(MetodoValorContext);
        }

        public List<Metodo> Obten_Metodo(int LaboratorioID, int ParametroID)
        {
            var list = this.dbContext.usp_MetodosLaboratorio(LaboratorioID, ParametroID).ToList();
            List<Metodo> Lista_Metodo = list.Select(i => new Metodo
            {
                MetodoID = i.MetodoID,
                Nombre = i.Nombre,
                Descripcion = i.Descripcion
            }).ToList();
            
            return Lista_Metodo;
        }

        public List<Metodo> Obten_Metodos()
        {
            var list = this.dbContext.Metodoes.Where(x => x.Activo).ToList();
            return list;
        }
    }
}