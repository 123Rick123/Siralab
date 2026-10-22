using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class ImpugnacionReincidenciaHistorico : System.Web.UI.Page
    {
        LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();
        public string IdReincidencia;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            IdReincidencia = Request.QueryString["IdReincidencia"];
        }

        protected void btnVerImpugnacion_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/DesgloseHistoricoImpugnacionReincidencia.aspx?id=" + IdReincidencia);
        }
    }
}