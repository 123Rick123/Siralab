using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.FIEL
{
    public class CadenaOriginal
    {
        public CadenaOriginal()
        {
            Datos = new List<string>();

        }
        public List<string> Datos { get; set; }
        public string CadenaConcatenada
        {
            get
            {
                var cadena = "||";
                if (this.Datos.Count > 0)
                {
                    foreach (string dato in this.Datos)
                    {
                        cadena += dato + "|";
                    }
                    cadena += "|";
                }
                else
                    cadena = string.Empty;
                return cadena;
            }
        }
    }
}