    <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucParametros.ascx.cs" Inherits="SIRALAB.Controles.wucParametros" %>

<%--<link href="style/Master.css" rel="stylesheet" type="text/css" />
<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<link href="style/botones.css" rel="stylesheet" />--%>
<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cargan);
    function cargan() {
        //function valH(e, field) {
        //    //ok grados centigrados
        //    //debugger;
        //    var regexp;
        //    var key = window.event ? window.event.keyCode : e.which;
        //    // backspace

        //    if (key == 8) return true;

        //    // 0-9 a partir del .decimal  

        //    if (field.value != "") {
        //        if ((field.value.indexOf(":")) > 0) {
        //            //si tiene un punto valida dos digitos en la parte decimal

        //            if (key > 47 && key < 58) {
        //                if (field.value == "") return true;
        //                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

        //                regexp = /[0-9]{2}$/;
        //                return !(regexp.test(field.value));
        //            }
        //        }
        //    }
        //    // 0-9 

        //    if (key > 47 && key < 58) {
        //        if (field.value == "") return true;
        //        regexp = /[0-9]{2}/;
        //        return !(regexp.test(field.value));
        //    }
        //    // .

        //    if (key == 58) {
        //        if (field.value == "")
        //            return false;
        //        regexp = /^[0-9]+$/;
        //        return regexp.test(field.value);
        //    }
        //    // other key

        //    return /\d/.test(String.fromCharCode(key));
        //}
        //function keyPress(e) {

        //    var keynum = window.event ? window.event.keyCode : e.which;
        //    if ((keynum == 8) || (keynum == 46))
        //        return true;

        //    return /\d/.test(String.fromCharCode(keynum));

        //}
        //function valNGA(e, field) {//ok grados centigrados
        //    var regexp;
        //    var key = window.event ? window.event.keyCode : e.which;
        //    // backspace

        //    if (key == 8) return true;

        //    // 0-9 a partir del .decimal  

        //    if (field.value != "") {
        //        if ((field.value.indexOf(".")) > 0) {
        //            //si tiene un punto valida dos digitos en la parte decimal

        //            if (key > 47 && key < 58) {
        //                if (field.value == "") return true;
        //                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

        //                regexp = /[0-9]{4}$/;
        //                return !(regexp.test(field.value));
        //            }
        //        }
        //    }
        //    // 0-9 

        //    if (key > 47 && key < 58) {
        //        if (field.value == "") return true;
        //        regexp = /[0-9]{10}/;
        //        return !(regexp.test(field.value));
        //    }
        //    // .

        //    if (key == 46) {
        //        if (field.value == "")
        //            return false;
        //        regexp = /^[0-9]+$/;
        //        return regexp.test(field.value);
        //    }
        //    // other key

        //    return /\d/.test(String.fromCharCode(key));
        //}
        //function valNCF(e, field) {//ok grados centigrados
        //    var regexp;
        //    var key = window.event ? window.event.keyCode : e.which;
        //    // backspace

        //    if (key == 8) return true;

        //    // 0-9 a partir del .decimal  

        //    if (field.value != "") {
        //        if ((field.value.indexOf(".")) > 0) {
        //            //si tiene un punto valida dos digitos en la parte decimal

        //            if (key > 47 && key < 58) {
        //                if (field.value == "") return true;
        //                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

        //                regexp = /[0-9]{0}$/;
        //                return !(regexp.test(field.value));
        //            }
        //        }
        //    }
        //    // 0-9 

        //    if (key > 47 && key < 58) {
        //        if (field.value == "") return true;
        //        regexp = /[0-9]{10}/;
        //        return !(regexp.test(field.value));
        //    }
        //    // .

        //    if (key == 46) {
        //        //if (field.value == "")
        //        return false;
        //        //regexp = /^[0-9]+$/;
        //        //return regexp.test(field.value);
        //    }
        //    // other key

        //    return /\d/.test(String.fromCharCode(key));
        //}
        //function valNC(e, field) {
        //    //ok grados centigrados
        //    var regexp;
        //    var key = window.event ? window.event.keyCode : e.which;
        //    // backspace

        //    if (key == 8) return true;

        //    // 0-9 a partir del .decimal  

        //    if (field.value != "") {
        //        if ((field.value.indexOf(".")) > 0) {
        //            //si tiene un punto valida dos digitos en la parte decimal

        //            if (key > 47 && key < 58) {
        //                if (field.value == "") return true;
        //                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

        //                regexp = /[0-9]{1}$/;
        //                return !(regexp.test(field.value));
        //            }
        //        }
        //    }
        //    // 0-9 

        //    if (key > 47 && key < 58) {
        //        if (field.value == "") return true;
        //        regexp = /[0-9]{3}/;
        //        return !(regexp.test(field.value));
        //    }
        //    // .

        //    if (key == 46) {
        //        if (field.value == "") return false;
        //        regexp = /^[0-9]+$/;
        //        return regexp.test(field.value);
        //    }
        //    // other key

        //    return /\d/.test(String.fromCharCode(key));
        //}
        //function valN(e, field) {
        //    //debugger;
        //    var regexp;
        //    var key = window.event ? window.event.keyCode : e.which;
        //    // backspace

        //    if (key == 8) return true;

        //    // 0-9 a partir del .decimal  

        //    if (field.value != "") {
        //        if ((field.value.indexOf(".")) > 0) {
        //            //si tiene un punto valida dos digitos en la parte decimal

        //            if (key > 47 && key < 58) {
        //                if (field.value == "") return true;
        //                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

        //                regexp = /[0-9]{2}$/;
        //                return !(regexp.test(field.value));
        //            }
        //        }
        //    }
        //    // 0-9 

        //    if (key > 47 && key < 58) {
        //        if (field.value == "") return true;
        //        regexp = /[0-9]{10}/;
        //        return !(regexp.test(field.value));
        //    }
        //    // .

        //    if (key == 46) {
        //        if (field.value == "") return false;
        //        regexp = /^[0-9]+$/;
        //        return regexp.test(field.value);
        //    }
        //    // other key

        //    return /\d/.test(String.fromCharCode(key));

        //}
    }
