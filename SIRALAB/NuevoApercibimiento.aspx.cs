using SIRALAB.Controles;
using SIRALAB.Utilerias;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Net;
using System.Xml;
using System.IO;
using MimeKit;
using SIRALAB.ServRefAmara;
using SIRALAB.Common;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using SIRALAB_BL.negocio.Comun;

using SIRALAB_BL;
using static SIRALAB_BL.negocio.LaboratoriosApercibidos.LabApercibidosYReincidentes;

namespace SIRALAB
{

    public partial class NuevoApercibimiento : System.Web.UI.Page
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


        [WebMethod(Description = "guardar  apercibimiento")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultadoApercibidosReinciedentes GuardarApercibimiento(string ArchivoBase64, string NombreArchivo, string numOficio, string fechaOficio, string fechaNotificacion, int trimestre, int anio, string observaciones, string iniciales, string noExpediente, string idLaboratorio)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var resultado = new ResultadoApercibidosReinciedentes();
            try
            {

                Laboratorio_BL labBLL = new Laboratorio_BL();
                var laboratorio = labBLL.Obten_Laboratorio(int.Parse(idLaboratorio));
           
                string RFC = laboratorio.RFC;
                string nombreLab = laboratorio.Nombre;

                var nombreCompleto = NombreUsuario;
                byte[] archivoBytes = { };
                archivoBytes = System.Convert.FromBase64String(Utilerias.Utilerias.eliminaBase64(ArchivoBase64));
                var idIC2 = IC2.SubirarchivoIc2(archivoBytes, NombreArchivo, WebConfigurationManager.AppSettings["CarpetaGuardaDocumentos"], numOficio);
                var IdDocumentoIC2 = idIC2;
                resultado.idIC2 = IdDocumentoIC2;

                var nombreCompletoResponsable = string.Empty;


                var guarda = BLL.GuardarApercibimiento(int.Parse(idLaboratorio), nombreLab, RFC, IdDocumentoIC2, numOficio, fechaOficio, fechaNotificacion, trimestre, anio, observaciones, iniciales, noExpediente, nombreCompleto);
                if (guarda != "0")
                {
                    resultado.Message = "Datos guardados correctamente";
                    resultado.Result = true;
                    resultado.idApercibimiento = guarda;
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
                resultado.idApercibimiento = "0";
                return resultado;

            }

        }
  
        
        
        [WebMethod(Description = "Actualiza  apercibimiento")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultadoApercibidosReinciedentes ActualizaApercibimiento(int idApercibimiento, string idDocIC2, string ArchivoBase64, string NombreArchivo, string numOficio, string fechaOficio, string fechaNotificacion, int trimestre, int anio, string observaciones, string iniciales, string noExpediente, string idLaboratorio)
        {
            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var resultado = new ResultadoApercibidosReinciedentes();
            try
            {

                Laboratorio_BL labBLL = new Laboratorio_BL();
                var laboratorio = labBLL.Obten_Laboratorio(int.Parse(idLaboratorio));
           
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
                

                var nombreCompletoResponsable = string.Empty;


                var guarda = BLL.ActualizaApercibimiento(idApercibimiento,int.Parse(idLaboratorio), nombreLab, RFC, resultado.idIC2, numOficio, fechaOficio, fechaNotificacion, trimestre, anio, observaciones, iniciales, noExpediente, nombreCompleto);
                if (guarda != "0")
                {
                    resultado.Message = "Datos guardados correctamente";
                    resultado.Result = true;
                    resultado.idApercibimiento = guarda;
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
                resultado.idApercibimiento = "0";
                return resultado;

            }

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<ListaLaboratoriosBusqueda> ObtenerLaboratorios(string rfc, string NombreLaboratorio)
        {

            LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();


            var lista = ApercibidosReincientesBLL.ObtenerListaLab(rfc, NombreLaboratorio);


            return lista.ToList();
        }
    }
}