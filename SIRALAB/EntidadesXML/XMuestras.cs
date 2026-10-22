using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SIRALAB.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "analisis")]
    public class XMuestras
    {
        [XmlArray(ElementName = "muestras", IsNullable = false)]
        [XmlArrayItem(ElementName = "muestra")]
        public List<XMuestra> Muestras { get; set; }
    }
}
