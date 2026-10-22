using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class Vw_Conamer
    {
        public string Firmada { get; set; }
        public string Estatus_del_análisis { get; set; }
        public string Laboratorio_que_registra_la_OT { get; set; }
        public string RFC_Laboratorio { get; set; }
        public string Laboratorio_que_realiza_el_muestreo { get; set; }
        public string Muestreador { get; set; }
        public string Laboratorio_que_analizó { get; set; }
        public string Informe { get; set; }
        public int Folio_CONAGUA { get; set; }
        public string RFC_usuario { get; set; }
        public string Nombre_usuario { get; set; }
        public string Título { get; set; }
        public string Anexo { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Tipo_de_cuerpo_receptor { get; set; }
        public string Uso_del_cuerpo_receptor { get; set; }
        public string Norma { get; set; }
        public DateTime? Fecha_de_recepción_en_el_laboratorio { get; set; }
        public DateTime? Fecha_de_conformación { get; set; }
        public DateTime Fecha_de_emisión { get; set; }
        public DateTime? Fecha_de_firma { get; set; }
        public string Está_habilitado_ { get; set; }
        public int Horas_proceso { get; set; }
        public string Tipo_muestra { get; set; }
        public decimal? Caudal { get; set; }
        public string Parámetro { get; set; }
        public decimal? Resultado { get; set; }
        public string Exento { get; set; }
        public string No_analizado { get; set; }
        public string Unidad { get; set; }
        public decimal? Lc_Mc { get; set; }
        public string Método { get; set; }
    }
}
