using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;

namespace SIRALAB
{
    public partial class pop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Byte[] bufferByte =(byte[])Session["popReport"];
            Session["popReport"] = null;
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=Analisis.pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(bufferByte);
            Response.End();
        }
    }
}