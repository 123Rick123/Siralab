using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.FIEL;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_DAL.Extensiones;

namespace SIRALAB.Controles
{
    public partial class wucListaSignatario : System.Web.UI.UserControl
    {
        public static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(List<SignatarioLista> signatarios)
        {
            rptSignatario.DataSource = signatarios;
            rptSignatario.DataBind();
            upSignatario.Update();
        }

        protected void rptSignatario_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}