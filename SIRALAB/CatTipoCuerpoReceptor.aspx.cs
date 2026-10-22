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
    public partial class CatTipoCuerpoReceptor : System.Web.UI.Page
    {
        #region Atributos

        IEnumerable<SIRALAB_DAL.CatTipoCuerpoReceptor> listaCatTipoCuerpoReceptor
        {
            get
            {
                return (Session["listaCatTipoCuerpoReceptor"] == null) ? new List<SIRALAB_DAL.CatTipoCuerpoReceptor>() : (IEnumerable<SIRALAB_DAL.CatTipoCuerpoReceptor>)Session["listaCatTipoCuerpoReceptor"];
            }

            set
            {
                Session["listaCatTipoCuerpoReceptor"] = value;
            }
        }

        private int? CatTipoCuerpoReceptorID
        {
            get { return ViewState["CatTipoCuerpoReceptorID"] == null ? null : (int?)ViewState["CatTipoCuerpoReceptorID"]; }
            set { ViewState["CatTipoCuerpoReceptorID"] = value; }
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

        protected void GridViewCatTipoCuerpoReceptor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                CatTipoCuerpoReceptor_BL cpBL = new CatTipoCuerpoReceptor_BL();

                if (e.CommandName.Equals("AddNew"))
                {

                    TextBox txtNombre = (TextBox)GridViewCatTipoCuerpoReceptor.FooterRow.FindControl("txtNombreAdd");

                    if (txtNombre != null)
                    {
                        if (!cpBL.TieneDuplicidad(txtNombre.Text, null))
                        {
                            GuardarDato(txtNombre);
                            GridViewCatTipoCuerpoReceptor.EditIndex = -1;
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

        protected void GridViewCatTipoCuerpoReceptor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                }
            }
        }

        protected void GridViewCatTipoCuerpoReceptor_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void GridViewCatTipoCuerpoReceptor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewCatTipoCuerpoReceptor.SelectedIndex = -1;
                GridViewCatTipoCuerpoReceptor.PageIndex = e.NewPageIndex;
                GridViewCatTipoCuerpoReceptor.DataSource = listaCatTipoCuerpoReceptor;
                GridViewCatTipoCuerpoReceptor.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        protected void GridViewCatTipoCuerpoReceptor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCatTipoCuerpoReceptor.ShowFooter = false;
            GridViewCatTipoCuerpoReceptor.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridViewCatTipoCuerpoReceptor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            LimpiarFormulario();
            CargarInformacion();
        }

        protected void GridViewCatTipoCuerpoReceptor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CatTipoCuerpoReceptor_BL cpBL = new CatTipoCuerpoReceptor_BL();

                TextBox txtNombre = GridViewCatTipoCuerpoReceptor.Rows[e.RowIndex].FindControl("txtNombreEdit") as TextBox;
                int id = int.Parse(GridViewCatTipoCuerpoReceptor.DataKeys[e.RowIndex].Value.ToString());

                if (id != null && txtNombre != null)
                {
                    if (!cpBL.TieneDuplicidad(txtNombre.Text, id))
                    {
                        this.CatTipoCuerpoReceptorID = id;
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

        protected void GridViewCatTipoCuerpoReceptor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = int.Parse(GridViewCatTipoCuerpoReceptor.DataKeys[e.RowIndex].Value.ToString());
                //Elimina

                
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkAddCatTipoCuerpoReceptor_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

            GridViewCatTipoCuerpoReceptor.ShowFooter = true;
            this.CatTipoCuerpoReceptorID = null;

            BindGrid();
        }


        #endregion EventosPagina
        #region MetodosFunciones
        private void CargarInformacion()
        {
            try
            {
                CatTipoCuerpoReceptor_BL cpBL = new CatTipoCuerpoReceptor_BL();

                listaCatTipoCuerpoReceptor = cpBL.ObtenerTodos();

                if (listaCatTipoCuerpoReceptor == null)
                    listaCatTipoCuerpoReceptor = new List<SIRALAB_DAL.CatTipoCuerpoReceptor>();

                BindGrid();
            }
            catch (Exception ex)
            {

            }

        }

        private void LimpiarFormulario()
        {
            GridViewCatTipoCuerpoReceptor.ShowFooter = false;
            GridViewCatTipoCuerpoReceptor.EditIndex = -1;
            GridViewCatTipoCuerpoReceptor.SelectedIndex = -1;
        }

        private void BindGrid()
        {
            GridViewCatTipoCuerpoReceptor.DataSource = listaCatTipoCuerpoReceptor;
            GridViewCatTipoCuerpoReceptor.DataBind();

            ActualizarFormulario();
        }

        private void GuardarDato(TextBox txtbx)
        {
            try
            {
                CatTipoCuerpoReceptor_BL cpBL = new CatTipoCuerpoReceptor_BL();
                CatTipoCuerpoReceptorArg argumento = new CatTipoCuerpoReceptorArg();

                argumento.TipoCuerpoReceptorID = this.CatTipoCuerpoReceptorID;
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
        public class CatTipoCuerpoReceptorItem
        {
            public int TipoCuerpoReceptorID { get; set; }
            public string Nombre { get; set; }
        }

        public class CatTipoCuerpoReceptorArg : CatTipoCuerpoReceptorArgBase_BL
        {

        }

        #endregion Clases
    }
}