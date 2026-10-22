using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_SIRFIC;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SIRALAB.Controles
{
    public partial class wucConsultaCliente : System.Web.UI.UserControl
    {
        public SIRALAB_DAL.Cliente Cliente
        {
            get { return (SIRALAB_DAL.Cliente)Session["ClienteConsulta_"]; }
            set { Session["ClienteConsulta_"] = value; }
        }

        public int ClienteAutoID
        {
            get { return (int)Session["ClienteIDAutocomplete"]; }
            set { Session["ClienteIDAutocomplete"] = value; }
        }
        public bool InscritoNom001
        {
            get { return (bool)Session["InscritoNom001"]; }
            set { Session["InscritoNom001"] = value; }
        }

        [Browsable(true)]
        public event EventHandler<Cliente> Seleccion;
        [Browsable(true)]
        public event EventHandler<ClienteErrorArg> Error;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkbtnConsultaCliente_Click(object sender, EventArgs e)
        {
            /* Modificación para obtener de SIRFIC el Cliente */

            var pdSirfic = new ProcesamientoDatos();
            var cl = new ClienteCapturaArgs();
            var dir = new DireccionCapturaArgs();
            var clbl = new Cliente_BL();
            var sirficContext = new CNA_SIRFICEntities();
            try
            {
                if (txtClienteRFC.Text.Trim() != "")
                {
                    lnkbtnClienteConsulta.Visible = true;
                    // var contribuyente = pdSirfic.BuscaContribuyente(txtClienteRFC.Text);
                    var contrib = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                                   where clie.rfc == txtClienteRFC.Text.Trim()
                                   select new { clie.ID_CONTRIBUYENTE, clie.nombre, clie.apellidoPaterno, clie.apellidoMaterno, clie.rfc, clie.curp, clie.email }).Distinct().ToList();
                    if (contrib.Count == 0)
                    {
                        Error(this, new ClienteErrorArg { Mensaje = "Para poder registrar y enviar a la Conagua, los resultados de los análisis de laboratorio, es necesario que le informe al contribuyente, que ingrese al Sistema de Declaraciones y Pago Electrónico \"Declar@gua\" (http://201.116.60.171/DECLARAGUA/) y registre o actualice sus datos en el Padrón Único de Usuarios y Contribuyentes, pues de lo contrario el sistema no permitirá la captura y envío de la información." });

                        return;
                    }
                    cl.ClienteID = null;
                    dir.DireccionID = null;
                    clbl.obtener(txtClienteRFC.Text);
                    foreach (var t in contrib)
                    {
                        cl.ClienteID = (int)t.ID_CONTRIBUYENTE;
                        if (clbl.Cliente != null)
                        {
                            if (t.nombre != clbl.Cliente.Nombre || t.apellidoPaterno != clbl.Cliente.ApellidoPaterno || t.apellidoMaterno != clbl.Cliente.ApellidoMaterno || t.rfc != clbl.Cliente.RFC || t.curp != clbl.Cliente.CURP || t.email != clbl.Cliente.Correo)
                            {
                                cl.ClienteID = clbl.Cliente.ClienteID;
                                dir.DireccionID = clbl.Cliente.DireccionID;

                                cl.Nombre = t.nombre;
                                cl.ApellidoPaterno = t.apellidoPaterno;
                                cl.ApellidoMaterno = t.apellidoMaterno;
                                cl.RFC = t.rfc;
                                cl.CURP = t.curp;
                                cl.Correo = t.email;
                                var direc = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                                             where clie.rfc == txtClienteRFC.Text.Trim()
                                             select new { clie.vialidad, clie.numeroExterior1, clie.numeroInterior, clie.CLAVE_ESTADO, clie.CLAVE_MUNICIPIO, clie.CLAVE_LOCALIDAD, clie.ID_ASENTAMIENTO_HUMANO, clie.codigoPostal }).Distinct().ToList();
                                foreach (var td in direc)
                                {
                                    dir.Calle = td.vialidad;
                                    dir.NumeroExteior = td.numeroExterior1;
                                    dir.NumeroInterior = td.numeroInterior;
                                    dir.CP = td.codigoPostal;
                                    dir.EstadoID = (int)td.CLAVE_ESTADO;
                                    dir.MunicipioID = (int)td.CLAVE_MUNICIPIO;
                                    dir.LocalidadID = td.CLAVE_LOCALIDAD;
                                    dir.ColoniaID = (int)td.ID_ASENTAMIENTO_HUMANO;
                                }
                                cl.dir = dir;
                                clbl.Guarda(cl);
                                //    {
                                //        if (consecionBl.Consecion.ClienteID != cl.ClienteID || consecionBl.Consecion.PukConsecionID != titulo.TituloID)
                                //        {
                                //            consecion.ConsecionID = consecionBl.Consecion.ConsecionID;
                                //            consecion.ClienteID = (int)contribuyente.ClienteID;
                                //            consecion.Titulo = titulo.Titulo1;
                                //            consecion.PukConsecionID = titulo.TituloID;
                                //            consecionBl.GuardaTitulos(consecion);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        consecion.ClienteID = (int)contribuyente.ClienteID;
                                //            consecion.Titulo = titulo.Titulo1;
                                //            consecion.PukConsecionID = titulo.TituloID;
                                //        consecionBl.GuardaTitulos(consecion);
                                //    }
                                //    if (consecionBl.Consecion==null)
                                //    {
                                //        consecionBl.Obten((int)titulo.TituloID, (int)cl.ClienteID);
                                //    }

                                //        //    foreach (var Anexo in titulo.Anexos)
                                //        //    {
                                //        //var anexoBl = new Anexo_BL();
                                //        //var anexoArg = new AnexoCapturaArgs();
                                //        //var direccionAnexo = new DireccionCapturaArgs();

                                //        //var latitud = Anexo.Latitud.Split('|');
                                //        //var longitud = Anexo.Longitud.Split('|');

                                //        //var coordenada = new Coordenadas_BL(
                                //        //   decimal.Parse(latitud[0]),
                                //        //   decimal.Parse(latitud[1]),
                                //        //   decimal.Parse(latitud[2]), EnumCoordinatesLat.Norte,
                                //        //   decimal.Parse(longitud[0]),
                                //        //   decimal.Parse(longitud[1]),
                                //        //   decimal.Parse(longitud[2]), EnumCoordinatesLng.Oeste);

                                //        //        #region Consulta de la existencia del anexo
                                //        //        SIRALAB_DAL.Anexo exiteAnexo = null;

                                //        //var exiteAnexo1 = consecionBl.Consecion.Anexoes.FirstOrDefault(p => p.Anexo1 == Anexo.Anexo1 && p.ConsecionID == consecionBl.Consecion.ConsecionID);

                                //        //var exiteAnexo2 = consecionBl.Consecion.Anexoes.FirstOrDefault(p => p.PukAnexoID == Anexo.AnexoID && p.ConsecionID == consecionBl.Consecion.ConsecionID);

                                //        //if (exiteAnexo1 != null && exiteAnexo2 != null)
                                //        //        {
                                //        //    exiteAnexo = exiteAnexo2;
                                //        //        }
                                //        //else if (exiteAnexo1 == null && exiteAnexo2 != null)
                                //        //        {
                                //        //    exiteAnexo = exiteAnexo2;
                                //        //        }
                                //        //else if (exiteAnexo1 != null && exiteAnexo2 == null)
                                //        //        {
                                //        //    exiteAnexo = exiteAnexo1;
                                //        //        }
                                //        //        #endregion

                                //        //        if (exiteAnexo != null)
                                //        //        {
                                //        //    if (exiteAnexo.Anexo1 != Anexo.Anexo1 || exiteAnexo.PukAnexoID != Anexo.AprovechamientoID /*|| exiteAnexo.OrigenDescarga != Anexo.Aprovechamiento*/)
                                //        //    {
                                //        //            anexoArg.AnexoID = exiteAnexo.AnexoID;
                                //        //            direccionAnexo.DireccionID = exiteAnexo.DireccionID;
                                //        //        direccionAnexo.Calle = Anexo.Domicilio.Calle;
                                //        //        direccionAnexo.NumeroExteior = Anexo.Domicilio.NumeroExterior;
                                //        //        direccionAnexo.NumeroInterior = Anexo.Domicilio.NumeroInterior;
                                //        //        direccionAnexo.EstadoID = Anexo.Domicilio.EstadoID;
                                //        //        direccionAnexo.MunicipioID = Anexo.Domicilio.MunicipioID;
                                //        //        direccionAnexo.LocalidadID = Anexo.Domicilio.LocalidadID;
                                //        //        direccionAnexo.ColoniaID = Anexo.Domicilio.AsentamientoID;
                                //        //        direccionAnexo.CP = Anexo.Domicilio.CodigoPostal;

                                //        //        anexoArg.ConsecionID = consecionBl.Consecion.ConsecionID;
                                //        //        anexoArg.Anexo = Anexo.Anexo1;
                                //        //        anexoArg.Latitud = coordenada.Latitude;
                                //        //        anexoArg.Longitud = coordenada.Longitude;
                                //        //        anexoArg.Datum = "";
                                //        //        anexoArg.CuerpoReceptorID = 1; /* Son campos que deberán captura */
                                //        //        anexoArg.UsoAguaID = 1; /* Son campos que deberán captura */
                                //        //        anexoArg.NumeroMedidor = Anexo.SerieMedidor;
                                //        //        anexoArg.NombreCuerpoReceptor = Anexo.CuerpoReceptor;
                                //        //        anexoArg.FuenteAbastecimiento = "";
                                //        //        anexoArg.OrigenDescarga = Anexo.Aprovechamiento;
                                //        //        anexoArg.Efluente = false;
                                //        //        anexoArg.PukAnexoID = Anexo.AprovechamientoID;

                                //        //        anexoArg.dir = direccionAnexo;
                                //        //        anexoBl.Guarda(anexoArg);
                                //        //    }
                                //        //}
                                //        //else
                                //        //{
                                //        //    anexoArg.AnexoID = 0;
                                //        //    direccionAnexo.DireccionID = null;
                                //        //    direccionAnexo.Calle = Anexo.Domicilio.Calle;
                                //        //    direccionAnexo.NumeroExteior = Anexo.Domicilio.NumeroExterior;
                                //        //    direccionAnexo.NumeroInterior = Anexo.Domicilio.NumeroInterior;
                                //        //    direccionAnexo.EstadoID = Anexo.Domicilio.EstadoID;
                                //        //    direccionAnexo.MunicipioID = Anexo.Domicilio.MunicipioID;
                                //        //    direccionAnexo.LocalidadID = Anexo.Domicilio.LocalidadID;
                                //        //    direccionAnexo.ColoniaID = Anexo.Domicilio.AsentamientoID;
                                //        //    direccionAnexo.CP = Anexo.Domicilio.CodigoPostal;
                                //        //    anexoArg.ConsecionID = consecionBl.Consecion.ConsecionID;
                                //        //    anexoArg.Anexo = Anexo.Anexo1;
                                //        //    anexoArg.Latitud = coordenada.Latitude;
                                //        //    anexoArg.Longitud = coordenada.Longitude;
                                //        //    anexoArg.Datum = "";
                                //        //    anexoArg.CuerpoReceptorID = 1;
                                //        //    anexoArg.UsoAguaID = 1;
                                //        //    anexoArg.NumeroMedidor = Anexo.SerieMedidor;
                                //        //    anexoArg.NombreCuerpoReceptor = Anexo.CuerpoReceptor;
                                //        //    anexoArg.FuenteAbastecimiento = "";
                                //        //    anexoArg.OrigenDescarga = Anexo.Aprovechamiento;
                                //        //    anexoArg.Efluente = false;
                                //        //    anexoArg.PukAnexoID = Anexo.AprovechamientoID;

                                //        //    anexoArg.dir = direccionAnexo;
                                //        //    anexoBl.Guarda(anexoArg);
                                //        //}

                                //        //foreach (var medidor in Anexo.AnexoMedidores)
                                //        //            {                                    
                                //        //    var anexoMedidorBl = new AnexoMedidor_BL();
                                //        //    var anexoMedidorArg = new AnexoMedidorArgs();
                                //        //    int anexid;
                                //        //    if (exiteAnexo == null)
                                //        //        anexid = int.Parse(anexoBl.Anexo.AnexoID.ToString());
                                //        //    else
                                //        //        anexid = exiteAnexo.AnexoID != null ? int.Parse(exiteAnexo.AnexoID.ToString()) : 0;

                                //        //    anexoMedidorBl.Obten(anexid, long.Parse(medidor.ID_SERIE_MEDIDOR.ToString()));
                                //        //    if (anexoMedidorBl.AnexoMedidor != null)
                                //        //                {
                                //        //        if (anexoMedidorBl.AnexoMedidor.MARCA_MEDIDOR != medidor.MARCA_MEDIDOR || anexoMedidorBl.AnexoMedidor.ID_MARCA_MEDIDOR != medidor.ID_MARCA_MEDIDOR || anexoMedidorBl.AnexoMedidor.ID_SERIE_MEDIDOR != medidor.ID_SERIE_MEDIDOR || anexoMedidorBl.AnexoMedidor.numeroMedidor != medidor.numeroMedidor)
                                //        //        {
                                //        //            anexoMedidorArg.AnexoMedidorID = anexoMedidorBl.AnexoMedidor.AnexoMedidorID;
                                //        //            anexoMedidorArg.AnexoID = anexid;
                                //        //            anexoMedidorArg.ID_SERIE_MEDIDOR = medidor.ID_SERIE_MEDIDOR;
                                //        //            anexoMedidorArg.numeroMedidor = medidor.numeroMedidor;
                                //        //            anexoMedidorArg.ID_MARCA_MEDIDOR = medidor.ID_MARCA_MEDIDOR;
                                //        //            anexoMedidorArg.MARCA_MEDIDOR = medidor.MARCA_MEDIDOR;
                                //        //            anexoMedidorArg.FuenteAbastecimiento = (medidor.FuenteAbastecimiento == 1) ? true : false;
                                //        //            anexoMedidorBl.Guarda(anexoMedidorArg);
                                //        //                }

                                //        //    }
                                //        //    else
                                //        //    {
                                //        //        anexoMedidorArg.AnexoID = anexid;
                                //        //                anexoMedidorArg.ID_SERIE_MEDIDOR = medidor.ID_SERIE_MEDIDOR;
                            }
                        }
                        else
                        {
                            cl.Nombre = t.nombre;
                            cl.ApellidoPaterno = t.apellidoPaterno;
                            cl.ApellidoMaterno = t.apellidoMaterno;
                            cl.RFC = t.rfc;
                            cl.CURP = t.curp;
                            cl.Correo = t.email;
                            var direc = (from clie in sirficContext.SIRFIC_SIRALAB_ANCR
                                         where clie.rfc == txtClienteRFC.Text.Trim()
                                         select new { clie.vialidad, clie.numeroExterior1, clie.numeroInterior, clie.CLAVE_ESTADO, clie.CLAVE_MUNICIPIO, clie.CLAVE_LOCALIDAD, clie.ID_ASENTAMIENTO_HUMANO, clie.codigoPostal }).Distinct().ToList();
                            foreach (var td in direc)
                            {
                                dir.Calle = td.vialidad;
                                dir.NumeroExteior = td.numeroExterior1;
                                dir.NumeroInterior = td.numeroInterior;
                                dir.CP = td.codigoPostal;
                                dir.EstadoID = (int)td.CLAVE_ESTADO;
                                dir.MunicipioID = (int)td.CLAVE_MUNICIPIO;
                                dir.LocalidadID = td.CLAVE_LOCALIDAD;
                                dir.ColoniaID = (int)td.ID_ASENTAMIENTO_HUMANO;
                            }
                            cl.dir = dir;
                            clbl.Guarda(cl);
                        }
                    }
                    Cliente = clbl.Cliente;
                    CargaInformacionCliente();
                    ObtenCliente();
                    ScriptManager.RegisterStartupScript(lnkbtnClienteConsulta, lnkbtnClienteConsulta.GetType(), "moveScript", "t(1);", true);
                }
            }
            catch (Exception ex)
            {
                Error(this, new ClienteErrorArg { Mensaje = ex.Message + " \\n" + ex + "\\n" });
            }
        }

        public void ObtenCliente()
        {
            lnkbtnClienteConsulta.Visible = true;
            Cliente_BL cliente_bl = new Cliente_BL();

            if (txtClienteRFC.Text != "")
            {

                ScriptManager.RegisterStartupScript(lnkbtnClienteConsulta, lnkbtnClienteConsulta.GetType(), "moveScript", "t(1);", true);
                Seleccion(this, this.Cliente);
            }
            else
                Seleccion(this, null);
        }

        public void CargaClientes()
        {
            lnkbtnConsultaCliente.Visible = true;
            //LoadDrops.instance.Clientes(ddlCliente);
        }

        public void InabilitaPanel()
        {
            pnlDatosCliente.Enabled = false;
        }

        public void CargaInformacionCliente()
        {
            //Llenamos la oinformación del cliente con la información guardada.
            Direccion_BL direccion_bl = new Direccion_BL();

            direccion_bl.obtener(this.Cliente.DireccionID);
            var direccion = direccion_bl.Direccion;

            lblClienteCurp.Text = this.Cliente.CURP;
            if (this.Cliente.ApellidoPaterno != null)
                lblClienteNombre.Text = this.Cliente.Nombre + " " + this.Cliente.ApellidoPaterno + " " + this.Cliente.ApellidoMaterno;
            else
                lblClienteNombre.Text = this.Cliente.Nombre;

            lblClienteCalle.Text = direccion.Calle;
            lblClienteNE.Text = direccion.NumeroExterior;
            lblClienteNI.Text = direccion.NumeroInterior;
            lblClienteCP.Text = direccion.CP.ToString();
            lblClienteEstado.Text = direccion.Estado.Nombre;
            lblClienteMunicipio.Text = direccion.Municipio.Nombre;
            lblClienteColonia.Text = direccion.Colonia.Nombre;
            lblClienteLocalidad.Text = direccion.Localidad.Nombre;
        }

        public void CargaInformacionCliente_Consulta()
        {
            lnkbtnClienteConsulta.Visible = true;

            //LLenamos el DropDownList con la información del cliente al que se le realizará el análisis
            //LoadDrops.instance.Clientes(ddlCliente);
            //ddlCliente.SelectedValue = this.Cliente.ClienteID.ToString();
            //ddlCliente.Enabled = false;

            txtClienteRFC.Text = this.Cliente.RFC;
            txtClienteRFC.Enabled = false;

            //Llenamos la oinformación del cliente con la información guardada.
            Direccion_BL direccion_bl = new Direccion_BL();

            direccion_bl.obtener(this.Cliente.DireccionID);
            var direccion = direccion_bl.Direccion;

            lblClienteCurp.Text = this.Cliente.CURP;
            lblClienteNombre.Text = this.Cliente.Nombre;
            lblClienteCalle.Text = direccion.Calle;
            lblClienteNE.Text = direccion.NumeroExterior;
            lblClienteNI.Text = direccion.NumeroInterior;
            lblClienteCP.Text = direccion.CP.ToString();
            lblClienteEstado.Text = direccion.Estado.Nombre;
            lblClienteMunicipio.Text = direccion.Municipio.Nombre;
            lblClienteColonia.Text = direccion.Colonia.Nombre;
            lblClienteLocalidad.Text = direccion.Localidad.Nombre;

            lnkbtnConsultaCliente.Visible = false;
        }

        protected void lnkbtnClienteConsulta_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(lnkbtnClienteConsulta, lnkbtnClienteConsulta.GetType(), "moveScript", "t(1);", true);
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObtenCliente();
        }
    }

    public class ClienteErrorArg : EventArgs
    {
        public string Mensaje { get; set; }
    }
}