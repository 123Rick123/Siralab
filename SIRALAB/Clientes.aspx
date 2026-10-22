<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="SIRALAB.Clientes" ClientIDMode="AutoID" Culture="es-MX" UICulture="es-MX" %>


<%@ Register Src="Controles/wucCliente.ascx" TagName="wucCliente" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/bandeja.css" rel="stylesheet" type="text/css" />
    <link href="style/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .pnl-colapsado4 {
            margin-left: -1500px;
            display: block;
        }
    </style>
    <script type="text/javascript">
        function ve(d) {
            if (d == 3) {
                $('.pnl-colapsado4').animate({ 'marginLeft': '60px' }, 1300, function () { $('#<%= txtCliente.ClientID %>').focus(); 200 });

            }
            if (d == 4) {
                $('.pnl-colapsado4').animate({ 'marginLeft': '-1500px' }, 1300, function () { $('#<%= txtCliente.ClientID %>').focus(); 200 });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:Panel ID="pnldatos" runat="server" DefaultButton="lnkbtnActualiza">
        <asp:UpdatePanel ID="updClientes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkbtnActualiza" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkbtnNuevo" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="formulario" style="display: block;">
                    <ul>
                        <li class="titulo-pagina">Clientes
                        </li>
                        <li>
                            <div class="formulario-seccion">
                                <div class="seccion-encabezado">
                                    <div class="seccion-titulo">Busqueda de Clientes</div>
                                </div>
                                <ul>
                                    <li></li>
                                </ul>
                                <ul>
                                    <li>
                                        <div class="div-Grayseccion">
                                            Proporcione el RFC del cliente:
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <asp:TextBox ID="txtCliente" runat="server" Style="width: 350px; margin-top: 10px;" CssClass="control-campo"></asp:TextBox>
                                            <asp:LinkButton ID="lnkbtnActualiza" runat="server" OnClick="lnkbtnActualiza_Click" Style="display: none">Nuevo</asp:LinkButton>
                                            <asp:AutoCompleteExtender ID="aceClientes" runat="server" Enabled="True"
                                                ServicePath="~/wsConsultas.asmx" ServiceMethod="Encuentra_Clientes"
                                                MinimumPrefixLength="1" DelimiterCharacters="2" CompletionInterval="100" EnableCaching="true"
                                                CompletionSetCount="15" TargetControlID="txtCliente" CompletionListElementID="Div"
                                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                            </asp:AutoCompleteExtender>
                                        </div>
                                        <div>
                                            <asp:LinkButton ID="lnkbtnNuevo" runat="server" OnClick="lnkbtnNuevo_Click" ToolTip="Nuevo cliente" CssClass="icono-nuevo" Style="margin-left: 450px; margin-top: -40px; width: 48px; height: 48px;"></asp:LinkButton>
                                        </div>
                                        <div>
                                            <asp:LinkButton ID="lnkbtnBuscar" runat="server" OnClick="lnkbtnActualiza_Click" ToolTip="Buscar cliente" CssClass="icono-masinfo" Style="margin-left: 380px; margin-top: -30px; display: block"></asp:LinkButton>
                                        </div>
                                    </li>
                                    <li></li>
                                </ul>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="width: 650px;" class="pnl-colapsado4">
                    <uc1:wucCliente ID="wucCliente1" runat="server" ValidationGroup="Cliente" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
