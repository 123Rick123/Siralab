using System.Xml.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

namespace SIRALAB.Entidades
{

    [Serializable]
    [XmlRoot("CONAGUA", Namespace = "http://www.conagua.gob.mx/siralab", IsNullable = false)]
    public class XNormaConfiguracion
    {
        [XmlElement(ElementName = "normaConfiguracionID", IsNullable = false)]
        public int NormaConfiguracionID { get; set; }
    }
}
