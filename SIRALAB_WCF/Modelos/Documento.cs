using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class Documento
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public byte[] Archivo { get; set; }

        [DataMember]
        public TipoDocumento TipoDocumento { get; set; }
        [DataMember]
        public string Mime { get; set; }
    }



    
    [DataContract]
    public enum TipoDocumento
    {
        [EnumMember]
        Caratula_del_título_emison_y_1_hoja_del_anexo = 1,
        [EnumMember]
        Imagen_de_ubicación = 2,
        [EnumMember]
        Hoja_de_campo_cadena_custodia = 3,
        [EnumMember]
        Laboratorio_apoyo = 4,
        [EnumMember]
        Oficio_exención_parámetros = 5
    }

}