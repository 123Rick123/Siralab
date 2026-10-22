<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFiltro.ascx.cs" Inherits="SIRALAB.Controles.wucFiltro" %>


<link href="style/master.css" rel="stylesheet" />
<link href="style/busqueda.laboratorio.css" rel="stylesheet" />
<asp:UpdatePanel ID="upBusqueda" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" class="buscar">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbLimpiar" EventName="Click" />
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
                                <td class="buscar-campos" colspan="2">
                                    &nbsp;</td>
                            </tr>
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
                        </table>                        
                        <table class="buscar-table">                            
                            <tr>
                                <td class="buscar-campos">Fecha Inicial</td>
                                <td class="buscar-campos">Fecha Final</td>
                            </tr>
                            <tr>
                                <td class="buscar-campos">
                                    <asp:TextBox ID="txtFechaInicial" runat="server" CssClass="control-campo"></asp:TextBox>
                                    <asp:CalendarExtender ID="ce_txtFechaInicial" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFechaInicial" PopupPosition="TopLeft"></asp:CalendarExtender>
                                </td>
                                <td class="buscar-campos">
                                    <asp:TextBox ID="txtFechaFinal" runat="server" CssClass="control-campo"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFechaFinal" PopupPosition="TopLeft"></asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                        <table class="buscar-table">                            
                            <tr>
                                <td class="buscar-campos">Análisis Firmados <asp:CheckBox id="chFirmado" runat="server" Checked="true" /></td>
                                <td class="buscar-campos">Análisis No Firmados <asp:CheckBox id="chNoFirmado" runat="server" Checked="true" /></td> 
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

                desplegarAvanzado = function () {
                    $('.buscar-avanzado').css({ 'display': 'block' });
                    $('#<%= lnkbAvanzada.ClientID %>').toggleClass('buscar-button-avanzado-seleccionado');
            };

            dateSelectionChanged = function (sender, args) {
                selectedDate = sender.get_selectedDate();
            }
            </script>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>