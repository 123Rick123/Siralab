<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPersonaRecaudacion.ascx.cs" Inherits="SIRALAB.Controles.wucPersonaRecaudacion" %>


<%--<link href="style/popup-form.css" rel="stylesheet" />
<link href="style/master.css" rel="stylesheet" />
<link href="style/iconos.css" rel="stylesheet" />
<link href="style/master.css" rel="stylesheet" />
<link href="style/iconos.css" rel="stylesheet" />
<link href="style/autocomplete.css" rel="stylesheet" />--%>

<div class="modal" id="modalPersonaRecuadacion">
    <div class="modal-dialog modal-lg">
        <asp:UpdatePanel ID="pnlCapturaUsuario" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <asp:Label ID="lblTitulo" runat="server" Text="Configuración de Usuario"></asp:Label>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>Nombre*:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:Panel ID="pnlNombre" runat="server">
                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:LinkButton ID="lnkbtnSel" runat="server" CausesValidation="false" OnClick="lnkbtnSel_Click"></asp:LinkButton>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>Paterno*:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>Materno:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                <asp:Label ID="lblRFC" runat="server" Text="RFC:"></asp:Label></label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>CURP:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtCURP" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>Usuario*:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>Contraseña*</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtPass" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>Correo electrónico*:</label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtCorreoElectronico" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row modal-row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                <asp:CheckBox ID="chkActivo" runat="server" Text="   Activo" Checked="true" /></label>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <asp:RequiredFieldValidator ID="rfv_txtNombre" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtNombre" ErrorMessage="<b>Nombre</b> *<br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_txtNombre" runat="server" TargetControlID="rfv_txtNombre"></asp:ValidatorCalloutExtender>

                            <asp:RequiredFieldValidator ID="rfv_txtApellidoPaterno" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtApellidoPaterno" ErrorMessage="<b>Apellido Paterno</b> *<br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_txtApellidoPaterno" runat="server" TargetControlID="rfv_txtApellidoPaterno"></asp:ValidatorCalloutExtender>


                            <asp:RequiredFieldValidator ID="rfv_txtUsuario" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtUsuario" ErrorMessage="<b>Usuario</b> *<br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_txtUsuario" runat="server" TargetControlID="rfv_txtUsuario"></asp:ValidatorCalloutExtender>

                            <asp:RequiredFieldValidator ID="rfv_pass" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtPass" ErrorMessage="<b>Contraseña</b> *<br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vce" runat="server" TargetControlID="rfv_pass"></asp:ValidatorCalloutExtender>

                            <asp:RequiredFieldValidator ID="rfv_txtCorreoElectronico" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtCorreoElectronico" ErrorMessage="<b>Correo Electrónico</b> *<br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rev_txtCorreoElectronico" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtCorreoElectronico" ErrorMessage="El formato no es correcto" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_txtCorreoElectronico" runat="server" TargetControlID="rfv_txtCorreoElectronico"></asp:ValidatorCalloutExtender>
                            <asp:ValidatorCalloutExtender ID="vce_rfv_txtCorreoElectronico2" runat="server" TargetControlID="rev_txtCorreoElectronico"></asp:ValidatorCalloutExtender>





                            <asp:RegularExpressionValidator ID="revRFCexp" runat="server" ControlToValidate="txtRFC" ErrorMessage="El formato no es correcto" Display="None" ValidationExpression="^([a-zA-Z]{4})((\d{2}((0[13578]|1[02])(0[1-9]|[12]\d|3[01])|(0[13456789]|1[012])(0[1-9]|[12]\d|30)|02(0[1-9]|1\d|2[0-8])))|([02468][048]|[13579][26])0229)([a-zA-Z0-9]{3})$"></asp:RegularExpressionValidator>


                            <asp:ValidatorCalloutExtender ID="vceRFCexp" runat="server" TargetControlID="revRFCexp"></asp:ValidatorCalloutExtender>


                            <div style="display: none;">
                                <asp:HiddenField ID="hf" runat="server" />
                                <asp:HiddenField ID="hfLaboratorioID" runat="server" />
                                <asp:HiddenField ID="hfPersonaID" runat="server" />
                                <asp:HiddenField ID="hfUsuarioID" runat="server" />
                                <asp:HiddenField ID="hfRFC" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="btn btn-default" ToolTip="Guardar" ValidationGroup="gprDatos" OnClick="lnkbGuardar_Click">Guardar</asp:LinkButton>
                        
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>



