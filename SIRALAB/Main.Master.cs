using SIRALAB.Common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class Main : System.Web.UI.MasterPage
    {
        private EventHandler<SIRALAB.controles.MensajeArgs> mensajeDevolucion;
        public event EventHandler<SIRALAB.controles.MensajeArgs> MensajeDevolucion {
            add { mensajeDevolucion += value; }
            remove { mensajeDevolucion -= value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            wucMensaje.Devolucion += new EventHandler<SIRALAB.controles.MensajeArgs>(wucMensaje_Devolucion);
        }

        protected void wucMensaje_Devolucion(object sender, SIRALAB.controles.MensajeArgs e)
        {
            if (mensajeDevolucion != null)
            {
                mensajeDevolucion(this, e);
            }
            wucMensaje.Devolucion -= wucMensaje_Devolucion;
        }

       
    }
}