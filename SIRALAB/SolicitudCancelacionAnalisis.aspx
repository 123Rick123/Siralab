<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="SolicitudCancelacionAnalisis.aspx.cs" Inherits="SIRALAB.SolicitudCancelacionAnalisis" %>
<%@ Register Src="~/Controles/wucFiltroSolicitudCancelacionAnalisis.ascx" TagPrefix="uc1" TagName="wucFiltroSolicitudCancelacionAnalisis" %>
<%@ Register Src="~/Controles/wucPaginador.ascx" TagPrefix="uc1" TagName="wucPaginador" %>
<%@ Register Src="~/Controles/wucSolicitarCancelacion.ascx" TagPrefix="uc1" TagName="wucSolicitarCancelacion" %>
<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" TagPrefix="uc1" TagName="wucAppletFIEL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <%--<link href="style/inicio.css" rel="stylesheet" />--%>
  <%--  <link href="style/formularios.css" rel="stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <div class="formulario">
           <div class=" container top-buffer-submenu">
        <div class="row">
             <div class="col-sm-4">
        <h2>Solicitud Cancelación de Análisis</h2>
                 <hr class="red">
            </div>
            </div>
        <uc1:wucFiltroSolicitudCancelacionAnalisis runat="server" ID="wucFiltroSolicitudCancelacionAnalisis" OnBuscar="wucFiltroSolicitudCancelacionAnalisis_Buscar" />
        <div class="formulario-seccion">
            <!-- Aquí debemos mostrar los resultados de la búsqueda -->
             <div class="panel-group ficha-collapse" id="accordion">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        
   Búsqueda de análisis
        
      </h4>
    </div>
    <div class="panel-collapse collapse in" id="panel-01">
      <div class="panel-body">
 
     


            <asp:UpdatePanel ID="upSolicitudes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="wucPaginador" />
                </Triggers>
                <ContentTemplate>
                    <asp:Repeater ID="rptSolicitudes" runat="server" ItemType="SIRALAB.Controles.SolicitudCancelacionAnalisisItem"
                        OnItemDataBound="rptSolicitudes_ItemDataBound" EnableViewState="true">
                        <HeaderTemplate>
                            <table class="table table-striped">
                                
                                <tbody>
                                <tr>
                                    <th><asp:CheckBox ID="chkSeleccionaTodos" runat="server" ToolTip="Seleccionar todas las filas visibles" data-group="all" /></th>
                                    <th>No. de Informe</th>
                                    <th>Norma</th>
                                    <th>Cliente</th>
                                    <th>Rfc</th>
                                    <th>Fecha de emisión</th>
                                    <th>Título</th>
                                    <th>Anexo</th>
                                </tr>
                                 
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><asp:CheckBox ID="chkSeleccion" runat="server" ToolTip="Seleccionar" data-group="selection" data-analisis-id="<%#Item.AnalisisID%>" /></td>
                                <td><%#Item.NumeroInforme%></td>
                                <td><%#Item.Norma%></td>
                                <td><%#Item.Cliente%></td>
                                <td><%#Item.Rfc%></td>
                                <td><%#Item.FechaEmision.ToString("dd/MM/yyyy")%></td>
                                <td><%#Item.Titulo%></td>
                                <td><%#Item.Anexo%></td>
                            </tr>
                        </ItemTemplate>
                      
                        <FooterTemplate>
                               </tbody>
                                </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
             <div class="row">
                         <div class="col-md-6 col-md-offset-7">
            <asp:UpdatePanel ID="upGuardar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
              
                         
                        <asp:LinkButton ID="lnkGuardarSolicitud" runat="server" CssClass="btn btn-primary" ToolTip="Guardar solicitud" OnClick="lnkGuardarSolicitud_Click">
                            Guardar solicitud
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkLimpiarSeleccion" runat="server" CssClass="btn btn-danger" ToolTip="Limpiar solicitud" OnClick="lnkLimpiarSeleccion_Click">
                            Borrar selección
                        </asp:LinkButton>
                    </ContentTemplate>
            </asp:UpdatePanel>
        </div>
                   </div>
                              </div>
        </div></div>
        <uc1:wucPaginador runat="server" id="wucPaginador" OnPageChange="wucPaginador_PageChange" />
    </div>

    <uc1:wucSolicitarCancelacion runat="server" id="wucSolicitarCancelacion" OnGuardar="wucSolicitarCancelacion_Guardar" />
    <uc1:wucAppletFIEL runat="server" ID="wucFirma" OnFirmarColeccion="wucFirma_FirmarColeccion" />

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SelectAllCheckBox);
        Sys.Application.add_init(function () { SelectAllCheckBox(); });

        function SelectAllCheckBox() {
            $("span[data-group=all] > input[type=checkbox]").click(function () {
                $("span[data-group=selection] > input[type=checkbox]")
                    .prop("checked", $("span[data-group=all] > input[type=checkbox]").prop("checked"));
            });
        }
    </script>
</asp:Content>
