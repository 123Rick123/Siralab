using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using System.Linq.Expressions;

namespace SIRALAB_BL
{
    public class OrdenTrabajo_BL
    {
        public Siralab_Entities OrdenTrabajoContext { get; private set; }
        public OrdenTrabajo OrdenTrabajo { get; private set; }

        Repository<OrdenTrabajo> Repositorio;

        public OrdenTrabajo_BL()
        {
            OrdenTrabajoContext = new Siralab_Entities();
            Repositorio = new Repository<OrdenTrabajo>(OrdenTrabajoContext);
        }

        public void Guarda(OrdenTrabajoArgBase_BL args)
        {
            OrdenTrabajo ordentrabajo = new SIRALAB_DAL.OrdenTrabajo();
            ordentrabajo.NormaID = args.NormaID;
            ordentrabajo.ClienteID = args.ClienteID;

            ordentrabajo.EstatusID = args.EstatusID;
            ordentrabajo.FechaEmision = args.FechaEmision;
            ordentrabajo.NumeroInforme = args.NumeroInforme;
            ordentrabajo.Folio = args.Folio;
            ordentrabajo.Anio = args.Anio;
            ordentrabajo.Consecutivo = args.Consecutivo;
            ordentrabajo.HorasProceso = args.HorasProceso;
            ordentrabajo.AnexoMedidorID = args.AnexoMedidorID;
            ordentrabajo.CuerpoReceptorID = args.CuerpoReceptorID;
            ordentrabajo.UsoAguaID = args.UsoAguaID;

            /*Fuente abastecimiento*/
            ordentrabajo.FrecuenciaMuestreoID = args.FrecuenciaMuestreoID;
            ordentrabajo.FuenteAbastecimiento = args.FuenteAbastecimiento;

            //Cambios agregados el día 01/09/2014 CC3
            ordentrabajo.AnexoID = args.AnexoID;
            ordentrabajo.ConsecionID = args.ConsecionID;
            ordentrabajo.LaboratorioID = int.Parse(args.LaboratorioID.ToString());

            try
            {
                if (args.OrdenTrabajoID != null) //Es una actualización
                {
                    ordentrabajo.OrdenTrabajoID = int.Parse(args.OrdenTrabajoID.ToString());
                    Repositorio.Edit(ordentrabajo);
                    Repositorio.SaveChanges();

                    this.OrdenTrabajo = Repositorio.Single(x => x.OrdenTrabajoID == args.OrdenTrabajoID);
                }
                else  //Es un nuevo registro
                {
                    ordentrabajo.OrdenTrabajoID = Repositorio.NextNewID(x => x.OrdenTrabajoID);
                    Repositorio.Add(ordentrabajo);
                    Repositorio.SaveChanges();

                    this.OrdenTrabajo = Repositorio.Single(x => x.OrdenTrabajoID == ordentrabajo.OrdenTrabajoID);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public FrecuenciaMuestreo Calcula_FrecuenciaMuestreo(int HorasProceso)
        {
            FrecuenciaMuestreo frecuenciamuestreo = new FrecuenciaMuestreo();
            FrecuenciaMuestreo_BL frecuenciamuestreo_bl = new FrecuenciaMuestreo_BL();

            var fm = frecuenciamuestreo_bl.Listado_FrecuenciaMuestreo();

            foreach (var i in fm)
            {
                if (HorasProceso <= i.HorasMaximo)
                {
                    if (HorasProceso >= i.HorasMinimo)
                    {
                        frecuenciamuestreo = i;
                        break;
                    }
                }
            }
            return frecuenciamuestreo;
        }

        #region Grid
        public int Cuenta_OrdenTrabajo_JS(FiltroLaboratorioArgBase Args, int UsuarioRolID, int? LaboratorioID = null)
        {
            int Lista_Orden = 0;

            DateTime FechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime FechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);



            if (UsuarioRolID == enumUsuarioRol.Recaudacion.ParseToInt())
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Analisis.Where(y => y.Firmada).Count() > 0);

                Lista_Orden = Repositorio.Count(predicate);
            }
            else
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                    && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                    && (Args.Firmada && Args.NoFirmada ? true :
                       (Args.Firmada && !Args.NoFirmada) ? p.Analisis.Where(m => m.Firmada).Count() > 0 :
                       (!Args.Firmada && Args.NoFirmada) ? p.Analisis.Where(m => !m.Firmada).Count() > 0 : false)
                    && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                    && (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true : (p.LaboratorioID == LaboratorioID));



                Lista_Orden = Repositorio.Count(predicate);

            }
            return Lista_Orden;
        }

