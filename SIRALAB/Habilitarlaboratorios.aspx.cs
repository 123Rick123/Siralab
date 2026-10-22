using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.controles;
using SIRALAB.Controles;
using SIRALAB.FIEL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.ordentrabajo;
using SIRALAB_DAL;

namespace SIRALAB
{
    public partial class Habilitarlaboratorios : Page
    {

        #region Propiedades

        private static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }

        public FiltroshabilitarAnalisArg Filtro
        {
            set { Session["FiltroHabilitarAnalisis"] = value; }
            get => Session["FiltroHabilitarAnalisis"] == null ? new FiltroshabilitarAnalisArg() : (FiltroshabilitarAnalisArg)Session["FiltroHabilitarAnalisis"];
        }

        public int PaginaBusqueda
        {
            set { Session["PaginaBusquedaHabilitacion"] = value; }
            get => Session["PaginaBusquedaHabilitacion"] == null ? 0 : Convert.ToInt32(Session["PaginaBusquedaHabilitacion"]);
        }

        public int PagPaginador
        {
            set { Session["PagPaginadorHabilitacion"] = value; }
            get => Session["PagPaginadorHabilitacion"] == null ? 1 : Convert.ToInt32(Session["PagPaginadorHabilitacion"]);
        }


        public int TamanhoPagina
        {
            set { Session["TamahoPaginaHabilitarion"] = value; }
            get => Session["TamahoPaginaHabilitarion"] == null ? 10 : Convert.ToInt32(Session["TamahoPaginaHabilitarion"]);
        }

        #endregion




        //RFD 12/12/2020 -> Refactorizado para implementar gráfica base.
        protected void Page_Load(object sender, EventArgs e)
        {

            var main = (Main)Master.Master;
            main.MensajeDevolucion += main_MensajeDevolucion;

            if (!IsPostBack)
            {

                txtRFCLaboratorio.Text = Filtro.RFCLaboratorio;
                txtNombreLaboratorio.Text = Filtro.NombreLaboratorio;
                txtRFCCliente.Text = Filtro.RFCCliente;
                txtNombreCliente.Text = Filtro.NombreCliente;
                txtFolio.Text = Filtro.foliocna != null && Filtro.foliocna > 0 ? Filtro.foliocna.ToString() : "";
                txtNumInforme.Text = Filtro.informe;
                txtConformacionInicio.Text = Filtro.FechaInicialC != null ? ((DateTime)Filtro.FechaInicialC).ToString("dd/MM/yyyy") : "";
                txtConformacionFin.Text = Filtro.FechaFinalC != null ? ((DateTime)Filtro.FechaFinalC).ToString("dd/MM/yyyy") : "";
                txtFirmaInicio.Text = Filtro.FechaInicialF != null ? ((DateTime)Filtro.FechaInicialF).ToString("dd/MM/yyyy") : "";
                txtFirmaFin.Text = Filtro.FechaFinalF != null ? ((DateTime)Filtro.FechaFinalF).ToString("dd/MM/yyyy") : "";

                var totalRegistros = CountRegistros(Filtro);
                var listaAnalisis = obtenerAnalisis(Filtro, PaginaBusqueda, TamanhoPagina).ToList();
                CargarInformacion(listaAnalisis, PagPaginador, totalRegistros);
            }
        }



        public void CargarInformacion(IEnumerable<Vw_Habilitar_Analis> listaAnalisis, int pagina, int totalFilas)
        {
            rptHabilitacion.DataSource = listaAnalisis.ToList();
            rptHabilitacion.DataBind();
            wucPaginador.Initialize(pagina, totalFilas);
            chkSeleccionTodos.Checked = false;
            pnlListaAnalisis.Update();
        }

