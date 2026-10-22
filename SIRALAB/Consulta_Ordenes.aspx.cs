
using SIRALAB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SIRALAB.Controles;
using System.Configuration;

namespace SIRALAB
{
    public partial class Consulta_Ordenes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                wucFiltro_Consulta_Ordenes_Buscar(this, new FiltrosConsultaOrdenesArg());
        }

        protected void wucFiltro_Consulta_Ordenes_Buscar(object sender, FiltrosConsultaOrdenesArg e)
        {
            string nombreReportAcuse = "";
         
            try
            {
                    ReportParameter paramRFC = new ReportParameter("RFC", e.RFCCliente);
                    ReportParameter paramFolio = new ReportParameter("Folio", e.Numero);
                    ReportParameter paramFirmada = new ReportParameter("Firmada", e.Firmados.ToString());
                    ReportParameter paramFechaInicio = new ReportParameter("FechaInicioAux", e.FechaInicial.Value.ToShortDateString());
                    ReportParameter paramFechaFin = new ReportParameter("FechaFinAux", e.FechaFinal.Value.ToShortDateString());


                    rvConsulta.ServerReport.ReportServerCredentials = new ReportingCredentials(ConfigurationManager.AppSettings["RSUser"], ConfigurationManager.AppSettings["RSPassword"], ConfigurationManager.AppSettings["RSDomain"]);
                    rvConsulta.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["RSServidorURL"].ToString());
                    rvConsulta.ServerReport.ReportPath = ConfigurationManager.AppSettings["RSReportesSIRALABPath"].ToString() + "/consulta";
                    rvConsulta.ServerReport.SetParameters(paramRFC);
                    rvConsulta.ServerReport.SetParameters(paramFolio);
                    rvConsulta.ServerReport.SetParameters(paramFirmada);
                    rvConsulta.ServerReport.SetParameters(paramFechaInicio);
                    rvConsulta.ServerReport.SetParameters(paramFechaFin);
                    rvConsulta.ServerReport.Refresh();
                    rvConsulta.Visible = true;
               
            }
            catch (Exception ex)
            {

            }



        }
    }
}