using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_SIRFIC;

namespace SIRALAB
{
    /// <summary>
    /// Summary description for wsConsultas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]
    public class wsConsultas : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] Encuentra_Clientes(string prefixText, int count)
        {
            ProcesamientoDatos contribuyente = new ProcesamientoDatos();
            var ListadoRFC = contribuyente.GetContribuyentes(prefixText);

            //Cliente_BL cliente_bl=new Cliente_BL();
            //string[] db = cliente_bl.BuscaClientes(prefixText);

            return ListadoRFC.ToArray();
        }
        [WebMethod]
        public string[] Encuentra_Clientes_ACMensual(string prefixText, int count)
        {
            Cliente_BL cliente_bl=new Cliente_BL();
            string[] db = cliente_bl.BuscaClientes(prefixText);
            return db.ToArray();
        }
        [WebMethod]
        public string[] Encuentra_Consecion(string prefixText, int count)
        {
            Consecion_BL consecion_bl = new Consecion_BL();
            string[] db = null;
                db = consecion_bl.BuscaConsecion(prefixText);
            return db;
        }
        [WebMethod]
        public string[] Encuentra_Laboratorio(string prefixText, int count)
        {

            Analisis_BL analisis_bl = new Analisis_BL();
            string[] db = null;
            db = analisis_bl.BuscaLaboratorio(prefixText);
            return db;

        }
        [WebMethod]
        public string[] Listado_Laboratorio(string prefixText)
        {
            Analisis_BL analisis_bl = new Analisis_BL();
            string[] db = null;
            db = analisis_bl.BuscaLaboratorio(prefixText);
            return db;
        }
        [WebMethod]
        public string[] Encuentra_Muestreador(string prefixText, int LaboratorioID)
        {
            Cliente_BL cliente_bl = new Cliente_BL();
            string[] db = null;
            db = cliente_bl.BuscaMuestreador(LaboratorioID, prefixText);
            return db;
        }      
    }
}
