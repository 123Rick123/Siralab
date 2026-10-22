<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucEvidencias.ascx.cs" Inherits="SIRALAB.Controles.wucEvidencias" ClientIDMode="AutoID" %>
<%--<script src='<%= Page.ResolveUrl("js/jquery.flexslider.js") %>'></script>--%>



<asp:UpdatePanel ID="upnlFileUpload" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="rptrEvidecias"/>--%>
        <asp:AsyncPostBackTrigger ControlID="rptrEvidecias" />
        <asp:AsyncPostBackTrigger ControlID="asyncFileEvidencias" />
        <asp:AsyncPostBackTrigger ControlID="HiddenButton" />
        <asp:AsyncPostBackTrigger ControlID="AsyncFileUploadAnalisis" />
    </Triggers>
    <ContentTemplate>
        <script>
            function openModal() {
                $('#mdlArchivosAnalisis').modal('show');
            }

            function setAnalisisIDModal(analisisID) {
                $("#<%=txtAnalisID.ClientID%>").val(analisisID);
            }

            function openModalEvidencia(event, id) {
                if (parseInt($("#" + id).data("documento-id")) == 0) {
                    event.preventDefault();
                    $('#mdlVerificaDocumentoID').modal('show');
                    return false;
                }
                return true;
            }

        </script>

        <asp:Panel ID="pnlDocumentos" runat="server" Visible="false">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h5>Documentos</h5>
                        </div>
                        <div class="panel-body">
                            <div style="overflow: auto">
                                <div class="col-md-12">
                                    <div id="reviews-slider" class="flexslider row" style="margin: auto; overflow: hidden">
                                        <ul class="slides">
                                            <asp:Repeater ID="rptrEvidecias" runat="server" ItemType="SIRALAB_DAL.Documento">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="row evidencia">
                                                            <div class="col-md-12">
                                                                <div class="row">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkAbrirEvidencia" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="lnkAbrirEvidencia" CssClass="btn btn-default btn-sm" runat="server" ToolTip="Abrir evidencia" OnClick="lnkAbrirEvidencia_OnClick" data-documento-id='<%#DataBinder.Eval(Container, "DataItem.DocumentoID")%>' data-documento-guid='<%#DataBinder.Eval(Container, "DataItem.UniqueId")%>' OnClientClick="return openModalEvidencia(event, this.id)">
                                                                                <img alt="img-responsive" src="<%#DataBinder.Eval(Container, "DataItem.ImagePreView")%>" style="max-height: 300px; max-width: 300px;"/>
                                                                            </asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:LinkButton ID="lnkEliminarEvidencia" runat="server" ToolTip="Eliminar evidencia" OnClick="lnkEliminarEvidencia_OnClick" data-documento-id="<%#:Item.DocumentoID%>" data-documento-guid="<%#:Item.UniqueId%>" CssClass="item-preview-delete" Visible="<%# Item.eliminar != true %>"></asp:LinkButton>
                                                                </div>

                                                            </div>
                                                            <div class="row nombre-evidencia">
                                                                <div class="col-md-12 ">
                                                                    <%#:Item.TipoDocumento %>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>

                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <div runat="server" id="divSubirArchivo">
                                        <div id="fileUpload" style="margin-top: 5px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server">
                                                        </asp:DropDownList></td>
                                                    <td>
                                                        <asp:AsyncFileUpload ID="asyncFileEvidencias" runat="server" ThrobberID="imgLoad" CompleteBackColor="Green"
                                                            UploaderStyle="Traditional"
                                                            OnUploadedComplete="asyncFileEvidencias_UploadedComplete"
                                                            OnClientUploadComplete="uploadComplete"
                                                            OnClientUploadStarted="UpLoadStarted"
                                                            OnClientUploadError="uploadError" Style="margin: auto" />
                                                    </td>
                                                    <td>
                                                        <asp:Image runat="server" ID="imgLoad" ImageUrl="~/image/loading.gif" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <span class="txtMessageAsyncFile"></span>
                                    </div>
                                    <div>
                                        <span class="text-muted" style="font-size: 12px">* El tamaño de archivo no puede superar los 10Mb.</span><br>
                                        <span class="text-muted" style="font-size: 12px">** Extensiones de archivo soportadas: pdf, jpeg, jpg, png, gif.</span>
                                    </div>
                                    <div style="overflow: hidden; text-align: right; padding: 15px 4px;">
                                        <asp:LinkButton ID="lnkbtnGuardarEvidencia" runat="server" CssClass="btn btn-primary" OnClick="lnkGuardarEvidencia_Click" CausesValidation="false" Text="Guardar" ToolTip="Guardar Documentos"></asp:LinkButton>
                                    </div>


                                </div>

                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </asp:Panel>
        <%--<div class="seccion-encabezado">
            <div class="seccion-titulo">Documentos</div>
        </div>--%>
        <%-- <ul>
            <li>
                <div id="reviews-slider" class="flexslider">
                    <ul class="slides">
                        <asp:Repeater ID="rptrEvidecias" runat="server" ItemType="SIRALAB_DAL.Documento">
                            <ItemTemplate>
                                <li>
                                    <div class="evidencia">
                                        <div class="item-preview">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="lnkAbrirEvidencia" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lnkAbrirEvidencia" runat="server" ToolTip="Abrir evidencia" OnClick="lnkAbrirEvidencia_OnClick" data-documento-id='<%#DataBinder.Eval(Container, "DataItem.DocumentoID")%>' data-documento-guid='<%#DataBinder.Eval(Container, "DataItem.UniqueId")%>'>
                                                            <img alt="Preview" src='<%#DataBinder.Eval(Container, "DataItem.ImagePreView")%>' />
                                                    </asp:LinkButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:LinkButton ID="lnkEliminarEvidencia" runat="server" ToolTip="Eliminar evidencia" OnClick="lnkEliminarEvidencia_OnClick" data-documento-id="<%#:Item.DocumentoID%>" data-documento-guid="<%#:Item.UniqueId%>" CssClass="item-preview-delete"></asp:LinkButton>
                                        </div>
                                        <div class="nombre-evidencia">
                                            <%#:Item.TipoDocumento %>
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
                            <td>
                                <asp:DropDownList ID="ddlTipoDocumento" runat="server"></asp:DropDownList></td>
                            <td>
                                <asp:AsyncFileUpload ID="asyncFileEvidencias" runat="server" ThrobberID="imgLoad" CompleteBackColor="Green"
                                    UploaderStyle="Traditional"
                                    OnUploadedComplete="asyncFileEvidencias_UploadedComplete"
                                    OnClientUploadComplete="uploadComplete"
                                    OnClientUploadStarted="UpLoadStarted"
                                    OnClientUploadError="uploadError" />
                            </td>
                            <td>
                                <asp:Image runat="server" ID="imgLoad" ImageUrl="~/image/loading.gif" /></td>
                        </tr>
                    </table>
                </div>
                <span id="txtMessageAsyncFile"></span>
                <div style="overflow: hidden; text-align: right; padding: 15px 4px;">
                    <asp:LinkButton ID="lnkbtnGuardarEvidencia" runat="server" CssClass="btn btn-primary" OnClick="lnkGuardarEvidencia_Click" CausesValidation="false" Text="Guardar" ToolTip="Guardar Documentos"></asp:LinkButton>
                </div>
            </li>
        </ul>--%>

        <div id="mdlArchivosAnalisis" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header" id="divModalHeader">
                        <h5 class="modal-title">Asociar archivo al análisis.</h5>
                    </div>
                    <div class="modal-body" id="divMensajes">
                        <asp:TextBox runat="server" ID="txtAnalisID" hidden="hidden"></asp:TextBox>
                        <label>Tipo documento:</label>
                        <asp:DropDownList ID="ddlAnalisis" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Hoja de campo y cadena de custodia" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Laboratorio de apoyo" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                        <div>
                            <asp:AsyncFileUpload ID="AsyncFileUploadAnalisis" runat="server" ThrobberID="imgLoadMdl" CompleteBackColor="Green"
                                UploaderStyle="Traditional"
                                OnUploadedComplete="asyncFileEvidenciasAnalisis_UploadedComplete"
                                OnClientUploadComplete="uploadComplete"
                                OnClientUploadStarted="UpLoadStarted"
                                OnClientUploadError="uploadError" CssClass="form-control" />
                            <asp:Image runat="server" ID="imgLoadMdl" ImageUrl="~/image/loading.gif" />
                            <span class="txtMessageAsyncFile"></span>
                        </div>
                        <div>
                            <span class="text-muted" style="font-size: 12px">* El tamaño de archivo no puede superar los 10Mb.</span><br />
                            <span class="text-muted" style="font-size: 12px">** Extensiones de archivo soportadas: pdf, jpeg, jpg, png, gif.</span>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="clearTxt();">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="mdlVerificaDocumentoID" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header" id="divModalHeader">
                        <h5 class="modal-title">Operación no disponible.</h5>
                    </div>
                    <div class="modal-body" id="divMensajes">
                        <p>No se pudo obtener la evidencia. Debe guardar todos sus cambios, antes de intentar abrir el archivo.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:Button ID="HiddenButton" runat="server" Style="display: none" Text="HiddenButton" OnClick="HiddenButton_Click" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">

    var ifErrorExtension = false;

    function UpLoadStarted(sender, e) {

        /*inicializa */
        ifErrorExtension = false;
        ifErrorNombre = false;
        $('.txtMessageAsyncFile').text('');

        /*Extensiones validas*/
        /*var Extensions = /(gif|jpg|jpeg|tiff|png|bmp|pdf|doc|docx|xls|xlsx|pps|ppsx|ppt|pptx|rar|zip|7zip)$/i;*/
        var Extensions = /(gif|jpg|jpeg|tiff|png|bmp|pdf)$/i;

        /*Archivo*/
        var fileName = e.get_fileName();
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1).toLowerCase();

        //alert("regex : " + fileExtension + " : " + Extensions.test(fileExtension));

        if (!Extensions.test(fileExtension)) {

            ifErrorExtension = true;
            sender._stopLoad();
        }


    }
    function uploadComplete() {
        clickPostBack();
        clearContents();
        $('#txtMessageAsyncFile').text('ok');
    }
    function uploadError(sender, args) {
        if (ifErrorExtension) {
            $('.txtMessageAsyncFile').text("¡Extensión de archivo no soportada!");
        }

        else {
            $('.txtMessageAsyncFile').text('error');
        }

        //alert(args);

    }

    function clearTxt() {
        $('.txtMessageAsyncFile').text('');
    }

    function clearContents() {
        var span = $get("<%=asyncFileEvidencias.ClientID%>");
        var txts = span.getElementsByTagName("input");
        for (var i = 0; i < txts.length; i++) {
            if (txts[i].type == "file") {
                txts[i].value = "";
            }
        }
    }

    function clickPostBack() {
        $get("<%=HiddenButton.ClientID%>").click();
    }

    $(document).ready(function () {
        clickPostBack();
    });

</script>
