using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;

namespace SIRALAB_BL
{
    public class Cliente_BL
    {
        public Siralab_Entities ClienteContext { get; private set; }
        public Cliente Cliente { get; private set; }

        Repository<Cliente> Repositorio;

        public Cliente_BL()
        {
            ClienteContext = new Siralab_Entities();
            Repositorio = new Repository<Cliente>(ClienteContext);
        }


        public void Guarda(ClienteArgBase_BL args)
        {
            Cliente cli = new Cliente();

            if (args.ClienteID != null)
            {
                cli = Repositorio.Single(p => p.ClienteID == (int)args.ClienteID);
                if (cli == null) cli = new Cliente();
            }

            cli.Nombre = args.Nombre;
            cli.ApellidoPaterno = args.ApellidoPaterno;
            cli.ApellidoMaterno = args.ApellidoMaterno;
            cli.RFC = args.RFC;
            cli.CURP = args.CURP;
            cli.Correo = args.Correo;

            try
            {
                if (cli.ClienteID != 0) //Es una actualización
                {
                    cli.ClienteID = int.Parse(args.ClienteID.ToString());

                    Direccion_BL direccion_bl = new Direccion_BL();
                    direccion_bl.Guarda(args.dir);

                    if (direccion_bl.Direccion != null)
                    {
                        cli.DireccionID = direccion_bl.Direccion.DireccionID;
                        //Repositorio.Edit(cli);
                        Repositorio.SaveChanges();
                        this.Cliente = Repositorio.Single(x => x.ClienteID == cli.ClienteID);
                    }
                }
                else//Es un nuevo registro
                {
                    Direccion_BL direccion_bl = new Direccion_BL();
                    direccion_bl.Guarda(args.dir);

                    if (direccion_bl.Direccion != null)
                    {
                        cli.ClienteID = args.ClienteID != null ? (int)args.ClienteID : Repositorio.NextNewID(x => x.ClienteID);
                        cli.DireccionID = direccion_bl.Direccion.DireccionID;

                        Repositorio.Add(cli);
                        Repositorio.SaveChanges();
                        this.Cliente = Repositorio.Single(x => x.ClienteID == cli.ClienteID);
                    }

                }
            }
            catch (Exception ex)
            {
                this.Cliente = null;
            }
        }

        public void obtener(string RFC)
        {
            this.Cliente = Repositorio.Single(x => x.RFC == RFC);
        }

        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> ListaCliente = new List<SIRALAB_DAL.Cliente>();
            ListaCliente = Repositorio.GetAll().ToList();
            return ListaCliente;

        }

        public void obtener(int ClienteID)
        {
            this.Cliente = Repositorio.Single(x => x.ClienteID == ClienteID);
        }

        public string[] BuscaMuestreador(int LaboratorioID, string Nombre)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            Nombre = string.Join("%", Nombre.Split(' '));
            var Persona = sbd.sp_BuscaMuestreador(LaboratorioID, Nombre).Select(p => p.Nombre + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno).ToArray();
            return Persona.ToArray();
        }

        public List<Persona> BuscaMuestreador(int LaboratorioID)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            List<Persona> per = new List<Persona>();
            per = sbd.sp_BuscaMuestreador(LaboratorioID, "").ToList();
            return per;
        }

        public string[] BuscaClientes(string RFC)
        {
            Siralab_Entities sbd = new Siralab_Entities();
            RFC = string.Join("%", RFC.Split(' '));
            var clientes = sbd.sp_BuscaClientes(RFC).Select(p => p.RFC).ToArray();
            return clientes.ToArray();
        }
    }
}
