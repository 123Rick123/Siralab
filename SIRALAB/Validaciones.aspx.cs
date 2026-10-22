using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using SIRALAB.Controles;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_BL.negocio.Extemporaneos;
using SIRALAB_DAL;

namespace SIRALAB
{
    public partial class Validaciones : Page
    {
        private static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
        }

        public class laboratorioscnaarg : filtrolaboratoriocnaArgBase
        {

        }
        [WebMethod]
        public static string[] Busquedarfclab(string busqueda)
        {
            var clbl = new Laboratorio_BL();
            var ex = clbl.Obten(busqueda);
            object names = ex != null ? new[] { ex.RFC, ex.Nombre + " " + ex.ApellidoPaterno + " " + ex.ApellidoMaterno, ex.LaboratorioID.ToString(CultureInfo.InvariantCulture) } : null;


            return (string[])names;
        }
        [WebMethod]
        public static LaboratoriosCNA Habilitar(string busqueda)
        {
            var substrings = busqueda.Split(',');
            var clbl = new Laboratorio_BL();
            var ex = clbl.Obten(substrings[0]);
            var gu = new LaboratoriosCNA_BL();
            var args = new laboratorioscnaarg
            {
                idlaboratorio = ex.LaboratorioID,
                RFCLaboratorio = ex.RFC,
                correo = Convert.ToBoolean(substrings[1]),
                extemporaneo = Convert.ToBoolean(substrings[2]),
                fechainicial = Convert.ToDateTime(substrings[3]),
                fechafinal = Convert.ToDateTime(substrings[4])

            };
            var ed = gu.Guarda(args);
            return ed;
        }

        [WebMethod(Description = "Busqueda rfc ")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Grid Busquedarfc(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        {

            var nPagina = page;
            page -= 1;

            if (searchString != "")
                HttpContext.Current.Session["searchStringnfhh"] = searchString;

            var fil = new FiltroslaboratorioArg();
            if (searchString == "" && HttpContext.Current.Session["searchStringnfhh"] == null)
                fil.RFCLaboratorio = "";

            var oT = new Extemporaneos_BL();

            var trecords = oT.Cuenta(searchString);
            var rpagina = pageSize;
            var tpaginas = (trecords / rpagina) + (trecords % rpagina > 0 ? 1 : 0);

            if (page != 0)
            {
                HttpContext.Current.Session["pagefhh"] = page;
                HttpContext.Current.Session["npagefhh"] = nPagina;
            }

            if (HttpContext.Current.Session["ExtPagerfhh"] != null)
            {
                if (HttpContext.Current.Session["pagefhh"] != null)
                    page = (int)HttpContext.Current.Session["pagefhh"];
                if (HttpContext.Current.Session["npagefhh"] != null)
                    nPagina = (int)HttpContext.Current.Session["npagefhh"];
                HttpContext.Current.Session["ExtPagerfhh"] = null;
            }

            var ordenesT = oT.laboratoriosce(fil, HttpContext.Current.Session["pagefhh"] != null ? page : 0, rpagina, Login.UsuarioRolID, Login.LaboratorioID, sortIndex, sortDirection.ToUpper());

            var jqGrid = new Grid();
            var lOt = ordenesT.Select(p => new laboratorioscnak()
            {
                id = (int?)p.Id,
                IdLaboratorio = (int)p.IdLaboratorio,
                RfcLaboratorio = p.RfcLaboratorio,
                ValCorreo = (bool)p.ValCorreo,
                ValExtemporaneo = (bool)p.ValExtemporaneo,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin

            }).ToList();

            foreach (var ot in lOt)
            {
                var row = new Grid.Row { id = ot.id ?? 0 };
                row.cell.Add((ot.id).ToString());
                row.cell.Add((ot.IdLaboratorio.ToString(CultureInfo.InvariantCulture)));
                row.cell.Add((ot.RfcLaboratorio));
                row.cell.Add((ot.ValCorreo).ToString());
                row.cell.Add((ot.ValExtemporaneo).ToString());
                row.cell.Add((ot.FechaInicio).ToString());
                row.cell.Add((ot.FechaFin).ToString());

                jqGrid.rows.Add(row);
            }
            jqGrid.page = nPagina;
            jqGrid.records = trecords;
            jqGrid.total = tpaginas;

            return jqGrid;
        }
        public class Grid
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

            public Grid()
            {
                rows = new List<Row>();
            }
        }
    }
}