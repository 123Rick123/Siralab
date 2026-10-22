using System.Data;
using SIRALAB.Controles;
using SIRALAB_BL;
using SIRALAB_DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SIRALAB_BL.common;

namespace SIRALAB
{
    public partial class Signatario : System.Web.UI.Page
    {
        #region Propiedades

        public static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }
        public string Excel
        {
            get { return Session["ListaExportar"] == null ? string.Empty : (string)Session["ListaExportar"]; }
            set { Session["ListaExportar"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDrops.instance.Laboratorio(ddlLaboratorio);
                Buscar(ObtenerFiltro());
            }
        }

        public void Buscar(FiltrosMuestreador filtro)
        {
            var oT = new vw_oT();
            IEnumerable<uspConsultaSignatarios_Result> List = oT.ListaMuestreador(filtro);
            var muestreadores = List.Select(p => new SignatarioLista
            {
                LaboratorioId = p.LaboratorioID,
                Laboratorio = p.Laboratorio,
                Externo = p.Externos ?? 0,
                Interno = p.Internos ?? 0
            }).ToList();
            wucListaSignatario.Initialize(muestreadores.OrderBy(x => x.Laboratorio).ToList());
            Exportar(muestreadores);
        }

        public string HTMLEncodeSpecialChars(string text)
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

        private FiltrosMuestreador ObtenerFiltro()
        {
            var FI_ = txtFechaInicio.Text;
            var FF_ = txtFechaFin.Text;

            var filter = new FiltrosMuestreador
            {
                LaboratorioId = ddlLaboratorio.SelectedValue == "0" ? null : (int?)int.Parse(ddlLaboratorio.SelectedValue),
                FechaInicio = FI_ == "" ? null : (DateTime?)(new DateTime(int.Parse(FI_.Split('/')[2]), int.Parse(FI_.Split('/')[1]), int.Parse(FI_.Split('/')[0]))),
                FechaFin = FF_ == "" ? null : (DateTime?)(new DateTime(int.Parse(FF_.Split('/')[2]), int.Parse(FF_.Split('/')[1]), int.Parse(FF_.Split('/')[0])))
            };

            return filter;
        }

        protected void lnkBuscar_OnClick(object sender, EventArgs e)
        {
            Buscar(ObtenerFiltro());
        }

        protected void lnkLimpiar_OnClick(object sender, EventArgs e)
        {
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            ddlLaboratorio.SelectedValue = "0";
            Buscar(ObtenerFiltro());
        }

        public string Exportar(IEnumerable<SignatarioLista> list)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            StringBuilder Table = new StringBuilder(string.Empty);
            string styleTable = "style='border:#000 solid 1px;'";
            string styleHeader = "style='border:#000 solid 1px;text-align:center;color: Black;text-decoration: none;background-color: #F6F6F6;border: hidden;font-weight: bold;font-family: Arial;font-size: 12px'";
            string styleHeader_2 = "style='border:#000 solid 1px;text-align:center;color: Black;text-decoration: none;background-color: #F6F6F6;border: hidden;font-weight: bold;font-family: Arial;font-size: 12px'";
            string styleRow_Normal = "style='border:#000 solid 1px;text-align:center;font-family: Arial;'";
            string styleRow_Normal_Bold = "style='border:#000 solid 1px;text-align:center;font-family: Arial;font-weight: bold'";

            Table.Append("<span style='color:black; font-family:Arial;font-size:14px;font-weight: bold'>" + "Consulta de signatarios por laboratorio" + "</span>");
            Table.Append("<table " + styleTable + ">");

            Table.Append("<tr>");
            Table.Append("<th " + styleHeader + ">" + ("Laboratorio") + "</th>");
            Table.Append("<th " + styleHeader_2 + ">" + ("Internos") + "</th>");
            Table.Append("<th " + styleHeader_2 + ">" + ("Externos") + "</th>");
            Table.Append("</tr>");

            foreach (var row in list)
            {
                Table.Append("<tr>");
                Table.Append("<td " + styleRow_Normal + ">" + HTMLEncodeSpecialChars(row.Laboratorio) + "</td>");
                Table.Append("<td " + styleRow_Normal + ">" + row.Interno + "</td>");
                Table.Append("<td " + styleRow_Normal + ">" + row.Externo + "</td>");
                Table.Append("</tr>");
            }

            Table.Append("</table>");
            Excel = Table.ToString();
            Session["NombreExcel"] = "ConsultaSignatarios";
            return Table.ToString();
        }
    }

    public class FiltrosMuestreador : FiltrosMuestreadorArgsBase
    {

    }
}