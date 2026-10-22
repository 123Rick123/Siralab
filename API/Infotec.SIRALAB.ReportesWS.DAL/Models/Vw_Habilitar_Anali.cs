using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Vw_Habilitar_Anali
    {
        public string RFCLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; }
        public string RFCContribuyente { get; set; }
        public string NombreContribuyente { get; set; }
        public int OrdenTrabajoID { get; set; }
        public string Informe { get; set; }
        public int FoloCONAGUA { get; set; }
        public string Titulo { get; set; }
        public string Anexo { get; set; }
        public string Norma { get; set; }
        public DateTime? Fecha_de_conformación { get; set; }
        public DateTime? Fecha_de_firma { get; set; }
        public string Firmada { get; set; }
        public string Estatus_del_análisis { get; set; }
        public string Firmado_fuera_de_tiempo { get; set; }
    }
}
