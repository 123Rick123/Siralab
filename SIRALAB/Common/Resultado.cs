using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.Common
{
    public class Resultado
    {
        public bool Result { get; set; }
        public bool Finalized { get; set; }
        public string Message { get; set; }

    }
    public class ResultadoApercibidosReinciedentes
    {
        public bool Result { get; set; }
        public string idIC2 { get; set; }
        public string Message { get; set; }
        public string idApercibimiento { get; set; }
        public string idReincidencia { get; set; }

    }
    public class ResultadoMediosImpugnacion
    {
        public bool Result { get; set; }
        public string Message { get; set; }


    }
}