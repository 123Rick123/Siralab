using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IReportesService
    {

        byte[] CreatePDF(string html);
    }
}