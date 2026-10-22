using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB.Controles;
using Microsoft.Reporting.WebForms;
using System.Configuration;


namespace SIRALAB
{
    public partial class InformeMensual : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
            }
        }

        protected void wucConformacion_Respuesta(object sender, SIRALAB.Controles.wucConformacion.FiltroMensual e)
        {
            pnl.Style["display"]="block";                
            SIRALAB.Controles.wucConformacion.FiltroMensual fm = new SIRALAB.Controles.wucConformacion.FiltroMensual();
            fm = (SIRALAB.Controles.wucConformacion.FiltroMensual)e;
      
            try
            {
                /*
                 @pRFC='AUT9606011W5',@pTitulo='12CAM106224/31ERDL13',@pAnexo=7, @pTipo=1,@pAño=2013,@pPeriodo=1, @pNorma=1,@pOT=''
                 */
              

                ReportParameter pRFC = new ReportParameter("pRFC",fm.RFC);
                ReportParameter pTitulo = new ReportParameter("pTitulo", fm.Titulo);
                ReportParameter pAnexo = new ReportParameter("pAnexo", fm.Anexo.ToString());
                ReportParameter pTipo = new ReportParameter("pTipo", fm.Tipo.ToString());
                ReportParameter pPeriodo = new ReportParameter("pPeriodo", fm.Periodo.ToString());
                ReportParameter pAño = new ReportParameter("pAño", fm.Anio.ToString());
                ReportParameter pNorma = new ReportParameter("pNorma", fm.Norma.ToString());
                ReportParameter pOT = new ReportParameter("pOT", "x");

                ReportParameter pFechaInicial = new ReportParameter("pFechaInicial", fm.FechaInicial.ToShortDateString());
                ReportParameter pFechaFinal = new ReportParameter("pFechaFinal", fm.FechaFinal.ToShortDateString());

                rvMensual.ServerReport.ReportServerCredentials = new ReportingCredentials(ConfigurationManager.AppSettings["RSUser"], ConfigurationManager.AppSettings["RSPassword"], ConfigurationManager.AppSettings["RSDomain"]);
                rvMensual.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["RSServidorURL"].ToString());
                rvMensual.ServerReport.ReportPath = ConfigurationManager.AppSettings["RSReportesSIRALABPath"].ToString() + "/ReporteMensualAnalisis";
                rvMensual.ServerReport.SetParameters(pRFC);
                rvMensual.ServerReport.SetParameters(pTitulo);
                rvMensual.ServerReport.SetParameters(pAnexo);
                rvMensual.ServerReport.SetParameters(pTipo);
                rvMensual.ServerReport.SetParameters(pPeriodo);
                rvMensual.ServerReport.SetParameters(pNorma);
                rvMensual.ServerReport.SetParameters(pOT);
                rvMensual.ServerReport.SetParameters(pAño);
                rvMensual.ServerReport.SetParameters(pFechaInicial);
                rvMensual.ServerReport.SetParameters(pFechaFinal);
                rvMensual.ServerReport.Refresh();
                rvMensual.Visible = true;

                updPnlOrden.Update();
            }
            catch (Exception ex)
            {

            }

        }
    }
}