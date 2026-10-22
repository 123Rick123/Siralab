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
    public class LaboratorioSignatario_BL : Base_BL<LaboratorioSignatario>
    {
        public LaboratorioSignatario_BL() : base()
        {

        }

        public LaboratorioSignatario Obten_LaboratorioSignatario(int LaboratorioID, int PersonaID)
        {
            var LaboratorioSignatario = Repositorio.Find(x => x.LaboratorioID == LaboratorioID && x.PersonaID == PersonaID).FirstOrDefault();
            return LaboratorioSignatario;
        }

        public LaboratorioSignatario Obten_LaboratorioSignatario(int MuestraID)
        {
            var LaboratorioSignatarios = Repositorio.Find(x => x.Muestreadors.Any(q => q.MuestraID == MuestraID && (q.Activo ?? false)));
            var LaboratorioSignatario = LaboratorioSignatarios.FirstOrDefault();

            return LaboratorioSignatario;
        }
    }
}