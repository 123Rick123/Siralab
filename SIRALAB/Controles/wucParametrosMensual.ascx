<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucParametrosMensual.ascx.cs" Inherits="SIRALAB.Controles.wucParametrosMensual" %>

<link href="../style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="../style/titulo.css" rel="stylesheet" type="text/css" />
<link href="../style/iconos.css" rel="stylesheet" type="text/css" />
<link href="../style/botones.css" rel="stylesheet" />
<%--Inicio.- Repeater correspondiente a las muestraValor que arroja el sistema si se cumplen con las condiciones--%>
<asp:Panel ID="pnlRepeater" runat="server" Width="700px">
    <asp:Repeater ID="rptValores" runat="server" ItemType="SIRALAB_BL.common.MuestraValorArgBase_BL" OnItemDataBound="rptValores_ItemDataBound">
        <HeaderTemplate>
            <table class="table-list">
                <caption>
                    <asp:Label ID="lblTituloRepeater" runat="server"></asp:Label></caption>
                <tr>
                    <th style="width: 160px">Parámetro</th>
                    <th style="width: 60px">Resultado</th>
                    <th style="width: 60px">Unidad</th>
                    <th style="width: 60px">LC/MC</th>
                    <th style="width: 100px">Laboratorio</th>
                    <th style="width: 60px">Método</th>
                    <th style="width: 90px">Exentar</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 160px">
                    <%#:Item.Parametro.Nombre%>
                    <asp:LinkButton ID="lnkbtnMuestraValor" runat="server" CommandArgument='<%#:Item.MuestraValorID%>' Style="display: none"></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnParametroID" runat="server" CommandArgument='<%#:Item.Parametro.ParametroID%>' Style="display: none"></asp:LinkButton>
                </td>
                <td>
                    <asp:TextBox ID="txtResultado" runat="server" CssClass="control-campo" Style="width: 80px;"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rev_txtResultado" runat="server" ControlToValidate="txtResultado" ValidationGroup="parametros" ValidationExpression="^[0-9]{1,5}(\.[0-9]{0,5})?$" ErrorMessage="Número decimal fuera del rango establecido" Display="None" />
                    <asp:ValidatorCalloutExtender ID="vce_rev_txtResultado" runat="server" TargetControlID="rev_txtResultado" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                    <asp:DropDownList ID="ddlResultado" runat="server" CssClass="control-campo" Style="width: 93px;"></asp:DropDownList>
                    <asp:Label ID="lblResultado" runat="server" Visible="false" Text='<%#:Item.Valor%>' Style="width: 85px;"></asp:Label>
                </td>
                <td style="text-align: center">
                    <%#:Item.Parametro.CatUnidadMedida.Nombre %>
                </td>
                <td>
                    <asp:TextBox ID="txtLC" runat="server" CssClass="control-campo" Style="width: 65px"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="control-campo" Style="width: 100px" OnSelectedIndexChanged="ddlLaboratorio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <%--<asp:RequiredFieldValidator ID="rfv_ddlLaboratorio" runat="server" ValidationGroup="parametros" InitialValue="0" ControlToValidate="ddlLaboratorio" ErrorMessage="Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_ddlLaboratorio" runat="server" TargetControlID="rfv_ddlLaboratorio"></asp:ValidatorCalloutExtender>--%>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMetodo" runat="server" CssClass="control-campo" Style="width: 75px"></asp:DropDownList>
                    <%--<asp:RequiredFieldValidator ID="rfv_ddlMetodo" runat="server" ValidationGroup="parametros" InitialValue="0" ControlToValidate="ddlMetodo" ErrorMessage="Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_ddlMetodo" runat="server" TargetControlID="rfv_ddlMetodo"></asp:ValidatorCalloutExtender>--%>
                </td>
                <td style="text-align: center">
                    <asp:LinkButton ID="lnkbtnExentar" runat="server" ToolTip="Exentar Parámetro" CommandArgument='<%#:Item.MuestraValorID%>' OnClick="lnkbtnExentar_Click" CssClass="icono-certificado"></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnNoAnalizado" runat="server" ToolTip="Parámetro no analizado" CommandArgument='<%#:Item.MuestraValorID%>' CssClass="icono-noanalizado" OnClick="lnkbtnNoAnalizado_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnEditar" runat="server" ToolTip="Editar" CommandArgument='<%#:Item.MuestraValorID%>' CausesValidation="false" CssClass="icono-editarParametro" OnClick="lnkbtnEditar_Click"></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td style="width: 160px; background-color: #F1F1F1;">
                    <%#:Item.Parametro.Nombre%>
                    <asp:LinkButton ID="lnkbtnMuestraValor" runat="server" CommandArgument='<%#:Item.MuestraValorID%>' Style="display: none"></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnParametroID" runat="server" CommandArgument='<%#:Item.Parametro.ParametroID%>' Style="display: none"></asp:LinkButton>
                </td>
                <td style="background-color: #F2F2F2;">
                    <asp:TextBox ID="txtResultado" runat="server" CssClass="control-campo" Style="width: 80px;"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rev_txtResultado" runat="server" ControlToValidate="txtResultado" ValidationExpression="^[0-9]{1,5}(\.[0-9]{0,5})?$" ErrorMessage="Número decimal fuera del rango establecido" Display="None" />
                    <asp:ValidatorCalloutExtender ID="vce_rev_txtResultado" runat="server" TargetControlID="rev_txtResultado" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                    <asp:DropDownList ID="ddlResultado" runat="server" CssClass="control-campo" Style="width: 93px;"></asp:DropDownList>
                    <asp:Label ID="lblResultado" runat="server" Visible="false" Text='<%#:Item.Valor%>' Style="width: 85px;"></asp:Label>
                </td>
                <td style="text-align: center; background-color: #F1F1F1;">
                    <%#:Item.Parametro.CatUnidadMedida.Nombre %>
                </td>
                <td style="text-align: center; background-color: #F1F1F1;">
                    <asp:TextBox ID="txtLC" runat="server" CssClass="control-campo" Style="width: 65px"></asp:TextBox>
                </td>
                <td style="text-align: center; background-color: #F1F1F1;">
                    <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="control-campo" Style="width: 100px" OnSelectedIndexChanged="ddlLaboratorio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <%--<asp:RequiredFieldValidator ID="rfv_ddlLaboratorio" runat="server" ValidationGroup="parametros" InitialValue="0" ControlToValidate="ddlLaboratorio" ErrorMessage="Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_ddlLaboratorio" runat="server" TargetControlID="rfv_ddlLaboratorio"></asp:ValidatorCalloutExtender>--%>
                </td>
                <td style="text-align: center; background-color: #F1F1F1;">
                    <asp:DropDownList ID="ddlMetodo" runat="server" CssClass="control-campo" Style="width: 75px"></asp:DropDownList>
                    <%--<asp:RequiredFieldValidator ID="rfv_ddlMetodo" runat="server" ValidationGroup="parametros" InitialValue="0" ControlToValidate="ddlMetodo" ErrorMessage="Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_ddlMetodo" runat="server" TargetControlID="rfv_ddlMetodo"></asp:ValidatorCalloutExtender>--%>
                </td>
                <td style="text-align: center; background-color: #F1F1F1;">
                    <asp:LinkButton ID="lnkbtnExentar" runat="server" ToolTip="Exentar Parámetro" CommandArgument='<%#:Item.MuestraValorID%>' OnClick="lnkbtnExentar_Click" CssClass="icono-certificado"></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnNoAnalizado" runat="server" ToolTip="Parámetro no analizado" CommandArgument='<%#:Item.MuestraValorID%>' CssClass="icono-noanalizado" OnClick="lnkbtnNoAnalizado_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnEditar" runat="server" ToolTip="Editar" CommandArgument='<%#:Item.MuestraValorID%>' CausesValidation="false" CssClass="icono-editarParametro" OnClick="lnkbtnEditar_Click"></asp:LinkButton>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
                     <asp:Panel ID="pnl" runat="server">
                         <div style="margin-top: 10px;">
                             <asp:LinkButton ID="lnkbtnGuardarPmtos" runat="server" Visible="true" Style="margin-left: 650px;" OnClick="lnkbtnGuardarPmtos_Click" ValidationGroup="parametros" CssClass="icono-circulosave" ToolTip="Guardar todos los registros"></asp:LinkButton>
                             <%----%>
                         </div>
                     </asp:Panel>
        </FooterTemplate>
    </asp:Repeater>
</asp:Panel>

<%--Fin.- Repeater correspondiente a las muestraValor que arroja el sistema si se cumplen con las condiciones--%>
<asp:HiddenField ID="hfOrdenTrabajo" runat="server" />
<asp:HiddenField ID="hfNivel" runat="server" />
