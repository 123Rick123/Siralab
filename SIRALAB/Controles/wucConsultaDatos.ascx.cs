using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucConsultaDatos : System.Web.UI.UserControl
    {   

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void CargaInformación(SIRALAB_DAL.OrdenTrabajo ordentrabajoArgs)
        {
            lblRFC.Text= ordentrabajoArgs.Cliente.RFC;
            lblClienteCurp.Text = ordentrabajoArgs.Cliente.CURP;
            lblClienteNombre.Text = ordentrabajoArgs.Cliente.Nombre;


            lblInforme.Text = ordentrabajoArgs.NumeroInforme;
            lblFecha.Text = ordentrabajoArgs.FechaEmision.ToShortDateString();
        }
    }
}