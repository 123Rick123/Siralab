<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAltaReincidencia.ascx.cs" Inherits="SIRALAB.Controles.wucAltaReincidencia" %>


<%--panel formulario alta--%>
<asp:Panel ID="PanelFormulario" runat="server" Visible="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <%--         <asp:PostBackTrigger ControlID="lnkbtnValidarCampos"  />--%>
            <asp:AsyncPostBackTrigger ControlID="lnkbtnValidarCampos" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnDescarga" />
        </Triggers>
        <ContentTemplate>

            <div class="col-sm-12" id="esElGrid">
                <asp:GridView ID="grdLaboratorio" runat="server" DataKeyNames="idLaboratorio"
                    AutoGenerateColumns="False"
                    EmptyDataText="No hay resultados para la búsqueda solicitada."
                    AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True"
                    Width="97%" CssClass="table table-striped" CellSpacing="2" BorderWidth="1px">
                    <Columns>

                        <asp:BoundField DataField="idLaboratorio" ShowHeader="true" HeaderText="id" Visible="False" />
                        <asp:BoundField DataField="RFCLaboratorio" ShowHeader="true" HeaderText="RFC" />
                        <asp:BoundField DataField="NombreLaboratorio" ShowHeader="true" HeaderText="Nombre de laboratorio" />


                       <asp:TemplateField HeaderText="Números y vigencias de aprobación">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDEtalle" runat="server" Text="Ver" AutoPostBack="True" OnClick="lnkDEtalle_Click"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <input runat="server" type="text" id="txtIdApercibimiento" hidden />
                <input runat="server" type="text" id="txtBanderaEsConsulta" hidden />
            </div>


            <div class="row">
                <div class="col-md-6">
                    <label class="control-label">Número de oficio de la resolución*:</label>
                    <asp:TextBox ID="txtNoOficioResolucion" placeholder="Número de oficio de la resolución" CssClass="form-control input-sm" MaxLength="50" AutoCompleteType="None" autocomplete="cc-csc" runat="server" />
                    <div class="form-text MensajeError" id="MensajeErrorNoResolucion" runat="server" style="color: #D0021B"></div>

                </div>
                <div class="col-md-6">
                    <label for="name">Fecha de la resolución*:</label>
                    <br />

                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtFechaResolucion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" />
                        <div class="form-text MensajeError" id="MensajeErrorFechaResolucion" runat="server" style="color: #D0021B"></div>
                        <asp:RegularExpressionValidator ID="RExprFechaResolucion"
                            ControlToValidate="txtFechaResolucion"
                            ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                            Display="Static"
                            ErrorMessage="La fecha de la resolución no tiene el formato correcto."
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />

                        <%-- <input id="txtFechaResolucion" type="text" runat="server" class="form-control" maxlength="10" autocompletetype="None" />--%>
                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                      

                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <br />
                    <br />
                    <label for="name">Fecha de notificación de la resolución*:</label>

                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtFechaNotificacioResolucion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" />
                        <div class="form-text MensajeError" id="MensajeErrorFechaNotificacion" runat="server" style="color: #D0021B"></div>
                        <asp:RegularExpressionValidator ID="RExprFechaNotificacion"
                            ControlToValidate="txtFechaNotificacioResolucion"
                            ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                            Display="Static"
                            ErrorMessage="La fecha de notificación no tiene el formato correcto."
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />

                        <%--<input id="txtFechaNotificacioResolucion" type="text" runat="server" class="form-control" maxlength="10" autocompletetype="None" />--%>
                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                        
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <br />
                    <label class="control-label">Evidencia*:</label>
                    <br />
                    <div class="row">
                        <div class="col-md-3">
                            <asp:FileUpload ID="flDocumento" Style="color: transparent" onchange="ChangeDocumento()" runat="server" class="inputfile" data-multiple-caption="{count} files selected" multiple />
                        </div>
                        <div class="col-md-9">
                            <p for="contenido_upf_req">
                                <span id="nameFile" runat="server">Ningun archivo selec&hellip;</span>
                            </p>
                        </div>
                    </div>
                    <%--  <input id="flDocumento" type="file" onchange="ChangeDocumento()" runat="server">--%>
                    <div class="form-text MensajeError" id="MensajeErrorDocumento" runat="server" style="color: #D0021B"></div>
                    <input runat="server" type="text" id="txtErrorDocumento" hidden />
                    <input runat="server" type="text" id="txtbasecode" hidden />
                    <input runat="server" type="text" id="txtbasecodeNombreArchivo" hidden />
                    <input type="hidden" id="HdIdIc2" runat="server" />
                    <asp:Button Style="display: none" ID="btnDescargaDoc" runat="server" class="btn btn-success" OnClick="DescargarArchivo_Click" Text="Descargar" />
                    <input runat="server" type="text" id="txtidDoc" hidden />
                </div>
            </div>

              <div class="row">
                <div class="col-md-6">
                    <br />
                    <br />
                    <label for="name">Trimestre del incumplimiento*:</label>
                    <div class="form-group datepicker-group">
                        <asp:DropDownList ID="ddlTrimestre" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc" runat="server"></asp:DropDownList>
                        <div class="form-text MensajeError" id="MensajeErrorTrimestre" runat="server" style="color: #D0021B"></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <br />
                    <label class="control-label">Año del incumplimiento*:</label>
                    <asp:DropDownList ID="ddlAnio" runat="server" OnSelectedIndexChanged="SeleccionarAño_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc"></asp:DropDownList>
                    <div class="form-text MensajeError" id="MensajeErrorAnio" runat="server" style="color: #D0021B"></div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <br />
                    <br />
                    <label for="name">Número de expediente*:</label>
                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtNoExpediente" placeholder="Número de expediente" CssClass="form-control input-sm" AutoCompleteType="None" MaxLength="50" autocomplete="cc-csc" runat="server" />
                        <div class="form-text MensajeError" id="MensajeErrorNoExpediente" runat="server" style="color: #D0021B"></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <br />
                    <label class="control-label">Iniciales del responsable*:</label>
                    <asp:TextBox ID="txtIniciales" placeholder="Iniciales del responsable" CssClass="form-control input-sm" AutoCompleteType="None" MaxLength="50" autocomplete="cc-csc" runat="server" />
                    <div class="form-text MensajeError" id="MensajeErrorIniciales" runat="server" style="color: #D0021B"></div>
                </div>
            </div>


            <div class="form-group">
                <label>Observaciones*:</label>
                <textarea class="form-control" rows="5" id="txtObservaciones" maxlength="500" runat="server"></textarea>
                <div class="form-text MensajeError" id="MensajeErrorObservaciones" runat="server" style="color: #D0021B"></div>

            </div>

            <div>
                <%-- <a id="idDocumentoEdita" class="btn btn-success">
                    <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                    <label id="lblNombreDocumento">Descargar documento</label>
                </a>--%>
            </div>
            <div class="row">
                <div class="col-md-6" style="text-align: left;">
                    <asp:Button ID="btnDescarga" Style="display: none" runat="server" CssClass="btn btn-primary" OnClick="Descarga_Click" Text="Descargar reincidencia" />
                    <input runat="server" type="text" id="txtIdReincidencia" hidden />
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnCerrarPagina" runat="server" CssClass="btn btn-default" OnClick="btnCerrarPagina_Click" Text="Cerrar" />

                    <asp:Button ID="lnkbtnValidarCampos" runat="server" CssClass="btn btn-primary" OnClick="lnkbtnValidarCampos_Click" Text="Guardar" />
                    <%--<button type="button" class="btn btn-primary" onclick="ValidarCampos()">Guardar</button>--%>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescarga" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>


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
                        <button id="btnAceptarModalConfirm" type="button" class="btn btn-primary" onclick="GuardarReincidencia()">Aceptar</button>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<!-- /.modal -->

