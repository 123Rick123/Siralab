using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucPopFiles : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler Cerrar;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Mostrar<T>(T entidad) where T : class
        {
            this.mpePopFiles.Show();
            wucEvidenciaPop1.CargarEvidencias(entidad);
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            this.mpePopFiles.Hide();
            if (Cerrar != null)
            {
                Cerrar(this, new EventArgs());
            }
        }

        protected void wucEvidenciaPop1_Cargado(object sender, EventArgs e)
        {
            this.mpePopFiles.Hide();
            lnkbCerrar_Click(null, null);
        }
    }
}