</script>
<%--Inicio.- Repeater correspondiente a las muestraValor que arroja el sistema si se cumplen con las condiciones--%>
<asp:Panel ID="pnlRepeater" runat="server">
    <asp:GridView ID="gvValores" runat="server" AutoGenerateColumns="false"
        ItemType="SIRALAB_BL.common.MuestraValorArgBase_BL" CssClass="table table-responsive table-condensed table-striped text-center viewGrid"
        Caption=" " OnRowDataBound="gvValores_RowDataBound" GridLines="None" DataKeyNames="MuestraValorID" RowStyle-VerticalAlign="Middle">
        <Columns>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <HeaderTemplate>Parámetro</HeaderTemplate>
                <ItemTemplate><span runat="server" data-etiquetaid="<%# Item.ParametroID %>"> <%#:Item.Etiqueta ?? Item.Parametro.Nombre %></span></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>Límites</HeaderTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <asp:Panel ID="pnlLimiteInferior" runat="server">
                                    <table class="table table-bordered">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtLimiteInferior" runat="server" CssClass="form-control" Style="text-align: center; width: 150px; vertical-align: middle; font-size: 14px;" data-scale="<%#:Item.EscalaDecimal ?? 4 %>" data-parametroid="<%# Item.ParametroID %>"></asp:TextBox>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px">
                                <asp:Panel ID="pnlLimiteSuperior" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtLimiteSuperior" runat="server" CssClass="form-control" Style="text-align: center; width: 150px; vertical-align: middle; font-size: 14px;" data-scale="<%#:Item.EscalaDecimal ?? 4 %>" data-parametroid="<%# Item.ParametroID %>"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>Resultado</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtResultado" runat="server" CssClass="form-control" data-scale="<%#:Item.EscalaDecimal ?? 4 %>" Style="text-align: center; margin: auto; font-size: 14px; width: 150px" data-parametroid="<%#Item.ParametroID %>" data-truncamiento="<%# Item.Truncamiento ?? false %>"></asp:TextBox>
                    <asp:DropDownList ID="ddlResultado" runat="server" CssClass="control-select" Width="97%"></asp:DropDownList>
                    <asp:Label ID="lblResultado" runat="server" Visible="false" Text="<%#:Item.Valor %>"></asp:Label>
                    <span runat="server" id="spanResultado" class="text-danger" style="display: none"  data-parametroid="<%#Item.ParametroID %>">El resultado no puede ser inferior al LC/MC</span>
                    <span runat="server" ID="spnMinMax" class="text-danger" Visible="<%# Item.AceptaValor && (Item.LimitaCaptura ?? false) && (Item.MinAceptado != null || Item.MaxAceptado != null) %>" hidden="hidden">Valor fuera de los límites permitidos</span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <%--<ItemStyle HorizontalAlign="Center" />--%>
                <HeaderTemplate>LC/MC</HeaderTemplate>
                <ItemTemplate>
                    <div id="divLCM" runat="server">
                        <asp:TextBox ID="txtLC" runat="server" CssClass="form-control" data-scale="<%#:Item.EscalaDecimalLCM ?? Item.EscalaDecimal ?? 9 %>" Style="text-align: center; margin: auto; font-size: 14px; width: 150px" data-parametroid="<%#Item.ParametroID %>"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>Laboratorio</HeaderTemplate>
                <ItemTemplate>
                    <div id="divDdlLaboratorio" runat="server">
                        <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLaboratorio_SelectedIndexChanged" AutoPostBack="true" Style="font-size: 14px; min-width: 150px; max-width: 350px; margin: auto;"></asp:DropDownList>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle />
                <HeaderTemplate>Método</HeaderTemplate>
                <ItemTemplate>
                    <div ID="divDdlMetodo" runat="server">
                        <asp:DropDownList ID="ddlMetodo" runat="server" CssClass="form-control" Style="font-size: 14px; min-width: 150px; max-width: 350px; margin: auto"></asp:DropDownList>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle Width="80px" />
                <HeaderTemplate>Acción</HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtnExentar" runat="server" ToolTip="Exentar Parámetro" CommandArgument="<%#:Item.MuestraValorID %>" OnClick="lnkbtnExentar_Click" CssClass="btn btn-default btn-xs">
                          <span class="glyphicon glyphicon-saved"></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnNoAnalizado" runat="server" ToolTip="Parámetro no analizado" CommandArgument="<%#:Item.MuestraValorID %>" CssClass="btn btn-default btn-xs" OnClick="lnkbtnNoAnalizado_Click">
                        <span class="glyphicon glyphicon-ban-circle"></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnEditar" runat="server" ToolTip="Editar" CommandArgument="<%#:Item.MuestraValorID %>" CausesValidation="false" CssClass="btn btn-default btn-xs" OnClick="lnkbtnEditar_Click">
                         <span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Panel ID="pnl" runat="server" Style="padding: 10px; text-align: right;" Visible="false">
        <asp:LinkButton ID="lnkbtnGuardarPmtos" runat="server" Visible="true" Style="margin-right: 30px;" OnClick="lnkbtnGuardarPmtos_Click" CssClass="btn btn-default btn-md" ToolTip="Guardar todos los registros">
 <span class="glyphicon glyphicon-floppy-saved"></span>
        </asp:LinkButton>
    </asp:Panel>
</asp:Panel>
<%--Fin.- Repeater correspondiente a las muestraValor que arroja el sistema si se cumplen con las condiciones--%>
<div style="display: none;">
    <asp:HiddenField ID="hfOrdenTrabajo" runat="server" />
    <asp:HiddenField ID="hfNivel" runat="server" />
</div>
