using SIRALAB_BL.catalogos;
using System;
using System.ComponentModel;
using SIRALAB_BL.common;

namespace SIRALAB.Controles
{
    public partial class wucNorma : System.Web.UI.UserControl
    {
        #region Atributos

        public int? NormaID
        {
            set { Session["NormaID_Editar"] = value; }
            get { return Session["NormaID_Editar"] == null ? null : (int?)Session["NormaID_Editar"]; }
        }

        /// <summary>
        /// Si es una fuente de Abastecimiento
        /// </summary>
        public bool? EsFuenteAbastecimiento
        {
            set { Session["EsFuenteAbastecimiento"] = value; }
            get { return Session["EsFuenteAbastecimiento"] == null ? null : (bool?)Session["EsFuenteAbastecimiento"]; }
        }

        #endregion Atributos

        #region Eventos del control

        [Browsable(true)]
        public event EventHandler<NormaArg> Guardar;

        [Browsable(true)]
        public event EventHandler Cancelar;

        #endregion Eventos del control

        #region Metodos públicos

        public void Cargar(int? NormaID)
        {
            SIRALAB_DAL.Norma norma = null;
            this.NormaID = NormaID;
            if (NormaID != null)
            {
                Norma_BL cpBL = new Norma_BL();
                norma = cpBL.Obtener((int)NormaID);
            }

            Limpiar();
            if (norma != null)
            {
                Cargar(norma);
                Habilitar_Controles(false);
            }
            else
            {
                Habilitar_Controles(true);
            }

            upNorma.Update();
        }

        public void Editar_Norma()
        {
            if (this.NormaID != null)
            {
                Habilitar_Controles(true);
                upNorma.Update();
            }
        }

        public void Cancelar_Norma()
        {
            if (this.NormaID != null)
            {
                Habilitar_Controles(false);
                upNorma.Update();
            }
        }

        #endregion Metodos públicos

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void CargarCombos()
        {
            LoadDrops.instance.NormaPredecesora(ddlNormaPredecesora);
        }

        #region Metodos privados

        private void Cargar(SIRALAB_DAL.Norma norma)
        {
            CargarCombos();
            txtNombre.Text = norma.Nombre;
            txtFechaInicio.Text = norma.FechaInicio.ToShortDateString();
            txtDescripcion.Text = norma.Descripcion;
            chkActivo.Checked = norma.Activo;
            chkExentar.Checked = norma.ExentaParametros;
            chkCaudal.Checked = norma.RequiereCaudal;
            chkCaudal_CheckedChanged(null, null);
            chkMuestraCompuesta.Checked = norma.RequiereCompuesta;
            chkMuestraCompuesta_CheckedChanged(null, null);
            chkCPD.Checked = norma.CPD;
            rblValida.SelectedValue = norma.Evaluacion ? "1" : "0";
            chkFuenteAbastecimiento.Checked = norma.FuenteAbastecimiento ?? false;
            EsFuenteAbastecimiento = norma.FuenteAbastecimiento ?? false;
            chkMuestraSimples.Checked = Convert.ToBoolean(norma.MuestrasMultiple);
            chkAnalisisPrevio.Checked = norma.NormaPadreID == null ? false : true;
            chkPromedioMensual.Checked = Convert.ToBoolean(norma.RequiereMensual);
            ddlNormaPredecesora.SelectedValue = norma.NormaPadreID != null ? norma.NormaPadreID.ToString() : "0";
            ddlNormaPredecesora.Enabled = false;
            txtDesplazamiento.Text = norma.Desplazamiento.ToString();
            chkHorario.Checked = norma.RequiereHora ?? false;
            chkObservaciones.Checked = norma.RequiereObservaciones ?? false;
            chkRequiereUsoDeAgua.Checked = norma.RequiereUsoDeAgua ?? false;
            chk2021.Checked = norma.Nom2021 ?? false;
            //chkRequiereValorInstantaneo.Checked = norma.RequiereValorInstantaneo ?? false;
            upNorma.Update();
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtFechaInicio.Text = "";
            txtDescripcion.Text = "";
            chkActivo.Checked = true;
            chkCaudal.Checked = false;
            chkExentar.Checked = false;
            chkMuestraCompuesta.Checked = false;
            chkPromedioMensual.Checked = false;
            chkPromedioMensual.Visible = false;
            chkMuestraCompuesta.Visible = false;
            chkCPD.Checked = false;
            chkCaudal_CheckedChanged(null, null);
            chkMuestraCompuesta_CheckedChanged(null, null);
            chkFuenteAbastecimiento.Checked = false;
            EsFuenteAbastecimiento = null;
            ddlNormaPredecesora.Enabled = false;
            txtDesplazamiento.Text = "";

            upNorma.Update();
        }

