<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Application.master" CodeBehind="CatCuerpoReceptor.aspx.cs" Inherits="SIRALAB.CatCuerpoReceptor" ClientIDMode="AutoID" UICulture="es-MX" Culture="es-MX" %>

<%@ Register Src="~/Controles/wucFiltroCuerpoReceptor.ascx" TagPrefix="uc1" TagName="wucFiltroCuerpoReceptor" %>


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
        <div class="titulo-pagina">Cuerpo receptor</div>
            </div>
        <br /><br />
        <div class="formulario-seccion">
            <uc1:wucFiltroCuerpoReceptor runat="server" ID="wucFiltroCuerpoReceptor" OnBuscar="wucFiltroCuerpoReceptor_Buscar" OnLimpiar="wucFiltroCuerpoReceptor_Limpiar" />
        </div>
        <div class="formulario-seccion">
            <div class="seccion-encabezado">
                <table style="border-collapse: collapse; width: 100%;">
                    <tr>
                        <td></td>
                        <td style="width: 30px;">
                            <asp:UpdatePanel ID="upAgregar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lnkAddCuerpoReceptor" runat="server" ToolTip="Agregar" Text="Agregar" CssClass="btn btn-default btn-xs" OnClick="lnkAddCuerpoReceptor_Click" CausesValidation="true"><div></div></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>

            <asp:UpdatePanel ID="upAccion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" ClientIDMode="AutoID" style="width: 780px; margin: 5px auto;">
                <ContentTemplate>

                    <asp:GridView ID="GridViewCuerpoReceptor" runat="server" AutoGenerateColumns="False" Caption="Cuerpos receptores"
                        AllowPaging="True" AllowSorting="True" Visible="true" ShowHeaderWhenEmpty="true"
                        DataKeyNames="CuerpoReceptorID"
                        OnRowCommand="GridViewCuerpoReceptor_RowCommand"
                        OnRowDataBound="GridViewCuerpoReceptor_RowDataBound"
                        OnRowCancelingEdit="GridViewCuerpoReceptor_RowCancelingEdit"
                        OnRowDeleting="GridViewCuerpoReceptor_RowDeleting"
                        OnRowEditing="GridViewCuerpoReceptor_RowEditing"
                        OnRowUpdating="GridViewCuerpoReceptor_RowUpdating"
                        OnPageIndexChanging="GridViewCuerpoReceptor_PageIndexChanging"
                        OnSelectedIndexChanged="GridViewCuerpoReceptor_SelectedIndexChanged"
                        Width="97%" CssClass="table table-striped" CellSpacing="2" BorderWidth="0">
                        <Columns>
                            <%--Campo: Tipo de Cuerpo Receptor--%>
                            <asp:TemplateField HeaderText="Tipo de cuerpo receptor">
                                <ItemTemplate>
                                    <asp:Label ID="LblTipoCuerpoReceptorNombre" Text='<%# Eval("CatTipoCuerpoReceptor.Nombre") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlTipoCuerpoReceptorEdit" runat="server" ValidationGroup="vgEdit" CssClass="control-select" Width="95%" />
                                    <asp:RequiredFieldValidator ID="rfvTipoCuerpoReceptorEdit" runat="server" Text="*" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="ddlTipoCuerpoReceptorEdit" ValidationGroup="vgEdit" />
                                    <asp:ValidatorCalloutExtender ID="vceTipoCuerpoReceptorEdit" runat="server" Enabled="True" TargetControlID="rfvTipoCuerpoReceptorEdit" Width="200px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoCuerpoReceptorAdd" runat="server" ValidationGroup="vgAdd" CssClass="control-select" Width="95%" />
                                    <asp:RequiredFieldValidator ID="rfvTipoCuerpoReceptorAdd" runat="server" Text="*" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="ddlTipoCuerpoReceptorAdd" ValidationGroup="vgAdd" />
                                    <asp:ValidatorCalloutExtender ID="vceTipoCuerpoReceptorAdd" runat="server" Enabled="True" TargetControlID="rfvTipoCuerpoReceptorAdd" Width="200px" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <%--Campo: Nombre--%>
                            <asp:TemplateField HeaderText="Nombre del cuerpo receptor">
                                <ItemTemplate>
                                    <asp:Label ID="LblNombre" Text='<%# Eval("Nombre") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombreEdit" runat="server" Text='<%# Eval("Nombre") %>' MaxLength="100" ValidationGroup="vgEdit" CssClass="control-campo" Width="95%" />
                                    <asp:RequiredFieldValidator ID="rfvCuerpoReceptorNombreEdit" runat="server" Text="*" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="txtNombreEdit" ValidationGroup="vgEdit" />
                                    <asp:ValidatorCalloutExtender ID="vceCuerpoReceptorNombreEdit" runat="server" Enabled="True" TargetControlID="rfvCuerpoReceptorNombreEdit" Width="200px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNombreAdd" runat="server" MaxLength="100" ValidationGroup="vgAdd" CssClass="control-campo" Width="95%" />
                                    <asp:RequiredFieldValidator ID="rfvCuerpoReceptorNombreAdd" runat="server" Text="*" ErrorMessage="¡Esta campo es requerido!" ControlToValidate="txtNombreAdd" ValidationGroup="vgAdd" />
                                    <asp:ValidatorCalloutExtender ID="vceCuerpoReceptorNombreAdd" runat="server" Enabled="True" TargetControlID="rfvCuerpoReceptorNombreAdd" Width="200px" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <%--Botones de Acción--%>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Editar" CssClass="icono-editar-16" CausesValidation="false"><div></div></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" ToolTip="Guardar" CssClass="icono-guardar-16" runat="server" CommandName="Update" ValidationGroup="vgEdit" />
                                    <asp:LinkButton ID="btnUpdateCancel" ToolTip="Cancelar" CssClass="icono-cerrarrojo-16" runat="server" CommandName="Cancel" CausesValidation="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="btnAdd" ToolTip="Guardar" runat="server" CssClass="icono-guardar-16" CommandName="AddNew" ValidationGroup="vgAdd" />
                                    <asp:LinkButton ID="btnAddCancel" ToolTip="Cancelar" runat="server" CssClass="icono-cerrarrojo-16" CommandName="Cancel" CausesValidation="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
       
</asp:Content>

