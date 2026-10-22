using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.common;
using SIRALAB_BL;
using SIRALAB_DAL;
using AjaxControlToolkit;
using System.ComponentModel;
using SIRALAB_BL.catalogos;

namespace SIRALAB.Controles
{
    public partial class wucMuestraDatos : System.Web.UI.UserControl
    {
        #region Propiedades
        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_"]; }
        }
        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }
        public Laboratorio LaboratorioDatos
        {
            get { return (Laboratorio)Session["LaboratorioDatos_"]; }
        }
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        public LaboratorioVigencia Vigencia
        {
            get { return Session["LaboratorioVigencia"] == null ? null : (LaboratorioVigencia)Session["LaboratorioVigencia"]; }
        }
        public LaboratorioSignatario Signatario_Laboratorio_Persona
        {
            get { return (LaboratorioSignatario)Session["Signatario_Laboratorio_Persona_"]; }
        }
        public bool EsCompuesta
        {
            get { return (bool)Session["EsCompuesta_"]; }
        }
        public int NivelID
        {
            get { return hfNivel.Value.Trim() == "" ? 0 : int.Parse(hfNivel.Value); }
            set { hfNivel.Value = value.ToString(); }
        }
        #endregion

        #region Eventos desencadenados por el control
        [Browsable(true)]
        public event EventHandler<MuestraCapturaArgs> Guardar;
        [Browsable(true)]
        public event EventHandler<bool> TipoMuestra;
        [Browsable(true)]
        public event EventHandler<bool> DatosIncompletos;
        [Browsable(true)]
        public event EventHandler<bool> Simple_DatosIncompletos;
        [Browsable(true)]
        public event EventHandler<bool> Compuesta_DatosIncompletos;
        [Browsable(true)]
        public event EventHandler<bool> GuardaParametros;
        [Browsable(true)]
        public event EventHandler<bool> RecalculaCompuesta;
        [Browsable(true)] 
        public event EventHandler<bool> GuardadoSimple; 
        [Browsable(true)] 
        public event EventHandler<Dictionary<bool, string>> CheckDQOCOT; 
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            wucParametros1.CheckDQOCOT += wucParametros1_CheckDQOCOT;
            
            if (!IsPostBack)
            {
                DateTime? initMuestreo = null;
                DateTime endMuestreo = DateTime.Now;

                DateTime? initRec = null;
                DateTime endRec = DateTime.Now;
                if (Vigencia != null)
                {
                    initMuestreo = Vigencia.FechaInicio;
                    initRec = Vigencia.FechaInicio;
                }
                startTxtOrdenFechaMuestreo.Value = initMuestreo != null ? initMuestreo.Value.ToShortDateString() : string.Empty;
                startTxtMuestraFechaRec.Value = initRec != null ? initRec.Value.ToShortDateString() : string.Empty;
                endTxtOrdenFechaMuestreo.Value = endMuestreo.ToShortDateString();
                endTxtMuestraFechaRec.Value = endRec.ToShortDateString();

                if (this.EsCompuesta)
                {
                    this.NivelID = 2;
                    lbl.InnerText = "Hora:";
                    lblFechaMuestreo.InnerText = "Fecha de conformación de la muestra";
                    //lnkbtnRecalcular.Visible = true;
                    lnkbtnRecalcular.Visible = false;
                    TipoMuestra(this, true);

                    txtMuestraIdentificacion.Enabled = false;
                    txtMuestraIdentificacion.Text = "N/A";
                    txtMuestraDescripcion.Enabled = false;
                    txtMuestraDescripcion.Text = "N/A";
                }
                else
                {
                    this.NivelID = 1;
                    TipoMuestra(this, false);
                }
                #region Norma SST DQO/COT
                ApplicationSettings applicationSettings = new ApplicationSettings();
                if (this.OrdenTrabajo.Norma.Nombre.Replace(" ","") == applicationSettings.NormaCOT 
                    || this.OrdenTrabajo.Norma.NormaID == 15 
                    || this.OrdenTrabajo.Norma.Nombre.ToUpper().Replace(" ", "") == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                {
                    if (chkCOT.Checked)
                        ValidaCOT();
                    else if (chkDQO.Checked)
                        ValidaDQO();
                }
                else
                {
                    divDatosDQOCOT.Visible = false;
                }
                #endregion
            }
        }

        protected void rblOrdenLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lnkbtnOrdenGuardar_Click(object sender, EventArgs e)
        {
            MuestraCapturaArgs muestraArgs = new MuestraCapturaArgs();

            if ((this.LaboratorioDatos != null && this.Signatario_Laboratorio_Persona != null) || this.EsCompuesta)
            {
                //Se obtienen los argumentos y se guardan o actualizan en bd
                muestraArgs = GenerarArgumentos();

                //if (this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse((txtMuestraCaudal.Text)) <= 0)
                //{
                //    muestraArgs.Descripcion = "CaudalCero";
                //}

                //if (muestraArgs.Descripcion != "ErrorFecha" && muestraArgs.Descripcion != "CaudalCero")
                if (muestraArgs.Descripcion != "ErrorFecha")
                {
                    Muestra_BL muestra_bl = new Muestra_BL();
                    var muestra = muestra_bl.Actualiza(muestraArgs);

                    //TODO: Actualizar los calculos para la muestra compuesta
                    muestra_bl.RecalculaSimple(muestra.MuestraID);
                    //TODO: Actualizar los calculos para la muestra compuesta


                    //se generan los argumentos de muestreado y se guardan en bd
                    Muestreador_BL muestreador_bl = new Muestreador_BL();
                    MuestreadorCapturaArgs muestreadorArgs = new MuestreadorCapturaArgs();
                    muestreadorArgs.MuestraID = muestra.MuestraID;

                    var muestreador = muestreador_bl.Obten_Muestra_Muestra(muestra.MuestraID);

                    //Se comprueba si el registro ya existia
                    if (muestreador != null)
                        muestreadorArgs.MuestreadorID = muestreador.MuestreadorID;

                    //En caso que se trate de una muestra simple, insertará en esta tabla, en caso de que se trate de una muestra compuesta
                    //no insertara ningun registro en la tabla
                    if (this.EsCompuesta == false)
                    {
                        muestreadorArgs.LaboratorioSignatarioID = this.Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreador_bl.Guarda(muestreadorArgs);
                    }

                    if (this.OrdenTrabajo.Norma.NormaConfiguracions.Count > 0)
                    {
                        this.NivelID = this.Muestra.NivelID;

                        //*********************************************************
                        //En esta parte se mandará la variable si corresponde a una muestra simple o corresponde a una muestra compuesta                    
                        //*********************************************************
                        //Se agrega validación del valor del caudal, en caso que sea cero, no se debe registrar ningun muestra valor.
                        if (this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse((txtMuestraCaudal.Text)) > 0)
                        {
                            wucParametros1.CargaInicialParametros((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID, this.NivelID, this.EsCompuesta);
                        }
                        else if (!this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal)
                        {
                            wucParametros1.CargaInicialParametros((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID, this.NivelID, this.EsCompuesta);
                        }
                        else if (this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse((txtMuestraCaudal.Text)) <= 0)
                        {
                            muestra_bl.EliminarMuestraValor(this.Muestra.MuestraID);
                            wucParametros1.DisableParametros();
                        }

                        //TODO: verifica que la compuesta exista y no sean ceros
                        muestra_bl.EliminarMuestraCompuesta(muestra.MuestraID);
                        //TODO: verifica que la compuesta exista y no sean ceros

                        Guardar(this, muestraArgs);
                    }
                    else
                    {
                        muestraArgs.Descripcion = "SinParametros";
                        Guardar(this, muestraArgs);
                    }
                }
                else
                {
                    if (muestraArgs.Descripcion == "ErrorFecha")
                    {
                        muestraArgs.Descripcion = "ErrorFecha";
                    }
                    else if (muestraArgs.Descripcion == "CaudalCero")
                    {
                        muestraArgs.Descripcion = "CaudalCero";
                    }
                    Guardar(this, muestraArgs);
                }
            }
            else
            {
                muestraArgs.Descripcion = "SinMuestreador";
                Guardar(this, muestraArgs);
            }
            //Response.Redirect("AnalisisParametro.aspx");
        }

        protected void lnkbtnBackAnalisi_Click(object sender, EventArgs e)
        {


            //////

            //var muestraArgs = new MuestraCapturaArgs();

            //if ((LaboratorioDatos != null && Signatario_Laboratorio_Persona != null) || EsCompuesta)
            //{
            //Se obtienen los argumentos y se guardan o actualizan en bd
            //    muestraArgs = GenerarArgumentos();

            //if (this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse((txtMuestraCaudal.Text)) <= 0)
            //{
            //    muestraArgs.Descripcion = "CaudalCero";
            //}

            //if (muestraArgs.Descripcion != "ErrorFecha" && muestraArgs.Descripcion != "CaudalCero")
            //    if (muestraArgs.Descripcion != "ErrorFecha")
            //    {
            //        var muestraBl = new Muestra_BL();
            //        var muestra = muestraBl.Actualiza(muestraArgs);

            //TODO: Actualizar los calculos para la muestra compuesta
            //        muestraBl.RecalculaSimple(muestra.MuestraID);
            //TODO: Actualizar los calculos para la muestra compuesta


            //se generan los argumentos de muestreado y se guardan en bd
            //        var muestreadorBl = new Muestreador_BL();
            //        var muestreadorArgs = new MuestreadorCapturaArgs { MuestraID = muestra.MuestraID };

            //        var muestreador = muestreadorBl.Obten_Muestra_Muestra(muestra.MuestraID);

            //Se comprueba si el registro ya existia
            //        if (muestreador != null)
            //            muestreadorArgs.MuestreadorID = muestreador.MuestreadorID;

            //En caso que se trate de una muestra simple, insertará en esta tabla, en caso de que se trate de una muestra compuesta
            //no insertara ningun registro en la tabla
            //        if (EsCompuesta == false)
            //        {
            //            if (Signatario_Laboratorio_Persona != null)
            //                muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
            //            muestreadorBl.Guarda(muestreadorArgs);
            //        }

            //        if (OrdenTrabajo.Norma.NormaConfiguracions.Count > 0)
            //        {
            //            NivelID = Muestra.NivelID;

            //*********************************************************
            //En esta parte se mandará la variable si corresponde a una muestra simple o corresponde a una muestra compuesta                    
            //*********************************************************
            //Se agrega validación del valor del caudal, en caso que sea cero, no se debe registrar ningun muestra valor.
            //            if (Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse((txtMuestraCaudal.Text)) > 0)
            //            {
            //                if (OrdenTrabajo.NormaID != null)
            //                    wucParametros1.CargaInicialParametros((int)OrdenTrabajo.NormaID, Muestra.MuestraID, NivelID, EsCompuesta);
            //            }
            //            else if (!Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal)
            //            {
            //                if (OrdenTrabajo.NormaID != null)
            //                    wucParametros1.CargaInicialParametros((int)OrdenTrabajo.NormaID, Muestra.MuestraID, NivelID, EsCompuesta);
            //            }
            //            else if (Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse((txtMuestraCaudal.Text)) <= 0)
            //            {
            //                muestraBl.EliminarMuestraValor(Muestra.MuestraID);
            //                wucParametros1.DisableParametros();
            //            }

            //TODO: verifica que la compuesta exista y no sean ceros
            //   muestraBl.EliminarMuestraCompuesta(muestra.MuestraID);
            //TODO: verifica que la compuesta exista y no sean ceros
            //            var obternerMuestra = new Muestra_BL();
            //            var muestraAlmacenada = obternerMuestra.Obten_Muestra(muestra.MuestraID);
            //            muestraAlmacenada.FlagCompuesta = true;
            //            obternerMuestra.ActualizaMuestra(muestraAlmacenada);
            //            obternerMuestra = null;
            //            muestraAlmacenada = null;

            //            Guardar(this, muestraArgs);


            //            muestraArgs.Descripcion = "SinParametros";
            //            Guardar(this, muestraArgs);
            //        }
            //    }
            //    else
            //    {
            //        switch (muestraArgs.Descripcion)
            //        {
            //            case "ErrorFecha":
            //                muestraArgs.Descripcion = "ErrorFecha";
            //                break;
            //            case "CaudalCero":
            //                muestraArgs.Descripcion = "CaudalCero";
            //                break;
            //        }
            //        Guardar(this, muestraArgs);
            //    }
            //}
            //else
            //{
            //    muestraArgs.Descripcion = "SinMuestreador";
            //    Guardar(this, muestraArgs);
            //}
            Response.Redirect("~/Analisis.aspx");
            //////
        }

        protected void lnkbtnRecalcular_Click(object sender, EventArgs e)
        {
            if (RecalculaCompuesta != null)
            {
                Muestra_BL muestra_bl = new Muestra_BL();
                var respuesta = muestra_bl.Recalcula((int)this.Muestra.Analisi.OrdenTrabajo.NormaID, this.Muestra.MuestraID);
                wucParametros1.CargaInicialParametros((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID, this.NivelID, this.EsCompuesta);

                RecalculaCompuesta(this, respuesta);
            }
        }

        protected void wucParametros1_DatosFaltantes(object sender, bool e)
        {
            Simple_DatosIncompletos(this, true);
        }

        protected void wucParametros1_Guardado(object sender, bool e)
        {
            if (GuardaParametros != null)
                GuardaParametros(sender, e);
        }

        protected void wucParametros1_GuardadoSimple(object sender, bool e)
        {
            GuardadoSimple?.Invoke(sender, e);
        }

        protected void wucParametros1_CheckDQOCOT(object sender, Dictionary<bool, string> e)
        {
            CheckDQOCOT?.Invoke(sender, e);
        }

        public void CargaDatos()
        {
            if (this.EsCompuesta == true) //TODO refactorizar:
            {
                lbl.InnerText = "Hora";
                lblFechaMuestreo.InnerText = "Fecha conformación";
                txtMuestraCaudal.Enabled = false;

                //RFD 05/08/20 -> Deshabilita los datepickers de la muestra compuesta.
                var activaConformacion = this.Muestra.FechaConformacion == null && this.Muestra.Fecha == null;
                var activaRecepcion = this.Muestra.FechaRecepcion == null && this.Muestra.Fecha == null;

                txtMuestraFechaMuestreo.Enabled = activaConformacion;
                txtMuestraHora.Enabled = activaConformacion;
                txtMuestraFechaRec.Enabled = activaRecepcion;
                txtMuestraHoraRec.Enabled = activaRecepcion;

            }


            //RFD 05/08/2020 -> se modifica la asignación de la fecha y hora de conformación/recepción de la muestra para que tome en cuenta el nuevo campo.
            txtMuestraFechaMuestreo.Text = this.Muestra.FechaConformacion != null ? DateTime.Parse(this.Muestra.FechaConformacion.ToString()).ToShortDateString() : this.Muestra.Fecha != null ? DateTime.Parse(this.Muestra.Fecha.ToString()).ToShortDateString() : "";
            txtMuestraHora.Text = this.Muestra.FechaConformacion != null ? DateTime.Parse(this.Muestra.FechaConformacion.ToString()).ToString("HH:mm") : this.Muestra.Fecha != null ? DateTime.Parse(this.Muestra.Fecha.ToString()).ToString("HH:mm") : "";
            txtMuestraDescripcion.Text = this.Muestra.Descripcion;
            txtMuestraIdentificacion.Text = this.Muestra.Identificacion;
            txtMuestraFechaRec.Text = this.Muestra.FechaRecepcion != null ? DateTime.Parse(this.Muestra.FechaRecepcion.ToString()).ToShortDateString() : ""; //this.Muestra.FechaRecepcion.ToShortDateString();
            txtMuestraHoraRec.Text = this.Muestra.FechaRecepcion != null ? DateTime.Parse(this.Muestra.FechaRecepcion.ToString()).ToString("HH:mm") : ""; //this.Muestra.FechaRecepcion.ToString("hh:mm");
            txtMuestraCaudal.Text = this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal ? this.Muestra.Caudal.ToString("######.##") : "";

            txtMuestraCaudal.Enabled = this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal;
            txtMuestraCaudal.Enabled = this.Muestra.NivelID == enumNivel.Simple.ConvertToInt();
            rfv_txtMuestraCaudal.Enabled = this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal;



            txtMuestraObservaciones.Text = this.Muestra.Observaciones;
            Label4.InnerText = this.Muestra.NivelID == enumNivel.Simple.ConvertToInt() ? "Caudal (l/s) *" : "Caudal promedio (l/s)";

            if (this.Muestra.MuestraValors.Count > 0)
            {
                Analisis_BL analisis_BL = new Analisis_BL();
                var analisis = analisis_BL.Obten_Analisis(this.Muestra.AnalisisID);
                if (analisis.NormaDQOCOT != null && analisis.NormaDQOCOT == "COT")
                    chkCOT.Checked = true;
                else if (analisis.NormaDQOCOT != null && analisis.NormaDQOCOT == "DQO")
                    chkDQO.Checked = true;

                wucParametros1.CargaParametros();
            }
        }

        public MuestraCapturaArgs GenerarArgumentos()
        {
            MuestraCapturaArgs muestraArgs = new MuestraCapturaArgs();

            muestraArgs.MuestraID = this.Muestra.MuestraID;

            muestraArgs.Descripcion = txtMuestraDescripcion.Text;
            muestraArgs.Identificacion = txtMuestraIdentificacion.Text;
            muestraArgs.Caudal = decimal.Parse(this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal ? txtMuestraCaudal.Text : "-1");

            //Validacion sobre una muestra simple o una muestra compues
            if (ValidarFecha(txtMuestraFechaMuestreo.Text) || ValidarFecha(txtMuestraFechaRec.Text))
            {
                muestraArgs.Descripcion = "ErrorFecha";
            }

            string FechaMuestra = txtMuestraFechaMuestreo.Text;
            string HoraMuestra = txtMuestraHora.Text;
            string FechaHoraMuestra = FechaMuestra + " " + HoraMuestra;

            muestraArgs.Fecha = DateTime.ParseExact(FechaHoraMuestra, "dd/MM/yyyy H:mm", System.Globalization.CultureInfo.InvariantCulture);
            muestraArgs.FechaConformacion = DateTime.ParseExact(FechaHoraMuestra, "dd/MM/yyyy H:mm", System.Globalization.CultureInfo.InvariantCulture);

            string FechaEntrega = txtMuestraFechaRec.Text;
            string HoraEntrega = txtMuestraHoraRec.Text;
            string FechaHoraEntrega = FechaEntrega + " " + HoraEntrega;

            muestraArgs.FechaRecepcion = DateTime.ParseExact(FechaHoraEntrega, "dd/MM/yyyy H:mm", System.Globalization.CultureInfo.InvariantCulture);
            muestraArgs.Observaciones = txtMuestraObservaciones.Text;

            //Estos datos solo se mandaran para completar los argumentos del objeto
            muestraArgs.AnalisisID = this.Muestra.AnalisisID;
            muestraArgs.Datum = this.Muestra.Datum;
            muestraArgs.DireccionID = this.Muestra.DireccionID;
            muestraArgs.Numero = this.Muestra.Numero;
            muestraArgs.Latitud = this.Muestra.Latitud;
            muestraArgs.Longitud = this.Muestra.Longitud;
            muestraArgs.DireccionID = this.Muestra.DireccionID;
            muestraArgs.Cancelado = this.Muestra.Analisi.OrdenTrabajo.Norma.RequiereCaudal && decimal.Parse(txtMuestraCaudal.Text) == decimal.Parse("0");

            muestraArgs.NivelID = this.NivelID;
            //Fin del complemento
            return muestraArgs;
        }

        public bool ValidarFecha(string FechaXValidarCadena)
        {
            bool Validacion = true;
            DateTime FechaActual = DateTime.Now;
            DateTime FechaXValidar = DateTime.Now;
            if (DateTime.TryParse(FechaXValidarCadena, out FechaXValidar))
            {
                if (FechaXValidar.Date <= FechaActual.Date && FechaXValidar.Year >= 2014)
                {
                    Validacion = false;
                }
            }
            return Validacion;
        }

        protected void chkCOT_CheckedChanged(object sender, EventArgs e)
        {
            Dictionary<bool, string> valores;
            if (chkCOT.Checked)
            {
                chkDQO.Checked = false;
                wucParametros1.ChkCOT(chkCOT.Checked);
                valores = ValidaCOT();
                CheckDQOCOT(this, valores);
            }
            else if (this.OrdenTrabajo.Analisis.Any())
            {
                chkDQO.Checked = true;
                wucParametros1.ChkDQO(chkDQO.Checked);
                valores = ValidaDQO();
                CheckDQOCOT(this, valores);
            }
            else
            {
                wucParametros1.ChkCOT(chkCOT.Checked);
            }
        }

        protected void chkDQO_CheckedChanged(object sender, EventArgs e)
        {
            Dictionary<bool, string> valores;
            if (chkDQO.Checked)
            {
                chkCOT.Checked = false;
                wucParametros1.ChkDQO(chkDQO.Checked);
                valores = ValidaDQO();
                CheckDQOCOT(this, valores);
            } 
            else if (this.OrdenTrabajo.Analisis.Any())
            {
                chkCOT.Checked = true;
                wucParametros1.ChkCOT(chkCOT.Checked);
                valores = ValidaCOT();
                CheckDQOCOT(this, valores);
            }
            else
            {
                wucParametros1.ChkDQO(chkDQO.Checked);
            }
        }

        public Dictionary<bool, string> ValidaCOT()
        {
            Siralab_Entities siralab_Entities = new Siralab_Entities();
            int trimestreMuestra = (int)Math.Ceiling((double)this.Muestra.FechaConformacion.Value.Month / 3);
            var query = from ot in siralab_Entities.OrdenTrabajoes
                        join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                        join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                        join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                        where ot.NormaID == this.OrdenTrabajo.NormaID && ot.ClienteID == this.OrdenTrabajo.ClienteID 
                        && m.FechaConformacion.Value.Year == this.Muestra.FechaConformacion.Value.Year && _a.NormaDQOCOT != null
                        && _a.EstatusID != 4 && ot.ConsecionID == this.OrdenTrabajo.ConsecionID
                        && ot.AnexoID == this.OrdenTrabajo.AnexoID
                        orderby _a.AnalisisID descending
                        select new
                        {
                            _a.AnalisisID,
                            Fecha = m.FechaConformacion,
                            Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),
                            _a.NormaDQOCOT
                        };
            var valores = new Dictionary<bool, string>();
            if (query != null)
            {
                var prueba = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != this.Muestra.AnalisisID).FirstOrDefault();
                if (prueba != null)
                {
                    valores.Add(prueba.NormaDQOCOT == "DQO", "COT");
                    hfDQOCOT.Value = valores.First().Key ? "DQO" : "COT";
                }
                else
                {
                    valores.Add(false, "");
                    hfDQOCOT.Value = "COT";
                }
            }
            else
            {
                valores.Add(false, "");
                hfDQOCOT.Value = "COT";
            }

            return valores;
        }

        public Dictionary<bool, string> ValidaDQO()
        {
            Siralab_Entities siralab_Entities = new Siralab_Entities();
            int trimestreMuestra = (int)Math.Ceiling((double)this.Muestra.FechaConformacion.Value.Month / 3);
            var query = from ot in siralab_Entities.OrdenTrabajoes
                        join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                        join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                        join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                        where ot.NormaID == this.OrdenTrabajo.NormaID && ot.ClienteID == this.OrdenTrabajo.ClienteID 
                        && m.FechaConformacion.Value.Year == this.Muestra.FechaConformacion.Value.Year && _a.NormaDQOCOT != null
                        && _a.EstatusID != 4 && ot.ConsecionID == this.OrdenTrabajo.ConsecionID
                        && ot.AnexoID == this.OrdenTrabajo.AnexoID
                        orderby _a.AnalisisID descending
                        select new
                        {
                            _a.AnalisisID,
                            Fecha = m.FechaConformacion,
                            Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),
                            _a.NormaDQOCOT
                        };
            var valores = new Dictionary<bool, string>();
            if (query != null)
            {
                var prueba = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != this.Muestra.AnalisisID).FirstOrDefault();
                if (prueba != null)
                {
                    valores.Add(prueba.NormaDQOCOT == "COT", "DQO");
                    hfDQOCOT.Value = valores.First().Key ? "COT" : "DQO";
                }
                else
                {
                    valores.Add(false, "");
                    hfDQOCOT.Value = "DQO";
                }
            }
            else
            {
                valores.Add(false, "");
                hfDQOCOT.Value = "DQO";
            }

            return valores;
        }
    }
}