<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucEvidenciaPop.ascx.cs" Inherits="SIRALAB.Controles.wucEvidenciaPop" %>
<script src='<%= Page.ResolveUrl("~/js/jquery.flexslider.js") %>'></script>


<script type="text/javascript">

    var ifErrorExtension_pop = false;

    function UpLoadStarted_pop(sender, e) {

        /*inicializa */
        ifErrorExtension_pop = false;
        ifErrorNombre_pop = false;
        $('#txtMessageAsyncFile_pop').text('');

        /*Extensiones validas*/
        var Extensions_pop = /(gif|GIF|jpg|JPG|jpeg|JPEG|tiff|TIFF|png|PNG|bmp|BMP|pdf|PDF)$/i;

        /*Archivo*/
        var fileName_pop = e.get_fileName();
        var fileExtension_pop = fileName_pop.substring(fileName_pop.lastIndexOf('.') + 1).toLowerCase();



        //alert("regex : " + fileExtension_pop + " : " + Extensions_pop.test(fileExtension_pop));

        if (!Extensions_pop.test(fileExtension_pop)) {

            ifErrorExtension_pop = true;
            sender._stopLoad();
        }


    }
    function uploadComplete_pop() {
        clickPostBack_pop();
        clearContents_pop();
        $('#txtMessageAsyncFile_pop').text('ok');
    }

    function uploadError_pop() {
        if (ifErrorExtension_pop) {
            $('#txtMessageAsyncFile_pop').text("¡Extension de archivo no soportado!");
        }

        else {
            $('#txtMessageAsyncFile_pop').text('error');
        }

    }

    function clearContents_pop() {
        var span = $get("<%=asyncFileEvidencias_pop.ClientID%>");
        var txts = span.getElementsByTagName("input");
        for (var i = 0; i < txts.length; i++) {
            if (txts[i].type == "file") {
                txts[i].value = "";
            }
        }
    }

    function clickPostBack_pop() {
        $get("<%=HiddenButton_pop.ClientID%>").click();
    }

    $(document).ready(function () {
        clickPostBack_pop();
        function cargarEvidencias_pop() {
            $('#reviews-slider_pop').flexslider({
                animation: "slide",
                itemWidth: 200,
                itemMargin: 10,
                minItems: 4,
                maxItems: 50,
                move: 0
            });
        }
    });
</script>
<asp:Button ID="HiddenButton_pop" runat="server"
    Style="display: none"
    Text="HiddenButton_pop" OnClick="HiddenButton_pop_Click" />

<asp:UpdatePanel ID="upnlFileUpload_pop" runat="server"
    UpdateMode="Conditional"
    ChildrenAsTriggers="true">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rptrEvidecias_pop" />
        <asp:AsyncPostBackTrigger ControlID="asyncFileEvidencias_pop" />
        <asp:AsyncPostBackTrigger ControlID="HiddenButton_pop" />
        <asp:AsyncPostBackTrigger ControlID="lnkbtnGuardarEvidencia_pop" />
    </Triggers>
    <ContentTemplate>
        <div id="reviews-slider_pop" class="flexslider">
            <ul class="slides">
                <asp:Repeater ID="rptrEvidecias_pop"
                    runat="server"
                    ItemType="SIRALAB_DAL.Documento">
                    <ItemTemplate>
                        <li>
                            <div class="evidencia">
                                <div class="item-preview">

                                    <asp:LinkButton ID="lnkAbrirEvidencia_pop"
                                        runat="server"
                                        ToolTip="Abrir evidencia"
                                        OnCommand="lnkAbrirEvidencia_pop_Command"
                                        CommandArgument='<%#:Item.DocumentoID%>'>
                                <img alt="Preview" src='<%#:Item.ImagePreView%>'  />
                                
                                <div class="nombre-evidencia" >
                                    '<%#:Item.Nombre %>'
                                </div>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div id="fileUpload" style="margin-top: 5px;">
            <table>
                <tr>
                    <td><asp:DropDownList ID="ddlTipoDocumento_pop" runat="server" Width="120px"></asp:DropDownList></td>
                    <td>
                        <asp:AsyncFileUpload ID="asyncFileEvidencias_pop" runat="server" ThrobberID="imgLoad_pop"
                            CompleteBackColor="Green"
                            UploaderStyle="Traditional"
                            OnUploadedComplete="asyncFileEvidencias_pop_UploadedComplete"
                            OnClientUploadComplete="uploadComplete_pop"
                            OnClientUploadStarted="UpLoadStarted_pop"
                            OnClientUploadError="uploadError_pop" />
                    </td>
                    <td><asp:Image runat="server" ID="imgLoad_pop" ImageUrl="~/image/loading.gif" /></td>
                </tr>
            </table>
        </div>
        <span id="txtMessageAsyncFile_pop"></span>
        <div style="overflow: hidden; text-align: right; padding: 15px 4px;">
            <asp:LinkButton ID="lnkbtnGuardarEvidencia_pop" runat="server" CssClass="boton" OnClick="lnkbtnGuardarEvidencia_pop_Click" Text="Guardar" ToolTip="Guardar Documentos"></asp:LinkButton>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
