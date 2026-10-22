using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.FIEL
{
    public static class Constantes
    {
        public static string IdCertificado
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["IdCertificadoWS"]; 
                  return System.Configuration.ConfigurationManager.AppSettings["IdCertificadoWS"].ToString();
                //return "ID-SELLOS-PBAS";
            }
        }

        public static string SerieCertificado
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["NumSerieCerficadoWS"];
                  return System.Configuration.ConfigurationManager.AppSettings["NumSerieCerficadoWS"].ToString();
                //return "20001000000100005761";
            }
        }

        public static string AlgoritmoHash
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["AlgoritmoHash"];
                  return System.Configuration.ConfigurationManager.AppSettings["AlgoritmoHash"].ToString();
                //return "SHA512";
            }
        }

        public static string RfcCertificado
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["RfcCertificado"];
                return System.Configuration.ConfigurationManager.AppSettings["RfcCertificado"].ToString();
                //return "AAA010101AAA";
            }
        }

    }
}