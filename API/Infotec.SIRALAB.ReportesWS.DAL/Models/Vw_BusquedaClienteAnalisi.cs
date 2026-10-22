using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Vw_BusquedaClienteAnalisi
    {
        public int? AnalisisID { get; set; }
        public string Firmada { get; set; }
        public DateTime? fechaFirmaAnalisis { get; set; }
        public string Estatus { get; set; }
        public string NombreInforme { get; set; }
        public string Titulo { get; set; }
        public string Anexo { get; set; }
        public string nombreCuerpoReceptor { get; set; }
        public string RFCCliente { get; set; }
        public string NombreCliente { get; set; }
        public string Norma { get; set; }
        public DateTime FechaEmision { get; set; }
        public string RFCLaboratorio { get; set; }
        public string NomLab { get; set; }
    }
}
