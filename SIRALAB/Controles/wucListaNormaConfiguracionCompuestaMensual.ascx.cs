using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using System.ComponentModel;
using AjaxControlToolkit;

namespace SIRALAB.Controles
{
    public partial class wucListaNormaConfiguracionCompuestaMensual : System.Web.UI.UserControl
    {
        #region Eventos del control
        [Browsable(true)]
        public event EventHandler CheckItem;
        [Browsable(true)]
        public event EventHandler EvalItem;
        [Browsable(true)]
        public event EventHandler<AgregarSimplesArgs> Agregar;
        [Browsable(true)]
        public event EventHandler<NormaConfiguracionItemArg> Activar; 
        #endregion

        #region Propiedades
        private int? NormaID
        {
            get { return (int?)ViewState["NormaIDCompuestaMensual"]; }
            set { ViewState["NormaIDCompuestaMensual"] = value; }
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
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void rptrNormaConfiguracion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var di = (NormaConfiguracionItem)e.Item.DataItem;
            if (di != null)
            {
                var ddl = (DropDownList)e.Item.FindControl("ddlOperacion");
                var chk = (CheckBox)e.Item.FindControl("chkIncluir");
                var chkNoEvalua = (CheckBox)e.Item.FindControl("chkNoEvalua");
                var vce = (ValidatorCalloutExtender)e.Item.FindControl("vceOperacion");
                var rfv = (RequiredFieldValidator)e.Item.FindControl("rfvOperacion");
                var lnkbActivar = (LinkButton)e.Item.FindControl("lnkbActivar");

                var ddlTrataDecimales = (DropDownList)e.Item.FindControl("ddlTrataDecimales");

                LoadDrops.instance.OperacionConsolidado(ddl);
                chk.InputAttributes.Add("ItemRow", e.Item.ItemIndex.ToString());
                chk.InputAttributes.Add("AceptaValor", di.AceptaValor ? "1" : "0");
                chk.InputAttributes.Add("ItemNormaConfiguracionID", di.NormaConfiguracionID.ToString());
                chk.InputAttributes.Add("ItemNormaConfiguracionPadreID", di.NormaConfiguracionPadreID.ToString());

                chkNoEvalua.InputAttributes.Add("ItemRow", e.Item.ItemIndex.ToString());
                chkNoEvalua.InputAttributes.Add("AceptaValor", di.AceptaValor ? "1" : "0");
                chkNoEvalua.InputAttributes.Add("ItemNormaConfiguracionID", di.NormaConfiguracionID.ToString());
                chkNoEvalua.InputAttributes.Add("ItemNormaConfiguracionPadreID", di.NormaConfiguracionPadreID.ToString());
                chkNoEvalua.Checked = !di.Evalua;

                vce.PopupPosition = ValidatorCalloutPosition.TopRight;

                EstadoInactivoLFD(e.Item);
                if (di.NormaConfiguracionID != 0)
                {
                    /// Elemento que puede ser agregado
                    chk.Checked = true;
                    chk.Enabled = false;
                    lnkbActivar.Visible = true;

                    if (di.AceptaValor && di.LimiteInferior != null)
                    {
                        ddl.ClearSelection();
                        ddl.Items.FindByValue(di.OperacionID == null ? "0" : di.OperacionID.ToString()).Selected = true;

                        ddlTrataDecimales.ClearSelection();
                        ddlTrataDecimales.Items.FindByValue(di.Truncamiento == null ? "0" : di.Truncamiento == false ? "1" : "2").Selected = true;

                        Estado01(e.Item, di.LFD);
                    }
                    else if (di.AceptaValor && di.LimiteInferior == null)
                    {
                        ddl.ClearSelection();
                        ddl.Items.FindByValue(di.OperacionID == null ? "0" : di.OperacionID.ToString()).Selected = true;

                        ddlTrataDecimales.ClearSelection();
                        ddlTrataDecimales.Items.FindByValue(di.Truncamiento == null ? "0" : di.Truncamiento == false ? "1" : "2").Selected = true;

                        ddlTrataDecimales.SelectedIndex = di.Truncamiento == null ? 0 : di.Truncamiento == false ? 1 : 2;

                        Estado02(e.Item, di.LFD);
                    }
                    else
                    {
                        ddl.ClearSelection();
                        ddl.Items.FindByValue(di.OperacionID == null ? "0" : di.OperacionID.ToString()).Selected = true;

                        ddlTrataDecimales.ClearSelection();
                        ddlTrataDecimales.Items.FindByValue(di.Truncamiento == null ? "0" : di.Truncamiento == false ? "1" : "2").Selected = true; ;

                        Estado03(e.Item, di.LFD);
                    }
                }
                else
                {
                    /// Elemento que ya se encuentra agregado y se puede modificar
                    chk.Checked = false;
                    chk.Enabled = true;
                    lnkbActivar.Visible = false;

                    if (di.AceptaValor && di.LimiteInferior != null)
                    {
                        ddl.ClearSelection();
                        ddlTrataDecimales.ClearSelection();

                        Estado01(e.Item, di.LFD);
                        //Estado04(e.Item, di.LFD);
                    }
                    else if (di.AceptaValor && di.LimiteInferior == null)
                    {
                        ddl.ClearSelection();
                        ddlTrataDecimales.ClearSelection();

                        Estado02(e.Item, di.LFD);
                        //Estado05(e.Item, di.LFD);
                    }
                    else
                    {
                        ddl.ClearSelection();
                        ddl.Enabled = false;
                        rfv.Enabled = false;

                        ddlTrataDecimales.ClearSelection();
                        ddlTrataDecimales.Enabled = false;

                        Estado03(e.Item, di.LFD);
                        //Estado06(e.Item, di.LFD);
                    }
                }

                if (!di.Evalua)
                {
                    EstadoNoEvalua(e.Item, di.LFD);
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                var pnlNoitem = (Panel)e.Item.FindControl("pnlNoItems");
                pnlNoitem.Visible = rptrNormaConfiguracion.Items.Count == 0;
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
                CheckItem(sender, e);
        }
        protected void chkNoEvalua_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var item = int.Parse(chk.InputAttributes["ItemRow"]);
            var AceptaValor = chk.InputAttributes["AceptaValor"] == "1";
            var ItemNormaConfiguracionID = int.Parse(chk.InputAttributes["ItemNormaConfiguracionID"]);
            var ItemNormaConfiguracionPadreID = int.Parse(chk.InputAttributes["ItemNormaConfiguracionPadreID"]);
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var di = ncBL.Obtener(ItemNormaConfiguracionID == 0 ? ItemNormaConfiguracionPadreID : ItemNormaConfiguracionID);
            var Row = rptrNormaConfiguracion.Items[item];

            if (!chk.Checked)
            {
                if (di.AceptaValor && di.LimiteInferior != null)
                {
                    Estado01(Row, di.LFD);
                }
                else if (di.AceptaValor && di.LimiteInferior == null)
                {
                    Estado02(Row, di.LFD);
                }
                else
                {
                    Estado03(Row, di.LFD);
                }
            }
            else
            {
                EstadoNoEvalua(Row, di.LFD);
            }

            if (EvalItem != null)
                EvalItem(sender, e);
        }
        protected void lnkbActivar_Click(object sender, EventArgs e)
        {
            /// Es eliminar el registro del nivel
            var NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            if (Activar != null)
            {
                Activar(sender, new NormaConfiguracionItemArg
                {
                    NormaConfiguracionID = NormaConfiguracionID,
                    Accion = NormaConfiguracionItemArg.enumAccion.Activar
                });
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
                    var chkNoEvalua = (CheckBox)rptrItem.FindControl("chkNoEvalua");
                    var txtLimiteInferior = (TextBox)rptrItem.FindControl("txtLimiteInferior");
                    var txtLimiteSuperior = (TextBox)rptrItem.FindControl("txtLimiteSuperior");
                    var txtLimiteInferiorLFD = (TextBox)rptrItem.FindControl("txtLimiteInferiorLFD");
                    var txtLimiteSuperiorLFD = (TextBox)rptrItem.FindControl("txtLimiteSuperiorLFD");
                    var ddlTrataDecimales = (DropDownList) rptrItem.FindControl("ddlTrataDecimales");

                    if (ddl != null && chk != null)
                    {
                        if (chk.Checked)
                        {
                            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                            /* Estos son los valores de simple que agregaremos a la compuesta con el operador de consolidado */
                            var NormaConfiguracionID = int.Parse(chk.InputAttributes["ItemNormaConfiguracionID"]);
                            var NormaConfiguracionPadreID = int.Parse(chk.InputAttributes["ItemNormaConfiguracionPadreID"]);
                            var NormaConfigCompuesta = ncBL.Obtener(NormaConfiguracionID == 0 ? NormaConfiguracionPadreID : NormaConfiguracionID);
                            var NormaConfigMensual = new NormaConfiguracionArgs
                            {
                                NormaConfiguracionID = NormaConfiguracionID == 0 ? null : (int?)NormaConfigCompuesta.NormaConfiguracionID,
                                NormaConfiguracionPadreID = NormaConfiguracionID == 0 ? (int?)NormaConfigCompuesta.NormaConfiguracionID : NormaConfigCompuesta.NormaConfiguracionPadreID,
                                NormaID = NormaConfigCompuesta.NormaID,
                                ParametroID = NormaConfigCompuesta.ParametroID,
                                CuerpoReceptorID = NormaConfigCompuesta.CuerpoReceptorID,
                                UsoAguaID = NormaConfigCompuesta.UsoAguaID,
                                NivelID = enumNivel.Mensual.ConvertToInt(),
                                OperacionID = ddl.SelectedValue != "0" ? (int?)int.Parse(ddl.SelectedValue) : null,
                                AceptaValor = NormaConfigCompuesta.AceptaValor,
                                Exenta = NormaConfigCompuesta.Exenta,
                                LimiteInferior = NormaConfigCompuesta.AceptaValor && NormaConfigCompuesta.LimiteInferior != null ? chkNoEvalua.Checked ? -1 : (decimal?)decimal.Parse(txtLimiteInferior.Text) : null,
                                LimiteSuperior = NormaConfigCompuesta.AceptaValor ? chkNoEvalua.Checked ? -1 : (decimal?)decimal.Parse(txtLimiteSuperior.Text) : null,
                                LFD = NormaConfigCompuesta.LFD,
                                LimiteInferiorLFD = NormaConfigCompuesta.LFD && NormaConfigCompuesta.AceptaValor && NormaConfigCompuesta.LimiteInferiorLFD != null && txtLimiteInferiorLFD != null ? chkNoEvalua.Checked ? -1 : (decimal?)decimal.Parse(txtLimiteInferiorLFD.Text) : null,
                                LimiteSuperiorLFD = NormaConfigCompuesta.LFD && NormaConfigCompuesta.AceptaValor && txtLimiteSuperiorLFD != null ? chkNoEvalua.Checked ? -1 : (decimal?)decimal.Parse(txtLimiteSuperiorLFD.Text) : null,
                                Orden = NormaConfiguracionID == 0 ? 0 : NormaConfigCompuesta.Orden,
                                Evalua = !chkNoEvalua.Checked,
                                Activo = true,
                                FechaRegistro = DateTime.Now,
                                Grupo = false,
                                EscalaDecimal = NormaConfigCompuesta.EscalaDecimal,
                                Truncamiento = ddlTrataDecimales.SelectedIndex == 0 ? (bool?) null : ddlTrataDecimales.SelectedIndex == 2
                            };
                            

                            asi.Listado.Add(NormaConfigMensual);
                        }
                    }
                }

                Agregar(this, asi);
            }
        }
        #endregion

