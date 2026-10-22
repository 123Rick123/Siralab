using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;

namespace SIRALAB.Controles
{
    public partial class wucListaOrdenCanceladas : System.Web.UI.UserControl
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

        }
        [WebMethod(Description = "Lista Ordenes de trabajo ")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static JqGridC ListaOrdenesTrabajoNoFirmadas(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        {
            var nPagina = page;
            page -= 1;
            var filtro = new FiltrosLaboratorioArg();
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


            var oT = new vw_oT();
            var trecords = oT.CuentaOrdenTrabajoC(filtro, Login.UsuarioRolID, Login.LaboratorioID);
            var rpagina = pageSize;
            var tpaginas = (trecords / rpagina) + (trecords % rpagina > 0 ? 1 : 0);

            IEnumerable<Vw_Ot_Grid> ordenesT = new List<Vw_Ot_Grid>();
            var lOt = new List<OrdenTrabajoLista1>();

            ordenesT = oT.ListaOrdenTrabajoC(filtro, page, rpagina, Login.UsuarioRolID, Login.LaboratorioID, sortIndex, sortDirection.ToUpper());
            lOt = ordenesT.Select(p => new OrdenTrabajoLista1
            {
                OrdenTrabajoId = p.OrdenTrabajoID,
                Folio = p.Informe,
                Titulo = p.Titulo,
                Anexo = p.Anexo.ToString(),
                RFCCliente = p.RFC,
                Registro = p.FechaEmision.ToShortDateString(),
                muestreado = p.EstatusID.ToString(),
                NombreLaboratorio = p.NombreLaboratorio,
                Norma = p.Norma
            }).ToList();

            var jqGrid = new JqGridC();


            foreach (var ot in lOt)
            {
                var row = new JqGridC.Row();

                row.id = ot.OrdenTrabajoId ?? 0;

                row.cell.Add((ot.OrdenTrabajoId ?? 0).ToString(CultureInfo.InvariantCulture));
                row.cell.Add((ot.Folio));
                row.cell.Add((ot.Titulo));
                row.cell.Add((ot.Anexo));
                row.cell.Add((ot.RFCCliente));
                row.cell.Add((ot.Norma));
                row.cell.Add((ot.Registro));


                jqGrid.rows.Add(row);
            }
            jqGrid.page = nPagina;
            jqGrid.records = trecords;
            jqGrid.total = tpaginas;

            return jqGrid;
        }

        public class JqGridC
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

            public JqGridC()
            {
                rows = new List<Row>();
            }
        }
    }
}