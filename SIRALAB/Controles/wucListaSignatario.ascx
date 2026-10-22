<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucListaSignatario.ascx.cs" Inherits="SIRALAB.Controles.wucListaSignatario" %>

<asp:UpdatePanel ID="upSignatario" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional" ClientIDMode="AutoID">
    <ContentTemplate>
        <asp:Panel runat="server" ScrollBars="Vertical" Height="300">
            <asp:Repeater ID="rptSignatario" runat="server" ItemType="SIRALAB.Controles.SignatarioLista" OnItemDataBound="rptSignatario_OnItemDataBound">
                <HeaderTemplate>
                    <table class="table table-striped">
                        <tr>
                            <th style="width: 150px;">Laboratorio</th>
                            <th style="width: 50px;">Internos</th>
                            <th style="width: 50px;">Externos</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr data-id="<%#:Item.LaboratorioId %>">
                        <td><%#:Item.Laboratorio %></td>
                        <td style="text-align: center;"><%#:Item.Interno %></td>
                        <td style="text-align: center;"><%#:Item.Externo %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hfSignatario" runat="server" />
