using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Norma
    {
        public Norma()
        {
            InverseNormaPadre = new HashSet<Norma>();
            NormaConfiguracions = new HashSet<NormaConfiguracion>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int NormaID { get; set; }
        public int? ClienteID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool? ExentaParametros { get; set; }
        public bool RequiereCaudal { get; set; }
        public bool? RequiereCompuesta { get; set; }
        public bool? RequiereMensual { get; set; }
        public bool? Evaluacion { get; set; }
        public bool CPD { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? FuenteAbastecimiento { get; set; }
        public bool? MuestrasMultiple { get; set; }
        public int? NormaPadreID { get; set; }
        public int? Desplazamiento { get; set; }
        public bool? RequiereHora { get; set; }
        public bool? RequiereObservaciones { get; set; }
        public bool? Nom2021 { get; set; }
        public bool? RequiereUsoDeAgua { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Norma NormaPadre { get; set; }
        public virtual ICollection<Norma> InverseNormaPadre { get; set; }
        public virtual ICollection<NormaConfiguracion> NormaConfiguracions { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
