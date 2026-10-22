using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB_DAL;

namespace SIRALAB.Controles
{
    public partial class wucFiltroNormaConfiguracion : System.Web.UI.UserControl
    {
        public bool? RequiereUsoDeAgua
        {
            set { Session["RequiereUsoDeAgua"] = value; }
            get { return Session["RequiereUsoDeAgua"] == null ? null : (bool?)Session["RequiereUsoDeAgua"]; }
        }
        public bool? Nom2021
        {
            set { Session["Nom2021"] = value; }
            get { return Session["Nom2021"] == null ? null : (bool?)Session["Nom2021"]; }
        }
        public string NombreNorma2021
        {
            set { Session["NombreNorma2021"] = value; }
            get { return Session["NombreNorma2021"] == null ? null : (string)Session["NombreNorma2021"]; }
        }
        #region Atributos
        public FiltroNormaConfiguracionArg Filtro { get { return CargarFiltro(); } }
        #endregion Atributos

        #region Eventos del control
        [Browsable(true)]
        public event EventHandler<FiltroNormaConfiguracionArg> Buscar;

        [Browsable(true)]
        public event EventHandler<FiltroNormaConfiguracionArg> Limpiar;

        [Browsable(true)]
        public event EventHandler After_Load;


        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDrops(NombreNorma2021);
                After_Load?.Invoke(this, null);
            }
           
        }
        protected void chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            Buscar?.Invoke(this, CargarFiltro());
        }
        protected void ddlCuerpoReceptor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar?.Invoke(this, CargarFiltro());
        }
        protected void ddlUsoAgua_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar?.Invoke(this, CargarFiltro());
        }
        #endregion Eventos

        #region MetodosFunciones
        private FiltroNormaConfiguracionArg CargarFiltro()
        {
            FiltroNormaConfiguracionArg filtro = new FiltroNormaConfiguracionArg();

            filtro.ParametroNombre = "";
            filtro.CuerpoReceptorID = int.Parse(ddlCuerpoReceptor.SelectedValue);
            filtro.UsoAguaID = int.Parse(ddlUsoAgua.SelectedValue);
            filtro.Inactivos = true;
            
            return filtro;
        }
        private void CargarDrops(string nombreNorma)
        {
            CatUsoAgua_BL ctusoAgua = new CatUsoAgua_BL();
            var usoAgua = ctusoAgua.ObtenerTodos().OrderBy(p => p.Nombre);
            ddlUsoAgua.LoadDropSinSel("Nombre", "UsoAguaID", usoAgua);

            CuerpoReceptor_BL ctcr_bl = new CuerpoReceptor_BL();
            var cuerposReceptores = ctcr_bl.ObtenerTodos().OrderBy(p => p.Nombre);
            var aplicationSettings = new ApplicationSettings();
            //RGG Se valida para mostrar los cuerpos receptores solo de la norma 2021 

            if (Nom2021 != null && Nom2021 == true)
            {
                if (nombreNorma.Replace(" ", "") == aplicationSettings.NormaCOT)
                {
                    var cuerposDOT = cuerposReceptores.Where(x => x.Nombre == "N/A").ToList();
                    ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposDOT);
                }
                else if (nombreNorma.ToUpper().Replace(" ", "") == aplicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                {
                    var cuerposDOT = cuerposReceptores.Where(x => x.Nombre == "N/A").ToList();
                    ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposDOT);
                }
                else
                {
                    var cuerposReceptores2021 = cuerposReceptores.Where(x => x.Norma == "2021").ToList();
                    ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposReceptores2021);
                }               
            }
            else
            {
                var cuerposReceptoresOtrasNormas = cuerposReceptores.Where(x => x.Norma != "2021").ToList();
                ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposReceptoresOtrasNormas);
            }



            //if ((RequiereUsoDeAgua != null && RequiereUsoDeAgua.Value == true) && (NombreNorma2021 != null && NombreNorma2021.Trim() == "NOM-001-SEMARNAT-2021"))
            if (RequiereUsoDeAgua != null && RequiereUsoDeAgua.Value == false)
            {
                ddlUsoAgua.SelectedValue = "10";//RGG El 10 indica que se va a usar el N/A en uso de agua 
                ddlCuerpoReceptor.SelectedIndex = 0;
                ddlUsoAgua.Visible = false;
                lblUsoDeAgua.Visible = false;
            }
            else
            {
                ddlUsoAgua.SelectedIndex = 0;
                ddlCuerpoReceptor.SelectedIndex = 0;
                ddlUsoAgua.Visible = true;
                lblUsoDeAgua.Visible = true;
            }
        
        }
        //RGG Metodo para actualizar los drops de uso del agua ponerlo N/A si es para la norma 2021 
        public void CargarDrops2021(string nombreNorma)
        {
            CatUsoAgua_BL ctusoAgua = new CatUsoAgua_BL();
            var usoAgua = ctusoAgua.ObtenerTodos().OrderBy(p => p.Nombre);
            ddlUsoAgua.LoadDropSinSel("Nombre", "UsoAguaID", usoAgua);
            //if (RequiereUsoDeAgua.Value == true && NombreNorma2021.Trim() == "NOM-001-SEMARNAT-2021")
            if (RequiereUsoDeAgua.Value == false)
            {
                ddlUsoAgua.SelectedValue = "10";//El 10 indica que se va a usar el N/A en uso de agua
                ddlUsoAgua.Visible = false;
                lblUsoDeAgua.Visible = false;             
            }
            else
            {
                ddlUsoAgua.SelectedIndex = 0;
                ddlUsoAgua.Visible = true;
                lblUsoDeAgua.Visible = true;
            }
            CuerpoReceptor_BL ctcr_bl = new CuerpoReceptor_BL();
            var cuerposReceptores = ctcr_bl.ObtenerTodos().OrderBy(p => p.Nombre);
            var aplicationSettings = new ApplicationSettings();
            //RGG Se valida para mostrar los cuerpos receptores solo de la norma 2021 

            if (Nom2021 != null && Nom2021 == true)
            {
                if (nombreNorma.Replace(" ","") == aplicationSettings.NormaCOT)
                {
                    var cuerposDOT = cuerposReceptores.Where(x => x.Nombre == "N/A").ToList();
                    ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposDOT);
                }
                else if (nombreNorma.ToUpper().Replace(" ", "") == aplicationSettings.NormaFuenteAbastecimiento2021.Replace(" ", ""))
                {
                    var cuerposDOT = cuerposReceptores.Where(x => x.Nombre == "N/A").ToList();
                    ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposDOT);
                }
                else
                {
                    var cuerposReceptores2021 = cuerposReceptores.Where(x => x.Norma == "2021").ToList();
                    ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposReceptores2021);
                }
                Buscar?.Invoke(this, CargarFiltro());
            }
            else
            {
                var cuerposReceptoresOtrasNormas = cuerposReceptores.Where(x => x.Norma != "2021").ToList();
                ddlCuerpoReceptor.LoadDropSinSel("Nombre", "CuerpoReceptorID", cuerposReceptoresOtrasNormas);
                Buscar?.Invoke(this, CargarFiltro());
            }
        }
        #endregion MetodosFunciones
    }
}