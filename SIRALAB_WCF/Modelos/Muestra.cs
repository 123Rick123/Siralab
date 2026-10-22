using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SIRALAB_DAL;
using SIRALAB_BL.common;

namespace SIRALAB_WCF.Modelos
{
    public class Muestra
    {             
        [DataMember]
        public Direccion Direccion { get; set; }      
      
        public string numero { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Identificacion { get; set; }     
       
        [DataMember]
        public decimal? Caudal { get; set; }    
        
        [DataMember]
        public DateTime? Fecha_Muestra { get; set; }
        [DataMember]
        public TimeSpan? Hora_Muestra { get; set; }
        [DataMember]
        public DateTime? Fecha_RecepcionCompuesta { get; set; }
        [DataMember]
        public TimeSpan? Hora_RecepcionCompuesta { get; set; }
        [DataMember]
        public bool? Cancelado { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public List<Parametro> Parametros { get; set; }
        [DataMember]
        public decimal Volumen_Muestra_compuesta { get; set; }
    }
}