<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConductividadElectrica.ascx.cs" Inherits="SIRALAB.Controles.wucConductividadElectrica" %>

<%--<%@ Register Src="AnexoE/wucMuestrasSimples.ascx" TagName="wucMuestrasSimples" TagPrefix="uc1" %>--%>
<%@ Register Src="~/Controles/AnexoE/wucMuestrasSimples.ascx" TagPrefix="uc1" TagName="wucMuestrasSimples" %>

<div class="panel-body">
    <div class="col-md-12">
        <asp:CheckBox ID="chkEColi" runat="server" CssClass="text" AutoPostBack="true" Text="La conductividad eléctrica es menor a 3500 µS/cm" OnCheckedChanged="chkEColi_CheckedChanged"></asp:CheckBox>
        <br />
        <asp:CheckBox ID="chkEnterococos" runat="server" CssClass="text" AutoPostBack="true" Text="La conductividad eléctrica es mayor o igual a 3500 µS/cm" OnCheckedChanged="chkEnterococos_CheckedChanged"></asp:CheckBox>
    </div>
    <asp:HiddenField ID="hfEColiEntero" runat="server" />
    <asp:HiddenField ID="hfConductividad" runat="server" />
    <asp:HiddenField ID="hfGuardaConductividad" runat="server" />
    <uc1:wucMuestrasSimples ID="wucMuestrasSimples1" runat="server" />
</div>
