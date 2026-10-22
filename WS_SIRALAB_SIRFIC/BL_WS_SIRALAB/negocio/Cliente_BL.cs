using DL_WS_SIRALAB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.negocio
{
    public class Cliente_BL : IDisposable
    {
        private Siralab_Entities se { get; set; }
        public Cliente_BL() { se = new Siralab_Entities(); }

        public DL_WS_SIRALAB.Cliente Obten_Cliente(int ClienteID)
        {
            var cliente = se.Clientes.Where(p => p.ClienteID == ClienteID).FirstOrDefault();
            return cliente;
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

        ~Cliente_BL()
        {
            this.Dispose(false);
        }
    }
}
