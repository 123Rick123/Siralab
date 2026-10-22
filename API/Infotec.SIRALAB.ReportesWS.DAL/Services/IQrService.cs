using System.Collections.Generic;
using System.Text;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public interface IQrService
    {
        string GenerarQR(string cadenaOriginal);
    }
}
