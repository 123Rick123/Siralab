using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRALAB_DAL;

namespace SIRALAB.Entidades
{
    public class LaboratorioApercibidos
    {
        public int Id { get; set; }
        public int IdApercibimiento { get; set; }
        public string Nombre { get; set; }
        public string RFC { get; set; }
        public string Fecha_Resolucion { get; set; }
        public string Numero_Oficio { get; set; }
        public string Fecha_Notificacion { get; set; }
        public string Fecha_Vigencia { get; set; }
        //public string Observaciones { get; set; }
        public string Evidencia { get; set; }
        //public int? Trimestre { get; set; }
        public int? Anio { get; set; }
        public bool? Activo { get; set; }
        public string Fecha_captura { get; set; }

        public int EstatusId { get; set; }

        public string Estatus
        {
            get
            {
                switch (EstatusId)
                {
                    case 1 :
                        return "Apercibido";
                    case 2:
                        return "Apercibido";
                    case 3 :
                        return "Reincidente";
                  case 4:
                       return "Reincidente";
                    default:
                        return "";
                }
            }
        }

        public int LaboratorioId
        {
            set
            {
                var repositorio = new SIRALAB_BL.negocio.LaboratoriosApercibidos.LabApercibidosYReincidentesBLL();
                var resultado = repositorio.ObtenerNumerosYVigenciasDeAprobacion(value)
                    .Select(a => new VigenciaResult()
                    {
                        NumeroAprobacion = a.Numero,
                       
                        FechaInicio = a.FechaFin.Value 
                    }).ToList();

                if (resultado != null)
                {

                    LaboratorioVigencias = resultado;
                }
                else
                {
                    LaboratorioVigencias = new List<VigenciaResult>();
                }

            }
        }

        public List<VigenciaResult> LaboratorioVigencias { get; set; }

        //public string NoExpediente { get; set; }
        //public string Memorando { get; set; }
        //public string InicialesResponsable { get; set; }
    }


    public class VigenciaResult
    {

        public string NumeroAprobacion { get; set; }

        public DateTime FechaInicio { get; set; }
        public string VigenciaAprobacion { get { return FechaInicio.ToShortDateString(); } }
    }
    public class ListaImpugnacion
    {
        public int Id;
        public int IdMediosDeImpugnacion;
        public int Estatus;
        public string AnteQuien;
        public string Observaciones;
        public DateTime FechaImpugnacion;
        public DateTime FechaCaptura;
        public DateTime FechaResolucion;
        public int IdLaboratorio;
        public int IdApercibimiento;
        public int IdReincidencia;
        public string NombreResponsable;
        public bool Activo;
        public string Expediente;

    }
}