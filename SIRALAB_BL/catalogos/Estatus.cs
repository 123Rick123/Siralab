using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class Estatus_BL : Base_BL<Estatu>
    {
        public Estatus_BL() : base()
        {

        }

        public IEnumerable<Estatu> Obtener()
        {
            return Repositorio.GetAll().OrderBy(p => p.EstatusTipoID).OrderBy(p => p.Nombre);
        }

        public IEnumerable<Estatu> Obtener(enumEstatusTipo estatusTipo)
        {
            int estatusTipoID = estatusTipo.ParseToInt();
            return Repositorio.Find(p => p.EstatusTipoID == estatusTipoID).OrderBy(p => p.Nombre);
        }
    }
}
