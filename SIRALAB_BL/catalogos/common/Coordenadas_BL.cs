using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRALAB_BL;
using SIRALAB_DAL;

namespace SIRALAB_BL
{
    public class Coordenadas_BL
    {
        public decimal GradosLat { get; private set; }
        public decimal MinutosLat { get; private set; }
        public decimal SegundosLat { get; private set; }
        public EnumCoordinatesLat PosicionLat { get; private set; }

        public decimal GradosLng { get; private set; }
        public decimal MinutosLng { get; private set; }
        public decimal SegundosLng { get; private set; }
        public EnumCoordinatesLng PosicionLng { get; private set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public Coordenadas_BL(decimal latitude, decimal longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;

            CoordsToSexLat(latitude);
            CoordsToSexLng(longitude);
        }

        public Coordenadas_BL(decimal gradosLat, decimal minutosLat, decimal segundosLat, EnumCoordinatesLat posicionLat,
            decimal gradosLng, decimal minutosLng, decimal segundosLng, EnumCoordinatesLng posicionLng)
        {
            this.GradosLat = gradosLat;
            this.MinutosLat = minutosLat;
            this.SegundosLat = segundosLat;
            this.PosicionLat = posicionLat;
            CoordsToDecLat(gradosLat, minutosLat, segundosLat, posicionLat);

            this.GradosLng = gradosLng;
            this.MinutosLng = minutosLng;
            this.SegundosLng = segundosLng;
            this.PosicionLng = posicionLng;
            CoordsToDecLng(gradosLng, minutosLng, segundosLng, posicionLng);
        }

        private void CoordsToDecLat(decimal gradosLat, decimal minutosLat, decimal segundosLat, EnumCoordinatesLat posicionLat)
        {
            decimal gradosCalulados = minutosLat;
            minutosLat = segundosLat / 60;
            gradosCalulados = (gradosCalulados + minutosLat) / 60;

            this.Latitude = (gradosCalulados + gradosLat) * ((posicionLat == EnumCoordinatesLat.Norte) ? 1 : -1);
        }
        private void CoordsToDecLng(decimal gradosLng, decimal minutosLng, decimal segundosLng, EnumCoordinatesLng posicionLng)
        {
            decimal gradosCalulados = minutosLng;
            minutosLng = segundosLng / 60;
            gradosCalulados = (gradosCalulados + minutosLng) / 60;

            this.Longitude = (gradosCalulados + gradosLng) * ((posicionLng == EnumCoordinatesLng.Este) ? 1 : -1);
        }

        private void CoordsToSexLat(decimal latitude)
        {
            EnumCoordinatesLat position = EnumCoordinatesLat.Norte;
            if (latitude < 0)
                position = EnumCoordinatesLat.Sur;
            if (latitude > 0)
                position = EnumCoordinatesLat.Norte;

            var grados = ConvertSexaToDec(latitude);
            this.GradosLat = grados[0];
            this.MinutosLat = grados[1];
            this.SegundosLat = grados[2];
            this.PosicionLat = position;
        }
        private void CoordsToSexLng(decimal longitude)
        {
            EnumCoordinatesLng position0 = EnumCoordinatesLng.Este;
            if (longitude < 0)
                position0 = EnumCoordinatesLng.Oeste;
            if (longitude > 0)
                position0 = EnumCoordinatesLng.Este;

            var grados0 = ConvertSexaToDec(longitude);
            this.GradosLng = grados0[0];
            this.MinutosLng = grados0[1];
            this.SegundosLng = grados0[2];
            this.PosicionLng = position0;
        }
        private decimal[] ConvertSexaToDec(decimal value)
        {
            var decimalValue = Convert.ToDecimal(value);
            decimalValue = Math.Abs(decimalValue);

            var grados = Decimal.Truncate(decimalValue);
            decimalValue = (decimalValue - grados) * 60;

            var minutos = Decimal.Truncate(decimalValue);
            var segundos = Math.Round((decimalValue - minutos) * 60, 3, MidpointRounding.AwayFromZero);

            return new decimal[] { grados, minutos, segundos };
        }
    }
    public enum EnumCoordinatesLng
    {
        Este = 0, Oeste = 1,
    }
    public enum EnumCoordinatesLat
    {
        Norte = 0, Sur = 1,
    }
}