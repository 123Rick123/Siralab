using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface ILaboratorioAcreditacionService
    {
        Task<IEnumerable<LaboratorioAcreditacion>> GetByLaboratorioId(int laboratorioId);
    }
}