using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_DAL;
using System.ComponentModel;
using SIRALAB_BL;
using SIRALAB_BL.negocio.LaboratoriosApercibidos;
using System.Web.UI.WebControls;
using SIRALAB_BL.negocio.login;

namespace SIRALAB.Controles
{
    public partial class wucSucursalLogin : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<LoginSucursalArg> Seleccionar;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lnkbAceptarReincidencia_Click(object sender, EventArgs e)
        {
            var url = txtURL.Value;
            Response.Redirect(url);
        }
        protected void lnkbAceptarApercibimiento_Click(object sender, EventArgs e)
        {
            var url = txtURL.Value;
            Response.Redirect(url);
        }
        protected void lnkbAceptarReincidenciaPreventiva_Click(object sender, EventArgs e)
        {
            var url = txtURL.Value;
            Response.Redirect(url);
        }
        protected void lnkbSeleccionar_Click(object sender, EventArgs e)
        {
            if (Seleccionar != null)
            {
                var LaboratorioID = int.Parse(((LinkButton)sender).CommandArgument);
                Laboratorio_BL lBL = new Laboratorio_BL();

                var lab = lBL.LaboratorioContext.Laboratorios.Where(p => p.LaboratorioID == LaboratorioID).FirstOrDefault();

                Seleccionar(this, new LoginSucursalArg { laboratorio = lab });

            }
        }



        public void Mostrar(string RFC)
        {
            Laboratorio_BL lBL = new Laboratorio_BL();
            var lab = lBL.LaboratorioContext.Laboratorios.Where(p => p.RFC == RFC && p.Usuarios.Count > 0).ToList();

            rptrSucursales.DataSource = lab;
            rptrSucursales.DataBind();

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "mostrarSucursal", $"$('#ModalSucursal').modal()", true);
            upSucursal.Update();
        }
        public void MostrarModal(int LaboratorioID, string URL)
        {
            LabApercibidosYReincidentesBLL ApercibidosReincientesBLL = new LabApercibidosYReincidentesBLL();
            bool TieneReincidencia = false;
            bool TieneApercibimiento = false;
            bool TieneReincidenciaPreventiva = false;
            txtidLaboratorio.Value = LaboratorioID.ToString();
            txtURL.Value = URL;
            TieneReincidencia = ApercibidosReincientesBLL.TieneReincidenciaByIdLaboratorio(LaboratorioID);
            if (TieneReincidencia)
            {
                
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "mostrarSucursal", $"$('#alertaSimpleReincidencia').modal()", true);
                upSucursal.Update();
            }
            TieneReincidenciaPreventiva = ApercibidosReincientesBLL.TieneReincidenciaPreventivaByIdLaboratorio(LaboratorioID);
            if (TieneReincidenciaPreventiva)
            {

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "mostrarSucursal", $"$('#alertaSimpleReincidenciaPreventiva').modal()", true);
                upSucursal.Update();
            }
            TieneApercibimiento = ApercibidosReincientesBLL.TieneApercibimientoByIdLaboratorio(LaboratorioID);
            if (TieneApercibimiento)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "mostrarSucursal", $"$('#alertaSimpleApercibimiento').modal()", true);
                upSucursal.Update();
            }
            else if(TieneApercibimiento==false && TieneReincidencia==false && TieneReincidenciaPreventiva==false)
            {
                Response.Redirect(URL);
            }
            
  
        }
        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {

        }
    }

    public class LoginSucursalArg : EventArgs
    {
        public Laboratorio laboratorio { get; set; }
    }
}