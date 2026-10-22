using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SIRALAB_WS_FileTransfer
{
    /// <summary>
    /// Descripción breve de FileTransfer
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class FileTransfer : System.Web.Services.WebService
    {
        public FileTransfer() { }

        [WebMethod(Description = "Recibe un buffer lleno con el archivo a guardar y lo escribe en disco", MessageName = "InsertarArchivo")]
        public Int16 InsertFile(string fileName, byte[] bufferData)
        {
            string ruta = "";
            try
            {
                ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;

                FileInfo file = new System.IO.FileInfo(ruta);

                if (!file.Exists)
                {

                    DirectoryInfo dir = new DirectoryInfo(file.DirectoryName);
                    if (!dir.Exists)
                        dir.Create();
                    FileStream stream = file.Create();

                    stream.Write(bufferData, 0, bufferData.Length);

                    stream.Close();

                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("No se pudo guardar el archivo.  " + ex.Message);
            }
        }

        [WebMethod(Description = "Regresa un buffer lleno con el documento, si es que existe", MessageName = "ObtenerDocumento")]
        public byte[] GetFile(string fileName)
        {

            byte[] buffer = null;
            string ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;

            FileInfo existingFile = null;
            try
            {
                existingFile = new FileInfo(ruta);
                if (existingFile.Exists)
                {
                    buffer = new byte[existingFile.Length];
                    FileStream streamFile = new FileStream(ruta, FileMode.Open);
                    streamFile.Read(buffer, 0, buffer.Length);
                    streamFile.Close();
                    return buffer;
                }
                throw new ArgumentException("No existe el archivo que solicitado");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("No se solicitar el archivo.  " + ex.Message);
            }
        }

        [WebMethod(Description = "Recibe el nombre del documento y lo elimina", MessageName = "EliminarArchivo")]
        public int DeleteFile(string fileName)
        {
            string ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;
            FileInfo existingFile = null;
            try
            {
                existingFile = new FileInfo(ruta);
                if (existingFile.Exists)
                {
                    existingFile.Delete();
                    return 0;
                }
                else
                {
                    throw new ArgumentException("No se puedo eliminar el archivo");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("No se eliminar el archivo.  " + ex.Message);
            }
        }

        [WebMethod(Description = "Verifica que exista el documento", MessageName = "IfFileExist")]
        public bool IfFileExists(string fileName)
        {
            string ruta = ConfigurationManager.AppSettings["PathArchivos"].ToString() + fileName;
            FileInfo existingFile = null;
            existingFile = new FileInfo(ruta);
            if (existingFile.Exists)
                return true; 
            else
                return false; 
        }
    }
}
