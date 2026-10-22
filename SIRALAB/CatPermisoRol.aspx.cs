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
using SIRALAB_DAL;


namespace SIRALAB 
{
    public partial class CatPermisoRol : System.Web.UI.Page
    {
        public IEnumerable<SIRALAB_DAL.CatPermisosRol> PermisoRols
        {
            get
            {
                return (Session["_PermisoRol"] == null) ? new List<SIRALAB_DAL.CatPermisosRol>() : (IEnumerable<SIRALAB_DAL.CatPermisosRol>)Session["_PermisoRol"];
            }
            set
            {
                Session["_PermisoRol"] = value;
            }
        }

        public int UsuarioRolId
        {
            get
            {
                return (Session["_UsuarioRolId"] == null) ? 0 : int.Parse(Session["_UsuarioRolId"].ToString());
            }
            set
            {
                Session["_UsuarioRolId"] = value;
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


            CargaCboUsuarioRol();
            BindGrid();



        }



        private void BindGrid()
        {
            CatPermisosRol_BL PermisoRolBL = new CatPermisosRol_BL();

            PermisoRols = PermisoRolBL.ObtenerTodos();

            grdPermisoRol.DataSource = PermisoRols.Where(a => a.UsuarioRolID == UsuarioRolId).ToList();
            grdPermisoRol.DataBind();



        }
        private static void CargaCboPagina(DropDownList ddlPagina)
        {

            CatPagina_BL paginaBL = new CatPagina_BL();
            var datos = paginaBL.ObtenerTodos();


            ddlPagina.DataSource = datos.ToList();
            ddlPagina.DataTextField = "URL";
            ddlPagina.DataValueField = "PaginaID";
            ddlPagina.DataBind();

        }

        #region CatUsuarioRol




        private void CargaCboUsuarioRol()
        {

            CatUsuarioRol_BL BL = new CatUsuarioRol_BL();
            var datos = BL.ObtenerTodos();


            cboUsuarioRol.DataSource = datos;
            cboUsuarioRol.DataTextField = "Nombre";
            cboUsuarioRol.DataValueField = "UsuarioRolId";
            cboUsuarioRol.DataBind();
            cboUsuarioRol.SelectedIndex = 0;
            UsuarioRolId = int.Parse(cboUsuarioRol.SelectedValue);

        }

        protected void cboUsuarioRol_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UsuarioRolId = int.Parse(cboUsuarioRol.SelectedValue);
            grdPermisoRol.ShowFooter = false;
            grdPermisoRol.EditIndex = -1;
            BindGrid();

        }

        #endregion
        /// <summary>
        /// Se agregara una demanda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPermisoRol_Click(object sender, EventArgs e)
        {
            grdPermisoRol.ShowFooter = true;

            
            BindGrid();
        }

        protected void grdPermisoRol_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    SIRALAB_DAL.CatPermisosRol rol = (SIRALAB_DAL.CatPermisosRol)e.Row.DataItem;

                    DropDownList ddlPagina = (e.Row.FindControl("ddlPagina") as DropDownList);

                    CargaCboPagina(ddlPagina);

                    ddlPagina.SelectedValue = rol.PaginaID.ToString();



                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlPagina = (e.Row.FindControl("ddlPagina") as DropDownList);

                CargaCboPagina(ddlPagina);
            }
        }

        /// <summary>
        /// PermisoRolcion del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPermisoRol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPermisoRol.PageIndex = e.NewPageIndex;

            BindGrid();
        }

        /// <summary>
        /// Se cancela la edición de las demandas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPermisoRol_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdPermisoRol.ShowFooter = false;
            grdPermisoRol.EditIndex = -1;
            BindGrid();
        }

        /// <summary>
        /// Agrega item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPermisoRol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Insert"))
                {
                    CatPermisosRol_BL BL = new CatPermisosRol_BL();
                    CatPermisoRolsArg Arg = new CatPermisoRolsArg();
                    CatPermisosRol rol = new CatPermisosRol();

                    DropDownList ddlPagina = (grdPermisoRol.FooterRow.FindControl("ddlPagina") as DropDownList);

                    CheckBox chkActivo = (grdPermisoRol.FooterRow.FindControl("chkActivo") as CheckBox);
                    CheckBox chkEscritura = (grdPermisoRol.FooterRow.FindControl("chkEscritura") as CheckBox);


                    Arg.UsuarioRolID = UsuarioRolId;
                    Arg.PaginaID = int.Parse(ddlPagina.SelectedValue);
                    rol = BL.Obtener(UsuarioRolId, int.Parse(ddlPagina.SelectedValue));

                    if (rol != null)
                    {
                        this.PopMensaje("La relación ya existe", "Catalogo Permisos Rol", controles.Opciones.SoloAceptar, controles.Icono.error);

                    }
                    else
                    {

                        Arg.Activo = chkActivo.Checked;
                        Arg.Escritura = chkEscritura.Checked;

                        BL.Guardar(Arg);

                        grdPermisoRol.ShowFooter = false;
                        grdPermisoRol.EditIndex = -1;
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Catalogo PermisoRols", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }

        /// <summary>
        /// Se edita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPermisoRol_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdPermisoRol.EditIndex = e.NewEditIndex;


            BindGrid();
        }

        /// <summary>
        /// Se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPermisoRol_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = int.Parse(grdPermisoRol.DataKeys[e.RowIndex].Value.ToString());



                CatPermisosRol_BL BL = new CatPermisosRol_BL();
                CatPermisoRolsArg Arg = new CatPermisoRolsArg();
                CatPermisosRol rol = new CatPermisosRol();

                DropDownList ddlPagina = (grdPermisoRol.Rows[e.RowIndex].FindControl("ddlPagina") as DropDownList);

                CheckBox chkActivo = (grdPermisoRol.Rows[e.RowIndex].FindControl("chkActivo") as CheckBox);
                CheckBox chkEscritura = (grdPermisoRol.Rows[e.RowIndex].FindControl("chkEscritura") as CheckBox);


                Arg.PermisosRolID = id;
                Arg.UsuarioRolID = UsuarioRolId;
                Arg.PaginaID = int.Parse(ddlPagina.SelectedValue);

                rol = BL.Obtener(UsuarioRolId, int.Parse(ddlPagina.SelectedValue));
                if (rol != null && id != rol.PermisosRolID )
                {
                    this.PopMensaje("La relación ya existe", "Catalogo Permisos Rol", controles.Opciones.SoloAceptar, controles.Icono.error);

                }
                else
                {
                    Arg.Activo = chkActivo.Checked;
                    Arg.Escritura = chkEscritura.Checked;

                    BL.Guardar(Arg);

                    grdPermisoRol.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Catalogo PermisoRols", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }




        private class CatPermisoRolsArg : CatPermisosRolArgBase_BL
        {
        }


    }
}