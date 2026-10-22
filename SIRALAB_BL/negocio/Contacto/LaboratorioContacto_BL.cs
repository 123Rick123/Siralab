using System.Collections.Generic;
using System.Linq;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.Contacto
{
    public class LaboratorioContacto_BL
    {
        public Siralab_Entities LaboratorioContactoContext { get; private set; }
        public SIRALAB_DAL.LaboratorioContacto LabarotorioContacto { get; private set; }

        Repository<SIRALAB_DAL.LaboratorioContacto> Repositorio;

        public LaboratorioContacto_BL()
        {
            LaboratorioContactoContext = new Siralab_Entities();
            Repositorio = new Repository<SIRALAB_DAL.LaboratorioContacto>(LaboratorioContactoContext);
        }

        public List<LaboratorioContacto> ObtenContactosByLaboratorioId(int laboratorioId)
        {
            return Repositorio.Find(contactos => contactos.LaboratorioID == laboratorioId).ToList();
        }
    }
}