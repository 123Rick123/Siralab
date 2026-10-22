<%@ Page Title="" Language="C#" MasterPageFile="~/Application.Master" AutoEventWireup="true" CodeBehind="InformeMensual.aspx.cs" Inherits="SIRALAB.InformeMensual" ClientIDMode="AutoID"
    Culture="es-MX" UICulture="es-MX" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="Controles/wucConformacion.ascx" TagName="wucConformacion" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/botones.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updOrdenTrabajo" runat="server">
        <ContentTemplate>
            <div class="formulario">
                <ul>
                    <li class="titulo-pagina">
                        <asp:LinkButton ID="lnkbtnBack" runat="server" CssClass="icono-atras" ToolTip="Regresar a la pagina anterior"></asp:LinkButton>
                        <asp:Panel ID="pnlTitulo" runat="server" Style="width: 500px; height: 40px;">
                            <asp:Label ID="lbl" runat="server" Text=" Informe Mensual del Contribuyente" Style="margin-top: -45px; margin-left: 60px; display: block;"></asp:Label>
                        </asp:Panel>
                    </li>
                    <li>
                        <div class="formulario-seccion">
                            <div class="seccion-encabezado">
                                <div class="seccion-titulo">Datos de conformación</div>
                            </div>
                            <ul style="width: 700px;">
                                <li>
                                    <uc1:wucConformacion ID="wucConformacion1" runat="server" OnRespuesta="wucConformacion_Respuesta" />
                                </li>
                            </ul>
                        </div>
                    </li>

                    <li>
                        <div class="formulario-seccion">
                            <div class="seccion-encabezado">
                                <div class="seccion-titulo">Informe Mensual</div>
                            </div>
                            <ul style="width: 750px;">
                                <li>
                                    <asp:UpdatePanel ID="updPnlOrden" runat="server" style="margin: 5px;" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnl" runat="server" style="display:none">
                                                <rsweb:ReportViewer ID="rvMensual" runat="server" Width="100%"
                                                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                                                    ProcessingMode="Remote" WaitMessageFont-Names="Verdana"
                                                    Visible="true" ZoomPercent="100"
                                                    WaitMessageFont-Size="14pt" PromptAreaCollapsed="true" ShowFindControls="false"
                                                    ShowBackButton="false" ShowCredentialPrompts="false"
                                                    ShowParameterPrompts="false" ShowRefreshButton="false"
                                                    ShowWaitControlCancelLink="false" ShowZoomControl="false"
                                                    ShowPageNavigationControls="true"
                                                    ShowExportControls="true" >
                                                </rsweb:ReportViewer>
                                                
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
