using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SIRALAB
{
    /// <summary>
    /// Summary description for closeSessionHandler
    /// </summary>
    public class closeSessionHandler : IHttpHandler, IRequiresSessionState 
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Session.Abandon();
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