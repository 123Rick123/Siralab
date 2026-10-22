using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.Common
{
    public class ExcelExportModel
    {
        public string Firmada { get; set; }
        public string EstatusAnalisis { get; set; }
        public string LaboratorioRegistraOT { get; set; }
        public string RFCLaboratorio { get; set; }
        public string LaboratorioMuestreo { get; set; }
        public string Muestreador { get; set; }
        public string LaboratorioAnalizo { get; set; }
        public string NumeroInforme { get; set; }
        public int FolioCONAGUA { get; set; }
        public string RFCUsuarioContribuyente { get; set; }
        public string NombreUsuarioContribuyente { get; set; }
        public string Titulo { get; set; }
        public string Anexo { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string TipoCuerpoReceptor { get; set; }
        public string UsoCuerpoReceptor { get; set; }
        public string Norma { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaConformacion { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaFirma { get; set; }
        public string EstaHabilitado { get; set; }
        public int HorasProceso { get; set; }
        public string TipoMuestra { get; set; }
        public decimal? Caudal { get; set; }
        public string Parametro { get; set; }
        public decimal? Resultado { get; set; }
        public string Exento { get; set; }
        public string NoAnalizado { get; set; }
        public string Unidad { get; set; }
        public decimal? LC_MC { get; set; }
        public string Metodo { get; set; }
    }
}