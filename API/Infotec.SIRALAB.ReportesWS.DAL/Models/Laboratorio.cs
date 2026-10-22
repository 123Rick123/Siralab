using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Laboratorio
    {
        public Laboratorio()
        {
            Analisis = new HashSet<Analisi>();
            LaboratorioAcreditacions = new HashSet<LaboratorioAcreditacion>();
            LaboratorioAprobacions = new HashSet<LaboratorioAprobacion>();
            LaboratorioClientes = new HashSet<LaboratorioCliente>();
            LaboratorioContactos = new HashSet<LaboratorioContacto>();
            LaboratorioRepresentantes = new HashSet<LaboratorioRepresentante>();
            LaboratorioSignatarios = new HashSet<LaboratorioSignatario>();
            LaboratorioSucursalLaboratorioHijos = new HashSet<LaboratorioSucursal>();
            LaboratorioSucursalLaboratorioPadres = new HashSet<LaboratorioSucursal>();
            LaboratorioVigencia = new HashSet<LaboratorioVigencium>();
            MuestraValors = new HashSet<MuestraValor>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
            SolicitudCancelacions = new HashSet<SolicitudCancelacion>();
            Usuarios = new HashSet<Usuario>();
        }

        public int LaboratorioID { get; set; }
        public int DireccionID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public bool Fisica { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Direccion Direccion { get; set; }
        public virtual ICollection<Analisi> Analisis { get; set; }
        public virtual ICollection<LaboratorioAcreditacion> LaboratorioAcreditacions { get; set; }
        public virtual ICollection<LaboratorioAprobacion> LaboratorioAprobacions { get; set; }
        public virtual ICollection<LaboratorioCliente> LaboratorioClientes { get; set; }
        public virtual ICollection<LaboratorioContacto> LaboratorioContactos { get; set; }
        public virtual ICollection<LaboratorioRepresentante> LaboratorioRepresentantes { get; set; }
        public virtual ICollection<LaboratorioSignatario> LaboratorioSignatarios { get; set; }
        public virtual ICollection<LaboratorioSucursal> LaboratorioSucursalLaboratorioHijos { get; set; }
        public virtual ICollection<LaboratorioSucursal> LaboratorioSucursalLaboratorioPadres { get; set; }
        public virtual ICollection<LaboratorioVigencium> LaboratorioVigencia { get; set; }
        public virtual ICollection<MuestraValor> MuestraValors { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
        public virtual ICollection<SolicitudCancelacion> SolicitudCancelacions { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