        public IEnumerable<OrdenTrabajo> Lista_OrdenTrabajo(FiltroLaboratorioArgBase Args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null, string OrderBy = "", string sortDirection = "ASC")
        {

            IEnumerable<OrdenTrabajo> Lista_Orden = new List<OrdenTrabajo>();
            DateTime FechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime FechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            var sortMethods = new List<Func<OrdenTrabajo, object>>();

            switch (OrderBy)
            {
                case "":
                    Func<OrdenTrabajo, object> obFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obFechaEmision);
                    break;
                case "Registro":
                    Func<OrdenTrabajo, object> obRegistro = p => p.FechaEmision;
                    sortMethods.Add(obRegistro);
                    break;
                case "Folio":
                    Func<OrdenTrabajo, object> obFolio = p => p.NumeroInforme;
                    sortMethods.Add(obFolio);
                    break;
                case "Titulo":
                    Func<OrdenTrabajo, object> obTitulo = p => p.Consecion.Titulo;
                    sortMethods.Add(obTitulo);
                    break;
                case "Anexo":
                    Func<OrdenTrabajo, object> obAnexo = p => p.Anexo.Anexo1.ToString();
                    sortMethods.Add(obAnexo);
                    break;
                case "RFCCliente":
                    Func<OrdenTrabajo, object> obRFCCliente = p => p.Cliente.RFC;
                    sortMethods.Add(obRFCCliente);
                    break;
                case "Norma":
                    Func<OrdenTrabajo, object> obNorma = p => p.Norma.Nombre;
                    sortMethods.Add(obNorma);
                    break;
                default:
                    Func<OrdenTrabajo, object> obDefFechaEmision = p => p.FechaEmision;
                    sortMethods.Add(obDefFechaEmision);
                    break;

            }

