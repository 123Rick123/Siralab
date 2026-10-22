using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using A = DocumentFormat.OpenXml.Drawing;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using Font = DocumentFormat.OpenXml.Wordprocessing.Font;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.Comun
{
    public class PlantillaBLL
    {
        private Siralab_Entities _contexto = new Siralab_Entities();
        public bool ReemplazarEtitquetas(PlantillaDTO documento)
        {
            bool exito = true;
            try
            {
                if (!documento.EtiquetasCuerpo.Any() && !documento.EtiquetasEncabezado.Any() && !documento.EtiquetasPiePagina.Any())
                    return false;

                if (documento.EsNuevoDocumento)
                {
                    //bool exitoCopiado = CopiarPlantilla(documento.Path + @"Plantillas\Plantilla.docx", documento.Url);
                    bool exitoCopiado = CopiarPlantilla(documento.RutaPlantilla, documento.Url);
                    if (!exitoCopiado)
                        return false;
                }

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento.Url, true))
                {
                    string docText = null;
                    Regex regexText;
                    if (documento.EtiquetasCuerpo.Any())
                    {
                        #region Cuerpo
                        using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream(), System.Text.Encoding.UTF8))
                        {
                            docText = sr.ReadToEnd();
                        }

                        foreach (var item in documento.EtiquetasCuerpo)
                        {
                            regexText = new Regex(item.Key);
                            docText = regexText.Replace(docText, item.Value ?? "");
                        }

                        using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create), System.Text.Encoding.UTF8))
                        {
                            sw.Write(docText);

                        }
                        #endregion
                    }

                    if (documento.EtiquetasEncabezado.Any())
                    {
                        #region Encabezado
                        foreach (var itemHeader in wordDoc.MainDocumentPart.HeaderParts)
                        {
                            using (StreamReader sr = new StreamReader(itemHeader.GetStream()))
                            {
                                docText = sr.ReadToEnd();
                            }

                            foreach (var item in documento.EtiquetasEncabezado)
                            {
                                regexText = new Regex(item.Key);
                                docText = regexText.Replace(docText, item.Value ?? "");
                            }

                            using (StreamWriter sw = new StreamWriter(itemHeader.GetStream(FileMode.Create)))
                            {
                                sw.Write(docText);
                            }

                        }
                        #endregion
                    }

                    if (documento.EtiquetasPiePagina.Any())
                    {
                        #region Pie de página
                        foreach (var itemFooter in wordDoc.MainDocumentPart.FooterParts)
                        {
                            using (StreamReader sr = new StreamReader(itemFooter.GetStream()))
                            {
                                docText = sr.ReadToEnd();
                            }

                            foreach (var item in documento.EtiquetasPiePagina)
                            {
                                regexText = new Regex(item.Key);
                                docText = regexText.Replace(docText, item.Value);
                            }

                            using (StreamWriter sw = new StreamWriter(itemFooter.GetStream(FileMode.Create)))
                            {
                                sw.Write(docText);
                            }
                        }
                        #endregion
                    }

                    wordDoc.Close();
                }
            }
            catch (Exception ex)
            {
                exito = false;
            }
            return exito;
        }

        public void ReemplazaElemento(Table tabla, string tag, string valor)
        {
            TableRow primerElemento = tabla.Descendants<TableRow>().First(q => q.InnerText.Contains(tag));
            TableCell cell = primerElemento.Elements<TableCell>().First(w => w.InnerText.Contains(tag));
            Paragraph paragraph = cell.Elements<Paragraph>().First(e => e.InnerText.Contains(tag));
            Run run = paragraph.Elements<Run>().First();
            Text texto = run.Elements<Text>().First();
            texto.Text = valor;
        }

        public void ReemplazaElementoParrafo(Table tabla, string tag, string valor)
        {
            Paragraph paragraph = tabla.Descendants<Paragraph>().First(e => e.InnerText.Contains(tag));
            Run run = paragraph.Elements<Run>().First();
            Text texto = run.Elements<Text>().First();
            texto.Text = valor;
        }

        public TableRow ReemplazaElementoCelda(Table tabla, string tag, string valor, string tag2, string valor2)
        {
            TableRow nuevoRow = new TableRow();
            TableRow primerPersonal = tabla.Descendants<TableRow>().First(a => a.InnerText.Contains(tag));
            nuevoRow = (TableRow)primerPersonal.CloneNode(true);
            TableCell cellFirst = nuevoRow.Elements<TableCell>().First(s => s.InnerText.Contains(tag));
            TableCell cellSecond = nuevoRow.Elements<TableCell>().First(d => d.InnerText.Contains(tag2));
            Paragraph paragraphFirst = cellFirst.Elements<Paragraph>().First(f => f.InnerText.Contains(tag));
            Paragraph paragraphSecond = cellSecond.Elements<Paragraph>().First(g => g.InnerText.Contains(tag2));
            Run runFirst = paragraphFirst.Elements<Run>().First(h => h.InnerText.Contains(tag));
            Run runSecond = paragraphSecond.Elements<Run>().First(j => j.InnerText.Contains(tag2));
            Text textoFirst = runFirst.Elements<Text>().First();
            Text textoSecond = runSecond.Elements<Text>().First();
            textoFirst.Text = "\u2022 " + valor + " -";
            textoSecond.Text = valor2;
            return nuevoRow;
        }

        public TableRow ReemplazaElementoRow(Table tabla, string tag, string valor)
        {
            TableRow nuevo = new TableRow();
            TableRow primerPersonal = tabla.Descendants<TableRow>().First(s => s.InnerText.Contains(tag));
            nuevo = (TableRow)primerPersonal.CloneNode(true);
            TableCell cell1 = nuevo.Elements<TableCell>().First(f => f.InnerText.Contains(tag));
            Paragraph paragraph1 = cell1.Elements<Paragraph>().First(h => h.InnerText.Contains(tag));
            Run run1 = paragraph1.Elements<Run>().First(e => e.InnerText.Contains(tag));
            Text texto1 = run1.Elements<Text>().First();
            texto1.Text = valor;
            return nuevo;

        }

        public Paragraph ReemplazaElementoRowParagraph(TableRow tabla, string tag, string valor)
        {
            TableRow nuevo = new TableRow();
            //TableRow primerPersonal = tabla.Descendants<TableRow>().First(s => s.InnerText.Contains(tag));
            nuevo = (TableRow)tabla.CloneNode(true);
            TableCell cell1 = nuevo.Elements<TableCell>().First(f => f.InnerText.Contains(tag));
            Paragraph paragraph1 = cell1.Elements<Paragraph>().First(h => h.InnerText.Contains(tag));

            Run run1 = paragraph1.Elements<Run>().First(e => e.InnerText.Contains(tag));
            Text texto1 = run1.Elements<Text>().First();
            texto1.Text = valor;
            return paragraph1;

        }

        public void CopiaAgregaElemento(Table tabla, string tag, string valor)
        {
            TableRow primerPersonal = tabla.Descendants<TableRow>().First(j => j.InnerText.Contains(tag));
            Body body = new Body();
            var rowCopia = primerPersonal.CloneNode(true);

            if (tag != "Estándar de Competencia:")
            {
                TableCell cell = rowCopia.Elements<TableCell>().First(n => n.InnerText.Contains(tag));
                Paragraph paragraph = cell.Elements<Paragraph>().First(g => g.InnerText.Contains(tag));
                Run run = paragraph.Elements<Run>().First();
                Text texto = run.Elements<Text>().First();
                texto.Text = valor;
            }

            body.Append(rowCopia);
            tabla.Append(body);
        }

        public static string EscapeXMLValue(string xmlString)
        {
            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("&", "&amp;");
        }

        public byte[] ArchivoABytes(string Url)
        {
            byte[] result = null;

            if (File.Exists(Url))
                result = File.ReadAllBytes(Url);

            return result;
        }

        private bool CopiarPlantilla(string URLPlantilla, string URLCopiaPlantilla)
        {
            bool exito = true;
            try
            {
                string[] ListDirectorios = (URLPlantilla + "," + URLCopiaPlantilla).Split(',');
                foreach (var Directorio in ListDirectorios)
                {
                    ValidaExisteDirectorio(Directorio);
                }

                using (var Plantilla = WordprocessingDocument.Open(URLPlantilla, false))
                using (var CopiaPlantilla = WordprocessingDocument.Create(URLCopiaPlantilla, WordprocessingDocumentType.Document))
                {
                    foreach (var part in Plantilla.Parts)
                        CopiaPlantilla.AddPart(part.OpenXmlPart, part.RelationshipId);
                }
            }
            catch (Exception ex)
            {
                exito = false;
            }
            return exito;
        }

        public bool GenerarQr(PlantillaDTO documento)
        {
            bool resultado = true;
            try
            {
                InsertCodeQR(documento);
                InsertIntoWord(documento);
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        private void InsertCodeQR(PlantillaDTO documento)
        {
            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            var qrCode = qrEncoder.Encode(documento.QRUrl);
            var qrImage = string.Format(@"{0}CodigoQR\qrcode{1}.png", documento.Path, documento.Id);
            var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            ValidaExisteDirectorio(qrImage);
            using (var stream = new FileStream(qrImage, FileMode.Create))
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
        }

        private void InsertIntoWord(PlantillaDTO documento)
        {
            try
            {
                using (WordprocessingDocument wordProcessingDocument = WordprocessingDocument.Open(documento.Url, true))
                {
                    //En la carpeta template del proyecto eliminar la imagen actual y agregar el tag sin ##
                    Text textPlaceHolder = wordProcessingDocument.MainDocumentPart.Document.Body.Descendants<Text>().Where((x) => x.Text == "etiquetacodigoqr").First();

                    if (textPlaceHolder == null)
                        Console.WriteLine("Text holder not found!");
                    else
                    {
                        var parent = textPlaceHolder.Parent;

                        if (!(parent is Run))
                            Console.Out.WriteLine("Parent is not run");
                        else
                        {
                            MainDocumentPart mainPart = wordProcessingDocument.MainDocumentPart;
                            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                            var qrImage = string.Format(@"{0}CodigoQR\qrcode{1}.png", documento.Path, documento.Id);
                            ValidaExisteDirectorio(qrImage);
                            using (FileStream stream = new FileStream(qrImage, FileMode.Open))
                            {
                                imagePart.FeedData(stream);
                            }

                            textPlaceHolder.Parent.InsertAfter(GetDrawing(wordProcessingDocument, mainPart.GetIdOfPart(imagePart)), textPlaceHolder);
                            textPlaceHolder.Remove();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Success");
        }

        private static Drawing GetDrawing(WordprocessingDocument wordDoc, string relationshipId)
        {
            return
             new Drawing(
                 new DW.Inline(
                     new DW.Extent() { Cx = 999999L, Cy = 999999L },
                     new DW.EffectExtent()
                     {
                         LeftEdge = 0L,
                         TopEdge = 0L,
                         RightEdge = 0L,
                         BottomEdge = 0L
                     },
                     new DW.DocProperties()
                     {
                         Id = (UInt32Value)1U,
                         Name = "Picture 1"
                     },
                     new DW.NonVisualGraphicFrameDrawingProperties(
                         new A.GraphicFrameLocks() { NoChangeAspect = true }),
                     new A.Graphic(
                         new A.GraphicData(
                             new PIC.Picture(
                                 new PIC.NonVisualPictureProperties(
                                     new PIC.NonVisualDrawingProperties()
                                     {
                                         Id = (UInt32Value)0U,
                                         Name = "New Bitmap Image.jpg"
                                     },
                                     new PIC.NonVisualPictureDrawingProperties()),
                                 new PIC.BlipFill(
                                     new A.Blip(
                                         new A.BlipExtensionList(
                                             new A.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" })
                                     )
                                     {
                                         Embed = relationshipId,
                                         CompressionState = A.BlipCompressionValues.Print
                                     },
                                     new A.Stretch(
                                         new A.FillRectangle())),
                                 new PIC.ShapeProperties(
                                     new A.Transform2D(
                                         new A.Offset() { X = 0L, Y = 0L },
                                         new A.Extents() { Cx = 999999L, Cy = 999999L }),
                                     new A.PresetGeometry(
                                         new A.AdjustValueList()
                                     )
                                     { Preset = A.ShapeTypeValues.Rectangle }))
                         )
                         { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                 )
                 {
                     DistanceFromTop = (UInt32Value)0U,
                     DistanceFromBottom = (UInt32Value)0U,
                     DistanceFromLeft = (UInt32Value)0U,
                     DistanceFromRight = (UInt32Value)0U,
                     EditId = "50D07946"
                 });
        }

        /// <summary>
        /// JJM 12/09/2019
        /// Valida si el directorio existe de lo contrario lo crea.
        /// </summary>
        /// <param name="Directorio"></param>
        public static void ValidaExisteDirectorio(string Directorio)
        {
            string ruta = Path.GetDirectoryName(Directorio);
            // Determina si el directorio ya existe.
            if (!Directory.Exists(ruta))
            {
                //si el directorio no existe lo crea 
                DirectoryInfo di = Directory.CreateDirectory(ruta);
            }
        }

        

    }
}
