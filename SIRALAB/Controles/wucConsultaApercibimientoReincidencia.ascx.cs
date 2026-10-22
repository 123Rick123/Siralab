using MimeKit;
using SIRALAB.ServRefAmara;
using SIRALAB_BL;
using SIRALAB_BL.negocio.Comun;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SIRALAB.Controles
{
    public partial class wucConsultaApercibimientoReincidencia : System.Web.UI.UserControl
    {
        LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();
        public string IdApercibimiento;
        public string IdReincidencia;
        public string banderaImpugnacion;
        public string banderaRegresar;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnDescargaDoc);

            IdApercibimiento = Request.QueryString["idApercibimiento"];
            IdReincidencia = Request.QueryString["IdReincidencia"];
            banderaImpugnacion = Request.QueryString["banderaImpugnacion"];
            banderaRegresar = Request.QueryString["banderaRegresar"];

            if (banderaImpugnacion == "1")
            {
                btnVerImpugnacion.Visible = true;
            }
            else
            {
                btnVerImpugnacion.Visible = false;
            }
            llenarCampos();
            CargarCampos();

        }
        public void llenarCampos()
        {

            //llenar el ddlTrimestre
            ListItem trimestre;
            trimestre = new ListItem("Seleccione", "0");
            ddlTrimestre.Items.Add(trimestre);
            for (int i = 1; i < 5; i++)
            {
                switch (i)
                {
                    case 1:
                        var trimestre1 = " Enero-Marzo";
                        trimestre = new ListItem(i.ToString() + trimestre1, i.ToString());
                        break;
                    case 2:
                        var trimestre2 = " Abril-Junio";

                        trimestre = new ListItem(i.ToString() + trimestre2, i.ToString());
                        break;
                    case 3:
                        var trimestre3 = " Julio-Septiembre";
                        trimestre = new ListItem(i.ToString() + trimestre3, i.ToString());
                        break;
                    case 4:
                        var trimestre4 = " Octubre-Diciembre";
                        trimestre = new ListItem(i.ToString() + trimestre4, i.ToString());
                        break;
                    default:
                        break;
                }




                ddlTrimestre.Items.Add(trimestre);
            }

            //llena el ddlAnio
            ListItem Anio;
            Anio = new ListItem("Seleccione", "0");
            ddlAnio.Items.Add(Anio);
            for (int i = 0; i < 6; i++)
            {
                var fecha = DateTime.Now.Year;
                if (i != 0)
                {
                    fecha = DateTime.Now.Year - i;
                    Anio = new ListItem(fecha.ToString(), fecha.ToString());
                    ddlAnio.Items.Add(Anio);

                }
                else
                {
                    Anio = new ListItem(fecha.ToString(), fecha.ToString());
                    ddlAnio.Items.Add(Anio);
                }

            }

        }
        public void CargarCampos()
        {
            if (IdApercibimiento != null)
            {
                var idApercibimiento = int.Parse(IdApercibimiento);
                var Apercibimiento = ApercibidosReincientesBLL.BuscarApercibimientoByID(idApercibimiento);
                //Se valida si el valor del año del apercibimiento se encuentra dentro del ddl y se añade de ser necesario
                if(ddlAnio.Items.FindByValue(Apercibimiento.Año.ToString()) == null)
                    ddlAnio.Items.Add(Apercibimiento.Año.ToString());

                txtNoOficioResolucion.Text = Apercibimiento.Numero_Oficio;
                txtNoOficioResolucion.Enabled = false;
                txtFechaResolucion.Text = Apercibimiento.Fecha_Resolucion.Value.ToShortDateString();
                txtFechaResolucion.Enabled = false;
                txtFechaNotificacioResolucion.Text = Apercibimiento.Fecha_Notificacion.Value.ToShortDateString();
                txtFechaNotificacioResolucion.Enabled = false;
                txtidDoc.Value = Apercibimiento.Evidencia;
                flDocumento.Visible = false;
                nameFile.Visible = false;
                ddlTrimestre.SelectedValue = Apercibimiento.Trimestre.ToString();
                ddlTrimestre.Enabled = false;
                ddlAnio.SelectedValue = Apercibimiento.Año.ToString();
                ddlAnio.Enabled = false;
                txtNoExpediente.Text = Apercibimiento.NoExpediente;
                txtNoExpediente.Enabled = false;
                txtIniciales.Text = Apercibimiento.InicialesResponsable;
                txtIniciales.Enabled = false;
                txtObservaciones.Value = Apercibimiento.Observaciones;
                txtObservaciones.Disabled = true;
                txtIdApercibimiento.Value = Apercibimiento.Id.ToString();
                btnDescarga.Visible = true;

                Laboratorio_BL labBLL = new Laboratorio_BL();
                var laboratorio = labBLL.Obten_Laboratorio(int.Parse(Apercibimiento.Id_Laboratorio.ToString()));
                DataTable dt = new DataTable();
                dt.Columns.Add("idLaboratorio", typeof(int));
                dt.Columns.Add("NombreLaboratorio", typeof(string));
                dt.Columns.Add("RFCLaboratorio", typeof(string));


                DataRow rw = dt.NewRow();
                rw["idLaboratorio"] = laboratorio.LaboratorioID;
                rw["NombreLaboratorio"] = laboratorio.Nombre;
                rw["RFCLaboratorio"] = laboratorio.RFC;

                dt.Rows.Add(rw);

                grdHistorico.DataSource = dt;
                grdHistorico.DataBind();
                if (IdReincidencia != null && IdReincidencia != string.Empty)
                {
                    btnCerrarPagina.Visible = false;
                }
                if (IdApercibimiento != null && (IdReincidencia != null && IdReincidencia != string.Empty))
                {
                    btnCerrarPagina.Visible = false;
                }
            }





        }

        protected void btnCerrarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ApercibidosYReincidentes.aspx");

        }

        public void DescargarArchivo_Click(object sender, EventArgs e)
        {
            string id = txtidDoc.Value;
            HttpWebRequest request = CreateWebRequest();

            request.Method = "POST";
            request.ContentType = "text/xml;charset=\"utf-8\"";

            /*
             * Request
             */
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
                     <mod:login>" + WebConfigurationManager.AppSettings["AmaraUser"] + @"</mod:login>
                     <mod:password>" + WebConfigurationManager.AppSettings["AmaraPass"] + @"</mod:password>
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

            byte[] resultby;
            byte[] bufferby = new byte[4096];
            WebResponse response = request.GetResponse();

            /*obtención de los Attach del response*/

            var contentType = MimeKit.ContentType.Parse(response.ContentType);

            var mime = (Multipart)MimeEntity.Load(contentType, response.GetResponseStream());

            int i = 1;
            foreach (var attachment in mime.OfType<MimePart>())
            {
                if (attachment.ContentId == "contenido")
                {
                    MemoryStream stream = new MemoryStream();

                    attachment.Content.DecodeTo(stream);
                    stream.WriteTo(strDocument);
                    strDocument.Position = 0;
                }
            }

            /*Consulta el webservice de amara para obtener la información del archivo*/

            OpServiciosClient psc = new OpServiciosClient();
            ResponseVerDoc respuesta = psc.verDoc(new RequestVerDoc()
            {
                iddocumento = id,
                login = WebConfigurationManager.AppSettings["AmaraUser"],
                password = WebConfigurationManager.AppSettings["AmaraPass"],
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

            /*Descarga archivo*/
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AppendHeader("Content-Disposition", cd.ToString());

            HttpContext.Current.Response.ContentType = respuesta.MimeType;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

            strDocument.Position = 0;
            strDocument.WriteTo(HttpContext.Current.Response.OutputStream);
            strDocument.Close();

            HttpContext.Current.Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            HttpContext.Current.Response.End();
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

        protected void Descarga_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtIdApercibimiento.Value);

            LabApercibidosYReincidentesBLL BLL = new LabApercibidosYReincidentesBLL();
            var consulta = BLL.BuscarApercibimientoByID(id);

            var nombresGerenteYSubgerente = BLL.BuscarGerenteYSubgerente();




            PlantillaBLL operaciones;
            PlantillaDTO documento;
            var informacionGeneral = new InformacionGeneral();


            string nombreDocAcuse = "APERCIBIMIENTO_" + consulta.RFC + ".docx";
            //ACUSE
            string pathDocumentoAcuse = Server.MapPath("~/Reportes/Plantillas/tmp");
            var plantillaAcuse = string.Empty;
            string pathPlantillaAcuse = string.Empty;

            plantillaAcuse = "Plantilla_APERCIBIMIENTO.docx";

            if (!Directory.Exists(pathDocumentoAcuse))
                Directory.CreateDirectory(pathDocumentoAcuse);

            pathPlantillaAcuse = Path.Combine(Server.MapPath("~/Reportes/Plantillas/"), plantillaAcuse);
            pathDocumentoAcuse = Path.Combine(pathDocumentoAcuse, nombreDocAcuse);

            System.IO.File.Copy(pathPlantillaAcuse, pathDocumentoAcuse, true);
            //aqui poner la consulta para recuperar al laboratorio
            informacionGeneral.noExpediente = consulta.NoExpediente;
            informacionGeneral.nombreLaboratorio = consulta.Nombre;
            informacionGeneral.noResolucion = consulta.Numero_Oficio;
            informacionGeneral.fechaEmision = consulta.Fecha_Resolucion.Value.ToShortDateString();
            informacionGeneral.fechaNotificacion = consulta.Fecha_Notificacion.Value.ToShortDateString();
            informacionGeneral.GerenteCalidad = nombresGerenteYSubgerente.NombreGerente;
            informacionGeneral.subgerente = nombresGerenteYSubgerente.NombreSubgerente;
            informacionGeneral.Memorando = consulta.Memorando;
            informacionGeneral.Iniciales = consulta.InicialesResponsable;
            informacionGeneral.fechaCaptura = consulta.Fecha_captura.Value.ToShortDateString();
            actualizaInformacionDoc(informacionGeneral, pathDocumentoAcuse, out operaciones, out documento);

            //este método va a remplazar todos los tag
            bool reemplazoExitoAcuse = operaciones.ReemplazarEtitquetas(documento);
            if (reemplazoExitoAcuse)
            {
                //Se comenta código para convertir a PDF 
                ////se recupera el documento actualizado
                //byte[] documentoByte = operaciones.ArchivoABytes(pathDocumentoAcuse);

                ////convirte PDF
                //string pathDocumentoPDF = string.Empty;
                //pathDocumentoPDF = Path.Combine(Server.MapPath("~/Reportes/Plantillas/tmp/"), Path.GetFileNameWithoutExtension(pathDocumentoAcuse) + ".pdf");

                //Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                //Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(pathDocumentoAcuse);
                //doc.SaveAs2(pathDocumentoPDF, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                //doc.Close();
                //app.Quit();

                nombreDocAcuse = System.IO.Path.GetFileNameWithoutExtension(pathDocumentoAcuse) + ".docx"; //+ ".pdf";
                //pathDocumentoAcuse = pathDocumentoPDF;




              
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("content-disposition", "attachment; filename=" + nombreDocAcuse);
                Response.TransmitFile(Server.MapPath("~/Reportes/Plantillas/tmp/" + nombreDocAcuse));
                Response.Flush();
                Response.End();

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Guardar", "GuardarApercibimiento()", true);


            }


        }






        /*Método para remplazar las Tags */
        private static void actualizaInformacionDoc(InformacionGeneral infoGnral, string pathDocumento, out PlantillaBLL operaciones, out PlantillaDTO documento)
        {

            /*Remplaza plantilla*/
            operaciones = new PlantillaBLL();
            documento = new PlantillaDTO();


            documento.EsNuevoDocumento = false;
            documento.Url = pathDocumento;
            documento.RutaPlantilla = pathDocumento;

            //Encabezado
            documento.EtiquetasEncabezado.Add("tagMemorando", infoGnral.Memorando);

            documento.EtiquetasEncabezado.Add("tagFechaEmision", infoGnral.fechaEmision);
            //Cuerpo

            documento.EtiquetasCuerpo.Add("tagExpediente", infoGnral.noExpediente);
            documento.EtiquetasCuerpo.Add("tagGerenteCalidad", infoGnral.GerenteCalidad);
            documento.EtiquetasCuerpo.Add("tagNoOficioResolucion ", infoGnral.noResolucion);

            documento.EtiquetasCuerpo.Add("tagNombreLab", infoGnral.nombreLaboratorio);
            documento.EtiquetasCuerpo.Add("tagNoOficioRes", infoGnral.noResolucion);
            documento.EtiquetasCuerpo.Add("tagFechaEmisionTabla", infoGnral.fechaEmision);
            documento.EtiquetasCuerpo.Add("tagFechaNotificacion", infoGnral.fechaNotificacion);
            documento.EtiquetasCuerpo.Add("tagSubgerente", infoGnral.subgerente);
            documento.EtiquetasCuerpo.Add("tagIniciales", infoGnral.Iniciales);
            var fecha = Convert.ToDateTime(infoGnral.fechaCaptura);
            var fechaConvertida = fecha.ToString("f", CultureInfo.CreateSpecificCulture("es-MX"));
            var arregloFecha = fechaConvertida.Split(' ');
            var fechaCaptura = arregloFecha[1] + " " + arregloFecha[2] + " " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(arregloFecha[3]) + " " + arregloFecha[4] + " " + arregloFecha[5];
            documento.EtiquetasEncabezado.Add("tagFechaCaptura", fechaCaptura);


        }



        public class InformacionGeneral
        {
            public string Memorando { get; set; }
            public string noExpediente { get; set; }
            public string GerenteCalidad { get; set; }
            public string noResolucion { get; set; }
            public string nombreLaboratorio { get; set; }
            public string fechaEmision { get; set; }
            public string fechaNotificacion { get; set; }
            public string subgerente { get; set; }

            public string Iniciales { get; set; }
            public string fechaCaptura { get; set; }


        }

        protected void lnkDEtalle_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            GridView gv = clickedRow.NamingContainer as GridView;
            var lbCode = gv.DataKeys[clickedRow.RowIndex].Values[0].ToString();

            var lista = ApercibidosReincientesBLL.ObtenerNumerosYVigenciasDeAprobacion(int.Parse(lbCode));

            if (lista.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalDetalleARSinInfo", "MostrarModalARDetalleSinInfo()", true);
            }
            else
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("Numero", typeof(string));
                dt.Columns.Add("FechaFin", typeof(string));

                foreach (var item in lista)
                {
                    DateTime vigencia = (DateTime)item.FechaFin;

                    DataRow rw = dt.NewRow();
                    rw["Numero"] = item.Numero;
                    rw["FechaFin"] = vigencia.ToString("dd/MM/yyyy");

                    dt.Rows.Add(rw);
                }
                gvdetalleAR.DataSource = dt;
                gvdetalleAR.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalDetalleAR", "MostrarModalDetalleAR()", true);
            }
        }

        protected void btnVerImpugnacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DesgloseHistoricoImpugnacionApercibimiento.aspx?idApercibimiento=" + IdApercibimiento + "&IdReincidencia=" + IdReincidencia + "&banderaImpugnacion=" + banderaImpugnacion + "&banderaRegresar=" + banderaRegresar);
        }
    }
}