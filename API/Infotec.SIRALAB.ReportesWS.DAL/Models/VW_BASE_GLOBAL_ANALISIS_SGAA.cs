using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class VW_BASE_GLOBAL_ANALISIS_SGAA
    {
        public DateTime? FECHA_DE_FIRMA { get; set; }
        public string LABORATORIO { get; set; }
        public string INFORME { get; set; }
        public int HORAS_PROCESO { get; set; }
        public string RFC { get; set; }
        public string CLIENTE { get; set; }
        public string TÍTULO { get; set; }
        public string ANEXO { get; set; }
        public decimal LONGITUD { get; set; }
        public decimal LATITUD { get; set; }
        public string ESTADO { get; set; }
        public string MUNICIPIO { get; set; }
        public string LOCALIDAD { get; set; }
        public int CÓDGIO_POSTAL { get; set; }
        public string TIPO_ASENTAMIENTO_HUMANO { get; set; }
        public string ASENTAMIENTO_HUMANO { get; set; }
        public string CALLE { get; set; }
        public string NÚMERO_EXTERIOR { get; set; }
        public string NÚMERO_INTERIOR { get; set; }
        public string NORMA { get; set; }
        public DateTime? FECHA_DE_CONFORMACIÓN { get; set; }
        public decimal CAUDAL { get; set; }
        public string TIPO_DE_MUESTRA { get; set; }
        public string PARÁMETRO { get; set; }
        public decimal? RESULTADO { get; set; }
        public string UNIDAD { get; set; }
        public decimal? LC_MC { get; set; }
        public string MÉTODO { get; set; }
        public string CUERPO_RECEPTOR { get; set; }
        public string USO_DE_AGUA { get; set; }
        public string Nombre_de_Cuerpo_Receptor { get; set; }
    }
}
