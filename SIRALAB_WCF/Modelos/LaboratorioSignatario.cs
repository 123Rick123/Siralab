using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{ 
    [DataContract]
    public class LaboratorioSignatario
    {
        [DataMember]
        public int LaboratorioID { get; set; }

        [DataMember]
        public int PersonaId { get; set; }

        [DataMember]
        public string NombrePersonaSignatario { get; set; }
        [DataMember]
        public string ApellidoPaternoPersonaSignatario{ get; set; }
        [DataMember]
        public string ApellidoMaternoPersonaSignatario{ get; set; }
        [DataMember]
        public bool Activo { get; set; }

        [DataMember]
        public  string RFC { get; set; }


        public string NombreCompleto
        {
            get
            {
                string nombre = "";
                if (this.ApellidoMaternoPersonaSignatario != "" && this.ApellidoMaternoPersonaSignatario != null && this.ApellidoMaternoPersonaSignatario!=" ")
                {
                    nombre = this.NombrePersonaSignatario + " " + this.ApellidoPaternoPersonaSignatario + " " + this.ApellidoMaternoPersonaSignatario;
                }
                else
                {
                    nombre = this.NombrePersonaSignatario + " " + this.ApellidoPaternoPersonaSignatario;
                }

                return nombre;
            }
        }

    }
}