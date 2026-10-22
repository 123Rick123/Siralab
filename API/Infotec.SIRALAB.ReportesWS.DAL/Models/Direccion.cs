using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Direccion
    {
        public Direccion()
        {
            Anexos = new HashSet<Anexo>();
            Clientes = new HashSet<Cliente>();
            Laboratorios = new HashSet<Laboratorio>();
            Muestras = new HashSet<Muestra>();
        }

        public int DireccionID { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public int EstadoID { get; set; }
        public int MunicipioID { get; set; }
        public int LocalidadID { get; set; }
        public int ColoniaID { get; set; }
        public int CP { get; set; }

        public virtual Colonium Colonia { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Localidad Localidad { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual ICollection<Anexo> Anexos { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Laboratorio> Laboratorios { get; set; }
        public virtual ICollection<Muestra> Muestras { get; set; }
    }
}
