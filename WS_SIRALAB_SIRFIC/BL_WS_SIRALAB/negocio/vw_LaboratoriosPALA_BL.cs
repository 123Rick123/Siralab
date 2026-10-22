using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using DL_WS_SIRALAB;
using BL_WS_SIRALAB;
using BL_WS_SIRALAB.extend;
namespace BL_WS_SIRALAB.negocio
{
    public class vw_LaboratoriosPALA_BL
    {
        private Siralab_Entities se { get; set; }
        public vw_LaboratoriosPALA_BL()
        {
            se = new Siralab_Entities();
            se.Configuration.AutoDetectChangesEnabled = false;
            se.Configuration.LazyLoadingEnabled = true;
            se.Configuration.ProxyCreationEnabled = true;
        }




        public vw_LaboratoriosPALA GetDatos(string RFC)
        {
            vw_LaboratoriosPALA respuesta = new vw_LaboratoriosPALA();
            respuesta = se.vw_LaboratoriosPALA.Where(p => p.RFC == RFC).FirstOrDefault();

            return respuesta;
        }
    }
}
