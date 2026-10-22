using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IUspReporteMensualHeader
    {
        Task<usp_ReporteMensualHeader> Obten(int clienteId, int normaId, int? anexoId, int? cuerpoReceptorId, int? usoAguaId);
    }
}

