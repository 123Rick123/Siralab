<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAppletFIEL.ascx.cs" Inherits="SIRALAB.FIEL.wucAppletFIEL" %>

<script type="text/javascript">

    var idPanelClienteFirmar = '<%= pnlWucFirma.ClientID %>';
    function mostrarFirma() {
        $('#' + idPanelClienteFirmar).modal();
    }
    function ocultarFirma() {
        $('#' + idPanelClienteFirmar).modal('hide');
        //$('.modal-backdrop').remove();
    }

    var CadenaFirmada = function (val) {
        if (val != null) {
            document.getElementById('<%= hfCadenaFirmada.ClientID %>').value = val;
        }
        return document.getElementById('<%= hfCadenaFirmada.ClientID %>').value;
    };
    var FirmaRFC = function (val) {
        if (val != null) {
            document.getElementById('<%= hfRFCFirma.ClientID %>').value = val;
        }
        return document.getElementById('<%= hfRFCFirma.ClientID %>').value;
    };

    function resultadoFiel(resultado, rfc, firma) {
        CadenaFirmada(firma);
        if (CadenaFirmada != null) {
            FirmaRFC(rfc);
            document.getElementById('<%= lnkbtnFirmado.ClientID %>').click();
            //alert('Firmado Correctamente. mandar evento');
        }
    }






    var ifErrorExtension2 = false;

    function UpLoadStarted2(sender, e) {
        
        /*inicializa */
        ifErrorExtension2 = false;
        ifErrorNombre = false;


        /*Extensiones validas*/
        var Extensions = /(cer)$/i;

        /*Archivo*/
        var fileName = e.get_fileName();
        var fileExtension2 = fileName.substring(fileName.lastIndexOf('.') + 1).toLowerCase();

        //alert("regex : " + fileExtension + " : " + Extensions.test(fileExtension));

        if (!Extensions.test(fileExtension2)) {

            ifErrorExtension2 = true;

            sender._stopLoad("error------ Archivo no válido");

        }
    }

    function UpLoadStarted3(sender, e) {
        
        /*inicializa */
        ifErrorExtension2 = false;
        ifErrorNombre = false;


        /*Extensiones validas*/
        var Extensions = /(key)$/i;

        /*Archivo*/
        var fileName = e.get_fileName();
        var fileExtension2 = fileName.substring(fileName.lastIndexOf('.') + 1).toLowerCase();

        //alert("regex : " + fileExtension + " : " + Extensions.test(fileExtension));

        if (!Extensions.test(fileExtension2)) {

            ifErrorExtension2 = true;

            sender._stopLoad("error------ Archivo no válido");

        }
    }













</script>
<script>
        $().ready(function(){
            
            $("#" + idPanelClienteFirmar).on('hidden.bs.modal', function (e) {
              
                if ($('.modal-backdrop').is(':visible')) {
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').hide();
                }
            });
        });
        
</script>
<style type="text/css">
    .btn-hide {
        display: none;
    }
</style>

<asp:UpdatePanel ID="upFirmaElectronica" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbtnFirmado" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlWucFirma" runat="server" class="modal ">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton ID="lnkbCerrar" Visible="false" runat="server" CssClass="popform-close-button" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
                        <span>Firmar</span>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1">
                                <div class="form-group">
                                    <label for="name">Archivo Cer<span class="form-text">*</span>:</label>
                                    <asp:AsyncFileUpload ID="asyncFileCer" runat="server" ThrobberID="imgLoadCer" CompleteBackColor="Transparent" UploaderStyle="Traditional" OnUploadedComplete="asyncFileCer_UploadedComplete" OnClientUploadStarted="UpLoadStarted2" CssClass="form-control" Accept=".cer" />
                                    <asp:Image runat="server" ID="imgLoadCer" ImageUrl="~/image/loading.gif" />
                                    <span id="errer" class="help-block with-errors"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1">
                                <div class="form-group">
                                    <label for="name">Archivo Key<span class="form-text">*</span>:</label>
                                    <asp:AsyncFileUpload ID="AsyncFileKey" runat="server" ThrobberID="imgLoadKey" CompleteBackColor="Transparent" UploaderStyle="Traditional" OnUploadedComplete="asyncFileKey_UploadedComplete" OnClientUploadStarted="UpLoadStarted3" CssClass="form-control" Accept=".key" />
                                    <asp:Image runat="server" ID="imgLoadKey" ImageUrl="~/image/loading.gif" />
                                    <span id="errer" class="help-block with-errors"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1">
                                <div class="form-group">
                                    <label for="name">Contraseña<span class="form-text">*</span>:</label>
                                    <asp:TextBox ID="txtContra" TextMode="Password" runat="server" CssClass="form-control" Style="text-align: center;"></asp:TextBox>
                                    <span class="help-block with-errors" style="display: none;">Este campo es obligatorio </span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1">
                                <div class="form-group">
                                    <label for="name">RFC<span class="form-text">*</span>:</label>
                                    <asp:TextBox ID="txtRFC" runat="server" class="form-control" Style="text-align: center;"></asp:TextBox>
                                    <span id="errer" class="help-block with-errors"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lnkbAceptar" runat="server" Text="Firmar" CssClass="btn btn-primary" OnClick="lnkbtnFirmado_Click"></asp:LinkButton>
                        <a id="lnkbCancelar" class="btn btn-default" onclick="ocultarFirma();">Cancelar</a>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:ModalPopupExtender ID="mpeWucFirma" Enabled="false" runat="server" PopupControlID="pnlWucFirma" TargetControlID="hf" BackgroundCssClass="popUpBackground"></asp:ModalPopupExtender>
        <asp:HiddenField ID="hf" runat="server" />
        <div>
            <asp:HiddenField ID="hfRFCFirma" runat="server" />
            <asp:HiddenField ID="hfCadenaFirmada" runat="server" />
            <asp:HiddenField ID="hfInfoAFirmar" runat="server" />
            <asp:HiddenField ID="hfCadenaOriginalParaSello" runat="server" />
            <asp:LinkButton ID="lnkbtnFirmado" runat="server" OnClick="lnkbtnFirmado_Click" CssClass="btn-hide"></asp:LinkButton>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
