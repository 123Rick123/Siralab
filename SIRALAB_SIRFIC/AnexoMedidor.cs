using System;

namespace SIRALAB_SIRFIC
{
    public class AnexoMedidor_vw : IEquatable<AnexoMedidor_vw>
    {
        public Nullable<long> ID_SERIE_MEDIDOR { get; set; }

        public string numeroMedidor { get; set; }

        public Nullable<long> ID_MARCA_MEDIDOR { get; set; }

        public string MARCA_MEDIDOR { get; set; }

        public int FuenteAbastecimiento { get; set; }

        public bool Equals(AnexoMedidor_vw other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return ID_SERIE_MEDIDOR.Equals(other.ID_SERIE_MEDIDOR)
                 && numeroMedidor.Equals(other.numeroMedidor) && ID_MARCA_MEDIDOR.Equals(other.ID_MARCA_MEDIDOR)
                 && MARCA_MEDIDOR.Equals(other.MARCA_MEDIDOR);
        }
    }
}