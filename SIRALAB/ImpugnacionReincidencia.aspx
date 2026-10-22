<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="ImpugnacionReincidencia.aspx.cs" Inherits="SIRALAB.ImpugnacionReincidencia" %>

<%@ Register Src="Controles/wucConsultaApercibimientoReincidencia.ascx" TagName="wucConsultaApercibimientoReincidencia" TagPrefix="uc1" %>
<%@ Register Src="Controles/wucConsultaReincidencia.ascx" TagName="wucConsultaReincidencia" TagPrefix="uc1" %>
<%@ Register Src="Controles/wucMetodoDeImpugnacionReincidencia.ascx" TagName="wucMetodoDeImpugnacionReincidencia" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    
    <asp:UpdatePanel ID="updBusquedaApercibidosYReincidentes" runat="server">
        <contenttemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Consulta apercibimiento </h5>
                    </div>
                    <div class="panel-body">
                        <%--  LLAMADA A USER CONTROL DE consula--%>
                        <uc1:wucConsultaApercibimientoReincidencia ID="wucConsultaApercibimientoReincidencia" runat="server"  />

                    </div>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updReincidencia" runat="server">
        <contenttemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Consulta reincidencia </h5>
                    </div>
                    <div class="panel-body">
                        <%--  LLAMADA A USER CONTROL DE consula--%>
                        <uc1:wucConsultaReincidencia ID="wucConsultaReincidencia" runat="server"  />

                    </div>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upImpugnacionReincidencia" runat="server">
        <contenttemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Métodos de impugnación </h5>
                    </div>
                    <div class="panel-body">
                        <%--  LLAMADA A USER CONTROL DE consula--%>
                        <uc1:wucMetodoDeImpugnacionReincidencia ID="wucMetodoDeImpugnacionReincidencia" runat="server"  />

                    </div>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>


</asp:Content>
