<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFiltro_Consulta_Ordenes.ascx.cs" Inherits="SIRALAB.Controles.wucFiltro_Consulta_Ordenes" %>


<link href="style/master.css" rel="stylesheet" />
<link href="style/busqueda.laboratorio.css" rel="stylesheet" />

<asp:UpdatePanel ID="upBusqueda" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" class="buscar">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbLimpiar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbBuscar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel runat="server" DefaultButton="lnkbBuscar">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="buscar-txt" STYLE="display:none;"></asp:TextBox>
            <asp:LinkButton ID="lnkbBuscar" runat="server" CssClass="buscar-button" ToolTip="Buscar" OnClick="lnkbBuscar_Click"><div></div></asp:LinkButton>
            <asp:LinkButton ID="lnkbAvanzada" runat="server" CssClass="buscar-button-avanzado" OnClientClick="mostrarAvanzado(); return false;" ToolTip="Avanzados"><div></div></asp:LinkButton>
            <asp:LinkButton ID="lnkbLimpiar" runat="server" CssClass="buscar-button-limpiar" ToolTip="Limpiar" OnClick="lnkbLimpiar_Click"><div></div></asp:LinkButton>
            <div class="buscar-avanzado">
                <ul>
                    <li class="buscar-avanzado-titulo">Búsqueda avanzada</li>
                    <li>
                        <table class="buscar-table">
                            <tr>
                                <td class="buscar-campos">RFC</td>
                                <td class="buscar-campos">Folio</td>
                            </tr>
                            <tr>
                                <td class="buscar-campos">
                                    <asp:TextBox ID="txtRFC" runat="server" CssClass="control-campo"></asp:TextBox>
                                </td>
                                <td class="buscar-campos">
                                    <asp:TextBox ID="txtNumero" runat="server" CssClass="control-campo"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="buscar-campos" colspan="2">
                                    <asp:RadioButton ID="rbtPeriodo_Mensual" runat="server" Text="Mensual" GroupName="Periodo" AutoPostBack="true" OnCheckedChanged="rbtPeriodo_Mensual_CheckedChanged" />
                                    <asp:RadioButton ID="rbtPeriodo_Trimestral" runat="server" Text="Trimestral" GroupName="Periodo" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtPeriodo_Trimestral_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td class="buscar-campos">Año</td>
                                <td class="buscar-campos"><asp:Label ID="lblPeriodo" runat="server" Text="Mes"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="buscar-campos">
                                    <asp:DropDownList id="ddlAnio" runat="server" Width="97%"></asp:DropDownList>
                                </td>
                                <td class="buscar-campos">
                                    <asp:DropDownList id="ddlPeriodo" runat="server" Width="97%"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="buscar-campos" colspan="2">
                                    <asp:CheckBox id="chFirmado" runat="server" Text="Análisis firmados" Checked="false" /><br />
                                    <%--<asp:CheckBox id="chNoFirmado" runat="server" Text="Análisis no firmados" Checked="true" />--%>
                                </td>   
                            </tr>
                        </table>
                    </li>
                </ul>
            </div>
            <script type="text/javascript">
                mostrarAvanzado = function () {
                    $('.buscar-avanzado').slideToggle();
                    $('#<%= lnkbAvanzada.ClientID %>').toggleClass('buscar-button-avanzado-seleccionado');
                }

                desplegarAvanzado = function (e) {
                    $('.buscar-avanzado').css({ 'display': 'block' });
                    $('#<%= lnkbAvanzada.ClientID %>').toggleClass('buscar-button-avanzado-seleccionado');

                    if (e) {
                        if ($('.buscar-avanzado').css('display') == 'block') {
                            $('.buscar-avanzado').slideToggle();
                            $('#<%= lnkbAvanzada.ClientID %>').toggleClass('buscar-button-avanzado-seleccionado');
                        }
                    }
                };
            </script>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
