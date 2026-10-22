<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCambiarPassword.ascx.cs" Inherits="SIRALAB.controles.wucCambiarPassword" %>
<script src="../js/ValidadoresPassword.js"></script>

<asp:UpdatePanel ID="upCambiarPassword" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbCancelar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <div class="modal fade" id="modal1" role="dialog">
            <div class="modal-dialog" role="document" style="margin-top: 124px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Ejemplo de ventana modal de confirmación</h4>
                        <asp:LinkButton ID="lnkbCerrar" runat="server" cssclass="close" ToolTip="Cerrar" OnClick="lnkbCerrar_Click" OnClientClick="$('body').removeClass('modal-open');$('.modal-backdrop').hide();"></asp:LinkButton>
                    </div>
                    <div  class="modal-body">
                       
                            <div class="row">
                                <div class="col-md-4 col-md-offset-3">
                                    <div class="form-group">
                                        <label for="name">Correo Electrónico<span class="control-labe">*</span>:</label>
                                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" Width="270px" ValidationGroup="vgCambiarPassword"></asp:TextBox>                                       
                                        <span id="errer" class="help-block with-errors"></span>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-md-offset-3">
                                    <div class="form-group">
                                        <label for="name">Password actual<span class="form-text">*</span>:</label>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Width="270px" MaxLength="25" ValidationGroup="vgCambiarPassword"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-md-offset-3">
                                    <div class="form-group">
                                        <label for="name">Nuevo password<span class="form-text">*</span>:</label>
                                        <asp:TextBox ID="txtPasswordNuevo" runat="server" TextMode="Password"  CssClass="form-control" Width="270px" MaxLength="25" ValidationGroup="vgCambiarPassword"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-md-offset-3">
                                    <div class="form-group">
                                        <label for="name">Repite password<span class="form-text">*</span>:</label>
                                        <asp:TextBox ID="txtPasswordRepite" runat="server" TextMode="Password" CssClass="form-control" Width="270px" MaxLength="25" ValidationGroup="vgCambiarPassword"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                   
                    <%-- <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ControlToValidate="txtCorreo" Display="None" ValidationGroup="vgCambiarPassword" ErrorMessage="Campo requerido."></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="vceCorreo" runat="server" TargetControlID="rfvCorreo"></asp:ValidatorCalloutExtender>--%>
                    <%-- <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="No es un correo electrónico válido" Display="None" ValidationGroup="vgCambiarPassword"></asp:RegularExpressionValidator>
                    <asp:ValidatorCalloutExtender ID="vceCorreo_2" runat="server" TargetControlID="revCorreo"></asp:ValidatorCalloutExtender>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="None" ValidationGroup="vgCambiarPassword" ErrorMessage="Campo requerido."></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="vcePassword" runat="server" TargetControlID="rfvPassword"></asp:ValidatorCalloutExtender>
                    <asp:RequiredFieldValidator ID="rfvPasswordNuevo" runat="server" ControlToValidate="txtPasswordNuevo" Display="None" ValidationGroup="vgCambiarPassword" ErrorMessage="Campo requerido."></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="vcePasswordNuevo" runat="server" TargetControlID="rfvPasswordNuevo"></asp:ValidatorCalloutExtender>
                    <asp:RequiredFieldValidator ID="rfvPasswordRepite" runat="server" ControlToValidate="txtPasswordRepite" InitialValue="0" Display="None" ValidationGroup="vgCambiarPassword" ErrorMessage="Campo requerido."></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="vcePasswordRepite" runat="server" TargetControlID="rfvPasswordRepite"></asp:ValidatorCalloutExtender>
                    <asp:CompareValidator ID="cvPasswordIgual" runat="server" ControlToCompare="txtPasswordNuevo" ControlToValidate="txtPasswordRepite" Operator="Equal" ErrorMessage="Las contraseñas no coinciden" Display="None" ValidationGroup="vgCambiarPassword"></asp:CompareValidator>
                    <asp:ValidatorCalloutExtender ID="vcePasswordIgual" runat="server" TargetControlID="cvPasswordIgual"></asp:ValidatorCalloutExtender>--%>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbGuardar_Click" ValidationGroup="vgCambiarPassword"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbCancelar" runat="server" CssClass="btn btn-default" Text="Cancelar" OnClick="lnkbCancelar_Click" OnClientClick="$('body').removeClass('modal-open');$('.modal-backdrop').hide();"></asp:LinkButton>

                    </div>
                </div>

            </div>
        </div>
        <div style="display: none;">
            <asp:HiddenField ID="hf" runat="server" />
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
