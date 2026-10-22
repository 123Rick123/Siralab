<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="SolicitudCancelacion.aspx.cs" Inherits="SIRALAB.SolicitudCancelacion" ClientIDMode="AutoID" %>

<%@ Import Namespace="SIRALAB_BL.common" %>

<%@ Register Src="~/Controles/wucFiltroSolicitudCancelacion.ascx" TagPrefix="uc1" TagName="wucFiltroSolicitudCancelacion" %>
<%@ Register Src="~/Controles/wucSolicitarCancelacionDetalle.ascx" TagPrefix="uc1" TagName="wucSolicitarCancelacionDetalle" %>
<%@ Register Src="~/Controles/wucPaginador.ascx" TagPrefix="uc1" TagName="wucPaginador" %>
<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" TagPrefix="uc1" TagName="wucAppletFIEL" %>
<%@ Register Src="~/Controles/wucMensajes.ascx" TagPrefix="uc1" TagName="wucMensajes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/inicio.css" rel="stylesheet" />
    <link href="style/formularios.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <div class=" container top-buffer-submenu">
        <div class="row">

            <div class="col-sm-6">
                <h2>Solicitud de Cancelación</h2>
                <hr class="red">
            </div>

        </div>
        <div class="row">
            <div class="col-md-12">

                <div class="panel-heading">

                    <div class="panel-body">
                        <uc1:wucFiltroSolicitudCancelacion runat="server" ID="wucFiltroSolicitudCancelacion" OnBuscar="wucFiltroSolicitudCancelacion_Buscar" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="panel-body">
                <div style="padding: 15px 10px 10px 10px; text-align: right;">
                    <asp:UpdatePanel ID="upButtons" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkNuevaSolicitud" runat="server" ToolTip="Nueva solicitud" class="btn btn-primary" OnClick="lnkNuevaSolicitud_Click">
                        Nueva solicitud
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkAprobar" runat="server" ToolTip="Aprobar solicitud(es)" CssClass="btn btn-primary">
                        Aprobar solicitud(es)
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkRechazar" runat="server" ToolTip="Rechazar solicitud(es)" CssClass="btn btn-primary">
                        Rechazar solicitud(es)
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


            </div>
        </div>




        <!-- Aquí debemos mostrar los resultados de la búsqueda -->
        <asp:UpdatePanel ID="upSolicitudes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="wucPaginador" />
            </Triggers>
            <ContentTemplate>
                <asp:Repeater ID="rptSolicitudes" runat="server" ItemType="SIRALAB.Controles.SolicitudCancelacionItem">
                    <HeaderTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-default">



                                    <div class="panel-body">
                                        <table class="table table-bordered">
                                            <caption>Búsqueda de solicitudes</caption>
                                            <tr>
                                                <%; if (UsuarioRol == enumUsuarioRol.Recaudacion)
                                                    { %>
                                                <th style="width: 20px;">
                                                    <asp:CheckBox ID="chkSeleccionaTodos" runat="server" ToolTip="Seleccionar todas las filas visibles" data-group="all" />
                                                </th>
                                                <% } %>
                                                <th>Folio</th>
                                                <th>Laboratorio</th>
                                                <th>Motivo</th>
                                                <th>Fecha de solicitud</th>
                                                <th>Estatus</th>
                                                <th></th>
                                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <%; if (UsuarioRol == enumUsuarioRol.Recaudacion)
                                { %>
                            <td>
                                <asp:CheckBox ID="chkSeleccion" runat="server" ToolTip="Seleccionar" data-group="selection" data-solicitud-id="<%#Item.SolicitudCancelacionID%>" Visible="<%# Item.EstatusID == enumEstatusCancelacion.Enviada.ParseToInt() %>" />
                            </td>
                            <% } %>
                            <td><%#Item.Folio%></td>
                            <td><%#Item.Laboratorio%></td>
                            <td><%#Item.Motivo%></td>
                            <td><%#Item.FechaRegistro.ToString("dd/MM/yyyy")%></td>
                            <td><%#Item.Estatus%></td>
                            <td>
                                <asp:LinkButton ID="lnkVerDetalle" runat="server" ToolTip="Detalle" CssClass="btn btn-default btn-sm" OnClick="lnkVerDetalle_Click" data-solicitud-id="<%#Item.SolicitudCancelacionID%>"><span class="glyphicon glyphicon-list-alt"></span></asp:LinkButton>
                                <asp:LinkButton ID="lnkEliminar" runat="server" ToolTip="Eliminar" CssClass="btn btn-default  btn-sm" OnClick="lnkEliminar_Click" Visible="<%#Item.EstatusID == enumEstatusCancelacion.Capturada.ParseToInt() %>" data-solicitud-id="<%#Item.SolicitudCancelacionID%>"> <span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                <asp:LinkButton ID="lnkFirmar" runat="server" ToolTip="Firmar" CssClass="btn btn-default  btn-sm" OnClick="lnkFirmar_Click" Visible="<%#Item.EstatusID == enumEstatusCancelacion.Capturada.ParseToInt() %>" data-solicitud-id="<%#Item.SolicitudCancelacionID%>"><span class="glyphicon glyphicon-edit"></span></asp:LinkButton>
                            </td>
                        </tr>
                        </div>
                    </div>
            </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <uc1:wucPaginador runat="server" ID="wucPaginador" OnPageChange="wucPaginador_PageChange" />
                <uc1:wucSolicitarCancelacionDetalle runat="server" ID="wucSolicitarCancelacionDetalle" OnDuplicar="wucSolicitarCancelacionDetalle_OnDuplicar" />
               <div class="modal" tabindex="-1" role="dialog" id="ConfimacionAprova">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Solicitud de cancelación</h5>
                    <button type="button" class="close" >
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>¿Esta seguro que desea aprobar la solicitud de cancelación? </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" aria-label="Close">Cancelar</button>
                    <asp:LinkButton ID="btnconfirmacion" runat="server" ToolTip="Aprobar solicitud(es)" CssClass="btn btn-primary" OnClientClick="$('#ConfimacionAprova').modal('toggle');" OnClick="lnkAprobar_Click">
                        Aceptar
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
        <div class="modal" tabindex="-1" role="dialog" id="ConfimacionRechazo">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Solicitud de cancelación</h5>
                    <button type="button" class="close" >
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>¿Esta seguro que desea rechazar la solicitud de cancelación?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" aria-label="Close">Cancelar</button>
                   <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Rechazar solicitud(es)" OnClientClick="$('#ConfimacionRechazo').modal('toggle');" CssClass="btn btn-primary" OnClick="lnkRechazar_Click">
                        Aceptar
                            </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
                  <uc1:wucAppletFIEL runat="server" ID="wucAppletFIEL" OnFirmarColeccion="wucAppletFIEL_FirmarColeccion" />
            </ContentTemplate>
        </asp:UpdatePanel>


      
    </div>
   
    
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SelectAllCheckBox);
        //Sys.Application.add_init(function () { SelectAllCheckBox(); });
        function pageLoad() {
            SelectAllCheckBox();
        }
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(clicks);
            function clicks() {
                $('#myModal').on('hidden.bs.modal', function (e) {
                    // do something...
                })
                $('[id*="lnkAprobar"]').click(function () {
                    $('#ConfimacionAprova').modal('toggle');
                });
                $('[id*="lnkRechazar"]').click(function () {
                    $('#ConfimacionRechazo').modal('toggle');
                });
            }

        });
        function SelectAllCheckBox() {
            $("span[data-group=all] > input[type=checkbox]").click(function () {
                $("span[data-group=selection] > input[type=checkbox]")
                    .prop("checked", $("span[data-group=all] > input[type=checkbox]").prop("checked"));
            });
           
        }
       
    </script>
</asp:Content>
