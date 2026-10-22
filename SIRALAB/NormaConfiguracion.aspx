<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Application.master" CodeBehind="NormaConfiguracion.aspx.cs" Inherits="SIRALAB.NormaConfiguracion" ClientIDMode="AutoID" Theme="Siralam_Thm" StylesheetTheme="Siralam_Thm" %>

<%@ Register Src="~/Controles/wucNorma.ascx" TagPrefix="uc1" TagName="wucNorma" %>
<%@ Register Src="~/Controles/wucFiltroNormaConfiguracion.ascx" TagPrefix="uc1" TagName="wucFiltroNormaConfiguracion" %>
<%@ Register Src="~/Controles/wucNormaConfiguracion.ascx" TagPrefix="uc1" TagName="wucNormaConfiguracion" %>
<%@ Register Src="~/Controles/wucConsultaNormaConfiguracion.ascx" TagPrefix="uc1" TagName="wucConsultaNormaConfiguracion" %>
<%@ Register Src="~/Controles/wucListaNormaConfiguracion.ascx" TagPrefix="uc1" TagName="wucListaNormaConfiguracion" %>
<%@ Register Src="~/Controles/wucGrupoConfiguracion.ascx" TagPrefix="uc1" TagName="wucGrupoConfiguracion" %>
<%@ Register Src="~/Controles/wucListaNormaConfiguracionSimpleCompuesta.ascx" TagPrefix="uc1" TagName="wucListaNormaConfiguracionSimpleCompuesta" %>
<%@ Register Src="~/Controles/wucListaNormaConfiguracionCompuestaMensual.ascx" TagPrefix="uc1" TagName="wucListaNormaConfiguracionCompuestaMensual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <%--<link href="style/master.css" rel="stylesheet" type="text/css" />--%>
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/busqueda.cuerporeceptor.css" rel="stylesheet" />
    <link href="style/popup-form.css" rel="stylesheet" type="text/css" />
    <%--<link href="style/bandeja.css" rel="stylesheet" type="text/css" />--%>
    <link href="style/autocomplete.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .container-move-path {
            width: 720px;
            height: 450px;
            overflow-x: hidden;
            border: 1px solid #777;
            margin: 0 auto;
            display: block;
            position: relative;
        }

        .content-move {
            width: 700px;
            height: 410px;
            position: absolute;
            left: -700px;
            top: 30px;
        }

        .previous, .next {
            font-size: 7pt;
            cursor: pointer;
            margin-top: 10px;
            border: 1px solid #999;
            color: #999;
            padding: 3px 5px;
        }

            .previous:hover, .next:hover {
                border: 1px solid #777;
                color: #777;
            }

        .sheetIndex {
            position: absolute;
            bottom: 20px;
            left: 20px;
        }

            .sheetIndex a {
                margin-left: 5px;
                padding: 2px 5px;
            }

        .table-list > tbody > tr:nth-child(even) > td {
            background-color: #F2F2F2;
        }
    </style>
    <script src="js/simple-compuesta.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-sm-1">
                <br />
                <asp:LinkButton ID="btnBackPage" runat="server" CssClass="btn btn-default btn-ms" OnClick="btnBackPage_Click" ToolTip="Regresar a la pagina anterior"> <span class="glyphicon glyphicon-chevron-left"></span></asp:LinkButton>
            </div>
            <div class="col-sm-11">
                <h2>Norma</h2>
                <hr class="red">
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-6"><span>Datos Generales</span></div>
                <div class="col-md-6">
                    <asp:UpdatePanel ID="upNormaConfiguracionCaptura" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnEditNorma" runat="server" CausesValidation="false" CssClass="btn btn-default btn-xs pull-right" ToolTip="Editar" OnClick="btnEditNorma_Click"> <span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <uc1:wucNorma ID="wucNormaCaptura" runat="server" OnGuardar="wucNormaCaptura_Guardar" OnCancelar="wucNormaCaptura_Cancelar" />
        </div>
    </div>
    <asp:UpdatePanel ID="upConfiguraciones" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tcConfiguraciones" EventName="ActiveTabChanged" />
            <asp:AsyncPostBackTrigger ControlID="wucFiltroNormaConfiguracion" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlNormaConfiguracion" runat="server" CssClass="panel panel-default" Style="min-height: 400px;" Visible="false">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-6">Configuraciones</div>
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkbNuevaNormaConfiguracion" runat="server" OnClick="lnkbNuevaNormaConfiguracion_Click" ToolTip="Nueva configuración" Text="Nueva configuración" CssClass="btn btn-default btn-xs pull-right"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div style="width: 97%; margin: 4px auto;">
                    <uc1:wucFiltroNormaConfiguracion ID="wucFiltroNormaConfiguracion" runat="server" OnBuscar="wucFiltroNormaConfiguracion_Buscar" OnLimpiar="wucFiltroNormaConfiguracion_Limpiar" OnAfter_Load="wucFiltroNormaConfiguracion_After_Load" />
                </div>
                <div style="width: 97%; margin: 4px auto;">
                    <asp:TabContainer ID="tcConfiguraciones" runat="server" ActiveTabIndex="0" Width="100%" AutoPostBack="True" OnActiveTabChanged="tcConfiguraciones_ActiveTabChanged">
                        <asp:TabPanel ID="tabSimple" runat="server" HeaderText="Simple" Height="450px">
                            <ContentTemplate>
                                <div id="listaSimple" style="width: 720px; margin: 10px auto; font-size: 8pt; overflow-x: hidden; height: 400px;">
                                    <uc1:wucListaNormaConfiguracion runat="server" ID="wucListaNormaConfiguracion" OnEditar="wucListaNormaConfiguracion_Editar" OnModificarOrden="wucListaNormaConfiguracion_ModificarOrden" />
                                </div>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabCompuesta" runat="server" HeaderText="Compuesta" Visible="false" Height="450px">
                            <ContentTemplate>
                                <div id="listaSimple_Compuesta" class="container-move-path">
                                    <div class="content-move" itemid="0" style="background-color: #fff;">
                                        <uc1:wucListaNormaConfiguracionSimpleCompuesta runat="server" ID="wucListaNormaConfiguracionSimpleCompuesta" OnCheckItem="wucListaNormaConfiguracionSimpleCompuesta_CheckItem" OnAgregar="wucListaNormaConfiguracionSimpleCompuesta_Agregar" />
                                    </div>
                                    <div class="content-move" itemid="1" style="background-color: #fff;">
                                        <uc1:wucListaNormaConfiguracion runat="server" ID="wucListaNormaConfiguracion1" OnEditar="wucListaNormaConfiguracion_Editar" OnModificarOrden="wucListaNormaConfiguracion_ModificarOrden" />
                                    </div>
                                    <span class="previous">Parámetros de muestra simple</span>
                                    <span class="next">Parámetros de muestra compuesta</span>
                                </div>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabMensual" runat="server" HeaderText="Mensual" Visible="false" Height="400px">
                            <ContentTemplate>
                                <div id="listaMensual" style="overflow-x: hidden; height: 400px;">
                                    <uc1:wucListaNormaConfiguracionCompuestaMensual runat="server" ID="wucListaNormaConfiguracionCompuestaMensual" OnCheckItem="wucListaNormaConfiguracionSimpleCompuesta_CheckItem" OnAgregar="wucListaNormaConfiguracionCompuestaMensual_Agregar" OnEvalItem="wucListaNormaConfiguracionCompuestaMensual_EvalItem" OnActivar="wucListaNormaConfiguracionCompuestaMensual_Activar" />
                                </div>
                            </ContentTemplate>
                        </asp:TabPanel>

                        <%-- <asp:TabPanel ID="tabValorInstantaneo" runat="server" HeaderText="Valor instantaneo" Visible="false" Height="450px">
                          <ContentTemplate>
                                <div id="listaValorInstantaneo" style="width: 720px; margin: 10px auto; font-size: 8pt; overflow-x: hidden; height: 400px;">
                                    <uc1:wucListaNormaConfiguracion runat="server" ID="wucListaNormaConfiguracionInstantaneo" OnEditar="wucListaNormaConfiguracion_Editar" OnModificarOrden="wucListaNormaConfiguracion_ModificarOrden" />
                                </div>
                            </ContentTemplate>
                        </asp:TabPanel>--%>
                    </asp:TabContainer>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:wucNormaConfiguracion runat="server" ID="wucNormaConfiguracion" OnCancelar="wucNormaConfiguracion_Cancelar" OnGuardar="wucNormaConfiguracion_Guardar" OnAgregarGrupo="wucNormaConfiguracion_AgregarGrupo" />
    <uc1:wucGrupoConfiguracion runat="server" ID="wucGrupoConfiguracion" OnGuardar="wucGrupoConfiguracion_Guardar" />

    <script type="text/javascript">
        function itemPos() {
            this.id = '';
            //this.xPos = 0;
            this.yPos = 0;
        };

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        $(document).ready(function () {
            endRequestHandler();

            var itemPos1 = new itemPos();
            var itemPos2 = new itemPos();
            var itemPos3 = new itemPos();
            itemPos1.id = 'listaSimple';
            itemPos2.id = 'listaSimple_Compuesta';
            itemPos3.id = 'listaMensual';

            scrollHandler.scrollItem.push(itemPos1);
            scrollHandler.scrollItem.push(itemPos2);
            scrollHandler.scrollItem.push(itemPos3);

        });

        scrollHandler = {
            scrollItem: []
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler_Scroll);
        prm.add_endRequest(EndRequestHandler_Scroll);
        function BeginRequestHandler_Scroll(sender, args) {
            for (i = 0; i < scrollHandler.scrollItem.length; i++) {
                scrollItemTemp = scrollHandler.scrollItem[i];
                //scrollItemTemp.xPos = $('#' + scrollItemTemp.id).scrollLeft();
                scrollItemTemp.yPos = $('#' + scrollItemTemp.id).scrollTop();
            }
        }
        function EndRequestHandler_Scroll(sender, args) {

            for (i = 0; i < scrollHandler.scrollItem.length; i++) {
                scrollItemTemp = scrollHandler.scrollItem[i];
                //$('#' + scrollItemTemp.id).scrollLeft(scrollItemTemp.xPos);
                $('#' + scrollItemTemp.id).scrollTop(scrollItemTemp.yPos);
            }
        }

        function scrollTo(x, y) {

        }
    </script>
</asp:Content>
