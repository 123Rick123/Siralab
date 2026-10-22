using System;
using System.Data;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infotec.SIRALAB.ReportesWS.DAL.Services
{
    public class UspReporteMensualHeader : IUspReporteMensualHeader
    {
        private readonly IConfiguration _configuration;

        public UspReporteMensualHeader(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<usp_ReporteMensualHeader> Obten(int clienteId, int normaId, int? anexoId, int? cuerpoReceptorId, int? usoAguaId)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();
                var cmd = new SqlCommand("usp_ReporteMensualHeader", conn) {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.Add(new SqlParameter("@clienteId", clienteId));
                cmd.Parameters.Add(new SqlParameter("@normaId", normaId));
                cmd.Parameters.Add(new SqlParameter("@anexoId", anexoId));
                cmd.Parameters.Add(new SqlParameter("@cuerpoReceptorId", cuerpoReceptorId));
                cmd.Parameters.Add(new SqlParameter("@usoAguaId", usoAguaId));

                var header = new usp_ReporteMensualHeader();

                await using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        header.ClienteNombre = rdr["ClienteNombre"].ToString();
                        header.ClienteRFC = rdr["ClienteRFC"].ToString();
                        header.Anexo = rdr["AnexoAnexo"].ToString();
                        header.AnexoCuerpoReceptorNombre = rdr["AnexoCuerpoReceptorNombre"].ToString();
                        header.PuntoDescargaNombreCuerpoReceptor = rdr["PuntoDescargaNombreCuerpoReceptor"].ToString();
                        header.PuntoDescargaTitulo = rdr["PuntoDescargaTitulo"].ToString();
                        header.NormaNombre = rdr["NormaNombre"].ToString();
                        header.UsoAguaNombre = rdr["UsoAguaNombre"].ToString();
                        header.CuerpoReceptorNombre = rdr["CuerpoReceptorNombre"].ToString();
                    }
                }

                return header;
            }
            catch (Exception e)
            {
                return new usp_ReporteMensualHeader();
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}