using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SIRALAB_BL;
using SIRALAB_DAL;
using System.Drawing;
using System.Drawing.Drawing2D;
using SIRALAB_BL.common;
using System.ComponentModel;

namespace SIRALAB.Controles
{
    public partial class wucEvidenciaPop : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler Cargado;

        public List<SIRALAB_DAL.Documento> listaEvidencias
        {
            get { return Session["wuc_fileUpload_listaEvidencias" + this.ClientID] != null ? (List<SIRALAB_DAL.Documento>)Session["wuc_fileUpload_listaEvidencias" + this.ClientID] : new List<SIRALAB_DAL.Documento>(); }
            set { Session["wuc_fileUpload_listaEvidencias" + this.ClientID] = value; }
        }

        public object ClaseGenericaEvidencia
        {
            get { return (object)Session["wuc_fileUpload_ClaseGenericaEvidencia" + this.ClientID]; }
            set { Session["wuc_fileUpload_ClaseGenericaEvidencia" + this.ClientID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptInclude(this.Page, typeof(Page), "FlexSlider_", ResolveClientUrl("~/js/jquery.flexslider.js"));
            ScriptManager.RegisterClientScriptInclude(this.Page, typeof(Page), "wuc_EvidenciaPop", ResolveClientUrl("~/js/wuc_Evidencias.js"));
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "cargadocumentospreview", "cargarEvidencias_pop();", true);

        }
        public void Enabled(bool enabled)
        {

            //wucEvidenciaContent.Attributes.Add("disabled", enabled.ToString());
            upnlFileUpload_pop.Visible = enabled;
            lnkbtnGuardarEvidencia_pop .Enabled = enabled;
            upnlFileUpload_pop.Update();

        }

