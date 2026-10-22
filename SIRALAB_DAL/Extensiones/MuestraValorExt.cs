using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL
{
    public partial class MuestraValor
    {
        public bool Editable { get; set; }

        public float? caudal { get; set; }
        public bool? requiereCaudal { get; set; }
        public bool? requiereCompuesta { get; set; }
    }
}
