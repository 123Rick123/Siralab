using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL
{
    public partial class NormaConfiguracion
    {      

        public string CuerpoReceptorNombre {
            get
            {
                string nombre = string.Empty;

                try
                {
                    if (CuerpoReceptor!= null)
                    {
                        nombre += CuerpoReceptor.Nombre;
                    }


                }
                catch (Exception ex)
                {

                }

                return nombre;
            }
        }

        public string UsoAguaNombre
        {
            get
            {
                string nombre = string.Empty;

                try
                {
                    if (CatUsoAgua != null)
                    {
                        nombre += CatUsoAgua.Nombre;
                    }


                }
                catch (Exception ex)
                {

                }

                return nombre;
            }
        }

        public string ParametroNombre
        {
            get
            {
                string nombre = string.Empty;

                try
                {
                    
                    if (this.Parametro != null)
                    {
                        nombre += this.Parametro.Nombre;
                    }
                }
                catch (Exception ex)
                {

                }

                return nombre;
            }
        }

        public class NormaConfiguracionEqualityComparer : IEqualityComparer<NormaConfiguracion>
        {
            public bool Equals(NormaConfiguracion NormaConfiguracion1, NormaConfiguracion NormaConfiguracion2)
            {
                if (
                    NormaConfiguracion1.NormaConfiguracionID == NormaConfiguracion2.NormaConfiguracionID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(NormaConfiguracion NormaConfiguracion)
            {
                return new { NormaConfiguracion.NormaID, NormaConfiguracion.Parametro.ParametroID, NormaConfiguracion.CuerpoReceptorID,NormaConfiguracion.UsoAguaID }.GetHashCode();
            }

        }
    
    
    
    
    
    }
}
