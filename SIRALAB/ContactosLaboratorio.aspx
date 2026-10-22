<%@ Page Language="C#" MasterPageFile="~/Application.Master" AutoEventWireup="true" CodeBehind="ContactosLaboratorio.aspx.cs" Inherits="SIRALAB.ContactosLaboratorio"
    ClientIDMode="AutoID"
    Culture="es-MX" UICulture="es-MX" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script src="js/ValidadorRequerido.js"></script>
    <style type="text/css">
        .encabezado {
            text-align: center;
            vertical-align: middle !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updContactoLaboratorio" runat="server">
        <ContentTemplate>
            <br />
            <div class=" container">
                <div class="row">
                    <div class="col-sm-1">
                        <asp:LinkButton ID="lnkbtnback" runat="server" CssClass="btn btn-default btn-ms" OnClick="lnkbtnback_Click" ToolTip="Regresar a la pagina anterior">
                            <span class="glyphicon glyphicon-chevron-left"></span>
                        </asp:LinkButton>
                    </div>
                    <div class="col-sm-11">
                        <h2>Contactos de laboratorio</h2>
                        <hr class="red">
                    </div>
                </div>

                <div class="row">
                    <div class="alert alert-danger">
                        <strong>¡Atención!</strong>
                        <p class="text-center">
                            Para la configuración de los receptores del acuse de firma y el reporte de análisis de laboratorio sólo se permite la selección de los contactos previamente registrados en el Padrón de Laboratorios (PALA).
                        </p>
                        <p>
                            El registro y actualización de contactos deben realizarse en dicho padrón.
                        </p>
                    </div>
                </div>

                <div class="row">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h5>Contactos de laboratorio.</h5>
                                </div>
                                <div class="panel-body">
                                    <asp:Panel runat="server" ID="pnlGrid">
                                        <asp:GridView runat="server" AutoGenerateColumns="false" Caption=" "
                                            CssClass="table table-responsive table-condensed table-striped text-center viewGrid" DataKeyNames="ContactoID" GridLines="None"
                                            ID="gvContactos" ItemType="SIRALAB_BL.negocio.Contacto.ContactoArgs" OnRowDataBound="gvContactos_RowDataBound"
                                            RowStyle-VerticalAlign="Middle">

                                            <Columns>
                                                <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                                                    <HeaderStyle CssClass="encabezado" />
                                                    <ItemStyle CssClass="encabezado" />
                                                    <HeaderTemplate>Contacto</HeaderTemplate>
                                                    <ItemTemplate><%#Item.Nombre%> <%# Item.ApellidoPaterno %> <%# Item.ApellidoMaterno %> </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                                                    <HeaderStyle CssClass="encabezado" />
                                                    <ItemStyle CssClass="encabezado" />
                                                    <HeaderTemplate>Correo</HeaderTemplate>
                                                    <ItemTemplate><%# Item.Correo %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                                                    <HeaderStyle CssClass="encabezado" />
                                                    <ItemStyle CssClass="encabezado" />
                                                    <HeaderTemplate>Recibe acuse</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span runat="server">
                                                            <asp:CheckBox runat="server" ID="chkRecibeAcuse" Checked="<%# Item.RecibeAcuse ?? false %>" Visible="<%# !Item.EsLaboratorio %>" />
                                                            <span runat="server" id="spnQuestion" class='glyphicon glyphicon-info-sign'
                                                                title="Correo registrado en el Padrón de Laboratorios. Siempre recibe acuse de firma"
                                                                visible="<%# Item.EsLaboratorio %>"></span>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Panel ID="pnl" runat="server" Style="padding: 10px; text-align: right;" Visible="true">
                                            <asp:LinkButton ID="lnkbtnGuardarPmtos" runat="server" Visible="true" Style="margin-right: 30px;" OnClick="lnkbtnGuadar" CssClass="btn btn-default btn-md" ToolTip="Guardar todos los registros">
                                                <span class="glyphicon glyphicon-floppy-saved"></span>
                                            </asp:LinkButton>
                                        </asp:Panel>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
