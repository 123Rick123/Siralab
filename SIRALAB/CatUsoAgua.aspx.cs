using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB_DAL;

namespace SIRALAB
{
    public partial class CatUsoAgua : System.Web.UI.Page
    {
        #region Atributos

        IEnumerable<SIRALAB_DAL.CatUsoAgua> listaUsoAgua
        {
            get
            {
                return (Session["listaCatUsoAgua"] == null) ? new List<SIRALAB_DAL.CatUsoAgua>() : (IEnumerable<SIRALAB_DAL.CatUsoAgua>)Session["listaCatUsoAgua"];
            }

            set
            {
                Session["listaCatUsoAgua"] = value;
            }
        }

        private int? CatUsoAguaID
        {
            get { return ViewState["CatUsoAguaID"] == null ? null : (int?)ViewState["CatUsoAguaID"]; }
            set { ViewState["CatUsoAguaID"] = value; }
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
               // this.MensajeExcepcion(ex);
            }
        }
        
        protected void GridViewCatUsoAgua_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                CatUsoAgua_BL cpBL = new CatUsoAgua_BL();

                if (e.CommandName.Equals("AddNew"))
                {

                    TextBox txtNombre = (TextBox)GridViewCatUsoAgua.FooterRow.FindControl("txtNombreAdd");

                    if (txtNombre != null)
                    {
                        if (!cpBL.TieneDuplicidad(txtNombre.Text, null))
                        {
                            GuardarDato(txtNombre);
                            GridViewCatUsoAgua.EditIndex = -1;
                            LimpiarFormulario();
                            CargarInformacion();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewCatUsoAgua_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                }
            }
        }

        protected void GridViewCatUsoAgua_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        protected void GridViewCatUsoAgua_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewCatUsoAgua.SelectedIndex = -1;
                GridViewCatUsoAgua.PageIndex = e.NewPageIndex;
                GridViewCatUsoAgua.DataSource = listaUsoAgua;
                GridViewCatUsoAgua.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        protected void GridViewCatUsoAgua_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCatUsoAgua.ShowFooter = false;
            GridViewCatUsoAgua.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridViewCatUsoAgua_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            LimpiarFormulario();
            CargarInformacion();
        }

        protected void GridViewCatUsoAgua_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CatUsoAgua_BL cpBL = new CatUsoAgua_BL();

                TextBox txtNombre = GridViewCatUsoAgua.Rows[e.RowIndex].FindControl("txtNombreEdit") as TextBox;
                int id = int.Parse(GridViewCatUsoAgua.DataKeys[e.RowIndex].Value.ToString());

                if (id != null && txtNombre != null)
                {
                    if (!cpBL.TieneDuplicidad(txtNombre.Text, id))
                    {
                        this.CatUsoAguaID = id;
                        GuardarDato(txtNombre);
                        LimpiarFormulario();
                        CargarInformacion();
                    }
                }


            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewCatUsoAgua_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = int.Parse(GridViewCatUsoAgua.DataKeys[e.RowIndex].Value.ToString());
                //Elimina

            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkAddCatUsoAgua_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

            GridViewCatUsoAgua.ShowFooter = true;
            this.CatUsoAguaID = null;

            BindGrid();
        }


        #endregion EventosPagina

        #region MetodosFunciones
        private void CargarInformacion()
        {
            try
            {
                CatUsoAgua_BL cpBL = new CatUsoAgua_BL();

                listaUsoAgua = cpBL.ObtenerTodos();

                if (listaUsoAgua == null)
                    listaUsoAgua = new List<SIRALAB_DAL.CatUsoAgua>();

                BindGrid();
            }
            catch (Exception ex)
            {

            }

        }

        private void LimpiarFormulario()
        {
            GridViewCatUsoAgua.ShowFooter = false;
            GridViewCatUsoAgua.EditIndex = -1;
            GridViewCatUsoAgua.SelectedIndex = -1;
        }

        private void BindGrid()
        {
            GridViewCatUsoAgua.DataSource = listaUsoAgua;
            GridViewCatUsoAgua.DataBind();

            ActualizarFormulario();
        }

        private void GuardarDato(TextBox txtbx)
        {
            try
            {
                CatUsoAgua_BL cpBL = new CatUsoAgua_BL();
                UsoAguaArg argumento = new UsoAguaArg();

                argumento.UsoAguaID = this.CatUsoAguaID;
                argumento.Nombre = txtbx.Text;

                cpBL.Guardar(argumento);
            }
            catch (Exception ex)
            {

            }
        }

        private void ActualizarFormulario()
        {
            upAccion.Update();
        }


        #endregion MetodosFunciones

        #region Clases
        public class UsoAguaItem
        {
            public int UsoAguaID { get; set; }
            public string Nombre { get; set; }
        }

        public class UsoAguaArg : CatUsoAguaArgBase_BL
        {

        }

        #endregion Clases
    }
}