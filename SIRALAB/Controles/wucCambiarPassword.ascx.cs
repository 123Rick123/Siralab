using SIRALAB_BL.catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.controles
{
    public partial class wucCambiarPassword : System.Web.UI.UserControl
    {

        [Browsable(true)]
        public event EventHandler<CambiarPasswordArg> Guardar;
        [Browsable(true)]
        public event EventHandler Cancelar;
        [Browsable(true)]
        public event EventHandler Cerrar;
        private int UsuarioID
        {
            get { return int.Parse(ViewState["UsuarioID"].ToString()); }
            set { ViewState["UsuarioID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Mostrar(int UsuarioID)
        {
            this.UsuarioID = UsuarioID;
            //Cargamos el correo electrónico del Usuario
            Usuario_BL uBL = new Usuario_BL();
            var usuario = uBL.Obten(UsuarioID);
        //    this.control_SetFocus("txtPassword");
            Limpiar();
            txtCorreo.Text = usuario.Correo;
            lnkbGuardar.Attributes.Add("data-toglet", usuario.Correo);
            hf.Value = usuario.Correo;
        
            
        }

        private void Limpiar()
        {
            txtCorreo.Text = "";
            txtPassword.Text = "";
            txtPasswordNuevo.Text = "";
            txtPasswordRepite.Text = "";
        }

        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            var Args = new CambiarPasswordArg {
                UsuarioID = this.UsuarioID,
                Correo = txtCorreo.Text,
                PasswordActual = txtPassword.Text,
                PasswordNuevo = txtPasswordNuevo.Text,
                PasswordRepite = txtPasswordRepite.Text
            };
            if (Guardar != null)
            {
                Guardar(this, Args);
            }
        }

        protected void lnkbCancelar_Click(object sender, EventArgs e)
        {
            if (Cancelar != null)
            {
                Cancelar(this, new EventArgs());
            }
        }

        protected void lnkbCerrar_Click(object sender, EventArgs e)
        {
            if (Cerrar != null)
            {
                Cerrar(this, new EventArgs());
            }
        }
    }

    public class CambiarPasswordArg : EventArgs
    {
        public int UsuarioID { get; set; }
        public string Correo { get; set; }
        public string PasswordActual { get; set; }
        public string PasswordNuevo { get; set; }
        public string PasswordRepite { get; set; }
    }
}