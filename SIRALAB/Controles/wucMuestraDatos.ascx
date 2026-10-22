<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMuestraDatos.ascx.cs" Inherits="SIRALAB.Controles.wucMuestraDatos" %>

<%@ Register Src="wucParametros.ascx" TagName="wucParametros" TagPrefix="uc1" %>


<%--<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/botones.css" rel="stylesheet" type="text/css" />--%>
<link href="style/grid.css" rel="stylesheet" type="text/css" />
<%--<link href="style/paginador.css" rel="stylesheet" />--%>
<script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
<%--<link href="style/siralab/iconos.css" rel="stylesheet" />--%>
<%--<link href="style/iconos.css" rel="stylesheet" />--%>

<script>
    function toDate(dateStr) {
        var parts = dateStr.split("/")
        return new Date(parts[2], parts[1] - 1, parts[0])
    }
    $(document).ready(function () {
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

        var fechaInicioMuestreo = null;
        var fechaFinMuestreo = null;

        if ($('[id*=startTxtOrdenFechaMuestreo]').length && $('[id*=startTxtOrdenFechaMuestreo]').val().length > 0) {
            fechaInicioMuestreo = toDate($('[id*=startTxtOrdenFechaMuestreo]').val());
        }
        if ($('[id*=endTxtOrdenFechaMuestreo]').length && $('[id*=endTxtOrdenFechaMuestreo]').val().length > 0) {
            fechaFinMuestreo = toDate($('[id*=endTxtOrdenFechaMuestreo]').val());
        }

        var fechaInicioRec = null;
        var fechaFinRec = null;

        if ($('[id*=startTxtMuestraFechaRec]').length && $('[id*=startTxtMuestraFechaRec]').val().length > 0) {
            fechaInicioRec = toDate($('[id*=startTxtMuestraFechaRec]').val());
        }
        if ($('[id*=endTxtMuestraFechaRec]').length && $('[id*=endTxtMuestraFechaRec]').val().length > 0) {
            fechaFinRec = toDate($('[id*=endTxtMuestraFechaRec]').val());
        }

        $('#<%=txtMuestraFechaMuestreo.ClientID%>').datepicker({
            minDate: fechaInicioMuestreo,
            maxDate: fechaFinMuestreo,
            autoclose: true
        });

        $('#<%=txtMuestraFechaRec.ClientID%>').datepicker({
            minDate: fechaInicioRec,
            maxDate: fechaFinRec,
            autoclose: true
        });

        $('[data-toggle="tooltip"]').tooltip();
    }

    );


    $('.botonF1').hover(function () {
        $('.btn').addClass('animacionVer');
    })
    $('.contenedor').mouseleave(function () {
        $('.btn').removeClass('animacionVer');
    })
</script>