        #region Métodos
        public void Mostrar(int NormaID, int CuerpoReceptorID, int UsoAguaID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var lista = ncBL.ObtenerNormaConfiguracion(NormaID, CuerpoReceptorID, UsoAguaID, enumNivel.Compuesta.ConvertToInt()).Select(p => new NormaConfiguracionItem
            {
                NormaConfiguracionID = 0,
                NormaConfiguracionPadreID = p.NormaConfiguracionID,
                NormaID = p.NormaID,
                ParametroID = p.ParametroID,
                CuerpoReceptorID = p.CuerpoReceptorID,
                UsoAguaID = p.UsoAguaID,
                NivelID = p.NivelID,
                OperacionID = p.OperacionID,
                Grupo = p.Grupo,
                Orden = p.Orden,
                NombreParametro = p.ParametroNombre,
                NombreGrupo = p.NormaConfiguracionPadreID != null ? p.NormaConfiguracion2.Grupo ? p.NormaConfiguracion2.Parametro.Nombre : "-" : "-",
                AceptaValor = p.AceptaValor,
                Limite = p.AceptaValor ? "" : "Presencia",
                LimiteInferior = p.LimiteInferior != null && p.LimiteInferior != -1 ? p.LimiteInferior : null,
                LimiteSuperior = p.LimiteSuperior != null && p.LimiteSuperior != -1 ? p.LimiteSuperior : null,
                LimiteInferiorLFD = p.LimiteInferiorLFD != null && p.LimiteInferiorLFD != -1 ? p.LimiteInferiorLFD : null,
                LimiteSuperiorLFD = p.LimiteSuperiorLFD != null && p.LimiteSuperiorLFD != -1 ? p.LimiteSuperiorLFD : null,
                LFD = p.LFD,
                Evalua = p.Evalua,
                Exenta = p.Exenta,
                Activo = p.Activo,
                Truncamiento = p.Truncamiento
            });

            var lista2 = ncBL.ObtenerNormaConfiguracion(NormaID, CuerpoReceptorID, UsoAguaID, enumNivel.Mensual.ConvertToInt()).Select(p => new NormaConfiguracionItem
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
                NombreGrupo = p.NormaConfiguracionPadreID != null ? p.NormaConfiguracion2.Grupo ? p.NormaConfiguracion2.Parametro.Nombre : "-" : "-",
                AceptaValor = p.AceptaValor,
                Limite = p.AceptaValor ? "" : "Presencia",
                LimiteInferior = p.LimiteInferior != null && p.LimiteInferior != -1 ? p.LimiteInferior : null,
                LimiteSuperior = p.LimiteSuperior != null && p.LimiteSuperior != -1 ? p.LimiteSuperior : null,
                LimiteInferiorLFD = p.LimiteInferiorLFD != null && p.LimiteInferiorLFD != -1 ? p.LimiteInferiorLFD : null,
                LimiteSuperiorLFD = p.LimiteSuperiorLFD != null && p.LimiteSuperiorLFD != -1 ? p.LimiteSuperiorLFD : null,
                LFD = p.LFD,
                Evalua = p.Evalua,
                Exenta = p.Exenta,
                Activo = p.Activo,
                Truncamiento = p.Truncamiento
            });

