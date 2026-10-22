using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using System.Collections;
using System.Threading;
using System.Web;
using SIRALAB_BL.correo;

namespace SIRALAB_BL.negocio.login
{
    public class Login_BL
    {
        private Login_Usuario _Login { get; set; }

        private Login_Usuario Login
        {
            get { return _Login; }
            set { _Login = value; }
        }

        public List<Usuario> ListaUsuario { get; private set; }
        public string Mensaje { get; private set; }
        public Usuario Entidad { get; private set; }
        public LaboratorioVigencia Vigencia { get; private set; }
        public bool esLaboratorio { get; private set; }

        public Login_BL()
        {
            this.Login = new Login_Usuario();
        }

        public bool TieneAcceso(string usuario, string contrasenia)
        {
            /// Acá solo los Roles que tengan el acceso a la aplicación podrán ingresar a la aplicación de laboratorios;
            /// sí es un laboratorio se tiene que verificar sí el laboratorio tiene aprobación activa y representante activo

            bool respuesta = false;
            var usr = this.Login.Obten(usuario);

            if (usr != null)
            {
                respuesta = validaUsuarioIngreso(usr, contrasenia);
            }
            else
            {
                if (this.Login.ListaUsuario != null)
                    if (this.Login.ListaUsuario.Count > 0)
                    {
                        /* Es un laboratorio con sucursales */
                        /* Mostramos el listado de Laboratorios que tienen ese usuario como RFC y que tienen todos los requisitos para ingresar a SIRALAB */
                        this.esLaboratorio = true;
                        this.ListaUsuario = this.Login.ListaUsuario;
                        respuesta = false;
                    }
                    else
                    {
                        this.Mensaje = "No se encuentra registrado el usuario.";
                    }
                else
                {
                    this.Mensaje = "No se encuentra registrado el usuario.";
                }
            }

            return respuesta;
        }
        public bool TieneAcceso(Laboratorio laboratorio, string usuario, string contrasenia)
        {
            /// Acá solo los Roles que tengan el acceso a la aplicación podrán ingresar a la aplicación de laboratorios;
            /// sí es un laboratorio se tiene que verificar sí el laboratorio tiene aprobación activa y representante activo
            /// 
            bool respuesta = false;
            var usr = laboratorio.Usuarios.First();

            if (usr != null)
            {
                respuesta = validaUsuarioIngreso(usr, contrasenia);
            }
            else
            {
                this.Mensaje = "No se encuentra registrado el usuario.";
            }

            return respuesta;
        }

        private bool validaUsuarioIngreso(Usuario usr, string contrasenia)
        {
            bool respuesta = false;
            if (usr.Activo)
            {
                if (usr.Contrasenia == contrasenia.Encriptar())
                {
                    /// Ya sabemos que se ha firmado con credenciales válidas, ahora hay que verificar sí tiene aprobación activa y representante activo
                    /// 
                    if (usr.PersonaID != null || usr.LaboratorioID == null)
                    {
                        /// Acá verificamos que sea wl administrador de siralab o usuario recaudación
                        /// 
                        Seguridad_BL sec = new Seguridad_BL();
                        respuesta = sec.TienePermisoPagina(usr);
                        this.Entidad = usr;
                        this.esLaboratorio = false;

                        if (!respuesta)
                        {
                            this.Mensaje = "Usted no cuenta con los permisos suficientes para ingresar al sistema.";
                            this.Entidad = null;
                        }
                    }
                    else if (usr.PersonaID == null && usr.LaboratorioID != null)
                    {
                        var representante = usr.Laboratorio.LaboratorioRepresentantes.Where(p => p.Activo).SingleOrDefault();
                        var aprobacion = usr.Laboratorio.LaboratorioVigencias.Where(p => p.Activo).SingleOrDefault();
                        if (usr.Laboratorio.LaboratorioRepresentantes.Count > 0 && representante != null &&
                            usr.Laboratorio.LaboratorioVigencias.Count > 0 && aprobacion != null)
                        {
                            /// Si tiene representante activo,
                            /// Si tiene aprobación activa

                            Seguridad_BL sec = new Seguridad_BL();
                            respuesta = sec.TienePermisoPagina(usr);
                            this.Entidad = usr;
                            this.Vigencia = aprobacion;
                            this.esLaboratorio = true;

                            if (aprobacion.FechaFin < DateTime.Now)
                            {
                                respuesta = false;
                                //this.Entidad = null;
                                this.Mensaje = "Existe un problema, su aprobación ha expirado, unicamente tendrá disponible capturar análisis del periodo en el que su aprobación se encontraba vigente.";
                            }

                            if (!respuesta)
                            {
                                //this.Mensaje = "Usted no cuenta con los permisos suficientes para ingresar al sistema.";
                                this.Entidad = null;
                            }
                        }
                        else
                        {
                            if (representante == null)
                                this.Mensaje = "Existe un problema, no exite un representante activo para su laboratorio.";
                            if (aprobacion == null)
                                this.Mensaje = "Existe un problema, no tiene aprobación activa para su laboratorio.";
                        }
                    }
                    else
                        this.Mensaje = "Existe un problema con su cuenta, favor de verificarlo con el administrador de cuentas.";
                }
                else
                {
                    this.Mensaje = "Su contraseña de usuario no es correcta.";
                }
            }
            else
                this.Mensaje = "Su cuenta de usuario no está activa.";

            return respuesta;
        }

