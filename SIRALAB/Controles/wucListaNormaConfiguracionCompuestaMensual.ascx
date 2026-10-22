<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucListaNormaConfiguracionCompuestaMensual.ascx.cs" Inherits="SIRALAB.Controles.wucListaNormaConfiguracionCompuestaMensual" ClientIDMode="AutoID" %>


<style>
    .text-aligment{ text-align: justify; font-size: 8pt; }
</style>

<asp:Repeater ID="rptrNormaConfiguracion" runat="server" ClientIDMode="AutoID" ItemType="SIRALAB.Controles.NormaConfiguracionItem" OnItemDataBound="rptrNormaConfiguracion_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-bordered" style="font-size: 7pt;">
            <caption>
                <asp:Label ID="lblTitulo" runat="server" Text="Parámetros de muestra compuesta"></asp:Label>
                <asp:LinkButton ID="lnkbAgregar_1" runat="server" CssClass="icono-guardar-16" OnClick="lnkbAgregar_Click" ToolTip="Agregar parámetros a mensual" ValidationGroup="vgListaCompuestaMensual"></asp:LinkButton>
            </caption>
            <tr>
                <th>Parámetro</th>
                <th style="width: 140px;">Límite</th>
                <th style="width: 140px;">Límite LFD</th>
                <th style="width: 50px;">Incluir</th>
                <th style="width: 50px;">No evalúa</th>
                <th style="width: 120px;">Operación</th>
                <th style="width: 120px;">Tratamiento decimales</th>
                <th style="width: 40px;"></th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="text-align: left;"><%#: Item.NombreParametro %></td>
            <td style="text-align: center;">
                <asp:Label ID="lblPermanente" runat="server" Text="Presencia"></asp:Label>
                <asp:Panel ID="pnlLimite" runat="server">
                    <table style="border-collapse: collapse;">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlLimiteInf" runat="server">
                                    <table style="width: 130px; margin: 0 auto; border-collapse: collapse;">
                                        <tr>
                                            <td style="width: 30px;">Inf.</td>
                                            <td  style=\"width: 100px;\">
                                                <asp:TextBox ID="txtLimiteInferior" runat="server" Text="<%#: Item.LimiteInferior %>" CssClass="control-campo" Width="80px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fteLimiteInferior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteInferior" ValidChars="." />
                                                <asp:RegularExpressionValidator ID="revLimiteInferior" runat="server" Display="None" Enabled="false" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgListaCompuestaMensual" ControlToValidate="txtLimiteInferior" ValidationExpression="^\d{0,10}(\.\d{1,9})?$" />
                                                <asp:ValidatorCalloutExtender ID="vcerevLimiteInferior" runat="server" TargetControlID="revLimiteInferior" />
                                                <asp:RequiredFieldValidator ID="rfvLimiteInferior" runat="server" ControlToValidate="txtLimiteInferior" Enabled="false" ValidationGroup="vgListaCompuestaMensual" ErrorMessage="Los límites son requeridos." Display="None"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vcerfvLimiteInferior" runat="server" TargetControlID="rfvLimiteInferior" PopupPosition="TopRight" Width="200px"></asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlLimiteSup" runat="server">
                                    <table style="width: 130px; margin: 0 auto; border-collapse: collapse;">
                                        <tr>
                                            <td style="width: 30px;">Sup.</td>
                                            <td  style=\"width: 100px;\">
                                                <asp:TextBox ID="txtLimiteSuperior" runat="server" Text="<%#: Item.LimiteSuperior %>" CssClass="control-campo" Width="80px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fteLimiteSuperior" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteSuperior" ValidChars="." />
                                                <asp:RegularExpressionValidator ID="revLimiteSuperior" runat="server" Display="None" Enabled="false" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgListaCompuestaMensual" ControlToValidate="txtLimiteInferior" ValidationExpression="^\d{0,10}(\.\d{1,9})?$" />
                                                <asp:ValidatorCalloutExtender ID="vcerevLimiteSuperior" runat="server" TargetControlID="revLimiteSuperior" />
                                                <asp:RequiredFieldValidator ID="rfvLimiteSuperior" runat="server" ControlToValidate="txtLimiteSuperior" Enabled="false" ValidationGroup="vgListaCompuestaMensual" ErrorMessage="Los límites son requeridos." Display="None"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vcerfvLimiteSuperior" runat="server" TargetControlID="rfvLimiteSuperior" PopupPosition="TopRight" Width="200px"></asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td style="text-align: center;">
                <asp:Label ID="lblPermanenteLFD" runat="server" Text="Presencia"></asp:Label>
                <asp:Panel ID="pnlLimiteLFD" runat="server">
                    <table style="border-collapse: collapse;">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlLimiteInfLFD" runat="server">
                                    <table style="width: 130px; margin: 0 auto; border-collapse: collapse;">
                                        <tr>
                                            <td style="width: 30px;">Inf.</td>
                                            <td  style=\"width: 100px;\">
                                                <asp:TextBox ID="txtLimiteInfLFD" runat="server" Text="<%#: Item.LimiteInferiorLFD %>" CssClass="control-campo" Width="80px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fteLimiteInferiorLFD" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteInfLFD" ValidChars="." />
                                                <asp:RegularExpressionValidator ID="revLimiteInferiorLFD" runat="server" Display="None" Enabled="false" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgListaCompuestaMensual" ControlToValidate="txtLimiteInfLFD" ValidationExpression="^\d{0,10}(\.\d{1,9})?$" />
                                                <asp:ValidatorCalloutExtender ID="vcerevLimiteInferiorLFD" runat="server" TargetControlID="revLimiteInferiorLFD" />
                                                <asp:RequiredFieldValidator ID="rfvLimiteInferiorLFD" runat="server" ControlToValidate="txtLimiteInfLFD" Enabled="false" ValidationGroup="vgListaCompuestaMensual" ErrorMessage="Los límites son requeridos." Display="None"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vcerfvLimiteInferiorLFD" runat="server" TargetControlID="rfvLimiteInferiorLFD" PopupPosition="TopRight" Width="200px"></asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlLimiteSupLFD" runat="server">
                                    <table style="width: 130px; margin: 0 auto; border-collapse: collapse;">
                                        <tr>
                                            <td style="width: 30px;">Sup.</td>
                                            <td  style=\"width: 100px;\">
                                                <asp:TextBox ID="txtLimiteSuperiorLFD" runat="server" Text="<%#: Item.LimiteSuperiorLFD %>" CssClass="control-campo" Width="80px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fteLimiteSuperiorLFD" runat="server" FilterType="Numbers, Custom" TargetControlID="txtLimiteSuperiorLFD" ValidChars="." />
                                                <asp:RegularExpressionValidator ID="revLimiteSuperiorLFD" runat="server" Display="None" Enabled="false" ErrorMessage="Introduzca correctamente un valor decimal." ValidationGroup="vgListaCompuestaMensual" ControlToValidate="txtLimiteSuperiorLFD" ValidationExpression="^\d{0,10}(\.\d{1,9})?$" />
                                                <asp:ValidatorCalloutExtender ID="vcerevLimiteSuperiorLFD" runat="server" TargetControlID="revLimiteSuperiorLFD" />
                                                <asp:RequiredFieldValidator ID="rfvLimiteSuperiorLFD" runat="server" ControlToValidate="txtLimiteSuperiorLFD" Enabled="false" ValidationGroup="vgListaCompuestaMensual" ErrorMessage="Los límites son requeridos." Display="None"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vcerfvLimiteSuperiorLFD" runat="server" TargetControlID="rfvLimiteSuperiorLFD" PopupPosition="TopRight" Width="200px"></asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td style="text-align: center;"><asp:CheckBox ID="chkIncluir" runat="server" AutoPostBack="true" OnCheckedChanged="chkIncluir_CheckedChanged" /></td>
            <td style="text-align: center;"><asp:CheckBox ID="chkNoEvalua" runat="server" AutoPostBack="true" OnCheckedChanged="chkNoEvalua_CheckedChanged" /></td>
            <td style="text-align: center;">
                <asp:DropDownList ID="ddlOperacion" runat="server" Width="97%" CssClass="control-select">
                    </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvOperacion" runat="server" ControlToValidate="ddlOperacion" Enabled="false" InitialValue="0" ValidationGroup="vgListaCompuestaMensual" ErrorMessage="Debe seleccionar una operación de consolidado." Display="None"></asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="vceOperacion" runat="server" TargetControlID="rfvOperacion" PopupPosition="TopRight" Width="200px"></asp:ValidatorCalloutExtender>
            </td>
            <td style="text-align: center;">
                <asp:DropDownList ID="ddlTrataDecimales" runat="server" Width="97%" CssClass="control-select">
                    <asp:ListItem Value="0" Text="Seleccione"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Redondeo"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Truncamiento"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="text-align: center;">
                <asp:LinkButton ID="lnkbActivar" runat="server" CommandArgument="<%#: Item.NormaConfiguracionID %>" CssClass="icono-cerrarrojo-16" OnClick="lnkbActivar_Click" ToolTip="Quita el registro dentro del Mensual">
                </asp:LinkButton>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
        <asp:Panel ID="pnlNoItems" runat="server" Visible="false" style="text-align: center; margin: 20px auto; color: #777; font-size: 12pt;">
            No hay elementos en el listado.
        </asp:Panel>
    </FooterTemplate>
</asp:Repeater>
