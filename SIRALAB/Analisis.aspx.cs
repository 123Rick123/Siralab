using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SIRALAB.Common;
using SIRALAB.controles;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB.Controles;
using SIRALAB.Controles.AnexoE;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB.Entidades;


namespace SIRALAB
{

    public partial class Analisis : System.Web.UI.Page
    {

        public int OrdenTrabajoID
        {
            get { return (int)Session["OrdenTrabajoID_"] == 0 ? 0 : (int)Session["OrdenTrabajoID_"]; }
            set { Session["OrdenTrabajoID_"] = value; }
        }

        public int AnalisisID
        {
            get { return (int)Session["AnalisisID_"] == 0 ? 0 : (int)Session["AnalisisID_"]; }
            set { Session["AnalisisID_"] = value; }
        }

        public List<XMuestras> muestrasXML
        {
            get
            {
                return (List<XMuestras>)Session["muestras_"] == null
                    ? new List<XMuestras>()
                    : (List<XMuestras>)Session["muestras_"];
            }
            set { Session["muestras_"] = value; }
        }

        public Anexo Anexo
        {
            get { return (SIRALAB_DAL.Anexo)Session["AnexoConsulta_"]; }
        }

        public Analisi Analisis_
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }

        public Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }

        public string SesionEcoliEnterococos
        {
            get { return (string)Session["SesionEcoliEnterococos"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region RegionConductividad
            wucConductividadElectrica1.CheckEcoliEnterococos += wucConductividadElectrica_CheckEcoliEnterococos;
            if (Analisis_.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")//se valida para la norma 2021
            {
                Analisis_BL analisisBL = new Analisis_BL();
                var an = analisisBL.Obten_Analisis(AnalisisID);
                if (an.Muestras.Count > 0)
                {
                    pnlConductividadElectrica.Visible = true;
                }
                else
                {
                    pnlConductividadElectrica.Visible = false;
                }

            }
            else
            {
                pnlConductividadElectrica.Visible = false;
            }
            #endregion
            if (!IsPostBack)
            {
                Session["SesionEcoliEnterococos"] = null;
                var ordenTrabajoBl = new OrdenTrabajo_BL();
                var muestraBl = new Muestra_BL();

                wucDatosMuestra1.CargaDrops();

                var orden = ordenTrabajoBl.Obten(this.OrdenTrabajoID);
                var listaMuestra = muestraBl.Lista_Muestra(this.Analisis_.AnalisisID);

                if (orden != null)
                {
                    lblFolio.Text = "Número: " + orden.NumeroInforme;
                    if (orden.Norma.MuestrasMultiple ?? false)
                    {
                        pnlMuestraSimple.Visible = true;
                        pnlSignatario.Visible = true;
                        lblTtituloPanelMuestraSimple.Text =
                            Analisis_.OrdenTrabajo.NormaID == 4 ? "Muestra" : "Muestras simples";
                        switch (listaMuestra.Count)
                        {
                            case 0:
                                pnlMuestraSimple.Visible = false;
                                pnlSignatario.Visible = false;
                                //wucMuestraXML.Visible = false;
                                break;
                            default:
                                pnlMuestraSimple.Visible = true;
                                pnlSignatario.Visible = true;
                                //wucMuestraXML.Visible = true;
                                wucMuestreador1.ObtenInformacionMuestreador();
                                break;
                        }
                    }
                    else
                    {
                        pnlMuestraSimple.Visible = false;
                        pnlSignatario.Visible = false;
                    }

                }

                //if (orden != null)
                //    switch (orden.Norma.NormaID)
                //    {

                //        case 1:
                //        case 14:
                //            pnlMuestraSimple.Visible = true;
                //            pnlSignatario.Visible = true;
                //            switch (listaMuestra.Count)
                //            {
                //                case 0:
                //                    pnlMuestraSimple.Visible = false;
                //                    pnlSignatario.Visible = false;
                //                    break;
                //                default:
                //                    pnlMuestraSimple.Visible = true;
                //                    pnlSignatario.Visible = true;
                //                    wucMuestreador1.ObtenInformacionMuestreador();
                //                    break;
                //            }
                //            break;
                //        default:
                //            pnlMuestraSimple.Visible = false;
                //            pnlSignatario.Visible = false;
                //            break;
                //    }
                if (this.Analisis_ != null)
                {
                    if (listaMuestra.Count > 0)
                        CargaInformacion(this.Analisis_.AnalisisID);
                }
            }
        }

        public void CargaInformacion(int AnalisisID)
        {
            var ordentrabajoBl = new OrdenTrabajo_BL();
            var orden = ordentrabajoBl.Obten(this.OrdenTrabajoID);
            Session["AnalisisID_"] = this.Analisis_.AnalisisID;
            this.AnalisisID = this.Analisis_.AnalisisID;

            if (orden != null)
            {
                lblFolio.Text = "Número: " + orden.NumeroInforme;
                if (orden.Norma.MuestrasMultiple ?? false)
                {
                    //RFD 15/06/2020 -> Se comenta para eliminar la funcionalidad a petición del usuario.
                    //lnkAgregarParametro.Visible = orden.Norma.Norma2 != null;
                    lnkGenerarCompuesta.Visible = orden.Norma.RequiereCompuesta;
                    pnlMuestraSimple.Visible = true;
                    wucMuestrasMultiple.Initialize(Analisis_.AnalisisID);
                    lbGuardarSimples.Visible = lnkGenerarCompuesta.Visible;
                }
                else
                {
                    wucMuestraSimple1.CargaInformacion(this.Analisis_.AnalisisID);
                    pnlMuestraSimple.Visible = false;
                }
            }
            //if (orden != null)
            //    switch (orden.Norma.NormaID)
            //    {
            //        case 1:
            //            pnlMuestraSimple.Visible = true;
            //            //_wucMuestraSiempleAnexoE.CargaInformacion(Analisis_.AnalisisID);
            //            wucMuestrasMultiple.Initialize(Analisis_.AnalisisID);
            //            break;
            //        default:
            //            wucMuestraSimple1.CargaInformacion(this.Analisis_.AnalisisID);
            //            pnlMuestraSimple.Visible = false;
            //            break;
            //    }

            wucDatosMuestra1.CargaAnalisis(this.Analisis_.Muestras.First().MuestraID);
            wucMuestraCompuesta1.CargaInformacion(this.Analisis_.AnalisisID);
            //wucEvidencias1.CargarEvidencias(this.Analisis_);


            Muestra_BL muestra_bl = new Muestra_BL();
            var MuestraCompuesta = muestra_bl.Lista_Muestra(AnalisisID).Where(x => x.NivelID == 2).ToList();
            if (MuestraCompuesta.Count > 0 || !this.Analisis_.OrdenTrabajo.Norma.RequiereCompuesta)
            {
                /* Oculta el boton de generar compuesta sí ya existe una compuesta o si no se requiere de la compuesta */
                lnkGenerarCompuesta.Visible = false;
                lbGuardarSimples.Visible = lnkGenerarCompuesta.Visible;
            }
        }

        protected void wucDatosMuestra_Guardar(object sender, MuestraCapturaArgs e)
        {
            var arg = (MuestraCapturaArgs)e;

            if (arg != null)
            {
                if (arg.Descripcion == "DatosFaltantes")
                {
                    this.PopMensaje("Faltan datos para generar la muestra", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);

                }
                else
                {
                    this.PopMensaje("Muestras generadas exitosamente", "Muestras generadas",
                        controles.Opciones.SoloAceptar, controles.Icono.information);
                    CargaInformacion(this.Analisis_.AnalisisID);
                }

            }
        }

        protected void wucMuestraSimple1_Guardar(object sender, MuestraCapturaArgs e)
        {
            var arg = (MuestraCapturaArgs)e;

            if (arg != null)
            {
                //this.PopMensaje("Muestra compuesta generada exitosamente", "Muestras compuesta generada", controles.Opciones.SoloAceptar, controles.Icono.information);

                //RFD 15/07/2020 se elimina PopMensaje para que muestre el mensaje de guardado correctamente en un modal.
                lblModalTitle.Text = "Datos almacenados correctamente";
                lblModalBody.Text = "La muestra compuesta se ha generado correctamente";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();

                //RFD 15/07/2020 -> si la muestra compuesta se genera correctamente se oculta el botón "Generar compuesta".
                wucDatosMuestra1.CargaAnalisis(this.Analisis_.Muestras.First().MuestraID);
                if (!(this.Analisis_.OrdenTrabajo.Norma.MuestrasMultiple ?? false))
                    wucMuestraSimple1.CargaInformacion(this.Analisis_.AnalisisID);
                wucMuestraCompuesta1.CargaInformacion(this.Analisis_.AnalisisID);

                //Se añaden para ocultar el botón de generar compuesta.
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "divMensajeAvisoGuardar",
                    "$('#divMensajeAvisoGuardar').hide();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "classLnkGenerarCompuesta",
                    "$('.classLnkGenerarCompuesta').hide();", true);

                //Ajusta los valores que aparecen en tablaMuestraSimpel
                var script =
                    "var x = document.getElementById('tablaMuestraSimple').rows[0].cells; $('#mnsValidaHojaCampo').attr('colSpan', x.length - 4); ";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "tablaMuestraSimple", script, true);

            }
        }

        protected void wucMuestra_MSIncompleta(object sender, bool e)
        {
            bool MSincompleta = e;
            if (!MSincompleta)
            {
                //RFD 26/06/2020 se añade método para generar mensaje dependiendo de los errores presentados al intentar generar la muestra compuesta.
                if (!Analisis_.OrdenTrabajo.Norma.Evaluacion)
                {
                    this.PopMensaje("Verifique los valores capturados para las muestras",
                        "Muestras compuesta no generada", controles.Opciones.SoloAceptar, controles.Icono.error);
                }
                else
                {
                    var muestraBL = new Muestra_BL();
                    var muestras = muestraBL.Lista_Muestra(this.Analisis_.AnalisisID);
                    var mensaje = generaMensajeValidacionCompuesta(muestras);
                    this.PopMensaje(mensaje, "Muestras compuesta no generada", controles.Opciones.SoloAceptar,
                        controles.Icono.error);
                }

            }
        }

        protected void lnkbtnBack_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(2);
            Response.Redirect(regreso);
        }



        protected void wucMuestra_Evidencia(object sender, EventArgs e)
        {
            //RFD 15/06/2020 -> Se comenta a petición del usuario.
            //wucPopFiles.Mostrar((Muestra)sender);
        }

        protected void wucAgregarParametroAnalisis_OnGuardar(object sender, AgregarParametroArgs e)
        {
            if (e.MuestraValores == null || e.MuestraValores?.Count == 0)
            {
                this.PopMensaje(
                    "La selección es incorrecta, debe seleccionar el/los parametro(s) y capturar su escala decimal, en caso de no requerir decimales favor de ingresar el valor 0.",
                    "Parámetro particular");
                return;
            }

            /// TODO: debemos complementar con MuestraID
            ///AnalisisID
            /// 
            Muestra_BL muestraBl = new Muestra_BL();
            var muestras = muestraBl.Lista_Muestra(this.AnalisisID);

            foreach (var muestra in muestras.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()))
            {
                foreach (var muestraValor in e.MuestraValores)
                {
                    MuestraValor_BL muestraValorBl = new MuestraValor_BL();
                    muestraValorBl.Guardar(new MuestraValorArgBase_BL
                    {
                        MuestraID = muestra.MuestraID,
                        ParametroID = muestraValor.ParametroID,
                        Exenta = muestraValor.Exenta,
                        Evalua = muestraValor.Evalua,
                        AceptaValor = muestraValor.AceptaValor,
                        LFD = muestraValor.LFD,
                        EscalaDecimal = muestraValor.EscalaDecimal,
                        LCMC = muestraValor.LCMC
                    });
                }
            }

            CargaInformacion(this.AnalisisID);
            upGrid.Update();
        }

        protected void wucEvidenciaPop1_Cerrar(object sender, EventArgs e)
        {

        }

        protected void wucMuestraDatos1_Guardar(object sender, EventArgs e)
        {
            switch (((MuestraCapturaArgs)e).Descripcion)
            {
                case "SinParametros":
                    this.PopMensaje(
                        "La norma que ha seleccionado para realizar el análisis no contiene parámetros, por favor contacte al administrador del sistema para informarle la situación presentada",
                        "Norma sin parámetros", controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "SinMuestreador":
                    this.PopMensaje("Falta seleccionar el laboratorio que realiza el muestreo", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorHoras24":
                    this.PopMensaje("La hora debe ser en formato 24 horas: ejemplo 01:01 a 23:59", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorHorasV":
                    this.PopMensaje("Se encuentra vacio el campo de hora", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "Errorhora":
                    this.PopMensaje("No puede estar vacia la hora de ingreso al laboratorio", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorHoras":
                    this.PopMensaje("Las horas de muestreo no pueden empezar de mayor a menor", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorFecha":
                    this.PopMensaje("La fecha de muestreo y/o la fecha de recepción en laboratorio son incorrectas",
                        "Datos incompletos", controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorCHs":
                    this.PopMensaje(
                        "Debes de confirmar que los valores de la hoja de campo física concuerdan con los de la electrónica",
                        "Datos incompletos", controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "Errorlc":
                    this.PopMensaje("El valor del parámetro no puede estar por debajo del límite de cuantificación ",
                        "Datos incompletos", controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "CaudalCero":
                    this.PopMensaje("El caudal debe ser mayor a cero", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorMuestra":
                    this.PopMensaje("El Valor de la muestra compuesta debe ser mayor a cero", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "Errorcaudal":
                    this.PopMensaje("El caudal debe de ser igual  o mayor a '0'", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                case "ErrorT":
                    this.PopMensaje(Session["errort"].ToString(), "Datos incompletos", controles.Opciones.SoloAceptar,
                        controles.Icono.error);
                    break;
                case "incompleta":
                    this.PopMensaje("No se puede generar la muestra simple está incompleta. ", "Datos incompletos",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    break;
                default:
                    this.PopMensaje("Información guardada/actualizada correctamente", "Información guardada",
                        controles.Opciones.SoloAceptar, controles.Icono.ok);
                    break;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Metodo> CargaMetodos(int laboratorioId, int parametroId)
        {
            Metodo_BL _metBl = new Metodo_BL();
            var metodos = _metBl.Obten_Metodo(laboratorioId, parametroId);
            return metodos;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Resultado GuardarSimples(List<Muestra> muestras, int analisisId, LaboratorioSignatario laboratorioSignatario, bool muestracompleta, List<FechaMuestraSimple> listaFechaMuestraSimples, string Conductividad, string GuardaConductividad)
        {
            Resultado resultado = new Resultado();

            //RFD -> 06/01/21: se añade para evitar conflictos con zonas horarias:
            var fechasObjetc = listaFechaMuestraSimples.FirstOrDefault();
            var fechaConformacion = ConvertirFecha(fechasObjetc?.FechaConformacion, fechasObjetc?.HoraConformacion);
            var fechaRecepcion = ConvertirFecha(fechasObjetc?.FechaRecepcion, fechasObjetc?.HoraRecepcion);
            #region RegionConductividad 
            //metodo para validar cuando ya se guardo un analisis previo, con otro valor en conductividad 
            Analisis_BL analisisBLL = new Analisis_BL();
            var analisisVC = analisisBLL.Obten_Analisis(analisisId);
            if (analisisVC.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")//funciona para la norma NOM-001-SEMARNAT-2021
            {
                if (analisisVC.Conductividad != null && analisisVC.Conductividad != string.Empty)
                {
                    var ValidaGuardado1 = analisisBLL.ValidaConductividadByFechaConformacion(analisisId, Conductividad, analisisVC, fechaConformacion);
                    string FConformacion = string.Empty;
                    if (analisisVC.Muestras.First().FechaConformacion != null)
                    {
                        FConformacion = analisisVC.Muestras.First().FechaConformacion.Value.ToShortDateString();
                    }
                    if (!ValidaGuardado1 && fechaConformacion.Value.ToShortDateString() != FConformacion)
                    {
                        string conductividad = analisisVC.Conductividad;
                        if (analisisVC.Conductividad.Contains("Escherichia"))
                        {
                            conductividad = conductividad + " " + "coli";
                        }
                        else if (analisisVC.Conductividad.Contains("Enterococos"))
                        {
                            conductividad = conductividad + " " + "fecales";
                        }
                        resultado.Message = "No se puede guardar la información, debido a que la conductividad es diferente a la de los análisis reportados en el trimestre.";
                        return resultado;
                    }
                }
            }
            #endregion
            foreach (var muestra in muestras)
            {
                Muestreador_BL muestreadorBl = new Muestreador_BL();
                LaboratorioSignatario_BL signatarioBl = new LaboratorioSignatario_BL();
                var signatario = signatarioBl.Obten_LaboratorioSignatario(laboratorioSignatario.LaboratorioID,
                    laboratorioSignatario.PersonaID);
                var signatarioSaved = signatarioBl.Obten_LaboratorioSignatario(muestra.MuestraID);
                var muestreadorArgs = new MuestreadorCapturaArgs();
                if (signatario != null || signatarioSaved != null)
                {
                    muestreadorArgs.MuestraID = muestra.MuestraID;
                    muestreadorArgs.LaboratorioSignatarioID = signatario == null
                        ? signatarioSaved.LaboratorioSignatarioID
                        : signatario.LaboratorioSignatarioID;
                    muestreadorBl.Guarda(muestreadorArgs);
                }

                Muestra_BL muestraBl = new Muestra_BL();
                var muestraTemp = muestraBl.Obten_Muestra(muestra.MuestraID);
                Analisis_BL an = new Analisis_BL();
                NormaConfiguracion_BL ncb = new NormaConfiguracion_BL();
                var anl = an.Obten_Analisis(analisisId);

                //Aquí se almacenan los valores que se van a la tabla muestra
                muestraTemp.Numero = muestraTemp.Numero;
                muestraTemp.Descripcion = muestraTemp.Descripcion;
                muestraTemp.Identificacion = muestraTemp.Identificacion;
                muestraTemp.Caudal = muestra.Caudal;

                var fechaMuestraSimple = listaFechaMuestraSimples.FirstOrDefault(fecha => fecha.MuestraId == muestra.MuestraID);
                muestraTemp.Fecha = ConvertirFecha(fechaMuestraSimple?.Fecha, fechaMuestraSimple?.Hora);
                muestraTemp.FechaRecepcion = fechaRecepcion;
                muestraTemp.FechaConformacion = fechaConformacion;
                muestraTemp.Cancelado = muestra.Cancelado;
                muestraTemp.Observaciones = muestra.Observaciones;
                muestraTemp.NivelID = muestraTemp.NivelID;
                muestraTemp.ValMuestraC = muestra.ValMuestraC ?? 0;
                muestraTemp.ValHojaCampo = muestra.ValHojaCampo ?? 0;
                if (muestraTemp.NivelID == 1 &&
                    !new Norma_BL().Obtener(Convert.ToInt32(anl.OrdenTrabajo.NormaID)).RequiereCompuesta)
                {
                    muestraTemp.FlagCompuesta = muestracompleta;
                    muestraTemp.FechaRecepcion = DateTime.Now;
                }

                muestraBl.ActualizaMuestra(muestraTemp);

                foreach (var muestraMuestraValor in muestra.MuestraValors)
                {
                    MuestraValor_BL muestraValorBl = new MuestraValor_BL();
                    ParametroMetodo_BL parametroMetodoBl = new ParametroMetodo_BL();
                    MuestraValorArgBase_BL mvabBl = new MuestraValorArgBase_BL();

                    var mv = muestraValorBl.Obten(muestraMuestraValor.MuestraValorID);
                    var mp = parametroMetodoBl.Obten(muestraMuestraValor.ParametroID,
                        muestraMuestraValor.ParametroMetodoID ?? 0);

                    mvabBl.MuestraValorID = (int)mv.MuestraValorID;
                    mvabBl.MuestraID = mv.MuestraID;
                    mvabBl.ParametroID = (int)mv.ParametroID;
                    mvabBl.OperacionID = mv.OperacionID;
                    mvabBl.Exenta = (bool)mv.Exenta;
                    mvabBl.Evalua = (bool)mv.Evalua;
                    mvabBl.AceptaValor = mv.AceptaValor;
                    mvabBl.LFD = (bool)mv.LFD;


                    mvabBl.LimiteInferiorLFD = mv.LimiteInferiorLFD;
                    mvabBl.LimiteSuperiorLFD = mv.LimiteSuperiorLFD;
                    mvabBl.Grupo = (bool)mv.Grupo;
                    mvabBl.EscalaDecimal = mv.EscalaDecimal;
                    mvabBl.LCMC = mv.LCMC;
                    mvabBl.GeneradoSP = false;
                    mvabBl.Valor = muestraMuestraValor.Valor ?? 0;
                    mvabBl.ValorValidacion = muestraMuestraValor.ValorValidacion;
                    mvabBl.LaboratorioID = muestraMuestraValor.LaboratorioID;
                    mvabBl.ParametroMetodoID = mp?.ParametroMetodoID;
                    mvabBl.LaboratorioID = muestraMuestraValor.LaboratorioID;
                    mvabBl.Exento = (bool)muestraMuestraValor.Exento;
                    mvabBl.NoAnalizado = (bool)muestraMuestraValor.NoAnalizado;
                    mvabBl.LC_MC = muestraMuestraValor.LC_MC;

                    //RFD 29/07/2020 -> Permite capturar el Límite Máximo Permisible cuando es CPD:
                    if (muestraTemp.Analisi.OrdenTrabajo.Norma.CPD)
                    {
                        mvabBl.LimiteInferior = muestraMuestraValor.LimiteInferior;
                        mvabBl.LimiteSuperior = muestraMuestraValor.LimiteSuperior;
                    }
                    else
                    {
                        mvabBl.LimiteInferior = mv.LimiteInferior;
                        mvabBl.LimiteSuperior = mv.LimiteSuperior;
                    }

                    if (mp != null)
                        mvabBl.ParametroMetodoID = mp.ParametroMetodoID;

                    muestraValorBl.Guardar(mvabBl);
                }
            }
            Muestra_BL muestraBll = new Muestra_BL();
            var MuestraCompuesta = muestraBll.ObtenerMuestaCompuestaByAnalisisId(analisisId);
            if (MuestraCompuesta != null)
            {
                var actualizaFecha = muestraBll.ActualizaFechaMuestra(MuestraCompuesta.MuestraID, fechaConformacion.Value);
                resultado.Finalized = true;
            }
            #region RegionConductividad
            Analisis_BL analisisBL = new Analisis_BL();
            var analisisValidacion = analisisBL.Obten_Analisis(analisisId);
            if (analisisValidacion.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")//funciona para la norma NOM-001-SEMARNAT-2021
            {
                if (Conductividad != string.Empty && GuardaConductividad == "true")
                {
                    var ValidaGuardado = analisisBL.ValidaConductividad(analisisId, Conductividad, analisisValidacion);
                    if (ValidaGuardado)
                    {
                        var guardaConductividad = analisisBL.ActualizaConductividadElectrica(analisisId, Conductividad);
                    }


                }
            }
            #endregion
            //RFD 11/08/2020 -> se agrega para recalcular la muestra compuesta cuando se modifica la muestra simple.
            try
            {
                Muestra_BL muestra_bl = new Muestra_BL();
                var muestraCompuestaList = muestra_bl.Lista_Muestra(analisisId).Where(x => x.NivelID == 2).ToList();

                if (muestraCompuestaList.Any())
                {
                    var muestraCompuesta = muestraCompuestaList.FirstOrDefault();
                    muestra_bl.Recalcula(muestraCompuesta.Analisi.OrdenTrabajo.NormaID.Value,
                        muestraCompuesta.MuestraID);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }

      
            return resultado;
        }

        public void cargarC()
        {
            upModal.Update();
            //RFD 15/07/2020 -> si la muestra compuesta se genera correctamente se oculta el botón "Generar compuesta".
            wucDatosMuestra1.CargaAnalisis(this.Analisis_.Muestras.First().MuestraID);
            if (!(this.Analisis_.OrdenTrabajo.Norma.MuestrasMultiple ?? false))
                wucMuestraSimple1.CargaInformacion(this.Analisis_.AnalisisID);
            wucMuestraCompuesta1.CargaInformacion(this.Analisis_.AnalisisID);
        }
        private static DateTime? ConvertirFecha(string fecha, string hora)
        {
            if (string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(hora))
                return null;
            DateTime.TryParse(fecha + " " + hora, out var fechaFinal);
            return fechaFinal;
        }

        protected void lnkGenerarCompuesta_OnClick(object sender, EventArgs e)
        {
            bool Avanza_Compuesta = false;
            decimal CompuestaCaudal = 0;
            //Metodo para generar la muestra compuesta
            Muestra_BL muestra_bl = new Muestra_BL();
            Norma_BL ncBL = new Norma_BL();
            MuestraCapturaArgs muestraArgs = new MuestraCapturaArgs();
            Muestra m = null;
            var bandera = true;
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

            if (this.Analisis_ != null)
            {
                //Se realiza la validación para no permitir que se genere la muestra compuesta hasta que esten completas las muestars simples
                var lista_muestra = muestra_bl.Lista_Muestra(this.Analisis_.AnalisisID);
                #region RegionConductividad
                var muestra1 = lista_muestra.FirstOrDefault();
                var EcoliEnterococos = string.Empty;
                if (muestra1.Analisi.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")
                {


                    if (Session["SesionEcoliEnterococos"] != null && Session["SesionEcoliEnterococos"].ToString() != string.Empty)
                    {
                        EcoliEnterococos = Session["SesionEcoliEnterococos"].ToString();
                    }
                    else
                    {
                        EcoliEnterococos = muestra1.Analisi.Conductividad;
                    }
                    foreach (var item in lista_muestra)
                    {

                        if (EcoliEnterococos == "Escherichia")
                        {
                            var borrar = item.MuestraValors.FirstOrDefault(x => x.Parametro.ParametroID == 689);
                            var eliminada = item.MuestraValors.Remove(borrar);
                            Dictionary<bool, string> valores;

                            valores = ValidaEColi();
                            wucConductividadElectrica_CheckEcoliEnterococos(this, valores);
                            if (valores.First().Key)
                            {
                                bandera = false;
                            }
                            else
                            {

                                if (muestra1.Analisi.Conductividad == null || muestra1.Analisi.Conductividad == string.Empty)
                                {
                                    bandera = false;
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeConductividad').modal();", true);
                                }
                                else
                                {
                                    bandera = true;
                                }

                            }

                        }
                        else if (EcoliEnterococos == "Enterococos")
                        {
                            var borrar = item.MuestraValors.FirstOrDefault(x => x.Parametro.ParametroID == 694);
                            var eliminada = item.MuestraValors.Remove(borrar);

                            Dictionary<bool, string> valores;
                            valores = ValidaEnterococos();
                            wucConductividadElectrica_CheckEcoliEnterococos(this, valores);

                            if (valores.First().Key)
                            {
                                bandera = false;
                            }
                            else
                            {

                                if (muestra1.Analisi.Conductividad == null || muestra1.Analisi.Conductividad == string.Empty)
                                {
                                    bandera = false;
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensajeConductividad').modal();", true);
                                }
                                else
                                {
                                    bandera = true;
                                }
                            }

                        }

                    }
                }
                #endregion

                if (bandera) //con esta validacion nos aseguramos que no afecte a las otras normas solo la norma 2021
                {
                    if (lista_muestra.Any(p => p.Caudal != 0))
                    {
                        CompuestaCaudal = lista_muestra.Where(p => p.Caudal != 0).Average(x => x.Caudal);
                    } //Realiza el cálculo aunque tenga valores a cero


                    //Validación utilizada para comprobar que la lista tiene registros
                    if (lista_muestra.Count > 0)
                    {
                        foreach (var muestra in lista_muestra)
                        {
                            MuestraValor_BL muestravalorBL = new MuestraValor_BL();
                            var lista_valores = muestravalorBL.Lista_MuestraValor(muestra.MuestraID);
                            var lista_valoresConfig = ncBL.Obtener((int)muestra.Analisi.OrdenTrabajo.NormaID);
                            #region RegionConductividad
                            if (muestra.Analisi.OrdenTrabajo.Norma.Nombre == "NOM-001-SEMARNAT-2021")
                            {
                                if (EcoliEnterococos == "Escherichia")
                                {

                                    var eliminada = lista_valores.RemoveAll(mv => mv.ParametroID == 689);

                                }
                                else if (EcoliEnterococos == "Enterococos")
                                {
                                    var eliminada = lista_valores.RemoveAll(mv => mv.ParametroID == 694);

                                }
                            }
                            #endregion
                            //Validación utilizada para comprobar que existen registros en muestra valor
                            if (lista_valores.Count > 0 &&
                                lista_valoresConfig.NormaConfiguracions.Count(p =>
                                    p.NivelID == enumNivel.Simple.ConvertToInt()) > 0)
                            {
                                foreach (var val in lista_valores)
                                {
                                    if (val.GeneradoSP == false)
                                    {
                                        Avanza_Compuesta = true;
                                    }
                                    else
                                    {
                                        Avanza_Compuesta = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //Cuando la muestra no requiere caudal, pero tiene compuesta.
                                if (!this.Analisis_.OrdenTrabajo.Norma.RequiereCaudal && muestra.Caudal == -1)
                                {
                                    Avanza_Compuesta = true;
                                }
                                else
                                {
                                    if (muestra.Cancelado)
                                    {
                                        Avanza_Compuesta = true;
                                    }
                                    else
                                    {
                                        if (!Avanza_Compuesta)
                                        {
                                            Avanza_Compuesta =
                                                lista_valoresConfig.NormaConfiguracions.Count(p =>
                                                    p.NivelID == enumNivel.Simple.ConvertToInt()) == 0;
                                        }

                                        break;
                                    }
                                }

                            }
                        }
                    }


                    //Verifica que no todas las muestras tengan un valor nulo.
                    if (Avanza_Compuesta)
                    {
                        int suma = 0;
                        foreach (Muestra VerifCaudal in lista_muestra)
                        {
                            if (VerifCaudal.Caudal <= 0)
                            {
                                suma += 1;
                            }
                        }

                        if (suma == lista_muestra.Count)
                        {
                            Avanza_Compuesta = false;
                        }
                    }

                    if (lista_muestra.Any(muestra =>
                        muestra.NivelID == enumNivel.Simple.ConvertToInt() && muestra.MuestraValors.Count > 0))
                        Avanza_Compuesta = Avanza_Compuesta && validaGeneracionCompuesta(lista_muestra);

                    //SE REALIZA UN CICLO CON EL NUMERO DE MUESTRAS CONFIGURADO EN LA ORDEN DE TRABAJO
                    if (Avanza_Compuesta)
                    {
                        MuestraValor_BL muestravalorBLL = new MuestraValor_BL();
                        //INICIO Funcionalidad para modificar los parametros, no analizado y exento
                        var lista_MuestraValor = muestravalorBLL.Lista_MuestraValor_Analisis(this.Analisis_.AnalisisID);

                        #region RegionConductividad
                        //Aqui no validamos la norma porque ya se valido arriba 
                        if (EcoliEnterococos == "Escherichia")
                        {

                            var eliminada = lista_MuestraValor.RemoveAll(mv => mv.ParametroID == 689);

                        }
                        else if (EcoliEnterococos == "Enterococos")
                        {
                            var eliminada = lista_MuestraValor.RemoveAll(mv => mv.ParametroID == 694);

                        }

                        #endregion
                        foreach (var val in lista_MuestraValor)
                        {
                            var lista_Parametro_NoAnalizado =
                                lista_MuestraValor.Where(x => x.ParametroID == val.ParametroID && x.NoAnalizado);
                            if (lista_Parametro_NoAnalizado.Any())
                            {
                                var Lista_ACtualizar_NoAnalizado =
                                    lista_MuestraValor.Where(q => q.ParametroID == val.ParametroID).ToList();

                                foreach (var ma in Lista_ACtualizar_NoAnalizado)
                                {
                                    muestravalor_bl.Actualiza_NoAnalizado(ma.MuestraValorID, true);
                                }
                            }

                            var lista_Parametro_NoExento =
                                lista_MuestraValor.Where(x => x.ParametroID == val.ParametroID && x.Exento == true);
                            if (lista_Parametro_NoExento.Count() > 0)
                            {
                                var Lista_ACtualizar_Exento =
                                    lista_MuestraValor.Where(q => q.ParametroID == val.ParametroID).ToList();

                                foreach (var exe in Lista_ACtualizar_Exento)
                                {
                                    muestravalor_bl.Actualiza_Exento(exe.MuestraValorID, true);
                                }
                            }



                        }
                        //FIN Funcionalidad para modificar los parametros, no analizado y exento
                        Muestra_BL muestraBL = new Muestra_BL();
                        var unicamuestra = muestraBL.Lista_Muestra(this.Analisis_.AnalisisID).FirstOrDefault();


                        muestraArgs.AnalisisID = this.Analisis_.AnalisisID;
                        muestraArgs.DireccionID = unicamuestra.DireccionID;
                        muestraArgs.Numero = unicamuestra.Numero;
                        muestraArgs.Descripcion = "";
                        muestraArgs.Identificacion = "";
                        muestraArgs.Caudal = CompuestaCaudal;
                        muestraArgs.Latitud = unicamuestra.Latitud;
                        muestraArgs.Longitud = unicamuestra.Longitud;
                        muestraArgs.Datum = unicamuestra.Datum;
                        muestraArgs.Fecha = unicamuestra.FechaConformacion;
                        muestraArgs.EsCompuesta = true;
                        muestraArgs.FechaRecepcion = unicamuestra.FechaRecepcion;
                        muestraArgs.Cancelado = false;
                        muestraArgs.Observaciones = "";
                        muestraArgs.NivelID = enumNivel.Compuesta.ConvertToInt();
                        muestraArgs.FechaConformacion = unicamuestra.FechaConformacion;



                        var muestreadores = muestra_bl.Lista_Muestra(this.Analisis_.AnalisisID)
                          .SelectMany(p => p.Muestreadors);


                        var tieneMuestreador =
                            muestreadores
                                .Any(); //lista_MuestraValor.Any(p => p.Muestra.Muestreadors.Any(q => q.Activo ?? false));
                        var laboratorioSinCaptura = lista_MuestraValor.Any(p =>
                            p.LaboratorioID == null && p.Exento == false && p.NoAnalizado == false && p.Muestra.Caudal > 0);
                        var metodoSinCaptura = lista_MuestraValor.Any(p =>
                            p.ParametroMetodoID == null && p.Exento == false && p.NoAnalizado == false &&
                            p.Muestra.Caudal > 0);
                        var valorSinCaptura = lista_MuestraValor.Any(p =>
                            (p.Valor == null || p.Valor < 0) && p.AceptaValor && p.Exento == false &&
                            p.NoAnalizado == false && p.Muestra.Caudal > 0);

                        if (!this.Analisis_.OrdenTrabajo.Norma.NormaConfiguracions.Any(q => q.NivelID == 1))
                        {
                            laboratorioSinCaptura = metodoSinCaptura = valorSinCaptura = false;
                        }
                        var completo = tieneMuestreador && !laboratorioSinCaptura && !metodoSinCaptura && !valorSinCaptura;

                        if (this.Analisis_.Muestras.Any(q => q.NivelID == 2))
                        {
                            this.PopMensaje("El análisis ya contiene una muestra compuesta", "Análisis");
                            lnkGenerarCompuesta.Visible = false;
                        }
                        else
                        {
                            if (completo)
                            {
                                m = muestraBL.Guarda(muestraArgs);
                                lnkGenerarCompuesta.Visible = false;

                                //RFD 20/08/2020 -> Se agrega para generar directamente los muestra valor de la compuesta:
                                if (m != null)
                                {
                                    if (Analisis_.OrdenTrabajo.NormaID != null)
                                        muestravalor_bl.GuardaObtenCompuesta(Analisis_.OrdenTrabajo.NormaID.Value,
                                            m.MuestraID);
                                }
                            }
                            else
                            {
                                this.PopMensaje("Las muestras simples aún no se encuentran completas", "Análisis");
                                lnkGenerarCompuesta.Visible = true;
                            }
                        }

                        if (m != null)
                            wucMuestraSimple1_Guardar(lnkGenerarCompuesta, muestraArgs);
                        else
                            wucMuestraSimple1_Guardar(lnkGenerarCompuesta, null);
                    }
                    else
                    {
                        //Mandamos evento indicando que las muestras simples estan incompletas
                        wucMuestra_MSIncompleta(lnkGenerarCompuesta, false);
                    }
                }//fin if bandera
            }
        }
        protected void lnkSendSaveMessage_Click(object sender, EventArgs e)
        {
            this.PopMensaje("Se ha guardado con éxito", "Análisis");
        }

        //RFD 25/06/2020 añadidas validaciones para la generación de la muestra compuesta.
        private string generaMensajeValidacionCompuesta(List<Muestra> muestras)
        {
            var mensaje = "<ul class='text-justify'>";

            if (isCaudalAndObservacionesCorrectas(muestras))
            {
                mensaje +=
                    "<li>Las observaciones son obligatorias cuando el caudal de la muestra es igual a cero.</li>";
            }

            if (isAnalisisConCaudal(muestras))
            {
                mensaje +=
                    "<li>No es posible generar una muestra compuesta si el valor del caudal es cero en todas las muestras simples.</li>";
            }

            if (!isFechasCorrecta(muestras))
            {
                mensaje += "<li>Verifique que las fechas y horas capturadas para cada muestra son correctas.</li>";
            }

            if (isMuestraConParmetrosCapturados(muestras))
            {
                mensaje += "<li>Verifique que los valores capturados para cada parámetro son correctos.</li>";
            }

            if (validaLCMCorrecto(muestras))
            {
                mensaje += "<li>Verifique los valores capturados para los parámetros que requieren LC/MC.";
            }

            if (isVolumenMuestraCompuestaCorrecto(muestras))
            {
                mensaje += "<li>Verifique el valor capturado para el Volumen de muestra compuesta.</li>";
            }

            if (isFechaRecepcionCorrecta(muestras))
            {
                mensaje += "<li>Verifique la fecha y hora de ingreso al laboratorio.</li>";
            }

            if (isLaboratorioCapturado(muestras))
            {
                mensaje +=
                    "<li>Verifique que se ha seleccionado un laboratorio para todos los parámetros analizados.</li>";
            }

            if (isMetodoCapturado(muestras))
            {
                mensaje +=
                    "<li>Verifique que se ha seleccionado un método para todos los parámetros analizados.</li>";
            }

            if (isFechaConformacionCorrecta(muestras))
            {
                mensaje +=
                    "<li>La fecha de conformación no puede ser anterior a las fechas capturadas para cada una de las muestras simples.</li>";
            }

            if (isRecepcionAnteriorConformacion(muestras))
            {
                mensaje +=
                    "<li>La fecha de ingreso al laboratorio no puede ser anterior a la fecha de conformación de la muestra compuesta.</li>";
            }

            if (validaMinMaxAceptado(muestras))
            {
                mensaje += "<li>Verifique los valores capturados</li>";
            }

            if (Analisis_.OrdenTrabajo.Norma.CPD)
            {
                if (isLMPInferiorCorrecto(muestras))
                    mensaje += "<li>Verifique el límite máximo permisible inferior del parámetro pH (UpH).</li>";

                if (isLMPSuperiorCorrecto(muestras))
                    mensaje += "<li>Verifique los límites máximos permisibles capturados.</li>";

                if (isLMPcorrecto(muestras))
                    mensaje +=
                        "<li>El LMP superior del parámetro pH (UpH) no puede ser menor a su límite inferior.</li>";
            }


            mensaje += "<ul>";
            return mensaje;
        }


        //Método que ejecuta las validaciones.
        private bool validaGeneracionCompuesta(List<Muestra> muestras)
        {
            return !isCaudalAndObservacionesCorrectas(muestras) && !isAnalisisConCaudal(muestras) &&
                   isFechasCorrecta(muestras) && !isMuestraConParmetrosCapturados(muestras) &&
                   !validaLCMCorrecto(muestras)
                   && !validaMinMaxAceptado(muestras)
                   && !isVolumenMuestraCompuestaCorrecto(muestras) && !isFechaRecepcionCorrecta(muestras) &&
                   !isLaboratorioCapturado(muestras) && !isMetodoCapturado(muestras)
                   && !isFechaConformacionCorrecta(muestras) && !isRecepcionAnteriorConformacion(muestras)
                   && (!Analisis_.OrdenTrabajo.Norma.CPD || !isLMPInferiorCorrecto(muestras) &&
                       !isLMPSuperiorCorrecto(muestras) && !isLMPcorrecto(muestras));
        }


        //Validar que si el caudal es 0 se han capturado las observaciones. Se valida si regresa false.
        private bool isCaudalAndObservacionesCorrectas(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.Caudal == 0 && muestra.Observaciones == "");
        }

        //Valida que no se capturen todos las muestras con caudal cero y observaciones. Se valida si regresa false.
        private bool isAnalisisConCaudal(List<Muestra> muestras)
        {
            return muestras.Count(muestra => muestra.Caudal == 0 && muestra.Observaciones != "") == muestras.Count;
        }

        //Validar que todas las fechas son posteriores o iguales.
        private bool isFechasCorrecta(List<Muestra> muestras)
        {
            for (var i = 0; i < muestras.Count - 1; i++)
            {
                for (var j = i + 1; j < muestras.Count; j++)
                {
                    if (muestras[i].Fecha >= muestras[j].Fecha)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Validar que no hay parámetros menores que 0 o ''.
        private bool isMuestraConParmetrosCapturados(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.Caudal > 0 && muestra.MuestraValors.Any(parametro =>
                (parametro.Valor < 0 || parametro.Valor == null) && !parametro.Exento && !parametro.NoAnalizado));
        }

        //Validar LCM
        private bool validaLCMCorrecto(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.Caudal > 0 && muestra.MuestraValors.Any(parametro =>
                parametro.LC_MC != null && parametro.LC_MC.Value >= 0 && parametro.Valor < parametro.LC_MC.Value &&
                !parametro.NoAnalizado && !parametro.Exento));
        }


        //Validar volumen muestra compuesta.
        private bool isVolumenMuestraCompuestaCorrecto(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.ValMuestraC != null && (muestra.ValMuestraC.Value <= 0));
        }


        //Validar fecha y hora de ingreso al laboratorio.
        private bool isFechaRecepcionCorrecta(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.FechaRecepcion < muestra.Fecha);
        }

        //Valida que la fecha de ingreso al laboratorio no sea anterior a la fecha de conformación de la muestra compuesta.
        private bool isFechaConformacionCorrecta(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.FechaConformacion < muestra.Fecha);
        }

        private bool isRecepcionAnteriorConformacion(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.FechaRecepcion < muestra.FechaConformacion);
        }


        //Validar que todos los parámetros tienen capturados método.
        private bool isLaboratorioCapturado(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.MuestraValors.Any(mv =>
                mv.LaboratorioID == null && !mv.Exento && !mv.NoAnalizado));
        }

        //Validar que todos los parámetros tienen capturados laboratorio.
        private bool isMetodoCapturado(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.MuestraValors.Any(mv =>
                mv.ParametroMetodoID == null && !mv.Exento && !mv.NoAnalizado));
        }


        //Valida LMP para las cpd's => 
        private bool isLMPSuperiorCorrecto(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.MuestraValors.Any(mv =>
                    mv.AceptaValor && !mv.NoAnalizado && !mv.Exento &&
                    (mv.LimiteSuperior < 0 || mv.LimiteSuperior == null)))
                ;
        }

        private bool isLMPInferiorCorrecto(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.MuestraValors.Any(mv =>
                mv.AceptaValor && !mv.NoAnalizado && !mv.Exento &&
                (mv.LimiteInferior < 0 || mv.LimiteInferior == null) && mv.Parametro.ParametroID ==
                Analisis_.OrdenTrabajo.Norma.NormaConfiguracions
                    .FirstOrDefault(nc => nc.ParametroNombre.Contains("Potencial Hidrógeno ") && nc.Parametro.Activo)
                    ?.Parametro.ParametroID)); //Solo el ph tiene límite inferior.
        }

        private bool isLMPcorrecto(List<Muestra> muestras)
        {
            return muestras.Any(muestra => muestra.MuestraValors.Any(mv =>
                mv.AceptaValor && !mv.NoAnalizado && !mv.Exento && mv.LimiteInferior > mv.LimiteSuperior &&
                mv.Parametro.ParametroID == Analisis_.OrdenTrabajo.Norma.NormaConfiguracions
                    .FirstOrDefault(nc => nc.ParametroNombre.Contains("Potencial Hidrógeno ") && nc.Parametro.Activo)
                    ?.Parametro.ParametroID));
        }

        private bool existeMuestraCompuesta()
        {
            return Analisis_.Muestras.Any(m => m.NivelID == enumNivel.Compuesta.ConvertToInt());
        }

        private bool validaMinMaxAceptado(List<Muestra> muestras)
        {
            return muestras.Any(m => m.MuestraValors.Any(mv =>
                mv.AceptaValor && mv.LimitaCaptura == true &&
                !mv.NoAnalizado && !mv.Exento &&
                ((mv.MinAceptado != null && mv.Valor < mv.MinAceptado) || (mv.MaxAceptado != null && mv.Valor > mv.MaxAceptado))));
        }
        #region RegionConductividad
        protected void wucConductividadElectrica_CheckEcoliEnterococos(object sender, Dictionary<bool, string> valuePairs)
        {
            if (valuePairs.First().Key && valuePairs.First().Value != "SinFecha")
            {
                var valor = valuePairs.First().Value == "Escherichia" ? "Escherichia Coli" : "Enterococos Fecales";
                var valorAnterior = valor == "Escherichia Coli" ? "Enterococos Fecales" : "Escherichia Coli";
                var mensaje = "Se observa que en la muestra simple anterior reportó " + valorAnterior +
                    " y actualmente se está reportando " + valor + ", lo cual afectará la aplicación por parte de su " +
                    "cliente del beneficio fiscal a que se refiere el artículo 282 fracción I de la Ley Federal de Derechos.";
                this.PopMensaje(mensaje, "Muestras de análisis", controles.Opciones.SoloAceptar, controles.Icono.warning);
            }
            CargaInformacion(this.Analisis_.AnalisisID);
            //upGrid.Update();
            //upGrid.DataBind();
        }


        public Dictionary<bool, string> ValidaEColi()
        {
            Siralab_Entities siralab_Entities = new Siralab_Entities();
            var valores = new Dictionary<bool, string>();
            Analisis_BL analisisBLL = new Analisis_BL();
            var an = analisisBLL.Obten_Analisis(AnalisisID);

            var fechaConformacion = an.Muestras.First().FechaConformacion.Value;
            int trimestreMuestra = (int)Math.Ceiling((double)fechaConformacion.Month / 3);// validar que fecha poner 
            var query = from ot in siralab_Entities.OrdenTrabajoes
                        join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                        join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                        join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                        where ot.NormaID == 15 && ot.ClienteID == an.OrdenTrabajo.ClienteID
                        && m.FechaConformacion.Value.Year == fechaConformacion.Year // validar que fecha poner
                        && _a.Conductividad != null
                        && _a.EstatusID != 4 && ot.ConsecionID == an.OrdenTrabajo.ConsecionID
                        && ot.AnexoID == this.Analisis_.OrdenTrabajo.AnexoID
                        orderby _a.AnalisisID descending
                        select new
                        {
                            _a.AnalisisID,
                            Fecha = m.FechaConformacion,// validar que fecha poner 
                            Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),// validar que fecha poner 
                            _a.Conductividad
                        };

            if (query != null)
            {
                var prueba = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != this.AnalisisID).FirstOrDefault();
                if (prueba != null)
                {
                    valores.Add(prueba.Conductividad == "Enterococos", "Escherichia");

                }
                else
                {
                    valores.Add(false, "");

                }
            }
            else
            {
                valores.Add(false, "");

            }

            return valores;
        }
        public Dictionary<bool, string> ValidaEnterococos()
        {
            Siralab_Entities siralab_Entities = new Siralab_Entities();
            var valores = new Dictionary<bool, string>();
            Analisis_BL analisisBLL = new Analisis_BL();
            var an = analisisBLL.Obten_Analisis(AnalisisID);

            var fechaConformacion = an.Muestras.First().FechaConformacion.Value;
            int trimestreMuestra = (int)Math.Ceiling((double)fechaConformacion.Month / 3);// validar que fecha poner 
            var query = from ot in siralab_Entities.OrdenTrabajoes
                        join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                        join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                        join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                        where ot.NormaID == 15 && ot.ClienteID == an.OrdenTrabajo.ClienteID
                        && m.FechaConformacion.Value.Year == fechaConformacion.Year // validar que fecha poner
                        && _a.Conductividad != null
                        && _a.EstatusID != 4 && ot.ConsecionID == an.OrdenTrabajo.ConsecionID
                        && ot.AnexoID == an.OrdenTrabajo.AnexoID
                        orderby _a.AnalisisID descending
                        select new
                        {
                            _a.AnalisisID,
                            Fecha = m.FechaConformacion,// validar que fecha poner 
                            Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),// validar que fecha poner 
                            _a.Conductividad
                        };

            if (query != null)
            {
                var prueba = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != this.AnalisisID).FirstOrDefault();
                if (prueba != null)
                {
                    valores.Add(prueba.Conductividad == "Escherichia", "Enterococos");

                }
                else
                {
                    valores.Add(false, "");

                }
            }
            else
            {
                valores.Add(false, "");

            }

            return valores;
        }
        #endregion

    }
}