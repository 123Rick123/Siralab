<%@ Page Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="ApercibidosYReincidentes.aspx.cs" Inherits="SIRALAB.ApercibidosYReincidentes" Culture="es-MX" UICulture="es-MX" %>

<%--PAGINA PRINCIPAL--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphNeasted" runat="server">
 <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <script src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>
 <%--    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>--%>
    <link href="https://cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>


    <script>

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
            $('#txtFechaInicio').datepicker({ maxDate: 0 });
            $("#txtFechaFin").datepicker({ maxDate: 0 });

            //const fecIni = $('#txtFechaInicio').datepicker().on("change", function () {
            //    let fechaInicio = getDate($('#txtFechaInicio').val());

            //    fecFin.datepicker("option", "minDate", fechaInicio);
            //});

            //const fecFin = $("#txtFechaFin").datepicker().on("change", function () {

            //    let fechaFin = getDate($('#txtFechaFin').val());


            //    fecIni.datepicker("option", "maxDate", fechaFin);
            //});;

            //$('[data-toggle="tooltip"]').tooltip();


            function getDate(value) {
                var date;
                try {
                    date = $.datepicker.parseDate('dd/mm/yy', value);
                } catch (error) {
                    date = null;
                }

                return date;
            }

        });





    </script>
    <style>
        .panel-heading b {
            font-size: 16px !important;
        }

        .table-sm {
            font-size: 10pt;
        }

        .filter-header {
            border-bottom: 1px solid #e2e2e2;
        }
    </style>
    <br />
    <div class="row">
        <div class="col-sm-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h5><span>Búsqueda apercibidos y reincidentes</span></h5>
                </div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="name">Nombre de laboratorio:</label>
                                <input id="txtNombreLaboratorio" data-query="" type="text" class="form-control" placeholder="Nombre laboratorio" autocompletetype="None" autocomplete="cc-csc" />
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="name">RFC de laboratorio:</label>
                                <input id="txtRfc" data-query="" type="text" class="form-control input-sm" maxlength="13" placeholder="RFC de laboratorio" autocompletetype="None" autocomplete="cc-|csc" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="name">Número de aprobación:</label>
                                <input id="txtNoAprobacion" data-query="" type="text" class="form-control input-sm" maxlength="11" placeholder="Número de aprobación" autocompletetype="None" autocomplete="cc-csc" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <h6 class="filter-header"><span style="color: white;">.</span>  </h6>
                                </div>
                                <div class="col-md-12">
                                    <label for="name">Estatus:</label>
                                    <select id="ddlEstatus" class="form-control">
                                        <option value="0">Todos</option>
                                        <option value="apercibido">Apercibidos</option>
                                      <%--  <option value="2">Apercibidos con Impugnación</option>--%>
                                        <option value="reincidente">Reincidentes</option>
                                       <%-- <option value="4">Reincidentes con Impugnación</option>--%>
                                    </select>
                                </div>

                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <h6 class="filter-header">Periodo</h6>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <div class="form-group datepicker-group">
                                            <label for="name">Inicio:</label>
                                            <input id="txtFechaInicio" data-query="" type="text" class="form-control input-sm" maxlength="10" autocompletetype="None" autocomplete="cc-csc" />
                                            <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <div class="form-group datepicker-group">
                                            <label for="name">Fin:</label>
                                            <input id="txtFechaFin" data-query="" type="text" class="form-control input-sm" maxlength="10" autocompletetype="None" autocomplete="cc-csc" />
                                            <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-md-12 text-right">
                            <button id="btnLimpiar" class="btn btn-default">Limpiar</button>
                            <button id="btnBuscar" class="btn btn-primary">Buscar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row" role="group">
            <div class="col-sm-12 text-left">
                <%-- <button type="button" name="name" id="btnNuevoApercibimiento" runat="server" onclick="lnkbtnNuevo_Click" class="btn btn-default">Nuevo apercibimiento</button>--%>
                <asp:LinkButton runat="server" ID="btnNuevo" ToolTip="Nuevo apercibimiento" CssClass="btn btn-default btn-sm" OnClick="lnkbtnNuevo_Click">
                    <span class="glyphicon glyphicon-plus"></span>&nbsp;
                    Nuevo apercibimiento
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <br />
    <div class="container">

        <div class="row" role="group">
            <table id="example" class="display" style="width: 100%">
                <thead>
                    <tr>
                        <th></th>
                        <th>RFC</th>
                        <th>Nombre de laboratorio</th>
                        <th>Fecha Resolución</th>
                        <th>Número de oficio de la resolución</th>
                        <th>Fecha de la notificación</th>
                        <th>Vigencia</th>
                        <th>Estatus</th>
                        <th>Acciones</th>
                        <th>Medio de Impugnación</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>

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
                            <p id="TextoAlertaSimple" style="text-align: center;"></p>
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
    <script src="js/Apercibimiento/busqueda.js"></script>
</asp:Content>


