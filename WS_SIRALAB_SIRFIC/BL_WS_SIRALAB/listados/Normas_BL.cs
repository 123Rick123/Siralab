using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL_WS_SIRALAB;

namespace BL_WS_SIRALAB.listados
{
    public class Normas_BL
    {
        private Siralab_Entities se { get; set; }
        public Normas_BL() { 
        }

        public List<BL_WS_SIRALAB.Norma> Obten_Normas()
        {
            using (var se = new Siralab_Entities())
            {
                var lNormas = se.Normas.Select(q => new BL_WS_SIRALAB.Norma
                {
                    id = q.NormaID,
                    Nombre = q.Nombre,
                    Descripcion = q.Descripcion,
                    Vigente = q.Activo
                }).ToList();
                return lNormas;
            }            
        }

        //private bool _isDisposed = false;
        //public void Dispose(bool disposing)
        //{
        //    if (_isDisposed)
        //    {
        //        return;
        //    }
        //    _isDisposed = true;
        //    if (disposing)
        //    {
        //        GC.SuppressFinalize(this);
        //    }
        //    se.Dispose();
        //    se = null;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //}

        //~Normas_BL()
        //{
        //    this.Dispose(false);
        //}
    }
}
