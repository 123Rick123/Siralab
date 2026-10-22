using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SIRALAB.controles;
using SIRALAB.Controles;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB
{
    public partial class CatDiasInhabiles : Page
    {
        #region Atributos

        IEnumerable<CatDiaInhabil> listaDiaInhabil
        {
            get
            {
                return (Session["listaCatDiaInhabil"] == null) ? new List<CatDiaInhabil>() : (IEnumerable<CatDiaInhabil>)Session["listaCatDiaInhabil"];
            }

            set
            {
                Session["listaCatDiaInhabil"] = value;
            }
        }

        private int? CatDiaInhabilID
        {
            get { return ViewState["CatDiaInhabilID"] == null ? null : (int?)ViewState["CatDiaInhabilID"]; }
            set { ViewState["CatDiaInhabilID"] = value; }
        }
        #endregion Atributos

        #region EventosPagina

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Main)Master.Master;
            master.MensajeDevolucion += Master_MensajeDevolucion; ;
            try
            {
                if (!Page.IsPostBack)
                {
                    CargarInformacion();
                    GridViewCatDiasInhabiles.ShowFooter = false;

                }
            }
            catch (Exception ex)
            {
                // this.MensajeExcepcion(ex);
            }
        }

        private void Master_MensajeDevolucion(object sender, MensajeArgs e)
        {
            var comando = e.Command;
            if (comando != null)
            {
                switch (comando)
                {
                    case "EliminarDía":
                        var id = int.Parse(e.Args[0]);
                        CatDiaInhabil_BL cpBL = new CatDiaInhabil_BL();

                        if (cpBL.Elimina(id))
                        {
                            this.PopMensaje("Se ha eliminado el día inhábil", "Días inhábiles");
                            CargarInformacion();
                        }
                        else
                        {
                            this.PopMensaje("Ocurrió un error al intentar eliminar el día inhábil", "Días inhábiles");
                        }
                        break;
                }
            }
        }

        protected void GridViewCatDiasInhabiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                CatDiaInhabil_BL cpBL = new CatDiaInhabil_BL();

                if (e.CommandName.Equals("AddNew"))
                {

                    TextBox txtDiaInhabil = (TextBox)GridViewCatDiasInhabiles.FooterRow.FindControl("txtDiaInhabilAdd");

                    if (txtDiaInhabil != null)
                    {
                        if (!cpBL.TieneDuplicidad(Convert.ToDateTime(txtDiaInhabil.Text), null))
                        {
                            GuardarDato(Convert.ToDateTime(txtDiaInhabil.Text));
                            GridViewCatDiasInhabiles.EditIndex = -1;
                            LimpiarFormulario();
                            CargarInformacion();
                        }
                        else
                        {
                            this.PopMensaje("La fecha ya ha sido capturada.", "Días inhábiles");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewCatDiasInhabiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            //    {
            //        CalendarExtender cal = (CalendarExtender)e.Row.FindControl("ce_DiaInhabilEdit");
            //        cal.StartDate = DateTime.Now;
            //    }

            //}
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    CalendarExtender cal = (CalendarExtender)e.Row.FindControl("ce_DiaInhabilAdd");

            //    cal.StartDate = DateTime.Now;
            //}
        }

        protected void GridViewCatDiasInhabiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GridViewCatDiasInhabiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewCatDiasInhabiles.SelectedIndex = -1;
                GridViewCatDiasInhabiles.PageIndex = e.NewPageIndex;
                GridViewCatDiasInhabiles.DataSource = listaDiaInhabil;
                GridViewCatDiasInhabiles.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GridViewCatDiasInhabiles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCatDiasInhabiles.ShowFooter = false;
            GridViewCatDiasInhabiles.EditIndex = e.NewEditIndex;
            BindGrid();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setDatepeickers", "setDatepicker();", true);

        }

        protected void GridViewCatDiasInhabiles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            LimpiarFormulario();
            CargarInformacion();
        }

        protected void GridViewCatDiasInhabiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CatDiaInhabil_BL cpBL = new CatDiaInhabil_BL();

                TextBox txtDiaInhabil = GridViewCatDiasInhabiles.Rows[e.RowIndex].FindControl("txtDiaInhabilEdit") as TextBox;
                int id = int.Parse(GridViewCatDiasInhabiles.DataKeys[e.RowIndex].Value.ToString());

                if (id != null && txtDiaInhabil != null)
                {
                    if (!cpBL.TieneDuplicidad(Convert.ToDateTime(txtDiaInhabil.Text), id))
                    {
                        CatDiaInhabilID = id;
                        GuardarDato(Convert.ToDateTime(txtDiaInhabil.Text));
                        LimpiarFormulario();
                        CargarInformacion();
                    }
                    else
                    {
                        this.PopMensaje("La fecha ya ha sido capturada.", "Días inhábiles");
                    }
                }


            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewCatDiasInhabiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Elimina
            try
            {
                int id = int.Parse(GridViewCatDiasInhabiles.DataKeys[e.RowIndex].Value.ToString());
                Eliminar(id);
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkAddCatDiaInhabil_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

            GridViewCatDiasInhabiles.ShowFooter = true;
            CatDiaInhabilID = null;

            BindGrid();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setDatepeickers", "setDatepicker();", true);
        }


        #endregion EventosPagina

        #region MetodosFunciones
        private void CargarInformacion()
        {
            try
            {
                var cpBL = new CatDiaInhabil_BL();

                listaDiaInhabil = cpBL.ObtenerTodos() ?? new List<CatDiaInhabil>();
                BindGrid();
                

            }
            catch (Exception ex)
            {

            }

        }

        private void LimpiarFormulario()
        {
            GridViewCatDiasInhabiles.ShowFooter = false;
            GridViewCatDiasInhabiles.EditIndex = -1;
            GridViewCatDiasInhabiles.SelectedIndex = -1;
        }

        private void BindGrid()
        {
            GridViewCatDiasInhabiles.DataSource = listaDiaInhabil;
            GridViewCatDiasInhabiles.DataBind();

            ActualizarFormulario();
        }

        private void GuardarDato(DateTime dia)
        {
            try
            {
                CatDiaInhabil_BL cpBL = new CatDiaInhabil_BL();
                DiaInhabilArg argumento = new DiaInhabilArg();

                argumento.DiaInhabilId = CatDiaInhabilID;
                argumento.DiaInhabil = dia;

                cpBL.Guardar(argumento);
            }
            catch (Exception ex)
            {

            }
        }
        private void Eliminar(int idDiaInhabil)
        {
            try
            {
                this.PopMensaje("¿Desea eliminar el día inhábil?", "Días inhábiles", Opciones.SiNo, Icono.question,
                    "EliminarDía", new[] {idDiaInhabil.ToString()});
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
        public class DiaInhabilItem
        {
            public int DiaInhabilID { get; set; }
            public string Nombre { get; set; }
        }

        public class DiaInhabilArg : CatDiaInhabilArgBase_BL
        {

        }

        protected void lnkbtnback_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(1);
            Response.Redirect(regreso);
        }

        #endregion Clases


    }
}