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
using SIRALAB.controles;
using SIRALAB_BL.common;
using Documento = SIRALAB_BL.common.Documento;

namespace SIRALAB.Controles
{
    public partial class wucEvidencias : System.Web.UI.UserControl
    {
        public List<SIRALAB_DAL.Documento> listaEvidencias
        {
            get { return Session["wuc_fileUpload_listaEvidencias"] != null ? (List<SIRALAB_DAL.Documento>)Session["wuc_fileUpload_listaEvidencias"] : new List<SIRALAB_DAL.Documento>(); }
            set { Session["wuc_fileUpload_listaEvidencias"] = value; }
        }
        public object ClaseGenericaEvidencia
        {
            get { return (object)Session["wuc_fileUpload_ClaseGenericaEvidencia" + this.ClientID]; }
            set { Session["wuc_fileUpload_ClaseGenericaEvidencia" + this.ClientID] = value; }
        }

        public bool? subirArchivosOT
        {
            set { ViewState["SubirArchivoOT"] = value; }

            get { return (bool?)ViewState["SubirArchivoOT"]; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptInclude(this.Page, typeof(Page), "FlexSlider_", ResolveClientUrl("~/js/jquery.flexslider.js"));
            ScriptManager.RegisterClientScriptInclude(this.Page, typeof(Page), "wuc_Evidencias", ResolveClientUrl("~/js/wuc_Evidencias.js"));
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "cargadocumentospreview", "cargarEvidencias();", true);

            var main = (Main)this.Page.Master.Master;
            main.MensajeDevolucion += Main_MensajeDevolucion;
        }

        private void Main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            if (e.Command != null && e.Opcion == Opcion.Aceptar)
            {
                switch (e.Command)
                {
                    case "EliminarDocumentos":
                        Guid uniqueId = new Guid(e.Args[0]);
                        int id = int.Parse(e.Args[1]);
                        EliminarEvidencia(uniqueId, id);

                        break;
                }
            }
        }