<asp:Panel ID="pnlOrden" runat="server">
    <asp:HiddenField runat="server" ID="startTxtOrdenFechaMuestreo" Value="" />
    <asp:HiddenField runat="server" ID="endTxtOrdenFechaMuestreo" Value="" />
    <asp:HiddenField runat="server" ID="startTxtMuestraFechaRec" Value="" />
    <asp:HiddenField runat="server" ID="endTxtMuestraFechaRec" Value="" />
    <div class="div-colapsado">
        <div class="row">
            <%--panel Fecha conformación--%>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6></h6>
                        <p class="text-center">
                            <label id="lblFechaMuestreo" runat="server" for="name">
                                Fecha de muestreo<span></span>:</label>
                        </p>
                        <h6></h6>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="name" runat="server" class="control-label">Fecha:</label>

                                    <div class="form-group datepicker-group">
                                        <asp:TextBox ID="txtMuestraFechaMuestreo" runat="server" CssClass="form-control" ValidationGroup="Muestra" Style="text-align: center"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_ce_txtMuestraFechaMuestreo" runat="server" ValidationGroup="Muestra" ControlToValidate="txtMuestraFechaMuestreo"
                                            ErrorMessage="Campo requerido" Display="Dynamic"
                                            CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="name" runat="server" id="lbl" class="control-label">Hora:</label>
                                    <asp:TextBox ID="txtMuestraHora" runat="server" CssClass="form-control" Style="text-align: center;" ValidationGroup="Muestra"></asp:TextBox>
                                    <%--Inicio validador formato 24 horas--%>
                                    <asp:RegularExpressionValidator ID="rev_txtMuestraHora" runat="server" ValidationGroup="Muestra" ControlToValidate="txtMuestraHora"
                                        ErrorMessage="El Formato no esta en 24 horas (00:00 - 23:59)" Display="Dynamic" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"></asp:RegularExpressionValidator>

                                    <asp:RequiredFieldValidator ID="rfv_txtMuestraHora" runat="server" ControlToValidate="txtMuestraHora"
                                        ErrorMessage="Campo requerido" Display="Dynamic"
                                        CssClass="text-danger" Text="*Campo requerido" ValidationGroup="Muestra"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--Panel recepción en laboratorio--%>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6></h6>
                        <p class="text-center">
                            <label>
                                Fecha de ingreso al laboratorio
                            </label>
                        </p>
                        <h6></h6>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="name" runat="server" class="control-label">Fecha:</label>
                                    <div class="form-group datepicker-group">
                                        <asp:TextBox ID="txtMuestraFechaRec" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_txtMuestraFechaRec" runat="server" ValidationGroup="Muestra" ControlToValidate="txtMuestraFechaRec"
                                            ErrorMessage="Campo requerido" Display="Dynamic"
                                            CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>

                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">

                                    <label for="name" class="control-label">Hora:</label>
                                    <asp:TextBox ID="txtMuestraHoraRec" runat="server" CssClass="form-control" Width="85%" Style="text-align: center;"></asp:TextBox>
                                    <%--Inicio validador formato 24 horas--%>
                                    <asp:RegularExpressionValidator ID="rev_txtMuestraHoraRec" runat="server" ValidationGroup="Muestra" ControlToValidate="txtMuestraHoraRec"
                                        ErrorMessage="El Formato no esta en 24 horas" Display="Dynamic" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"></asp:RegularExpressionValidator>

                                    <%--Fin validador formato 24 horas--%>
                                    <asp:RequiredFieldValidator ID="rfv_txtMuestraHoraRec" runat="server" ControlToValidate="txtMuestraHoraRec"
                                        ErrorMessage="Campo requerido" Display="Dynamic"
                                        CssClass="text-danger" Text="*Campo requerido" ValidationGroup="Muestra"></asp:RequiredFieldValidator>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6></h6>
                        <p class="text-center">
                            <label>Caudal (l/s)</label>
                        </p>
                        <h6></h6>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label for="name" runat="server" id="Label4" class="control-label">Caudal (l/s)<span class="control-label">*</span>:</label>
                                    <asp:TextBox ID="txtMuestraCaudal" runat="server" CssClass="form-control" data-scale="2" Style="text-align: center"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rev_txtMuestraCaudal" runat="server" ControlToValidate="txtMuestraCaudal" ValidationGroup="Muestra" ValidationExpression="^[\d]{0,9}(\.[\d]{0,6})?$"
                                        CssClass="text-danger" ErrorMessage="Número decimal fuera del rango establecido" Display="Dynamic" />
                                    <asp:RequiredFieldValidator ID="rfv_txtMuestraCaudal" runat="server" ValidationGroup="Muestra" ControlToValidate="txtMuestraCaudal"
                                        ErrorMessage="Campo requerido" Display="Dynamic"
                                        CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6></h6>
                        <p class="text-center">
                            <label>Observaciones</label>
                        </p>
                        <h6></h6>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label for="name">Observaciones<span class="form-text"></span>:</label>
                                    <asp:TextBox ID="txtMuestraObservaciones" runat="server" CssClass="form-control"></asp:TextBox>

                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6" hidden>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h6></h6>
                    <p class="text-center">
                        Identificación
                    </p>
                    <h6></h6>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="name" class="control-label">Identificación<span class="control-label">*</span>:</label>
                                <div class="form-group datepicker-group" id="Label1">
                                    <asp:TextBox ID="txtMuestraIdentificacion" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfv_txtMuestraIdentificacion" runat="server" ControlToValidate="txtMuestraIdentificacion"
                                        ErrorMessage="Campo requerido" Display="Dynamic"
                                        CssClass="text-danger" Text="*Campo requerido" ValidationGroup="Muestra"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6" hidden>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h6></h6>
                    <p class="text-center">
                        Descripción
                    </p>
                    <h6></h6>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="name" class="control-label">Descripción<span class="control-label">*</span>:</label>
                                <asp:TextBox ID="txtMuestraDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                                <%-- <asp:RequiredFieldValidator ID="rfv_txtMuestraDescripcion" runat="server" ValidationGroup="Muestra" ControlToValidate="txtMuestraDescripcion"
                                    ErrorMessage="Campo requerido" Display="Dynamic"
                                    CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>


        <table style="width: 100%; margin: 0 auto;">
            <tr>
                <td style="height: 40px; text-align: left;">
                    <asp:LinkButton ID="lnkbtnBackAnalisi" runat="server" OnClick="lnkbtnBackAnalisi_Click" CausesValidation="false" ToolTip="Regresar a las muestras" CssClass="btn btn-default btn-md">
                    <span class="glyphicon glyphicon-chevron-left"></span>
                    </asp:LinkButton></td>
                <td style="text-align: right;">
                    <asp:LinkButton ID="lnkbtnRecalcular" runat="server" OnClick="lnkbtnRecalcular_Click" ToolTip="Recalcular" ValidationGroup="Muestra" CssClass="btn btn-default btn-md" Visible="false">
                     <span class="glyphicon glyphicon-repeat"></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnOrdenGuardar" runat="server" OnClick="lnkbtnOrdenGuardar_Click" ValidationGroup="Muestra" ToolTip="Guardar" CssClass="btn btn-default btn-md">
                     <span class="glyphicon glyphicon-floppy-disk"></span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <%--Nuevo campo agregado para SST, DQO/COT RGG--%>
        <div class="row" runat="server" id="divDatosDQOCOT" visible="true">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6></h6>
                        <p class="text-left">
                            <label>DATOS DQO COT</label>
                        </p>
                        <h6></h6>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:CheckBox ID="chkCOT" runat="server" CssClass="text" AutoPostBack="true" Text="Los Cloruros son mayor o igual a 1000 mg/L" OnCheckedChanged="chkCOT_CheckedChanged"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkDQO" runat="server" CssClass="text" AutoPostBack="true" Text="Los Cloruros son menor a 1000 mg/L" OnCheckedChanged="chkDQO_CheckedChanged"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<div class="row">
    <div class="col-md-12">
        <div class="panel-body">
            <div class="table-responsive">
                <uc1:wucParametros ID="wucParametros1" runat="server" OnDatosFaltantes="wucParametros1_DatosFaltantes" OnGuardado="wucParametros1_Guardado" OnGuardadoSimple="wucParametros1_GuardadoSimple" />
                <div style="display: none;">
                    <asp:HiddenField ID="hfOrdenTrabajo" runat="server" />
                    <asp:HiddenField ID="hfNivel" runat="server" />
                    <asp:HiddenField ID="hfDQOCOT" runat="server" />
                </div>
            </div>
        </div>
    </div>
</div>
