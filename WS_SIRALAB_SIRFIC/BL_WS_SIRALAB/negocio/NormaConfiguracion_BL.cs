using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL_WS_SIRALAB;

namespace BL_WS_SIRALAB.negocio
{
    public class NormaConfiguracion_BL : IDisposable
    {
        private Siralab_Entities se { get; set; }
        public NormaConfiguracion_BL() { se = new Siralab_Entities(); }

        public List<NormaConfiguracion> Obten_Configuracion(int NormaID, int CuerpoReceptorID, int UsoAguaID, enumNivel Nivel)
        {
            var lNormaConfiguracion = se.NormaConfiguracions;
            int NivelInt = 0;
            switch (Nivel)
            {
                case enumNivel.Simple:
                    {
                        NivelInt = 1;
                    }
                    break;
                case enumNivel.Compuesta:                    
                    {
                        NivelInt = 2;
                    }
                    break;
                case enumNivel.Mensual:
                    {
                        NivelInt = 3;
                    }
                    break;
                case enumNivel.Fuente_Abastecimiento:
                    {
                        NivelInt = 4;
                    }
                    break;
            }
            var Resultado = lNormaConfiguracion.Where(p => p.NormaID == NormaID && p.CuerpoReceptorID == CuerpoReceptorID && p.UsoAguaID == UsoAguaID && p.NivelID == NivelInt).ToList();
            return Resultado;
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

        ~NormaConfiguracion_BL()
        {
            this.Dispose(false);
        }
    }
}
