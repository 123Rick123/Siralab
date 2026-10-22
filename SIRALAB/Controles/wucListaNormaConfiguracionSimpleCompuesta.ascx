<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucListaNormaConfiguracionSimpleCompuesta.ascx.cs" Inherits="SIRALAB.Controles.wucListaNormaConfiguracionSimpleCompuesta" ClientIDMode="AutoID" %>


<asp:Repeater ID="rptrNormaConfiguracion" runat="server" ClientIDMode="AutoID" ItemType="SIRALAB.Controles.NormaConfiguracionItem" OnItemDataBound="rptrNormaConfiguracion_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-bordered" style="font-size: 7pt;">
            <caption>
                <asp:Label ID="lblTitulo" runat="server" Text="Parámetros de muestra simple"></asp:Label>
                <asp:LinkButton ID="lnkbAgregar" runat="server" CssClass="icono-guardar-16" ToolTip="Agregar a compuesta" OnClick="lnkbAgregar_Click" ValidationGroup="vgListaCompuesta" style="float: right;"></asp:LinkButton>
            </caption>
            <tr>
                <th>Parámetro</th>
                <th style="width: 120px;">Límite</th>
                <th style="width: 120px;">Límite LFD</th>
                <th style="width: 70px;">Incluir</th>
                <th style="width: 120px;">Operación</th>
                <th  style="width: 120px;">
                    Tratamiento decimales
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#: Item.NombreParametro %></td>
            <td style="text-align: center;"><%#: Item.Limite %></td>
            <td style="text-align: center;"><%#: Item.LimiteLFD %></td>
            <td style="text-align: center;"><asp:CheckBox ID="chkIncluir" runat="server" AutoPostBack="true" OnCheckedChanged="chkIncluir_CheckedChanged" /></td>
            <td style="text-align: center; font-size: 8pt;">
                <asp:DropDownList ID="ddlOperacion" runat="server" Width="97%" CssClass="control-select"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvOperacion" runat="server" ControlToValidate="ddlOperacion" Enabled="false" InitialValue="0" ValidationGroup="vgListaCompuesta" ErrorMessage="Debe seleccionar una operación de consolidado." Display="None"></asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="vceOperacion" runat="server" TargetControlID="rfvOperacion"></asp:ValidatorCalloutExtender>
            </td>
            <td style="text-align: center; font-size: 8pt;">
                <asp:DropDownList runat="server" ID="ddlTruncamiento" Width="97%" CssClass="control-select">
                    <asp:ListItem Value="0" Text="Seleccione"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Redondeo"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Truncamiento"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr>
            <td><%#: Item.NombreParametro %></td>
            <td style="text-align: center;"><%#: Item.Limite %></td>
            <td style="text-align: center;"><%#: Item.LimiteLFD %></td>
            <td style="text-align: center;"><asp:CheckBox ID="chkIncluir" runat="server" AutoPostBack="true" OnCheckedChanged="chkIncluir_CheckedChanged" /></td>
            <td style="text-align: center;">
                <asp:DropDownList ID="ddlOperacion" runat="server" Width="97%" CssClass="control-select"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvOperacion" runat="server" ControlToValidate="ddlOperacion" Enabled="false" InitialValue="0" ValidationGroup="vgListaCompuesta" ErrorMessage="Debe seleccionar una operación de consolidado." Display="None"></asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="vceOperacion" runat="server" TargetControlID="rfvOperacion"></asp:ValidatorCalloutExtender>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlTruncamiento" Width="97%" CssClass="control-select">
                    <asp:ListItem Value="0" Text="Seleccione"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Redondeo"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Truncamiento"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
        <asp:Panel ID="pnlNoItems" runat="server" Visible="false" style="text-align: center; margin: 20px auto; color: #777; font-size: 12pt;">No hay elementos en el listado.</asp:Panel>
    </FooterTemplate>
</asp:Repeater>
