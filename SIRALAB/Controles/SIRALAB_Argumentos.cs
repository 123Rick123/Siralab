using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.Controles
{
    public class ClienteCapturaArgs : ClienteArgBase_BL { }
    public class DireccionCapturaArgs : DireccionArgBase_BL { }
    public class ConsecionCapturaArgs : ConsecionArgBase_BL { }
    public class MuestraCapturaArgs : MuestraArgBase_BL { }

    public class MuestraLista : MuestraArgBase_BL
    {
        public int? MuestraID_;
        public string Fecha_;
        public string Hora_;
        public string Observaciones_;
        public int Consecutivo_;
        public virtual ICollection<MuestraValor> MuestraValors { get; set; }
        public decimal? Caudal_;
    }

    public class OrdenTrabajoLista : OrdenTrabajoArgBase_BL
    {
        public int? OrdenTrabajoId;
        public string Folio;
        public string Titulo;
        public string Anexo;
        public string RFCCliente;
        public string Registro;
        public string muestreado;
        public string NombreLaboratorio;
        public string Norma;
        public List<AnalisisLista> Analisis;
    }

    public class SignatarioLista : SignatarioArgBase_BL
    {
        public int LaboratorioId;
        public string Laboratorio;
        public int Interno;
        public int Externo;
    }

    public class laboratorioscnak
    {
        public int? id { get; set; }
        public int IdLaboratorio { get; set; }
        public string RfcLaboratorio { get; set; }
        public bool? ValCorreo { get; set; }
        public bool? ValExtemporaneo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class HabilitarInabilitar
    {
        public int? foliocna { get; set; }
        public string RFCLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; }
        public string RFCCliente { get; set; }
        public string NombreCliente { get; set; }
        public string informe { get; set; }
        public string titulo { get; set; }
        public string anexo { get; set; }
        public string norma { get; set; }
        public string estatus { get; set; }
        public DateTime? FechaC { get; set; }
        public DateTime? FechaF { get; set; }

    }

    public class OrdenTrabajoLista1
    {
        public int? OrdenTrabajoId;
        public string Folio;
        public string Titulo;
        public string Anexo;
        public string RFCCliente;
        public string Registro;
        public string muestreado;
        public string NombreLaboratorio;
        public string Norma;
    }

    public class AnalisisLista : AnalisisArgBase_BL
    {
        public int? AnaisisID_;
        public int? AnaisisPadreID_;
        public int EstatusID_;
        public string FechaRegistro_;
        public int MuestrasSimples_;
        public string Enviado_;
        public bool EnviadoBool_;
        public string Consecutivo_;
        public bool Firmado_;
        public bool FirmaVisible_;
        public bool Complementario_;
        public bool? AnalisisEncolado_;
    }
    public class OrdenTrabajoCapturaArgs : OrdenTrabajoArgBase_BL { }
    public class AnalisisCapturaArgs : AnalisisArgBase_BL { }
    public class AnexoCapturaArgs : AnexoArgBase_BL { }
    public class AnexoMedidorArgs : AnexoMedidorArgBase_BL { }
    public class NormaArg : NormaArgBase_BL { }

    public class UsuarioArgs : UsuarioArgBase
    {
        public Persona PersonaArgs { get; set; }

    }

    public class PersonaArgs : PersonaArgsBase { }

    public class MuestraValorCapturaArgs : MuestraValorArgBase_BL
    {
    }

    public class Navegacion
    {
        public Navegacion() { }

        public string ObtenNavegacion(int Id)
        {
            string Retorno = "";
            switch (Id)
            {
                case 1:
                    Retorno = "~/Orden.aspx";
                    break;
                case 2:
                    Retorno = "~/OrdenTrabajo.aspx";
                    break;
                case 3:
                    Retorno = "~/Analisis.aspx";
                    break;
            }
            return Retorno;
        }
    }

    public class NormaConfiguracionArgs : NormaConfiguracionArgBase_BL
    {
        
    }

    public class ListNormaConfiguracionArgs : EventArgs
    {
        public List<NormaConfiguracionArgs> ListaConfiguracion { get; set; }
        public ListNormaConfiguracionArgs(List<NormaConfiguracionArgs> datos)
        {
            ListaConfiguracion = datos;
        }
    }

    public class FiltroNormaConfiguracionArg : NormaConfiguracionArgBase_BL
    {
        public string ParametroNombre { set; get; }
        public int CuerpoReceptorID { set; get; }
        public int UsoAguaID { set; get; }
        public int NivelID { set; get; }
        public bool Inactivos { set; get; }
    }

    public class MuestreadorCapturaArgs : MuestreadorArgBase_BL { }
    [Serializable]
    public class SolicitudCancelacionItem
    {
        public int SolicitudCancelacionID { get; set; }
        public int Folio { get; set; }
        public string Laboratorio { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int EstatusID { get; set; }
        public string Estatus { get; set; }
    }

    [Serializable]
    public class SolicitudCancelacionAnalisisItem
    {
        public int AnalisisID { get; set; }
        public string Laboratorio { get; set; }
        public string RfcLaboratorio { get; set; }
        public string NumeroInforme { get; set; }
        public string Rfc { get; set; }
        public string Cliente { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Titulo { get; set; }
        public string Anexo { get; set; }
        public string Norma { get; set; }
    }

    [Serializable]
    public class SolicitudCancelacionDetalleItem
    {
        public int SolicitudCancelacionID { get; set; }
        public int SolicitudCancelacionDetalleID { get; set; }
        public int AnalisisID { get; set; }
        public int? AnalisisPadreID { get; set; }
        public int LaboratorioID { get; set; }
        public string Laboratorio { get; set; }
        public string RFCLaboratorio { get; set; }
        public string NumeroInforme { get; set; }
        public string RFC { get; set; }
        public string Cliente { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Titulo { get; set; }
        public string Anexo { get; set; }
        public string Norma { get; set; }
        public string Estatus { get; set; }
    }

    public class FechaMuestraSimple
    {
        public int? MuestraId { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string FechaConformacion { get; set; }
        public string HoraConformacion { get; set; }
        public string FechaRecepcion { get; set; }
        public string HoraRecepcion { get; set; }
    }

   
}