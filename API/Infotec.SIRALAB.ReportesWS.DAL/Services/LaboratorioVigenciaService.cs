using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class LaboratorioVigenciaService : ILaboratorioVigenciaService
    {
        private readonly SiralabContext _context;

        public LaboratorioVigenciaService(SiralabContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LaboratorioVigencium>> GetByLaboratorioId(int laboratorioId)
        {
            return await _context.LaboratorioVigencia.Where(lv => lv.LaboratorioID == laboratorioId).ToListAsync();
        }
    }
}