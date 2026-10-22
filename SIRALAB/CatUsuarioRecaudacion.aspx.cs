using SIRALAB.Controles;
using SIRALAB_BL;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class CatUsuarioRecaudacion : System.Web.UI.Page
    {
        public IEnumerable<SIRALAB_DAL.Persona> Personas
        {
            get
            {
                return (Session["_CatPersona"] == null) ? new List<SIRALAB_DAL.Persona>() : (IEnumerable<SIRALAB_DAL.Persona>)Session["_CatPersona"];
            }
            set
            {
                Session["_CatPersona"] = value;
            }
        }

        private int UsuarioRolID
        {
            get
            {
                ApplicationSettings aS = new ApplicationSettings();
                return int.Parse(aS.UsuarioRolIdRecaudacion);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargaInformacion();
            }
        }

        /// <summary>
        /// carga la información
        /// </summary>
        private void CargaInformacion()
        {
            Persona_BL Persona = new Persona_BL();

            Personas = Persona.Obten_Todos(UsuarioRolID);
            BindGrid();
        }

        /// <summary>
        /// Bind grid
        /// </summary>
        private void BindGrid()
        {
            grdPersona.DataSource = Personas;
            grdPersona.DataBind();
        }

        /// <summary>
        /// Se agregara una demanda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPagina_Click(object sender, EventArgs e)
        {
            wucPersonaRecaudacion1.Mostrar();
        }

        /// <summary>
        /// Paginacion del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPersona_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPersona.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdPersona_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                var PersonaId = int.Parse(e.CommandArgument.ToString());

                Usuario_BL usuario_bl = new Usuario_BL();
                Persona_BL persona_bl = new Persona_BL();
                Persona per = new Persona();

                per = persona_bl.Obten_Persona(PersonaId);

                wucPersonaRecaudacion1.Mostrar();

                var usr = per.Usuarios.Select(a => a).FirstOrDefault();

                wucPersonaRecaudacion1.RellenaCampos(per);

                if (usr != null)
                    wucPersonaRecaudacion1.CargaUsuario(usr.UsuarioID);

                wucPersonaRecaudacion1.PersonaID = PersonaId;
                wucPersonaRecaudacion1.UsuarioID = usr.UsuarioID;

            }
        }

        protected void wucCapturaUsuarioPALA_Seleccionar(object sender, EventArgs e)
        {
            wucPersonaRecaudacion1.Mostrar();
        }

        protected void wucCapturaUsuarioPALA_Guardar(object sender, UsuarioArgs e)
        {
            try
            {
                var UsuarioArgs = e;

                Usuario_BL usuario_bl = new Usuario_BL();
                Persona_BL persona_bl = new Persona_BL();
                var persona = persona_bl.Guardar(UsuarioArgs.PersonaArgs);

                if (persona != null)
                {
                    e.PersonaID = persona.PersonaID;
                    usuario_bl.Guardar(e);
                    wucPersonaRecaudacion1.Ocultar();
                    CargaInformacion();
                }
            }
            catch (Exception ex)
            {
                this.PopMensaje(ex.Message, "Usuario", controles.Opciones.SoloAceptar, controles.Icono.information);
            }
        }

        private class PersonasArg : PersonaArgsBase
        {
        }

        private class UsuarioArg : UsuarioArgBase
        {
        }

        protected void grdPersona_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void lnkbtnback_Click(object sender, EventArgs e)
        {
            Navegacion nav = new Navegacion();
            string regreso = nav.ObtenNavegacion(1);
            Response.Redirect(regreso);
        }
    }
}