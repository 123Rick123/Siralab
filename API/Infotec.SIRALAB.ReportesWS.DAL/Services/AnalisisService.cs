using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Infotec.SIRALAB.ReportesWS.DAL.Models.DAO;
using Infotec.SIRALAB.ReportesWS.DAL.Models.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class AnalisisService : IAnalisisService
    {
        private readonly SiralabContext _siralabContext;
        private readonly IConfiguration _configuration;
        private readonly IUspReporteMensualCalculoService _calculoService;
       
        public AnalisisService(SiralabContext siralabContext, IUspReporteMensualCalculoService calculoService, IConfiguration configuration)
        {
            _siralabContext = siralabContext;
            _calculoService = calculoService;
            _configuration = configuration;
        }
      

      
        public  List<Parametro> GetParametros()
        {
            return  _siralabContext.Parametros.ToList();
        }
        public async Task<Analisi> GetAnalisis(int analisisId)
        {   
            return await _siralabContext.Analises
                .Include(a => a.Muestras)
                .ThenInclude(m => m.MuestraValors)
                .ThenInclude(mv => mv.ParametroMetodo)
                .ThenInclude(pm => pm.Metodo)
                .Include(a => a.Muestras)
                .ThenInclude(m => m.MuestraValors)
                .ThenInclude(mv => mv.Laboratorio)
                .Include(a => a.Muestras)
                .ThenInclude(m => m.Muestreadors)
                .ThenInclude(m => m.LaboratorioSignatario)
                .ThenInclude(ls => ls.Persona)
                .Include(a => a.Muestras)
                .ThenInclude(m => m.Muestreadors)
                .ThenInclude(m => m.LaboratorioSignatario)
                .ThenInclude(ls => ls.Laboratorio)
                .Include(a => a.AnalisisFirmas)
                .Include(a => a.OrdenTrabajo)
                .ThenInclude(ot => ot.Norma)
                .ThenInclude(n => n.NormaConfiguracions)
                .Include(a => a.OrdenTrabajo).ThenInclude(ot => ot.Cliente)
                .Include(a => a.Laboratorio)
                .Include(a=> a.Anexo)
                .Include(a=>a.Consecion)
                .FirstOrDefaultAsync(a => a.AnalisisID == analisisId);
        }

        public async Task<List<Analisi>> GetAnalisisByPeriodo(int? consecionId, int? anexoId, int normaId, DateTime fechaInicio, DateTime fechaFin)
        {
            var analisisList = await _siralabContext.Analises
                .Include(a => a.Muestras)
                .ThenInclude(m => m.MuestraValors)
                .Include(a => a.AnalisisFirmas)
                .Include(a => a.OrdenTrabajo)
                .ThenInclude(ot => ot.Norma)
                .ThenInclude(n => n.NormaConfiguracions)
                .Include(a => a.Laboratorio)
                .Where(a =>
                    a.OrdenTrabajo.ConsecionID == consecionId
                    && a.OrdenTrabajo.NormaID == normaId
                    && a.AnexoID == anexoId
                    && a.Muestras.FirstOrDefault().Fecha >= fechaInicio
                    && a.Muestras.FirstOrDefault().Fecha <= fechaFin
                    && a.Firmada
                    && !a.FaltaPago
                    && a.EstatusID == 3 //Estatus activo.
                    && !a.Procesado)
                .ToListAsync();

            
            return analisisList;
        }



        public List<AnalisisPDInformeMensual> GetResultadosPD(List<Analisi> analisisList, TipoEvaluacion tipoEvaluacion)
        {
            var requiereCompuesta = analisisList.FirstOrDefault()?.OrdenTrabajo.Norma.RequiereCompuesta;
            var parametrosMS = analisisList.FirstOrDefault()?.Muestras.FirstOrDefault(m => m.NivelID == 1)?.MuestraValors.Select(mv => mv.ParametroID);
            var parametrosMC = analisisList.FirstOrDefault()?.Muestras.FirstOrDefault(m => m.NivelID == 2)?.MuestraValors.Where(mv => !parametrosMS.Contains(mv.ParametroID)).Select(mv => mv.ParametroID);
            var etiquetasDictionary = GetEtiquetas(parametrosMS, analisisList.FirstOrDefault(), NivelMuestra.Simple);
            var etiquetasMC = GetEtiquetas(parametrosMC, analisisList.FirstOrDefault(), NivelMuestra.Compuesta);
            var listaAnalisisPD = new List<AnalisisPDInformeMensual>();
            var etiqueta = "";

            foreach (var analisis in analisisList)
            {
                listaAnalisisPD.AddRange(EstableceCaudal(analisis));

                //Añadir muestra simple:
                foreach (var parametroId in parametrosMS)
                {
                    etiqueta = etiquetasDictionary.FirstOrDefault(d => d.Key == parametroId).Value;
                    foreach (var muestra in analisis.Muestras.Where(m => m.NivelID == 1))
                    {
                        var mv = muestra.MuestraValors.FirstOrDefault(mv =>
                            mv.MuestraID == muestra.MuestraID && mv.ParametroID == parametroId);

                        var analisisResultado = new AnalisisPDInformeMensual()
                        {
                            AnalisisId = analisis.AnalisisID,
                            NivelId = 1,
                            Etiqueta = etiqueta + " " + (muestra.Analisis.Muestras.Where(m => m.NivelID == 1).ToList().FindIndex(m => m.MuestraID == muestra.MuestraID) + 1),
                            Resultado = ObtenResultado(mv),
                            LMP = ObtenLimite(mv, tipoEvaluacion),
                            Cumple = ObtenCumplimiento(mv, tipoEvaluacion)
                        };
                        listaAnalisisPD.Add(analisisResultado);


                        if (analisis.Muestras.Where(m => m.NivelID == 1).ToList().LastOrDefault()?.MuestraID == muestra.MuestraID)
                        {
                            var muestraValor = analisis.Muestras.FirstOrDefault(m => m.NivelID == 2)?.MuestraValors.FirstOrDefault(mv => mv.ParametroID == parametroId);

                            if (muestraValor != null)
                            {
                                var analisisResultadoMsMC = new AnalisisPDInformeMensual()
                                {
                                    AnalisisId = analisis.AnalisisID,
                                    NivelId = 1,
                                    Etiqueta = etiqueta + ObtenOperacion(muestraValor?.OperacionID),
                                    Resultado = ObtenResultado(muestraValor),
                                    LMP = ObtenLimite(muestraValor, tipoEvaluacion),
                                    Cumple = ObtenCumplimiento(muestraValor, tipoEvaluacion)
                                };
                                listaAnalisisPD.Add(analisisResultadoMsMC);
                            }
                        }
                    }
                }

                //Añadir muestra compuesta:
                if (requiereCompuesta ?? true)
                {
                    var muestraCompuesta = analisis.Muestras.FirstOrDefault(m => m.NivelID == 2);
                    foreach (var parametroId in parametrosMC)
                    {
                        etiqueta = etiquetasMC.FirstOrDefault(d => d.Key == parametroId).Value;

                        var analisisResultadoMC = new AnalisisPDInformeMensual()
                        {
                            AnalisisId = analisis.AnalisisID,
                            NivelId = 2,
                            Etiqueta = etiqueta,
                            Resultado = ObtenResultado(muestraCompuesta.MuestraValors.SingleOrDefault(mv => mv.ParametroID == parametroId)),
                            LMP = ObtenLimite(muestraCompuesta.MuestraValors.SingleOrDefault(mv => mv.ParametroID == parametroId), tipoEvaluacion),
                            Cumple = ObtenCumplimiento(muestraCompuesta.MuestraValors.SingleOrDefault(mv => mv.ParametroID == parametroId), tipoEvaluacion)
                        };
                        listaAnalisisPD.Add(analisisResultadoMC);
                    }
                }
            }
            return listaAnalisisPD;
        }

        public async Task<List<AnalisisPDInformeMensual>> ObtenEvaluacion(int clienteId, int normaId, int anexoId, int cuerpoReceptorId, int usoAguaId, decimal caudal, DateTime fechaInicio, DateTime fechaFin, TipoEvaluacion tipoEvaluacion, List<Analisi> analisisList)
        {
            try
            {
                var resultados = await _calculoService.Obten(clienteId, normaId, anexoId, cuerpoReceptorId, usoAguaId, 0, fechaInicio, fechaFin, tipoEvaluacion);
                var normaConfiguraciones = analisisList.FirstOrDefault()?.OrdenTrabajo.Norma.NormaConfiguracions
                    .Where(nc =>
                        nc.NormaID == normaId && nc.NivelID == 2 && nc.CuerpoReceptorID == cuerpoReceptorId &&
                        nc.UsoAguaID == usoAguaId).OrderBy(nc => nc.Orden);



                

                resultados = tipoEvaluacion == TipoEvaluacion.Evaluar_LFD ? resultados.Where(r => r.Evalua && r.LFD).ToList() : resultados.Where(r => r.Evalua).ToList();

                if (normaConfiguraciones != null && resultados.Any())
                {
                    return (from normaConfiguracion in normaConfiguraciones
                            select resultados.FirstOrDefault(r => r.ParmetroID == normaConfiguracion.ParametroID)
                        into resultado
                            where resultado != null
                            select new AnalisisPDInformeMensual()
                            {
                                AnalisisId = 0,
                                NivelId = 3,
                                Etiqueta = normaConfiguraciones != null
                                    ? normaConfiguraciones?.FirstOrDefault(nc => nc.ParametroID == resultado.ParmetroID)
                                        ?.Etiqueta
                                    : resultado.ParametroNombre,
                                Resultado = ObtenResultadoMensual(resultado, analisisList, normaId, cuerpoReceptorId, usoAguaId),
                                LMP = ObtenLMPMensual(tipoEvaluacion, resultado),
                                Cumple = tipoEvaluacion != TipoEvaluacion.NoEvaluar ? resultado.EsValido ? "Sí" : "No" : "N/A"
                            }).ToList();
                }

                return new List<AnalisisPDInformeMensual>();
            }
            catch (Exception e)
            {
                return new List<AnalisisPDInformeMensual>();
            }

        }

        public async Task<int> GetNumeroPD(int ordenTrabajoId, int analisisId)
        {
            var analisisList = await _siralabContext.Analises.Where(a => a.OrdenTrabajoID == ordenTrabajoId).OrderBy(a => a.AnalisisID).ToListAsync();

            return analisisList.FindIndex(a => a.AnalisisID == analisisId) + 1;
        }


        #region Métodos auxiliares

        private Dictionary<int, string> GetEtiquetas(IEnumerable<int> parametrosIdList, Analisi analisis, NivelMuestra nivel)
        {
            var diccionario = new Dictionary<int, string>();

            foreach (var parametro in parametrosIdList)
            {
                var cuerpoReceptorId = analisis.OrdenTrabajo.CuerpoReceptorID;
                var usoAguaId = analisis.OrdenTrabajo.UsoAguaID;
                var normaId = analisis.OrdenTrabajo.NormaID;

                var etiqueta = analisis.OrdenTrabajo.Norma.NormaConfiguracions.FirstOrDefault(nc =>
                    nc.NormaID == normaId.Value
                    && nc.CuerpoReceptorID == cuerpoReceptorId.Value
                    && nc.UsoAguaID == usoAguaId.Value
                    && nc.NivelID == (int)nivel
                    && nc.ParametroID == parametro).Etiqueta;

                diccionario.Add(parametro, etiqueta);
            }

            return diccionario;
        }

        
        private string ObtenResultado(MuestraValor muestraValor)
        {

            if (muestraValor != null && (muestraValor.Muestra.Caudal > 0 || !RequiereCaudal(muestraValor)))
            {
                if (muestraValor.AceptaValor ?? true)
                {
                    var nfi = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ".",
                        NumberGroupSeparator = ",",
                        NumberDecimalDigits = muestraValor.EscalaDecimal ?? 4
                    };

                    return muestraValor.Valor?.ToString("N", nfi);
                }

                return muestraValor.ValorValidacion == false ? "Presente" : "Ausente";
            }

            return "---";
        }

        private string ObtenLimite(MuestraValor muestraValor, TipoEvaluacion tipoEvaluacion)
        {
            if (tipoEvaluacion == TipoEvaluacion.NoEvaluar)
                return "N/A";

            if (muestraValor.Muestra.Caudal > 0 || !RequiereCaudal(muestraValor))
            {
                if (muestraValor.Evalua == false || muestraValor.NoAnalizado || muestraValor.Exento)
                    return "N/A";

                if (muestraValor.AceptaValor ?? true)
                {
                    var nfi = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ".",
                        NumberGroupSeparator = ",",
                        NumberDecimalDigits = muestraValor.EscalaDecimal ?? 4
                    };

                    switch (tipoEvaluacion)
                    {
                        case TipoEvaluacion.Evaluar_Simple: //Se evalúan los límites
                            if (muestraValor.LimiteInferior != null)
                                return muestraValor.LimiteInferior?.ToString("N", nfi) + "-" + muestraValor.LimiteSuperior?.ToString("N", nfi);

                            return muestraValor.LimiteSuperior?.ToString("N", nfi);
                        case TipoEvaluacion.Evaluar_LFD: //Se evalúa LFD
                            if (muestraValor.LFD)
                            {
                                if (muestraValor.LimiteInferiorLFD != null)
                                    return muestraValor.LimiteInferior?.ToString("N", nfi) + "-" + muestraValor.LimiteSuperior?.ToString("N", nfi);

                                return muestraValor.LimiteSuperiorLFD?.ToString("N", nfi);
                            }
                            return "N/A";
                        case TipoEvaluacion.Evaluar_LFD_Simple: //Se evalúa LFD y Limites

                            if (muestraValor.LFD)
                            {
                                if (muestraValor.LimiteInferiorLFD != null)
                                    return muestraValor.LimiteInferior?.ToString("N", nfi) + "-" + muestraValor.LimiteSuperior?.ToString("N", nfi);

                                return muestraValor.LimiteSuperior?.ToString("N", nfi);
                            }

                            if (muestraValor.LimiteInferior != null)
                                return muestraValor.LimiteInferior?.ToString("N", nfi) + "-" + muestraValor.LimiteSuperior?.ToString("N", nfi);

                            return muestraValor.LimiteSuperior?.ToString("N", nfi);
                        default:
                            return "N/A";
                    }

                }

                return tipoEvaluacion == TipoEvaluacion.Evaluar_LFD ? muestraValor.LFD ? "Ausente" : "N/A" : "Ausente";
            }

            return "---";
        }


        private string ObtenCumplimiento(MuestraValor muestraValor, TipoEvaluacion tipoEvaluacion)
        {
            if (tipoEvaluacion == TipoEvaluacion.NoEvaluar)
                return "N/A";

            if (muestraValor.Muestra.Caudal > 0 || !RequiereCaudal(muestraValor))
            {
                if (muestraValor.Evalua == false || muestraValor.NoAnalizado || muestraValor.Exento)
                    return "N/A";

                if (muestraValor.AceptaValor ?? true)
                {
                    switch (tipoEvaluacion)
                    {
                        case TipoEvaluacion.Evaluar_Simple: //Se evalúan los límites
                            if (muestraValor.LimiteInferior != null)
                                return muestraValor.Valor >= muestraValor.LimiteInferior && muestraValor.Valor <= muestraValor.LimiteSuperior ? "Sí" : "No";

                            return muestraValor.Valor <= muestraValor.LimiteSuperior ? "Sí" : "No";
                        case TipoEvaluacion.Evaluar_LFD: //Se evalúa LFD
                            if (muestraValor.LFD)
                            {
                                if (muestraValor.LimiteInferiorLFD != null)
                                    return muestraValor.Valor >= muestraValor.LimiteInferiorLFD && muestraValor.Valor <= muestraValor.LimiteSuperiorLFD ? "Sí" : "No";

                                return muestraValor.Valor <= muestraValor.LimiteSuperiorLFD ? "Sí" : "No"; ;
                            }
                            return "N/A";
                        case TipoEvaluacion.Evaluar_LFD_Simple: //Se evalúa LFD y Limites

                            if (muestraValor.LFD)
                            {
                                if (muestraValor.LimiteInferiorLFD != null)
                                    return muestraValor.Valor >= muestraValor.LimiteInferiorLFD && muestraValor.Valor <= muestraValor.LimiteSuperiorLFD ? "Sí" : "No";

                                return muestraValor.Valor <= muestraValor.LimiteSuperiorLFD ? "Sí" : "No";
                            }

                            if (muestraValor.LimiteInferior != null)
                                return muestraValor.Valor >= muestraValor.LimiteInferior && muestraValor.Valor <= muestraValor.LimiteSuperior ? "Sí" : "No";

                            return muestraValor.Valor <= muestraValor.LimiteSuperior ? "Sí" : "No";
                        default:
                            return "N/A";
                    }

                }

                if (tipoEvaluacion == TipoEvaluacion.Evaluar_LFD)
                {
                    return muestraValor.LFD ? muestraValor.ValorValidacion == true ? "Sí" : "No" : "N/A";
                }

                return muestraValor.ValorValidacion == true ? "Sí" : "No";
            }

            return "---";

        }


        private string ObtenResultadoMensual(usp_ReporteMensualCalculos resultado, List<Analisi> analisisList, int normaId, int cuerpoReceptorId, int usoAguaId)
        {
            if (resultado.AceptaValor)
            {
                var nfi = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = ",",
                    NumberDecimalDigits = 7
                };

                var valor = resultado.Valor?.ToString("N", nfi);
                return valor?.Remove(valor.Length - 1, 1);
            }

            return resultado.EsValido ? "Ausente" : "Presente";
        }

        private string ObtenLMPMensual(TipoEvaluacion tipoEvaluacion, usp_ReporteMensualCalculos resultado)
        {
            if (tipoEvaluacion == TipoEvaluacion.NoEvaluar)
                return "N/A";

            if (resultado.AceptaValor)
            {
                var nfi = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = ",",
                    NumberDecimalDigits = 6
                };

                switch (tipoEvaluacion)
                {
                    case TipoEvaluacion.Evaluar_Simple:
                        if (resultado.LimiteInferior != null)
                            return resultado.LimiteInferior?.ToString("N", nfi) + "-" + resultado.LimiteSuperior?.ToString("N", nfi);

                        return resultado.LimiteSuperior?.ToString("N", nfi);
                    case TipoEvaluacion.Evaluar_LFD:
                        if (resultado.LimiteInferiorLFD != null)
                            return resultado.LimiteInferiorLFD?.ToString("N", nfi) + "-" + resultado.LimiteSuperiorLFD?.ToString("N", nfi);
                        return resultado.LimiteSuperiorLFD?.ToString("N", nfi);
                    case TipoEvaluacion.Evaluar_LFD_Simple:
                        if (resultado.LFD)
                        {
                            if (resultado.LimiteInferiorLFD != null)
                                return resultado.LimiteInferiorLFD?.ToString("N", nfi) + "-" + resultado.LimiteSuperiorLFD?.ToString("N", nfi);
                            return resultado.LimiteSuperiorLFD?.ToString("N", nfi);
                        }

                        if (resultado.LimiteInferior != null)
                            return resultado.LimiteInferior?.ToString("N", nfi) + "-" + resultado.LimiteSuperior?.ToString("N", nfi);

                        return resultado.LimiteSuperior?.ToString("N", nfi);
                    default:
                        return "N/A";
                }
            }

            return "Ausente";
        }

        private bool RequiereCaudal(MuestraValor muestraValor)
        {
            return muestraValor.Muestra.Analisis.OrdenTrabajo.Norma.RequiereCaudal;
        }


        private List<AnalisisPDInformeMensual> EstableceCaudal(Analisi analisis)
        {
            var i = 1;
            return analisis.Muestras.Select(muestra =>
                    new AnalisisPDInformeMensual()
                    {
                        AnalisisId = analisis.AnalisisID,
                        NivelId = 1,
                        Etiqueta = muestra.NivelID == (int)NivelMuestra.Simple ? "Caudal (l/s) " + i++ : "Caudal promedio (l/s)",
                        Resultado = muestra.Caudal > 0 ? muestra.Caudal.ToString("N") : "---",
                        LMP = "N/A",
                        Cumple = "N/A"
                    })
                .ToList();
        }


        private string ObtenOperacion(int? operacionId)
        {
            if (operacionId == null)
                return "";

            var operacion = (TipoOperacion)operacionId;

            return operacion switch
            {
                TipoOperacion.Sumatoria => " Sumatoria",
                TipoOperacion.Promedio => " Promedio",
                TipoOperacion.PromedioPonderado => " Promedio Ponderado en Función del Caudal",
                TipoOperacion.MediaGeometrica => " Media Geométrica",
                _ => ""
            };
        }

        #endregion


        #region Nom001
        public bool ObtieneDatosInscritosNom001(string Anexo, string Titulo, string rfcCliente)
        {
            SqlConnection conn = null;
            conn = new SqlConnection(_configuration.GetConnectionString("ConnectionNom001"));

            List<InscriosNom001> listaInscritos = new List<InscriosNom001>();
            var resultado = false;
            //List<ListaLaboratoriosBusqueda> listaLab = new List<ListaLaboratoriosBusqueda>();
            string queryString = " SELECT * from dbo.vw_Nom001 WHERE " + "RFC = '" + rfcCliente + "'" + " AND PermisoDeDescarga='" + Titulo + "'" + " AND NumeroDeAnexo='" + Anexo + "'" + " AND Estatus='Vigente'";

            conn.Open();
            SqlCommand com = new SqlCommand(queryString, conn);

            SqlDataReader registros = com.ExecuteReader();

            while (registros.Read())
            {
                InscriosNom001 lista = new InscriosNom001
                {
                    RFC = registros["RFC"].ToString(),
                    NombreResponsable = registros["NombreResponsabledescarga"].ToString(),
                    Estatus = registros["Estatus"].ToString(),
                    NumAnexo = registros["NumeroDeAnexo"].ToString(),
                    Titulo = registros["PermisoDeDescarga"].ToString(),

                };
                listaInscritos.Add(lista);
            }
            if (listaInscritos.Count() > 0)
            {
                resultado = true;
            }
            conn.Close();


            return resultado;
        }

    
    public bool RequierePrograma()
        {
            var datos2021 = _siralabContext.MensajePCNOM.First().Activo;
            return datos2021 ?? false;
        }

        #endregion
    }
}

public class InscriosNom001
{
    public string NombreResponsable { get; set; }
    public string Titulo { get; set; }
    public string NumAnexo { get; set; }
    public string Estatus { get; set; }
    public string RFC { get; set; }
}