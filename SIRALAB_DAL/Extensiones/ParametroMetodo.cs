using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL
{
    public partial class ParametroMetodo
    {
        public string NombreParametroMetodo 
        {
            get {
                string nombre = string.Empty;

                try
                {
                    if (Parametro != null && Metodo != null)
                    {
                        nombre += Parametro.Nombre + " / " + Metodo.Nombre;
                    }


                }
                catch (Exception ex)
                {

                }

                return nombre;
            }
        }
    }
}
