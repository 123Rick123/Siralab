using SIRALAB.Controles;
using SIRALAB.Entidades;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using SIRALAB_BL.negocio.LaboratoriosApercibidos.Enum;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class ApercibidosYReincidentes : System.Web.UI.Page
    {
        LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            //ejemplo

            if (!IsPostBack)
            {
                var listaDeApercibimientos = BLL.GetAllApercibidos().Where(x => x.Activo == true);
               
                foreach (var item in listaDeApercibimientos)
                {
                    if (item.Fecha_Vigencia.Value != null)
                    {
                        if (DateTime.Now > item.Fecha_Vigencia.Value)
                        {
                            var desactivaApercibimiento = BLL.desactivaApercibimiento(item.Id);
                            var Impugnacion = BLL.ObtenerImpugnacionByIdApercibimiento(item.Id);
                            if (Impugnacion != null)
                            {

                                var desabilitarImpugnacion = BLL.desactivaImpugnacionById(Impugnacion.Id);
                            }
                        }
                    }

                }

                var listaDeReincidencias = BLL.GetAllReincidencias().Where(x => x.Activo == true);
                foreach (var item in listaDeReincidencias)
                {
                    var Reincidencia = BLL.BuscarReincidenciaByID(item.Id);
                    var VigenciasAprobacion = BLL.ObtenerNumerosYVigenciasDeAprobacion(Reincidencia.Id_Laboratorio.Value).OrderBy(x => x.FechaRegistro).First();
                    if (VigenciasAprobacion.FechaFin != null)
                    {

                        if (DateTime.Now > VigenciasAprobacion.FechaFin)
                        {
                            var desactivaReincidencia = BLL.desactivaReincidencia(item.Id);
                            var Impugnacion = BLL.ObtenerImpugnacionByIdReincidencia(item.Id);
                            
                            if (Impugnacion != null)
                            {
                                var desabilitarImpugnacion = BLL.desactivaImpugnacionById(Impugnacion.Id);
                            }
                        }
                    }
                }
            }
        }
        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {


        }
        protected void lnkBuscar_OnClick(object sender, EventArgs e)
        {

        }
        protected void lnkbtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NuevoApercibimiento.aspx");

        }

        [HttpPost]
        public object ObtenerLaboratorios(string RFC, string NombreLaboratorio)
        {
            try
            {

                return null;

            }
            catch (Exception e)
            {

                throw;
            }

        }

        public string ObtenerEstatus(int id)
        {

            switch (id)
            {
                case 1:
                    return "Apercibido";
                case 2:
                    return "Apercibido con Impugnación";
                case 3:
                    return "Reincidencia";
                case 4:
                    return "Reincidencia con Impugnación";
                default:
                    return "";

            }

        }

        [WebMethod]
        public static List<LaboratorioApercibidos> BusqedaApercibidoReincidente(string rfc, string nombreLaboratorio, string numeroAprobacion, string estatus, string periodoInicio, string periodoFin)
        {
            try
            {
                var repositorio = new SIRALAB_BL.negocio.LaboratoriosApercibidos.LabApercibidosYReincidentesBLL();

                List<LaboratorioApercibidos> resultadoFinal = new List<LaboratorioApercibidos>();

                DateTime? fechaInicio = null;

                if (string.IsNullOrEmpty(periodoInicio) != true)
                {
                    fechaInicio = DateTime.Parse(periodoInicio);
                }

                DateTime? fechaFin = null;

                if (string.IsNullOrEmpty(periodoFin) != true)
                {
                    fechaFin = DateTime.Parse(periodoFin);
                }
               
                var laboratorios = repositorio.BusquedaApercibido(rfc, nombreLaboratorio, fechaInicio, fechaFin, numeroAprobacion, estatus);
                var reincidencia = repositorio.BusquedaReincidencias(rfc, nombreLaboratorio, fechaInicio, fechaFin, numeroAprobacion, estatus);

                var resultApercibido = laboratorios.Select(tabla => new Entidades.LaboratorioApercibidos
                {
                    Id = tabla.Id,
                    LaboratorioId = (int)tabla.Id_Laboratorio,
                    Activo = tabla.Activo,
                    Anio = tabla.Año,
                    Evidencia = tabla.Evidencia,
                    Fecha_captura = tabla.Fecha_captura?.ToShortDateString(),
                    Nombre = tabla.Nombre,
                    RFC = tabla.RFC,
                    Numero_Oficio = tabla.Numero_Oficio,
                    Fecha_Resolucion = tabla.Fecha_Resolucion?.ToShortDateString(),
                    Fecha_Notificacion = tabla.Fecha_Notificacion?.ToShortDateString(),
                    Fecha_Vigencia = tabla.Fecha_Vigencia?.ToShortDateString(),
                    EstatusId = (int)tabla.Estatus,
                }).ToList();




                var resutlReincidencia = reincidencia.Select(tabla => new Entidades.LaboratorioApercibidos
                {
                    Id = tabla.Id,
                    IdApercibimiento = tabla.idApercibimiento.Value,
                    LaboratorioId = (int)tabla.Id_Laboratorio,
                    Activo = tabla.Activo,
                    Evidencia = tabla.Evidencia,
                    Fecha_captura = tabla.Fecha_captura?.ToShortDateString(),
                    Nombre = tabla.Nombre,
                    RFC = tabla.RFC,
                    Numero_Oficio = tabla.Numero_Oficio,
                    Fecha_Resolucion = tabla.Fecha_Resolucion?.ToShortDateString(),
                    Fecha_Notificacion = tabla.Fecha_Notificacion?.ToShortDateString(),
                    Fecha_Vigencia = tabla.Fecha_Vigencia?.ToShortDateString(),
                    EstatusId = (int)tabla.Estatus,
                }).ToList();


                if (estatus != null &&  estatus == "apercibido" )
                {
                    resultadoFinal.AddRange(resultApercibido);
                }

                else if (estatus != null &&  estatus == "reincidente")
                {
                    resultadoFinal.AddRange(resutlReincidencia);
                }
                else
                {
                    resultApercibido.AddRange(resutlReincidencia);
                    resultadoFinal.AddRange(resultApercibido);
                }

                if (string.IsNullOrEmpty(numeroAprobacion) != true)
                {

                    var vigencias = resultadoFinal.Where(a => a.LaboratorioVigencias.Where(b => b.NumeroAprobacion.Contains(numeroAprobacion)).Any()).ToList();

                    return vigencias;

                }


                return resultadoFinal;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        [WebMethod]
        public static bool ExisteReincidencia(int idApercibimiento)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var consulta = BLL.BuscarReincidenciaByIdApercibimiento(idApercibimiento);
            if (consulta != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public static bool ApercibimientoEstaImpugnado(int idApercibimiento)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var listaImpugnacion = BLL.ObtenerImpugnacionesByIdApercibimiento(idApercibimiento);
            var ImpugnacionApercibimiento = listaImpugnacion.Where(x => x.Activo == true && x.IdMediosDeImpugnacion == 4).SingleOrDefault();

            if (ImpugnacionApercibimiento != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [WebMethod]
        public static bool ApercibimientoImpugnado(int idApercibimiento)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var listaImpugnacion = BLL.ObtenerImpugnacionesByIdApercibimiento(idApercibimiento);
            //var ImpugnacionApercibimiento = listaImpugnacion.Where(x => x.Activo == true && x.Estatus == (int)EnumEstatusImpugnacion.EnProceso).SingleOrDefault();

            if (listaImpugnacion.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [WebMethod]
        public static bool ReincidenciaEstaImpugnado(int idReincidencia)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var listaImpugnacion = BLL.ObtenerImpugnacionesByIdReincidencia(idReincidencia);
            var ImpugnacionReincidencia = listaImpugnacion.Where(x => x.Activo == true).SingleOrDefault();

            if (ImpugnacionReincidencia != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public static bool ReincidenciaImpugnada(int idReincidencia)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var listaImpugnacion = BLL.ObtenerImpugnacionesByIdReincidencia(idReincidencia);
            //var ImpugnacionReincidencia = listaImpugnacion.Where(x =>x.Activo == true && x.Estatus == (int)EnumEstatusImpugnacion.EnProceso).SingleOrDefault();

            if (listaImpugnacion.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public static bool VigenciaAprobacionValida(int idApercibimiento)
        {
            try
            {
                bool resultado = false;
                LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
                var apercibimiento = BLL.BuscarApercibimientoByID(idApercibimiento);
                var VigenciasAprobacion = BLL.ObtenerNumerosYVigenciasDeAprobacion(apercibimiento.Id_Laboratorio.Value).OrderBy(x=>x.FechaRegistro).SingleOrDefault();

                if (VigenciasAprobacion.FechaFin != null)
                {
                   
                    if (DateTime.Now > VigenciasAprobacion.FechaFin)
                    {
                        resultado = false;
                    }
                    else
                    {
                        resultado = true;
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}