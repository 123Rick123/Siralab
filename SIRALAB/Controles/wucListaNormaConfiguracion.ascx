<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucListaNormaConfiguracion.ascx.cs" Inherits="SIRALAB.Controles.wucListaNormaConfiguracion" ClientIDMode="AutoID" %>

<link href="../style/Master.css" rel="stylesheet" type="text/css" />
<link href="../style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="../style/titulo.css" rel="stylesheet" type="text/css" />
<link href="../style/iconos.css" rel="stylesheet" type="text/css" />
<link href="../style/botones.css" rel="stylesheet" />
<style>
    .text-aligment { text-align: justify; font-size: 8pt; }
</style>

<asp:Repeater ID="rptrNormaConfiguracion" runat="server" ClientIDMode="AutoID" ItemType="SIRALAB.Controles.NormaConfiguracionItem" OnItemDataBound="rptrNormaConfiguracion_ItemDataBound">
    <HeaderTemplate>
        <table id="tablaListaNormaConfiguracion" class="table table-bordered" style="font-size: 7pt;">
            <caption><asp:Label ID="lblTitulo" runat="server" Text="Muestra"></asp:Label></caption>
            <tr>
                <th style="width: 40px;">Orden</th>
                <th>Parámetro</th>
                <th style="width: 40px;">Exenta</th>
                <th>Grupo</th>
                <th>Límite</th>
                <th>Límite (LFD)</th>
                <th style="width: 50px;"></th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:LinkButton ID="lnkbSubir" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="icono-subir" OnClick="lnkbSubir_Click"></asp:LinkButton>
                <asp:LinkButton ID="lnkbBajar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="icono-bajar" OnClick="lnkbBajar_Click"></asp:LinkButton>
            </td>
            <td><%#: Item.NombreParametro %></td>
            <td style="text-align: center;"><%#: Item.Exenta ? "Si" : "No" %></td>
            <td style="text-align: center;"><%#: Item.NombreGrupo %></td>
            <td style="text-align: center;"><%#: Item.Limite %></td>
            <td style="text-align: center;"><%#: Item.LimiteLFD %></td>
            <td style="text-align: center;">
                <asp:LinkButton ID="lnkbEditar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="icono-editar-16" OnClick="lnkbEditar_Click" ToolTip="Click para editar."></asp:LinkButton>
                <asp:LinkButton ID="lnkbEliminar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="icono-cerrarrojo-16" OnClick="lnkbEliminar_Click" ToolTip="Click para eliminar."></asp:LinkButton>
                <asp:Label ID="lblbInfo" runat="server" ToolTip="Parámetro no editable" CssClass="icono-info-16"></asp:Label>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>