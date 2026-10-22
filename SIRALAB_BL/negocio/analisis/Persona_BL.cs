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
    public class Persona_BL
    {
        public Siralab_Entities PersonaContext { get; private set; }
        public Persona Persona { get; private set; }

        Repository<Persona> Repositorio;

        public Persona_BL()
        {
            PersonaContext = new Siralab_Entities();
            Repositorio = new Repository<Persona>(PersonaContext);
        }

        public Persona Obten_Persona(string Nombre)
        {
            var per = Repositorio.Find(x => x.Nombre + " " + x.ApellidoPaterno + " " + x.ApellidoMaterno == Nombre).FirstOrDefault();
            return per;
        }

        public Persona Obten_Persona(int PersonaID)
        {
            var per = Repositorio.Find(x => x.PersonaID == PersonaID).FirstOrDefault();
            return per;
        }

        public List<Persona> Obten_Todos()
        {
            var per = Repositorio.GetAll().ToList();
            return per;
        }


        public List<Persona> Obten_Todos(int UsuarioRolID)
        {
            var per = Repositorio.Find(u => u.Usuarios.Where(a => a.UsuarioRolID == UsuarioRolID).Count() > 0).ToList();
            return per;
        }


        public IEnumerable<Persona> Buscar(string param, enumTipoPersonaBusqueda tipoBusqueda)
        {
            IEnumerable<Persona> personaDB = null;
            IEnumerable<Persona> temppersonaDB = null;
            switch (tipoBusqueda)
            {
                case enumTipoPersonaBusqueda.Nombre:
                    // Búsqueda por medio del nombre de la persona
                    temppersonaDB = Repositorio.GetAll().ToList();
                    personaDB = temppersonaDB.Where(p => (p.Nombre + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno).Like(param.FilterSQLData()));
                    break;
                case enumTipoPersonaBusqueda.RFC:
                    // Búsqueda por medio del RFC de la persona
                    temppersonaDB = Repositorio.GetAll().ToList();
                    personaDB = temppersonaDB.Where(p => p.RFC.Like(param.FilterSQLData()));
                    break;
            }

            return personaDB;
        }

        public Persona Guardar(Persona PersonaArg)
        {
            Persona persona = new Persona();

            persona.Nombre = PersonaArg.Nombre;
            persona.ApellidoPaterno = PersonaArg.ApellidoPaterno;
            persona.ApellidoMaterno = PersonaArg.ApellidoMaterno;
            persona.RFC = PersonaArg.RFC;
            persona.CURP = PersonaArg.CURP;
            persona.Activo = PersonaArg.Activo;
            try
            {
                if (PersonaArg.PersonaID != 0) //Es una actualización
                {
                    persona.PersonaID = int.Parse(PersonaArg.PersonaID.ToString());
                    Repositorio.Edit(persona);
                    Repositorio.SaveChanges();
                    persona = Repositorio.Single(x => x.PersonaID == persona.PersonaID);
                }
                else//Es un nuevo registro
                {
                    persona.PersonaID = Repositorio.NextNewID(x => x.PersonaID);
                    Repositorio.Add(persona);
                    Repositorio.SaveChanges();
                    persona = Repositorio.Single(x => x.PersonaID == persona.PersonaID);
                }
                return persona;
            }
            catch (Exception ex)
            {
                persona = null;
                return persona;
            }
        }


    }
}