        public bool RecuperaContrasena(string correo)
        {
            var retorno = this.Login.RecuperaContrasena(correo);
            this.Mensaje = this.Login.Mensaje;

            return retorno;
        }

        public bool CambioContrasena(string correo)
        {
            var retorno = this.Login.CambioContrasena(correo);
            this.Mensaje = this.Login.Mensaje;

            return retorno;
        }

        public bool CambiarContrasena(Usuario Usuario, string AnteriorCapturada, string Nueva, string Confirmada, string Correo)
        {
            var retorno = this.Login.CambiarContrasena(Usuario, AnteriorCapturada, Nueva, Confirmada, Correo);
            this.Mensaje = this.Login.Mensaje;

            return retorno;
        }

        private class Login_Usuario : Base_BL<Usuario>
        {
            public List<Usuario> ListaUsuario { get; set; }
            public Login_Usuario()
                : base()
            {

            }
            internal Usuario Obten(string usuario)
            {
                Usuario usr = null;
                List<Usuario> lusr = new List<Usuario>();

                try
                {
                    //usr = this.dbContext.Usuarios
                    //    .Include("Persona")
                    //    .Include("Laboratorio")
                    //    .Include("CatUsuarioRol")
                    //    .Where(p => p.Usuario1 == usuario).FirstOrDefault();
                    usr = this.Repositorio.Single(p => p.Usuario1 == usuario);
                    this.Entidad = usr;
                }
                catch
                {
                    //lusr = this.dbContext.Usuarios
                    //    .Include("Persona")
                    //    .Include("Laboratorio")
                    //    .Include("CatUsuarioRol")
                    //    .Where(p => p.Usuario1 == usuario).ToList();
                    lusr = this.Repositorio.Find(p => p.Usuario1 == usuario).ToList();
                    this.ListaUsuario = lusr;
                }

                return usr;
            }

