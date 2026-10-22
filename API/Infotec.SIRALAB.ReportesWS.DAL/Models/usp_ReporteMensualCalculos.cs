namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public class usp_ReporteMensualCalculos
    {
        public int ParmetroID { get; set; }
        public string ParametroNombre  { get; set; }
        public string UnidadMedida { get; set; }
        public bool Exento { get; set; }
        public bool Evalua { get; set; }
        public bool NoAnalizado { get; set; }
        public double? Valor { get; set; }
        public bool AceptaValor { get; set; }
        public bool LFD { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public decimal? LimiteInferiorLFD { get; set; }
        public decimal? LimiteSuperiorLFD { get; set; }
        public bool EsValido { get; set; }
    }
}