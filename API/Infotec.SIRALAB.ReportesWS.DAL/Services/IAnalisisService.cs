using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Infotec.SIRALAB.ReportesWS.DAL.Models.DAO;
using Infotec.SIRALAB.ReportesWS.DAL.Models.Enums;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IAnalisisService
    {
        Task<Analisi> GetAnalisis(int analisisId);
        Task<List<Analisi>> GetAnalisisByPeriodo(int? consecionId, int? anexoId, int normaId,DateTime fechaInicio, DateTime fechaFin);
        List<AnalisisPDInformeMensual> GetResultadosPD(List<Analisi> analisisList, TipoEvaluacion tipoEvaluacion);
        Task<List<AnalisisPDInformeMensual>> ObtenEvaluacion(int clienteId, int normaId, int anexoId, int cuerpoReceptorId, int usoAguaId, decimal caudal, DateTime fechaInicio, DateTime fechaFin, TipoEvaluacion tipoEvaluacion, List<Analisi> analisisList);
        Task<int> GetNumeroPD(int ordenTrabajoId, int analisisId);
        List<Parametro> GetParametros();
        bool ObtieneDatosInscritosNom001(string Anexo, string Titulo, string RFC);

        public bool RequierePrograma();
    }
}