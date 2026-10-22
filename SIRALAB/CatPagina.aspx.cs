using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.common;
using SIRALAB_BL.seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Net;

namespace SIRALAB
{
    public partial class CatPagina : System.Web.UI.Page
    {

        public IEnumerable<SIRALAB_DAL.CatPagina> Paginas
        {
            get
            {
                return (Session["_Pagina"] == null) ? new List<SIRALAB_DAL.CatPagina>() : (IEnumerable<SIRALAB_DAL.CatPagina>)Session["_Pagina"];
            }
            set
            {
                Session["_Pagina"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargaInformacion();
            }
        }

        /// <summary>
        /// carga la información
        /// </summary>
        private void CargaInformacion()
        {
            CatPagina_BL PaginaBL = new CatPagina_BL();

            Paginas = PaginaBL.ObtenerTodos().OrderBy(a => a.ModuloID).ThenBy(a => a.Orden).ThenBy(a => a.URL);

            BindGrid();
        }

        /// <summary>
        /// Bind grid 
        /// </summary>
        private void BindGrid()
        {
            grdPagina.DataSource = Paginas;
            grdPagina.DataBind();

            DropDownList ddlModulo = (grdPagina.FooterRow.FindControl("ddlModulo") as DropDownList);

            CargaCboModulos(ddlModulo);
        }

        /// <summary>
        /// carga el combo de los modulos en el footer del grid.
        /// </summary>
        /// <param name="ddlModulo"></param>
        private static void CargaCboModulos(DropDownList ddlModulo)
        {

            CatModulos_BL moduloBL = new CatModulos_BL();
            var modulosP = moduloBL.ObtenerTodos();


            ddlModulo.DataSource = modulosP;
            ddlModulo.DataTextField = "Nombre";
            ddlModulo.DataValueField = "ModuloId";
            ddlModulo.DataBind();

        }

        /// <summary>
        /// Se agregara una demanda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPagina_Click(object sender, EventArgs e)
        {
            grdPagina.ShowFooter = true;

            BindGrid();
        }

        protected void grdPagina_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    SIRALAB_DAL.CatPagina pag = (SIRALAB_DAL.CatPagina)e.Row.DataItem;

                    DropDownList ddlModulo = (e.Row.FindControl("ddlModulo") as DropDownList);

                    CargaCboModulos(ddlModulo);

                    ddlModulo.SelectedValue = pag.ModuloID.ToString();


                    /*Elimina la codificacion html*/
                    TextBox txtDescripcion = (e.Row.FindControl("txtDescripcion") as TextBox);
                   
                    txtDescripcion.Text = WebUtility.HtmlDecode(txtDescripcion.Text);
                    
                }
            }
        }

        /// <summary>
        /// Paginacion del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPagina_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPagina.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        /// <summary>
        /// Se cancela la edición de las demandas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPagina_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdPagina.ShowFooter = false;
            grdPagina.EditIndex = -1;
            CargaInformacion();
        }

        /// <summary>
        /// Agrega item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPagina_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Insert"))
                {
                    CatPagina_BL PaginaBL = new CatPagina_BL();
                    CatPaginasArg PaginaArg = new CatPaginasArg();

                    TextBox txtURL = grdPagina.FooterRow.FindControl("txtURL") as TextBox;
                    TextBox txtDescripcion = grdPagina.FooterRow.FindControl("txtDescripcion") as TextBox;
                    DropDownList ddlModulo = (grdPagina.FooterRow.FindControl("ddlModulo") as DropDownList);
                    CheckBox chkActivo = (grdPagina.FooterRow.FindControl("chkActivo") as CheckBox);
                    CheckBox chkVisibleMenu = (grdPagina.FooterRow.FindControl("chkVisibleMenu") as CheckBox);
                    TextBox txtOrden = grdPagina.FooterRow.FindControl("txtOrden") as TextBox;
                    TextBox txtCssClass = grdPagina.FooterRow.FindControl("txtCssClass") as TextBox;

                    /*validacion*/
                    SIRALAB_DAL.CatPagina pagina = new SIRALAB_DAL.CatPagina();
                    pagina = PaginaBL.Obtener(txtURL.Text.Trim());

                    if (pagina != null)
                    {
                        this.PopMensaje("La pagina ya existe", "Catalogo Paginas", controles.Opciones.SoloAceptar, controles.Icono.error);

                    }
                    else
                    {
                        PaginaArg.URL = txtURL.Text.Trim();
                        PaginaArg.Descripcion = txtDescripcion.Text.Trim();
                        PaginaArg.ModuloID = int.Parse(ddlModulo.SelectedValue);
                        PaginaArg.Activo = chkActivo.Checked;
                        PaginaArg.VisibleMenu = chkVisibleMenu.Checked;

                        int orden = (txtOrden.Text != "") ? int.Parse(txtOrden.Text) : 0;
                        PaginaArg.Orden = orden;
                        PaginaArg.CssClass = txtCssClass.Text;

                        PaginaBL.Guardar(PaginaArg);

                        grdPagina.ShowFooter = false;
                        grdPagina.EditIndex = -1;
                        CargaInformacion();
                    }
                }
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Catalogo Paginas", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }

        /// <summary>
        /// Se edita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPagina_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdPagina.EditIndex = e.NewEditIndex;

            BindGrid();
        }

        /// <summary>
        /// Se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPagina_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = int.Parse(grdPagina.DataKeys[e.RowIndex].Value.ToString());

                TextBox txtURL = grdPagina.Rows[e.RowIndex].FindControl("txtURL") as TextBox;
                TextBox txtDescripcion = grdPagina.Rows[e.RowIndex].FindControl("txtDescripcion") as TextBox;
                DropDownList ddlModulo = (grdPagina.Rows[e.RowIndex].FindControl("ddlModulo") as DropDownList);
                CheckBox chkActivo = (grdPagina.Rows[e.RowIndex].FindControl("chkActivo") as CheckBox);
                CheckBox chkVisibleMenu = (grdPagina.Rows[e.RowIndex].FindControl("chkVisibleMenu") as CheckBox);
                TextBox txtOrden = grdPagina.Rows[e.RowIndex].FindControl("txtOrden") as TextBox;
                TextBox txtCssClass = grdPagina.Rows[e.RowIndex].FindControl("txtCssClass") as TextBox;

                //actualiza
                CatPagina_BL PaginaBL = new CatPagina_BL();

                CatPaginasArg PaginaArg = new CatPaginasArg();
                /*validacion*/
                SIRALAB_DAL.CatPagina pagina = new SIRALAB_DAL.CatPagina();
                pagina = PaginaBL.Obtener(txtURL.Text.Trim());

                if (pagina != null && pagina.PaginaID != id)
                {
                    this.PopMensaje("La pagina ya existe", "Catalogo Paginas", controles.Opciones.SoloAceptar, controles.Icono.error);

                }
                else
                {


                    PaginaArg.PaginaID = id;
                    PaginaArg.URL = txtURL.Text.Trim();
                    PaginaArg.Descripcion = txtDescripcion.Text.Trim();
                    PaginaArg.ModuloID = int.Parse(ddlModulo.SelectedValue);
                    PaginaArg.Activo = chkActivo.Checked;
                    int orden = (txtOrden.Text != "") ? int.Parse(txtOrden.Text) : 0;
                    PaginaArg.Orden = orden;
                    PaginaArg.CssClass = txtCssClass.Text;
                    PaginaArg.VisibleMenu = chkVisibleMenu.Checked;

                    PaginaBL.Guardar(PaginaArg);
                    grdPagina.EditIndex = -1;
                    CargaInformacion();
                }
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Catalogo Paginas", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }

        private class CatPaginasArg : CatPaginaArgBase_BL
        {
        }
    }
}