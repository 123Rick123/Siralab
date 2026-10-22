using SIRALAB.Controles;
using SIRALAB.FIEL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.cancelacion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.controles;
using SIRALAB_BL.correo;
using SIRALAB_DAL;

namespace SIRALAB
{
    public partial class SolicitudCancelacion : System.Web.UI.Page
    {
        private List<SolicitudCancelacionItem> SolicitudesSeleccionadas
        {
            get
            {
                return (List<SolicitudCancelacionItem>)ViewState["SolicitudesSeleccionadas"];
            }
            set
            {
                ViewState["SolicitudesSeleccionadas"] = value;
            }
        }
        private List<SolicitudCancelacionItem> SolicitudesConsultadas
        {
            get
            {
                return (List<SolicitudCancelacionItem>)ViewState["SolicitudesConsultadas"];
            }
            set
            {
                ViewState["SolicitudesConsultadas"] = value;
            }
        }
        private int SolicitudCancelacionID { set { ViewState["SolicitudCancelacionID"] = value; } get { return int.Parse(ViewState["SolicitudCancelacionID"].ToString()); } }
        private string FirmaAprobacion
        {
            get
            {
                return ViewState["FirmaAprobacion"].ToString();
            }
            set
            {
                ViewState["FirmaAprobacion"] = value;
            }
        }
        private int Accion { set { ViewState["enumEstatusCancelacion"] = value; } get { return int.Parse(ViewState["enumEstatusCancelacion"].ToString()); } }
        public SIRALAB_DAL.Usuario Usuario
        {
            get
            {
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];
                return usuario != null ? usuario : null;
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
        public static enumUsuarioRol UsuarioRol
        {
            get {
                var result = enumUsuarioRol.Recaudacion;
                var usuario = (SIRALAB_DAL.Usuario)HttpContext.Current.Session["UsuarioLog"];

                return usuario != null ? usuario.UsuarioRolID.ParseToEnumUsuarioRol() : result;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var mainForm = (Main)this.Master.Master;
            mainForm.MensajeDevolucion += MainForm_MensajeDevolucion;

            if (!IsPostBack)
            {
                Initialize();
            }
        }
        private void MainForm_MensajeDevolucion(object sender, MensajeArgs e)
        {
            var command = e.Command;
            if(command != null)
            {
                int solicitudCancelacionID = 0;
                if (e.Opcion == controles.Opcion.Aceptar)
                {
                    switch (command)
                    {
                        case "Eliminar":
                            solicitudCancelacionID = int.Parse(e.Args[0]);
                            Eliminar(solicitudCancelacionID);
                            break;
                        case "Enviar":
                            this.FirmaAprobacion = e.Args[0];
                            Enviar();
                            break;
                        case "Rechazar":
                            this.FirmaAprobacion = e.Args[0];
                            Rechazar();
                            break;
                        case "Aprobar":
                            this.FirmaAprobacion = e.Args[0];
                            Aprobar();
                            break;
                    }
                }
            }
        }
        private void Initialize()
        {
            lnkNuevaSolicitud.Visible = UsuarioRol == enumUsuarioRol.Laboratorio;
            lnkAprobar.Visible = lnkRechazar.Visible = UsuarioRol == enumUsuarioRol.Recaudacion;

            /* En este método vamos a realizar la consulta inicial */
            wucFiltroSolicitudCancelacion.Initialize();

            var filter = wucFiltroSolicitudCancelacion.ObtenerFiltro();

            filter.CurrentPage = 1;
            filter.PageSize = wucPaginador.PageSize;

            Filtrar(filter);
        }
        private void SelectionListManage()
        {
            var listadoSeleccionado = SolicitudesSeleccionadas ?? new List<SolicitudCancelacionItem>();
            var listadoConsultado = SolicitudesConsultadas ?? new List<SolicitudCancelacionItem>();
            foreach (RepeaterItem item in rptSolicitudes.Items)
            {
                if (item.FindControl("chkSeleccion") != null)
                {
                    var chk = (CheckBox)item.FindControl("chkSeleccion");
                    var SolicitudCancelacionID = int.Parse(chk.Attributes["data-solicitud-id"]);
                    if (chk.Checked)
                    {
                        if (!listadoSeleccionado.Any(p => p.SolicitudCancelacionID == SolicitudCancelacionID))
                        {
                            listadoSeleccionado.Add(listadoConsultado.FirstOrDefault(q => q.SolicitudCancelacionID == SolicitudCancelacionID));
                        }
                    }
                    else
                    {
                        if (listadoSeleccionado.Any(p => p.SolicitudCancelacionID == SolicitudCancelacionID))
                        {
                            var removeItem = listadoSeleccionado.FirstOrDefault(q => q.SolicitudCancelacionID == SolicitudCancelacionID);
                            listadoSeleccionado.Remove(removeItem);
                        }
                    }
                }
            }
            SolicitudesSeleccionadas = listadoSeleccionado;
        }
        private void Filtrar(FiltroSolicitudCancelacion filtro)
        {
            /* Ahora sí hay que realizar la búsqueda de los registros de forma paginada */
            SolicitudCancelacion_BL solicitud = new SolicitudCancelacion_BL();
            var resultado = solicitud.BuscaSolicitudes(filtro).Select(p=> new SolicitudCancelacionItem {
                SolicitudCancelacionID = p.SolicitudCancelacionID,
                Folio = p.Folio,
                Laboratorio = p.Laboratorio,
                Motivo = p.Motivo,
                FechaRegistro = (DateTime)p.FechaRegistro,
                EstatusID = p.EstatusID,
                Estatus = p.Estatus
            });
            var total = solicitud.CuentaSolicitudes(filtro);

            this.SolicitudesConsultadas = resultado.ToList();
            rptSolicitudes.DataSource = resultado;
            rptSolicitudes.DataBind();

            upSolicitudes.Update();
            wucPaginador.Initialize(filtro.CurrentPage, total);
        }
        private void Eliminar(int solicitudCancelacionID)
        {
            SolicitudCancelacion_BL solicitudBL = new SolicitudCancelacion_BL();
            if (solicitudBL.Eliminar(solicitudCancelacionID))
            {
                var listadoSeleccionado = SolicitudesSeleccionadas ?? new List<SolicitudCancelacionItem>();
                if (listadoSeleccionado.Any(p => p.SolicitudCancelacionID == solicitudCancelacionID))
                    listadoSeleccionado.Remove(listadoSeleccionado.Single(p => p.SolicitudCancelacionID == solicitudCancelacionID));

                var filter = wucFiltroSolicitudCancelacion.ObtenerFiltro();
                filter.CurrentPage = 1;
                filter.PageSize = wucPaginador.PageSize;
                Filtrar(filter);
            }
        }
        private void Enviar()
        {
            SolicitudesSeleccionadas = new List<SolicitudCancelacionItem>
            {
                new SolicitudCancelacionItem
                {
                    SolicitudCancelacionID = this.SolicitudCancelacionID
                }
            };
            CambiarEstatus(enumEstatusCancelacion.Enviada);
        }
        private void Rechazar()
        {
            CambiarEstatus(enumEstatusCancelacion.Rechazada);
        }
        private void Aprobar()
        {
            CambiarEstatus(enumEstatusCancelacion.Aprobada);
        }
        private void CambiarEstatus(enumEstatusCancelacion estatus)
        {
            List<SolicitudCancelacionArg> listadoSeleccionado = new List<SolicitudCancelacionArg>();
            if (estatus != enumEstatusCancelacion.Enviada)
                SelectionListManage();
            
            listadoSeleccionado = SolicitudesSeleccionadas?.Select(p => new SolicitudCancelacionArg
            {
                SolicitudCancelacionID = p.SolicitudCancelacionID,
                EstatusID = estatus.ParseToInt(),
                FirmaAprobacion = this.FirmaAprobacion,
                FechaAprobacion = DateTime.Now
            }).ToList() ?? new List<SolicitudCancelacionArg>();
            SolicitudCancelacion_BL scBL = new SolicitudCancelacion_BL();

            if (scBL.GuardarAprobacion(listadoSeleccionado, estatus))
            {
                switch (estatus)
                {
                    case enumEstatusCancelacion.Aprobada:
                    case enumEstatusCancelacion.Enviada:
                    case enumEstatusCancelacion.Rechazada:
                        EnviarNotificacion(estatus);
                        break;
                }
                SolicitudesSeleccionadas = new List<SolicitudCancelacionItem>();
                foreach (var solicitud in listadoSeleccionado)
                {
                    scBL.FirmaAprobacion(solicitud);
                }
                var filter = wucFiltroSolicitudCancelacion.ObtenerFiltro();
                filter.CurrentPage = wucPaginador.CurrentPage;
                filter.PageSize = wucPaginador.PageSize;
                Filtrar(filter);
                this.SolicitudCancelacionID = 0;
            }
            else
            {
                this.PopMensaje("Ocurrio un error inesperado favor de contactar al administrador", "Solicitud de Cancelación", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
        private void FirmarSolicitud(string llave)
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FirmaElectronica();
            FIEL.CadenaOriginal cadenaOriginal = new CadenaOriginal();

            string Llave = llave;
            cadenaOriginal.Datos.Add("Fecha de Firma:" + DateTime.Now.ToString("yyyy-MM-dd"));
            cadenaOriginal.Datos.Add("Hora de Firma:" + DateTime.Now.ToString("hh:mm:ss"));

            resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);

            wucAppletFIEL.enviarAFirma(resultado.DatosAFirmar);
            wucAppletFIEL.Visible = true;
        }
        protected void wucFiltroSolicitudCancelacion_Buscar(object sender, FiltroSolicitudCancelacion e)
        {
            SelectionListManage();
            e.PageSize = wucPaginador.PageSize;
            e.CurrentPage = 1;
            Filtrar(e);
        }
        protected void wucPaginador_PageChange(object sender, PageArgs e)
        {
            SelectionListManage();
            var filter = wucFiltroSolicitudCancelacion.ObtenerFiltro();
            filter.CurrentPage = e.CurrentPage;
            filter.PageSize = wucPaginador.PageSize;

            Filtrar(filter);
        }
        protected void lnkNuevaSolicitud_Click(object sender, EventArgs e)
        {
            /* Aquí llamamos a la pantalla de captura de una nueva solicitud de cancelación */
            Response.Redirect("~/SolicitudCancelacionAnalisis.aspx");
        }
        protected void lnkVerDetalle_Click(object sender, EventArgs e)
        {
            SolicitudCancelacion_BL solicitud = new SolicitudCancelacion_BL();
            SolicitudCancelacionDetalle_BL detalle = new SolicitudCancelacionDetalle_BL();
            var solicitudCancelacionID = int.Parse(((LinkButton)sender).Attributes["data-solicitud-id"]);

            var filtro = new FiltroSolicitudCancelacion
                { Folio = solicitudCancelacionID, IncluirCapturados = true, PageSize = 10, CurrentPage = 1 };
            var solicitudItem = solicitud.BuscaSolicitudes(filtro).Select(p => new SolicitudCancelacionItem
            {
                SolicitudCancelacionID = p.SolicitudCancelacionID,
                Folio = p.Folio,
                Laboratorio = p.Laboratorio,
                Motivo = p.Motivo,
                FechaRegistro = (DateTime)p.FechaRegistro,
                EstatusID = p.EstatusID,
                Estatus = p.Estatus
            }).FirstOrDefault();

            var detallesItems = detalle.BuscaSolicitudes((int?)solicitudCancelacionID).Select(p =>
                new SolicitudCancelacionDetalleItem
                {
                    SolicitudCancelacionID = p.SolicitudCancelacionID,
                    SolicitudCancelacionDetalleID = p.SolicitudCancelacionDetalleID,
                    AnalisisID = p.AnalisisID,
                    AnalisisPadreID = p.AnalisisPadreID,
                    LaboratorioID = p.LaboratorioID,
                    Laboratorio = p.Laboratorio,
                    RFCLaboratorio = p.RFCLaboratorio,
                    NumeroInforme = p.NumeroInforme,
                    RFC = p.RFC,
                    Cliente = p.Cliente,
                    FechaEmision = p.FechaEmision,
                    Titulo = p.Titulo,
                    Anexo = p.Anexo,
                    Norma = p.Norma,
                    Estatus = p.Estatus
                }).ToList();
            wucSolicitarCancelacionDetalle.ActualizarMostrar(solicitudItem, detallesItems);
        }
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            var solicitudCancelacionID = ((LinkButton)sender).Attributes["data-solicitud-id"];

            this.PopMensaje($"¿Se encuentra seguro de eliminar la solicitud con folio {solicitudCancelacionID}?", "Solicitud de Cancelación", controles.Opciones.SiNo, controles.Icono.question, "Eliminar", new string[] { solicitudCancelacionID });
        }
        protected void lnkAprobar_Click(object sender, EventArgs e)
        {
            Accion = enumEstatusCancelacion.Aprobada.ParseToInt();
            FirmarSolicitud("Llave");
        }
        protected void lnkRechazar_Click(object sender, EventArgs e)
        {
            Accion = enumEstatusCancelacion.Rechazada.ParseToInt();
            FirmarSolicitud("Llave");
        }
        protected void lnkFirmar_Click(object sender, EventArgs e)
        {
            Accion = enumEstatusCancelacion.Enviada.ParseToInt();
            var solicitudCancelacionID = ((LinkButton)sender).Attributes["data-solicitud-id"];
            this.SolicitudCancelacionID = int.Parse(solicitudCancelacionID);
            FirmarSolicitud(solicitudCancelacionID);
        }
        protected void wucSolicitarCancelacionDetalle_OnDuplicar(object sender, DuplicarAnalisisItem e)
        {
            var ID = e.AnalisisID;
            Analisis_BL an_bl = new Analisis_BL();
            Muestra_BL muestraBl = new Muestra_BL();
            Muestreador_BL muestreadorBl = new Muestreador_BL();

            var an = an_bl.Obten_Analisis(ID);
            
            AnalisisCapturaArgs Args = new AnalisisCapturaArgs();
            Args.AnalisisPadreID = ID;
            Args.OrdenTrabajoID = an.OrdenTrabajoID;
            Args.AnexoID = an.AnexoID;
            Args.LaboratorioID = an.LaboratorioID;
            Args.EstatusID = 3;
            Args.Firmada = false;
            Args.FechaFirmada = null;
            Args.Enviada = false;
            Args.Procesado = false;
            Args.FechaProceso = null;
            Args.FechaRegistro = an.FechaRegistro;
            Args.ConsecionID = an.ConsecionID;

            var analisis = an_bl.DuplicarAnalisi(Args);
            var itemDuplicado = e.Listado.First(p => p.AnalisisID == e.AnalisisID);
            itemDuplicado.AnalisisPadreID = analisis.AnalisisID;
            this.PopMensaje($"Se ha creado el análisis en la orden de trabajo {itemDuplicado.NumeroInforme}", "Solicitud de cancelación", Opciones.SoloAceptar, Icono.information);
            wucSolicitarCancelacionDetalle.Hide();
        }
        protected void wucAppletFIEL_FirmarColeccion(object sender, DatosFirma_Args e)
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

            string resultPKCS7 = String.Empty;
            string resultFirmaElectronica = String.Empty;
            string resutlCadenaOriginal = String.Empty;
            string resultCertificado = String.Empty;

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

            /*Sí existen problemas con el certificado entonces se le notifica por mensaje.*/
            /*Sí la firma no es correcta entonces no guardamos nada y mandamos un mensaje de error de la firma.*/
            if (ListaErrorCertificado.Count > 0 ||listaErrorFirma.Count > 0)
            {
                var mensajeError = string.Join("<br />", listaErrorFirma) + string.Join("<br />", ListaErrorCertificado);
                    this.PopMensaje(mensajeError, "Solicitud de cancelación", controles.Opciones.SoloAceptar, controles.Icono.error);
                return;
            }/**/

            string mensaje = "";
            string command = "";
            enumEstatusCancelacion accion = enumEstatusCancelacion.Enviada;
            if(Accion.TryParseEnum(out accion))
            {
                switch (accion)
                {
                    case enumEstatusCancelacion.Enviada:
                        mensaje = "¿Se encuentra seguro de enviar la solicitud seleccionada?.";
                        command = "Enviar";
                        break;
                    case enumEstatusCancelacion.Rechazada:
                        mensaje = "¿Se encuentra seguro de Rechazar los análisis contenidos en las solicitudes seleccionadas?.";
                        command = "Rechazar";
                        break;
                    case enumEstatusCancelacion.Aprobada:
                        mensaje = "¿Se encuentra seguro de Aprobar los análisis contenidos en las solicitudes seleccionadas?.";
                        command = "Aprobar";
                        break;
                }
                this.PopMensaje(mensaje, "Solicitud de cancelación", controles.Opciones.SiNo, controles.Icono.question, command, new string[] { resultFirmaElectronica, resultPKCS7 });

            }
            else
            {
                this.PopMensaje("Ocurrió un error favor de reportarlo al administrador.", "Solicitud de cancelación", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
        private void EnviarNotificacion(enumEstatusCancelacion estatus)
        {
            ApplicationSettings ap = new ApplicationSettings();
            Hashtable ht = new Hashtable();

            ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
            ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));
            List<string> toList = new List<string>();
            string[] ToArr = new string[] {};
            try
            {
                System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-ES");
                var fechaOT = DateTime.Now;
                var fecha = c.DateTimeFormat.GetDayName(fechaOT.DayOfWeek) + " " + fechaOT.Day + " " + c.DateTimeFormat.GetMonthName(fechaOT.Month) + " de " + fechaOT.Year;
                string Message = "";

                switch (estatus)
                {
                    case enumEstatusCancelacion.Aprobada:
                        Message = $"Se le informa que la Comisión Nacional de Agua ha aprobado con fecha {fecha} a las {DateTime.Now.ToString("HH:mm")} horas, la(s) solicitud(es) de cancelación. <br /><br />";
                        break;
                    case enumEstatusCancelacion.Enviada:
                        Message = $"Se le informa que la Comisión Nacional de Agua ha recibido con fecha {fecha} a las {DateTime.Now.ToString("HH:mm")} horas, la(s) solicitud(es) de cancelación. <br /><br />";
                        break;
                    case enumEstatusCancelacion.Rechazada:
                        Message = $"Se le informa que la Comisión Nacional de Agua ha rechazado con fecha {fecha} a las {DateTime.Now.ToString("HH:mm")} horas, la(s) solicitud(es) de cancelación. <br /><br />";
                        break;
                }
                
                foreach (var item in SolicitudesSeleccionadas)
                {
                    SolicitudCancelacion_BL solicitudCancelacion_BL = new SolicitudCancelacion_BL();
                    SolicitudCancelacionDetalle_BL solicitudCancelacionDetalle_BL = new SolicitudCancelacionDetalle_BL();
                    var solicitudCancelacionItem = solicitudCancelacion_BL.ObtenSolicitudCancelacion((int)item.SolicitudCancelacionID);
                    var analisis = solicitudCancelacionDetalle_BL.BuscaSolicitudes(item.SolicitudCancelacionID);

                    switch (estatus)
                    {
                        case enumEstatusCancelacion.Aprobada:
                        case enumEstatusCancelacion.Rechazada:
                            var solicitud = solicitudCancelacion_BL.Obtener(item.SolicitudCancelacionID);
                            toList.AddRange(solicitud.Laboratorio.Usuarios.Select(r => r.Correo));

                            break;
                        case enumEstatusCancelacion.Enviada:
                            toList.Add(this.Usuario.Correo);

                            break;
                    }

                    var infoTabla = $"<tr><th colspan='2' style=\"border: 1px solid #e2e2e2;\">Folio: {solicitudCancelacionItem.Folio}</th><th colspan=\"3\" style=\"border: 1px solid #e2e2e2;\">Motivo: {solicitudCancelacionItem.Motivo}</th></tr>";
                    var titulosTabla = $"<tr><th style=\"border: 1px solid #e2e2e2;\">Orden de trabajo</th><th style=\"border: 1px solid #e2e2e2;\">Cliente</th><th style=\"border: 1px solid #e2e2e2;\">Título</th><th style=\"border: 1px solid #e2e2e2;\">Anexo</th><th style=\"border: 1px solid #e2e2e2;\">Norma</th></tr>";
                    var tabla = "";
                    analisis.ToList().ForEach(r =>
                    {
                        tabla += $"<tr><td style=\"border: 1px solid #e2e2e2;\">{r.NumeroInforme}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Cliente}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Titulo}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Anexo}</td><td style=\"border: 1px solid #e2e2e2;\">{r.Norma}</td></tr>";
                    });

                    Message += $"<table style=\"border-collapse: collapse; border: 1px solid #e2e2e2; width: 50%; min-width: 600px; margin: 0 auto;\">{infoTabla}{titulosTabla}{tabla}</table><br /><br />";
                }

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