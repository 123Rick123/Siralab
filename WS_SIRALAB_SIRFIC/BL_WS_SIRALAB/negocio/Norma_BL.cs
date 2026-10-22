using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL_WS_SIRALAB;

namespace BL_WS_SIRALAB.negocio
{
    public class Norma_BL : IDisposable
    {
        private Siralab_Entities se { get; set; }
        public Norma_BL() { se = new Siralab_Entities(); }

        public DL_WS_SIRALAB.Norma Obten_Norma(int NormaID)
        {
            var lNorma = se.Normas.Where(p =>
                    p.NormaID == NormaID
                ).FirstOrDefault();

            return lNorma;
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

        ~Norma_BL()
        {
            this.Dispose(false);
        }

    }
}
