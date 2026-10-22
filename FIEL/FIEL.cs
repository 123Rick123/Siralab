// Decompiled with JetBrains decompiler
// Type: CONAGUA.Seguridad.FIEL
// Assembly: FIEL, Version=1.0.5714.20960, Culture=neutral, PublicKeyToken=null
// MVID: E006EA5F-2200-4D6C-8D8C-F12E9D433420
// Assembly location: C:\CNA_TFS_DESARROLLO\Fabrica\SIRALAB\SIRALAB\bin\FIEL.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Pkcs;

namespace CONAGUA.Seguridad
{
    /// <summary>
    /// Clase que permite vierficar la FIEL con sus cuatro componentes para autentificarse.
    /// Implementa firma electrónica con la llave privada.
    /// TODO: Verificar que una firma haya sido emitida por la FIEL o que esté integra.
    /// </summary>
    public class FIEL
    {
        private SecureString _pass;
        private SecureString _rfc;
        private RSACryptoServiceProvider _privateKey;
        private int _privateKeySize;
        /// <summary>
        /// Certificado público, expuesto para obtener las características públicas de la FIEL
        /// </summary>
        public X509Certificate2 Cert;
        private RSACryptoServiceProvider _publicKey;
        private int _publicKeySize;

        /// <summary>
        /// Constructor.
        /// Inicializa las llaves públicas y privadas de la FIEL
        /// </summary>
        /// <param name="privateKey">Stream de la llave privada (archivo key)</param>
        /// <param name="publicKey">Stream de la llave pública (archivo cer)</param>
        /// <param name="password">Contraseña de la llave privada</param>
        /// <param name="rfc">RFC a quien se le emitió la FIEL</param>
        public FIEL(Stream privateKey, Stream publicKey, string password, string rfc)
        {
            this._pass = FIEL.StringToSecureString(password);
            this._rfc = FIEL.StringToSecureString(rfc);
            try
            {
                this._privateKey = FIEL.decodeKey(privateKey, this.convertToUNSecureString(this._pass).ToCharArray());


            }
            catch (Exception ex)
            {
                throw new Exception("Contraseña incorrecta");
            }

            this._privateKeySize = this._privateKey.KeySize;

            try
            {
                byte[] array;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    publicKey.Position = 0L;
                    publicKey.CopyTo((Stream)memoryStream);
                    array = memoryStream.ToArray();
                }
                this.Cert = new X509Certificate2(array);
            }
            catch
            {
                throw;
            }
            try
            {
                this._publicKey = FIEL.decodeCERT(publicKey);
            }
            catch
            {
                throw new Exception("Error en el certificado");
            }
            this._publicKeySize = this._publicKey.KeySize;
        }

