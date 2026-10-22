using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatUnidadMedidum
    {
        public CatUnidadMedidum()
        {
            Parametros = new HashSet<Parametro>();
        }

        public int UnidadMedidaID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Parametro> Parametros { get; set; }
    }
}
