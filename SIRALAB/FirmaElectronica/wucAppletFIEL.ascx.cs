using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CONAGUA.Seguridad;
using System.IO;

namespace SIRALAB.FIEL
{
    public partial class wucAppletFIEL : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<DatosFirma_Args> FirmarColeccion;

        FirmaElectronica colecionAFirmar
        {
            get { return Session["Applet.FirmaElectronica"] != null ? (FirmaElectronica)Session["Applet.FirmaElectronica"] : new FirmaElectronica(); }
            set { Session["Applet.FirmaElectronica"] = value; }
        }

        public char separdor
        {
            get { return colecionAFirmar.separador; }
            set { colecionAFirmar.separador = value; }
        }
        public string infoAFirmar
        {
            get { return this.hfInfoAFirmar.Value; }
            set { this.hfInfoAFirmar.Value = value; }
        }

        public string rfcFrima
        {
            get { return this.hfRFCFirma.Value; }
            set { this.hfRFCFirma.Value = value; }
        }

        public string cadenaOriginalParaSello
        {
            get { return this.hfCadenaOriginalParaSello.Value; }
            set { this.hfCadenaOriginalParaSello.Value = value; }
        }
        private string cadenaResult
        {
            get { return this.hfCadenaFirmada.Value; }
            set { this.hfCadenaFirmada.Value = value; }
        }

        public byte[] ArchivoKey
        {
            get { return Session["ArchivoKey"] != null ? (byte[])Session["ArchivoKey"] : null; }
            set { Session["ArchivoKey"] = value; }
        }

        public MemoryStream ArchivoKeyMS
        {
            get { return Session["ArchivoKeyMS"] != null ? (MemoryStream)Session["ArchivoKeyMS"] : null; }
            set { Session["ArchivoKeyMS"] = value; }
        }

        public byte[] ArchivoCer
        {
            get { return Session["ArchivoCer"] != null ? (byte[])Session["ArchivoCer"] : null; }
            set { Session["ArchivoCer"] = value; }
        }       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                inicializarValores();
            }
        }

        private void inicializarValores()
        {
            this.colecionAFirmar = new FirmaElectronica();
            this.infoAFirmar = null;
            this.cadenaOriginalParaSello = null;
            this.cadenaResult = null;
            txtContra.Text = string.Empty;
            txtRFC.Text = string.Empty;
        }

        public void enviarAFirma(FirmaElectronica firma)
        {
            try
            {
                inicializarValores();
                this.colecionAFirmar = firma;
                foreach (Contenedor content in colecionAFirmar.listaContenedores)
                {
                    this.infoAFirmar += content.cadenaAFirmarBase64 + separdor;
                }
                //Quito la coma final
                this.infoAFirmar = this.infoAFirmar.Substring(0, (this.infoAFirmar.Length - 1));
            }
            catch (Exception ex) { firma.exception = ex; }
            ScriptManager.RegisterStartupScript(upFirmaElectronica, upFirmaElectronica.GetType(), "AlerMostrarFirma", "$(document).ready(function(){mostrarFirma();});", true);
            upFirmaElectronica.Update();
        }

        public FirmaElectronica obtenerColeccionFirma()
        {
            this.cadenaResult = this.infoAFirmar;           
            return FirmaElectronica.obtenerListaFirmada(this.colecionAFirmar, this.cadenaResult, this.separdor);
        }

        protected void lnkbtnFirmado_Click(object sender, EventArgs e)
        {
            if (FirmarColeccion != null)
            {
                
                DatosFirma_Args datosFirma = new DatosFirma_Args();
                datosFirma.DatosAFirmar = obtenerColeccionFirma();
                datosFirma.RFCFirma = txtRFC.Text.Replace(',', ' ').Trim();
                datosFirma.ArchivoCer = (byte[])Session["ArchivoCer"];// ArchivoCer;
                datosFirma.ArchivoKey = (byte[])Session["ArchivoKey"];//  ArchivoKey;              
                datosFirma.txtContra = txtContra.Text;
                FirmarColeccion(this, datosFirma);
                lnkbCerrar_Click(sender, e);
            }
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(upFirmaElectronica, upFirmaElectronica.GetType(), "OcultarFirma", "$(document).ready(function(){ocultarFirma();});", true);
          //  ScriptManager.RegisterStartupScript(upFirmaElectronica, upFirmaElectronica.GetType(), "ocultar", "if ($('.modal-backdrop').is(':visible')) {$('body').removeClass('modal-open');$('.modal-backdrop').remove();}", true);
            upFirmaElectronica.Update();
        }
        
        //Metodos para obtener los archivos para firmar.
        protected void asyncFileCer_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                if (asyncFileCer.HasFile)
                {
                    HttpPostedFile postedFile = asyncFileCer.PostedFile;
                    double fileSize = postedFile.ContentLength;
                    double fileinMB = fileSize / (1024 * 1024);
                    int fileLen = (int)postedFile.ContentLength;
                    byte[] buffer = new byte[fileLen];
                    asyncFileCer.FileContent.Read(buffer, 0, buffer.Length);

                    postedFile.InputStream.Read(buffer, 0, fileLen);                                                         
                    ArchivoCer = buffer;
                }
            }
            catch (Exception Ex)
            {
 
            }
        }

        protected void asyncFileKey_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                if (AsyncFileKey.HasFile)
                {
                    HttpPostedFile postedFile = AsyncFileKey.PostedFile;
                    double fileSize = postedFile.ContentLength;
                    double fileinMB = fileSize / (1024 * 1024);
                    int fileLen = (int)postedFile.ContentLength;
                    byte[] buffer = new byte[fileLen];
                    AsyncFileKey.FileContent.Read(buffer, 0, buffer.Length);
                  
                  
                    ArchivoKey = buffer;
                }
            }
            catch (Exception Ex)
            {
 
            }
        }
        //Metodos para obtener los archivos para firmar.        
    }
}