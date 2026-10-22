using SIRALAB.Controles;
using SIRALAB.FIEL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.cancelacion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class SolicitudCancelacionAnalisis : System.Web.UI.Page
    {
        private List<SolicitudCancelacionAnalisisItem> AnalisisSeleccionados
        {
            get
            {
                return (List<SolicitudCancelacionAnalisisItem>)ViewState["AnalisisSeleccionados"];
            }
            set
            {
                ViewState["AnalisisSeleccionados"] = value;
            }
        }
        private List<SolicitudCancelacionAnalisisItem> AnalisisConsultados
        {
            get
            {
                return (List<SolicitudCancelacionAnalisisItem>)ViewState["AnalisisConsultados"];
            }
            set
            {
                ViewState["AnalisisConsultados"] = value;
            }
        }
        private int SolicitudCancelacionID
        {
            get
            {
                return ViewState["SolicitudCancelacionID"] == null ? 0 : (int)ViewState["SolicitudCancelacionID"];
            }
            set
            {
                ViewState["SolicitudCancelacionID"] = value;
            }
        }
        private string FirmaSolicitud
        {
            get
            {
                return ViewState["FirmaSolicitud"].ToString();
            }
            set
            {
                ViewState["FirmaSolicitud"] = value;
            }
        }
        public static int? LaboratorioID
        {
            get
            {
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];
                return usuario != null ? usuario.LaboratorioID : null;
            }
        }
        public SIRALAB_DAL.Usuario Usuario
        {
            get
            {
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];
                return usuario != null ? usuario : null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Main)this.Master.Master;
            master.MensajeDevolucion += Master_MensajeDevolucion1;

            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Master_MensajeDevolucion1(object sender, controles.MensajeArgs e)
        {
            var comando = e.Command;
            if (comando != null)
            {
                switch (comando)
                {
                    case "ErrorGuardar":
                        break;
                    case "Firmar":
                        if (e.Opcion == controles.Opcion.Aceptar)
                            FirmarSolicitud(int.Parse(e.Args[0]));
                        else
                            Response.Redirect("~/SolicitudCancelacion.aspx");
                        break;
                    case "Firmado":
                        this.FirmaSolicitud = e.Args[0];
                        if (e.Opcion == controles.Opcion.Aceptar)
                            CambiarEstatus(enumEstatusCancelacion.Enviada);
                        else
                            Response.Redirect("~/SolicitudCancelacion.aspx");
                        break;
                    case "GuardadoFirmado":
                        upSolicitudes.Update();
                        Response.Redirect("~/SolicitudCancelacion.aspx");
                        break;
                    case "ErrorFirma":
                        break;
                }
            }
        }

        //private void Master_MensajeDevolucion(object sender, controles.MensajeArgs e)
        //{
        //    var comando = e.Command;
        //    if (comando != null)
        //    {
        //        switch (comando)
        //        {
        //            case "ErrorGuardar":
        //                break;
        //            case "Firmar":
        //                if (e.Opcion == controles.Opcion.Aceptar)
        //                    FirmarSolicitud(int.Parse(e.Args[0]));
        //                else
        //                    Response.Redirect("~/SolicitudCancelacion.aspx");
        //                break;
        //            case "Firmado":
        //                this.FirmaSolicitud = e.Args[0];
        //                if (e.Opcion == controles.Opcion.Aceptar)
        //                    CambiarEstatus(enumEstatusCancelacion.Enviada);
        //                else
        //                    Response.Redirect("~/SolicitudCancelacion.aspx");
        //                    break;
        //            case "GuardadoFirmado":
        //                Response.Redirect("~/SolicitudCancelacion.aspx");
        //                break;
        //            case "ErrorFirma":
        //                break;
        //        }
        //    }
        //}
        private void Initialize()
        {
            /* En este método vamos a realizar la consulta inicial */
            wucFiltroSolicitudCancelacionAnalisis.Initialize(new FiltroSolicitudCancelacionAnalisis { LaboratorioID = LaboratorioID });

            var filter = wucFiltroSolicitudCancelacionAnalisis.ObtenerFiltro();

            filter.CurrentPage = 1;
            filter.PageSize = wucPaginador.PageSize;
            filter.FechaInicialEmision = new DateTime(DateTime.Now.Year, 1, 1);
            filter.FechaFinalEmision = DateTime.Now;

            Filtrar(filter);
        }
        private void Filtrar(FiltroSolicitudCancelacionAnalisis filtro)
        {
            /* Ahora sí hay que realizar la búsqueda de los registros de forma paginada */
            SolicitudCancelacionAnalisis_BL solicitud = new SolicitudCancelacionAnalisis_BL();
            var resultado = solicitud.BuscaSolicitudesAnalisis(filtro).Select(p => new SolicitudCancelacionAnalisisItem
            {
                AnalisisID = (int)p.AnalisisID,
                Laboratorio = p.Laboratorio,
                RfcLaboratorio = p.RFCLaboratorio,
                NumeroInforme = p.NumeroInforme,
                Rfc = p.RFC,
                Cliente = p.Cliente,
                FechaEmision = p.FechaEmision,
                Titulo = p.Titulo,
                Anexo = p.Anexo,
                Norma = p.Norma
            });
            var total = solicitud.CuentaSolicitudesAnalisis(filtro);

            this.AnalisisConsultados = resultado.ToList();
            rptSolicitudes.DataSource = resultado;
            rptSolicitudes.DataBind();

            upSolicitudes.Update();
            wucPaginador.Initialize(filtro.CurrentPage, total);
        }
        private void SelectionListManage()
        {
            var listadoSeleccionado = AnalisisSeleccionados ?? new List<SolicitudCancelacionAnalisisItem>();
            var listadoConsultado = AnalisisConsultados ?? new List<SolicitudCancelacionAnalisisItem>();
            foreach (RepeaterItem item in rptSolicitudes.Items)
            {
                if (item.FindControl("chkSeleccion") != null)
                {
                    var chk = (CheckBox)item.FindControl("chkSeleccion");
                    var AnalisisID = int.Parse(chk.Attributes["data-analisis-id"]);
                    if (chk.Checked)
                    {
                        if (!listadoSeleccionado.Any(p => p.AnalisisID == AnalisisID))
                        {
                            listadoSeleccionado.Add(listadoConsultado.FirstOrDefault(q=> q.AnalisisID == AnalisisID));
                        }
                    }
                    else
                    {
                        if (listadoSeleccionado.Any(p => p.AnalisisID == AnalisisID))
                        {
                            var removeItem = listadoConsultado.FirstOrDefault(q => q.AnalisisID == AnalisisID);
                            listadoSeleccionado.Remove(removeItem);
                        }
                    }
                }
            }
            AnalisisSeleccionados = listadoSeleccionado;
        }
        private void FirmarSolicitud(int solicitudCancelacionID)
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FirmaElectronica();

            /* SolicitudID este sería el que se va a firmar como la llave */
            this.SolicitudCancelacionID = solicitudCancelacionID;
            string Llave = solicitudCancelacionID.ToString();

            /// TODO: Extraemos la información de la solicitud

            FIEL.CadenaOriginal cadenaOriginal = new CadenaOriginal();
            cadenaOriginal.Datos.Add("Laboratorio:");
            cadenaOriginal.Datos.Add("RFC:");
            cadenaOriginal.Datos.Add("Fecha de Registro:");
            cadenaOriginal.Datos.Add("Fecha de Firma:" + DateTime.Now.ToString("yyyy-MM-dd"));
            cadenaOriginal.Datos.Add("Hora de Firma:" + DateTime.Now.ToString("hh:mm:ss"));

            resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);

            wucFirma.enviarAFirma(resultado.DatosAFirmar);
            wucFirma.Visible = true;
        }
        private void CambiarEstatus(enumEstatusCancelacion estatus)
        {
            SelectionListManage();
            var listadoSeleccionado = new List<SolicitudCancelacionArg>
            {
                new SolicitudCancelacionArg {
                    SolicitudCancelacionID = this.SolicitudCancelacionID,
                    EstatusID = estatus.ParseToInt(),
                    FirmaSolicitud = this.FirmaSolicitud
                }
            };
            SolicitudCancelacion_BL scBL = new SolicitudCancelacion_BL();
            if (scBL.GuardarAprobacion(listadoSeleccionado, estatus))
            {
                foreach (var solicitud in listadoSeleccionado)
                {
                    scBL.FirmaSolicitud(solicitud);
                    EnviarNotificacion(solicitud);
                }
                this.PopMensaje("Se ha enviado la solicitud para la aprobación.", "Solicitud de Cancelación", controles.Opciones.SoloAceptar, controles.Icono.ok, "GuardadoFirmado");
            }
            else
            {
                this.PopMensaje("Ocurrió un error inesperado favor de contactar al administrador", "Solicitud de Cancelación", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            Response.Redirect("~/SolicitudCancelacion.aspx");

        }
        protected void wucFiltroSolicitudCancelacionAnalisis_Buscar(object sender, FiltroSolicitudCancelacionAnalisis e)
        {
            e.PageSize = wucPaginador.PageSize;
            e.CurrentPage = 1;

            SelectionListManage();
            Filtrar(e);
        }
        protected void wucPaginador_PageChange(object sender, PageArgs e)
        {
            var filter = wucFiltroSolicitudCancelacionAnalisis.ObtenerFiltro();

            filter.CurrentPage = e.CurrentPage;
            filter.PageSize = wucPaginador.PageSize;

            SelectionListManage();
            Filtrar(filter);
        }
        protected void rptSolicitudes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var listadoSeleccionado = AnalisisSeleccionados ?? new List<SolicitudCancelacionAnalisisItem>();
            var chk = (CheckBox)e.Item.FindControl("chkSeleccion");

            if (chk != null) {
                var AnalisisID = int.Parse(chk.Attributes["data-analisis-id"]);
                chk.Checked = listadoSeleccionado.Any(p => p.AnalisisID == AnalisisID);
            }
        }
        protected void lnkGuardarSolicitud_Click(object sender, EventArgs e)
        {
            SelectionListManage();
            var listadoSeleccionado = AnalisisSeleccionados ?? new List<SolicitudCancelacionAnalisisItem>();
            if (!listadoSeleccionado.Any())
            {
                this.PopMensaje("Es necesario que seleccione al menos un análisis.", "Solicitud de cancelación", controles.Opciones.SoloAceptar, controles.Icono.error);
                return;
            }
            wucSolicitarCancelacion.ActualizarMostrar(listadoSeleccionado);
        }
        protected void lnkLimpiarSeleccion_Click(object sender, EventArgs e)
        {
            AnalisisSeleccionados = new List<SolicitudCancelacionAnalisisItem>();

            var filter = wucFiltroSolicitudCancelacionAnalisis.ObtenerFiltro();
            filter.CurrentPage = wucPaginador.CurrentPage;
            filter.PageSize = wucPaginador.PageSize;

            Filtrar(filter);
        }
        protected void wucSolicitarCancelacion_Guardar(object sender, SolicitudCancelacionArg e)
        {
            try
            {
                SolicitudCancelacion_BL scBL = new SolicitudCancelacion_BL();
                
                e.FechaRegistro = DateTime.Now;
                e.LaboratorioID = LaboratorioID ?? 0;
                e.EstatusID = enumEstatusCancelacion.Capturada.ParseToInt();
                e.Detalle = AnalisisSeleccionados.Select(p => (SolicitudCancelacionDetalleArgBase_BL)(new SolicitudCancelacionDetalleArg { AnalisisID = p.AnalisisID })).ToList();

                var solicitudCancelacionID = scBL.Guarda(e).SolicitudCancelacionID;
                //Sí todo OK entonces pedimos sí desea firmar la solicitud
                this.PopMensaje($"Se ha guardado la solicitud con el folio : {solicitudCancelacionID}<br /><br />¿Desea proceder a firmar la solicitud?", "Solicitud de Cancelación",
                    controles.Opciones.SiNo, controles.Icono.information, "Firmar", new string[] { solicitudCancelacionID.ToString() });
            }
            catch (Exception ex)
            {
                this.PopMensaje($"Ocurrió un error al intentar guardar la solicitud.<br /><br />Favor de intentarlo más tarde.<br /><br />{ex.Message}", "Solicitud de Cancelación",
                    controles.Opciones.SiNo, controles.Icono.information);
            }
        }
        protected void wucFirma_FirmarColeccion(object sender, DatosFirma_Args e)
        {
            FirmaElectronica datosFirma;
            datosFirma = e.DatosAFirmar;
            string SelloCNA = string.Empty;
            List<string> listaErrorFirma = new List<string>();
            List<string> ListaErrorCertificado = new List<string>();

            Contenedor contenedor = datosFirma.listaContenedores[0];
            contenedor.ArchivoCer = e.ArchivoCer;
            contenedor.ArchivoKey = e.ArchivoKey;
            contenedor.txtRFC = e.RFCFirma;
            contenedor.txtContra = e.txtContra;

            string resultPKCS7 = string.Empty;
            string resultFirmaElectronica = string.Empty;
            string resutlCadenaOriginal = string.Empty;
            string resultCertificado = string.Empty;
            if (contenedor.valido)
            {
                SelloDigital selloDigital = new SelloDigital();
                resultPKCS7 = contenedor.cadenaCMS;
                resultFirmaElectronica = contenedor.firmaDigital;
                resutlCadenaOriginal = contenedor.cadenaAFirmar;
                resultCertificado = contenedor.cetificadoDigital;

                try
                {
                    selloDigital.cadenaOriginal = contenedor.cadenaAFirmar;
                    selloDigital.SellarCadenaOriginal();
                    SelloCNA = selloDigital.sello;
                }
                catch (Exception Ex)
                {
                    SelloCNA = "Hubo algún problema al solicitar el sello de CONAGUA.";
                    string errorStr = "Hubo algún problema al solicitar el sello de CONAGUA.<br/>";
                    errorStr += "Error al sellar la(s) solicitud(es).<br />";
                    errorStr += "Descripcion: ";
                    errorStr += Ex.Message;
                    listaErrorFirma.Add(errorStr);
                }
            }
            else //añadir lista de errores
            {
                string errorStr = "<br/>";
                errorStr += "Descripción: ";
                errorStr += contenedor.mensajeError;
                ListaErrorCertificado.Add(errorStr);
            }

            // TODO: descomentar estas líneas en cuanto el servicio de metatrust
            /*Sí existen problemas con el certificado entonces se le notifica por mensaje.*/
            /*Sí la firma no es correcta entonces no guardamos nada y mandamos un mensaje de error de la firma.*/
            if (ListaErrorCertificado.Count > 0 || listaErrorFirma.Count > 0)
            {
                var mensajeError = string.Join("<br />", listaErrorFirma) + string.Join("<br />", ListaErrorCertificado);
                this.PopMensaje(mensajeError, "Solicitud de cancelación", controles.Opciones.SoloAceptar, controles.Icono.error);
                return;
            }
            string mensaje = "¿Se encuentra seguro de enviar la solicitud generada?.";
            string command = "Firmado";
            this.PopMensaje(mensaje, "Solicitud de cancelación", controles.Opciones.SiNo, controles.Icono.question, command, new string[] { resultFirmaElectronica });
        }
        private void EnviarNotificacion(SolicitudCancelacionArg solicitud)
        {
            ApplicationSettings ap = new ApplicationSettings();
            Hashtable ht = new Hashtable();

            ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
            ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));
            List<string> toList = new List<string>();
            string[] ToArr = new string[] { };
            try
            {
                System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-ES");
                var fechaOT = DateTime.Now;
                var fecha = c.DateTimeFormat.GetDayName(fechaOT.DayOfWeek) + " " + fechaOT.Day + " " + c.DateTimeFormat.GetMonthName(fechaOT.Month) + " de " + fechaOT.Year;
                string Message = $"Se le informa que la Comisión Nacional de Agua ha recibido con fecha {fecha} a las {DateTime.Now:HH:mm} horas, la(s) solicitud(es) de cancelación. <br /><br />";

                SolicitudCancelacion_BL solicitudCancelacion_BL = new SolicitudCancelacion_BL();
                SolicitudCancelacionDetalle_BL solicitudCancelacionDetalle_BL = new SolicitudCancelacionDetalle_BL();
                var item = solicitudCancelacion_BL.Obtener((int)solicitud.SolicitudCancelacionID);
                var solicitudCancelacionItem = solicitudCancelacion_BL.ObtenSolicitudCancelacion((int)item.SolicitudCancelacionID);
                var analisis = solicitudCancelacionDetalle_BL.BuscaSolicitudes(item.SolicitudCancelacionID);

                toList.Add(this.Usuario.Correo);
                var infoTabla = $"<tr><th colspan='3' style=\"border: 1px solid #e2e2e2;\">Folio: {solicitudCancelacionItem.Folio}</th><th colspan=\"3\" style=\"border: 1px solid #e2e2e2;\">Motivo: {solicitudCancelacionItem.Motivo}</th></tr>";
                var titulosTabla = $"<tr><th style=\"border: 1px solid #e2e2e2;\">Orden de trabajo</th><th style=\"border: 1px solid #e2e2e2;\">Cliente</th><th style=\"border: 1px solid #e2e2e2;\">Fecha</th><th style=\"border: 1px solid #e2e2e2;\">Título</th><th style=\"border: 1px solid #e2e2e2;\">Anexo</th><th style=\"border: 1px solid #e2e2e2;\">Norma</th></tr>";
                var tabla = "";
                analisis.ToList().ForEach(r =>
                {
                    tabla += $"<tr><td style=\"border: 1px solid #e2e2e2;\">{r.NumeroInforme}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Cliente}</td><td style=\"border: 1px solid #e2e2e2;\">{r.FechaEmision:dd/MM/yyyy}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Titulo}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Anexo}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Norma}</td></tr>";
                });

                Message += $"<table style=\"border-collapse: collapse; border: 1px solid #e2e2e2; width: 50%; min-width: 600px; margin: 0 auto;\">{infoTabla}{titulosTabla}{tabla}</table><br /><br />";

                ToArr = toList.ToArray();

                ApplicationSettings appSettings = new ApplicationSettings();
                if(appSettings.useTestMail)
                    ToArr = appSettings.testMail;

                foreach (var to in ToArr)
                {
                    Thread threadSendMails = new Thread(delegate ()
                    {
                        SIRALAB_BL.correo.Mailer.SendMail(to, "", "", Message, "Solicitud de cancelación de análisis de laboratorio", ht);
                    });

                    threadSendMails.IsBackground = true;
                    threadSendMails.Start();
                }
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}