<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAgregarParametroAnalisis.ascx.cs" Inherits="SIRALAB.Controles.AnexoE.wucAgregarParametroAnalisis" %>
<style>
    body{
 
}
.popUpBackground{
   background-color:#5f5c5c;
   opacity:0.6; /* Opacidad 60% */
   width:200px;
   height:70px;
}
</style>
<asp:UpdatePanel ID="upAgregarPrametro" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbCancelar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlAgregarParametroPop" runat="server" CssClass="panel panel-default" style="width: 600px;" DefaultButton="lnkbGuardar">
            <div class="panel-heading">
                <asp:LinkButton ID="lnkbCerrar" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
                <asp:Label ID="lblTitulo" runat="server" Text="Agregar parámetro particular"></asp:Label>
            </div>
            <div class="panel-body">
                <ul style="box-sizing: border-box; margin: 0; padding: 10px 10px 0 10px;">
                    <li>
                        <table class="table-form">
                            <tr>
                                <td style="width: 30%;">Laboratorio</td>
                                <td style="width: 20%;">Método</td>
                                <td>Parámetro</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="control-select"/>
                                </td>
                                <td><asp:DropDownList ID="ddlMetodo" runat="server" CssClass="control-select"/></td>
                                <td><asp:TextBox ID="txtParametro" runat="server" CssClass="control-campo"></asp:TextBox></td>
                            </tr>
                        </table>
                    </li>
                </ul>
            </div>
            <div class="pop-contenido">
                <ul style="box-sizing: border-box; margin: 0; padding: 10px 10px 0 10px;">
                    <li class="popform-buttons">
                        <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="lnkBuscar_OnClick"></asp:LinkButton>
                    </li>
                </ul>
                <hr/>
            </div>
            <div class="pop-contenido">
                <ul style="box-sizing: border-box; margin: 0; padding: 10px;">
                    <li>
                        <div style="height: 250px; overflow-x: none; overflow-y: auto;">
                            <asp:Repeater ID="rptParametros" runat="server" ItemType="SIRALAB_DAL.Parametro" OnItemDataBound="rptParametros_OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="tabla-busqueda">
                                    <tr>
                                        <th><asp:CheckBox ID="chkTodos" runat="server" ToolTip="Seleccionar todos" data-parametros-todos /></th>
                                        <th>Parámetro</th>
                                        <th>Escala decimal</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:CheckBox ID="chkParametro" runat="server" data-parametro-id="<%#:Item.ParametroID %>"/></td>
                                        <td><%#:Item.Nombre %></td>
                                        <td><asp:TextBox ID="txtEscalaDecimal" runat="server" disabled="disabled" data-parametro-id="<%#:Item.ParametroID %>"></asp:TextBox></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="pop-contenido">
                <ul style="box-sizing: border-box; margin: 0; padding: 10px;">
                    <li class="popform-buttons">
                        <asp:LinkButton ID="lnkbGuardar" runat="server" CssClass="boton" Text="Aceptar" OnClick="lnkbGuardar_Click"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbCancelar" runat="server" CssClass="boton" Text="Cancelar" OnClick="lnkbCancelar_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </asp:Panel>
        
        <asp:ModalPopupExtender ID="mpeAgregarPrametro" runat="server" PopupControlID="pnlAgregarParametroPop" TargetControlID="hf" BackgroundCssClass="popUpBackground"></asp:ModalPopupExtender>
        <asp:HiddenField ID="hf" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitializeParametros);
    function InitializeParametros() {
        $("span[data-parametros-todos] > input[type='checkbox']").unbind("click").click(function() {
            $("span[data-parametro-id] > input[type='checkbox']").prop("checked", $(this).prop("checked"));

            if ($(this).prop("checked"))
                $("input[type='text'][data-parametro-id]").removeAttr("disabled");
            else
                $("input[type='text'][data-parametro-id]").attr("disabled", "disabled");
        });
        $("span[data-parametro-id] > input[type='checkbox']").unbind("click").click(function () {
            $("span[data-parametro-id] > input[type='checkbox']").each(function (id, item) {
                $("span[data-parametros-todos] > input[type='checkbox']").prop("checked", true);
                if ($(item).prop("checked") === false) {
                    $("span[data-parametros-todos] > input[type='checkbox']").prop("checked", false);
                    return false;
                }
            });
            var parametroId = $(this).closest("span").attr("data-parametro-id");

            if ($(this).prop("checked")) {
                $("input[type='text'][data-parametro-id='" + parametroId + "']").removeAttr("disabled");
            } else {
                $("input[type='text'][data-parametro-id='" + parametroId + "']").attr("disabled", "disabled");
            }
        });
    }

    $(document).ready(function() {
        InitializeParametros();
    });
</script>