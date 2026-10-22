using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRALAB_SIRFIC
{
    public class Anexo : IEquatable<Anexo>
    {
        public long AprovechamientoID { get; set; }
        public string Anexo1 { get; set; }
        public string TipoCuerpoReceptorID { get; set; }
        public string Aprovechamiento { get; set; }
        public long? AnexoID { get; set; }
        public int? NumeroPuntoDescarga { get; set; }
        public string ClasificacionDescarga { get; set; }
        public Nullable<long> CuerpoReceptorID { get; set; }
        public string CuerpoReceptor { get; set; }
        public long? SerieMedidorID { get; set; }
        public string SerieMedidor { get; set; }
        public Nullable<long> MarcaMedidorID { get; set; }
        public string MarcaMedidor { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public Direccion Domicilio { get; set; }

        public List<AnexoMedidor_vw> AnexoMedidores { get; set; }
        public bool Equals(Anexo other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return AprovechamientoID.Equals(other.AprovechamientoID) && Anexo1.Equals(other.Anexo1) && TipoCuerpoReceptorID.Equals(other.TipoCuerpoReceptorID)
                 && Aprovechamiento.Equals(other.Aprovechamiento) && AnexoID.Equals(other.AnexoID) && NumeroPuntoDescarga.Equals(other.NumeroPuntoDescarga)
                 && ClasificacionDescarga.Equals(other.ClasificacionDescarga) && CuerpoReceptorID.Equals(other.CuerpoReceptorID) && CuerpoReceptor.Equals(other.CuerpoReceptor)
                 && SerieMedidorID.Equals(other.SerieMedidorID) && MarcaMedidorID.Equals(other.MarcaMedidorID) && MarcaMedidor.Equals(other.MarcaMedidor)
                 && Latitud.Equals(other.Latitud) && Longitud.Equals(other.Longitud);
        }
    }
}
