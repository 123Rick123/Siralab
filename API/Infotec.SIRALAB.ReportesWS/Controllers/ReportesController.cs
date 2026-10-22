using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Infotec.SIRALAB.ReportesWS.DAL.Models.Enums;
using Infotec.SIRALAB.ReportesWS.DAL.Services;
using Infotec.SIRALAB.ReportesWS.Reportes.Models;
using Infotec.SIRALAB.ReportesWS.Reportes.POCO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Razor.Templating.Core;


namespace Infotec.SIRALAB.ReportesWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAnalisisService _analisisService;
        private readonly IReportesService _reportesService;
        private readonly IUspReporteAnalisisHeader _uspReporteAnalisisHeader;
        private readonly IConfiguration _config;
        private readonly IUspReporteMensualHeader _uspReporteMensualHeader;
        private readonly IConsecionService _consecionService;
        private readonly ILaboratorioAcreditacionService _laboratorioAcreditacionService;
        private readonly ILaboratorioVigenciaService _laboratorioVigenciaService;
        private readonly IQrService _qrService;

        public ReportesController(IWebHostEnvironment environment, IAnalisisService analisisService,
            IReportesService reportesService, IUspReporteAnalisisHeader uspReporteAnalisisHeader,
            IConfiguration config, IUspReporteMensualHeader uspReporteMensualHeader, IConsecionService consecionService,
            ILaboratorioAcreditacionService laboratorioAcreditacionService, ILaboratorioVigenciaService laboratorioVigenciaService,
            IQrService qrService)
        {
            _environment = environment;
            _analisisService = analisisService;
            _reportesService = reportesService;
            _uspReporteAnalisisHeader = uspReporteAnalisisHeader;
            _config = config;
            _uspReporteMensualHeader = uspReporteMensualHeader;
            _consecionService = consecionService;
            _laboratorioAcreditacionService = laboratorioAcreditacionService;
            _laboratorioVigenciaService = laboratorioVigenciaService;
            _qrService = qrService;
        }

        [HttpGet("DescargarPDF/{tipoReporte}")]
        public async Task<IActionResult> DescargarPDF(TipoReporte tipoReporte, TipoEvaluacion? tipoEvaluacion, int? analisisId, int? clienteId, DateTime? fechaInicio, DateTime? fechaFin, string titulo, string anexo, int? normaId)
        {
            MemoryStream stream = null;
            var nombreArchivo = "";
            var reporte = new ReporteRespuesta();
            switch (tipoReporte)
            {
                case TipoReporte.InformeLaboratorio:
                    if (analisisId == null)
                        return NotFound();

                    nombreArchivo = "InformeLaboratorio.pdf";
                    reporte = await GeneraInformeLab(analisisId.Value);
                    stream = new MemoryStream(reporte.Respuesta);
                    break;

                case TipoReporte.AcuseFirma:
                    if (analisisId == null)
                        return NotFound();

                    nombreArchivo = "AcuseFirma.pdf";
                    reporte = await GeneraAcuseFirma(analisisId.Value);
                    stream = new MemoryStream(reporte.Respuesta);
                    break;
                case TipoReporte.InformeMensual:
                    nombreArchivo = "InformeMensual.pdf";
                    //var fechaInicio = new DateTime(2020, 11, 1, 00, 00, 00);
                    //var fechaFin = new DateTime(2020, 12, 1, 00, 00, 00);
                    //clienteId = 7973
                    //titulo = "02SON101327/09FRDA14"
                    //anexo = "4.1"
                    //normaId = 1
                    //tipoEvaluacion = valor del enum.

                    if (tipoEvaluacion != null && clienteId != null && fechaInicio != null && fechaFin != null && !string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(anexo) && normaId != null)
                    {
                        reporte = await GeneraInformeMensual(clienteId.Value, fechaInicio.Value, fechaFin.Value, titulo, anexo, normaId.Value, tipoEvaluacion.Value);
                        stream = new MemoryStream(reporte.Respuesta);
                        break;
                    }
                    return NotFound();
                default:
                    return NotFound();
            }

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = nombreArchivo
            };
        }

        //[HttpGet("GeneraInformeLab/{analisisId}")]
        //public async Task<ReporteRespuesta> GeneraInformeLab(int analisisId)
        //{
        //    var html = "";
        //    try
        //    {
        //        var path = _environment.ContentRootFileProvider.GetFileInfo("images/EncCorreo.png")?.PhysicalPath;
        //        var analisis = await _analisisService.GetAnalisis(analisisId);
        //        var pathcCss = _config.GetSection("Configuraciones").GetSection("pathCss").Value;
        //        var pathInformeLaboratorio = _config.GetSection("Configuraciones").GetSection("pathInformeLaboratorio").Value;
        //        var parametros = _analisisService.GetParametros();
        //        var ValidaPrograma = _config.GetSection("Configuraciones").GetSection("ValidaPrograma").Value;

        //        if (analisis.OrdenTrabajo.Norma.Nom2021 == true)
        //        {
        //            var muestraCompuesta = analisis.Muestras.Where(muestra => muestra.NivelID == 2);
        //            var muestrasSimples = analisis.Muestras.Where(muestra => muestra.NivelID == 1);
        //            foreach (var item in muestraCompuesta)
        //            {
        //                var lista = item.MuestraValors.ToList();
        //                for (int i = lista.Count - 1; i >= 0; i--)
        //                {
        //                    var idCuerpoReceptor = analisis.OrdenTrabajo.CuerpoReceptorID;
        //                    var estaActivo = analisis.OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(nc => nc.ParametroID == lista[i].ParametroID && nc.NivelID == 2 && nc.CuerpoReceptorID == idCuerpoReceptor)?.Activo;
        //                    if (estaActivo == false)
        //                    {
        //                        lista.RemoveAt(i);
        //                    }
        //                }
        //                item.MuestraValors = lista;
        //            }
        //            //Esto solo para la norma SST,DQO/COT
        //            var NombreNorma = analisis.OrdenTrabajo.Norma.Nombre.Replace(" ","");
                   
        //            if (NombreNorma == _config.GetSection("Configuraciones").GetSection("normaCOT").Value || analisis.OrdenTrabajo.NormaID==15
        //                || NombreNorma.ToUpper().Replace(" ", "") == _config.GetSection("Configuraciones").GetSection("NormaFuenteAbastecimiento2021").Value.Replace(" ", ""))
        //            {
        //                foreach (var item in muestraCompuesta)
        //                {
        //                    var TipoDeParametro = analisis.NormaDQOCOT;
        //                    var Conductividad = analisis.Conductividad;
        //                    var listaMuestraValors = item.MuestraValors.ToList();
        //                    for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
        //                    {
        //                        //Si el tipo de parametro se hizo para cot se remueve DQO y si se hizo para DQO remueve COT RGG
        //                        if (TipoDeParametro != null)
        //                        {
        //                            if (TipoDeParametro == "COT")
        //                            {
        //                                if (listaMuestraValors[i].ParametroID == 667)// el valor es de DQO
        //                                {
        //                                    listaMuestraValors.RemoveAt(i);
        //                                }
        //                            }
        //                            else if (TipoDeParametro == "DQO")
        //                            {
        //                                if (listaMuestraValors[i].ParametroID == 553 || listaMuestraValors[i].ParametroID == 602) //Los dos valores indican el id del parametro COT
        //                                {
        //                                    listaMuestraValors.RemoveAt(i);
        //                                }
        //                            }
        //                        }
        //                        if (Conductividad != null && Conductividad != string.Empty)
        //                        {
        //                            if (Conductividad == "Escherichia")
        //                            {
        //                                if (listaMuestraValors[i].ParametroID == 689)
        //                                {
        //                                    listaMuestraValors.RemoveAt(i);
        //                                }

        //                            }
        //                            else if (Conductividad == "Enterococos")
        //                            {
        //                                if (listaMuestraValors[i].ParametroID == 694)
        //                                {
        //                                    listaMuestraValors.RemoveAt(i);
        //                                }
        //                            }
        //                        }
        //                    }

        //                    item.MuestraValors = listaMuestraValors;
        //                }


        //                foreach (var item in muestrasSimples)
        //                {
                          
        //                    var Conductividad = analisis.Conductividad;
        //                    var listaMuestraValors = item.MuestraValors.ToList();
        //                    for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
        //                    {
        //                        //Si el tipo de parametro se hizo para Escherichia se remueve Enterococos y si se hizo para Enterococos remueve Escherichia RGG
                 
        //                        if (Conductividad != null && Conductividad != string.Empty)
        //                        {
        //                            if (Conductividad == "Escherichia")
        //                            {
        //                                if (listaMuestraValors[i].ParametroID == 689)
        //                                {
        //                                    listaMuestraValors.RemoveAt(i);
        //                                }

        //                            }
        //                            else if (Conductividad == "Enterococos")
        //                            {
        //                                if (listaMuestraValors[i].ParametroID == 694)
        //                                {
        //                                    listaMuestraValors.RemoveAt(i);
        //                                }
        //                            }
        //                        }
        //                    }

        //                    item.MuestraValors = listaMuestraValors;
        //                }

        //            }
        //        }


        //        var msjNom001 = string.Empty;
        //        var ValidaAnioPrograma = int.Parse(_config.GetSection("Configuraciones").GetSection("ValidaAnioPrograma").Value);
        //        var anioDeAplicacionMensaje = analisis.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha.Value.Year;
        //        //var RequierePrograma = _analisisService.RequierePrograma();
        //        if (ValidaPrograma == "true" && anioDeAplicacionMensaje >= ValidaAnioPrograma)
        //        {
        //            var EstaInscritoNom001 = _analisisService.ObtieneDatosInscritosNom001(analisis.Anexo.Anexo1, analisis.Consecion.Titulo, analisis.OrdenTrabajo.Cliente.RFC);
        //            if (EstaInscritoNom001)
        //            {
        //                msjNom001 = "El contribuyente " + analisis.OrdenTrabajo.Cliente.Nombre + " con RFC " + analisis.OrdenTrabajo.Cliente.RFC + " CUENTA con programa para el cumplimiento vigente, establecido en el artículo cuarto transitorio de la Norma Oficial Mexicana “NOM-001-SEMARNAT-2021, Que establece los límites permisibles de contaminantes en las descargas de aguas residuales en cuerpos receptores propiedad de la nación.”";

        //            }
        //            else
        //            {
        //                msjNom001 = "El contribuyente " + analisis.OrdenTrabajo.Cliente.Nombre + " con RFC " + analisis.OrdenTrabajo.Cliente.RFC + " NO CUENTA con programa para el cumplimiento vigente, establecido en el artículo cuarto transitorio de la Norma Oficial Mexicana “NOM-001-SEMARNAT-2021, Que establece los límites permisibles de contaminantes en las descargas de aguas residuales en cuerpos receptores propiedad de la nación.”";

        //            }
        //        }
             
        //        var model = new InformeDTO()
        //        {
        //            Titulo = "Informe analisis - " + analisisId,
        //            Logo = path,
        //            DatosGenerales = await  _uspReporteAnalisisHeader.Obten(analisisId),
        //            Analisis = analisis,
        //            Css = pathcCss,
        //            numPD = await _analisisService.GetNumeroPD(analisis.OrdenTrabajoID, analisisId),
        //            QR = analisis.Firmada ? _qrService.GenerarQR(analisis.AnalisisFirmas.FirstOrDefault()?.Cadena) : "",
        //            mensajeNom001 = msjNom001
        //        };
        //        html = await RazorTemplateEngine.RenderAsync(pathInformeLaboratorio, model);
        //    }
        //    catch (Exception e)
        //    {
        //        html = "<p>Ocurrió un error al generar este reporte<p>";
        //    }

        //    return new ReporteRespuesta() { Respuesta = _reportesService.CreatePDF(html) };
        //}

        [HttpGet("GeneraInformeLab/{analisisId}")]
        public async Task<ReporteRespuesta> GeneraInformeLab(int analisisId)
        {
            var html = "";
            try
            {
                var path = _environment.ContentRootFileProvider.GetFileInfo("images/EncCorreo.png")?.PhysicalPath;
                var analisis = await _analisisService.GetAnalisis(analisisId);
                var pathcCss = _config.GetSection("Configuraciones").GetSection("pathCss").Value;
                var pathInformeLaboratorio = _config.GetSection("Configuraciones").GetSection("pathInformeLaboratorio").Value;
                var parametros = _analisisService.GetParametros();
                var ValidaPrograma = _config.GetSection("Configuraciones").GetSection("ValidaPrograma").Value;

                if (analisis.OrdenTrabajo.Norma.Nom2021 == true)
                {
                    var muestraCompuesta = analisis.Muestras.Where(muestra => muestra.NivelID == 2);
                    var muestrasSimples = analisis.Muestras.Where(muestra => muestra.NivelID == 1);
                    foreach (var item in muestraCompuesta)
                    {
                        var lista = item.MuestraValors.ToList();
                        for (int i = lista.Count - 1; i >= 0; i--)
                        {
                            var idCuerpoReceptor = analisis.OrdenTrabajo.CuerpoReceptorID;
                            var estaActivo = analisis.OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(nc => nc.ParametroID == lista[i].ParametroID && nc.NivelID == 2 && nc.CuerpoReceptorID == idCuerpoReceptor)?.Activo;
                            if (estaActivo == false)
                            {
                                lista.RemoveAt(i);
                            }
                        }
                        item.MuestraValors = lista;
                    }
                    //Esto solo para la norma SST,DQO/COT
                    var NombreNorma = analisis.OrdenTrabajo.Norma.Nombre.Replace(" ", "");

                    if (NombreNorma == _config.GetSection("Configuraciones").GetSection("normaCOT").Value || analisis.OrdenTrabajo.NormaID == 15
                        || NombreNorma.ToUpper().Replace(" ", "") == _config.GetSection("Configuraciones").GetSection("NormaFuenteAbastecimiento2021").Value.Replace(" ", ""))
                    {
                        foreach (var item in muestraCompuesta)
                        {
                            var TipoDeParametro = analisis.NormaDQOCOT;
                            var Conductividad = analisis.Conductividad;
                            var listaMuestraValors = item.MuestraValors.ToList();
                            for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
                            {
                                //Si el tipo de parametro se hizo para cot se remueve DQO y si se hizo para DQO remueve COT RGG
                                if (TipoDeParametro != null)
                                {
                                    if (TipoDeParametro == "COT")
                                    {
                                        if (listaMuestraValors[i].ParametroID == 667)// el valor es de DQO
                                        {
                                            listaMuestraValors.RemoveAt(i);
                                        }
                                    }
                                    else if (TipoDeParametro == "DQO")
                                    {
                                        if (listaMuestraValors[i].ParametroID == 553 || listaMuestraValors[i].ParametroID == 602) //Los dos valores indican el id del parametro COT
                                        {
                                            listaMuestraValors.RemoveAt(i);
                                        }
                                    }
                                }
                                if (Conductividad != null && Conductividad != string.Empty)
                                {
                                    if (Conductividad == "Escherichia")
                                    {
                                        if (listaMuestraValors[i].ParametroID == 689)
                                        {
                                            listaMuestraValors.RemoveAt(i);
                                        }

                                    }
                                    else if (Conductividad == "Enterococos")
                                    {
                                        if (listaMuestraValors[i].ParametroID == 694)
                                        {
                                            listaMuestraValors.RemoveAt(i);
                                        }
                                    }
                                }
                            }

                            item.MuestraValors = listaMuestraValors;
                        }


                        foreach (var item in muestrasSimples)
                        {

                            var Conductividad = analisis.Conductividad;
                            var listaMuestraValors = item.MuestraValors.ToList();
                            for (int i = listaMuestraValors.Count - 1; i >= 0; i--)
                            {
                                //Si el tipo de parametro se hizo para Escherichia se remueve Enterococos y si se hizo para Enterococos remueve Escherichia RGG

                                if (Conductividad != null && Conductividad != string.Empty)
                                {
                                    if (Conductividad == "Escherichia")
                                    {
                                        if (listaMuestraValors[i].ParametroID == 689)
                                        {
                                            listaMuestraValors.RemoveAt(i);
                                        }

                                    }
                                    else if (Conductividad == "Enterococos")
                                    {
                                        if (listaMuestraValors[i].ParametroID == 694)
                                        {
                                            listaMuestraValors.RemoveAt(i);
                                        }
                                    }
                                }
                            }

                            item.MuestraValors = listaMuestraValors;
                        }

                    }
                }


                var msjNom001 = string.Empty;
                var ValidaAnioPrograma = int.Parse(_config.GetSection("Configuraciones").GetSection("ValidaAnioPrograma").Value);
                var anioDeAplicacionMensaje = analisis.Muestras.LastOrDefault(x => x.NivelID == 1).Fecha.Value.Year;
                //var RequierePrograma = _analisisService.RequierePrograma();
                if (ValidaPrograma == "true" && anioDeAplicacionMensaje >= ValidaAnioPrograma)
                {
                    var EstaInscritoNom001 = _analisisService.ObtieneDatosInscritosNom001(analisis.Anexo.Anexo1, analisis.Consecion.Titulo, analisis.OrdenTrabajo.Cliente.RFC);
                    if (EstaInscritoNom001)
                    {
                        msjNom001 = "El contribuyente " + analisis.OrdenTrabajo.Cliente.Nombre + " con RFC " + analisis.OrdenTrabajo.Cliente.RFC + " CUENTA con programa para el cumplimiento vigente, establecido en el artículo cuarto transitorio de la Norma Oficial Mexicana “NOM-001-SEMARNAT-2021, Que establece los límites permisibles de contaminantes en las descargas de aguas residuales en cuerpos receptores propiedad de la nación.”";

                    }
                    else
                    {
                        msjNom001 = "El contribuyente " + analisis.OrdenTrabajo.Cliente.Nombre + " con RFC " + analisis.OrdenTrabajo.Cliente.RFC + " NO CUENTA con programa para el cumplimiento vigente, establecido en el artículo cuarto transitorio de la Norma Oficial Mexicana “NOM-001-SEMARNAT-2021, Que establece los límites permisibles de contaminantes en las descargas de aguas residuales en cuerpos receptores propiedad de la nación.”";

                    }
                }

                var DatosGenerales = _uspReporteAnalisisHeader.ObtenDatosGenerales(analisisId);
                var model = new InformeDTO()
                {
                    Titulo = "Informe analisis - " + analisisId,
                    Logo = path,
                    DatosGenerales = DatosGenerales,
                    Analisis = analisis,
                    Css = pathcCss,
                    numPD = await _analisisService.GetNumeroPD(analisis.OrdenTrabajoID, analisisId),
                    QR = analisis.Firmada ? _qrService.GenerarQR(analisis.AnalisisFirmas.FirstOrDefault()?.Cadena) : "",
                    mensajeNom001 = msjNom001
                };
                html = await RazorTemplateEngine.RenderAsync(pathInformeLaboratorio, model);
            }
            catch (Exception e)
            {
                html = "<p>Ocurrió un error al generar este reporte<p>";
            }

            return new ReporteRespuesta() { Respuesta = _reportesService.CreatePDF(html) };
        }

        [HttpGet("GeneraAcuseFirma/{analisisId}")]
        public async Task<ReporteRespuesta> GeneraAcuseFirma(int analisisId)
        {
            var html = "";
            try
            {
                var path = _environment.ContentRootFileProvider.GetFileInfo("images/EncCorreo.png")?.PhysicalPath;
                var analisis = await _analisisService.GetAnalisis(analisisId);
                var pathcCss = _config.GetSection("Configuraciones").GetSection("pathCss").Value;
                var pathAcuseFirma = _config.GetSection("Configuraciones").GetSection("pathAcuseFirma").Value;

                var model = new InformeDTO()
                {
                    Titulo = "Acuse Firma",
                    Logo = path,
                    DatosGenerales = await _uspReporteAnalisisHeader.Obten(analisisId),
                    Analisis = analisis,
                    Css = pathcCss,
                    numPD = await _analisisService.GetNumeroPD(analisis.OrdenTrabajoID, analisisId),
                    QR = analisis.Firmada ? _qrService.GenerarQR(analisis.AnalisisFirmas.FirstOrDefault()?.Cadena) : "",
                    ReglaMiscelanea = _config.GetSection("Configuraciones").GetSection("reglaMiscelanea").Value
                };
                html = await RazorTemplateEngine.RenderAsync(pathAcuseFirma, model);
            }
            catch (Exception e)
            {
                html = "<p>Ocurrió un error al generar este reporte<p>";
            }

            return new ReporteRespuesta() { Respuesta = _reportesService.CreatePDF(html) };
        }


        [HttpGet("GeneraInformeMensual")]
        public async Task<ReporteRespuesta> GeneraInformeMensual(int clienteId, DateTime fechaInicio, DateTime fechaFin, string titulo, string anexo, int normaId, TipoEvaluacion evaluacion)
        {
            var html = "";
            try
            {
                var path = _environment.ContentRootFileProvider.GetFileInfo("images/EncCorreo.png")?.PhysicalPath;
                var pathcCss = _config.GetSection("Configuraciones").GetSection("pathCss").Value;
                var pathInformeMensual = _config.GetSection("Configuraciones").GetSection("pathInformeMensual").Value;
                var concesion = await _consecionService.GetConsecionByTituloAndClienteId(titulo, clienteId);
                var anexoObj = concesion.Anexos.FirstOrDefault(a => a.Anexo1 == anexo);
                var analisisList = await _analisisService.GetAnalisisByPeriodo(concesion.ConsecionID, anexoObj?.AnexoID, normaId, fechaInicio, fechaFin);


                foreach (var analisi in analisisList)
                {
                    analisi.Laboratorio.LaboratorioAcreditacions = await _laboratorioAcreditacionService.GetByLaboratorioId(analisi.LaboratorioID) as ICollection<LaboratorioAcreditacion>;
                    analisi.Laboratorio.LaboratorioVigencia = await _laboratorioVigenciaService.GetByLaboratorioId(analisi.LaboratorioID) as ICollection<LaboratorioVigencium>;
                }


                var cuerpoReceptorId = analisisList.FirstOrDefault()?.OrdenTrabajo.CuerpoReceptorID;
                var usoAguaId = analisisList.FirstOrDefault()?.OrdenTrabajo.UsoAguaID;

                var model = new InformeMensualDTO()
                {
                    Titulo = "Informe Mensual",
                    Logo = path,
                    Css = pathcCss,
                    UspReporteMensualHeader = await _uspReporteMensualHeader.Obten(clienteId, normaId, anexoObj?.AnexoID, cuerpoReceptorId, usoAguaId),
                    Analisis = analisisList,
                    AnalisisPD = _analisisService.GetResultadosPD(analisisList, evaluacion),
                    CalculoMensual = await _analisisService.ObtenEvaluacion(clienteId, normaId, anexoObj.AnexoID, cuerpoReceptorId.Value, usoAguaId.Value, 0, fechaInicio, fechaFin, evaluacion, analisisList),
                    TipoEvaluacion = evaluacion
                };

                html = await RazorTemplateEngine.RenderAsync(pathInformeMensual, model);

            }
            catch (Exception e)
            {
                html = "<p>Ocurrió un error al generar este reporte<p>";
            }

            return new ReporteRespuesta() { Respuesta = _reportesService.CreatePDF(html) };
        }


    }
}
