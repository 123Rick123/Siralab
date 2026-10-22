<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMetodoDeImpugnacionApercibimiento.ascx.cs" Inherits="SIRALAB.Controles.wucMetodoDeImpugnacionApercibimiento" %>
<asp:Panel ID="PanelFormulario" runat="server" Visible="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <%--         <asp:PostBackTrigger ControlID="lnkbtnValidarCampos"  />--%>
            <asp:AsyncPostBackTrigger ControlID="lnkbtnValidarCampos" EventName="Click" />
            <%-- <asp:PostBackTrigger ControlID="btnDescarga" />--%>
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-6">
                    <label for="name">Medio de impugnación*:</label>
                    <asp:DropDownList ID="ddlMedioImpugnacion" AutoPostBack="true" OnSelectedIndexChanged="ddlMedioImpugnacion_SelectedIndexChanged" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc" runat="server"></asp:DropDownList>
                    <div class="form-text MensajeError" id="MensajeErrorMedioImpugnacion" runat="server" style="color: #D0021B"></div>
                </div>
                <div class="col-md-6">
                    <label for="name">Fecha de impugnación*:</label>

                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtFechaImpugnacion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" />

                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                        <div class="form-text MensajeError" id="MensajeErrorFechaImpugnacion" runat="server" style="color: #D0021B"></div>
                        <asp:RegularExpressionValidator ID="RExprFechaImpugnacion"
                            ControlToValidate="txtFechaImpugnacion"
                            ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                            Display="Static"
                            ErrorMessage="La fecha de impugnación no tiene el formato correcto."
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <label class="control-label">Ante quien*:</label>
                    <asp:TextBox ID="txtAnteQuien" placeholder="Ante quien" CssClass="form-control input-sm" MaxLength="50" AutoCompleteType="None" autocomplete="cc-csc" runat="server" />
                    <div class="form-text MensajeError" id="MensajeErrorAnteQuien" runat="server" style="color: #D0021B"></div>
                </div>
                <div class="col-md-6">
                    <label for="name">Estatus*:</label>
                    <asp:DropDownList ID="ddlEstatus" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged" AutoPostBack="true" autocompletetype="None" autocomplete="cc-csc" runat="server"></asp:DropDownList>
                    <div class="form-text MensajeError" id="MensajeErrorEstatus" runat="server" style="color: #D0021B"></div>
                </div>
                
            </div>

            <div class="row">
                 <div class="col-md-6">
                    <br />
                    <label for="name">Número de expediente*:</label>
                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtNoExpediente" placeholder="Número de expediente" CssClass="form-control input-sm" AutoCompleteType="None" MaxLength="50" autocomplete="cc-csc" runat="server" />
                        <div class="form-text MensajeError" id="MensajeErrorNoExpediente" runat="server" style="color: #D0021B"></div>
                    </div>
                </div>
                <br />
                 <div class="col-md-6">
                    <label id="lblFechaResolucion" runat="server" for="name" visible="false">Fecha de resolución*:</label>
                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtFechaResolucion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" visible="false"/>

                        <span class="glyphicon glyphicon-calendar" id="spanCalendarioFechaRes" runat="server" aria-hidden="true" visible="false"></span>
                        <div class="form-text MensajeError" id="MensajeErrorFechaResolucion" runat="server" style="color: #D0021B"></div>
                        <asp:RegularExpressionValidator ID="RExprFechaResolucion"
                            ControlToValidate="txtFechaResolucion"
                            ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                            Display="Static"
                            ErrorMessage="La fecha de resolución no tiene el formato correcto."
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label>Observaciones*:</label>
                <textarea class="form-control" rows="5" id="txtObservaciones" maxlength="500" runat="server"></textarea>
                <div class="form-text MensajeError" id="MensajeErrorObservaciones" runat="server" style="color: #D0021B"></div>

            </div>
            <div class="row">
                <div class="col-md-6" style="text-align: left;">
                    <%--<asp:Button ID="btnDescarga" Style="display: none" runat="server" CssClass="btn btn-primary" OnClick="Descarga_Click" Text="Descargar reincidencia" />--%>

                   
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnCerrarPagina" runat="server" CssClass="btn btn-default" OnClick="btnCerrarPagina_Click" Text="Cerrar" />

                    <asp:Button ID="lnkbtnValidarCampos" runat="server" CssClass="btn btn-primary" OnClick="lnkbtnValidarCampos_Click" Text="Guardar" />
                    <%--<button type="button" class="btn btn-primary" onclick="ValidarCampos()">Guardar</button>--%>
                </div>
            </div>

            <%-- controles hidden--%>
            <input runat="server" type="text" id="txtidApercibimiento" hidden />
            <input runat="server" type="text" id="txtidReincidencia" hidden />
            <input runat="server" type="text" id="txtBanderaEsConsulta" hidden />


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


