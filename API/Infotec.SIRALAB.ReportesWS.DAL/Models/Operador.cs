using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Operador
    {
        public Operador()
        {
            FrecuenciaMuestreoOperadorIDMaximoNavigations = new HashSet<FrecuenciaMuestreo>();
            FrecuenciaMuestreoOperadorIDMinimoNavigations = new HashSet<FrecuenciaMuestreo>();
        }

        public int OperadorID { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }

        public virtual ICollection<FrecuenciaMuestreo> FrecuenciaMuestreoOperadorIDMaximoNavigations { get; set; }
        public virtual ICollection<FrecuenciaMuestreo> FrecuenciaMuestreoOperadorIDMinimoNavigations { get; set; }
    }
}
