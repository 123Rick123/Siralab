using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using System.Web;
using SIRALAB.controles;

namespace SIRALAB
{
    /// <summary>
    /// Summary description for hdExportarExcel
    /// </summary>
    public class hdExportarExcel : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["ListaExportar"] != null)
            {
                context.Response.Clear();
                context.Response.ContentType = "application/ms-excel";
                context.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", (string)context.Session["NombreExcel"]) + ".xls");
                context.Response.BufferOutput = true;
                context.Response.Charset = "";
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Write(context.Session["ListaExportar"].ToString());
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}