            if (UsuarioRolID == enumUsuarioRol.Recaudacion.ParseToInt())
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Analisis.Where(y => y.Firmada).Count() > 0);

                Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);
            }
            else
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (
                    p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "") /*RFC cliente*/
                    && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                    && (Args.Firmada && Args.NoFirmada ? true :
                       (Args.Firmada && !Args.NoFirmada) ? p.Analisis.Where(m => m.Firmada).Count() > 0 :
                       (!Args.Firmada && Args.NoFirmada) ? p.Analisis.Where(m => !m.Firmada).Count() > 0 : false)
                    && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                    && (p.Anexo.Anexo1.Contains(Args.Anexo != "" ? Args.Anexo : "") || Args.Anexo == null || Args.Anexo == "") /*Anexo*/
                     && (p.Norma.Nombre.Contains(Args.Norma != "" ? Args.Norma : "") || Args.Norma == null || Args.Norma == "") /*Norma*/
                      && (p.Consecion.Titulo.Contains(Args.Titulo != "" ? Args.Titulo : "") || Args.Titulo == null || Args.Titulo == "") /*Titulo*/

                    && (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true : (p.LaboratorioID == LaboratorioID));



                Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);

            }
            return Lista_Orden;
        }


        #endregion

        /*public List<OrdenTrabajo> Lista_OrdenTrabajo(FiltroLaboratorioArgBase Args, int UsuarioRolID, int? LaboratorioID = null)
        {
            List<OrdenTrabajo> Lista_Orden = new List<OrdenTrabajo>();
            DateTime FechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime FechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            if (UsuarioRolID == 2)
            {
                Expression<Func<OrdenTrabajo, bool>> predicate =  p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Analisis.Where(y => y.Firmada).Count() > 0);

                Expression<Func<OrdenTrabajo, DateTime>> orderby = p => p.FechaEmision;

                Lista_Orden = Repositorio.Find(predicate).OrderBy(q => q.FechaEmision).ToList();
            }
            else
            {
                Lista_Orden = Repositorio.Find(
                    p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                    && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                    && (Args.Firmada ? p.Analisis.Where(m => m.Firmada == Args.Firmada).Count() > 0 : true)
                    && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                    && (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true : (p.LaboratorioID == LaboratorioID))
                ).ToList();
            }
            return Lista_Orden;
        }*/
        public IEnumerable<OrdenTrabajo> Lista_OrdenTrabajo(FiltroLaboratorioArgBase Args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null)
        {
            IEnumerable<OrdenTrabajo> Lista_Orden = new List<OrdenTrabajo>();
            DateTime FechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime FechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            if (UsuarioRolID == enumUsuarioRol.Recaudacion.ParseToInt())
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Analisis.Where(y => y.Firmada).Count() > 0);

                Expression<Func<OrdenTrabajo, DateTime>> orderby = p => p.FechaEmision;
                Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, orderby);
            }
            else
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                    && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                    && (Args.Firmada && Args.NoFirmada ? true :
                       (Args.Firmada && !Args.NoFirmada) ? p.Analisis.Where(m => m.Firmada).Count() > 0 :
                       (!Args.Firmada && Args.NoFirmada) ? p.Analisis.Where(m => !m.Firmada).Count() > 0 : false)
                    && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                    && (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true : (p.LaboratorioID == LaboratorioID));

                Expression<Func<OrdenTrabajo, DateTime>> orderby = p => p.FechaEmision;
                Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, orderby);
            }
            return Lista_Orden;
        }

        public int Cuenta_OrdenTrabajo(FiltroLaboratorioArgBase Args, int UsuarioRolID, int? LaboratorioID = null)
        {
            int Lista_Orden = 0;
            DateTime FechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime FechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            if (UsuarioRolID == 2)
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                        && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                        && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                        && (p.Analisis.Where(y => y.Firmada).Count() > 0);

                Lista_Orden = Repositorio.Count(predicate);
            }
            else
            {
                Expression<Func<OrdenTrabajo, bool>> predicate = p => (p.Cliente.RFC.Contains(Args.RFCCliente != "" ? Args.RFCCliente : "") || Args.RFCCliente == null || Args.RFCCliente == "")
                    && (p.NumeroInforme.Contains(Args.Numero != "" ? Args.Numero : "") || Args.Numero == null || Args.Numero == "")
                    && (Args.Firmada ? p.Analisis.Where(m => m.Firmada == Args.Firmada).Count() > 0 : true)
                    && (p.FechaEmision >= (Args.FechaInicial != null ? Args.FechaInicial : FechaIniTemp) && p.FechaEmision <= (Args.FechaFinal != null ? Args.FechaFinal : FechaFinTemp) || (Args.FechaInicial == null && Args.FechaFinal == null))
                    && (p.Analisis.Where(q => q.LaboratorioID == LaboratorioID).Count() > 0 ? true : (p.LaboratorioID == LaboratorioID));

                Lista_Orden = Repositorio.Count(predicate);
            }
            return Lista_Orden;
        }

        public List<OrdenTrabajo> Lista_OrdenTrabajo()
        {
            //List<OrdenTrabajo> Lista_Orden = new List<OrdenTrabajo>();
            var Lista_Orden = Repositorio.GetAll().ToList();
            return Lista_Orden;
        }

        public OrdenTrabajo Obten(int OrdenTrabajoID)
        {
            var Orden = Repositorio.First(x => x.OrdenTrabajoID == OrdenTrabajoID);
            return Orden;
        }

        public OrdenTrabajo GuardaComplementario(int OrdenTrabajoID, int NormaID)
        {
            var args = Repositorio.First(x => x.OrdenTrabajoID == OrdenTrabajoID);
            OrdenTrabajo ordentrabajo = new OrdenTrabajo();
            ordentrabajo.OrdenTrabajoID = Repositorio.NextNewID(x => x.OrdenTrabajoID);
            ordentrabajo.NormaID = NormaID;
            ordentrabajo.ClienteID = args.ClienteID;
            ordentrabajo.EstatusID = args.EstatusID;
            ordentrabajo.FechaEmision = args.FechaEmision;
            ordentrabajo.NumeroInforme = args.NumeroInforme + "C";
            ordentrabajo.Folio = args.Folio;
            ordentrabajo.Anio = args.Anio;
            ordentrabajo.Consecutivo = args.Consecutivo;
            ordentrabajo.HorasProceso = args.HorasProceso;
            ordentrabajo.AnexoMedidorID = args.AnexoMedidorID;
            ordentrabajo.CuerpoReceptorID = args.CuerpoReceptorID;
            ordentrabajo.UsoAguaID = args.UsoAguaID;

            /*Fuente abastecimiento*/
            ordentrabajo.FrecuenciaMuestreoID = args.FrecuenciaMuestreoID;
            ordentrabajo.FuenteAbastecimiento = args.FuenteAbastecimiento;
            
            ordentrabajo.AnexoID = args.AnexoID;
            ordentrabajo.ConsecionID = args.ConsecionID;
            ordentrabajo.LaboratorioID = int.Parse(args.LaboratorioID.ToString());

            try
            {
                ordentrabajo.OrdenTrabajoID = Repositorio.NextNewID(x => x.OrdenTrabajoID);
                Repositorio.Add(ordentrabajo);
                Repositorio.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return ordentrabajo;
        }

        public string Eliminar(List<int> OrdenesTrabajo)
        {
            string mensajeResultado = "El proceso de eliminar concluyó correctamente.";
            var ordenes = Repositorio.Find(x => OrdenesTrabajo.Any(y => y == x.OrdenTrabajoID));
            var ordenesConFirma = ordenes.Where(p => p.Analisis.Any(q => q.Firmada));
            var ordenesSinFirma = ordenes.Where(p => !p.Analisis.Any(q => q.Firmada));


            using (Siralab_Entities sbd = new Siralab_Entities())
            {
                List<Analisis_BL.OrdenEliminarParam> Ids = ordenesSinFirma.Select(p => new Analisis_BL.OrdenEliminarParam { OrdenTrabajoID = p.OrdenTrabajoID }).ToList();
                try
                {
                    var table = Ids.ConvertToDataTable();
                    var paramName = "@OrdenesIds";
                    var procedureName = "usp_EliminaOrdenes";
                    var typeName = "OrdenEliminar";
                    //// create parameter 
                    System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter(paramName, table);
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.TypeName = typeName;

                    //// execute sp sql 
                    string sql = $"EXEC {procedureName} {paramName};";
                    //// execute sql 
                    sbd.Database.ExecuteSqlCommand(sql, parameter);
                    sbd.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            var ordenesNoEliminadas = string.Join(",", ordenesConFirma.Select(q => q.NumeroInforme));
            if (ordenesConFirma.Count() > 0)
            {
                return mensajeResultado + " Las órdenes que no se han eliminado son: " +
                       ordenesNoEliminadas + " ya que contienen análisis con firma.";
            }

            return mensajeResultado;
        }

        public void Dispose()
        {
            Repositorio.Dispose();
        }
    }
}
