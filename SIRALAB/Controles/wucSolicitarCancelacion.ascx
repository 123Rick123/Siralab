<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucSolicitarCancelacion.ascx.cs" Inherits="SIRALAB.Controles.wucSolictarCancelacion" %>
<style>
    body{
 
}
.popUpBackground{
   background-color:#5f5c5c;
   opacity:0.6; /* Opacidad 60% */
   width:200px;
   height:70px;
}
</style>
<asp:UpdatePanel ID="upCancelarSolicitud" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbCancelar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlCancelarSolicitud" runat="server" CssClass="panel panel-default" style="width: 80%;" DefaultButton="lnkbGuardar">
           
            <div class="panel-body" style="max-height: 300px; overflow-y: scroll; overflow-x: no-display;">
                <div class="container">

               
                 <asp:LinkButton ID="lnkbCerrar" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
                <asp:Label ID="lblTitulo" runat="server" Text="Solicitud de cancelación"></asp:Label>
                        <asp:Repeater ID="rptDetalle" runat="server" ItemType="SIRALAB.Controles.SolicitudCancelacionAnalisisItem">
                            <HeaderTemplate>
                                <table class="table table-bordered">
                                    <caption>Análisis selecionados</caption>
                                    <tr>
                                        <th>No. de Informe</th>
                                        <th>Norma</th>
                                        <th>Fecha de Emisión</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%#Item.NumeroInforme%></td>
                                    <td><%#Item.Norma%></td>
                                    <td><%#Item.FechaEmision.ToString("dd/MM/yyyy")%></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                   
            
            <div class="pop-contenido">
               
                        <table class="table table-bordered">
                            <tr>
                                <td class="titulo-campo">Motivo de cancelación *</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtMotivo" runat="server" CssClass="control-campo" ValidationGroup="vgCancelarSolicitud" TextMode="MultiLine" style="height: 8em; width: 100%; box-sizing: border-box; resize: none;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMotivo" runat="server" ControlToValidate="txtMotivo" ValidationGroup="vgCancelarSolicitud" Display="None" SetFocusOnError="true" ErrorMessage="Campo requerido."></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vceMotivo" runat="server" TargetControlID="rfvMotivo"></asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                        </table>
                    
                   
                      
                  
            </div>
                </div>
                 </div>
            <div class="panel-footer">
                  <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbGuardar_Click" ValidationGroup="vgCancelarSolicitud" OnClientClick="return Page_ClientValidate('vgCancelarSolicitud')"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="lnkbCancelar_Click"></asp:LinkButton>
            </div>
        </asp:Panel>
        
        <asp:ModalPopupExtender ID="mpeCancelarSolicitud" runat="server" PopupControlID="pnlCancelarSolicitud" TargetControlID="hf" BackgroundCssClass="popUpBackground"></asp:ModalPopupExtender>
        <asp:HiddenField ID="hf" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>