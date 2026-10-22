<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConformacion.ascx.cs" Inherits="SIRALAB.Controles.wucConformacion" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="../style/iconos.css" rel="stylesheet" />
<link href="../style/autocomplete.css" rel="stylesheet" />

<div>
    <asp:Panel ID="pnlConformacion" runat="server">
        <asp:UpdatePanel ID="updpnlConformacion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="ddlPtoEstado" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPtoMunicipio" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="chPtoDescarga" EventName="CheckedChanged" />--%>
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlPtoConsecion" runat="server" DefaultButton="lnkbtnPtoConsecion">
                    <table style="width: 700px;">
                        <tr>
                            <td style="width: 205px;">
                                <div class="div-Grayseccion" style="width: 205px;">RFC Contribuyente</div>
                            </td>
                            <td style="width: 20px;"></td>
                            <td>
                                <div class="div-Grayseccion" style="width: 205px;">Titulo</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 205px;">Anexo</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoConsecion" runat="server" CssClass="control-campo" Style="width: 205px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtPtoConsecion" runat="server" ControlToValidate="txtPtoConsecion" ErrorMessage="Campo requerido" Display="None" ValidationGroup="Mensual"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vce_rfv_txtMuestraIdentificacion" runat="server" TargetControlID="rfv_txtPtoConsecion" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                                </div>
                                <asp:AutoCompleteExtender ID="aceClientes" runat="server" Enabled="True"
                                    ServicePath="~/wsConsultas.asmx" ServiceMethod="Encuentra_Clientes_ACMensual"
                                    MinimumPrefixLength="1" DelimiterCharacters="2" CompletionInterval="100" EnableCaching="true"
                                    CompletionSetCount="15" TargetControlID="txtPtoConsecion" CompletionListElementID="Div"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td style="width: 20px;">
                                <asp:LinkButton ID="lnkbtnPtoConsecion" runat="server" CssClass="icono-buscar" OnClick="lnkbtnPtoConsecion_Click" ValidationGroup="Mensual" ToolTip="Buscar Permiso"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTitulo" runat="server" CssClass="control-campo" Style="width: 205px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTitulo_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfv_ddlTitulo" runat="server" ControlToValidate="ddlTitulo" InitialValue="0" ErrorMessage="Campo requerido" Display="None" ValidationGroup="MensualGenerar"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vce_rfv_ddlTitulo" runat="server" TargetControlID="rfv_ddlTitulo" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAnexo" runat="server" CssClass="control-campo" Style="width: 205px;" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfv_ddlAnexo" runat="server" InitialValue="0" ControlToValidate="ddlAnexo" ErrorMessage="Campo requerido" Display="None" ValidationGroup="MensualGenerar"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vce_rfv_ddlAnexo" runat="server" TargetControlID="rfv_ddlAnexo" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 700px;">
                    <tr>
                        <td style="width: 205px;">
                             <asp:RadioButton ID="rbtPeriodo_Mensual" runat="server" Text="Mensual" Checked="true" GroupName="Periodo" AutoPostBack="true" OnCheckedChanged="rbtPeriodo_Mensual_CheckedChanged" />
                                    <asp:RadioButton ID="rbtPeriodo_Trimestral" runat="server" Text="Trimestral" GroupName="Periodo" AutoPostBack="true" OnCheckedChanged="rbtPeriodo_Trimestral_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <table style="width: 500px;">
                    <tr>
                        <td style="width: 150px;">
                            <div class="div-Grayseccion" style="width: 150px;">
                                Año
                            </div>
                        </td>
                        <td>
                            <div class="div-Grayseccion" style="width: 205px;">Periodo</div>
                        </td>
                        <td>
                            <div class="div-Grayseccion" style="width: 205px;">Norma</div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlAnio" runat="server" CssClass="control-campo" Style="width: 150px; display: block;" AutoPostBack="true"></asp:DropDownList>
                        </td>

                        <td>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="control-campo" Style="width: 205px;" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNorma" runat="server" CssClass="control-campo" Style="width: 205px;" AutoPostBack="true"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfv_ddlNorma" runat="server" InitialValue="0" ControlToValidate="ddlNorma" ErrorMessage="Campo requerido" Display="None" ValidationGroup="MensualGenerar"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vce_rfv_ddlNorma" runat="server" TargetControlID="rfv_ddlNorma" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkbtnPtoGenerar" runat="server" CssClass="icono-mensual" ToolTip="Generar informe mensual" ValidationGroup="MensualGenerar" Style="margin-left: 25px;" OnClick="lnkbtnPtoGenerar_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>

                <table style="width: 600px;">
                    <tr>
                        <td></td>
                    </tr>

                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
<asp:HiddenField ID="hfPtoId" runat="server" />
<asp:HiddenField ID="hfPtoCliente" runat="server" />
<asp:HiddenField ID="hfPtoDireccion" runat="server" />
<asp:HiddenField ID="hfPtoMuestraID" runat="server" />
<asp:HiddenField ID="hfPtoIdOpcionJS" runat="server" />
