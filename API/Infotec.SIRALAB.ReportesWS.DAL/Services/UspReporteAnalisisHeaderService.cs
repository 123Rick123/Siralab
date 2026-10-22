using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using iText.Kernel.Font;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class UspReporteAnalisisHeaderService : IUspReporteAnalisisHeader
    {

        private readonly IConfiguration _iConfig;

        public UspReporteAnalisisHeaderService(IConfiguration iConfig)
        {
            _iConfig = iConfig;
        }

        public async Task<usp_ReporteAnalisisHeader_Result> Obten(int analisisId)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(_iConfig.GetConnectionString("DefaultConnection"));
                conn.Open();
                var cmd = new SqlCommand("usp_ReporteAnalisisHeader", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@analisisId", analisisId));

                var datosGenerales = new usp_ReporteAnalisisHeader_Result();
                await using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        datosGenerales.AnalisisIdentificador = (int) rdr["AnalisisIdentificador"];
                        datosGenerales.AnalisisEnumeracion = rdr["AnalisisEnumeracion"]?.ToString();
                        datosGenerales.OrdenTrabajoIdentificador = (int) rdr["OrdenTrabajoIdentificador"];
                        datosGenerales.LaboratorioIdentificador = (int) rdr["LaboratorioIdentificador"];
                        datosGenerales.AnalisisMuestraEsCompuesta = (bool?) rdr["AnalisisMuestraEsCompuesta"];
                        datosGenerales.AnalisisFirmado = (bool) rdr["AnalisisFirmado"];
                        datosGenerales.AnalisisFechaFirma = rdr["AnalisisFechaFirma"] as DateTime?;
                        datosGenerales.AnalisisFechaInicio = rdr["AnalisisFechaInicio"] as DateTime?;
                        datosGenerales.AnalisisFechaFin = rdr["AnalisisFechaFin"] as DateTime?;
                        datosGenerales.FirmaCadenaOriginal = rdr["FirmaCadenaOriginal"]?.ToString();
                        datosGenerales.FirmaDatosFirmados = rdr["FirmaDatosFirmados"]?.ToString();
                        datosGenerales.FirmaDatosSelloDigital = rdr["FirmaDatosSelloDigital"]?.ToString();
                        datosGenerales.OrdenTrabajoNumeroInforme = rdr["OrdenTrabajoNumeroInforme"]?.ToString();
                        datosGenerales.OrdenTrabajoHorasProceso = (int?) rdr["OrdenTrabajoHorasProceso"];
                        datosGenerales.CuerpoReceptorNombre = rdr["CuerpoReceptorNombre"]?.ToString();
                        datosGenerales.UsoAguaNombre = rdr["UsoAguaNombre"]?.ToString();
                        datosGenerales.NormaNombre = rdr["NormaNombre"]?.ToString();
                        datosGenerales.LaboratorioEsPersonaFisica = rdr["LaboratorioEsPersonaFisica"] as bool?;
                        datosGenerales.LaboratorioRFC = rdr["LaboratorioRFC"]?.ToString();
                        datosGenerales.LaboratorioCURP = rdr["LaboratorioCURP"]?.ToString();
                        datosGenerales.LaboratorioNombre = rdr["LaboratorioNombre"]?.ToString();
                        datosGenerales.LaboratorioApellidoPaterno = rdr["LaboratorioApellidoPaterno"]?.ToString();
                        datosGenerales.LaboratorioApellidoMaterno = rdr["LaboratorioApellidoMaterno"]?.ToString();
                        datosGenerales.ClienteID = rdr["ClienteID"] as int?;
                        datosGenerales.ClienteRFC = rdr["ClienteRFC"]?.ToString();
                        datosGenerales.ClienteCURP = rdr["ClienteCURP"]?.ToString();
                        datosGenerales.ClienteNombre = rdr["ClienteNombre"]?.ToString();
                        datosGenerales.ClienteApellidoPaterno = rdr["ClienteApellidoPaterno"]?.ToString();
                        datosGenerales.ClienteApellidoMaterno = rdr["ClienteApellidoMaterno"]?.ToString();
                        datosGenerales.PuntoDescargaAnexo = rdr["PuntoDescargaAnexo"]?.ToString();
                        datosGenerales.PuntoDescargaLatitud = rdr["PuntoDescargaLatitud"]?.ToString();
                        datosGenerales.PuntoDescargaLongitud = rdr["PuntoDescargaLongitud"]?.ToString();
                        datosGenerales.PuntoDescargaDatum = rdr["PuntoDescargaDatum"]?.ToString();
                        datosGenerales.PuntoDescargaNombreCuerpoReceptor = rdr["PuntoDescargaNombreCuerpoReceptor"]?.ToString();
                        datosGenerales.PuntoDescargaTitulo = rdr["PuntoDescargaTitulo"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionCalle = rdr["PuntoDescargaDireccionCalle"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionNumExt = rdr["PuntoDescargaDireccionNumExt"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionNumInt = rdr["PuntoDescargaDireccionNumInt"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionColonia = rdr["PuntoDescargaDireccionColonia"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionLocalidad = rdr["PuntoDescargaDireccionLocalidad"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionMunicipio = rdr["PuntoDescargaDireccionMunicipio"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionEstado = rdr["PuntoDescargaDireccionEstado"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionCP = rdr["PuntoDescargaDireccionCP"] as int?;
                        datosGenerales.MuestraLatitud = rdr["MuestraLatitud"]?.ToString();
                        datosGenerales.MuestraLongitud = rdr["MuestraLongitud"]?.ToString();
                        datosGenerales.MuestraDireccion = rdr["MuestraDireccion"]?.ToString();
                        datosGenerales.EsFuenteAbastecimiento = (bool) rdr["EsFuenteAbastecimiento"];
                        datosGenerales.Aprobacion = rdr["Aprobacion"]?.ToString();
                        datosGenerales.AcreditacionEMA = rdr["AcreditacionEMA"]?.ToString();
                        datosGenerales.MuestraIdentificador = rdr["MuestraIdentificador"] as int?;
                    }
                }

                return datosGenerales;
            }
            catch (Exception e)
            {
                return new usp_ReporteAnalisisHeader_Result();
            }
            finally
            {
                conn?.Close();
            }


        }

        public usp_ReporteAnalisisHeader_Result ObtenDatosGenerales(int analisisId)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(_iConfig.GetConnectionString("DefaultConnection"));
                conn.Open();
                var cmd = new SqlCommand("usp_ReporteAnalisisHeader", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@analisisId", analisisId));

                var datosGenerales = new usp_ReporteAnalisisHeader_Result();
               using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        datosGenerales.AnalisisIdentificador = (int)rdr["AnalisisIdentificador"];
                        datosGenerales.AnalisisEnumeracion = rdr["AnalisisEnumeracion"]?.ToString();
                        datosGenerales.OrdenTrabajoIdentificador = (int)rdr["OrdenTrabajoIdentificador"];
                        datosGenerales.LaboratorioIdentificador = (int)rdr["LaboratorioIdentificador"];
                        datosGenerales.AnalisisMuestraEsCompuesta = (bool?)rdr["AnalisisMuestraEsCompuesta"];
                        datosGenerales.AnalisisFirmado = (bool)rdr["AnalisisFirmado"];
                        datosGenerales.AnalisisFechaFirma = rdr["AnalisisFechaFirma"] as DateTime?;
                        datosGenerales.AnalisisFechaInicio = rdr["AnalisisFechaInicio"] as DateTime?;
                        datosGenerales.AnalisisFechaFin = rdr["AnalisisFechaFin"] as DateTime?;
                        datosGenerales.FirmaCadenaOriginal = rdr["FirmaCadenaOriginal"]?.ToString();
                        datosGenerales.FirmaDatosFirmados = rdr["FirmaDatosFirmados"]?.ToString();
                        datosGenerales.FirmaDatosSelloDigital = rdr["FirmaDatosSelloDigital"]?.ToString();
                        datosGenerales.OrdenTrabajoNumeroInforme = rdr["OrdenTrabajoNumeroInforme"]?.ToString();
                        datosGenerales.OrdenTrabajoHorasProceso = (int?)rdr["OrdenTrabajoHorasProceso"];
                        datosGenerales.CuerpoReceptorNombre = rdr["CuerpoReceptorNombre"]?.ToString();
                        datosGenerales.UsoAguaNombre = rdr["UsoAguaNombre"]?.ToString();
                        datosGenerales.NormaNombre = rdr["NormaNombre"]?.ToString();
                        datosGenerales.LaboratorioEsPersonaFisica = rdr["LaboratorioEsPersonaFisica"] as bool?;
                        datosGenerales.LaboratorioRFC = rdr["LaboratorioRFC"]?.ToString();
                        datosGenerales.LaboratorioCURP = rdr["LaboratorioCURP"]?.ToString();
                        datosGenerales.LaboratorioNombre = rdr["LaboratorioNombre"]?.ToString();
                        datosGenerales.LaboratorioApellidoPaterno = rdr["LaboratorioApellidoPaterno"]?.ToString();
                        datosGenerales.LaboratorioApellidoMaterno = rdr["LaboratorioApellidoMaterno"]?.ToString();
                        datosGenerales.ClienteID = rdr["ClienteID"] as int?;
                        datosGenerales.ClienteRFC = rdr["ClienteRFC"]?.ToString();
                        datosGenerales.ClienteCURP = rdr["ClienteCURP"]?.ToString();
                        datosGenerales.ClienteNombre = rdr["ClienteNombre"]?.ToString();
                        datosGenerales.ClienteApellidoPaterno = rdr["ClienteApellidoPaterno"]?.ToString();
                        datosGenerales.ClienteApellidoMaterno = rdr["ClienteApellidoMaterno"]?.ToString();
                        datosGenerales.PuntoDescargaAnexo = rdr["PuntoDescargaAnexo"]?.ToString();
                        datosGenerales.PuntoDescargaLatitud = rdr["PuntoDescargaLatitud"]?.ToString();
                        datosGenerales.PuntoDescargaLongitud = rdr["PuntoDescargaLongitud"]?.ToString();
                        datosGenerales.PuntoDescargaDatum = rdr["PuntoDescargaDatum"]?.ToString();
                        datosGenerales.PuntoDescargaNombreCuerpoReceptor = rdr["PuntoDescargaNombreCuerpoReceptor"]?.ToString();
                        datosGenerales.PuntoDescargaTitulo = rdr["PuntoDescargaTitulo"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionCalle = rdr["PuntoDescargaDireccionCalle"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionNumExt = rdr["PuntoDescargaDireccionNumExt"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionNumInt = rdr["PuntoDescargaDireccionNumInt"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionColonia = rdr["PuntoDescargaDireccionColonia"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionLocalidad = rdr["PuntoDescargaDireccionLocalidad"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionMunicipio = rdr["PuntoDescargaDireccionMunicipio"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionEstado = rdr["PuntoDescargaDireccionEstado"]?.ToString();
                        datosGenerales.PuntoDescargaDireccionCP = rdr["PuntoDescargaDireccionCP"] as int?;
                        datosGenerales.MuestraLatitud = rdr["MuestraLatitud"]?.ToString();
                        datosGenerales.MuestraLongitud = rdr["MuestraLongitud"]?.ToString();
                        datosGenerales.MuestraDireccion = rdr["MuestraDireccion"]?.ToString();
                        datosGenerales.EsFuenteAbastecimiento = (bool)rdr["EsFuenteAbastecimiento"];
                        datosGenerales.Aprobacion = rdr["Aprobacion"]?.ToString();
                        datosGenerales.AcreditacionEMA = rdr["AcreditacionEMA"]?.ToString();
                        datosGenerales.MuestraIdentificador = rdr["MuestraIdentificador"] as int?;
                    }
                }

                return datosGenerales;
            }
            catch (Exception e)
            {
                return new usp_ReporteAnalisisHeader_Result();
            }
            finally
            {
                conn?.Close();
            }


        }
    }
}