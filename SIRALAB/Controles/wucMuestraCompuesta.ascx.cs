using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using System.ComponentModel;

namespace SIRALAB.Controles
{
    public partial class wucMuestraCompuesta : System.Web.UI.UserControl
    {

        #region Propiedades
        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
            set { Session["MuestraMuestra_"] = value; }
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
        public event EventHandler Evidencia;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        public void CargaInformacion(int AnalisisID)
        {
            Analisis_BL aBL = new Analisis_BL();
            var analisis = aBL.Obten_Analisis(AnalisisID);

            if (analisis.OrdenTrabajo.Norma.RequiereCaudal)
            {
                List<MuestraLista> muestra_lista = new List<MuestraLista>();
                Muestra_BL muestra_bl = new Muestra_BL();
                var lista_muestra = muestra_bl.Lista_Muestra(AnalisisID).Where(x => x.NivelID == 2).ToList();

                int cont = 1;
                foreach (var i in lista_muestra)
                {
                    muestra_lista.Add(new MuestraLista
                    {
                        MuestraID_ = i.MuestraID,
                        Fecha_ = i.FechaConformacion != null ? DateTime.Parse(i.FechaConformacion.ToString()).ToShortDateString() : i.Fecha != null ? DateTime.Parse(i.Fecha.ToString()).ToShortDateString() : "",// .ToShortDateString(),
                        Hora_ = i.FechaConformacion != null ? DateTime.Parse(i.FechaConformacion.ToString()).ToShortTimeString() : i.Fecha != null ?  DateTime.Parse(i.Fecha.ToString()).ToShortTimeString() : "",
                        Observaciones_ = i.Observaciones,
                        Consecutivo_ = cont
                    });
                    cont++;
                }

                pnlMuestraCompuesta.Visible = lista_muestra.Count > 0;
                rptTitulo.DataSource = muestra_lista;
                rptTitulo.DataBind();
            }
            else
            {
                pnlMuestraCompuesta.Visible = false;
            }
        }

        protected void lnkbtnSeleccionar_Click(object sender, EventArgs e)
        {
            int MuestraID = int.Parse(((LinkButton)sender).CommandArgument);
            if (MuestraID >0)
            {
                this.LaboratorioDatos = null;
                this.Signatario_Laboratorio_Persona = null;

                this.EsCompuesta = true;
                Muestra_BL muestra_bl = new Muestra_BL();
                this.Muestra=muestra_bl.Obten_Muestra(MuestraID);
                Response.Redirect("~/AnalisisParametro.aspx");
            }
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