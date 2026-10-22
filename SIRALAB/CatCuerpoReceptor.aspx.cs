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
using SIRALAB.Controles;

namespace SIRALAB
{
    public partial class CatCuerpoReceptor : System.Web.UI.Page
    {
        #region Atributos

        IEnumerable<SIRALAB_DAL.CuerpoReceptor> listaCuerpoReceptor
        {
            get
            {
                return (Session["listaCuerpoReceptor"] == null) ? new List<SIRALAB_DAL.CuerpoReceptor>() : (IEnumerable<SIRALAB_DAL.CuerpoReceptor>)Session["listaCuerpoReceptor"];
            }

            set
            {
                Session["listaCuerpoReceptor"] = value;
            }
        }

        private int? CuerpoReceptorID
        {
            get { return ViewState["CuerpoReceptorID"] == null ? null : (int?)ViewState["CuerpoReceptorID"]; }
            set { ViewState["CuerpoReceptorID"] = value; }
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



        protected void GridViewCuerpoReceptor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                CuerpoReceptor_BL cpBL = new CuerpoReceptor_BL();

                if (e.CommandName.Equals("AddNew"))
                {

                    TextBox txtNombre = (TextBox)GridViewCuerpoReceptor.FooterRow.FindControl("txtNombreAdd");
                    DropDownList ddlTCR = (DropDownList)GridViewCuerpoReceptor.FooterRow.FindControl("ddlTipoCuerpoReceptorAdd");

                    if (txtNombre != null && ddlTCR != null)
                    {
                        if (!cpBL.TieneDuplicidad(txtNombre.Text, null))
                        {
                            GuardarDato(txtNombre, ddlTCR);
                            GridViewCuerpoReceptor.EditIndex = -1;
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

        protected void GridViewCuerpoReceptor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    SIRALAB_DAL.CuerpoReceptor cuerpoReceptor = (SIRALAB_DAL.CuerpoReceptor)e.Row.DataItem;

                    DropDownList ddlTCR = (DropDownList)e.Row.FindControl("ddlTipoCuerpoReceptorEdit");

                    if (ddlTCR != null)
                    {
                        CargaTipoCuerpoReceptor(ref ddlTCR);
                        ddlTCR.SelectedValue = cuerpoReceptor.TipoCuerpoReceptorID.ToString();
                    }
                }
            }

        }

        protected void GridViewCuerpoReceptor_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void GridViewCuerpoReceptor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewCuerpoReceptor.SelectedIndex = -1;
                GridViewCuerpoReceptor.PageIndex = e.NewPageIndex;
                GridViewCuerpoReceptor.DataSource = listaCuerpoReceptor;
                GridViewCuerpoReceptor.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        protected void GridViewCuerpoReceptor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCuerpoReceptor.ShowFooter = false;
            GridViewCuerpoReceptor.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridViewCuerpoReceptor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            LimpiarFormulario();
            CargarInformacion();
        }

        protected void GridViewCuerpoReceptor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CuerpoReceptor_BL cpBL = new CuerpoReceptor_BL();

                TextBox txtNombre = GridViewCuerpoReceptor.Rows[e.RowIndex].FindControl("txtNombreEdit") as TextBox;
                DropDownList ddlTCR = GridViewCuerpoReceptor.Rows[e.RowIndex].FindControl("ddlTipoCuerpoReceptorEdit") as DropDownList;
                int id = int.Parse(GridViewCuerpoReceptor.DataKeys[e.RowIndex].Value.ToString());

                if (id != null && txtNombre != null && ddlTCR != null)
                {
                    if (!cpBL.TieneDuplicidad(txtNombre.Text, id))
                    {
                        this.CuerpoReceptorID = id;
                        GuardarDato(txtNombre, ddlTCR);
                        LimpiarFormulario();
                        CargarInformacion();
                    }
                }


            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewCuerpoReceptor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = int.Parse(GridViewCuerpoReceptor.DataKeys[e.RowIndex].Value.ToString());
                //Elimina


            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkAddCuerpoReceptor_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarFormulario();
                GridViewCuerpoReceptor.ShowFooter = true;
                this.CuerpoReceptorID = null;

                BindGrid();
            }
            catch (Exception ex)
            {

            }

        }


        #endregion EventosPagina
        #region MetodosFunciones
        private void CargarInformacion()
        {
            try
            {
                CuerpoReceptor_BL cpBL = new CuerpoReceptor_BL();

                listaCuerpoReceptor = cpBL.ObtenerTodos();

                if (listaCuerpoReceptor == null)
                    listaCuerpoReceptor = new List<SIRALAB_DAL.CuerpoReceptor>();

                BindGrid();
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarInformacion(List<SIRALAB_DAL.CuerpoReceptor> listaFiltrada)
        {
            try
            {
                listaCuerpoReceptor = listaFiltrada;

                if (listaCuerpoReceptor == null)
                    listaCuerpoReceptor = new List<SIRALAB_DAL.CuerpoReceptor>();

                BindGrid();
            }
            catch (Exception ex)
            {

            }

        }

        private void LimpiarFormulario()
        {
            GridViewCuerpoReceptor.ShowFooter = false;
            GridViewCuerpoReceptor.EditIndex = -1;
            GridViewCuerpoReceptor.SelectedIndex = -1;
        }

        private void BindGrid()
        {
            GridViewCuerpoReceptor.DataSource = listaCuerpoReceptor;
            GridViewCuerpoReceptor.DataBind();

            DropDownList ddlTCR = (DropDownList)GridViewCuerpoReceptor.FooterRow.FindControl("ddlTipoCuerpoReceptorAdd");

            if (ddlTCR != null)
            {
                CargaTipoCuerpoReceptor(ref ddlTCR);

            }
            else
            {
                GridViewCuerpoReceptor.ShowFooter = false;
            }

            ActualizarFormulario();
        }

        private void GuardarDato(TextBox txtbx, DropDownList ddl)
        {
            try
            {
                CuerpoReceptor_BL cpBL = new CuerpoReceptor_BL();
                CuerpoReceptorArg argumento = new CuerpoReceptorArg();

                argumento.CuerpoReceptorID = this.CuerpoReceptorID;
                argumento.TipoCuerpoReceptorID = Convert.ToInt32(ddl.SelectedValue);
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

        protected void CargaTipoCuerpoReceptor(ref DropDownList ddlTCR)
        {
            LoadDrops.instance.TipoCuerpoReceptor(ddlTCR);
        }

        #endregion MetodosFunciones
        #region Clases
        public class CuerpoReceptorItem
        {
            public int CuerpoReceptorID { get; set; }

            public int TipoCuerpoReceptorID { get; set; }

            public string Nombre { get; set; }
        }
        public class CuerpoReceptorArg : CuerpoReceptorArgBase_BL
        {

        }

        #endregion Clases
        #region Busqueda
        protected void wucFiltroCuerpoReceptor_Buscar(object sender, FiltroCuerpoReceptorArg e)
        {
            CuerpoReceptor_BL cuerpoReceptorBL = new CuerpoReceptor_BL();
            var listaFiltrada = cuerpoReceptorBL.Filtrar(e);
            if (listaFiltrada.Count > 0)
            {
                CargarInformacion(listaFiltrada);

            }
            else
            {
                this.PopMensaje("No se encontraron resultados, con los criterios de búsqueda espeficados.", "Búsqueda");
            }

        }
        protected void wucFiltroCuerpoReceptor_Limpiar(object sender, FiltroCuerpoReceptorArg e)
        {
            CargarInformacion();
        } 
        #endregion

    }
}