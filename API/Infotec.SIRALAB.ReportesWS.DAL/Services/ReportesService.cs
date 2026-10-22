using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.StyledXmlParser.Css.Media;
using Microsoft.Extensions.Configuration;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class ReportesService : IReportesService
    {

        private readonly IConfiguration _config;
        public ReportesService(IConfiguration config)
        {
            _config = config;
        }

        //Método genérico para crear el pdf:
        public byte[] CreatePDF(string html)
        {

            try
            {
                ConverterProperties properties = new ConverterProperties();

                var stream = new MemoryStream();


                using (var pdfWriter = new PdfWriter(stream))
                {
                    var pdfDocument = new PdfDocument(pdfWriter);
                    pdfDocument.SetTagged();
                    pdfDocument.SetDefaultPageSize(PageSize.LETTER);
                    var mediaDeviceDescription = new MediaDeviceDescription(MediaType.PRINT);
                    properties.SetMediaDeviceDescription(mediaDeviceDescription);
                    properties.SetCharset("UTF-8");

                    FontProvider fontProvider = new DefaultFontProvider(false, false, false);
                    var pathFuentes = _config.GetSection("Configuraciones").GetSection("pathFuente").Value;
                    fontProvider.AddDirectory(pathFuentes);
                    properties.SetFontProvider(fontProvider);
                    HtmlConverter.ConvertToPdf(html, pdfDocument, properties);
                }

                return stream.ToArray();
            }
            catch (Exception e)
            {
                return new MemoryStream().ToArray();
            }
        }
    }
}