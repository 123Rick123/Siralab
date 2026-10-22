using AjaxControlToolkit;
using SIRALAB_BL.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucFiltro_Consulta_Ordenes : System.Web.UI.UserControl
    {
        #region Propiedades

        [Browsable(true)]
        public event EventHandler<FiltrosConsultaOrdenesArg> Buscar;

        [Browsable(true)]
        public event EventHandler<FiltrosConsultaOrdenesArg> Limpiar;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarDrops();
        }

        protected void lnkbBuscar_Click(object sender, EventArgs e)
        {
            if (Buscar != null)            
                Buscar(this, CargarFiltro());

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewAdvance", "desplegarAvanzado(true);", true);
        }

        protected void lnkbLimpiar_Click(object sender, EventArgs e)
        {

            //Limpiamos los criterios de busqueda
            txtBuscar.Text = "";
            txtNumero.Text = "";
            txtRFC.Text = "";
            ddlAnio.ClearSelection();
            ddlPeriodo.ClearSelection();
            chFirmado.Checked = true;
            //chNoFirmado.Checked = true;

            if (Limpiar != null)
            {
                Limpiar(this, CargarFiltro());
            }
            upBusqueda.Update();
        }

        protected void rbtPeriodo_Mensual_CheckedChanged(object sender, EventArgs e)
        {
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;

            if (rbtPeriodo_Mensual.Checked)
            {
                ddlPeriodo.LoadDropSinSel("Nombre", "Value", Meses_Anio());
                ddlPeriodo.ClearSelection();
                ddlPeriodo.Items.FindByValue(mes.ToString()).Selected = true;
            }

            List<Anio> Anios = new List<Anio>();
            for (int i = anio - 4; i <= anio + 5; i++)
            {
                Anios.Add(new Anio { Value = i, Nombre = i.ToString() });
            }
            ddlAnio.LoadDropSinSel("Nombre", "Value", Anios);
            ddlAnio.ClearSelection();
            ddlAnio.Items.FindByValue(anio.ToString()).Selected = true;
            upBusqueda.Update();

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewAdvance", "desplegarAvanzado();", true);
        }

        protected void rbtPeriodo_Trimestral_CheckedChanged(object sender, EventArgs e)
        {
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;

            if (rbtPeriodo_Trimestral.Checked)
            {
                ddlPeriodo.LoadDropSinSel("Nombre", "Value", Trimestres());
                ddlPeriodo.ClearSelection();
                var messelec = MesEnTrimestre(mes).ToString();
                ddlPeriodo.Items.FindByValue(messelec).Selected = true;
            }

            List<Anio> Anios = new List<Anio>();
            for (int i = anio - 4; i <= anio + 5; i++)
            {
                Anios.Add(new Anio { Value = i, Nombre = i.ToString() });
            }
            ddlAnio.LoadDropSinSel("Nombre", "Value", Anios);
            ddlAnio.ClearSelection();
            ddlAnio.Items.FindByValue(anio.ToString()).Selected = true;
            upBusqueda.Update();

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewAdvance", "desplegarAvanzado();", true);
        }

        private FiltrosConsultaOrdenesArg CargarFiltro()
        {
            DateTime? FechaInicial_ = null, FechaFinal_ = null;

            if(rbtPeriodo_Mensual.Checked)
            {
                int anio = int.Parse(ddlAnio.SelectedValue);
                int mes = int.Parse(ddlPeriodo.SelectedValue);

                FechaInicial_ = new DateTime(anio, mes, 1);
                FechaFinal_ = ((DateTime)FechaInicial_).AddMonths(1).AddDays(-1);
            }

            if (rbtPeriodo_Trimestral.Checked)
            {
                int anio = int.Parse(ddlAnio.SelectedValue);
                int mes = int.Parse(ddlPeriodo.SelectedValue);
                mes = (3 * mes) - 2;

                FechaInicial_ = new DateTime(anio, mes, 1);
                FechaFinal_ = ((DateTime)FechaInicial_).AddMonths(3).AddDays(-1);
            }

            FiltrosConsultaOrdenesArg filtro = new FiltrosConsultaOrdenesArg
            {
                RFCCliente = txtRFC.Text.Trim(),
                Numero = txtNumero.Text.Trim(),
                FechaInicial = FechaInicial_,
                FechaFinal = FechaFinal_,
                Firmados = chFirmado.Checked,
                //NoFirmados = chNoFirmado.Checked
            };
            
            return filtro;
        }

        private void CargarDrops()
        {

            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;

            if (rbtPeriodo_Mensual.Checked)
            {
                ddlPeriodo.LoadDropSinSel("Nombre", "Value", Meses_Anio());
                ddlPeriodo.ClearSelection();
                ddlPeriodo.Items.FindByValue(mes.ToString()).Selected = true;
            }

            if (rbtPeriodo_Trimestral.Checked)
            {
                ddlPeriodo.LoadDropSinSel("Nombre", "Value", Trimestres());
                ddlPeriodo.ClearSelection();
                var messelec = MesEnTrimestre(mes).ToString();
                ddlPeriodo.Items.FindByValue(messelec).Selected = true;
            }

            List<Anio> Anios = new List<Anio>();
            for (int i = anio - 9; i <= anio; i++)
            {
                Anios.Add(new Anio { Value = i, Nombre = i.ToString() });
            }
            ddlAnio.LoadDropSinSel("Nombre", "Value", Anios);
            ddlAnio.ClearSelection();
            ddlAnio.Items.FindByValue(anio.ToString()).Selected = true;
        }

        private List<Meses> Meses_Anio()
        {
            int anio = DateTime.Now.Year;
            List<Meses> Meses = new List<Meses>();
            for (int i = 1; i <= 12; i++)
            {
                DateTime Fecha = new DateTime(anio, i, 1);
                System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");

                Meses.Add(new Meses { Value = i, Nombre = c.DateTimeFormat.GetMonthName(Fecha.Month) });
            }

            return Meses;
        }

        private List<Meses> Trimestres()
        {
            int anio = DateTime.Now.Year;
            List<Meses> Meses = new List<Meses>();
            int value = 1;
            for (int i = 1; i <= 12; i += 3)
            {
                DateTime Fecha = new DateTime(anio, i, 1);
                DateTime Fecha_Final = new DateTime(anio, i, 1).AddMonths(3).AddDays(-1);
                System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                Meses.Add(new Meses { Value = value, Nombre = c.DateTimeFormat.GetMonthName(Fecha.Month) + " - " + c.DateTimeFormat.GetMonthName(Fecha_Final.Month) });
                value++;
            }

            return Meses;
        }

        private int MesEnTrimestre(int mes)
        {
            int res = mes % 3;
            var selecion = (int)(mes / 3);
            int resultado = selecion + (res > 0 ? 1 : 0);
            return resultado;
        }

        private class Meses
        {
            public int Value { get; set; }
            public string Nombre { get; set; }
        }
        private class Anio
        {
            public int Value { get; set; }
            public string Nombre { get; set; }
        }
    }

    public class FiltrosConsultaOrdenesArg : EventArgs
    {
        public string RFCCliente { get; set; }
        public string Numero { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool Firmados { get; set; }
        public bool NoFirmados { get; set; }
    }
}