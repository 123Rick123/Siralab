using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_BL;
using SIRALAB_DAL;

namespace SIRALAB_BL
{
    public class FrecuenciaMuestreo_BL
    {
        public Siralab_Entities FrecuenciaMuestreoContext { get; private set; }
        public FrecuenciaMuestreo FrecuenciaMuestreo { get; private set; }

        Repository<FrecuenciaMuestreo > Repositorio;

        public FrecuenciaMuestreo_BL()
        {
            FrecuenciaMuestreoContext = new Siralab_Entities();
            Repositorio = new Repository<FrecuenciaMuestreo>(FrecuenciaMuestreoContext);
        }

        public List<FrecuenciaMuestreo> Listado_FrecuenciaMuestreo()
        {
            var Listado = Repositorio.GetAll().ToList();
            return Listado;
        }
    }
}
