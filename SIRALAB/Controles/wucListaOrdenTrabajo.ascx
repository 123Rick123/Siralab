<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucListaOrdenTrabajo.ascx.cs" Inherits="SIRALAB.Controles.wucListaOrdenTrabajo" %>

<asp:UpdatePanel ID="upOrdenTrabajo" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional" ClientIDMode="AutoID">
    <ContentTemplate>
        <asp:HiddenField ID="hfSelectedOrdenTrabajo" runat="server" />
        <asp:HiddenField ID="hfSelectedAnalisis" runat="server" />
        <asp:Repeater ID="rptOrdenTrabajo" runat="server" ItemType="SIRALAB.Controles.OrdenTrabajoLista" OnItemDataBound="rptOrdenTrabajo_OnItemDataBound">
            <HeaderTemplate>
                <table class="table table-condensed table-striped table-sm text-center">
                    <tr>
                        <th style="width: 20px;"></th>
                        <th id="thChkTodos" runat="server" visible="False" style="width: 30px; text-align: center;">
                            <asp:CheckBox ID="chkOrdenTodos" runat="server" data-check-all-orden /></th>
                        <th style="text-align: center;">Número de informe</th>
                        <th style="text-align: center;">Laboratorio</th>
                        <th style="text-align: center;">Título</th>
                        <th style="width: 50px; text-align: center;">Anexo</th>
                        <th style="text-align: center;">RFC del cliente</th>
                        <th style="text-align: center;">Norma</th>
                        <th style="text-align: center;">Fecha de emisión</th>
                        <th style="width: 50px; text-align: center;">Acción</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td id="tdAnalisisExtra" style="text-align: center;">
                        <asp:LinkButton runat="server" ID="lnkExpandir" CssClass="btn btn-default btn-xs" ToolTip="Expandir / Contraer" Visible='<%#Item.Analisis.Count > 0 %>' OnClientClick="return false;" data-id="<%#:Item.OrdenTrabajoId%>">
                            <span class="glyphicon glyphicon-plus"></span>
                        </asp:LinkButton>
                    </td>
                    <td id="tdChkTodos" runat="server" visible="False" style="text-align: center;" >
                        <asp:CheckBox ID="chkSeleccionar" runat="server" data-id="<%#:Item.OrdenTrabajoId%>" data-check-orden /></td>
                    <td style="text-align: center;"><%#:Item.Folio %></td>
                    <td><%#:Item.NombreLaboratorio %></td>
                    <td><%#:Item.Titulo %></td>
                    <td style="text-align: center;"><%#:Item.Anexo %></td>
                    <td style="text-align: center;"><%#:Item.RFCCliente %></td>
                    <td style="text-align: center;"><%#:Item.Norma %></td>
                    <td style="text-align: center;"><%#:Item.Registro %></td>
                    <td style="text-align: center;">
                        <asp:LinkButton ID="lnkbtnSeleccionar" runat="server" ToolTip="Editar" CssClass=" btn btn-default btn-xs"  CommandArgument="<%#:Item.OrdenTrabajoId %>" OnClick="lnkbtnSeleccionar_Click">
                            <span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                        <%--<asp:LinkButton ID="lnkbtnBorrar" runat="server" ToolTip="Eliminar" CssClass="botonEliminar" Style="display: none;" CommandArgument="<%#:Item.OrdenTrabajoId %>" OnClick="lnkbtnBorrar_Click"></asp:LinkButton>--%>
                    </td>
                </tr>
                <tr id="trAnalisis" runat="server" visible="False" data-id="<%#:Item.OrdenTrabajoId%>">
                    <td colspan="2"></td>
                    <td colspan="8">
                        <div>
                            <asp:Repeater ID="rptAnalisis" runat="server" ItemType="SIRALAB.Controles.AnalisisLista">
                                <HeaderTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <th id="thChkATodos" runat="server" visible="False" style="width: 30px; text-align: center;">
                                                <asp:CheckBox ID="chkAnalisisTodos" runat="server" data-check-all-analisis />
                                            </th>
                                            <th style="text-align: center;">Firmado</th>
                                            <th style="text-align: center;">Completo</th>
                                            <th style="text-align: center;">En proceso de firma</th>
                                            <th style="text-align: center;">Análisis</th>
                                            <th style="text-align: center;">Fecha conformación</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td id="tdChkATodos" runat="server" visible="False" style="text-align: center;">
                                            <asp:CheckBox ID="chkAnalisis" runat="server" data-check-analisis Checked='<%#Item.Firmado_%>' Enabled='<%#!Item.Firmado_%>' Visible='<%#Item.FirmaVisible_ && !Item.Firmado_ && (Item.AnalisisEncolado_ != true)%>' data-id='<%#Item.AnaisisID %>' />
                                            <asp:HiddenField ID="hfAnalisisID" runat="server" Value='<%#Item.AnaisisID_%>' />
                                        </td>
                                        <td style="text-align: center;"><b><%#Item.Firmado_ ? "Si" : "No"%></b></td>
                                        <td style="text-align: center;"><b><%#Item.FirmaVisible_ || Item.Firmado_ ? "Sí" : "No"%></b></td>
                                        <td style="text-align: center;"><b><%#Item.AnalisisEncolado_== true ? "Si" : "No"%></b></td>
                                        <td style="text-align: center;"><%#Item.Consecutivo_%></td>
                                        <td style="text-align: center;"><%#Item.FechaRegistro_%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hfOrdenTrabajo" runat="server" />

