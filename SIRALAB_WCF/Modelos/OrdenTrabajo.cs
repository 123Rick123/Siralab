using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class OrdenTrabajo:BusquedaOT
    {

        public string EstatusAnalisis { get; set; }

        public string LaboratorioRegistraOT { get; set; }
      
        public string LaboratorioMuestreo { get; set; }
   
        public string Muestreador { get; set; }
      
        public string LaboratorioAnalizo { get; set; }
    
        public int FolioCONAGUA { get; set; }


        public string NombreUsuarioContribuyente { get; set; }

  
        public string Estado { get; set; }
   
        public string Municipio { get; set; }
  
        public string TipoCuerpoReceptor { get; set; }
       
        public string UsoCuerpoReceptor { get; set; }
        
        public DateTime? FechaRecepcion { get; set; }

        public DateTime? FechaConformacion { get; set; }
        
        public DateTime? FechaEmision { get; set; }

        public DateTime? FechaFirma { get; set; }
         public string EstaHabilitado { get; set; }
       
        public int HorasProceso { get; set; }
 
        public string TipoMuestra { get; set; }

        public Decimal Caudal { get; set; }
   
        public string Parametro { get; set; }

        public Decimal Resultado { get; set; }

        public string Exento { get; set; }

        public string NoAnalizado { get; set; }
      
        public string Unidad { get; set; }
        
        public Decimal LC_MC { get; set; }
        
        public string Metodo { get; set; }
       
        public string Firmada { get; set; }
        [DataMember]
        public int? OrdenTrabajoID { get; set; }
        
        public int AnexoID { get; set; }
       
        public int ConsecionID { get; set; }
       
        public int LaboratorioID { get; set; }
        
        public int Consecutivo { get; set; }
       
        public int ClienteID { get; set; }
      
        public int EstatusID { get; set; }
       
    }
}