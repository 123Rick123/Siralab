using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Usuario
    {
        public int UsuarioID { get; set; }
        public int? LaboratorioID { get; set; }
        public int? PersonaID { get; set; }
        public int UsuarioRolID { get; set; }
        public string Usuario1 { get; set; }
        public string Contrasenia { get; set; }
        public string Correo { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual CatUsuarioRol UsuarioRol { get; set; }
    }
}