        protected void btnBusqueda_OnClick(object sender, EventArgs e)
        {
            PagPaginador = 1;
            PaginaBusqueda = 0;
            TamanhoPagina = wucPaginador.PageSize;


            var fil = ObtenerBusqueda();
            var totalRegistros = CountRegistros(fil);

            var ordenesT = obtenerAnalisis(fil, PaginaBusqueda, TamanhoPagina).ToList();
            CargarInformacion(ordenesT, PagPaginador, totalRegistros);

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtRFCLaboratorio.Text = "";
            txtNombreLaboratorio.Text = "";
            txtRFCCliente.Text = "";
            txtNombreCliente.Text = "";
            txtFolio.Text = "";
            txtNumInforme.Text = "";
            txtConformacionInicio.Text = "";
            txtConformacionFin.Text = "";
            txtFirmaInicio.Text = "";
            txtFirmaFin.Text = "";

            PagPaginador = 1;
            PaginaBusqueda = 0;
            
            var fil = ObtenerBusqueda();
            var totalRegistros = CountRegistros(fil);
            var ordenesT = obtenerAnalisis(fil, PaginaBusqueda, TamanhoPagina).ToList();
            CargarInformacion(ordenesT, PagPaginador, totalRegistros);
        }

        protected void lnkbtnback_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(1);
            Response.Redirect(regreso);
        }

        protected void wucPaginador_OnPageChange(object sender, PageArgs e)
        {
            ObtenerBusqueda();
            PagPaginador = e.CurrentPage;
            PaginaBusqueda = PagPaginador - 1;
            TamanhoPagina = wucPaginador.PageSize;
            var totalRegistros = CountRegistros(Filtro);
            var listaAnalisis = obtenerAnalisis(Filtro, PaginaBusqueda, TamanhoPagina).ToList();
            CargarInformacion(listaAnalisis, PagPaginador, totalRegistros);
        }

        private FiltroshabilitarAnalisArg ObtenerBusqueda()
        {
            try
            {
                Filtro = new FiltroshabilitarAnalisArg
                {
                    foliocna = string.IsNullOrEmpty(txtFolio.Text) ? (int?) null : Convert.ToInt32(txtFolio.Text),
                    RFCLaboratorio = txtRFCLaboratorio.Text,
                    NombreLaboratorio = txtNombreLaboratorio.Text,
                    RFCCliente = txtRFCCliente.Text,
                    NombreCliente = txtNombreCliente.Text,
                    informe = txtNumInforme.Text,
                    FechaInicialC = string.IsNullOrEmpty(txtConformacionInicio.Text)
                        ? (DateTime?) null
                        : Convert.ToDateTime(txtConformacionInicio.Text),
                    FechaFinalC = string.IsNullOrEmpty(txtConformacionFin.Text)
                        ? (DateTime?) null
                        : Convert.ToDateTime(txtConformacionFin.Text),
                    FechaInicialF = string.IsNullOrEmpty(txtFirmaInicio.Text)
                        ? (DateTime?) null
                        : Convert.ToDateTime(txtFirmaInicio.Text),
                    FechaFinalF = string.IsNullOrEmpty(txtFirmaFin.Text)
                        ? (DateTime?) null
                        : Convert.ToDateTime(txtFirmaFin.Text),
                };
            }
            catch (Exception e)
            {
                this.PopMensaje("Verifique los datos capturados", "Error búsqueda", Opciones.SoloAceptar, Icono.error);
            }

            return Filtro;
        }

        private IEnumerable<Vw_Habilitar_Analis> obtenerAnalisis(FiltroshabilitarAnalisArg fil, int? pagina, int tamanhoPagina)
        {
            var oT = new VwOtK();
            var sortIndex = "";
            var sortDirection = "";
            return oT.ListaOrdenTrabajoNofirmada(fil, pagina ?? 0, tamanhoPagina, Login.UsuarioRolID, Login.LaboratorioID, sortIndex, sortDirection.ToUpper());
        }

        private int CountRegistros(FiltroshabilitarAnalisArg filtro)
        {
            var oT = new VwOtK();
            return oT.Cuenta_OrdenTrabajo(filtro);
        }

        protected void wucAppletFIEL_OnFirmarColeccion(object sender, DatosFirma_Args e)
        {
            FirmaElectronica datosFirma;
            datosFirma = e.DatosAFirmar;
            byte[] ArchivoKey = e.ArchivoKey;
            byte[] ArchivoCer = e.ArchivoCer;
            string CNT = e.txtContra;
            List<string> listaErrorFirma = new List<string>();


            if (Convert.ToInt32(Login.UsuarioRolID) == enumUsuarioRol.Habilitar.ParseToInt())
            {
                if (RFCValidoParaHabilitar(Login.Persona, e.RFCFirma))
                {
                    if (e.ArchivoCer == null)
                    {
                        listaErrorFirma.Add("No ha introducido el archivo .cer");
                    }

                    if (e.ArchivoKey == null)
                    {
                        listaErrorFirma.Add("No ha introducido el archivo .key");
                    }

                    if (string.IsNullOrEmpty(e.RFCFirma))
                    {
                        listaErrorFirma.Add("No ha introducido el RFC");
                    }

                    if (string.IsNullOrEmpty(e.txtContra))
                    {
                        listaErrorFirma.Add("No ha introducido la contraseña");
                    }

                    if (!listaErrorFirma.Any())
                    {
                        foreach (var contenedor in datosFirma.listaContenedores)
                        {
                            contenedor.ArchivoCer = e.ArchivoCer;
                            contenedor.ArchivoKey = e.ArchivoKey;
                            contenedor.txtRFC = e.RFCFirma;
                            contenedor.txtContra = e.txtContra;

                            if (contenedor.valido)
                            {
                                var analisisId = 0;
                                int.TryParse(contenedor.primaryKey, out analisisId);
                                var xCert = new X509Certificate2();
                                var ahora = DateTime.Now;

                                try
                                {
                                    xCert = new X509Certificate2(e.ArchivoCer);
                                }
                                catch (Exception exception)
                                {
                                    listaErrorFirma.Add("Ocurrió un error al intentar recuperar el Número de serie del certificado");
                                }

                                if (!listaErrorFirma.Any())
                                {
                                    var arg = new AnalisisCapturaArgs { Procesado = false };
                                    var otb = new FiltrosBitacora
                                    {
                                        FechaMod = ahora,
                                        IdUser = Login.UsuarioID,
                                        NUser = Login.Usuario1,
                                        NSerieCert = xCert.SerialNumber,
                                        ModuloMod = "Habilita análisis"
                                    };

                                    arg.AnaisisID = analisisId;
                                    var cadena = e.RFCFirma + "|" + ahora + "|" + Login.Correo + "|" + Login.Usuario1 + "|" + arg.AnaisisID;
                                    otb.CadenaFirma = cadena;
                                    contenedor.cadenaAFirmar = cadena;
                                    otb.Firma = contenedor.cadenaAFirmarBase64;
                                    otb.TipoMov = "Se habilita análisis firmado fuera de tiempo:" + arg.AnaisisID;

                                    var analisisBl = new Analisis_BL();
                                    var analisisVw = analisisBl.GuardaVw(arg);

                                    if (analisisVw == null)
                                    {
                                        listaErrorFirma.Add("Ocurrió un error al actualizar el estatus del análisis con Folio CNA: " + analisisId);
                                    }
                                    else
                                    {
                                        var oT = new VwOtK();
                                        var bitacora = oT.GuaradAdd(otb);

                                        if (bitacora == null)
                                        {
                                            listaErrorFirma.Add("Ocurrió un error durante el guardado de la bitácora del análisis con Folio CNA: " + analisisId);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                listaErrorFirma.Add(contenedor.mensajeError);
                                break;
                            }
                        }
                    }
                }
                else
                {

                    listaErrorFirma.Add("El RFC autorizado para habilitar no coincide con el capturado.");
                }
            }
            else
            {
                listaErrorFirma.Add("El usuario no cuenta con los permisos para habilitar análisis extemporáneos.");
            }

            if (listaErrorFirma.Any())
            {
                this.PopMensaje(GenerarCadenaErrores(listaErrorFirma), "Errores habilitación", Opciones.SoloAceptar, Icono.error);
            }
            else
            {
                this.PopMensaje("Análisis habilitado/s con éxito", "Habilitacion análisis");

                var page = 1;
                var pageBusqueda = 0;
                var rpagina = wucPaginador.PageSize;

                var fil = ObtenerBusqueda();
                var totalRegistros = CountRegistros(fil);

                var ordenesT = obtenerAnalisis(fil, pageBusqueda, rpagina).ToList();
                CargarInformacion(ordenesT, page, totalRegistros);
            }
        }

        protected void btnHabilitar_OnClick(object sender, EventArgs e)
        {
            this.PopMensaje("¿Desea continuar con la habilitación de análisis extemporáneos?", "Habilitación de análisis extemporáneo", Opciones.SiNo, Icono.information, "FirmarAnalisis", new string[] { });
        }

        protected void main_MensajeDevolucion(object sender, MensajeArgs e)
        {
            switch (e.Command)
            {
                case "FirmarAnalisis":
                    //Presenta el modal para la firma electrónica.
                    if (e.Opcion == Opcion.Si || e.Opcion == Opcion.Aceptar)
                    {
                        var datosFirma = ObtenerDatosAFirmar();

                        wucAppletFIEL.enviarAFirma(datosFirma.DatosAFirmar);
                        wucAppletFIEL.Visible = true;
                    }
                    break;
            }
        }


        //TODO -> Consultar si es necesario agregar toda la información del análisis.
        private DatosFirma_Args ObtenerDatosAFirmar()
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FirmaElectronica();

            foreach (RepeaterItem dato in rptHabilitacion.Items)
            {
                var chkSeleccionAnalisis = (CheckBox)dato.FindControl("chkSeleccionAnalisis");

                if (chkSeleccionAnalisis != null)
                {
                    var analisisId = Convert.ToInt32(chkSeleccionAnalisis.Attributes["data-analisisid"]);

                    Analisi analisisDal = new Analisi();
                    Analisis_BL analisisBL = new Analisis_BL();
                    analisisDal = analisisBL.Obten_Analisis(analisisId);

                    if (analisisDal != null)
                    {
                        var seleccionado = chkSeleccionAnalisis.Checked;

                        if (seleccionado)
                        {
                            var Llave = analisisDal.AnalisisID.ToString();
                            CadenaOriginal cadenaOriginal = new CadenaOriginal();
                            cadenaOriginal.Datos.Add("Cadena a sustituir: ");
                            resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);

                        }
                    }
                }
            }
            return resultado;
        }

        private bool RFCValidoParaHabilitar(Persona persona, string rfcCertificado)
        {
            var flagRFC = ConfigurationManager.AppSettings["flagRFC"] == "true";

            if (!flagRFC)
            {
                var rfcHabilitador = persona.RFC;

                return rfcHabilitador != null && string.Equals(rfcCertificado.Trim(), rfcHabilitador.Trim(), StringComparison.CurrentCultureIgnoreCase) && persona.Activo;
            }

            return true;
        }


        private string GenerarCadenaErrores(List<string> errores)
        {
            var cadena = "<ul>";
            errores.ForEach(error => cadena += "<li>" + error + "</li>");
            cadena += "</ul>";
            return cadena;
        }

        
        private bool ObjectIsNullOrEmpty(object objeto)
        {
            return objeto.GetType().GetProperties().Select(p => p.GetValue(objeto)).All(valor => valor is null || valor is string s && string.IsNullOrEmpty(s));
        }
    }
}