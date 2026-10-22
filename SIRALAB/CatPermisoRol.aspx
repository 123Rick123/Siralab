<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="CatPermisoRol.aspx.cs" Inherits="SIRALAB.CatPermisoRol" Theme="Siralam_Thm" StylesheetTheme="Siralam_Thm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Import Namespace="SIRALAB_DAL.Extensiones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .titulo-campo, .titulo-seccion, .titulo-pagina {
            color: #B38E5D;
            padding: 5% 0 0 2%;
        }
        select {
            margin: 2%;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <div class="titulo-pagina">Permisos de Rol</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <!-- PermisoRol -->
                <div style="display: block; overflow: hidden;">
                    <asp:LinkButton ID="lnkPermisoRol" runat="server" OnClick="lnkPermisoRol_Click" class="icono-agregar" ToolTip="Nuevo permiso de rol" />
                </div>

                <asp:DropDownList ID="cboUsuarioRol" runat="server" OnSelectedIndexChanged="cboUsuarioRol_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                <asp:GridView ID="grdPermisoRol" runat="server" AllowPaging="false" PageSize="25" AutoGenerateColumns="False"
                    OnRowCancelingEdit="grdPermisoRol_RowCancelingEdit"
                    OnRowCommand="grdPermisoRol_RowCommand"
                    OnRowEditing="grdPermisoRol_RowEditing"
                    DataKeyNames="PermisosRolId"
                    OnRowUpdating="grdPermisoRol_RowUpdating"
                    ItemType="SIRALAB_DAL.CatPermisosRol" Caption="Permisos a páginas"
                    OnPageIndexChanging="grdPermisoRol_PageIndexChanging" OnRowDataBound="grdPermisoRol_RowDataBound">
                    <Columns>

                        <asp:TemplateField HeaderText="Página">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPagina" Text='<%#:Item.urlPagina()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlPagina" runat="server" Width="95%" CssClass="control-select"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlPagina" runat="server" Width="95%" CssClass="control-select"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEstatus" Text='<%#:Item.esActivo()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" Checked="<%#Item.Activo%>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Permiso" HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPermiso" Text='<%#:Item.esEscritura()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkEscritura" runat="server" Checked="<%#Item.Escritura %>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkEscritura" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" ToolTip="Editar" runat="server" CommandName="Edit" CssClass="icono-editar-16" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" ToolTip="Aceptar" runat="server" CommandName="Update" CssClass="icono-guardar-16" />
                                <asp:LinkButton ID="btnCancel" ToolTip="Cancel" runat="server" CommandName="Cancel" CssClass="icono-cerrarrojo-16" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="btnAdd" ToolTip="Agregar" runat="server" CommandName="Insert" UseSubmitBehavior="False" CssClass="icono-guardar-16" />
                                <asp:LinkButton ID="btnCancelAdd" ToolTip="Cancel" runat="server" CommandName="Cancel" CssClass="icono-cerrarrojo-16" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>