           /// <summary>
        /// Verifica la correspondencia de los cuatro componentes de FIEL para autentificarse, además verifica que esté dentro de las fechas de vigencia
        /// </summary>
        /// <returns>Verdadero sí cumple con todas las condiciones.</returns>
        public bool Login()
        {
            bool flag = false;
            DateTime notBefore = this.Cert.NotBefore;
            DateTime notAfter = this.Cert.NotAfter;
            DateTime now = DateTime.Now;
            if (!(notBefore <= now) || !(notAfter >= now))
                throw new Exception("Certificado fuera de vigencia");
            flag = true;
            string unSecureString = this.convertToUNSecureString(this._rfc);
            string[] strArray = this.Cert.Subject.Split(',');
            string str = strArray[1].Substring(strArray[1].LastIndexOf("=", StringComparison.Ordinal) + 1).Split('/')[0].Trim();
            if (!unSecureString.Equals(str, StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("RFC no pertenece al certificado");
            flag = true;
            byte[] signature = new byte[0];
            byte[] bytes = Encoding.UTF8.GetBytes(unSecureString);
            switch (this._privateKeySize)
            {
                case 1024:
                    signature = this._privateKey.SignData(bytes, (object)new SHA1CryptoServiceProvider());
                    break;
                case 2048:
                    signature = this._privateKey.SignData(bytes, (object)new SHA256CryptoServiceProvider());
                    break;
            }
            object halg = new object();
            switch (this._publicKeySize)
            {
                case 1024:
                    halg = (object)new SHA1CryptoServiceProvider();
                    break;
                case 2048:
                    halg = (object)new SHA256CryptoServiceProvider();
                    break;
            }
            if (this._publicKey.VerifyData(bytes, halg, signature))
                return true;
            throw new Exception("Certificado no corresponde a la llave privada");
        }

        /// <summary>Firma un string con la llave privada</summary>
        /// <param name="cadena">Texto a firmar</param>
        /// <param name="codificacion">Formato de codificaión del texto</param>
        /// <returns>Arreglo de bytes de la firma</returns>
        public byte[] firma(string cadena, Encoding codificacion)
        {
            byte[] bytes = codificacion.GetBytes(cadena);
            byte[] numArray = new byte[0];
            switch (this._privateKeySize)
            {
                case 1024:
                    numArray = this._privateKey.SignData(bytes, (object)new SHA1CryptoServiceProvider());
                    break;
                case 2048:
                    numArray = this._privateKey.SignData(bytes, (object)new SHA256CryptoServiceProvider());
                    break;
            }
            return numArray;
        }

        /// <summary>Firma un arreglo de bytes con la llave privada</summary>
        /// <param name="bytes">Bytes a firmar</param>
        /// <returns>Arreglo de bytes de la firma</returns>
        public byte[] firma(byte[] bytes)
        {
            byte[] buffer = bytes;
            byte[] numArray = new byte[0];
            switch (this._privateKeySize)
            {
                case 1024:
                    numArray = this._privateKey.SignData(buffer, (object)new SHA1CryptoServiceProvider());
                    break;
                case 2048:
                    numArray = this._privateKey.SignData(buffer, (object)new SHA256CryptoServiceProvider());
                    break;
            }
            return numArray;
        }

        /// <summary>Firma un string con la llave privada</summary>
        /// <param name="cadena">Texto a firmar</param>
        /// <param name="codificacion">Formato de codificación del texto</param>
        /// <returns>Cadena en Base64 de la firma</returns>
        public string firmastrBase64(string cadena, Encoding codificacion)
        {
            byte[] bytes = codificacion.GetBytes(cadena);
            byte[] inArray = new byte[0];
            switch (this._privateKeySize)
            {
                case 1024:
                    inArray = this._privateKey.SignData(bytes, (object)new SHA1CryptoServiceProvider());
                    break;
                case 2048:
                    inArray = this._privateKey.SignData(bytes, (object)new SHA256CryptoServiceProvider());
                    break;
            }
            return Convert.ToBase64String(inArray);
        }

        /// <summary>Firma un arreglo de bytes con la llave privada</summary>
        /// <param name="bytes">Texto a firmar</param>
        /// <returns>Cadena en Base64 de la firma</returns>
        public string firmastrBase64(byte[] bytes)
        {
            byte[] buffer = bytes;
            byte[] inArray = new byte[0];
            switch (this._privateKeySize)
            {
                case 1024:
                    inArray = this._privateKey.SignData(buffer, (object)new SHA1CryptoServiceProvider());
                    break;
                case 2048:
                    inArray = this._privateKey.SignData(buffer, (object)new SHA256CryptoServiceProvider());
                    break;
            }
            return Convert.ToBase64String(inArray);
        }

        #region decode VH =>

        public static RSACryptoServiceProvider decodeKey(Stream privateKey, char[] password)
        {
           

            RSAParameters rsaParametros;

            try
            {
                Asn1InputStream ais = new Asn1InputStream(privateKey);
                Asn1Object ao = ais.ReadObject();
                EncryptedPrivateKeyInfo epki = EncryptedPrivateKeyInfo.GetInstance(ao);

                PrivateKeyInfo pki = PrivateKeyInfoFactory.CreatePrivateKeyInfo(password, epki);
                RsaPrivateKeyStructure rsapks = new RsaPrivateKeyStructure((Asn1Sequence)pki.PrivateKey);


                RsaPrivateCrtKeyParameters rpckp = new RsaPrivateCrtKeyParameters(rsapks.Modulus, rsapks.PublicExponent,
                    rsapks.PrivateExponent, rsapks.Prime1, rsapks.Prime2, rsapks.Exponent1, rsapks.Exponent2, rsapks.Coefficient);
                rsaParametros = DotNetUtilities.ToRSAParameters(rpckp);
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.InnerException);
                throw;
            }

            

            //RSAParameters rsaParameters;
            //try
            //{
            //    rsaParameters = DotNetUtilities.ToRSAParameters(PrivateKeyFactory.DecryptKey(password, privateKey) as RsaPrivateCrtKeyParameters);
            //}
            //catch
            //{
            //    throw;
            //}
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();


            RSAParameters parameters = new RSAParameters()
            {
                Modulus = rsaParametros.Modulus,
                Exponent = rsaParametros.Exponent,
                D = rsaParametros.D,
                P = rsaParametros.P,
                Q = rsaParametros.Q,
                DP = rsaParametros.DP,
                DQ = rsaParametros.DQ,
                InverseQ = rsaParametros.InverseQ
            };

            #region calculos

            byte[] numArray1 = new byte[parameters.D.Length];
            if ((double)parameters.D.Length / 128.0 < 1.0)
                numArray1 = new byte[128];
            else if ((double)parameters.D.Length / 128.0 > 1.0)
                numArray1 = new byte[256];
            parameters.D.CopyTo((Array)numArray1, numArray1.Length - parameters.D.Length);
            parameters.D = numArray1;
            byte[] numArray2 = new byte[parameters.DP.Length];
            if ((double)parameters.DP.Length / 64.0 < 1.0)
                numArray2 = new byte[64];
            else if ((double)parameters.DP.Length / 64.0 > 1.0)
                numArray2 = new byte[128];
            parameters.DP.CopyTo((Array)numArray2, numArray2.Length - parameters.DP.Length);
            parameters.DP = numArray2;
            byte[] numArray3 = new byte[parameters.DQ.Length];
            if ((double)parameters.DQ.Length / 64.0 < 1.0)
                numArray3 = new byte[64];
            else if ((double)parameters.DQ.Length / 64.0 > 1.0)
                numArray3 = new byte[128];
            parameters.DQ.CopyTo((Array)numArray3, numArray3.Length - parameters.DQ.Length);
            parameters.DQ = numArray3;
            byte[] numArray4 = new byte[parameters.InverseQ.Length];
            if ((double)parameters.InverseQ.Length / 64.0 < 1.0)
                numArray4 = new byte[64];
            else if ((double)parameters.InverseQ.Length / 64.0 > 1.0)
                numArray4 = new byte[128];
            parameters.InverseQ.CopyTo((Array)numArray4, numArray4.Length - parameters.InverseQ.Length);
            parameters.InverseQ = numArray4;
            byte[] numArray5 = new byte[parameters.Modulus.Length];
            if ((double)parameters.Modulus.Length / 128.0 < 1.0)
                numArray5 = new byte[128];
            else if ((double)parameters.Modulus.Length / 128.0 > 1.0)
                numArray5 = new byte[256];
            parameters.Modulus.CopyTo((Array)numArray5, numArray5.Length - parameters.Modulus.Length);
            parameters.Modulus = numArray5;
            byte[] numArray6 = new byte[parameters.P.Length];
            if ((double)parameters.P.Length / 64.0 < 1.0)
                numArray6 = new byte[64];
            else if ((double)parameters.P.Length / 64.0 > 1.0)
                numArray6 = new byte[128];
            parameters.P.CopyTo((Array)numArray6, numArray6.Length - parameters.P.Length);
            parameters.P = numArray6;
            byte[] numArray7 = new byte[parameters.Q.Length];
            if ((double)parameters.Q.Length / 64.0 < 1.0)
                numArray7 = new byte[64];
            else if ((double)parameters.Q.Length / 64.0 > 1.0)
                numArray7 = new byte[128];
            parameters.Q.CopyTo((Array)numArray7, numArray7.Length - parameters.Q.Length);
            parameters.Q = numArray7;
            #endregion
            try
            {
                cryptoServiceProvider.ImportParameters(parameters);
            }
            catch
            {
                throw;
            }
            return cryptoServiceProvider;
        }

        #endregion


        #region RSACryptoServiceProvider original.

        /// <summary>Desencripta la llave privada de un archivo .key</summary>
        /// <param name="privateKey">Stream del archivo .key</param>
        /// <param name="password">Contraseña de la llave privada</param>
        /// <returns>Llave privada de la FIEL</returns>
        //public static RSACryptoServiceProvider decodeKey(Stream privateKey, char[] password)
        //{
        //    RSAParameters rsaParameters;
        //    try
        //    {
        //        rsaParameters = DotNetUtilities.ToRSAParameters(PrivateKeyFactory.DecryptKey(password, privateKey) as RsaPrivateCrtKeyParameters);
        //    }
        //    catch (Exception exception)
        //    {
        //        throw;
        //    }
        //    RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
        //    RSAParameters parameters = new RSAParameters()
        //    {
        //        Modulus = rsaParameters.Modulus,
        //        Exponent = rsaParameters.Exponent,
        //        D = rsaParameters.D,
        //        P = rsaParameters.P,
        //        Q = rsaParameters.Q,
        //        DP = rsaParameters.DP,
        //        DQ = rsaParameters.DQ,
        //        InverseQ = rsaParameters.InverseQ
        //    };
        //    byte[] numArray1 = new byte[parameters.D.Length];
        //    if ((double)parameters.D.Length / 128.0 < 1.0)
        //        numArray1 = new byte[128];
        //    else if ((double)parameters.D.Length / 128.0 > 1.0)
        //        numArray1 = new byte[256];
        //    parameters.D.CopyTo((Array)numArray1, numArray1.Length - parameters.D.Length);
        //    parameters.D = numArray1;
        //    byte[] numArray2 = new byte[parameters.DP.Length];
        //    if ((double)parameters.DP.Length / 64.0 < 1.0)
        //        numArray2 = new byte[64];
        //    else if ((double)parameters.DP.Length / 64.0 > 1.0)
        //        numArray2 = new byte[128];
        //    parameters.DP.CopyTo((Array)numArray2, numArray2.Length - parameters.DP.Length);
        //    parameters.DP = numArray2;
        //    byte[] numArray3 = new byte[parameters.DQ.Length];
        //    if ((double)parameters.DQ.Length / 64.0 < 1.0)
        //        numArray3 = new byte[64];
        //    else if ((double)parameters.DQ.Length / 64.0 > 1.0)
        //        numArray3 = new byte[128];
        //    parameters.DQ.CopyTo((Array)numArray3, numArray3.Length - parameters.DQ.Length);
        //    parameters.DQ = numArray3;
        //    byte[] numArray4 = new byte[parameters.InverseQ.Length];
        //    if ((double)parameters.InverseQ.Length / 64.0 < 1.0)
        //        numArray4 = new byte[64];
        //    else if ((double)parameters.InverseQ.Length / 64.0 > 1.0)
        //        numArray4 = new byte[128];
        //    parameters.InverseQ.CopyTo((Array)numArray4, numArray4.Length - parameters.InverseQ.Length);
        //    parameters.InverseQ = numArray4;
        //    byte[] numArray5 = new byte[parameters.Modulus.Length];
        //    if ((double)parameters.Modulus.Length / 128.0 < 1.0)
        //        numArray5 = new byte[128];
        //    else if ((double)parameters.Modulus.Length / 128.0 > 1.0)
        //        numArray5 = new byte[256];
        //    parameters.Modulus.CopyTo((Array)numArray5, numArray5.Length - parameters.Modulus.Length);
        //    parameters.Modulus = numArray5;
        //    byte[] numArray6 = new byte[parameters.P.Length];
        //    if ((double)parameters.P.Length / 64.0 < 1.0)
        //        numArray6 = new byte[64];
        //    else if ((double)parameters.P.Length / 64.0 > 1.0)
        //        numArray6 = new byte[128];
        //    parameters.P.CopyTo((Array)numArray6, numArray6.Length - parameters.P.Length);
        //    parameters.P = numArray6;
        //    byte[] numArray7 = new byte[parameters.Q.Length];
        //    if ((double)parameters.Q.Length / 64.0 < 1.0)
        //        numArray7 = new byte[64];
        //    else if ((double)parameters.Q.Length / 64.0 > 1.0)
        //        numArray7 = new byte[128];
        //    parameters.Q.CopyTo((Array)numArray7, numArray7.Length - parameters.Q.Length);
        //    parameters.Q = numArray7;
        //    try
        //    {
        //        cryptoServiceProvider.ImportParameters(parameters);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return cryptoServiceProvider;
        //}

        #endregion

        

        /// <summary>Decodifica la llave pública del archivo .cer</summary>
        /// <param name="publicKey">Stream del archivo .cer</param>
        /// <returns>Llave pública de la FIEL</returns>
        public static RSACryptoServiceProvider decodeCERT(Stream publicKey)
        {
            byte[] array;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                publicKey.Position = 0L;
                publicKey.CopyTo((Stream)memoryStream);
                array = memoryStream.ToArray();
            }
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            try
            {
                return (RSACryptoServiceProvider)new X509Certificate2(array).PublicKey.Key;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>Convierte un texto a tipo seguro en memoria</summary>
        /// <param name="input">Cadena a encriptar en memoria</param>
        /// <returns>Cadena encriptada</returns>
        public static SecureString StringToSecureString(string input)
        {
            SecureString secureString = new SecureString();
            int length = input.Length;
            foreach (char c in input.ToCharArray(0, length))
                secureString.AppendChar(c);
            return secureString;
        }

        /// <summary>Desencripta un texto de un tipo seguro</summary>
        /// <param name="secstrPassword">Variable con el texto encriptado</param>
        /// <returns>Texto desencriptado</returns>
        public string convertToUNSecureString(SecureString secstrPassword)
        {
            IntPtr num = IntPtr.Zero;
            try
            {
                num = Marshal.SecureStringToGlobalAllocUnicode(secstrPassword);
                return Marshal.PtrToStringUni(num);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(num);
            }
        }
    }
}
