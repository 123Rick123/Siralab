using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using System.Linq.Expressions;

namespace SIRALAB_BL
{
    public class Consecion_BL
    {
        public Siralab_Entities ConsecionContext { get; private set; }
        public Consecion Consecion { get; private set; }

        Repository<Consecion> Repositorio;

        public Consecion_BL()
        {
            ConsecionContext = new Siralab_Entities();
            Repositorio = new Repository<Consecion>(ConsecionContext);
        }

        public void GuardaTitulos(ConsecionArgBase_BL args)
        {
            var con = new Consecion
            {
                ClienteID = args.ClienteID,
                Titulo = args.Titulo,
                PukConsecionID = args.PukConsecionID
            };
            try
            {
                if (args.ConsecionID == null)
                {
                    con.ConsecionID = Repositorio.NextNewID(x => x.ConsecionID);
                }
                else
                {
                    con.ConsecionID = (int)args.ConsecionID;
                }
                Repositorio.Add(con);
                Repositorio.SaveChanges();
                con = Repositorio.Single(x => x.ConsecionID == con.ConsecionID);
            }
            catch (Exception)
            {
                con = null;
            }
        }
        public void Guarda(ConsecionArgBase_BL args)
        {
            Consecion con = new Consecion();

            if (args.ConsecionID == null)
            {
                Obten(args.PukConsecionID, args.ClienteID, args.Titulo);
            }
            else
            {
                Obten((int)args.ConsecionID);
            }

            con = this.Consecion == null ? new Consecion() : this.Consecion;
            con.ConsecionID = this.Consecion != null ? this.Consecion.ConsecionID : Repositorio.NextNewID(x => x.ConsecionID);
            con.ClienteID = args.ClienteID;
            con.Titulo = args.Titulo;
            con.PukConsecionID = args.PukConsecionID;

            if (this.Consecion == null) Repositorio.Add(con);
            Repositorio.SaveChanges();
            this.Consecion = con;
        }

        public void Obten(string titulo, int clienteID)
        {
            this.Consecion = Repositorio.Single(x => x.Titulo == titulo && x.ClienteID == clienteID);
        }
        public void ObtenConsecion(string titulo, int clienteID)
        {
            this.Consecion = Repositorio.Single(x => x.Titulo == titulo && x.ClienteID == clienteID);
        }
        public void Obten(long? PukConsecionID, int clienteID)
        {
            this.Consecion = Repositorio.Single(x => x.PukConsecionID == PukConsecionID && x.ClienteID == clienteID);
        }

        public void Obten(long? PukConsecionID, int clienteID, string titulo)
        {
            var ConsecionTemp_1 = Repositorio.Single(x => x.Titulo.Trim() == titulo.Trim() && x.ClienteID == clienteID);
            var ConsecionTemp_2 = Repositorio.Single(x => x.PukConsecionID == PukConsecionID && x.ClienteID == clienteID);
            this.Consecion = null;

            if (ConsecionTemp_1 != null && ConsecionTemp_2 != null)
            {
                this.Consecion = ConsecionTemp_2;
            }
            else if (ConsecionTemp_1 == null && ConsecionTemp_2 != null)
            {
                //El título fue modificado
                this.Consecion = ConsecionTemp_2;
            }
            else if (ConsecionTemp_1 != null && ConsecionTemp_2 == null)
            {
                this.Consecion = ConsecionTemp_1;
            }
        }

        public void Obten(int ConsecionID)
        {
            this.Consecion = Repositorio.Single(x => x.ConsecionID == ConsecionID);
        }
        public void ObtenTitulo(int con)
        {
            this.Consecion = Repositorio.Single(x => x.ConsecionID == con);
        }

        public List<Consecion> Listado_Consecion(int ClienteID)
        {
            List<Consecion> consecion = new List<Consecion>();
            consecion = Repositorio.Find(x => x.ClienteID == ClienteID).ToList();
            return consecion;
        }
        public List<Consecion> Listado_Consecion(int ClienteID, bool fuente)
        {
            List<Consecion> consecion = new List<Consecion>();
            List<Consecion> resultado = new List<Consecion>();
            consecion = Repositorio.Find(x => x.ClienteID == ClienteID).ToList();

            consecion = Repositorio.Find(x => x.ClienteID == ClienteID && x.Anexoes.Any(a => a.AnexoMedidors.Any(an => an.FuenteAbastecimiento == fuente))).ToList();
           
            return consecion;
        }

        public List<Consecion> Listado_Consecion_titulo(string titulo)
        {
            List<Consecion> consecion = new List<Consecion>();
            consecion = Repositorio.Find(x => x.Titulo == titulo).ToList();
            return consecion;
        }

        public List<Consecion> Listado_Consecion_titulo(int ConsecionID)
        {
            List<Consecion> consecion = new List<Consecion>();
            consecion = Repositorio.Find(x => x.ConsecionID == ConsecionID).ToList();
            return consecion;
        }

        public string[] BuscaConsecion(string Consecion)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            Consecion = string.Join("%", Consecion.Split(' '));
            var conseciones = sbd.sp_BuscaConseciones(Consecion).Select(p => p.Titulo).ToArray();
            return conseciones.ToArray();
        }
    }
}
