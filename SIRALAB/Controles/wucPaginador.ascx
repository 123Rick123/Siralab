<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPaginador.ascx.cs" Inherits="SIRALAB.Controles.wucPaginador" ClientIDMode="AutoID" %>
<%@ Import Namespace="System.Windows.Forms.VisualStyles" %>

<asp:UpdatePanel ID="upPaginador" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rptPaginas" />
        <asp:AsyncPostBackTrigger ControlID="ddlPageSize" />
    </Triggers>
    <ContentTemplate>
        <div class="row">
            <div class="col-md-4">
                  <asp:Repeater ID="rptPaginas" runat="server" ItemType="SIRALAB_BL.common.Pagina">
                    <HeaderTemplate>
                        <ul class="pagination" style="text-align:left">
                            <li<%= !ShowPrevious ? " style=\"display: none;\" " : " title=\"Primera Página\" " %>><asp:LinkButton ID="lnkInicio" runat="server" Text="<<" OnClick="lnkInicio_OnClick"></asp:LinkButton></li>
                                <li<%= !ShowPrevious ? " style=\"display: none;\" " : " title=\"Atras\" " %>><asp:LinkButton ID="lnkAnterior" runat="server" Text="<" OnClick="lnkAnterior_OnClick"></asp:LinkButton>
                        </li>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:LinkButton ID="lnkPage" runat="server" Text="<%# Item.Page %>" CommandArgument="<%# Item.Page %>" CssClass='<%# Item.ActivePage ? "pager-link pager-active" : "pager-link" %>' OnClick="lnkPage_OnClick"></asp:LinkButton>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <li<%= !ShowNext ? " style=\"display: none;\" " : " title=\"Siguiente\" " %>><asp:LinkButton ID="lnkSiguiente" runat="server" Text=">" OnClick="lnkSiguiente_OnClick"></asp:LinkButton></li>
                                <li<%= !ShowNext ? " style=\"display: none;\" " : " title=\"Última Página\" " %>><asp:LinkButton ID="lnkFin" runat="server" Text=">>" CommandArgument="<%# ShowFinal %>" OnClick="lnkFin_OnClick"></asp:LinkButton>
                        </li>
                            </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
          
            <div class="col-md-3">
                <div class="form-group">
                      <asp:Label ID="ddlTotalRegistros" runat="server"></asp:Label>
                    <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_OnSelectedIndexChanged" AutoPostBack="True" CssClass="form-control">
                        <Items>
                            <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="200" Value="200"></asp:ListItem>
                        </Items>
                    </asp:DropDownList>
                </div>
                 
            </div>
            <div class="col-md-2 col-md-offset-3">
                <div class="row">
                    <div class="col-md-12">
                          <asp:Label ID="lblPaginaActual" runat="server" Text="Página 1 de 23"></asp:Label>
                    </div>
                     <div class="col-md-12">
                          <asp:Label ID="lblRegistros" runat="server" Text="9999 registros"></asp:Label>
                    </div>
                </div>                
                    
               
            </div>
        </div>
        
    </ContentTemplate>
</asp:UpdatePanel>
