using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIRALAB_BL.catalogos
{
    public class Usuario_BL : Base_BL<Usuario>
    {
        public Usuario_BL()
            : base()
        {
        }

        public void Guardar(UsuarioArgBase usuario)
        {
            /// Validamos que el usuario de laboratorio no exista antes de guardarlo,
            /// sí el laboratorio tiene ya un usuario este no deberá poder guardar uno nuevo sino editar el existente
            /// sí el nombre de usuario ya existe entonces deberá enviar un error con la descripción de dicho inconveniente
            ///
            Usuario _usrDB;
            if (usuario.UsuarioID != null)
                _usrDB = Repositorio.Single(u => u.UsuarioID == usuario.UsuarioID);
            else
            {
                _usrDB = new SIRALAB_DAL.Usuario();
                _usrDB.UsuarioID = Repositorio.NextNewID(p => p.UsuarioID);
            }

            if (usuario.PersonaID != null)
                _usrDB.PersonaID = usuario.PersonaID;

            _usrDB.LaboratorioID = usuario.LaboratorioID;
            _usrDB.Usuario1 = usuario.Usuario;
            _usrDB.Contrasenia = Crypt.Encriptar(usuario.Password);
            _usrDB.UsuarioRolID = usuario.UsuarioRolID;
            _usrDB.Correo = usuario.Correo;
            _usrDB.Activo =  usuario.Activo;
            _usrDB.FechaRegistro = DateTime.Now;

            var usrLab2 = Repositorio.Single(p => p.Usuario1 == usuario.Usuario);
            if (usrLab2 != null && usuario.UsuarioID == null)
            {
                throw new Exception("Ya existe un usuario con ese nombre.");
            }
            var correo = Repositorio.Single(p => p.Usuario1 == usuario.Usuario);
            if (correo != null && usuario.UsuarioID == null)
            {
                throw new Exception("Ya existe un usuario con ese correo.");
            }
            if (usuario.UsuarioID != null)
            {
                Repositorio.Edit(_usrDB);
            }
            else
            {
                Repositorio.Add(_usrDB);
            }

            Repositorio.SaveChanges();
            this.Entidad = _usrDB;
        }

       


        public Usuario Obtener(int UsuarioID)
        {
            this.Entidad = Repositorio.Single(a => a.UsuarioID == UsuarioID);
            return Entidad;
        }

        public void GuardarSIRALAB(UsuarioArgBase usuario)
        {
            try
            {
                if (usuario.UsuarioID != null)
                    Obtener((int)usuario.UsuarioID);
                else
                    this.Entidad = null;

                if (this.Entidad != null) //Actualiza
                {
                    this.Entidad.LaboratorioID = usuario.LaboratorioID;
                    this.Entidad.Usuario1 = usuario.Usuario;
                    this.Entidad.Contrasenia = Crypt.Encriptar(usuario.Password);
                    this.Entidad.UsuarioRolID = usuario.UsuarioRolID;
                    this.Entidad.Correo = usuario.Correo;
                    this.Entidad.Activo = usuario.UsuarioID != null ? true : usuario.Activo;
                    this.Entidad.FechaRegistro = DateTime.Now;


                    Repositorio.SaveChanges();
                }
                else  //Nuevo
                {
                    this.Entidad = new Usuario();
                    this.Entidad.UsuarioID = Repositorio.NextNewID(x => x.UsuarioID);
                    this.Entidad.LaboratorioID = usuario.LaboratorioID;
                    this.Entidad.Usuario1 = usuario.Usuario;
                    this.Entidad.Contrasenia = Crypt.Encriptar(usuario.Password);
                    this.Entidad.UsuarioRolID = usuario.UsuarioRolID;
                    this.Entidad.Correo = usuario.Correo;
                    this.Entidad.Activo = usuario.UsuarioID != null ? true : usuario.Activo;
                    this.Entidad.FechaRegistro = DateTime.Now;

                    Repositorio.Add(Entidad);
                    Repositorio.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                this.Entidad = null;
                throw new Exception(ex.Message, ex.InnerException);
            }


        }



        public void CambiarPassword(int UsuarioID, string password1, string password2)
        {
            var _usrDB = Repositorio.Single(p => p.UsuarioID == UsuarioID);
            if (_usrDB != null)
            {
                if (Crypt.Encriptar(password1) == _usrDB.Contrasenia)
                {
                    _usrDB.Contrasenia = Crypt.Encriptar(password2);

                    Repositorio.SaveChanges();
                    this.Entidad = _usrDB;
                }
                else
                {
                    this.Mensaje = "La contraseña no es la correcta.";
                    throw new Exception("La contraseña no es la correcta.");
                }
            }
            else
            {
                this.Mensaje = "El usuario no es correcto o no existe.";
                throw new Exception("El usuario no es correcto o no existe.");
            }
        }

        public List<Usuario> Listado_usuarios()
        {
            List<Usuario> Lista_usuarios = new List<SIRALAB_DAL.Usuario>();
            Lista_usuarios = Repositorio.GetAll().ToList();
            return Lista_usuarios;
        }

        public Usuario Obten(int UsuarioID)
        {
            Usuario us = new SIRALAB_DAL.Usuario();
            us = Repositorio.Single(x => x.UsuarioID == UsuarioID);
            return us;
        }

        public List< Usuario> ObtenUsuarioRol(int UsuarioRolID)
        {
            List<Usuario> us = new List<Usuario>();
            us = Repositorio.Find(x => x.UsuarioRolID == UsuarioRolID).ToList();
            return us;
        }
        public bool Existe(string Usuario)
        {
            var us = Repositorio.Single(x => x.Usuario1 == Usuario);
            if (us != null)
                return true;
            else
                return false;
        }

        public List<Usuario> ObtenUsuariosByLaboritorioId(int laboratorioID)
        {
            return Repositorio.Find(user =>
                user.LaboratorioID == laboratorioID && user.Correo != "correo@correo.com" &&
                user.Correo != "corre@correo.com")
                .ToList();
        }

    }
}