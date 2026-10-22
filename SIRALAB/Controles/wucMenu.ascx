<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMenu.ascx.cs" Inherits="SIRALAB.Controles.wucMenu" %>

<%@ Register Src="~/Controles/wucCambiarPassword.ascx" TagPrefix="uc1" TagName="wucCambiarPassword" %>

<%--<link href="style/menu.css" rel="stylesheet" type="text/css" />--%>
<%--<link href="style/menuNuevo.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript">
    function Salir() {
        MenuHandler.serverButtonClick('<%= lnkbSalir.ClientID %>');
    }
    function Submenu(e) {
        MenuHandler.submenu($(e).parent());
    }
</script>
<nav class="navbar navbar-inverse sub-navbar navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">

            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#subenlaces">
                <span class="sr-only">Interruptor de Navegación</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="/">CONAGUA</a>
        </div>
        <div id="divMenu" runat="server" class="collapse navbar-collapse">
         
        </div>
       
    </div>
</nav>


<asp:LinkButton ID="lnkbSalir" runat="server" OnClick="Salir_Click" Style="display: none;"></asp:LinkButton>
