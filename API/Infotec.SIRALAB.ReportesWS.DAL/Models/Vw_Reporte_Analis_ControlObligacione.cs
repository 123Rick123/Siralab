using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Vw_Reporte_Analis_ControlObligacione
    {
        public string Firmada { get; set; }
        public string Estatus_del_análisis { get; set; }
        public string RFC_laboratorio { get; set; }
        public string Laboratorio { get; set; }
        public string RFC_cliente { get; set; }
        public string CURP_cliente { get; set; }
        public string NOMBRE_cliente { get; set; }
        public int Folio_CONAGUA { get; set; }
        public string TITULO { get; set; }
        public string ANEXO { get; set; }
        public string NORMA { get; set; }
        public string Cuerpo_receptor { get; set; }
        public string Uso_de_agua { get; set; }
        public string NumeroInforme { get; set; }
        public int HorasProceso { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaFirma { get; set; }
        public DateTime? Fecha_Registro_Analisis { get; set; }
        public DateTime? Fecha_conformación { get; set; }
    }
}
