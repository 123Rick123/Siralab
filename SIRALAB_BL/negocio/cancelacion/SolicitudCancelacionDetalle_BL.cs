using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.cancelacion
{
    public class SolicitudCancelacionDetalle_BL
    {
        public Siralab_Entities Context { get; private set; }
        public SolicitudCancelacionDetalle Solicitud { get; private set; }

        private Repository<SolicitudCancelacionDetalle> Repositorio;

        public SolicitudCancelacionDetalle_BL()
        {
            Context = new Siralab_Entities();
            Repositorio = new Repository<SolicitudCancelacionDetalle>(Context);
        }
        public IEnumerable<usp_SolicitudCancelacionDetalle_Result> BuscaSolicitudes(int? solicitudCancelacionID)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            IEnumerable<usp_SolicitudCancelacionDetalle_Result> list;

            list = sbd.usp_SolicitudCancelacionDetalle(solicitudCancelacionID);
            return list;
        }
    }
}
