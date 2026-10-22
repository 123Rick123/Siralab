<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPtoDescarga.ascx.cs" Inherits="SIRALAB.Controles.wucPtoDescarga" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function te(d) {
        if (d == 1)
        { $('.div-animado').slideToggle(400); };
    }
</script>
<script type="text/javascript">
    function movePositionPto(id)
    {
        valor = 0;
        var offs = $('.login-mode').css('margin-left');
        if (id == 2) {
            valor = 2;
            RecuperaValor();
            $('.login-mode').animate({ 'marginLeft': '-840px' }, 1200, function () { });}
        else if (id == 3) {
            valor = 3;
            RecuperaValor();
            $('.login-mode').animate({ 'marginLeft': '-1040px' }, 1200, function () { });
        }
        else if (id == 1) {
            valor = 1;
            RecuperaValor();
            $('.login-mode').animate({ 'marginLeft': '0px' }, 1200, function () { });
        }
    }
    function RecuperaValor() {
        document.getElementById('<%= hfPtoIdOpcionJS.ClientID %>').value = valor;
    }
</script>

<div>
    <asp:Panel ID="pnlPtoDescarga" runat="server">
        <asp:UpdatePanel ID="updpnlPtoDescarga" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlPtoEstado" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPtoMunicipio" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="lnkbtnCancelar" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="formulario-seccion">
                    <div class="seccion-encabezado">
                        <div class="seccion-titulo">Datos del punto de descarga</div>

                    </div>
                    <table style="width: 600px;">
                        <tr>
                            <td>
                                <div class="div-Grayseccion">Titulo *</div>
                            </td>
                            <td id="Td1" runat="server">
                                <div class="div-Grayseccion">Anexo *</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPtoTitulo" runat="server" CssClass="control-campo" MaxLength="25" Style="width: 290px;"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revTitulo" runat="server" ControlToValidate="txtPtoTitulo" ValidationGroup="punto" ValidationExpression="([\w\d]{1}\d{1}\w{3}\d{6}/\d{2}\w{4}\d{2})$" ErrorMessage="Titulo no valido" Display="None"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_revTitulo" runat="server" TargetControlID="revTitulo"></asp:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoTitulo" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoTitulo" ErrorMessage="<b>Titulo*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoTitulo" runat="server" TargetControlID="rfv_txtPtoTitulo"></asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPtoAnexo" runat="server" Style="width: 280px;" MaxLength="3" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftb_txtPtoAnexo" runat="server" TargetControlID="txtPtoAnexo" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoAnexo" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoAnexo" ErrorMessage="<b>Anexo*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoAnexo" runat="server" TargetControlID="rfv_txtPtoAnexo"></asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 600px;">
                        <%--Domicilio--%>
                        <tr>
                            <td>
                                <div class="div-Grayseccion">Calle*</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 90px;">No. Exterior</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 90px;">No. Interior</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoCalle" runat="server" CssClass="control-campo" MaxLength="150" Style="width: 355px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtPtoCalle" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoCalle" ErrorMessage="<b>Calle*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoCalle" runat="server" TargetControlID="rfv_txtPtoCalle"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoNExterior" runat="server" CssClass="control-campo" MaxLength="20" Style="width: 100px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtPtoNExterior" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoNExterior" ErrorMessage="<b>Número exterior*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoNExterior" runat="server" TargetControlID="rfv_txtPtoNExterior"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoNInterior" runat="server" CssClass="control-campo" MaxLength="20" Style="width: 100px;"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 600px;">
                        <tr>
                            <td style="width: 100px;">
                                <div class="div-Grayseccion">C.P. *</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 100px;">
                                    Estado *
                                </div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 100px;">Municipio *</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 100px;">Localidad *</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoCP" runat="server" CssClass="control-campo" MaxLength="7"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftb_txtPtoCP" runat="server" TargetControlID="txtPtoCP" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="rfv_txtPtoCP" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoCP" ErrorMessage="<b>CP*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoCP" runat="server" TargetControlID="rfv_txtPtoCP"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlPtoEstado" runat="server" CssClass="control-campo" Style="width: 150px;" OnSelectedIndexChanged="ddlPtoEstado_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoEstado" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoEstado" ErrorMessage="<b>Estado*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_ddlPtoEstado" runat="server" TargetControlID="rfv_ddlPtoEstado"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlPtoMunicipio" runat="server" CssClass="control-campo" Style="width: 150px;" OnSelectedIndexChanged="ddlPtoMunicipio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoMunicipio" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoMunicipio" ErrorMessage="<b>Delegación/Municipio*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_ddlPtoMunicipio" runat="server" TargetControlID="rfv_ddlPtoMunicipio"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlPtoLocalidad" runat="server" CssClass="control-campo" Style="width: 150px;"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoLocalidad" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoLocalidad" ErrorMessage="<b>Localidad*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_ddlPtoLocalidad" runat="server" TargetControlID="rfv_ddlPtoLocalidad"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px;">
                                <div class="div-Grayseccion">Colonia *</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion">Datum *</div>
                            </td>
                            <td colspan="2">
                                <div class="div-Grayseccion" style="width: 200px;">Efluente de Planta Tratamiento</div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlPtoColonia" runat="server" CssClass="control-campo" Style="width: 140px;"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoColonia" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoColonia" ErrorMessage="<b>Colonia*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_ddlPtoColonia" runat="server" TargetControlID="rfv_ddlPtoColonia"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPtoDatum" runat="server" Style="width: 120px;" CssClass="control-campo"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoDatum" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoDatum" ErrorMessage="<b>Datum*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoDatum" runat="server" TargetControlID="rfv_txtPtoDatum"></asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblPtoEfluente" runat="server" RepeatDirection="Horizontal" Style="margin-left: 20px;">
                                    <asp:ListItem Value="true" Selected="True" Text="SI"></asp:ListItem>
                                    <asp:ListItem Value="false" Text="NO"></asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="3">
                                <div class="div-Grayseccion" style="width: 300px;">Latitud</div>
                            </td>
                            <td colspan="3">
                                <div class="div-Grayseccion" style="width: 300px;">Longitud</div>
                            </td>
                        </tr>
                        <tr>
                            <td>Grados *:</td>
                            <td>Minutos *:</td>
                            <td>Segundos *:</td>
                            <td>Grados *:</td>
                            <td>Minutos *:</td>
                            <td>Segundos *:</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtPtoLatitudGrados" runat="server" Style="width: 70px;" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftb_txtPtoLatitudGrados" runat="server" TargetControlID="txtPtoLatitudGrados" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoLatitudGrados" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLatitudGrados" ErrorMessage="<b>Grados*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoLatitudGrados" runat="server" TargetControlID="rfv_txtPtoLatitudGrados"></asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtPtoLatitudMinutos" runat="server" Style="width: 70px;" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftb_txtPtoLatitudMinutos" runat="server" TargetControlID="txtPtoLatitudMinutos" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoLatitudMinutos" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLatitudMinutos" ErrorMessage="<b>Minutos*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoLatitudMinutos" runat="server" TargetControlID="rfv_txtPtoLatitudMinutos"></asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtPtoLatitudSeg" runat="server" Style="width: 70px;" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftv_txtPtoLatitudSeg" runat="server" TargetControlID="txtPtoLatitudSeg" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoLatitudSeg" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLatitudSeg" ErrorMessage="<b>Segundos*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoLatitudSeg" runat="server" TargetControlID="rfv_txtPtoLatitudSeg"></asp:ValidatorCalloutExtender>

                                <asp:DropDownList ID="ddlLatitud" runat="server" CssClass="control-campo">
                                    <asp:ListItem Text="N" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="S" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPtoLongitudGrados" runat="server" Style="width: 70px;" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftb_txtPtoLongitudGrados" runat="server" TargetControlID="txtPtoLongitudGrados" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoLongitudGrados" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLongitudGrados" ErrorMessage="<b>Grados*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoLongitudGrados" runat="server" TargetControlID="rfv_txtPtoLongitudGrados"></asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtPtoLongitudMinutos" runat="server" Style="width: 70px;" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftb_txtPtoLongitudMinutos" runat="server" TargetControlID="txtPtoLongitudMinutos" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoLongitudMinutos" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLongitudMinutos" ErrorMessage="<b>Minutos*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoLongitudMinutos" runat="server" TargetControlID="rfv_txtPtoLongitudMinutos"></asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtPtoLongitudSeg" runat="server" Style="width: 70px;" CssClass="control-campo"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftb_txtPtoLongitudSeg" runat="server" TargetControlID="txtPtoLongitudSeg" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfv_txtPtoLongitudSeg" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLongitudSeg" ErrorMessage="<b>Segundos*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_txtPtoLongitudSeg" runat="server" TargetControlID="rfv_txtPtoLongitudSeg"></asp:ValidatorCalloutExtender>

                                <asp:DropDownList ID="ddlLongitud" runat="server" CssClass="control-campo">
                                    <asp:ListItem Text="E" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="O" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>

                    <%--Comienza cuerpo receptor, uso de agua y número del medidor--%>

                        <table>
                        <tr>
                            <td>
                                <div class="div-Grayseccion" style="width: 190px;">Tipo Cuerpo Receptor</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 130px;">Uso de Agua</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 110px;">Num. Medidor</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:DropDownList ID="ddlCuerpoReceptor" runat="server" CssClass="control-campo" Style="width: 200px;"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlCuerpoReceptor" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlCuerpoReceptor" ErrorMessage="<b>Cuerpo Receptor*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_ddlCuerpoReceptor" runat="server" TargetControlID="rfv_ddlCuerpoReceptor"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:DropDownList ID="ddlUsoAgua" runat="server" CssClass="control-campo" Style="width: 200px;"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlUsoAgua" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlUsoAgua" ErrorMessage="<b>Uso de Agua*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"  ID="vce_rfv_ddlUsoAgua" runat="server" TargetControlID="rfv_ddlUsoAgua"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtNumeroMedidor" runat="server" MaxLength="50" CssClass="control-campo" Style="width: 190px;"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <%--Fin de Cuerpo receptor, uso agua y número de medidor--%>

                    <table>
                        <tr>
                            <td>
                                <div class="div-Grayseccion" style="width: 190px;">Nombre Cuerpo Receptor</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 130px;">Fuente Abastecimiento</div>
                            </td>
                            <td>
                                <div class="div-Grayseccion" style="width: 110px;">Origen Descarga</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoCuerpoR" runat="server" CssClass="control-campo" MaxLength="150" Style="width: 280px;"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoFuenteA" runat="server" MaxLength="150" CssClass="control-campo" Style="width: 150px;"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtPtoOrigenD" runat="server" MaxLength="150" CssClass="control-campo" Style="width: 140px;"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table style="display: block; text-align: center; padding-top: 20px;">
                        <tr>
                            <td style="width: 70px; margin-left: 400px;">
                                <asp:LinkButton ID="lnkbtnCancelar" runat="server" CssClass="icono-cancelar"  OnClick="lnkbtnCancelar_Click" ToolTip="Cancelar"></asp:LinkButton>
                                <%--<a href="javascript:movePosition(1)" id="A3" class="icono-agregar-16 collapse-action">Cancelar</a>--%>
                            </td>
                            <td style="width: 70px;">
                                <asp:LinkButton ID="lnkbtnPtoGuardar" runat="server" CssClass="icono-guardar" ValidationGroup="punto" OnClick="lnkbtnPtoGuardar_Click" ToolTip="Guardar"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hfPtoConsecion" runat="server" />
                <asp:HiddenField ID="hfTransaccion" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
<asp:HiddenField ID="hfPtoId" runat="server" />
<asp:HiddenField ID="hfPtoCliente" runat="server" />
<asp:HiddenField ID="hfPtoDireccion" runat="server" />

<asp:HiddenField ID="hfPtoIdOpcionJS" runat="server" />