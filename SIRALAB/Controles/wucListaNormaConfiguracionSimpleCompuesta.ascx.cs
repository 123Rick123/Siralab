using AjaxControlToolkit;
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
    public partial class wucListaNormaConfiguracionSimpleCompuesta : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler CheckItem;
        [Browsable(true)]
        public event EventHandler<AgregarSimplesArgs> Agregar;
        private int? NormaID
        {
            get { return (int?)ViewState["NormaID"]; }
            set { ViewState["NormaID"] = value; }
        }
        private List<NormaConfiguracionItem> Listado
        {
            set { Session["Listado_NormaConfiguracionItem"] = value; }
            get { return Session["Listado_NormaConfiguracionItem"] == null ? new List<NormaConfiguracionItem>() : (List<NormaConfiguracionItem>)Session["Listado_NormaConfiguracionItem"]; }
        }
        private IEnumerable<OperacionConsolidado> Operaciones
        {
            set { Session["Listado_Operaciones"] = value; }
            get { return Session["Listado_Operaciones"] == null ? new List<OperacionConsolidado>() : (List<OperacionConsolidado>)Session["Listado_Operaciones"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public void Mostrar(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var lista = ncBL.ObtenerNormaConfiguracion(NormaID, CuerpoReceptorID, UsoAguaID, enumNivel.Simple.ConvertToInt()).Select(p => new NormaConfiguracionItem
            {
                NormaConfiguracionID = p.NormaConfiguracionID,
                NormaConfiguracionPadreID = p.NormaConfiguracionPadreID,
                NormaID = p.NormaID,
                ParametroID = p.ParametroID,
                CuerpoReceptorID = p.CuerpoReceptorID,
                UsoAguaID = p.UsoAguaID,
                NivelID = p.NivelID,
                OperacionID = p.OperacionID,
                Grupo = p.Grupo,
                Orden = p.Orden,
                NombreParametro = p.ParametroNombre,
                NombreGrupo = p.NormaConfiguracionPadreID != null ? p.NormaConfiguracion2.Parametro.Nombre : "-",
                AceptaValor = p.AceptaValor,
                Limite = p.Evalua ? p.AceptaValor ? (p.LimiteInferior == null ? p.LimiteSuperior.ToString() : string.Format("[{0}-{1}]", p.LimiteInferior, p.LimiteSuperior)) : "Presencia" : "No evalúa",
                LimiteLFD = p.Evalua ? p.AceptaValor ? (p.LimiteInferiorLFD == null ? p.LimiteSuperiorLFD.ToString() : string.Format("[{0} - {1}]", p.LimiteInferiorLFD, p.LimiteSuperiorLFD)) : "Presencia" : "No evalúa",
                Exenta = p.Exenta,
                Activo = p.Activo
            });

            var lista2 = ncBL.ObtenerNormaConfiguracion(NormaID, CuerpoReceptorID, UsoAguaID, enumNivel.Compuesta.ConvertToInt()).Select(p => p.NormaConfiguracionPadreID);

            var lst = lista.Where(p => !lista2.Contains(p.NormaConfiguracionID));

            rptrNormaConfiguracion.DataSource = lst;
            rptrNormaConfiguracion.DataBind();
        }

        protected void rptrNormaConfiguracion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var di = (NormaConfiguracionItem)e.Item.DataItem;
            var ddl = (DropDownList)e.Item.FindControl("ddlOperacion");
            var chk = (CheckBox)e.Item.FindControl("chkIncluir");
            var vce = (ValidatorCalloutExtender)e.Item.FindControl("vceOperacion");
            var ddlTrataDecimales = (DropDownList)e.Item.FindControl("ddlTruncamiento");
            if (ddl != null && chk != null && ddlTrataDecimales != null)
            {
                LoadDrops.instance.OperacionConsolidado(ddl);
                chk.InputAttributes.Add("ItemRow", e.Item.ItemIndex.ToString());
                chk.InputAttributes.Add("ItemNormaConfiguracionID", di.NormaConfiguracionID.ToString());
                chk.InputAttributes.Add("AceptaValor", di.AceptaValor ? "1" : "0");

                vce.PopupPosition = ValidatorCalloutPosition.TopRight;

                if (!di.AceptaValor)
                {
                    ddl.ClearSelection();
                    ddl.Enabled = false;
                    ddlTrataDecimales.Enabled = false;
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                var pnlNoitem = (Panel)e.Item.FindControl("pnlNoItems");
                pnlNoitem.Visible = rptrNormaConfiguracion.Items.Count == 0;
            }
        }

        protected void lnkbAgregar_Click(object sender, EventArgs e)
        {
            if (Agregar != null)
            {
                /// Se requiere extraer los datos del repeater y convertirlos a argumento
                /// 
                AgregarSimplesArgs asi = new AgregarSimplesArgs();
                asi.Listado = new List<NormaConfiguracionArgs>();

                foreach (var Item in rptrNormaConfiguracion.Items)
                {
                    var rptrItem = (RepeaterItem)Item;

                    /* Encontramos los registros que tienen seleccionada la casilla de verificación de incluir */
                    var ddl = (DropDownList)rptrItem.FindControl("ddlOperacion");
                    var chk = (CheckBox)rptrItem.FindControl("chkIncluir");
                    var truncamiento = (DropDownList) rptrItem.FindControl("ddlTruncamiento");
                    if (ddl != null && chk != null)
                    {
                        if (chk.Checked)
                        {
                            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                            /* Estos son los valores de simple que agregaremos a la compuesta con el operador de consolidado */
                            var NormaConfiguracionID = int.Parse(chk.InputAttributes["ItemNormaConfiguracionID"]);
                            var NormaConfigSimple = ncBL.Obtener(NormaConfiguracionID);
                            var NormaConfigCompuesta = new NormaConfiguracionArgs
                            {
                                NormaConfiguracionID = null,
                                NormaConfiguracionPadreID = NormaConfiguracionID,
                                NormaID = NormaConfigSimple.NormaID,
                                ParametroID = NormaConfigSimple.ParametroID,
                                CuerpoReceptorID = NormaConfigSimple.CuerpoReceptorID,
                                UsoAguaID = NormaConfigSimple.UsoAguaID,
                                NivelID = enumNivel.Compuesta.ConvertToInt(),
                                OperacionID = ddl.SelectedValue != "0" ? (int?)int.Parse(ddl.SelectedValue) : null,
                                AceptaValor = NormaConfigSimple.AceptaValor,
                                Exenta = NormaConfigSimple.Exenta,
                                LimiteInferior = NormaConfigSimple.LimiteInferior,
                                LimiteSuperior = NormaConfigSimple.LimiteSuperior,
                                LFD = NormaConfigSimple.LFD,
                                LimiteInferiorLFD = NormaConfigSimple.LimiteInferiorLFD,
                                LimiteSuperiorLFD = NormaConfigSimple.LimiteSuperiorLFD,
                                Orden = NormaConfigSimple.Orden,
                                Evalua = NormaConfigSimple.Evalua,
                                Activo = true,
                                FechaRegistro = DateTime.Now,
                                Grupo = false,
                                EscalaDecimal = NormaConfigSimple.EscalaDecimal,
                                EscalaDecimalLCM = NormaConfigSimple.EscalaDecimalLCM,
                                Etiqueta=NormaConfigSimple.Etiqueta,
                                LCMC = NormaConfigSimple.LCMC,
                                Truncamiento = truncamiento.SelectedValue == "2"
                            };

                            asi.Listado.Add(NormaConfigCompuesta);
                        }
                    }
                }

                Agregar(this, asi);
            }
        }

        protected void chkIncluir_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var item = int.Parse(chk.InputAttributes["ItemRow"]);
            var AceptaValor = chk.InputAttributes["AceptaValor"] == "1";
            var rfv = (RequiredFieldValidator)rptrNormaConfiguracion.Items[item].FindControl("rfvOperacion");
            rfv.Enabled = AceptaValor ? chk.Checked : false;

            if (CheckItem != null)
                CheckItem(rptrNormaConfiguracion.Items[item], new EventArgs());
        }
    }

    public class AgregarSimplesArgs : EventArgs
    {
        public List<NormaConfiguracionArgs> Listado { get; set; }
    }
}