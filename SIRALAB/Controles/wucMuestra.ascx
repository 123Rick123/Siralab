<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMuestra.ascx.cs" Inherits="SIRALAB.Controles.wucMuestra" %>
<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<ul>
    <li class="horizontal">Tipo de uso del agua</li>
    <li class="horizontal">Frecuencía de muestreo</li>
</ul>
<ul>
    <li class="horizontal"><asp:DropDownList ID="ddlMuestraUsoAgua" runat="server"></asp:DropDownList></li>
    <li class="horizontal"><asp:DropDownList ID="ddlMuestraFrecuencia" runat="server"></asp:DropDownList></li>
</ul>
<ul>
    <li class="horizontal"><asp:CheckBox ID="chkMuestraDomicilio" runat="server"/>Domicilio de Punto de descarga o Aprovechamiento es el mismo que el punto de muestreo Calle</li>
</ul>
<ul>
    <li class="horizontal">Domicilio o descripción de la ubicación del lugar donde se realiza el muestreo</li>
</ul>
<ul>
    <li class="horizontal"><asp:TextBox ID="txtMuestraDomicilioMuestra" runat="server"></asp:TextBox></li>
</ul>
<ul>
    <li class="horizontal">Calle</li>
    <li class="horizontal">No. Exterior</li>
    <li class="horizontal">No. Interior</li>
    <li class="horizontal">C.P.</li>
</ul>
<ul>
    <li class="horizontal"><asp:TextBox ID="txtPtoCalle" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoNoExterior" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoNoInterior" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoCP" runat="server"></asp:TextBox></li>
</ul>
<ul>
    <li class="horizontal">Estado</li>
    <li class="horizontal">Municipio</li>
    <li class="horizontal">Localidad</li>
</ul>
<ul>
    <li class="horizontal"><asp:DropDownList ID="ddlPtoEstado" runat="server"></asp:DropDownList></li>
    <li class="horizontal"><asp:DropDownList ID="ddlPtoMunicipio" runat="server"></asp:DropDownList></li>
    <li class="horizontal"><asp:DropDownList ID="ddlPtoLocalidad" runat="server"></asp:DropDownList></li>
</ul>

<ul>
    <li class="horizontal">Latitud</li>
    <li class="horizontal">Longitud</li>
    <li class="horizontal">datum</li>
</ul>
<ul>
    <li class="horizontal">Grados</li>
    <li class="horizontal">Minutos</li>
    <li class="horizontal">Segundos</li>
    <li class="horizontal">Grados</li>
    <li class="horizontal">Minutos</li>
    <li class="horizontal">Segundos</li>
    <li class="horizontal"></li>
</ul>
<ul>
    <li class="horizontal"><asp:TextBox ID="txtPtoLaGrados" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoLaMinutos" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoLaSegundos" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoLoGrados" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoLoMinutos" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoLoSegundos" runat="server"></asp:TextBox></li>
    <li class="horizontal"><asp:TextBox ID="txtPtoDatum" runat="server"></asp:TextBox></li>
</ul>