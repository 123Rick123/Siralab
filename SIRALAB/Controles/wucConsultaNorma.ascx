<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaNorma.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaNorma" %>

<div>
    <asp:Repeater ID="rptrNormas" runat="server" ItemType="SIRALAB.Controles.NormaItem">
        <HeaderTemplate>
            <table class="table table-striped table-condensed">
                <tr>
                    <th>Nombre</th>
                    <th style="width: 60px">Caudal</th>
                    <th style="width: 60px">Compuesta</th>
                    <th style="width: 60px">Mensual</th>
                    <th style="width: 60px">Fuente Abastecimiento</th>
                    <th style="width: 60px">Activo</th>
                    <th style="width: 60px">Acciones</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#: Item.Norma %></td>
                <td style="text-align: center;"><%#: Item.Caudal %></td>
                <td style="text-align: center;"><%#: Item.Compuesta %></td>
                <td style="text-align: center;"><%#: Item.Mensual %></td>
                <td style="text-align: center;"><%#: Item.FuenteAbastecimiento %></td>
                <td style="text-align: center;"><%#: Item.Activo %></td>
                <td style="text-align: center;">
                    <asp:LinkButton ID="lnkbEditar" runat="server" CommandArgument="<%#: Item.NormaID %>" CssClass="btn btn-default btn-xs" OnClick="lnkbEditar_Click"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td style="background-color: #F2F2F2;"><%#: Item.Norma %></td>
                <td style="background-color: #F2F2F2; text-align: center;"><%#: Item.Caudal %></td>
                <td style="background-color: #F2F2F2; text-align: center;"><%#: Item.Compuesta %></td>
                <td style="background-color: #F2F2F2; text-align: center;"><%#: Item.Mensual %></td>
                <td style="background-color: #F2F2F2; text-align: center;"><%#: Item.FuenteAbastecimiento %></td>
                <td style="background-color: #F2F2F2; text-align: center;"><%#: Item.Activo %></td>
                <td style="background-color: #F2F2F2; text-align: center;">
                    <asp:LinkButton ID="lnkbEditar" runat="server" CommandArgument="<%#: Item.NormaID %>" CssClass="btn btn-default btn-xs" OnClick="lnkbEditar_Click"> <span class="glyphicon glyphicon-pencil"></span></asp:LinkButton></td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
