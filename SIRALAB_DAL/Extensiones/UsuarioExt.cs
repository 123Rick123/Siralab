using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL
{
    public partial class Usuario //: EntityObject
    {
        public string URL_Default
        {
            get
            {
                var pagina = "";
                if (this.UsuarioRolID == 1 || this.UsuarioRolID == 2 || this.UsuarioRolID == 8 || this.UsuarioRolID==11)
                {
                    pagina = ConfigurationManager.AppSettings["DefaultPage"].ToString();
                }
                else
                {
                    pagina = ConfigurationManager.AppSettings["DefaultPageAdmin"].ToString();
                }

                return pagina;
            }
        }

        /// <summary>
        /// Está activo
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public string esActivo
        {
            get
            {
                return (this.Activo) ? "Activo" : "No Activo";


            }
        }
    }
}
