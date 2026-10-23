using System.Data;
using System.Drawing;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.Style;
using SIRALAB.Controles;
using SIRALAB_BL;
using SIRALAB_BL.negocio.ordentrabajo;
using SIRALAB_DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SIRALAB.controles;
using SIRALAB.Common;
using SIRALAB.FIEL;
using SIRALAB_BL.common;
using SIRALAB_BL.negocio.analisis;
using SIRALAB_DAL.Extensiones;
using System.Transactions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SIRALAB.Queue;

namespace SIRALAB
{
    public partial class Orden : System.Web.UI.Page
    {
        #region Propiedades

        public static int OrdenTrabajoID
        {
            get { return (int)HttpContext.Current.Session["OrdenTrabajoID_"] == 0 ? 0 : (int)HttpContext.Current.Session["OrdenTrabajoID_"]; }
            set { HttpContext.Current.Session["OrdenTrabajoID_"] = value; }
        }
        public SIRALAB_DAL.Anexo Anexo
        {
            get { return (SIRALAB_DAL.Anexo)Session["AnexoConsulta_"]; }
            set { Session["AnexoConsulta_"] = value; }
        }
        public SIRALAB_DAL.Cliente Cliente
        {
            get { return (SIRALAB_DAL.Cliente)Session["ClienteConsulta_"]; }
            set { Session["ClienteConsulta_"] = value; }
        }
        public static SIRALAB_DAL.OrdenTrabajo OT_del
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)HttpContext.Current.Session["OT_del"]; }
            set { HttpContext.Current.Session["OT_del"] = value; }
        }
        public static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }
        private int AnalisisID { set { ViewState["AnalisisID"] = value; } get { return int.Parse(ViewState["AnalisisID"].ToString()); } }
        public string Excel
        {
            get { return Session["ListaExportar"] == null ? string.Empty : (string)Session["ListaExportar"]; }
            set { Session["ListaExportar"] = value; }
        }

        public FiltroOrdenTrabajo Filtro
        {
            set { Session["FiltroOrdenTrabajo"] = value; }
            get { return Session["FiltroOrdenTrabajo"] == null ? new FiltroOrdenTrabajo() : (FiltroOrdenTrabajo)Session["FiltroOrdenTrabajo"]; }
        }
        #endregion

        static readonly List<string> SesionList = new List<string>();
        static readonly List<string> SesionListnf = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)this.Master.Master;
            main.MensajeDevolucion += main_MensajeDevolucion;
         

            if (!IsPostBack)
            {
                LoadDrops.instance.Norma(ddlNorma);
                if (Login.CatUsuarioRol.UsuarioRolID != 1)
                {
                    btnNuevo.Visible = false;
                    divLaboratorio.Visible = true;
                    LoadDrops.instance.Laboratorio(ddlLaboratorio);
                }
                else if (Login.CatUsuarioRol.UsuarioRolID == 1)
                {
                    rbFirmados.Checked = false;
                    rbNoFirmados.Checked = true;
                    Limpiar();
                    
                }

                SetFiltro();
                Buscar(ObtenerFiltro());

                Session["BackUri"] = Request.Url.AbsoluteUri;
                Session["OrdenTrabajoID_"] = 0;
            }
        }
        public void Limpiar()
        {
            Session["FiltroOrdenTrabajo"] = null;
            //this.Filtro.NumeroInforme = String.Empty;
            //this.Filtro.RFC = String.Empty;
            //this.Filtro.Titulo = String.Empty;
            //this.Filtro.Anexo = String.Empty;
            //this.Filtro.NormaID = 0;
            //this.Filtro.EstatusCancelacion = 2;
            //this.Filtro.FechaEmisionInicio = null;
            //this.Filtro.FechaEmisionFin = null; ;
            //this.Filtro.FechaConformacionInicio = null;
            //this.Filtro.FechaConformacionFin = null;
            //this.Filtro.FechaRegistroInicio = null;
            //this.Filtro.FechaRegistroFin = null;
        }
        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            rbFirmados.Checked = false;
            rbNoFirmados.Checked = true;
            Session["FiltroOrdenTrabajo"] = null;
            SetFiltro();
            var filtro = ObtenerFiltro();
            this.Filtro = filtro;
            Buscar(filtro);
            
        }
        private void SetFiltro()
        {
           
            txtEmisionFechaInicio.Text = this.Filtro.FechaEmisionInicio?.ToString("dd/MM/yyyy");
            txtEmisionFechaFin.Text = this.Filtro.FechaEmisionFin?.ToString("dd/MM/yyyy");
            txtRegistroFechaInicio.Text = this.Filtro.FechaRegistroInicio?.ToString("dd/MM/yyyy");
            txtRegistroFechaFin.Text = this.Filtro.FechaRegistroInicio?.ToString("dd/MM/yyyy");
            txtConformacionFechaInicio.Text = this.Filtro.FechaConformacionInicio?.ToString("dd/MM/yyyy");
            txtConformacionFechaFin.Text = this.Filtro.FechaConformacionInicio?.ToString("dd/MM/yyyy");

            rbCancelados.Checked = this.Filtro.EstatusCancelacion == 0;
            rbNoCancelados.Checked = this.Filtro.EstatusCancelacion == 1;
            rbTodos.Checked = this.Filtro.EstatusCancelacion == 2;

            if (txtEmisionFechaInicio.Text == "" && txtEmisionFechaFin.Text == "" &&
                txtRegistroFechaInicio.Text == "" && txtRegistroFechaFin.Text == "" &&
                txtConformacionFechaInicio.Text == "" && txtConformacionFechaFin.Text == "")
            {
                txtEmisionFechaInicio.Text = (new DateTime(DateTime.Now.Year, 1, 1)).ToString("dd/MM/yyyy");
                txtEmisionFechaFin.Text = (new DateTime(DateTime.Now.Year, 12, 31)).ToString("dd/MM/yyyy");
            }

            ddlNorma.ClearSelection();
            var item = ddlNorma.Items.FindByValue(this.Filtro.NormaID.ToString());
            if (item != null)
                item.Selected = true;

            txtNumeroInforme.Text = this.Filtro.NumeroInforme;
            txtRfc.Text = this.Filtro.RFC;
            txtTitulo.Text = this.Filtro.Titulo;
            txtAnexo.Text = this.Filtro.Anexo;
            wucPaginador.CurrentPage = this.Filtro.CurrentPage;

            //rbFirmados.Checked = this.Filtro.Firma;
            //rbNoFirmados.Checked = !this.Filtro.Firma;

            if (Login.CatUsuarioRol.UsuarioRolID == 2)
            {
                rbFirmados.Checked = true;
                rbNoFirmados.Checked = false;
            }
        }

        protected void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            switch (e.Command)
            {
                case "EliminarOrden":

                    if (e.Opcion == Opcion.Aceptar)
                    {
                        var an = OT_del.Analisis.Where(x => x.Firmada).ToList();
                        if (an.Count > 0)
                        {
                            this.PopMensaje("Los análisis ya se encuentran firmado, imposible proceder a la eliminación", "No se ha podido eliminar", controles.Opciones.SoloAceptar, controles.Icono.ok, "");
                        }
                        else
                        {
                            OrdenTrabajo_BL orden_bl = new OrdenTrabajo_BL();
                            foreach (var Analisis in OT_del.Analisis)
                            {
                                foreach (var Muestra in Analisis.Muestras)
                                {

                                }
                            }
                        }
                    }
                    break;
                case "EliminarOrdenes":
                    if (e.Opcion == Opcion.Aceptar)
                    {
                        var listado = e.Args.Select(int.Parse).ToList();
                        OrdenTrabajo_BL bl = new OrdenTrabajo_BL();
                        string mensaje = bl.Eliminar(listado);
                        var filtro = ObtenerFiltro();
                        Buscar(filtro);
                        this.PopMensaje(mensaje, "Órdenes de trabajo", Opciones.SoloAceptar, Icono.ok, "");
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Eliminado", "doSearch();", true);
                    }
                    break;
                case "FirmaMasiva":
                    Control wucMsj = Page.Master.Master.FindControl("wucMensaje");
                    if (e.Opcion == Opcion.Aceptar)
                    {
                        ((wucMensajes)wucMsj).Ocultar();
                        wucListaOrdenTrabajo.Firmar();
                    }
                    break;
            }
        }

        protected void wucListaOrden1_Eliminar(object sender, SIRALAB_DAL.OrdenTrabajo e)
        {
            if (e != null)
            {
                OT_del = (SIRALAB_DAL.OrdenTrabajo)e;
                this.PopMensaje("¿Esta seguro de eliminar la orden de trabajo?, Solo se podra eliminar la información si no se encuentra firmada.", "Eliminar orden de trabajo", controles.Opciones.SiNo, controles.Icono.warning, "EliminarOrden");
            }
        }

        protected void wucFiltro1_Buscar(object sender, FiltrosLaboratorioArg e)
        {
            //wucListaOrden1.pagina_actual = 0;
            //wucListaOrden1.bloque_actual = 0;
            //wucListaOrden1.CargaInformacion_(e);
        }

        protected void wucFiltro1_Limpiar(object sender, FiltrosLaboratorioArg e)
        {
            //wucListaOrden1.pagina_actual = 0;
            //wucListaOrden1.bloque_actual = 0;
            //wucListaOrden1.CargaInformacion_(e);
        }

        //private void ExportToExcelnf()
        //{
        //    using (var pck = new ExcelPackage())
        //    {
        //        var filtro = new FiltrosLaboratorioArg
        //        {
        //            NoFirmada = true,
        //            Firmada = false,
        //        };
        //        if (Text1nF.Value != "")
        //            filtro.Numero = Text1nF.Value;
        //        if (Text2nF.Value != "")
        //            filtro.Titulo = Text2nF.Value;
        //        if (Text3nF.Value != "")
        //            filtro.Anexo = Text3nF.Value;
        //        if (Text4nF.Value != "")
        //            filtro.RFCCliente = Text4nF.Value;
        //        if (Text5nF.Value != "")
        //            filtro.Norma = Text5nF.Value;
        //        if (TextBox3.Text != "")
        //            filtro.FechaInicial = Convert.ToDateTime(TextBox3.Text);
        //        if (TextBox4.Text != "")
        //            filtro.FechaFinal = Convert.ToDateTime(TextBox4.Text);

        //        var dt = new DataTable();
        //        try
        //        {
        //            dt = VwOtK.ReportSet(filtro, Login.LaboratorioID).Tables["Reporte"];
        //        }
        //        catch (Exception)
        //        {
        //            var col = dt.Columns.Add("Error");
        //            var row = dt.NewRow();
        //            row[0] = "La consulta devolvió muchos resultados, redefina la búsqueda para poder mostralos";
        //            dt.Rows.Add(row);
        //        }

        //        var ws = pck.Workbook.Worksheets.Add("Siralabnf");
        //        ws.Cells["A1"].LoadFromDataTable(dt, true);
        //        ws.Cells.AutoFitColumns();
        //        const string rangocell = "A1:AG1";
        //        using (var rng = ws.Cells["" + rangocell + ""])
        //        {
        //            rng.Style.Font.Bold = true;
        //            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(74, 150, 106));
        //            rng.Style.Font.Color.SetColor(Color.White);

        //            using (ExcelRange col = ws.Cells[2, 18, 2 + dt.Rows.Count, 18])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 19, 2 + dt.Rows.Count, 19])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 20, 2 + dt.Rows.Count, 20])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 21, 2 + dt.Rows.Count, 21])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 22, 2 + dt.Rows.Count, 22])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            var fileName = "Reporte_No_Firmadas_" + Guid.NewGuid() + ".xlsx";
        //            Response.Clear();
        //            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.BinaryWrite(pck.GetAsByteArray());
        //            Response.End();
        //        }
        //    }
        //}

        //private void ExportToExcel()
        //{

        //    using (var pck = new ExcelPackage())
        //    {
        //        var filtro = new FiltrosLaboratorioArg
        //        {
        //            NoFirmada = false,
        //            Firmada = true,
        //            Cancel = CheckBox1.Checked ? "true" : "false"
        //        };
        //        if (Text1F.Value != "")
        //            filtro.Numero = Text1F.Value;
        //        if (Text2F.Value != "")
        //            filtro.Titulo = Text2F.Value;
        //        if (Text3F.Value != "")
        //            filtro.Anexo = Text3F.Value;
        //        if (Text4F.Value != "")
        //            filtro.RFCCliente = Text4F.Value;
        //        if (Text5F.Value != "")
        //            filtro.Norma = Text5F.Value;
        //        if (Text6F.Value != "")
        //            filtro.RFCLaboratorio = Text6F.Value;
        //        if (Text7F.Value != "")
        //            filtro.NombreLaboratorio = Text7F.Value;
        //        if (TextBox1.Text != "")
        //            filtro.FechaInicial = Convert.ToDateTime(TextBox1.Text);
        //        if (TextBox2.Text != "")
        //            filtro.FechaFinal = Convert.ToDateTime(TextBox2.Text);

        //        var dt = new DataTable();
        //        try
        //        {
        //            dt = VwOtK.ReportSet(filtro, Login.LaboratorioID).Tables["Reporte"];
        //        }
        //        catch (Exception)
        //        {
        //            var col = dt.Columns.Add("Error");
        //            var row = dt.NewRow();
        //            row[0] = "La consulta devolvió muchos resultados, redefina la búsqueda para poder mostralos";
        //            dt.Rows.Add(row);
        //        }

        //        var ws = pck.Workbook.Worksheets.Add("Siralab");
        //        ws.Cells["A1"].LoadFromDataTable(dt, true);
        //        ws.Cells.AutoFitColumns();
        //        const string rangocell = "A1:AG1";
        //        using (var rng = ws.Cells["" + rangocell + ""])
        //        {
        //            rng.Style.Font.Bold = true;
        //            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(74, 150, 106));
        //            rng.Style.Font.Color.SetColor(Color.White);


        //            using (ExcelRange col = ws.Cells[2, 18, 2 + dt.Rows.Count, 18])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 19, 2 + dt.Rows.Count, 19])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 20, 2 + dt.Rows.Count, 20])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 21, 2 + dt.Rows.Count, 21])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }
        //            using (ExcelRange col = ws.Cells[2, 22, 2 + dt.Rows.Count, 22])
        //            {
        //                col.Style.Numberformat.Format = "dd/mm/yyyy";
        //            }

        //            var fileName = "Reporte_Firmadas_" + Guid.NewGuid() + ".xlsx";
        //            Response.Clear();
        //            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.BinaryWrite(pck.GetAsByteArray());
        //            //MemoryStream archivoExcel = new MemoryStream();
        //            //pck.SaveAs(archivoExcel);
        //            //Response.BinaryWrite(archivoExcel.GetBuffer());
        //            Response.End();
        //        }
        //    }
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static JqGridf CargaDatos(FiltroOrdenTrabajo filter)
        {
            var oT = new vw_oT();
            filter.LaboratorioID = Login.LaboratorioID;
            var trecords = oT.ListaOrdenTrabajoCount(filter);
            var rpagina = filter.CurrentPage;
            var tpaginas = (trecords / filter.PageSize) + (trecords % filter.PageSize > 0 ? 1 : 0);

            IEnumerable<Vw_Ot_Grid> ordenesT = oT.ListaOrdenTrabajo(filter);
            var lOt = ordenesT.Select(p => new OrdenTrabajoLista1
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

            var jqGrid = new JqGridf();

            foreach (var ot in lOt)
            {
                var row = new JqGridf.Row();

                row.id = ot.OrdenTrabajoId ?? 0;
                row.cell.Add((ot.OrdenTrabajoId ?? 0).ToString(CultureInfo.InvariantCulture));
                row.cell.Add(ot.NombreLaboratorio);
                row.cell.Add(ot.Folio);
                row.cell.Add(ot.Titulo);
                row.cell.Add(ot.Anexo);
                row.cell.Add(ot.RFCCliente);
                row.cell.Add(ot.Norma);
                row.cell.Add(ot.Registro);

                jqGrid.rows.Add(row);
            }

            jqGrid.page = filter.CurrentPage;
            jqGrid.records = trecords;
            jqGrid.total = tpaginas;

            return jqGrid;
        }

        private string[] ToRecaudacion(int AnalisisID)
        {
            ArrayList arr = new ArrayList();

            /*recaudazion*/
            /*SIRALAB_BL.catalogos.Usuario_BL bl = new SIRALAB_BL.catalogos.Usuario_BL();

            List<Usuario> usrRec = bl.ObtenUsuarioRol(2);

            foreach (Usuario us in usrRec)
            {
                arr.Add(us.Correo);
            }*/

            /*Laboratorio*/
            Analisis_BL anBL = new Analisis_BL();
            Analisi analisi = anBL.Obten_Analisis(AnalisisID);

            foreach (Usuario usLab in analisi.Laboratorio.Usuarios)
            {
                if (usLab.Correo != "correo@correo.com" && usLab.Correo != "corre@correo.com")
                {
                    arr.Add(usLab.Correo);
                }
            }

            /*cliente*/
            arr.Add(analisi.OrdenTrabajo.Cliente.Correo);

            return (String[])arr.ToArray(typeof(string));
        }

        public string HTMLEncodeSpecialChars(string text)
        {
            if (text != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in text)
                {
                    if (c > 127) // special chars
                        sb.Append(String.Format("&#{0};", (int)c));
                    else
                        sb.Append(c);
                }
                return sb.ToString();
            }
            else { return text; }
        }

        //[WebMethod(Description = "Lista Ordenes de trabajo ")]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static JqGridf ListaOrdenesTrabajoFirmadasx(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        //{
        //    var nPagina = page;
        //    page -= 1;
        //    var filtro = new FiltrosLaboratorioArg { NoFirmada = true, Firmada = true };
        //    if (searchString != "")
        //    {
        //        HttpContext.Current.Session["searchStringf"] = searchString;
        //    }

        //    if (_search || HttpContext.Current.Session["searchStringf"] != null)
        //    {
        //        string[] split = HttpContext.Current.Session["searchStringf"] != null ? HttpContext.Current.Session["searchStringf"].ToString().Split(new[] { ',' }) : searchString.Split(new[] { ',' });
        //        SesionList.Clear();
        //        SesionList.Add(searchString);
        //        if (split[0] != "")
        //            filtro.Numero = split[0];
        //        if (split[1] != "")
        //            filtro.Titulo = split[1];
        //        if (split[2] != "")
        //            filtro.Anexo = split[2];
        //        if (split[3] != "")
        //            filtro.RFCCliente = split[3];
        //        if (split[4] != "")
        //            filtro.Norma = split[4];
        //        if (split[5] != "")
        //        {
        //            filtro.FechaInicial = DateTime.Parse(split[5]);
        //            //if (split[6] == "")
        //            //{
        //            //    this.PopMensaje("Falta la fecha final",
        //            //        "No se ha podido hacer la busqueda", controles.Opciones.SoloAceptar, controles.Icono.ok, "");
        //            //    return null;
        //            //}
        //        }
        //        if (split[6] != "")
        //        {
        //            filtro.FechaFinal = DateTime.Parse(split[6]);
        //            //if (split[5] == "")
        //            //{
        //            //    this.PopMensaje("Falta la fecha inicial",
        //            //        "No se ha podido hacer la busqueda", controles.Opciones.SoloAceptar, controles.Icono.ok, "");
        //            //    return null;
        //            //}
        //        }
        //        if (split[7] != "")
        //            filtro.Cancel = split[7];
        //        if (split[8] != "")
        //            filtro.RFCLaboratorio = split[8];
        //        if (split[9] != "")
        //            filtro.NombreLaboratorio = split[9];
        //    }

        //    var oT = new vw_oT();
        //    var trecords = oT.Cuenta_OrdenTrabajo(filtro, Login.UsuarioRolID, Login.LaboratorioID);
        //    var rpagina = pageSize;
        //    var tpaginas = (trecords / rpagina) + (trecords % rpagina > 0 ? 1 : 0);

        //    if (page != 0)
        //    {
        //        HttpContext.Current.Session["pagef"] = page;
        //        HttpContext.Current.Session["npagef"] = nPagina;
        //    }

        //    if (HttpContext.Current.Session["ExtPagerf"] != null)
        //    {
        //        if (HttpContext.Current.Session["pagef"] != null)
        //            page = (int)HttpContext.Current.Session["pagef"];
        //        if (HttpContext.Current.Session["npagef"] != null)
        //            nPagina = (int)HttpContext.Current.Session["npagef"];
        //        HttpContext.Current.Session["ExtPagerf"] = null;
        //    }

        //    IEnumerable<Vw_Ot_Grid> ordenesT = oT.ListaOrdenTrabajofirmada(filtro, HttpContext.Current.Session["pagef"] != null ? page : 0, rpagina, Login.UsuarioRolID, Login.LaboratorioID, sortIndex, sortDirection.ToUpper());
        //    var lOt = ordenesT.Select(p => new OrdenTrabajoLista1
        //    {
        //        OrdenTrabajoId = p.OrdenTrabajoID,
        //        Folio = p.Informe,
        //        Titulo = p.Titulo,
        //        Anexo = p.Anexo.ToString(),
        //        RFCCliente = p.RFC,
        //        Registro = p.FechaEmision.ToShortDateString(),
        //        muestreado = p.EstatusID.ToString(),
        //        NombreLaboratorio = p.NombreLaboratorio,
        //        Norma = p.Norma,


        //    }).ToList();

        //    var jqGrid = new JqGridf();


        //    foreach (var ot in lOt)
        //    {
        //        var row = new JqGridf.Row();

        //        row.id = ot.OrdenTrabajoId ?? 0;

        //        row.cell.Add((ot.OrdenTrabajoId ?? 0).ToString(CultureInfo.InvariantCulture));
        //        row.cell.Add((ot.NombreLaboratorio));
        //        row.cell.Add((ot.Folio));
        //        row.cell.Add((ot.Titulo));
        //        row.cell.Add((ot.Anexo));
        //        row.cell.Add((ot.RFCCliente));
        //        row.cell.Add((ot.Norma));
        //        row.cell.Add((ot.Registro));


        //        jqGrid.rows.Add(row);
        //    }
        //    jqGrid.page = nPagina;
        //    jqGrid.records = trecords;
        //    jqGrid.total = tpaginas;

        //    return jqGrid;
        //}

        public class JqGridf
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

            public JqGridf()
            {
                rows = new List<Row>();
            }
        }

        //[WebMethod(Description = "Lista Ordenes de trabajo ")]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static JqGridNof ListaOrdenesTrabajoNoFirmadasx(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        //{
        //    var nPagina = page;
        //    page -= 1;
        //    var filtro = new FiltrosLaboratorioArg { NoFirmada = true, Firmada = true };

        //    if (searchString != "")
        //    {
        //        HttpContext.Current.Session["searchStringnf"] = searchString;
        //    }


        //    if (_search || HttpContext.Current.Session["searchStringnf"] != null)
        //    {
        //        string[] split = HttpContext.Current.Session["searchStringnf"] != null ? HttpContext.Current.Session["searchStringnf"].ToString().Split(new[] { ',' }) : searchString.Split(new[] { ',' });
        //        SesionListnf.Clear();
        //        SesionListnf.Add(searchString);

        //        if (split[0] != "")
        //            filtro.Numero = split[0];
        //        if (split[1] != "")
        //            filtro.Titulo = split[1];
        //        if (split[2] != "")
        //            filtro.Anexo = split[2];
        //        if (split[3] != "")
        //            filtro.RFCCliente = split[3];
        //        if (split[4] != "")
        //            filtro.Norma = split[4];
        //        if (split[5] != "")
        //        {
        //            filtro.FechaInicial = DateTime.Parse(split[5]);
        //            //if (split[6] == "")
        //            //{
        //            //    PopMensaje("Falta la fecha final",
        //            //        "No se ha podido hacer la busqueda", controles.Opciones.SoloAceptar, controles.Icono.ok, "");
        //            //    return null;
        //            //}
        //        }
        //        if (split[6] != "")
        //        {
        //            filtro.FechaFinal = DateTime.Parse(split[6]);
        //            //if (split[5] == "")
        //            //{
        //            //    this.PopMensaje("Falta la fecha inicial",
        //            //        "No se ha podido hacer la busqueda", controles.Opciones.SoloAceptar, controles.Icono.ok, "");
        //            //    return null;
        //            //}
        //        }
        //    }


        //    var oT = new vw_oT();
        //    var trecords = oT.CuentaOrdenTrabajoNoF(filtro, Login.UsuarioRolID, Login.LaboratorioID);
        //    var rpagina = pageSize;
        //    var tpaginas = (trecords / rpagina) + (trecords % rpagina > 0 ? 1 : 0);

        //    if (page != 0)
        //    {
        //        HttpContext.Current.Session["pageNOf"] = page;
        //        HttpContext.Current.Session["npageNOf"] = nPagina;
        //    }

        //    if (HttpContext.Current.Session["ExtPagerNOf"] != null)
        //    {
        //        if (HttpContext.Current.Session["pageNOf"] != null)
        //            page = (int)HttpContext.Current.Session["pageNOf"];
        //        if (HttpContext.Current.Session["npageNOf"] != null)
        //            nPagina = (int)HttpContext.Current.Session["npageNOf"];
        //        HttpContext.Current.Session["ExtPagerNOf"] = null;
        //    }
        //    IEnumerable<Vw_Ot_Grid> ordenesT = oT.ListaOrdenTrabajoNofirmada(filtro, HttpContext.Current.Session["pageNOf"] != null ? page : 0, rpagina, Login.UsuarioRolID, Login.LaboratorioID, sortIndex, sortDirection.ToUpper());
        //    var lOt = ordenesT.Select(p => new OrdenTrabajoLista1
        //    {
        //        OrdenTrabajoId = p.OrdenTrabajoID,
        //        Folio = p.Informe,
        //        Titulo = p.Titulo,
        //        Anexo = p.Anexo.ToString(CultureInfo.InvariantCulture),
        //        RFCCliente = p.RFC,
        //        Registro = p.FechaEmision.ToShortDateString(),
        //        muestreado = p.EstatusID.ToString(CultureInfo.InvariantCulture),
        //        NombreLaboratorio = p.NombreLaboratorio,
        //        Norma = p.Norma
        //    }).ToList();

        //    var jqGrid = new JqGridNof();


        //    foreach (var ot in lOt)
        //    {
        //        var row = new JqGridNof.Row();

        //        row.id = ot.OrdenTrabajoId ?? 0;

        //        row.cell.Add((ot.OrdenTrabajoId ?? 0).ToString(CultureInfo.InvariantCulture));
        //        row.cell.Add((ot.NombreLaboratorio));
        //        row.cell.Add((ot.Folio));
        //        row.cell.Add((ot.Titulo));
        //        row.cell.Add((ot.Anexo));
        //        row.cell.Add((ot.RFCCliente));
        //        row.cell.Add((ot.Norma));
        //        row.cell.Add((ot.Registro));


        //        jqGrid.rows.Add(row);
        //    }
        //    jqGrid.page = nPagina;
        //    jqGrid.records = trecords;
        //    jqGrid.total = tpaginas;

        //    return jqGrid;
        //}

        //public class JqGridNof : JqGridf
        //{

        //}

        //#region jqgrid

        //[WebMethod(Description = "Lista Ordenes de trabajo en Proceso")]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static wucListaOrden.JQGrid ListaOrdenesTrabajoP_Json(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        //{
        //    wucListaOrden.JQGrid Json = new wucListaOrden.JQGrid();

        //    Json = wucListaOrden.ListaOrdenesTrabajoP_Json(page, pageSize, sortIndex, sortDirection, _search, searchField, searchString, searchOper);
        //    return Json;
        //}
        ////[WebMethod(Description = "Lista Ordenes de trabajo en Proceso Firma")]
        ////[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        ////public static wucListaOrdenFirmadas.JqGridf ListaOrdenesTrabajoFirmadas(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        ////{
        ////    var Json = new wucListaOrdenFirmadas.JqGridf();

        ////    Json = wucListaOrdenFirmadas.ListaOrdenesTrabajoFirmadas(page, pageSize, sortIndex, sortDirection, _search, searchField, searchString, searchOper);
        ////    return Json;
        ////}
        ////  [WebMethod(Description = "Lista Ordenes de trabajo en Proceso ListaOrdenesTrabajoNoFirmadas")]
        ////[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        ////public static wucListaOrdenNoFirmadas.JqGridNof ListaOrdenesTrabajoNoFirmadas(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        ////{
        ////    var Json = new wucListaOrdenNoFirmadas.JqGridNof();

        ////    Json = wucListaOrdenNoFirmadas.ListaOrdenesTrabajoNoFirmadas(page, pageSize, sortIndex, sortDirection, _search, searchField, searchString, searchOper);
        ////    return Json;
        ////}
        //[WebMethod(Description = "Lista Ordenes de trabajo en Proceso ListaOrdenesTrabajoCanceladas")]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static wucListaOrdenCanceladas.JqGridC ListaOrdenesTrabajoCanceladas(int page, int pageSize, string sortIndex, string sortDirection, bool _search, string searchField, string searchString, string searchOper)
        //{
        //    var Json = new wucListaOrdenCanceladas.JqGridC();

        //    Json = wucListaOrdenCanceladas.ListaOrdenesTrabajoNoFirmadas(page, pageSize, sortIndex, sortDirection, _search, searchField, searchString, searchOper);
        //    return Json;
        //}



        //[WebMethod(Description = "Edita la Orden de Trabajo", EnableSession = true)]
        //public static string EditOrdenTrabajo(int Id, string url)
        //{

        //    HttpContext.Current.Session["BackUri"] = url;
        //    OrdenTrabajoID = Id;
        //    return "";
        //}


        //[WebMethod(Description = "Elimina la Orden de Trabajo", EnableSession = true)]
        //public static string EliminarOrdenTrabajo(int Id)
        //{
        //    OrdenTrabajoID = Id;
        //    OrdenTrabajo_BL orden_bl = new OrdenTrabajo_BL();
        //    OT_del = orden_bl.Obten(Id);
        //    var an = OT_del.Analisis.Where(x => x.Firmada == true).ToList();

        //    if (an.Count > 0)
        //    {
        //        return "Los análisis ya se encuentran firmado, imposible proceder a la eliminación";
        //    }
        //    else
        //    {


        //        foreach (var Analisis in OT_del.Analisis)
        //        {
        //            foreach (var Muestra in Analisis.Muestras)
        //            { }
        //        }
        //    }

        //    return "Los análisis ya eliminarón";
        //}


        //#endregion

        //protected void Excel_Click(object sender, EventArgs e)
        //{
        //    ExportToExcel();

        //}
        //protected void Excelnf_Click(object sender, EventArgs e)
        //{
        //    ExportToExcelnf();

        //}

        protected void lnkbtnNuevo_Click(object sender, EventArgs e)
        {
            this.Anexo = null;
            this.Cliente = null;

            Session["OrdenTrabajoID_"] = 0;
            Response.Redirect("~/OrdenTrabajo.aspx");
        }

        protected void lnkBuscar_OnClick(object sender, EventArgs e)
        {
            var filtro = ObtenerFiltro();
            this.Filtro = filtro;
            Buscar(filtro);
        }

    
        protected void lnkEliminar_OnClick(object sender, EventArgs e)
        {
            var ots = wucListaOrdenTrabajo.ObtenerOrdenesAEliminar();
            if (ots.Count > 0)
                this.PopMensaje("¿Está seguro de eliminar las órdenes seleccionadas?", "Órdenes de Trabajo", Opciones.SiNo, Icono.question, "EliminarOrdenes", ots.ToArray());
            else
                this.PopMensaje("No ha seleccionado ningún registro", "Órdenes de Trabajo", Opciones.SoloAceptar, Icono.information);
        }

        protected void lnkFirmar_OnClick(object sender, EventArgs e)
        {
            this.PopMensaje("¿Está seguro de firmar los análisis seleccionados?", "Firma de análisis", Opciones.SiNo, Icono.question, "FirmaMasiva", null);
        }

        public void Buscar(FiltroOrdenTrabajo filtro)
        {
            var oT = new vw_oT();
            //filtro.LaboratorioID = Login.LaboratorioID;

            if (!filtro.Firma)
                Session["NombreExcel"] = "Reporte_No_Firmadas_" + Guid.NewGuid();
            else
                Session["NombreExcel"] = "Reporte_Firmadas_" + Guid.NewGuid();

            IEnumerable<Vw_Ot_Grid> ordenesT = oT.ListaOrdenTrabajo(filtro);
            
            var lOt = ordenesT.Select(p => new OrdenTrabajoLista
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

            lOt = lOt.GroupBy(otl => otl.OrdenTrabajoId).Select(x => x.FirstOrDefault()).ToList();

            try
            {
                var analisis_BL = new Analisis_BL();
                var listaAux = lOt.Where(o => o.OrdenTrabajoId != null).Select(ot => ot.OrdenTrabajoId).ToList();
                var lista = analisis_BL.ObtenAnalisisResults(listaAux);


                foreach (var f in lOt)
                {
                    var enumerable = lista.Where(p => p.OrdenTrabajoID == f.OrdenTrabajoId).ToList();
                    var i = 0;
                    var listaDeAnalisisLista = new List<AnalisisLista>();

                    

                    foreach (var an in enumerable)
                    {
                        var analisisLista = new AnalisisLista();
                        analisisLista.AnaisisID = an.AnalisisID;
                        analisisLista.Firmado_ = an.Firmada;
                        analisisLista.Consecutivo_ = an.Consecutivo ?? "";
                        analisisLista.FechaRegistro_ = an.Fecha ?? "";
                        analisisLista.FirmaVisible_ = an.FirmaVisible;
                        analisisLista.AnalisisEncolado_ = an.AnalisisEncolado;
                        
                        listaDeAnalisisLista.Add(analisisLista);
                    } 
                    
                    f.Analisis = listaDeAnalisisLista;


                    
                }
               
            }
            catch (Exception e)
            {

            }
            
           

            //var lAnalisis = lOt.Select(p => p.Analisis).ToList();
            var total = Contar(filtro);
            wucListaOrdenTrabajo.Initialize(lOt);
            wucPaginador.Initialize(filtro.CurrentPage, total);
        }

        private int Contar(FiltroOrdenTrabajo filtro)
        {
            var oT = new vw_oT();
            //filtro.LaboratorioID = Login.LaboratorioID;
            var records = oT.ListaOrdenTrabajoCount(filtro);

            return records;
        }

        private FiltroOrdenTrabajo ObtenerFiltro(int? page = null)
        {
            var emisionFI_ = txtEmisionFechaInicio.Text;
            var emisionFF_ = txtEmisionFechaFin.Text;
            var registroFI_ = txtRegistroFechaInicio.Text;
            var registroFF_ = txtRegistroFechaFin.Text;
            var conformacionFI_ = txtConformacionFechaInicio.Text;
            var conformacionFF_ = txtConformacionFechaFin.Text;

            var emisionFI = emisionFI_ == "" ? null : (DateTime?)(new DateTime(int.Parse(emisionFI_.Split('/')[2]), int.Parse(emisionFI_.Split('/')[1]), int.Parse(emisionFI_.Split('/')[0])));
            var emisionFF = emisionFF_ == "" ? null : (DateTime?)(new DateTime(int.Parse(emisionFF_.Split('/')[2]), int.Parse(emisionFF_.Split('/')[1]), int.Parse(emisionFF_.Split('/')[0])));
            var registroFI = registroFI_ == "" ? null : (DateTime?)(new DateTime(int.Parse(registroFI_.Split('/')[2]), int.Parse(registroFI_.Split('/')[1]), int.Parse(registroFI_.Split('/')[0])));
            var registroFF = registroFF_ == "" ? null : (DateTime?)(new DateTime(int.Parse(registroFF_.Split('/')[2]), int.Parse(registroFF_.Split('/')[1]), int.Parse(registroFF_.Split('/')[0])));
            var conformacionFI = conformacionFI_ == "" ? null : (DateTime?)(new DateTime(int.Parse(conformacionFI_.Split('/')[2]), int.Parse(conformacionFI_.Split('/')[1]), int.Parse(conformacionFI_.Split('/')[0])));
            var conformacionFF = conformacionFF_ == "" ? null : (DateTime?)(new DateTime(int.Parse(conformacionFF_.Split('/')[2]), int.Parse(conformacionFF_.Split('/')[1]), int.Parse(conformacionFF_.Split('/')[0])));
            var estatusCancelacion =
            rbCancelados.Checked && !rbNoCancelados.Checked && !rbTodos.Checked ? int.Parse(rbCancelados.Attributes["Value"]) :
            !rbCancelados.Checked && rbNoCancelados.Checked && !rbTodos.Checked ? int.Parse(rbNoCancelados.Attributes["Value"]) :
            int.Parse(rbTodos.Attributes["Value"]);

            var filter = new FiltroOrdenTrabajo
            {
                NumeroInforme = txtNumeroInforme.Text,
                NormaID = ddlNorma.SelectedValue == "0" ? null : (int?)int.Parse(ddlNorma.SelectedValue),
                LaboratorioID = Login.UsuarioRolID != 1 ? ddlLaboratorio.SelectedValue == "0" ? null : (int?)int.Parse(ddlLaboratorio.SelectedValue) : Login.LaboratorioID, //TODO -> Refactor
                Firma = rbFirmados.Checked,
                RFC = txtRfc.Text,
                Titulo = txtTitulo.Text,
                Anexo = txtAnexo.Text,
                EstatusCancelacion = estatusCancelacion,
                FechaEmisionInicio = emisionFI,
                FechaEmisionFin = emisionFF,
                FechaRegistroInicio = registroFI,
                FechaRegistroFin = registroFF,
                FechaConformacionInicio = conformacionFI,
                FechaConformacionFin = conformacionFF,
                CurrentPage = page ?? 1,
                PageSize = wucPaginador.PageSize,
                OrderBy = "FechaEmision",
                OrderByDirection = "ASC"
            };

            return filter;
        }

        protected void wucPaginador_OnPageChange(object sender, PageArgs e)
        {
            var filtro = ObtenerFiltro(e.CurrentPage);
            filtro.CurrentPage = e.CurrentPage;
            filtro.PageSize = wucPaginador.PageSize;
            Buscar(filtro);
        }

        protected void wucAppletFIEL_FirmarColeccion(object sender, DatosFirma_Args e)
        {
            FIEL.FirmaElectronica datosFirma;
            datosFirma = e.DatosAFirmar;
            byte[] ArchivoKey = e.ArchivoKey;
            byte[] ArchivoCer = e.ArchivoCer;
            string CNT = e.txtContra;
            List<string> listaErrorFirma = new List<string>();
            var contenedores = new List<Contenedor>();
            var limiteSSTDQO = false;
            var ClorurosConductividad = false;
            var Cloruros = false;

            if (Login.Laboratorio.RFCValidoParaFirmar(e.RFCFirma))
            {
                // Health-check: si la selladora de CONAGUA no está disponible, no se encola
                // nada y se avisa al usuario en el momento para que reintente más tarde.
                if (!SelloDigital.SelladoraDisponible())
                {
                    this.PopMensaje(
                        "El servicio de sellado de CONAGUA no está disponible en este momento. Por favor, intente firmar más tarde.",
                        "Servicio de sellado no disponible",
                        controles.Opciones.SoloAceptar, controles.Icono.error);
                    return;
                }

                try
                {
                    FirmaQueue queue = new FirmaQueue();
                    var banderaDetieneCola = true;
                    var banderaCreaRegistros = false;

                    var analisisColaDetalleBL = new AnalisisColaDetalle_BL();
                    var analisisColaEncabezadoBL = new AnalisisColaEncabezado_BL();

                    //var provider = CultureInfo.InvariantCulture;
                    //const string format = "dd-MM-yyyy HH:mm:ss";
                    var date = DateTime.Now;

                    var colaDetalle = new AnalisisColaDetalle()
                    {
                        AnalisisColaDetalleID = 0,
                        Activo = true,
                        Completo = false,
                        FechaCreacion = date,
                    };

                    foreach (var contenedor in datosFirma.listaContenedores)
                    {
                        contenedor.ArchivoCer = ArchivoCer;
                        contenedor.ArchivoKey = ArchivoKey;
                        contenedor.txtRFC = e.RFCFirma;
                        contenedor.txtContra = CNT;

                        if (contenedor.valido) //Como los datos son siempre iguales, si falla el primero fallan todos:
                        {
                            #region ValidacionLimiteNormaSSTDQO
                            try
                            {

                                ApplicationSettings applicationSettings = new ApplicationSettings();
                                var idAnalisis = int.Parse(contenedor.primaryKey);
                                Analisis_BL analisisBL = new Analisis_BL();
                                var analisisDal = analisisBL.Obten_Analisis(idAnalisis);
                                if (analisisDal.OrdenTrabajo.NormaID == 3 || analisisDal.OrdenTrabajo.NormaID == 13)
                                {
                                    var fechaUltimaMuestra = analisisDal.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha;
                                    var fechaLimiteAnalisisSSTDQO = Convert.ToDateTime(applicationSettings.LimiteNormaSSTDQO);
                                    if (fechaUltimaMuestra >= fechaLimiteAnalisisSSTDQO)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que se encuentran fuera de la vigencia de la norma");
                                        limiteSSTDQO = true;
                                        continue;
                                    }
                                }
                                else if (analisisDal.OrdenTrabajo.Norma.Nombre.Replace(" ", "") == applicationSettings.NormaCOT || analisisDal.OrdenTrabajo.Norma.Nombre.Replace(" ", "").ToUpper() == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                                {
                                    var fechaUltimaMuestra = analisisDal.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha;
                                    var fechaLimiteAnalisisSSTDQO = Convert.ToDateTime(applicationSettings.LimiteNormaSSTDQO);
                                    if (fechaUltimaMuestra < fechaLimiteAnalisisSSTDQO)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que se encuentran fuera de la vigencia de la norma");
                                        limiteSSTDQO = true;
                                        continue;
                                    }
                                }

                                if (analisisDal.OrdenTrabajo.NormaID == 15)//valida que sea la norma 2021
                                {
                                    if (analisisDal.NormaDQOCOT == null || analisisDal.NormaDQOCOT == string.Empty || analisisDal.Conductividad == null || analisisDal.Conductividad == string.Empty)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros y/o conductividad eléctrica.");
                                        ClorurosConductividad = true;
                                        continue;
                                    }

                                }
                                else if (analisisDal.OrdenTrabajo.Norma.Nombre.Replace(" ", "").ToUpper() == applicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))//valida la norma agua clara
                                {
                                    if (analisisDal.NormaDQOCOT == null || analisisDal.NormaDQOCOT == string.Empty)
                                    {
                                        listaErrorFirma.Add("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros.");
                                        Cloruros = true;
                                        continue;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                listaErrorFirma.Add("Ocurrió un error al intentar crear el proceso de firma.Inténtelo de nuevo en unos minutos");
                                break;
                            }

                            #endregion
                            if (banderaDetieneCola) //En la primera iteración crea la cola. Si ocurre un error al crear el registro, se regresa un mensaje de error (no se persiste nada y no se abre un hilo para las firmas).
                            {
                                colaDetalle = analisisColaDetalleBL.Guarda(colaDetalle);
                                banderaDetieneCola = colaDetalle == null;
                                queue.ColaID = !banderaDetieneCola ? (int)colaDetalle.AnalisisColaDetalleID : 0;
                                queue.ServerUtility = System.Web.HttpContext.Current.Server;

                                if (banderaDetieneCola)
                                {
                                    listaErrorFirma.Add("Ocurrió un error al intentar crear el proceso de firma. Inténtelo de nuevo en unos minutos");
                                    break;
                                }
                            }

                            var analisisID = 0;
                            int.TryParse(contenedor.primaryKey, out analisisID);
                            var analisisColaEncabezado = new AnalisisColaEncabezado()
                            {
                                AnalisisColaEncabezadoID = 0,
                                AnalisisColaDetalleID = colaDetalle.AnalisisColaDetalleID,
                                AnalisisID = analisisID,
                                FechaEnvio = DateTime.Now,
                                EstatusID = enumEstatusAnalisis.EnProceso.ParseToInt(),
                                LaboratorioID = Login.LaboratorioID ?? 0
                            };

                            //Si ocurre algún error al persistir el registro se eliminan todos los registros de la cola:
                            banderaCreaRegistros = analisisColaEncabezadoBL.SaveOrUpdate(analisisColaEncabezado) == null;

                            if (banderaCreaRegistros)
                            {
                                analisisColaEncabezadoBL.DeleteAllByColaID((int)colaDetalle.AnalisisColaDetalleID);
                                analisisColaDetalleBL.DeleteById((int)colaDetalle.AnalisisColaDetalleID);
                                listaErrorFirma.Add("Ocurrió un error al intentar crear el proceso de firma. Inténtelo de nuevo en unos minutos");
                                break;
                            }

                            contenedores.Add(contenedor);
                        }
                        else //Si alguno de los datos es erróneo finaliza la firma.
                        {
                            listaErrorFirma.Add(contenedor.mensajeError);
                            System.Diagnostics.Debug.WriteLine(contenedor.mensajeError);
                            break;
                        }
                    }

                    //Si los registros se crean correctamente, entonces se añaden todos a la cola.
                    if (!banderaDetieneCola && !banderaCreaRegistros)
                        contenedores.ForEach(contenedor => queue.Enqueue(contenedor));



                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.InnerException);
                }




                //wucListaOrdenTrabajo.CargaInformacion(OrdenTrabajoID);


                if (limiteSSTDQO && contenedores.Count > 0)
                {
                    this.PopMensaje("Algunos análisis no pudieron ser firmados debido a que se encuentran fuera de la vigencia de la norma", "Firma de análisis", Opciones.Si, Icono.question, "LímiteSSTyDQO", null);
                }
                else if (ClorurosConductividad && contenedores.Count > 0)
                {
                    this.PopMensaje("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros y/o conductividad eléctrica.", "Firma de análisis", Opciones.Si, Icono.question, "LímiteSSTyDQO", null);
                }
                else if (Cloruros && contenedores.Count > 0)
                {
                    this.PopMensaje("Algunos análisis no pudieron ser firmados debido a que es necesario validar lo correspondiente a cloruros.", "Firma de análisis", Opciones.Si, Icono.question, "LímiteSSTyDQO", null);
                }
                else if (!listaErrorFirma.Any())
                {
                    //Page_Load(sender, e);
                    Response.Redirect("~/EstatusFirmado.aspx", true);
                    this.PopMensaje("Análisis enviados a firma", "Firmado");
                }
                else
                    this.PopMensaje(listaErrorFirma.FirstOrDefault().ToString(), "Error al firma", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            else
            {
                this.PopMensaje("El RFC autorizado para firmar no coincide con el del certificado. Favor de verificarlo.", "RFC Diferentes.", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            var filtro = ObtenerFiltro();
            Buscar(filtro);
        }

        protected void wucListaOrdenTrabajo_FirmarAnalisis(object sender, DatosFirma_Args e)
        {
            if (e.DatosAFirmar.listaContenedores.Count > 0)
            {
                wucAppletFIEL.enviarAFirma(e.DatosAFirmar);
                wucAppletFIEL.Visible = true;
            }
        }

        private void sendMail(int AnalisisID)
        {
            ApplicationSettings ap = new ApplicationSettings();
            SIRALAB_DAL.Analisi analisisDal = new Analisi();
            Analisis_BL analisisBL = new Analisis_BL();
            analisisDal = analisisBL.Obten_Analisis(AnalisisID);

            string nombreReportAnalisis = analisisDal.OrdenTrabajo.Norma.RequiereCompuesta ? "AnalisisFirmado" : "AnalsisisSDT";
            string nombreReportAcuse = analisisDal.Procesado ? "AcuseFirmadoVencido" : "AcuseFirmado";

            List<ReportParameter> listParametrosAnalisisFirmado = new List<ReportParameter>();
            ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            listParametrosAnalisisFirmado.Add(rpAnalisisID);
            string ubicacionDeMuestra = "";
            var muestra = analisisDal.Muestras.FirstOrDefault();
            ubicacionDeMuestra += "Calle: " + muestra.Direccion.Calle;
            ubicacionDeMuestra += " Num: " + muestra.Direccion.NumeroExterior;
            ubicacionDeMuestra += ", " + muestra.Direccion.NumeroInterior;
            ubicacionDeMuestra += ", Colonia: " + muestra.Direccion.Colonia.Nombre;
            ubicacionDeMuestra += " Localidad: " + muestra.Direccion.Localidad.Nombre;
            ubicacionDeMuestra += " Municipio: " + muestra.Direccion.Municipio.Nombre;
            ubicacionDeMuestra += " Estado: " + muestra.Direccion.Estado.Nombre;
            ubicacionDeMuestra += " C.P. " + muestra.Direccion.CP;
            ubicacionDeMuestra += " Longitud: " + muestra.Longitud;
            ubicacionDeMuestra += " Latitud: " + muestra.Latitud;

            var norma = analisisDal.OrdenTrabajo.Norma;
            string normaOParametros = string.Empty;
            if ((bool)norma.Evaluacion)
            {
                normaOParametros += norma.Nombre;
            }
            else
            {
                string strMuestraValor = string.Empty;
                analisisDal.Muestras.ToList().ForEach(mst => mst.MuestraValors.ToList().ForEach(mv => strMuestraValor += mv.Parametro.Nombre + " "));
                normaOParametros += strMuestraValor;
            }
            bool procesado = analisisDal.Procesado;
            List<ReportParameter> listParametrosAcuse = new List<ReportParameter>();
            ReportParameter paramAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            ReportParameter paramNumInforme = new ReportParameter("NumeroDeInforme", analisisDal.OrdenTrabajo.NumeroInforme.ToString());
            ReportParameter paramHoraDFirma = new ReportParameter("HoraDeFirma", DateTime.Now.ToShortTimeString());
            ReportParameter paramNomPuntDes = new ReportParameter("NombrePuntoDeDescarga", analisisDal.Anexo.NombreCuerpoReceptor);
            ReportParameter paramCuerpoRece = new ReportParameter("CuerpoReceptor", analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
            ReportParameter paramFrecMuestr = new ReportParameter("FrecuenciaMuestreo", analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
            ReportParameter paramNormaOPmts = new ReportParameter("NormaOParametros", normaOParametros);
            ReportParameter paramUbicaMuest = new ReportParameter("UbicacionMuestreo", ubicacionDeMuestra);
            string selloDigitalCNA = analisisDal.AnalisisFirmas.Where(a => a.AnalisisID == analisisDal.AnalisisID).Last().SelloDigital;
            ReportParameter paramSelloDeCNA = new ReportParameter("SelloCONAGUA", selloDigitalCNA);
            ReportParameter paramAcuseRegla = new ReportParameter("AcuseRegla", string.Format(ap.AcuseTexto, ap.AcuseRegla, ap.AcuseAnio));

            listParametrosAcuse.Add(paramAnalisisID);
            listParametrosAcuse.Add(paramNumInforme);
            listParametrosAcuse.Add(paramHoraDFirma);
            listParametrosAcuse.Add(paramNomPuntDes);
            listParametrosAcuse.Add(paramCuerpoRece);
            listParametrosAcuse.Add(paramFrecMuestr);
            listParametrosAcuse.Add(paramNormaOPmts);
            listParametrosAcuse.Add(paramUbicaMuest);
            listParametrosAcuse.Add(paramSelloDeCNA);


            /* Mensages */
            System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-ES");
            var fechaOT = DateTime.Now;
            var fecha = c.DateTimeFormat.GetDayName(fechaOT.DayOfWeek) + " " +
                fechaOT.Day.ToString() + " " +
                c.DateTimeFormat.GetMonthName(fechaOT.Month) + " de " +
                fechaOT.Year.ToString();

            string Message = "Se le informa que la Comisión Nacional de Agua ha recibido con fecha " + fecha +
                           " a las " + DateTime.Now.ToString("HH:mm") + " horas, el informe número " + analisisDal.OrdenTrabajo.NumeroInforme + "/" + analisisBL.AnalisisNumero(AnalisisID) +
                           " del análisis de laboratorio que su representada efectuó para el contribuyente "
                           + (analisisDal.OrdenTrabajo.Cliente.Nombre +
                           " " + (analisisDal.OrdenTrabajo.Cliente.ApellidoPaterno ?? "") +
                           " " + (analisisDal.OrdenTrabajo.Cliente.ApellidoMaterno ?? "")).Trim() +
                           ", con RFC " + analisisDal.OrdenTrabajo.Cliente.RFC +
                           ".";

            string[] ToArr = null;

            /* Llenar con las direcciones a quienes se les enviará el correo */
            ToArr = ToRecaudacion(AnalisisID);

            ApplicationSettings appSettings = new ApplicationSettings();
            if (appSettings.useTestMail)
                ToArr = appSettings.testMail;

            var fileAnalisisPdf = OpenReports.BufferPDFRemoteReport(nombreReportAnalisis, listParametrosAnalisisFirmado);


            if (procesado)
                listParametrosAcuse.Add(paramAcuseRegla);

            var fileAcusePdf = OpenReports.BufferPDFRemoteReport(nombreReportAcuse, listParametrosAcuse);

            Hashtable ht = new Hashtable();

            ht.Add(nombreReportAnalisis + ".pdf", fileAnalisisPdf);
            ht.Add(nombreReportAcuse + ".pdf", fileAcusePdf);
            ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
            ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));



            try
            {
                foreach (var to in ToArr)
                {
                    Thread threadSendMails = new Thread(delegate ()
                    {
                        SIRALAB_BL.correo.Mailer.SendMail(to, "", "", Message, "Acuse de recibo del informe del análisis de laboratorio", ht);
                    });

                    threadSendMails.IsBackground = true;
                    threadSendMails.Start();
                }
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void sendMailThread(int AnalisisID)
        {
            ApplicationSettings ap = new ApplicationSettings();
            SIRALAB_DAL.Analisi analisisDal = new Analisi();
            Analisis_BL analisisBL = new Analisis_BL();
            analisisDal = analisisBL.Obten_Analisis(AnalisisID);

            string nombreReportAnalisis = analisisDal.OrdenTrabajo.Norma.RequiereCompuesta ? "AnalisisFirmado" : "AnalsisisSDT";
            string nombreReportAcuse = analisisDal.Procesado ? "AcuseFirmadoVencido" : "AcuseFirmado";

            List<ReportParameter> listParametrosAnalisisFirmado = new List<ReportParameter>();
            ReportParameter rpAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            listParametrosAnalisisFirmado.Add(rpAnalisisID);

            var fileAnalisisPdf = OpenReports.BufferPDFRemoteReport(nombreReportAnalisis, listParametrosAnalisisFirmado);

            List<ReportParameter> listParametrosAcuse = new List<ReportParameter>();
            string ubicacionDeMuestra = "";
            Muestra muestra = analisisDal.Muestras.FirstOrDefault();
            ubicacionDeMuestra += "Calle: " + muestra.Direccion.Calle;
            ubicacionDeMuestra += " Num: " + muestra.Direccion.NumeroExterior;
            ubicacionDeMuestra += ", " + muestra.Direccion.NumeroInterior;
            ubicacionDeMuestra += ", Colonia: " + muestra.Direccion.Colonia.Nombre;
            ubicacionDeMuestra += " Localidad: " + muestra.Direccion.Localidad.Nombre;
            ubicacionDeMuestra += " Municipio: " + muestra.Direccion.Municipio.Nombre;
            ubicacionDeMuestra += " Estado: " + muestra.Direccion.Estado.Nombre;
            ubicacionDeMuestra += " C.P. " + muestra.Direccion.CP;
            ubicacionDeMuestra += " Longitud: " + muestra.Longitud;
            ubicacionDeMuestra += " Latitud: " + muestra.Latitud;

            SIRALAB_DAL.Norma norma = analisisDal.OrdenTrabajo.Norma;
            string normaOParametros = string.Empty;
            if ((bool)norma.Evaluacion)
            {
                normaOParametros += norma.Nombre;
            }
            else
            {
                string strMuestraValor = string.Empty;
                analisisDal.Muestras.ToList().ForEach(mst => mst.MuestraValors.ToList().ForEach(mv => strMuestraValor += mv.Parametro.Nombre + " "));
                normaOParametros += strMuestraValor;
            }

            ReportParameter paramAnalisisID = new ReportParameter("AnalisisID", analisisDal.AnalisisID.ToString());
            ReportParameter paramNumInforme = new ReportParameter("NumeroDeInforme", analisisDal.OrdenTrabajo.NumeroInforme.ToString());
            ReportParameter paramHoraDFirma = new ReportParameter("HoraDeFirma", DateTime.Now.ToShortTimeString());
            ReportParameter paramNomPuntDes = new ReportParameter("NombrePuntoDeDescarga", analisisDal.Anexo.NombreCuerpoReceptor);
            ReportParameter paramCuerpoRece = new ReportParameter("CuerpoReceptor", analisisDal.OrdenTrabajo.CuerpoReceptor.Nombre);
            ReportParameter paramFrecMuestr = new ReportParameter("FrecuenciaMuestreo", analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
            ReportParameter paramNormaOPmts = new ReportParameter("NormaOParametros", normaOParametros);
            ReportParameter paramUbicaMuest = new ReportParameter("UbicacionMuestreo", ubicacionDeMuestra);
            string selloDigitalCNA = analisisDal.AnalisisFirmas.Where(a => a.AnalisisID == analisisDal.AnalisisID).Last().SelloDigital;
            ReportParameter paramSelloDeCNA = new ReportParameter("SelloCONAGUA", selloDigitalCNA);
            ReportParameter paramAcuseRegla = new ReportParameter("AcuseRegla", string.Format(ap.AcuseTexto, ap.AcuseRegla, ap.AcuseAnio));

            listParametrosAcuse.Add(paramAnalisisID);
            listParametrosAcuse.Add(paramNumInforme);
            listParametrosAcuse.Add(paramHoraDFirma);
            listParametrosAcuse.Add(paramNomPuntDes);
            listParametrosAcuse.Add(paramCuerpoRece);
            listParametrosAcuse.Add(paramFrecMuestr);
            listParametrosAcuse.Add(paramNormaOPmts);
            listParametrosAcuse.Add(paramUbicaMuest);
            listParametrosAcuse.Add(paramSelloDeCNA);

            if (analisisDal.Procesado)
                listParametrosAcuse.Add(paramAcuseRegla);

            var fileAcusePdf = OpenReports.BufferPDFRemoteReport(nombreReportAcuse, listParametrosAcuse);

            Hashtable ht = new Hashtable();

            ht.Add(nombreReportAnalisis + ".pdf", fileAnalisisPdf);
            ht.Add(nombreReportAcuse + ".pdf", fileAcusePdf);
            ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
            ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));

            string[] ToArr = null;

            /* Llenar con las direcciones a quienes se les enviará el correo */
            ToArr = To(AnalisisID);

            ApplicationSettings appSettings = new ApplicationSettings();
            if (appSettings.useTestMail)
                ToArr = appSettings.testMail;

            try
            {
                System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-ES");
                var fechaOT = DateTime.Now;
                var fecha = c.DateTimeFormat.GetDayName(fechaOT.DayOfWeek) + " " +
                    fechaOT.Day.ToString() + " " +
                    c.DateTimeFormat.GetMonthName(fechaOT.Month) + " de " +
                    fechaOT.Year.ToString();
                string Message = "Se le informa que la Comisión Nacional de Agua ha recibido con fecha " + fecha +
                            " a las " + DateTime.Now.ToString("HH:mm") + " horas, el informe número " + analisisDal.OrdenTrabajo.NumeroInforme + "/" + analisisBL.AnalisisNumero(AnalisisID) +
                            " del análisis de laboratorio que su representada efectuó para el contribuyente "
                            + (analisisDal.OrdenTrabajo.Cliente.Nombre +
                            " " + (analisisDal.OrdenTrabajo.Cliente.ApellidoPaterno ?? "") +
                            " " + (analisisDal.OrdenTrabajo.Cliente.ApellidoMaterno ?? "")).Trim() +
                            ", con RFC " + analisisDal.OrdenTrabajo.Cliente.RFC +
                            ".";
                foreach (var to in ToArr)
                {
                    Thread threadSendMails = new Thread(delegate ()
                    {
                        SIRALAB_BL.correo.Mailer.SendMail(to, "", "", Message, "Acuse de recibo del informe del análisis de laboratorio", ht);
                    });

                    threadSendMails.IsBackground = true;
                    threadSendMails.Start();
                }
            }
            finally
            {
                GC.Collect();
            }
        }

        public static string[] To(int AnalisisID)
        {
            ArrayList arr = new ArrayList();

            /*Laboratorio*/
            Analisis_BL anBL = new Analisis_BL();
            Analisi analisi = anBL.Obten_Analisis(AnalisisID);

            foreach (Usuario usLab in analisi.Laboratorio.Usuarios)
            {
                if (usLab.Correo != "correo@correo.com" && usLab.Correo != "corre@correo.com")
                {
                    arr.Add(usLab.Correo);
                }
            }
            /*cliente*/
            arr.Add(analisi.OrdenTrabajo.Cliente.Correo);
            return (String[])arr.ToArray(typeof(string));
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            var oT = new vw_oT();
            var filtro = ObtenerFiltro();
            //filtro.LaboratorioID = Login.LaboratorioID;

            if (!filtro.Firma)
                Session["NombreExcel"] = "Reporte_No_Firmadas_" + Guid.NewGuid();
            else
                Session["NombreExcel"] = "Reporte_Firmadas_" + Guid.NewGuid();

            IEnumerable<uspBusquedaOrdenesTrabajoExcel_Result> ordenesE = oT.ListaOrdenTrabajoExcel(filtro);
            var ordenes = ordenesE.Select(p => new ExcelExportModel
            {
                Firmada = p.Firmada,
                EstatusAnalisis = p.EstatusAnalisis,
                LaboratorioRegistraOT = p.LaboratorioRegistraOT,
                RFCLaboratorio = p.RFCLaboratorio,
                LaboratorioMuestreo = p.LaboratorioMuestreo,
                Muestreador = p.Muestreador,
                LaboratorioAnalizo = p.LaboratorioAnalizo,
                NumeroInforme = p.NumeroInforme,
                FolioCONAGUA = p.FolioCONAGUA,
                RFCUsuarioContribuyente = p.RFCUsuarioContribuyente,
                NombreUsuarioContribuyente = p.NombreUsuarioContribuyente,
                Titulo = p.Titulo,
                Anexo = p.Anexo,
                Estado = p.Estado,
                Municipio = p.Municipio,
                TipoCuerpoReceptor = p.TipoCuerpoReceptor,
                UsoCuerpoReceptor = p.UsoCuerpoReceptor,
                Norma = p.Norma,
                FechaRecepcion = p.FechaRecepcion,
                FechaConformacion = p.FechaConformacion,
                FechaEmision = p.FechaEmision,
                FechaFirma = p.FechaFirma,
                EstaHabilitado = p.EstaHabilitado,
                HorasProceso = p.HorasProceso,
                TipoMuestra = p.TipoMuestra,
                Caudal = p.Caudal,
                Parametro = p.Parametro,
                Resultado = p.Resultado,
                Exento = p.Exento,
                NoAnalizado = p.NoAnalizado,
                Unidad = p.Unidad,
                LC_MC = p.LC_MC,
                Metodo = p.Metodo
            }).ToList();

            foreach (var item in ordenes)
            {
                if (item.FechaFirma==null)
                {
                    item.EstaHabilitado = string.Empty;
                }
            }
            Exportar(ordenes);

            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ToExcel", "exportarExcel();", true);
        }

        public void Exportar(List<ExcelExportModel> list)
        {
            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add("Siralab");
                ws.Cells["A1"].LoadFromCollection<ExcelExportModel>(list, true);
                ws.Cells.AutoFitColumns();
                const string rangocell = "A1:AG1";
                using (var rng = ws.Cells["" + rangocell + ""])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(74, 150, 106));
                    rng.Style.Font.Color.SetColor(Color.White);

                    int count = 2 + list.Count;
                    using (ExcelRange col = ws.Cells[2, 18, count, 18])
                    {
                        col.Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    using (ExcelRange col = ws.Cells[2, 19, count, 19])
                    {
                        col.Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    using (ExcelRange col = ws.Cells[2, 20, count, 20])
                    {
                        col.Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    using (ExcelRange col = ws.Cells[2, 21, count, 21])
                    {
                        col.Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    using (ExcelRange col = ws.Cells[2, 22, count, 22])
                    {
                        col.Style.Numberformat.Format = "dd/mm/yyyy";
                    }

                    var fileName = "Reporte_Firmadas_" + Guid.NewGuid() + ".xlsx";
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(pck.GetAsByteArray());
                    //MemoryStream archivoExcel = new MemoryStream();
                    //pck.SaveAs(archivoExcel);
                    //Response.BinaryWrite(archivoExcel.GetBuffer());
                    Response.End();
                }
            }
        }

      
    }

    public class FiltroOrdenTrabajo : FiltroOrdenTrabajoArgBase
    {
        public FiltroOrdenTrabajo()
        {
            EstatusCancelacion = 2;
        }

    }
}