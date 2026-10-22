using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface ILaboratorioVigenciaService
    {
        Task<IEnumerable<LaboratorioVigencium>> GetByLaboratorioId(int LaboratorioId);
    }
}