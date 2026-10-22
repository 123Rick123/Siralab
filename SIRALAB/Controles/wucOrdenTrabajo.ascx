<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucOrdenTrabajo.ascx.cs" Inherits="SIRALAB.Controles.wucOrdenTrabajo" %>

<script>
    function toDate(dateStr) {
        var parts = dateStr.split("/")
        return new Date(parts[2], parts[1] - 1, parts[0]);
    }
    $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            $.datepicker.regional.es = {
                closeText: 'Cerrar',
                prevText: 'Ant',
                nextText: 'Sig',
                currentText: 'Hoy',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                dayNamesMin: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'S&aacute;b'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            $.datepicker.setDefaults($.datepicker.regional.es);

            var fechaInicio = null;
            var fechaFin = null;

            if ($('[id*=startTxtOrdenFechaEmision]').length && $('[id*=startTxtOrdenFechaEmision]').val().length > 0) {
                fechaInicio = toDate($('[id*=startTxtOrdenFechaEmision]').val());
            }
            if ($('[id*=endTxtOrdenFechaEmision]').length && $('[id*=endTxtOrdenFechaEmision]').val().length > 0) {
                fechaFin = toDate($('[id*=endTxtOrdenFechaEmision]').val());
            }

            $('[id*=txtFecha]').datepicker();
            $('[id*=txtOrdenFechaEmision]').datepicker({
                minDate: fechaInicio,
                maxDate: fechaFin,
                autoclose: true
            });

            $('[data-toggle="tooltip"]').tooltip();
        }
    });
</script>