<%--Modal confirm--%>
<div id="modalConfirm" class="modal fade" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="upModalConfirm" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h4 class="modal-title">
                            <label id="tituloConfirm"></label>
                        </h4>
                    </div>
                    <div class="modal-body text-center">
                        <p id="cuerpoConfirm"></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        <button id="btnAceptarModalConfirm" type="button" class="btn btn-primary" onclick="GuardarImpugnacion()">Aceptar</button>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<!-- /.modal -->


<%-- @*Alerta Success*@--%>
<div class="modal fade" id="alertSuccess" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="align-self: center;">
                <h4 class="modal-title">Exitoso</h4>
            </div>
            <div class="modal-body">
                <p id="TextoSuccess" style="text-align: center;"></p>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-primary" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->

<%-- @*Alerta Error*@--%>
<div class="modal fade" id="alertError" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="align-self: center;">
                <h4 class="modal-title">Error</h4>
            </div>
            <div class="modal-body">
                <p id="TextoError" style="text-align: center;"></p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->

<%--  AlertaSimple--%>
<div class="modal fade" id="alertaSimple" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header" style="align-self: center;">
                        <h4 class="modal-title">Alerta</h4>
                    </div>
                    <div class="modal-body">
                        <p runat="server" id="TextoAlertaSimple" style="text-align: center;"></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Aceptar</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- /.modal-dialog -->
</div>


