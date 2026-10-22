<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFiltroNormaConfiguracion.ascx.cs" Inherits="SIRALAB.Controles.wucFiltroNormaConfiguracion" %>
<%--<asp:UpdatePanel ID="upBusqueda" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" class="buscar">
    <ContentTemplate>--%>
        <asp:Panel runat="server">
            <div class="row">
                <div class="col-md-3">
                    <label for="name">Cuerpo receptor<span class="control-label"></span>:</label>
                    <asp:DropDownList ID="ddlCuerpoReceptor" runat="server" Width="97%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCuerpoReceptor_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                     <label for="name" id="lblUsoDeAgua" runat="server">Uso de agua<span class="control-label"></span>:</label>
                    <asp:DropDownList ID="ddlUsoAgua" runat="server" Width="97%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUsoAgua_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
        </asp:Panel>
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>