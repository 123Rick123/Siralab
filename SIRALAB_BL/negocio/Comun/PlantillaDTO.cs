using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.Comun
{
    public class PlantillaDTO
    {
        public PlantillaDTO()
        {
            EtiquetasCuerpo = new Dictionary<string, string>();
            EtiquetasEncabezado = new Dictionary<string, string>();
            EtiquetasPiePagina = new Dictionary<string, string>();
        }
        public long Id { get; set; }
        public string Url { get; set; }
        public string QRUrl { get; set; }
        public Dictionary<string, string> EtiquetasCuerpo { get; set; }
        public Dictionary<string, string> EtiquetasEncabezado { get; set; }
        public Dictionary<string, string> EtiquetasPiePagina { get; set; }
        public bool EsNuevoDocumento { get; set; }
        public string Path { get; set; }
        public string RutaPlantilla { get; set; }
    }


}
