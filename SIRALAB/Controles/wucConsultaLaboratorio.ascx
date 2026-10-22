<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaLaboratorio.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaLaboratorio" %>
<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .div-colapsado2
    {
        display: none;
    }
</style>

<script type="text/javascript">
    function te(d) {
        if (d == 2)
        { $('.div-colapsado2').slideToggle(400); };
    }
    //<td style="width: 16px;"><a href="javascript:t(1)" id="button2" class="icono-agregar-16 collapse-action">f</a></td>
</script>

<asp:Panel ID="pnlCapturaPersona" runat="server" Visible="true" Style="width: 900px;">
     <a href="javascript:te(2)" id="button2" class="icono-agregar-16 collapse-action">V</a>
    <table style="width: 640px;">
        <tr>
            <td style="width: 290px;">RFC</td>
            <td style="width: 320px;">CURP</td>
        </tr>
        <tr>
            <td>
                <div class="div-seccion" style="width: 280px;">
                    <asp:Label ID="lbl" runat="server" Text="RFC-LABORATORIO"></asp:Label>
                </div>
            </td>
            <td>
                <div class="div-seccion" style="width: 280px;">
                    <asp:Label ID="Label1" runat="server" Text="CURP-LABORATORIO"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">Nombre
            </td>

        </tr>
        <tr>
            <td colspan="2">
                <div class="div-seccion" style="width: 585px;">
                    <asp:Label ID="Label2" runat="server" Text="NOMBRE O RAZÓN SOCIAL-LABORATORIO"></asp:Label>
                </div>
            </td>
        </tr>
    </table>

    <div class="div-colapsado2">    
        <table style="width: 620px;">
            <tr>
                <td style="width: 290px;">Calle</td>
                <td style="width: 250px;">No. Exterior</td>
                <td style="width: 250px;">No. Interior</td>
            </tr>
            <tr>
                <td style="width: 290px;">
                    <div class="div-seccion" style="width: 280px;">
                        <asp:Label ID="Label3" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>
                <td style="width: 250px;">
                    <div class="div-seccion" style="width: 130px;">
                        <asp:Label ID="Label4" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>
                <td>
                    <div class="div-seccion" style="width: 130px;">
                        <asp:Label ID="Label6" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 620px;">
            <tr>
                <td style="width: 70px;">C.P.</td>
                <td style="width: 120px;">Estado</td>
                <td style="width: 160px;">Municipio</td>
                <td style="width: 160px;">Colonia</td>
            </tr>
            <tr>
                <td>
                    <div class="div-seccion" style="width: 70px;">
                        <asp:Label ID="Label5" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>


                <td>
                    <div class="div-seccion" style="width: 120px;">
                        <asp:Label ID="Label7" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>

                <td>
                    <div class="div-seccion" style="width: 160px;">
                        <asp:Label ID="Label8" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>

                <td>
                    <div class="div-seccion" style="width: 160px;">
                        <asp:Label ID="Label9" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 620px;">
            <tr>
                <td style="width: 585px;">Localidad</td>
            </tr>
            <tr>
                <td>
                    <div class="div-seccion" style="width: 585px;">
                        <asp:Label ID="Label10" runat="server" Text="Centenario"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 640px;">
            <tr>
                <td style="width: 290px;">Telefono</td>
                <td style="width: 320px;">Correo</td>
            </tr>
            <tr>
                <td>
                    <div class="div-seccion" style="width: 280px;">
                        <asp:Label ID="Label11" runat="server" Text="RFC-CLIENTE"></asp:Label>
                    </div>
                </td>
                <td>
                    <div class="div-seccion" style="width: 280px;">
                        <asp:Label ID="Label12" runat="server" Text="CURP-CLIENTE"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
