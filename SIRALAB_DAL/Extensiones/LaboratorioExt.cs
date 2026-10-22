using System;
using System.Configuration;
using System.Linq;

namespace SIRALAB_DAL
{
    public static class LaboratorioExt
    {
        public static string LaboratorioNombre(this Laboratorio lab)
        {
            string nombreReturn;
            if (lab.Fisica)
            {
                nombreReturn = lab.Nombre + " " + lab.ApellidoPaterno + " " + lab.ApellidoMaterno;
            }
            else
            {
                nombreReturn = lab.Nombre;
            }

            return nombreReturn;
        }
        public static string LaboratorioNombreEstado(this Laboratorio lab)
        {
            string nombreReturn;
            nombreReturn = lab.Direccion.Estado.Nombre;

            return nombreReturn;
        }

        
        public static bool RFCValidoParaFirmar(this Laboratorio lab, string rfc)
        {
            var flagRFC = ConfigurationManager.AppSettings["flagRFC"] == "true";
           

            if (!flagRFC)
            {
                var rfcRepresentante = lab.LaboratorioRepresentantes.SingleOrDefault(a => a.Activo);

                return rfcRepresentante != null &&
                       string.Equals(rfc.Trim(), lab.RFC.Trim(), StringComparison.CurrentCultureIgnoreCase) &&
                       rfcRepresentante.Activo;
            }

            return true;
        }


        
    }
}
