using DL_WS_SIRALAB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.negocio
{
   public class AnexoMedidor_BL:IDisposable
    {
         private Siralab_Entities se { get; set; }
        public AnexoMedidor_BL() { se = new Siralab_Entities(); }

        public List<int> Obten_AnexoMedidor(long? medidor)
        {
            List<int> AnexoMedidorID = new List<int>();
            var anexoC = se.AnexoMedidors.Where(p => p.ID_SERIE_MEDIDOR == medidor && p.FuenteAbastecimiento == true).ToList();
            if (anexoC != null)
                AnexoMedidorID = anexoC.Select(P => P.AnexoID).ToList();
            return AnexoMedidorID;
        }

        #region Destructor
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
        ~AnexoMedidor_BL()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
