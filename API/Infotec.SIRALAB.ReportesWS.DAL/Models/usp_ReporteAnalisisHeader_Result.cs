using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    [Keyless]
    public class usp_ReporteAnalisisHeader_Result
    {
        public int AnalisisIdentificador { get; set; }
        public string AnalisisEnumeracion { get; set; }
        public int OrdenTrabajoIdentificador { get; set; }
        public int LaboratorioIdentificador { get; set; }
        public Nullable<bool> AnalisisMuestraEsCompuesta { get; set; }
        public bool AnalisisFirmado { get; set; }
        public Nullable<System.DateTime> AnalisisFechaFirma { get; set; }
        public Nullable<System.DateTime> AnalisisFechaInicio { get; set; }
        public Nullable<System.DateTime> AnalisisFechaFin { get; set; }
        public string FirmaCadenaOriginal { get; set; }
        public string FirmaDatosFirmados { get; set; }
        public string FirmaDatosSelloDigital { get; set; }
        public string OrdenTrabajoNumeroInforme { get; set; }
        public Nullable<int> OrdenTrabajoHorasProceso { get; set; }
        public string CuerpoReceptorNombre { get; set; }
        public string UsoAguaNombre { get; set; }
        public string NormaNombre { get; set; }
        public Nullable<bool> LaboratorioEsPersonaFisica { get; set; }
        public string LaboratorioRFC { get; set; }
        public string LaboratorioCURP { get; set; }
        public string LaboratorioNombre { get; set; }
        public string LaboratorioApellidoPaterno { get; set; }
        public string LaboratorioApellidoMaterno { get; set; }
        public Nullable<int> ClienteID { get; set; }
        public string ClienteRFC { get; set; }
        public string ClienteCURP { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApellidoPaterno { get; set; }
        public string ClienteApellidoMaterno { get; set; }
        public string PuntoDescargaAnexo { get; set; }
        public string PuntoDescargaLatitud { get; set; }
        public string PuntoDescargaLongitud { get; set; }
        public string PuntoDescargaDatum { get; set; }
        public string PuntoDescargaNombreCuerpoReceptor { get; set; }
        public string PuntoDescargaTitulo { get; set; }
        public string PuntoDescargaDireccionCalle { get; set; }
        public string PuntoDescargaDireccionNumExt { get; set; }
        public string PuntoDescargaDireccionNumInt { get; set; }
        public string PuntoDescargaDireccionColonia { get; set; }
        public string PuntoDescargaDireccionLocalidad { get; set; }
        public string PuntoDescargaDireccionMunicipio { get; set; }
        public string PuntoDescargaDireccionEstado { get; set; }
        public Nullable<int> PuntoDescargaDireccionCP { get; set; }
        public string MuestraLatitud { get; set; }
        public string MuestraLongitud { get; set; }
        public string MuestraDireccion { get; set; }
        public bool EsFuenteAbastecimiento { get; set; }
        public string Aprobacion { get; set; }
        public string AcreditacionEMA { get; set; }
        public Nullable<int> MuestraIdentificador { get; set; }

    }
}
