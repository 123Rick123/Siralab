using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB.FIEL;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_DAL.Extensiones;

namespace SIRALAB.Controles
{
    public partial class wucListaOrdenTrabajo : System.Web.UI.UserControl
    {
        public static Usuario Login
        {
            get { return (Usuario)HttpContext.Current.Session["UsuarioLog"]; }
        }
        [Browsable(true)]
        public event EventHandler<DatosFirma_Args> FirmarAnalisis;
        public int OrdenTrabajoID
        {
            get { return int.Parse(hfOrdenTrabajo.Value == "" ? "0" : hfOrdenTrabajo.Value); }
            set { hfOrdenTrabajo.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(List<OrdenTrabajoLista> ordenes)
        {
            rptOrdenTrabajo.DataSource = ordenes;
            rptOrdenTrabajo.DataBind();
            upOrdenTrabajo.Update();
        }

        protected void lnkbtnSeleccionar_Click(object sender, EventArgs e)
        {
            var ordenTrabajoId = int.Parse(((LinkButton)sender).CommandArgument);
            Session["OrdenTrabajoID_"] = ordenTrabajoId;
            Response.Redirect("~/OrdenTrabajo.aspx");
        }

        protected void rptOrdenTrabajo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
                if (Login.CatUsuarioRol.UsuarioRolID != 2)
                    e.Item.FindControl("thChkTodos").Visible = true;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var rowData = (OrdenTrabajoLista)e.Item.DataItem;
                var check = (CheckBox)e.Item.FindControl("chkSeleccionar");
                var tr = e.Item.FindControl("trAnalisis");
                var repeater = (Repeater)e.Item.FindControl("rptAnalisis");

                if (Login.CatUsuarioRol.UsuarioRolID != 2)
                    e.Item.FindControl("tdChkTodos").Visible = true;


                if (check != null) //Aquí bloquear si se envío a firmar.
                {
                    if (rowData.Analisis != null && rowData.Analisis.Count > 0)
                        if (rowData.Analisis.Any(p => p.Firmado_ && p.AnalisisPadreID == null && p.AnalisisEncolado_ != true))
                            check.Visible = false; // No puede ser eliminada debido a que la orden contiene un análisis firmado
                }

                if (tr != null)
                {
                    if (rowData.Analisis != null && rowData.Analisis.Count > 0)
                    {
                        repeater.ItemDataBound += Repeater_ItemDataBound;
                        tr.Visible = true;
                        repeater.DataSource = rowData.Analisis;
                        repeater.DataBind();
                    }
                }
            }
        }

