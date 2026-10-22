using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Analisi
    {
        public Analisi()
        {
            AnalisisColaEncabezados = new HashSet<AnalisisColaEncabezado>();
            AnalisisFaltaPagos = new HashSet<AnalisisFaltaPago>();
            AnalisisFirmas = new HashSet<AnalisisFirma>();
            DocumentoAnalisis = new HashSet<DocumentoAnalisi>();
            InverseAnalisisPadre = new HashSet<Analisi>();
            Muestras = new HashSet<Muestra>();
            SolicitudCancelacionDetalles = new HashSet<SolicitudCancelacionDetalle>();
        }

        public int AnalisisID { get; set; }
        public int? AnalisisPadreID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int AnexoID { get; set; }
        public int LaboratorioID { get; set; }
        public int EstatusID { get; set; }
        public bool Firmada { get; set; }
        public DateTime? FechaFirma { get; set; }
        public bool Enviado { get; set; }
        public bool Procesado { get; set; }
        public DateTime? FechaProceso { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Observaciones { get; set; }
        public int ConsecionID { get; set; }
        public bool FaltaPago { get; set; }
        public string NormaDQOCOT { get; set; }

        public string Conductividad { get; set; }

        public virtual Analisi AnalisisPadre { get; set; }
        public virtual Anexo Anexo { get; set; }
        public virtual Consecion Consecion { get; set; }
        public virtual Estatus Estatus { get; set; }
        public virtual Laboratorio Laboratorio { get; set; }
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual ICollection<AnalisisColaEncabezado> AnalisisColaEncabezados { get; set; }
        public virtual ICollection<AnalisisFaltaPago> AnalisisFaltaPagos { get; set; }
        public virtual ICollection<AnalisisFirma> AnalisisFirmas { get; set; }
        public virtual ICollection<DocumentoAnalisi> DocumentoAnalisis { get; set; }
        public virtual ICollection<Analisi> InverseAnalisisPadre { get; set; }
        public virtual ICollection<Muestra> Muestras { get; set; }
        public virtual ICollection<SolicitudCancelacionDetalle> SolicitudCancelacionDetalles { get; set; }
    }
}
