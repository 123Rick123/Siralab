using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteContactos = new HashSet<ClienteContacto>();
            Consecions = new HashSet<Consecion>();
            LaboratorioClientes = new HashSet<LaboratorioCliente>();
            Normas = new HashSet<Norma>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int ClienteID { get; set; }
        public int DireccionID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Correo { get; set; }

        public virtual Direccion Direccion { get; set; }
        public virtual ICollection<ClienteContacto> ClienteContactos { get; set; }
        public virtual ICollection<Consecion> Consecions { get; set; }
        public virtual ICollection<LaboratorioCliente> LaboratorioClientes { get; set; }
        public virtual ICollection<Norma> Normas { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
