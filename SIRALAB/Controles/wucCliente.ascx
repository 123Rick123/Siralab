<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCliente.ascx.cs" Inherits="SIRALAB.Controles.wucCliente" %>


<%@ Register Src="wucPtoDescarga.ascx" TagName="wucPtoDescarga" TagPrefix="uc1" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />

<style type="text/css">
    .div-colapsadoAnexo {
        display: none;
    }
</style>

<script type="text/javascript">
    function MuestraAnexo(d) {
        if (d == 2)
        { $('.div-colapsadoAnexo').slideToggle(400); };
    }
</script>
<script type="text/javascript">
    function movePosition(id) {
        valor = 0;
        var offs = $('.div-mode').css('margin-left');
        if (id == 2) {
            valor = 2;
            RecuperaValor();
            $('.div-mode').animate({ 'marginLeft': '-840px' }, 1200, function () { });
        }
        else if (id == 1) {
            valor = 1;
            RecuperaValor();
            $('.div-mode').animate({ 'marginLeft': '0px' }, 1200, function () { });
        }
    }
    function RecuperaValor() {
        document.getElementById('<%= hfIdOpcionJS.ClientID %>').value = valor;
    }
</script>

<div id="div_wucCliente" runat="server" class="div-mode">
    <asp:Panel ID="pnlDatos" runat="server" Style="width: 650px; height: 750px;">
        <asp:UpdatePanel ID="upClientes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlClienteEstado" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlClienteMunicipio" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="rbl" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="lnkbtnClienteNuevoAnexo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkbtnPtoDescarga" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="formulario-seccion">
                    <div class="seccion-encabezado">
                        <div class="seccion-titulo">Datos generales del cliente</div>
                    </div>
                    <div style="padding: 5px 10px 10px 15px">
                        <table style="width: 600px;">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbl" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbl_SelectedIndexChanged" AutoPostBack="true" Enabled="true">
                                        <asp:ListItem Value="1" Selected="True">Fisica</asp:ListItem>
                                        <asp:ListItem Value="2">Moral</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div-Grayseccion">RFC *</div>
                                </td>
                                <td id="DivCURP" runat="server">
                                    <div class="div-Grayseccion">CURP</div>
                                </td>
                            </tr>
                            <tr>
                                <td>


                                    <asp:TextBox ID="txtClienteRFC" runat="server" CssClass="control-campo" MaxLength="13" Style="width: 290px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtClienteRFC" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteRFC" ErrorMessage="<b>RFC*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vce_rfv_txtClienteRFC" runat="server" TargetControlID="rfv_txtClienteRFC" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>

                                    <asp:RegularExpressionValidator ID="revRFC_moral" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteRFC" ErrorMessage="El RFC no tiene el formato correcto, persona moral" Display="None" ValidationExpression="^([a-zA-Z]{3})((\d{2}((0[13578]|1[02])(0[1-9]|[12]\d|3[01])|(0[13456789]|1[012])(0[1-9]|[12]\d|30)|02(0[1-9]|1\d|2[0-8])))|([02468][048]|[13579][26])0229)([a-zA-Z0-9]{3})$" Enabled="false"></asp:RegularExpressionValidator><!-- Persona Moral -->
                                    <asp:RegularExpressionValidator ID="revRFC_fisica" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteRFC" ErrorMessage="El RFC no tiene el formato correcto, persona fisica" Display="None" ValidationExpression="^([a-zA-Z]{4})((\d{2}((0[13578]|1[02])(0[1-9]|[12]\d|3[01])|(0[13456789]|1[012])(0[1-9]|[12]\d|30)|02(0[1-9]|1\d|2[0-8])))|([02468][048]|[13579][26])0229)([a-zA-Z0-9]{3})$"></asp:RegularExpressionValidator><!-- Persona Física -->

                                    <asp:ValidatorCalloutExtender ID="vceRFCExp" runat="server" TargetControlID="revRFC_moral" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                                    <asp:ValidatorCalloutExtender ID="vceRFCExp2" runat="server" TargetControlID="revRFC_fisica" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>


                                </td>
                                <td>
                                    <asp:TextBox ID="txtClienteCURP" runat="server" Style="width: 280px;" MaxLength="50" CssClass="control-campo"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="div-Grayseccion" style="width: 250px;">Nombre o Razón Social *</div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtClienteNombre" runat="server" Style="width: 590px;" MaxLength="250" CssClass="control-campo"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtClienteNombre" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteNombre" ErrorMessage="<b>Nombre*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vce_rfv_txtClienteNombre" runat="server" TargetControlID="rfv_txtClienteNombre" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                            <tr>
                                <td id="DIVAPaterno" runat="server">
                                    <div class="div-Grayseccion">
                                        <asp:Label ID="lblClienteAPaterno" runat="server" Text="Ap. Paterno *"></asp:Label>
                                    </div>
                                </td>
                                <td id="DIVAMaterno" runat="server">
                                    <div class="div-Grayseccion">
                                        <asp:Label ID="lblClienteAMaterno" runat="server" Text="Ap. Materno"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td id="DIVtxtAPaterno" runat="server">
                                    <asp:TextBox ID="txtClienteAPaterno" runat="server" CssClass="control-campo" MaxLength="70" Style="width: 290px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtClienteAPaterno" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteAPaterno" ErrorMessage="<b>Apellido Paterno*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vce_rfv_txtClienteAPaterno" runat="server" TargetControlID="rfv_txtClienteAPaterno" PopupPosition="BottomLeft"></asp:ValidatorCalloutExtender>


                                </td>
                                <td id="DIVtxtAMaterno" runat="server">
                                    <asp:TextBox ID="txtClienteAMaterno" runat="server" Style="width: 280px;" MaxLength="70" CssClass="control-campo"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <div class="div-Grayseccion" style="width: 595px;">Correo Electrónico</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtCorreoElectronico" runat="server" CssClass="control-campo" Width="580px"></asp:TextBox>
                                </td>
                                <asp:RequiredFieldValidator ID="rfv_txtCorreoElectronico" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtCorreoElectronico" ErrorMessage="<b>Descripción</b> *<br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="rev_txtCorreoElectronico" runat="server" ValidationGroup="gprDatos" ControlToValidate="txtCorreoElectronico" ErrorMessage="El formato no es correcto" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="vce_rfv_txtCorreoElectronico" runat="server" PopupPosition="BottomLeft" TargetControlID="rfv_txtCorreoElectronico"></asp:ValidatorCalloutExtender>
                                <asp:ValidatorCalloutExtender ID="vce_rfv_txtCorreoElectronico2" runat="server" PopupPosition="BottomLeft" TargetControlID="rev_txtCorreoElectronico"></asp:ValidatorCalloutExtender>
                    </div>
                    </td>
                                </tr>
                            
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <div class="div-Grayseccion" style="width: 595px;">Domicilio Fiscal</div>
                                </td>
                            </tr>
                        </table>

                    <table style="width: 600px;">
                        <%--Domicilio--%>
                        <tr>
                            <td>
                                <div class="div-Grayseccion">Calle *</div>
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
                                    <asp:TextBox ID="txtClienteCalle" runat="server" CssClass="control-campo" MaxLength="150" Style="width: 355px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtClienteCalle" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteCalle" ErrorMessage="<b>Calle*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_txtClienteCalle" runat="server" TargetControlID="rfv_txtClienteCalle"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtClienteNInterior" runat="server" MaxLength="20" CssClass="control-campo" Style="width: 100px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtClienteNInterior" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteNInterior" ErrorMessage="<b>Número interior*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_txtClienteNInterior" runat="server" TargetControlID="rfv_txtClienteNInterior"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtClienteNExterior" runat="server" MaxLength="20" CssClass="control-campo" Style="width: 100px;"></asp:TextBox>
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
                                <div class="div-Grayseccion" style="width: 100px;">Colonia *</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtClienteCP" runat="server" CssClass="control-campo" MaxLength="7"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftb_txtClienteCP" runat="server" TargetControlID="txtClienteCP" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="rfv_txtClienteCP" runat="server" ValidationGroup="Cliente" ControlToValidate="txtClienteCP" ErrorMessage="<b>Número interior*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_txtClienteCP" runat="server" TargetControlID="rfv_txtClienteCP"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlClienteEstado" runat="server" CssClass="control-campo" Style="width: 150px;" OnSelectedIndexChanged="ddlClienteEstado_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlClienteEstado" runat="server" ValidationGroup="Cliente" InitialValue="0" ControlToValidate="ddlClienteEstado" ErrorMessage="<b>Estado*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_ddlClienteEstado" runat="server" TargetControlID="rfv_ddlClienteEstado"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlClienteMunicipio" runat="server" CssClass="control-campo" Style="width: 150px;" OnSelectedIndexChanged="ddlClienteMunicipio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlClienteMunicipio" runat="server" ValidationGroup="Cliente" InitialValue="0" ControlToValidate="ddlClienteMunicipio" ErrorMessage="<b>Delegación/Municipio*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_ddlClienteMunicipio" runat="server" TargetControlID="rfv_ddlClienteMunicipio"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlClienteColonia" runat="server" CssClass="control-campo" Style="width: 150px;"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlClienteColonia" runat="server" ValidationGroup="Cliente" InitialValue="0" ControlToValidate="ddlClienteColonia" ErrorMessage="<b>Colonia*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_ddlClienteColonia" runat="server" TargetControlID="rfv_ddlClienteColonia"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px;">
                                <div class="div-Grayseccion">Localidad *</div>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <div>
                                    <asp:DropDownList ID="ddlClienteLocalidad" runat="server" CssClass="control-campo" Style="width: 140px;"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlClienteLocalidad" runat="server" ValidationGroup="Cliente" InitialValue="0" ControlToValidate="ddlClienteLocalidad" ErrorMessage="<b>Localidad*</b><br />Campo Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="vce_rfv_ddlClienteLocalidad" runat="server" TargetControlID="rfv_ddlClienteLocalidad"></asp:ValidatorCalloutExtender>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table style="display: block; text-align: center; padding-top: 20px;">
                        <tr>
                            <td style="width: 70px; margin-left: 350px;">
                                <asp:LinkButton ID="lnkbtnClienteCancelar" runat="server" OnClick="lnkbtnClienteCancelar_Click" CssClass="icono-cancelar" ToolTip="Cancelar"><div></div></asp:LinkButton>
                            </td>
                            <td style="width: 50px;">
                                <asp:LinkButton ID="lnkbtnGuardar" runat="server" ValidationGroup="Cliente" OnClick="lnkbtnGuardar_Click" CssClass="icono-guardar" ToolTip="Guardar"><div></div></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="divPto" runat="server" style="text-align: right; width: 600px;">
                                <asp:LinkButton ID="lnkbtnPtoDescarga" runat="server" OnClick="lnkbtnPtoDescarga_Click" Visible="false" CssClass="icono-nvopto" ToolTip="Agregar nuevo titulo"><div></div></asp:LinkButton>

                            </td>
                        </tr>
                    </table>
                    <asp:LinkButton ID="lnkbtnClienteNuevoAnexo" runat="server" Style="display: none">NA</asp:LinkButton>
                    <div class="control-campo" style="margin-left: 10px; width: 560px;">
                        <asp:Repeater ID="rptTitulo" runat="server" OnItemDataBound="rptTitulo_ItemDataBound" ItemType="SIRALAB_DAL.Consecion">
                            <HeaderTemplate>
                                <table style="width: 560px; border-collapse: collapse;">
                                    <div class="div-Grayseccion" style="width: 550px; background-color: #555; color: white;">Titulos del cliente</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="seccion-tituloAnexo" style="width: 380px; position: inherit">
                                        <asp:LinkButton ID="lnkbtnClienteNuevoAnexo" runat="server" OnClick="lnkbtnClienteNuevoAnexo_Click" CommandArgument='<%#:Item.ConsecionID%>' CommandName='<%#:Item.ClienteID%>' CssClass="icono-anexo" ToolTip="Agregar nuevo anexo al titulo"></asp:LinkButton>
                                        <%#:Item.Titulo%>
                                    </td>
                                    <td class="seccion-tituloAnexo" style="width: 170px; position: inherit;">
                                        <a href="javascript:MuestraAnexo(2)" id="A1" style="color: green">Anexos</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div class="div-colapsadoAnexo" style="width: 555px;">
                                            <div class="control-campo">
                                                <asp:Repeater ID="rptAnexo" runat="server" ItemType="SIRALAB.Controles.AnexoCapturaArgs">
                                                    <HeaderTemplate>
                                                        <table style="width: 450px; text-align: center; margin-left: 30px;">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="seccion-anexo" style="width: 40px;">No. Anexo:
                                                                    <asp:LinkButton ID="lnkbtnAnexoConsulta" runat="server" Text='<%#:Item.Anexo %>' OnClick="lnkbtnAnexoConsulta_Click" CommandArgument='<%#:Item.AnexoID %>'></asp:LinkButton></td>
                                                            <td class="seccion-anexo" style="width: 100px;">Estado:
                                                                     <%#:Item.Direc.Estado.Nombre%>
                                                            </td>
                                                            <td class="seccion-anexo" style="width: 100px;">Municipio: <%#:Item.Direc.Municipio.Nombre%></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="seccion-anexo-division" style="width: 100px;">Calle: <%#:Item.Direc.Calle%></td>
                                                            <td class="seccion-anexo-division" style="width: 100px;">Num. Exterior: <%#:Item.Direc.NumeroExterior%></td>
                                                            <td class="seccion-anexo-division" style="width: 100px;">Num. Interior: <%#:Item.Direc.NumeroInterior%></td>
                                                        </tr>
                                                        <%--<%--' <%# ((SIRALAB.Controles.AnexoCapturaArgs)(Container.DataItem)).Direc.Calle%>'<div >No. Anexo:  <asp:LinkButton ID="lnkbtnAnexoConsulta" runat="server" Text='<%# ((SIRALAB.Controles.AnexoCapturaArgs)(Container.DataItem)).Anexo%>' ></asp:LinkButton></div>--%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <%--</div>
                    </div>--%>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
<div id="div_wucAnexo" runat="server" class="div-mode_">
    <asp:Panel ID="pnlPtoDescarga" runat="server" Style="width: 650px; margin-top: -500px; height: 550px; margin-left: 850px; display: block;">
        <div style="margin-top: -200px;">
            <uc1:wucPtoDescarga ID="wucPtoDescarga1" runat="server" />
        </div>
    </asp:Panel>
</div>




<asp:HiddenField ID="hfId" runat="server" />
<asp:HiddenField ID="hfCliente" runat="server" />
<asp:HiddenField ID="hfDireccion" runat="server" />
<asp:HiddenField ID="hfIdOpcionJS" runat="server" />
