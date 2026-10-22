using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Anexo
    {
        public Anexo()
        {
            Analisis = new HashSet<Analisi>();
            AnexoMedidors = new HashSet<AnexoMedidor>();
            OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int AnexoID { get; set; }
        public int ConsecionID { get; set; }
        public int DireccionID { get; set; }
        public string Anexo1 { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Datum { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int? UsoAguaID { get; set; }
        public string NombreCuerpoReceptor { get; set; }
        public string FuenteAbastecimiento { get; set; }
        public string OrigenDescarga { get; set; }
        public string Medidor { get; set; }
        public string MarcaMedidor { get; set; }
        public bool Efluente { get; set; }
        public long? PukAnexoID { get; set; }
        public long? idUman { get; set; }

        public virtual Consecion Consecion { get; set; }
        public virtual CuerpoReceptor CuerpoReceptor { get; set; }
        public virtual Direccion Direccion { get; set; }
        public virtual CatUsoAgua UsoAgua { get; set; }
        public virtual ICollection<Analisi> Analisis { get; set; }
        public virtual ICollection<AnexoMedidor> AnexoMedidors { get; set; }
        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
