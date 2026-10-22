<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMensajes.ascx.cs" Inherits="SIRALAB.controles.wucMensajes" %>

<div id="ModalMensaje" role="dialog" class="modal" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <div id="AlertMensaje" runat="server" class="alert alert-success alert-dismissible" >
            <button id="btnCierraAlertMensaje" type="button" class="close" data-dismiss="modal" onclick="eliminaBackdrop()">
                <span aria-hidden="true">&times;</span>
            </button>
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </div>
    </div>
</div>

<div id="ModalDialog" class="modal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">
                    <asp:Label ID="lblTitulo" runat="server"></asp:Label></h4>
            </div>
            <div class="modal-body">
                <asp:Label ID="lblmensajeconfirm" runat="server"></asp:Label>
            </div>
            <div class="modal-footer">
                <asp:LinkButton ID="lnkbAceptar" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbAceptar_Click"></asp:LinkButton>
                <asp:LinkButton ID="lnkbCancelar" runat="server" CssClass="btn btn-default" Text="Cerrar"></asp:LinkButton>
                <asp:LinkButton ID="lnkbSi" runat="server" CssClass="btn btn-primary" Text="Si" OnClick="lnkbAceptar_Click"></asp:LinkButton>
                <asp:LinkButton ID="lnkbNo" runat="server" CssClass="btn btn-default" Text="No" OnClick="lnkbNo_Click"></asp:LinkButton>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->

<asp:HiddenField ID="hndMensaje" runat="server" />
<asp:HiddenField ID="hndArgs" runat="server" />
<asp:HiddenField ID="hndCommand" runat="server" />

<script>
    $().ready(function () {

        $("[id*='ModalMensaje']").on('hidden.bs.modal', function (e) {

            if ($('.modal-backdrop').is(':visible')) {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').hide();
            }
        });
        $("[id*='lblMensaje']").change(function () {
            alert("Hola Mundoi");
            $("[id*='AlertMensaje']").show();
        });
    });

    function eliminaBackdrop() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').hide();
    }
</script>
