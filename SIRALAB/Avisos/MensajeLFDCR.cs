using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SIRALAB.Avisos
{
    public class MensajeLFDCR
    {
        private readonly string _source;
        private int NormaId { get; set; }
        public MensajeLFDCR(string source, int normaId)
        {
            _source = source;
            NormaId = normaId;
        }

        public void Show(Page page, Anexo arg)
        {
            var data = JsonConvert.DeserializeObject<Aviso>(File.ReadAllText(_source));
            if (data.Estados.Any(q => q.Id == arg.Direccion?.EstadoID) && DateTime.Now <= data.FechaFin && data.NormaId == this.NormaId)
                page.PopMensaje(data.Mensaje, "Aviso", controles.Opciones.SoloAceptar, controles.Icono.warning);
        }

        private class Aviso
        {
            public int NormaId { get; set; }
            public DateTime FechaFin { get; set; }
            public string Mensaje { get; set; }
            public List<Catalogo> Estados { get; set; }
        }

        private class Catalogo
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
    }
}