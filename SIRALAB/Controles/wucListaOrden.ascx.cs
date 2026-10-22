using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Script.Services;

namespace SIRALAB.Controles
{
    public partial class wucListaOrden : System.Web.UI.UserControl
    {

        [Browsable(true)]
        public event EventHandler<SIRALAB_DAL.OrdenTrabajo> Eliminar;
        #region Propiedades
        public int OrdenTrabajoID
        {
            get { return (int)Session["OrdenTrabajoID_"]; }
            set { Session["OrdenTrabajoID_"] = value; }
        }
        public IEnumerable<OrdenTrabajoLista> ListaOrdentrabajo
        {
            get { return (IEnumerable<OrdenTrabajoLista>)Session["ListaOrdentrabajo_"]; }
            set { Session["ListaOrdentrabajo_"] = value; }
        }
        public static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }
        private FiltrosLaboratorioArg filtro
        {
            get
            {
                return Session["Controles.FiltroOrdenes"] != null ? (FiltrosLaboratorioArg)Session["Controles.FiltroOrdenes"] : new FiltrosLaboratorioArg
                {
                    RFCCliente = "",
                    Numero = "",
                    FechaInicial = null,
                    FechaFinal = null,
                    Firmada = false
                };
            }
            set { Session["Controles.FiltroOrdenes"] = value; }
        }

