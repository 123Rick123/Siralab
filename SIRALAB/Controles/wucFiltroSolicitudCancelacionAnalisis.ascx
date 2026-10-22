<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFiltroSolicitudCancelacionAnalisis.ascx.cs" Inherits="SIRALAB.Controles.wucFiltroSolicitudCancelacionAnalisis" %>

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

        var fechaFinInicialEmision = null;
        var fechaFinFinalEmision = null;

        if ($('[id*=endTxtFechaInicialEmision]').length && $('[id*=endTxtFechaInicialEmision]').val().length > 0) {
            fechaFinInicialEmision = toDate($('[id*=endTxtFechaInicialEmision]').val());
        }
        if ($('[id*=endTxtFechaFinalEmision]').length && $('[id*=endTxtFechaFinalEmision]').val().length > 0) {
            fechaFinFinalEmision = toDate($('[id*=endTxtFechaFinalEmision]').val());
        }

        $('#<%= txtFechaInicialEmision.ClientID%>').datepicker({
            maxDate: fechaFinInicialEmision,
            autoclose: true
        });
        $('#<%= txtFechaFinalEmision.ClientID%>').datepicker({
            maxDate: fechaFinFinalEmision,
            autoclose: true
        });

        $('[data-toggle="tooltip"]').tooltip();
    });


    $('.botonF1').hover(function () {
        $('.btn').addClass('animacionVer');
    })
    $('.contenedor').mouseleave(function () {
        $('.btn').removeClass('animacionVer');
    })
</script>

<asp:UpdatePanel ID="upBusqueda" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" class="buscar">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkLimpiar" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlBusqueda" runat="server" CssClass="formulario-seccion" DefaultButton="lnkbBuscar">
            <asp:HiddenField runat="server" ID="endTxtFechaInicialEmision" Value="" />            
            <asp:HiddenField runat="server" ID="endTxtFechaFinalEmision" Value="" />
            <div class="row">
                <div class="col-md-12">
                    <div class="panel-group ficha-collapse" id="accordion">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                   Búsqueda
                                   
                                </h4>
                            </div>
                            <div class="panel-collapse collapse in" id="panel-01">
                                <div class="panel-body">


                                    <div class="row">


                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="name">No. Informe <span class="form-text"></span>:</label>
                                                <asp:TextBox ID="txtNumeroInforme" runat="server" CssClass="form-control" MaxLength="390"  placeholder="Ingresa tu Informe"/>
                                            </div>
                                        </div>




                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="name">Inicial <span class="form-text"></span>:</label>
                                                <div class="form-group datepicker-group">
                                                    <asp:TextBox ID="txtFechaInicialEmision" runat="server" class="form-control "></asp:TextBox>
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    <%-- <asp:CalendarExtender  ID="cetxtFechaInicialEmision" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFechaInicialEmision" ></asp:CalendarExtender>--%>
                                                </div>
                                            </div>

                                        </div>



                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="name">Final <span class="form-text"></span>:</label>
                                                <div class="form-group datepicker-group">
                                                    <asp:TextBox ID="txtFechaFinalEmision" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    <%-- <asp:CalendarExtender ID="cetxtFechaFinalEmision" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFechaFinalEmision"></asp:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="name">Norma<span class="form-text"></span>:</label>
                                                <td>
                                                    <asp:DropDownList ID="ddlNorma" runat="server" class="form-control"></asp:DropDownList></td>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="name">RFC <span class="form-text"></span>:</label>
                                                <asp:TextBox ID="txtRfc" runat="server" class="form-control" MaxLength="390"  placeholder="Ingresa tu RFC"/>
                                            </div>
                                        </div>



                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label for="name">Título <span class="form-text"></span>:</label>
                                                <asp:TextBox ID="txtTitulo" runat="server" class="form-control" MaxLength="390"  placeholder="Ingresa el Título" />
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-4 col-md-offset-9">
                                                <asp:LinkButton ID="lnkbBuscar" runat="server" CssClass="btn btn-default" ToolTip="Buscar" Text="Buscar" ValidationGroup="NormaGroup" OnClick="lnkbBuscar_Click"></asp:LinkButton>

                                                <asp:LinkButton ID="lnkLimpiar" runat="server" CssClass="btn btn-primary" ToolTip="Limpiar búsqueda" Text="Limpiar" OnClick="lnkLimpiar_Click"></asp:LinkButton>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
