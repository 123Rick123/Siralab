using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using SIRALAB.ServRefAmara;
using MimeKit;
using System.IO;
using System.Net;
namespace SIRALAB.Utilerias
{
    public class IC2
    {
        //Método utilizado para el módulo de la MI
        public static string SubirarchivoIc2(byte[] archivo, string nombrecompletoarchivo, string carpertaGuardaDocumentos, string tmpIdDocumento = null)
        {
            string iddoc = string.Empty;
            try
            {
                List<ServRefAmara.DatoPropiedad> pr = new List<ServRefAmara.DatoPropiedad>();
                pr.Add(new ServRefAmara.DatoPropiedad()
                {
                    clave = "Titulo",
                    listaValores = new string[] { nombrecompletoarchivo }
                });

                OpServiciosClient s = new OpServiciosClient();

                ResponseImportar respuesta = new ResponseImportar();
                RequestImportar peticion = new RequestImportar();
                peticion.archivo = archivo;
                peticion.datosAuditoria = "Administrador";
                peticion.idenUsuarioLog = "disponibilidad";
                peticion.nombre = nombrecompletoarchivo;
                peticion.padre = carpertaGuardaDocumentos;
                peticion.login = WebConfigurationManager.AppSettings["AmaraUser"];
                peticion.password = WebConfigurationManager.AppSettings["AmaraPass"];
                peticion.propiedades = pr.ToArray();
                peticion.skip = "N";
                peticion.tipo_documental = WebConfigurationManager.AppSettings["TipoDocumental"];
                respuesta = s.importar(peticion);
                iddoc = respuesta.identificador;

                if (tmpIdDocumento != null)
                {
                    string[] arra = { tmpIdDocumento.ToString() };
                    ServRefAmara.RequestBorrar Borrar = new RequestBorrar()
                    {
                        identificadores = arra,
                        usuario = WebConfigurationManager.AppSettings["AmaraUser"],
                        password = WebConfigurationManager.AppSettings["AmaraPass"],
                        idenUsuarioLog = "disponibilidad",
                        datosAuditoria = "Administrador"
                    };

                    ResponseBorrar respuestaBorrar = new ResponseBorrar();
                    respuestaBorrar = s.borrar(Borrar);
                }
            }
            catch (Exception e)
            {
                iddoc = string.Empty;
            }
            return iddoc;
        }
        public static byte[] Descarga(string IdDocumento)
        {
            OpServiciosClient s = new OpServiciosClient();

            ResponseVerDoc Response = new ResponseVerDoc();
            RequestVerDoc Request = new RequestVerDoc();

            Request.iddocumento = IdDocumento;
            Request.idenUsuarioLog = "disponibilidad";
            Request.login = WebConfigurationManager.AppSettings["AmaraUser"];
            Request.password = WebConfigurationManager.AppSettings["AmaraPass"];
            Request.skip = "N";
            Request.datosAuditoria = "Administrador";

            Response = s.verDoc(Request);
            return Response.archivo;
        }

        public static byte[] CopyStreamByte(Stream src)
        {
            byte[] buffer;

            MemoryStream target = new MemoryStream();

            var tamano = src.Length;
            buffer = new byte[tamano];
            src.Read(buffer, 0, buffer.Length);
            target.Close();

            return buffer;
        }

        public static DocumentoDescaga DescargarArchivo(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            HttpWebRequest request = CreateWebRequest();
            request.Method = "POST";
            request.ContentType = "text/xml;charset=\"utf-8\"";
            var amaraUser = WebConfigurationManager.AppSettings["AmaraUser"];
            var amaraPass = WebConfigurationManager.AppSettings["AmaraPass"];
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"
            <soapenv:Envelope 
            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
            xmlns:impl=""http://impl.vista.archiva.indra.es"" 
            xmlns:mod=""http://modelo.vista.archiva.indra.es"">
               <soapenv:Header/>
               <soapenv:Body>
                  <impl:request7>
                     <mod:iddocumento>" + id + @"</mod:iddocumento>
                     <mod:idenUsuarioLog>disponibilidad</mod:idenUsuarioLog>
                     <mod:login>" + amaraUser + @"</mod:login>
                     <mod:password>" + amaraPass + @"</mod:password>
                     <mod:propsExtension>
                        <!--Zero or more repetitions:-->
                        <impl:item>
                           <mod:claves>Titulo</mod:claves>
                           <mod:valores>
                              <!--Zero or more repetitions:-->
                              <impl:item>Documento prueba SOAP psl</impl:item>
                           </mod:valores>
                        </impl:item>
                     </mod:propsExtension>
                     <mod:skip>N</mod:skip>
                     <mod:datosAuditoria>Administrador</mod:datosAuditoria>
                  </impl:request7>
               </soapenv:Body>
            </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            MemoryStream strDocument = new MemoryStream();
            byte[] resultby = new byte[] { };
            byte[] bufferby = new byte[4096];
            WebResponse response = request.GetResponse();

            /*obtención de los Attach del response*/
            var contentType = ContentType.Parse(response.ContentType);
            var mime = (Multipart)MimeEntity.Load(contentType, response.GetResponseStream());

            foreach (var attachment in mime.OfType<MimePart>())
            {

                if (attachment.ContentId == "contenido")
                {
                    MemoryStream stream = new MemoryStream();

                    attachment.Content.DecodeTo(stream);
                    stream.WriteTo(strDocument);
                    strDocument.Position = 0;
                    resultby = IC2.CopyStreamByte(strDocument);
                }
            }

            /*Consulta el webservice de amara para obtener la información del archivo*/
            OpServiciosClient psc = new OpServiciosClient();
            ResponseVerDoc respuesta = psc.verDoc(new RequestVerDoc()
            {
                iddocumento = id,
                login = amaraUser,
                password = amaraPass,
                datosAuditoria = "Administrador",
                idenUsuarioLog = "disponibilidad",
                skip = "N"

            });

            string filename = respuesta.nombre;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = false,
            };

            var resultado = new DocumentoDescaga()
            {
                DocumentoEnBytes = resultby,
                MimeType = respuesta.MimeType,
                ContentDisposition = cd.ToString()
            };
            return resultado;
        }

        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(WebConfigurationManager.AppSettings["UrlAmaraArchiva"]);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        ///consulta info de carpetas o documentos para recuperar la nomenclatura del doc. de la solicitud
        public static string ConsultarInfoPropiedades(string idObjeto)
        {
            OpServiciosClient serClient = new OpServiciosClient();
            ResponseObtenerPropiedades obtenerPropiedades = new ResponseObtenerPropiedades();

            ServRefAmara.RequestObtenerPropiedades obtenerProp = new RequestObtenerPropiedades()
            {
                idenUsuarioLog = "disponibilidad",
                identificador = idObjeto,
                login = WebConfigurationManager.AppSettings["AmaraUser"],
                password = WebConfigurationManager.AppSettings["AmaraPass"],
                datosAuditoria = "Administrador"
            };

            obtenerPropiedades = serClient.obtenerPropiedades(obtenerProp);
            int pos = 0;
            foreach (var item in obtenerPropiedades.propiedades)
            {
                if (item.clave == "Titulo")
                    break;
                pos++;
            }
            return obtenerPropiedades.propiedades[pos].listaValores[0].ToString();
            //return "folio prueba";
        }
    }
    public class DocumentoDescaga
    {
        public byte[] DocumentoEnBytes { get; set; }
        public string MimeType { get; set; }
        public string ContentDisposition { get; set; }
    }
}