using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BL_WS_SIRALAB;
using BL_WS_SIRALAB.clases;
using System.ServiceModel.Web;
using System.Net.Security;

namespace WS_SIRALAB
{
    [ServiceContract]
    public interface IwsSIRALAB
    {
        [OperationContract]
        List<PeriodResult> Obten_Informacion(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion);
        [OperationContract]
        List<PeriodResult> Obten_Informacion_PD(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion);
        [OperationContract]
        List<Norma> Obten_Normas();
        [OperationContract]
        bool Validacion(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, ref bool PDValido);
        [OperationContract]
        Validacion Listado_Validacion(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion);
        [OperationContract]
        byte[] Archivo_PDF_Analisis(int AnalisisID);
        [OperationContract]
        byte[] Archivo_PDF_Mensual(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion);

        #region Fuente_De_Abastecimiento
        [OperationContract]
        Validacion Listado_Validacion_Fuente(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion,List<int> MedidorID);
        [OperationContract]
        List<PeriodResult> Obten_Informacion_Fuente(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID, enumTipoEvaluacion Evaluacion, List<int> MedidorID);
        #endregion
        #region ApercibidosYReincidentes
        [OperationContract]
        bool TieneReincidencia(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID);
        [OperationContract]
        bool TieneReincidenciaPreventiva(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID);
        [OperationContract]
        bool TieneReincidenciaImpugnada(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID);
        [OperationContract]
        bool TieneApercibimiento(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID);
        [OperationContract]
        bool TieneApercibimientoImpugnado(int ClienteID, DateTime FechaInicio, DateTime FechaFin, string Titulo, string Anexo, int NormaID);

        #endregion

        [OperationContract]
        bool EstaInscritoNom001(string Anexo, string Titulo, int clienteID, DateTime FechaMesDeclaracion);
        [OperationContract]
        bool EstaInscritoNom001SinEfectos(string Anexo, string Titulo, int clienteID);
    }
}
