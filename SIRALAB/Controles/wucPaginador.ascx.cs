    using SIRALAB_BL.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucPaginador : System.Web.UI.UserControl
    {
        public event EventHandler<PageArgs> PageChange;

        public int CurrentPage
        {
            get { int s = ViewState["Paginador"] != null ? (int)ViewState["Paginador"] : 1; return s; }
            set { ViewState["Paginador"] = value; }
        }

        private int TotalRows { get; set; }

        public int PageSize
        {
            get { return int.Parse(ddlPageSize.SelectedValue); }
        }

        private int TotalPages
        {
            get { return (TotalRows / PageSize) + (TotalRows % PageSize > 0 ? 1 : 0); }
        }

        private int CurrentBlock
        {
            get
            {
                return (CurrentPage / BlockSize) + (CurrentPage % BlockSize > 0 ? 1 : 0);
            }
        }

        private int BlockSize
        {
            get
            {
                ApplicationSettings applicationSettings = new ApplicationSettings();
                return applicationSettings.PaginationBlockSize;
            }
        }

        public bool ShowPrevious
        {
            get
            {
                if (CurrentBlock == 1) return false;

                return true;
            }
        }

        public bool ShowNext
        {
            get
            {
                var totalBlocks = (TotalPages / BlockSize) + (TotalPages % BlockSize > 0 ? 1 : 0);
                if (CurrentBlock < totalBlocks) return true;

                return false;
            }
        }

        public int ShowFinal
        {
            get
            {
                return TotalPages;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CurrentPage = 1;
            }
        }

        public void Initialize(int currentPage, int totalRows)
        {
            var blockSize = BlockSize;
            CurrentPage = currentPage;
            TotalRows = totalRows;

            var startPage = (CurrentBlock - 1) * blockSize + 1;
            var endPage = startPage + blockSize - 1;

            if (endPage > TotalPages)
            {
                blockSize = blockSize - (endPage - TotalPages);
                endPage = TotalPages;
            }

            lblPaginaActual.Visible = TotalPages > 0;
            ddlPageSize.Visible = TotalPages > 0;
            var listPages = Enumerable.Range(startPage, blockSize).Select(p => new Pagina { Page = p, ActivePage = p == currentPage });

            lblPaginaActual.Text = TotalPages > 0 ? $"Página { currentPage } de { TotalPages }" : "";
            lblRegistros.Text = $"{ totalRows } registros";
            ddlTotalRegistros.Text = TotalPages > 0 ? "Registros por página" : "";

            rptPaginas.DataSource = listPages;
            rptPaginas.DataBind();
            Actualizar();
        }

        public void Actualizar()
        {
            upPaginador.Update();
        }

        protected void lnkPage_OnClick(object sender, EventArgs e)
        {
            var link = (LinkButton)sender;
            CurrentPage = int.Parse(link.CommandArgument);

            if (PageChange != null) PageChange.Invoke(this, new PageArgs { CurrentPage = CurrentPage });
        }

        protected void ddlPageSize_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            if (PageChange != null) PageChange.Invoke(this, new PageArgs { CurrentPage = CurrentPage });
        }

        protected void lnkSiguiente_OnClick(object sender, EventArgs e)
        {
            CurrentPage = (CurrentBlock * BlockSize) + 1;
            if (PageChange != null) PageChange.Invoke(this, new PageArgs { CurrentPage = CurrentPage });
        }

        protected void lnkAnterior_OnClick(object sender, EventArgs e)
        {
            CurrentPage = (CurrentBlock - 1) * BlockSize;
            if (PageChange != null) PageChange.Invoke(this, new PageArgs { CurrentPage = CurrentPage });
        }

        protected void lnkInicio_OnClick(object sender, EventArgs e)
        {
            var link = (LinkButton)sender;
            CurrentPage = 1;

            if (PageChange != null) PageChange.Invoke(this, new PageArgs { CurrentPage = CurrentPage });
        }

        protected void lnkFin_OnClick(object sender, EventArgs e)
        {
            var link = (LinkButton)sender;
            CurrentPage = int.Parse(link.CommandArgument);

            if (PageChange != null) PageChange.Invoke(this, new PageArgs { CurrentPage = CurrentPage });
        }
    }
}