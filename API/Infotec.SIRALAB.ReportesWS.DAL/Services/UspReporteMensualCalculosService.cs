using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Infotec.SIRALAB.ReportesWS.DAL.Models.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class UspReporteMensualCalculosService : IUspReporteMensualCalculoService
    {
        private readonly IConfiguration _configuration;

        public UspReporteMensualCalculosService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<usp_ReporteMensualCalculos>> Obten(int clienteId, int normaId, int anexoId, int cuerpoReceptorId, int usoAguaId, decimal caudal,
            DateTime fechaInicio, DateTime fechaFin, TipoEvaluacion tipoEvaluacion)
        {
            SqlConnection conn = null;

            try
            {

                conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();
                var cmd = new SqlCommand("usp_ReporteMensualCalculos", conn) { CommandType = CommandType.StoredProcedure };
                cmd.CommandTimeout = 300;
                cmd.Parameters.Add(new SqlParameter("@Cliente", clienteId));
                cmd.Parameters.Add(new SqlParameter("@normaId", normaId));
                cmd.Parameters.Add(new SqlParameter("@anexoId", anexoId));
                cmd.Parameters.Add(new SqlParameter("@cuerpoReceptorId", cuerpoReceptorId));
                cmd.Parameters.Add(new SqlParameter("@UsoAguaID", usoAguaId));
                cmd.Parameters.Add(new SqlParameter("@caudal", caudal));
                cmd.Parameters.Add(new SqlParameter("@fechaInicio", fechaInicio));
                cmd.Parameters.Add(new SqlParameter("@fechaFin", fechaFin));
                cmd.Parameters.Add(new SqlParameter("@tipoEvaluacion", (int)tipoEvaluacion));

                var calculosList = new List<usp_ReporteMensualCalculos>();

                await using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        calculosList.Add(new usp_ReporteMensualCalculos
                        {
                            ParmetroID = (int ) rdr["ParametroId"],
                            ParametroNombre = rdr["ParametroNombre"].ToString(),
                            UnidadMedida = rdr["UnidadMedida"].ToString(),
                            Exento = (bool) rdr["Exento"],
                            Evalua = (bool) rdr["Evalua"],
                            NoAnalizado = (bool) rdr["NoAnalizado"],
                            Valor = (double?) rdr["Valor"],
                            AceptaValor = (bool) rdr["AceptaValor"],
                            LFD = (bool) rdr["LFD"],
                            LimiteInferior = rdr["LimiteInferior"] as decimal?,
                            LimiteSuperior = rdr["LimiteSuperior"] as decimal?,
                            LimiteInferiorLFD = rdr["LimiteInferiorLFD"] as decimal?,
                            LimiteSuperiorLFD = rdr["LimiteSuperiorLFD"] as decimal?,
                            EsValido = (bool) rdr["EsValido"]
                        });
                    }
                }

                return calculosList;
            }
            catch (Exception e)
            {
                return new List<usp_ReporteMensualCalculos>();
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}