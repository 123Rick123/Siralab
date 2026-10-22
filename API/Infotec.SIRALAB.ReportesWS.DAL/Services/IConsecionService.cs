using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IConsecionService
    {
        Task<Consecion> GetConsecionByTituloAndClienteId(string titulo, int clienteId);
    }
}