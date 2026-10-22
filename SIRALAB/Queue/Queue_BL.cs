using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebSockets;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_DAL;

namespace SIRALAB.Queue
{
    public class Queue_BL
    {

        public void setEstatusAnalisisEncabezadoEnFirma(int AnalisisID, int ColaID, int EstatusID)
        {
            var colaEncabezadoBL = new AnalisisColaEncabezado_BL();
            var encabezado = colaEncabezadoBL.FindByAnalisisIDAndColaID(AnalisisID, ColaID); //Recupera AnalisisColaEncabezado -> 

            if (encabezado != null)
            {
                encabezado.EstatusID = EstatusID;
                colaEncabezadoBL.SaveOrUpdate(encabezado);
            }
            else
            {
                EstatusID = enumEstatusAnalisis.FirmaFallida.ParseToInt();
            }

            if (EstatusID == enumEstatusAnalisis.FirmaFallida.ParseToInt()) //Si ha fallado la firma intentamos borrar él registro de AnalisisFirma.
                deleteAnalisisFirma(AnalisisID);
        }

        public void setEstatusAnalisisEncabezadoEnCorreo(int AnalisisID, int ColaID, int EstatusID)
        {
            var colaEncabezadoBL = new AnalisisColaEncabezado_BL();
            var encabezado = colaEncabezadoBL.FindByAnalisisIDAndColaID(AnalisisID, ColaID); //Recupera AnalisisColaEncabezado -> 

            if (encabezado != null)
            {
                encabezado.EstatusID = EstatusID;
                encabezado.FechaFirma = DateTime.Now;
                colaEncabezadoBL.SaveOrUpdate(encabezado);
            }
        }



        public void deleteAnalisisFirma(int AnalisisID)
        {
            var analisisFirmaBL = new AnalisisFirma_BL();

            if (analisisFirmaBL.IfAnalisisFirmado(AnalisisID))
            {
                analisisFirmaBL.DeleteByAnalisisID(AnalisisID);
            }
        }


        public List<AnalisisColaEncabezado> getAnalisisColaEncabezados(int AnalisisColaDetalleID)
        {
            var colaEncabezadoBL = new AnalisisColaEncabezado_BL();
            return colaEncabezadoBL.FindAllByAnalisisColaDetalleID(AnalisisColaDetalleID);
        }

        public void detenerProceso(int? ColaID)
        {
            var analisisColaDetalleBL = new AnalisisColaDetalle_BL();

            if (ColaID != null)
            {
                var acd = analisisColaDetalleBL.FindByID((int)ColaID);

                if (acd != null)
                {
                    acd.Activo = false;
                    acd.Completo = false;
                    acd.FechaFinalizacion = DateTime.Now;
                    analisisColaDetalleBL.Guarda(acd);
                }
            }
        }


        public void finalizarCola(int? ColaID)
        {
            var analisisColaDetalleBL = new AnalisisColaDetalle_BL();

            if (ColaID != null)
            {
                var acd = analisisColaDetalleBL.FindByID((int)ColaID);

                if (acd != null)
                {
                    acd.Activo = false;
                    acd.Completo = getAnalisisColaEncabezados(ColaID.Value).All(a => a.EstatusID == enumEstatusAnalisis.Finalizado.ParseToInt());
                    acd.FechaFinalizacion = DateTime.Now;
                    analisisColaDetalleBL.Guarda(acd);
                }
            }
        }
    }
}