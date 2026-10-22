using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class LaboratorioAcreditacionService : ILaboratorioAcreditacionService
    {
        private readonly SiralabContext _context;

        public LaboratorioAcreditacionService(SiralabContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LaboratorioAcreditacion>> GetByLaboratorioId(int laboratorioId)
        {
            return await _context.LaboratorioAcreditacions.Where(la => la.LaboratorioID == laboratorioId).ToListAsync();
        }
    }
}