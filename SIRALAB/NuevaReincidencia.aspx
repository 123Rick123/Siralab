<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="NuevaReincidencia.aspx.cs" Inherits="SIRALAB.NuevaReincidencia" %>

<%@ Register Src="Controles/wucAltaReincidencia.ascx" TagName="wucAltaReincidencia" TagPrefix="uc1" %>
<%@ Register Src="Controles/wucConsultaApercibimientoReincidencia.ascx" TagName="wucConsultaApercibimientoReincidencia" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script src="js/jquery.flexslider.js"></script>
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updConsulta" runat="server">
        <contenttemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Consulta apercibimiento </h5>
                    </div>
                    <div class="panel-body">


                        <%--  LLAMADA A USER CONTROL DE Consulta--%>
                         <uc1:wucConsultaApercibimientoReincidencia ID="wucConsultaApercibimientoReincidencia" runat="server"  />

                    </div>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>

 <asp:UpdatePanel ID="updBusquedaApercibidosYReincidentes" runat="server">
        <contenttemplate>
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h5>Alta reincidencia </h5>
                    </div>
                    <div class="panel-body">


                        <%--  LLAMADA A USER CONTROL DE AltaReincidencia--%>
                        <uc1:wucAltaReincidencia ID="wucAltaReincidencia" runat="server"  />

                    </div>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
