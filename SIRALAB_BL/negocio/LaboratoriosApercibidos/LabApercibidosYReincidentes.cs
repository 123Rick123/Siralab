using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.LaboratoriosApercibidos
{
   public class LabApercibidosYReincidentes
    {

        public class ListaLaboratoriosBusqueda
        {
            public int idLaboratorio;
            public string RFCLaboratorio;
            public string NombreLaboratorio; 
            public string Numero;
            public DateTime Vigencia;
            //public List<ListaLaboratoriosBusquedaVigencia> vigenciasLab;
        }

        public class ListaLaboratoriosBusquedaVigencia
        {
            public int idLaboratorio;
            public string Numero;
            public string FechaInicio;
            public string FechaFin;


        }

        public class ListaImpugnacion
        {
            public int      Id;
            public int      IdMediosDeImpugnacion;
            public int      Estatus;
            public string   AnteQuien;
            public string   Observaciones;
            public DateTime FechaImpugnacion;
            public DateTime FechaCaptura;
            public DateTime FechaResolucion;
            public int      IdLaboratorio;
            public int      IdApercibimiento;
            public int      IdReincidencia;
            public string   NombreResponsable;
            public bool     Activo;
            public string   Expediente;

        }
    }
}