<script type="text/javascript">

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SelectAllCheckBox);
    Sys.Application.add_init(function () { SelectAllCheckBox(); });

    function SelectAllCheckBox() {
        $("span[data-check-all-orden] > input[type='checkbox']").click(function () {
            var bool = $(this).prop("checked");
            $("span[data-check-orden] input[type='checkbox']").prop("checked", $(this).prop("checked"));

            if (!bool)
                $("a[delete-action]").css("display", "none");

            $('table tr').each(function (i, row) {
                var $actualrow = $(row);
                var $checkbox = $actualrow.find('span[data-check-orden] input:checked').prop("checked");
                if ($checkbox)
                    $("a[delete-action]").css("display", "inline-block");
            });
        });
        $("span[data-check-all-analisis] > input[type='checkbox']").click(function () {
            var bool = $(this).prop("checked");
            var table = $(this).closest("table");
            $("tr > td > span[data-check-analisis] input[type='checkbox']", table).prop("checked", $(this).prop("checked"));

            if (!bool)
                $("a[sign-action]").css("display", "none");

            $('table tr').each(function (i, row) {
                var $actualrow = $(row);
                var $checkbox = $actualrow.find('span[data-check-analisis] input:checked').prop("checked");
                if ($checkbox)
                    $("a[sign-action]").css("display", "inline-block");
            });

        });

        $("a[delete-action]").css("display", "none");
        $("a[sign-action]").css("display", "none");
        CollapseAll();

        $("a[data-id]").click(function () {
            var anchor = $(this);
            var trId = $(this).attr("data-id");

            $("tr[data-id='" + trId + "'] td > div").stop().slideToggle("fast", function () {
                if ($("tr[data-id='" + trId + "'] td")[0].style.padding === "5px") {
                    $("tr[data-id='" + trId + "'] td").css("padding", "0");
                    $("tr[data-id='" + trId + "'] td").css("border", "none");
                    $(anchor).children('span').removeClass("glyphicon glyphicon-minus").addClass("glyphicon glyphicon-plus");
                }
                else {
                    $("tr[data-id='" + trId + "'] td").css("padding", "5px");
                    $("tr[data-id='" + trId + "'] td").css("border", "1px solid #e2e2e2");
                    $(anchor).children('span').removeClass("glyphicon glyphicon-plus").addClass("glyphicon glyphicon-minus");
                }
            });
        });

        $("span[data-check-orden] input[type='checkbox']").click(function () {
            var chkAll = $("tr th span[data-check-all-orden] input[type='checkbox']", $(this).closest("table"));
            $("span[data-check-orden] input[type='checkbox']").each(function (id, item) {
                $("a[delete-action]").css("display", "none");
                if ($(item).prop("checked") === true) {
                    $("a[delete-action]").css("display", "inline-block");
                    return false;
                }
            });

            $("span[data-check-orden] input[type='checkbox']", $(this).closest("table")).each(function (id, item) {
                $(chkAll).prop("checked", true);
                if ($(item).prop("checked") === false) {
                    $(chkAll).prop("checked", false);
                    return false;
                }
            });
        });

        $("span[data-check-analisis] input[type='checkbox']").click(function () {
            var chkAll = $("tr th span[data-check-all-analisis] input[type='checkbox']", $(this).closest("table"));
            $("span[data-check-analisis] input[type='checkbox']").each(function (id, item) {

                $("a[sign-action]").css("display", "none");
                if ($(item).prop("checked") === true) {
                    $("a[sign-action]").css("display", "inline-block");
                    return false;
                }
            });

            $("span[data-check-analisis] input[type='checkbox']", $(this).closest("table")).each(function (id, item) {
                $(chkAll).prop("checked", true);
                if ($(item).prop("checked") === false) {
                    $(chkAll).prop("checked", false);
                    return false;
                }
            });
        });
    }

    function CollapseAll() {
        $("tr[data-id] td > div").css("display", "none");
        $("tr[data-id] td").css("padding", "0");
        $("tr[data-id] td").css("border", "none");
    }
</script>
