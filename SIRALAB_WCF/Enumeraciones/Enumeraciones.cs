using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SIRALAB_WCF.Enumeraciones
{
    public class Enumeraciones
    {
        [DataContract(Name = "enumNivel")]
        public enum enumNivel
        {
            [EnumMember]
            Simple = 1,
            [EnumMember]
            Compuesta,
            [EnumMember]
            Mensual,
            [EnumMember]
            Fuente_Abastecimiento
        }
    }
}