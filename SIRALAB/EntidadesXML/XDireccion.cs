using System.Xml.Serialization;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIRALAB.Entidades
{

    [Serializable]
    public class XDireccion
    {

        [XmlElement(ElementName = "calle", IsNullable = false)]
        //[StringLength(150)]
        public string Calle { get; set; }

        [XmlElement(ElementName = "numeroExterior", IsNullable = false)]
        //[StringLength(20)]
        public string NumeroExterior { get; set; }

        [XmlElement(ElementName = "numeroInterior")]
        //[StringLength(20)]
        public string NumeroInterior { get; set; }

        [XmlElement(ElementName = "codigoPostal", IsNullable = false)]
        public int CP { get; set; }
    }
}