        private void Habilitar_Controles(bool Habilitado)
        {
            txtNombre.Enabled = Habilitado;
            txtFechaInicio.Enabled = Habilitado;
            txtDescripcion.Enabled = Habilitado;
            chkActivo.Enabled = Habilitado;
            chkCaudal.Enabled = Habilitado;
            chkExentar.Enabled = Habilitado;
            chkMuestraCompuesta.Enabled = Habilitado;
            chkPromedioMensual.Enabled = Habilitado;
            chkCPD.Enabled = Habilitado;
            rblValida.Enabled = Habilitado;
            chkMuestraSimples.Enabled = Habilitado;
            chkAnalisisPrevio.Enabled = Habilitado;
            lnkbtnCancelar.Visible = Habilitado;
            lnkbtnGuardar.Visible = Habilitado;
            chkFuenteAbastecimiento.Enabled = Habilitado;
            txtDesplazamiento.Enabled = Habilitado;
            chkHorario.Enabled = Habilitado;
            chkObservaciones.Enabled = Habilitado;
            //chkRequiereValorInstantaneo.Enabled = Habilitado;
            chkRequiereUsoDeAgua.Enabled = Habilitado;
            //if (chkAnalisisPrevio.Checked && Habilitado)
            ddlNormaPredecesora.Enabled = chkAnalisisPrevio.Checked && Habilitado;
            //else ddlNormaPredecesora.Enabled = Habilitado;
            chk2021.Enabled = Habilitado;
            upNorma.Update();
        }

        #endregion Metodos privados

        #region Eventos de controles

        protected void lnkbtnCancelar_Click(object sender, EventArgs e)
        {
            if (this.NormaID != null)
            {
                Habilitar_Controles(false);
                //upNorma.Update();
                if (Cancelar != null)
                {
                    Cancelar(this, new EventArgs());
                    Cargar(this.NormaID);
                }
            }
            else
            {
                Limpiar();
                Session["NormaID_Editar"] = null;
                Response.Redirect("~/Norma.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void lnkbtnGuardar_Click(object sender, EventArgs e)
        {
            if (Guardar != null)
            {
                try
                {
                  
                    Guardar(this, new NormaArg
                    {
                        NormaID = this.NormaID,
                        ClienteID = null,
                        Nombre = txtNombre.Text,
                        Descripcion = txtDescripcion.Text,
                        FechaInicio = DateTime.Parse(txtFechaInicio.Text),
                        FechaFinal = DateTime.Now,
                        ExentaParametros = chkExentar.Checked,
                        RequiereCaudal = chkCaudal.Checked,
                        RequiereCompuesta = chkCaudal.Checked ? chkMuestraCompuesta.Checked : false,
                        RequiereMensual = chkCaudal.Checked ? chkPromedioMensual.Checked : false,
                        Evaluacion = rblValida.SelectedValue == "1",
                        CPD = chkCPD.Checked,
                        Activo = chkActivo.Checked,
                        FuenteAbastecimiento = chkFuenteAbastecimiento.Checked,
                        MuestrasMultiple = chkMuestraSimples.Checked,
                        NormaPadreID = ddlNormaPredecesora.SelectedValue==string.Empty ? null :(int?)int.Parse(ddlNormaPredecesora.SelectedValue),
                        Desplazamiento = txtDesplazamiento.Text == String.Empty ? null : (int?)int.Parse(txtDesplazamiento.Text),
                        RequiereHora = true,
                        RequiereObservaciones = true,
                        Nom2021 = chk2021.Checked,
                        RequiereUsoDeAgua = chkRequiereUsoDeAgua.Checked

                        //RequiereValorInstantaneo=chkRequiereValorInstantaneo.Checked
                    });
                }
                catch (Exception ex)
                {


                }
            }
            ddlNormaPredecesora.Enabled = false;
            upNorma.Update();
        }

        protected void chkCaudal_CheckedChanged(object sender, EventArgs e)
        {
            chkMuestraCompuesta.Visible = chkCaudal.Checked;
            chkMuestraCompuesta.Checked = false;

            chkMuestraCompuesta_CheckedChanged(null, null);
        }

        protected void chkMuestraCompuesta_CheckedChanged(object sender, EventArgs e)
        {
            chkPromedioMensual.Visible = chkMuestraCompuesta.Checked;
            chkPromedioMensual.Checked = false;
        }
        protected void chkAnalisisPrevio_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkAnalisisPrevio.Checked)
                ddlNormaPredecesora.ClearSelection();
            ddlNormaPredecesora.Enabled = chkAnalisisPrevio.Checked;
            upNorma.Update();
        }

        #endregion Eventos de controles


    
    }
}