            var lst = from p in lista
            join q in lista2 on p.ParametroID equals q.ParametroID into res1
            from r in res1.DefaultIfEmpty() select new NormaConfiguracionItem
            {
                NormaConfiguracionID = r != null ? r.NormaConfiguracionID : p.NormaConfiguracionID,
                NormaConfiguracionPadreID = r != null ? r.NormaConfiguracionPadreID : p.NormaConfiguracionPadreID,
                NormaID = r != null ? r.NormaID : p.NormaID,
                ParametroID = r != null ? r.ParametroID : p.ParametroID,
                CuerpoReceptorID = r != null ? r.CuerpoReceptorID : p.CuerpoReceptorID,
                UsoAguaID = r != null ? r.UsoAguaID : p.UsoAguaID,
                NivelID = r != null ? r.NivelID : p.NivelID,
                OperacionID = r != null ? r.OperacionID : p.OperacionID,
                Grupo = r != null ? r.Grupo : p.Grupo,
                Orden = r != null ? r.Orden : p.Orden,
                NombreParametro = r != null ? r.NombreParametro : p.NombreParametro,
                NombreGrupo = r != null ? r.NombreGrupo : p.NombreGrupo,
                AceptaValor = r != null ? r.AceptaValor : p.AceptaValor,
                Limite = r != null ? r.Limite : p.Limite,
                LimiteInferior = r != null ? r.LimiteInferior : p.LimiteInferior,
                LimiteSuperior = r != null ? r.LimiteSuperior : p.LimiteSuperior,
                LimiteInferiorLFD = r != null ? r.LimiteInferiorLFD : p.LimiteInferiorLFD,
                LimiteSuperiorLFD = r != null ? r.LimiteSuperiorLFD : p.LimiteSuperiorLFD,
                LFD = r != null ? r.LFD : p.LFD,
                Evalua = r != null ? r.Evalua : p.Evalua,
                Exenta = r != null ? r.Exenta : p.Exenta,
                Activo = r != null ? r.Activo : p.Activo,
                Truncamiento = r?.Truncamiento
            };

