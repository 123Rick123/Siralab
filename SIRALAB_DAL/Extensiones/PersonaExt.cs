using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL
{
    public partial class Persona //: EntityObject
    {
        public string UsuarioNombreRecaudacion
        {
            get
            {

                int UsuarioRolID = 2;
                string nombre = this.Usuarios.Where(u => u.PersonaID == this.PersonaID && u.UsuarioRolID == UsuarioRolID).Select(u => u.Usuario1).FirstOrDefault() ?? "";
                return nombre;
            }
        }

        public string UsuarioEstatus
        {
            get
            {

                int UsuarioRolID = 2;
                bool estatus = this.Usuarios.Where(u => u.PersonaID == this.PersonaID && u.UsuarioRolID == UsuarioRolID).Select(u => u.Activo).FirstOrDefault();
                return (estatus) ? "Activo" : "No Activo";
            }
        }
        public string NombreCompleto
        {
            get
            {
                string nombre = this.Nombre + " " + this.ApellidoPaterno + " " + this.ApellidoMaterno;
                return nombre;
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