<div id="div_wucCliente" runat="server" class="div-mode">
    <asp:Panel ID="pnlDatosOrden" runat="server">
        <asp:Panel ID="pnlOrden" runat="server">
            <asp:UpdatePanel ID="updOrdenTrabajo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkbtnOrdenGuardar" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="startTxtOrdenFechaEmision" Value="" />
                    <asp:HiddenField runat="server" ID="endTxtOrdenFechaEmision" Value="" />
                    <div class="panel-body">
                        <div class="row form-group">
                            <div class="col-md-3">
                                <label for="name" class="control-label">Norma oficial mexicana <span>*</span>:</label>
                                <asp:DropDownList ID="ddlNorma" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlNorma_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfv_ddlNorma" runat="server" InitialValue="0" ControlToValidate="ddlNorma" ValidationGroup="vgOrdenTrabajo"
                                    CssClass="text-danger" ErrorMessage="Campo requerido" Display="Dynamic" Text="Este campo es obligatorio "></asp:RequiredFieldValidator>
                            </div>
                            <div runat="server" id="divCuerpoReceptor" class="col-md-3">
                                <label for="name" class="control-label">Cuerpo receptor <span>*</span>:</label>
                                <asp:DropDownList ID="ddlCuerpoReceptor" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlCuerpoReceptor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfv_ddlCuerpoReceptor" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlCuerpoReceptor"
                                    CssClass="text-danger" ErrorMessage="Campo requerido" Display="Dynamic" Text="Este campo es obligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </div>
                            <div runat="server" id="divUsoAgua" class="col-md-6">
                                <label for="name" class="control-label">Uso de agua (tipo de cuerpo receptor según LFD).<span>*</span>:</label>
                                <asp:DropDownList ID="ddlUsoAgua" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfv_ddlUsoAgua" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlUsoAgua"
                                    CssClass="text-danger" ErrorMessage="Campo requerido" Display="Dynamic" Text="Este campo es obligatorio "></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4">
                                <label for="name">Número de informe de prueba <span class="control-label">*</span>:</label>
                                <asp:TextBox ID="txtOrdenNumero" runat="server" CssClass="form-control input-sm" placeholder="Ingresa tu número de informe de prueba" autocomplete="cc-csc" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_txtOrdenNumero" runat="server" ControlToValidate="txtOrdenNumero" ErrorMessage="Campo requerido" Display="Dynamic" ValidationGroup="vgOrdenTrabajo"
                                    Text="Este campo es obligatorio " CssClass="text-danger">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div runat="server" id="divHorasDescarga" class="col-md-4">
                                <label for="name">Horas del proceso de descarga<span class="control-label">*</span>:</label>
                                <asp:TextBox ID="txtOrdenProcesoHoras" runat="server" aria-required="true" CssClass="form-control input-sm" placeholder="Ingresa tu horas del proceso de descarga" autocomplete="cc-csc"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_txtOrdenProcesoHoras" runat="server" ControlToValidate="txtOrdenProcesoHoras" ErrorMessage="Campo requerido" Display="Dynamic"
                                    ValidationGroup="vgOrdenTrabajo" CssClass="text-danger" SetFocusOnError="true" Text="Este campo es obligatorio"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rv_txtOrdenProcesoHoras" runat="server" MinimumValue="1" MaximumValue="24" Type="Integer" ValidationGroup="vgOrdenTrabajo"
                                    ControlToValidate="txtOrdenProcesoHoras" CssClass="text-danger" Display="Dynamic" ErrorMessage="Error de captura, solo se puede capturar números dentro del rango de 1 a 24."></asp:RangeValidator>

                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="name">Fecha de emisión<span class="form-text"></span>:</label>

                                    <div class="form-group datepicker-group">
                                        <asp:TextBox ID="txtOrdenFechaEmision" runat="server" CssClass="form-control input-sm fechaEmision" autocomplete="cc-csc" OnClick="setDatePicker(this.id)"></asp:TextBox>
                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <table style="width: 97%; margin: 0 auto;">
                        <tr>
                            <td style="width: 34%;">
                                <asp:Label ID="lblNormaOM" runat="server" Text=""></asp:Label></td>
                            <td style="width: 33%;">
                                <asp:Label ID="lblCReceptor" runat="server" Text=""></asp:Label></td>
                            <td style="width: 33%;">
                                <asp:Label ID="lblUAgua" runat="server" Text=""></asp:Label></td>
                        </tr>
                    </table>
                    <div style="text-align: right;">
                        <asp:LinkButton ID="lnkbtnOrdenGuardar" runat="server" Style="margin: 10px 20px 0 0;" OnClick="lnkbtnOrdenGuardar_Click" ToolTip="Guardar orden de trabajo" ValidationGroup="vgOrdenTrabajo" CssClass="btn btn-default btn-md">
                             <span class="glyphicon glyphicon-floppy-disk"></span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Panel ID="pnlOrdenConsulta" runat="server" Visible="false">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="panel-body">
                        <div class="row">
                            <div runat="server" id="divNOM" class="col-md-4">
                                <label for="name">Norma oficial mexicana <span class="form-text"></span>:</label>
                                <asp:Label ID="lblInfoNM" runat="server"></asp:Label></td>
                                <asp:Label ID="lblNormaMexicana" runat="server" CssClass="form-text" Style="text-align: center"></asp:Label>

                            </div>
                            <div runat="server" id="divCuerpoReceptorNO" class="col-md-4">
                                <label for="name">Cuerpo receptor <span class="form-text"></span>:</label>
                                <asp:Label ID="lblCuerpoReceptor" runat="server"></asp:Label>
                                <asp:Label ID="DropDownList2" runat="server" CssClass="form-text" OnSelectedIndexChanged="ddlCuerpoReceptor_SelectedIndexChanged" AutoPostBack="true"></asp:Label>

                            </div>
                            <div runat="server" id="divUsoAguaNO" class="col-md-4">
                                <label for="name">Uso de agua<span class="form-text"></span>:</label>
                                <asp:Label ID="lblInfoUA" runat="server"></asp:Label></td>
                                <asp:Label ID="lblUsoAgua" runat="server"></asp:Label>
                                <asp:Label ID="DropDownList3" runat="server" CssClass="form-text"></asp:Label>


                            </div>
                        </div>


                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div runat="server" id="divNumPrueba" class="col-md-4">
                                <label for="name">Número de informe de prueba <span class="form-text"></span>:</label>
                                <asp:Label ID="lblOrdenNumero" runat="server" class="form-text"></asp:Label>

                            </div>
                            <div class="col-md-4" runat="server" id="divHoraProcesoDescarga">
                                <label runat="server" for="name" id="lblHorasDescarga_2">Horas del proceso de descarga<span class="form-text"></span>:</label>
                                <asp:Label runat="server"></asp:Label></td>
                               <asp:Label ID="lblOrdenProcesoHoras" runat="server" class="form-text"></asp:Label>
                            </div>
                            <div runat="server" id="divFechaEmision" class="col-md-4">
                                <label for="name">Fecha de emisión<span class="form-text"></span>:</label>
                                <asp:Label ID="lblOrdenFechaEmision" runat="server" class="form-text"></asp:Label>
                            </div>
                        </div>


                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlBoton" runat="server" Enabled="false" Visible="false">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" style="text-align: right;">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkbtnGenerarAnalisis" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:LinkButton ID="lnkbtnGenerarAnalisis" runat="server" ToolTip="Generar análisis" CssClass="btn btn-default btn-md" OnClick="lnkbtnGenerarAnalisis_Click">
                      <i class="fas fa-flask fa-lg" aria-hidden="true"></i>
                </asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
<asp:HiddenField ID="hfOrdenTrabajo" runat="server" />



<div id="modalMensajeRestriccionNom001"  class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeader">
                    <h5 class="modal-title">Mensaje del sistema</h5>
                </div>
                <div class="modal-body" id="divMensajes">
                    Usted no puede hacer sus analisis por esta norma.
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>