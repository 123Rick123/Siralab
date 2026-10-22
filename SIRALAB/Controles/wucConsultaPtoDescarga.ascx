<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaPtoDescarga.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaPtoDescarga" %>

<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/autocomplete.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" />
<style type="text/css">
    .div-colapsado3 {
        display: none;
    }

    .rbl input[type="radio"]
    {
        margin-left: 10px;
        margin-right: 1px;
    }
</style>

<script type="text/javascript">
    function ve(d) {
        if (d == 3) {
            $('.div-colapsado3').slideToggle(800);
        };
    }
    //<td style="width: 16px;"><a href="javascript:t(1)" id="button2" class="icono-agregar-16 collapse-action">f</a></td>
</script>

<asp:Panel ID="pnlCapturaPersona" runat="server" Visible="true">
    <asp:UpdatePanel ID="updPtoDescarga" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lnkbtnPtoConsulta" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:LinkButton ID="lnkbtnPtoConsulta" runat="server" Visible="false" OnClientClick="ve(3); return false;" CssClass="btn btn-default btn-xs" ToolTip="Ver información completa">
                <span class="glyphicon glyphicon-chevron-down"></span>
            </asp:LinkButton>
            <asp:Panel ID="pnlDatosPto" runat="server">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:RadioButtonList ID="rdConcesion" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="rdConcesion_SelectedIndexChanged" RepeatDirection="Horizontal" CssClass="rbl">
                                <asp:ListItem Selected="True">Título de concesión/permiso</asp:ListItem>
                                <asp:ListItem>Fuente abastecimiento</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row form-group">
                        <div class="col-md-9">
                            <label for="name">Permiso de descarga o aprovechamiento o título de concesión o asignación<span class="form-text">*</span>:</label>
                            <asp:Panel ID="pnlPtoConsecion" runat="server" DefaultButton="lnkbtnPtoConsecion">
                                <asp:DropDownList ID="ddlConsecion" runat="server" CssClass="form-control input-sm" style="max-width: 270px" OnSelectedIndexChanged="ddlConsecion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:LinkButton ID="lnkbtnPtoConsecion" runat="server" CssClass="icono-buscar" OnClick="lnkbtnPtoConsecion_Click" Style="display: none;" ToolTip="Buscar Permiso"><div></div></asp:LinkButton>
                            </asp:Panel>
                        </div>
                        <div class="col-md-2">
                            <label for="name">Anexo<span class="form-text"></span>:</label>
                            <asp:DropDownList ID="ddlPtoAnexo" runat="server" AutoPostBack="true" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlPtoAnexo_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <%--solo debe ser visible el medidor con fuente de abastecimiento--%>
                    <div class="row form-group">
                        <div class="col-md-8">
                            <label for="ddlMedidorFuente"><asp:Label ID="lblMedidorTag" runat="server" Text="Medidor:"></asp:Label></label>
                            <asp:DropDownList ID="ddlMedidorFuente" AutoPostBack="true" CssClass="form-control input-sm" style="max-width: 270px" runat="server" OnSelectedIndexChanged="ddlMedidorFuente_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="div-colapsado3">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h5>DOMICILIO O DESCRIPCIÓN DE LA UBICACIÓN DEL PUNTO DE DESCARGA</h5>
                                </div>
                                <div class="panel-body">
                                    <div class="row">

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Calle<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoCalle" runat="server" CssClass="form-text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">No. Exterior<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoNE" runat="server" CssClass="form-text"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">No. Interior<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoNI" runat="server" CssClass="form-text"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">C.P<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoCP" runat="server" CssClass="form-text"></asp:Label>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Estado<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoEstado" runat="server" CssClass="form-text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Municipio<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoMunicipio" runat="server" CssClass="form-text"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Colonia<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoColonia" runat="server" CssClass="form-text"></asp:Label>
                                                <asp:Label ID="Label3" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Localidad<span class="form-text"></span>:</label>
                                                <asp:Label ID="lblPtoLocalidad" runat="server" CssClass="form-text"></asp:Label>
                                                <asp:Label ID="Label4" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="col-md-5">
                                            Latitud
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Grados<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoGraLat" runat="server" CssClass="form-text"></asp:Label>

                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Minutos<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoMinLat" runat="server" CssClass="form-text"></asp:Label>

                                                    </div>
                                                </div>


                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Segundos<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoSegLat" runat="server" CssClass="form-text"></asp:Label>


                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Dirección<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoDirLat" runat="server" CssClass="form-text"></asp:Label>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            Longitud
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Grados<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoGraLong" runat="server" CssClass="form-text"></asp:Label>


                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Minutos<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoMinLong" runat="server" CssClass="form-text"></asp:Label>


                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Segundos<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoSegLong" runat="server" CssClass="form-text"></asp:Label>


                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label for="name">Direccion<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoDirLong" runat="server" CssClass="form-text"></asp:Label>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            Datum
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">

                                                        <asp:Label ID="lblPtoDatum" runat="server" CssClass="form-text"></asp:Label>



                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label for="name">Nombre de cuerpo receptor<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoCuerpoReceptor" runat="server" CssClass="form-text">></asp:Label>

                                                    </div>



                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label for="name">Efluente P. tratamiento<span class="form-text"></span>:</label>
                                                        <asp:Label ID="lblPtoEPTratamiento" runat="server" CssClass="form-text"></asp:Label>
                                                    </div>



                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <table style="display: none;">
                    <tr>
                        <td style="width: 290px;">Fuente de abastecimiento</td>
                        <td style="width: 250px;">Origen descarga</td>
                        <td style="width: 250px;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 290px;">
                            <div style="width: 280px;">
                                <asp:TextBox ID="txtPtoAbastecimiento" CssClass="control-campo" runat="server" Enabled="false" Style="width: 240px;"></asp:TextBox>
                            </div>
                        </td>
                        <td style="width: 250px;">
                            <div style="width: 130px;">
                                <asp:TextBox ID="txtPtoDescarga" CssClass="control-campo" runat="server" Enabled="false" Style="width: 240px;"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div>
                                <asp:LinkButton ID="lnkbtnPtoAbasDes" runat="server" CssClass="icono-guardar" ToolTip="Guardar Fuente de abastecimiento y origen de descarga" Enabled="false" Style="margin-left: 100px;" OnClick="lnkbtnPtoAbasDes_Click" Visible="false"></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                </table>

            </asp:Panel>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

  <div id="modalMensajeNom001"  class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeader">
                    <h5 class="modal-title">Mensaje del sistema</h5>
                </div>
                <div class="modal-body" id="divMensajes" style="text-align:justify">
                   <p id="txtMensajeModalNom001" runat="server">Usted puede realizar sus analisis por la NOM-001-SEMARNAT-2021</p> 
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
