using SIRALAB_BL;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;

namespace SIRALAB.Controles
{
    public partial class wucMuestraSimple : System.Web.UI.UserControl
    {
        #region Propiedades

        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
            set { Session["MuestraMuestra_"] = value; }
        }

        public SIRALAB_DAL.Analisi Analisis
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }

        public bool EsCompuesta
        {
            get { return (bool)Session["EsCompuesta_"]; }
            set { Session["EsCompuesta_"] = value; }
        }

        public Laboratorio LaboratorioDatos
        {
            get { return (Laboratorio)Session["LaboratorioDatos_"]; }
            set { Session["LaboratorioDatos_"] = value; }
        }

        public LaboratorioSignatario Signatario_Laboratorio_Persona
        {
            get { return (LaboratorioSignatario)Session["Signatario_Laboratorio_Persona_"]; }
            set { Session["Signatario_Laboratorio_Persona_"] = value; }
        }

        #endregion

        #region Eventos

        [Browsable(true)]
        public event EventHandler<MuestraCapturaArgs> Guardar;

        [Browsable(true)]
        public event EventHandler Evidencia;

        [Browsable(true)]
        public event EventHandler<bool> MSIncompleta;
       
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void CargaInformacion(int AnalisisID)
        {

            List<MuestraLista> muestra_lista = new List<MuestraLista>();
            Muestra_BL muestra_bl = new Muestra_BL();
            var lista_muestra = muestra_bl.Lista_Muestra(AnalisisID).Where(x => x.NivelID == 1).ToList();

            //Verificamos si ya existe una muestra compuesta, en caso de que ya exista una muestra compuesta, ponemos invisible el boton de agregar muestra compuesta
            var MuestraCompuesta = muestra_bl.Lista_Muestra(AnalisisID).Where(x => x.NivelID == 2).ToList();
            if (MuestraCompuesta.Count > 0 || !this.Analisis.OrdenTrabajo.Norma.RequiereCompuesta)
            {
                /* Oculta el boton de generar compuesta sí ya existe una compuesta o si no se requiere de la compuesta */
                lnkbtnMuestraCompuesta.Visible = false;
            }

            int cont = 1;
            foreach (var i in lista_muestra)
            {
                muestra_lista.Add(new MuestraLista
                {
                    MuestraID_ = i.MuestraID,
                    Fecha_ = i.Fecha != null ? DateTime.Parse(i.Fecha.ToString()).ToShortDateString() : "",
                    Hora_ = i.Fecha != null ? DateTime.Parse(i.Fecha.ToString()).ToShortTimeString() : "",
                    Observaciones_ = i.Observaciones,
                    Consecutivo_ = cont
                });
                cont++;
            }

            pnlMuestraSimple.Visible = muestra_lista.Count > 0;
            rptTitulo.DataSource = muestra_lista;
            rptTitulo.DataBind();

        }

        protected void lnkbtnSeleccionar_Click(object sender, EventArgs e)
        {
            int MuestraID = int.Parse(((LinkButton)sender).CommandArgument);
            if (MuestraID > 0)
            {
                this.LaboratorioDatos = null;
                this.Signatario_Laboratorio_Persona = null;

                this.EsCompuesta = false;
                Muestra_BL muestra_bl = new Muestra_BL();
                this.Muestra = muestra_bl.Obten_Muestra(MuestraID);
                Response.Redirect("~/AnalisisParametro.aspx");
            }
        }

        protected void lnkbtnMuestraCompuesta_Click(object sender, EventArgs e)
        {
            bool Avanza_Compuesta = false;
            decimal CompuestaCaudal = 0;
            //Metodo para generar la muestra compuesta
            Muestra_BL muestra_bl = new Muestra_BL();
            Norma_BL ncBL = new Norma_BL();
            MuestraCapturaArgs muestraArgs = new MuestraCapturaArgs();
            Muestra m = new Muestra();

            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

            if (this.Analisis != null)
            {
                //Se realiza la validación para no permitir que se genere la muestra compuesta hasta que esten completas las muestars simples
                var lista_muestra = muestra_bl.Lista_Muestra(this.Analisis.AnalisisID);

                if (lista_muestra.Where(p => p.Caudal != 0).Count() > 0)
                {
                    CompuestaCaudal = lista_muestra.Where(p => p.Caudal != 0).Average(x => x.Caudal);
                }
                //Validación utilizada para comprobar que la lista tiene registros
                if (lista_muestra.Count > 0)
                {
                    foreach (var muestra in lista_muestra)
                    {
                        var lista_valores = muestravalor_bl.Lista_MuestraValor(muestra.MuestraID);
                        var lista_valoresConfig = ncBL.Obtener((int)muestra.Analisi.OrdenTrabajo.NormaID);

                        //Validación utilizada para comprobar que existen registros en muestra valor
                        if (lista_valores.Count > 0 && lista_valoresConfig.NormaConfiguracions.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()).Count() > 0)
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
                            if (!this.Analisis.OrdenTrabajo.Norma.RequiereCaudal && muestra.Caudal == -1)
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
                                    Avanza_Compuesta = lista_valoresConfig.NormaConfiguracions.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()).Count() == 0;
                                    break;
                                }
                            }

                        }
                    }
                }

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
                

                //SE REALIZA UN CICLO CON EL NUMERO DE MUESTRAS CONFIGURADO EN LA ORDEN DE TRABAJO
                if (Avanza_Compuesta)
                {
                    //INICIO Funcionalidad para modificar los parametros, no analizado y exento
                    var lista_MuestraValor = muestravalor_bl.Lista_MuestraValor_Analisis(this.Analisis.AnalisisID);

                    foreach (var val in lista_MuestraValor)
                    {
                        var lista_Parametro_NoAnalizado = lista_MuestraValor.Where(x => x.ParametroID == val.ParametroID && x.NoAnalizado == true);
                        if (lista_Parametro_NoAnalizado.Count() > 0)
                        {
                            var Lista_ACtualizar_NoAnalizado = lista_MuestraValor.Where(q => q.ParametroID == val.ParametroID).ToList();

                            foreach (var ma in Lista_ACtualizar_NoAnalizado)
                            {
                                var muestra = muestravalor_bl.Actualiza_NoAnalizado(ma.MuestraValorID, true);
                            }
                        }

                        var lista_Parametro_NoExento = lista_MuestraValor.Where(x => x.ParametroID == val.ParametroID && x.Exento == true);
                        if (lista_Parametro_NoExento.Count() > 0)
                        {
                            var Lista_ACtualizar_Exento = lista_MuestraValor.Where(q => q.ParametroID == val.ParametroID).ToList();

                            foreach (var exe in Lista_ACtualizar_Exento)
                            {
                                var muestra = muestravalor_bl.Actualiza_Exento(exe.MuestraValorID, true);
                            }
                        }


                    }
                    //FIN Funcionalidad para modificar los parametros, no analizado y exento

                    var unicamuestra = muestra_bl.Lista_Muestra(this.Analisis.AnalisisID).FirstOrDefault();

                    muestraArgs.AnalisisID = this.Analisis.AnalisisID;
                    muestraArgs.DireccionID = unicamuestra.DireccionID;
                    muestraArgs.Numero = unicamuestra.Numero;
                    muestraArgs.Descripcion = "";
                    muestraArgs.Identificacion = "";
                    muestraArgs.Caudal = CompuestaCaudal;
                    muestraArgs.Latitud = unicamuestra.Latitud;
                    muestraArgs.Longitud = unicamuestra.Longitud;
                    muestraArgs.Datum = unicamuestra.Datum;
                    muestraArgs.Fecha = null;
                    muestraArgs.EsCompuesta = true;
                    muestraArgs.FechaRecepcion = null;
                    muestraArgs.Cancelado = false;
                    muestraArgs.Observaciones = "";
                    muestraArgs.NivelID = enumNivel.Compuesta.ConvertToInt();

                    m = muestra_bl.Guarda(muestraArgs);

                    lnkbtnMuestraCompuesta.Visible = false;

                    if (m != null)
                        Guardar(this, muestraArgs);
                    else
                        Guardar(this, null);
                }
                else
                {
                    //Mandamos evento indicando que las muestras simples estan incompletas
                    MSIncompleta(this, false);
                }
            }
        }

        protected void lnkbtnDocs_Click(object sender, EventArgs e)
        {

        }

        protected void lnkbtnEvidencia_Click(object sender, EventArgs e)
        {
            int MuestraID = int.Parse(((LinkButton)sender).CommandArgument);
            if (MuestraID > 0)
            {
                Muestra_BL muestra_bl = new Muestra_BL();
                var MuestraEvidencia = muestra_bl.Obten_Muestra(MuestraID);
                if (Evidencia != null)
                {
                    Evidencia(MuestraEvidencia, new EventArgs());
                }
            }
        }


    }
}