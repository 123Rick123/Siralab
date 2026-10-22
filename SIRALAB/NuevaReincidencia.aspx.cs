using DocumentFormat.OpenXml.Drawing.Diagrams;
using SIRALAB.Common;
using SIRALAB.Utilerias;
using SIRALAB_BL;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class NuevaReincidencia : System.Web.UI.Page
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

        [WebMethod(Description = "guardar  reincidencia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultadoApercibidosReinciedentes GuardarReincidencia(string ArchivoBase64, string NombreArchivo, string numOficio, string fechaOficio, string fechaNotificacion, string observaciones, string iniciales, string noExpediente, int idApercibimiento, int trimestre, int anio)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var resultado = new ResultadoApercibidosReinciedentes();
            try
            {
              var Apercibimiento = BLL.BuscarApercibimientoByID(idApercibimiento);
                int idLaboratorio = Apercibimiento.Id_Laboratorio.Value;
                Laboratorio_BL labBLL = new Laboratorio_BL();
                var laboratorio = labBLL.Obten_Laboratorio(idLaboratorio);
                //falta obtener
                string RFC = laboratorio.RFC;
                string nombreLab = laboratorio.Nombre;

                var nombreCompleto = NombreUsuario;
                byte[] archivoBytes = { };
                archivoBytes = System.Convert.FromBase64String(Utilerias.Utilerias.eliminaBase64(ArchivoBase64));
                var idIC2 = IC2.SubirarchivoIc2(archivoBytes, NombreArchivo, WebConfigurationManager.AppSettings["CarpetaGuardaDocumentos"], numOficio);
                var IdDocumentoIC2 = idIC2;
                resultado.idIC2 = IdDocumentoIC2;


                var guarda = BLL.GuardarReincidencia(idApercibimiento, idLaboratorio, nombreLab, RFC, IdDocumentoIC2, numOficio, fechaOficio, fechaNotificacion, observaciones, iniciales, noExpediente, nombreCompleto, trimestre, anio);
                if (guarda != "0")
                {
                    resultado.Message = "Datos guardados correctamente";
                    resultado.Result = true;
                    resultado.idReincidencia = guarda;
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
                resultado.idReincidencia = "0";
                return resultado;

            }

        }

        [WebMethod(Description = "Actualiza  reincidencia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultadoApercibidosReinciedentes ActualizarReincidencia(int idReincidencia, string idDocIC2, string ArchivoBase64, string NombreArchivo, string numOficio, string fechaOficio, string fechaNotificacion, string observaciones, string iniciales, string noExpediente, int trimestre, int anio)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var resultado = new ResultadoApercibidosReinciedentes();
            try
            {
                var Reincidencia = BLL.BuscarReincidenciaByID(idReincidencia);
                int idLaboratorio = Reincidencia.Id_Laboratorio.Value;
                Laboratorio_BL labBLL = new Laboratorio_BL();
                var laboratorio = labBLL.Obten_Laboratorio(idLaboratorio);
                //falta obtener
                string RFC = laboratorio.RFC;
                string nombreLab = laboratorio.Nombre;

                var nombreCompleto = NombreUsuario;
                if (ArchivoBase64 == string.Empty && idDocIC2 != string.Empty)
                {
                    resultado.idIC2 = idDocIC2;
                }
                else
                {
                    byte[] archivoBytes = { };
                    archivoBytes = System.Convert.FromBase64String(Utilerias.Utilerias.eliminaBase64(ArchivoBase64));
                    var idIC2 = IC2.SubirarchivoIc2(archivoBytes, NombreArchivo, WebConfigurationManager.AppSettings["CarpetaGuardaDocumentos"], numOficio);
                    var IdDocumentoIC2 = idIC2;
                    resultado.idIC2 = IdDocumentoIC2;
                }
                


                var guarda = BLL.ActualizarReincidencia(idReincidencia, idLaboratorio, nombreLab, RFC, resultado.idIC2, numOficio, fechaOficio, fechaNotificacion, observaciones, iniciales, noExpediente, nombreCompleto,trimestre,anio);
                if (guarda != "0")
                {
                    resultado.Message = "Datos guardados correctamente";
                    resultado.Result = true;
                    resultado.idReincidencia = guarda;
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
                resultado.idReincidencia = "0";
                return resultado;

            }

        }

    }
}