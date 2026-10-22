using System.Xml.Serialization;
using System;

namespace SIRALAB.Entidades
{

    [Serializable]
    [XmlRoot("CONAGUA", Namespace = "http://www.conagua.gob.mx/siralab", IsNullable = false)]
    public class XLaboratorioSignatario
    {
        [XmlElement(ElementName = "laboratorioSignatarioID")]
        public int LaboratorioSignatarioID { get; set; }

        [XmlElement(ElementName = "catTipoSignatarioID", IsNullable = false)]
        public virtual CatTipoSignatario CatTipoSignatario { get; set; }
    }
}
