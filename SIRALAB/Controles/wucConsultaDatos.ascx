<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaDatos.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaDatos" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/autocomplete.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .div-colapsado_ {
        display: none;
    }
</style>

<script type="text/javascript">
    function t(d) {
        if (d == 1) {
            $('.div-colapsado_').slideToggle(600);
        };
    }
</script>

<asp:Panel ID="pnlCapturaPersona" runat="server" Visible="true" Style="width: 900px;">
    <asp:UpdatePanel ID="updCliente" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
       
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlDatosCliente" runat="server">
                <table style="width: 640px;">
                    <tr>
                        <td style="width: 320px;"><asp:Label ID="lblRFC_" runat="server" Text="RFC" style="padding:0px 0px 0px 15px" ></asp:Label><asp:Label ID="lblRFC" class="div-seccion" runat="server" Width="260px"></asp:Label>
                        </td>
                        <td style="width: 320px;">CURP<asp:Label ID="lblClienteCurp" class="div-seccion" runat="server" Width="260px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Nombre<asp:Label ID="lblClienteNombre" class="div-seccion" runat="server" Width="560px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 320px;">Permiso<asp:Label ID="lblPermisos" class="div-seccion" runat="server" Width="250px"></asp:Label>
                        </td>
                        <td style="width: 320px;">Anexo<asp:Label ID="lblAnexo" class="div-seccion" runat="server" Width="250px"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 320px;">Informe<asp:Label ID="lblInforme" class="div-seccion" runat="server" Width="260px"></asp:Label>
                        </td>
                        <td style="width: 320px;">Fecha<asp:Label ID="lblFecha" class="div-seccion" runat="server" Width="260px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