<%--Modal Detalle--%>
<div id="modalDetalle" class="modal fade" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h4 class="modal-title">
                            <label id="tituloDetalle">Números y vigencias de aprobación</label>
                        </h4>
                    </div>
                    <div class="modal-body text-center">
                        <asp:GridView runat="server" ID="gvdetalle" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="Numero" HeaderText="Número de aprobación" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Vigencia de aprobación" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<!-- /.modal -->

<%--Modal DetalleSinInfo--%>
<div id="modalDetaSinInfo" class="modal fade" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h4 class="modal-title">
                            <label id="tituloDetalleSinInfo">Números y vigencias de aprobación</label>
                        </h4>
                    </div>
                    <div class="modal-body text-center">
                        <p>Sin información</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>

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
<script type="text/javascript">

    var extensiones = [];
    variables = [];
    variables.basecode = '';
    variables.basecodeNombreArchivo = '';
    extensiones.documentos = new Array("PDF", "pdf")
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
        $("#<%=txtFechaResolucion.ClientID%>").datepicker({ maxDate:0 });
        $("#<%=txtFechaNotificacioResolucion.ClientID%>").datepicker({ maxDate: 0 });
        $('[data-toggle="tooltip"]').tooltip();
        if (variables.basecodeNombreArchivo.name != undefined && variables.basecodeNombreArchivo.name != '') {
            var nameFile = document.getElementById('<%=nameFile.ClientID%>');
            nameFile.innerText = variables.basecodeNombreArchivo.name;
        }
    }

    function MostrarModalConfirm() {
        $("#modalConfirm").modal("show");
        var texto = document.getElementById("cuerpoConfirm");
        texto.innerHTML = "¿Está seguro que desea guardar la información?";
    }

    function MostrarModalDetalle() {
        $("#modalDetalle").modal("show");
    }
    function MostrarModalDetaSinInfo() {
        $("#modalDetaSinInfo").modal("show");
    }

    $('.botonF1').hover(function () {
        $('.btn').addClass('animacionVer');
    });

    $('.contenedor').mouseleave(function () {
        $('.btn').removeClass('animacionVer');
    });

    //RGG obtiene las propiedades del archivo
    function ChangeDocumento() {
        
        var nameFile = document.getElementById('<%=nameFile.ClientID%>');

        variables.basecode = '';
        variables.basecodeNombreArchivo = '';
        document.getElementById('<%= txtErrorDocumento.ClientID %>').value = '';

        document.getElementById('<%= txtbasecode.ClientID %>').value = '';
        document.getElementById('<%= txtbasecodeNombreArchivo.ClientID %>').value = '';
        document.getElementById('<%= MensajeErrorDocumento.ClientID %>').innerText = '';
        /*        var filesSelected = document.getElementById('flDocumento').files;*/

        var filesSelected = document.getElementById('<%= flDocumento.ClientID %>').files;
        var fileName = filesSelected[filesSelected.length - 1];
        if (fileName == undefined) {
            nameFile.innerText = 'Ningun archivo selec...';
        }
        var extension = fileName.name.split('.').pop().toLowerCase();
        nameFile.innerText = fileName.name;
        /*Solo doc y docx*/
        var ExtPermitida = ObtenerExtensiones(extension);
        if (ExtPermitida) {
            $("#MensajeErrorDocumento").html('');
            if (filesSelected.length > 0) {
                var fileToLoad = filesSelected[0];
                var fileReader = new FileReader();
                fileReader.onload = function (fileLoadedEvent) {
                    if (extension == "jpg" || extension == "JPG") {
                        var base64value2 = fileReader.result.replace("data:", "")
                            .replace(/^.+,/, "");
                        variables.basecode = base64value2;
                        document.getElementById('<%= txtbasecode.ClientID %>').value = base64value2;
                    }
                    else {
                        var base64value = fileLoadedEvent.target.result;
                        variables.basecode = base64value;
                        document.getElementById('<%= txtbasecode.ClientID %>').value = base64value;
                    }

                    variables.basecodeNombreArchivo = fileName;

                    document.getElementById('<%= txtbasecodeNombreArchivo.ClientID %>').value = fileName;
                    if (filesSelected.length > 0) {
                        var archivo = filesSelected[0];
                        var sizeByte = archivo.size;
                        var sizekiloByte = parseInt(sizeByte / 1024);
                        if (sizekiloByte > 10240) {
                            $('#flDocumento').val('').clone(true);
                            variables.basecode = '';
                            variables.basecodeNombreArchivo = '';
                            /* $("#MensajeErrorDocumento").html('El archivo excede el tamaño de 5 MB.');*/
                            document.getElementById('<%= MensajeErrorDocumento.ClientID %>').innerText = 'El archivo excede el tamaño de 10 MB.';
                    <%--        document.getElementById('<%= txtErrorDocumento.ClientID %>').value = 'El archivo excede el tamaño de 5 MB.';--%>
                            /* ejecutarModal(variables.encabezadoError, 'El archivo excede el tamaño de 5 Mb.');*/
                            variables.basecode = '';
                            document.getElementById('<%= txtbasecode.ClientID %>').value = variables.basecode;
                        }
                        else {
                            $("#MensajeErrorDocumento").html('');
                        }
                    }
                }
                fileReader.readAsDataURL(fileToLoad);

            }


        } else {
            $('#flDocumento').val('').clone(true);
            variables.basecode = '';
          /*  variables.basecodeNombreArchivo = '';*/
            /*  ejecutarModal(variables.encabezadoError, 'El tipo de archivo es incorrecto.');*/
        <%--    document.getElementById('<%= txtErrorDocumento.ClientID %>').value = 'El tipo de archivo es incorrecto.';--%>
            document.getElementById('<%= MensajeErrorDocumento.ClientID %>').innerText = 'El tipo de archivo es incorrecto.';
        }



    }

    function ObtenerExtensiones(ext) {
        var permitida = false;
        for (var i = 0; i < extensiones.documentos.length; i++) {
            if (extensiones.documentos[i] == ext) {
                permitida = true;
                break;
            }
        }
        return permitida;
    }

    function GuardarReincidencia() {
        var botonAceptar = document.getElementById("btnAceptarModalConfirm");
        botonAceptar.disabled = true
        var idApercibimiento = document.getElementById('<%= txtIdApercibimiento.ClientID %>').value;
        var noOficio = document.getElementById('<%= txtNoOficioResolucion.ClientID %>').value;
        var fechaResolucion = document.getElementById('<%= txtFechaResolucion.ClientID %>').value;
        var fechaNotificacion = document.getElementById('<%= txtFechaNotificacioResolucion.ClientID %>').value;
        var trimestre = document.getElementById('<%= ddlTrimestre.ClientID %>').value;
        var anio = document.getElementById('<%= ddlAnio.ClientID %>').value;
        var observaciones = document.getElementById('<%= txtObservaciones.ClientID %>').value;
        var noExpediente = document.getElementById('<%= txtNoExpediente.ClientID %>').value;
        var iniciales = document.getElementById('<%= txtIniciales.ClientID %>').value;

        //para Actualización
        var BanderaEsConsulta = document.getElementById('<%= txtBanderaEsConsulta.ClientID %>').value;
        var idDocIC2 = document.getElementById('<%= txtidDoc.ClientID %>').value;
        var idReincidencia = document.getElementById('<%= txtIdReincidencia.ClientID %>').value;
        var nombreArchivo
        if (variables.basecodeNombreArchivo == "") {
            nombreArchivo = "";
        }
        else {
            nombreArchivo = variables.basecodeNombreArchivo.name;
        }

        if (BanderaEsConsulta == "true") {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "NuevaReincidencia.aspx/ActualizarReincidencia",
                dataType: "json",
                data: JSON.stringify({ idReincidencia: idReincidencia, idDocIC2: idDocIC2, ArchivoBase64: variables.basecode, NombreArchivo: nombreArchivo, numOficio: noOficio, fechaOficio: fechaResolucion, fechaNotificacion: fechaNotificacion, observaciones: observaciones, iniciales: iniciales, noExpediente: noExpediente, trimestre: trimestre, anio: anio}),
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
                        document.getElementById('<%= txtIdReincidencia.ClientID %>').value = data.d.idReincidencia;
                        document.getElementById('<%= txtidDoc.ClientID %>').value = data.d.idIC2;
                        document.getElementById('<%= btnDescargaDoc.ClientID %>').style.display = "";
                        document.getElementById('<%= btnDescarga.ClientID %>').style.display = "";

                        
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
                url: "NuevaReincidencia.aspx/GuardarReincidencia",
                dataType: "json",
                data: JSON.stringify({ ArchivoBase64: variables.basecode, NombreArchivo: variables.basecodeNombreArchivo.name, numOficio: noOficio, fechaOficio: fechaResolucion, fechaNotificacion: fechaNotificacion, observaciones: observaciones, iniciales: iniciales, noExpediente: noExpediente, idApercibimiento: idApercibimiento, trimestre: trimestre, anio: anio }),
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
                        document.getElementById('<%= txtIdReincidencia.ClientID %>').value = data.d.idReincidencia;
                        document.getElementById('<%= txtidDoc.ClientID %>').value = data.d.idIC2;
                        document.getElementById('<%= btnDescargaDoc.ClientID %>').style.display = "";
                        document.getElementById('<%= btnDescarga.ClientID %>').style.display = "";
                        document.getElementById('<%= lnkbtnValidarCampos.ClientID %>').style.display = "none";


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
        document.getElementById('<%= txtNoOficioResolucion.ClientID %>').disabled = true;
        document.getElementById('<%= txtFechaResolucion.ClientID %>').disabled = true;
        document.getElementById('<%= txtFechaNotificacioResolucion.ClientID %>').disabled = true;
        document.getElementById('<%= ddlTrimestre.ClientID %>').disabled = true;
        document.getElementById('<%= ddlAnio.ClientID %>').disabled = true;
        document.getElementById('<%= txtObservaciones.ClientID %>').disabled = true;
        document.getElementById('<%= txtNoExpediente.ClientID %>').disabled = true;
        document.getElementById('<%= txtIniciales.ClientID %>').disabled = true;
        variables.basecode = '';
        variables.basecodeNombreArchivo = '';
        document.getElementById('<%= txtbasecode.ClientID %>').value = '';
        document.getElementById('<%= txtbasecodeNombreArchivo.ClientID %>').value = '';
        var nameFile = document.getElementById('<%=nameFile.ClientID%>');
        nameFile.innerText = 'Ningun archivo selec...';
    }
</script>
