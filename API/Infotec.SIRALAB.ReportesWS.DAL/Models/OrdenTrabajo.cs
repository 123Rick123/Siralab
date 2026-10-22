using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class OrdenTrabajo
    {
        public OrdenTrabajo()
        {
            Analisis = new HashSet<Analisi>();
            DocumentoAnexos = new HashSet<DocumentoAnexo>();
        }

        public int OrdenTrabajoID { get; set; }
        public int ClienteID { get; set; }
        public int? FrecuenciaMuestreoID { get; set; }
        public int EstatusID { get; set; }
        public int? NormaID { get; set; }
        public int? CuerpoReceptorID { get; set; }
        public int? UsoAguaID { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroInforme { get; set; }
        public string Folio { get; set; }
        public int Anio { get; set; }
        public int Consecutivo { get; set; }
        public int HorasProceso { get; set; }
        public int? AnexoID { get; set; }
        public int? ConsecionID { get; set; }
        public int LaboratorioID { get; set; }
        public bool? FuenteAbastecimiento { get; set; }
        public int? AnexoMedidorID { get; set; }

        public virtual Anexo Anexo { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Consecion Consecion { get; set; }
        public virtual CuerpoReceptor CuerpoReceptor { get; set; }
        public virtual Estatus Estatus { get; set; }
        public virtual FrecuenciaMuestreo FrecuenciaMuestreo { get; set; }
        public virtual Laboratorio Laboratorio { get; set; }
        public virtual Norma Norma { get; set; }
        public virtual CatUsoAgua UsoAgua { get; set; }
        public virtual ICollection<Analisi> Analisis { get; set; }
        public virtual ICollection<DocumentoAnexo> DocumentoAnexos { get; set; }
    }
}
