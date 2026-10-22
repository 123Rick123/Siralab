using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace SIRALAB.FIEL
{
    public static class CommonFirma
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static String ToHexString(byte[] byteArray)
        {
            byte[] hexEncodedArray = Org.BouncyCastle.Utilities.Encoders.Hex.Encode(
                byteArray, 0, byteArray.Length);
            String hexEncoded = Encoding.Default.GetString(hexEncodedArray);

            return hexEncoded;
        }

        /// <summary>
        /// Convierte un string a una cadena en UTF8 y Base 64
        /// </summary>
        /// <param name="cadena">cadena a transformar</param>
        /// <returns>string en Base64</returns>
        public static string ToUTF8Base64String(string cadena)
        {
            try
            {
                return Convert.ToBase64String(UTF8Bytes(cadena));
            }
            catch { return null; }
        }

        /// <summary>
        /// Convierte un string en un arreglo de bytes con formtato UTF8
        /// </summary>
        /// <param name="cadena">cadena a transformar</param>
        /// <returns>byte array en UTF8</returns>
        public static byte[] UTF8Bytes(string cadena)
        {
            try
            {
                return Encoding.UTF8.GetBytes(cadena);
            }
            catch { return null; }
        }

    }
}