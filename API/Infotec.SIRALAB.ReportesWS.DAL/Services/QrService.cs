using System;
using System.Drawing;
using System.IO;
using QRCoder;


namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class QrService : IQrService
    {
        public string GenerarQR(string cadenaOriginal)
        {

            //TODO -> obtener últimos 12 carácteres:

            //TODO -> Obtener RFC

            //TODO -> Obtener URL
            const string url = "http://172.29.149.13/siralab/qa/siralabReportsApi/swagger/index.html";

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            return Convert.ToBase64String(BitMapToByte(qrCodeImage));
        }

        private byte[] BitMapToByte(Image imagen)
        {
            var stream = new MemoryStream();
            imagen.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}