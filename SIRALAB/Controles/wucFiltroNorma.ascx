<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFiltroNorma.ascx.cs" Inherits="SIRALAB.Controles.wucFiltroNorma" %>

<asp:UpdatePanel ID="upBusqueda" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" class="buscar">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbLimpiar" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel runat="server" DefaultButton="lnkbBuscar">
            <div class="row">
                <div class="col-md-8">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" aria-label="..." placeholder="Búsqueda por nombre de la norma"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <div class="btn-group btn-group-sm" role="group" aria-label="Justified button group with nested dropdown">
                        <asp:LinkButton ID="lnkbBuscar" runat="server" CssClass="btn btn-default btn-sm" ToolTip="Buscar" OnClick="lnkbBuscar_Click" ValidationGroup="vgFiltroNorma"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkbLimpiar" runat="server" CssClass="btn btn-default btn-sm" ToolTip="Limpiar" OnClick="lnkbLimpiar_Click"><i class="glyphicon glyphicon-asterisk"></i></asp:LinkButton>
                        <div class="btn-group" role="group">
                            <a href="#" class="btn btn-default btn-sm dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="true"><span class="caret"></span></a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a><asp:TextBox ID="txtNormaDescripcion" runat="server" CssClass="form-control input-sm" placeholder="Descripcion"></asp:TextBox></a></li>
                                <li class="divider"></li>
                                <li>
                                    <a><asp:TextBox ID="txtNormaFechaInicio" runat="server" CssClass="form-control input-sm" placeholder="Fecha de inicio"></asp:TextBox></a>
                                    <asp:CalendarExtender ID="ceNormaFechaInicio" runat="server" TargetControlID="txtNormaFechaInicio" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
