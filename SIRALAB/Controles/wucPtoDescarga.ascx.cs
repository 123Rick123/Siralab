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
    public partial class wucPtoDescarga : System.Web.UI.UserControl
    {
        #region Propiedades
        public SIRALAB_DAL.Cliente Cliente
        {
            get { return (SIRALAB_DAL.Cliente)Session["Cliente_"]; }
            set { Session["Cliente_"] = value; }
        }

        public SIRALAB_DAL.Anexo Anexo
        {
            get { return (SIRALAB_DAL.Anexo)Session["Anexo_"]; }
            set { Session["Anexo_"] = value; }
        }

        public int ConsecionID
        {
            get { return int.Parse(hfPtoConsecion.Value == "" ? "0" : hfPtoConsecion.Value); }
            set { hfPtoConsecion.Value = value.ToString(); }
        }

        public string Transaccion
        {
            get { return hfTransaccion.Value != "" ? hfTransaccion.Value : ""; }
            set { hfTransaccion.Value = value; }
        }
        #endregion
        
        [Browsable(true)]
        public event EventHandler<AnexoCapturaArgs> GuardarAnexoArgs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargaInfo();
        }
        #region Metodos

        public void CargaInfo()
        {
            //CArga Drops de cuerpo Receptor y uso del agua
            LoadDrops.instance.CuerpoReceptor(ddlCuerpoReceptor);
            LoadDrops.instance.UsoAgua(ddlUsoAgua);

            //CARGA LOS DROP EN EL SISTEMA            
            LoadDrops.instance.Estado_Load(ddlPtoEstado);

            //VERIFICA SI EL OBJETO CLIENTE NO ESTA VACIO, POR LO TANTO MUESTRA LA INFORMACION DE SUS ANEXOS CAPTURADOS
            Consecion_BL consecion_bl = new Consecion_BL();
            Anexo_BL anexo_bl = new Anexo_BL();

            //CONSULTA LOS ANEXOS GUARDADOS EN ESE TITULO
            var list_consecion = consecion_bl.Listado_Consecion(1);
        }

        public void NuevoAnexo(int ClienteID, int ConsecionID)
        {
            //Instancia de cliente y se obtiene el valor del objetco cliente
            Cliente_BL cliente_bl = new Cliente_BL();
            cliente_bl.obtener(ClienteID);
            this.Cliente = cliente_bl.Cliente;

            //Instancia de consecion y se obtiene el valor del objetco consecion
            Consecion_BL consecion_bl = new Consecion_BL();
            consecion_bl.Obten(ConsecionID);
            var consecion = consecion_bl.Consecion;
            this.ConsecionID = consecion.ConsecionID;

            txtPtoTitulo.Text = consecion.Titulo;
            txtPtoTitulo.Enabled = false;
        }

        public void Limpiar()
        {
            this.Anexo = null;
            //this.Transaccion = "";

            txtPtoTitulo.Text = "";
            txtPtoTitulo.Enabled = true;
            txtPtoAnexo.Text = "";
            txtPtoCalle.Text = "";
            txtPtoNExterior.Text = "";
            txtPtoNInterior.Text = "";
            txtPtoDatum.Text = "";
            txtPtoLatitudGrados.Text = "";
            txtPtoLatitudMinutos.Text = "";
            txtPtoLatitudSeg.Text = "";
            txtPtoLongitudGrados.Text = "";
            txtPtoLongitudMinutos.Text = "";
            txtPtoLongitudSeg.Text = "";
            txtPtoCuerpoR.Text = "";
            txtPtoFuenteA.Text = "";
            txtPtoOrigenD.Text = "";

            txtPtoCP.Text = "";
            ddlPtoEstado.SelectedValue = "0";
            ddlPtoMunicipio.SelectedValue = "0";
            ddlPtoLocalidad.SelectedValue = "0";
            ddlPtoColonia.SelectedValue = "0";
            rblPtoEfluente.SelectedValue = "0";
            ddlLongitud.ClearSelection();
            ddlLatitud.ClearSelection();

            ddlCuerpoReceptor.ClearSelection();
            ddlUsoAgua.ClearSelection();
            txtNumeroMedidor.Text = "";
            
        }

        public void CargaAnexo(int AnexoID)
        {
            //CArga Drops de cuerpo Receptor y uso del agua
            LoadDrops.instance.CuerpoReceptor(ddlCuerpoReceptor);
            LoadDrops.instance.UsoAgua(ddlUsoAgua);

            Anexo_BL anexo = new Anexo_BL();
            anexo.Obten(AnexoID);
            this.Anexo = anexo.Anexo;
            txtPtoTitulo.Enabled = false;

            txtPtoTitulo.Text = anexo.Anexo.Consecion.Titulo;
            txtPtoAnexo.Text = anexo.Anexo.Anexo1.ToString();
            txtPtoCalle.Text = anexo.Anexo.Direccion.Calle;
            txtPtoCP.Text = anexo.Anexo.Direccion.CP.ToString();

            txtPtoNExterior.Text = anexo.Anexo.Direccion.NumeroExterior.ToString();
            txtPtoNInterior.Text = anexo.Anexo.Direccion.NumeroInterior.ToString();
            ddlPtoEstado.SelectedValue = anexo.Anexo.Direccion.EstadoID.ToString();

            ddlCuerpoReceptor.SelectedValue = anexo.Anexo.CuerpoReceptorID.ToString();
            ddlUsoAgua.SelectedValue = anexo.Anexo.UsoAguaID.ToString();
            txtNumeroMedidor.Text = anexo.Anexo.Medidor;

            LoadDrops.instance.Municipio_Load(ddlPtoMunicipio, anexo.Anexo.Direccion.EstadoID);
            ddlPtoMunicipio.SelectedValue = anexo.Anexo.Direccion.MunicipioID.ToString();
            LoadDrops.instance.Localidad(ddlPtoLocalidad, anexo.Anexo.Direccion.EstadoID, anexo.Anexo.Direccion.MunicipioID);
            LoadDrops.instance.Colonia(ddlPtoColonia, anexo.Anexo.Direccion.EstadoID, anexo.Anexo.Direccion.MunicipioID);
            ddlPtoLocalidad.SelectedValue = anexo.Anexo.Direccion.LocalidadID.ToString();
            ddlPtoColonia.SelectedValue = anexo.Anexo.Direccion.ColoniaID.ToString();
            txtPtoDatum.Text = anexo.Anexo.Datum;
            rblPtoEfluente.SelectedValue = anexo.Anexo.Efluente.ToString();

            Coordenadas_BL coordenada = new Coordenadas_BL(anexo.Anexo.Latitud, anexo.Anexo.Longitud);
            txtPtoLatitudGrados.Text = coordenada.GradosLat.ToString();
            txtPtoLatitudMinutos.Text = coordenada.MinutosLat.ToString();
            txtPtoLatitudSeg.Text = Math.Round(coordenada.SegundosLat).ToString("#.00");

            ddlLatitud.ClearSelection();
            if (coordenada.PosicionLat == EnumCoordinatesLat.Norte)
                ddlLatitud.Items.FindByValue("0").Selected = true;// coordenada.PosicionLat.ToString() == "North" ? "0" : "1";  //coordenada.PosicionLat.ToString();
            if (coordenada.PosicionLat == EnumCoordinatesLat.Sur)
                ddlLatitud.Items.FindByValue("1").Selected = true;// coordenada.

            txtPtoLongitudGrados.Text = coordenada.GradosLng.ToString();
            txtPtoLongitudMinutos.Text = coordenada.MinutosLng.ToString();
            txtPtoLongitudSeg.Text = Math.Round(coordenada.SegundosLng).ToString("#.00");

            ddlLongitud.ClearSelection();
            if (coordenada.PosicionLng == EnumCoordinatesLng.Este)
                ddlLongitud.Items.FindByValue("0").Selected = true;// coordenada.PosicionLat.ToString() == "North" ? "0" : "1";  //coordenada.PosicionLat.ToString();
            if (coordenada.PosicionLng == EnumCoordinatesLng.Oeste)
                ddlLongitud.Items.FindByValue("1").Selected = true;// coordenada.    

            //ddlLongitud.SelectedValue = coordenada.PosicionLng.ToString() == "East" ? "0" : "1";

            txtPtoCuerpoR.Text = anexo.Anexo.NombreCuerpoReceptor;
            txtPtoFuenteA.Text = anexo.Anexo.FuenteAbastecimiento;
            txtPtoOrigenD.Text = anexo.Anexo.OrigenDescarga;
        }

        public void ActualizarPanelPto()
        {
            updpnlPtoDescarga.Update();
        }

        #endregion

        #region Eventos

        protected void lnkbtnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            ScriptManager.RegisterStartupScript(lnkbtnCancelar, lnkbtnCancelar.GetType(), "moveScript", "movePosition(1);", true);
            GuardarAnexoArgs(this, null);
        }

        protected void lnkbtnPtoGuardar_Click(object sender, EventArgs e)
        {
            bool Continua = false;
            Consecion_BL consecion_bl = new Consecion_BL();
            ConsecionCapturaArgs consecion = new ConsecionCapturaArgs();
            AnexoCapturaArgs anexo = new AnexoCapturaArgs();

            consecion.ClienteID = this.Cliente.ClienteID;
            consecion.Titulo = txtPtoTitulo.Text;

            consecion_bl.Obten(consecion.Titulo, this.Cliente.ClienteID);
            this.ConsecionID = consecion_bl.Consecion != null ? consecion_bl.Consecion.ConsecionID : 0;

            if (this.Transaccion == "Nuevo")
            {
                //El titulo ya esxiste para el o para otro cliente
                if (consecion_bl.Consecion != null)
                {
                    Continua = false;
                    anexo.Transaccion = "TituloExistente";
                    GuardarAnexoArgs(this, anexo);
                    ScriptManager.RegisterStartupScript(lnkbtnCancelar, lnkbtnCancelar.GetType(), "moveScript", "movePosition(1);", true);
                }
                else
                    Continua = true;
            }
            if (this.Transaccion == "Actualizacion")
            {
                Continua = true;
            }

            if (Continua)
            {
                if (this.ConsecionID != 0)
                {
                    consecion.ConsecionID = this.ConsecionID;
                }
                consecion_bl.Guarda(consecion);
                this.ConsecionID = consecion_bl.Consecion.ConsecionID;

                if (consecion_bl.Consecion != null)
                {
                    Coordenadas_BL coordenada = new Coordenadas_BL(
                        decimal.Parse(txtPtoLatitudGrados.Text),
                        decimal.Parse(txtPtoLatitudMinutos.Text),
                        decimal.Parse(txtPtoLatitudSeg.Text),
                        (EnumCoordinatesLat)Enum.Parse(typeof(EnumCoordinatesLat), ddlLatitud.SelectedItem.Value),
                        decimal.Parse(txtPtoLongitudGrados.Text),
                        decimal.Parse(txtPtoLongitudMinutos.Text),
                        decimal.Parse(txtPtoLongitudSeg.Text),
                        (EnumCoordinatesLng)Enum.Parse(typeof(EnumCoordinatesLng), ddlLongitud.SelectedValue));

                    if (this.Anexo != null)
                        anexo.AnexoID = this.Anexo.AnexoID;
                    else
                        anexo.AnexoID = 0;
                    anexo.ConsecionID = this.ConsecionID != 0 ? this.ConsecionID : 0;
                    anexo.Anexo = txtPtoAnexo.Text;
                    anexo.Latitud = coordenada.Latitude;
                    anexo.Longitud = coordenada.Longitude;
                    anexo.Datum = txtPtoDatum.Text;

                    anexo.CuerpoReceptorID =int.Parse(ddlCuerpoReceptor.SelectedValue);
                    anexo.UsoAguaID = int.Parse(ddlUsoAgua.SelectedValue);
                    anexo.NumeroMedidor = txtNumeroMedidor.Text;

                    anexo.NombreCuerpoReceptor = txtPtoCuerpoR.Text;
                    anexo.FuenteAbastecimiento = txtPtoFuenteA.Text;
                    anexo.OrigenDescarga = txtPtoOrigenD.Text;
                    anexo.Efluente = bool.Parse(rblPtoEfluente.SelectedValue);

                    DireccionCapturaArgs direccion = new DireccionCapturaArgs();
                    if (this.Anexo != null)
                        direccion.DireccionID = this.Anexo.Direccion.DireccionID;
                    else
                        direccion.DireccionID = 0;
                    direccion.Calle = txtPtoCalle.Text;
                    direccion.NumeroExteior = txtPtoNExterior.Text;
                    direccion.NumeroInterior = txtPtoNInterior.Text;
                    direccion.EstadoID = int.Parse(ddlPtoEstado.SelectedValue);
                    direccion.MunicipioID = int.Parse(ddlPtoMunicipio.SelectedValue);
                    direccion.LocalidadID = int.Parse(ddlPtoLocalidad.SelectedValue);
                    direccion.ColoniaID = int.Parse(ddlPtoColonia.SelectedValue);
                    direccion.CP = int.Parse(txtPtoCP.Text);

                    anexo.dir = direccion;
                    Anexo_BL anexo_bl = new Anexo_BL();
                    anexo_bl.Guarda(anexo);
                    anexo.AnexoID = anexo_bl.Anexo.AnexoID;
                    anexo.PukAnexoID = anexo_bl.Anexo.PukAnexoID;
                    GuardarAnexoArgs(this, anexo);
                    Limpiar();
                    this.ConsecionID = 0;
                    this.Transaccion = "";
                    ScriptManager.RegisterStartupScript(lnkbtnCancelar, lnkbtnCancelar.GetType(), "moveScript", "movePosition(1);", true);
                }
            }
        }

        protected void ddlPtoEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDrops.instance.Municipio_Load(ddlPtoMunicipio, int.Parse(ddlPtoEstado.SelectedValue));
        }

        protected void ddlPtoMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDrops.instance.Localidad(ddlPtoLocalidad, int.Parse(ddlPtoEstado.SelectedValue), int.Parse(ddlPtoMunicipio.SelectedValue));
            LoadDrops.instance.Colonia(ddlPtoColonia, int.Parse(ddlPtoEstado.SelectedValue), int.Parse(ddlPtoMunicipio.SelectedValue));
        }

        #endregion
    }
}