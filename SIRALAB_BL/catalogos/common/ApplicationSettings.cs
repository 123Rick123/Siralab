using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SIRALAB_BL.common
{
    public class ApplicationSettings
    {
        public string MailStyleBody { get { return ConfigurationManager.AppSettings["MailStyleBody"]; } }
        public string MailImagePath { get { return ConfigurationManager.AppSettings["MailImagePath"]; } }
        public string MailImageFooterPath { get { return ConfigurationManager.AppSettings["MailImageFooterPath"]; } }
        public string MailImageHeaderSRC { get { return ConfigurationManager.AppSettings["MailImageHeaderSRC"]; } }
        public string MailImageFooterSRC { get { return ConfigurationManager.AppSettings["MailImageFooterSRC"]; } }
        public string MailImageStyle { get { return ConfigurationManager.AppSettings["MailImageStyle"]; } }
        public string MailImageFooterStyle { get { return ConfigurationManager.AppSettings["MailImageFooterStyle"]; } }
        public string MailMessageStyle { get { return ConfigurationManager.AppSettings["MailMessageStyle"]; } }
        public string MailFooterStyle { get { return ConfigurationManager.AppSettings["MailFooterStyle"]; } }
        public string MailFooterMessage { get { return ConfigurationManager.AppSettings["MailFooterMessage"]; } }
        public string MailHeaderDetailStyle { get { return ConfigurationManager.AppSettings["MailHeaderDetailStyle"]; } }
        public string MailDetailStyle { get { return ConfigurationManager.AppSettings["MailDetailStyle"]; } }
        public string smtpServer { get { return ConfigurationManager.AppSettings["smtpServer"]; } }
        public string smtpPort { get { return ConfigurationManager.AppSettings["smtpPort"]; } }
        public string smtpUser { get { return ConfigurationManager.AppSettings["smtpUser"]; } }
        public string smtpPass { get { return ConfigurationManager.AppSettings["smtpPass"]; } }
        public string smtpMail { get { return ConfigurationManager.AppSettings["smtpMail"]; } }
        public bool smtpSSL { get { return ConfigurationManager.AppSettings["smtpSSL"] == "true"; } }
        public bool useTestMail { get { return ConfigurationManager.AppSettings["useTestMail"] == "true"; } }
        public string[] testMail { get { return ConfigurationManager.AppSettings["testMail"].Split(','); } }

        public DateTime fechaEnvioAcontactos => Convert.ToDateTime(ConfigurationManager.AppSettings["fechaEnvioAcontactos"]);

        public DateTime fechaNuevosReportes => Convert.ToDateTime(ConfigurationManager.AppSettings["fechaNuevosReportes"]);

        public string CorreoTituloRecupera1 { get { return ConfigurationManager.AppSettings["CorreoTituloRecupera1"]; } }
        public string CorreoTituloRecupera2 { get { return ConfigurationManager.AppSettings["CorreoTituloRecupera2"]; } }
        public string CorreoTituloRecupera3 { get { return ConfigurationManager.AppSettings["CorreoTituloRecupera3"]; } }
        public string CorreoMensajeRecupera { get { return ConfigurationManager.AppSettings["CorreoMensajeRecupera"]; } }
        public string CorreoAsuntoRecupera { get { return ConfigurationManager.AppSettings["CorreoAsuntoRecupera"]; } }
        public string GoogleMapsKey { get { return ConfigurationManager.AppSettings["GoogleMapsKey"]; } }
        public string MenuXmlFile { get { return ConfigurationManager.AppSettings["MenuXmlFile"]; } }
        public string FileXMLExt { get { return ConfigurationManager.AppSettings["FileXMLExt"]; } }
        public string SessionVarLaboratorioArg { get { return ConfigurationManager.AppSettings["SessionVarLaboratorioArg"]; } }

        public string SistemaId { get { return ConfigurationManager.AppSettings["SistemaId"]; } }

        public string UsuarioRolIdRecaudacion { get { return ConfigurationManager.AppSettings["UsuarioRolIdRecaudacion"]; } }

        public string AcuseTexto { get { return ConfigurationManager.AppSettings["AcuseTexto"]; } }
        public string AcuseRegla { get { return ConfigurationManager.AppSettings["AcuseRegla"]; } }
        public string AcuseAnio { get { return ConfigurationManager.AppSettings["AcuseAnio"]; } }
        public int DiasInhabiles { get { return int.Parse(ConfigurationManager.AppSettings["DiasInhabiles"]); } }
        public int PaginationBlockSize { get { return int.Parse(ConfigurationManager.AppSettings["PaginationBlockSize"]); } }

        #region Archivos
        public string PathImagenesMIME { get { return ConfigurationManager.AppSettings["PathImagenesMIME"]; } }
        public string PathOrdenTrabajo { get { return ConfigurationManager.AppSettings["PathOrdenTrabajo"]; } }
        public string PathMuestra { get { return ConfigurationManager.AppSettings["PathMuestra"]; } }
        public string PathAnalisis { get { return ConfigurationManager.AppSettings["PathAnalisis"]; } }
        public string NombreCarpetaEvidencia { get { return ConfigurationManager.AppSettings["NombreCarpetaEvidencia"]; } }
        public string ImagenPreview { get { return ConfigurationManager.AppSettings["ImagenPreview"]; } }
        public string fileSizeMB { get { return ConfigurationManager.AppSettings["fileSizeMB"]; } }
        public string pathInformeLaboratorio { get { return ConfigurationManager.AppSettings["pathInformeLaboratorio"]; } }
        public string pathAcuseFirma
        {
            get { return ConfigurationManager.AppSettings["pathAcuseFirma"]; }
        }
        public string pathCSSReporte { get { return ConfigurationManager.AppSettings["pathCSSReporte"]; } }

        #endregion

        public string LeyendaFaltaPago { get { return ConfigurationManager.AppSettings["FaltaPago"]; } }
        public string ValidaPrograma { get { return ConfigurationManager.AppSettings["ValidaPrograma"]; } }
        public string NormaCOT { get { return ConfigurationManager.AppSettings["normaCOT"]; } }
        public string NormaFuenteAbastecimiento2021 { get { return ConfigurationManager.AppSettings["NormaFuenteAbastecimiento2021"]; } }
        public string LimiteNormaSSTDQO { get { return ConfigurationManager.AppSettings["limiteNormaSSTDQO"]; } }
        public string ipBaseNOM001 { get { return ConfigurationManager.AppSettings["ipBaseNOM001"]; } }

        #region API Reportes
        public string UrlApiReportes { get { return ConfigurationManager.AppSettings["urlApiReportes"]; } }
        public string UrlInformeLaboratorio { get { return ConfigurationManager.AppSettings["urlInformeLaboratorio"]; } }
        public string UrlAcuseFirma
        {
            get { return ConfigurationManager.AppSettings["urlAcuseFirma"]; }
        }
        #endregion
        public ApplicationSettings() { }
    }
}
