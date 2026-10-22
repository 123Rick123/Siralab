using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System.Data;

namespace SIRALAB_BL.negocio.cancelacion
{
    public class SolicitudCancelacion_BL
    {
        public Siralab_Entities Context { get; private set; }

        public SolicitudCancelacion Solicitud { get; private set; }

        private Repository<SolicitudCancelacion> Repositorio;

        public SolicitudCancelacion_BL()
        {
            Context = new Siralab_Entities();
            Repositorio = new Repository<SolicitudCancelacion>(Context);
        }

        public usp_BuscaSolicitudCancelacion_Result ObtenSolicitudCancelacion(int solicitudCancelacionID)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            IEnumerable<usp_BuscaSolicitudCancelacion_Result> list;

            list = sbd.usp_BuscaSolicitudCancelacion(
                null,
                solicitudCancelacionID,
                null,
                null,
                null,
                true,
                1,
                10).ToList();

            return list.FirstOrDefault();
        }
        public SolicitudCancelacion Guarda(SolicitudCancelacionArgBase_BL args)
        {
            SolicitudCancelacion solicitud = new SolicitudCancelacion();

            solicitud.EstatusID = args.EstatusID;
            solicitud.Motivo = args.Motivo;
            solicitud.LaboratorioID = args.LaboratorioID;
            solicitud.FirmaSolicitud = args.FirmaSolicitud;
            solicitud.FirmaAprobacion = args.FirmaAprobacion;
            solicitud.FechaRegistro = args.FechaRegistro;
            solicitud.FechaAprobacion = args.FechaAprobacion;
            solicitud.SolicitudCancelacionDetalle = args.Detalle.Select(p => new SolicitudCancelacionDetalle
            {
                AnalisisID = p.AnalisisID
            }).ToList();
            try
            {
                if (args.SolicitudCancelacionID != null)
                    solicitud.SolicitudCancelacionID = (int)args.SolicitudCancelacionID;
                if (args.SolicitudCancelacionID != null) //Es una actualización
                {
                    Repositorio.Edit(solicitud);
                }
                else
                {
                    Repositorio.Add(solicitud);
                }

                Repositorio.SaveChanges();
                this.Solicitud = Repositorio.Single(x => x.SolicitudCancelacionID == solicitud.SolicitudCancelacionID);

                //foreach(var detalle in args.Detalle)
                //{
                //    SolicitudCancelacionDetalle_BL scd = new SolicitudCancelacionDetalle_BL();
                //    SolicitudCancelacionDetalleArg detalleItem = new SolicitudCancelacionDetalleArg();

                //    detalleItem.AnalisisID = detalle.AnalisisID;
                //    detalleItem.SolicitudCancelacionID = solicitud.SolicitudCancelacionID;

                //    scd.Guarda(detalleItem);
                //}

                return solicitud = this.Solicitud;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SolicitudCancelacion FirmaSolicitud(SolicitudCancelacionArgBase_BL args)
        {
            var ID = Convert.ToInt32(args.SolicitudCancelacionID);
            var sol =
                Context.SolicitudCancelacion.Single(p => p.SolicitudCancelacionID == ID);

            sol.FirmaSolicitud = args.FirmaSolicitud;

            try
            {
                if (args.SolicitudCancelacionID != null)

                Repositorio.SaveChanges();
                this.Solicitud = Repositorio.Single(x => x.SolicitudCancelacionID == sol.SolicitudCancelacionID);

                return this.Solicitud;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SolicitudCancelacion FirmaAprobacion(SolicitudCancelacionArgBase_BL args)
        {
            var ID = Convert.ToInt32(args.SolicitudCancelacionID);
            var sol =
                Context.SolicitudCancelacion.Single(p => p.SolicitudCancelacionID == ID);

            sol.FirmaAprobacion = args.FirmaAprobacion;
            sol.FechaAprobacion = args.FechaAprobacion;

            try
            {
                if (args.SolicitudCancelacionID != null)

                    Repositorio.SaveChanges();
                this.Solicitud = Repositorio.Single(x => x.SolicitudCancelacionID == sol.SolicitudCancelacionID);

                return this.Solicitud;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool GuardarAprobacion(IEnumerable<SolicitudCancelacionArgBase_BL> solicitudes, enumEstatusCancelacion estatus)
        {
            using (Siralab_Entities sbd = new Siralab_Entities())
            {
                List<int> Ids = solicitudes.Select(r => (int)r.SolicitudCancelacionID).ToList();
                IEnumerable<SolicitudCancelacion> solicitudesList = sbd.SolicitudCancelacion.Where(p => Ids.Contains(p.SolicitudCancelacionID));
                try
                {
                    foreach (var cancelacion in solicitudesList)
                    {
                        /// TODO: Debemos grabar la firma electronica de la operación
                        cancelacion.EstatusID = estatus.ParseToInt();
                    }

                    if (estatus == enumEstatusCancelacion.Aprobada) /* Sí se aprueba la cancelación entonces cancelamos los análisis */
                    {
                        var table = solicitudes.Select(p => new Analisis_BL.SolicitudCancelacionParam
                        {
                            SolicitudCancelacionID = (int)p.SolicitudCancelacionID,
                            EstatusID = enumEstatusAnalisis.Cancelado.ParseToInt(),
                        }).ToList().ConvertToDataTable();

                        var paramName = "@Listado";
                        var procedureName = "usp_CancelaAnalisis";
                        var typeName = "SolicitudCancelacion";
                        //// create parameter 
                        System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter(paramName, table);
                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = typeName;

                        //// execute sp sql 
                        string sql = $"EXEC {procedureName} {paramName};";
                        //// execute sql 
                        sbd.Database.ExecuteSqlCommand(sql, parameter);
                    }

                    sbd.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<usp_BuscaSolicitudCancelacion_Result> BuscaSolicitudes(FiltroSolicitudCancelacionArgBase filter)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            IEnumerable<usp_BuscaSolicitudCancelacion_Result> list;

            list = sbd.usp_BuscaSolicitudCancelacion(
                filter.LaboratorioID,
                filter.Folio,
                filter.Estatus?.ParseToInt(),
                filter.FechaInicialEmision,
                filter.FechaFinalEmision,
                filter.IncluirCapturados,
                filter.CurrentPage,
                filter.PageSize).ToList();

            return list;
        }

        public int CuentaSolicitudes(FiltroSolicitudCancelacionArgBase filter)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            int? count;

            count = sbd.usp_BuscaSolicitudCancelacionCount(
                filter.LaboratorioID,
                filter.Folio,
                filter.Estatus?.ParseToInt(),
                filter.FechaInicialEmision,
                filter.FechaFinalEmision,
                filter.IncluirCapturados).FirstOrDefault()?.Total;

            return count ?? 0;
        }

        public bool Eliminar(int solicitudCancelacionID)
        {
            try
            {
                var solicitud = Context.SolicitudCancelacion.Single(p => p.SolicitudCancelacionID == solicitudCancelacionID);
                var detalle = solicitud.SolicitudCancelacionDetalle.ToList();

                foreach (var item in detalle)
                    Context.SolicitudCancelacionDetalle.Remove(item);

                Context.SolicitudCancelacion.Remove(solicitud);
                Context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public SolicitudCancelacion Obtener(int solicitudCancelacionID)
        {
            SolicitudCancelacion result = new SolicitudCancelacion();
            try
            {
                var solicitud = Context.SolicitudCancelacion.Include("Laboratorio.Usuarios")
                    .Single(p => p.SolicitudCancelacionID == solicitudCancelacionID);
                return solicitud;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
