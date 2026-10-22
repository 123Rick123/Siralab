<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucSolicitarCancelacionDetalle.ascx.cs" Inherits="SIRALAB.Controles.wucSolicitarCancelacionDetalle" %>
<%@ Import Namespace="SIRALAB_BL.common" %>
<%@ Import Namespace="System.Windows.Forms.VisualStyles" %>
<style>
    body {
    }

    .popUpBackground {
        background-color: #5f5c5c;
        opacity: 0.6; /* Opacidad 60% */
        width: 200px;
        height: 70px;
    }
</style>
<asp:UpdatePanel ID="upCancelarSolicitud" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbAceptar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlCancelarSolicitud" runat="server" CssClass="panel panel-default" Style="width: 80%;" DefaultButton="lnkbAceptar">

          

            <div class="panle-body">
                <div class="container">
                      <asp:LinkButton ID="lnkbCerrar" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
            <asp:Label ID="lblTitulo" runat="server" Text="Solicitud de cancelación - detalle"></asp:Label>
                    <div class="row">
                        <div class="col-md-12">
                            <table class="table table-bordered">
                                <tr>
                                    <td>Folio</td>
                                    <td>Laboratorio</td>
                                    <td>Fecha de Solicitud</td>
                                    <td>Fecha de Envío</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFolio" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblLaboratorio" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblFechaSolicitud" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblFechaEnvio" runat="server"></asp:Label></td>
                                </tr>
                                <td class="titulo-campo" colspan="4">Motivo</td>
                                <tr>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div style="max-height: 200px; overflow-y: auto; overflow-x: no-display;">
                                            <asp:Label ID="lblMotivo" runat="server"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Repeater ID="rptDetalle" runat="server" ItemType="SIRALAB.Controles.SolicitudCancelacionDetalleItem">
                                <HeaderTemplate>
                                    <table class="table table-bordered">
                                        <caption>Análisis selecionados</caption>
                                        <tr>
                                            <th>No. de Informe</th>
                                            <th>Norma</th>
                                            <th>Cliente</th>
                                            <th>Fecha de Emisión</th>
                                            <th>Título</th>
                                            <th>Anexo</th>
                                            <%; if (UsuarioRol == enumUsuarioRol.Laboratorio && Solicitud.EstatusID == enumEstatusCancelacion.Aprobada.ParseToInt())
                                                { %>
                                            <th></th>
                                            <% } %>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Item.NumeroInforme%></td>
                                        <td><%#Item.Norma%></td>
                                        <td><%#Item.Cliente%></td>
                                        <td><%#Item.FechaEmision.ToString("dd/MM/yyyy")%></td>
                                        <td><%#Item.Titulo%></td>
                                        <td><%#Item.Anexo%></td>
                                        <%; if (UsuarioRol == enumUsuarioRol.Laboratorio && Solicitud.EstatusID == enumEstatusCancelacion.Aprobada.ParseToInt())
                                            { %>
                                        <td>
                                            <asp:LinkButton ID="lnkEditarAnalisis" runat="server" CssClass="btn btn-default" ToolTip="Corregir análisis" OnClick="lnkEditarAnalisis_OnClick" data-analisis-id="<%#Item.AnalisisID%>" Visible="<%# Item.AnalisisPadreID == null %>"><span class="glyphicon glyphicon-list-alt"></span></asp:LinkButton>
                                        </td>
                                        <% } %>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

            </div>
            <div class="panel-footer">
                <asp:LinkButton ID="lnkbAceptar" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbAceptar_Click"></asp:LinkButton>
            </div>
        </asp:Panel>

        <asp:ModalPopupExtender ID="mpeCancelarSolicitud" runat="server" PopupControlID="pnlCancelarSolicitud" TargetControlID="hf" BackgroundCssClass="popUpBackground"></asp:ModalPopupExtender>
        <asp:HiddenField ID="hf" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
