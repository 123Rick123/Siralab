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
    public partial class wucDatosMuestra : System.Web.UI.UserControl
    {
        #region Propiedades
        public int OrdenTrabajoID
        {
            get { return (int)Session["OrdenTrabajoID_"] == 0 ? 0 : (int)Session["OrdenTrabajoID_"]; }
            set { Session["OrdenTrabajoID_"] = value; }
        }
        public SIRALAB_DAL.Anexo Anexo
        {
            get { return (SIRALAB_DAL.Anexo)Session["AnexoConsulta_"]; }
        }
        public int MuestraID
        {
            get { return int.Parse(hfPtoMuestraID.Value == "" ? "0" : hfPtoMuestraID.Value); }
            set { hfPtoMuestraID.Value = value.ToString(); }
        }
        public Laboratorio LaboratorioDatos
        {
            get { return (Laboratorio)Session["LaboratorioDatos_"]; }
        }
        public Persona LaboratorioSiganatario
        {
            get { return (Persona)Session["LaboratorioSignatario_"]; }
        }

        public SIRALAB_DAL.Analisi Analisis
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }

        #endregion

        [Browsable(true)]
        public event EventHandler<MuestraCapturaArgs> Guardar;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var ordentrabajoBl = new OrdenTrabajo_BL();
                var orden = ordentrabajoBl.Obten(OrdenTrabajoID);
                switch (orden.Norma.NormaID)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        chPtoDescarga.Text = " Domicilio del punto de descarga es el mismo que el punto de muestreo:";
                        break;
                    case 4:
                    case 12:
                    case 13:
                        chPtoDescarga.Text = " Domicilio del aprovechamiento es el mismo que el punto de muestreo: ";
                        break;
                }
                var anexo = orden.Anexo;
                var analisis = orden.Analisis.Where(x => x.AnalisisID == this.Analisis.AnalisisID).First();
                if (analisis.Muestras.Count > 0)
                {
                    bool esDireccion = anexo.Direccion.CP == analisis.Muestras.First().Direccion.CP &&
                                      anexo.Direccion.Calle == analisis.Muestras.First().Direccion.Calle &&
                                      anexo.Direccion.MunicipioID == analisis.Muestras.First().Direccion.MunicipioID &&
                                      anexo.Direccion.LocalidadID == analisis.Muestras.First().Direccion.LocalidadID &&
                                      anexo.Direccion.EstadoID == analisis.Muestras.First().Direccion.EstadoID &&
                                      anexo.Direccion.ColoniaID == analisis.Muestras.First().Direccion.ColoniaID &&
                                      anexo.Latitud == analisis.Muestras.First().Latitud &&
                                       anexo.Longitud == analisis.Muestras.First().Longitud;
                    if (esDireccion)
                    {
                        chPtoDescarga.Checked = esDireccion;
                    }

                }
                
                //CargaInfo();
            }
        }

        #region Metodos
       
        public void CargaDrops()
        {
            ListItem li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
            LoadDrops.instance.Estado_Load(ddlPtoEstado);
            ddlPtoMunicipio.Items.Insert(0, li);
            ddlPtoLocalidad.Items.Insert(0, li);
            ddlPtoColonia.Items.Insert(0, li);
        }

        public void Limpiar()
        {
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

            txtPtoCP.Text = "";

            ddlPtoEstado.ClearSelection();
            ddlPtoMunicipio.ClearSelection();
            ddlPtoLocalidad.ClearSelection();
            ddlPtoColonia.ClearSelection();
            ddlLongitud.ClearSelection();
            ddlLatitud.ClearSelection();
            if(rblPtoEfluente.Enabled&& rblPtoEfluente.Visible)
            {
                rblPtoEfluente.SelectedValue = "0";
            }
           
        }

        public void CargaAnalisis(int MuestraID)
        {
            pnlPtoDescarga.Enabled = false;
            lnkbtnPtoGuardar.Visible = false;
            CargaMuestra(MuestraID);
        }

        public void CargaMuestra(int MuestraID)
        {
            //Anexo_BL anexo = new Anexo_BL();
            //anexo.Obten(AnexoID);

            ScriptManager.RegisterStartupScript(lnkbtnClienteConsulta, lnkbtnClienteConsulta.GetType(), "moveScript", "t(1);", true);
            Muestra_BL muestra_bl = new Muestra_BL();
            var muestra = muestra_bl.Obten_Muestra(MuestraID);


            txtPtoCalle.Text = muestra.Direccion.Calle;
            txtPtoCP.Text = muestra.Direccion.CP.ToString();

            txtPtoNExterior.Text = muestra.Direccion.NumeroExterior;
            if (muestra.Direccion.NumeroInterior != null)
                txtPtoNInterior.Text = muestra.Direccion.NumeroInterior;

            LoadDrops.instance.Estado_Load(ddlPtoEstado);

            ddlPtoEstado.SelectedValue = muestra.Direccion.EstadoID.ToString();
            LoadDrops.instance.Municipio_Load(ddlPtoMunicipio, muestra.Direccion.EstadoID);

            ddlPtoMunicipio.SelectedValue = muestra.Direccion.MunicipioID.ToString();
            LoadDrops.instance.Localidad(ddlPtoLocalidad, muestra.Direccion.EstadoID, muestra.Direccion.MunicipioID);
            LoadDrops.instance.Colonia(ddlPtoColonia, muestra.Direccion.EstadoID, muestra.Direccion.MunicipioID);
            ddlPtoLocalidad.SelectedValue = muestra.Direccion.LocalidadID.ToString();
            ddlPtoColonia.SelectedValue = muestra.Direccion.ColoniaID.ToString();

            switch (muestra.Datum)
            {
                case "1":
                case "2":
                case "3":
                    DDDatum.SelectedValue = muestra.Datum;
                    break;
                default:
                    txtPtoDatum.Text = muestra.Datum;
                    break;
            }

            rblPtoEfluente.SelectedValue = this.Anexo.Efluente == true ? "true" : "false";

            Coordenadas_BL coordenada = new Coordenadas_BL(muestra.Latitud, muestra.Longitud);
            txtPtoLatitudGrados.Text = coordenada.GradosLat.ToString();
            txtPtoLatitudMinutos.Text = coordenada.MinutosLat.ToString();
            txtPtoLatitudSeg.Text = coordenada.SegundosLat.ToString();

            ddlLongitud.ClearSelection();
            ddlLongitud.Items.FindByValue(coordenada.PosicionLng == EnumCoordinatesLng.Este ? "0" : "1").Selected = true;

            txtPtoLongitudGrados.Text = coordenada.GradosLng.ToString();
            txtPtoLongitudMinutos.Text = coordenada.MinutosLng.ToString();
            txtPtoLongitudSeg.Text = coordenada.SegundosLng.ToString();

            Analisis_BL analisis_bl = new Analisis_BL();
            var an = analisis_bl.Obten_Analisis(this.Analisis.AnalisisID);

            txtObservaciones.Text = an.Observaciones;

            ddlLatitud.ClearSelection();
            ddlLatitud.Items.FindByValue(coordenada.PosicionLat == EnumCoordinatesLat.Norte ? "0" : "1").Selected = true;
        }

        protected void CargaAnexo(int AnexoID)
        {
            Anexo_BL anexo = new Anexo_BL();
            anexo.Obten(AnexoID);

            txtPtoCalle.Text = anexo.Anexo.Direccion.Calle;
            txtPtoCP.Text = anexo.Anexo.Direccion.CP.ToString();
            if (anexo.Anexo.Direccion.NumeroExterior != null)
            txtPtoNExterior.Text = anexo.Anexo.Direccion.NumeroExterior;
            if (anexo.Anexo.Direccion.NumeroInterior != null)
                txtPtoNInterior.Text = anexo.Anexo.Direccion.NumeroInterior;
            LoadDrops.instance.Estado_Load(ddlPtoEstado);

            ddlPtoEstado.SelectedValue = anexo.Anexo.Direccion.EstadoID.ToString();

            LoadDrops.instance.Municipio_Load(ddlPtoMunicipio, anexo.Anexo.Direccion.EstadoID);
            ddlPtoMunicipio.SelectedValue = anexo.Anexo.Direccion.MunicipioID.ToString();
            LoadDrops.instance.Localidad(ddlPtoLocalidad, anexo.Anexo.Direccion.EstadoID, anexo.Anexo.Direccion.MunicipioID);
            LoadDrops.instance.Colonia(ddlPtoColonia, anexo.Anexo.Direccion.EstadoID, anexo.Anexo.Direccion.MunicipioID);
            ddlPtoLocalidad.SelectedValue = anexo.Anexo.Direccion.LocalidadID.ToString();
            ddlPtoColonia.SelectedValue = anexo.Anexo.Direccion.ColoniaID.ToString();
            switch (anexo.Anexo.Datum)
            {
                case "1":
                case "2":
                case "3":
                    DDDatum.SelectedValue = anexo.Anexo.Datum;
                    break;
                default:
                    txtPtoDatum.Text = anexo.Anexo.Datum;
                    break;
            }

            rblPtoEfluente.SelectedValue = anexo.Anexo.Efluente == true ? "true" : "false";// bool.Parse(anexo.Anexo.Efluente.ToString());//.ToString();

            Coordenadas_BL coordenada = new Coordenadas_BL(anexo.Anexo.Latitud, anexo.Anexo.Longitud);
            txtPtoLatitudGrados.Text = coordenada.GradosLat.ToString();
            txtPtoLatitudMinutos.Text = coordenada.MinutosLat.ToString();
            txtPtoLatitudSeg.Text = coordenada.SegundosLat.ToString("#.00");

            ddlLatitud.ClearSelection();
            if (coordenada.PosicionLat == EnumCoordinatesLat.Norte)
                ddlLatitud.Items.FindByValue("0").Selected = true;// coordenada.PosicionLat.ToString() == "North" ? "0" : "1";  //coordenada.PosicionLat.ToString();
            if (coordenada.PosicionLat == EnumCoordinatesLat.Sur)
                ddlLatitud.Items.FindByValue("1").Selected = true;// coordenada.

            txtPtoLongitudGrados.Text = coordenada.GradosLng.ToString();
            txtPtoLongitudMinutos.Text = coordenada.MinutosLng.ToString();
            txtPtoLongitudSeg.Text = coordenada.SegundosLng.ToString("#.00");

            Analisis_BL analisis_bl = new Analisis_BL();
            var an = analisis_bl.Obten_Analisis(this.Analisis.AnalisisID);

            txtObservaciones.Text = an.Observaciones;

            ddlLongitud.ClearSelection();
            ddlLongitud.Items.FindByValue(coordenada.PosicionLng == EnumCoordinatesLng.Este ? "0" : "1").Selected = true;// coordenada.PosicionLat.ToString() == "North" ? "0" : "1";  //coordenada.PosicionLat.ToString();
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
            Guardar(this, null);
        }

        protected void lnkbtnPtoGuardar_Click(object sender, EventArgs e)
        {
            var muestraBl = new Muestra_BL();
            var muestraArgs = new MuestraCapturaArgs();

            if (DDDatum.SelectedValue == "0" || txtPtoNExterior.Text == "" || txtPtoCalle.Text == "" || txtPtoCP.Text=="")
            {
                muestraArgs.Descripcion = "DatosFaltantes";
                Guardar(this, muestraArgs);
                return;
            }
            if (Analisis != null)
            {
                //SE REALIZA UN CICLO CON EL NUMERO DE MUESTRAS CONFIGURADO EN LA ORDEN DE TRABAJO

                for (int a = 0; a <= (int)Analisis.OrdenTrabajo.FrecuenciaMuestreo.MuestrasSimples - 1; a++)
                {
                    muestraArgs.AnalisisID = this.Analisis.AnalisisID;

                    //Se llena el objeto de dirección

                    var direccion = new DireccionCapturaArgs();
                    //if (this.Anexo != null)
                    //    direccion.DireccionID = this.Anexo.Direccion.DireccionID;
                    //else
                    direccion.DireccionID = 0;
                    direccion.Calle = txtPtoCalle.Text;
                    direccion.NumeroExteior = txtPtoNExterior.Text;
                    direccion.NumeroInterior = txtPtoNInterior.Text;
                    direccion.EstadoID = int.Parse(ddlPtoEstado.SelectedValue);
                    direccion.MunicipioID = int.Parse(ddlPtoMunicipio.SelectedValue);
                    direccion.LocalidadID = int.Parse(ddlPtoLocalidad.SelectedValue);
                    direccion.ColoniaID = int.Parse(ddlPtoColonia.SelectedValue);
                    direccion.CP = int.Parse(txtPtoCP.Text);

                    muestraArgs.dir = direccion;
                    //se termina de llenar el objeto dirección

                    muestraArgs.Numero = this.Analisis.OrdenTrabajo.NumeroInforme;
                    muestraArgs.Descripcion = "";
                    muestraArgs.Identificacion = "";
                    muestraArgs.Caudal = 0;

                    ////////if(t)
                    ////////muestraArgs.NivelID

                    var coordenada = new Coordenadas_BL(
                        decimal.Parse(txtPtoLatitudGrados.Text),
                        decimal.Parse(txtPtoLatitudMinutos.Text),
                        decimal.Parse(txtPtoLatitudSeg.Text),
                        (EnumCoordinatesLat)Enum.Parse(typeof(EnumCoordinatesLat), ddlLatitud.SelectedItem.Value),
                        decimal.Parse(txtPtoLongitudGrados.Text),
                        decimal.Parse(txtPtoLongitudMinutos.Text),
                        decimal.Parse(txtPtoLongitudSeg.Text),
                        (EnumCoordinatesLng)Enum.Parse(typeof(EnumCoordinatesLng), ddlLongitud.SelectedValue));

                    muestraArgs.Latitud = coordenada.Latitude;
                    muestraArgs.Longitud = coordenada.Longitude;
                    muestraArgs.Datum = DDDatum.SelectedValue;

                    muestraArgs.Fecha = null;

                    //if (a == (int)this.Analisis.OrdenTrabajo.FrecuenciaMuestreo.MuestrasSimples)
                    //    muestraArgs.EsCompuesta = true;
                    //else
                    muestraArgs.EsCompuesta = false;
                    muestraArgs.NivelID = 1;
                    muestraArgs.FechaRecepcion = null;
                    muestraArgs.Cancelado = false;
                    muestraArgs.Observaciones = "";

                    muestraBl.Guarda(muestraArgs);


                    var analisisBl = new Analisis_BL();
                    analisisBl.ActualizaObservaciones(Analisis.AnalisisID, txtObservaciones.Text);
                }

                pnlPtoDescarga.Enabled = false;
                lnkbtnPtoGuardar.Visible = false;
                Response.Redirect(Request.Url.AbsoluteUri);
                Guardar(this, muestraArgs);
                //FIN DEL CICLO DEL NUMERO DE MUESTARS CONFIGURADO EN LA ORDEN DE TRABAJO
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

        protected void chPtoDescarga_CheckedChanged(object sender, EventArgs e)
        {
            if (chPtoDescarga.Checked)
            {
                if (this.Anexo != null)
                    CargaAnexo(this.Anexo.AnexoID);
            }
            else
                Limpiar();
        }
    }
}