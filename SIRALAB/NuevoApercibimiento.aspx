<%@ Page Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="NuevoApercibimiento.aspx.cs" Inherits="SIRALAB.NuevoApercibimiento" %>

<%@ Register Src="Controles/wucBusquedaAltaApercibidos.ascx" TagName="wucBusquedaAltaApercibidos" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <script src="js/ValidadorRequerido.js"></script>
    <script src="js/jquery.flexslider.js"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updBusquedaApercibidosYReincidentes" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Nuevo apercibimiento</h5>
                    </div>
                    <div class="panel-body">
                      <%--  LLAMADA A USER CONTROL DE BUSQUEDA--%>
                        <uc1:wucBusquedaAltaApercibidos ID="wucBusquedaAltaApercibidos1" runat="server" />
                        
                        </div>
                </div>
            </div>
          
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
