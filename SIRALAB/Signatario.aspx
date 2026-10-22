<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="Signatario.aspx.cs" Inherits="SIRALAB.Signatario" Culture="es-MX" UICulture="es-MX" %>

<%@ Register Src="~/Controles/wucListaSignatario.ascx" TagPrefix="uc1" TagName="wucListaSignatario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/botones.css" rel="stylesheet" type="text/css" />
    <link href="style/grid.css" rel="stylesheet" type="text/css" />
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <%--<link href="style/paginador.css" rel="stylesheet" />--%>
    <link href="style/siralab/iconos.css" rel="stylesheet" />
    <style type="text/css">
        .tabla-busqueda {
            box-sizing: border-box;
        }

            .tabla-busqueda td, .tabla-busqueda th {
                border: 1px solid #e2e2e2;
            }
    </style>
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
                $('[id*=txtFechaInicio]').datepicker();
                $('[id*=txtFechaFin]').datepicker();

                $('[data-toggle="tooltip"]').tooltip();
          
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <!-- JqGrid -->
    <%--<link href="JQGridReq/jquery-ui.css" rel="stylesheet" />--%>
    <link href="JQGridReq/ui.jqgrid.css" rel="stylesheet" />
    <link href="JQGridReq/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <script src="JQGridReq/jquery-ui.min.js"></script>
    <script src="JQGridReq/jquery.jqGrid.min.js"></script>
    <script src="JQGridReq/grid.locale-es.js"></script>
    <style type="text/css">
        .header-buttons {
            padding: 10px 20px;
            height: 40px;
            width: 100%;
            box-sizing: border-box;
        }

        .add-new, .add-new:hover {
            float: right;
        }

        .help-tutorial, .help-tutorial:hover {
            width: auto;
            padding-left: 28px;
            background-position-x: left;
            display: table-cell;
            vertical-align: middle;
            font-size: 1.7em;
            color: #00753E;
        }
    </style>
    <div style="min-height: 100px;">
        <div class="container bottom-buffer">
            <div class="titulo-pagina">Signatarios</div>
        </div>
    </div>
    <div class="formulario">
        <asp:UpdatePanel ID="upBusqueda" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkLimpiar" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlBusqueda" runat="server" CssClass="panel panel-default" DefaultButton="lnkBuscar">
                    <div class="panel-heading"><span>Consulta de signatarios por laboratorio</span></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <label for="name">Laboratorio<span class="control-label"></span>:</label>
                                <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="form-control" data-filter></asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <div class="form-group datepicker-group">
                                        <label for="name">Inicial<span class="control-label"></span>:</label>
                                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" MaxLength="10" data-filter />
                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <div class="form-group datepicker-group">
                                        <label for="name">Final<span class="control-label"></span>:</label>
                                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" MaxLength="10" data-filter />
                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <table class="table-form">
                            <tr>
                                <td>
                                    <a href="hdExportarExcel.ashx" class=" btn btn-default" title="Exportar a excel" runat="server">Exportar a excel</a>
                                </td>
                                <td style="text-align: right; padding-top: 10px;">
                                    <asp:LinkButton ID="lnkLimpiar" runat="server" CssClass="btn btn-danger" ToolTip="Limpiar búsqueda" Text="Limpiar" OnClick="lnkLimpiar_OnClick" clear-action></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-primary" ToolTip="Buscar" Text="Buscar" OnClick="lnkBuscar_OnClick" find-action></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="margin: 10px;">
            <uc1:wucListaSignatario runat="server" ID="wucListaSignatario" />
        </div>
    </div>
</asp:Content>

