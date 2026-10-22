using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.Controles;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.negocio.Contacto;
using SIRALAB_DAL;

namespace SIRALAB
{


    public partial class ContactosLaboratorio : Page
    {
        private static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            CargarContactos();
        }

        private void CargarContactos()
        {
            var contactoBl = new Contacto_BL();


            var usuarioBL = new Usuario_BL();

            var usuariosList = usuarioBL.ObtenUsuariosByLaboritorioId(Login.LaboratorioID ?? 0);
            var usuariosToArgs = usuariosList.Select(ctc => new ContactoArgs()
            {
                ContactoID = 0,
                Nombre = ctc.Laboratorio.Nombre,
                ApellidoPaterno = "",
                ApellidoMaterno = "",
                Correo = ctc.Correo,
                RecibeAcuse = true,
                EsLaboratorio = true
            }).ToList();

            var contactosLaboratorio = contactoBl.ObtenContactosCorreoAcuse(Login.LaboratorioID ?? 0);

            var correos = usuariosToArgs.Select(user => user.Correo).ToList();

            lnkbtnGuardarPmtos.Visible = contactosLaboratorio.Any(contacto => !correos.Contains(contacto.Correo));

            usuariosToArgs.AddRange(contactosLaboratorio.Where(contacto => !correos.Contains(contacto.Correo)));


            gvContactos.DataSource = usuariosToArgs;
            gvContactos.DataBind();
        }

        protected void lnkbtnback_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(1);
            Response.Redirect(regreso);
        }

        protected void gvContactos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected void lnkbtnGuadar(object sender, EventArgs e)
        {
            bool almacenadoCorrectamente = true;

            foreach (GridViewRow contacto in gvContactos.Rows)
            {
                var contactoBl = new Contacto_BL();
                var chkRecibeAcuse = (CheckBox)contacto.FindControl("chkRecibeAcuse");

                if (chkRecibeAcuse != null)
                {
                    var contactoId = (int?)gvContactos.DataKeys[contacto.RowIndex]?["ContactoID"];
                    if (contactoId != null && contactoId > 0)
                    {
                        var contactoObjeto = contactoBl.ObtenContacto(contactoId.Value);
                        contactoObjeto.RecibeAcuse = chkRecibeAcuse.Checked;


                        if (!contactoBl.UpdateContacto(contactoObjeto))
                        {
                            almacenadoCorrectamente = false;
                            break;
                        }
                    }
                }
            }

            CargarContactos();

            if (almacenadoCorrectamente)
            {
                this.PopMensaje("Receptores de acuse de firma y reporte de análisis de laboratorio almacenados correctamente.", "Contactos actualizados", controles.Opciones.SoloAceptar, controles.Icono.ok);
            }
            else
            {
                this.PopMensaje("Ocurrió un error al actualizar los receptores de acuse de firma y reporte de análisis de laboratorio.", "Contactos actualizados", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
    }
}