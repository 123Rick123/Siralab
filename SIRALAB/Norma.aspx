<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Application.master" CodeBehind="Norma.aspx.cs" Inherits="SIRALAB.Norma" Theme="Siralam_Thm" StylesheetTheme="Siralam_Thm" %>
<%@ Register Src="~/Controles/wucFiltroNorma.ascx" TagPrefix="uc1" TagName="wucFiltroNorma" %>
<%@ Register Src="~/Controles/wucConsultaNorma.ascx" TagPrefix="uc1" TagName="wucConsultaNorma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2>Normas</h2>
            <hr class="red" />
        </div>
        <div class="col-md-8">
            <uc1:wucFiltroNorma runat="server" ID="wucFiltroNorma1" OnBuscar="wucFiltroNorma_Buscar" OnLimpiar="wucFiltroNorma_Limpiar" />
        </div>
        <div class="col-md-4 text-right">
            <asp:LinkButton ID="lnkbtnNuevo" runat="server" CssClass="btn btn-default" ToolTip="Agregar norma" OnClick="lnkbtnNuevo_Click"><span>Nueva norma</span></asp:LinkButton>
        </div>
    </div><br/>
        
    <asp:UpdatePanel ID="upNormaConsulta" runat="server" style="min-height: 400px;">
        <ContentTemplate>
            <uc1:wucConsultaNorma runat="server" ID="wucConsultaNorma" OnSeleccionar="wucConsultaNorma_Seleccionar" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