<script type="text/javascript">
    $(document).ready(function () {
        Calendario();

        <%--$("#<%=idDocumentoEdita.ClientID%>").attr('href', "wucBusquedaAltaApercibidos.ascx/DescargarArchivo?id=" + '101c825f-ad23-4612-9bc4-d6993389443f.1');--%>
    });

    function Calendario() {
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
        $("#<%=txtFechaImpugnacion.ClientID%>").datepicker();
        $("#<%=txtFechaResolucion.ClientID%>").datepicker(); 

    }

    function MostrarModalConfirm() {
        $("#modalConfirm").modal("show");
        var texto = document.getElementById("cuerpoConfirm");
        texto.innerHTML = "¿Está seguro que desea guardar la información?";
    }

    function GuardarImpugnacion() {

        var MedioImpugnacion = document.getElementById('<%= ddlMedioImpugnacion.ClientID %>').value;
        var FechaImpugnacion = document.getElementById('<%= txtFechaImpugnacion.ClientID %>').value;
        var Expediente= document.getElementById('<%= txtNoExpediente.ClientID %>').value;
        var AnteQuien = document.getElementById('<%= txtAnteQuien.ClientID %>').value;
        var Estatus = document.getElementById('<%= ddlEstatus.ClientID %>').value;
        var Observaciones = document.getElementById('<%= txtObservaciones.ClientID %>').value;
        var idApercibimiento = document.getElementById('<%= txtidApercibimiento.ClientID%>').value;
        var idReincidencia = document.getElementById('<%= txtidReincidencia.ClientID%>').value;
        if (idApercibimiento == "") {
            idApercibimiento = 0;
        }
        if (idReincidencia == "") {
            idReincidencia = 0;
        }
        var FechaResolucion = '';
        if (Estatus!='1') {
             FechaResolucion = document.getElementById('<%= txtFechaResolucion.ClientID %>').value;
        }

        //Para actualizacion
        var BanderaEsConsulta = document.getElementById('<%= txtBanderaEsConsulta.ClientID %>').value;
        if (BanderaEsConsulta == "true") {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ImpugnacionApercibimiento.aspx/ActualizarImpugnacion",
                dataType: "json",
                data: JSON.stringify({ MedioImpugnacion: MedioImpugnacion, FechaImpugnacion: FechaImpugnacion, AnteQuien: AnteQuien, Estatus: Estatus, Observaciones: Observaciones, idApercibimiento: idApercibimiento, idReincidencia: idReincidencia, FechaResolucion: FechaResolucion, Expediente: Expediente }),
                success: function (data, textStatus) {
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();

                    $("#modalConfirm").modal("hide");
                    if (data.d.Message.includes('Error') || data.d.Message.includes('error')) {
                        $("#alertError").modal("show");
                        var texto = document.getElementById("TextoError");
                        texto.innerHTML = data.d.Message;
                    }
                    else {
                        limpiarCampos();
                        $("#alertSuccess").modal("show");
                        var texto = document.getElementById("TextoSuccess");
                        texto.innerHTML = data.d.Message;
                    }


                },
                error: function (data, textStatus) {
                    $("#alertError").modal("show");
                    var texto = document.getElementById("TextoError");
                    texto.innerHTML = "Ocurrió un error al guardar.";


                }
            });
        }
        else {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ImpugnacionApercibimiento.aspx/GuardarImpugnacion",
                dataType: "json",
                data: JSON.stringify({ MedioImpugnacion: MedioImpugnacion, FechaImpugnacion: FechaImpugnacion, AnteQuien: AnteQuien, Estatus: Estatus, Observaciones: Observaciones, idApercibimiento: idApercibimiento, idReincidencia: idReincidencia, FechaResolucion: FechaResolucion, Expediente: Expediente }),
                success: function (data, textStatus) {
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();

                    $("#modalConfirm").modal("hide");
                    if (data.d.Message.includes('Error') || data.d.Message.includes('error')) {
                        $("#alertError").modal("show");
                        var texto = document.getElementById("TextoError");
                        texto.innerHTML = data.d.Message;
                    }
                    else {
                        limpiarCampos();
                        $("#alertSuccess").modal("show");
                        var texto = document.getElementById("TextoSuccess");
                        texto.innerHTML = data.d.Message;
                    }


                },
                error: function (data, textStatus) {
                    $("#alertError").modal("show");
                    var texto = document.getElementById("TextoError");
                    texto.innerHTML = "Ocurrió un error al guardar.";


                }
            });
        }
    }

    function limpiarCampos() {
        document.getElementById('<%= ddlMedioImpugnacion.ClientID %>').value = '0';
        document.getElementById('<%= txtFechaImpugnacion.ClientID %>').value = '';
        document.getElementById('<%= txtAnteQuien.ClientID %>').value = '';
    
        document.getElementById('<%= txtObservaciones.ClientID %>').value = '';
        document.getElementById('<%= lnkbtnValidarCampos.ClientID %>').style.display = "none";
        document.getElementById('<%= txtNoExpediente.ClientID %>').value = '';

        var estatus = document.getElementById('<%= ddlEstatus.ClientID %>').value;

        if (estatus!='1') {
            document.getElementById('<%= txtFechaResolucion.ClientID %>').value = '';
            document.getElementById('<%= txtFechaResolucion.ClientID %>').style.visibility = 'hidden';
            document.getElementById('<%= lblFechaResolucion.ClientID %>').style.visibility = 'hidden';
            document.getElementById('<%= spanCalendarioFechaRes.ClientID %>').style.visibility = 'hidden';
        }
        document.getElementById('<%= ddlEstatus.ClientID %>').value = '0';
        
    }


    
</script>
