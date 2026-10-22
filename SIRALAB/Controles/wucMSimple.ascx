<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMSimple.ascx.cs" Inherits="SIRALAB.Controles.wucMSimple" %>
<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<ul>
    <li class="horizontal">Muestra Simple</li>
</ul>
<ul>
    <li>
        <asp:Repeater ID="rptMSimple" runat="server">
            <HeaderTemplate>
                <div style="width: 5px;"></div>
            <table class="bandeja-grid" style="margin: 0 auto;">
                <tr>
                    <th>Parámetro</th>
                    <th>Resultado</th>
                    <th>Unidades</th>
                    <th>LC</th>
                    <th>Laboratorio</th>
                    <th>Método</th>
                    <th>Observaciones</th>
                </tr>
            </HeaderTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </li>
</ul>