            rptrNormaConfiguracion.DataSource = lst;
            rptrNormaConfiguracion.DataBind();
        } 
        #endregion

        #region Estados de Controles de fila
        private void Estado01(RepeaterItem Item, bool LFD)
        {
            #region Controles
            var lblPermanente = (Label)Item.FindControl("lblPermanente");
            var pnlLimite = (Panel)Item.FindControl("pnlLimite");
            var pnlLimiteInf = (Panel)Item.FindControl("pnlLimiteInf");
            var pnlLimiteSup = (Panel)Item.FindControl("pnlLimiteSup");
            var txtLimiteInferior = (TextBox)Item.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Item.FindControl("txtLimiteSuperior");
            var revLimiteInferior = (RegularExpressionValidator)Item.FindControl("revLimiteInferior");
            var rfvLimiteInferior = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferior");
            var revLimiteSuperior = (RegularExpressionValidator)Item.FindControl("revLimiteSuperior");
            var rfvLimiteSuperior = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperior");
            var vcerevLimiteInferior = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferior");
            var vcerfvLimiteSuperior = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperior");
            var lblPermanenteLFD = (Label)Item.FindControl("lblPermanenteLFD");
            var pnlLimiteLFD = (Panel)Item.FindControl("pnlLimiteLFD");
            var pnlLimiteInfLFD = (Panel)Item.FindControl("pnlLimiteInfLFD");
            var pnlLimiteSupLFD = (Panel)Item.FindControl("pnlLimiteSupLFD");
            var txtLimiteInferiorLFD = (TextBox)Item.FindControl("txtLimiteInferiorLFD");
            var txtLimiteSuperiorLFD = (TextBox)Item.FindControl("txtLimiteSuperiorLFD");
            var revLimiteInferiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteInferiorLFD");
            var rfvLimiteInferiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferiorLFD");
            var revLimiteSuperiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteSuperiorLFD");
            var rfvLimiteSuperiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperiorLFD");
            var vcerevLimiteInferiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferiorLFD");
            var vcerfvLimiteSuperiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperiorLFD"); 
            #endregion

            lblPermanente.Visible = false;
            pnlLimite.Visible = true;
            pnlLimiteInf.Visible = true;
            pnlLimiteSup.Visible = true;
            revLimiteInferior.Enabled = true;
            rfvLimiteInferior.Enabled = true;
            revLimiteSuperior.Enabled = true;
            rfvLimiteSuperior.Enabled = true;
            vcerevLimiteInferior.PopupPosition = ValidatorCalloutPosition.TopRight;
            vcerfvLimiteSuperior.PopupPosition = ValidatorCalloutPosition.TopRight;
            if (LFD)
            {
                lblPermanenteLFD.Visible = false;
                pnlLimiteLFD.Visible = true;
                pnlLimiteInfLFD.Visible = true;
                pnlLimiteSupLFD.Visible = true;
                revLimiteInferiorLFD.Enabled = true;
                rfvLimiteInferiorLFD.Enabled = true;
                revLimiteSuperiorLFD.Enabled = true;
                rfvLimiteSuperiorLFD.Enabled = true;
                vcerevLimiteInferiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
                vcerfvLimiteSuperiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
            }
        }
        private void Estado02(RepeaterItem Item, bool LFD)
        {
            #region Controles
            var lblPermanente = (Label)Item.FindControl("lblPermanente");
            var pnlLimite = (Panel)Item.FindControl("pnlLimite");
            var pnlLimiteInf = (Panel)Item.FindControl("pnlLimiteInf");
            var pnlLimiteSup = (Panel)Item.FindControl("pnlLimiteSup");
            var txtLimiteInferior = (TextBox)Item.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Item.FindControl("txtLimiteSuperior");
            var revLimiteInferior = (RegularExpressionValidator)Item.FindControl("revLimiteInferior");
            var rfvLimiteInferior = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferior");
            var revLimiteSuperior = (RegularExpressionValidator)Item.FindControl("revLimiteSuperior");
            var rfvLimiteSuperior = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperior");
            var vcerevLimiteInferior = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferior");
            var vcerfvLimiteSuperior = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperior");
            var lblPermanenteLFD = (Label)Item.FindControl("lblPermanenteLFD");
            var pnlLimiteLFD = (Panel)Item.FindControl("pnlLimiteLFD");
            var pnlLimiteInfLFD = (Panel)Item.FindControl("pnlLimiteInfLFD");
            var pnlLimiteSupLFD = (Panel)Item.FindControl("pnlLimiteSupLFD");
            var txtLimiteInferiorLFD = (TextBox)Item.FindControl("txtLimiteInferiorLFD");
            var txtLimiteSuperiorLFD = (TextBox)Item.FindControl("txtLimiteSuperiorLFD");
            var revLimiteInferiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteInferiorLFD");
            var rfvLimiteInferiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferiorLFD");
            var revLimiteSuperiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteSuperiorLFD");
            var rfvLimiteSuperiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperiorLFD");
            var vcerevLimiteInferiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferiorLFD");
            var vcerfvLimiteSuperiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperiorLFD"); 
            #endregion

            lblPermanente.Visible = false;
            pnlLimite.Visible = true;
            pnlLimiteInf.Visible = false;
            pnlLimiteSup.Visible = true;
            revLimiteInferior.Enabled = false;
            rfvLimiteInferior.Enabled = false;
            revLimiteSuperior.Enabled = true;
            rfvLimiteSuperior.Enabled = true;
            vcerevLimiteInferior.PopupPosition = ValidatorCalloutPosition.TopRight;
            vcerfvLimiteSuperior.PopupPosition = ValidatorCalloutPosition.TopRight;

            if (LFD)
            {
                lblPermanenteLFD.Visible = false;
                pnlLimiteLFD.Visible = true;
                pnlLimiteInfLFD.Visible = false;
                pnlLimiteSupLFD.Visible = true;
                revLimiteInferiorLFD.Enabled = false;
                rfvLimiteInferiorLFD.Enabled = false;
                revLimiteSuperiorLFD.Enabled = true;
                rfvLimiteSuperiorLFD.Enabled = true;
                vcerevLimiteInferiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
                vcerfvLimiteSuperiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
            }
        }
        private void Estado03(RepeaterItem Item, bool LFD)
        {
            #region Controles
            var lblPermanente = (Label)Item.FindControl("lblPermanente");
            var pnlLimite = (Panel)Item.FindControl("pnlLimite");
            var pnlLimiteInf = (Panel)Item.FindControl("pnlLimiteInf");
            var pnlLimiteSup = (Panel)Item.FindControl("pnlLimiteSup");
            var txtLimiteInferior = (TextBox)Item.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Item.FindControl("txtLimiteSuperior");
            var revLimiteInferior = (RegularExpressionValidator)Item.FindControl("revLimiteInferior");
            var rfvLimiteInferior = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferior");
            var revLimiteSuperior = (RegularExpressionValidator)Item.FindControl("revLimiteSuperior");
            var rfvLimiteSuperior = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperior");
            var vcerevLimiteInferior = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferior");
            var vcerfvLimiteSuperior = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperior");
            var lblPermanenteLFD = (Label)Item.FindControl("lblPermanenteLFD");
            var pnlLimiteLFD = (Panel)Item.FindControl("pnlLimiteLFD");
            var pnlLimiteInfLFD = (Panel)Item.FindControl("pnlLimiteInfLFD");
            var pnlLimiteSupLFD = (Panel)Item.FindControl("pnlLimiteSupLFD");
            var txtLimiteInferiorLFD = (TextBox)Item.FindControl("txtLimiteInferiorLFD");
            var txtLimiteSuperiorLFD = (TextBox)Item.FindControl("txtLimiteSuperiorLFD");
            var revLimiteInferiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteInferiorLFD");
            var rfvLimiteInferiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferiorLFD");
            var revLimiteSuperiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteSuperiorLFD");
            var rfvLimiteSuperiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperiorLFD");
            var vcerevLimiteInferiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferiorLFD");
            var vcerfvLimiteSuperiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperiorLFD"); 
            #endregion

            lblPermanente.Visible = true;
            lblPermanente.Text = "Presencia";
            pnlLimite.Visible = false;
            pnlLimiteInf.Visible = false;
            pnlLimiteSup.Visible = false;
            revLimiteInferior.Enabled = false;
            rfvLimiteInferior.Enabled = false;
            revLimiteSuperior.Enabled = false;
            rfvLimiteSuperior.Enabled = false;
            vcerevLimiteInferior.PopupPosition = ValidatorCalloutPosition.TopRight;
            vcerfvLimiteSuperior.PopupPosition = ValidatorCalloutPosition.TopRight;

            if (LFD)
            {
                lblPermanenteLFD.Visible = true;
                lblPermanenteLFD.Text = "Presencia";
                pnlLimiteLFD.Visible = false;
                pnlLimiteInfLFD.Visible = false;
                pnlLimiteSupLFD.Visible = false;
                revLimiteInferiorLFD.Enabled = false;
                rfvLimiteInferiorLFD.Enabled = false;
                revLimiteSuperiorLFD.Enabled = false;
                rfvLimiteSuperiorLFD.Enabled = false;
                vcerevLimiteInferiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
                vcerfvLimiteSuperiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
            }
        }
        private void EstadoNoEvalua(RepeaterItem Item, bool LFD)
        {
            #region Controles
            var lblPermanente = (Label)Item.FindControl("lblPermanente");
            var pnlLimite = (Panel)Item.FindControl("pnlLimite");
            var pnlLimiteInf = (Panel)Item.FindControl("pnlLimiteInf");
            var pnlLimiteSup = (Panel)Item.FindControl("pnlLimiteSup");
            var txtLimiteInferior = (TextBox)Item.FindControl("txtLimiteInferior");
            var txtLimiteSuperior = (TextBox)Item.FindControl("txtLimiteSuperior");
            var revLimiteInferior = (RegularExpressionValidator)Item.FindControl("revLimiteInferior");
            var rfvLimiteInferior = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferior");
            var revLimiteSuperior = (RegularExpressionValidator)Item.FindControl("revLimiteSuperior");
            var rfvLimiteSuperior = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperior");
            var vcerevLimiteInferior = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferior");
            var vcerfvLimiteSuperior = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperior");
            var lblPermanenteLFD = (Label)Item.FindControl("lblPermanenteLFD");
            var pnlLimiteLFD = (Panel)Item.FindControl("pnlLimiteLFD");
            var pnlLimiteInfLFD = (Panel)Item.FindControl("pnlLimiteInfLFD");
            var pnlLimiteSupLFD = (Panel)Item.FindControl("pnlLimiteSupLFD");
            var txtLimiteInferiorLFD = (TextBox)Item.FindControl("txtLimiteInferiorLFD");
            var txtLimiteSuperiorLFD = (TextBox)Item.FindControl("txtLimiteSuperiorLFD");
            var revLimiteInferiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteInferiorLFD");
            var rfvLimiteInferiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferiorLFD");
            var revLimiteSuperiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteSuperiorLFD");
            var rfvLimiteSuperiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperiorLFD");
            var vcerevLimiteInferiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferiorLFD");
            var vcerfvLimiteSuperiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperiorLFD"); 
            #endregion

            lblPermanente.Visible = true;
            lblPermanente.Text = "No evalúa";
            pnlLimite.Visible = false;
            pnlLimiteInf.Visible = false;
            pnlLimiteSup.Visible = false;
            revLimiteInferior.Enabled = false;
            rfvLimiteInferior.Enabled = false;
            revLimiteSuperior.Enabled = false;
            rfvLimiteSuperior.Enabled = false;
            vcerevLimiteInferior.PopupPosition = ValidatorCalloutPosition.TopRight;
            vcerfvLimiteSuperior.PopupPosition = ValidatorCalloutPosition.TopRight;

            if (LFD)
            {
                lblPermanenteLFD.Visible = true;
                lblPermanenteLFD.Text = "No evalúa";
                pnlLimiteLFD.Visible = false;
                pnlLimiteInfLFD.Visible = false;
                pnlLimiteSupLFD.Visible = false;
                revLimiteInferiorLFD.Enabled = false;
                rfvLimiteInferiorLFD.Enabled = false;
                revLimiteSuperiorLFD.Enabled = false;
                rfvLimiteSuperiorLFD.Enabled = false;
                vcerevLimiteInferiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
                vcerfvLimiteSuperiorLFD.PopupPosition = ValidatorCalloutPosition.TopRight;
            }
        }
        private void EstadoInactivoLFD(RepeaterItem Item)
        {
            #region Controles
            var lblPermanenteLFD = (Label)Item.FindControl("lblPermanenteLFD");
            var pnlLimiteLFD = (Panel)Item.FindControl("pnlLimiteLFD");
            var pnlLimiteInfLFD = (Panel)Item.FindControl("pnlLimiteInfLFD");
            var pnlLimiteSupLFD = (Panel)Item.FindControl("pnlLimiteSupLFD");
            var txtLimiteInferiorLFD = (TextBox)Item.FindControl("txtLimiteInferiorLFD");
            var txtLimiteSuperiorLFD = (TextBox)Item.FindControl("txtLimiteSuperiorLFD");
            var revLimiteInferiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteInferiorLFD");
            var rfvLimiteInferiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteInferiorLFD");
            var revLimiteSuperiorLFD = (RegularExpressionValidator)Item.FindControl("revLimiteSuperiorLFD");
            var rfvLimiteSuperiorLFD = (RequiredFieldValidator)Item.FindControl("rfvLimiteSuperiorLFD");
            var vcerevLimiteInferiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerevLimiteInferiorLFD");
            var vcerfvLimiteSuperiorLFD = (ValidatorCalloutExtender)Item.FindControl("vcerfvLimiteSuperiorLFD"); 
            #endregion

            lblPermanenteLFD.Visible = false;
            pnlLimiteLFD.Visible = false;
            pnlLimiteInfLFD.Visible = false;
            pnlLimiteSupLFD.Visible = false;
            revLimiteInferiorLFD.Enabled = false;
            rfvLimiteInferiorLFD.Enabled = false;
            revLimiteSuperiorLFD.Enabled = false;
            rfvLimiteSuperiorLFD.Enabled = false;
        }
        #endregion
    }
}