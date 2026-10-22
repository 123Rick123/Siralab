<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="CatModulo.aspx.cs" Inherits="SIRALAB.CatModulo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Import Namespace="SIRALAB_DAL.Extensiones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />

    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style type="text/css">
        .table-list > tbody > tr:nth-child(even) > td {
            background-color:#F2F2F2;
        }
        .titulo-campo, .titulo-seccion, .titulo-pagina {
            color: #B38E5D;
            padding: 5% 0 0 2%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <div class="titulo-pagina">Módulos del sistema</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 780px; margin: 5px auto;">
        <ContentTemplate>
            <div class="formulario">
                <!-- Modulo -->
                <div style="display: block; overflow: hidden;">
                    <asp:LinkButton ID="lnkModulo" runat="server" CssClass="icono-agregar" ToolTip="Agregar" OnClick="lnkModulo_Click"></asp:LinkButton>
                </div>
                <asp:GridView
                    ID="grdModulo"
                    runat="server"
                    AllowPaging="false"
                    PagerStyle-CssClass="catalogo-grid-pager"
                    AutoGenerateColumns="False"
                    OnRowCancelingEdit="grdModulo_RowCancelingEdit"
                    OnRowCommand="grdModulo_RowCommand"
                    OnRowEditing="grdModulo_RowEditing"
                    DataKeyNames="ModuloId"
                    OnRowUpdating="grdModulo_RowUpdating"
                    ItemType="SIRALAB_DAL.CatModulo" Caption="Módulos del sistema"
                    OnPageIndexChanging="grdModulo_PageIndexChanging" OnRowDataBound="grdModulo_RowDataBound"
                    Width="97%" CssClass="table-list" CellSpacing="2" BorderWidth="0">

                    <Columns>
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblNombre"
                                    HtmlEncode="False"
                                    Text='<%#:Item.Nombre %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox
                                    ID="txtNombre"
                                    runat="server"
                                    HtmlEncode="False"
                                    Text='<%#:Item.Nombre%>' MaxLength="50" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtNombre" runat="server" MaxLength="50" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDescripcion" HtmlEncode="False" Text='<%#:Item.Descripcion %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescripcion" runat="server" HtmlEncode="True" Text='<%#: Item.Descripcion%>' MaxLength="250" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="250" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="CssClass">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblCssClass"
                                    Text='<%#:Item.CssClass %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox
                                    ID="txtCssClass"
                                    runat="server"
                                    Text='<%#:Item.CssClass %>' MaxLength="50" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="txtCssClass"
                                    runat="server" MaxLength="50" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="URL">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblURL"
                                    Text='<%#:Item.URL %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox
                                    ID="txtURL"
                                    runat="server"
                                    Text='<%#:Item.URL %>' MaxLength="50" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="txtURL"
                                    runat="server" MaxLength="50" CssClass="control-campo" Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Orden">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrden" Text='<%#:Item.Orden %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrden" runat="server" Text='<%#:Item.Orden%>' MaxLength="2" onkeypress="return isNumberKey(event)" CssClass="control-campo" Width="25px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtOrden" runat="server" MaxLength="2" onkeypress="return isNumberKey(event)" CssClass="control-campo" Width="25px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modulo Padre">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblModuloPadreId" Text='<%#:Item.NombreModuloPadre()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlModuloPadre" runat="server"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlModuloPadre" runat="server"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" ToolTip="Editar" runat="server" CssClass="icono-editar-16" CommandName="Edit" />
                            </ItemTemplate>

                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" ToolTip="Aceptar" runat="server" CssClass="icono-guardar-16" CommandName="Update" />
                                <asp:LinkButton ID="btnCancel" ToolTip="Cancel" runat="server" CssClass="icono-cerrarrojo-16" CommandName="Cancel" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="btnAdd" ToolTip="Agregar" runat="server" CommandName="Insert" CssClass="icono-guardar-16" UseSubmitBehavior="False" />
                                <asp:LinkButton ID="btnCancelAdd" ToolTip="Cancel" runat="server" CssClass="icono-cerrarrojo-16" CommandName="Cancel" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>