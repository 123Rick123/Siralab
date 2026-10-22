<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="Orden.aspx.cs" Inherits="SIRALAB.Orden" Culture="es-MX" UICulture="es-MX" %>

<%@ Register Src="~/Controles/wucListaOrdenTrabajo.ascx" TagPrefix="uc1" TagName="wucListaOrdenTrabajo" %>
<%@ Register Src="~/Controles/wucPaginador.ascx" TagPrefix="uc1" TagName="wucPaginador" %>
<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" TagPrefix="uc1" TagName="wucAppletFIEL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <script>
        var muestraBtnFirma = false;
        var muestraBtnEliminar = false;
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
            $('#<%=txtEmisionFechaInicio.ClientID%>').datepicker();
            $("#<%=txtEmisionFechaFin.ClientID%>").datepicker();
            $("#<%=txtRegistroFechaInicio.ClientID%>").datepicker();
            $("#<%=txtRegistroFechaFin.ClientID%>").datepicker();
            $("#<%=txtConformacionFechaInicio.ClientID%>").datepicker();
            $("#<%=txtConformacionFechaFin.ClientID%>").datepicker();
            $('[data-toggle="tooltip"]').tooltip();

        });

        $('.botonF1').hover(function () {
            $('.btn').addClass('animacionVer');
        });

        $('.contenedor').mouseleave(function () {
            $('.btn').removeClass('animacionVer');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <!-- JqGrid -->
    <link href="JQGridReq/ui.jqgrid.css" rel="stylesheet" />
    <link href="JQGridReq/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <script src="JQGridReq/jquery-ui.min.js"></script>
    <script src="JQGridReq/jquery.jqGrid.min.js"></script>
    <script src="JQGridReq/grid.locale-es.js"></script>
    <div class=" container">
        <div class="row">
            <div class="col-sm-12">
                <h2>Inicio</h2>
                <hr class="red" style="margin-bottom: 30px !important;">
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row" role="group">
            <div class="col-sm-3">
                <a href="Manual/SIRALABmanual_v1.pdf" target="_blank" class="btn btn-default btn-sm">
                    <span class=" glyphicon glyphicon-book"></span>&nbsp;
                    Manual de mejoras
                </a>
            </div>
            <div class="col-sm-6"></div>
            <div class="col-sm-3 text-right">
                <asp:LinkButton runat="server" ID="btnNuevo" ToolTip="Nueva orden de trabajo" CssClass="btn btn-default btn-sm" OnClick="lnkbtnNuevo_Click">
                    <span class="glyphicon glyphicon-plus"></span>&nbsp;
                    Nueva orden de trabajo
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="upBusqueda" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" class="container">
        <Triggers>
           <asp:PostBackTrigger ControlID="lnkLimpiar" />
            <asp:PostBackTrigger ControlID="lnkExportar" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h5><span>Consulta de órdenes de trabajo</span></h5>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:RadioButton ID="rbFirmados" runat="server" Text=" &nbsp;Ordenes de trabajo generadas con análisis firmados" GroupName="Firmados" value="1" data-main-filter />
                                </div>
                                <div class="col-sm-12">
                                    <asp:RadioButton ID="rbNoFirmados" runat="server" Text=" &nbsp;Ordenes de trabajo generadas con análisis no firmados" GroupName="Firmados" value="0" data-main-filter />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="name">No. Informe:</label>
                                        <asp:TextBox ID="txtNumeroInforme" runat="server" class="form-control" placeholder="No. informe" MaxLength="390" data-filter autocomplete="cc-csc" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="name">Norma:</label>
                                        <asp:DropDownList ID="ddlNorma" runat="server" CssClass="form-control" data-filter></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="name">RFC:</label>
                                        <asp:TextBox ID="txtRfc" runat="server" CssClass="form-control" MaxLength="14" placeholder="RFC" data-filter autocomplete="cc-csc" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="name">Título:</label>
                                        <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Título" data-filter autocomplete="cc-csc" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="name">Anexo:</label>
                                        <asp:TextBox ID="txtAnexo" runat="server" CssClass="form-control" placeholder="Anexo" data-filter autocomplete="cc-csc" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <asp:RadioButton ID="rbCancelados" runat="server" Text="&nbsp;Cancelados" GroupName="Cancelados" value="0" data-filter /><br />
                                        <asp:RadioButton ID="rbNoCancelados" runat="server" Text="&nbsp;No cancelados" GroupName="Cancelados" value="1" data-filter /><br />
                                        <asp:RadioButton ID="rbTodos" runat="server" Text="&nbsp;Todos" GroupName="Cancelados" Checked="true" value="2" data-filter />
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h6 class="filter-header">Emisión</h6>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="form-group datepicker-group">
                                                    <label for="name">Inicial:</label>
                                                    <asp:TextBox ID="txtEmisionFechaInicio" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" data-filter autocomplete="cc-csc" />
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="form-group datepicker-group">
                                                    <label for="name">Final:</label>
                                                    <asp:TextBox ID="txtEmisionFechaFin" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="Disabled" data-filter autocomplete="cc-csc"></asp:TextBox>
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h6 class="filter-header">Registro de análisis</h6>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="form-group datepicker-group">
                                                    <label for="name">Inicial:</label>
                                                    <asp:TextBox ID="txtRegistroFechaInicio" runat="server" CssClass="form-control input-sm" MaxLength="10" data-filter autocomplete="cc-csc" />
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="form-group datepicker-group">
                                                    <label for="name">Final:</label>
                                                    <asp:TextBox ID="txtRegistroFechaFin" runat="server" CssClass="form-control input-sm" MaxLength="10" data-filter autocomplete="cc-csc" />
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h6 class="filter-header">Conformación de muestra compuesta</h6>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="form-group datepicker-group">
                                                    <label for="name">Inicial:</label>
                                                    <asp:TextBox ID="txtConformacionFechaInicio" runat="server" CssClass="form-control input-sm" MaxLength="10" data-filter autocomplete="cc-csc" />
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <div class="form-group datepicker-group">
                                                    <label for="name">Final:</label>
                                                    <asp:TextBox ID="txtConformacionFechaFin" runat="server" CssClass="form-control input-sm" MaxLength="10" data-filter autocomplete="cc-csc" />
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" ID="divLaboratorio" class="row" Visible="False">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="name">Laboratorio:</label>
                                        <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="form-control" data-filter></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:LinkButton ID="lnkExportar" runat="server" class=" btn btn-default" ToolTip="Exportar a excel" Text="Exportar a excel" OnClick="lnkExportar_Click"></asp:LinkButton>
                                    <a id="exporta" href="hdExportarExcel.ashx" class=" btn btn-default" title="Exportar a excel" style="display: none;">Exportar a excel</a>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="btn btn-danger" ToolTip="Eliminar órdenes seleccionadas" Text="Eliminar" OnClick="lnkEliminar_OnClick" delete-action Style="display: none;"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkFirmar" runat="server" CssClass="btn btn-default" ToolTip="Firmar análisis seleccionadas" Text="Firmar análisis" OnClick="lnkFirmar_OnClick" sign-action Style="display: none;"></asp:LinkButton>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:LinkButton ID="lnkLimpiar" runat="server" CssClass="btn btn-danger" ToolTip="Limpiar búsqueda" Text="Limpiar"  OnClick="lnkLimpiar_Click"  clear-action></asp:LinkButton>
<%--                                     <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-danger" ToolTip="Limpiar búsqueda" Text="Limpiar"  OnClientClick="return false;" clear-action></asp:LinkButton>--%>
                                    <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-primary" ToolTip="Buscar" Text="Buscar" OnClick="lnkBuscar_OnClick" find-action></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="container">
        <uc1:wucListaOrdenTrabajo runat="server" ID="wucListaOrdenTrabajo" OnFirmarAnalisis="wucListaOrdenTrabajo_FirmarAnalisis" />
        <uc1:wucPaginador runat="server" ID="wucPaginador" OnPageChange="wucPaginador_OnPageChange" />
    </div>
    <uc1:wucAppletFIEL runat="server" ID="wucAppletFIEL" OnFirmarColeccion="wucAppletFIEL_FirmarColeccion" />

    <script type="text/javascript">
        $(document).ready(function () {
       
            $("input[type=text]:first-child").focus();
            $("a[clear-action]").click(function () {
                $("input[type='text'][data-filter]").val("");
                $("select[data-filter] option:selected").removeAttr("selected");

                $("span[data-filter] input[type='radio']:checked").prop("checked", false);
                $("span[data-filter]:last-child input[type='radio']").prop("checked", true);
            });

            $(".Close").on("click",
                function () {
                    $(".modal-backdrop").hide();
                });
        });

        function exportarExcel() {
            document.getElementById("exporta").click();
        }
       
    </script>
</asp:Content>
