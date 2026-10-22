using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;
using Org.BouncyCastle.X509;

namespace SIRALAB.FIEL
{
    /// <summary>
    /// Clase que contiene las propiedades y metodos para firmar electronicamente 
    /// </summary>
    public class Contenedor
    {
        private string _PrimaryKey { get; set; }
        private string _CadenaAFirmar { get; set; }
        private string _CadenaCMS { get; set; }
        private Exception _AppletError { get; set; }
        private wsIDTrust.ValidaFirmaResponseType _RespuestaOperacionValida { get; set; }
        private bool _Valid { get; set; }
        private string _FirmaDigital { get; set; }
        private string _CetificadoDigital { get; set; }
        private string _PametroExtra { get; set; }
        private string[] _ParamArray { get; set; }

        public byte[] ArchivoKey{ get; set; }       
        public byte[] ArchivoCer{ get; set; }
        public string txtContra { get; set; }
        public string txtRFC { get; set; }
               
        private string _SelloCNA { get; set; }

        /// <summary>
        /// Propiedad que obtine la firma digital del contenedor CMS (cadenaCMS)
        /// </summary>
        public string firmaDigital
        {
            get
            {
                if (_Valid && _CadenaCMS != null && _CadenaCMS != "")
                {
                    string envelopedData = _CadenaCMS;                    
                    System.IO.Stream PrivateKey = new System.IO.MemoryStream(ArchivoKey);
                    System.IO.Stream PublicKey = new System.IO.MemoryStream(ArchivoCer);
                    string PasswordC = txtContra;
                    string RFCContribuyente = txtRFC;                    
                    CONAGUA.Seguridad.FIEL FielValidar = new CONAGUA.Seguridad.FIEL(PrivateKey, PublicKey, PasswordC, RFCContribuyente);
                    if (FielValidar.Login())
                    {
                        byte[] Firmas = FielValidar.firma(_CadenaCMS, System.Text.Encoding.UTF8);
                        _FirmaDigital = CommonFirma.ToHexString(Firmas).ToUpper();
                    }
                    return _FirmaDigital;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Propiedad que obtiene el certificado digital con que se firmo
        /// </summary>
        public string cetificadoDigital
        {
            get
            {
                if (_Valid && _CadenaCMS != null && _CadenaCMS != "")
                {

                    string envelopedData = _CadenaCMS;
                    System.IO.Stream PrivateKey = new System.IO.MemoryStream(ArchivoKey);
                    System.IO.Stream PublicKey = new System.IO.MemoryStream(ArchivoCer);
                    string PasswordC = txtContra;
                    string RFCContribuyente = txtRFC;
                    CONAGUA.Seguridad.FIEL FielValidar = new CONAGUA.Seguridad.FIEL(PrivateKey, PublicKey, PasswordC, RFCContribuyente);
                    if (FielValidar.Login())
                    {
                        try
                        {
                            _CetificadoDigital = FielValidar.Cert.ToString();
                        }
                        catch
                        {
                            _CetificadoDigital = null; 
                        }
                        
                    }
                }
                return _CetificadoDigital;
            }
        }


        /// <summary>
        /// Inicializa los valores a null
        /// </summary>
        public Contenedor()
        {
            primaryKey = null;
            cadenaAFirmar = null;
            cadenaCMS = null;
        }

        /// <summary>
        /// Instancia un Contenedor con su llave primaria y su cadena a Firmar
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="cadena"></param>
        public Contenedor(string pk, string cadena)
        {
            primaryKey = pk;
            cadenaAFirmar = cadena;
            cadenaCMS = null;
        }

        /// <summary>
        /// Instancia un Contenedor con su llave primaria, cadena a firmar y un parametro extra
        /// </summary>
        /// <param name="pk">llave que identificara el contenedor</param>
        /// <param name="cadena">informacion que se va a firmar</param>
        /// <param name="parametro">parametro extra string</param>
        public Contenedor(string pk, string cadena, string parametro)
        {
            primaryKey = pk;
            cadenaAFirmar = cadena;
            cadenaCMS = null;
            parametroExtra = parametro;
        }


        /// <summary>
        /// Array de string para uso multiple
        /// </summary>
        public string[] paramArray
        {
            get { return _ParamArray; }
            set { _ParamArray = value; }
        }


        /// <summary>
        /// Llave que identificara a contenedor e informacion a firmar
        /// </summary>
        public string primaryKey
        {
            get
            {
                return _PrimaryKey;
            }
            set { _PrimaryKey = value; }

        }

        /// <summary>
        /// String con la informacion a firmar
        /// </summary>
        public string cadenaAFirmar
        {
            get { return _CadenaAFirmar; }
            set { _CadenaAFirmar = value; }
        }

        /// <summary>
        /// Obtiene un la propiedad "cadenaAFirmar" en Base 64 y UTF8
        /// </summary>
        public string cadenaAFirmarBase64
        {
            get
            {
                return CommonFirma.ToUTF8Base64String(cadenaAFirmar);
            }
        }

        /// <summary>
        /// string con el contenedor CMS o PKCS7
        /// </summary>
        public string cadenaCMS
        {
            get { return _CadenaCMS; }
            set { _CadenaCMS = value; }
        }

        /// <summary>
        /// Si hubo algun error al hacer la peticion de validarFirma regresa la Excepcion
        /// </summary>
        public Exception wsException
        {
            get { return _AppletError; }
            set { _AppletError = value; }
        }

        /// <summary>
        /// Cuando se hace el get a esta propiedad realiza una peticion al webservices de IDTrust para validar la firma
        /// </summary>
        public bool valido
        {
            get
            {
                _Valid = validaFima();
                return _Valid;
            }
            set { _Valid = value; }
        }

        /// <summary>
        /// Representa el codigo de error de la operacion ValidaFirma. 0 si no hubo error
        /// </summary>
        public int codigoError
        {
            get
            {
               if (_RespuestaOperacionValida != null)
                   return _RespuestaOperacionValida.resultado.codigo;
               else
                    return 0;
            }
        }

        /// <summary>
        /// Mensaje de error de la operacion ValidaFirma, solo si "codigoError" diferente a 0
        /// </summary>
        public string mensajeError
        {
            get
            {
                if (_RespuestaOperacionValida != null)
                    return _RespuestaOperacionValida.resultado.texto;
                else
                    return null;
            }
        }

        /// <summary>
        /// Prametro string extra para cualquier otra operacion
        /// </summary>
        public string parametroExtra
        {
            get { return _PametroExtra; }
            set { _PametroExtra = value; }
        }


        /// <summary>
        /// Realiza una peticion al web services de IDTrust para validar la firma electronica 
        /// </summary>
        /// <param name="contenido">Contenedor a validar; con los datos a firmar y la cadena cms</param>
        /// <returns>"true" si la firma es valida "false" de lo contrario</returns>
        private bool validaFima()
        {           
            try
            {
                System.IO.Stream PrivateKey = new System.IO.MemoryStream(ArchivoCer);
              
                System.IO.Stream PublicKey = new System.IO.MemoryStream(ArchivoKey,0,ArchivoKey.Length);
                string PasswordC = txtContra;
                string RFCContribuyente = txtRFC;
                CONAGUA.Seguridad.FIEL FielValidar = new CONAGUA.Seguridad.FIEL(PublicKey, PrivateKey, PasswordC, RFCContribuyente);
               
                if (FielValidar.Login())
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.wsException = ex;
                this._RespuestaOperacionValida = new wsIDTrust.ValidaFirmaResponseType();
                this._RespuestaOperacionValida.resultado = new wsIDTrust.ResultadoType();
                this._RespuestaOperacionValida.resultado.codigo = ex.HResult;
                this._RespuestaOperacionValida.resultado.texto = ex.Message;
                return false;
            }
            finally
            {
                
            }
        }      
    }


    public  class DatosFirma_Args : EventArgs {
        public FirmaElectronica DatosAFirmar { get; set; }
        public string RFCFirma { get; set; }
        public byte[] ArchivoKey { get; set; }
        public MemoryStream ArchivoKeyMS { get; set; }

        public byte[] ArchivoCer { get; set; }
        public string txtContra { get; set; }
    }
}