<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucGrupoConfiguracion.ascx.cs" Inherits="SIRALAB.Controles.wucGrupoConfiguracion" ClientIDMode="AutoID" %>

<style type="text/css">
    
    .modal-xl {
        width: 90%;
        max-width: 1200px;
    }
</style>
<div id="modalGrupoConfiguracion" class="modal">
    <asp:UpdatePanel ID="upGrupoConfiguracion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="modal-dialog modal-xl">
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="rbRango" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rbValor" EventName="CheckedChanged" />
            <%--<asp:AsyncPostBackTrigger ControlID="lnkbCancelar" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="lnkbGuardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="rptGrupoConfiguracion" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlGrupoConfiguracion" runat="server" DefaultButton="lnkbGuardar" CssClass="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <%--<asp:LinkButton ID="lnkbCerrar" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>--%>
                    <asp:Label ID="lblTitulo" runat="server" Text="Grupos de parámetros" CssClass="modal-title"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Parámetro de grupo*:</label>
                                    <asp:DropDownList ID="ddlParametroGrupo" runat="server" CssClass="form-control" Width="97%"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvNombreGrupo" runat="server" Display="None" ErrorMessage="Este campo es requerido." InitialValue="0" ControlToValidate="ddlParametroGrupo" ValidationGroup="vgGrupoConfiguracion" />
                                    <asp:ValidatorCalloutExtender ID="rfvvceNombreGrupo" runat="server" TargetControlID="rfvNombreGrupo" />
                                </div>
                            </div>


                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Operación de consolidado*:</label>
                                    <asp:DropDownList ID="ddlOperacion" runat="server" CssClass="form-control" Width="97%"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvOperacion" runat="server" Display="None" ErrorMessage="Este campo es requerido." InitialValue="0" ControlToValidate="ddlOperacion" ValidationGroup="vgGrupoConfiguracion" />
                                    <asp:ValidatorCalloutExtender ID="rfvvceOperacion" runat="server" TargetControlID="rfvOperacion" />
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-12">
                                <label>Tipo de evaluación*:</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:RadioButton ID="rbRango" runat="server" Text="Rango" GroupName="Evaluacion" Checked="true" AutoPostBack="true" OnCheckedChanged="rbRango_CheckedChanged" />
                                        <asp:RadioButton ID="rbValor" runat="server" Text="Valor" GroupName="Evaluacion" AutoPostBack="true" OnCheckedChanged="rbValor_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:CheckBox ID="chkNoEvalua" runat="server" Text="No aplica evaluación" AutoPostBack="true" OnCheckedChanged="chkNoEvalua_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:CheckBox ID="chkExenta" runat="server" Text="Exenta" Checked="true" />
                                        <asp:CheckBox ID="chkActivo" runat="server" Checked="true" Text="Activo" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:CheckBox ID="chkLimitesLFD" runat="server" Text="Límites para la Ley Federal de Derechos" AutoPostBack="true" OnCheckedChanged="chkLimitesLFD_CheckedChanged" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label><asp:Label ID="lblLimiteInferior" runat="server" Text="Límite inferior*:"></asp:Label></label>
                                        <asp:TextBox ID="txtLimiteInferior" runat="server" CssClass="form-control" placeholder="0.000000000"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fteLimiteInferior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteInferior" ValidChars="." />
                                        <asp:RegularExpressionValidator ID="revLimiteInferior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgGrupoConfiguracion" ControlToValidate="txtLimiteInferior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                        <asp:ValidatorCalloutExtender ID="vcerevLimiteInferior" runat="server" TargetControlID="revLimiteInferior" Enabled="True" />
                                        <asp:RequiredFieldValidator ID="rfvLimiteInferior" runat="server" Display="None" ErrorMessage="Este campo es requerido." ControlToValidate="txtLimiteInferior" ValidationGroup="vgGrupoConfiguracion" />
                                        <asp:ValidatorCalloutExtender ID="rfvvceLimiteInferior" runat="server" Enabled="True" TargetControlID="rfvLimiteInferior" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label><asp:Label ID="lblLimiteSuperior" runat="server" Text="Límite superior*:"></asp:Label></label>
                                        <asp:TextBox ID="txtLimiteSuperior" runat="server" CssClass="form-control" placeholder="0.000000000"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fteLimiteSuperior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteSuperior" ValidChars="." />
                                        <asp:RegularExpressionValidator ID="revLimiteSuperior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgGrupoConfiguracion" ControlToValidate="txtLimiteSuperior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                        <asp:ValidatorCalloutExtender ID="vceLimiteSuperior" runat="server" Enabled="True" TargetControlID="revLimiteSuperior" />
                                        <asp:RequiredFieldValidator ID="rfvLimiteSuperior" runat="server" Display="None" ErrorMessage="Este campo es requerido." ControlToValidate="txtLimiteSuperior" ValidationGroup="vgGrupoConfiguracion" />
                                        <asp:ValidatorCalloutExtender ID="vcerfvLimiteSuperior" runat="server" TargetControlID="rfvLimiteSuperior" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnlLimitesLFD" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label><asp:Label ID="lblLFDInferior" runat="server" Text="Límite inferior LFD*:"></asp:Label></label>
                                            <asp:TextBox ID="txtLFDInferior" runat="server" CssClass="form-control" placeholder="0.000000000"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fteLFDInferior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLFDInferior" ValidChars="." />
                                            <asp:RegularExpressionValidator ID="revLFDInferior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgConfiguracion" ControlToValidate="txtLFDInferior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                            <asp:ValidatorCalloutExtender ID="vceLFDInferior1" runat="server" TargetControlID="revLFDInferior" Enabled="True" />
                                            <asp:RequiredFieldValidator ID="rfvLFDInferior" runat="server" Display="None" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="txtLFDInferior" ValidationGroup="vgConfiguracion" />
                                            <asp:ValidatorCalloutExtender ID="vceLFDInferior2" runat="server" Enabled="True" TargetControlID="rfvLFDInferior" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label><asp:Label ID="lblLFDSuperior" runat="server" Text="Límite superior LFD*:"></asp:Label></label>
                                            <asp:TextBox ID="txtLFDSuperior" runat="server" CssClass="form-control" placeholder="0.000000000"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fteLFDSuperior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLFDSuperior" ValidChars="." />
                                            <asp:RegularExpressionValidator ID="revLFDSuperior" runat="server" Display="None" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgConfiguracion" ControlToValidate="txtLFDSuperior" ValidationExpression="^[\d]{0,9}(\.[\d]{0,9})?$" />
                                            <asp:ValidatorCalloutExtender ID="vceLFDSuperior1" runat="server" Enabled="True" TargetControlID="revLFDSuperior" />
                                            <asp:RequiredFieldValidator ID="rfvLFDSuperior" runat="server" Display="None" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="txtLFDSuperior" ValidationGroup="vgConfiguracion" />
                                            <asp:ValidatorCalloutExtender ID="vceLFDSuperior2" runat="server" TargetControlID="rfvLFDSuperior" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h5>Grupos de norma.</h5>
                                    </div>
                                    <div class="panel-body table-responsive">
                                        <asp:Repeater ID="rptGrupoConfiguracion" runat="server" ItemType="SIRALAB.Controles.GrupoConfiguracionItem">
                                            <HeaderTemplate>
                                                <table class="table table-condensed table-striped">
                                                    <tr>
                                                        <th>Nombre</th>
                                                        <th style="width: 120px">Límites</th>
                                                        <th style="width: 120px">Límites LFD</th>
                                                        <th style="width: 120px">Operación</th>
                                                        <th style="width: 60px">Acciones</th>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#: Item.NombreGrupo %></td>
                                                    <td style="text-align: center;"><%#: Item.Limite %></td>
                                                    <td style="text-align: center;"><%#: Item.LimiteLFD %></td>
                                                    <td style="text-align: center;"><%#: Item.NombreOperacion %></td>
                                                    <td style="text-align: center;">
                                                        <asp:LinkButton ID="lnkbEditar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="btn btn-default btn-xs" ToolTip="Click para editar el grupo" OnClick="lnkbEditar_Click" CausesValidation="false">
                                                            <span class="glyphicon glyphicon-pencil"></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbActivar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="btn btn-default btn-xs" ToolTip="Click para desactivar del grupo" OnClick="lnkbActivar_Click" CausesValidation="false">
                                                            <span class="fas fa-toggle-on"></span>
                                                            
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr>
                                                    <td style="text-align: center;"><%#: Item.NombreGrupo %></td>
                                                    <td style="text-align: center;"><%#: Item.Limite %></td>
                                                    <td style="text-align: center;"><%#: Item.LimiteLFD %></td>
                                                    <td style="text-align: center;"><%#: Item.NombreOperacion %></td>
                                                    <td style="text-align: center;">
                                                        <asp:LinkButton ID="lnkbEditar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="btn btn-default btn-xs" ToolTip="Click para editar el grupo" OnClick="lnkbEditar_Click" CausesValidation="false">
                                                            <span class="glyphicon glyphicon-pencil"></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbActivar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="btn btn-default btn-xs" ToolTip="Click para desactivar del grupo" OnClick="lnkbActivar_Click" CausesValidation="false">
                                                            <span class="fas fa-toggle-on"></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:LinkButton ID="lnkbCancelar" runat="server" CssClass="btn btn-danger" ToolTip="Cancelar" CausesValidation="false" OnClick="lnkbCancelar_Click" Text="Cancelar">Reiniciar</asp:LinkButton>
                            <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="btn btn-default" ToolTip="Guardar" ValidationGroup="vgGrupoConfiguracion" OnClick="lnkbGuardar_Click" Text="Guardar">Guardar</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

