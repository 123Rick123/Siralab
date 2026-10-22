using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class vw_LaboratoriosPALA
    {
        public string Estado { get; set; }
        public string Laboratorio { get; set; }
        public string RFC { get; set; }
        public string AcreditacionEMA { get; set; }
        public DateTime AcreditacionFechaInicio { get; set; }
        public DateTime? AcreditacionFechaFin { get; set; }
        public string Aprobacion { get; set; }
        public DateTime AprobacionFechaInicio { get; set; }
        public DateTime? AprobacionFechaFin { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public int CP { get; set; }
        public string Contacto { get; set; }
        public string TipoContacto { get; set; }
        public string Representante { get; set; }
        public string Parametro { get; set; }
        public string TecnicaAnalitica { get; set; }
        public string Metodo { get; set; }
    }
}
