<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="Habilitarlaboratorios.aspx.cs" Inherits="SIRALAB.Habilitarlaboratorios" %>

<%@ Register TagPrefix="uc1" TagName="wucPaginador" Src="~/Controles/wucPaginador.ascx" %>
<%@ Register TagPrefix="uc1" TagName="wucAppletFIEL" Src="~/FirmaElectronica/wucAppletFIEL.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <%--    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>--%>

    <style>
        .panel-heading b {
            font-size: 16px !important;
        }

        .table-sm {
            font-size: 10pt;
            
        }
        .table-sm th {
            text-align: center !important;
            vertical-align: middle !important;
        }
        .table-sm td {
            text-align: center !important;
            vertical-align: middle !important;
        }

        .filter-header {
            border-bottom: 1px solid #e2e2e2;
        }

        .container {
            width: auto;
            max-width: 80%;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <%--RFD -> 11/12/2020: Refactorizado para implementar gráfica base:--%>
            <br />
            <div class=" container">
                <div class="row">
                    <div class="col-md-1">
                        <asp:LinkButton ID="lnkbtnback" runat="server" CssClass="btn btn-default btn-ms" OnClick="lnkbtnback_Click" ToolTip="Regresar a la pagina anterior">
                <span class="glyphicon glyphicon-chevron-left"></span>
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-11">
                        <h2>Habilitar análisis.</h2>
                        <hr class="red">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h5><span>Habilitar análisis de laboratorio.</span></h5>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">

                                        <div class="form-group">
                                            <label for="name">No. Informe</label>
                                            <asp:TextBox ID="txtNumInforme" CssClass="form-control input-sm" placeholder="No. Informe" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="name">Folio CONAGUA:</label>
                                            <asp:TextBox ID="txtFolio" CssClass="form-control input-sm" placeholder="Folio CONAGUA" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="filter-header">Laboratorio:</h6>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="name">RFC:</label>
                                                    <asp:TextBox ID="txtRFCLaboratorio" CssClass="form-control input-sm" placeholder="RFC laboratorio" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="name">Nombre:</label>
                                                    <asp:TextBox ID="txtNombreLaboratorio" CssClass="form-control input-sm" placeholder="Nombre laboratorio" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="filter-header">Cliente:</h6>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="name">RFC:</label>
                                                    <asp:TextBox ID="txtRFCCliente" CssClass="form-control input-sm" placeholder="RFC cliente" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="name">Nombre:</label>
                                                    <asp:TextBox ID="txtNombreCliente" CssClass="form-control input-sm" placeholder="Nombre cliente" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="filter-header">Fecha conformacion:</h6>
                                            </div>
                                            <div class="col-md-6">
                                                <div classs="form-group">
                                                    <div class="form-group datepicker-group">
                                                        <label for="name">Inicio:</label>
                                                        <asp:TextBox ID="txtConformacionInicio" CssClass="form-control input-sm fecha" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div classs="form-group">
                                                    <div class="form-group datepicker-group">
                                                        <label for="name">Fin:</label>
                                                        <asp:TextBox ID="txtConformacionFin" CssClass="form-control input-sm fecha" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="filter-header">Fecha firma:</h6>
                                            </div>
                                            <div class="col-md-6">
                                                <div classs="form-group">
                                                    <div class="form-group datepicker-group">
                                                        <label for="name">Inicio:</label>
                                                        <asp:TextBox ID="txtFirmaInicio" CssClass="form-control input-sm fecha" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div classs="form-group">
                                                    <div class="form-group datepicker-group">
                                                        <label for="name">Fin:</label>
                                                        <asp:TextBox ID="txtFirmaFin" CssClass="form-control input-sm fecha" runat="server" autocomplete="cc-csc"></asp:TextBox>
                                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 text-right">
                                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar parametros" CssClass="btn btn-primary" OnClick="btnLimpiar_Click" />
                                        <asp:Button runat="server" ID="btnBusqueda" Text="Buscar" CssClass="btn btn-default" OnClick="btnBusqueda_OnClick" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="pnlListaAnalisis" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <br />
                                <br />
                                <br />
                            </div>
                            <div class="col-md-6 text-right">
                                <asp:Button runat="server" ID="btnHabilitar" Text="Habilitar" CssClass="btn btn-default" OnClick="btnHabilitar_OnClick" Style="display: none" />
                            </div>
                        </div>
                        <div runat="server" id="divTblHabilitacion" class="table table-responsive">
                            <table id="tblHabilitacion" class="table table-condensed table-striped table-sm">
                                <thead>
                                    <tr s>
                                        <th>
                                            <input runat="server" type="checkbox" id="chkSeleccionTodos" class="form-check-input" />
                                        </th>
                                        <th>Folio CONAGUA</th>
                                        <th>Laboratorio</th>
                                        <th>RFC Cliente</th>
                                        <th>Cliente</th>
                                        <th>Informe</th>
                                        <th>Título</th>
                                        <th>Anexo</th>
                                        <th>Norma</th>
                                        <th>Fecha conformación</th>
                                        <th>Fecha firma</th>
                                        <th>Estatus</th>
                                    </tr>
                                </thead>
                                <asp:Repeater runat="server" ID="rptHabilitacion" ItemType="SIRALAB_DAL.Vw_Habilitar_Analis">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkSeleccionAnalisis" data-analisisid="<%#Item.FoloCONAGUA %>" CssClass="form-check-input" />
                                                </td>
                                                <td><%#Item.FoloCONAGUA %></td>
                                                <td><%#Item.NombreLaboratorio %></td>
                                                <td><%#Item.RFCContribuyente %></td>
                                                <td><%#Item.NombreContribuyente %></td>
                                                <td><%#Item.Informe %></td>
                                                <td><%#Item.Titulo %></td>
                                                <td><%#Item.Anexo %></td>
                                                <td><%#Item.Norma %></td>
                                                <td><%#Item.Fecha_de_conformación != null ? ((DateTime) Item.Fecha_de_conformación).ToString("dd/MM/yyyy") : "" %></td>
                                                <td><%#Item.Fecha_de_firma != null ? ((DateTime) Item.Fecha_de_firma).ToString("dd/MM/yyyy") : "" %></td>
                                                <td><%#Item.Estatus_del_análisis %></td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                        <div class="container">
                            <uc1:wucPaginador runat="server" ID="wucPaginador" OnPageChange="wucPaginador_OnPageChange" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>



            </div>

            <script>
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Inicializar);

                function pageLoad() {
                    Inicializar();
                }

                function Inicializar() {
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
                        $("#<%=txtFirmaFin.ClientID%>").datepicker();
                        $("#<%=txtFirmaInicio.ClientID%>").datepicker();
                        $("#<%=txtConformacionFin.ClientID%>").datepicker();
                        $("#<%=txtConformacionInicio.ClientID%>").datepicker();

                        mostrarBoton();

                        $("input[id*='chkSeleccionTodos']").change(function () {

                            var isChecked = $(this).is(":checked");

                            $("input[id*='chkSeleccionAnalisis']").prop("checked", isChecked);
                            if (isChecked) {
                                $("[id*='btnHabilitar']").show();
                            } else {
                                $("[id*='btnHabilitar']").hide();
                            }
                        });

                        $("input[id*='chkSeleccionAnalisis']").change(function () {
                            mostrarBoton();
                        });


                        //$("[id*='btnBusqueda']").on("click",
                        //    function (e) {
                        //        e.stopPropagation();

                        //        $(".fecha").each(function() {
                        //            if ($(this).val() !== "" && !FechaValida($(this).val())) {
                        //                e.preventDefault();
                        //            }
                        //        });
                        //    });

                        $("input").unbind("paste").on("paste", function(e) {
                                e.preventDefault();
                            });
                    });

                    function mostrarBoton() {
                        var anyChecked = false;
                        var allChecked = 0;
                        $("input[id*='chkSeleccionAnalisis']").each(function () {
                            if ($(this).is(":checked")) {
                                anyChecked = true;
                                allChecked++;
                            }
                        });
                        if (anyChecked) {
                            $("[id*='btnHabilitar']").show();
                        } else {
                            $("[id*='btnHabilitar']").hide();
                        }

                        $("input[id*='chkSeleccionTodos']").prop("checked", allChecked === $("input[id*='chkSeleccionAnalisis']").size() && $("input[id*='chkSeleccionAnalisis']").size() > 0);
                    }

                    function FechaValida(fechaFormatoHispano) {
                        var regEx = /^\d{2}\/\d{2}\/\d{4}$/;
                        return fechaFormatoHispano.match(regEx);
                    }
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:wucAppletFIEL runat="server" ID="wucAppletFIEL" OnFirmarColeccion="wucAppletFIEL_OnFirmarColeccion" />
</asp:Content>