        public void CargarEvidencias<T>(T claseGenerica) where T : class
        {
            this.listaEvidencias = null;
            ddlTipoDocumento_pop.Visible = true;

            List<SIRALAB_DAL.Documento> vlistaEvidencias = SIRALAB_BL.Documento_BL.GetEvidencias(claseGenerica);
            string folder = Archivos.GetPathFolder(claseGenerica);

            //try
            //{
            //    LoadDrops.instance.CatTipoDocumentoAnexoSinSel(ddlTipoDocumento_pop);
            //    ClaseGenericaEvidencia = claseGenerica as SIRALAB_DAL.OrdenTrabajo;
            //}
            //catch { }

            //if (ClaseGenericaEvidencia == null)
            //    try
            //    {
            //        LoadDrops.instance.CatTipoDocumentoAnalisisMuestraSimple(ddlTipoDocumento_pop);
            //        ClaseGenericaEvidencia = (claseGenerica as SIRALAB_DAL.Analisi);
            //    }
            //    catch { }

            //if (ClaseGenericaEvidencia == null)
            //    try
            //    {
            //        ClaseGenericaEvidencia = (claseGenerica as SIRALAB_DAL.Muestra);
            //        var muestra = (claseGenerica as SIRALAB_DAL.Muestra);

            //        if (muestra.NivelID == 2)
            //            LoadDrops.instance.CatTipoDocumentoMuestraCompuestaSinSel(ddlTipoDocumento_pop);
            //        else
            //            LoadDrops.instance.CatTipoDocumentoMuestraSimpleSinSel(ddlTipoDocumento_pop);
            //    }
            //    catch { }

            if (claseGenerica.GetType().BaseType.Name == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
            {
                LoadDrops.instance.CatTipoDocumentoAnexoSinSel(ddlTipoDocumento_pop);
                ClaseGenericaEvidencia = (claseGenerica as SIRALAB_DAL.OrdenTrabajo);
            }
            else if (claseGenerica.GetType().BaseType.Name == typeof(SIRALAB_DAL.Analisi).Name)
            {

                LoadDrops.instance.CatTipoDocumentoAnalisisMuestraSimple(ddlTipoDocumento_pop);

                ClaseGenericaEvidencia = (claseGenerica as SIRALAB_DAL.Analisi);
            }
            else if (claseGenerica.GetType().BaseType.Name == typeof(SIRALAB_DAL.Muestra).Name)
            {

                ClaseGenericaEvidencia = (claseGenerica as SIRALAB_DAL.Muestra);
                var muestra = (claseGenerica as SIRALAB_DAL.Muestra);
              
                if (muestra.NivelID==enumNivel.Compuesta.ConvertToInt())
                    LoadDrops.instance.CatTipoDocumentoMuestraCompuestaSinSel(ddlTipoDocumento_pop);
                else
                    LoadDrops.instance.CatTipoDocumentoMuestraSimpleSinSel(ddlTipoDocumento_pop);
                
            }



            if (vlistaEvidencias != null && vlistaEvidencias.Count > 0)
            {
                List<SIRALAB_DAL.Documento> lstEviAux = new List<SIRALAB_DAL.Documento>();
                List<SIRALAB_DAL.Documento> lstEviErr = new List<SIRALAB_DAL.Documento>();
                foreach (SIRALAB_DAL.Documento evidencia in vlistaEvidencias)
                {
                    Archivos fileAux = Archivos.ObtenPropiedades(evidencia.Nombre);
                    SIRALAB_DAL.Documento evidenciaAux = evidencia;
                    if (evidenciaAux.DocumentoID > 0)
                    {
                        if (fileAux.TipoMime.Contains("jpg") || fileAux.TipoMime.Contains("gif")
                           || fileAux.TipoMime.Contains("png") || fileAux.TipoMime.Contains("jpeg"))
                        {
                            evidenciaAux.ImagePreView = Archivos.GetRutaImagenPreview(claseGenerica, evidencia.DocumentoID, true);
                        }
                        else
                            evidenciaAux.ImagePreView = fileAux.ImagenPreliminar;

                        evidenciaAux.Nombre = folder + "\\" + Path.GetFileName(evidencia.Nombre);

                        if (Archivos.ExisteDocumento(evidenciaAux.Nombre))
                            lstEviAux.Add(evidenciaAux);
                        else
                        {
                            lstEviErr.Add(evidenciaAux);
                        }
                    }
                    else if (evidenciaAux.Nombre != string.Empty && evidenciaAux.BufferData != null)
                    {
                        if (fileAux.TipoMime.Contains("jpg") || fileAux.TipoMime.Contains("gif")
                           || fileAux.TipoMime.Contains("png") || fileAux.TipoMime.Contains("jpeg"))
                        {
                            string base64String = Convert.ToBase64String(evidenciaAux.BufferData, 0, evidenciaAux.BufferData.Length);
                            evidenciaAux.ImagePreView = "data:image/png;base64," + base64String;
                        }
                        else
                            evidenciaAux.ImagePreView = fileAux.ImagenPreliminar;

                        lstEviAux.Add(evidenciaAux);
                    }
                }


                this.listaEvidencias = lstEviAux;



            }
            CargarEvidencias();
        }

        protected void lnkAbrirEvidencia_pop_Command(object sender, CommandEventArgs e)
        {
            string sId = e.CommandArgument.ToString();
            int OId = 0;
            if (int.TryParse(sId, out OId))
            {
                SIRALAB_DAL.Documento evidencia = listaEvidencias.Where(a => a.DocumentoID == OId).SingleOrDefault();
                if (evidencia.DocumentoID > 0)
                {
                    if (Archivos.CargarArchivoFisico(evidencia))
                    {
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(evidencia.Nombre));
                        Response.ContentType = evidencia.TipoMime;
                        Response.BinaryWrite(evidencia.BufferData);
                        Response.End();
                    }
                    else
                    {
                        this.Page.PopMensaje(titulo: "No se puedo obtener la evidencia.", mensaje: "Intente de nuevo. Si presiste el error contacte a su administrador del sistema.");
                    }
                }
                else
                {
                    this.Page.PopMensaje(titulo: "No se puedo obtener la evidencia.", mensaje: "Debe guardar todos sus cambios, antes de intentar abrir el archivo.");
                }

            }
        }

        protected void lnkEliminarEvidencia_Command(object sender, CommandEventArgs e)
        {
            string sId = e.CommandArgument.ToString();
            string nombreArchivo = e.CommandName.ToString();
            int OId = 0;
            List<SIRALAB_DAL.Documento> lstEviAux = new List<SIRALAB_DAL.Documento>();
            lstEviAux = this.listaEvidencias;
            if (int.TryParse(sId, out OId))
            {
                if (OId > 0)
                {
                    SIRALAB_DAL.Documento evidenciaRemove = listaEvidencias.Where(a => a.DocumentoID == OId).SingleOrDefault();
                    lstEviAux.Remove(evidenciaRemove);
                    this.listaEvidencias = lstEviAux;
                }
                else if (nombreArchivo != string.Empty)
                {
                    SIRALAB_DAL.Documento evidenciaRemove = listaEvidencias.Where(a => a.DocumentoID == 0 && a.Nombre == nombreArchivo).SingleOrDefault();
                    lstEviAux.Remove(evidenciaRemove);
                    this.listaEvidencias = lstEviAux;
                }
                CargarEvidencias();
            }

        }

