using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    [DataContract(Name = "enumOperacion")]
    public enum enumOperacion
    {
        [EnumMember]
        Sumatoria = 1,
        [EnumMember]
        Promedio_Aritmetico,
        [EnumMember]
        Ponderado,
        [EnumMember]
        Media_Geométrica,
        [EnumMember]
        No_Aplica
    }
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
    [DataContract(Name = "enumSignatario")]
    public enum enumSignatario
    {
        [EnumMember]
        Analista = 1,
        [EnumMember]
        Muestreador
    }
    [DataContract(Name = "enumTipoCuerpoReceptor")]
    public enum enumTipoCuerpoReceptor
    {
        [EnumMember]
        A = 1,
        [EnumMember]
        B,
        [EnumMember]
        C,
        [EnumMember]
        D
    }
    [DataContract(Name = "enumTipoEvaluacion")]
    public enum enumTipoEvaluacion
    {
        [EnumMember]
        NoEvaluar = 1,
        [EnumMember]
        Evaluar_Simple,
        [EnumMember]
        Evaluar_LFD,
        [EnumMember]
        Evaluar_LFD_Simple
    }
    [DataContract(Name = "enumAnalisisEstatus")]
    public enum enumAnalisisEstatus
    {
        [EnumMember]
        Activo = 3,
        [EnumMember]
        Cancelado
    }

    [DataContract(Name = "enumEvaluacionPromedios")]
    public enum enumEvaluacionPromedios
    {
        [EnumMember]
        Completo = 1,
        [EnumMember]
        NO_PD,
        [EnumMember]
        Incompleto
    }

}
