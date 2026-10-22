using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System.ComponentModel;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucConformacion : System.Web.UI.UserControl
    {
        #region Propiedades

        [Browsable(true)]
        public event EventHandler<FiltroMensual> Respuesta;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargaDrops();
            }
        }

        public void CargaDrops()
        {          
            LoadDrops.instance.Norma(ddlNorma);
            rbtPeriodo_Mensual_CheckedChanged(null,null);

            ListItem li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
           
            ddlTitulo.Items.Insert(0, li);
            ddlAnexo.Items.Insert(0, li);            
          
        }        
  
        public void Limpiar()
        {
        }

        public void ActualizarPanelPto()
        {
            updpnlConformacion.Update();            
        }

        protected void lnkbtnPtoConsecion_Click(object sender, EventArgs e)
        {
            Cliente_BL cliente_bl = new Cliente_BL();
            cliente_bl.obtener(txtPtoConsecion.Text);
            if(cliente_bl.Cliente!=null)
            LoadDrops.instance.Conseciones(ddlTitulo, cliente_bl.Cliente.ClienteID); 
        }

        protected void ddlTitulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Consecion_BL consecion_bl = new Consecion_BL();
            consecion_bl.Obten(int.Parse(ddlTitulo.SelectedValue));
            var consecion = consecion_bl.Consecion;

            if (consecion != null)
            {
                Anexo_BL anexo_bl = new Anexo_BL();
                LoadDrops.instance.Anexos(ddlAnexo, consecion.ConsecionID);
            }
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

            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewAdvance", "desplegarAvanzado();", true);
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
            //upBusqueda.Update();

            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewAdvance", "desplegarAvanzado();", true);
        }

        protected void lnkbtnPtoGenerar_Click(object sender, EventArgs e)
        {
            


            DateTime? FechaInicial_ = null, FechaFinal_ = null;

            if (rbtPeriodo_Mensual.Checked)
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

            FiltroMensual FM = new FiltroMensual();
            FM.RFC = txtPtoConsecion.Text;
            FM.Titulo = ddlTitulo.SelectedItem.Text;
            FM.Anexo = int.Parse(ddlAnexo.SelectedValue);
            FM.Anio = int.Parse(ddlAnio.SelectedValue);
            FM.Norma = int.Parse(ddlNorma.SelectedValue);
            FM.Periodo = int.Parse(ddlPeriodo.SelectedValue);
            FM.Tipo = (rbtPeriodo_Mensual.Checked) ? 1 : 2;
            FM.FechaInicial =(DateTime)FechaInicial_;
            FM.FechaFinal = (DateTime)FechaFinal_;

            this.Respuesta(this, FM);
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
        
        private int MesEnTrimestre(int mes)
        {
            int res = mes % 3;
            var selecion = (int)(mes / 3);
            int resultado = selecion + (res > 0 ? 1 : 0);
            return resultado;
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

        public class FiltroMensual
        {
            public string RFC { get; set; }
            public string Titulo { get; set; }
            public int Anexo { get; set; }
            public int Tipo { get; set; }
            public int Anio { get; set; }
            public int Periodo { get; set; }
            public int Norma { get; set; }
            public string OT { get; set; }
            public DateTime FechaInicial { get; set; }
            public DateTime FechaFinal { get; set; }
        
        }
    }   
}
