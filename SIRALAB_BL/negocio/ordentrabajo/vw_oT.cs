using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB_BL
{
    public class vw_oT
    {
        private Siralab_Entities OrdenTrabajoContext { get; set; }
        public Vw_Ot_Grid OrdenTrabajo { get; private set; }
        Repository<Vw_Ot_Grid> Repositorio;

        public vw_oT()
        {
            OrdenTrabajoContext = new Siralab_Entities();
            Repositorio = new Repository<Vw_Ot_Grid>(OrdenTrabajoContext);
        }

        public int Cuenta_OrdenTrabajo(FiltroLaboratorioArgBase args, int usuarioRolId, int? laboratorioId = null)
        {
            int listaOrden = 0;
            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            if (usuarioRolId == enumUsuarioRol.Recaudacion.ParseToInt() || usuarioRolId == enumUsuarioRol.Habilitar.ParseToInt())
            {
                Expression<Func<Vw_Ot_Grid, bool>> predicate = p => (p.RFC.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
                        && (p.Informe.Contains(args.Numero != "" ? args.Numero : "") || args.Numero == null || args.Numero == "")
                        && (p.FechaEmision >= (args.FechaInicial ?? fechaIniTemp) && p.FechaEmision <= (args.FechaFinal ?? fechaFinTemp))
                        && (p.Anexo.Contains(args.Anexo != "" ? args.Anexo : "") || args.Anexo == null || args.Anexo == "") /*Anexo*/
                        && (p.Norma.Contains(args.Norma != "" ? args.Norma : "") || args.Norma == null || args.Norma == "") /*Norma*/
                        && (p.Titulo.Contains(args.Titulo != "" ? args.Titulo : "") || args.Titulo == null || args.Titulo == "") /*Titulo*/
                        && (p.NombreLaboratorio.Contains(args.NombreLaboratorio != "" ? args.NombreLaboratorio : "") || args.NombreLaboratorio == null || args.NombreLaboratorio == "") /*NombreLaboratorio*/
                        && (p.RFCLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") || args.RFCLaboratorio == null || args.RFCLaboratorio == "") /*Titulo*/
                        && (p.Firmada == "SÍ");

                listaOrden = Repositorio.Count(predicate);
            }
            else
            {
                Expression<Func<Vw_Ot_Grid, bool>> predicate = p =>
                        p.RFC.Contains(args.RFCCliente ?? "") &&
                        p.Informe.Contains(args.Numero ?? "") &&
                        p.Norma.Contains(args.Norma ?? args.Norma) && /*Norma*/
                        p.Titulo.Contains(args.Titulo ?? "") && /*Titulo*/
                        p.Anexo.Contains(args.Anexo ?? "") && /*Anexo*/
                        p.FechaEmision >= (args.FechaInicial ?? fechaIniTemp) &&
                        p.FechaEmision <= (args.FechaFinal ?? fechaFinTemp) &&
                        p.Firmada == "SÍ" &&
                        p.LaboratorioID == laboratorioId;
                //(p.RFC.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
                //&& (p.Informe.Contains(args.Numero != "" ? args.Numero : "") || args.Numero == null || args.Numero == "")
                //&& (p.FechaEmision >= (args.FechaInicial ?? fechaIniTemp) && p.FechaEmision <= (args.FechaFinal ?? fechaFinTemp))
                //&& (p.Anexo.Contains(args.Anexo != "" ? args.Anexo : "") || args.Anexo == null || args.Anexo == "") /*Anexo*/
                //&& (p.Norma.Contains(args.Norma != "" ? args.Norma : "") || args.Norma == null || args.Norma == "") /*Norma*/
                //&& (p.Titulo.Contains(args.Titulo != "" ? args.Titulo : "") || args.Titulo == null || args.Titulo == "") /*Titulo*/
                //&& (p.Firmada == "SÍ") && (p.LaboratorioID == laboratorioId);

                listaOrden = Repositorio.Count(predicate);
            }
            return listaOrden;
        }
        public IEnumerable<Vw_Ot_Grid> ListaOrdenTrabajofirmada(FiltroLaboratorioArgBase Args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null, string OrderBy = "", string sortDirection = "ASC")
        {

            IEnumerable<Vw_Ot_Grid> Lista_Orden = new List<Vw_Ot_Grid>();
            DateTime FechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime FechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            var sortMethods = new List<Func<Vw_Ot_Grid, object>>();

            switch (OrderBy)
            {
                case "":
                    Func<Vw_Ot_Grid, object> obFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obFechaEmision);
                    break;
                case "Registro":
                    Func<Vw_Ot_Grid, object> obRegistro = p => p.FechaEmision;
                    sortMethods.Add(obRegistro);
                    break;
                case "Folio":
                    Func<Vw_Ot_Grid, object> obFolio = p => p.Informe;
                    sortMethods.Add(obFolio);
                    break;
                case "Titulo":
                    Func<Vw_Ot_Grid, object> obTitulo = p => p.Titulo;
                    sortMethods.Add(obTitulo);
                    break;
                case "Anexo":
                    Func<Vw_Ot_Grid, object> obAnexo = p => p.Anexo.ToString();
                    sortMethods.Add(obAnexo);
                    break;
                case "RFCCliente":
                    Func<Vw_Ot_Grid, object> obRFCCliente = p => p.RFC;
                    sortMethods.Add(obRFCCliente);
                    break;
                case "Norma":
                    Func<Vw_Ot_Grid, object> obNorma = p => p.Norma;
                    sortMethods.Add(obNorma);
                    break;
                default:
                    Func<Vw_Ot_Grid, object> obDefFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obDefFechaEmision);
                    break;
            }
            Expression<Func<Vw_Ot_Grid, bool>> predicate;
            if (UsuarioRolID == enumUsuarioRol.Recaudacion.ParseToInt() || UsuarioRolID == enumUsuarioRol.Habilitar.ParseToInt())
            {

                if (Args.Cancel == "true")
                {
                    /* Expression<Func<Vw_Ot_Grid, bool>> */
                    predicate = p => (p.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.Informe.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Anexo.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                        && (p.Norma.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                        && (p.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/
                        && (p.NombreLaboratorio.Contains(Args.NombreLaboratorio != "" ? Args.NombreLaboratorio : "") || Args.NombreLaboratorio == null || Args.NombreLaboratorio == "") /*NombreLaboratorio*/
                         && (p.RFCLaboratorio.Contains(Args.RFCLaboratorio != "" ? Args.RFCLaboratorio : "") || Args.RFCLaboratorio == null || Args.RFCLaboratorio == "") /*Titulo*/

                        && (p.EstatusID == enumEstatusAnalisis.Cancelado.ParseToInt())/*canceladas*/
                        && (p.Firmada == "SÍ");
                }
                else
                { /* Expression<Func<Vw_Ot_Grid, bool>> */
                    predicate = p => (p.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.Informe.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Anexo.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                        && (p.Norma.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                        && (p.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/
                        && (p.NombreLaboratorio.Contains(Args.NombreLaboratorio != "" ? Args.NombreLaboratorio : "") || Args.NombreLaboratorio == null || Args.NombreLaboratorio == "") /*NombreLaboratorio*/
                       && (p.RFCLaboratorio.Contains(Args.RFCLaboratorio != "" ? Args.RFCLaboratorio : "") || Args.RFCLaboratorio == null || Args.RFCLaboratorio == "") /*Titulo*/

                        && (p.Firmada == "SÍ");

                }


                Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);
            }
            else
            {
                if (Args.Cancel == "true")
                {
                    predicate = p => (
                         p.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "") /*RFC cliente*/
                         && (p.Informe.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                         && (p.Firmada == "SÍ")
                         && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                         && (p.Anexo.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                          && (p.Norma.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                           && (p.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/
                              && (p.EstatusID == enumEstatusAnalisis.Cancelado.ParseToInt())/*canceladas*/
                         && (p.LaboratorioID == LaboratorioID);
                }
                else
                {
                    predicate = p => (
                       p.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "") /*RFC cliente*/
                       && (p.Informe.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                       && (p.Firmada == "SÍ")
                       && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                       && (p.Anexo.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                        && (p.Norma.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                         && (p.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/

                       && (p.LaboratorioID == LaboratorioID);
                }




                Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);

            }
            return Lista_Orden;
        }

        public int CuentaOrdenTrabajoNoF(FiltroLaboratorioArgBase args, int usuarioRolId, int? laboratorioId = null)
        {
            int listaOrden = 0;

            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);



            Expression<Func<Vw_Ot_Grid, bool>> predicate = p => (p.RFC.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
                && (p.Informe.Contains(args.Numero != "" ? args.Numero : "") || args.Numero == null || args.Numero == "")
               && (p.Firmada == "NO") //&& (Args.Firmada && Args.NoFirmada ? true :
                                      //   (Args.Firmada && !Args.NoFirmada) ? p.Firmada =="SI" :
                                      //   (!Args.Firmada && Args.NoFirmada) ? p.Firmada == "NO" : false)
                && (p.FechaEmision >= (args.FechaInicial ?? fechaIniTemp) && p.FechaEmision <= (args.FechaFinal ?? fechaFinTemp) || (args.FechaInicial == null && args.FechaFinal == null))
                //&& (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true :
                && (p.Anexo.Contains(args.Anexo != "" ? args.Anexo : "") || args.Anexo == null || args.Anexo == "") /*Anexo*/
                && (p.Norma.Contains(args.Norma != "" ? args.Norma : "") || args.Norma == null || args.Norma == "") /*Norma*/
                && (p.Titulo.Contains(args.Titulo != "" ? args.Titulo : "") || args.Titulo == null || args.Titulo == "") /*Titulo*/
                && (p.LaboratorioID == laboratorioId);



            listaOrden = Repositorio.Count(predicate);


            return listaOrden;
        }
        public IEnumerable<Vw_Ot_Grid> ListaOrdenTrabajoNofirmada(FiltroLaboratorioArgBase Args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null, string OrderBy = "", string sortDirection = "ASC")
        {

            IEnumerable<Vw_Ot_Grid> Lista_Orden = new List<Vw_Ot_Grid>();
            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            var sortMethods = new List<Func<Vw_Ot_Grid, object>>();

            switch (OrderBy)
            {
                case "":
                    Func<Vw_Ot_Grid, object> obFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obFechaEmision);
                    break;
                case "Registro":
                    Func<Vw_Ot_Grid, object> obRegistro = p => p.FechaEmision;
                    sortMethods.Add(obRegistro);
                    break;
                case "Folio":
                    Func<Vw_Ot_Grid, object> obFolio = p => p.Informe;
                    sortMethods.Add(obFolio);
                    break;
                case "Titulo":
                    Func<Vw_Ot_Grid, object> obTitulo = p => p.Titulo;
                    sortMethods.Add(obTitulo);
                    break;
                case "Anexo":
                    Func<Vw_Ot_Grid, object> obAnexo = p => p.Anexo.ToString();
                    sortMethods.Add(obAnexo);
                    break;
                case "RFCCliente":
                    Func<Vw_Ot_Grid, object> obRFCCliente = p => p.RFC;
                    sortMethods.Add(obRFCCliente);
                    break;
                case "Norma":
                    Func<Vw_Ot_Grid, object> obNorma = p => p.Norma;
                    sortMethods.Add(obNorma);
                    break;
                default:
                    Func<Vw_Ot_Grid, object> obDefFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obDefFechaEmision);
                    break;

            }


            Expression<Func<Vw_Ot_Grid, bool>> predicate = p => (
                p.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "") /*RFC cliente*/
                && (p.Informe.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                && (p.Firmada == "NO")
                && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : fechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : fechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                && (p.Anexo.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                 && (p.Norma.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                  && (p.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/

                && (p.LaboratorioID == LaboratorioID);



            Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);


            return Lista_Orden;
        }

        public int CuentaOrdenTrabajoC(FiltroLaboratorioArgBase args, int usuarioRolId, int? laboratorioId = null)
        {
            int listaOrden = 0;

            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);



            Expression<Func<Vw_Ot_Grid, bool>> predicate = p => (p.RFC.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
                && (p.Informe.Contains(args.Numero != "" ? args.Numero : "") || args.Numero == null || args.Numero == "")
               && (p.EstatusID == enumEstatusAnalisis.Cancelado.ParseToInt()) //&& (Args.Firmada && Args.NoFirmada ? true :
                                                                              //   (Args.Firmada && !Args.NoFirmada) ? p.Firmada =="SI" :
                                                                              //   (!Args.Firmada && Args.NoFirmada) ? p.Firmada == "NO" : false)
                && (p.FechaEmision >= (args.FechaInicial ?? fechaIniTemp) && p.FechaEmision <= (args.FechaFinal ?? fechaFinTemp) || (args.FechaInicial == null && args.FechaFinal == null))
                //&& (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true :
                && (p.LaboratorioID == laboratorioId);



            listaOrden = Repositorio.Count(predicate);


            return listaOrden;
        }
        public IEnumerable<Vw_Ot_Grid> ListaOrdenTrabajoC(FiltroLaboratorioArgBase Args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null, string OrderBy = "", string sortDirection = "ASC")
        {

            IEnumerable<Vw_Ot_Grid> Lista_Orden = new List<Vw_Ot_Grid>();
            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            var sortMethods = new List<Func<Vw_Ot_Grid, object>>();

            switch (OrderBy)
            {
                case "":
                    Func<Vw_Ot_Grid, object> obFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obFechaEmision);
                    break;
                case "Registro":
                    Func<Vw_Ot_Grid, object> obRegistro = p => p.FechaEmision;
                    sortMethods.Add(obRegistro);
                    break;
                case "Folio":
                    Func<Vw_Ot_Grid, object> obFolio = p => p.Informe;
                    sortMethods.Add(obFolio);
                    break;
                case "Titulo":
                    Func<Vw_Ot_Grid, object> obTitulo = p => p.Titulo;
                    sortMethods.Add(obTitulo);
                    break;
                case "Anexo":
                    Func<Vw_Ot_Grid, object> obAnexo = p => p.Anexo.ToString();
                    sortMethods.Add(obAnexo);
                    break;
                case "RFCCliente":
                    Func<Vw_Ot_Grid, object> obRFCCliente = p => p.RFC;
                    sortMethods.Add(obRFCCliente);
                    break;
                case "Norma":
                    Func<Vw_Ot_Grid, object> obNorma = p => p.Norma;
                    sortMethods.Add(obNorma);
                    break;
                default:
                    Func<Vw_Ot_Grid, object> obDefFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obDefFechaEmision);
                    break;

            }


            Expression<Func<Vw_Ot_Grid, bool>> predicate = p => (
                p.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "") /*RFC cliente*/
                && (p.Informe.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                && (p.EstatusID == enumEstatusAnalisis.Cancelado.ParseToInt())
                && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : fechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : fechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                && (p.Anexo.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                 && (p.Norma.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                  && (p.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/

                && (p.LaboratorioID == LaboratorioID);



            Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);


            return Lista_Orden;
        }

        public IEnumerable<Vw_Ot_Grid> ListaOrdenTrabajo(FiltroOrdenTrabajoArgBase Args)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            var result = sbd.usp_BusquedaOrdenesTrabajo(
                Args.NumeroInforme,
                Args.NormaID,
                Args.RFC,
                Args.Titulo,
                Args.Anexo,
                Args.FechaEmisionInicio,
                Args.FechaEmisionFin,
                Args.FechaRegistroInicio,
                Args.FechaRegistroFin,
                Args.FechaConformacionInicio,
                Args.FechaConformacionFin,
                Args.LaboratorioID,
                Args.Firma,
                Args.EstatusCancelacion,
                Args.CurrentPage,
                Args.PageSize,
                Args.OrderBy,
                Args.OrderByDirection);
            return result;
        }

        public IEnumerable<uspBusquedaOrdenesTrabajoExcel_Result> ListaOrdenTrabajoExcel(FiltroOrdenTrabajoArgBase Args)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            ((IObjectContextAdapter)sbd).ObjectContext.CommandTimeout = 180;
            var result = sbd.uspBusquedaOrdenesTrabajoExcel(
                    string.IsNullOrEmpty(Args.NumeroInforme) ? null : Args.NumeroInforme,
                    Args.NormaID,
                    string.IsNullOrEmpty(Args.RFC) ? null : Args.RFC ,
                    string.IsNullOrEmpty(Args.Titulo) ? null : Args.Titulo,
                    string.IsNullOrEmpty(Args.Anexo) ? null : Args.Anexo,
                    Args.FechaEmisionInicio,
                    Args.FechaEmisionFin,
                    Args.FechaRegistroInicio,
                    Args.FechaRegistroFin,
                    Args.FechaConformacionInicio,
                    Args.FechaConformacionFin,
                    Args.LaboratorioID,
                    Args.Firma,
                    Args.EstatusCancelacion);
            return result;
        }

        public IEnumerable<uspConsultaSignatarios_Result> ListaMuestreador(FiltrosMuestreadorArgsBase Args)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            var result = sbd.uspConsultaSignatarios(
                Args.LaboratorioId,
                Args.FechaInicio,
                Args.FechaFin);
            return result;
        }

        public int ListaOrdenTrabajoCount(FiltroOrdenTrabajoArgBase filter)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            int? count;
            count = sbd.usp_BusquedaOrdenesTrabajoCount(
                filter.NumeroInforme,
                filter.NormaID,
                filter.RFC,
                filter.Titulo,
                filter.Anexo,
                filter.FechaEmisionInicio,
                filter.FechaEmisionFin,
                filter.FechaRegistroInicio,
                filter.FechaRegistroFin,
                filter.FechaConformacionInicio,
                filter.FechaConformacionFin,
                filter.LaboratorioID,
                filter.Firma,
                filter.EstatusCancelacion).FirstOrDefault().Total;

            return count ?? 0;
            return 0;
        }

        public IEnumerable<Analisi> ListaAnalisis(List<int> OrdenesTrabajo)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            var result = sbd.Analisis.Where(p => OrdenesTrabajo.Contains(p.OrdenTrabajoID))
                .Include(q => q.OrdenTrabajo.Norma)
                .Include(q => q.Muestras);

            return result;
        }
    }
}
