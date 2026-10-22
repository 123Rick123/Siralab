<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaCliente.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaCliente" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/autocomplete.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .div-colapsado {
        display: none;
    }
</style>

<script type="text/javascript">
    function t(d) {
        if (d == 1) {
            $('.div-colapsado').slideToggle(600);
        };
    }
</script>

<asp:Panel ID="pnlCapturaPersona" runat="server" Visible="true">
    <asp:UpdatePanel ID="updCliente" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lnkbtnClienteConsulta" EventName="Click" />
        </Triggers>
        <ContentTemplate>

            <asp:LinkButton ID="lnkbtnClienteConsulta" Visible="false" runat="server" OnClientClick="t(1); return false;" CssClass="btn btn-default btn-xs" ToolTip="Ver información completa">
                 <span class="glyphicon glyphicon-chevron-down"></span>
            </asp:LinkButton>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="name">RFC<span class="control-label">*</span>:</label>

                            <div class="input-group">
                                <asp:TextBox ID="txtClienteRFC" runat="server" CssClass="form-control" placeholder="Ingresa tu RFC" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_ddlCliente" runat="server" ValidationGroup="Cliente" InitialValue="0" ControlToValidate="txtClienteRFC"
                                    ErrorMessage="Campo requerido" Display="Dynamic"
                                    CssClass="text-danger" Text="Este campo es obligatorio"></asp:RequiredFieldValidator>

                                <div class="input-group-btn">
                                    <asp:LinkButton ID="lnkbtnConsultaCliente" runat="server" CssClass="btn btn-default btn-sm" ToolTip="Buscar RFC" OnClick="lnkbtnConsultaCliente_Click">
                                 <span class="glyphicon glyphicon-search"></span>
                                    </asp:LinkButton>

                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="col-md-4">
                        <div class="form-group">

                            <label for="name">CURP<span class="form-text"></span>:</label>
                            <asp:Label ID="lblClienteCurp" runat="server" CssClass="form-text"></asp:Label>

                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="name">Nombre<span class="form-text"></span>:</label>
                            <asp:Label ID="lblClienteNombre" runat="server" CssClass="form-tex"></asp:Label>


                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlDatosCliente" runat="server">

                    <table style="width: 100%; margin: 0 auto;">
                        <%--<tr>
                        <td colspan="2" class="field-title">RFC</td>
                        <td style="width: 25%;" class="field-title">CURP</td>
                        <td style="width: 50%;" class="field-title">Nombre</td>
                    </tr>--%>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlCliente" runat="server" DefaultButton="lnkbtnConsultaCliente">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <div class="div-colapsado">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Calle<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteCalle" runat="server" class="form-text"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">No. Exterior<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteNE" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">No. Interior<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteNI" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">C.P<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteCP" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Estado<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteEstado" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Municipio<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteMunicipio" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Colonia<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteColonia" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Localidad<span class="form-text"></span>:</label>
                                    <asp:Label ID="lblClienteLocalidad" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                    </div>
                </asp:Panel>
            </div>
            <div class="panel-body">
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
