using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SIRALAB_BL.common;
using SIRALAB_BL;


namespace SIRALAB
{
    public partial class imgPreview : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string tipo = Request.QueryString["classType"] != null ? Request.QueryString["classType"].ToString() : string.Empty;
            string idObject = Request.QueryString["idObject"] != null ? Request.QueryString["idObject"].ToString() : string.Empty;
            string idEvidencia = Request.QueryString["idEvidencia"] != null ? Request.QueryString["idEvidencia"].ToString() : string.Empty;
            string minimize = Request.QueryString["min"] != null ? Request.QueryString["min"].ToString() : string.Empty;

            if (tipo != string.Empty && idObject != string.Empty && idEvidencia != string.Empty)
            {


                int id = int.Parse(idObject);
                string folder;
                switch (int.Parse(tipo))
                {
                    case 0:
                        SIRALAB_BL.OrdenTrabajo_BL bl = new OrdenTrabajo_BL();
                        SIRALAB_DAL.OrdenTrabajo ot = bl.Obten(id);
                        folder = Archivos.GetPathFolder(ot);
                        ObtenerEvidenciaGenerica(idEvidencia, minimize, folder);
                        break;
                    case 1:
                        SIRALAB_BL.Analisis_BL BL = new Analisis_BL();
                        SIRALAB_DAL.Analisi an = BL.Obten_Analisis(id);
                        folder = Archivos.GetPathFolder(an);
                        ObtenerEvidenciaGenerica(idEvidencia, minimize, folder);
                        break;
                    case 2:
                        SIRALAB_BL.Muestra_BL BLL = new Muestra_BL();
                        SIRALAB_DAL.Muestra ms = BLL.Obten_Muestra(id);
                        folder = Archivos.GetPathFolder(ms);
                        ObtenerEvidenciaGenerica(idEvidencia, minimize, folder);
                        break;

                    default:
                        folder = null;
                        break;
                }


            }
        }
        private void ObtenerEvidenciaGenerica(string idEvidencia, string minimize, string folder)
        {
            SIRALAB_BL.Documento_BL bl = new Documento_BL();
            SIRALAB_DAL.Documento evidencia = bl.Obtener(int.Parse(idEvidencia));
            if (evidencia != null)
            {
                Archivos fileAux = Archivos.ObtenPropiedades(evidencia.Nombre);
                evidencia.Nombre = folder + "\\" + Path.GetFileName(evidencia.Nombre);

                if (Archivos.CargarArchivoFisico(evidencia))
                {
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(evidencia.Nombre));
                    BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                    byte[] fileData = null;
                    if (int.Parse(minimize) != 0)
                    {
                        switch (int.Parse(minimize))
                        {
                            case 1:
                                fileData = fileAux.TipoMime.StartsWith("image") ? MinimizeFile(evidencia.BufferData) : FileImagePreview(fileAux);
                                break;
                            case 2:
                                fileData = fileAux.TipoMime.StartsWith("image") ? MinimizeFileCircle(evidencia.BufferData) : FileImagePreview(fileAux);
                                break;
                            default:
                                fileData = fileAux.TipoMime.StartsWith("image") ? evidencia.BufferData : FileImagePreview(fileAux);
                                break;
                        }
                    }
                    else
                    {
                        fileData = fileAux.TipoMime.StartsWith("image") ? evidencia.BufferData : FileImagePreview(fileAux);
                    }

                    bw.Write(fileData);
                    bw.Close();
                    Response.ContentType = fileAux.TipoMime;
                    Response.End();
                }
                else
                {
                    throw new FileNotFoundException("El archivo no existe", evidencia.Nombre);
                }
            }
        }
        private byte[] FileImagePreview(Archivos fileOpts)
        {
            var fileRoot = Server.MapPath(fileOpts.ImagenPreliminar);
            FileStream msImg = new FileStream(fileRoot, FileMode.Open, FileAccess.ReadWrite);
            byte[] fileByte = new byte[msImg.Length];
            msImg.Read(fileByte, 0, Convert.ToInt32(msImg.Length));
            msImg.Close();
            return MinimizeFile(fileByte);
        }
        private byte[] MinimizeFile(byte[] FileToMinimize)
        {
            byte[] result = null;
            if (FileToMinimize != null)
            {
                MemoryStream msImg = new MemoryStream(FileToMinimize);
                Bitmap photo = new Bitmap(msImg);
                int _thumbnailSize = 120;

                int width, height;
                if (photo.Width < _thumbnailSize || photo.Height < _thumbnailSize)
                {
                    width = photo.Width;
                    height = photo.Height;
                }
                else
                {
                    if (photo.Width < photo.Height)
                    {
                        width = _thumbnailSize * photo.Width / photo.Height;
                        height = _thumbnailSize;
                    }
                    else
                    {
                        width = _thumbnailSize;
                        height = _thumbnailSize * photo.Height / photo.Width;
                    }
                }
                Bitmap target = new Bitmap(_thumbnailSize, _thumbnailSize);

                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(photo, (_thumbnailSize - width) / 2, (_thumbnailSize - height) / 2, width, height);


                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        target.Save(memoryStream, ImageFormat.Png);
                        result = memoryStream.ToArray();
                    }
                }
                target.Dispose();
                photo.Dispose();
            }
            return result;
        }
        private byte[] MinimizeFileCircle(byte[] FileToCircle)
        {
            byte[] FileToMinimize = MinimizeFile(FileToCircle);
            MemoryStream msImg = new MemoryStream(FileToMinimize);
            Bitmap photo = new Bitmap(msImg);
            int _thumbnailSize = 90;
            Bitmap target = new Bitmap(_thumbnailSize, _thumbnailSize);
            byte[] result;
            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                Rectangle CropRect = new Rectangle(0, 0, _thumbnailSize, _thumbnailSize);
                Bitmap CroppedImage = photo.Clone(CropRect, photo.PixelFormat);
                TextureBrush TB = new TextureBrush(CroppedImage);
                graphics.FillEllipse(TB, 0, 0, _thumbnailSize, _thumbnailSize);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    target.Save(memoryStream, ImageFormat.Png);
                    result = memoryStream.ToArray();
                }
            }
            target.Dispose();
            photo.Dispose();

            return result;
        }
    }
}