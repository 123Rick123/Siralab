<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPopFiles.ascx.cs" Inherits="SIRALAB.Controles.wucPopFiles" %>




<%@ Register src="wucEvidenciaPop.ascx" tagname="wucEvidenciaPop" tagprefix="uc1" %>



<asp:Panel ID="pnlPopFiles" runat="server">
    <div class="popform">
        <div class="popform-title">
            <asp:LinkButton ID="lnkbCerrar" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
            <asp:Label ID="lblTitulo" runat="server" Text="Carga de Evidencia"></asp:Label>
        </div>
        <div class="pop-contenido" style="width: 600px; padding: 10px; overflow: hidden;">
           
            <uc1:wucEvidenciaPop ID="wucEvidenciaPop1" runat="server" OnCargado="wucEvidenciaPop1_Cargado" />
           
        </div>
    </div>
</asp:Panel>
        
<asp:ModalPopupExtender ID="mpePopFiles" runat="server" PopupControlID="pnlPopFiles" TargetControlID="hf" BackgroundCssClass="popUpBackground"></asp:ModalPopupExtender>
<div style="display: none;">
    <asp:HiddenField ID="hf" runat="server" />
</div>