        public void Enabled(bool enabled)
        {
            pnlDocumentos.Visible = enabled;

            asyncFileEvidencias.UploadedComplete += asyncFileEvidencias_UploadedComplete;
            upnlFileUpload.Update();
        }
        public void CargarEvidencias()
        {
            //RFD -> 02/10/2020. Se agrega llamada a método para habilitar/deshabilitar el botón eliminar archivo. Regla de negocio.
            isDocumentoFirmado();
            rptrEvidecias.DataSource = listaEvidencias;
            rptrEvidecias.DataBind();
            upnlFileUpload.Update();

            //RFD 02/10/20 -> Oculta subir archivo asociado a la orden de trabajo si esta contiene algún análisis firmado.
            var verSubirArchivo = listaEvidencias.All(evi => evi.eliminar != true);
            if (!verSubirArchivo || subirArchivosOT == true)
                divSubirArchivo.Attributes.Add("hidden", "hidden");


            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "cargadocumentospreview", "cargarEvidencias();", true);
            //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "cerrarModal", "cerrarModal();", true);
        }
        public void CargarEvidencias<T>(T claseGenerica) where T : class
        {
            this.listaEvidencias = null;
            ddlTipoDocumento.Visible = true;

            List<SIRALAB_DAL.Documento> vlistaEvidencias = SIRALAB_BL.Documento_BL.GetEvidencias(claseGenerica);
            string folder = Archivos.GetPathFolder(claseGenerica);


            if (claseGenerica.GetType().BaseType.Name == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
            {
                LoadDrops.instance.CatTipoDocumentoAnexoSinSel(ddlTipoDocumento);
                ClaseGenericaEvidencia = (claseGenerica as SIRALAB_DAL.OrdenTrabajo);



            }
            else if (claseGenerica.GetType().BaseType.Name == typeof(Analisi).Name)
            {
                LoadDrops.instance.CatTipoDocumentoAnalisisMuestraSimple(ddlTipoDocumento);
                ClaseGenericaEvidencia = (claseGenerica as Analisi);
            }
            else if (claseGenerica.GetType().BaseType.Name == typeof(Muestra).Name)
            {
                ClaseGenericaEvidencia = (claseGenerica as Muestra);
                var muestra = (claseGenerica as Muestra);

                /*if (muestra.EsCompuesta)
                    LoadDrops.instance.CatTipoDocumentoMuestraCompuestaSinSel(ddlTipoDocumento);
                else
                    LoadDrops.instance.CatTipoDocumentoMuestraSimpleSinSel(ddlTipoDocumento);
                */
            }

            ddlAnalisis.Visible = true;

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

                //RFD -> 15/09/20:
                if (claseGenerica.GetType().BaseType.Name == typeof(SIRALAB_DAL.OrdenTrabajo).Name)
                {
                    foreach (var documento in lstEviAux)
                    {
                        if (documento.DocumentoAnalisis.Any())
                        {
                            var analisisID = documento.DocumentoAnalisis.FirstOrDefault(da => da.DocumentoID == documento.DocumentoID)?.AnalisisID;
                            var analisisBL = new Analisis_BL();

                            if (analisisID != null)
                            {
                                var analisis = analisisBL.Obten_Analisis(analisisID.Value);

                                var numeroArchivo = lstEviAux?.Where(evi =>
                                    evi.AnalisisID == analisisID &&
                                    evi.TipoDocumentoID == documento.TipoDocumentoID).ToList().FindIndex(dc => dc.DocumentoID == documento.DocumentoID) + 1;

                                documento.TipoDocumento = analisis.OrdenTrabajo.Norma.RequiereCompuesta ? documento.TipoDocumento + "_PD_" + (analisis.OrdenTrabajo.Analisis.OrderBy(a => a.AnalisisID).ToList().FindIndex(a => a.AnalisisID == analisisID) + 1) + "_" + numeroArchivo
                                    : documento.TipoDocumento + "_Muestra_" + (analisis.OrdenTrabajo.Analisis.OrderBy(a => a.AnalisisID).ToList().FindIndex(a => a.AnalisisID == analisisID) + 1) + "_" + numeroArchivo;
                            }
                        }
                        else
                        {
                            if (documento.DocumentoAnexoes != null)
                            {
                                documento.TipoDocumento = documento.TipoDocumento + "_" + (lstEviAux.Where(doc => doc.TipoDocumentoID == documento.TipoDocumentoID).ToList().FindIndex(dc => dc.DocumentoID == documento.DocumentoID) + 1);
                            }

                        }

                    }
                }

                this.listaEvidencias = lstEviAux;
                CargarEvidencias();
            }

            if (subirArchivosOT == true)
                divSubirArchivo.Attributes.Add("hidden", "hidden");

            if(!vlistaEvidencias.Any())
                CargarEvidencias();

            Enabled(claseGenerica != null);
        }
        private void EliminarEvidencia(Guid uniqueId, int id)
        {
            List<SIRALAB_DAL.Documento> lstEviAux = this.listaEvidencias;

            if (id == 0)
            {
                SIRALAB_DAL.Documento evidenciaRemove = listaEvidencias.SingleOrDefault(a => a.UniqueId == uniqueId);
                lstEviAux.Remove(evidenciaRemove);
            }
            else
            {
                SIRALAB_DAL.Documento evidenciaRemove = listaEvidencias.SingleOrDefault(a => a.DocumentoID == id);
                lstEviAux.Remove(evidenciaRemove);
            }

            listaEvidencias = lstEviAux;
            CargarEvidencias();
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
        public bool ValidExtension(string ExtValidar)
        {
            bool Valid = false;
            switch (ExtValidar)
            {
                case ".gif":
                case ".jpg":
                case ".jpeg":
                case ".tiff":
                case ".png":
                case ".bmp":
                case ".pdf":
                    { Valid = true; }
                    break;
            }
            return Valid;
        }
        public void Limpiar()
        {
            listaEvidencias = new List<SIRALAB_DAL.Documento>();
            rptrEvidecias.DataSource = listaEvidencias;
            rptrEvidecias.DataBind();
            upnlFileUpload.Update();
        }

        protected void lnkAbrirEvidencia_Command(object sender, CommandEventArgs e)
        {
            string sId = e.CommandArgument.ToString();
            int OId = 0;
            if (int.TryParse(sId, out OId))
            {
                SIRALAB_DAL.Documento evidencia = listaEvidencias.SingleOrDefault(a => a.DocumentoID == OId);
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
                        this.Page.PopMensaje(titulo: "No se pudo obtener la evidencia.", mensaje: "Intente de nuevo. Si presiste el error contacte a su administrador del sistema.");
                    }
                }
                else
                {
                    this.Page.PopMensaje(titulo: "No se pudo obtener la evidencia.", mensaje: "Debe guardar todos sus cambios, antes de intentar abrir el archivo.");
                }
            }
        }
        protected void lnkAbrirEvidencia_OnClick(object sender, EventArgs e)
        {
            Guid uniqueId;
            LinkButton lnk = (LinkButton)sender;
            var id = lnk.Attributes["data-documento-id"];
            Guid.TryParse(lnk.Attributes["data-documento-guid"], out uniqueId);

            string sId = id;
            int OId;
            if (int.TryParse(sId, out OId) && OId > 0)
            {
                SIRALAB_DAL.Documento evidencia = listaEvidencias.SingleOrDefault(a => a.DocumentoID == OId);
                if (evidencia?.DocumentoID > 0)
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
                        this.Page.PopMensaje("No se pudo obtener la evidencia. Debe guardar todos sus cambios, antes de intentar abrir el archivo.", "Error", icon: controles.Icono.error);
                    }
                }
                else
                {
                    this.Page.PopMensaje("No se pudo obtener la evidencia. Debe guardar todos sus cambios, antes de intentar abrir el archivo.", "Error", icon: controles.Icono.error);
                }
            }
            else
            {
                this.Page.PopMensaje("No se pudo obtener la evidencia. Debe guardar todos sus cambios, antes de intentar abrir el archivo.", "Error", icon: controles.Icono.error);
            }
        }
        protected void lnkEliminarEvidencia_OnClick(object sender, EventArgs e)
        {
            Guid uniqueId;
            LinkButton lnk = (LinkButton)sender;
            var id = lnk.Attributes["data-documento-id"];
            Guid.TryParse(lnk.Attributes["data-documento-guid"], out uniqueId);

            Page.PopMensaje("¿Se encuentra seguro de eliminar el documento?", "Documentación", Opciones.SiNo, Icono.warning, "EliminarDocumentos", new string[] { uniqueId.ToString(), id.ToString() });
        }
        protected void lnkGuardarEvidencia_Click(object sender, EventArgs e)
        {
            Documento_BL documentoBL = new Documento_BL();
            List<SIRALAB_DAL.Documento> evidenciasAntesDeGuardar = Documento_BL.GetEvidencias(ClaseGenericaEvidencia);

            try
            {
                CorrigeNombre(listaEvidencias);
                documentoBL.GuardarDocumentos(listaEvidencias, ClaseGenericaEvidencia);


                //evidenciasAntesDeGuardar = Documento_BL.GetEvidencias(ClaseGenericaEvidencia);

                List<Archivos.ResultOperacionArchivo> result = Archivos.GuardarEvidencias(evidenciasAntesDeGuardar, ClaseGenericaEvidencia, listaEvidencias);
                int error = result.Count(a => a.codeError == -1);

                string mensaje = string.Join("<br />", result.Select(p => p.comentarios));
                if (error > 0)
                    this.Page.PopMensaje("Hubo algún error al guardar. Intente de nuevo. <br />", "Error", icon: controles.Icono.error);
                else
                {
                    CargarEvidencias(ClaseGenericaEvidencia);
                    this.Page.PopMensaje("Documentos guardados exitosamente.", "Doctumentos Guardados");
                }

            }
            catch (Exception ex)
            {
                this.Page.PopMensaje("Hubo algún error al guardar. \n", "Error", icon: controles.Icono.error);
            }
        }
        protected void HiddenButton_Click(object sender, EventArgs e)
        {
            CargarEvidencias();
        }
        protected void asyncFileEvidencias_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            if (asyncFileEvidencias.HasFile)
            {
                HttpPostedFile postedFile = asyncFileEvidencias.PostedFile;
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
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "error", "alert('¡El archivo excedió el tamaño máximo!');", true);
                }
                else
                {
                    fileAux = Archivos.ObtenPropiedades(postedFile.FileName.ToLower());

                    if (ValidExtension(fileAux.Extension))
                    {
                        SIRALAB_DAL.Documento evidenciaAux = new SIRALAB_DAL.Documento();
                        int fileLen = postedFile.ContentLength;
                        evidenciaAux.BufferData = new byte[fileLen];

                        evidenciaAux.Nombre = Guid.NewGuid() + "_" + fileAux.Nombre;
                        evidenciaAux.TipoMime = fileAux.TipoMime;
                        evidenciaAux.ImagePreView = fileAux.ImagenPreliminar;
                        evidenciaAux.Extension = Path.GetExtension(fileAux.Nombre);
                        evidenciaAux.TipoDocumentoID = int.Parse(ddlTipoDocumento.SelectedValue);
                        evidenciaAux.TipoDocumento = asignaNombreArchivo(int.Parse(ddlTipoDocumento.SelectedValue));

                        postedFile.InputStream.Read(evidenciaAux.BufferData, 0, fileLen);

                        if (evidenciaAux.TipoMime.Contains("jpg") || evidenciaAux.TipoMime.Contains("gif")
                         || evidenciaAux.TipoMime.Contains("png") || evidenciaAux.TipoMime.Contains("jpeg"))
                        {
                            string base64String = Convert.ToBase64String(evidenciaAux.BufferData, 0, evidenciaAux.BufferData.Length);
                            evidenciaAux.ImagePreView = "data:image/png;base64," + base64String;
                        }
                        evidenciaAux.UniqueId = Guid.NewGuid();

                        List<SIRALAB_DAL.Documento> lstEviAux = listaEvidencias;
                        lstEviAux.Add(evidenciaAux);
                        listaEvidencias = lstEviAux;

                        CargarEvidencias();
                    }
                    else
                    {
                        CargarEvidencias();
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "error", "alert('¡Extension de archivo no soportada!');", true);
                    }
                }
            }
        }



        protected void asyncFileEvidenciasAnalisis_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            if (AsyncFileUploadAnalisis.HasFile)
            {
                HttpPostedFile postedFile = AsyncFileUploadAnalisis.PostedFile;
                Archivos fileAux = new Archivos();

                ApplicationSettings aS = new ApplicationSettings();
                int fileSizeMB = int.Parse(aS.fileSizeMB);
                /*
                * 1024 kilobytes = 1 megabyte (MB)
                * 1 Kilobyte = 1024 bytes
                */
                double fileSize = postedFile.ContentLength;// (double)asyncFileEvidencias.FileBytes.Length;
                double fileinMB = fileSize / (1024 * 1024);

                /*Excede el tamaño permitido*/
                if (fileinMB > fileSizeMB)
                {
                    CargarEvidencias();
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "error", "alert('¡El archivo excedió el tamaño máximo!');", true);
                }
                else
                {
                    fileAux = Archivos.ObtenPropiedades(postedFile.FileName.ToLower());

                    if (ValidExtension(fileAux.Extension))
                    {
                        SIRALAB_DAL.Documento evidenciaAux = new SIRALAB_DAL.Documento();
                        int fileLen = postedFile.ContentLength;
                        var analisisID = txtAnalisID.Text;
                        evidenciaAux.Nombre = Guid.NewGuid() + "_" + fileAux.Nombre;
                        evidenciaAux.BufferData = new byte[fileLen];
                        evidenciaAux.TipoMime = fileAux.TipoMime;
                        evidenciaAux.ImagePreView = fileAux.ImagenPreliminar;
                        evidenciaAux.Extension = Path.GetExtension(fileAux.Nombre);
                        evidenciaAux.TipoDocumentoID = int.Parse(ddlAnalisis.SelectedValue);

                        var analisisBL = new SIRALAB_BL.Analisis_BL();
                        var analisis = analisisBL.Obten_Analisis(Convert.ToInt32(txtAnalisID.Text));
                        var numeroArchivo = listaEvidencias?.Count(evi =>
                            evi.AnalisisID == int.Parse(txtAnalisID.Text) &&
                            evi.TipoDocumentoID == int.Parse(ddlAnalisis.SelectedValue)) + 1;
                        evidenciaAux.TipoDocumento =
                            analisis.OrdenTrabajo.Norma.RequiereCompuesta ?
                                ddlAnalisis.SelectedItem.Text + "_PD_" + (analisis.OrdenTrabajo.Analisis.OrderBy(a => a.AnalisisID).ToList().FindIndex(a => a.AnalisisID == Convert.ToInt32(txtAnalisID.Text)) + 1) + "_" + numeroArchivo
                                : ddlAnalisis.SelectedItem.Text + "_Muestra_" + (analisis.OrdenTrabajo.Analisis.OrderBy(a => a.AnalisisID).ToList().FindIndex(a => a.AnalisisID == Convert.ToInt32(txtAnalisID.Text)) + 1) + "_" + numeroArchivo;


                        evidenciaAux.AnalisisID = Convert.ToInt32(txtAnalisID.Text);

                        postedFile.InputStream.Read(evidenciaAux.BufferData, 0, fileLen);

                        if (evidenciaAux.TipoMime.Contains("jpg") || evidenciaAux.TipoMime.Contains("gif")
                         || evidenciaAux.TipoMime.Contains("png") || evidenciaAux.TipoMime.Contains("jpeg"))
                        {
                            string base64String = Convert.ToBase64String(evidenciaAux.BufferData, 0, evidenciaAux.BufferData.Length);
                            evidenciaAux.ImagePreView = "data:image/png;base64," + base64String;
                        }
                        evidenciaAux.UniqueId = Guid.NewGuid();

                        List<SIRALAB_DAL.Documento> lstEviAux = listaEvidencias;
                        lstEviAux.Add(evidenciaAux);
                        listaEvidencias = lstEviAux;

                        CargarEvidencias();
                    }
                    else
                    {
                        CargarEvidencias();
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "error", "alert('¡Extension de archivo no soportada!');", true);
                    }
                }
            }
        }


        private string asignaNombreArchivo(int tipoDocumentoId)
        {
            var numeroArchivos = getNumeroArchivo(tipoDocumentoId);

            switch (tipoDocumentoId)
            {
                case 1:
                    return "Carátula del título / permiso y 1ª hoja del anexo_" + numeroArchivos;
                case 2:
                    return "Imagen de ubicación_" + numeroArchivos;
                case 3:
                    return "_" + numeroArchivos;
                case 4:
                    return "_" + numeroArchivos;
                case 5:
                    return "Oficio de exención de parámetros_" + numeroArchivos;
                default:
                    return "" + numeroArchivos;
            }

        }

        private int getNumeroArchivo(int tipoDocumentoId)
        {
            return listaEvidencias.Count(doc => doc.TipoDocumentoID == tipoDocumentoId) + 1;
        }

        //RFD 02/10/20 -> Se utiliza para ocultar el botón eliminar si la evidencia está asociada una OT o análisis que ya está firmado.
        private void isDocumentoFirmado()
        {

            foreach (var evidencia in this.listaEvidencias.Where(evidencia => evidencia.TipoDocumentoID != null))
            {

                if (evidencia.TipoDocumentoID > 2 && evidencia.TipoDocumentoID < 5
                ) //La evidencia está asociada al analisis.
                {
                    if (evidencia.AnalisisID != null && evidencia.AnalisisID > 0)
                    {
                        var analisisBL = new Analisis_BL();
                        var analisis = analisisBL.Obten_Analisis(evidencia.AnalisisID.Value);
                        evidencia.eliminar = analisis.Firmada;
                    }
                    else
                    {
                        evidencia.eliminar = isOrdenTrabajoFirmada(evidencia);
                    }
                }
                else //La evidencia está asociada a la orden de trabajo.
                {
                    evidencia.eliminar = isOrdenTrabajoFirmada(evidencia);
                }
            }
        }

        private bool? isOrdenTrabajoFirmada(SIRALAB_DAL.Documento evidencia)
        {
            return evidencia.DocumentoAnexoes.FirstOrDefault()?.OrdenTrabajo.Analisis
                .Any(an => an.Firmada);
        }
    }
}