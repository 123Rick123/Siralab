using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIRALAB.Entidades
{
    [Serializable]
    public class XMuestra
    {
        [XmlElement(ElementName = "numero")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "descripcion")]
        public string Descripcion { get; set; }

        [XmlElement(ElementName = "identificacion")]
        public string Identificacion { get; set; }

        [XmlElement(ElementName = "caudal")]
        public decimal Caudal { get; set; }

        [XmlElement(ElementName = "fecha")]
        public DateTime? Fecha { get; set; }

        [XmlElement(ElementName = "fecha_recepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [XmlElement(ElementName = "observaciones")]
        public string Observaciones { get; set; }

        [XmlArray(ElementName = "valores", IsNullable = false)]
        [XmlArrayItem(ElementName = "valor")]
        public virtual List<XMuestraValor> MuestraValor { get; set; }
    }

    public enum CatTipoSignatario
    {
        Muestreador = 1,
        Analista = 2,
        Ambos = 3
    }

    public enum NivelConfiguracion
    {
        Simple = 1,
        Compuesta = 2
    }
}
