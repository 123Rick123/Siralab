using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class vw_reporte_SDT
    {
        public string Nombre_del_contribuyente { get; set; }
        public string RFC { get; set; }
        public string Título { get; set; }
        public string Anexo { get; set; }
        public decimal? Valor { get; set; }
        public string Firmada { get; set; }
        public string Estatus_del_análisis { get; set; }
        public DateTime? Fecha_de_conformación { get; set; }
    }
}
