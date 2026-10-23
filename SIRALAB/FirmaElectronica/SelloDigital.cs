using System;

namespace SIRALAB.FIEL
{
    /// <summary>
    /// Clase que contiene las propiedades y metodos para obtener un sello digital
    /// </summary>
    public class SelloDigital
    {
        private string _CadenaOriginal { get; set; }
        private string _IdCertificado { get; set; }
        private string _SerieCertificado { get; set; }
        private string _AlgoritmoHash { get; set; }
        private string _RFCCertificado { get; set; }
        private string _SelloDigital { get; set; }
        private wsIDTrust.FirmaResponseType _RepuestaOperacionFirma { get; set; }
        private Exception _ErroExecption { get; set; }

        /// <summary>
        /// String con la caldena original que se va a firma
        /// </summary>
        public string cadenaOriginal { get { return _CadenaOriginal; } set { _CadenaOriginal = value; } }

        /// <summary>
        /// Identificador del certificado de la autoridad que sellara
        /// </summary>
        public string idCertificado { get { return _IdCertificado; } set { _IdCertificado = value; } }

        /// <summary>
        /// Numero de serie del critificado de la autoridad que sellara
        /// </summary>
        public string serieCertificado { get { return _SerieCertificado; } set { _SerieCertificado = value; } }

        /// <summary>
        /// Algoritmo de digestion con el cual se firmara
        /// </summary>
        public string algoritmoHash { get { return _AlgoritmoHash; } set { _AlgoritmoHash = value; } }

        /// <summary>
        /// RFC del dueño del certificado
        /// </summary>
        public string rfcCertificado { get { return _RFCCertificado; } set { _RFCCertificado = value; } }

        /// <summary>
        /// Se llena cuan exista alguna excepcion en el proceso de firma
        /// </summary>
        public Exception erroExecption { get { return _ErroExecption; } set { _ErroExecption = value; } }

        /// <summary>
        /// Respuesta del webservices de la operacion firma
        /// </summary>
        private wsIDTrust.FirmaResponseType repuestaOperacionFirma { get { return _RepuestaOperacionFirma; } set { _RepuestaOperacionFirma = value; } }
        
        /// <summary>
        /// Codigo de error de la firma.  0 (Cero) en caso de ser exitosa -1 en caso de ser null
        /// </summary>
        public int codigoError
        {
            get
            {
                if (_RepuestaOperacionFirma != null)
                    return _RepuestaOperacionFirma.resultado.codigo;
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Mensaje de error de la firma en caso que "codigoError" sera diferente de 0 (Cero)
        /// </summary>
        public string mensajeError
        {
            get
            {
                if (_RepuestaOperacionFirma != null)
                    return _RepuestaOperacionFirma.resultado.texto;
                {
                    return "No hubo respuesta del servidor.";
                }
            }

        }

        /// <summary>
        /// Sello digital resultado de firmar la cadena original
        /// </summary>
        public string sello
        {
            get
            {
                if (codigoError == 0)
                    return _RepuestaOperacionFirma.firma;
                else
                    return string.Empty;
            }
        }
        
        /// <summary>
        /// Inicializa la claser y toma los valores por default del certificado de la AC,
        /// estos valores se obtienen del webconfig por medio de la case "Constantes"
        /// </summary>
        public SelloDigital()
        {
            cadenaOriginal = null;
            idCertificado = Constantes.IdCertificado;
            serieCertificado = Constantes.SerieCertificado;
            algoritmoHash = Constantes.AlgoritmoHash;
            rfcCertificado = Constantes.RfcCertificado;
            _SelloDigital = null;
        }

        public void SellarCadenaOriginal()
        {
     
            wsIDTrust.GlobalTrustPortTypeClient webService = new wsIDTrust.GlobalTrustPortTypeClient();
            wsIDTrust.FirmaRequestType selloRequest = new wsIDTrust.FirmaRequestType();
            try
            {
                selloRequest.id = this.idCertificado;
                selloRequest.numeroSerie = this.serieCertificado;
                selloRequest.cadenaOriginal = CommonFirma.UTF8Bytes(this.cadenaOriginal);
                selloRequest.algHash = this.algoritmoHash;
                selloRequest.rfc = this.rfcCertificado;

                this.repuestaOperacionFirma = webService.Firma(selloRequest);
            }
            catch (Exception ex)
            {
                this.erroExecption = ex;
                this.repuestaOperacionFirma = new wsIDTrust.FirmaResponseType();
                this.repuestaOperacionFirma.resultado = new wsIDTrust.ResultadoType();
                this.repuestaOperacionFirma.resultado.codigo = ex.HResult;
                this.repuestaOperacionFirma.resultado.texto = ex.Message;
            }
            finally
            {
                webService.Close();
            }
        }

        public string SellarCadenaOriginalSirfic()
        {
            SelladoSirficWS.ServicioSelladoSoapClient Prueba = new SelladoSirficWS.ServicioSelladoSoapClient();
            try
            {
                Prueba.Open();
                string CadenaSello = Prueba.Generar(this.cadenaOriginal);
                return CadenaSello;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Prueba.Close();
            }
        }

        /// <summary>
        /// Verifica que la selladora de CONAGUA (SIRFIC) esté disponible antes de iniciar
        /// un proceso de firma. Hace una llamada ligera a ObtenerNumeroSerie; si no responde
        /// o lanza excepción, se considera que el servicio no está disponible.
        /// </summary>
        /// <returns>true si la selladora respondió correctamente; false en caso contrario.</returns>
        public static bool SelladoraDisponible()
        {
            SelladoSirficWS.ServicioSelladoSoapClient cliente =
                new SelladoSirficWS.ServicioSelladoSoapClient();
            try
            {
                cliente.Open();
                string serie = cliente.ObtenerNumeroSerie();
                return !string.IsNullOrWhiteSpace(serie);
            }
            catch
            {
                return false;
            }
            finally
            {
                try { cliente.Close(); } catch { cliente.Abort(); }
            }
        }

    }
}