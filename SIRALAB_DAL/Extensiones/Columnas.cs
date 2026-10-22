using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_DAL.Extensiones
{
    public class Columnas
    {
        public int? ParametroID { get; set; }
        public string Nombre { get; set; }
        public int? LaboratorioID { get; set; }
        public int? MetodoID { get; set; }
        public int? Accion { get; set; }
        public int? OperacionID { get; set; }
        public bool RequiereLC_MC { get; set; }
        public decimal Valor { get; set; }
        public bool? RequiereSelect { get; set; }
        public bool? RequiereCompuesta { get; set; }
        public decimal? EscalaDecimal { get; set; }
        public bool? RequiereCaudal { get; set; }
        public bool? RequiereParticipacion { get; set; }
        public bool? RequiereVolumen { get; set; }
        public bool? RequiereParametro { get; set; }
        public bool? muestraTotalNoRequerido { get; set; }
        public decimal? limiteSuperior { get; set; }
        public decimal? limiteInferior { get; set; }
        public bool? requiereLimiteInferior { get; set; }
        public bool? capturaLMP { get; set; }
        public bool? Truncamiento { get; set; }

    }
}
