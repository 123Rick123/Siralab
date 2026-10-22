using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using SIRALAB_DAL;
using SIRALAB;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class Analisis
    {
        public Analisis()
        {
            MuestrasSimple = new List<Muestra>();
        }
      
        public int AnalisisID { get; set; }
    
        public int? AnalisisPadreID { get; set; }
      
        public int OrdenTrabajoID { get; set; }
       
        public int AnexoID { get; set; }

        public int LaboratorioID { get; set; }

        public int EstatusID { get; set; }

        
        public DateTime? FechaFirma { get; set; }

        [DataMember]
        public DateTime? FechaProceso { get; set; }
        [DataMember]
        public DateTime? FechaRegistro { get; set; }
        [DataMember]
        public string Observaciones { get; set; }

        public int ConsecionID { get; set; }
       
        public Nullable<int> NormaID { get; set; }
        [DataMember]
        public List<Muestra> MuestrasSimple { get; set; }
        [DataMember]
        public List<Muestra> MuestrasCompuesta { get; set; }
        [DataMember]
        public Decimal LatitudGrados { get; set; }
        [DataMember]
        public Decimal LatitudMinutos { get; set; }
        [DataMember]
        public Decimal LatitudSeg { get; set; }
        [DataMember]
        public Decimal LongitudGrados { get; set; }
        [DataMember]
        public Decimal LongitudMinutos { get; set; }
        [DataMember]
        public Decimal LongitudSeg { get; set; }
        [DataMember]
        public EnumLongitud Longitud { get; set; }
        [DataMember]
        public EnumLatitud Latitud { get; set; }
        [DataMember]
        public Datum Datum { get; set; }
        [DataMember]
        public Signatario Signatario { get; set; }
    }
    [DataContract]
    public enum Datum
    {
        [EnumMember]
        WGS84=1,
        [EnumMember]
        NAD27 = 2,
        [EnumMember]
        NAD83 = 3,
    }

    [DataContract]
    public enum EnumLongitud
    {
        [EnumMember]
        O = 1,
        [EnumMember]
        E = 0,
    }
    [DataContract]
    public enum EnumLatitud
    {
        [EnumMember]
        N = 0,
        [EnumMember]
        S = 1,
        
    }
}