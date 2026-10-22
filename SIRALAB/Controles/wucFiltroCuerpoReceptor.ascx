<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFiltroCuerpoReceptor.ascx.cs" Inherits="SIRALAB.Controles.wucFiltroCuerpoReceptor" %>

<link href="style/Master.css" rel="stylesheet" />
<link href="style/busqueda.cuerporeceptor.css" rel="stylesheet" />

<asp:UpdatePanel ID="upBusqueda" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" class="buscar">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbLimpiar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel runat="server" DefaultButton="lnkbBuscar">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="buscar-txt"></asp:TextBox>
            <asp:LinkButton ID="lnkbBuscar" runat="server" CssClass="buscar-button" ToolTip="Buscar" OnClick="lnkbBuscar_Click"><div></div></asp:LinkButton>
            <asp:LinkButton ID="lnkbAvanzada" runat="server" CssClass="buscar-button-avanzado" OnClientClick="mostrarAvanzado(); return false;" ToolTip="Avanzados"><div></div></asp:LinkButton>
            <asp:LinkButton ID="lnkbLimpiar" runat="server" CssClass="buscar-button-limpiar" ToolTip="Limpiar" OnClick="lnkbLimpiar_Click"><div></div></asp:LinkButton>
            <div class="buscar-avanzado">
                <ul>
                    <li class="buscar-avanzado-titulo">Búsqueda avanzada</li>
                    <li>
                        <table class="buscar-table">
                            <tr>
                                <td class="buscar-campos">Tipo cuerpo receptor</td>
                            </tr>
                            <tr>
                                <td class="buscar-campos">
                                    <asp:DropDownList runat="server" ID="ddlTipoCuerpoReceptor" CssClass="control-select"></asp:DropDownList>
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
                ocultarAvanzado = function () {
                    $('.buscar-avanzado').slideUp(100);
                    $('#<%= lnkbAvanzada.ClientID %>').removeClass('buscar-button-avanzado-seleccionado');
                    $('#<%= lnkbAvanzada.ClientID %>').addClass('buscar-button-avanzado');
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
