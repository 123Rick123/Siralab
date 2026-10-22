<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Application.master" CodeBehind="CatTipoCuerpoReceptor.aspx.cs" Inherits="SIRALAB.CatTipoCuerpoReceptor" ClientIDMode="AutoID" UICulture="es-MX" Culture="es-MX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
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
    <div style="min-height: 100px;">
        <div class="container bottom-buffer">
        <div class="titulo-pagina">Tipo de cuerpo receptor</div>
            </div>
        </div>
        <div class="formulario-seccion">
            <div class="seccion-encabezado">
                <table style="border-collapse: collapse; width: 100%;">
                    <tr>
                        <td></td>
                        <td style="width: 30px;">
                            <asp:UpdatePanel ID="upAgregar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lnkAddCatTipoCuerpoReceptor" runat="server" ToolTip="Agregar" Text="Agregar" OnClick="lnkAddCatTipoCuerpoReceptor_Click" CausesValidation="true" CssClass="btn btn-default btn-xs"><div></div></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="upAccion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" ClientIDMode="AutoID" style="width: 780px; margin: 5px auto;">
                <ContentTemplate>
                    <asp:GridView ID="GridViewCatTipoCuerpoReceptor" runat="server" Caption="Tipos de cuerpo receptor" CssClass="table table-striped"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="12" AllowSorting="True" Visible="true" Width="97%" BorderWidth="0"
                        DataKeyNames="TipoCuerpoReceptorID"
                        OnRowCommand="GridViewCatTipoCuerpoReceptor_RowCommand"
                        OnRowDataBound="GridViewCatTipoCuerpoReceptor_RowDataBound"
                        OnRowCancelingEdit="GridViewCatTipoCuerpoReceptor_RowCancelingEdit"
                        OnRowDeleting="GridViewCatTipoCuerpoReceptor_RowDeleting"
                        OnRowEditing="GridViewCatTipoCuerpoReceptor_RowEditing"
                        OnRowUpdating="GridViewCatTipoCuerpoReceptor_RowUpdating"
                        OnPageIndexChanging="GridViewCatTipoCuerpoReceptor_PageIndexChanging"
                        OnSelectedIndexChanged="GridViewCatTipoCuerpoReceptor_SelectedIndexChanged">
                        <Columns>
                            <%--Campo: Nombre--%>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" Text='<%# Eval("Nombre") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombreEdit" runat="server" Text='<%# Eval("Nombre") %>' MaxLength="50" ValidationGroup="vgEdit" CssClass="control-campo" Width="95%" />
                                    <asp:RequiredFieldValidator ID="rfvVMRSCatalogNameNombreEdit" runat="server" Display="None" ErrorMessage="Este campo es requerido." ControlToValidate="txtNombreEdit" ValidationGroup="vgEdit" />
                                    <asp:ValidatorCalloutExtender ID="vceVMRSCatalogNameNombreEdit" runat="server" Enabled="True" TargetControlID="rfvVMRSCatalogNameNombreEdit" PopupPosition="TopLeft" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNombreAdd" runat="server" ValidationGroup="vgAdd" MaxLength="50" CssClass="control-campo" Width="95%" />
                                    <asp:RequiredFieldValidator ID="rfvVMRSCatalogNameNombreAdd" runat="server" Display="None" ErrorMessage="Este campo es requerido." ControlToValidate="txtNombreAdd" ValidationGroup="vgAdd" />
                                    <asp:ValidatorCalloutExtender ID="vceVMRSCatalogNameNombreAdd" runat="server" Enabled="True" TargetControlID="rfvVMRSCatalogNameNombreAdd" PopupPosition="TopLeft" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--Botones de Acción--%>
                            <asp:TemplateField>
                                <HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" CssClass="icono-editar-16" ToolTip="Editar"><div></div></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" ValidationGroup="vgEdit" CssClass="icono-guardar-16" ToolTip="Guardar" />
                                    <asp:LinkButton ID="btnUpdateCancel" runat="server" CommandName="Cancel" CausesValidation="false" CssClass="icono-cerrarrojo-16" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="btnAdd" runat="server" CommandName="AddNew" ValidationGroup="vgAdd" CssClass="icono-guardar-16" ToolTip="Guardar" />
                                    <asp:LinkButton ID="btnAddCancel" runat="server" CommandName="Cancel" CausesValidation="false" CssClass="icono-cerrarrojo-16" ToolTip="Cancelar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

