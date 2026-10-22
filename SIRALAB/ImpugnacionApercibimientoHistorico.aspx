<%@ Page Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="ImpugnacionApercibimientoHistorico.aspx.cs" Inherits="SIRALAB.ImpugnacionApercibimientoHistorico" %>

<%@ Register Src="Controles/wucConsultaApercibimientoReincidencia.ascx" TagName="wucConsultaApercibimientoReincidencia" TagPrefix="uc1" %>
<%@ Register Src="Controles/wucMetodoDeImpugnacionApercibimiento.ascx" TagName="wucMetodoDeImpugnacionApercibimiento" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updBusquedaApercibidosYReincidentes" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Consulta apercibimiento </h5>
                    </div>
                    <div class="panel-body">
                        <%--  LLAMADA A USER CONTROL DE consula--%>
                        <uc1:wucConsultaApercibimientoReincidencia ID="wucConsultaApercibimientoReincidencia" runat="server" />

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
