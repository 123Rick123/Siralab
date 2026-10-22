<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="CatUsuarioRecaudacion.aspx.cs" Inherits="SIRALAB.CatUsuarioRecaudacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<%@ Import Namespace="SIRALAB_DAL.Extensiones" %>
<%@ Register Src="Controles/wucPersonaRecaudacion.ascx" TagName="wucPersonaRecaudacion" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function removeBackdrop() {
            $("body").removeAttr("Style");
            $("body").removeClass("modal-open");
            $("div").removeClass("modal-backdrop");
            $("div").removeClass("in");
        }

    </script>
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .titulo-campo, .titulo-seccion, .titulo-pagina {
            color: #B38E5D;
            padding: 5% 0 0 2%;
        }

        td {
            vertical-align: middle !important;
        }

        .modal-row {
            margin-bottom: 10px;
        }

        .edit {
            text-align: center;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="container">
        <ContentTemplate>
            <br />
            <div class="row">
                <div class="col-sm-1">
                    <br />
                    <asp:LinkButton ID="lnkbtnback" runat="server" CssClass="btn btn-default btn-ms" OnClick="lnkbtnback_Click" ToolTip="Regresar a la pagina anterior">
                        <span class="glyphicon glyphicon-chevron-left"></span>
                    </asp:LinkButton>
                </div>
                <div class="col-sm-11">
                    <h2>Registro Usuarios Recaudación y Fiscalización</h2>
                    <hr class="red">
                </div>
            </div>

            <div class="formulario">
                <!-- Pagina -->
                <div style="display: block; overflow: hidden;">
                    <asp:LinkButton
                        ID="lnkPagina"
                        runat="server"
                        CssClass="btn btn-default"
                        ToolTip="Agregar"
                        OnClick="lnkPagina_Click"
                        Text="Agregar">
                      <span class="fa fa-plus-circle fa-sm"> Agregar</span>
                    </asp:LinkButton>
                </div>
                <br />
                <asp:GridView
                    ID="grdPersona"
                    runat="server"
                    AllowPaging="false"
                    AutoGenerateColumns="False"
                    CssClass="table table-condensed table-striped" PagerStyle-CssClass="catalogo-grid-pager"
                    OnRowCommand="grdPersona_RowCommand"
                    OnRowEditing="grdPersona_RowEditing"
                    DataKeyNames="PersonaID"
                    ItemType="SIRALAB_DAL.Persona"
                    OnPageIndexChanging="grdPersona_PageIndexChanging" GridLines="None">

                    <Columns>
                        <asp:TemplateField
                            HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblNombre"
                                    Text='<%#:Item.NombreCompleto %>' />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField
                            HeaderText="RFC">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblRFC"
                                    Text='<%#:Item.RFC %>' />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Curp">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblCURP"
                                    Text='<%#:Item.CURP %>' />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Estatus">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblEstatus"
                                    Text='<%#:Item.esActivo%>' />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField
                            HeaderText="Usuario">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblUsuarioNombre"
                                    Text='<%#:Item.UsuarioNombreRecaudacion %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Usuario">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblUsuarioEstatus"
                                    Text='<%#:Item.UsuarioEstatus %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Acción"
                            ItemStyle-VerticalAlign="Middle">

                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="btnEdit"
                                    ToolTip="Editar"
                                    runat="server"
                                    CssClass="btn btn-default btn-xs"
                                    CommandArgument="<%#Item.PersonaID  %>"
                                    CommandName="Edit"
                                    Style="text-align: center">
                                    <span class="glyphicon glyphicon-pencil"></span>

                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="edit" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <uc1:wucPersonaRecaudacion ID="wucPersonaRecaudacion1" runat="server" OnSeleccionar="wucCapturaUsuarioPALA_Seleccionar" OnGuardar="wucCapturaUsuarioPALA_Guardar" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
