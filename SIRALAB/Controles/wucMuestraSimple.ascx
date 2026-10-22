<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMuestraSimple.ascx.cs" Inherits="SIRALAB.Controles.wucMuestraSimple" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/botones.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .table-list > tbody > tr:nth-child(even) > td {
        background-color: #F2F2F2;
    }
</style>
<asp:UpdatePanel ID="updOrdenTrabajo" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlMuestraSimple" runat="server" CssClass="panel panel-default" Visible="false">
            <div class="panel-heading">                
                 <h5><span>Muestra simple</span></h5>
            </div>
            <div class="panel-body">
                 <asp:Repeater ID="rptTitulo" runat="server" ItemType="SIRALAB.Controles.MuestraLista">
                <HeaderTemplate>
                    <table class="table table-bordered table-condensed table-striped">
                        <caption class="text-center">Muestras</caption>
                        <tr>
                            <th style="width: 80px;">Número</th>
                            <th style="width: 110px;">Fecha</th>
                            <th style="width: 110px;">Hora</th>
                            <th>Observación</th>
                            <th style="width: 80px;">Acción</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="text-align: center;"><%#:Item.Consecutivo_%></td>
                        <td style="text-align: center;"><%#:Item.Fecha_%></td>
                        <td style="text-align: center;"><%#:Item.Hora_%></td>
                        <td><%#:Item.Observaciones_%></td>
                        <td style="text-align: center;">
<%--                            <asp:LinkButton ID="lnkbtnDocs" runat="server" ToolTip="Documentos" CssClass="botonAgregarDoc" OnClick="lnkbtnEvidencia_Click"  CommandArgument="<%#:Item.MuestraID_%>"></asp:LinkButton>--%>
                            <asp:LinkButton ID="lnkbtnSeleccionar" runat="server" ToolTip="Editar" CssClass="btn btn-default btn-sm" OnClick="lnkbtnSeleccionar_Click" CommandArgument="<%#:Item.MuestraID_%>"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div style="text-align: right; width: 90%; margin: 15px auto 0 auto;">
                <asp:LinkButton ID="lnkbtnMuestraCompuesta" runat="server" ToolTip="Generar muestra compuesta" CssClass="icono-compuesta" OnClick="lnkbtnMuestraCompuesta_Click"><div></div></asp:LinkButton>
            </div>
            </div>
           
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
