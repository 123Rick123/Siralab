using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using AjaxControlToolkit;
using SIRALAB_BL.common;

namespace SIRALAB.Controles
{
    public partial class wucMuestreador : System.Web.UI.UserControl
    {

        #region Propiedades
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
        public Persona LaboratorioSiganatario
        {
            get { return (Persona)Session["LaboratorioSignatario_"]; }
            set { Session["LaboratorioSignatario_"] = value; }
        }
        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }
        public SIRALAB_DAL.Analisi Analisis_
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadDrops.instance.ListaLaboratorio(ddlNombreLab);
            }
        }

        public void CargaMuestreador(int LaboratorioID)
        {
            pnlConsultaLaboratorio.Visible = true;
            //pnlConsultaLaboratorio.Enabled = true;
            pnlSignatario.Visible = true;
            //pnlSignatario.Enabled = true;
            //pnlLab.Enabled = false;
            pnlLab.Visible = false;
            pnlSignatario.Visible = true;

            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "sliderButtons", "SearchText();", true);
        }

        protected void lnkbtnConsultaCliente_Click(object sender, EventArgs e)
        {
            Laboratorio_BL laboratorio_bl = new Laboratorio_BL();
            var NombreLaboratorio = hfValor.Value;

            if (NombreLaboratorio != null)
            {
                ObtenLaboratorio();

                //pnlSignatario.Enabled = true;
                //pnlLab.Enabled = false;
                pnlLab.Visible = false;
                pnlSignatario.Visible = true;

                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "sliderButtons", "SearchText_(" + this.LaboratorioDatos.LaboratorioID + ");", true);
            }
        }

        public void CargaSignatarioLogin()
        {
            if (this.Login != null)
            {
                pnlConsultaLaboratorio.Visible = true;
                //pnlConsultaLaboratorio.Enabled = true;
                //pnlSignatario.Enabled = true;
                //pnlLab.Enabled = false;
                pnlLab.Visible = false;
                pnlSignatario.Visible = true;
                //CargaScript();
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "sliderButtons", "SearchText_(" + this.Login.LaboratorioID + ");", true);
            }
        }

        protected void rblTipoLaboratorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (rblTipoLaboratorio.SelectedItem.Value == "2")
            {
                Session["cheksignatario"] = null;
                pnlLab.Visible = true;
                pnlSignatario.Visible = false;

                ////Llenamos la información del dropdown list con la información de los laboratorios externos
                LoadDrops.instance.ListaLaboratorio(ddlNombreLab);

                pnlConsultaLaboratorio.Visible = true;
                pnlLab.Visible = true;
                //pnlLab.Enabled = true;
                pnlSignatario.Visible = false;
                this.LaboratorioDatos = null;
                this.LaboratorioSiganatario = null;
            }
            else
            {
                Session["cheksignatario"] = 1;
                //Cargamos los muestreadores del laboratorio 
                ddlMuestreador.Items.Clear();
                LoadDrops.instance.ListaMuestreador(ddlMuestreador, this.Login.Laboratorio.LaboratorioID);

                pnlConsultaLaboratorio.Visible = false;
                this.LaboratorioDatos = null;
                this.LaboratorioSiganatario = null;

                //Seleccion Automatica
                if (ddlMuestreador.Items.Count == 2)
                {
                    ddlMuestreador.SelectedIndex = 1;
                    ddlMuestreador_SelectedIndexChanged(null, null);
                }
                //Seleccion Automatica

                CargaSignatarioLogin();
            }

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hiddenFields", "cargaSelectMateriaFlotante()", true);
            //CargaScript();
        }

        public void CargaScript()
        {
            const string sScript = "$(function llamarsum() { javascript:sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
            //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "sliderButtons", "SearchText();", true);
        }

        public void Restablecer()
        {
            this.LaboratorioDatos = null;
            this.LaboratorioSiganatario = null;

            pnlSignatario.Visible = false;
            pnlConsultaLaboratorio.Visible = false;
        }

        protected void lnkbtnConsultaSignatario_Click1(object sender, EventArgs e)
        {
            Persona_BL persona_bl = new Persona_BL();
            var NombrePersona = hfValorSignatario.Value;

            if (NombrePersona != null)
            {
                ObtenSignatario();
                pnlSignatario.Visible = false;

                if (this.LaboratorioSiganatario != null)
                {
                    ObtenerInformacion();
                    LaboratorioSignatario_BL laboratoriosignatario_bl = new LaboratorioSignatario_BL();
                    this.Signatario_Laboratorio_Persona = laboratoriosignatario_bl.Obten_LaboratorioSignatario(this.LaboratorioDatos.LaboratorioID, this.LaboratorioSiganatario.PersonaID);
                }
            }
        }

        public void ObtenerInformacion()
        {
            Laboratorio_BL laboratorio_bl = new Laboratorio_BL();
            if (rblTipoLaboratorio.SelectedValue == "1")
            {
                this.LaboratorioDatos = laboratorio_bl.Obten_Laboratorio(this.Login.Laboratorio.Nombre);
            }
        }

        public void ObtenInformacionMuestreador()
        {
            if (this.Analisis_.OrdenTrabajo.Norma.MuestrasMultiple ?? false)
            {
                var muestreadorbl = new Muestreador_BL();
                var laboratoriosignatariobl = new LaboratorioSignatario_BL();

                var muestraBl = new Muestra_BL();
                var listaMuestra = muestraBl.Lista_Muestra(this.Analisis_.AnalisisID);
                var muestreador = muestreadorbl.Obten_Muestra_Muestra(listaMuestra[0].MuestraID);

                hfLaboratorioAnalisisID.Value = this.Analisis_.OrdenTrabajo.LaboratorioID.ToString();

                if (muestreador != null)
                {
                    Session["cheksignatario"] = 1;
                    pnlConsultaLaboratorio.Visible = true;

                    LoadDrops.instance.ListaLaboratorio(ddlNombreLab);
                    ddlNombreLab.ClearSelection();
                    ddlNombreLab.Items.FindByValue(muestreador.LaboratorioSignatario.Laboratorio.LaboratorioID.ToString()).Selected = true;
                    ddlNombreLab.SelectedValue = muestreador.LaboratorioSignatario.Laboratorio.LaboratorioID.ToString();

                    LoadDrops.instance.ListaMuestreador(ddlMuestreador, muestreador.LaboratorioSignatario.Laboratorio.LaboratorioID);
                    ddlMuestreador.ClearSelection();
                    ddlMuestreador.Items.FindByValue(muestreador.LaboratorioSignatario.Persona.PersonaID.ToString()).Selected = true;
                    ddlMuestreador.SelectedValue = muestreador.LaboratorioSignatario.Persona.PersonaID.ToString();

                    pnlLab.Visible = true;
                    pnlSignatario.Visible = true;
                    //pnlSignatario.Enabled = true;

                    this.Signatario_Laboratorio_Persona = laboratoriosignatariobl.Obten_LaboratorioSignatario(muestreador.LaboratorioSignatario.LaboratorioID, muestreador.LaboratorioSignatario.Persona.PersonaID);
                    this.LaboratorioDatos = this.Signatario_Laboratorio_Persona.Laboratorio;

                    if (this.Login.Laboratorio.LaboratorioID == this.LaboratorioDatos.LaboratorioID)
                    {
                        rblTipoLaboratorio.SelectedValue = "1";
                        pnlLab.Visible = false;
                    }
                    else
                    {
                        rblTipoLaboratorio.SelectedValue = "2";
                    }

                }
                else
                {
                    Session["cheksignatario"] = null;
                    pnlConsultaLaboratorio.Visible = true;
                }
            }

            if (this.Muestra != null)
            {
                Session["cheksignatario"] = 1;
                Muestreador_BL muestreador_bl = new Muestreador_BL();
                LaboratorioSignatario_BL laboratoriosignatario_bl = new LaboratorioSignatario_BL();

                var muestreador = muestreador_bl.Obten_Muestra_Muestra(this.Muestra.MuestraID);

                if (muestreador != null)
                {
                    pnlConsultaLaboratorio.Visible = true;

                    LoadDrops.instance.ListaLaboratorio(ddlNombreLab);
                    ddlNombreLab.ClearSelection();
                    ddlNombreLab.Items.FindByValue(muestreador.LaboratorioSignatario.Laboratorio.LaboratorioID.ToString()).Selected = true;
                    ddlNombreLab.SelectedValue = muestreador.LaboratorioSignatario.Laboratorio.LaboratorioID.ToString();

                    LoadDrops.instance.ListaMuestreador(ddlMuestreador, muestreador.LaboratorioSignatario.Laboratorio.LaboratorioID);
                    ddlMuestreador.ClearSelection();
                    ddlMuestreador.Items.FindByValue(muestreador.LaboratorioSignatario.Persona.PersonaID.ToString()).Selected = true;
                    ddlMuestreador.SelectedValue = muestreador.LaboratorioSignatario.Persona.PersonaID.ToString();

                    pnlLab.Visible = true;
                    pnlSignatario.Visible = true;

                    this.Signatario_Laboratorio_Persona = laboratoriosignatario_bl.Obten_LaboratorioSignatario(muestreador.LaboratorioSignatario.LaboratorioID, muestreador.LaboratorioSignatario.Persona.PersonaID);
                    this.LaboratorioDatos = this.Signatario_Laboratorio_Persona.Laboratorio;

                    if (this.Login.Laboratorio.LaboratorioID == this.LaboratorioDatos.LaboratorioID)
                    {
                        rblTipoLaboratorio.SelectedValue = "1";
                        pnlLab.Visible = false;
                    }
                    else
                    {
                        rblTipoLaboratorio.SelectedValue = "2";
                    }
                    
                }
                else
                {
                    pnlConsultaLaboratorio.Visible = true;
                }
            }
        }

        protected void lnkbtnLimpiar_Click(object sender, EventArgs e)
        {
            rblTipoLaboratorio.ClearSelection();
            Restablecer();
        }

        protected void ddlNombreLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            Laboratorio_BL laboratorio_bl = new Laboratorio_BL();

            if (ddlNombreLab.SelectedValue == "0")
            {

            }

            if (ddlNombreLab.SelectedItem.Text != "Seleccione")
            {
                Session["cheksignatario"] = null;
                ObtenLaboratorio();
                //pnlSignatario.Enabled = true;
                
                ddlMuestreador.Items.Clear();
                LoadDrops.instance.ListaMuestreador(ddlMuestreador, this.LaboratorioDatos.LaboratorioID);
                pnlSignatario.Visible = true;
            }
            else
            {
                //Desabilitamos el drop down list correspondiente a los muestreadores

            }
        }

        public void ObtenLaboratorio()
        {
            Laboratorio_BL lBL = new Laboratorio_BL();
            this.LaboratorioDatos = lBL.Obten_Laboratorio(ddlNombreLab.SelectedItem.Text);
        }

        protected void ddlMuestreador_SelectedIndexChanged(object sender, EventArgs e)
        {
            Persona_BL persona_bl = new Persona_BL();

            if (ddlMuestreador.SelectedItem.Text != "Seleccione")
            {
                ObtenSignatario();

                if (this.LaboratorioSiganatario != null)
                {
                    ObtenerInformacion();
                    LaboratorioSignatario_BL laboratoriosignatario_bl = new LaboratorioSignatario_BL();
                    this.Signatario_Laboratorio_Persona = laboratoriosignatario_bl.Obten_LaboratorioSignatario(this.LaboratorioDatos.LaboratorioID, this.LaboratorioSiganatario.PersonaID);
                    Session["cheksignatario"] = 1;
                }
            }else
            { Session["cheksignatario"] = null; }

        }

        public void ObtenSignatario()
        {
            Persona_BL persona_bl = new Persona_BL();

            if (ddlMuestreador.SelectedItem.Text != "Seleccione")
            {
                this.LaboratorioSiganatario = persona_bl.Obten_Persona(int.Parse(ddlMuestreador.SelectedValue));
            }
        }
    }
}