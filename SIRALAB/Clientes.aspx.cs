using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using SIRALAB_BL;
using AjaxControlToolkit;
using SIRALAB.controles;

namespace SIRALAB
{
    public partial class Clientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var main = (Main)this.Master.Master;
            main.MensajeDevolucion += main_MensajeDevolucion;
            wucCliente1.GuardarCaptura += wucCliente1_GuardarCaptura;
        }

        public void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
          
        }

        protected void wucCliente1_GuardarCaptura(object sender, Controles.ClienteCapturaArgs e)
        {
            if (e != null)
            {
                if (e.Transaccion != "SinTransaccion")
                {
                    if (e.ClienteID == 0 && e.DireccionID == 0)
                        this.PopMensaje("El titulo ya se encuentra registrado", "Titulo registrado", controles.Opciones.SoloAceptar, controles.Icono.information);
                    if (e.Transaccion == "NuevoRegistroCliente")
                    {
                        wucCliente1.Inhabilitar();
                        this.PopMensaje("Cliente guardado exitosamente", "Registro de cliente", controles.Opciones.SoloAceptar, controles.Icono.information);                    
                    }
                    
                    if (e.Transaccion == "TituloExistente")
                        this.PopMensaje("Titulo ya registrado, verifique nuevamente el número de titulo", "Titulo Existente", controles.Opciones.SoloAceptar, controles.Icono.information);
                    if (e.Transaccion == "ClienteExistente")
                        this.PopMensaje("Cliente ya registrado, verifique nuevamente el RFC del cliente", "Cliente Existente", controles.Opciones.SoloAceptar, controles.Icono.information);
                }
            }
        }

        protected void lnkbtnActualiza_Click(object sender, EventArgs e)
        {
            Cliente_BL cliente_bl = new Cliente_BL();
            cliente_bl.obtener(txtCliente.Text);
            if (cliente_bl.Cliente != null)
            {
                wucCliente1.CargaCliente(cliente_bl.Cliente.ClienteID);
                ScriptManager.RegisterStartupScript(lnkbtnActualiza, lnkbtnActualiza.GetType(), "moveScript", "ve(3);", true);
            }
            else
            {
                this.PopMensaje("El RFC proporcionado no se encuentra registrado en el sistema", "RFC no encontrado", controles.Opciones.SoloAceptar, controles.Icono.information);
            
            }
        }
        protected void lnkbtnNuevo_Click(object sender, EventArgs e)
        {
            wucCliente1.Limpiar();
            ScriptManager.RegisterStartupScript(lnkbtnActualiza, lnkbtnActualiza.GetType(), "moveScript", "ve(3);", true);
        }
    }
}