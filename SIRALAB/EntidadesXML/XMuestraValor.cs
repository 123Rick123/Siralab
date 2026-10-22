using System.Xml.Serialization;
using System;

namespace SIRALAB.Entidades
{
    [Serializable]
    public class XMuestraValor
    {

        [XmlElement(ElementName = "parametro_id", IsNullable = false)]
        public int ParametroID { get; set; }
        [XmlElement(ElementName = "parametro_metodo_id", IsNullable = true)]
        public int? ParametroMetodoID { get; set; }
        [XmlElement(ElementName = "laboratorio_id", IsNullable = true)]
        public int? LaboratorioID { get; set; }
        [XmlElement(ElementName = "exento", IsNullable = false)]
        public bool Exento { get; set; }
        [XmlElement(ElementName = "no_analizado", IsNullable = false)]
        public bool NoAnalizado { get; set; }
        [XmlElement(ElementName = "valor", IsNullable = true)]
        public decimal? Valor { get; set; }
        [XmlElement(ElementName = "lc_mc", IsNullable = true)]
        public decimal? LC_MC { get; set; }
    }
}
