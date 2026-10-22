using System;
using System.Collections.Generic;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class NormaConfiguracion
    {
        public NormaConfiguracion()
        {
            InverseNormaConfiguracionPadre = new HashSet<NormaConfiguracion>();
        }

        public int NormaConfiguracionID { get; set; }
        public int? NormaConfiguracionPadreID { get; set; }
        public int NormaID { get; set; }
        public int ParametroID { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int UsoAguaID { get; set; }
        public int NivelID { get; set; }
        public int? OperacionID { get; set; }
        public bool? AceptaValor { get; set; }
        public bool Exenta { get; set; }
        public bool Evalua { get; set; }
        public bool LFD { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public decimal? LimiteInferiorLFD { get; set; }
        public decimal? LimiteSuperiorLFD { get; set; }
        public int Orden { get; set; }
        public bool Grupo { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EscalaDecimal { get; set; }
        public bool? LCMC { get; set; }
        public string Etiqueta { get; set; }
        public bool? Truncamiento { get; set; }
        //public bool? LimitaCaptura { get; set; }
        //public decimal? MinAceptado { get; set; }
        //public decimal? MaxAceptado { get; set; }
        //public int? EscalaDecimalLCM { get; set; }

        public virtual CuerpoReceptor CuerpoReceptor { get; set; }
        public virtual NivelConfiguracion Nivel { get; set; }
        public virtual Norma Norma { get; set; }
        public virtual NormaConfiguracion NormaConfiguracionPadre { get; set; }
        public virtual OperacionConsolidado Operacion { get; set; }
        public virtual Parametro Parametro { get; set; }
        public virtual CatUsoAgua UsoAgua { get; set; }
        public virtual ICollection<NormaConfiguracion> InverseNormaConfiguracionPadre { get; set; }
    }
}
