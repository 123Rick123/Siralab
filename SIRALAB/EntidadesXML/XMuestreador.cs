using System;
using System.Xml.Serialization;

namespace SIRALAB.Entidades
{
    [Serializable]
    public class XMuestreador
    {
        [XmlElement(ElementName = "muestraID", IsNullable = false)]
        public int MuestraID { get; set; }

        [XmlElement(ElementName = "laboratorioSignatarioID", IsNullable = false)]
        public int LaboratorioSignatarioID { get; set; }
    }
}