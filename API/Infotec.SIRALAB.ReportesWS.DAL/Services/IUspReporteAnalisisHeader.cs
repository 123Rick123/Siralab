using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IUspReporteAnalisisHeader
    {
        Task<usp_ReporteAnalisisHeader_Result> Obten(int analisisId);
        usp_ReporteAnalisisHeader_Result ObtenDatosGenerales(int analisisId);
    }
}