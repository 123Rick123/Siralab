using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class MuestraValor
    {
        public int MuestraValorID { get; set; }
        public int MuestraID { get; set; }
        public int ParametroID { get; set; }
        public int? ParametroMetodoID { get; set; }
        public int? LaboratorioID { get; set; }
        public int? OperacionID { get; set; }
        public bool Exenta { get; set; }
        public bool Evalua { get; set; }
        public bool Exento { get; set; }
        public bool NoAnalizado { get; set; }
        public decimal? Valor { get; set; }
        public bool? ValorValidacion { get; set; }
        public bool? AceptaValor { get; set; }
        public bool LFD { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public decimal? LimiteInferiorLFD { get; set; }
        public decimal? LimiteSuperiorLFD { get; set; }
        public decimal? LC_MC { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? GeneradoSP { get; set; }
        public bool Grupo { get; set; }
        public int? EscalaDecimal { get; set; }
        public bool? LCMC { get; set; }
        public bool? Truncamiento { get; set; }
        //public bool? LimitaCaptura { get; set; }
        //public decimal? MinAceptado { get; set; }
        //public decimal? MaxAceptado { get; set; }
        //public int? EscalaDecimalLCM { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }
        public virtual Muestra Muestra { get; set; }
        public virtual OperacionConsolidado Operacion { get; set; }
        public virtual Parametro Parametro { get; set; }
        public virtual ParametroMetodo ParametroMetodo { get; set; }
    }
}
