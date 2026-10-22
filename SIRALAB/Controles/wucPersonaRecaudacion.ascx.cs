using SIRALAB_BL;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucPersonaRecaudacion : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<UsuarioArgs> Guardar;

        [Browsable(true)]
        public event EventHandler Cancelar;

        [Browsable(true)]
        public event EventHandler<PersonaArgs> Seleccionar;

        public int? LaboratorioID { get { return hfLaboratorioID.Value == "" ? null : (int?)int.Parse(hfLaboratorioID.Value); } set { hfLaboratorioID.Value = value == null ? "" : value.ToString(); } }

        public int? PersonaID { get { return hfPersonaID.Value == "" ? null : (int?)int.Parse(hfPersonaID.Value); } set { hfPersonaID.Value = value == null ? "" : value.ToString(); } }

        public int? UsuarioID { get { return hfUsuarioID.Value == "" ? null : (int?)int.Parse(hfUsuarioID.Value); } set { hfUsuarioID.Value = value == null ? "" : value.ToString(); } }


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

        }

        public void Mostrar()
        {
            Limpiar();
            Actualizar();
        }


        public UsuarioArgs ObtenArgumentos()
        {
            UsuarioArgs us = new UsuarioArgs();

            if (this.UsuarioID > 0) 
                us.UsuarioID = this.UsuarioID;
            us.Usuario = txtUsuario.Text;
            us.Password = txtPass.Text;
            us.Correo = txtCorreoElectronico.Text;
            us.Activo = chkActivo.Checked;
            us.UsuarioRolID = UsuarioRolID;
            us.LaboratorioID = null;

            Persona personaargs = new Persona();

            if (this.PersonaID > 0)
                personaargs.PersonaID = (int)this.PersonaID;
            personaargs.Nombre = txtNombre.Text;
            personaargs.ApellidoPaterno = txtApellidoPaterno.Text;
            personaargs.ApellidoMaterno = txtApellidoMaterno.Text;

            personaargs.RFC = txtRFC.Text;
            personaargs.CURP = txtCURP.Text;
            personaargs.Activo = true;

            us.PersonaArgs = personaargs;

            return us;
        }

        public void Limpiar()
        {
            this.PersonaID = null;
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtRFC.Text = "";
            txtCURP.Text = "";
            txtCorreoElectronico.Text = "";
            txtPass.Text = "";
            txtUsuario.Text = "";
        }

        public void Actualizar()
        {
            //mpeCapturaUsuario.Show();
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalPersonaRecuadacion').modal({ backdrop : 'static' });", true);
            pnlCapturaUsuario.Update();
        }

        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            var arg = ObtenArgumentos();
            if (Guardar != null)
            {
                Guardar(this, arg);
            }

            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "HideModal", "removeBackdrop();", true);
            pnlCapturaUsuario.Update();
        }

        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            Cancelar(this, null);
        }

        public void Ocultar()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "HideModal", "removeBackdrop();", true);
            pnlCapturaUsuario.Update();
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void lnkbtnSel_Click(object sender, EventArgs e)
        {
            var text = txtNombre;

            SeleccionarBusqueda(text.Text, enumTipoPersonaBusqueda.Nombre);
            if (Seleccionar != null)
                Seleccionar(this, null);
        }

        private void SeleccionarBusqueda(string param, enumTipoPersonaBusqueda tipo_persona)
        {
            Persona_BL pBL = new Persona_BL();
            IEnumerable<Persona> personas = new List<Persona>();
            personas = pBL.Buscar(param, tipo_persona);
            var persona = personas.FirstOrDefault();

            if (persona != null)
            {
                RellenaCampos(persona);
            }
        }

        public void RellenaCampos(Persona persona)
        {
            var textNombre = txtNombre;
            var textRFC = txtRFC;

            this.PersonaID = persona.PersonaID;
            textNombre.Text = persona.Nombre;
            txtApellidoPaterno.Text = persona.ApellidoPaterno;
            txtApellidoMaterno.Text = persona.ApellidoMaterno;
            textRFC.Text = persona.RFC;
            txtCURP.Text = persona.CURP;
        }

        public void CargaUsuario(int UsuarioID)
        {
            Usuario_BL usuario_bl = new Usuario_BL();
            var usu = usuario_bl.Obten(UsuarioID);
            RellenaCampos(usu.Persona);

            txtUsuario.Text = usu.Usuario1;
            txtPass.Text = Crypt.Desencriptar(usu.Contrasenia);
            txtCorreoElectronico.Text = usu.Correo;
            chkActivo.Checked = usu.Activo;

        }
    }
}