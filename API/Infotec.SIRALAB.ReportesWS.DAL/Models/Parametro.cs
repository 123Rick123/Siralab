using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Parametro
    {
        public Parametro()
        {
            MuestraValors = new HashSet<MuestraValor>();
            NormaConfiguracions = new HashSet<NormaConfiguracion>();
            ParametroMetodos = new HashSet<ParametroMetodo>();
        }

        public int ParametroID { get; set; }
        public int UnidadMedidaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? EsCompuesta { get; set; }

        public virtual CatUnidadMedidum UnidadMedida { get; set; }
        public virtual ICollection<MuestraValor> MuestraValors { get; set; }
        public virtual ICollection<NormaConfiguracion> NormaConfiguracions { get; set; }
        public virtual ICollection<ParametroMetodo> ParametroMetodos { get; set; }
    }
}
