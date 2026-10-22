using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.ordentrabajo
{
    public class VwOtK
    {
        private Siralab_Entities OrdenTrabajoContext { get; set; }
        Repository<Vw_Habilitar_Analis> Repositorio;
        Repository<TBitacora> Repo;
        public TBitacora Bitacora { get; private set; }
        public VwOtK()
        {
            OrdenTrabajoContext = new Siralab_Entities();
            Repositorio = new Repository<Vw_Habilitar_Analis>(OrdenTrabajoContext);
            Repo = new Repository<TBitacora>(OrdenTrabajoContext);

        }

        public static DataSet Lab(string parametro, string analis)
        {
            var query = string.Format(@"select LaboratorioID from muestravalor where ParametroID=" + parametro + "  and " +
                                      "MuestraID in (select MuestraID from muestra where AnalisisID=" + analis + " and NivelID=1) " +
                                      "and LaboratorioID is not null group by LaboratorioID");
            var queryResult = Selectdatos(query);
            return queryResult;
        }

        public static DataSet ReportSet(FiltroLaboratorioArgBase args, int? laboratorioId = null)
        {
            var anexo = "";
            if (args.Firmada)
                anexo += " and ANL.Firmada ='1' ";
            if (args.NoFirmada)
                anexo += " and ANL.Firmada ='0' ";
            if (args.Cancel == "true")
                anexo += " AND anl.EstatusID in(2,4) ";
            else
                anexo += " AND anl.EstatusID in(1,3) ";
            if (args.FechaInicial != null)
                anexo += " and OT.FechaEmision >= '" + Cambiar(args.FechaInicial.ToString()) + "' ";
            if (args.FechaFinal != null)
                anexo += " and OT.FechaEmision <= '" + Cambiar(args.FechaFinal.ToString()) + "' ";
            if (laboratorioId != null)
                anexo += " AND L.LaboratorioID='" + laboratorioId + "' ";
            if (args.RFCLaboratorio != null)
                anexo += " and L.RFC LIKE'%" + args.RFCLaboratorio + "%' ";
            if (args.NombreLaboratorio != null)
                anexo += " AND L.Nombre LIKE'%" + args.NombreLaboratorio + "%' ";




            var query =
                string.Format(@"SELECT CASE ANL.Firmada WHEN 1 	THEN 'SÍ' WHEN 0 THEN 'NO' 	END AS Firmada 	, CASE isnull(anl.EstatusID,1)   WHEN 1 THEN 'Activo'   WHEN 2 THEN 'Cancelado'   WHEN 3 THEN 'Activo'    WHEN 4 THEN 'Cancelado'   END AS [Estatus del análisis]   ,L.Nombre AS [Laboratorio que registra la OT] ,L.RFC AS [RFC Laboratorio] ,LABM.Nombre AS [Laboratorio que realiza el muestreo],PERS.Nombre + ' ' + PERS.ApellidoPaterno + ' ' + PERS.ApellidoMaterno AS Muestreador  ,LAB.Nombre AS [Laboratorio que analizó] ,OT.NumeroInforme AS Informe ,ANL.AnalisisID AS[Folio CONAGUA] 	,C.RFC AS [RFC usuario] 
	,C.Nombre AS [Nombre usuario] 	,CON.Titulo AS [Título] 	,A.Anexo AS Anexo 	,EDO.Nombre AS Estado 	,MUN.Nombre AS Municipio 	,CR.Nombre AS [Tipo de cuerpo receptor] 	,UA.Nombre AS [Uso del cuerpo receptor] 	,N.Nombre AS Norma 	,M.FechaRecepcion AS [Fecha de recepción en el laboratorio] 	,M.Fecha AS [Fecha de conformación] 	,OT.FechaEmision AS [Fecha de emisión] 	,ANL.FechaFirma AS [Fecha de firma] 	 , CASE anl.Procesado  WHEN 0   THEN 'SÍ'  WHEN 1  THEN 'NO' END AS [Está habilitado ] 	,OT.HorasProceso AS [Horas proceso]
	,NC.Nombre AS [Tipo muestra] ,CASE M.Caudal WHEN -1  THEN null  else M.Caudal end AS Caudal ,P.Nombre AS [Parámetro]  ,MV.Valor AS Resultado ,CASE MV.exento   WHEN 1  THEN 'SÍ'  ELSE 'NO'  END AS Exento       ,CASE MV.NoAnalizado   WHEN 1  THEN 'SÍ'   ELSE 'NO'  END AS [No analizado] ,UM.Nombre AS Unidad 	,MV.LC_MC AS [Lc/Mc] ,MTD.Nombre AS [Método]
FROM OrdenTrabajo OT 
INNER JOIN Laboratorio L ON OT.LaboratorioID = L.LaboratorioID INNER JOIN Cliente C ON C.ClienteID = OT.ClienteID INNER JOIN FrecuenciaMuestreo FM ON FM.FrecuenciaMuestreoID = OT.FrecuenciaMuestreoID INNER JOIN Estatus E ON E.EstatusID = OT.EstatusID INNER JOIN Norma N ON N.NormaID = OT.NormaID INNER JOIN CuerpoReceptor CR ON CR.CuerpoReceptorID = OT.CuerpoReceptorID INNER JOIN CatUsoAgua UA ON UA.UsoAguaID = OT.UsoAguaID
INNER JOIN Consecion CON ON CON.ConsecionID = OT.ConsecionID INNER JOIN Anexo A ON A.AnexoID = OT.AnexoID INNER JOIN Direccion DIR ON A.DireccionID = DIR.DireccionID INNER JOIN Estado EDO ON DIR.EstadoID = EDO.EstadoID INNER JOIN Municipio MUN ON DIR.EstadoID = MUN.EstadoID 	AND DIR.MunicipioID = MUN.MunicipioID INNER JOIN Analisis ANL ON ANL.OrdenTrabajoID = OT.OrdenTrabajoID LEFT JOIN Muestra M ON ANL.AnalisisID = M.AnalisisID
LEFT JOIN MuestraValor MV ON M.MuestraID = MV.MuestraID LEFT JOIN Laboratorio LAB ON LAB.LaboratorioID = mv.LaboratorioID LEFT JOIN Parametro P ON MV.ParametroID = P.ParametroID LEFT JOIN CatUnidadMedida UM ON P.UnidadMedidaID = UM.UnidadMedidaID LEFT JOIN ParametroMetodo PM ON MV.ParametroMetodoID = PM.ParametroMetodoID LEFT JOIN Metodo MTD ON PM.MetodoID = MTD.MetodoID LEFT JOIN NivelConfiguracion NC ON M.NivelID = NC.NivelID 
LEFT JOIN Muestreador MUES ON M.MuestraID = MUES.MuestraID LEFT JOIN LaboratorioSignatario LS ON MUES.LaboratorioSignatarioID = LS.LaboratorioSignatarioID LEFT JOIN Persona PERS ON LS.PersonaID = PERS.PersonaID LEFT JOIN Laboratorio LABM ON LABM.LaboratorioID = LS.LaboratorioID "
                + " WHERE c.rfc like'%" + args.RFCCliente + "%' 	AND OT.NumeroInforme like '%" + args.Numero + "%' 	and CON.Titulo like'%" + args.Titulo + "%' and A.Anexo like'%" + args.Anexo + "%' and N.Nombre like '%" + args.Norma + "%' "
                + "" + anexo + ""
                + " ORDER BY OT.FechaEmision,M.muestraid,OT.NumeroInforme ");
            var queryResult = Selectdatos(query);
            return queryResult;
        }

        private static string Cambiar(string s)
        {
            const string format = "yyyy-MM-dd";
            var dt = Convert.ToDateTime(s);
            s = dt.ToString(format);

            return s;
        }
        private static string ObtenerCadenaConexion()
        {
            var a = new Siralab_Entities();
            string conn = null;
            if (a.Database.Connection.ConnectionString != null)
                conn = a.Database.Connection.ConnectionString;

            return conn;
        }

        private static DataSet Selectdatos(string query)
        {
            SqlConnection sqlConexion = null;
            SqlDataAdapter proveedor = null;
            DataSet datos = null;
            try
            {
                sqlConexion = new SqlConnection(ObtenerCadenaConexion());
                proveedor = new SqlDataAdapter(query, sqlConexion);
                datos = new DataSet();
                proveedor.Fill(datos, "Reporte");
                return datos;
            }
            catch (Exception exp)
            {
                return null;
            }
            finally
            {
                if (sqlConexion != null) sqlConexion.Dispose();
                if (proveedor != null) proveedor.Dispose();
                if (datos != null) datos.Dispose();
            }
        }
        private DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            var properties =
               TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public IQueryable Vw(filtroshabilitarAnalisArgBase filtro)
        {
            IQueryable l2EQuery;
            if (filtro.foliocna != null)
            {
                l2EQuery = from st in OrdenTrabajoContext.Vw_Habilitar_Analis
                           where st.RFCLaboratorio.Contains(filtro.RFCLaboratorio) && st.NombreLaboratorio.Contains(filtro.NombreLaboratorio)
                               && st.RFCContribuyente.Contains(filtro.RFCCliente) && st.NombreContribuyente.Contains(filtro.NombreCliente)
                               && st.FoloCONAGUA == filtro.foliocna
                               && st.Informe.Contains(filtro.informe)
                           select st;

            }
            else
            {
                l2EQuery = from st in OrdenTrabajoContext.Vw_Habilitar_Analis
                           where st.RFCLaboratorio.Contains(filtro.RFCLaboratorio) && st.NombreLaboratorio.Contains(filtro.NombreLaboratorio)
                               && st.RFCContribuyente.Contains(filtro.RFCCliente) && st.NombreContribuyente.Contains(filtro.NombreCliente)

                               && st.Informe.Contains(filtro.informe)
                           select st;
            }

            return l2EQuery;
        }
        public int Cuenta_Vw(filtroshabilitarAnalisArgBase filtro)
        {
            var l2EQuery = 0;
            if (filtro.foliocna != null)
            {
                l2EQuery = (from st in OrdenTrabajoContext.Vw_Habilitar_Analis
                            where st.RFCLaboratorio.Contains(filtro.RFCLaboratorio) && st.NombreLaboratorio.Contains(filtro.NombreLaboratorio)
                                && st.RFCContribuyente.Contains(filtro.RFCCliente) && st.NombreContribuyente.Contains(filtro.NombreCliente)
                                && st.FoloCONAGUA == filtro.foliocna
                                && st.Informe.Contains(filtro.informe)
                            select st).Count();

            }
            else
            {
                l2EQuery = (from st in OrdenTrabajoContext.Vw_Habilitar_Analis
                            where st.RFCLaboratorio.Contains(filtro.RFCLaboratorio) && st.NombreLaboratorio.Contains(filtro.NombreLaboratorio)
                                && st.RFCContribuyente.Contains(filtro.RFCCliente) && st.NombreContribuyente.Contains(filtro.NombreCliente)

                                && st.Informe.Contains(filtro.informe)
                            select st).Count();
            }
            return l2EQuery;
        }
        public int Cuenta_OrdenTrabajo(filtroshabilitarAnalisArgBase args)
        {
            var listaOrden = 0;

            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
            Expression<Func<Vw_Habilitar_Analis, bool>> predicate;
            if (args.foliocna != null)
            {
                predicate = p =>
                    (p.RFCLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") || args.RFCLaboratorio == null || args.RFCLaboratorio == "")
                    && (p.NombreLaboratorio.Contains(args.NombreLaboratorio != "" ? args.NombreLaboratorio : "") || args.NombreLaboratorio == null || args.NombreLaboratorio == "")
                    && (p.RFCContribuyente.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
                    && (p.NombreContribuyente.Contains(args.NombreCliente != "" ? args.NombreCliente : "") || args.NombreCliente == null || args.NombreCliente == "")
                    && (p.Informe.Contains(args.informe != "" ? args.informe : "") || args.informe == null || args.informe == "")
                    && (p.FoloCONAGUA == args.foliocna)
                    && (p.Fecha_de_conformación >= (args.FechaInicialC != null ? args.FechaInicialC : fechaIniTemp) && p.Fecha_de_conformación <= (args.FechaFinalC != null ? args.FechaFinalC : fechaFinTemp) || (args.FechaInicialC == null && args.FechaFinalC == null))
                    && (p.Fecha_de_firma >= (args.FechaInicialF != null ? args.FechaInicialF : fechaIniTemp) && p.Fecha_de_firma <= (args.FechaFinalF != null ? args.FechaFinalF : fechaFinTemp) || (args.FechaInicialF == null && args.FechaFinalF == null));
            }
            else
            {
                predicate = p =>
                    (p.RFCLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") || args.RFCLaboratorio == null || args.RFCLaboratorio == "")
                    && (p.NombreLaboratorio.Contains(args.NombreLaboratorio != "" ? args.NombreLaboratorio : "") || args.NombreLaboratorio == null || args.NombreLaboratorio == "")
                    && (p.RFCContribuyente.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
                    && (p.NombreContribuyente.Contains(args.NombreCliente != "" ? args.NombreCliente : "") || args.NombreCliente == null || args.NombreCliente == "")
                    && (p.Informe.Contains(args.informe != "" ? args.informe : "") || args.informe == null || args.informe == "")
                    && (p.Fecha_de_conformación >= (args.FechaInicialC != null ? args.FechaInicialC : fechaIniTemp) && p.Fecha_de_conformación <= (args.FechaFinalC != null ? args.FechaFinalC : fechaFinTemp) || (args.FechaInicialC == null && args.FechaFinalC == null))
                    && (p.Fecha_de_firma >= (args.FechaInicialF != null ? args.FechaInicialF : fechaIniTemp) && p.Fecha_de_firma <= (args.FechaFinalF != null ? args.FechaFinalF : fechaFinTemp) || (args.FechaInicialF == null && args.FechaFinalF == null));

            }
            listaOrden = Repositorio.Count(predicate);


            return listaOrden;
        }
        public IEnumerable<Vw_Habilitar_Analis> ListaOrdenTrabajoNofirmada(filtroshabilitarAnalisArgBase args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null, string OrderBy = "", string sortDirection = "ASC")
        {

            IEnumerable<Vw_Habilitar_Analis> Lista_Orden = new List<Vw_Habilitar_Analis>();
            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            var sortMethods = new List<Func<Vw_Habilitar_Analis, object>>();

            switch (OrderBy)
            {
                case "":
                    Func<Vw_Habilitar_Analis, object> obFechaEmision = p => p.Fecha_de_firma;
                    sortMethods.Add(obFechaEmision);
                    break;
                case "Registro":
                    Func<Vw_Habilitar_Analis, object> obRegistro = p => p.Fecha_de_firma;
                    sortMethods.Add(obRegistro);
                    break;
                case "Folio":
                    Func<Vw_Habilitar_Analis, object> obFolio = p => p.Informe;
                    sortMethods.Add(obFolio);
                    break;
                case "Titulo":
                    Func<Vw_Habilitar_Analis, object> obTitulo = p => p.Titulo;
                    sortMethods.Add(obTitulo);
                    break;
                case "Anexo":
                    Func<Vw_Habilitar_Analis, object> obAnexo = p => p.Anexo.ToString();
                    sortMethods.Add(obAnexo);
                    break;
                case "RFCCliente":
                    Func<Vw_Habilitar_Analis, object> obRFCCliente = p => p.RFCContribuyente;
                    sortMethods.Add(obRFCCliente);
                    break;
                case "Norma":
                    Func<Vw_Habilitar_Analis, object> obNorma = p => p.Norma;
                    sortMethods.Add(obNorma);
                    break;
                default:
                    Func<Vw_Habilitar_Analis, object> obDefFechaEmision = p => p.Fecha_de_firma;
                    sortMethods.Add(obDefFechaEmision);
                    break;

            }
            Expression<Func<Vw_Habilitar_Analis, bool>> predicate;
            if (args.foliocna != null)
            {
                predicate = p =>
       (p.RFCLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") || args.RFCLaboratorio == null || args.RFCLaboratorio == "")
    && (p.NombreLaboratorio.Contains(args.NombreLaboratorio != "" ? args.NombreLaboratorio : "") || args.NombreLaboratorio == null || args.NombreLaboratorio == "")
    && (p.RFCContribuyente.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
    && (p.NombreContribuyente.Contains(args.NombreCliente != "" ? args.NombreCliente : "") || args.NombreCliente == null || args.NombreCliente == "")
    && (p.Informe.Contains(args.informe != "" ? args.informe : "") || args.informe == null || args.informe == "")
        && (p.FoloCONAGUA == args.foliocna)
    && (p.Fecha_de_conformación >= (args.FechaInicialC != null ? args.FechaInicialC : fechaIniTemp) && p.Fecha_de_conformación <= (args.FechaFinalC != null ? args.FechaFinalC : fechaFinTemp) || (args.FechaInicialC == null && args.FechaFinalC == null))
    && (p.Fecha_de_firma >= (args.FechaInicialF != null ? args.FechaInicialF : fechaIniTemp) && p.Fecha_de_firma <= (args.FechaFinalF != null ? args.FechaFinalF : fechaFinTemp) || (args.FechaInicialF == null && args.FechaFinalF == null));

            }
            else
            {
                predicate = p =>
       (p.RFCLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") || args.RFCLaboratorio == null || args.RFCLaboratorio == "")
    && (p.NombreLaboratorio.Contains(args.NombreLaboratorio != "" ? args.NombreLaboratorio : "") || args.NombreLaboratorio == null || args.NombreLaboratorio == "")
    && (p.RFCContribuyente.Contains(args.RFCCliente != "" ? args.RFCCliente : "") || args.RFCCliente == null || args.RFCCliente == "")
    && (p.NombreContribuyente.Contains(args.NombreCliente != "" ? args.NombreCliente : "") || args.NombreCliente == null || args.NombreCliente == "")
    && (p.Informe.Contains(args.informe != "" ? args.informe : "") || args.informe == null || args.informe == "")

    && (p.Fecha_de_conformación >= (args.FechaInicialC != null ? args.FechaInicialC : fechaIniTemp) && p.Fecha_de_conformación <= (args.FechaFinalC != null ? args.FechaFinalC : fechaFinTemp) || (args.FechaInicialC == null && args.FechaFinalC == null))
    && (p.Fecha_de_firma >= (args.FechaInicialF != null ? args.FechaInicialF : fechaIniTemp) && p.Fecha_de_firma <= (args.FechaFinalF != null ? args.FechaFinalF : fechaFinTemp) || (args.FechaInicialF == null && args.FechaFinalF == null));

            }





            Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);


            return Lista_Orden;
        }

        public TBitacora GuaradAdd(FiltrosBitacoraBase args)
        {

            try
            {
                var count = OrdenTrabajoContext.TBitacoras.Count();
                count = count + 1;
                var bit = new TBitacora
                {
                    idUser = args.IdUser,
                    Idrow = count,
                    FechaMod = args.FechaMod,
                    NUser = args.NUser,
                    ModuloMod = args.ModuloMod,
                    Firma = args.Firma,
                    CadenaFirma = args.CadenaFirma,
                    TipoMov = args.TipoMov,
                    NSerieCert = args.NSerieCert
                };
                Repo.Add(bit);
                Repo.SaveChanges();

                return bit;
            }
            catch (Exception ex)
            {
                this.Bitacora = null;
                return this.Bitacora;
            }

        }
    }
}
