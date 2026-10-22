<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="CatUsoAgua.aspx.cs" Inherits="SIRALAB.CatUsoAgua" %>

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
    <div style="min-height: 400px;">
        <div class="container bottom-buffer">
        <div class="titulo-pagina">Uso de Agua</div>
            </div>
          <br /><br />
        <div class="formulario-seccion">
            <div class="seccion-encabezado">
                <table style="border-collapse: collapse; width: 100%;">
                    <tr>
                        <td></td>
                        <td style="width: 30px;">
                            <asp:UpdatePanel ID="upAgregar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lnkAddCatUsoAgua" runat="server" ToolTip="Agregar" Text="Agregar" CssClass="btn btn-default btn-xs" OnClick="lnkAddCatUsoAgua_Click" CausesValidation="true"></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="upAccion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" ClientIDMode="AutoID" style="width: 780px; margin: 5px auto;">
                <ContentTemplate>
                    <asp:GridView ID="GridViewCatUsoAgua" runat="server" AutoGenerateColumns="False" AllowPaging="True" Caption="Usos de agua"
                        AllowSorting="True" Visible="true" ShowHeaderWhenEmpty="true" PageSize="12"
                        DataKeyNames="UsoAguaID"
                        OnRowCommand="GridViewCatUsoAgua_RowCommand"
                        OnRowDataBound="GridViewCatUsoAgua_RowDataBound"
                        OnRowCancelingEdit="GridViewCatUsoAgua_RowCancelingEdit"
                        OnRowDeleting="GridViewCatUsoAgua_RowDeleting"
                        OnRowEditing="GridViewCatUsoAgua_RowEditing"
                        OnRowUpdating="GridViewCatUsoAgua_RowUpdating"
                        OnPageIndexChanging="GridViewCatUsoAgua_PageIndexChanging"
                        OnSelectedIndexChanged="GridViewCatUsoAgua_SelectedIndexChanged"
                        Width="97%" CssClass="table table-striped" CellSpacing="2" BorderWidth="0">
                        <Columns>
                            <%--Campo: Nombre--%>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="LblNombre" Text='<%# Eval("Nombre") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombreEdit" runat="server" Text='<%# Eval("Nombre") %>' MaxLength="50" ValidationGroup="vgEdit" CssClass="control-campo" Width="75%" />
                                    <asp:RequiredFieldValidator ID="rfvUsoAguaNombreEdit" runat="server" Text="*" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="txtNombreEdit" ValidationGroup="vgEdit" />
                                    <asp:ValidatorCalloutExtender ID="vceUsoAguaNombreEdit" runat="server" Enabled="True" TargetControlID="rfvUsoAguaNombreEdit" Width="200px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNombreAdd" runat="server" MaxLength="50" ValidationGroup="vgAdd" CssClass="control-campo" Width="75%" />
                                    <asp:RequiredFieldValidator ID="rfvUsoAguaNombreAdd" runat="server" Text="*" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="txtNombreAdd" ValidationGroup="vgAdd" />
                                    <asp:ValidatorCalloutExtender ID="vceUsoAguaNombreAdd" runat="server" Enabled="True" TargetControlID="rfvUsoAguaNombreAdd" Width="200px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--Botones de Acción--%>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="50px">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" ToolTip="Editar" CssClass="icono-editar-16"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" ToolTip="Guardar" runat="server" CommandName="Update" ValidationGroup="vgEdit" CssClass="icono-guardar-16" />
                                    <asp:LinkButton ID="btnUpdateCancel" ToolTip="Cancelar" runat="server" CommandName="Cancel" CausesValidation="false" CssClass="icono-cerrarrojo-16" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="btnAdd" ToolTip="Guardar" runat="server" CommandName="AddNew" ValidationGroup="vgAdd" CssClass="icono-guardar-16" />
                                    <asp:LinkButton ID="btnAddCancel" ToolTip="Cancelar" runat="server" CommandName="Cancel" CausesValidation="false" CssClass="icono-cerrarrojo-16" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
