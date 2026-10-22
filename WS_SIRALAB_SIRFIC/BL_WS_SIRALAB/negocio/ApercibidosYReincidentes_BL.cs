using DL_WS_SIRALAB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.negocio
{

    public class ApercibidosYReincidentes_BL
    {
        private Siralab_Entities Context { get; set; }

        public ApercibidosYReincidentes_BL()
        {
            Context = new Siralab_Entities();

        }

        public List<int> BuscaIdLaboratorio(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo)
        {


            var result = from AN in Context.Analisis
                         join OT in Context.OrdenTrabajoes on AN.OrdenTrabajoID equals OT.OrdenTrabajoID
                         join CL in Context.Clientes on OT.ClienteID equals CL.ClienteID
                         join LAB in Context.Laboratorios on AN.LaboratorioID equals LAB.LaboratorioID
                         join TITULO in Context.Consecions on CL.ClienteID equals TITULO.ClienteID
                         join ANEXO in Context.Anexoes on TITULO.ConsecionID equals ANEXO.ConsecionID
                         where CL.ClienteID == ClienteID && TITULO.Titulo == Titulo && ANEXO.Anexo1 == Anexo && (AN.Muestras.Where(q => q.Fecha >= FechaInicio && q.Fecha < FechaFin).Count() > 0)
                         group LAB by new { LAB.LaboratorioID } into laboratorio
                         select laboratorio;
            //select new Laboratorio()
            //{
            //   LaboratorioID= LAB.LaboratorioID,
            //   nombre= LAB.Nombre,
            //   rfc=LAB.RFC,
            //   apellidoPaterno=LAB.ApellidoPaterno,
            //   apellidoMaterno=LAB.ApellidoMaterno,
            //   persona_fisica=LAB.Fisica,
            //   Activo=LAB.Activo,

            //};
           List<int> idsLaboratorios= new List<int>();

            if (result.Any())
            {
                foreach (var item in result)
                {
                    int idLab = item.Key.LaboratorioID;
                    idsLaboratorios.Add(idLab);
                }
                
            }


            return idsLaboratorios;
        }

        //valida que la reincidencia este despues de los 30 dias habiles es decir a partir del dia 31
        public bool BuscaLaboratorioEnReincidenciaByLaboratorioID(int LaboratorioID)
        {
            var resultado = false;
            try
            {
                DiaInhabil_BL diaBLL = new DiaInhabil_BL();
                // El estatus 3 indica que la reincidencia este sin impugnación
                var EstaEnReincidencia = Context.tbl_Reincidencia.Where(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 3).First();
                if (EstaEnReincidencia != null)
                {
                    var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                    diasHabiles = diasHabiles + 1;
                    var Reincidencia = BuscaReincidenciaByLaboratorioID(EstaEnReincidencia.Id_Laboratorio.Value);
                    var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                    //
                    if (DateTime.Now >= fechaLimite)
                    {
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                    }


                }
                return resultado;
            }
            catch (Exception ex)
            {

                resultado = false;
                return resultado;
            }
        }

        //valida que la reincidencia este entre los 30 dias habiles 
        public bool EstaEnReincidenciaPreventivaByLaboratorioID(int LaboratorioID)
        {
            var resultado = false;
            try
            {
                DiaInhabil_BL diaBLL = new DiaInhabil_BL();
                // El estatus 3 indica que la reincidencia este sin impugnación
                var EstaEnReincidencia = Context.tbl_Reincidencia.Where(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 3).First();
                if (EstaEnReincidencia != null)
                {
                    var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                    diasHabiles = diasHabiles + 1;
                    var Reincidencia = BuscaReincidenciaByLaboratorioID(EstaEnReincidencia.Id_Laboratorio.Value);
                    var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                    var fechaNotificacion = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, 1);
                    //
                    if (DateTime.Now >= fechaNotificacion)
                    {
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                    }


                }
                return resultado;
            }
            catch (Exception)
            {

                resultado = false;
                return resultado;
            }
        }
        public bool BuscaLaboratorioEnReincidenciaImpugnadaByLaboratorioID(int LaboratorioID)
        {
            var resultado = false;
            try
            {
                // El estatus 4 indica que la reincidencia este con impugnación
                var EstaEnReincidencia = Context.tbl_Reincidencia.Where(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 4);
                if (EstaEnReincidencia.Any())
                {
                    resultado = true;
                }
                return resultado;
            }
            catch (Exception)
            {

                resultado = false;
                return resultado;
            }
        }
        public bool BuscaLaboratorioConApercibimientoByLaboratorioID(int LaboratorioID)
        {
            var resultado = false;
            try
            {
                // El estatus 1 indica que es apercibimiento normal
                var TieneApercibimiento= Context.Laboratorios_Apercibidos.Where(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 1);
                if (TieneApercibimiento.Any())
                {
                    resultado = true;
                }
                return resultado;
            }
            catch (Exception)
            {

                resultado = false;
                return resultado;
            }
        }
        public bool BuscaLaboratorioConApercibimientoImpugnadoByLaboratorioID(int LaboratorioID)
        {
            var resultado = false;
            try
            {
                // El estatus 2 indica que es apercibimiento con impugnación
                var TieneApercibimiento = Context.Laboratorios_Apercibidos.Where(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 2);
                if (TieneApercibimiento.Any())
                {
                    resultado = true;
                }
                return resultado;
            }
            catch (Exception)
            {

                resultado = false;
                return resultado;
            }
        }

        public tbl_Reincidencia BuscaReincidenciaByLaboratorioID(int LaboratorioID)
        {
            tbl_Reincidencia Reincidencia = new tbl_Reincidencia();
            try
            {
                // El estatus 3 indica que la reincidencia este sin impugnación
                 Reincidencia = Context.tbl_Reincidencia.FirstOrDefault(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 3);
                
                return Reincidencia;
            }
            catch (Exception)
            {

                return Reincidencia;
            }
        }
        public tblImpugnacion BuscaReincidenciaImpugnadaByLaboratorioID(int LaboratorioID)
        {
            tblImpugnacion ReincidenciaImpugnada = new tblImpugnacion();
            try
            {
                // El estatus 4 indica que la reincidencia este con impugnación
               var Reincidencia = Context.tbl_Reincidencia.FirstOrDefault(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 4);
                 ReincidenciaImpugnada = Context.tblImpugnacions.FirstOrDefault(x => x.IdReincidencia == Reincidencia.Id && x.Activo == true);
                return ReincidenciaImpugnada;
            }
            catch (Exception)
            {

                return ReincidenciaImpugnada;
            }
        }
        public Laboratorios_Apercibidos ApercibimientoByLaboratorioID(int LaboratorioID)
        {
            Laboratorios_Apercibidos Apercibimiento = new Laboratorios_Apercibidos();
            try
            {
                // obtiene el apercibimiento en base a el id del laboratorio
                Apercibimiento = Context.Laboratorios_Apercibidos.FirstOrDefault(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 1);

                return Apercibimiento;
            }
            catch (Exception)
            {

                return Apercibimiento;
            }
        }
        public tblImpugnacion ApercibimientoImpugnadoByLaboratorioID(int LaboratorioID)
        {
            tblImpugnacion ApercibimientoImpugnado = new tblImpugnacion();
            try
            {
                // obtiene el apercibimiento en base a el id del laboratorio
               var  Apercibimiento = Context.Laboratorios_Apercibidos.FirstOrDefault(x => x.Id_Laboratorio == LaboratorioID && x.Activo == true && x.Estatus == 2);
                ApercibimientoImpugnado = Context.tblImpugnacions.FirstOrDefault(x => x.IdApercibimiento == Apercibimiento.Id && x.Activo == true);
                return ApercibimientoImpugnado;
            }
            catch (Exception)
            {

                return ApercibimientoImpugnado;
            }
        }
        public List<tbl_Reincidencia> GetAllReincidencias()
        {
            try
            {
                var resultado = Context.tbl_Reincidencia.ToList();
                return resultado;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public tbl_Reincidencia BuscarReincidenciaByID(int id)
        {
            try
            {
                var Reincidencia = Context.tbl_Reincidencia.FirstOrDefault(x=> x.Id == id);
                return Reincidencia;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<LaboratorioVigencia> ObtenerNumerosYVigenciasDeAprobacion(int idLaboratorio)
        {
            var busqueda = Context.LaboratorioVigencias.Where(x => x.LaboratorioID == idLaboratorio && x.Activo == true).ToList();
            return busqueda;
        }
        public bool desactivaReincidencia(int idReincidencia)
        {
            try
            {
                var Actializo = Context.tbl_Reincidencia.Where(x => x.Id == idReincidencia && x.Activo == true).FirstOrDefault();
                Actializo.Activo = false;
                Context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }


        }
        public tblImpugnacion ObtenerImpugnacionByIdReincidencia(int IdReincidencia)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacions.Where(x => x.IdReincidencia == IdReincidencia && x.Activo == true).First();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public bool desactivaImpugnacionById(int ID)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacions.Where(x => x.Id == ID).First();
                Impugnacion.Activo = false;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
