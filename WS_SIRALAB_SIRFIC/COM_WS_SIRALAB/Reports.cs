using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using System.Configuration;

namespace COM_WS_SIRALAB
{
    public static class Reports
    {
        public static byte[] BufferPDFRemoteReport(string NombreReporte, List<ReportParameter> listParametros)
        {
            ReportViewer rv = new ReportViewer();
            rv.ServerReport.ReportServerCredentials = new ReportingCredentials(ConfigurationManager.AppSettings["RS_User"], ConfigurationManager.AppSettings["RS_Password"], ConfigurationManager.AppSettings["RS_Domain"]);
            rv.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["RS_ServidorURL"].ToString());
            rv.ServerReport.ReportPath = ConfigurationManager.AppSettings["RS_ReportesSIRALABPath"].ToString() + "/" + NombreReporte;
            rv.ProcessingMode = ProcessingMode.Remote;

            listParametros.ForEach(param => rv.ServerReport.SetParameters(param));

            rv.ServerReport.Refresh();
            string MimeType;
            string EncodingString;
            string FileNameExts;
            Warning[] Warnings;
            string[] StreamIds;
            return rv.ServerReport.Render("PDF", null, out MimeType, out EncodingString, out FileNameExts, out StreamIds, out Warnings);

        }
    }
}