            public bool RecuperaContrasena(string correo)
            {
                bool _Valido = false;
                Usuario usr = Repositorio.Single(p => p.Correo == correo);
                ApplicationSettings ap = new ApplicationSettings();

                if (usr != null)
                {
                    var rol = usr.UsuarioRolID.ParseToEnumUsuarioRol();
                    if (rol == enumUsuarioRol.Administrador_SIRALAB || rol == enumUsuarioRol.Laboratorio || rol == enumUsuarioRol.Recaudacion)
                    {
                        /*
                            /// Elimiar de comentarios sí se desea que el password se modifique al solicitar la recuperación de password

                            string ContrasenaNueva = Usuario_DAL.GeneraClave();
                            string ContrasenaBase = Crypt.Crypt.Encriptar(ContrasenaNueva);
                            usuario.Contrasena = ContrasenaBase; // aquí se actualiza la contraseña del usuario
                        */

                        string Contrasena = Crypt.Desencriptar(usr.Contrasenia);

                        Hashtable ht = new Hashtable();
                        ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
                        ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));

                        string titulo1 = ap.CorreoTituloRecupera1;
                        string titulo2 = string.Format(ap.CorreoTituloRecupera2, Contrasena);
                        string mensaje = ap.CorreoMensajeRecupera;
                        string asunto = ap.CorreoAsuntoRecupera;

                        Thread threadSendMails3;
                        threadSendMails3 = new Thread(delegate ()
                        {
                            Mailer.SendMail(correo, titulo1, titulo2, mensaje, asunto, ht);
                        });
                        threadSendMails3.IsBackground = true;
                        threadSendMails3.Start();

                        this.Mensaje = "Se ha enviado la contraseña solicitada al correo electrónico indicado.";
                        _Valido = true;
                    }
                    else
                        this.Mensaje = "El Usuario no tiene permisos sobre esta aplicación.";
                }
                else
                    this.Mensaje = "El correo electrónico especificado no está registrado.";

                return _Valido;
            }
            public bool CambioContrasena(string correo)
            {
                bool _Valido = false;
                Usuario usr = Repositorio.Single(p => p.Correo == correo);
                ApplicationSettings ap = new ApplicationSettings();

                if (usr != null)
                {
                    var rol = usr.UsuarioRolID.ParseToEnumUsuarioRol();
                    if (rol == enumUsuarioRol.Administrador_SIRALAB || rol == enumUsuarioRol.Laboratorio || rol == enumUsuarioRol.Recaudacion)
                    {
                        string Contrasena = Crypt.Desencriptar(usr.Contrasenia);

                        Hashtable ht = new Hashtable();
                        ht.Add("enc_img_correo.png", HttpContext.Current.Server.MapPath(ap.MailImagePath));
                        ht.Add("Pie_Correo.png", HttpContext.Current.Server.MapPath(ap.MailImageFooterPath));

                        string titulo1 = ap.CorreoTituloRecupera3;
                        string titulo2 = string.Format(ap.CorreoTituloRecupera2, Contrasena);
                        string mensaje = ap.CorreoMensajeRecupera;
                        string asunto = ap.CorreoAsuntoRecupera;

                        Thread threadSendMails3;
                        threadSendMails3 = new Thread(delegate ()
                        {
                            Mailer.SendMail(correo, titulo1, titulo2, mensaje, asunto, ht);
                        });
                        threadSendMails3.IsBackground = true;
                        threadSendMails3.Start();

                        this.Mensaje = "Se ha enviado la contraseña modificada al correo electrónico indicado.";
                        _Valido = true;
                    }
                    else
                        this.Mensaje = "El Usuario no tiene permisos sobre esta aplicación.";
                }
                else
                    this.Mensaje = "El correo electrónico especificado no está registrado.";

                return _Valido;
            }
            public bool CambiarContrasena(Usuario Usuario, string AnteriorCapturada, string Nueva, string Confirmada, string Correo)
            {
                Usuario usrDAL = Repositorio.Single(p => p.UsuarioID == Usuario.UsuarioID);
                string ContrasenaAnterior = Crypt.Encriptar(AnteriorCapturada);
                bool valor = false;

                if (ContrasenaAnterior == Usuario.Contrasenia) // Contraseña capturada es igual a la que se encuentra almacenada en la base de datos?
                {
                    if (Nueva == Confirmada) // se verifica que la nueva contraseña sea igual a la confirmada y se procede a actualizar la nueva contraseña.
                    {
                        string ContrasenaNueva = Crypt.Encriptar(Nueva);
                        usrDAL.Correo = Correo;
                        usrDAL.Contrasenia = ContrasenaNueva;
                        Repositorio.SaveChanges();

                        this.Mensaje = "Contraseña modificada satisfactoriamente";
                        valor = true;
                    }
                    else
                        this.Mensaje = "No coinciden los datos de la nueva contraseña";
                }
                else
                    this.Mensaje = "No coincide con la contraseña anterior";

                return valor;
            }
        }
    }
}
