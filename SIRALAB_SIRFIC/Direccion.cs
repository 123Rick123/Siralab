using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRALAB_SIRFIC
{
    public class Direccion : IEquatable<Direccion>
    {
        public int EstadoID { get; set; }
        public int MunicipioID { get; set; }
        public int LocalidadID { get; set; }
        public int AsentamientoID { get; set; }
        public string Calle { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Asentamiento { get; set; }
        public string TipoAsentamiento { get; set; }
        public int CodigoPostal { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }

        public bool Equals(Direccion other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return EstadoID.Equals(other.EstadoID) && MunicipioID.Equals(other.MunicipioID) && LocalidadID.Equals(other.LocalidadID)
                 && Calle.Equals(other.Calle) && Estado.Equals(other.Estado) && Municipio.Equals(other.Municipio)
                 && Localidad.Equals(other.Localidad) && Asentamiento.Equals(other.Asentamiento) && TipoAsentamiento.Equals(other.TipoAsentamiento)
                 && CodigoPostal.Equals(other.CodigoPostal) && NumeroExterior.Equals(other.NumeroExterior) && NumeroInterior.Equals(other.NumeroInterior);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int hashProductName = EstadoID == null ? 0 : EstadoID.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = MunicipioID.GetHashCode();
            int hashProductCode2 = LocalidadID.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode ^ hashProductCode2;
        }
    }
}
