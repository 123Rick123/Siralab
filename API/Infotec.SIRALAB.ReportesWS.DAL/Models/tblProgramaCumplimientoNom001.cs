using System;
using System.Collections.Generic;
using System.Text;

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class tblProgramaCumplimientoNom001
    {
        public int Id { get; set; }
        public string RFC { get; set; }
        public int IdTitulo { get; set; }
        public string NombreTitulo { get; set; }
        public int IdAnexo { get; set; }
        public string numeroAnexo { get; set; }
        public string NombreAnexo { get; set; }
        public DateTime Vigencia { get; set; }
    }
}
