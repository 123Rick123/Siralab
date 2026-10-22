using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.LaboratoriosApercibidos.Enum
{
    public enum EnumEstatusApercibidosReincidentes
    {
        Apercibido = 1,
        ApercibidoConImpugnacion = 2,
        Reincidencia = 3,
        ReincidenciaConImpugnacion = 4

    }
    public enum EnumEstatusImpugnacion
    {
        EnProceso = 1,
        AFavorDeConagua = 2,
        EnContraDeConagua = 3,
        ParaEfectos = 4

    }
    public enum EnumMedioImpugnacion
    {
        RecursoDeRevisión = 1,
        DemandaDeNulidad = 2,
        Amparo = 3,
        Otro = 4
    }
}
