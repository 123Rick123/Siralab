using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.ordentrabajo
{
    public class Correspondencia_CR_UA_BL
    {
        public Siralab_Entities SiralabContext { get; private set; }

        public Correspondencia_CR_UA_BL()
        {
            SiralabContext = new Siralab_Entities();
        }

        public int ObtenerCuerpoReceptorID(Int64? ID_TIPO_CR, Int64? ID_USO_CR)
        {
            var resultador =
                SiralabContext.Correspondencia_CR_UA_SIRFIC_SIRALAB.FirstOrDefault(p => (p.ID_TIPO_CR == ID_TIPO_CR || (ID_TIPO_CR == null && p.ID_TIPO_CR == null)) && (p.ID_USO_CR == ID_USO_CR || (ID_USO_CR == null && p.ID_USO_CR == null)));

            if (resultador == null) return 0;
            return resultador.CuerpoReceptorID;
        }

        public int ObtenerUsoAguaID(Int64? ID_TIPO_CR, Int64? ID_USO_CR)
        {
            var resultador = SiralabContext.Correspondencia_CR_UA_SIRFIC_SIRALAB.FirstOrDefault(p => (p.ID_TIPO_CR == ID_TIPO_CR || (ID_TIPO_CR == null && p.ID_TIPO_CR == null)) && (p.ID_USO_CR == ID_USO_CR || (ID_USO_CR == null && p.ID_USO_CR == null)));

            if (resultador == null) return 0;
            return resultador.UsoAguaID;
        }
    }
}
