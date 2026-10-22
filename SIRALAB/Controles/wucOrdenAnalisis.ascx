<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucOrdenAnalisis.ascx.cs" Inherits="SIRALAB.Controles.wucOrdenAnalisis" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/autocomplete.css" rel="stylesheet" type="text/css" />
<link href="style/botones.css" rel="stylesheet" type="text/css" />
<link href="style/popup-form.css" rel="stylesheet" />
<link href="style/grid.css" rel="stylesheet" type="text/css" />
<%--<link href="style/paginador.css" rel="stylesheet" />--%>
<link href="style/iconos.css" rel="stylesheet" type="text/css" />

<%@ Register Src="~/Controles/wucAnalisisComplementario.ascx" TagPrefix="uc1" TagName="wucAnalisisComplementario" %>

<style type="text/css">
    .table-list > tbody > tr:nth-child(even) > td {
        background-color: #F2F2F2;
    }
</style>

<asp:UpdatePanel ID="updOrdenTrabajo" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlAnalisis" runat="server" Visible="false">

            <asp:Panel ID="pnlNormasMexicanas" runat="server">
            </asp:Panel>
            <div class="row">
                <div class="col-sm-12">

                    <div class="panel-group ficha-collapse" id="accordion">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h5>
                                    <span id="txtPanel" runat="server"></span>
                                </h5>
                            </div>
                            <div style="width: 97%; margin: 0 auto;">
                                <div class="panel-body">
                                    <asp:Repeater ID="rptTitulo" runat="server" ItemType="SIRALAB.Controles.AnalisisLista" OnItemDataBound="rptTitulo_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="table table-condensed table-striped text-center">
                                                <tr>
                                                    <th style="text-align: center; width: 60px;">Firmado</th>
                                                    <th style="text-align: center; width: 120px;">
                                                        <asp:Label ID="lblThAnalisis" runat="server"></asp:Label>
                                                    </th>
                                                    <th style="text-align: center; width: 80px;">
                                                        <asp:Label ID="lblThFechaConformacion" runat="server"></asp:Label>
                                                    </th>
                                                    <th runat="server" id="thMuestra" style="text-align: center; width: 120px;">Muestras simples</th>
                                                    <th style="text-align: center; width: 80px;">Enviado</th>
                                                    <th style="text-align: center; width: 120px;">Acción</th>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="chkAnalisisFirmado" runat="server" Checked='<%#Item.Firmado_%>' Enabled='<%#!Item.Firmado_%>' Visible='<%#Item.FirmaVisible_ && Item.AnalisisEncolado_ != true%>' />
                                                    <asp:HiddenField ID="hfAnalisisID" runat="server" Value='<%#Item.AnaisisID_%>' />
                                                </td>
                                                <td runat="server" id="tdAnalisis" style="text-align: center;">
                                                    <asp:Label ID="lblAnalisis" runat="server" Text='<%#Item.Consecutivo_%>'></asp:Label>
                                                </td>
                                                <td runat="server" id="tdFechaConformacion" style="text-align: center;">
                                                    <asp:Label ID="lblFecha" runat="server" Text='<%#Item.FechaRegistro_%>'></asp:Label>
                                                </td>
                                                <td runat="server" id="tdMuestra" style="text-align: center;">
                                                    <asp:Label ID="lblMuestra" runat="server" Text='<%#Item.MuestrasSimples_%>'></asp:Label>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:ImageButton ID="imgbtnEnviado" Enabled="false" runat="server" Width="16px" Height="16px" Visible="true" ImageUrl='<%#Item.Enviado_%>' />
                                                    <asp:LinkButton ID="lnkbtnHeredado" runat="server" CssClass="botonHeredado" ToolTip="Análisis Asociado" Enabled="false"></asp:LinkButton>
                                                </td>

                                                <td>
                                                    <asp:LinkButton ID="lnkbtnDocumento" runat="server" ToolTip="Documento" CssClass="btn btn-default btn-xs" CommandArgument="<%#Item.AnaisisID_ %>" OnClick="lnkbtnDocumento_Click">
                                <span class="glyphicon glyphicon-file"></span>
                                                    </asp:LinkButton>
                                                     <asp:LinkButton ID="lnkbtnAcuse" runat="server" Visible="<%#Item.Firmado_ %>" ToolTip="Acuse" CssClass="btn btn-default btn-xs" CommandArgument="<%#Item.AnaisisID_ %>" OnClick="lnkbtnAcuse_Click">
                                <span class="glyphicon glyphicon-file"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnSeleccionar" runat="server" ToolTip="Editar" CssClass=" btn btn-default btn-xs" OnClick="lnkbtnSeleccionar_Click" CommandArgument="<%#Item.AnaisisID_ %>">
                                <span class="glyphicon glyphicon-pencil"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnCancelar" runat="server" ToolTip="Cancelar" CssClass="btn btn-default btn-xs" CommandArgument="<%#Item.AnaisisID_ %>" OnClick="lnkbtnCancelar_Click" >   
                                                        <span class="glyphicon glyphicon-minus-sign"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnAsociar" runat="server" ToolTip="Asociar nuevo análisis" CssClass="btn btn-default btn-xs" OnClick="lnkbtnAsociar_Click" CommandArgument="<%#Item.AnaisisID_ %>">
                                                        <span class="glyphicon glyphicon-check"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnEliminar" runat="server" ToolTip="Eliminar análisis" CssClass="btn btn-default btn-xs" OnClick="lnkbtnEliminar_Click" CommandArgument="<%#Item.AnaisisID_ %>" Visible="<%# !Item.Firmado_  && Item.AnalisisEncolado_ != true %>">
                                                        <span class="glyphicon glyphicon-trash"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="linkSubirArchivo" runat="server" ToolTip="Asociar archivo al análisis" CssClass="btn btn-default btn-xs" OnClick="linkSubirArchivo_OnClick" CommandArgument="<%#Item.AnaisisID_ %>" Visible="<%# !Item.Firmado_ %>">
                                                        <span class="fa fa-upload"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnFaltaPago" runat="server" ToolTip="Indicar falta de pago" CssClass="btn btn-default btn-xs" OnClick="lnkbtnFaltaPago_Click" CommandArgument="<%#Item.AnaisisID_ %>">
                                                        <span class="glyphicon glyphicon-info-sign"></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnComplementar" runat="server" ToolTip="Generar análisis complementario" CssClass="btn btn-default btn-xs" OnClick="lnkbtnComplementar_Click" CommandArgument="<%#Item.AnaisisID_ %>">
                                                        <span class="glyphicon glyphicon-import"></span>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <div style="text-align: right; width: 90%; margin: 15px auto 0 auto;">
                                        <asp:LinkButton ID="lnkbtnFirmar" runat="server" CssClass="btn btn-default btn-md" Visible="false" ToolTip="Firmar y enviar" OnClick="lnkbtnFirmar_Click">
                                            <span class="glyphicon glyphicon-edit"></span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hfOrdenTrabajo" runat="server" />

<uc1:wucAnalisisComplementario runat="server" ID="wucAnalisisComplementario" OnGuardar="wucAnalisisComplementario_Guardar" OnCerrar="wucAnalisisComplementario_Cerrar" OnCancelar="wucAnalisisComplementario_Cancelar" />
