using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class ConsultaApercibimientoReincidencia : System.Web.UI.Page
    {
        public string IdReincidencia;
        protected void Page_Load(object sender, EventArgs e)
        {
            IdReincidencia = Request.QueryString["IdReincidencia"];
            if (IdReincidencia==null)
            {
                updReincidencia.Visible = false;
            }
        }
    }
}