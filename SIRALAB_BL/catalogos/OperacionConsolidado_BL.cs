using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;

namespace SIRALAB_BL.catalogos
{
    public class OperacionConsolidado_BL : Base_BL<OperacionConsolidado>
    {
        public OperacionConsolidado_BL()
            : base()
        {

        }

        public IEnumerable<OperacionConsolidado> Obtener()
        {
            return Repositorio.GetAll();
        }
    }
}
