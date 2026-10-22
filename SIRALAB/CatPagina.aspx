<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="CatPagina.aspx.cs" Inherits="SIRALAB.CatPagina" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Import Namespace="SIRALAB_DAL.Extensiones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
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
    <div class="titulo-pagina">Paginas del sistema</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 780px; margin: 5px auto;">
        <ContentTemplate>

            <div class="formulario">
                <!-- Pagina -->
                <div style="display: block; overflow: hidden;">
                    <asp:LinkButton
                        ID="lnkPagina"
                        runat="server"
                        CssClass="icono-agregar"
                        ToolTip="Agregar"
                        OnClick="lnkPagina_Click">
                      <i class="fa fa-plus-circle fa-lg"></i>
                    </asp:LinkButton>
                </div>
                <asp:GridView
                    ID="grdPagina"
                    runat="server"
                    AllowPaging="false"
                    AutoGenerateColumns="False"
                    PagerStyle-CssClass="catalogo-grid-pager"
                    OnRowCancelingEdit="grdPagina_RowCancelingEdit"
                    OnRowCommand="grdPagina_RowCommand"
                    OnRowEditing="grdPagina_RowEditing"
                    DataKeyNames="PaginaId"
                    OnRowUpdating="grdPagina_RowUpdating"
                    ItemType="SIRALAB_DAL.CatPagina" Caption="Páginas de acceso"
                    OnPageIndexChanging="grdPagina_PageIndexChanging" OnRowDataBound="grdPagina_RowDataBound"
                    Width="97%" CssClass="table-list" CellSpacing="2" BorderWidth="0">
                    <Columns>
                        <asp:TemplateField
                            HeaderText="URL">
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
                                    Text='<%#:Item.URL %>' MaxLength="50" CssClass="control-campo"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="txtURL"
                                    runat="server" MaxLength="50" CssClass="control-campo"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Descripcion">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblDescripcion"
                                    Text='<%#:Item.Descripcion %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox
                                    ID="txtDescripcion"
                                    runat="server"
                                    Text='<%#:Item.Descripcion%>' MaxLength="250" CssClass="control-campo"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="txtDescripcion"
                                    runat="server" MaxLength="250" CssClass="control-campo"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Modulo">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblModuloId"
                                    Text='<%#:Item.NombreModulo()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlModulo" runat="server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlModulo" runat="server">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Estatus">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblEstatus"
                                    Text='<%#:Item.esActivo()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" Checked="<%#Item.Activo%>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Visible en Menú">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblVisible"
                                    Text='<%#:Item.esVisibleMenu()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkVisibleMenu" runat="server" Checked="<%#Item.VisibleMenu%>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkVisibleMenu" runat="server" />
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
                        <asp:TemplateField
                            HeaderText="Acción">

                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="btnEdit"
                                    ToolTip="Editar"
                                    runat="server"
                                    CssClass="icono-editar-16"
                                    CommandName="Edit" />
                            </ItemTemplate>

                            <EditItemTemplate>
                                <asp:LinkButton
                                    ID="btnUpdate"
                                    ToolTip="Aceptar"
                                    runat="server"
                                    CssClass="icono-guardar-16"
                                    CommandName="Update" />
                                <asp:LinkButton
                                    ID="btnCancel"
                                    ToolTip="Cancel"
                                    runat="server"
                                    CssClass="icono-cerrarrojo-16"
                                    CommandName="Cancel" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton
                                    ID="btnAdd"
                                    ToolTip="Agregar"
                                    runat="server"
                                    CommandName="Insert"
                                    CssClass="icono-guardar-16"
                                    UseSubmitBehavior="False" />
                                <asp:LinkButton
                                    ID="btnCancelAdd"
                                    ToolTip="Cancel"
                                    runat="server"
                                    CssClass="icono-cerrarrojo-16"
                                    CommandName="Cancel" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


                
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
