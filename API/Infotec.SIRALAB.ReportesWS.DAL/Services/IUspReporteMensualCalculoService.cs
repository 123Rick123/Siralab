using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Infotec.SIRALAB.ReportesWS.DAL.Models.DAO;
using Infotec.SIRALAB.ReportesWS.DAL.Models.Enums;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IUspReporteMensualCalculoService
    {
        Task<List<usp_ReporteMensualCalculos>> Obten(int clienteId, int normaId, int anexoId, int cuerpoReceptorId, int usoAguaId, decimal caudal, DateTime fechaInicio, DateTime fechaFin, TipoEvaluacion tipoEvaluacion);

        
    }
}