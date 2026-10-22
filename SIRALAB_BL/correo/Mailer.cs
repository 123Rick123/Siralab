using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.IO;
using System.Net.Mime;
using System.Web;
using SIRALAB_BL.common;

namespace SIRALAB_BL.correo
{
    public class Mailer
    {
        public Mailer()
        {

        }

        #region Eventos para enviar Correos
        public static void SendMail(string To, string titulo1, string titulo2, string mensaje, string asunto, Hashtable AttachedFilePath)
        {
            ApplicationSettings ap = new ApplicationSettings();
            string c_Correo = ap.smtpMail;
            string user_N = ap.smtpUser;
            string p_Pass = ap.smtpPass;

            MailReciveReport mrr = new MailReciveReport();
            MailReciveReport.Detail mrrd1 = new MailReciveReport.Detail();
            mrrd1.Title = "";
            mrrd1.Data.Add(new MailReciveReport.DetailContent { mensaje = titulo1 + "<br><br>" + titulo2 + "<br><br>", titulo = "" });

            mrr.Details.Add(mrrd1);
            mrr.MessageLines = new string[] { "", "<center><b>Notificación Electrónica</b></center>", "<b>" + mensaje + "</b>" };

            MailMessage mail = new MailMessage();

            if (To == null) return;
            mail.To.Add(To);
            mail.From = new MailAddress(c_Correo);
            mail.Subject = asunto;
            mail.Body = string.Format(mrr.GetMessage(ap));
            mail.IsBodyHtml = true;

            if (AttachedFilePath != null)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");


                foreach (DictionaryEntry de in AttachedFilePath)
                {
                    if (de.Value is string)
                    {
                        Attachment imgAtt = new Attachment(de.Value.ToString());
                        imgAtt.ContentId = de.Key.ToString();
                        mail.Attachments.Add(imgAtt);
                        var path = "";

                        if (de.Key.ToString() == "enc_img_correo.png")
                        {
                            path = Path.Combine(HttpRuntime.AppDomainAppPath, ap.MailImagePath.Replace("~", "").Replace("/", "\\").Substring(1));
                        }

                        if (de.Key.ToString() == "Pie_Correo.png")
                        {
                            path = Path.Combine(HttpRuntime.AppDomainAppPath, ap.MailImageFooterPath.Replace("~", "").Replace("/", "\\").Substring(1));
                        }

                        if (!string.IsNullOrEmpty(path))
                        {
                            LinkedResource imagelink = new LinkedResource(path, "image/png");
                            imagelink.ContentId = de.Key.ToString();
                            imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                            htmlView.LinkedResources.Add(imagelink);
                        }
                        

                    }
                    else
                    {
                        Attachment imgAtt = new Attachment(new MemoryStream((byte[])de.Value), de.Key.ToString());
                        imgAtt.ContentId = de.Key.ToString();
                        mail.Attachments.Add(imgAtt);
                    }
                }

                mail.AlternateViews.Add(htmlView);

            }


            SmtpClient client = new SmtpClient(ap.smtpServer);
            client.EnableSsl = ap.smtpSSL; // false Para los que no tienen seguridad activada no poner EnableSsl como el de conagua, true para los quen sí como gmail.
            NetworkCredential credentials = new NetworkCredential(user_N, p_Pass);
            client.Credentials = credentials;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public class MailReciveReport
        {
            public List<Detail> Details;
            public string[] MessageLines { get; set; }

            public MailReciveReport()
            {
                Details = new List<Detail>();
            }

            public string GetMessage(ApplicationSettings smpt)
            {
                string tagHtml = "<div style=\"" + smpt.MailStyleBody + "\">" + Head.GetElement(smpt);
                tagHtml += "<div style=\"" + smpt.MailMessageStyle + "\">";

                tagHtml += Message.GetElement(this.MessageLines);

                foreach (var det in Details)
                {
                    tagHtml += det.GetElement(smpt);
                }
                tagHtml += Foot.GetElement(smpt);
                tagHtml += "</div></div>";
                return tagHtml;
            }

            private static class Head
            {
                public static string GetElement(ApplicationSettings smtp)
                {
                    string htmlTag = "<div><img src=\"" + smtp.MailImageHeaderSRC + "\" style=\"" + smtp.MailImageStyle + "\" ></div>";
                    return htmlTag;
                }
            }

            private static class Message
            {
                public static string GetElement(string[] message)
                {
                    string tagHtml = "";

                    foreach (var m in message)
                    {
                        tagHtml += m + "<br /><br />";
                    }

                    return tagHtml;
                }
            }

            public class Detail
            {
                public string Title { get; set; }
                private List<DetailContent> _Data;
                public List<DetailContent> Data { get { return _Data; } set { _Data = value; } }
                public Detail() { _Data = new List<DetailContent>(); }

                public string GetElement(ApplicationSettings smtp)
                {
                    string htmlTag = "<br /><span style=\"" + smtp.MailHeaderDetailStyle + "\">" + this.Title + "</span><br />";
                    htmlTag += "<span style=\"" + smtp.MailDetailStyle + "\">";
                    foreach (DetailContent item in this.Data)
                    {
                        if (item.titulo == "")
                            htmlTag += item.mensaje + "<br />";
                        else
                            htmlTag += item.titulo + ": " + item.mensaje + "<br />";
                    }
                    htmlTag += "</span>";
                    return htmlTag;
                }
            }

            private static class Foot
            {
                public static string GetElement(ApplicationSettings smtp)
                {
                    string htmlTag = "<div><img src=\"" + smtp.MailImageFooterSRC + "\" style=\"" + smtp.MailImageFooterStyle + "\" ></div>";
                    htmlTag += "<br><div style=\"" + smtp.MailFooterStyle + "\"><br /><br />" + smtp.MailFooterMessage + "</div>";
                    return htmlTag;
                }
            }

            public class DetailContent
            {
                public string titulo { get; set; }
                public string mensaje { get; set; }

                public DetailContent() { }
            }
        }
    }
}
