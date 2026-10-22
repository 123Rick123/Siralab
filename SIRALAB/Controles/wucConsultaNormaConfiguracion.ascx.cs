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
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucConsultaNormaConfiguracion : System.Web.UI.UserControl
    {
        #region Atributos

        [Browsable(true)]
        public event EventHandler Seleccionar;

        public int? NormaIdCapturado
        {
            set { Session["NormaIdCapturado"] = value; }
            get { return Session["NormaIdCapturado"] == null ? null : (int?)Session["NormaIdCapturado"]; }
        }

        public int? NormaConfiguracionIdSelecionado
        {
            set { Session["NormaConfiguracionIdSelecionado"] = value; }
            get { return Session["NormaConfiguracionIdSelecionado"] == null ? null : (int?)Session["NormaConfiguracionIdSelecionado"]; }
        }

        private IEnumerable<SIRALAB_DAL.usp_ParametrosMetodosNorma_Result> ListaNormaConfiguracion
        {
            get
            {
                return (Session["listaNormaConfiguracion"] == null) ? new List<SIRALAB_DAL.usp_ParametrosMetodosNorma_Result>() : (IEnumerable<SIRALAB_DAL.usp_ParametrosMetodosNorma_Result>)Session["listaNormaConfiguracion"];
            }

            set
            {
                Session["listaNormaConfiguracion"] = value;
            }
        }

        public enumOperacionesWUC ModoOperacion
        {
            get { return ViewState["NormaConfiguracionID"] == null ? enumOperacionesWUC.Edicion_Cosulta : (enumOperacionesWUC)ViewState["NormaConfiguracionID"]; }
            set { ViewState["NormaConfiguracionID"] = value; }
        }

        #endregion Atributos

        #region EventosPagina


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnEdit_Click(object sender, CommandEventArgs e)
        {
            string obId = e.CommandName.ToString();
            int id = 0;

            if (int.TryParse(obId, out id))
            {
                NormaConfiguracionIdSelecionado = id;
                if (Seleccionar != null)
                    Seleccionar(this, null);
            }
            BindGrid();
        }

        protected void GridViewNormaConfiguracion_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewNormaConfiguracion.EditIndex = e.NewEditIndex;

           
            BindGrid();
        }
        protected void GridViewNormaConfiguracion_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridViewNormaConfiguracion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewNormaConfiguracion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                NormaConfiguracion_BL ncf_BL = new NormaConfiguracion_BL();

                if (GridViewNormaConfiguracion.SelectedDataKey != null)
                {

                    string obParametroId = GridViewNormaConfiguracion.SelectedDataKey.Values[0].ToString();
                    string obCuerpoReceptorId = GridViewNormaConfiguracion.SelectedDataKey.Values[1].ToString();
                    string obUsoAguaId = GridViewNormaConfiguracion.SelectedDataKey.Values[2].ToString();
                    string obPromedioId = GridViewNormaConfiguracion.SelectedDataKey.Values[3].ToString();
                    
                    int ParametroId = 0;
                    int CuerpoReceptorId = 0;
                    int UsoAguaId = 0;
                    int PromedioId = 0;

                    if (int.TryParse(obParametroId, out ParametroId) && int.TryParse(obCuerpoReceptorId, out CuerpoReceptorId) &&
                        int.TryParse(obUsoAguaId, out UsoAguaId) && int.TryParse(obPromedioId, out PromedioId))
                    {
                        NormaConfiguracionIdSelecionado = ncf_BL.Obtener_NormaConfiguracion((int)NormaIdCapturado, ParametroId, CuerpoReceptorId, UsoAguaId, PromedioId).NormaConfiguracionID;
                        if (Seleccionar != null)
                            Seleccionar(this, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        protected void GridViewNormaConfiguracion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewNormaConfiguracion.SelectedIndex = -1;
                GridViewNormaConfiguracion.PageIndex = e.NewPageIndex;
                GridViewNormaConfiguracion.DataSource = ListaNormaConfiguracion;
                GridViewNormaConfiguracion.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion EventosPagina

        #region MetodosFunciones

        private void BindGrid()
        {
            try
            {
                GridViewNormaConfiguracion.DataSource = ListaNormaConfiguracion;
                GridViewNormaConfiguracion.DataBind();

                if (GridViewNormaConfiguracion.Rows.Count > 0)
                {
                    if (ModoOperacion.Equals(enumOperacionesWUC.Consulta))
                    {
                        GridViewNormaConfiguracion.Columns[GridViewNormaConfiguracion.Columns.Count - 1].Visible = false;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }

        }

        public void CargarInformacion(int? NormaId)
        {
            try
            {
                NormaConfiguracion_BL cpBL = new NormaConfiguracion_BL();

                NormaIdCapturado = NormaId;

                ListaNormaConfiguracion = cpBL.ObtenerNormaConfiguracionByNormaID((int)NormaId);

                if (ListaNormaConfiguracion == null)
                    ListaNormaConfiguracion = new List<SIRALAB_DAL.usp_ParametrosMetodosNorma_Result>();

                BindGrid();

            }
            catch (Exception ex)
            {

            }
        }

        public void CargarListaFiltrada(List<usp_ParametrosMetodosNorma_Result> lista)
        {
            try {
                ListaNormaConfiguracion = lista;

                if (ListaNormaConfiguracion == null)
                    ListaNormaConfiguracion = new List<SIRALAB_DAL.usp_ParametrosMetodosNorma_Result>();

                
                BindGrid();
            }
            catch(Exception ex)
            {
            }
        }


        private void LimpiarFormulario()
        {
            GridViewNormaConfiguracion.SelectedIndex = -1;
        }

        #endregion MetodosFunciones

      
    }
}