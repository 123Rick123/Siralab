using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Net;
using SIRALAB_BL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SIRALAB_BL.common;
using System.Web;

namespace SIRALAB_BL
{
    public class Archivos
    {
        public string Nombre;
        public string Extension;
        public string TipoMime;
        public string ImagenPreliminar;

        public Archivos() {

            //FileTransfer.FileTransferSoapClient ws = new FileTransfer.FileTransferSoapClient();
           
        }

        /// <summary>
        /// Método que obtine las propiedades de un archivo físico
        /// </summary>
        /// <param name="NombreArchivo">Nombre del archivo que se desea obtener su mimeType</param>
        /// <param name="RutaXML">Ruta del archivo XML que contiene la información de los mimeType y ruta de imagen</param>
        /// <returns>Objeto del tipo ArchivoAtributosEstaticos con su mimeType y la ruta de imagen del mime</returns>
        public static Archivos ObtenPropiedades(string NombreArchivo)
        {
            ApplicationSettings aS = new ApplicationSettings();
            string fileExtension = Path.GetExtension(NombreArchivo);
            string FileXMLExt = aS.FileXMLExt;
            string PathImagenesMIME = aS.PathImagenesMIME;
            var extItems = from c in XElement.Load(GetRutaFisica(FileXMLExt)).Elements("ext")
                           select new Archivos
                           {
                               Nombre = Path.GetFileName(NombreArchivo),
                               Extension = c.Attribute("extension").Value,
                               TipoMime = c.Attribute("header").Value,
                               ImagenPreliminar = PathImagenesMIME + c.Attribute("previewImage").Value
                           };

            var EstRet = extItems.Where(p => p.Extension == fileExtension).Select(p => p).FirstOrDefault();
            if (EstRet == null)
            {
                EstRet = extItems.Where(p => p.Extension == "").Select(p => p).FirstOrDefault();
            }

            return EstRet;
        }
        public static string GetRutaFisica(string claveUriXml)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(claveUriXml)))
            {
                return HttpContext.Current.Server.MapPath(claveUriXml);
            }
            return null;
        }
        public static string GetRutaImagenPreview<T>(T classDal, int evidenciaId, bool minimizar)
        {
            ApplicationSettings aS = new ApplicationSettings();

            string ruta = aS.ImagenPreview;
            ruta += "classType=" + GetIntTipoClase(classDal).ToString();
            ruta += "&idObject=" + GetIdObject(classDal).ToString();
            ruta += "&idEvidencia=" + evidenciaId.ToString();
            ruta += "&min=" + (minimizar ? "1" : "0");
            return ruta;
        }
        protected static int GetIntTipoClase<T>(T claseDal)
        {
            int val = 0;//GetType().BaseType.Name
            string tipo = claseDal.GetType().BaseType.Name;

            if (tipo == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
            {
                val = 0;
            }
            else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
            {
                val = 1;
            }
            else if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
            {
                val = 2;
            }
            else
                val = -1;

            return val;
        }
        public static ResultOperacionArchivo ObtenerDocumentoFisico(string path)
        {
            ResultOperacionArchivo resultado = new ResultOperacionArchivo();
            try
            {
                FileTransfer.FileTransferSoapClient ws = new FileTransfer.FileTransferSoapClient();
                resultado.evidencia = new SIRALAB_DAL.Documento();
                resultado.evidencia.BufferData = ws.GetFile(path);
                resultado.codeError = 0;
            }
            catch (Exception ex)
            {
                resultado.codeError = -1;
                resultado.comentarios = ex.Message;
            }
            return resultado;
        }
        public static List<ResultOperacionArchivo> GuardarEvidencias<T>(List<SIRALAB_DAL.Documento> listaEvidenciaOriginal, T claseGenericaEvidencia, List<SIRALAB_DAL.Documento> listaEvidenciasGuardadas) where T : class
        {
            FileTransfer.FileTransferSoapClient ws = new FileTransfer.FileTransferSoapClient();
            List<ResultOperacionArchivo> listaResultados = new List<ResultOperacionArchivo>();
            // List<SIRALAB_DAL.Documento> listaEvidenciasGuardadas = SIRALAB_BL.Documento_BL.GetEvidencias(claseGenericaEvidencia);

            string pathFolder = GetPathFolder(claseGenericaEvidencia);
            List<SIRALAB_DAL.Documento> listaEvidenciaAdd;
            List<SIRALAB_DAL.Documento> listaEvidenciaUpd;
            List<SIRALAB_DAL.Documento> listaEvidenciaDel;
            
            if (listaEvidenciasGuardadas != null)
            {
                listaEvidenciaAdd = listaEvidenciasGuardadas.Except(listaEvidenciaOriginal, rel => rel.DocumentoID).ToList();
                listaEvidenciaUpd = listaEvidenciaOriginal.Intersect(listaEvidenciasGuardadas, new EvidenciaEqualityComparer()).ToList();
                listaEvidenciaDel = listaEvidenciaOriginal.Except(listaEvidenciasGuardadas, rel => rel.DocumentoID).ToList();

                if (listaEvidenciaAdd != null && listaEvidenciaAdd.Count > 0)
                {
                    foreach (SIRALAB_DAL.Documento evidenciaInserta in listaEvidenciaAdd)
                    {
                        ResultOperacionArchivo result = new ResultOperacionArchivo();
                        result.evidencia = evidenciaInserta;
                        try
                        {
                            evidenciaInserta.Nombre = pathFolder + "\\" + Path.GetFileName(evidenciaInserta.Nombre);
                            result.codeError = ws.InsertFile(evidenciaInserta.Nombre, evidenciaInserta.BufferData);
                            result.comentarios = "Insertado correctamente";
                        }
                        catch (Exception ex)
                        {
                            result.codeError = -1;
                            result.comentarios = ex.Message;
                        }
                        finally
                        {
                            listaResultados.Add(result);
                        }

                    }
                }

                if (listaEvidenciaUpd != null && listaEvidenciaUpd.Count > 0)
                {
                    foreach (SIRALAB_DAL.Documento evidenciaActualiza in listaEvidenciaUpd)
                    {
                        SIRALAB_DAL.Documento evidenciaActual = listaEvidenciaOriginal.Where(i => i.DocumentoID == evidenciaActualiza.DocumentoID).FirstOrDefault();

                        if (evidenciaActual != null && evidenciaActual.BufferData == null)
                        {
                            ResultOperacionArchivo result = new ResultOperacionArchivo();
                            result.evidencia = evidenciaActual;
                            try
                            {
                                result.codeError = 0;
                                result.comentarios = "Actualizado correctamente";
                            }
                            catch (Exception ex)
                            {
                                result.codeError = -1;
                                result.comentarios = ex.Message;
                            }
                            finally
                            {
                                listaResultados.Add(result);
                            }
                        }
                        else if (evidenciaActual.BufferData != null)
                        {
                            ResultOperacionArchivo result = new ResultOperacionArchivo();
                            result.evidencia = evidenciaActual;
                            try
                            {
                                evidenciaActual.Nombre = pathFolder + Path.GetFileName(evidenciaActual.Nombre);
                                result.codeError = ws.InsertFile(evidenciaActual.Nombre, evidenciaActual.BufferData);
                                result.comentarios = "Insertado correctamente";
                            }
                            catch (Exception ex)
                            {
                                result.codeError = -1;
                                result.comentarios = ex.Message;
                            }
                            finally
                            {
                                listaResultados.Add(result);
                            }
                        }
                    }
                }

                if (listaEvidenciaDel != null && listaEvidenciaDel.Count > 0)
                {
                    foreach (SIRALAB_DAL.Documento evidenciaEliminada in listaEvidenciaDel)
                    {
                        //evento.Evidencia.Remove(evidenciaEliminada);
                        ResultOperacionArchivo result = new ResultOperacionArchivo();
                        result.evidencia = evidenciaEliminada;
                        evidenciaEliminada.Nombre = pathFolder + "\\" + Path.GetFileName(evidenciaEliminada.Nombre);
                        try
                        {
                            result.codeError = ws.DeleteFile(evidenciaEliminada.Nombre);
                            result.comentarios = "Eliminado correctamente";
                        }
                        catch (Exception ex)
                        {
                            result.codeError = -1;
                            result.comentarios = ex.Message;
                        }
                        finally
                        {
                            listaResultados.Add(result);
                        }
                    }
                }
            }

            return listaResultados;
        }
        public static string GetPathFolder<T>(T claseAccion) where T : class
        {
            ApplicationSettings aS = new ApplicationSettings();
            string pathFinal = string.Empty;
            string tipo = claseAccion != null ? claseAccion.GetType().BaseType.Name : string.Empty;
            int id = GetIdObject(claseAccion);

            string pathEvidencia = aS.NombreCarpetaEvidencia;
            string PathOrdenTrabajo = aS.PathOrdenTrabajo;
            string PathMuestra = aS.PathMuestra;
            string PathAnalisis = aS.PathAnalisis;

            DocumentoEntidad ot = GetOrdenTrabajo(claseAccion);

            if (tipo == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
            {
                pathFinal += PathOrdenTrabajo;
                pathFinal += "\\" + id.ToString().PadLeft(6, '0');
                pathFinal += pathEvidencia;

            }
            else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
            {
                pathFinal += PathOrdenTrabajo;
                pathFinal += "\\" + ot.OrdenTrabajoID.ToString().PadLeft(6, '0');
                pathFinal += PathAnalisis;
                pathFinal += "\\" + id.ToString().PadLeft(6, '0');
                pathFinal += pathEvidencia;
            }
            else if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
            {
                SIRALAB_DAL.Muestra m = (claseAccion as SIRALAB_DAL.Muestra);
                string mPath = (m.NivelID == 2) ? "Compuesta" : "Simple";
                /*
                 * hay que obtener el nivel y referenciarlo para los directorios de los archivos
                 */
                pathFinal += PathOrdenTrabajo;
                pathFinal += "\\" + ot.OrdenTrabajoID.ToString().PadLeft(6, '0');
                pathFinal += PathAnalisis;
                pathFinal += "\\" + ot.AnalisisID.ToString().PadLeft(6, '0');
                pathFinal += PathMuestra;
                pathFinal += "\\" + mPath;
                pathFinal += "\\" + id.ToString().PadLeft(6, '0');

                pathFinal += pathEvidencia;
            }
            else
            {
                pathFinal = string.Empty;
            }
            return pathFinal;
        }
        public static int GetIdObject<T>(T claseAccion)
        {
            int id = 0;
            string tipo = claseAccion.GetType().BaseType.Name;

            if (tipo == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
            {
                SIRALAB_DAL.OrdenTrabajo ordenTrabajo = (claseAccion as SIRALAB_DAL.OrdenTrabajo);
                id = ordenTrabajo.OrdenTrabajoID;
            }
            else if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
            {
                SIRALAB_DAL.Muestra muestra = (claseAccion as SIRALAB_DAL.Muestra);
                id = muestra.MuestraID;
            }
            else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
            {
                SIRALAB_DAL.Analisi analisis = (claseAccion as SIRALAB_DAL.Analisi);
                id = analisis.AnalisisID;
            }

            return id;
        }
        public static DocumentoEntidad GetOrdenTrabajo<T>(T claseAccion)
        {
            DocumentoEntidad docEntidad = new DocumentoEntidad();
            string tipo = claseAccion.GetType().BaseType.Name;
            int id = 0;

            try
            {
                if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
                {
                    Muestra_BL mtBL = new Muestra_BL();

                    SIRALAB_DAL.Muestra muestra = (claseAccion as SIRALAB_DAL.Muestra);
                    id = (int)muestra.MuestraID;

                    SIRALAB_DAL.Muestra muestraBusqueda = mtBL.Obten_Muestra(id);
                    if (muestraBusqueda != null)
                    {
                        docEntidad.OrdenTrabajoID = muestraBusqueda.Analisi.OrdenTrabajo.OrdenTrabajoID;
                        docEntidad.AnalisisID = muestraBusqueda.Analisi.AnalisisID;
                        docEntidad.MuestraID = muestraBusqueda.MuestraID;
                    }
                }
                else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
                {
                    Analisis_BL anlsBL = new Analisis_BL();

                    SIRALAB_DAL.Analisi analisis = (claseAccion as SIRALAB_DAL.Analisi);
                    id = (int)analisis.AnalisisID;

                    SIRALAB_DAL.Analisi analisisBusqueda = anlsBL.Obten_Analisis(id);

                    if (analisisBusqueda != null)
                    {
                        docEntidad.OrdenTrabajoID = analisisBusqueda.OrdenTrabajoID;
                        docEntidad.AnalisisID = analisisBusqueda.AnalisisID;
                        docEntidad.MuestraID = null;
                    }
                }
                else
                {
                    docEntidad = new DocumentoEntidad
                    {
                        AnalisisID = 0,
                        MuestraID = 0,
                        OrdenTrabajoID = 0
                    };
                }
            }
            catch (Exception ex)
            {

            }

            return docEntidad;
        }
        public static bool ExisteDocumento(string path)
        {
            FileTransfer.FileTransferSoapClient ws = new FileTransfer.FileTransferSoapClient();
            if (ws.IfFileExists(path))
                return true;
            else
                return false;
        }
        public static bool CargarArchivoFisico(SIRALAB_DAL.Documento evidencia)
        {

            bool result = false;
            SIRALAB_BL.Archivos.ResultOperacionArchivo resultado = Archivos.ObtenerDocumentoFisico(evidencia.Nombre);

            if (resultado.codeError == 0)
            {
                result = true;

                Archivos fileAux = new Archivos();
                fileAux = Archivos.ObtenPropiedades(evidencia.Nombre);
                //evidencia.Titulo = fileAux.Nombre;
                evidencia.TipoMime = fileAux.TipoMime;
                //evidencia.ImagePreView = fileAux.ImagenPreliminar;
                evidencia.BufferData = resultado.evidencia.BufferData;
            }
            return result;
        }
        public class ResultOperacionArchivo
        {
            public int codeError { get; set; }
            public SIRALAB_DAL.Documento evidencia { get; set; }
            public string comentarios { get; set; }

        }
        public class EvidenciaEqualityComparer : IEqualityComparer<SIRALAB_DAL.Documento>
        {
            public bool Equals(SIRALAB_DAL.Documento evidencia1, SIRALAB_DAL.Documento evidencia2)
            {
                if (evidencia1.DocumentoID == evidencia2.DocumentoID)
                    return true;
                
                return false;
            }
            public int GetHashCode(SIRALAB_DAL.Documento evidencia)
            {
                return new { evidencia.DocumentoID, evidencia.Nombre }.GetHashCode();
            }
        }
    }
}