        #region Paginación
        private int total_registros { get { return (int)ViewState["total_registros"]; } set { ViewState["total_registros"] = value; } }
        private int registros_por_pagina { get { return (int)ViewState["registros_por_pagina"]; } set { ViewState["registros_por_pagina"] = value; } }
        private int total_paginas { get { return (int)ViewState["total_paginas"]; } set { ViewState["total_paginas"] = value; } }
        private int paginas_por_bloque { get { return (int)ViewState["paginas_por_bloque"]; } set { ViewState["paginas_por_bloque"] = value; } }
        private int total_bloques { get { return (int)ViewState["total_bloques"]; } set { ViewState["total_bloques"] = value; } }
        public int bloque_actual { get { return (int)ViewState["bloque_actual"]; } set { ViewState["bloque_actual"] = value; } }
        public int pagina_actual { get { return (int)ViewState["pagina_actual"]; } set { ViewState["pagina_actual"] = value; } }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.bloque_actual = 0;
                this.pagina_actual = 0;
                CargaInformacion_(this.filtro);
            }
        }

        /*public void CargaInformacion()
        {
            OrdenTrabajo_BL ordentrabajo_bl = new OrdenTrabajo_BL();
            this.ListaOrdentrabajo = new List<OrdenTrabajoLista>();

            List<SIRALAB_DAL.OrdenTrabajo> orden = new List<SIRALAB_DAL.OrdenTrabajo>();

            //Validamos de que perfil se trata si de recaudación o de laboratorio
            if (this.Login.UsuarioRolID == 2)
                orden = ordentrabajo_bl.Lista_OrdenTrabajo();
            else
                orden = ordentrabajo_bl.Lista_OrdenTrabajo().Where(x => x.LaboratorioID == this.Login.LaboratorioID).ToList();

            foreach (var i in orden)
            {
                ListaOrdentrabajo.Add(new OrdenTrabajoLista
                {
                    OrdenTrabajoId = i.OrdenTrabajoID,
                    Folio = i.NumeroInforme,
                    Titulo = i.Consecion.Titulo,
                    Anexo = i.Anexo.Anexo1.ToString(),
                    RFCCliente = i.Cliente.RFC,
                    Registro = i.FechaEmision.ToShortDateString(),
                    muestreado = i.EstatusID.ToString()
                });
            }
            LlenaGrid();
        }*/
        [WebMethod(Description = "Lista Ordenes de trabajo ")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static JQGrid ListaOrdenesTrabajoP_Json(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        {
            int nPagina = page;
            page -= 1;
            FiltrosLaboratorioArg filtro = new FiltrosLaboratorioArg();
            filtro.NoFirmada = true;
            filtro.Firmada = true;

            if (_search)
            {
                switch (searchField)
                {
                    case "RFCCliente": filtro.RFCCliente = searchString;
                        break;
                    case "Folio": filtro.Numero = searchString;
                        break;
                    case "Titulo": filtro.Titulo = searchString;
                        break;
                    case "Anexo": filtro.Anexo = searchString;
                        break;
                    case "Norma": filtro.Norma = searchString;
                        break;
                    case "Registro": filtro.FechaInicial = DateTime.Parse(searchString);
                        filtro.FechaFinal = DateTime.Parse(searchString);
                        break;
                }
            }

            OrdenTrabajo_BL ordentrabajo_bl = new OrdenTrabajo_BL();
            List<OrdenTrabajoLista> ListaOrdentrabajo = new List<OrdenTrabajoLista>();

            IEnumerable<SIRALAB_DAL.OrdenTrabajo> Ordenes = new List<SIRALAB_DAL.OrdenTrabajo>();


            int _totalrecords = ordentrabajo_bl.Cuenta_OrdenTrabajo_JS(filtro, Login.UsuarioRolID, Login.LaboratorioID);
            int _registros_por_pagina = pageSize;
            int _total_paginas = (_totalrecords / _registros_por_pagina) + (_totalrecords % _registros_por_pagina > 0 ? 1 : 0);

            Ordenes = ordentrabajo_bl.Lista_OrdenTrabajo(filtro, page, _registros_por_pagina, Login.UsuarioRolID, Login.LaboratorioID, sortIndex, sortDirection.ToUpper());

            ListaOrdentrabajo = Ordenes.Select(p => new OrdenTrabajoLista
            {
                OrdenTrabajoId = p.OrdenTrabajoID,
                Folio = p.NumeroInforme,
                Titulo = p.Consecion.Titulo,
                Anexo = p.Anexo.Anexo1.ToString(),
                RFCCliente = p.Cliente.RFC,
                Registro = p.FechaEmision.ToShortDateString(),
                muestreado = p.EstatusID.ToString(),
                NombreLaboratorio = p.Laboratorio.RFC,
                Norma = p.Norma.Nombre
            }).ToList(); ;






            JQGrid jqGrid = new JQGrid();


            foreach (OrdenTrabajoLista ot in ListaOrdentrabajo)
            {
                JQGrid.Row row = new JQGrid.Row();

                row.id = ot.OrdenTrabajoId ?? 0;

                row.cell.Add((ot.OrdenTrabajoId ?? 0).ToString());
                row.cell.Add((ot.Folio).ToString());
                row.cell.Add((ot.Titulo).ToString());
                row.cell.Add((ot.Anexo).ToString());
                row.cell.Add((ot.RFCCliente).ToString());
                row.cell.Add((ot.Norma).ToString());
                row.cell.Add((ot.Registro).ToString());


                jqGrid.rows.Add(row);
            }
            jqGrid.page = nPagina;
            jqGrid.records = _totalrecords;
            jqGrid.total = _total_paginas;

            return jqGrid;
        }

        public class JQGrid
        {
            public class Row
            {
                public int id { get; set; }
                public List<string> cell { get; set; }

                public Row()
                {
                    cell = new List<string>();
                }
            }

            public int page { get; set; }
            public int total { get; set; }
            public int records { get; set; }
            public List<Row> rows { get; set; }

            public JQGrid()
            {
                rows = new List<Row>();
            }
        }
        public void CargaInformacion_(FiltrosLaboratorioArg Args)
        {
            this.filtro = Args;
            OrdenTrabajo_BL ordentrabajo_bl = new OrdenTrabajo_BL();
            this.ListaOrdentrabajo = new List<OrdenTrabajoLista>();

            IEnumerable<SIRALAB_DAL.OrdenTrabajo> Ordenes = new List<SIRALAB_DAL.OrdenTrabajo>();

            //Validamos de que perfil se trata si de recaudación o de laboratorio
            this.total_registros = ordentrabajo_bl.Cuenta_OrdenTrabajo(this.filtro, Login.UsuarioRolID, Login.LaboratorioID);
            this.registros_por_pagina = 20;
            this.paginas_por_bloque = 12;
            this.total_paginas = (this.total_registros / this.registros_por_pagina) + (this.total_registros % this.registros_por_pagina > 0 ? 1 : 0);
            this.total_bloques = (this.total_paginas / this.paginas_por_bloque) + (this.total_paginas % this.paginas_por_bloque > 0 ? 1 : 0);

            Ordenes = ordentrabajo_bl.Lista_OrdenTrabajo(this.filtro, this.pagina_actual, this.registros_por_pagina, Login.UsuarioRolID, Login.LaboratorioID);

            ListaOrdentrabajo = Ordenes.Select(p => new OrdenTrabajoLista
            {
                OrdenTrabajoId = p.OrdenTrabajoID,
                Folio = p.NumeroInforme,
                Titulo = p.Consecion.Titulo,
                Anexo = p.Anexo.Anexo1.ToString(),
                RFCCliente = p.Cliente.RFC,
                Registro = p.FechaEmision.ToShortDateString(),
                muestreado = p.EstatusID.ToString(),
                NombreLaboratorio = p.Laboratorio.RFC,
                Norma = p.Norma.Nombre
            }).ToList();

            /*
            var listado = new List<OrdenTrabajoLista>();
            listado.AddRange(ListaOrdentrabajo);

            gvOrdenes.DataSource = listado;
            gvOrdenes.DataBind();
            */
            ordentrabajo_bl.Dispose();
            Paginar();
        }

        private void Paginar()
        {
            //rptOrdenes.DataSource = this.ListaOrdentrabajo;
            //rptOrdenes.DataBind();

            #region Paginación
            int inicioBloque = this.bloque_actual * this.paginas_por_bloque;

            IEnumerable<int> paginas = Enumerable.Range(inicioBloque, this.paginas_por_bloque);
            if (this.bloque_actual == this.total_bloques - 1)
            {
                var residuo = this.total_paginas % this.paginas_por_bloque;
                if (residuo != 0)
                    paginas = Enumerable.Range(inicioBloque, residuo);
            }

            IEnumerable<PageItem> pages = paginas.Select(p => new PageItem { Page = p });

            //rptPaginador.DataSource = pages;
            //rptPaginador.DataBind();
            #endregion
        }

        protected void lnkbtnSeleccionar_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(((LinkButton)sender).CommandArgument);
            Session["BackUri"] = Request.Url.AbsoluteUri;

            //Asiganmos el valor del Id de la orden de trabajo, para pasarla la pagina orden de trabajo.
            this.OrdenTrabajoID = Id;
            Response.Redirect("~/OrdenTrabajo.aspx");
        }
        protected void lnkbtnBorrar_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(((LinkButton)sender).CommandArgument);
            Session["BackUri"] = Request.Url.AbsoluteUri;

            OrdenTrabajo_BL orden_bl = new OrdenTrabajo_BL();
            var orden = orden_bl.Obten(Id);
            Eliminar(this, orden);
        }

        protected void lnkbPagina_Click(object sender, EventArgs e)
        {
            var lnk = (LinkButton)sender;
            var pagina = int.Parse(lnk.CommandArgument);

            this.pagina_actual = pagina;
            CargaInformacion_(this.filtro);
        }
        protected void lnkbSiguente_Click(object sender, EventArgs e)
        {
            if (this.bloque_actual < this.total_bloques - 1)
                this.bloque_actual += 1;

            Paginar();
        }
        protected void lnkbAnterior_Click(object sender, EventArgs e)
        {
            if (this.bloque_actual > 0)
                this.bloque_actual -= 1;

            Paginar();
        }

        protected void rptPaginador_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var lnkAnterior = (LinkButton)e.Item.FindControl("lnkbAnterior");
            var lnkSiguiente = (LinkButton)e.Item.FindControl("lnkbSiguente");
            var lnkPagina = (LinkButton)e.Item.FindControl("lnkbPagina");
            var item = (PageItem)e.Item.DataItem;
            if (lnkAnterior != null)
            {
                lnkAnterior.Visible = (this.bloque_actual != 0);
            }
            if (lnkSiguiente != null)
            {
                lnkSiguiente.Visible = (this.bloque_actual != (this.total_bloques - 1));
            }
            if (lnkPagina != null && item != null)
            {
                if (item.Page == this.pagina_actual)
                {
                    lnkPagina.Attributes.Add("selected-page", "true");
                    // CssClass='<%#: this.pagina_actual == Item.Page ? "page-selected" : "" %>'
                }
            }
        }



    }

    public class PageItem
    {
        public int Page { get; set; }
    }
}