        private void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Login.CatUsuarioRol.UsuarioRolID != 2)
                {
                    var headerRow = e.Item.FindControl("thChkATodos");
                    var itemRow = e.Item.FindControl("tdChkATodos");
                    if (headerRow != null)
                        headerRow.Visible = true;
                    if (itemRow != null)
                        itemRow.Visible = true;
                }
            }
        }

        public List<string> ObtenerOrdenesAEliminar()
        {
            List<string> ordenesDeTrabajo = new System.Collections.Generic.List<string>();
            foreach (RepeaterItem item in rptOrdenTrabajo.Items)
            {
                switch (item.ItemType)
                {
                    case ListItemType.Item:
                    case ListItemType.AlternatingItem:
                        var chk = (CheckBox)item.FindControl("chkSeleccionar");
                        if (chk != null)
                        {
                            if (chk.Checked)
                                ordenesDeTrabajo.Add(chk.Attributes["data-id"]);
                        }
                        break;
                }
            }

            return ordenesDeTrabajo;
        }

        public DatosFirma_Args ObtenerDatosAFirmar()
        {
            DatosFirma_Args resultado = new DatosFirma_Args();
            resultado.DatosAFirmar = new FIEL.FirmaElectronica();

            foreach (RepeaterItem dato in rptOrdenTrabajo.Items)
            {
                var rptAnalisis = (Repeater)dato.FindControl("rptAnalisis");
                foreach (RepeaterItem item in rptAnalisis.Items)
                {
                    var chk = (CheckBox)item.FindControl("chkAnalisis");
                    var analisisID = Convert.ToInt32(chk.Attributes["data-id"]);

                    SIRALAB_DAL.Analisi analisisDal = new Analisi();
                    Analisis_BL analisisBL = new Analisis_BL();
                    analisisDal = analisisBL.Obten_Analisis(analisisID);

                    if (analisisDal != null)
                    {
                        var chkAnalisisFirmado = ((CheckBox)item.FindControl("chkAnalisis"));

                        if (chkAnalisisFirmado != null && chkAnalisisFirmado.Visible)
                        {
                            bool seleccionado = chkAnalisisFirmado.Checked;
                            bool noGuardado = chkAnalisisFirmado.Enabled;


                            if (seleccionado && noGuardado)
                            {
                                string Llave = analisisDal.AnalisisID.ToString();
                                FIEL.CadenaOriginal cadenaOriginal = new CadenaOriginal();

                                cadenaOriginal.Datos.Add("RFC Laboratorio: " + analisisDal.Laboratorio.RFC);
                                cadenaOriginal.Datos.Add("Nombre Laboratorio: " + analisisDal.Laboratorio.Nombre);
                                cadenaOriginal.Datos.Add("RFC Contribuyente: " + analisisDal.OrdenTrabajo.Cliente.RFC);
                                cadenaOriginal.Datos.Add("Nombre Contribuyente: " +
                                                         analisisDal.OrdenTrabajo.Cliente.Nombre);
                                cadenaOriginal.Datos.Add("Fecha Análisis: " +
                                                         analisisDal.OrdenTrabajo.FechaEmision.ToShortDateString());
                                cadenaOriginal.Datos.Add("Título de concesión: " + analisisDal.Consecion.Titulo);
                                cadenaOriginal.Datos.Add("Número de anexo: " + analisisDal.Anexo.Anexo1);
                                cadenaOriginal.Datos.Add("Nombre del punto de descarga: " +
                                                         analisisDal.Anexo.NombreCuerpoReceptor);
                                /*cadenaOriginal.Datos.Add("Periodo: " + analisisDal.OrdenTrabajo.FrecuenciaReporte.Nombre);*/
                                cadenaOriginal.Datos.Add("Fecha de presentación del informe: " +
                                                         DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                cadenaOriginal.Datos.Add("Número de Informe: " +
                                                         analisisDal.OrdenTrabajo.NumeroInforme);
                                cadenaOriginal.Datos.Add("Norma: " + analisisDal.OrdenTrabajo.Norma.Nombre);
                                cadenaOriginal.Datos.Add("Frecuencia de muestreo: " +
                                                         analisisDal.OrdenTrabajo.FrecuenciaMuestreo.Nombre);
                                /*cadenaOriginal.Datos.Add("Resultado: " + analisisDal.Resultado());*/

                                resultado.DatosAFirmar.AddCadena(Llave, cadenaOriginal.CadenaConcatenada);
                            }
                        }
                    }
                }
            }
            return resultado;
        }

        public void Firmar()
        {
            if (FirmarAnalisis != null)
            {
                FirmarAnalisis(this, ObtenerDatosAFirmar());
            }
        }

        public void CargaInformacion(int OrdenTrabajoID)
        {
            this.OrdenTrabajoID = OrdenTrabajoID;

            Analisis_BL analisis_bl = new Analisis_BL();
            List<Analisi> ListAnalisis = new List<Analisi>();
            List<AnalisisLista> Analisis_rp = new List<AnalisisLista>();

            //Validamos si se trata de perfil de recaudación y fiscalizacion
            if (Login.UsuarioRolID == 2)
            {
                ListAnalisis = analisis_bl.Lista_Analisis(OrdenTrabajoID).Where(x => x.Firmada == true).OrderBy(x => x.AnalisisID).ToList();
            }
            else
            {
                ListAnalisis = analisis_bl.Lista_Analisis(OrdenTrabajoID);
            }

            int Consecutivo = 1;

            foreach (var i in ListAnalisis)
            {
                Muestra FechaConformacion = null;
                FechaConformacion = i.Muestras.Where(D => D.NivelID == 2).FirstOrDefault();
                string FechaAnalisis = string.Empty;
                DateTime ValidDatetime = new DateTime();
                if (FechaConformacion != null && DateTime.TryParse(FechaConformacion.Fecha.ToString(), out ValidDatetime))
                {
                    FechaAnalisis = DateTime.Parse(FechaConformacion.Fecha.ToString()).ToShortDateString();
                }
                Analisis_rp.Add(new AnalisisLista
                {
                    Firmado_ = i.Firmada,
                    FirmaVisible_ = i.VerificaBanderaMuestraCompuesta(),
                    AnaisisID_ = i.AnalisisID,
                    AnaisisPadreID_ = i.AnalisisPadreID,
                    EstatusID_ = i.EstatusID,
                    FechaRegistro_ = FechaAnalisis,
                    MuestrasSimples_ = i.OrdenTrabajo.FrecuenciaMuestreo.MuestrasSimples,
                    Enviado_ = i.Firmada == true ? "~/image/botones/Enviado.png" : "~/image/botones/NoEnviado.png",
                    EnviadoBool_ = i.Firmada,
                    Consecutivo_ = (i.OrdenTrabajo.Norma.RequiereCaudal ? "PD" : "Puntual") + Consecutivo++

                });
            }

            upOrdenTrabajo.Visible = Analisis_rp.Count > 0;
        }

    }
}