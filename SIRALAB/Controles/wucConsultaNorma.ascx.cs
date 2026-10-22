using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB_DAL;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucConsultaNorma : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler Seleccionar;

        #region Atributos
        public int? NormaID
        {
            get { return ViewState["NormaID"] == null ? null : (int?)ViewState["NormaID"]; }
            private set { ViewState["NormaID"] = value; }
        }
        private List<NormaItem> Listado { set { Session["ListadoNormas"] = value; } get { return Session["ListadoNormas"] == null ? null : (List<NormaItem>)Session["ListadoNormas"]; } }
        public int PaginaActual
        {
            get { return (int)ViewState["PaginaActual"]; }
            private set { ViewState["PaginaActual"] = value; }
        }
        #endregion Atributos

        #region EventosPagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    CargarInformacion();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion EventosPagina

        #region Métodos
        private void Paginar()
        {
            PagedDataSource pageSource = new PagedDataSource();
            pageSource.DataSource = this.Listado;
            pageSource.AllowPaging = true;
            pageSource.PageSize = 20; //Inidica el Número de registros por página
            pageSource.CurrentPageIndex = this.PaginaActual;

            if (pageSource.PageCount > 0)
            {
                rptrNormas.Visible = true;
                rptrNormas.DataSource = pageSource;
                rptrNormas.DataBind();
            }

            if (this.Listado.Count > pageSource.PageSize)
            {
                //lnkBAnterior.Visible = this.PaginaActual != 0;
                //lnkBSiguiente.Visible = pageSource.PageCount != this.PaginaActual + 1;
            }
            else
            {
                //lnkBAnterior.Visible = lnkBSiguiente.Visible = false;
            }
        }
        public void CargarInformacion()
        {
            try
            {
                Norma_BL cpBL = new Norma_BL();
                var list = cpBL.ObtenerTodos(); 
                Listado = list.Select(p => new NormaItem
                {
                    NormaID = p.NormaID,
                    Norma = p.Nombre,
                    Caudal = p.RequiereCaudal ? "Si" : "No",
                    Compuesta = p.RequiereCompuesta ? "Si": "No",
                    Mensual = p.RequiereMensual? "Si" : "No",
                    FuenteAbastecimiento=(p.FuenteAbastecimiento??false )?"Si":"No",
                    Activo = p.Activo ? "Si" : "No"
                }).ToList();

                this.PaginaActual = 0;
                Paginar();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        protected void lnkbEditar_Click(object sender, EventArgs e)
        {
            var lnk = (LinkButton)sender;
            var id = (int?)int.Parse(lnk.CommandArgument);

            /* 
            Acá hacemos response a la página de detalle, la enviamos a sessión para que se tenga
            el id sobre el cuál se debe trabajar las modificaciones.
            */

            Session["NormaID_Editar"] = id;
            Response.Redirect("NormaConfiguracion.aspx");
        }
    }

    public class NormaItem
    {
        public int NormaID { get; set; }
        public string Norma { get; set; }
        public string Caudal { get; set; }
        public string Compuesta { get; set; }
        public string Mensual { get; set; }
        public string FuenteAbastecimiento { get; set; }
        public string Activo { get; set; }
    }
}