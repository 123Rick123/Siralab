using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Muestra
    {
        public Muestra()
        {
            DocumentoMuestras = new HashSet<DocumentoMuestra>();
            MuestraValors = new HashSet<MuestraValor>();
            Muestreadors = new HashSet<Muestreador>();
        }

        public int MuestraID { get; set; }
        public int AnalisisID { get; set; }
        public int DireccionID { get; set; }
        public int NivelID { get; set; }
        public string Numero { get; set; }
        public string Descripcion { get; set; }
        public string Identificacion { get; set; }
        public decimal Caudal { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Datum { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public bool Cancelado { get; set; }
        public string Observaciones { get; set; }
        public decimal? ValHojaCampo { get; set; }
        public decimal? ValMuestraC { get; set; }
        public bool? FlagCompuesta { get; set; }
        public DateTime? FechaConformacion { get; set; }

        public virtual Analisi Analisis { get; set; }
        public virtual Direccion Direccion { get; set; }
        public virtual NivelConfiguracion Nivel { get; set; }
        public virtual ICollection<DocumentoMuestra> DocumentoMuestras { get; set; }
        public virtual ICollection<MuestraValor> MuestraValors { get; set; }
        public virtual ICollection<Muestreador> Muestreadors { get; set; }
    }
}
