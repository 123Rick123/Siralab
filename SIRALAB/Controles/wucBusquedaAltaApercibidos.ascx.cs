using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using static SIRALAB_BL.negocio.LaboratoriosApercibidos.LabApercibidosYReincidentes;
//using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using System.Net;
using System.Web.Configuration;
using System.Xml;
using System.IO;
using MimeKit;
using SIRALAB.Utilerias;
using SIRALAB.ServRefAmara;
using System.Web.Mvc;
using SIRALAB_BL.negocio.Comun;
using SIRALAB_BL;
using SIRALAB_BL.negocio.LaboratoriosApercibidos.Enum;
using System.Globalization;

namespace SIRALAB.Controles
{

    public partial class wucBusquedaAltaApercibidos : System.Web.UI.UserControl
    {
        LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();
        public string IdApercibimiento;
        public string ValTrimestre;
        public string ValAnio;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnDescargaDoc);


            if (!IsPostBack)
            {
                llenarCampos();
                IdApercibimiento = Request.QueryString["idApercibimiento"];
                if (IdApercibimiento != null)
                {
                    txtBanderaEsConsulta.Value = "true";

                    CargarCampos();
                }
                else
                {
                    txtBanderaEsConsulta.Value = "false";
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "calendario", "Calendario()", true);
            }

        }
        public void CargarCampos()
        {
            var idApercibimiento = int.Parse(IdApercibimiento);
            var Apercibimiento = ApercibidosReincientesBLL.BuscarApercibimientoByID(idApercibimiento);

            txtNoOficioResolucion.Text = Apercibimiento.Numero_Oficio;
            txtFechaResolucion.Text = Apercibimiento.Fecha_Resolucion.Value.ToShortDateString();
            txtFechaNotificacioResolucion.Text = Apercibimiento.Fecha_Notificacion.Value.ToShortDateString();
            txtidDoc.Value = Apercibimiento.Evidencia;
            ddlTrimestre.SelectedValue = Apercibimiento.Trimestre.ToString();
            ddlAnio.SelectedValue = Apercibimiento.Año.ToString();
            txtNoExpediente.Text = Apercibimiento.NoExpediente;
            txtIniciales.Text = Apercibimiento.InicialesResponsable;
            txtObservaciones.Value = Apercibimiento.Observaciones;
            txtIdApercibimiento.Value = Apercibimiento.Id.ToString();
            btnDescarga.Style["display"] = "";
            btnDescargaDoc.Style["display"] = "";
            txtNombreBusqueda.Visible = false;
            txtRFC.Visible = false;
            lblNombreBusqueda.Visible = false;
            lblRFC.Visible = false;
            lnkbtnConsultaLaboratorio.Visible = false;
            CheckBox chkb_item;
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

            foreach (GridViewRow item2 in grdHistorico.Rows)
            {
                chkb_item = (CheckBox)item2.FindControl("chkSeleccion");
                chkb_item.Checked = true;
                chkb_item.Enabled = false;
            }
        }
        protected void btnCerrarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ApercibidosYReincidentes.aspx");
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

        protected void SeleccionarAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValTrimestre = ddlTrimestre.SelectedValue;
            ddlTrimestre.Items.Clear();
            var TrimestreActual = 0;
            //llenar el ddlTrimestre
            ListItem trimestre;
            trimestre = new ListItem("Seleccione", "0");
            ddlTrimestre.Items.Add(trimestre);
            if (ddlAnio.SelectedValue == DateTime.Now.Year.ToString())
            {
                switch (DateTime.Now.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        TrimestreActual = 1;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        TrimestreActual = 2;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        TrimestreActual = 3;
                        break;
                    case 10:
                    case 11:
                    case 12:
                        TrimestreActual = 4;
                        break;
                    default:
                        break;
                }
                for (int i = 1; i < 5; i++)
                {
                    if (TrimestreActual >= i)
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
                }
                if (ValTrimestre != "0")
                {

                    if (TrimestreActual >= int.Parse(ValTrimestre))
                    {
                        ddlTrimestre.SelectedValue = ValTrimestre;
                    }


                }
            }
            else
            {
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
                ddlTrimestre.SelectedValue = ValTrimestre;
            }

        }
        protected void lnkbtnValidarCampos_Click(object sender, EventArgs e)
        {
            var idLaboratorio = 0;
            CheckBox chkb_item;
            foreach (GridViewRow item in grdHistorico.Rows)
            {
                chkb_item = (CheckBox)item.FindControl("chkSeleccion");
                if (chkb_item.Checked)
                {
                    idLaboratorio = Convert.ToInt32(grdHistorico.DataKeys[item.DataItemIndex].Values["idLaboratorio"]);
                    txtIdLabBusqueda.Value = idLaboratorio.ToString();
                }
            }

            var errores = string.Empty;

            if (txtFechaResolucion.Text != string.Empty && txtFechaNotificacioResolucion.Text != string.Empty)
            {

                if (RExprFechaNotificacion.IsValid == true && RExprFechaResolucion.IsValid == true)
                {
                    var fechaRes = Convert.ToDateTime(txtFechaResolucion.Text);
                    var fechaNotifi = Convert.ToDateTime(txtFechaNotificacioResolucion.Text);

                    if (fechaRes > fechaNotifi)
                    {
                        var texto = TextoAlertaSimple;
                        texto.InnerHtml = "La fecha de resolución o puede ser mayor a la fecha de notificación.";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
                        errores += "ErrorFechas";
                    }


                }

            }


            if (idLaboratorio != 0)
            {
                errores += string.Empty;
                MensajeErrorIdLab.InnerText = string.Empty;
            }
            else
            {

                errores += "ErrorIdLab";
                MensajeErrorIdLab.InnerText = "Seleccione un laboratorio.";
                var texto = TextoAlertaSimple;
                texto.InnerHtml = "Es obligatorio seleccionar un laboratorio para continuar con el registro de apercibimiento. ";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
            }
            if (txtNoOficioResolucion.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorNoResolucion.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorNoOficio";
                MensajeErrorNoResolucion.InnerText = "Este campo es obligatorio.";
            }
            if (txtFechaResolucion.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorFechaResolucion.InnerText = string.Empty;
                if (RExprFechaResolucion.IsValid == false)
                {
                    errores += "ErrorFechaResolucion";

                }
                if (Convert.ToDateTime(txtFechaResolucion.Text) > DateTime.Now)
                {
                    errores += "ErrorFechaResolucion";
                    MensajeErrorFechaResolucion.InnerText = "La fecha de resolución no puede ser mayor a la fecha actual.";
                }
            }
            else
            {
                errores += "ErrorFechaResolucion";
                MensajeErrorFechaResolucion.InnerText = "Este campo es obligatorio.";
            }
            if (txtFechaNotificacioResolucion.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorFechaNotificacion.InnerText = string.Empty;
                if (RExprFechaNotificacion.IsValid == false)
                {
                    errores += "ErrorFechaNotificacion";

                }
                if (Convert.ToDateTime(txtFechaNotificacioResolucion.Text) > DateTime.Now )
                {
                    errores += "ErrorFechaNotificacion";
                    MensajeErrorFechaNotificacion.InnerText = "La fecha de notificación no puede ser mayor a la fecha actual.";
                }
            }
            else
            {
                errores += "ErrorFechaNotificacion";
                MensajeErrorFechaNotificacion.InnerText = "Este campo es obligatorio.";
            }
            if (txtObservaciones.Value != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorObservaciones.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorObservaciones";
                MensajeErrorObservaciones.InnerText = "Este campo es obligatorio.";
            }


            if (txtbasecode.Value != "undefined" && txtbasecode.Value != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorDocumento.InnerText = string.Empty;
            }
            else
            {
                if (txtErrorDocumento.Value != string.Empty)
                {
                    errores += "ErrorDocumento";
                    MensajeErrorDocumento.InnerText = txtErrorDocumento.Value;

                }
                else
                {
                    if (txtBanderaEsConsulta.Value == "true")
                    {
                        errores += string.Empty;
                        MensajeErrorDocumento.InnerText = string.Empty;
                    }
                    else
                    {
                        errores += "ErrorDocumentoVacio";
                        MensajeErrorDocumento.InnerText = "Este campo es obligatorio.";
                    }
                }


            }


            if (ddlAnio.SelectedValue != "0")
            {
                errores += string.Empty;
                MensajeErrorAnio.InnerText = string.Empty;

            }
            else
            {
                errores += "ErrorAnio";
                MensajeErrorAnio.InnerText = "Este campo es obligatorio.";
            }
            if (ddlTrimestre.SelectedValue != "0")
            {
                errores += string.Empty;
                MensajeErrorTrimestre.InnerText = string.Empty;

            }
            else
            {
                errores += "ErrorTrimestre";
                MensajeErrorTrimestre.InnerText = "Este campo es obligatorio.";
            }


            if (txtNoExpediente.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorNoExpediente.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorNoExpediente";
                MensajeErrorNoExpediente.InnerText = "Este campo es obligatorio.";
            }

            if (txtIniciales.Text != string.Empty)
            {
                errores += string.Empty;
                MensajeErrorIniciales.InnerText = string.Empty;
            }
            else
            {
                errores += "ErrorIniciales";
                MensajeErrorIniciales.InnerText = "Este campo es obligatorio.";
            }
            if (ddlAnio.SelectedValue != "0" && ddlTrimestre.SelectedValue != "0")
            {
                //valida fecha de vigencia 
                int trimestre = 0;
                switch (DateTime.Now.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        trimestre = 01;
                        break;

                    case 4:
                    case 5:
                    case 6:
                        trimestre = 02;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        trimestre = 03;
                        break;
                    case 10:
                    case 11:
                    case 12:
                        trimestre = 04;
                        break;
                }
                var anioVigencia = DateTime.Now.Year - 5;
                if (trimestre > int.Parse(ddlTrimestre.SelectedValue) && int.Parse(ddlAnio.SelectedValue) <= anioVigencia)
                {
                    var texto = TextoAlertaSimple;
                    texto.InnerHtml = "Sólo es posible seleccionar del trimestre " + trimestre + " en adelante.";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
                    errores += "ErrorSeleccionTrimestres";
                }

            }
            else
            {
                errores += string.Empty;
            }

            if (errores.Length == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "MostrarModalConfirm", "MostrarModalConfirm()", true);
            }

        }

        protected void lnkbtnConsultaLaboratorio_Click(object sender, EventArgs e)
        {

            var NombreLaboratorio = txtNombreBusqueda.Text;
            var rfc = txtRFC.Text;

            if ((NombreLaboratorio.Trim() == string.Empty) && (rfc.Trim() == string.Empty))
            {
                var texto = TextoAlertaSimple;
                texto.InnerHtml = "Escribe un nombre de laboratorio o RFC";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
                /*     $('#alertaSimple').modal({ backdrop: 'static', keyboard: false })*/
                upModal.Update();
            }

            else
            {
                var lista = ApercibidosReincientesBLL.ObtenerListaLab(txtRFC.Text, txtNombreBusqueda.Text);

                DataTable dt = new DataTable();
                dt.Columns.Add("idLaboratorio", typeof(int));
                dt.Columns.Add("NombreLaboratorio", typeof(string));
                dt.Columns.Add("RFCLaboratorio", typeof(string));

                foreach (var item in lista)
                {
                    DataRow rw = dt.NewRow();
                    rw["idLaboratorio"] = item.idLaboratorio;
                    rw["NombreLaboratorio"] = item.NombreLaboratorio;
                    rw["RFCLaboratorio"] = item.RFCLaboratorio;

                    dt.Rows.Add(rw);
                }
                grdHistorico.DataSource = dt;
                grdHistorico.DataBind();
            }
        }
        public void chkSeleccion_click(object sender, EventArgs e)
        {
            var idLaboratorio = 0;
            CheckBox chkb_item;
            foreach (GridViewRow item in grdHistorico.Rows)
            {
                chkb_item = (CheckBox)item.FindControl("chkSeleccion");
                if (chkb_item.Checked)
                {

                    idLaboratorio = Convert.ToInt32(grdHistorico.DataKeys[item.DataItemIndex].Values["idLaboratorio"]);

                    LabApercibidosYReincidentesBLL apercibidoBLL = new LabApercibidosYReincidentesBLL();
                    var existeRegistro = apercibidoBLL.laboratorioRepetido(idLaboratorio);
                    var existeRegistroEnReincidencia = apercibidoBLL.laboratorioEnReincidenciaRepetido(idLaboratorio);
                    if (existeRegistro == true || existeRegistroEnReincidencia == true)
                    {
                        var texto = TextoAlertaSimple;
                        texto.InnerHtml = "El laboratorio ya cuenta con registro de apercibimiento.";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertaSimple", "$('#alertaSimple').modal();", true);
                        /*     $('#alertaSimple').modal({ backdrop: 'static', keyboard: false })*/
                        upModal.Update();
                        chkb_item.Checked = false;

                    }
                    else
                    {


                        Laboratorio_BL labBLL = new Laboratorio_BL();
                        var laboratorio = labBLL.Obten_Laboratorio(idLaboratorio);
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

                        foreach (GridViewRow item2 in grdHistorico.Rows)
                        {
                            chkb_item = (CheckBox)item2.FindControl("chkSeleccion");
                            chkb_item.Checked = true;
                        }
                        MensajeErrorIdLab.InnerText = string.Empty;
                    }
                }



            }
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

        #region Codigo comentado
        //protected void GuardarApercibimiento_Click(object sender, EventArgs e)
        //{
        //    string RFC = "ALQ0409231H7";
        //    string noExpediente = "2222";
        //    string nombreLab = "Agua Limpia de Quintana Roo  S.A. de C.V.";
        //    string gerenteCalidad = "Q. María Margarita Dafne Lobato Calleros";
        //    string subgerente = "Lic. Inés De Jesús León";
        //    string memorando = "B00.9.00.00.02";
        //    string conCopiaPara = "RGG";



        //    PlantillaBLL operaciones;
        //    PlantillaDTO documento;
        //    var informacionGeneral = new InformacionGeneral();


        //    string nombreDocAcuse = "APERCIBIMIENTO_" + RFC + ".docx";
        //    //ACUSE
        //    string pathDocumentoAcuse = Server.MapPath("~/Reportes/Plantillas/tmp");
        //    var plantillaAcuse = string.Empty;
        //    string pathPlantillaAcuse = string.Empty;

        //    plantillaAcuse = "Plantilla_APERCIBIMIENTO.docx";

        //    if (!Directory.Exists(pathDocumentoAcuse))
        //        Directory.CreateDirectory(pathDocumentoAcuse);

        //    pathPlantillaAcuse = Path.Combine(Server.MapPath("~/Reportes/Plantillas/"), plantillaAcuse);
        //    pathDocumentoAcuse = Path.Combine(pathDocumentoAcuse, nombreDocAcuse);

        //    System.IO.File.Copy(pathPlantillaAcuse, pathDocumentoAcuse, true);
        //    //aqui poner la consulta para recuperar al laboratorio
        //    informacionGeneral.noExpediente = noExpediente;
        //    informacionGeneral.nombreLaboratorio = nombreLab;
        //    informacionGeneral.noResolucion = txtNoOficioResolucion.Text;
        //    informacionGeneral.fechaEmision = DateTime.Now.ToShortDateString();
        //    informacionGeneral.fechaNotificacion = txtFechaResolucion.Text;
        //    informacionGeneral.GerenteCalidad = gerenteCalidad;
        //    informacionGeneral.subgerente = subgerente;
        //    informacionGeneral.Memorando = memorando;
        //    informacionGeneral.conCopiaPara = conCopiaPara;

        //    actualizaInformacionDoc(informacionGeneral, pathDocumentoAcuse, out operaciones, out documento);

        //    //este método va a remplazar todos los tag
        //    bool reemplazoExitoAcuse = operaciones.ReemplazarEtitquetas(documento);
        //    if (reemplazoExitoAcuse)
        //    {
        //        //se recupera el documento actualizado
        //        byte[] documentoByte = operaciones.ArchivoABytes(pathDocumentoAcuse);

        //        //convirte PDF
        //        string pathDocumentoPDF = string.Empty;
        //        pathDocumentoPDF = Path.Combine(Server.MapPath("~/Reportes/Plantillas/tmp/"), Path.GetFileNameWithoutExtension(pathDocumentoAcuse) + ".pdf");

        //        Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
        //        Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(pathDocumentoAcuse);
        //        doc.SaveAs2(pathDocumentoPDF, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
        //        doc.Close();
        //        app.Quit();

        //        nombreDocAcuse = System.IO.Path.GetFileNameWithoutExtension(pathDocumentoAcuse) + ".pdf";
        //        pathDocumentoAcuse = pathDocumentoPDF;


        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment; filename="+ nombreDocAcuse);
        //        HttpContext.Current.Response.BinaryWrite(System.IO.File.ReadAllBytes(pathDocumentoAcuse));
        //        Response.Flush();
        //        Response.End();

        //        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Guardar", "GuardarApercibimiento()", true);


        //    }


        //}
        #endregion

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
            var fechaCaptura =  arregloFecha[1] + " " + arregloFecha[2] + " " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(arregloFecha[3]) + " " + arregloFecha[4] + " " + arregloFecha[5];
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

        protected void lnkDEtalle_Click1(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            GridView gv = clickedRow.NamingContainer as GridView;
            var lbCode = gv.DataKeys[clickedRow.RowIndex].Values[0].ToString();

            var lista = ApercibidosReincientesBLL.ObtenerNumerosYVigenciasDeAprobacion(int.Parse(lbCode));

            if (lista.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalDetalleAASinInfo", "MostrarModalDetalleAASinInfo()", true);
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
                gvdetalleA.DataSource = dt;
                gvdetalleA.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalDetalle", "MostrarModalDetalle()", true);
            }
        }
    }
}