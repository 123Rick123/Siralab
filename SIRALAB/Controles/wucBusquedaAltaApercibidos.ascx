<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucBusquedaAltaApercibidos.ascx.cs" Inherits="SIRALAB.Controles.wucBusquedaAltaApercibidos" ClientIDMode="AutoID" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/autocomplete.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
<link href="https://cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />

<%--   <script src="https://code.jquery.com/jquery-3.5.1.js" type="text/javascript"></script>--%>
<script src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js" type="text/javascript"></script>






<%--<link href="../style/Estilos.css" rel="stylesheet" />--%>

 <style type="text/css">
        .data-table {
            border-collapse: collapse;
            border: 1px solid #E2E2E2;
        }

            .data-table > tbody > tr > td, .data-table > tbody > tr > th {
                border: 1px solid #E2E2E2;
                padding: 3px;
            }

            .data-table input[type='text'], .data-table select, .data-table textarea {
                width: 100%;
                box-sizing: content-box;
            }

            .data-table > tbody > tr:nth-child(even) {
                background: #F2F2F2;
            }

            .data-table > tbody > tr > th {
                background: #CCC;
            }

       
    </style>



<%--panel busqueda laboratorio--%>
<asp:Panel ID="pnlCapturaApercibido" runat="server" Visible="true">
    <asp:UpdatePanel ID="updBusquedaLab" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lnkbtnConsultaLaboratorio" EventName="Click" />
        </Triggers>
        <ContentTemplate>


            <div class="panel-body">
                <div class="row">

                    <div class="col-md-5">
                        <label id="lblRFC" runat="server" class="control-label">RFC:</label>
                        <%--<input class="form-control" id="txtRFC" placeholder="Ingresa tu RFC" runat="server" type="text" autocompletetype="None">--%>
                        <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control" placeholder="RFC del laboratorio" AutoCompleteType="None" autocomplete="cc-csc"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            ControlToValidate="txtRFC"
                            ValidationExpression="^[A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3}$"
                            Display="Static"
                            ErrorMessage="El RFC no tiene el formato correcto"
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />
                    </div>
                    <div class="col-md-5">
                        <label id="lblNombreBusqueda" runat="server" class="control-label">Nombre de laboratorio:</label>
                        <%--<input class="form-control" id="txtNombreBusqueda" placeholder="Ingresa un nombre de laboratorio" runat="server" type="text" />--%>
                        <asp:TextBox CssClass="form-control" ID="txtNombreBusqueda" placeholder="Nombre de laboratorio" runat="server" type="text" AutoCompleteType="None" autocomplete="cc-csc"></asp:TextBox>
                    </div>
                    <br />

                    <div class="col-md-2">
                        <asp:LinkButton ID="lnkbtnConsultaLaboratorio" runat="server" CssClass="btn btn-default" ToolTip="Buscar RFC" OnClick="lnkbtnConsultaLaboratorio_Click">
                            Buscar
                                 <span class="glyphicon glyphicon-search"></span>
                        </asp:LinkButton>

                        <%--                 <button type="button" id="btnBuscarLab" class="btn btn-default" onclick="muestraResultBuscarlaboratorio()">busca otro</button>--%>
                        <input runat="server" type="text" id="txtIdLabBusqueda" hidden />
                        <input runat="server" type="text" id="txtBanderaEsConsulta" hidden />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="table-responsive m-b">
                        <table id="example" class="table table-striped table-bordered table-sm row-border hover order-column" cellspacing="0" style="display: none">
                            <thead>
                                <tr>
                                    <th style="min-width: 10px;"></th>
                                    <th style="min-width: 100px;">RFC</th>
                                    <th style="min-width: 120px;">Nombre de laboratorio</th>
                                </tr>
                            </thead>
                        </table>
                    </div>




                    <div class="table-responsive m-b" id="esElGrid">
                        <asp:GridView ID="grdHistorico" runat="server"  DataKeyNames="idLaboratorio" 
                            AutoGenerateColumns="False"
                            EmptyDataText="No hay resultados para la búsqueda solicitada."
                            AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True"
                            Width="97%" CssClass="table table-striped" CellSpacing="2" BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSeleccion" runat="server" OnCheckedChanged="chkSeleccion_click" AutoPostBack="True" onclick="CheckOne(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idLaboratorio" ShowHeader="true" HeaderText="id" Visible="False" />
                                <asp:BoundField DataField="RFCLaboratorio" ShowHeader="true" HeaderText="RFC" />
                                <asp:BoundField DataField="NombreLaboratorio" ShowHeader="true" HeaderText="Nombre de laboratorio" />
                               

                                <asp:TemplateField HeaderText="Números y vigencias de aprobación">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDEtalle" runat="server" Text="Ver" AutoPostBack="True" OnClick="lnkDEtalle_Click1" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button ID="Button1" runat="server" Text="Cargar" Style="display: none" />


                    
                    <div class="form-text MensajeError" id="MensajeErrorIdLab" runat="server" style="color: #D0021B"></div>
                </div>




            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<%--panel formulario alta--%>
<asp:Panel ID="PanelFormulario" runat="server" Visible="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <%--         <asp:PostBackTrigger ControlID="lnkbtnValidarCampos"  />--%>
            <asp:AsyncPostBackTrigger ControlID="lnkbtnValidarCampos" EventName="Click" />
        </Triggers>
        <ContentTemplate>
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
                                <span id="nameFile" runat="server">Ningún archivo selec&hellip;</span>
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
                    <asp:DropDownList ID="ddlAnio" OnSelectedIndexChanged="SeleccionarAño_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc"></asp:DropDownList>
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
                    <asp:Button ID="btnDescarga" runat="server" Style="display: none" CssClass="btn btn-primary" OnClick="Descarga_Click" Text="Descargar apercibimiento" />
                    <input runat="server" type="text" id="txtIdApercibimiento" hidden />
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
                        <button id="btnAceptarModalConfirm" type="button" class="btn btn-primary" onclick="GuardarApercibimiento()">Aceptar</button>

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
                        <asp:GridView runat="server" ID="gvdetalleA" AutoGenerateColumns="False">
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
<div id="modalDetalleAASinInfo" class="modal fade" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
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
    variables.resutadoBusqueda = [];
    variables.idLab = 0;
    variables.RFClab = '';
    variables.NombreLab = '';
    extensiones.documentos = new Array("PDF", "pdf")
    $(document).ready(function () {
      
        Calendario();

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
        $("#<%=txtFechaNotificacioResolucion.ClientID%>").datepicker({ maxDate: 0});
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
    function MostrarModalDetalleAASinInfo() {
        $("#modalDetalleAASinInfo").modal("show");
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
            nameFile.innerText = 'Ningún archivo selec...';
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

    function GuardarApercibimiento() {
        var botonAceptar = document.getElementById("btnAceptarModalConfirm");
        botonAceptar.disabled = true
        var idLaboratorio = document.getElementById('<%= txtIdLabBusqueda.ClientID %>').value;
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
        var idApercibimiento = document.getElementById('<%= txtIdApercibimiento.ClientID %>').value;
        var nombreArchivo
        if (variables.basecodeNombreArchivo=="") {
            nombreArchivo = "";
        }
        else {
            nombreArchivo = variables.basecodeNombreArchivo.name;
        }
        

        if (BanderaEsConsulta == "true") {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "NuevoApercibimiento.aspx/ActualizaApercibimiento",
                dataType: "json",
                data: JSON.stringify({ idApercibimiento: idApercibimiento, idDocIC2: idDocIC2, ArchivoBase64: variables.basecode, NombreArchivo: nombreArchivo, numOficio: noOficio, fechaOficio: fechaResolucion, fechaNotificacion: fechaNotificacion, trimestre: trimestre, anio: anio, observaciones: observaciones, iniciales: iniciales, noExpediente: noExpediente, idLaboratorio: idLaboratorio }),
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
                        document.getElementById('<%= txtIdApercibimiento.ClientID %>').value = data.d.idApercibimiento;
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
                url: "NuevoApercibimiento.aspx/GuardarApercibimiento",
                dataType: "json",
                data: JSON.stringify({ ArchivoBase64: variables.basecode, NombreArchivo: variables.basecodeNombreArchivo.name, numOficio: noOficio, fechaOficio: fechaResolucion, fechaNotificacion: fechaNotificacion, trimestre: trimestre, anio: anio, observaciones: observaciones, iniciales: iniciales, noExpediente: noExpediente, idLaboratorio: idLaboratorio }),
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
                        document.getElementById('<%= txtIdApercibimiento.ClientID %>').value = data.d.idApercibimiento;
<%--                        document.getElementById('<%= grdHistorico.ClientID %>').style.display = "none";--%>
                        document.getElementById('<%= txtidDoc.ClientID %>').value = data.d.idIC2;
                        document.getElementById('<%= btnDescargaDoc.ClientID %>').style.display = "";
                        document.getElementById('<%= btnDescarga.ClientID %>').style.display = "";
                        document.getElementById('<%= lnkbtnValidarCampos.ClientID %>').style.display = "none";
                        var tble = document.getElementById('<%= grdHistorico.ClientID %>');
                        var row = tble.rows; // Getting the rows
                        var cell = row[1].cells[0];
                        cell.childNodes[1].disabled = true;

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

    function muestraResultBuscarlaboratorio() {
        var RFC = document.getElementById('<%= txtRFC.ClientID %>').value;
        var NombreLaboratorio = document.getElementById('<%= txtNombreBusqueda.ClientID %>').value;

        $.ajax({
            type: "POST",
            url: "NuevoApercibimiento.aspx/ObtenerLaboratorios",
            cache: false,
            aysnc: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ rfc: RFC, NombreLaboratorio: NombreLaboratorio }),
            success: function (json_data) {
                var laboratoriosArray = [];

                $('#example').dataTable().fnClearTable();
                $('#example').dataTable().fnDestroy();
                $(".hidetable").hide();
                for (var i = 0; i < json_data.d.length; i++) {
                    laboratoriosArray.push([
                        //json_data[i].ID_Contrib,
                        "<div style='text-align:center'><input type = 'checkbox' id = 'chkIdLab" + "|" + (json_data.d[i].idLaboratorio).toString() + "'  onclick =\"Checks(this.id)\" /></div>"
                        , "<div style='text-align:center'>" + json_data.d[i].RFCLaboratorio + "</div>"
                        , "<div style='text-align:center'>" + json_data.d[i].NombreLaboratorio + "</div>"


                    ]);

                }
                variables.resutadoBusqueda = json_data.d;
                var table = $('#example').DataTable({
                    data: laboratoriosArray,
                    deferRender: true,
                    responsive: true,
                    searching: false,
                    bPaginate: true,
                    "processing": true,
                    select: true
                });
            },
            error: function (xhr, status, error) {
                alert('ricch Error!!');
            }
        });
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
        nameFile.innerText = 'Ningún archivo selec...';

        document.getElementById('<%= txtRFC.ClientID %>').disabled = true;
        document.getElementById('<%= txtNombreBusqueda.ClientID %>').disabled = true;

    }

    function Checks(id) {
        if (variables.idLab != 0) {
            var idChecAnterior = document.getElementById("chkIdLab|" + variables.idLab.toString());
            idChecAnterior.checked = false;
        }
        var chkObtenerId = document.getElementById(id);
        if (chkObtenerId.checked) {
            arrayDatosId = id.split("|");
            var id = parseInt(arrayDatosId[1]);
            variables.idLab = id;
            document.getElementById('<%= txtIdLabBusqueda.ClientID %>').value = variables.idLab;

        }
        else {
            arrayDatosId = id.split("|");
            var id = parseInt(arrayDatosId[1]);
            variables.idLab = 0;
            variables.RFClab = '';
            variables.NombreLab = '';
            document.getElementById('<%= txtIdLabBusqueda.ClientID %>').value = variables.idLab;
        }

    }

    function CheckOne(obj) {
        var grid = obj.parentNode.parentNode.parentNode;
        var inputs = grid.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "checkbox") {
                if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                    inputs[i].checked = false;
                }
            }
        }
    }


</script>


<%-- <script type="text/javascript" charset="utf8" src="https://code.jquery.com/jquery-3.5.1.js"></script>
 <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>--%>

