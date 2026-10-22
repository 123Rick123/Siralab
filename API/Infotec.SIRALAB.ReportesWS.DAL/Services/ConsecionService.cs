using System;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class ConsecionService : IConsecionService
    {

        private readonly SiralabContext _siralabContext;

        public ConsecionService(SiralabContext siralabContext)
        {
            _siralabContext = siralabContext;
        }


        public async Task<Consecion> GetConsecionByTituloAndClienteId(string titulo, int clienteId)
        {
            try
            {
                return await _siralabContext.Consecions
                    .Include(c => c.Anexos)
                    .FirstOrDefaultAsync(consecion => consecion.Titulo.Equals(titulo) && consecion.ClienteID == clienteId);
            }
            catch (Exception e)
            {
                return new Consecion();
            }
        }
    }
}