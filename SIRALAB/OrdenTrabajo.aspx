<%@ Page Title="" Language="C#" MasterPageFile="~/Application.Master" AutoEventWireup="true" CodeBehind="OrdenTrabajo.aspx.cs" Inherits="SIRALAB.OrdenTrabajo" ClientIDMode="AutoID"
    Culture="es-MX" UICulture="es-MX" %>

<%@ Register Src="Controles/wucConsultaCliente.ascx" TagName="wucConsultaCliente" TagPrefix="uc1" %>
<%@ Register Src="Controles/wucConsultaLaboratorio.ascx" TagName="wucConsultaLaboratorio" TagPrefix="uc2" %>
<%@ Register Src="Controles/wucConsultaPtoDescarga.ascx" TagName="wucConsultaPtoDescarga" TagPrefix="uc3" %>
<%@ Register Src="Controles/wucOrdenTrabajo.ascx" TagName="wucOrdenTrabajo" TagPrefix="uc4" %>
<%@ Register Src="Controles/wucOrdenAnalisis.ascx" TagName="wucOrdenAnalisis" TagPrefix="uc5" %>
<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" TagPrefix="uc1" TagName="wucAppletFIEL" %>
<%@ Register Src="~/Controles/wucEvidencias.ascx" TagPrefix="uc1" TagName="wucEvidencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <script src="js/ValidadorRequerido.js"></script>
    <script src="js/jquery.flexslider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updOrdenTrabajo" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlTitulo" runat="server" Style="width: 700px; height: 10px;">
            </asp:Panel>
            <div class="row">
                <div class="col-sm-1">
                    <asp:LinkButton ID="lnkbtnback" runat="server" CssClass="btn btn-default btn-ms" OnClick="lnkbtnback_Click" ToolTip="Regresar a la pagina anterior">
                        <span class="glyphicon glyphicon-chevron-left"></span>
                    </asp:LinkButton>
                </div>
                <div class="col-sm-11">
                    <h2>Registro de orden de trabajo</h2>
                    <hr class="red">
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 text-right">
                    <h6>
                        <asp:Label ID="lblFolio" runat="server"></asp:Label></h6>
                </div>
            </div>

            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Datos generales del cliente</h5>
                    </div>
                    <div class="panel-body">
                        <uc1:wucConsultaCliente ID="wucConsultaCliente1" runat="server" OnSeleccion="wucConsultaCliente1_Seleccion" OnError="wucConsultaCliente1_Error" />
                    </div>
                </div>
            </div>

            <div class="row" id="divPnlPuntoDescarga" runat="server">
                <asp:Panel ID="pnlPtoDescarga" runat="server">
                </asp:Panel>
                <%--INICIO. SECCIÓN PUNTO DE DESCARGA--%>
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h5><span>Punto de descarga o aprovechamiento</span></h5>
                        </div>
                        <div class="panel-body">
                            <uc3:wucConsultaPtoDescarga ID="wucConsultaPtoDescarga1" runat="server" OnSeleccion="wucConsultaPtoDescarga1_Seleccion" OnGuardar="wucConsultaPtoDescarga1_Guardar" OnLimpiarNormas="wucConsultaPtoDescarga1_Limpiar" />
                        </div>
                    </div>
                </div>
                <div class="alert alert-danger">
                    <strong>¡Precaución!</strong>Es importante que previo a generar la orden de trabajo, confirme que los datos<br />
                    que señaló son los correctos.
                                        <br />
                    Una vez generada la orden de trabajo éstos no podrán ser modificados.
                </div>
                <%--FIN. SECCIÓN PUNTO DE DESCARGA--%>
            </div>

            <div class="row">
                <%--INICIO. SECCIÓN CORRESPONDIENTE A LAS NORMAS OFICIALES MEXICANAS--%>
                <asp:Panel ID="pnlNormasMexicanas" runat="server">
                </asp:Panel>
                <div class="row" id="divNormasMexicanas" runat="server">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h5><span>Normas oficiales Mexicanas</span></h5>
                            </div>
                            <div class="panel-body">
                                <uc4:wucOrdenTrabajo ID="wucOrdenTrabajo1" runat="server" OnIncompleto="wucOrdenTrabajo1_Incompleto" OnRespuestaAnalisis="wucOrdenTrabajo1_RespuestaAnalisis" OnFechaErronea="wucOrdenTrabajo1_FechaErronea" OnNormaSeleccionada="wucOrdenTrabajo1_NormaSeleccionada" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--  FIN. SECCIÓN CORRESPONDIENTE A LAS NORMAS OFICIALES MEXICANAS--%>
            </div>

            <div class="row">
                <%--INICIO. SECCIÓN ANÁLISIS--%>
                <div class="row">
                    <div class="col-md-12">
                        <uc5:wucOrdenAnalisis ID="wucOrdenAnalisis1" runat="server"
                            OnFirmarAnalisis="wucOrdenAnalisis1_FirmarAnalisis"
                            OnCancelar="wucOrdenAnalisis_Cancelar"
                            OnEliminar="wucOrdenAnalisis_Eliminar"
                            OnFaltaPago="wucOrdenAnalisis_FaltaPago" />
                    </div>
                </div>
                <%--FIN. SECCIÓN ANÁLISIS--%>
            </div>

            <div class="row">
                <%--INICIO. SECCIÓN PERTENECIENTE A DOCUMENTOS--%>
                <div class="col-md-12">
                    <uc1:wucEvidencias runat="server" ID="wucEvidencias" />
                </div>
                <%--FIN. SECCIÓN PERTENECIENTE A DOCUMENTOS--%>
            </div>

            <uc1:wucAppletFIEL runat="server" ID="wucFirma" OnFirmarColeccion="wucFirma_FirmarColeccion" Visible="false" />
            <uc1:wucAppletFIEL runat="server" ID="wucFirmaFaltaPago" OnFirmarColeccion="wucFirmaFaltaPago_Firmar" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">


        function toDate(dateStr) {
            var parts = dateStr.split("/");
            return new Date(parts[2], parts[1] - 1, parts[0]);
        }

        function setDatePicker(id) {
            if (!$("#" + id).hasClass("hasDatepicker")) {
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
                    dateFormat: 'dd/mm/y',
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

                $('#' + id).datepicker('show');

                $('[data-toggle="tooltip"]').tooltip();
            }
        };
    </script>



</asp:Content>
