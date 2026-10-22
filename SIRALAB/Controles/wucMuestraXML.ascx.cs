using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using AjaxControlToolkit;
using SIRALAB.controles;
using SIRALAB.Entidades;
using SIRALAB_BL;
using SIRALAB_DAL;

namespace SIRALAB.Controles
{
    public partial class wucMuestraXML : UserControl
    {

        private string path
        {
            get { return Session["path_"].ToString(); }
            set { Session["path_"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)this.Page.Master.Master;
            main.MensajeDevolucion += Main_MensajeDevolucion;
        }

        private void Main_MensajeDevolucion(object sender, MensajeArgs e)
        {
            if (e.Command != null && e.Opcion == Opcion.Aceptar)
            {
                switch (e.Command)
                {
                    case "EnviarXML":
                        Muestra_BL muestraBl = new Muestra_BL();
                        var analisisID = (int)Session["AnalisisID_"];
                        var xml = (List<XMuestra>)Session["muestras_"];
                        var muestras = xml.Select(x => new Muestra
                        {
                            Caudal = x.Caudal,
                            Descripcion = x.Descripcion,
                            Numero = x.Numero,
                            Identificacion = x.Identificacion,
                            Fecha = x.Fecha,
                            FechaRecepcion = x.FechaRecepcion,
                            MuestraValors = x.MuestraValor.Select(y => new MuestraValor
                            {
                                ParametroMetodoID = y.ParametroMetodoID,
                                ParametroID = y.ParametroID,
                                LaboratorioID = y.LaboratorioID,
                                Exento = y.Exento,
                                NoAnalizado = y.NoAnalizado,
                                Valor = y.Valor,
                                LC_MC = y.LC_MC
                            }).ToList()
                        }).ToList();

                        var result = muestraBl.XMLActualizaMuestras(muestras, analisisID);
                        if (result)
                            Page.PopMensaje("Se han guardado los cambios de las muestras correctamente", "Carga Másiva de Muestras", Opciones.SoloAceptar, Icono.ok, null, null);
                        else
                            Page.PopMensaje("Ocurrio un error al guardar cambios de la muestra", "Carga Másiva de Muestras", Opciones.SoloAceptar, Icono.error, null, null);
                        break;
                }
            }
        }

        public static List<XMuestra> ObtenerMuestras(string path)
        {
            var muestras = new List<XMuestra>();
            try
            {
                string xmltext = File.ReadAllText(path);

                XmlSerializer serializer = new XmlSerializer(typeof(XMuestras), "http://www.conagua.gob.mx/siralab");
                using (TextReader reader = new StringReader(xmltext))
                {
                    //de esta manera se deserializa
                    XMuestras result = (XMuestras)serializer.Deserialize(reader);
                    muestras = result.Muestras.ToList();
                }
            }
            catch (Exception)
            {
                return new List<XMuestra>();
            }

            return muestras;
        }

        protected void DownLoad_Click(object sender, EventArgs e)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/xsd; charset=utf-8";
            response.AddHeader("Content-Disposition", "attachment; filename=muestra.xsd;");
            response.TransmitFile(Server.MapPath("muestra.xsd"));
            response.Flush();
            response.End();
        }

        protected void FileUploadControl_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            if (e.State == AsyncFileUploadState.Success && FileUploadControl.HasFile)
            {
                try
                {
                    XMuestras muestra = new XMuestras();
                    string filename = Path.GetFileName(FileUploadControl.FileName);
                    string msg = "";

                    if (filename.Contains("xml"))
                    {
                        this.path = Server.MapPath("~/") + filename;
                        Session["path_"] = Server.MapPath("~/") + filename;
                        FileUploadControl.SaveAs(path);
                        muestra.Muestras = ObtenerMuestras(path);
                        Session["muestras_"] = muestra.Muestras;
                    }
                    else
                    {
                        StatusLabel.Text = "XML invalido: El archivo debe ser con extensión xml o xsd.";
                        StatusLabel.BackColor = System.Drawing.Color.Red;
                        enviarXML.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "XML: El archivo no se pudo cargar. Ocurrió el siguiente error: " + ex.Message;
                    StatusLabel.BackColor = System.Drawing.Color.Red;
                    enviarXML.Visible = false;
                }
            }
        }

        protected void FileUploadControl_Click(object sender, EventArgs e)
        {
            this.path = Session["path_"].ToString();
            if (path != null || path != "")
            {
                try
                {
                    XMuestras muestra = new XMuestras();
                    string filename = path.Replace(Server.MapPath("~/"), "");
                    string msg = "";

                    if (filename.Contains("xml"))
                    {
                        muestra.Muestras = (List<XMuestra>)Session["muestras_"];
                        XmlSerializer ser = new XmlSerializer(typeof(XMuestras), "http://www.conagua.gob.mx/siralab");

                        using (var ms = new MemoryStream())
                        {
                            ser.Serialize(ms, muestra);
                            ms.Position = 0;
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(ms);
                            xmlDoc.Save(path);
                        }

                        MemoryStream memStream = new MemoryStream();
                        using (FileStream fileStream = File.OpenRead(path))
                        {
                            memStream.SetLength(fileStream.Length);
                            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                        }

                        XmlSchemaSet schemas = new XmlSchemaSet();
                        schemas.Add("http://www.conagua.gob.mx/siralab", Server.MapPath("~/") + "muestra.xsd");

                        XDocument doc = XDocument.Load(path);
                        doc.Validate(schemas, (o, ex) =>
                        {
                            msg += ex.Message + Environment.NewLine;
                        });
                        StatusLabel.Text = msg == "" ? "XML válido" : "XML invalido: " + msg;
                        StatusLabel.BackColor = msg == "" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        StatusLabel.Visible = true;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newfile"
                            , "window.parent.$find('" + FileUploadControl.ClientID + "').newFileName='" + filename + "';", true);
                        FileUploadControl.BackColor = msg == "" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        enviarXML.Visible = msg == "";
                    }
                    else
                    {
                        StatusLabel.Text = "XML invalido: El archivo debe ser con extensión xml o xsd.";
                        StatusLabel.BackColor = System.Drawing.Color.Red;
                        enviarXML.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "XML: El archivo no se pudo cargar. Ocurrió el siguiente error: " + ex.Message;
                    StatusLabel.BackColor = System.Drawing.Color.Red;
                    enviarXML.Visible = false;
                }
            }
        }

        protected void EnviarXML_Click(object sender, EventArgs e)
        {
            Page.PopMensaje("¿Se encuentra seguro de enviar el xml para guardar cambios de las muestras?", "Carga Másiva de Muestras", Opciones.SiNo, Icono.question, "EnviarXML");
        }
    }
}