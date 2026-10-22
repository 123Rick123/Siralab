using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Persona
    {
        public Persona()
        {
            ClienteContactos = new HashSet<ClienteContacto>();
            Contactos = new HashSet<Contacto>();
            LaboratorioContactos = new HashSet<LaboratorioContacto>();
            LaboratorioRepresentantes = new HashSet<LaboratorioRepresentante>();
            LaboratorioSignatarios = new HashSet<LaboratorioSignatario>();
            Usuarios = new HashSet<Usuario>();
        }

        public int PersonaID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<ClienteContacto> ClienteContactos { get; set; }
        public virtual ICollection<Contacto> Contactos { get; set; }
        public virtual ICollection<LaboratorioContacto> LaboratorioContactos { get; set; }
        public virtual ICollection<LaboratorioRepresentante> LaboratorioRepresentantes { get; set; }
        public virtual ICollection<LaboratorioSignatario> LaboratorioSignatarios { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
