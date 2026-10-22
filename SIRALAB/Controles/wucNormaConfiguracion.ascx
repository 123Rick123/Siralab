<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucNormaConfiguracion.ascx.cs" Inherits="SIRALAB.Controles.wucNormaConfiguracion" %>

<link href="../style/master.css" rel="stylesheet" type="text/css" />
<link href="../style/popup-form.css" rel="stylesheet" type="text/css" />
<link href="../style/iconos.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .text-control {
        border: 1px solid #D2D2D2;
        padding: 3px 5px;
    }

    .rbl label {
        margin-left: 10px;
        margin-right: 10px;
        font-weight: normal !important;
    }

    .modal-xl {
        width: 95%;
        max-width: 1300px;
    }
</style>

<div id="modalConfiguracion" class="modal fade">
    <asp:UpdatePanel ID="upConfiguracion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="modal-dialog modal-xl">
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="rbRango" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rbValor" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rbPresencia" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="lnkbAgregarGrupo" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="lnkbCancelar" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="lnkbGuardar" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlConfiguracion" runat="server" DefaultButton="lnkbGuardar" CssClass="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblTitulo" runat="server" Text="Configuración de norma"></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">

                    <div class="row">
                        <div class="col-md-4">
                            <label class="control-label">Parámetro *</label>
                            <asp:DropDownList ID="ddlParametro" runat="server" ValidationGroup="vgConfiguracion" CssClass="form-control "></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvParametro" runat="server" ErrorMessage="Este campo es requerido" ControlToValidate="ddlParametro" InitialValue="0" ValidationGroup="vgConfiguracion" Display="None" />
                            <asp:ValidatorCalloutExtender ID="vceParametro" runat="server" TargetControlID="rfvParametro" />
                        </div>
                        <div class="col-md-4" id="divUsoDelAgua" runat="server">
                            <label class="control-label">Uso de agua *</label>
                            <asp:DropDownList ID="ddlUsoAgua" runat="server" ValidationGroup="vgConfiguracion" CssClass="form-control "></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUsoAgua" runat="server" InitialValue="0" ErrorMessage="Este campo es requerido" ControlToValidate="ddlUsoAgua" ValidationGroup="vgConfiguracion" Display="None" />
                            <asp:ValidatorCalloutExtender ID="vceUsoAgua" runat="server" TargetControlID="rfvUsoAgua" />
                        </div>
                        <div class="col-md-4">
                            <label class="control-label">Cuerpo receptor *</label>
                            <asp:DropDownList ID="ddlCuerpoReceptor" runat="server" ValidationGroup="vgConfiguracion" CssClass="form-control "></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCuerpoReceptor" runat="server" InitialValue="0" ErrorMessage="Este campo es requerido" ControlToValidate="ddlCuerpoReceptor" ValidationGroup="vgConfiguracion" Display="None" />
                            <asp:ValidatorCalloutExtender ID="vceCuerpoReceptor" runat="server" TargetControlID="rfvCuerpoReceptor" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label">Etiqueta *</label>
                            <asp:TextBox ID="txtEtiqueta" runat="server" CssClass="form-control " ValidationGroup="vgConfiguracion"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEtiqueta" runat="server" ErrorMessage="Este campo es requerido" ControlToValidate="txtEtiqueta" ValidationGroup="vgConfiguracion" Display="None" CssClass="has-error" />
                        </div>
                       <div class="col-md-3">
                            <label class="control-label">
                                <asp:Label ID="lblEscalaDecimal" runat="server" Text="Escala Decimal*"></asp:Label>
                            </label>
                            <asp:TextBox ID="txtEscalaDecimal" runat="server" CssClass="form-control " TextMode="Number" min="0" max="9" MaxLength="1" ValidationGroup="vgConfiguracion"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEscalaDecimal" runat="server" InitialValue="" ErrorMessage="Este campo es requerido" ControlToValidate="txtEscalaDecimal" ValidationGroup="vgConfiguracion" Display="None" />
                            <asp:ValidatorCalloutExtender ID="vceEscalaDecimal" runat="server" TargetControlID="rfvEscalaDecimal" />
                        </div>
                        <div class="col-md-3">
                            <label class="control-label">
                                <asp:Label ID="lblEscalaDecimalLCM" runat="server" Text="Escala Decimal LC/MC*"></asp:Label>
                            </label>
                            <asp:TextBox ID="txtEscalaDecimalLCM" runat="server" CssClass="form-control " TextMode="Number" min="0" max="9" MaxLength="1" ValidationGroup="vgConfiguracion"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEscalaDecimalLCM" runat="server" InitialValue="" ErrorMessage="Este campo es requerido" ControlToValidate="txtEscalaDecimalLCM" ValidationGroup="vgConfiguracion" Display="None" />
                            <asp:ValidatorCalloutExtender ID="vceEscalaDecimalLCM" runat="server" TargetControlID="rfvEscalaDecimalLCM" />
                        </div>
                        <div class="col-md-3">
                            <label class="control-label">
                                <asp:Label ID="lblTrataDecimales" runat="server" Text="Tratamiento Decimales*"></asp:Label>
                            </label>
                            <asp:RadioButtonList ID="rbTrataDecimales" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem class="rbl" style="font-weight: normal"> Redondeo</asp:ListItem>
                                <asp:ListItem class="rbl" style="font-weight: normal"> Truncamiento</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <br />
                    <div class="row" runat="server" ID="pnlValoresMinMax">
                        <div class="col-md-4">
                            <div class="checkbox">
                                <label class="control-label">
                                    <asp:CheckBox ID="chkLimitaValoresAceptados" runat="server" AutoPostBack="true" OnCheckedChanged="chkLimitaValoresAceptados_CheckedChanged" />Limita valores aceptados
                                </label>
                            </div>

                        </div>
                        <asp:Panel runat="server" ID="pnlInputValoresAceptados" Visible="False">
                            <div class="col-md-4">
                                <label runat="server" id="lblMinAceptado" class="control-label">Valor mínimo aceptado</label>
                                <asp:TextBox runat="server" ID="txtMinAceptado" CssClass="form-control"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="fteMinAceptado" runat="server" FilterType="Numbers, Custom" TargetControlID="txtMinAceptado" ValidChars="." />
                            </div>
                            <div class="col-md-4">
                                <label runat="server" id="lblMaxAceptado" class="control-label">Valor máximo aceptado</label>
                                <asp:TextBox runat="server" ID="txtMaxAceptado" CssClass="form-control"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="fteMaxAceptado" runat="server" FilterType="Numbers, Custom" TargetControlID="txtMinAceptado" ValidChars="." />
                            </div>
                        </asp:Panel>

                    </div>
                    <br/>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <div class="checkbox">
                                    <label>
                                        <asp:CheckBox ID="chkNoEvalua" runat="server" AutoPostBack="true" OnCheckedChanged="chkNoEvalua_CheckedChanged" />No aplica evaluación
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Tipo de evaluación *</label><br />
                                <label class="radio-inline">
                                    <asp:RadioButton ID="rbPresencia" runat="server" GroupName="Evaluacion" AutoPostBack="true" OnCheckedChanged="rbPresencia_CheckedChanged" />Presencia
                                </label>
                                <label class="radio-inline">
                                    <asp:RadioButton ID="rbRango" runat="server" GroupName="Evaluacion" Checked="true" AutoPostBack="true" OnCheckedChanged="rbRango_CheckedChanged" />Rango
                                </label>
                                <label class="radio-inline">
                                    <asp:RadioButton ID="rbValor" runat="server" GroupName="Evaluacion" AutoPostBack="true" OnCheckedChanged="rbValor_CheckedChanged" />Valor
                                </label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="chkLimitesLFD" runat="server" AutoPostBack="true" OnCheckedChanged="chkLimitesLFD_CheckedChanged" />Límites para la Ley Federal de Derechos</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <label class="control-label">
                                        <asp:Label ID="lblLimiteInferior" runat="server" Text="Límite inferior"></asp:Label></label>
                                    <asp:TextBox ID="txtLimiteInferior" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fteLimiteInferior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteInferior" ValidChars="." />
                                    <asp:RegularExpressionValidator ID="revLimiteInferior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgConfiguracion" ControlToValidate="txtLimiteInferior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                    <asp:ValidatorCalloutExtender ID="vcerevLimiteInferior" runat="server" TargetControlID="revLimiteInferior" Enabled="True" />
                                    <asp:RequiredFieldValidator ID="rfvLimiteInferior" runat="server" Display="None" ErrorMessage="Este campo es requerido" ControlToValidate="txtLimiteInferior" ValidationGroup="vgConfiguracion" />
                                    <asp:ValidatorCalloutExtender ID="rfvvceLimiteInferior" runat="server" Enabled="True" TargetControlID="rfvLimiteInferior" />
                                </div>
                                <div class="col-md-6">
                                    <label class="control-label">
                                        <asp:Label ID="lblLimiteSuperior" runat="server" Text="Límite superior"></asp:Label></label>
                                    <asp:TextBox ID="txtLimiteSuperior" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fteLimiteSuperior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteSuperior" ValidChars="." />
                                    <asp:RegularExpressionValidator ID="revLimiteSuperior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgConfiguracion" ControlToValidate="txtLimiteSuperior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                    <asp:ValidatorCalloutExtender ID="vceLimiteSuperior" runat="server" Enabled="True" TargetControlID="rfvLimiteSuperior" />
                                    <asp:RequiredFieldValidator ID="rfvLimiteSuperior" runat="server" Display="None" ErrorMessage="Este campo es requerido" ControlToValidate="txtLimiteSuperior" ValidationGroup="vgConfiguracion" />
                                    <asp:ValidatorCalloutExtender ID="vcerfvLimiteSuperior" runat="server" TargetControlID="rfvLimiteSuperior" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <asp:Panel ID="pnlLimitesLFD" runat="server" Visible="false" CssClass="row">
                                <div class="col-md-6">
                                    <label class="control-label">
                                        <asp:Label ID="lblLFDInferior" runat="server" Text="Límite inferior"></asp:Label></label>
                                    <asp:TextBox ID="txtLFDInferior" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fteLFDInferior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLFDInferior" ValidChars="." />
                                    <asp:RegularExpressionValidator ID="revLFDInferior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgConfiguracion" ControlToValidate="txtLFDInferior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                    <asp:ValidatorCalloutExtender ID="vceLFDInferior1" runat="server" TargetControlID="revLFDInferior" Enabled="True" />
                                    <asp:RequiredFieldValidator ID="rfvLFDInferior" runat="server" Display="None" ErrorMessage="Este campo es requerido" ControlToValidate="txtLFDInferior" ValidationGroup="vgConfiguracion" />
                                    <asp:ValidatorCalloutExtender ID="vceLFDInferior2" runat="server" Enabled="True" TargetControlID="rfvLFDInferior" />
                                </div>
                                <div class="col-md-6">
                                    <label class="control-label">
                                        <asp:Label ID="lblLFDSuperior" runat="server" Text="Límite superior"></asp:Label></label>
                                    <asp:TextBox ID="txtLFDSuperior" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fteLFDSuperior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLFDSuperior" ValidChars="." />
                                    <asp:RegularExpressionValidator ID="revLFDSuperior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgConfiguracion" ControlToValidate="txtLFDSuperior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                    <asp:ValidatorCalloutExtender ID="vceLFDSuperior1" runat="server" Enabled="True" TargetControlID="revLFDSuperior" />
                                    <asp:RequiredFieldValidator ID="rfvLFDSuperior" runat="server" Display="None" ErrorMessage="Este campo es requerido" ControlToValidate="txtLFDSuperior" ValidationGroup="vgConfiguracion" />
                                    <asp:ValidatorCalloutExtender ID="vceLFDSuperior2" runat="server" TargetControlID="rfvLFDSuperior" />
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <br />
                            <label class="checkbox-inline">
                                <asp:CheckBox ID="chkExenta" runat="server" Checked="true" />Exenta</label>
                            <label class="checkbox-inline">
                                <asp:CheckBox ID="chkActivo" runat="server" Checked="true" />Activo</label>
                            <label class="checkbox-inline">
                                <asp:CheckBox ID="chkLCMC" runat="server" Checked="true" />LC/MC</label>
                        </div>
                        <div class="col-md-6">
                            <label class="control-label">
                                <asp:Label ID="lblGrupo" runat="server" Text="Grupo"></asp:Label></label>
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:LinkButton ID="lnkbAgregarGrupo" runat="server" CssClass="input-group-addon" OnClick="lnkbAgregarGrupo_Click" CausesValidation="false">
                                        <i class="glyphicon glyphicon-plus"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="btn btn-default" ToolTip="Guardar" ValidationGroup="vgConfiguracion" OnClick="lnkbGuardar_Click">Guardar</asp:LinkButton>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

