using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepuraACE.Logic
{
    class DepuracionColasBL
    {
        private SiralabEntities _entities;

        public DepuracionColasBL()
        {
            _entities = new SiralabEntities();
        }


        public void Depurar()
        {
            _entities.usp_DepuraACE();

        }

    }
}
