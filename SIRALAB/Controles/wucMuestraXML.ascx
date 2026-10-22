<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMuestraXML.ascx.cs" Inherits="SIRALAB.Controles.wucMuestraXML" %>
<link href="style/Formularios.css" rel="stylesheet" type="text/css" />

<div id="pnlFileUpload" class="formulario-seccion">
    <div class="seccion-encabezado">
        <div class="seccion-titulo">Carga Másiva de Muestras</div>
    </div>
    <ul>
        <li>
            <div id="fileUpload" style="margin-top: 5px;">
                <table>
                    <tr>
                        <td>
                            <a id="descargarXML" href="muestra.xsd" target="_blank" class="boton">Descargar XML</a>
                        </td>
                        <td>
                            <asp:AsyncFileUpload ID="FileUploadControl" runat="server" ThrobberID="imgLoad" data-file="" CompleteBackColor="White"
                                UploaderStyle="Traditional"
                                OnUploadedComplete="FileUploadControl_UploadedComplete"
                                OnClientUploadComplete="FileUploadComplete" />
                            <asp:Button ID="btnUpload" runat="server" OnClick="FileUploadControl_Click" Style="visibility: hidden;" />
                        </td>
                        <td>
                            <asp:Image runat="server" ID="imgLoad" ImageUrl="~/image/loading.gif" data-image="" Style="display: none;" />
                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="enviarXML" CssClass="boton" OnClick="EnviarXML_Click" Text="Enviar XML" Visible="False"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="StatusLabel" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </li>
    </ul>
</div>

<script type="text/javascript">

    function FileUploadComplete(sender) {
        /* Cuando finaliza la carga del archivo, entonces recibimos la notificación de que ya se encuentra agregado al listado temporal */
        $("[data-file]").removeAttr("style");
        $("#<%=StatusLabel.ClientID%>").removeAttr("style");
        $("[data-image]").css('display', "block");
        $("#<%=StatusLabel.ClientID%>").text('validando...');
        document.getElementById("<%= btnUpload.ClientID %>").click();
    }

    $(".boton").click(function () {
        $("[data-file]").removeAttr("style");
        $("#<%=StatusLabel.ClientID%>").removeAttr("style");
        $("#<%=StatusLabel.ClientID%>").text("");
        $("[data-button]").css("display", "none");
    });

    <%--$(document).ready(function () {

        $("[data-file]").change(function () {
            $("#<%=btnUpload.ClientID%>").click();
        });

        $(".boton").click(function () {
            $("[data-file]").removeAttr("style");
            $("#<%=StatusLabel.ClientID%>").removeAttr("style");
            $("#<%=StatusLabel.ClientID%>").text("");
            $("[data-button]").css("display", "none");
        });

    });--%>

</script>
