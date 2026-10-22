using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.Extemporaneos
{
    public class Extemporaneos_BL
    {
        private Siralab_Entities OrdenTrabajoContext { get; set; }
        Repository<LaboratoriosCNA> Repositorio;


        public Extemporaneos_BL()
        {
            OrdenTrabajoContext = new Siralab_Entities();
            Repositorio = new Repository<LaboratoriosCNA>(OrdenTrabajoContext);  
        }

        public static string Cuenta()
        {
            var a = new Siralab_Entities();
            string conn = null;
            if (a.Database.Connection.ConnectionString != null)
                conn = a.Database.Connection.ConnectionString;

            return conn;
        }

        public int Cuenta(string searchString)
        {
            var l2EQuery = 0;
            if (searchString != null)
            {
                l2EQuery = (from st in OrdenTrabajoContext.LaboratoriosCNAs
                            where st.RfcLaboratorio.Contains(searchString)
                            select st).Count();

            }

            return l2EQuery;
        }



        public IEnumerable<LaboratoriosCNA> laboratoriosce(filtrolaboratoriocnaArgBase args, int page, int pageSize, int UsuarioRolID, int? LaboratorioID = null, string OrderBy = "", string sortDirection = "ASC")
        {

            IEnumerable<LaboratoriosCNA> Lista_Orden = new List<LaboratoriosCNA>();
            var fechaIniTemp = new DateTime(DateTime.Now.Year, 1, 1);
            var fechaFinTemp = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);

            var sortMethods = new List<Func<LaboratoriosCNA, object>>();

            switch (OrderBy)
            {
                case "": Func<LaboratoriosCNA, object> obRFCCliente0 = p => p.RfcLaboratorio;
                    sortMethods.Add(obRFCCliente0);
                    break;

                case "RFCCliente": Func<LaboratoriosCNA, object> obRFCCliente1 = p => p.RfcLaboratorio;
                    sortMethods.Add(obRFCCliente1);
                    break;
                default: Func<LaboratoriosCNA, object> obRFCCliente2 = p => p.RfcLaboratorio;
                    sortMethods.Add(obRFCCliente2);
                    break;

            }
            Expression<Func<LaboratoriosCNA, bool>> predicate;
            if (args.RFCLaboratorio != null)
            {
                predicate = p =>
                    (p.RfcLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") ||
                     args.RFCLaboratorio == null || args.RFCLaboratorio == "");


            }
            else
            {
                predicate = p =>
                       (p.RfcLaboratorio.Contains(args.RFCLaboratorio != "" ? args.RFCLaboratorio : "") ||
                        args.RFCLaboratorio == null || args.RFCLaboratorio == "");
            }

            Lista_Orden = Repositorio.FindPaged(page, pageSize, predicate, sortMethods, sortDirection);


            return Lista_Orden;
        }

    }
}
