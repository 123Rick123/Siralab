using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRALAB_SIRFIC
{
    public class Titulo : IEquatable<Titulo>
    {
        public long TituloID { get; set; }
        public string Titulo1 { get; set; }
        public List<Anexo> Anexos { get; set; }
        public Titulo()
        {
            Anexos = new List<Anexo>();
        }

        public bool Equals(Titulo other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            return TituloID.Equals(other.TituloID) && Titulo1.Equals(other.Titulo1);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int hashProductName = TituloID == null ? 0 : TituloID.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = Titulo1.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
    }
}
