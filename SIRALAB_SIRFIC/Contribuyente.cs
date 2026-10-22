using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_SIRFIC
{
    public class Contribuyente : IEquatable<Contribuyente>
    {
        public long ClienteID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Correo { get; set; }
        public Direccion Domicilio { get; set; }
        public List<Titulo> Titulos { get; set; }
        public Contribuyente()
        {
            Domicilio = new Direccion();
            Titulos = new List<Titulo>();
        }

        public bool Equals(Contribuyente other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return ClienteID.Equals(other.ClienteID) && RFC.Equals(other.RFC);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int hashProductName = ClienteID == null ? 0 : ClienteID.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = RFC.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
    }
}
