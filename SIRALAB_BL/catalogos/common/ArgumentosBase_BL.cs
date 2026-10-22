using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.common
{
    public abstract class AnalisisArgBase_BL : EventArgs
    {
        public int? AnaisisID { get; set; }
        public int? AnalisisPadreID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int ConsecionID { get; set; }
        public int AnexoID { get; set; }
        public int LaboratorioID { get; set; }
        public int EstatusID { get; set; }
        public bool Firmada { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaFirmada { get; set; }
        public bool Enviada { get; set; }
        public bool Procesado { get; set; }
        public bool FaltaPago { get; set; }
        public string NormaDQOCOT { get; set; }
        public DateTime? FechaProceso { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public virtual IEnumerable<DocumentoAnalisis> DocumentoAnalisis { get; set; }

        public virtual IEnumerable<MuestraArgBase_BL> Muestras { get; set; }
    }
    public abstract class AnalisisFirmaArgBase_BL : EventArgs
    {
        public int? AnalisisFirmaID { get; set; }
        public int AnalisisID { get; set; }
        public string RFC { get; set; }
        public string Firma { get; set; }
        public string Cadena { get; set; }
        public string Contenedor { get; set; }
        public string SelloCNA { get; set; }
        public DateTime? FechaRegistro { get; set; }

    }
    public abstract class AnalisisFaltaPagoArgBase_BL : EventArgs
    {
        public int? AnalisisFaltaPagoID { get; set; }
        public int AnalisisID { get; set; }
        public string RFC { get; set; }
        public string Firma { get; set; }
        public string Cadena { get; set; }
        public string Contenedor { get; set; }
        public string SelloCNA { get; set; }
        public bool Marcado { get; set; }
        public DateTime? FechaRegistro { get; set; }

    }

    public abstract class AnalisisColaDetalleArgBase_BL : EventArgs
    {
        public int? AnalisisColaDetalleID { get; set; }
        public bool Activo { get; set; }
        public bool Completo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
    }


    public abstract class ConsecionArgBase_BL : EventArgs
    {
        public int? ConsecionID { get; set; }
        public long? PukConsecionID { get; set; }
        public int ClienteID { get; set; }
        public string Titulo { get; set; }
    }
    public abstract class AnexoMedidorArgBase_BL : EventArgs
    {
        public int? AnexoMedidorID { get; set; }
        public int? AnexoID { get; set; }

        public long? ID_SERIE_MEDIDOR { get; set; }

        public string numeroMedidor { get; set; }
        public long? ID_MARCA_MEDIDOR { get; set; }
        public string MARCA_MEDIDOR { get; set; }
        public bool? FuenteAbastecimiento { get; set; }
    }
    public abstract class AnexoArgBase_BL : EventArgs
    {
        public int? AnexoID { get; set; }
        public long? PukAnexoID { get; set; }
        public int ConsecionID { get; set; }
        public int? DireccionID { get; set; }
        public string Anexo { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Datum { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int UsoAguaID { get; set; }
        public string NumeroMedidor { get; set; }
        public string Marcamedidor { get; set; }
        public string NombreCuerpoReceptor { get; set; }
        public string FuenteAbastecimiento { get; set; }
        public string OrigenDescarga { get; set; }
        public bool Efluente { get; set; }

        public decimal GradosLatitud { get; set; }
        public decimal MinutosLatitud { get; set; }
        public decimal SegundosLatitud { get; set; }
        public decimal GradosLongitud { get; set; }
        public decimal MinutosLongitud { get; set; }
        public decimal SegundosLongitud { get; set; }
        public long? iduman { get; set; }

        public string Transaccion { get; set; }

        public Direccion Direc { get; set; }
        public DireccionArgBase_BL dir { get; set; }
        public long? ID_TIPO_CR { get; set; }
        public long? ID_USO_CR { get; set; }
    }
    public abstract class ClienteArgBase_BL : EventArgs
    {
        public int? ClienteID { get; set; }
        public int? DireccionID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Correo { get; set; }


        public string Transaccion { get; set; }

        public DireccionArgBase_BL dir { get; set; }
    }
    public abstract class DireccionArgBase_BL : EventArgs
    {

        public int? DireccionID { get; set; }
        public string Calle { get; set; }
        public string NumeroExteior { get; set; }
        public string NumeroInterior { get; set; }
        public int? EstadoID { get; set; }
        public int? MunicipioID { get; set; }
        public int? LocalidadID { get; set; }
        public int? ColoniaID { get; set; }
        public int? CP { get; set; }
    }
    public abstract class OrdenTrabajoArgBase_BL : EventArgs
    {
        public int? OrdenTrabajoID { get; set; }
        public int ClienteID { get; set; }
        public int FrecuenciaMuestreoID { get; set; }
        public int FrecuenciaReporteID { get; set; }
        public int EstatusID { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroInforme { get; set; }
        public string Folio { get; set; }
        public int Anio { get; set; }
        public int Consecutivo { get; set; }
        public int HorasProceso { get; set; }
        public bool EsMunicipal { get; set; }
        public int PoblacionToneladas { get; set; }
        public int PromedioDiario { get; set; }
        public int? NormaID { get; set; }
        public int? CuerpoReceptorID { get; set; }
        public int? UsoAguaID { get; set; }
        public int AnexoID { get; set; }
        public int ConsecionID { get; set; }
        public int? LaboratorioID { get; set; }

        public bool? FuenteAbastecimiento { get; set; }
        public int? AnexoMedidorID { get; set; }
        public virtual IEnumerable<DocumentoAnexo> DocumentoAnexo { get; set; }
        public virtual IEnumerable<AnalisisArgBase_BL> Analisis { get; set; }
    }
    public abstract class SignatarioArgBase_BL : EventArgs
    {
        public int LaboratorioId;
        public string Laboratorio;
        public int Interno;
        public int Externo;
    }
    public abstract class MuestraArgBase_BL : EventArgs
    {
        public int? MuestraID { get; set; }
        public int AnalisisID { get; set; }
        public int? DireccionID { get; set; }
        public string Numero { get; set; }
        public string Descripcion { get; set; }
        public string Identificacion { get; set; }
        public decimal Caudal { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Datum { get; set; }
        public DateTime? Fecha { get; set; }
        public bool EsCompuesta { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public bool Cancelado { get; set; }
        public string Observaciones { get; set; }
        public int NivelID { get; set; }
        public decimal ValMuestraC { get; set; }
        public decimal ValHojaCampo { get; set; }
        public DireccionArgBase_BL dir { get; set; }
        public DateTime? FechaConformacion { get; set; }
        public virtual IEnumerable<DocumentoMuestraArgBase> DocumentoMuestra { get; set; }

    }
    public abstract class MuestreadorArgBase_BL : EventArgs
    {
        public int? MuestreadorID { get; set; }
        public int MuestraID { get; set; }
        public int LaboratorioSignatarioID { get; set; }
        public bool? Activo { get; set; }
    }
    public class MuestraValorArgBase_BL : MuestraValor
    {
        /*----------------------------*/
        public int? MuestraValorID { get; set; }
        public int MuestraID { get; set; }
        public Parametro Parametro { get; set; }
        public ParametroMetodo ParametroMetodo { get; set; }
        public int? LaboratorioID { get; set; }
        public int? OperacionID { get; set; }
        public bool? Exenta { get; set; }
        public bool? Evalua { get; set; }
        public bool? Exento { get; set; }
        public bool? NoAnalizado { get; set; }
        public decimal Valor { get; set; }
        public bool? ValorValidacion { get; set; }
        public bool AceptaValor { get; set; }
        public bool? LFD { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public decimal? LimiteInferiorLFD { get; set; }
        public decimal? LimiteSuperiorLFD { get; set; }
        public decimal? LC_MC { get; set; }
        public bool? GeneradoSP { get; set; }
        public bool? Grupo { get; set; }
        public int? EscalaDecimal { get; set; }
        public bool? LCMC { get; set; }
        public string Etiqueta { get; set; }
    }
    public abstract class FiltroOrdenTrabajoArgBase : PageArgs
    {
        public virtual int OrdenTrabajoID { get; set; }
        public virtual string NumeroInforme { get; set; }
        public virtual int? NormaID { get; set; }
        public virtual string RFC { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Anexo { get; set; }
        public virtual DateTime? FechaEmisionInicio { get; set; }
        public virtual DateTime? FechaEmisionFin { get; set; }
        public virtual DateTime? FechaRegistroInicio { get; set; }
        public virtual DateTime? FechaRegistroFin { get; set; }
        public virtual DateTime? FechaConformacionInicio { get; set; }
        public virtual DateTime? FechaConformacionFin { get; set; }
        public virtual string Laboratorio { get; set; }
        public virtual int? LaboratorioID { get; set; }
        public virtual bool Firma { get; set; }
        public virtual int EstatusCancelacion { get; set; }
        public virtual string OrderBy { get; set; }
        public virtual string OrderByDirection { get; set; }
    }
    public abstract class FiltroLaboratorioArgBase : EventArgs
    {
        public virtual string RFCCliente { get; set; }
        public virtual string Numero { get; set; }
        public virtual DateTime? FechaInicial { get; set; }
        public virtual DateTime? FechaFinal { get; set; }
        public virtual bool Firmada { get; set; }
        public virtual bool NoFirmada { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Anexo { get; set; }
        public virtual string Norma { get; set; }
        public virtual string Cancel { get; set; }
        public virtual string RFCLaboratorio { get; set; }
        public virtual string NombreLaboratorio { get; set; }


    }
    public abstract class filtrolaboratoriocnaArgBase : EventArgs
    {
        public virtual int idlaboratorio { get; set; }
        public virtual string RFCLaboratorio { get; set; }
        public virtual bool? correo { get; set; }
        public virtual bool? extemporaneo { get; set; }
        public virtual DateTime? fechainicial { get; set; }
        public virtual DateTime? fechafinal { get; set; }
    }
    public abstract class filtroshabilitarAnalisArgBase : EventArgs
    {
        public virtual string RFCLaboratorio { get; set; }
        public virtual string NombreLaboratorio { get; set; }
        public virtual string RFCCliente { get; set; }
        public virtual string NombreCliente { get; set; }
        public virtual int? foliocna { get; set; }
        public virtual string informe { get; set; }
        public virtual DateTime? FechaInicialC { get; set; }
        public virtual DateTime? FechaFinalC { get; set; }
        public virtual DateTime? FechaInicialF { get; set; }
        public virtual DateTime? FechaFinalF { get; set; }

    }
    public abstract class FiltrosBitacoraBase : EventArgs
    {
        public virtual int? Id { get; set; }
        public virtual int? IdUser { get; set; }
        public virtual DateTime? FechaMod { get; set; }
        public virtual string NUser { get; set; }
        public virtual string ModuloMod { get; set; }
        public virtual string Firma { get; set; }
        public virtual string CadenaFirma { get; set; }
        public virtual string TipoMov { get; set; }
        public virtual string NSerieCert { get; set; }

    }
    public abstract class FiltrosMuestreadorArgsBase : EventArgs
    {
        public virtual int? LaboratorioId { get; set; }
        public virtual DateTime? FechaInicio { get; set; }
        public virtual DateTime? FechaFin { get; set; }

    }

    #region Solictud de cancelación
    public abstract class SolicitudCancelacionArgBase_BL : EventArgs
    {
        public virtual int? SolicitudCancelacionID { get; set; }
        public virtual int LaboratorioID { get; set; }
        public virtual int EstatusID { get; set; }
        public virtual string Motivo { get; set; }
        public virtual string FirmaSolicitud { get; set; }
        public virtual string FirmaAprobacion { get; set; }
        public virtual DateTime? FechaRegistro { get; set; }
        public virtual DateTime? FechaAprobacion { get; set; }
        public virtual List<SolicitudCancelacionDetalleArgBase_BL> Detalle { get; set; }
    }
    public abstract class SolicitudCancelacionDetalleArgBase_BL : EventArgs
    {
        public virtual int? SolicitudCancelacionDetalleID { get; set; }
        public virtual int SolicitudCancelacionID { get; set; }
        public virtual int AnalisisID { get; set; }
    }
    public class SolicitudCancelacionArg : SolicitudCancelacionArgBase_BL
    {

    }
    public class SolicitudCancelacionDetalleArg : SolicitudCancelacionDetalleArgBase_BL
    {

    }

    /// <summary>
    /// Representa la información por la cual deberá discriminar la consulta de acuerdo a
    /// los criterios ingresados.
    /// </summary>
    public abstract class FiltroSolicitudCancelacionDetalleArgBase : PageArgs
    {
        /// <summary>
        /// Número de orden de trabajo
        /// </summary>
        public virtual string Numero { get; set; }
        /// <summary>
        /// Título del análisis
        /// </summary>
        public virtual string Titulo { get; set; }
        /// <summary>
        /// Fecha en la que fue emitido el análisis
        /// </summary>
        public virtual DateTime? FechaInicialEmision { get; set; }
        /// <summary>
        /// Fecha final del rango de fechas en la que fue emitido el análisis
        /// </summary>
        public virtual DateTime? FechaFinalEmision { get; set; }
        /// <summary>
        /// RFC del cliente
        /// </summary>
        public virtual string Rfc { get; set; }
        /// <summary>
        /// Id del Laboratorio que realiza la solicitud
        /// </summary>
        public virtual int? LaboratorioID { get; set; }
        /// <summary>
        /// Norma del análisis
        /// </summary>
        public virtual int? NormaID { get; set; }
    }

    /// <summary>
    /// Representa la información por la cual deberá discriminar la consulta de acuerdo a
    /// los criterios ingresados.
    /// </summary>
    public abstract class FiltroSolicitudCancelacionArgBase : PageArgs
    {
        /// <summary>
        /// Id del Laboratorio que realiza la solicitud
        /// </summary>
        public virtual int? LaboratorioID { get; set; }
        /// <summary>
        /// Folio de la solicitud
        /// </summary>
        public virtual int? Folio { get; set; }
        /// <summary>
        /// Estatus de la solicitud
        /// </summary>
        public virtual enumEstatusCancelacion? Estatus { get; set; }
        /// <summary>
        /// Fecha en la que fue emitido el análisis
        /// </summary>
        public virtual DateTime? FechaInicialEmision { get; set; }
        /// <summary>
        /// Fecha final del rango de fechas en la que fue emitido el análisis
        /// </summary>
        public virtual DateTime? FechaFinalEmision { get; set; }
        /// <summary>
        /// Determina sí se incluye o no el estatus de capturado,
        /// este estatus únicamente lo puede visualizar el Laboratorio
        /// </summary>
        public virtual bool IncluirCapturados { get; set; }
    }
    public abstract class FiltroSolicitudCancelacionAnalisisArgBase : PageArgs
    {
        /// <summary>
        /// Id del Laboratorio que realiza la solicitud
        /// </summary>
        public virtual int? LaboratorioID { get; set; }
        /// <summary>
        /// Id de la Norma que tiene el análisis
        /// </summary>
        public virtual int? NormaID { get; set; }
        /// <summary>
        /// Número del informe de la solicitud
        /// </summary>
        public virtual string Numero { get; set; }
        /// <summary>
        /// Fecha en la que fue emitido el análisis
        /// </summary>
        public virtual DateTime? FechaInicialEmision { get; set; }
        /// <summary>
        /// Fecha final del rango de fechas en la que fue emitido el análisis
        /// </summary>
        public virtual DateTime? FechaFinalEmision { get; set; }
        /// <summary>
        /// Rfc del Cliente
        /// </summary>
        public string Rfc { get; set; }
        /// <summary>
        /// Título de conseción que se analiza
        /// </summary>
        public string Titulo { get; set; }
    }
    #endregion

    public class Pagina
    {
        public int Page { get; set; }
        public bool ActivePage { get; set; }
    }
    public class PageArgs : EventArgs
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    #region Catalogos

    public abstract class CatPromedioArgBase_BL : EventArgs
    {
        public int? PromedioID { get; set; }
        public string Nombre { get; set; }
    }
    public abstract class CatTipoCuerpoReceptorArgBase_BL : EventArgs
    {
        public int? TipoCuerpoReceptorID { get; set; }
        public string Nombre { get; set; }
    }
    public abstract class CatUsoAguaArgBase_BL : EventArgs
    {
        public int? UsoAguaID { get; set; }
        public string Nombre { get; set; }
    }
    public abstract class CatDiaInhabilArgBase_BL : EventArgs
    {
        public int? DiaInhabilId { get; set; }
        public DateTime DiaInhabil { get; set; }
    }
    public abstract class CuerpoReceptorArgBase_BL : EventArgs
    {
        public int? CuerpoReceptorID { get; set; }
        public string Nombre { get; set; }
        public int TipoCuerpoReceptorID { get; set; }
    }
    public abstract class NormaArgBase_BL : EventArgs
    {
        public int? NormaID { get; set; }
        public int? ClienteID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool ExentaParametros { get; set; }
        public bool RequiereCaudal { get; set; }
        public bool RequiereCompuesta { get; set; }
        public bool RequiereMensual { get; set; }
        public bool Evaluacion { get; set; }
        public bool CPD { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool FuenteAbastecimiento { get; set; }
        public bool MuestrasMultiple { get; set; }
        public int? NormaPadreID { get; set; }
        public int? Desplazamiento { get; set; }
        public bool? RequiereHora { get; set; }
        public bool? RequiereObservaciones { get; set; }
        //public bool? RequiereValorInstantaneo { get; set; }
        public bool? Nom2021 { get; set; }
        public bool? RequiereUsoDeAgua { get; set; }
    }
    public abstract class NormaConfiguracionArgBase_BL : EventArgs
    {
        public int? NormaConfiguracionID { get; set; }
        public int? NormaConfiguracionPadreID { get; set; }
        public int NormaID { get; set; }
        public int ParametroID { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int UsoAguaID { get; set; }
        public int NivelID { get; set; }
        public int? OperacionID { get; set; }
        public bool AceptaValor { get; set; }
        public bool Exenta { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public int Orden { get; set; }
        public bool Grupo { get; set; }
        public bool Activo { get; set; }
        public bool LFD { get; set; }
        public bool Evalua { get; set; }
        public decimal? LimiteInferiorLFD { get; set; }
        public decimal? LimiteSuperiorLFD { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EscalaDecimal { get; set; }
        public int? EscalaDecimalLCM { get; set; }
        public string Etiqueta { get; set; }

        public bool? LCMC;
        public bool? Truncamiento { get; set; }
        public bool? LimitaCaptura { get; set; }
        public decimal? MinAceptado { get; set; }
        public decimal? MaxAceptado { get; set; }

    }
    public class PersonaArgsBase : EventArgs
    {
        public int? PersonaID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public bool Activo { get; set; }
    }
    public abstract class UsuarioArgBase
    {
        public int? UsuarioID { get; set; }
        public int? LaboratorioID { get; set; }
        public int? PersonaID { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public int UsuarioRolID { get; set; }
        public bool Activo { get; set; }

    }
    public class GrupoConfiguracionArgBase : NormaConfiguracionArgBase_BL
    {

    }

    #endregion

    #region Seguridad


    public abstract class CatUsuarioRolArgBase_BL : EventArgs
    {
        public int? UsuarioRolID { get; set; }
        public string Nombre { get; set; }
        public bool Administrable { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }
    public abstract class CatPermisosRolArgBase_BL : EventArgs
    {
        public int? PermisosRolID { get; set; }
        public int UsuarioRolID { get; set; }
        public int PaginaID { get; set; }
        public bool Escritura { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
    public abstract class CatPaginaArgBase_BL : EventArgs
    {
        public int? PaginaID { get; set; }
        public int ModuloID { get; set; }
        public int SistemaID { get; set; }
        public int Orden { get; set; }
        public string CssClass { get; set; }
        public string URL { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public bool VisibleMenu { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
    public abstract class CatModulosArgBase_BL : EventArgs
    {
        public int? ModuloID { get; set; }
        public int? ModuloPadreID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string URL { get; set; }
        public int Orden { get; set; }
        public string CssClass { get; set; }
    }

    #endregion

    #region Archivos
    /* ABSTRACTO*/
    public abstract class DocumentoArgBase : EventArgs
    {
        public int? DocumentoID { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string TipoMime { get; set; }
        public string Ruta { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public byte[] Buffer { set; get; }
    }

    public abstract class DocumentoAnexoArgBase : EventArgs
    {
        public int? DocumentoAnexoID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int DocumentoID { get; set; }
        public int TipoDocumentoID { get; set; }
        public virtual Documento documento { get; set; }

    }

    public abstract class DocumentoMuestraArgBase : EventArgs
    {
        public int? DocumentoMuestraID { get; set; }
        public int MuestraID { get; set; }
        public int DocumentoID { get; set; }
        public int TipoDocumentoID { get; set; }
        public virtual Documento documento { get; set; }
    }

    public abstract class DocumentoAnalisisArgBase : EventArgs
    {
        public int? DocumentoAnalisisID { get; set; }
        public int DocumentoID { get; set; }
        public int AnalisisID { get; set; }
        public virtual Documento documento { get; set; }
    }

    /* HEREDADAS*/

    public class Documento : DocumentoArgBase
    {
        public int? TipoDocumentoID { set; get; }
    }

    public class DocumentoAnexo : DocumentoAnexoArgBase { }

    public class DocumentoMuestra : DocumentoMuestraArgBase { }

    public class DocumentoAnalisis : DocumentoAnalisisArgBase { }

    public class DocumentoEntidad
    {
        public int OrdenTrabajoID { set; get; }
        public int AnalisisID { set; get; }

        public int? MuestraID { set; get; }
    }

    //public class DocumentoArgsEqualityComparer : IEqualityComparer<SIRALAB_DAL.Documento>
    //{
    //   public bool Equals(Documento documento1, SIRALAB_DAL.Documento documento2)
    //    {
    //        if (
    //            documento1.DocumentoID == documento2.DocumentoID)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    public int GetHashCode(Documento documento)
    //    {
    //        return new { documento.DocumentoID, documento.Nombre, documento.TipoMime }.GetHashCode();
    //    }

    //}


    #endregion Archivos

}
