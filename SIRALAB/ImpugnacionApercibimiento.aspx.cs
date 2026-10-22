using SIRALAB.Common;
using SIRALAB_BL;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class ImpugnacionApercibimiento : System.Web.UI.Page
    {
        public static string NombreUsuario
        {
            get
            {
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];
                return usuario != null ? usuario.Persona.NombreCompleto : null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod(Description = "guardar  Impugnacion")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultadoMediosImpugnacion GuardarImpugnacion(int MedioImpugnacion, string FechaImpugnacion, string AnteQuien, int Estatus, string Observaciones, int idApercibimiento, int idReincidencia, string FechaResolucion, string Expediente)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var resultado = new ResultadoMediosImpugnacion();
            try
            {
                var idLaboratorio = 0;
                if (idReincidencia != 0)
                {
                    idLaboratorio = BLL.BuscarReincidenciaByID(idReincidencia).Id_Laboratorio.Value;
                }
                else if (idApercibimiento != 0)
                {
                    idLaboratorio = BLL.BuscarApercibimientoByID(idApercibimiento).Id_Laboratorio.Value;
                }

                var guarda = BLL.GuardarImpugnacion(MedioImpugnacion, FechaImpugnacion, AnteQuien, Estatus, Observaciones, idApercibimiento, idReincidencia, idLaboratorio, NombreUsuario, FechaResolucion, Expediente);
                if (guarda != false)
                {
                    resultado.Message = "Datos guardados correctamente";
                    resultado.Result = true;

                }
                else
                {
                    resultado.Message = "Ocurrió un error al guardar";
                    resultado.Result = false;
                }


                return resultado;

            }
            catch (Exception e)
            {
                resultado.Message = "Error, " + e.Message;
                resultado.Result = false;
                //resultado.idApercibimiento = "0";
                return resultado;

            }

        }


        [WebMethod(Description = "Actualizar Impugnacion")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultadoMediosImpugnacion ActualizarImpugnacion(int MedioImpugnacion, string FechaImpugnacion, string AnteQuien, int Estatus, string Observaciones, int idApercibimiento, int idReincidencia, string FechaResolucion, string Expediente)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var resultado = new ResultadoMediosImpugnacion();
            try
            {
   
                var idImpugnacion = 0;
                if (idReincidencia != 0)
                {
                   
                    idImpugnacion = BLL.ObtenerImpugnacionByIdReincidencia(idReincidencia).Id;
                }
                else if (idApercibimiento != 0)
                {
                  
                    idImpugnacion = BLL.ObtenerImpugnacionByIdApercibimiento(idApercibimiento).Id;
                }

                var guarda = BLL.ActualizarImpugnacion(MedioImpugnacion, FechaImpugnacion, AnteQuien, Estatus, Observaciones, idImpugnacion, FechaResolucion, Expediente);
                if (guarda != false)
                {
                    resultado.Message = "Datos actualizados correctamente";
                    resultado.Result = true;

                }
                else
                {
                    resultado.Message = "Ocurrió un error al guardar";
                    resultado.Result = false;
                }


                return resultado;

            }
            catch (Exception e)
            {
                resultado.Message = "Error, " + e.Message;
                resultado.Result = false;
                //resultado.idApercibimiento = "0";
                return resultado;

            }

        }


    }
}