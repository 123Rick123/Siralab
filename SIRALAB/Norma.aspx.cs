using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.controles;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB.Common;
using SIRALAB.Controles;

namespace SIRALAB
{
    public partial class Norma : System.Web.UI.Page
    {
        private List<Norma> Listado { set { Session["ListadoNormas"] = value; } get { return Session["ListadoNormas"] == null ? null : (List<Norma>)Session["ListadoNormas"]; } }

        #region EventosPagina
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void lnkbtnNuevo_Click(object sender, EventArgs e)
        {
            RedirectPage(null);
        }

        protected void wucFiltroNorma_Buscar(object sender, FiltroNormaArg e)
        {
            Norma_BL normaBL = new Norma_BL();
            var listaFiltrada = normaBL.Filtrar(e);
            if (listaFiltrada.Count > 0)
            {
                wucConsultaNorma.CargarInformacion();
            }
            else
                this.PopMensaje("No se encontraron resultados, con los criterios de búsqueda espeficados.", "Búsqueda", Opciones.SoloMensaje, Icono.error);
        }

        protected void wucFiltroNorma_Limpiar(object sender, FiltroNormaArg e)
        {
            wucConsultaNorma.CargarInformacion();
        }

        protected void wucConsultaNorma_Seleccionar(object sender, EventArgs e)
        {
            RedirectPage((int)wucConsultaNorma.NormaID);
        }
        #endregion EventosPagina

        #region MetodosFunciones
        
        public void RedirectPage(int? NormaID)
        {
            Session["NormaID_Editar"] = NormaID;
            Response.Redirect("~/NormaConfiguracion.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        #endregion MetodosFunciones
    }
}