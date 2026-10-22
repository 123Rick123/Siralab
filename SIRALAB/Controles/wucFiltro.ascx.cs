using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using System.ComponentModel;
using SIRALAB_BL.common;

namespace SIRALAB.Controles
{
    public partial class wucFiltro : System.Web.UI.UserControl
    {
        #region Propiedades
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }

        [Browsable(true)]
        public event EventHandler<FiltrosLaboratorioArg> Buscar;

        [Browsable(true)]
        public event EventHandler<FiltrosLaboratorioArg> Limpiar;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Login.UsuarioRolID == enumUsuarioRol.Recaudacion.ParseToInt())
                {
                    chFirmado.Checked = true;
                    chNoFirmado.Checked = false;
                    chFirmado.Enabled = false;
                    chNoFirmado.Enabled = false;
                }
            }
        }

        protected void lnkbBuscar_Click(object sender, EventArgs e)
        {
            if (Buscar != null)            
                Buscar(this, CargarFiltro());            
        }

        protected void lnkbLimpiar_Click(object sender, EventArgs e)
        {

            //Limpiamos los criterios de busqueda
            txtBuscar.Text = "";
            txtNumero.Text = "";
            txtRFC.Text = "";
            txtFechaInicial.Text = "";
            txtFechaFinal.Text = "";
            chFirmado.Checked = true;
            chNoFirmado.Checked = true;

            //if (Limpiar != null)
            //{
            Limpiar(this, CargarFiltro());
            //}
            upBusqueda.Update();
        }

        private FiltrosLaboratorioArg CargarFiltro()
        {
            DateTime? FechaInicial_;
            if (txtFechaInicial.Text == "")
                FechaInicial_ = null;
            else
                FechaInicial_ = DateTime.Parse(txtFechaInicial.Text);


            DateTime? FechaFinal_;
            if (txtFechaFinal.Text == "")
                FechaFinal_ = null;
            else
                FechaFinal_ = DateTime.Parse(txtFechaFinal.Text);

            FiltrosLaboratorioArg filtro = new FiltrosLaboratorioArg { RFCCliente = txtRFC.Text.Trim(), Numero = txtNumero.Text.Trim(), FechaInicial = FechaInicial_, FechaFinal = FechaFinal_, Firmada = chFirmado.Checked, NoFirmada = chNoFirmado.Checked };
            return filtro;
        }
    }

    public class FiltrosLaboratorioArg : FiltroLaboratorioArgBase
    {     
        public override string RFCCliente { get; set; }
        public override string Numero { get; set; }
        public override DateTime? FechaInicial { get; set; }
        public override DateTime? FechaFinal { get; set; }
        public override bool Firmada { get; set; }
        public override bool NoFirmada { get; set; }

        public override string Titulo { get; set; }
        public override string Anexo { get; set; }
        public override string Norma { get; set; }
        public override string Cancel { get; set; }
        public override string RFCLaboratorio { get; set; }
        public override string NombreLaboratorio { get; set; }

    }

    public class FiltroslaboratorioArg : filtrolaboratoriocnaArgBase
    {
        public override string RFCLaboratorio { get; set; }
    }

    public class FiltroshabilitarAnalisArg : filtroshabilitarAnalisArgBase
    {
        public override string RFCLaboratorio { get; set; }
        public override string NombreLaboratorio { get; set; }
        public override string RFCCliente { get; set; }
        public override string NombreCliente { get; set; }
        public override int? foliocna { get; set; }
        public override string informe { get; set; }
        public override DateTime? FechaInicialC { get; set; }
        public override DateTime? FechaFinalC { get; set; }
        public override DateTime? FechaInicialF { get; set; }
        public override DateTime? FechaFinalF { get; set; }

    }
    public class FiltrosBitacora : FiltrosBitacoraBase
    {
        public override int? Id { get; set; }
        public override int? IdUser { get; set; }
        public override DateTime? FechaMod { get; set; }
        public override string NUser { get; set; }
        public override string ModuloMod { get; set; }
        public override string Firma { get; set; }
        public override string CadenaFirma { get; set; }
        public override string TipoMov { get; set; }
        public override string NSerieCert { get; set; }

    }
    
}