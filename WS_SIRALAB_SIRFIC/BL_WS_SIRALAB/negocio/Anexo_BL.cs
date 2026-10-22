using DL_WS_SIRALAB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.negocio
{
    public class Anexo_BL : IDisposable
    {
        private Siralab_Entities se { get; set; }
        public Anexo_BL() { se = new Siralab_Entities(); }

        public DL_WS_SIRALAB.Anexo Obten_Anexo(string titulo, string anexo, int ClienteID)
        {
            var anexoC = se.Anexoes.Where(p => p.Consecion.Titulo == titulo && p.Anexo1 == anexo && p.Consecion.ClienteID == ClienteID).FirstOrDefault();
            return anexoC;
        }

        private bool _isDisposed = false;
        public void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            se.Dispose();
            se = null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~Anexo_BL()
        {
            this.Dispose(false);
        }
    }
}
