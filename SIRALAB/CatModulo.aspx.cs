using SIRALAB_BL.common;
using SIRALAB_BL.seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class CatModulo : System.Web.UI.Page
    {

        public IEnumerable<SIRALAB_DAL.CatModulo> Modulos
        {
            get
            {
                return (Session["_modulo"] == null) ? new List<SIRALAB_DAL.CatModulo>() : (IEnumerable<SIRALAB_DAL.CatModulo>)Session["_modulo"];
            }
            set
            {
                Session["_modulo"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargaInformacion();
            }
        }

        private void CargaInformacion()
        {
            CatModulos_BL moduloBL = new CatModulos_BL();

            Modulos = moduloBL.ObtenerTodos();

            BindGrid();
        }

        private void BindGrid()
        {
            grdModulo.DataSource = Modulos;
            grdModulo.DataBind();

            DropDownList ddlModuloPadre = (grdModulo.FooterRow.FindControl("ddlModuloPadre") as DropDownList);

            CargaCboModulos(ddlModuloPadre);
        }

        private static void CargaCboModulos(DropDownList ddlModuloPadre)
        {
            CatModulos_BL moduloBL = new CatModulos_BL();
            var modulosP = moduloBL.ObtenerTodos();

            ddlModuloPadre.DataSource = modulosP;
            ddlModuloPadre.DataTextField = "Nombre";
            ddlModuloPadre.DataValueField = "ModuloId";
            ddlModuloPadre.DataBind();
            ddlModuloPadre.Items.Insert(0, new ListItem("Ninguno"));
        }

        /// <summary>
        /// Se agregara una demanda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkModulo_Click(object sender, EventArgs e)
        {
            grdModulo.ShowFooter = true;

            BindGrid();
        }

        protected void grdModulo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    SIRALAB_DAL.CatModulo mod = (SIRALAB_DAL.CatModulo)e.Row.DataItem;

                    CatModulos_BL moduloBL = new CatModulos_BL();
                    DropDownList ddlModuloPadre = (e.Row.FindControl("ddlModuloPadre") as DropDownList);

                    int id = mod.ModuloID;

                    var modulosP = moduloBL.ObtenerTodos();
                    var datos = modulosP.Where(a => a.ModuloID != id).Select(a => a).ToList();
                    ddlModuloPadre.DataSource = datos;
                    ddlModuloPadre.DataTextField = "Nombre";
                    ddlModuloPadre.DataValueField = "ModuloId";
                    ddlModuloPadre.DataBind();

                    ddlModuloPadre.Items.Insert(0, new ListItem("Ninguno"));

                    if (mod.ModuloPadreID != null)
                        ddlModuloPadre.SelectedValue = mod.ModuloPadreID.ToString();


                    /*Elimina la codificacion html*/
                    TextBox txtDescripcion = (e.Row.FindControl("txtDescripcion") as TextBox);
                    TextBox txtNombre = (e.Row.FindControl("txtNombre") as TextBox);

                    txtDescripcion.Text = WebUtility.HtmlDecode(txtDescripcion.Text);
                    txtNombre.Text = WebUtility.HtmlDecode(txtNombre.Text);

                }
            }
        }

        /// <summary>
        /// Paginacion del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdModulo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdModulo.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        /// <summary>
        /// Se cancela la edición de las demandas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdModulo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            LimpiControl();

            grdModulo.ShowFooter = false;
            grdModulo.EditIndex = -1;
            CargaInformacion();
        }

        private void LimpiControl()
        {
            TextBox txtNombre = grdModulo.FooterRow.FindControl("txtNombre") as TextBox;
            TextBox txtDescripcion = grdModulo.FooterRow.FindControl("txtDescripcion") as TextBox;
            TextBox txtOrden = grdModulo.FooterRow.FindControl("txtOrden") as TextBox;
            TextBox txtURL = grdModulo.FooterRow.FindControl("txtURL") as TextBox;

            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtOrden.Text = "0";
            txtURL.Text = "";


        }

        /// <summary>
        /// Agrega item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdModulo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Insert"))
                {
                    CatModulos_BL moduloBL = new CatModulos_BL();
                    CatModulosArg moduloArg = new CatModulosArg();

                    TextBox txtNombre = grdModulo.FooterRow.FindControl("txtNombre") as TextBox;
                    TextBox txtDescripcion = grdModulo.FooterRow.FindControl("txtDescripcion") as TextBox;
                    TextBox txtOrden = grdModulo.FooterRow.FindControl("txtOrden") as TextBox;
                    TextBox txtURL = grdModulo.FooterRow.FindControl("txtURL") as TextBox;
                    TextBox txtCssClass = grdModulo.FooterRow.FindControl("txtCssClass") as TextBox;

                    DropDownList ddlModuloPadre = (grdModulo.FooterRow.FindControl("ddlModuloPadre") as DropDownList);

                    moduloArg.Nombre = txtNombre.Text.Trim();
                    moduloArg.Descripcion = txtDescripcion.Text.Trim();

                    if (ddlModuloPadre.SelectedIndex > 0)
                        moduloArg.ModuloPadreID = int.Parse(ddlModuloPadre.SelectedValue);
                    else
                        moduloArg.ModuloPadreID = null;

                    int orden = (txtOrden.Text != "") ? int.Parse(txtOrden.Text) : 0;
                    moduloArg.Orden = orden;


                    moduloArg.URL = txtURL.Text;
                    moduloArg.CssClass = txtCssClass.Text;

                    moduloBL.Guardar(moduloArg);

                    grdModulo.ShowFooter = false;
                    grdModulo.EditIndex = -1;
                    CargaInformacion();
                }
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Catalogo Modulos", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }

        /// <summary>
        /// Se edita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdModulo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdModulo.EditIndex = e.NewEditIndex;

            BindGrid();
        }

        /// <summary>
        /// Se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdModulo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = int.Parse(grdModulo.DataKeys[e.RowIndex].Value.ToString());

                //actualiza
                CatModulos_BL moduloBL = new CatModulos_BL();

                CatModulosArg moduloArg = new CatModulosArg();

                TextBox txtNombre = grdModulo.Rows[e.RowIndex].FindControl("txtNombre") as TextBox;
                TextBox txtDescripcion = grdModulo.Rows[e.RowIndex].FindControl("txtDescripcion") as TextBox;
                TextBox txtOrden = grdModulo.Rows[e.RowIndex].FindControl("txtOrden") as TextBox;
                TextBox txtURL = grdModulo.Rows[e.RowIndex].FindControl("txtURL") as TextBox;
                TextBox txtCssClass = grdModulo.Rows[e.RowIndex].FindControl("txtCssClass") as TextBox;

                DropDownList ddlModuloPadre = (grdModulo.Rows[e.RowIndex].FindControl("ddlModuloPadre") as DropDownList);

                moduloArg.ModuloID = id;

                moduloArg.Nombre = txtNombre.Text.Trim();
                moduloArg.Descripcion = txtDescripcion.Text.Trim();
                if (ddlModuloPadre.SelectedIndex > 0)
                    moduloArg.ModuloPadreID = int.Parse(ddlModuloPadre.SelectedValue);
                else
                    moduloArg.ModuloPadreID = null;



                int orden = (txtOrden.Text != "") ? int.Parse(txtOrden.Text) : 0;
                moduloArg.Orden = orden;
                moduloArg.URL = txtURL.Text;
                moduloArg.CssClass = txtCssClass.Text;

                moduloBL.Guardar(moduloArg);
                grdModulo.EditIndex = -1;
                CargaInformacion();
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Catalogo Modulos", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }

        private class CatModulosArg : CatModulosArgBase_BL
        {
        }
    }
}