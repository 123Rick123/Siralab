using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using SIRALAB_BL.common;
using SIRALAB_BL;
using SIRALAB_DAL;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucCliente : System.Web.UI.UserControl
    {
        #region Propiedades

        public SIRALAB_DAL.Cliente Cliente
        {
            get { return (SIRALAB_DAL.Cliente)Session["Cliente_"]; }
            set { Session["Cliente_"] = value; }
        }

        #endregion

        [Browsable(true)]
        public event EventHandler<ClienteCapturaArgs> GuardarCaptura;


        protected void Page_Load(object sender, EventArgs e)
        {
            wucPtoDescarga1.GuardarAnexoArgs += wucPtoDescarga1_GuardarAnexoArgs;
            if (!IsPostBack)
                CargaInfo();
        }

        public void OcultarwucCliente(string Mensaje)
        {
            ClienteCapturaArgs clientecaptura = new ClienteCapturaArgs();
            clientecaptura.Transaccion = Mensaje;
            ScriptManager.RegisterStartupScript(lnkbtnClienteCancelar, lnkbtnClienteCancelar.GetType(), "moveScript", "ve(4);", true);
            GuardarCaptura(this, clientecaptura);
        }

        public void OcultarwucPto(string Mensaje)
        {
            ClienteCapturaArgs clientecaptura = new ClienteCapturaArgs();
            clientecaptura.Transaccion = Mensaje;
            GuardarCaptura(this, clientecaptura);
        }

        void wucPtoDescarga1_GuardarAnexoArgs(object sender, AnexoCapturaArgs e)
        {
            if (e != null)
            {
                if (e.Transaccion == "SinTransaccion")
                    OcultarwucPto(e.Transaccion);
                if (e.AnexoID != 0)
                {
                    CargaAnexos(this.Cliente.ClienteID);
                    ActualizarPanelClientes();
                }
                if (e.Transaccion == "TituloExistente")
                    OcultarwucPto(e.Transaccion);
            }
        }

        protected void CargaInfo()
        {
            //CARGA LOS DROP EN EL SISTEMA
            LoadDrops.instance.Estado_Load(ddlClienteEstado);
            rbl.Enabled = true;
            //VERIFICA SI EL OBJETO CLIENTE NO ESTA VACIO, POR LO TANTO MUESTRA LA INFORMACION DE SUS ANEXOS CAPTURADOS
        }

        public void CargaCliente(int ClienteID)
        {
            Cliente_BL cliente_bl = new Cliente_BL();
            cliente_bl.obtener(ClienteID);
            this.Cliente = cliente_bl.Cliente;

            //Deshabilitamos las secciones de captura de datos fiscales
            txtClienteRFC.Text = this.Cliente.RFC;
            txtClienteRFC.Enabled = false;
            txtClienteRFC.ReadOnly = true;
            txtClienteCURP.Enabled = false;
            txtClienteCURP.ReadOnly = true;
            rbl.Enabled = false;
            txtClienteNombre.Enabled = false;
            txtClienteNombre.ReadOnly = true;
            txtClienteAPaterno.Enabled = false;
            txtClienteAPaterno.ReadOnly = true;
            txtClienteAMaterno.Enabled = false;
            txtClienteAMaterno.ReadOnly = true;
            //Fin de deshabilitar las secciones de captura de datos fiscales

            txtClienteCURP.Text = this.Cliente.CURP;
            txtClienteNombre.Text = this.Cliente.Nombre;
            txtClienteAPaterno.Text = this.Cliente.ApellidoPaterno;
            txtClienteAMaterno.Text = this.Cliente.ApellidoMaterno;
            txtCorreoElectronico.Text = this.Cliente.Correo;

            if (this.Cliente.ApellidoPaterno != "")
            {
                habilitaPersonaFisica();
                rbl.SelectedValue = "1";
            }
            else
            {
                inahabilitaPersonaMoral();
                rbl.SelectedValue = "2";
            }

            Direccion_BL direccion_bl = new Direccion_BL();
            direccion_bl.obtener(this.Cliente.DireccionID);

            txtClienteCalle.Text = direccion_bl.Direccion.Calle;
            txtClienteNExterior.Text = direccion_bl.Direccion.NumeroExterior;
            txtClienteNInterior.Text = direccion_bl.Direccion.NumeroInterior;
            txtClienteCP.Text = direccion_bl.Direccion.CP.ToString();
            ddlClienteEstado.SelectedValue = direccion_bl.Direccion.EstadoID.ToString();

            LoadDrops.instance.Municipio_Load(ddlClienteMunicipio, direccion_bl.Direccion.EstadoID);
            ddlClienteMunicipio.SelectedValue = direccion_bl.Direccion.MunicipioID.ToString();

            LoadDrops.instance.Localidad(ddlClienteLocalidad, direccion_bl.Direccion.EstadoID, direccion_bl.Direccion.MunicipioID);
            ddlClienteLocalidad.SelectedValue = direccion_bl.Direccion.LocalidadID.ToString();

            LoadDrops.instance.Colonia(ddlClienteColonia, direccion_bl.Direccion.EstadoID, direccion_bl.Direccion.MunicipioID);
            ddlClienteColonia.SelectedValue = direccion_bl.Direccion.ColoniaID.ToString();

            CargaAnexos(ClienteID);
        }

        public void CargaAnexos(int ClienteID)
        {
            lnkbtnPtoDescarga.Visible = true;
            Consecion_BL consecion_bl = new Consecion_BL();
            Anexo_BL anexo_bl = new Anexo_BL();

            //CONSULTA LOS ANEXOS GUARDADOS EN ESE TITULO
            var list_consecion = consecion_bl.Listado_Consecion(ClienteID);
            rptTitulo.DataSource = list_consecion;
            rptTitulo.DataBind();
        }

        #region Eventos
        protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbl.SelectedItem.Value == "1")
                habilitaPersonaFisica();
            else
                inahabilitaPersonaMoral();
        }
        protected void lnkbtnClienteNuevoAnexo_Click(object sender, EventArgs e)
        {
            int ConsecionID = int.Parse(((LinkButton)sender).CommandArgument);
            int ClienteID = int.Parse(((LinkButton)sender).CommandName);
            wucPtoDescarga1.NuevoAnexo(ClienteID, ConsecionID);
            wucPtoDescarga1.ActualizarPanelPto();
            wucPtoDescarga1.Transaccion = "Actualizacion";
            ScriptManager.RegisterStartupScript(lnkbtnClienteNuevoAnexo, lnkbtnClienteNuevoAnexo.GetType(), "moveScript", "movePosition(2);", true);
        }

        protected void lnkbtnGuardar_Click(object sender, EventArgs e)
        {
            Cliente_BL cliente_bl = new Cliente_BL();
            ClienteCapturaArgs cliente = new ClienteCapturaArgs();
            bool Continua = false;

            if (this.Cliente != null)
            {
                cliente.ClienteID = this.Cliente.ClienteID;
                cliente.DireccionID = this.Cliente.DireccionID;
                Continua = true;
            }
            else
            {
                cliente.ClienteID = null;
                cliente.DireccionID = null;
                cliente_bl.obtener(txtClienteRFC.Text);
                if (cliente_bl.Cliente != null)
                {
                    Continua = false;
                    cliente.Transaccion = "ClienteExistente";
                    OcultarwucCliente(cliente.Transaccion);
                }
                else
                    Continua = true;
            }
            if (Continua)
            {
                cliente.Nombre = txtClienteNombre.Text;
                cliente.ApellidoPaterno = txtClienteAPaterno.Text;
                cliente.ApellidoMaterno = txtClienteAMaterno.Text;
                cliente.RFC = txtClienteRFC.Text;
                cliente.CURP = txtClienteCURP.Text;
                cliente.Correo = txtCorreoElectronico.Text;

                DireccionCapturaArgs direccion = new DireccionCapturaArgs();

                if (this.Cliente == null)
                    direccion.DireccionID = 0;
                else
                    direccion.DireccionID = this.Cliente.DireccionID;

                direccion.Calle = txtClienteCalle.Text;
                direccion.NumeroExteior = txtClienteNExterior.Text;
                direccion.NumeroInterior = txtClienteNInterior.Text;
                direccion.CP = int.Parse(txtClienteCP.Text);
                direccion.EstadoID = int.Parse(ddlClienteEstado.SelectedValue);
                direccion.MunicipioID = int.Parse(ddlClienteMunicipio.SelectedValue);
                direccion.LocalidadID = int.Parse(ddlClienteLocalidad.SelectedValue);
                direccion.ColoniaID = int.Parse(ddlClienteColonia.SelectedValue);
                cliente.dir = direccion;

                cliente_bl.Guarda(cliente);
                cliente.ClienteID = cliente_bl.Cliente.ClienteID;
                this.Cliente = cliente_bl.Cliente;

                wucPtoDescarga1.Cliente = cliente_bl.Cliente;

                cliente.Transaccion = "NuevoRegistroCliente";
                lnkbtnPtoDescarga.Visible = true;
                GuardarCaptura(this, cliente);
            }
        }

        public void Inhabilitar()
        {
            revRFC_fisica.Enabled = false;
            revRFC_moral.Enabled = false;
        }

        public void Ocultar()
        {
            wucPtoDescarga1.Limpiar();
            OcultarwucCliente("SinTransaccion");
        }

        protected void ddlClienteEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDrops.instance.Municipio_Load(ddlClienteMunicipio, int.Parse(ddlClienteEstado.SelectedValue));
        }

        protected void ddlClienteMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDrops.instance.Localidad(ddlClienteLocalidad, int.Parse(ddlClienteEstado.SelectedValue), int.Parse(ddlClienteMunicipio.SelectedValue));
            LoadDrops.instance.Colonia(ddlClienteColonia, int.Parse(ddlClienteEstado.SelectedValue), int.Parse(ddlClienteMunicipio.SelectedValue));
        }

        protected void rptTitulo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var consecion = (Consecion)e.Item.DataItem;
            var rptAnexo = (Repeater)e.Item.FindControl("rptAnexo");

            Anexo_BL anexo_bl = new Anexo_BL();
            if (consecion != null)
            {
                var listanexo = anexo_bl.Listado_Anexo(consecion.ConsecionID);
                List<AnexoCapturaArgs> Anexo = new List<AnexoCapturaArgs>();
                foreach (var i in listanexo)
                {
                    Coordenadas_BL coordenada = new Coordenadas_BL(i.Latitud, i.Longitud);
                    Anexo.Add(new AnexoCapturaArgs
                    {
                        AnexoID = i.AnexoID,
                        Anexo =i.Anexo1,
                        GradosLatitud = coordenada.GradosLat,
                        MinutosLatitud = coordenada.MinutosLat,
                        SegundosLatitud = coordenada.SegundosLat,
                        GradosLongitud = coordenada.GradosLng,
                        MinutosLongitud = coordenada.MinutosLng,
                        SegundosLongitud = coordenada.SegundosLng,
                        Direc = i.Direccion
                    });
                }
                rptAnexo.DataSource = Anexo;
                rptAnexo.DataBind();
            }
        }

        protected void lnkbtnPtoDescarga_Click(object sender, EventArgs e)
        {
            wucPtoDescarga1.Limpiar();
            wucPtoDescarga1.ActualizarPanelPto();
            wucPtoDescarga1.Cliente = this.Cliente;
            wucPtoDescarga1.Transaccion = "Nuevo";
            ScriptManager.RegisterStartupScript(lnkbtnClienteNuevoAnexo, lnkbtnClienteNuevoAnexo.GetType(), "moveScript", "movePosition(2);", true);
        }

        public void Limpiar()
        {
            revRFC_fisica.Enabled = true;
            revRFC_moral.Enabled = false;

            lnkbtnPtoDescarga.Visible = false;
            rbl.Enabled = true;
            txtClienteCURP.Enabled = true;
            txtClienteCURP.ReadOnly = false;

            txtClienteNombre.Enabled = true;
            txtClienteNombre.ReadOnly = false;
            txtClienteAPaterno.Enabled = true;
            txtClienteAPaterno.ReadOnly = false;
            txtClienteAMaterno.Enabled = true;
            txtClienteAMaterno.ReadOnly = false;

            txtClienteCURP.Visible = true;
            txtClienteNombre.Visible = true;
            txtClienteAPaterno.Visible = true;
            txtClienteAMaterno.Visible = true;
            DivCURP.Visible = true;
            DIVAPaterno.Visible = true;
            DIVAMaterno.Visible = true;
            DIVtxtAPaterno.Visible = true;
            DIVtxtAMaterno.Visible = true;

            this.Cliente = null;
            rbl.SelectedValue = "1";
            txtClienteRFC.Text = "";
            txtClienteRFC.Enabled = true;
            txtClienteRFC.ReadOnly = false;

            txtClienteCURP.Text = "";
            txtClienteNombre.Text = "";
            txtClienteAPaterno.Text = "";
            txtClienteAMaterno.Text = "";
            txtCorreoElectronico.Text = "";
            txtClienteCalle.Text = "";
            txtClienteNExterior.Text = "";
            txtClienteNInterior.Text = "";
            txtClienteCP.Text = "";
            ddlClienteEstado.SelectedValue = "0";
            ddlClienteMunicipio.SelectedValue = "0";
            ddlClienteColonia.SelectedValue = "0";
            ddlClienteLocalidad.SelectedValue = "0";

            rptTitulo.DataSource = null;
            rptTitulo.DataBind();
        }

        public void ActualizarPanelClientes()
        {
            upClientes.Update();
        }

        protected void lnkbtnAnexoConsulta_Click(object sender, EventArgs e)
        {
            int AnexoID = int.Parse(((LinkButton)sender).CommandArgument);
            wucPtoDescarga1.Transaccion = "Actualizacion";
            wucPtoDescarga1.Cliente = this.Cliente;
            wucPtoDescarga1.CargaAnexo(AnexoID);
            wucPtoDescarga1.ActualizarPanelPto();
            ScriptManager.RegisterStartupScript(lnkbtnClienteNuevoAnexo, lnkbtnClienteNuevoAnexo.GetType(), "moveScript", "movePosition(2);", true);
        }

        protected void lnkbtnClienteCancelar_Click(object sender, EventArgs e)
        {
            wucPtoDescarga1.Limpiar();
            OcultarwucCliente("SinTransaccion");
        }

        public void habilitaPersonaFisica()
        {
            txtClienteCURP.Visible = true;
            DivCURP.Visible = true;
            DIVAPaterno.Visible = true;
            DIVAMaterno.Visible = true;
            DIVtxtAPaterno.Visible = true;
            DIVtxtAMaterno.Visible = true;
            revRFC_fisica.Enabled = true;
            revRFC_moral.Enabled = false;
        }

        public void inahabilitaPersonaMoral()
        {
            revRFC_fisica.Enabled = false;
            revRFC_moral.Enabled = true;
            txtClienteCURP.Visible = false;
            DivCURP.Visible = false;
            DIVAPaterno.Visible = false;
            DIVAMaterno.Visible = false;
            DIVtxtAPaterno.Visible = false;
            DIVtxtAMaterno.Visible = false;
        }
        #endregion
    }
}