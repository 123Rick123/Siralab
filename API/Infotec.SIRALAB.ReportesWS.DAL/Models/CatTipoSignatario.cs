using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class CatTipoSignatario
    {
        public CatTipoSignatario()
        {
            LaboratorioSignatarios = new HashSet<LaboratorioSignatario>();
        }

        public int TipoSignatarioID { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<LaboratorioSignatario> LaboratorioSignatarios { get; set; }
    }
}
