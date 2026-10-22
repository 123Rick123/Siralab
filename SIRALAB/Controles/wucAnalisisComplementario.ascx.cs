using SIRALAB_BL.catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;

namespace SIRALAB.controles
{
    public partial class wucAnalisisComplementario : System.Web.UI.UserControl
    {

        [Browsable(true)]
        public event EventHandler<AnalisisComplementarioArg> Guardar;
        [Browsable(true)]
        public event EventHandler Cancelar;
        [Browsable(true)]
        public event EventHandler Cerrar;

        private int AnalisisID
        {
            get { return int.Parse(ViewState["AnalisisID_"].ToString()); }
            set { ViewState["AnalisisID_"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var main = (Main)this.Page.Master.Master;
            main.MensajeDevolucion += Main_MensajeDevolucion;
            if (!IsPostBack)
            {
                OrdenTrabajo_BL ordenTrabajoBL = new OrdenTrabajo_BL();
                var NormaID = ordenTrabajoBL.Obten((int)Session["OrdenTrabajoID_"])?.NormaID;
                SIRALAB_BL.common.LoadDrops.instance.NormaHijo(ddlNormaComplementaria, NormaID ?? 0);
            }
        }

        private void Main_MensajeDevolucion(object sender, MensajeArgs e)
        {
            if (e.Command != null && e.Opcion == Opcion.Aceptar)
            {
                switch (e.Command)
                {
                    case "ComplementarAnalisis":
                        bool result;
                        Hide();
                        OrdenTrabajo_BL ordenTrabajoBL = new OrdenTrabajo_BL();
                        Analisis_BL analisisBL = new Analisis_BL();
                        var AnalisisID = (int)Session["AnalisisID_"];
                        this.AnalisisID = AnalisisID;
                        int normaID = Int32.Parse(e.Args[0]);
                        var ordenTrabajoID = (int)Session["OrdenTrabajoID_"];

                        var orden = ordenTrabajoBL.GuardaComplementario(ordenTrabajoID, normaID);
                        result = analisisBL.AnalisisComplementario(AnalisisID, orden.OrdenTrabajoID);

                        if (result)
                            Page.PopMensaje("Se ha complementado el análisis correctamente con el número de informe: " + orden.NumeroInforme + ".", "Generar análisis complementario", Opciones.SoloAceptar, Icono.ok, null, null);
                        else
                            Page.PopMensaje("Ocurrio un error al complementar el análisis.", "Generar análisis complementario", Opciones.SoloAceptar, Icono.error, null, null);
                        break;
                }
            }
        }

        public void Mostrar()
        {
            Limpiar();
            mpeAnalisisComplementario.Show();
            upAnalisisComplementario.Update();
        }

        public void Hide()
        {
            Limpiar();

            mpeAnalisisComplementario.Hide();
            upAnalisisComplementario.Update();
        }

        private void Limpiar()
        {
            ddlNormaComplementaria.SelectedIndex = 0;
        }

        protected void lnkbGuardar_Click(object sender, EventArgs e)
        {
            var normaID = Int32.Parse(ddlNormaComplementaria.SelectedValue);
            Page.PopMensaje("¿Se encuentra seguro de generar el análisis complementario?", "Generar análisis complementario", Opciones.SiNo, Icono.question, "ComplementarAnalisis", new string[] { normaID.ToString() });
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

    public class AnalisisComplementarioArg : EventArgs
    {
        public int AnalisisID { get; set; }
    }
}