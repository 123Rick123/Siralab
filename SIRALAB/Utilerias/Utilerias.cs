using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.Utilerias
{
    public class Utilerias
    {
        public static string eliminaBase64(string CadenaBase64)
        {
            try
            {
                if (CadenaBase64.Contains("data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,"))
                {
                    CadenaBase64 = CadenaBase64.Replace("data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,", "");
                }
                else if (CadenaBase64.Contains("data:application/msword;base64,"))
                {
                    CadenaBase64 = CadenaBase64.Replace("data:application/msword;base64,", "");
                }
                else if (CadenaBase64.Contains("data:application/pdf;base64,"))
                {
                    CadenaBase64 = CadenaBase64.Replace("data:application/pdf;base64,", "");
                }
                else if (CadenaBase64.Contains("data:application/x-x509-ca-cert;base64,"))
                {
                    CadenaBase64 = CadenaBase64.Replace("data:application/x-x509-ca-cert;base64,", "");
                }
                if (CadenaBase64.Contains("data:application/octet-stream;base64,"))
                {
                    CadenaBase64 = CadenaBase64.Replace("data:application/octet-stream;base64,", "");
                }
                else if (CadenaBase64.Contains("data:;base64,"))
                {
                    CadenaBase64 = CadenaBase64.Replace("data:;base64,", "");
                }
                return CadenaBase64;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}