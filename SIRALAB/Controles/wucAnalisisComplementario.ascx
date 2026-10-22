<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAnalisisComplementario.ascx.cs" Inherits="SIRALAB.controles.wucAnalisisComplementario" %>
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
<asp:UpdatePanel ID="upAnalisisComplementario" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbCancelar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlAnalisisComplementario" runat="server" CssClass="popform" style="width: 30%;" DefaultButton="lnkbGuardar">
            <div class="popform-title">
                <asp:LinkButton ID="lnkbCerrar" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
                <asp:Label ID="lblTitulo" runat="server" Text="Generar análisis complementario"></asp:Label>
            </div>
            <div class="pop-contenido">
                <ul>
                    <li>
                        <table class="formulario-tabla">
                            <tr>
                                <td class="titulo-campo">
                                    Norma
                                </td>
                            </tr>
                            <tr>
                                <td class="buscar-campos">
                                    <asp:DropDownList runat="server" ID="ddlNormaComplementaria" CssClass="control-select" Width="100%" AppendDataBoundItems="True"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </li>
                    <li class="popform-buttons">
                        <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="boton" Text="Aceptar" OnClick="lnkbGuardar_Click" ValidationGroup="vgAnalisisComplementario"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbCancelar" runat="server" CssClass="boton" Text="Cancelar" OnClick="lnkbCancelar_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
            <asp:RequiredFieldValidator ID="rfvNormaComplementaria" runat="server" ControlToValidate="ddlNormaComplementaria" Display="None" ValidationGroup="vgAnalisisComplementario" InitialValue="0" ErrorMessage="Selecciona el campo requerido" />
            <asp:ValidatorCalloutExtender ID="vceNormaComplementaria" runat="server" TargetControlID="rfvNormaComplementaria"></asp:ValidatorCalloutExtender>
        </asp:Panel>
        
        <asp:ModalPopupExtender ID="mpeAnalisisComplementario" runat="server" PopupControlID="pnlAnalisisComplementario" TargetControlID="hf" BackgroundCssClass="popUpBackground"></asp:ModalPopupExtender>
        <div style="display: none;">
            <asp:HiddenField ID="hf" runat="server" />
        </div>

    </ContentTemplate>
</asp:UpdatePanel>