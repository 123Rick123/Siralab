using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SIRALAB_WCF.Modelos
{
    [DataContract]
    public class UsuarioDataModel
    {
        
        public int UsuarioID { get; set; }

        public string RFC { get; set; }
        
        public Nullable<int> LaboratorioID { get; set; }
        
        public Nullable<int> PersonaID { get; set; }
        
        public int UsuarioRolID { get; set; }
        
        public string Usuario1 { get; set; }
        
        
        public bool Activo { get; set; }

        
        public string Token { get; set; }

    }
}
