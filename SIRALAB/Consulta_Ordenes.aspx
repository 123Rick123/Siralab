<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="Consulta_Ordenes.aspx.cs" Inherits="SIRALAB.Consulta_Ordenes" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/Controles/wucFiltro_Consulta_Ordenes.ascx" TagPrefix="uc1" TagName="wucFiltro_Consulta_Ordenes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/botones.css" rel="stylesheet" type="text/css" />
    <link href="style/grid.css" rel="stylesheet" type="text/css" />
    <link href="style/busqueda.laboratorio.css" rel="stylesheet" />
    <%--<link href="style/paginador.css" rel="stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
            <div class="formulario" style="min-height: 400px;">
                <ul>
                    <li class="titulo-pagina">Consulta de Ordenes de Trabajo
                    </li>
                    <li>
                        <uc1:wucFiltro_Consulta_Ordenes runat="server" ID="wucFiltro_Consulta_Ordenes" OnBuscar="wucFiltro_Consulta_Ordenes_Buscar" />
                        <div class="formulario-seccion" style="overflow-y:scroll;">
                            <div class="seccion-encabezado">
                                <div class="seccion-titulo">Ordenes de trabajo</div>
                            </div>
                            <asp:UpdatePanel ID="updPnlOrden" runat="server" style="margin: 5px;">
                                <ContentTemplate>
                                    <rsweb:ReportViewer ID="rvConsulta" runat="server" Width="100%"
                                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                                        ProcessingMode="Remote" WaitMessageFont-Names="Verdana"
                                        Visible="true" ZoomPercent="100"
                                        WaitMessageFont-Size="14pt" PromptAreaCollapsed="true" ShowFindControls="false"
                                        ShowBackButton="false" ShowCredentialPrompts="false"
                                        ShowParameterPrompts="false" ShowRefreshButton="false"
                                        ShowWaitControlCancelLink="false" ShowZoomControl="false"
                                        ShowPageNavigationControls="true"
                                        ShowExportControls="true">
                                    </rsweb:ReportViewer>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </li>
                </ul>
            </div>
    <div>
    </div>
</asp:Content>
