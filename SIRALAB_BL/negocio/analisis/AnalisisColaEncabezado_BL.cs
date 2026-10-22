using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.analisis
{
    public class AnalisisColaEncabezado_BL
    {
        private Siralab_Entities AnalisisContext { get; set; }

        private AnalisisColaEncabezado AnalisisColaEncabezado { get; set; }
        Repository<AnalisisColaEncabezado> Repositorio;

        public AnalisisColaEncabezado_BL()
        {
            AnalisisContext = new Siralab_Entities();
            Repositorio = new Repository<AnalisisColaEncabezado>(AnalisisContext);
        }

        public AnalisisColaEncabezado SaveOrUpdate(AnalisisColaEncabezado colaEncabezado)
        {
            try
            {
                if (colaEncabezado.AnalisisColaEncabezadoID > 0)
                {
                    Repositorio.Edit(colaEncabezado);
                }
                else
                {
                    colaEncabezado.AnalisisColaEncabezadoID = Repositorio.NextNewID(ace => (int)ace.AnalisisColaEncabezadoID);
                    Repositorio.Add(colaEncabezado);
                }

                return colaEncabezado;
            }
            catch (Exception e)
            {
                return this.AnalisisColaEncabezado;
            }
        }


        public void DeleteAllByColaID(int colaID)
        {
            try
            {
                if (Repositorio.Find(ace => ace.AnalisisColaDetalleID == colaID).Any())
                {
                    foreach (var ace in Repositorio.Find(ace => ace.AnalisisColaDetalleID == colaID))
                    {
                        Repositorio.Delete(ace);
                    }
                }
            }
            catch (Exception e)
            {
                //No hace nada.
            }
        }


        public AnalisisColaEncabezado FinAnalisisColaEncabezadoById(int analisisColaEncabazadoId)
        {
            try
            {
                return Repositorio.Find(ace => ace.AnalisisColaEncabezadoID == analisisColaEncabazadoId).FirstOrDefault();
            }
            catch (Exception e)
            {
                return this.AnalisisColaEncabezado;
            }
        }


        public AnalisisColaEncabezado FindByAnalisisID(int analisisID, int estatusFirma)
        {

            try
            {
                return Repositorio.Find(ace => ace.AnalisisID == analisisID && ace.EstatusID == estatusFirma)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                return this.AnalisisColaEncabezado;
            }
        }

        public AnalisisColaEncabezado FindByAnalisisIDAndColaID(int AnalisisID, int ColaID)
        {
            try
            {
                return Repositorio.Find(ace => ace.AnalisisID == AnalisisID && ace.AnalisisColaDetalleID == ColaID)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                return this.AnalisisColaEncabezado;
            }
        }

        public List<AnalisisColaEncabezado> FindAllByAnalisisColaDetalleID(int AnalisisColaDetalleID)
        {
            try
            {
                return Repositorio.Find(ace => ace.AnalisisColaDetalleID == AnalisisColaDetalleID).ToList();
            }
            catch (Exception e)
            {
                return new List<AnalisisColaEncabezado>();
            }
        }


        public List<AnalisisColaEncabezado> FindAllByLabIDAndColaActiva(int LaboratorioID)
        {
            try
            {
                return Repositorio.Find(ace => ace.LaboratorioID == LaboratorioID && ace.AnalisisColaDetalle.Activo)
                    .ToList();
            }
            catch
            {
                return new List<AnalisisColaEncabezado>();
            }
        }

        public List<AnalisisColaEncabezado> FindAllByLabIdFallido(int LaboratorioID)
        {
            var estatusEnProceso = enumEstatusAnalisis.EnProceso.ParseToInt();
            var estatusFirmaFallida = enumEstatusAnalisis.FirmaFallida.ParseToInt();
            var estatusEnvioFallido = enumEstatusAnalisis.EnvioFallido.ParseToInt();
            var estatusFinalizado = enumEstatusAnalisis.Finalizado.ParseToInt();
            var estatusFirmado = enumEstatusAnalisis.Firmado.ParseToInt();

            var analisisFallidos = Repositorio.Find(ace =>
               ace.LaboratorioID == LaboratorioID && !ace.AnalisisColaDetalle.Activo && (ace.EstatusID == estatusEnProceso || ace.EstatusID == estatusFirmado || ace.EstatusID == estatusFirmaFallida || ace.EstatusID == estatusEnvioFallido)).ToList();

            analisisFallidos = analisisFallidos.OrderByDescending(a => a.FechaEnvio).ToList();

            var analisisCompletos = Repositorio
                .Find(ace =>
                    ace.LaboratorioID == LaboratorioID && ace.EstatusID == estatusFinalizado)
                .Select(a => a.AnalisisID).ToList();

            return analisisFallidos.Where(ace => !analisisCompletos.Contains(ace.AnalisisID)).ToList();
        }
    }
}