        protected void asyncFileEvidencias_pop_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            if (asyncFileEvidencias_pop.HasFile)
            {

                HttpPostedFile postedFile = asyncFileEvidencias_pop.PostedFile;
                Archivos fileAux = new Archivos();

                ApplicationSettings aS = new ApplicationSettings();
                int fileSizeMB = int.Parse(aS.fileSizeMB);
                /*
                    * 1024 kilobytes = 1 megabyte (MB)
                    * 1 Kilobyte = 1024 bytes
                    */
                double fileSize = postedFile.ContentLength;// (double)asyncFileEvidencias.FileBytes.Length;
                double fileinMB = fileSize / (1024 * 1024);



                /*Tamaño adecuado*/

                if (fileinMB > fileSizeMB)
                {

                    CargarEvidencias();
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "error", "alert('¡El archivo excedio el tamaño maximo!');", true);
                }
                else
                {
                    fileAux = Archivos.ObtenPropiedades(postedFile.FileName);

                    SIRALAB_DAL.Documento evidenciaAux = new SIRALAB_DAL.Documento();

                    evidenciaAux.Nombre = fileAux.Nombre;
                    evidenciaAux.TipoMime = fileAux.TipoMime;
                    evidenciaAux.ImagePreView = fileAux.ImagenPreliminar;
                    evidenciaAux.Extension = Path.GetExtension(fileAux.Nombre);
                    evidenciaAux.TipoDocumentoID = int.Parse(ddlTipoDocumento_pop.SelectedValue);
                    int fileLen = (int)postedFile.ContentLength;
                    evidenciaAux.BufferData = new byte[fileLen];
                    postedFile.InputStream.Read(evidenciaAux.BufferData, 0, fileLen);

                    if (evidenciaAux.TipoMime.Contains("jpg") || evidenciaAux.TipoMime.Contains("gif")
                     || evidenciaAux.TipoMime.Contains("png") || evidenciaAux.TipoMime.Contains("jpeg"))
                    {
                        string base64String = Convert.ToBase64String(evidenciaAux.BufferData, 0, evidenciaAux.BufferData.Length);
                        evidenciaAux.ImagePreView = "data:image/png;base64," + base64String;
                    }


                    List<SIRALAB_DAL.Documento> lstEviAux = listaEvidencias;
                    lstEviAux.Add(evidenciaAux);
                    listaEvidencias = lstEviAux;


                    CargarEvidencias();

                }
            }
        }
        protected void HiddenButton_pop_Click(object sender, EventArgs e)
        {
            CargarEvidencias();
        }

        public void CargarEvidencias()
        {
            rptrEvidecias_pop.DataSource = listaEvidencias;
            rptrEvidecias_pop.DataBind();
            //
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            foreach (RepeaterItem Propiedad in rptrEvidecias_pop.Items)
            {
                scriptManager.RegisterPostBackControl((System.Web.UI.WebControls.LinkButton)Propiedad.Controls[1]);
            }
            //
            upnlFileUpload_pop.Update();

            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "cargadocumentospreviewpop", "cargarEvidencias_pop();", true);

        }

        public void Limpiar()
        {
            listaEvidencias = new List<SIRALAB_DAL.Documento>();
            rptrEvidecias_pop.DataSource = listaEvidencias;
            rptrEvidecias_pop.DataBind();
            upnlFileUpload_pop.Update();
        }

        protected void lnkbtnGuardarEvidencia_pop_Click(object sender, EventArgs e)
        {
            var evidencias = listaEvidencias;
            Documento_BL docoBL = new Documento_BL();
            List<SIRALAB_DAL.Documento> evidenciasAntesDeGuardar = SIRALAB_BL.Documento_BL.GetEvidencias(ClaseGenericaEvidencia);

            try
            {

                CorrigeNombre(listaEvidencias);
                docoBL.GuardarDocumentos(listaEvidencias, ClaseGenericaEvidencia);
                List<Archivos.ResultOperacionArchivo> result = Archivos.GuardarEvidencias(evidenciasAntesDeGuardar, ClaseGenericaEvidencia, listaEvidencias);
                int error = result.Where(a => a.codeError == -1).Count();
                if (error > 0)
                    this.Page.PopMensaje("Hubo algún error al guardar. Intente de nuevo.", "Error", icon: controles.Icono.error);
                else
                {
                    this.Page.PopMensaje("Documentos guardados exitosamente.", "Doctumentos Guardados");
                }

                if (Cargado != null)
                {
                    Cargado(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                this.Page.PopMensaje("Hubo algún error al guardar. \n" + ex.Message, "Error", icon: controles.Icono.error);
            }
        }

        private void CorrigeNombre(List<SIRALAB_DAL.Documento> listaEvidencias)
        {

            foreach (SIRALAB_DAL.Documento doc in listaEvidencias)
            {
                string name = doc.Nombre.ToString();
                doc.Ruta = name;
                doc.Nombre = Path.GetFileName(doc.Nombre);
            }
        }
    }


}