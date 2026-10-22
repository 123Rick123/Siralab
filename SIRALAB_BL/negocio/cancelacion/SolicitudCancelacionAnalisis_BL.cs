using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.cancelacion
{
    public class SolicitudCancelacionAnalisis_BL
    {
        public Siralab_Entities Context { get; private set; }
        public SolicitudCancelacionDetalle Detalle { get; private set; }

        private Repository<SolicitudCancelacionDetalle> Repositorio;
        public SolicitudCancelacionAnalisis_BL()
        {
            Context = new Siralab_Entities();
            Repositorio = new Repository<SolicitudCancelacionDetalle>(Context);
        }
        public SolicitudCancelacionDetalle Guarda(SolicitudCancelacionDetalleArgBase_BL args)
        {
            SolicitudCancelacionDetalle detalle = new SolicitudCancelacionDetalle();

            detalle.SolicitudCancelacionID = args.SolicitudCancelacionID;
            detalle.AnalisisID = args.AnalisisID;

            try
            {
                if (args.SolicitudCancelacionDetalleID != null) //Es una actualización
                {
                    detalle.SolicitudCancelacionDetalleID = (int)args.SolicitudCancelacionDetalleID;
                    Repositorio.Edit(detalle);
                }
                else
                {
                    Repositorio.Add(detalle);
                }

                Repositorio.SaveChanges();
                this.Detalle = Repositorio.Single(x => x.SolicitudCancelacionDetalleID == detalle.SolicitudCancelacionDetalleID);
                return detalle = this.Detalle;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<usp_BuscaSolicitudCancelacionDetalle_Result> BuscaSolicitudesAnalisis(FiltroSolicitudCancelacionAnalisisArgBase filter)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            IEnumerable<usp_BuscaSolicitudCancelacionDetalle_Result> list;

            list = sbd.usp_BuscaSolicitudCancelacionDetalle(
                filter.Numero,
                filter.Titulo,
                filter.FechaInicialEmision,
                filter.FechaFinalEmision,
                filter.Rfc,
                filter.LaboratorioID,
                filter.NormaID,
                filter.CurrentPage,
                filter.PageSize).ToList();

            return list;
        }
        public int CuentaSolicitudesAnalisis(FiltroSolicitudCancelacionAnalisisArgBase filter)
        {
            int? count;
            using (Siralab_Entities sbd = new Siralab_Entities())
            {
                count = sbd.usp_BuscaSolicitudCancelacionDetalleCount(
                    filter.Numero,
                    filter.Titulo,
                    filter.FechaInicialEmision,
                    filter.FechaFinalEmision,
                    filter.Rfc,
                    filter.LaboratorioID,
                    filter.NormaID).FirstOrDefault()?.Total;
            }
            return count ?? 0;
        }
    }
}
