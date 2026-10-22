using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucParametrosMensual : System.Web.UI.UserControl
    {
        #region Propiedades

        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_"]; }
        }

        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }

        public Laboratorio LaboratorioDatos
        {
            get { return (Laboratorio)Session["LaboratorioDatos_"]; }
        }

        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }

        public LaboratorioSignatario Signatario_Laboratorio_Persona
        {
            get { return (LaboratorioSignatario)Session["Signatario_Laboratorio_Persona_"]; }
        }

        public bool EsCompuesta
        {
            get { return (bool)Session["EsCompuesta_"]; }
        }

        public int NivelID
        {
            get { return hfNivel.Value.Trim() == "" ? 0 : int.Parse(hfNivel.Value); }
            set { hfNivel.Value = value.ToString(); }
        }

        [Browsable(true)]
        public event EventHandler<bool> DatosFaltantesCompuesta;

        #endregion Propiedades

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ddlLaboratorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice;
            var ddl = (DropDownList)sender;

            indice = int.Parse(ddl.ToolTip);

            var lnkbtnParametroID = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnParametroID");
            var ddlMetodo = (DropDownList)rptValores.Items[indice].FindControl("ddlMetodo");

            Metodo_BL metodo_bl = new Metodo_BL();
            var lista_Metodos = metodo_bl.Obten_Metodo(int.Parse(ddl.SelectedValue.ToString()), int.Parse(lnkbtnParametroID.CommandArgument));

            ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_Metodos);
        }

        public void CargaInicialParametros(int NormaID, int MuestraID, int NivelID, bool EsCompuesta)
        {
            //Se general los valores de la muestra
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

            List<MuestraValorArgBase_BL> list = new List<MuestraValorArgBase_BL>();

            if (NivelID == 1)

                list = muestravalor_bl.GuardaObten(NormaID, MuestraID, NivelID, EsCompuesta);
            else
                list = muestravalor_bl.GuardaObtenCompuesta(NormaID, MuestraID);

            rptValores.DataSource = list.OrderBy(p => p.MuestraValorID).ToList();
            rptValores.DataBind();
        }

        public void CargaParametros()
        {
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

            if (this.OrdenTrabajo.Norma.NormaConfiguracions.Count > 0)
            {
                int NivelID = this.OrdenTrabajo.Norma.NormaConfiguracions.First().NivelID;

                //*********************************************************
                //En esta parte se mandará la variable si corresponde a una muestra simple o corresponde a una muestra compuesta
                //*********************************************************

                var list = muestravalor_bl.GuardaObtenCompuesta((int)this.OrdenTrabajo.NormaID, this.Muestra.MuestraID);
                if (list.Count > 0)
                {
                    rptValores.DataSource = list.OrderBy(p => p.MuestraValorID).ToList();
                    rptValores.DataBind();
                }
            }
        }

        protected void lnkbtnEditar__Click(object sender, EventArgs e)
        {
            int indice;
            var boton = (LinkButton)sender;

            indice = int.Parse(boton.CommandName);
            //Este boton contiene el ID de la MuestraValor
            var lnkbtnMuestraValor = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnMuestraValor");
            //Este boton contiene el ID del parametro
            var lnkbtnParametroID = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnParametroID");
            //Textbox que contiene el resultado
            var txtResultado = (TextBox)rptValores.Items[indice].FindControl("txtResultado");
            //ddl Que contiene el resultado en caso que no acepte valor
            var ddlResultado = (DropDownList)rptValores.Items[indice].FindControl("ddlResultado");
            //DDL con laboratorio seleccionado
            var ddlLaboratorio = (DropDownList)rptValores.Items[indice].FindControl("ddlLaboratorio");
            //DDL con metodo seleccionado
            var ddlMetodo = (DropDownList)rptValores.Items[indice].FindControl("ddlMetodo");
            //Boton invisible solo para editar
            var lnkbtnEditar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnEditar");
            //Boton Guardar
            var lnkbtnGuardar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnGuardar");

            //Activamos los controles
            lnkbtnEditar.Visible = false;
            lnkbtnGuardar.Visible = true;

            ddlLaboratorio.Enabled = true;
            ddlMetodo.Enabled = true;
            txtResultado.Enabled = true;

            if (txtResultado.Visible)
            {
                ddlResultado.Visible = false;
                ddlResultado.Enabled = false;
                txtResultado.Visible = true;
            }
            else
            {
                ddlResultado.Visible = true;
                ddlResultado.Enabled = true;
                txtResultado.Visible = false;
            }
        }

        protected void rptValores_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var lblTituloRepeater = (Label)e.Item.FindControl("lblTituloRepeater");

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MuestraValorConfig_BL configuracion = new MuestraValorConfig_BL();
                //Este boton contiene el ID de la MuestraValor
                var lnkbtnParametroID = (LinkButton)e.Item.FindControl("lnkbtnParametroID");
                var ddlLaboratorio = (DropDownList)e.Item.FindControl("ddlLaboratorio");
                var txtResultado = (TextBox)e.Item.FindControl("txtResultado");
                var ddlResultado = (DropDownList)e.Item.FindControl("ddlResultado");
                var ddlMetodo = (DropDownList)e.Item.FindControl("ddlMetodo");
                //Varibale utilizada para poner en visible o invisible el boton de exentar parametro
                var lnkbtnExentar = (LinkButton)e.Item.FindControl("lnkbtnExentar");
                //Variable utilizada para poner en visible o invisible el boton editar
                var lnkbtnEditar_ = (LinkButton)e.Item.FindControl("lnkbtnEditar");
                //Variable utilizada para indicar que un parámetro nu efue analizado por orden del cliente
                var lnkbtnNoAnalizado = (LinkButton)e.Item.FindControl("lnkbtnNoAnalizado");
                //Variable utilizada para capturar el limite cuantificable.
                var txtLC = (TextBox)e.Item.FindControl("txtLC");
                var lblResultado = (Label)e.Item.FindControl("lblResultado");

                configuracion.ConfiguraParametros(lblTituloRepeater, lnkbtnParametroID, ddlLaboratorio, txtResultado, ddlResultado, ddlMetodo, lnkbtnExentar, lnkbtnEditar_, lnkbtnNoAnalizado, txtLC, lblResultado, this.EsCompuesta, (int)this.OrdenTrabajo.NormaID, int.Parse(lnkbtnParametroID.CommandArgument), (int)this.Login.LaboratorioID, (MuestraValorArgBase_BL)e.Item.DataItem);
            }
        }

        protected void lnkbtnGuardarPmtos_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem i in rptValores.Items)
            {
                bool continuar = false;

                //Este boton contiene el ID de la MuestraValor
                var lnkbtnMuestraValor = (LinkButton)i.FindControl("lnkbtnMuestraValor");
                //Este boton contiene el ID del parametro
                var lnkbtnParametroID = (LinkButton)i.FindControl("lnkbtnParametroID");
                //Textbox que contiene el resultado
                var txtResultado = (TextBox)i.FindControl("txtResultado");
                //DDL que contiene si aprueba o no aprueba
                var ddlResultado = (DropDownList)i.FindControl("ddlResultado");
                //DDL con laboratorio seleccionado
                var ddlLaboratorio = (DropDownList)i.FindControl("ddlLaboratorio");
                //DDL con metodo seleccionado
                var ddlMetodo = (DropDownList)i.FindControl("ddlMetodo");
                //Boton invisible solo para editar
                var lnkbtnEditar = (LinkButton)i.FindControl("lnkbtnEditar");
                //Boton Guardar
                var lnkbtnGuardar = (LinkButton)i.FindControl("lnkbtnGuardar");
                //Textbox donde se captura el limite cuantificable
                var txtLC = (TextBox)i.FindControl("txtLC");
                var lnkbtnExentar = (LinkButton)i.FindControl("lnkbtnExentar");
                var lnkbtnNoAnalizado = (LinkButton)i.FindControl("lnkbtnNoAnalizado");

                var lblResultado = (Label)i.FindControl("lblResultado");


                 if (txtResultado.Visible)
                {
                    if (txtResultado.Text != "" && ddlLaboratorio.SelectedValue != "0" && ddlMetodo.SelectedValue != "0")
                        continuar = true;
                    else
                        continuar = false;
                }
                if (ddlResultado.Visible)
                {
                    if (ddlLaboratorio.SelectedValue != "0" && ddlMetodo.SelectedValue != "0")
                        continuar = true;
                    else
                        continuar = false;
                }

                if (continuar)
                {

                MuestraValorConfig_BL configuracion = new MuestraValorConfig_BL();
                //DESCOMENTAR (25-09-2014)
                configuracion.GuardarPmtos(lnkbtnMuestraValor, lnkbtnParametroID, txtResultado, ddlResultado, ddlLaboratorio, ddlMetodo, lnkbtnEditar, lnkbtnGuardar, txtLC, enumNivel.Compuesta.ConvertToInt(), (int)this.OrdenTrabajo.NormaID, (int)this.OrdenTrabajo.CuerpoReceptorID, (int)this.OrdenTrabajo.UsoAguaID, this.Muestra.MuestraID, lnkbtnExentar, lnkbtnNoAnalizado, lblResultado);
                }
                if (!continuar)
                {
                    DatosFaltantesCompuesta(this, true);                    
                }
            }
        }

        protected void lnkbtnExentar_Click(object sender, EventArgs e)
        {
            int MuestraValorID;
            var boton = (LinkButton)sender;
            MuestraValorID = int.Parse(boton.CommandArgument);
            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            var muestra = muestravalor_bl.Actualiza_Exento(MuestraValorID, true);
            if (muestra != null)
            {
                //Inicio inabilitamos todas las opciones correspondientes a esa fila de registros.

                int indice = 0;
                var botonC = (LinkButton)sender;
                indice = int.Parse(botonC.CommandName);

                //Textbox que contiene el resultado
                var txtResultado = (TextBox)rptValores.Items[indice].FindControl("txtResultado");
                //ddl Que contiene el resultado en caso que no acepte valor
                var ddlResultado = (DropDownList)rptValores.Items[indice].FindControl("ddlResultado");
                //Textbox donde se captura el limite cuantificable
                var txtLC = (TextBox)rptValores.Items[indice].FindControl("txtLC");
                //DDL con laboratorio seleccionado
                var ddlLaboratorio = (DropDownList)rptValores.Items[indice].FindControl("ddlLaboratorio");
                //DDL con metodo seleccionado
                var ddlMetodo = (DropDownList)rptValores.Items[indice].FindControl("ddlMetodo");
                //Variable utilizada para exentar el parámetro
                var lnkbtnExentar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnExentar");
                //Variable utilizada para no analizar el parámetro
                var lnkbtnNoAnalizado = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnNoAnalizado");
                //Variable utilizada para no analizar el parámetro
                var lblResultado = (Label)rptValores.Items[indice].FindControl("lblResultado");
                //Variable utilizada para el boton editar
                var lnkbtnEditar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnEditar");

                ddlLaboratorio.Enabled = false;
                ddlMetodo.Enabled = false;
                txtResultado.Enabled = false;
                txtLC.Enabled = false;
                lblResultado.Visible = false;
                lnkbtnExentar.Visible = false;
                lnkbtnNoAnalizado.Visible = false;
                lnkbtnEditar.Visible = true;

                //Fin de inabilitar controles
            }
        }

        protected void lnkbtnNoAnalizado_Click(object sender, EventArgs e)
        {
            int indice_ = 0;
            int MuestraValorID;
            var boton = (LinkButton)sender;
            MuestraValorID = int.Parse(boton.CommandArgument);
            indice_ = int.Parse(boton.CommandName);

            var rfv_ddlLaboratorio = (RequiredFieldValidator)rptValores.Items[indice_].FindControl("rfv_ddlLaboratorio");
            var rfv_ddlMetodo = (RequiredFieldValidator)rptValores.Items[indice_].FindControl("rfv_ddlMetodo");

            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            var muestra = muestravalor_bl.Actualiza_NoAnalizado(MuestraValorID, true);

            if (muestra != null)
            {
                //Inicio inabilitamos todas las opciones correspondientes a esa fila de registros.

                int indice = 0;
                var botonC = (LinkButton)sender;
                indice = int.Parse(botonC.CommandName);

                //Textbox que contiene el resultado
                var txtResultado = (TextBox)rptValores.Items[indice].FindControl("txtResultado");
                //ddl Que contiene el resultado en caso que no acepte valor
                var ddlResultado = (DropDownList)rptValores.Items[indice].FindControl("ddlResultado");
                //Textbox donde se captura el limite cuantificable
                var txtLC = (TextBox)rptValores.Items[indice].FindControl("txtLC");
                //DDL con laboratorio seleccionado
                var ddlLaboratorio = (DropDownList)rptValores.Items[indice].FindControl("ddlLaboratorio");
                //DDL con metodo seleccionado
                var ddlMetodo = (DropDownList)rptValores.Items[indice].FindControl("ddlMetodo");
                //Variable utilizada para exentar el parámetro
                var lnkbtnExentar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnExentar");
                //Variable utilizada para no analizar el parámetro
                var lnkbtnNoAnalizado = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnNoAnalizado");
                //Variable utilizada para no analizar el parámetro
                var lblResultado = (Label)rptValores.Items[indice].FindControl("lblResultado");
                //Variable utilizada para el boton editar
                var lnkbtnEditar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnEditar");

                ddlLaboratorio.Enabled = false;
                ddlMetodo.Enabled = false;
                txtResultado.Enabled = false;
                ddlResultado.Enabled = false;
                txtLC.Enabled = false;
                lblResultado.Visible = false;
                lnkbtnExentar.Visible = false;
                lnkbtnNoAnalizado.Visible = false;
                lnkbtnEditar.Visible = true;

                //Fin de inabilitar controles
            }
        }

        protected void lnkbtnEditar_Click(object sender, EventArgs e)
        {
            int indice = 0;
            var boton = (LinkButton)sender;
            indice = int.Parse(boton.CommandName);
            int MuestraValorID = int.Parse(boton.CommandArgument);

            MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
            muestravalor_bl.Actualiza_NoAnalizado(MuestraValorID, false);
            muestravalor_bl.Actualiza_Exento(MuestraValorID, false);

            //Textbox que contiene el resultado
            var txtResultado = (TextBox)rptValores.Items[indice].FindControl("txtResultado");
            //ddl Que contiene el resultado en caso que no acepte valor
            var ddlResultado = (DropDownList)rptValores.Items[indice].FindControl("ddlResultado");
            //Textbox donde se captura el limite cuantificable
            var txtLC = (TextBox)rptValores.Items[indice].FindControl("txtLC");
            //DDL con laboratorio seleccionado
            var ddlLaboratorio = (DropDownList)rptValores.Items[indice].FindControl("ddlLaboratorio");
            //DDL con metodo seleccionado
            var ddlMetodo = (DropDownList)rptValores.Items[indice].FindControl("ddlMetodo");
            //Variable utilizada para exentar el parámetro
            var lnkbtnExentar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnExentar");
            //Variable utilizada para no analizar el parámetro
            var lnkbtnNoAnalizado = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnNoAnalizado");
            //Variable utilizada para no analizar el parámetro
            var lblResultado = (Label)rptValores.Items[indice].FindControl("lblResultado");
            //Variable utilizada para el boton editar
            var lnkbtnEditar = (LinkButton)rptValores.Items[indice].FindControl("lnkbtnEditar");

            var rfv_ddlLaboratorio = (RequiredFieldValidator)rptValores.Items[indice].FindControl("rfv_ddlLaboratorio");
            var rfv_ddlMetodo = (RequiredFieldValidator)rptValores.Items[indice].FindControl("rfv_ddlMetodo");

            var muestraValor = muestravalor_bl.Obten(MuestraValorID);

            ddlLaboratorio.Enabled = true;
            ddlMetodo.Enabled = true;
            lnkbtnExentar.Enabled = true;
            lnkbtnExentar.Visible = true;
            lnkbtnNoAnalizado.Enabled = true;
            lnkbtnNoAnalizado.Visible = true;
            lblResultado.Visible = false;
            lnkbtnEditar.Visible = false;

            //Configuramos sección de valores que pueden exentar
            if (muestraValor.Exenta)
                lnkbtnExentar.Visible = true;
            else
                lnkbtnExentar.Visible = false;

            //Configuramos parámetros que aceptan valor.
            if (muestraValor.AceptaValor)
            {
                txtResultado.Visible = true;
                txtResultado.Enabled = true;

                txtLC.Visible = true;
                txtLC.Enabled = true;
            }
            else
            {
                ddlResultado.Visible = true;
                ddlResultado.Enabled = true;
            }
        }
    }
}