using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SIRALAB.Common
{
    public static class OpenReports
    {
        public static byte[] BufferPDFRemoteReport(string NombreReporte, List<ReportParameter> listParametros)
        {
            ReportViewer rv = new ReportViewer();
            rv.ServerReport.ReportServerCredentials = new ReportingCredentials(
                ConfigurationManager.AppSettings["RSUser"], ConfigurationManager.AppSettings["RSPassword"],
                ConfigurationManager.AppSettings["RSDomain"]);
            rv.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["RSServidorURL"]);
            rv.ServerReport.ReportPath =
                ConfigurationManager.AppSettings["RSReportesSIRALABPath"] + "/" + NombreReporte;
            rv.ProcessingMode = ProcessingMode.Remote;
            listParametros.ForEach(param => rv.ServerReport.SetParameters(param));
            try
            {
                rv.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.ToString());
            }

            string MimeType;
            string EncodingString;
            string FileNameExts;
            Warning[] Warnings;
            string[] StreamIds;

            return rv.ServerReport.Render("PDF", null, out MimeType, out EncodingString, out FileNameExts, out StreamIds, out Warnings);
        }
    }
}